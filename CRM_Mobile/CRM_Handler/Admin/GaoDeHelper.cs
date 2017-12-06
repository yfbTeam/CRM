using CRM_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CRM_Handler.Admin
{
    public class GaoDeHelper
    {
        public static Position_Model GetPosition(string Url, string type)
        {
            Position_Model Position_Model = new Position_Model();
            try
            {
                string responseStr = GetUrltoHtml(Url, type);
                Position_Model = JsonConvert.DeserializeObject<Position_Model>(responseStr); ;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Position_Model;
        }


        #region 通过地址获取经纬度

        public static string GetUrltoHtml(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(ex);
            }
            return "";
        }

        #endregion      
    }

    public class Geocodes
    {
        /// <summary>
        /// 北京市大兴区永源路|15号
        /// </summary>
        public string formatted_address { get; set; }

        /// <summary>
        /// 116.088333,39.971276
        /// </summary>
        public string location { get; set; }
    }

    public class Position_Model
    {
        /// <summary>
        /// Geocodes
        /// </summary>
        public List<Geocodes> geocodes { get; set; }

      
    }

  
}