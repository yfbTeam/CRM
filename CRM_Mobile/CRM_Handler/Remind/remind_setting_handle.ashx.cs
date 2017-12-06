using CRM_BLL;
using CRM_Common;
using CRM_Model;
using CRM_Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace CRM_Handler.Remind
{
    /// <summary>
    /// remind_setting_handle 遗忘提醒设置 的摘要说明
    /// </summary>
    public class remind_setting_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///提示设置集合
        /// </summary>
        public static List<remind_setting> list_All = null;

        /// <summary>
        /// 指定某个用户的提示设置
        /// </summary>
        public static Dictionary<string, List<remind_setting>> dic_Self = new Dictionary<string, List<remind_setting>>();

        #endregion

        #region 中心入口点

        public void ProcessRequest(HttpContext context)
        {
            string func = context.Request["func"] ?? "";
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);

                switch (func)
                {
                    case "get_remind_setting_info":
                        get_remind_setting_info(context);
                        break;
                    case "edit_remind_setting":
                        edit_remind_setting(context);
                        break;
                    default:
                        jsonModel = new JsonModel()
                        {
                            errNum = 5,
                            errMsg = "没有此方法",
                            retData = ""
                        };
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

        #region 获取提醒设置实例【guid】

        /// <summary>
        /// 获取提醒设置实例
        /// </summary>
        /// <param name="context"></param>
        public void get_remind_setting_info(HttpContext context)
        {
            HttpRequest Request = context.Request;
            Hashtable ht = new Hashtable();
            try
            {
                string guid = Request["guid"];
                //获取当前人的联系人（进行分页）
                if (dic_Self.ContainsKey(guid))
                {
                    int page_Index = Convert.ToInt32(Request["PageIndex"]);
                    int page_Size = Convert.ToInt32(Request["PageSize"]);

                    //对象集合转为dic集合列表
                    List<Dictionary<string, object>> dicList = ConverList<remind_setting>.ListToDic(dic_Self[guid]);
                    //返回数据
                    PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = dicList.Count };
                    //数据库包（json格式）
                    jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
                }

                else
                {

                    ht.Add("TableName", "remind_setting");
                    //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                    string fileds = "*";
                    //新加字段fileds，主要是为了方便使用
                    jsonModel = Constant.remind_setting_S.GetPage(ht, fileds, false, GetWhere(context));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 编辑遗忘提醒【guid】

        /// <summary>
        /// 新增遗忘提醒
        /// </summary>
        /// <param name="context"></param>
        public void edit_remind_setting(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = request["id"];
            string guid = request["guid"];
            try
            {

                remind_setting remind = new remind_setting();
                remind.id = Convert.ToInt32(id);
                remind.remind_userid = request["remind_userid"];
                remind.remind_type = request["remind_type"];
                remind.remind_isdelete = "0";
                remind.remind_remark = request["remind_remark"];//重要
                if (dic_Self.ContainsKey(guid))
                {
                    List<remind_setting> dic2 = dic_Self[guid].Where(item => item.id == Convert.ToInt64(id)).ToList<remind_setting>();
                    if (dic2.Count > 0)
                    {
                        dic2[0] = remind;
                        //成功提示
                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = dic2.Count
                        };
                        //开启线程操作数据库
                        new Thread(() =>
                        {
                            jsonModel = Constant.bbc.edit_remind_setting(remind);
                        }) { }.Start();
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
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
                string remind_userid = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" and remind_isdelete=0 ");//提醒
                if (!string.IsNullOrEmpty(context.Request["remind_userid"]))
                {
                    remind_userid = context.Request["remind_userid"];
                    sb.Append(" and remind_userid ='" + remind_userid + "'");//提醒
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