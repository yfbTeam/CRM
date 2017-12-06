using CRM_Common;
using DDHelper.Business;
using DDHelper.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRMAPP
{
    public partial class Login : System.Web.UI.Page
    {
        string userCenter = System.Configuration.ConfigurationManager.AppSettings["userCenter"];
        protected void Page_Load(object sender, EventArgs e)
        {
            var userInfo = EnterPrise_Server.GetSelfInfo(this.Request);
            if (userInfo != null)
            {
                UserInfo userIn = JsonConvert.DeserializeObject<UserInfo>(userInfo);
               
                ////测试页面
                //string stt = "当前用户名为：" + userIn.name + "   " + "手机号为：" + userIn.mobile + "   "
                //    + "邮箱地址：" + userIn.email + "   " + "部门ID：";
                //this.txt.Value = stt;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(userCenter + "&ispage=false&Name=" + userIn.name + "&Phone=" + userIn.mobile + "&Status=1" );
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
                        Response.Write(responseStr);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                finally
                {
                    request = null;
                    requestStream = null;
                    response = null;
                }
            }

        }
    }
}