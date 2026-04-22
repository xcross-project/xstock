using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xstock.lib.comm
{
    internal class common
    {
        public static long datatimetounixtimestamp(DateTime datetime)
            => (datetime.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(
                new System.DateTime(0x07B2, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00)).Ticks) / 0x2710;

        public static string md5crypto(string proclaimed)
            => Regex.Replace(
                BitConverter.ToString(
                    MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(proclaimed))),
                "\\-", string.Empty, RegexOptions.None).ToLower();


        public static string hmacksha256(string data, byte[] key)
        {
            string __result = null;
            using (var __hmacsha256 = new HMACSHA256(key))
                __result = Hex.ToHexString(__hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(data)));
            return __result;
        }

        public static byte[] stringtohex(string hexstring)
        {
            byte[] __result = new byte[hexstring.Length / 0x02];
            for (var i = 0x00; i < __result.Length; i += 0x02)
                __result[i / 0x02] = Convert.ToByte(hexstring.Substring(i, 0x02), 0x10);
            return __result;
        }
    }
}
