using CRM_Common;
using DDHelper.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;



namespace DDHelper.Business
{
    /// <summary>
    /// jsapi跳转使用（）
    /// </summary>
    public class EnterPrise_MB
    {
        public static EnterPrise_MB _EnterPrise_MB = null;

        #region 字段

        /// <summary>
        /// 微应用ID
        /// </summary>
        public string appId = string.Empty;
        /// <summary>
        /// 企业ID
        /// </summary>
        public string corpId = string.Empty;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp = string.Empty;
        public string nonceStr = string.Empty;
        public string signature = string.Empty;

        ///// <summary>
        ///// 令牌
        ///// </summary>
        //public static string accessToken;
        /// <summary>
        /// js票据
        /// </summary>
        public static string jsApiTicket;

        /// <summary>
        /// 配置列表
        /// </summary>
        public ArrayList config_list = new ArrayList() {
            "runtime.info", 
            "biz.contact.choose",
            "device.notification.confirm",
            "device.notification.alert",
            "device.notification.prompt", 
            "biz.ding.post",
            "biz.util.openLink"
        };



        ///// <summary>
        ///// 配置列表
        ///// </summary>
        //public ArrayList config_list = new ArrayList() {
        //     "runtime.info",
        //     "device.notification.alert",
        //     "device.notification.confirm",
        //     "device.base.getUUID",
        //     "device.notification.modal",
        //     "device.geolocation.get",
        //      "runtime.permission.requestAuthCode",
        //      "biz.user.get",
        //      "biz.contact.choose",
        //      "device.notification.prompt",
        //      "biz.ding.post",
        //      "biz.util.openLink",
        //      "biz.navigation.setMenu",
        //       "device.base.getInterface",
        //       "device.nfc.nfcRead",
        //       "device.launcher.checkInstalledApps",
        //       "biz.util.uploadImage"
        // };

        ///// <summary>
        ///// 配置列表
        ///// </summary>
        //public ArrayList config_list = new ArrayList() {
        //    "runtime.info",
        //    "device.notification.alert",
        //    "device.notification.confirm",
        //    "device.base.getUUID",
        //    "device.notification.modal",
        //};


        public string ServerUri = Config.ServerUri;



        #endregion

