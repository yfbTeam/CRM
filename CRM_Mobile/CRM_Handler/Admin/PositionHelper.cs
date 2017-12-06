using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Handler.Admin
{
    class PositionHelper
    {
        //百度地图Api  Ak
        public const string BaiduAk = "9TxmFS8X1EXcUGZkqsDM4GKuayamwkbr";

        /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.ak  1.经度  2.纬度
        /// </summary>
        private const string BaiduGeoCoding_ApiUrl = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=json&pois=0";

        /// <summary>
        /// /// <summary>
        /// 经纬度  逆地理编码 Url  需要Format 0.经度  1.纬度 
        /// </summary>
        /// </summary>
        public static string Baidu_GeoCoding_ApiUrl
        {
            get
            {
                return string.Format(BaiduGeoCoding_ApiUrl, BaiduAk, "{0}", "{1}");
            }
        }

        /// <summary>
        /// 根据经纬度  获取 地址信息
        /// </summary>
        /// <param name="lat">经度</param>
        /// <param name="lng">纬度</param>
        /// <returns></returns>
        public static BaiDuGeoCoding GeoCoder(string lat, string lng)
        {
            string url = string.Format(Baidu_GeoCoding_ApiUrl, lat, lng);
            var model = HttpClientHelper.GetResponse<BaiDuGeoCoding>(url);
            return model;
        }


        public class HttpClientHelper
        {
            /// <summary>
            /// GET请求
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="url"></param>
            /// <returns></returns>
            public static T GetResponse<T>(string url) where T : class,new()
            {
                string returnValue = string.Empty;
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "GET";
                webReq.ContentType = "application/json";
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        returnValue = streamReader.ReadToEnd();
                        T result = default(T);
                        result = JsonConvert.DeserializeObject<T>(returnValue);
                        return result;
                    }
                }
            }
        }

        public class BaiDuGeoCoding
        {
            public int Status { get; set; }
            public Result Result { get; set; }
        }

        public class Result
        {
            public Location Location { get; set; }

            public string Formatted_Address { get; set; }

            public string Business { get; set; }

            public AddressComponent AddressComponent { get; set; }

            public string CityCode { get; set; }
        }

        public class AddressComponent
        {
            /// <summary>
            /// 省份
            /// </summary>
            public string Province { get; set; }
            /// <summary>
            /// 城市名
            /// </summary>
            public string City { get; set; }

            /// <summary>
            /// 区县名
            /// </summary>
            public string District { get; set; }

            /// <summary>
            /// 街道名
            /// </summary>
            public string Street { get; set; }

            public string Street_number { get; set; }

        }

        public class Location
        {
            public string Lng { get; set; }
            public string Lat { get; set; }
        }
    }
}
