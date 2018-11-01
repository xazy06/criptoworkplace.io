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
                aes.KeySize = 256;
                aes.Key = _key;
                aes.IV = _iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }
                    encrypted = msEncrypt.ToArray();
                }                
            }
            return BitConverter.GetBytes(data.Length).Concat(encrypted).ToArray();
        }

        public byte[] Decrypt(byte[] data)
        {
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
