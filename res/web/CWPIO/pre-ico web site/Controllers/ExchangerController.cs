using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System;
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

        public ExchangerController(
            ApplicationDbContext dbContext,
            TokenSaleContract contract,
            IRateStore rateStore,
            IOptions<EthSettings> options)
        {
            _dbContext = dbContext;
            _contract = contract;
            _options = options.Value;
            _rateStore = rateStore;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.Wallet) && !(await _contract.CheckWhitelistAsync(user.Wallet)))
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
            return Ok(UnitConversion.Convert.FromWei(rate.amount).ToString());
        }

        [HttpGet("addr")]
        public IActionResult GetAddr()
        {
            return Ok(_contract.Address);
        }

        [HttpGet("changerAddr")]
        public IActionResult GetChangerAddr()
        {
            return Ok(_options.ChangerAddr);
        }

        [HttpPost("initPurchasing")]
        public async Task<IActionResult> InitPurchase([FromBody]PurchaseRequestModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var fixRate = await initPurchase(user, model);
            if (fixRate == null)
            {
                return BadRequest(new { error = "Not in whitelist" });
            }

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

        private async Task<FixRateModel> initPurchase(ApplicationUser user, PurchaseRequestModel model)
        {
            if (!string.IsNullOrEmpty(user.Wallet) && await _contract.CheckWhitelistAsync(user.Wallet))
            {
                var rate = await GetRateAsync(model.Count);

                var fixRate = await _contract.SetRateForTransactionAsync(rate.rate, user.Wallet, rate.amount);
                return fixRate;
            }
            else
            {
                return null;
            }
        }

        [HttpPost("monitor")]
        public async Task<IActionResult> StartMonitorAsync([FromBody]ExchangeRequestModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var rate = await initPurchase(user, model);
            if (rate == null)
            {
                return BadRequest(new { error = "Not in whitelist" });
            }

            if (rate.Time > 0)
            {
                await _dbContext.AddAsync(new ExchangeStatus
                {
                    StartTx = model.Tx,
                    CurrentTx = model.Tx,
                    CreatedByUser = user,
                    EthAmount = rate.Amount.ToString(),
                    TokenAmount = model.Count.ToString()
                });
                await _dbContext.SaveChangesAsync();
            }

            return Ok(rate);
        }

        [HttpPost("whiteList")]
        public async Task<IActionResult> WhiteListAsync(string ercAddress)
        {
            if (string.IsNullOrEmpty(ercAddress))
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

            var tx = await _contract.AddAddressToWhitelistAsync(ercAddress);

            user.EthAddress = StringToByteArray(ercAddress);
            await _dbContext.SaveChangesAsync();
            return Ok(new { txHash = tx });
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



        private static byte[] StringToByteArray(string hex)
        {
            hex = hex.Replace("0x", "");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}