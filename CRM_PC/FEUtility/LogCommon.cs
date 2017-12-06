using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEUtility
{
    public class LogCommon
    {
        private static void writeLog(string path, string error1, string PageName)
        {
            //string path = @"c:\\UserCenter.txt";
            StringBuilder ErrMess = new StringBuilder();
            ErrMess.Append(@"WebPart：'" + PageName + "'  时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ErrMess.Append("错误信息：" + error1);
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(ErrMess);
                StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.GetEncoding("GB2312"));
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
            else
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码
                sw.WriteLine(ErrMess);
                sw.Close();
            }
        }
        /// <summary>
        /// 输出错误日志
        /// </summary>
        public void writeLogMessage(string error1, string PartName)
        {
            writeLog(@"c:\\ErrorMessage.txt", error1, PartName);
        }
     
    }
}
