using CRM_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DDHelper.Common
{
    public static class Helper
    {

        #region 字段

        /// <summary>
        /// 文本组合（用于获取随机号）
        /// </summary>
        static string textArray = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 默认时间字符串
        /// </summary>
        static string defaultTimer = "1970-01-01 00:00:00";

        #endregion

        #region 返回一个八位的随机号，用于签名

        /// <summary>
        /// 返回一个八位的随机号，用于签名
        /// </summary>
        /// <returns>八位的随机号</returns>
        public static string randNonce()
        {
            var result = "";

            try
            {
                var random = new Random((int)DateTime.Now.Ticks);
                for (var i = 0; i < 8; i++)
                {
                    result = result + textArray.Substring(random.Next() % textArray.Length, 1);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 时间戳的随机数

        /// <summary>
        /// 时间戳的随机数
        /// </summary>
        /// <returns></returns>
        public static string timeStamp()
        {
            string timeStr = null;
            try
            {
                DateTime dt1 = Convert.ToDateTime(defaultTimer);
                TimeSpan ts = DateTime.Now - dt1;
                timeStr = Math.Ceiling(ts.TotalSeconds).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return timeStr;
        }

        #endregion

        #region state 随机数

        /// <summary>
        /// state 随机数
        /// </summary>
        /// <returns></returns>
        public static string state()
        {
            string randomStr = null;
            try
            {
                Random ran = new Random();
                int RandKey = ran.Next(100, 999);
                randomStr = RandKey.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return randomStr;
        }

        #endregion
    }
}
