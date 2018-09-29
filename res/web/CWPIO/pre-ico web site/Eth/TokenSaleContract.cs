using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using System.Numerics;
using System.Threading.Tasks;

namespace pre_ico_web_site.Eth
{
    public class TokenSaleContract
    {
        private readonly Contract _contract;
        private readonly EthSettings _settings;
        private readonly ILogger _logger;
        private readonly Web3 _web3;
        public string Address { get { return _contract.Address; } }

        public TokenSaleContract(
            Web3 web3,
            IOptions<EthSettings> options,
            IHostingEnvironment hostingEnvironment,
            ILogger<TokenSaleContract> logger)
        {
            _logger = logger;
            _settings = options.Value;
            _web3 = web3;
            string contentRootPath = hostingEnvironment.WebRootPath;
            var JSON = System.IO.File.ReadAllText(contentRootPath + "/static/abi/CWTPTokenSale.json");
            dynamic jObject = JsonConvert.DeserializeObject<dynamic>(JSON);
            var abi = jObject.abi.ToString();
            var contractAddress = jObject.networks[_settings.Network.ToString()].address.ToString();
            _contract = web3.Eth.GetContract(abi, contractAddress);
        }

        public Task AddAddressToWhitelistAsync(string addr)
        {
            return _contract.GetFunction("addAddressToWhitelist")
                .SendTransactionAndWaitForReceiptAsync(_settings.AppAddress, null, addr);
        }

        public async Task<FixRateModel> SetRateForTransactionAsync(int rate, string buyer, BigInteger amount)
        {
            var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

            var hash = await _contract.GetFunction("setRateForTransaction")
                .SendTransactionAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), rate, buyer, amount);

            _logger.LogCritical("Transaction hash: {0}", hash);

            await WaitReciept(hash);
            _logger.LogCritical("done");
            return await _contract.GetFunction("fixRate").CallDeserializingToObjectAsync<FixRateModel>(buyer);
        }

        public Task<long> WeiRaisedAsync()
        {
            return _contract.GetFunction("weiRaised").CallAsync<long>();
        }

        private async Task WaitReciept(string hash)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);

            while (receipt == null)
            {
                await Task.Delay(1000);
                receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(hash);
            }
        }
    }
}
