using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
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


        #region Exchanger contract
        private const string EXCHANGER_BYTECODE = "608060405234801561001057600080fd5b5060405160608061051a8339810160409081528151602083015191909201516000805433600160a060020a0319918216178255600180548216600160a060020a0396871617905560038054821694861694909417909355600280549093169390911692909217905561049290819061008890396000f3006080604052600436106100615763ffffffff7c0100000000000000000000000000000000000000000000000000000000600035041663715018a6811461006b5780638da5cb5b14610080578063d81111ab146100b1578063f2fde38b146100c6575b6100696100e7565b005b34801561007757600080fd5b5061006961034b565b34801561008c57600080fd5b506100956103b7565b60408051600160a060020a039092168252519081900360200190f35b3480156100bd57600080fd5b506100696100e7565b3480156100d257600080fd5b50610069600160a060020a03600435166103c6565b600080548190600160a060020a0316331461010157600080fd5b600154600254604080517f626a9198000000000000000000000000000000000000000000000000000000008152600160a060020a0392831660048201529051919092169163626a91989160248083019260609291908290030181600087803b15801561016c57600080fd5b505af1158015610180573d6000803e3d6000fd5b505050506040513d606081101561019657600080fd5b506040908101516001549151909350600160a060020a039091169083156108fc029084906000818181858888f193505050501580156101d9573d6000803e3d6000fd5b50600354604080517f70a082310000000000000000000000000000000000000000000000000000000081523060048201529051600160a060020a03909216916370a08231916024808201926020929091908290030181600087803b15801561024057600080fd5b505af1158015610254573d6000803e3d6000fd5b505050506040513d602081101561026a57600080fd5b5051600354600254604080517fa9059cbb000000000000000000000000000000000000000000000000000000008152600160a060020a03928316600482015260248101859052905193945091169163a9059cbb916044808201926020929091908290030181600087803b1580156102e057600080fd5b505af11580156102f4573d6000803e3d6000fd5b505050506040513d602081101561030a57600080fd5b5050600254604051600160a060020a0390911690303180156108fc02916000818181858888f19350505050158015610346573d6000803e3d6000fd5b505050565b600054600160a060020a0316331461036257600080fd5b60008054604051600160a060020a03909116917ff8df31144d9c2f0f6b59d69b8b98abd5459d07f2742c4df920b25aae33c6482091a26000805473ffffffffffffffffffffffffffffffffffffffff19169055565b600054600160a060020a031681565b600054600160a060020a031633146103dd57600080fd5b6103e6816103e9565b50565b600160a060020a03811615156103fe57600080fd5b60008054604051600160a060020a03808516939216917f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e091a36000805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a03929092169190911790555600a165627a7a72305820d1d360a09c56f4b6a9b7f835fa16158f3bc9fb10ef6fac3488e8d4461997835e0029";

        private const string EXHCANGER_ABI = @"[
	{
		""constant"": false,
		""inputs"": [],
		""name"": ""renounceOwnership"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""owner"",
		""outputs"": [
			{
				""name"": """",
				""type"": ""address""

            }
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [],
		""name"": ""BuyTokens"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""name"": ""_newOwner"",
				""type"": ""address""
			}
		],
		""name"": ""transferOwnership"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""inputs"": [
			{
				""name"": ""crowdsale"",
				""type"": ""address""
			},
			{
				""name"": ""token"",
				""type"": ""address""
			},
			{
				""name"": ""buyer"",
				""type"": ""address""
			}
		],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""constructor""
	},
	{
		""payable"": true,
		""stateMutability"": ""payable"",
		""type"": ""fallback""
	},
	{
		""anonymous"": false,
		""inputs"": [
			{
				""indexed"": true,
				""name"": ""previousOwner"",
				""type"": ""address""
			}
		],
		""name"": ""OwnershipRenounced"",
		""type"": ""event""
	},
	{
		""anonymous"": false,
		""inputs"": [
			{
				""indexed"": true,
				""name"": ""previousOwner"",
				""type"": ""address""
			},
			{
				""indexed"": true,
				""name"": ""newOwner"",
				""type"": ""address""
			}
		],
		""name"": ""OwnershipTransferred"",
		""type"": ""event""
	}
]";
        #endregion

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
                    .SendTransactionAsync(
                        _settings.AppAddress,
                        new HexBigInteger(_settings.GasLimit),
                        new HexBigInteger(currentGasPrice),
                        new HexBigInteger(0),
                        addr);
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

        public async Task<string> CreateExchangerAsync(byte[] from, string ethAddress)
        {
            var account = new Account(from);
            var web3t = new Web3(account, _settings.NodeUrl ?? "http://localhost:8545");

            var receipt = await web3t.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(
                EXHCANGER_ABI,
                EXCHANGER_BYTECODE,
                account.Address,
                new HexBigInteger(1000000),
                null,
                _saleContract.Address,
                _tokenContract.Address,
                 ethAddress
                );
            return receipt.ContractAddress;
        }

        public Task<BigInteger> GetRefundAmountAsync(string ethAddress)
        {
            return _saleContract.GetFunction("payments").CallAsync<BigInteger>(ethAddress);
        }

        public (string address, string pk) NewAddress()
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            var account = new Account(privateKey);
            return (account.Address, privateKey);
        }

        public async Task WaitReciept(string hash)
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
