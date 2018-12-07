using ExchangerMonitor.Settings;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace ExchangerMonitor.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        public CryptoService(IOptions<EthSettings> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _key = settings.Value.Key.StringToByteArray();
            _iv = settings.Value.IV.StringToByteArray();
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = _key;
                aes.IV = _iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // Create the streams used for decryption.

                byte[] decrypted = data.Take(4).ToArray();
                var count = BitConverter.ToInt32(decrypted, 0);
                decrypted = new byte[count];
                using (MemoryStream msDecrypt = new MemoryStream(data.Skip(4).ToArray()))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(decrypted, 0, count);
                    }

                    return decrypted;
                }
            }
        }
    }
}
