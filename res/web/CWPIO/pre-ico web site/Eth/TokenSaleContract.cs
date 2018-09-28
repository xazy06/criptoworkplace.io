using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Web3;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using System.Threading;
using System.Threading.Tasks;

namespace pre_ico_web_site.Eth
{
    public class TokenSaleContract
    {
        private readonly Contract _contract;
        private readonly EthSettings _settings;

        public string Address { get { return _contract.Address; } }

        public TokenSaleContract(Web3 web3, IOptions<EthSettings> options, IHostingEnvironment hostingEnvironment)
        {
            _settings = options.Value;
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

        public Task<long> SetRateForTransactionAsync(int rate, string buyer)
        {
            return _contract.GetFunction("setRateForTransaction")
                .CallAsync<long>(_settings.AppAddress, rate, buyer);
        }
    }
}
