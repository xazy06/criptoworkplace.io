using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace pre_ico_web_site.Eth
{
    public class TokenSaleContract
    {
        private readonly Contract _saleContract;
        private readonly Contract _tokenContract;
        private readonly EthSettings _settings;
        private readonly ILogger _logger;
        private readonly Web3 _web3;
        private readonly IMemoryCache _memoryCache;
        private const string mem_key = "fixrate:{0}";
        public string Address { get { return _saleContract.Address; } }

        public TokenSaleContract(
            Web3 web3,
            IOptions<EthSettings> options,
            IFileRepository files,
            IOptions<GoogleDriveSettings> gdriveOptions,
            ILogger<TokenSaleContract> logger,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _settings = options.Value;
            _web3 = web3;
            _memoryCache = memoryCache;
            //string contentRootPath = hostingEnvironment.WebRootPath;
            using (var stream = new MemoryStream())
            {
                files.GetFileByName(gdriveOptions.Value.SaleContractFileName, stream);
                stream.Seek(0, SeekOrigin.Begin);
                _saleContract = LoadContractFromMetadata(web3, _settings.Network.ToString(), stream);
            }
            using (var stream = new MemoryStream())
            {
                files.GetFileByName(gdriveOptions.Value.TokenContractFileName, stream);
                stream.Seek(0, SeekOrigin.Begin);
                _tokenContract = LoadContractFromMetadata(web3, _settings.Network.ToString(), stream);
            }
        }

        private static Contract LoadContractFromMetadata(Web3 web3, string netId, Stream json)
        {
            //var JSON = System.IO.File.ReadAllText(contractFile);
            using (var reader = new StreamReader(json))
            {
                dynamic jObject = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
                var abi = jObject.abi.ToString();
                var contractAddress = jObject.networks[netId].address.ToString();
                return web3.Eth.GetContract(abi, contractAddress);
            }
        }

        internal Task<BigInteger> GetCapAsync()
        {
            return _saleContract.GetFunction("tokenCap").CallAsync<BigInteger>();
        }

        internal Task<BigInteger> TokenSoldAsync()
        {
            return _saleContract.GetFunction("tokenSold").CallAsync<BigInteger>();
        }

        internal Task<BigInteger> GetBallanceAsync(string ethAddress)
        {
            return _tokenContract.GetFunction("balanceOf").CallAsync<BigInteger>(ethAddress);
        }

        public async Task<string> AddAddressToWhitelistAsync(string addr)
        {
            if (!(await _saleContract.GetFunction("whitelist").CallAsync<bool>(addr)))
            {

                var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

                return await _saleContract.GetFunction("addAddressToWhitelist")
                    .SendTransactionAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), null, addr);
            }
            return string.Empty;
        }

        public async Task<FixRateModel> GetRateForBuyerAsync(string buyer)
        {
            var key = string.Format(mem_key, buyer);
            if (!_memoryCache.TryGetValue(key, out FixRateModel fixRateModel))
            {
                fixRateModel = await _saleContract.GetFunction("fixRate").CallDeserializingToObjectAsync<FixRateModel>(buyer);
                if (fixRateModel.Time > 0)
                {
                    long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    _memoryCache.Set(key, fixRateModel, TimeSpan.FromSeconds(fixRateModel.Time - unixTimestamp));
                }
            }
            return fixRateModel;
        }

        public async Task<FixRateModel> SetRateForTransactionAsync(int rate, string buyer, BigInteger amount)
        {
            FixRateModel fixRateModel = await GetRateForBuyerAsync(buyer);
            if (fixRateModel.Amount != amount)
            {
                var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

                var hash = await _saleContract.GetFunction("setRateForTransaction")
                    .SendTransactionAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), rate, buyer, amount);

                _logger.LogCritical("Transaction hash: {0}", hash);

                await WaitReciept(hash);
                _logger.LogCritical("done");
                fixRateModel = await GetRateForBuyerAsync(buyer);
            }
            return fixRateModel;
        }

        public Task<BigInteger> GetRefundAmountAsync(string ethAddress)
        {
            return _saleContract.GetFunction("payments").CallAsync<BigInteger>(ethAddress);
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
