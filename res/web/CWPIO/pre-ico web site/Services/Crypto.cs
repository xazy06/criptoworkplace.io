using Microsoft.Extensions.Options;
using pre_ico_web_site.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace pre_ico_web_site.Services
{
    public class Crypto
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;
        public Crypto(IOptions<EthSettings> settings)
        {
            _key = settings.Value.Key.StringToByteArray();
            _iv = settings.Value.IV.StringToByteArray();
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return BitConverter.GetBytes(data.Length).Concat(encrypted).ToArray();
        }

        public byte[] Decrypt(byte[] data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(data))
                {
                    byte[] decrypted = new byte[4];
                    msDecrypt.Read(decrypted, 0, 4);

                    var count = BitConverter.ToInt32(decrypted, 0);
                    decrypted = new byte[count];

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
