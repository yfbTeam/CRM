using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace FEUtility
{
    /// <summary>
    /// Des加密解密
    /// </summary>
    public class DesEncryptHelper
    {

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">要加密字符串</param>
        /// <returns>加密后</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, "cnnho");
        }

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="text">要加密字符串</param> 
        /// <param name="sKey">key值</param> 
        /// <returns>加密后</returns> 
        public static string Encrypt(string text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            des.Key =
                // ReSharper disable PossibleNullReferenceException
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(
                // ReSharper restore PossibleNullReferenceException
                    0, 8));
            des.IV =
                // ReSharper disable PossibleNullReferenceException
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(
                // ReSharper restore PossibleNullReferenceException
                    0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="text">要加密字符串</param>
        /// <param name="key">key值</param>
        /// <param name="iv">字符集</param>
        /// <returns>加密后</returns>
        public static string Encrypt(string text, out string key, out string iv)
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            des.GenerateIV();
            des.GenerateKey();
            iv = new UnicodeEncoding().GetString(des.IV);
            key = new UnicodeEncoding().GetString(des.Key);
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            var ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text">要解密字符串</param>
        /// <returns>解密后</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, "cnnho");
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="text">要解密字符串</param> 
        /// <param name="sKey">key值</param> 
        /// <returns>解密后</returns> 
        public static string Decrypt(string text, string sKey)
        {
            var des = new DESCryptoServiceProvider();
            int len = text.Length / 2;
            var inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                int i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key =
                // ReSharper disable PossibleNullReferenceException
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(
                // ReSharper restore PossibleNullReferenceException
                    0, 8));
            des.IV =
                // ReSharper disable PossibleNullReferenceException
                Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(
                // ReSharper restore PossibleNullReferenceException
                    0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="text">要解密字符串</param>
        /// <param name="key">key值</param>
        /// <param name="iv">字符集</param>
        /// <returns>解密后</returns>
        public static string Decrypt(string text, string key, string iv)
        {
            byte[] keyBuffer = new UnicodeEncoding().GetBytes(key);
            byte[] ivBuffer = new UnicodeEncoding().GetBytes(iv);
            var des = new DESCryptoServiceProvider();
            int len = text.Length / 2;
            var inputByteArray = new byte[len];
            int x;
            for (x = 0; x < len; x++)
            {
                int i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = keyBuffer;
            des.IV = ivBuffer;
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}