        #region 获取配置信息

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="Request">当前页面</param>
        public void GetConfig(HttpRequest Request)
        {
            try
            {
                _EnterPrise_MB = this;
                appId = Config.EAgentID;
                corpId = Config.ECorpId;
                string corpSecret = Config.ECorpSecret;
                string url = Request.Url.AbsoluteUri;

                nonceStr = Helper.randNonce();
                timestamp = Helper.timeStamp();
                //string url = Request.Url.ToString();               
                //if (Config.TokenModel == null)
                //{
                //    //这里重新实现
                //    Config.TokenModel = EnterpriseBusiness.GetToken(corpId, corpSecret);
                //}
                //if (string.IsNullOrEmpty(jsApiTicket))
                //{
                if (Config.TokenModel!=null &&!string.IsNullOrEmpty(Config.TokenModel.access_token))
                {
                    jsApiTicket = EnterpriseBusiness.GetTickets(Config.TokenModel.access_token);
                }
                //}

                string jsApiTicket_Message = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsApiTicket, nonceStr, timestamp, url);

                //string jsApiTicket_Message = string.Format("nonce:{0},timestamp:{1},url:{2},ticket:{3}", nonceStr, timestamp, url, jsApiTicket);

                signature = FormsAuthentication.HashPasswordForStoringInConfigFile(jsApiTicket_Message, "SHA1").ToLower();

                //GenSigurate(nonceStr, timestamp, jsApiTicket, url, ref signature);
                // 这里参数的顺序要按照 key 值 ASCII 码升序排序   
                //string rawstring = "{Keys.jsapi_ticket}=" +jsApiTicket  
                //                 + "&{Keys.noncestr}=" + nonceStr  
                //                 + "&{Keys.timestamp}=" + timestamp  
                //                 + "&{Keys.url}=" + url;  
                // signature = SignPackageHelper.Sha1Hex(rawstring).ToLower();  

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);             
            }
        }

        public void GetConfig(HttpRequest Request, string pageurl)
        {
            try
            {
                _EnterPrise_MB = this;
                appId = Config.EAgentID;
                corpId = Config.ECorpId;
                string corpSecret = Config.ECorpSecret;
                nonceStr = Helper.randNonce();
                timestamp = Helper.timeStamp();
                string url = pageurl;

                //LogManage.WriteLog(typeof(EnterPrise_MB), appId+"&"+ corpId);

                if (Config.TokenModel == null)
                {
                    //这里重新实现
                    Config.TokenModel = EnterpriseBusiness.GetToken(corpId, corpSecret);
                }
                if (Config.TokenModel != null)
                {
                    jsApiTicket = EnterpriseBusiness.GetTickets(Config.TokenModel.access_token);
                    //}

                    string jsApiTicket_Message = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsApiTicket, nonceStr, timestamp, url);

                    //string jsApiTicket_Message = string.Format("nonce:{0},timestamp:{1},url:{2},ticket:{3}", nonceStr, timestamp, url, jsApiTicket);

                    signature = FormsAuthentication.HashPasswordForStoringInConfigFile(jsApiTicket_Message, "SHA1").ToLower();
                }
                //GenSigurate(nonceStr, timestamp, jsApiTicket, url, ref signature);
                // 这里参数的顺序要按照 key 值 ASCII 码升序排序   
                //string rawstring = "{Keys.jsapi_ticket}=" +jsApiTicket  
                //                 + "&{Keys.noncestr}=" + nonceStr  
                //                 + "&{Keys.timestamp}=" + timestamp  
                //                 + "&{Keys.url}=" + url;  
                // signature = SignPackageHelper.Sha1Hex(rawstring).ToLower();  

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion


        /// <summary>
        ///开发者在web页面使用钉钉容器提供的jsapi时，需要验证调用权限，并以参数signature标识合法性
        ///签名生成的规则：
        ///List keyArray = sort(noncestr, timestamp, jsapi_ticket, url);
        /// String str = assemble(keyArray);
        ///signature = sha1(str);
        /// </summary>
        /// <param name="noncestr">随机字符串，自己随便填写即可</param>
        /// <param name="sTimeStamp">当前时间戳，具体值为当前时间到1970年1月1号的秒数</param>
        /// <param name="jsapi_ticket">获取的jsapi_ticket</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分</param>
        /// <param name="signature">生成的签名</param>
        /// <returns>0 成功，2 失败</returns>
        public static int GenSigurate(string noncestr, string sTimeStamp, string jsapi_ticket, string url, ref string signature)
        {


            //例如：
            //noncestr = Zn4zmLFKD0wzilzM
            //jsapi_ticket = mS5k98fdkdgDKxkXGEs8LORVREiweeWETE40P37wkidkfksDSKDJFD5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcKIDU8l
            //timestamp = 1414588745
            //url = http://open.dingtalk.com

            //步骤1.sort()含义为对所有待签名参数按照字段名的ASCII 码从小到大排序（字典序）
            //注意，此处是是按照【字段名】的ASCII字典序，而不是参数值的字典序（这个细节折磨我很久了)
            //0:jsapi_ticket 1:noncestr 2:timestamp 3:url;

            //步骤2.assemble()含义为根据步骤1中获的参数字段的顺序，使用URL键值对的格式（即key1 = value1 & key2 = value2…）拼接成字符串
            //string assemble = "jsapi_ticket=3fOo5UfWhmvRKnRGMmm6cWwmIxDMCnniyVYL2fqcz1I4GNU4054IOlif0dZjDaXUScEjoOnJWOVrdwTCkYrwSl&noncestr=CUMT1987wlrrlw&timestamp=1461565921&url=https://jackwangcumt.github.io/home.html";
            string assemble = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapi_ticket, noncestr, sTimeStamp, url);
            //步骤2.sha1()的含义为对在步骤2拼接好的字符串进行sha1加密。
            SHA1 sha;
            ASCIIEncoding enc;
            string hash = "";
            try
            {
                sha = new SHA1CryptoServiceProvider();
                enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(assemble);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return 2;
            }
            signature = hash;
            return 0;

        }

        /// <summary>
        /// 获取时间戳timestamp（当前时间戳，具体值为当前时间到1970年1月1号的秒数）
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 字典排序
        /// </summary>
        public class DictionarySort : System.Collections.IComparer
        {
            public int Compare(object oLeft, object oRight)
            {
                string sLeft = oLeft as string;
                string sRight = oRight as string;
                int iLeftLength = sLeft.Length;
                int iRightLength = sRight.Length;
                int index = 0;
                while (index < iLeftLength && index < iRightLength)
                {
                    if (sLeft[index] < sRight[index])
                        return -1;
                    else if (sLeft[index] > sRight[index])
                        return 1;
                    else
                        index++;
                }
                return iLeftLength - iRightLength;

            }
        }
    }



}
