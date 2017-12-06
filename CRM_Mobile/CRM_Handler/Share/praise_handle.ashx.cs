using CRM_BLL;
using CRM_Common;
using CRM_Handler.Common;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace CRM_Handler.Share
{
    /// <summary>
    /// praise_handle 的摘要说明
    /// </summary>
    public class praise_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///点赞群
        /// </summary>
        public static List<praise> list_All = null;

        ///// <summary>
        ///// 指定某个用户的点赞群
        ///// </summary>
        //public static Dictionary<string, List<praise>> dic_Self = new Dictionary<string, List<praise>>();

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
                    case "edit_praise":
                        edit_praise(context);
                        break;
                    case "get_praise":
                        get_praise(context);
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

        #region 获取点赞信息【ID  guid】

        public void get_praise(HttpContext context)
        {
            HttpRequest Request = context.Request;
            long tabelID = RequestHelper.long_transfer(Request, "id");
            try
            {

                //指定的一个客户
                List<praise> list1 = (from t in list_All
                                      where t.id == tabelID && t.praise_isdelete == "0"
                                      select t).ToList();
                if (list1.Count > 0)
                {
                    jsonModel = Constant.get_jsonmodel(0, "success", ConverList<praise>.ListToDic(list1));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
                context.Response.Write("{\"result\":" + jsonString + "}");
            }
        }

        #endregion

        #region 添加或删除点赞【通过ID进行编辑 guid        要么删要么增，不存在编辑】

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="context"></param>
        public void edit_praise(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = context.Request["id"];
            string guid = request["guid"];
            try
            {
                praise praise = new praise();
                praise.id = Convert.ToInt32(id);
                praise.praise_table_id = int.Parse(request["praise_table_id"].ToString());
                praise.praise_isdelete = "0";
                praise.praise_type = request["praise_type"];


                List<praise> list2 = list_All.Where(item => item.praise_table_id == Convert.ToInt64(praise.praise_table_id))
                    .Where(item => item.praise_userid == guid)
                    .Where(item => item.praise_type == praise.praise_type)
                    .ToList<praise>();
                //若存在则删除
                if (list2.Count > 0)
                {                   
                    list_All.Remove(list2[0]);

                    List<praise> list_Remove = list_All.Where(item => item.praise_table_id == Convert.ToInt64(praise.praise_table_id)).Where(item => item.praise_type == praise.praise_type).ToList<praise>();
                    //成功提示
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = list_Remove
                    };
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        Constant.praise_S.Delete(Convert.ToInt32(list2[0].id));

                    }) { IsBackground = true }.Start();
                }
                //若不存在则添加
                else 
                {
                    praise.praise_userid = request["praise_userid"];
                    praise.praise_username = request["praise_username"];
                    praise.praise_createdate = DateTime.Now;
                    praise.praise_updatedate = DateTime.Now;
                   
                    //添加
                    list_All.Add(praise);

                    List<praise> list_add = list_All.Where(item => item.praise_table_id == Convert.ToInt64(praise.praise_table_id)).Where(item => item.praise_type == praise.praise_type).ToList<praise>();

                    //成功提示
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = list_add
                    };
                   
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        DataTable report = Constant.bbc.edit_praise(praise);
                        List<Dictionary<string, object>> s_list = new List<Dictionary<string, object>>();
                        s_list = Constant.common.DataTableToList(report);
                    }) { IsBackground = true }.Start();
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