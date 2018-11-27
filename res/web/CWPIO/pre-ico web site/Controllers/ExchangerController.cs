using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using Newtonsoft.Json;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/exchanger")]
    public class ExchangerController : Controller
    {
        private readonly TokenSaleContract _contract;
        private readonly ApplicationDbContext _dbContext;
        private readonly EthSettings _options;
        private readonly IRateStore _rateStore;
        private readonly Crypto _crypto;
        private readonly IEmailSender _emailSender;

        public ExchangerController(
            ApplicationDbContext dbContext,
            TokenSaleContract contract,
            IRateStore rateStore,
            IOptions<EthSettings> options,
            Crypto crypto,
            IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _contract = contract;
            _options = options.Value;
            _rateStore = rateStore;
            _crypto = crypto;
            _emailSender = emailSender;
        }

        [HttpGet("contractabi")]
        public IActionResult GetSaleContractABI()
        {
            return Ok(JsonConvert.DeserializeObject(_contract.GetSaleContractABI()));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.Wallet) && (await _contract.CheckTxStatusAsync(user.WhiteListTransaction)) && !(await _contract.CheckWhitelistAsync(user.Wallet)))
            {
                user.EthAddress = null;
                await _dbContext.SaveChangesAsync();
            }

            return Ok(new
            {
                Sold = UnitConversion.Convert.FromWei(await _contract.TokenSoldAsync()),
                Cap = UnitConversion.Convert.FromWei(await _contract.GetCapAsync()),
                Ballance = string.IsNullOrEmpty(user.Wallet) ? 0 : UnitConversion.Convert.FromWei(await _contract.GetBallanceAsync(user.Wallet)),
                Refund = string.IsNullOrEmpty(user.Wallet) ? 0 : UnitConversion.Convert.FromWei(await _contract.GetRefundAmountAsync(user.Wallet))
            });
        }

        [HttpGet("calc/{amount}")]
        public async Task<IActionResult> GetCalcAsync([FromRoute]int amount)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var rate = await GetRateAsync(amount);
            var price = await _contract.GetGasPriceAsync();
            var gas = 280190 * price * 3;
            return Ok(new
            {
                totalAmount = UnitConversion.Convert.FromWei(rate.amount + gas).ToString(System.Globalization.CultureInfo.InvariantCulture),
                fee = UnitConversion.Convert.FromWei(gas).ToString()
            });
        }

        [HttpGet("calcExchange/{amount}")]
        public async Task<IActionResult> GetCalcExchangeAsync([FromRoute]int amount)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var ethrate = await GetRateAsync(amount);
            var price = await _contract.GetGasPriceAsync();
            var gas = (0xBB80 + 0xBB80 + 0x19A28 + 0x59D8 + 0x5208) * price * 3;
            return Ok(new
            {
                totalAmount = UnitConversion.Convert.FromWei(ethrate.amount + gas).ToString(),
                fee = UnitConversion.Convert.FromWei(gas).ToString()
            });
        }

        [HttpGet("addr")]
        public IActionResult GetAddr()
        {
            return Ok(_contract.Address);
        }

        [HttpGet("changerAddr")]
        public async Task<IActionResult> GetChangerAddr()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.TempAddress))
            {
                return Ok(user.TempAddress);
            }

            var (address, pk) = _contract.NewAddress();
            var exchanger = _crypto.Encrypt(pk.StringToByteArray()).ByteArrayToString();
            await _dbContext.Addresses.AddAsync(new Addres { Address = address, Exchanger = exchanger });
            user.TempAddress = address;
            await _dbContext.SaveChangesAsync();

            return Ok(address);

        }

        [HttpPost("initPurchasing")]
        public async Task<IActionResult> InitPurchase([FromBody]PurchaseRequestModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(user.Wallet) || !await _contract.CheckWhitelistAsync(user.Wallet))
            {
                return BadRequest(new { error = "Not in whitelist" });
            }
            var rate = await GetRateAsync(model.Count);

            var fixRate = await _contract.SetRateForTransactionAsync(rate.rate, user.Wallet, rate.amount);
            return Ok(new { amount = UnitConversion.Convert.FromWei(fixRate.Amount), fixRate });
        }

        private async Task<(int rate, BigInteger amount)> GetRateAsync(int count)
        {
            var rate = await _rateStore.GetRateAsync(); //220
            var erate = (int)Math.Round(rate / _options.TokenPrice); // 1 ether = erate tokens
            var amount = UnitConversion.Convert.ToWei(Math.Round(count / (decimal)erate, 9))
                + UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);
            return (rate: erate, amount: amount);
        }

        [HttpPost("monitor")]
        public async Task<IActionResult> StartMonitorAsync([FromBody]ExchangeRequestModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            if (string.IsNullOrEmpty(user.TempAddress))
            {
                return BadRequest(new { error = "Not set temp address" });
            }
            var item = _dbContext.ExchangeStatuses.FirstOrDefault(s => s.StartTx == model.Tx);
            if (item == null)
            {
                var rate = await GetRateAsync(model.Count);
                item = new ExchangeStatus
                {
                    StartTx = model.Tx,
                    CurrentTx = model.Tx,
                    CurrentStep = 0,
                    Rate = rate.rate,
                    CreatedByUser = user,
                    EthAmount = rate.amount.ToString(),
                    TokenCount = model.Count
                };
                await _dbContext.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }

            //if (string.IsNullOrEmpty(user.ExchangerContract))
            //{
            //    var rate = await GetRateAsync(model.Count);
            //    if (string.IsNullOrEmpty(user.Wallet) || !await _contract.CheckWhitelistAsync(user.Wallet))
            //    {
            //        return BadRequest(new { rate, user.Wallet, error = "Not in whitelist" });
            //    }
            //    var addr = await _dbContext.Addresses.FindAsync(user.TempAddress);
            //    string newContractAddr = await _contract.CreateExchangerAsync(
            //        _crypto.Decrypt(addr.Exchanger.StringToByteArray()),
            //        user.Wallet,
            //        new HexBigInteger(rate.amount),
            //        new HexBigInteger(rate.rate));
            //    var hash = await _contract.AddAddressToWhitelistAsync(newContractAddr);
            //    await _contract.WaitReciept(hash);
            //    user.ExchangerContract = newContractAddr;
            //    await _dbContext.SaveChangesAsync();

            //    await _dbContext.AddAsync(new ExchangeStatus
            //    {
            //        StartTx = model.Tx,
            //        CurrentTx = model.Tx,
            //        CreatedByUser = user,
            //        EthAmount = rate.amount.ToString(),
            //    });
            //    user.TempAddress = null;
            //    await _dbContext.SaveChangesAsync();
            //}

            return Ok(item.Id);
        }

        [HttpPost("whiteList")]
        public async Task<IActionResult> WhiteListAsync([FromBody]WhitelistRequestModel model)
        {
            if (string.IsNullOrEmpty(model.ErcAddress))
            {
                return BadRequest();
            }

            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (user.EthAddress != null)
            {
                return BadRequest();
            }

            var tx = await _contract.AddAddressToWhitelistAsync(model.ErcAddress);

            user.WhiteListTransaction = tx;
            user.EthAddress = model.ErcAddress.StringToByteArray();
            await _dbContext.SaveChangesAsync();
            return Ok(new { txHash = tx });
        }

        [HttpGet("status/{address}")]
        public async Task<IActionResult> StatusAsync([FromRoute]string address, [FromQuery]string fromBlock = null)
        {
            var res = BigInteger.TryParse(fromBlock, out BigInteger fromBlockInt);

            var (status, transaction) = await _contract.CheckStatusAsync(address, res ? fromBlockInt : -1);
            return Ok(new { Status = status, Transaction = transaction, TimeRemaining = 999 });
        }

        [HttpGet("purchasestatus/{id}")]
        public async Task<IActionResult> PurchaseStatus([FromRoute]string id)
        {
            var item = await _dbContext.ExchangeStatuses.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var result = new[]{
                new ExchangeStatusModel(),
                new ExchangeStatusModel(),
                new ExchangeStatusModel()
            };

            for (int i = 0; i < item.CurrentStep - 1; i++)
            {
                result[i].Name = "Complete";
                result[i].Description = "Complete";
            }

            var step = (item.CurrentStep - 1 < 0 ? 0 : item.CurrentStep - 1);
            result[step].Name = item.CurrentStep == 3 ? (item.IsEnded ? "Complete" : (item.IsFailed ? "Failed" : "In Progress")) : ( item.IsFailed ? "Failed" : "In Progress");
            result[step].Description = item.IsFailed ? "Failed" : "In Progress";
            return Ok(result);
        }

        //[HttpGet("refund")]
        //public async Task<IActionResult> GetRefundAsync()
        //{


        //    var user = await _dbContext.GetCurrentUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var contract = _contract.Eth.GetContract(_abi, _contractAddress);
        //    var result = await contract.GetFunction("refund").SendTransactionAsync(_options.AppAddress, $"0x{ByteArrayToString(user.EthAddress)}");
        //    return Ok(result);
        //}

        public IActionResult PostFailTransaction(FailTransactionModel model)
        {


            return Ok();
        }


    }
}