using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using pre_ico_web_site.Data;
using pre_ico_web_site.Eth;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System;
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

            return Ok(new
            {
                Sold = UnitConversion.Convert.FromWei(await _contract.WeiRaisedAsync())
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

            var rate = await _rateStore.GetRateAsync(); //220

            var erate = (int)Math.Round(rate / _options.TokenPrice); // 1 ether = erate tokens
            var x = UnitConversion.Convert.ToWei(amount / (decimal)erate);
            return Ok(UnitConversion.Convert.FromWei(x).ToString());
        }

        [HttpGet("addr")]
        public IActionResult GetAddr()
        {
            return Ok(_contract.Address);
        }

        [HttpPost("initPurchasing")]
        public async Task <IActionResult> InitPurchase([FromBody]PurchaseRequestModel model)
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var rate = await _rateStore.GetRateAsync(); //220

            var erate = (int)Math.Round(rate / _options.TokenPrice); // 1 ether = erate tokens
            var x = UnitConversion.Convert.ToWei(model.Count / (decimal)erate) + 1;
            var fixRate = await _contract.SetRateForTransactionAsync(erate, $"0x{ByteArrayToString(user.EthAddress)}", x);

            return Ok(new { amount = UnitConversion.Convert.FromWei(x), fixRate });
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

        private static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}