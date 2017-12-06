////public class Sign
////{
////    const string API_VERSION = "v2";
////    const string SIGNATURE_METHOD = "HMAC-SHA1";
////    const string SIGNATURE_VERSION = "1.0";
////    const string HTTP_METHOD = "GET";
////    const string ACCESS_KEY_ID = "1";
////    const string ACCESS_KEY_SECRET = "107";
////    const string SEARCH_BASE_URL = "http://opensearch-cn-hangzhou.aliyuncs.com/search";
////    private string _index_name = string.Empty;
////    public string GetSin()
////    {          
////        string query="ftype:'1' AND userid:'2612'&&config=start:0,hit:50&&sort=+flowtime";
////        //第一步、创建请求参数
////        SortedDictionary<String, String> parameters = new SortedDictionary<string, string>(StringComparer.Ordinal); //区分大小写排序，这个问题开始卡我半天
////        parameters.Add("Version", API_VERSION);
////        parameters.Add("AccessKeyId", ACCESS_KEY_ID);
////        parameters.Add("SignatureMethod", SIGNATURE_METHOD);
////        parameters.Add("Timestamp", DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
////        parameters.Add("SignatureVersion", SIGNATURE_VERSION);
////        parameters.Add("SignatureNonce", _makeNonceAliyun());
////        parameters.Add("query", query);
////        parameters.Add("index_name", _index_name);

////        string q = HttpBuildQuery(parameters);
////        string Signature = _makeSignAliyun(parameters);

////        return Signature;
////    }

////    /// <summary>
////    /// 获取13位unix timestamp
////    /// </summary>
////    /// <returns></returns>
////    private long getUnixTimeStamp()
////    {
////        TimeSpan span = DateTime.UtcNow - (new DateTime(1970, 1, 1));
////        return Convert.ToInt64(span.TotalMilliseconds);
////    }

////    /// <summary>
////    /// 请求唯一随机数，用于防止网络重放攻击。建议使用13位时间毫秒数+4位随机数。
////    /// </summary>
////    /// <returns></returns>
////    private string _makeNonceAliyun()
////    {
////        long stamp = getUnixTimeStamp();
////        Random ra = new Random();
////        int rand = ra.Next(1000, 9999);
////        StringBuilder sb = new StringBuilder();
////        sb.Append(stamp);
////        sb.Append(rand);
////        return sb.ToString();
////    }

////    /// <summary>
////    ///
////    /// 根据参数创建签名信息(阿里云的算法)。
////    ///
////    /// @param array 参数数组。
////    /// @return string 签名字符串。
////    ///
////    /// </summary>
////    /// <param name="parameter">参数数组</param>
////    /// <returns>签名字符串（已经UrlEncode）</returns>
////    private string _makeSignAliyun(SortedDictionary<String, String> parameters)
////    {
////        //第一步、使用请求参数构造规范化的请求字符串
////        string q = HttpBuildQuery(parameters);
////        //第二步：使用上一步构造的规范化字符串按照下面的规则构造用于计算签名的字符串。
////        string StringToSign = HTTP_METHOD + "&%2F&" + UrlEncodeX(q);
////        //第三步、按照RFC2104的定义，使用上面的用于签名的字符串计算签名HMAC值。
////        string Signature = HmacSha1Sign(StringToSign, ACCESS_KEY_SECRET + "&");
////        Signature = UrlEncodeX(Signature);
////        return Signature;
////    }

////    private string HttpBuildQuery(SortedDictionary<String, String> parameters)
////    {
////        StringBuilder sb = new StringBuilder();
////        foreach (KeyValuePair<String, String> kvp in parameters) //系统参数
////        {
////            sb.Append("&");
////            sb.Append(UrlEncodeX(kvp.Key));
////            sb.Append("=");
////            sb.Append(UrlEncodeX(kvp.Value));
////        }
////        return sb.ToString().Substring(1);
////    }

////    /// <summary>
////    /// 符合阿里云规定的URL编码（可以将%3A、%2F这样的大写，空格也可以直接转码成%20，好嗨皮）
////    /// </summary>
////    /// <param name="str"></param>
////    /// <returns></returns>
////    private string UrlEncodeX(string str)
////    {
////        string rt = Microsoft.JScript.GlobalObject.escape(str);
////        rt = rt.Replace("+", "%2B");    //js不编码“+”号
////        rt = rt.Replace("*", "%2A");    //js不编码“*”号
////        rt = rt.Replace("%7E", "~");    //js会编码“~”，但是阿里云要求“~”原滋原味不要被编码
////        rt = rt.Replace("%7e", "~");
////        return rt;
////    }

