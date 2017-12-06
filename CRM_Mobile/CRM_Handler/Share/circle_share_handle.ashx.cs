using CRM_BLL;
using CRM_Common;
using CRM_Handler;
using CRM_Handler.Common;
using CRM_Handler.LinkMan;
using CRM_Handler.Report;
using CRM_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;

namespace CRM_Handler.Share
{
    /// <summary>
    /// circle_share_handle 的摘要说明
    /// </summary>
    public class circle_share_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///分享集合
        /// </summary>
        public static List<share> list_All = null;

        /// <summary>
        /// 指定某个用户的分享群
        /// </summary>
        //public static Dictionary<string, List<share>> dic_Self = new Dictionary<string, List<share>>();

        #endregion

        #region 中心入口点

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);

                if (list_All.Count > 0)
                {
                    switch (func)
                    {
                        case "edit_share":
                            edit_share(context);
                            break;
                        case "get_share_list":
                            get_share_list(context);
                            break;
                        default:
                            jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                            break;
                    }
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

        #region 获取共享圈内容

        /// <summary>
        /// 获取共享圈内容
        /// </summary>
        /// <param name="context"></param>
        public void get_share_list(HttpContext context)
        {
            HttpRequest Request = context.Request;

            //当前用户ID
            string guid = RequestHelper.string_transfer(Request, "guid");
            try
            {
                //请求参数
                bool ispage = RequestHelper.bool_transfer(Request, "ispage");

                //每一页包含的数量
                int PageSize = RequestHelper.int_transfer(Request, "PageSize");
                //第几页
                int PageIndex = RequestHelper.int_transfer(Request, "PageIndex");

                //封装到PagedDataModel的元数据
                List<report> report_List = new List<report>();

                //进行分页
                List<share> list2 = GetPageByLinq(list_All, PageIndex, PageSize);

                //数据对应
                foreach (var fp in list2)
                {
                    List<workreport> list1 = (from t in workreport_handle.list_All
                                              where t.id == Convert.ToInt64(fp.table_id)
                                              select t).ToList();
                    //跟进记录（对应页面的实体类型）
                    report report = new report();

                    List<praise> list_praise = (from t in praise_handle.list_All
                                                where t.praise_table_id == Convert.ToInt64(fp.id) && t.praise_userid == guid && t.praise_type == "2"
                                                select t).ToList();



                    string is_praise = "";
                    if (list_praise.Count > 0)
                    {
                        is_praise = "1";
                    }
                    else
                    {
                        is_praise = "0";
                    }
                    //附件跟进记录（对应页面的实体类型【通过数据库映射填充实体所需部分信息】）
                    foreach (var work_report in list1)
                    {
                        report.report_info = new report_info()
                        {
                            report_content = work_report.report_content,
                            report_username = work_report.report_username,
                            report_createdate = Convert.ToDateTime(work_report.report_createdate).ToString("yyyy-MM-dd"),
                            work_report_id = Convert.ToInt64(work_report.id),
                            report_startdate = Convert.ToDateTime(work_report.report_startdate).ToString("yyyy-MM-dd"),
                            report_enddate = Convert.ToDateTime(work_report.report_enddate).ToString("yyyy-MM-dd"),
                            report_plan = work_report.report_plan,
                            is_praise = is_praise,
                            id = Convert.ToInt32(fp.id),
                            report_type = (int)work_report.report_type,

                        };
                    }

                    #region 获取图片

                    if (Constant.list_picture_All != null)
                    {
                        //获取指定的图片【类型 和ID】
                        List<picture> list_picture = (from t in Constant.list_picture_All
                                                      where t.pic_en_table == "workreport" && t.pic_table_id == Convert.ToInt32(fp.id)
                                                      select t).ToList<picture>();
                        List<Dictionary<string, object>> list_picture_1 = ConverList<picture>.ListToDic(list_picture);
                        report.picture = list_picture_1;
                    }
                    #endregion

                    #region 获取评论


                    var d = comment_handle.list_All.Where(i => i.com_type == "3").ToList();

                    //获取指定的图片【类型 和ID】
                    List<comment> list_p = (from t in comment_handle.list_All
                                            where t.com_table_id == Convert.ToInt32(fp.id) && t.com_isdelete == "0" && t.com_type == "3"
                                            select t).ToList<comment>();
                    List<Dictionary<string, object>> list = ConverList<comment>.ListToDic(list_p);
                    report.comment = list;

                    #endregion

                    #region 获取点赞人

                    List<praise> list_praises = (from t in praise_handle.list_All
                                                 where t.praise_table_id == Convert.ToInt32(fp.id) && t.praise_type == "2"
                                                 select t).ToList<praise>();
                    List<Dictionary<string, object>> list_p1 = ConverList<praise>.ListToDic(list_praises);
                    report.praise = list_p1;

                    #endregion

                    report_List.Add(report);
                }

                //返回数据
                PagedDataModel<report> psd = new PagedDataModel<report>()
                {
                    PagedData = report_List,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    RowCount = list_All.Count
                };

                //数据包（json格式）
                jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };

            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex.Message);
            }
            finally
            {
                #region 没有缓存机制的情况下用的

                //string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
                //context.Response.Write("{\"result\":" + jsonString + "}");

                #endregion

                string result = Constant.jss.Serialize(jsonModel);
                context.Response.Write("{\"result\":" + result + "}");
            }
        }

        #endregion

        #region 开始分享

        /// <summary>
        /// 开始分享
        /// </summary>
        /// <param name="context"></param>
        public void edit_share(HttpContext context)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;

            try
            {
                share _share = new CRM_Model.share();
                _share.table_id = RequestHelper.int_transfer(Request, "table_id");
                _share.type = RequestHelper.string_transfer(Request, "type");
                bool result = _share.table_id != 0 && !string.IsNullOrEmpty(_share.type) ? true : false;

                #region 判断是否已经分享过，已经分享过的话isshare为1 否则为0

                string isshare = "0";
                int exit_Count = circle_share_handle.list_All.Count(t => _share.table_id == (int)t.table_id && t.type == _share.type);

                if (exit_Count > 0)
                {
                    isshare = "1";
                    result = false;
                }
                else
                {
                    if (!list_All.Contains(_share))
                    {
                        list_All.Add(_share);
                    }
                }
                #endregion

                jsonModel = Constant.get_jsonmodel(0, "success", isshare);

                if (result)
                {
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            JsonModel js = Constant.share_S.Add(_share);
                            _share.id = Convert.ToInt64(js.retData);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }
                    }) { }.Start();
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
        /// 辅助方法【linq 分页】
        /// </summary>
        /// <param name="lstPerson"></param>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static List<share> GetPageByLinq(List<share> lstPerson, int pageIndex, int PageSize)
        {
            List<share> result = null;
            List<share> list = null;
            try
            {
                list = lstPerson.OrderByDescending(i => i.id).ToList();
                result = list.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
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