using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CWPIO.Data;
using CWPIO.Models;
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
            var claim = User.Claims.FirstOrDefault(c => c.Type == "EtherscanAccount");
            

            var contract = _web3.Eth.GetContract(_abi, _contractAddress);

            var currentCap = await contract.GetFunction("getCurrentCap").CallAsync<BigInteger>();
            var currentRate = await contract.GetFunction("getCurrentRate").CallAsync<BigInteger>();
            var currentTokenSold = await contract.GetFunction("getCurrentTokenSold").CallAsync<BigInteger>();
            var currentStep = await contract.GetFunction("getCurrentStep").CallAsync<byte>();

            BigInteger ballance = 0;
            if (!string.IsNullOrEmpty(claim?.Value))
            {
                ballance = await contract.GetFunction("balances").CallAsync<BigInteger>();
            }

            return Ok(new
            {
                Cap = UnitConversion.Convert.FromWei(currentCap),
                Rate = UnitConversion.Convert.FromWei(currentRate),
                Sold = UnitConversion.Convert.FromWei(currentTokenSold),
                Step = currentStep,
                Ballance = ballance
            });
        }

        [HttpGet("calc/{amount}")]
        public async Task<IActionResult> GetCalcAsync([FromRoute]int amount)
        {
            var contract = _web3.Eth.GetContract(_abi, _contractAddress);
            var result = await contract.GetFunction("getPriceForTokens").CallAsync<BigInteger>(UnitConversion.Convert.ToWei(amount));
            return Ok(UnitConversion.Convert.FromWei(result));
        }
    }
}