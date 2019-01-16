using ExchangerMonitor.Model;
using ExchangerMonitor.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ExchangerMonitor.Services
{
    public class EthService : IEthService
    {
        private Web3 _web3;
        private readonly ILogger _logger;
        private EthSettings _opts;
        private ICryptoService _crypto;
        private JsonModel _json;
        private readonly RpcClient _client;

        public EthService(IOptions<EthSettings> options, ICryptoService crypto, ILogger<EthService> logger)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = logger;
            _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
            _opts = options.Value;
            var account = new Account(_opts.AppPrivateKey);
            var uri = new Uri(_opts.NodeUrl ?? "http://localhost:8545");
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(uri.UserInfo));
            _client = new RpcClient(uri, new AuthenticationHeaderValue("Basic", authHeader));
            _web3 = new Web3(account, _client);

            var json = File.ReadAllText("Exchanger.json");

            _json = JsonConvert.DeserializeObject<JsonModel>(json);

        }

        public async Task<ExchangeOperationStatus> GetTransactionStatus(string txHash)
        {
            //await this.waitForConnect();
            _logger?.LogDebug("check transaction \"{0}\"", txHash);
            try
            {
                var tx = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txHash);
                if (tx == null)
                {
                    return ExchangeOperationStatus.Skip;
                }
                else
                {
                    bool result = tx.BlockNumber != null;
                    if (result)
                    {
                        var receipt = await GetTransactionReceiptAsync(tx.TransactionHash);
                        if (receipt == null)
                        {
                            return ExchangeOperationStatus.Skip;
                        }

                        result = result && (receipt.Status.Value > 0);
                    }
                    return result ? ExchangeOperationStatus.Ok : ExchangeOperationStatus.Failed;
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error");
                _logger.LogDebug(_opts.NodeUrl);
                throw;
            }
        }

        //public Task<Transaction> GetTransactionAsync(string currentTx)
        //{
        //    return _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(currentTx);
        //}

        public async Task<string> SetRateAsync(int rate, string buyer, BigInteger amount)
        {
            _logger?.LogDebug("Set rate {0} for buyer {1} and amount {2}", rate, buyer, amount);
            Contract contract = _web3.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
            FixRateModel fixRateModel = await GetRateForBuyerAsync(buyer);
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (fixRateModel.Amount != amount || fixRateModel.Time - unixTimestamp <= 0)
            {
                var gasPrice = await _web3.Eth.GasPrice.SendRequestAsync();
                var currentGasPrice = gasPrice.Value * 2;

                var hash = await contract.GetFunction("setRateForTransaction")
                    .SendTransactionAsync(_opts.AppAddress, new HexBigInteger(0x153D8), new HexBigInteger(currentGasPrice), new HexBigInteger(0), rate, buyer, amount);

                return hash;
            }
            return string.Empty;
        }

        public async Task<FixRateModel> GetRateForBuyerAsync(string buyer)
        {
            _logger?.LogDebug("Get rate for buyer {0}", buyer);
            Contract contract = _web3.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
            var fixRateModel = await contract.GetFunction("fixRate").CallDeserializingToObjectAsync<FixRateModel>(buyer);
            return fixRateModel;
        }

        //public async Task<string> AddToWhitelist(string address)
        //{
        //    _logger?.LogDebug("Add to whitelist {0}", address);
        //    Contract contract = _web3.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
        //    var price = await _web3.Eth.GasPrice.SendRequestAsync();

        //    return await contract.GetFunction("addAddressToWhitelist")
        //        .SendTransactionAsync(
        //        _opts.AppAddress,
        //        new HexBigInteger(0xBB80),
        //        new HexBigInteger(price.Value * 2),
        //        new HexBigInteger(0),
        //        address);

        //}

        public async Task<TransactionReceipt> GetTransactionReceiptAsync(string transactionHash)
        {
            return await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
        }

        public async Task<string> SendToSmartContractAsync(string privateKey, string buyer, BigInteger amount)
        {
            _logger?.LogDebug("Send ETH to smart contract from {0} amount {1}", buyer, amount);

            var account = new Account(_crypto.Decrypt(privateKey.StringToByteArray()));
            var web3local = new Web3(account, _client);

            var price = await web3local.Eth.GasPrice.SendRequestAsync();

            Contract contract = web3local.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
            var input = contract.GetFunction("buyTokens").CreateTransactionInput(
                account.Address,
                new HexBigInteger(0x30A28),
                new HexBigInteger(price.Value * 2),
                new HexBigInteger(amount),
                buyer
            );

            var gas = await web3local.Eth.TransactionManager.EstimateGasAsync(input);
            input.Gas = new HexBigInteger(gas.Value + 10000);
            var tx = await web3local.Eth.TransactionManager.SendTransactionAsync(input);
            return tx;
        }

        //public async Task<string> SendTokensToUserAsync(string pk, string toAddress, int tokensAmount)
        //{
        //    var account = new Account(_crypto.Decrypt(pk.StringToByteArray()));
        //    var _web3t = new Web3(account, _opts.NodeUrl);

        //    var price = await _web3.Eth.GasPrice.SendRequestAsync();

        //    Contract contract = _web3t.Eth.GetContract(_json.Token.ABI.ToString(), _opts.TokenContractAddr);
        //    var tokenBallance = await contract.GetFunction("balanceOf").CallAsync<BigInteger>(account.Address);
        //    if (UnitConversion.Convert.FromWei(tokenBallance) != tokensAmount)
        //    {
        //        throw new Exception("Tokens amount not valid!");
        //    }

        //    var gas = await contract.GetFunction("transfer").EstimateGasAsync(account.Address,
        //        new HexBigInteger(0x9C40),
        //        new HexBigInteger(0),
        //        toAddress,
        //        UnitConversion.Convert.ToWei(tokensAmount));

        //    var tx = await contract.GetFunction("transfer").SendTransactionAsync(account.Address,
        //        new HexBigInteger(gas.Value + 10000),
        //        new HexBigInteger(price.Value * 2),
        //        new HexBigInteger(0),
        //        toAddress,
        //        UnitConversion.Convert.ToWei(tokensAmount)
        //    );

        //    return tx;
        //}

        public async Task<string> SendRefundToUserAsync(string pk, string to)
        {
            _logger?.LogDebug("Send refund to user {0}", to);

            var account = new Account(_crypto.Decrypt(pk.StringToByteArray()));
            var web3local = new Web3(account, _client);
            var ballance = await web3local.Eth.GetBalance.SendRequestAsync(account.Address);
            var price = await web3local.Eth.GasPrice.SendRequestAsync();
            var input = new TransactionInput(
                        "",
                        to,
                        account.Address,
                        new HexBigInteger(0x30A28),
                        new HexBigInteger(price.Value * 2),
                        new HexBigInteger(0x5208 * price.Value * 2)
                    );
            var gas = await web3local.Eth.TransactionManager.EstimateGasAsync(input);

            if (ballance.Value > gas * price.Value * 2)
            {
                input.Value = new HexBigInteger(ballance.Value - gas.Value * price.Value * 2);
                input.Gas = gas;
                var tx = await web3local.Eth.TransactionManager.SendTransactionAsync(input);
                return tx;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<(string tx, int blockNumber)> GetInTransactionFromBlockAsync(string exchanger, int fromBlock)
        {
            int lastBlock = fromBlock;
            for (int i = fromBlock; i <= (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value; i++)
            {
                lastBlock = i;
                _logger.LogDebug("Check block {0}", i);
                var block = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new HexBigInteger(i));
                var tr = block.Transactions.Where(t => t.To != null && t.To.ToLowerInvariant() == exchanger.ToLowerInvariant()).FirstOrDefault();
                if (tr != null)
                    return (tr.TransactionHash, i);
            }

            return (null, lastBlock);
        }
    }
}


