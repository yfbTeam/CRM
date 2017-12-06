using CRM_Common;
using DDHelper.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;

namespace DDHelper.Business
{
    /// <summary>
    /// 外部调用
    /// </summary>
    public class EnterPrise_Server
    {
        #region 字段

        ///// <summary>
        ///// token
        ///// </summary>
        //static Entity.TokenModel tokenModel;

        /// <summary>
        /// 计时器（不应该每次免登都需要获取token,但是有效期是7200秒，所以每次过完这个时间段都需要重新获取一次token,timer的interval单位是毫秒）
        /// </summary>
        static Timer timer = null;

        /// <summary>
        /// 做一个缓存区域
        /// </summary>
        static Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();

        #endregion

        #region 获取用户信息

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string GetSelfInfo(HttpRequest Request)
        {
            //返回的用户信息
            
            string userInfo = null;
            try
            {
                string code = Request["code"];
                
                //是否强制获取，在这里默认为不强制获取
                bool isNeedForceGet = false;
                string strIsNeedForceGet = Request["isFoceGet"];
                if (!string.IsNullOrEmpty(strIsNeedForceGet))
                {
                    isNeedForceGet = Convert.ToBoolean(strIsNeedForceGet);
                }


                string mode = Request["mode"];

                //根据CropId与cropSecret去换取AccessToken
                //这里的AccessToken的主要含义是企业令牌，它的意思是说依靠这个令牌可以去拿取与企业相关的数据，
                //根据官方文档介绍这里的有效期是7200秒，
                if (Config.TokenModel == null)
                {
                    Config.TokenModel = EnterpriseBusiness.GetToken(Config.ECorpId, Config.ECorpSecret);
                }
                //暂时停止
                if (timer == null)
                {
                    timer = new Timer() { Interval = 6000 * 1000 };
                    timer.Elapsed += timer_Elapsed;
                    timer.Start();
                }
                string access_token = Config.TokenModel.access_token;

                /*
                 * 这里拿到企业令牌后，可以将其保存到数据库中，同时设定它的过期时间为当前时间+7200秒，             
                 * 每次使有令牌时判断当前时间是否已经超过了有效期，如果超过了有效期，请重新获取新的令牌
                 * 为了安全access_token在实际的开发过程当中不建议放到客户端，这个令牌一般禁止用户接触到，一般可放在服务器端的session里             
                 */
                //---------------利用access_token和code去换取当前用户
                UserModel userModel = EnterpriseBusiness.GetCurrentUser(access_token, code);
                //先从缓存池里进行获取
                if (dicUserInfo.ContainsKey(userModel.userid) && !isNeedForceGet)
                {
                    userInfo = dicUserInfo[userModel.userid];
                }
                else
                {
                    if (string.IsNullOrEmpty(mode))
                    {
                        mode = "pc";
                    }

                    //获取用户信息
                    userInfo = EnterpriseBusiness.GetUserInfoByString(access_token, userModel.userid);
                   
                    if (!string.IsNullOrEmpty(userInfo))
                    {
                        UserInfo userIn = JsonConvert.DeserializeObject<UserInfo>(userInfo);
                        if (userInfo != null && !string.IsNullOrEmpty(userIn.name))
                        {
                            //LogManage.WriteLog(typeof(EnterpriseBusiness), mode + "钉钉免登：" + userIn.name);

                            dicUserInfo.Add(userModel.userid, userInfo);
                        }
                        else
                        {
                            ErrorDealWith(mode);

                        }
                    }
                    else
                    {
                        ErrorDealWith(mode);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return userInfo;
        }

        /// <summary>
        /// 异常处理机制（重新刷新页面）
        /// </summary>
        static void ErrorDealWith(string mode)
        {
            try
            {
                ////未获取到用户信息,请在应用层进行刷新,刷新方式（PC端：  Response.Redirect("/OAuth.aspx"); 移动端：  Response.Redirect("/JsAPI.aspx");）
                //LogManage.WriteLog(typeof(EnterpriseBusiness), "未获取到用户信息,自动刷新页面重新获取--by" + mode);

                //if (mode == "mobile")
                //{
                //    HttpContext.Current.Response.Redirect("JsAPI.aspx");
                //}
                //else if (mode == "pc")
                //{
                //    HttpContext.Current.Response.Redirect("OAuth.aspx");
                //}               
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region 定时刷新token

        /// <summary>
        /// 定时刷新token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                //获取token
                Config.TokenModel = null;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);               
            }
        }

        #endregion

        #region 获取部门信息（任意一个操作之前都得先进行一次免登）

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="Request">页面请求者</param>
        /// <param name="ID">部门ID</param>
        /// <returns>返回部门信息</returns>
        public static DepartmentInfo GetSelfDepartment(HttpRequest Request, string ID)
        {
            //string code = Request["code"];
            //部门信息
            DepartmentInfo departmentInfo = null;
            try
            {
                //根据CropId与cropSecret去换取AccessToken
                //这里的AccessToken的主要含义是企业令牌，它的意思是说依靠这个令牌可以去拿取与企业相关的数据，
                //根据官方文档介绍这里的有效期是7200秒，
                if (Config.TokenModel == null)
                {
                    //获取token
                    Config.TokenModel = EnterpriseBusiness.GetToken(Config.ECorpId, Config.ECorpSecret);
                }
                var access_token = Config.TokenModel.access_token;
                //---------------利用access_token和code去换取当前用户
                departmentInfo = EnterpriseBusiness.GetAparentMent(access_token, ID);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return departmentInfo;
        }

        #endregion
    }
}
