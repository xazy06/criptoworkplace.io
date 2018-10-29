using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public class Eth
    {
        private Web3 _web3;
        private ILogger _logger;
        private EthSettings _opts;
        private Crypto _crypto;
        private JsonModel _json;

        public Eth(IOptions<EthSettings> options, Crypto crypto, ILogger<Eth> logger)
        {
            _logger = logger;
            _crypto = crypto;
            _opts = options.Value;
            var account = new Account(_opts.AppPrivateKey);
            _web3 = new Web3(account, _opts.NodeUrl);

            var json = File.ReadAllText("Exchanger.json");

            _json = JsonConvert.DeserializeObject<JsonModel>(json);

        }

        public async Task<ExchangeOperationStatus> GetTransactionStatus(string txHash)
        {
            //await this.waitForConnect();
            _logger.LogDebug("check transaction \"" + txHash + "\"");
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

        public Task<Transaction> GetTransactionToAsync(string currentTx)
        {
            return _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(currentTx);
        }

        public async Task<string> SetRateForTransactionAsync(int rate, string buyer, BigInteger amount)
        {
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
            Contract contract = _web3.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
            var fixRateModel = await contract.GetFunction("fixRate").CallDeserializingToObjectAsync<FixRateModel>(buyer);
            return fixRateModel;
        }

        public async Task<string> SendAddToWhitelist(string address)
        {
            Contract contract = _web3.Eth.GetContract(_json.Sale.ABI.ToString(), _opts.SmartContractAddr);
            var price = await _web3.Eth.GasPrice.SendRequestAsync();

            return await contract.GetFunction("addAddressToWhitelist")
                .SendTransactionAsync(
                _opts.AppAddress,
                new HexBigInteger(0xBB80),
                new HexBigInteger(price.Value * 2),
                new HexBigInteger(0),
                address);

        }

        public async Task<TransactionReceipt> GetTransactionReceiptAsync(string transactionHash)
        {
            return await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
        }

        public async Task<string> SendToSmartContractAsync(string pk, BigInteger amount)
        {
            var account = new Account(_crypto.Decrypt(pk.StringToByteArray()));
            var _web3t = new Web3(account, _opts.NodeUrl);

            var price = await _web3t.Eth.GasPrice.SendRequestAsync();
            var input = new TransactionInput(
                    "",
                    _opts.SmartContractAddr,
                    account.Address,
                    new HexBigInteger(0x30A28),
                    new HexBigInteger(price.Value * 2),
                    new HexBigInteger(amount)
                );

            var gas = await _web3t.Eth.TransactionManager.EstimateGasAsync(input);
            input.Gas = new HexBigInteger(gas.Value + 10000);
            var tx = await _web3t.Eth.TransactionManager.SendTransactionAsync(input);
            return tx;
        }

        public async Task<string> SendTokensToUserAsync(string pk, string toAddress, int tokensAmount)
        {
            var account = new Account(_crypto.Decrypt(pk.StringToByteArray()));
            var _web3t = new Web3(account, _opts.NodeUrl);

            var price = await _web3.Eth.GasPrice.SendRequestAsync();

            Contract contract = _web3t.Eth.GetContract(_json.Token.ABI.ToString(), _opts.TokenContractAddr);
            var tokenBallance = await contract.GetFunction("balanceOf").CallAsync<BigInteger>(account.Address);
            if (UnitConversion.Convert.FromWei(tokenBallance) != tokensAmount)
            {
                throw new Exception("Tokens amount not valid!");
            }

            var tx = await contract.GetFunction("transfer").SendTransactionAsync(account.Address,
                new HexBigInteger(0x59D8),
                    new HexBigInteger(price.Value * 2),
                    new HexBigInteger(0),
                    toAddress,
                    UnitConversion.Convert.ToWei(tokensAmount)
                );

            return tx;
        }

        public async Task<string> SendRefundToUserAsync(string pk, string to)
        {
            var account = new Account(_crypto.Decrypt(pk.StringToByteArray()));
            var _web3t = new Web3(account, _opts.NodeUrl);
            var ballance = await _web3t.Eth.GetBalance.SendRequestAsync(account.Address);
            var price = await _web3t.Eth.GasPrice.SendRequestAsync();

            if (ballance.Value > 0x5208 * price.Value * 2)
            {
                var tx = await _web3t.Eth.TransactionManager.SendTransactionAsync(
                    new TransactionInput(
                        "",
                        to,
                        account.Address,
                        new HexBigInteger(0x5208),
                        new HexBigInteger(price.Value * 2),
                        new HexBigInteger(ballance.Value - 0x5208 * price.Value * 2)
                    ));
                return tx;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

[FunctionOutput]
public class FixRateModel
{
    [Parameter("uint256", "rate", 1)]
    public int Rate { get; set; }

    [Parameter("uint256", "time", 2)]
    public long Time { get; set; }

    [Parameter("uint256", "amount", 3)]
    public BigInteger Amount { get; set; }
}

public enum ExchangeOperationStatus
{
    Skip = 0,
    Ok = 1,
    Failed = 2
}
