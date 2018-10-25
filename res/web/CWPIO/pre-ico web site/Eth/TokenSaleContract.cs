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

        public Task<bool> CheckWhitelistAsync(string wallet)
        {
            return _saleContract.GetFunction("whitelist").CallAsync<bool>(wallet);
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
                long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                if (fixRateModel.Time - unixTimestamp > 0)
                {

                    _logger.LogDebug($"fixRateModel.Time: {fixRateModel.Time}, unixTimestamp: {unixTimestamp}, div {fixRateModel.Time - unixTimestamp}");
                    _memoryCache.Set(key, fixRateModel, TimeSpan.FromSeconds(fixRateModel.Time - unixTimestamp));
                }
            }
            return fixRateModel;
        }

        public async Task<FixRateModel> SetRateForTransactionAsync(int rate, string buyer, BigInteger amount)
        {
            FixRateModel fixRateModel = await GetRateForBuyerAsync(buyer);
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (fixRateModel.Amount != amount || fixRateModel.Time - unixTimestamp <= 0)
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

        public async Task<string> CreateExchangerAsync(string ethAddress)
        {
            string contractByteCode = "0x608060405234801561001057600080fd5b506040516060806108b9833981018060405281019080805190602001909291908051906020019092919080519060200190929190505050336000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555082600160006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555081600360006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555080600260006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff16021790555050505061075d8061015c6000396000f300608060405260043610610062576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff168063715018a61461006457806389749adb1461007b5780638da5cb5b146100a8578063f2fde38b146100ff575b005b34801561007057600080fd5b50610079610142565b005b34801561008757600080fd5b506100a660048036038101908080359060200190929190505050610244565b005b3480156100b457600080fd5b506100bd6105ab565b604051808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200191505060405180910390f35b34801561010b57600080fd5b50610140600480360381019080803573ffffffffffffffffffffffffffffffffffffffff1690602001909291905050506105d0565b005b6000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614151561019d57600080fd5b6000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff167ff8df31144d9c2f0f6b59d69b8b98abd5459d07f2742c4df920b25aae33c6482060405160405180910390a260008060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550565b60008060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff161415156102a157600080fd5b600160009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166108fc839081150290604051600060405180830381858888f19350505050158015610309573d6000803e3d6000fd5b50600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166370a08231306040518263ffffffff167c0100000000000000000000000000000000000000000000000000000000028152600401808273ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001915050602060405180830381600087803b1580156103c757600080fd5b505af11580156103db573d6000803e3d6000fd5b505050506040513d60208110156103f157600080fd5b81019080805190602001909291905050509050600360009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff1663a9059cbb600260009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16836040518363ffffffff167c0100000000000000000000000000000000000000000000000000000000028152600401808373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200182815260200192505050602060405180830381600087803b1580156104eb57600080fd5b505af11580156104ff573d6000803e3d6000fd5b505050506040513d602081101561051557600080fd5b810190808051906020019092919050505050600260009054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff166108fc3073ffffffffffffffffffffffffffffffffffffffff16319081150290604051600060405180830381858888f193505050501580156105a6573d6000803e3d6000fd5b505050565b6000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1681565b6000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff163373ffffffffffffffffffffffffffffffffffffffff1614151561062b57600080fd5b61063481610637565b50565b600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff161415151561067357600080fd5b8073ffffffffffffffffffffffffffffffffffffffff166000809054906101000a900473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a3806000806101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550505600a165627a7a7230582005895e14cd6bb915d594ea46026fd0390224de7553f6d27adddcd72ccea8769c0029";
           
            var receipt = await _web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(contractByteCode, _settings.AppAddress,
                new
                {
                    crowdsale = _saleContract.Address,
                    token = _tokenContract.Address,
                    buyer = ethAddress
                });
            return receipt.ContractAddress;
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
