using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public class Eth
    {
        private Web3 _web3;
        private ILogger _logger;
        private EthSettings _opts;
     
        public Eth(IOptions<EthSettings> options, ILogger<Eth> logger)
        {
            _logger = logger;
            _opts = options.Value;
            var account = new Account(_opts.AppPrivateKey);
            _web3 = new Web3(account, _opts.NodeUrl);
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
                    var receipt = await WaitForReciept(tx.TransactionHash);
                    if (receipt == null)
                    {
                        return ExchangeOperationStatus.Skip;
                    }

                    result = result && (receipt.Status.Value > 0);
                }
                return result ? ExchangeOperationStatus.Ok : ExchangeOperationStatus.Failed;
            }
        }

        public async Task<HexBigInteger> GetTransactionAmount(string txHash)
        {
            //private await this.waitForConnect();
            var tx = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(txHash);
            if (tx == null)
            {
                return new HexBigInteger("0x0");
            }
            return tx.Value;
        }

        public async Task<string> GetTransactionToAsync(string currentTx)
        {
            var t = await _web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(currentTx);
            return t.To;
        }

        private async Task<TransactionReceipt> WaitForReciept(string tx)
        {
            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(tx);

            //while (receipt == null)
            //{
            //    await Task.Delay(1000);
            //    receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(tx);
            //}
            return receipt;
        }

        public Task<string> SendToUserAsync(string to, HexBigInteger tokens)
        {
            var contract= _web3.Eth.GetContract("[{\"constant\": false,\"inputs\": [{\"name\": \"_to\", \"type\": \"address\"},{\"name\": \"_value\", \"type\": \"uint256\"}], \"name\": \"transfer\", \"outputs\": [{\"name\": \"\",\"type\": \"bool\"}], \"payable\": false,	\"stateMutability\": \"nonpayable\", \"type\": \"function\"	}]",
                Environment.GetEnvironmentVariable("Ether:TokenContractAddr"));
            return contract.GetFunction("transfer").SendTransactionAsync(Environment.GetEnvironmentVariable("Ether:AppAddress"), to, tokens.Value);

        }

        public async Task<string> SendToSmartContractAsync(HexBigInteger amount)
        {
            var price = await _web3.Eth.GasPrice.SendRequestAsync();
            var tx = await _web3.Eth.TransactionManager.SendTransactionAsync(
                new TransactionInput(
                    "",
                    Environment.GetEnvironmentVariable("Ether:SmartContractAddr"),
                    Environment.GetEnvironmentVariable("Ether:AppAddress"),
                    new HexBigInteger("0x17318"),
                    new HexBigInteger(price.Value * 2),
                    amount
                ));
            return tx;
        }
    }
}

public enum ExchangeOperationStatus
{
    Skip = 0,
    Ok = 1,
    Failed = 2
}
