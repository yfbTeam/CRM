using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FEUtility
{
    /// <summary>
    /// 密码生成
    /// </summary>
    public class Key
    {
        /// <summary>
        /// 获取密码
        /// </summary>
        /// <param name="dt">当前时间</param>
        /// <param name="rely">生成密码依赖项</param>
        /// <returns></returns>
        public string GetKey()
        {
            string strAll = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //定义一个结果
            string result = "";
            //实例化Random对象
            Random random = new Random();
            //使用for循环得到6为字符
            for (int i = 0; i < 25; i++)
            {
                //返回一个小于62的int类型的随机数
                int rd = random.Next(36);
                //随机从指定的位置开始获取一个字符
                string oneChar = strAll.Substring(rd, 1);
                //循环加到6为
                result += oneChar;
            }
            return result;
        }
    }
}
