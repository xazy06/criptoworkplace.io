using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangerMonitor
{
    public static class StringExtensions
    {
        public static byte[] StringToByteArray(this string hex)
        {
            hex = hex.Replace("0x", "");
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string ByteArrayToString(this byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }
    }
}
