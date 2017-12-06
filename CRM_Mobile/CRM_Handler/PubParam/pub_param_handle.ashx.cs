using CRM_BLL;
using CRM_Common;
using CRM_Model;
using CRM_Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Handler.Common;

namespace CRM_Handler.PubParam
{
    /// <summary>
    /// pub_param_handle 公共参数 的摘要说明
    /// </summary>
    public class pub_param_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        public static List<pub_param> dicCustomer = null;

        /// <summary>
        /// 联系人等级集合
        /// </summary>
        public static Dictionary<string, string> dic_linkMan_Grade = null;

        /// <summary>
        /// 客户等级集合
        /// </summary>
        public static Dictionary<string, string> dic_customer_Level = null;

        /// <summary>
        /// 客户类型集合
        /// </summary>
        public static Dictionary<string, string> dic_customer_Type = null;

        /// <summary>
        /// 跟进类型
        /// </summary>
        public static Dictionary<string, string> dic_follow_Level = null;

        /// <summary>
        /// 客户属性
        /// </summary>
        public static Dictionary<string, string> dic_customer_Property = null;

        /// <summary>
        /// 联系人等级集合
        /// </summary>
        public static List<Dictionary<string, object>> dic_linkMan_Grade_TC = new List<Dictionary<string, object>>();

        /// <summary>
        /// 客户等级集合
        /// </summary>
        public static List<Dictionary<string, object>> dic_customer_Level_TC = new List<Dictionary<string, object>>();

        /// <summary>
        /// 客户类型集合
        /// </summary>
        public static List<Dictionary<string, object>> dic_customer_Type_TC = new List<Dictionary<string, object>>();

        /// <summary>
        /// 跟进类型
        /// </summary>
        public static List<Dictionary<string, object>> dic_follow_Level_TC = new List<Dictionary<string, object>>();

        /// <summary>
        /// 客户属性
        /// </summary>
        public static List<Dictionary<string, object>> dic_customer_Property_TC = new List<Dictionary<string, object>>();

        #endregion

        #region 中心入口点

        /// <summary>
        /// 中心入口点
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            string func = RequestHelper.string_transfer(context.Request, "func");
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);

                switch (func)
                {
                    case "get_pub_param":
                        get_pub_param(context);
                        break;
                    default:
                        jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 获取参数【guid】

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        public void get_pub_param(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string pub_title = RequestHelper.string_transfer(Request, "pub_title");
            string guid = RequestHelper.string_transfer(Request, "guid");
            try
            {
                //缓存应用
                if (!string.IsNullOrEmpty(pub_title))
                {
                    List<Dictionary<string, object>> dic_list = null;
                    switch (pub_title)
                    {
                        case "跟进类型":
                            dic_list = dic_follow_Level_TC;
                            break;

                        case "联系人级别":
                            dic_list = dic_linkMan_Grade_TC;
                            break;
                        case "客户级别":
                            dic_list = dic_customer_Level_TC;
                            break;
                        case "客户类型":
                            dic_list = dic_customer_Type_TC;
                            break;

                        case "客户属性":
                            dic_list = dic_customer_Property_TC;
                            break;

                        default:
                            break;
                    }                  
                    jsonModel = Constant.get_jsonmodel(0, "success", dic_list);
                }              
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                //管理者
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = "IsAdmin";
                }
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public string GetWhere(HttpContext context)
        {
            string result = string.Empty;
            try
            {
                string pub_title = "";
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(context.Request["pub_title"]))
                {
                    pub_title = context.Request["pub_title"];
                    sb.Append(" and  pub_parentid=(select id from pub_param where pub_title like '%" + pub_title + "%' and pub_parentid = 0)");// //参数类型，跟进类型、客户类型、客户级别等
                }
                result = sb.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 辅助字段
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

    }
}