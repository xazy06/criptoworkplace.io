using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using pre_ico_web_site.Data;
using pre_ico_web_site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using Nethereum.Contracts;
using pre_ico_web_site.Eth;

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

        public ExchangerController(
            ApplicationDbContext dbContext, 
            TokenSaleContract contract, 
            IOptions<EthSettings> options)
        {
            _dbContext = dbContext;
            _contract = contract;
            _options = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
                                  

            //if (user.EthAddress != null)
            //{
            //    ballance = await contract.GetFunction("balances").CallAsync<BigInteger>($"0x{ByteArrayToString(user.EthAddress)}");
            //    refund = await contract.GetFunction("deposited").CallAsync<BigInteger>($"0x{ByteArrayToString(user.EthAddress)}");
            //}

            return Ok(new
            {
                //Sold = UnitConversion.Convert.FromWei(currentTokenSold),
                //Step = currentStep,
                //Ballance = UnitConversion.Convert.FromWei(ballance),
                //Refund = UnitConversion.Convert.FromWei(refund),
                //StepEndTime = dueTime
            });
        }

        [HttpGet("calc/{amount}")]
        public async Task<IActionResult> GetCalcAsync([FromRoute]int amount)
        {
            //var result = await _contract.GetFunction("getPriceForTokens").CallAsync<BigInteger>(UnitConversion.Convert.ToWei(amount));
            return Ok(Math.Ceiling(UnitConversion.Convert.FromWei(1000000000000000000) * 1000000) / 1000000);
        }

        [HttpGet("addr")]
        public IActionResult GetAddr()
        {
            return Ok(_contract.Address);
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