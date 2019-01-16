using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using pre_ico_web_site.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private const string mem_key = "fixrate:{0}:{1}";
        private readonly string _saleContractABI;
        public string Address => _saleContract.Address;


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
                (_saleContract, _saleContractABI) = LoadContractFromMetadata(web3, _settings.Network.ToString(), stream);
            }
            using (var stream = new MemoryStream())
            {
                files.GetFileByName(gdriveOptions.Value.TokenContractFileName, stream);
                stream.Seek(0, SeekOrigin.Begin);
                (_tokenContract, _) = LoadContractFromMetadata(web3, _settings.Network.ToString(), stream);
            }
        }

        public async Task<bool> CheckTxStatusAsync(string whiteListTransaction)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(whiteListTransaction);
            return receipt != null;
        }

        public Task<bool> CheckWhitelistAsync(string wallet)
        {
            return _saleContract.GetFunction("whitelist").CallAsync<bool>(wallet);
        }

        private static (Contract contract, string abi) LoadContractFromMetadata(Web3 web3, string netId, Stream json)
        {
            //var JSON = System.IO.File.ReadAllText(contractFile);
            using (var reader = new StreamReader(json))
            {
                dynamic jObject = JsonConvert.DeserializeObject<dynamic>(reader.ReadToEnd());
                var abi = jObject.abi.ToString();
                var contractAddress = jObject.networks[netId].address.ToString();
                return (web3.Eth.GetContract(abi, contractAddress), abi);
            }
        }

        public string GetSaleContractABI()
        {
            return _saleContractABI;
        }

        public async Task<BigInteger> GetGasPriceAsync()
        {
            return (await _web3.Eth.GasPrice.SendRequestAsync()).Value;
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

        public async Task<FixRateModel> GetRateForBuyerAsync(string buyer, BigInteger amount)
        {
            var key = string.Format(mem_key, buyer, amount);
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
            FixRateModel fixRateModel = await GetRateForBuyerAsync(buyer, amount);
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (fixRateModel.Amount != amount || fixRateModel.Time - unixTimestamp <= 0)
            {
                var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

                var hash = await _saleContract.GetFunction("setRateForTransaction")
                    .SendTransactionAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), rate, buyer, amount);

                _logger.LogDebug("Transaction hash: {0}", hash);

                await WaitReciept(hash);
                _logger.LogDebug("done");
                fixRateModel = await GetRateForBuyerAsync(buyer, amount);
            }
            return fixRateModel;
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

        public async Task<(string status, string transaction)> CheckStatusAsync(string address, BigInteger fromBlock)
        {

            (string status, string transaction) result = ("error", "");

            if (fromBlock < 0)
            {
                fromBlock = (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value - 50;
                //var res = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(BlockParameter.CreatePending());
                //var tx = res.Transactions.Where(t => t != null && t.To != null && t.To.ToUpperInvariant() == address.ToUpperInvariant()).FirstOrDefault();
                //if (tx == null)
                //{
                //    result = ("recieved", "");
                //}
                //else
                //{
                //    result = ("complete", tx.TransactionHash);
                //}
            }
            //else
            //{
                List<Task<BlockWithTransactions>> tasks = new List<Task<BlockWithTransactions>>();

                for (BigInteger i = fromBlock; i < (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value; i++)
                {
                    tasks.Add(_web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(i)));
                }
                tasks.Add(_web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(BlockParameter.CreatePending()));

                var allTransactions = (await Task.WhenAll(tasks)).SelectMany(t => t.Transactions).Distinct(new TransactionComparer());
                var tx = allTransactions.Where(t => t != null && t.To != null && t.To.ToUpperInvariant() == address.ToUpperInvariant()).FirstOrDefault();

                if (tx == null)
                {
                    result = ("recieved", "");
                }
                else
                {
                    result = ("complete", tx.TransactionHash);
                }
            //}

            return result;
        }

        public async Task<BigInteger> GetCurrentBlockNumberAsync()
        {
            return (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value;
        }
    }
}
