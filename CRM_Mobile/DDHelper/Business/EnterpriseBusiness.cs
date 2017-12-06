using CRM_Common;
using DDHelper.Common;
using DDHelper.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDHelper.Business
{
    /// <summary>
    /// 内部逻辑
    /// </summary>
    public class EnterpriseBusiness
    {
        #region 字段

        /// <summary>
        /// 获取token
        /// </summary>
        static string gettoken_Uri = "https://oapi.dingtalk.com/gettoken?corpid=";

        /// <summary>
        /// 获取用户信息
        /// </summary>
        static string getuserinfo_Uri = "https://oapi.dingtalk.com/user/getuserinfo?access_token=";

        /// <summary>
        /// 获取部门信息
        /// </summary>
        static string getDepartment_Uri = "https://oapi.dingtalk.com/department/get?access_token=";

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        static string getUser_Uri = "https://oapi.dingtalk.com/user/get?access_token=";

        /// <summary>
        /// 获取js票据
        /// </summary>
        static string get_jsapi_ticket_Uri = "https://oapi.dingtalk.com/get_jsapi_ticket?access_token=";

        #endregion

        #region 构造函数

        public EnterpriseBusiness()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #endregion

        #region 拿到企业令牌

        /// <summary>
        /// 拿到企业令牌
        /// </summary>
        /// <param name="CorpId"></param>
        /// <param name="CorpSecret"></param>
        /// <returns></returns>
        public static TokenModel GetToken(string CorpId, string CorpSecret)
        {
            TokenModel tokenModel = null;
            try
            {
                string tagUrl = gettoken_Uri+ CorpId + "&corpsecret=" + CorpSecret;
                string result = HttpHelper.Get(tagUrl);

                tokenModel = JsonConvert.DeserializeObject<TokenModel>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return tokenModel;
        }

        #endregion

        #region 拿到当前登录的用户

        /// <summary>
        /// 拿到当前登录的用户
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static UserModel GetCurrentUser(string access_token, string code)
        {
            UserModel userModel = null;
            try
            {
                string tagUrl = getuserinfo_Uri + access_token + "&code=" + code;
                string result = HttpHelper.Get(tagUrl);

                userModel = JsonConvert.DeserializeObject<UserModel>(result);

                if(userModel == null)
                {
                    LogHelper.Info("未获取到用户信息");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return userModel;

        }

        #endregion

        #region 拿到当前登录的用户的部门信息

        /// <summary>
        /// 拿到当前登录的用户的部门信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DepartmentInfo GetAparentMent(string access_token, string id)
        {
            DepartmentInfo departmentInfo = null;
            try
            {
                string tagUrl = getDepartment_Uri + access_token + "&id=" + id;
                string result = HttpHelper.Get(tagUrl);

                departmentInfo = JsonConvert.DeserializeObject<DepartmentInfo>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return departmentInfo;
        }

        #endregion

        #region 拿到指定id的用户的详细信息

        /// <summary>
        /// 拿到指定id的用户的详细信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string access_token, string userID)
        {
            UserInfo userModel = null;
            try
            {
                string tagUrl = getUser_Uri + access_token + "&userid=" + userID;
                string result = HttpHelper.Get(tagUrl);

                userModel = JsonConvert.DeserializeObject<UserInfo>(result);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return userModel;
        }

        #endregion

        #region 拿到指定id的用户的详细信息

        /// <summary>
        /// 拿到指定id的用户的详细信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetUserInfoByString(string access_token, string userID)
        {
            string result = null;
            try
            {
                string tagUrl = getUser_Uri + access_token + "&userid=" + userID;
                result = HttpHelper.Get(tagUrl);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 拿到Tickets

        /// <summary>
        /// 拿到Tickets
        /// </summary>
        /// <param name="CorpId"></param>
        /// <param name="CorpSecret"></param>
        /// <returns></returns>
        public static string GetTickets(string access_token)
        {
            string ticket_str = null;
            try
            {
                string url = null;
                url = string.Format(get_jsapi_ticket_Uri +"{0}&type=jsapi", access_token);
                string result = HttpHelper.Get(url);
                JsApiTicket jsApiTicket = JsonConvert.DeserializeObject<JsApiTicket>(result);
                ticket_str = jsApiTicket.ticket;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ticket_str;
        }

        #endregion

    }
}
