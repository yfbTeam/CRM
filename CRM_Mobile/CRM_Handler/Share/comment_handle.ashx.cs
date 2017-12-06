using CRM_BLL;
using CRM_Common;
using CRM_Handler.Report;
using CRM_Handler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Model;
using CRM_Handler.Common;

namespace CRM_Handler.Share
{
    /// <summary>
    /// comment_handle 评论 的摘要说明
    /// </summary>
    public class comment_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///评论群
        /// </summary>
        public static List<comment> list_All = null;

        #endregion

        #region 中心入口点

        public void ProcessRequest(HttpContext context)
        {

            string func = RequestHelper.string_transfer(context.Request, "func");
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);

                switch (func)
                {
                    case "edit_comment":
                        edit_comment(context);
                        break;
                    case "get_comment":
                        get_comment(context);
                        break;
                    case "update_comment_isdelete":
                        update_comment_isdelete(context);
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

        #region 获取评论【tableID Type guid】

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="context"></param>
        public void get_comment(HttpContext context)
        {
            HttpRequest Request = context.Request;

            //获取评论
            long id = RequestHelper.long_transfer(Request, "id");
            string type = RequestHelper.string_transfer(Request, "type");
            try
            {

                //指定的一个客户
                List<comment> list1 = (from t in list_All
                                       where t.com_table_id == id && t.com_type == type && t.com_isdelete == "0"
                                       select t).ToList();
                if (list1.Count > 0)
                {
                    jsonModel = Constant.get_jsonmodel(0, "success", ConverList<comment>.ListToDic(list1));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 添加或编辑评论

        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="context"></param>
        public void edit_comment(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                comment comment = new comment();
                comment.com_table_id = RequestHelper.long_transfer(Request, "com_table_id");
                comment.com_parent_id = RequestHelper.long_transfer(Request, "com_parent_id");
                comment.com_content = RequestHelper.string_transfer(Request, "com_content");
                comment.com_type = RequestHelper.string_transfer(Request, "com_type");
                string guid = RequestHelper.string_transfer(Request, "guid");

                //修改《暂时修改功能》
                if (id > 0)
                {
                    //编辑评论
                    edit_comment(id, comment);

                }
                else if(id == 0)
                {
                    comment.com_userid = RequestHelper.string_transfer(Request, "com_userid");
                    comment.com_username = RequestHelper.string_transfer(Request, "com_username");
                    comment.com_createdate = DateTime.Now;
                    comment.com_updatedate = DateTime.Now;
                    comment.com_isdelete = "0";
                    if (!list_All.Contains(comment))
                    {
                        //缓存添加签到
                        list_All.Add(comment);
                    }
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);
                    new Thread(() =>
               {
                   try
                   {
                       //添加评论
                       add_comment(comment, guid);
                   }
                   catch (Exception ex)
                   {
                       LogHelper.Error(ex);
                   }
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

        //添加评论
        private void add_comment(comment comment, string guid)
        {

            try
            {
                //数据库标示
                jsonModel = Constant.comment_S.Add(comment);

                if (workreport_handle.dic_Self.ContainsKey(guid))
                {
                    //工作报告,当前用户
                    List<workreport> workreport_selfs = workreport_handle.dic_Self[guid];
                    //已阅读则进行标示【报告】
                    workreport workreport = workreport_selfs.Where(item => item.id == comment.com_table_id).FirstOrDefault();
                    //标示
                    if (workreport != null)
                    {                      
                            workreport.report_status = "1";
                            Constant.workreport_S.Update(workreport);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 编辑评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        private void edit_comment(long id, comment comment)
        {
            try
            {
                comment edit_comment = list_All.FirstOrDefault(item => item.id == id && item.com_type == comment.com_type);
                if (edit_comment != null)
                {
                    #region oldsolution

                    //edit_comment.com_table_id = comment.com_table_id;
                    //edit_comment.com_parent_id = comment.com_parent_id;
                    //edit_comment.com_userid = comment.com_userid;
                    //edit_comment.com_username = comment.com_username;

                    #endregion

                    edit_comment.com_content = comment.com_content;
                    edit_comment.com_type = comment.com_type;
                    edit_comment.com_isdelete = comment.com_isdelete;

                    //成功提示                       
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            jsonModel = Constant.comment_S.Update(edit_comment);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }
                    }) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 删除评论

        /// <summary>
        ///删除 
        /// </summary>
        /// <param name="context"></param>
        public void update_comment_isdelete(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            string guid = RequestHelper.string_transfer(Request, "guid");
            try
            {
                if (id != 0)
                {
                    //删除指定的评论
                    comment comment_delete = list_All.FirstOrDefault(t => t.id == id);
                    //进行评论删除
                    if (comment_delete != null)
                    {
                        if (list_All.Contains(comment_delete))
                        {
                            //删除客户需要两个地方
                            list_All.Remove(comment_delete);
                        }
                        jsonModel = Constant.get_jsonmodel(0, "success", 1);
                        //开启线程操作数据库
                        new Thread(() =>
                        {
                            try
                            {
                                comment_delete.com_isdelete = RequestHelper.string_transfer(Request, "com_isdelete");
                                jsonModel = Constant.comment_S.Update(comment_delete);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(ex);
                            }
                        }) { IsBackground = true }.Start();
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