////    /// <summary>
////    /// HmacSha1算法
////    /// </summary>
////    /// <param name="text"></param>
////    /// <param name="key"></param>
////    /// <returns></returns>
////    private string HmacSha1Sign(string text, string key)
////    {
////        Encoding encode = Encoding.UTF8;
////        byte[] byteData = encode.GetBytes(text);
////        byte[] byteKey = encode.GetBytes(key);
////        HMACSHA1 hmac = new HMACSHA1(byteKey);
////        CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
////        cs.Write(byteData, 0, byteData.Length);
////        cs.Close();
////        return Convert.ToBase64String(hmac.Hash);
////    }
////}

////public class T2
////{
////    #region FetchSignPackage Function 
////    /// <summary>
////    /// 获取签名包
////    /// </summary>
////    /// <param name="url"></param>
////    /// <returns></returns>
////    public static SignPackage FetchSignPackage(String url, JSTicket jsticket)
////    {
////        int unixTimestamp = SignPackageHelper.ConvertToUnixTimeStamp(DateTime.Now);
////        string timestamp = Convert.ToString(unixTimestamp);
////        string nonceStr = SignPackageHelper.CreateNonceStr();
////        if (jsticket == null)
////        {
////            return null;
////        }

////        // 这里参数的顺序要按照 key 值 ASCII 码升序排序 
////        string rawstring = $"{Keys.jsapi_ticket}=" + jsticket.ticket
////                         + $"&{Keys.noncestr}=" + nonceStr
////                         + $"&{Keys.timestamp}=" + timestamp
////                         + $"&{Keys.url}=" + url;
////        string signature = SignPackageHelper.Sha1Hex(rawstring).ToLower();

////        var signPackage = new SignPackage()
////        {
////            agentId = ConfigHelper.FetchAgentID(),
////            corpId = ConfigHelper.FetchCorpID(),
////            timeStamp = timestamp,
////            nonceStr = nonceStr,
////            signature = signature,
////            url = url,
////            rawstring = rawstring,
////            jsticket = jsticket.ticket
////        };
////        return signPackage;
////    }

////    /// <summary>
////    /// 获取签名包
////    /// </summary>
////    /// <param name="url"></param>
////    /// <returns></returns>
////    public static SignPackage FetchSignPackage(String url)
////    {
////        int unixTimestamp = SignPackageHelper.ConvertToUnixTimeStamp(DateTime.Now);
////        string timestamp = Convert.ToString(unixTimestamp);
////        string nonceStr = SignPackageHelper.CreateNonceStr();
////        var jsticket = FetchJSTicket();
////        var signPackage = FetchSignPackage(url, jsticket);
////        return signPackage;
////    }
////    #endregion 
////}

/////// <summary>
/////// 签名包
/////// </summary>
////public class SignPackage
////{
////    public String agentId { get; set; }

////    public String corpId { get; set; }

////    public String timeStamp { get; set; }

////    public String nonceStr { get; set; }

////    public String signature { get; set; }

////    public String url { get; set; }

////    public String rawstring { get; set; }

////    public string jsticket { get; set; }
////}

//public class SignPackageHelper
//{
//    #region Sha1Hex
//    public static string Sha1Hex(string value)
//    {
//        SHA1 algorithm = SHA1.Create();
//        byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
//        string sh1 = "";
//        for (int i = 0; i < data.Length; i++)
//        {
//            sh1 += data[i].ToString("x2").ToUpperInvariant();
//        }
//        return sh1;
//    }
//    #endregion

//    #region CreateNonceStr
//    /// <summary>
//    /// 创建随机字符串
//    /// </summary>
//    /// <returns></returns>
//    public static string CreateNonceStr()
//    {
//        int length = 16;
//        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//        string str = "";
//        Random rad = new Random();
//        for (int i = 0; i < length; i++)
//        {
//            str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
//        }
//        return str;
//    }
//    #endregion

//    #region ConvertToUnixTimeStamp
//    /// <summary>  
//    /// 将DateTime时间格式转换为Unix时间戳格式  
//    /// </summary>  
//    /// <param name="time">时间</param>  
//    /// <returns>double</returns>  
//    public static int ConvertToUnixTimeStamp(DateTime time)
//    {
//        int intResult = 0;
//        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
//        intResult = Convert.ToInt32((time - startTime).TotalSeconds);
//        return intResult;
//    }
//    #endregion
//}