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
    public partial class GetUserSubordinate1 : System.Web.UI.Page
    {
        string userCenter1 = System.Configuration.ConfigurationManager.AppSettings["userCenter1"];
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserName = Request["UserName"];
            string Phone = Request["Phone"];
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(userCenter1 + "&UserName="+ UserName + "&Phone="+ Phone + "");
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
               LogManage.WriteLog(this.GetType(), ex.Message);
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