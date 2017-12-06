using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FEUtility
{
    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncodeHelper
    {
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string security = HashEncoding(GetRandomValue());
            return security;
        }

        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            var seed = new Random();
            string randomVaule = seed.Next(1, int.MaxValue).ToString();
            return randomVaule;
        }

        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="security"></param>
        /// <returns></returns>
        public static string HashEncoding(string security)
        {
            var code = new UnicodeEncoding();
            byte[] message = code.GetBytes(security);
            var arithmetic = new SHA512Managed();
            byte[] value = arithmetic.ComputeHash(message);
            return value.Aggregate("", (current, o) => current + ((int) o + "O"));
        }
    }
}