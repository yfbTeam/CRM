using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FEUtility
{
    public class HttpHelper
    {
        private static string sdtUrl = ConfigHelper.GetConfigString("HttpService");
        public static string SDTUrl
        {
            get
            {
                sdtUrl = ConfigHelper.GetConfigString("HttpService");
                return sdtUrl;
            }
        }

        /// <summary>
        /// 通过Post方式访问url，并获取返回数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static string PostUrl(string relativeUrl, Hashtable ht, Hashtable htp)
        {
            string html = string.Empty;

            string url =  relativeUrl;
            try
            {
                ////构造WebRequest
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "post";
                //request.ContentType = "application/x-www-form-urlencoded";
                //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:19.0) Gecko/20100101 Firefox/19.0"; //模拟火狐
                //string param = CreateParam(ht);
                //byte[] postdatabyte = Encoding.UTF8.GetBytes(param);
                //request.ContentLength = postdatabyte.Length;
                //request.AllowAutoRedirect = false;

                //Stream stream;
                //stream = request.GetRequestStream();
                //stream.Write(postdatabyte, 0, postdatabyte.Length); //设置请求主体的内容
                //stream.Close();

                ////请求响应
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //Stream responseStream = response.GetResponseStream();
                //StreamReader sr = new StreamReader(responseStream);
                //html = sr.ReadToEnd();
                StringBuilder htmltext=new StringBuilder();
                string path = htp["purl"].ToString();
                using (StreamReader sr = new StreamReader(path))
                {
                    html = sr.ReadToEnd();
                    sr.Close();
                }
                if (htp.ContainsKey("url")) html = html.Replace("[WebPostUrl]", htp["url"].ToString());
                if (htp.ContainsKey("surl")) html = html.Replace("[WebServiceUrl]", htp["surl"].ToString());
                if (htp.ContainsKey("cid")) html = html.Replace("HUserIdCardValue", htp["cid"].ToString());
                if (htp.ContainsKey("uname")) html = html.Replace("HUserNameValue", htp["uname"].ToString());
                return html;
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        /// <summary>
        /// 生成参数字符串
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        private static string CreateParam(Hashtable ht)
        {
            if (ht == null) { return string.Empty; }

            string param = string.Empty;
            foreach (var key in ht.Keys)
            {
                if (ht[key] == null) { continue; }
                param += string.Format("&{0}={1}", key, ht[key].ToString());
            }

            if (param.Length > 0)
            {
                param = param.Substring(1);
            }
            return param;
        }

        


    }
}
