using Google.Apis.Drive.v3;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using Newtonsoft.Json;
using pre_ico_web_site.Models;
using System;
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
        public string Address { get { return _saleContract.Address; } }

        public TokenSaleContract(
            Web3 web3,
            IOptions<EthSettings> options,
            DriveService drive,
            IOptions<GoogleDriveSettings> gdriveOptions,
            ILogger<TokenSaleContract> logger)
        {
            _logger = logger;
            _settings = options.Value;
            _web3 = web3;
            //string contentRootPath = hostingEnvironment.WebRootPath;
            var contractFile = DownloadFileData(drive, gdriveOptions.Value.SaleContractFileName);
            _saleContract = LoadContractFromMetadata(web3, _settings.Network.ToString(), contractFile);
            contractFile = DownloadFileData(drive, gdriveOptions.Value.TokenContractFileName);
            _tokenContract = LoadContractFromMetadata(web3, _settings.Network.ToString(), contractFile);
        }

        private string DownloadFileData(DriveService drive, string fileName)
        {
            var list = drive.Files.List();
            list.Q = $"name = '{fileName}'";
            var searched = list.Execute();
            if (searched.Files.Count == 0)
            {
                throw new ArgumentNullException($"File {fileName} not found in google drive");
            }
            var getRequest = drive.Files.Get(searched.Files.First().Id);
            using (Stream data = new MemoryStream())
            {
                getRequest.Download(data);
                data.Seek(0, SeekOrigin.Begin);
                using (StreamReader r = new StreamReader(data))
                {
                    return r.ReadToEnd();
                }
            }
        }

        private static Contract LoadContractFromMetadata(Web3 web3, string netId, string json)
        {
            //var JSON = System.IO.File.ReadAllText(contractFile);
            dynamic jObject = JsonConvert.DeserializeObject<dynamic>(json);
            var abi = jObject.abi.ToString();
            var contractAddress = jObject.networks[netId].address.ToString();
            return web3.Eth.GetContract(abi, contractAddress);
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

        public Task AddAddressToWhitelistAsync(string addr)
        {
            var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

            return _saleContract.GetFunction("addAddressToWhitelist")
                .SendTransactionAndWaitForReceiptAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), null, addr);
        }

        public async Task<FixRateModel> SetRateForTransactionAsync(int rate, string buyer, BigInteger amount)
        {
            var currentGasPrice = _settings.GasPrice * UnitConversion.Convert.GetEthUnitValue(UnitConversion.EthUnit.Gwei);

            var hash = await _saleContract.GetFunction("setRateForTransaction")
                .SendTransactionAsync(_settings.AppAddress, new HexBigInteger(_settings.GasLimit), new HexBigInteger(currentGasPrice), new HexBigInteger(0), rate, buyer, amount);

            _logger.LogCritical("Transaction hash: {0}", hash);

            await WaitReciept(hash);
            _logger.LogCritical("done");
            return await _saleContract.GetFunction("fixRate").CallDeserializingToObjectAsync<FixRateModel>(buyer);
        }

        public Task<BigInteger> GetRefundAmountAsync(string ethAddress)
        {
            return _tokenContract.GetFunction("payments").CallAsync<BigInteger>(ethAddress);
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
