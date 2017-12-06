using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEUtility
{
    /// <summary>
    /// 异常输出类
    /// </summary>
    public class LogService
    {
        /// <summary>
        /// 日志存储路径
        /// </summary>
        public static string dir = HttpContext.Current.Server.MapPath("~/Log/");


        /// <summary>
        /// //写错误日志
        /// </summary>
        /// <param name="er"></param>
        public static void WriteErrorLog(string er)
        {
            try
            {
                if (!FileHelper.IsExistDirectory(dir))
                {
                    FileHelper.CreateDirectory(dir);
                }
                string time = DateTime.Now.Date.ToString("yyyy-MM-dd");
                using (StreamWriter sw = new StreamWriter(dir + time + ".err.txt", true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine("运行时错误出现时间：" + DateTime.Now.ToString());
                    sw.WriteLine("错误原因：" + er);
                    sw.WriteLine("\n");
                    sw.Close();
                }
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// //写运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void WriteLog(string log)
        {
            try
            {
                string time = DateTime.Now.Date.ToString("yyyy-MM-dd");
                using (StreamWriter sw = new StreamWriter(dir + time + ".txt", true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(System.DateTime.Now.ToString() + "：" + log);
                    sw.WriteLine("\n");
                    sw.Close();
                }
            }
            catch
            {
                //
            }
        }
    }
}
