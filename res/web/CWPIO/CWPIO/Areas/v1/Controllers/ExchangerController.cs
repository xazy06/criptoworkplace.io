using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CWPIO.Data;
using CWPIO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;

namespace CWPIO.Areas.v1.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Area("v1")]
    [Route("api/v1/exchanger")]
    public class ExchangerController : Controller
    {
        private Web3 _web3;
        private ApplicationDbContext _dbContext;
        private string _abi;
        private string _byteCode;
        private string _contractAddress;
        private readonly EthSettings _options;

        public ExchangerController(ApplicationDbContext dbContext, Web3 web3, IOptions<EthSettings> options, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _web3 = web3;
            _options = options.Value;

            string contentRootPath = hostingEnvironment.WebRootPath;
            var JSON = System.IO.File.ReadAllText(contentRootPath + "/static/abi/CWTPTokenSale.json");
            dynamic jObject = JsonConvert.DeserializeObject<dynamic>(JSON);
            _abi = jObject.abi.ToString();
            _byteCode = jObject.bytecode.ToString();
            _contractAddress = jObject.networks[_options.Network.ToString()].address;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
                                  

            var contract = _web3.Eth.GetContract(_abi, _contractAddress);

            var currentCap = await contract.GetFunction("getCurrentCap").CallAsync<BigInteger>();
            var currentRate = await contract.GetFunction("getCurrentRate").CallAsync<BigInteger>();
            var currentTokenSold = await contract.GetFunction("getCurrentTokenSold").CallAsync<BigInteger>();
            var currentStep = await contract.GetFunction("getCurrentStep").CallAsync<byte>();
            var dueTime = await contract.GetFunction("steps").CallAsync<long>(currentStep);
            BigInteger ballance = 0;
            BigInteger refund = 0;
            if (!string.IsNullOrEmpty(user.EthAddress))
            {
                ballance = await contract.GetFunction("balances").CallAsync<BigInteger>(user.EthAddress);
                refund = await contract.GetFunction("deposited").CallAsync<BigInteger>(user.EthAddress);
            }

            return Ok(new
            {
                Cap = UnitConversion.Convert.FromWei(currentCap),
                Rate = UnitConversion.Convert.FromWei(currentRate),
                Sold = UnitConversion.Convert.FromWei(currentTokenSold),
                Step = currentStep,
                Ballance = UnitConversion.Convert.FromWei(ballance),
                Refund = UnitConversion.Convert.FromWei(refund),
                StepEndTime = dueTime
            });
        }

        [HttpGet("calc/{amount}")]
        public async Task<IActionResult> GetCalcAsync([FromRoute]int amount)
        {
            var contract = _web3.Eth.GetContract(_abi, _contractAddress);
            var result = await contract.GetFunction("getPriceForTokens").CallAsync<BigInteger>(UnitConversion.Convert.ToWei(amount));
            return Ok(Math.Ceiling(UnitConversion.Convert.FromWei(result) * 1000000) / 1000000);
        }

        [HttpGet("addr")]
        public IActionResult GetAddr()
        {
            return Ok(_contractAddress);
        }

        [HttpGet("refund")]
        public async Task<IActionResult> GetRefundAsync()
        {
            var user = await _dbContext.GetCurrentUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var contract = _web3.Eth.GetContract(_abi, _contractAddress);
            var result = await contract.GetFunction("refund").SendTransactionAsync(_options.AppAddress, user.EthAddress);
            return Ok(result);
        }
    }
}