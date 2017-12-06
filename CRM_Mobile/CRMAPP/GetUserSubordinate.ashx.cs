using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CRMAPP
{
    /// <summary>
    /// GetUserSubordinate 的摘要说明
    /// </summary>
    public class GetUserSubordinate : IHttpHandler
    {

        string userCenter = System.Configuration.ConfigurationManager.AppSettings["userCenter"];
        public void ProcessRequest(HttpContext context)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(userCenter + "&ispage=false&Name=张功全&Phone=13911665621");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 10000;
            request.AllowAutoRedirect = false;
            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                    context.Response.Write(responseStr);
                }
            }
            catch (Exception ex)
            {
               LogManage.WriteLog(this.GetType(), ex.Message);
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}