using CRM_Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DDHelper.Common
{
    public static class HttpHelper
    {
        #region 字段
        
        /// <summary>
        /// 用户代理信息
        /// </summary>
        static string userAgent_flg = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";

        /// <summary>
        /// 传送函数关键字
        /// </summary>
        static string post_flg = "POST";

        /// <summary>
        /// 传送类型关键字
        /// </summary>
        static string contentType = "ContentType";

        #endregion

        #region 通过GET方式获取页面的方法

        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public static string Get(string urlString)
        {
            string htmlString = string.Empty;
            try
            {
                //定义局部变量
                HttpWebRequest httpWebRequest = null;
                HttpWebResponse httpWebRespones = null;
                Stream stream = null;

                //请求页面
                try
                {
                    httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
                }
                //处理异常
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                httpWebRequest.UserAgent = userAgent_flg;
                //获取服务器的返回信息
                try
                {
                    httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                    stream = httpWebRespones.GetResponseStream();
                }
                //处理异常
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                //读取返回页面
                try
                {
                    htmlString = streamReader.ReadToEnd();
                }
                //处理异常
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                //释放资源返回结果
                streamReader.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return htmlString;
        }

        #endregion

        #region 后台post事件
        
        /// <summary>
        /// 后台post事件
        /// 
        /// 调用方法：
        /// string url = "https://oapi.dingtalk.com/department/create?access_token=" + tokenstring;
        /// string param = "{\"access_token\":\"" + tokenstring + "\",\"name\":\"研发二部\",\"parentid\":\"1\",\"order\":\"2\",\"createDeptGroup\":\"false\"}";
        /// string callback = PostMoths(url, param);
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            string strValue = "";
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = post_flg;
                request.ContentType = contentType;
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";

                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return strValue;
        }

        #endregion

    }
}
