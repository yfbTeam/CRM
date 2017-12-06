using CRM_BLL;
using CRM_Common;
using CRM_Handler;
using CRM_Model;
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
    /// remind_handle 遗忘提醒 的摘要说明
    /// </summary>
    public class remind_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///提示集合
        /// </summary>
        public static List<remind> list_All = null;

        /// <summary>
        /// 指定某个用户的提示群
        /// </summary>
        public static Dictionary<string, List<remind>> dic_Self = new Dictionary<string, List<remind>>();

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
                    case "get_remind_list":
                        get_remind_list(context);
                        break;
                    case "edit_remain":
                        edit_remain(context);
                        break;
                    case "update_remind_isopen":
                        update_remind_isopen(context);
                        break;
                    case "update_remind_isdelete":
                        update_remind_isdelete(context);
                        break;
                    case "get_remind_info":
                        get_remind_info(context);
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

        #region 获取提醒信息【ID   guid】

        /// <summary>
        /// 获取提醒信息
        /// </summary>
        /// <param name="context"></param>
        public void get_remind_info(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            HttpRequest Request = context.Request;
            string id = Request["id"];
            string guid = Request["guid"];

            try
            {
                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    //指定的一个客户
                    List<remind> list1 = (from t in dic_Self[guid]
                                          where t.id == Convert.ToInt64(id)
                                          select t).ToList<remind>();
                    if (list1.Count > 0)
                    {
                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = ConverList<remind>.ListToDic(list1)
                        };
                    }
                }
                else
                {
                    ht.Add("TableName", "remind");
                    //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                    string fileds = "[id] ,[rem_userid],convert(varchar(100),rem_date,20) as rem_date,[rem_content],[rem_status],[rem_isopen],[rem_createdate],[rem_creator],[rem_updatedate],[rem_updateuser],[rem_isdelete],[rem_remark]";
                    //新加字段fileds，主要是为了方便使用
                    jsonModel = Constant.remind_S.GetPage(ht, fileds, false, " and id=" + id + "");
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

        #region 删除遗忘提醒【guid】

        /// <summary>
        /// 删除遗忘提醒
        /// </summary>
        /// <param name="context"></param>
        public void update_remind_isdelete(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = request["id"];
            string guid = request["guid"];
            try
            {
                if (id != "0")
                {
                    if (dic_Self.ContainsKey(guid))
                    {
                        //获取当前用户的所有联系人
                        List<remind> list1 = (from t in dic_Self[guid]
                                              where t.id == Convert.ToInt64(id)
                                              select t).ToList<remind>();
                        //删除指定ID的联系人
                        if (list1.Count > 0)
                        {
                            dic_Self[guid].Remove(list1[0]);
                            jsonModel = new JsonModel()
                            {
                                errNum = 0,
                                errMsg = "success",
                                retData = list1.Count
                            };
                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                try
                                {
                                    remind remind = Constant.remind_S.GetEntityById(Convert.ToInt32(id)).retData as remind;
                                    remind.rem_isdelete = request["rem_isdelete"];
                                    if (remind != null)
                                    {
                                        jsonModel = Constant.remind_S.Update(remind);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                }
                            }) { }.Start();
                        }
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

        #region 新增遗忘提醒【guid】

        /// <summary>
        /// 新增遗忘提醒
        /// </summary>
        /// <param name="context"></param>
        public void edit_remain(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = request["id"];
            string guid = request["guid"];
            try
            {
                remind remind = new remind();
                remind.id = Convert.ToInt32(id);
                remind.rem_date = DateTime.Parse(request["rem_date"].ToString());
                remind.rem_content = request["rem_content"];
                remind.rem_status = request["rem_status"];
                remind.rem_isopen = request["rem_isopen"];
                remind.rem_isdelete = "0";
                //修改《暂时修改功能》
                if (id != "0")
                {
                    if (dic_Self.ContainsKey(guid))
                    {
                        List<remind> dic2 = dic_Self[guid].Where(item => item.id == Convert.ToInt64(id)).ToList<remind>();
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
                                remind advert = Constant.remind_S.GetEntityById(Convert.ToInt32(id)).retData as remind;
                                if (advert != null)
                                {
                                    jsonModel = Constant.remind_S.Update(remind);
                                }
                            }) { }.Start();
                        }
                    }
                }
                else
                {
                    remind.rem_userid = request["rem_userid"];
                    if (dic_Self.ContainsKey(guid))
                    {
                        //缓存添加客户
                        dic_Self[guid].Add(remind);
                        //当前添加客户
                        list_All.Add(remind);

                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = 1
                        };
                        new Thread(() =>
                              {
                                  jsonModel = Constant.remind_S.Add(remind);
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

        #region 打开或关闭遗忘提醒【guid】

        /// <summary>
        /// 打开或关闭遗忘提醒
        /// </summary>
        /// <param name="context"></param>
        public void update_remind_isopen(HttpContext context)
        {
            HttpRequest request = context.Request;          
            HttpResponse response = context.Response;
            string id = request["id"];
               string guid = request["guid"];
            try
            {
                if (id != "0")
                {
                    if (dic_Self.ContainsKey(guid))
                    {
                        List<remind> dic2 = dic_Self[guid].Where(item => item.id == Convert.ToInt64(id)).ToList<remind>();
                        if (dic2.Count > 0)
                        {
                            dic2[0].rem_isopen = request["rem_isopen"];
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
                                remind remind = Constant.remind_S.GetEntityById(Convert.ToInt32(id)).retData as remind;
                                remind.rem_isopen = request["rem_isopen"];
                                if (remind != null)
                                {
                                    jsonModel = Constant.remind_S.Update(remind);
                                }
                            }) { }.Start();
                        }
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

        #region 遗忘提醒列表【guid】

        /// <summary>
        /// 遗忘提醒列表
        /// </summary>
        /// <param name="context"></param>
        public void get_remind_list(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            HttpRequest Request = context.Request;
            try
            {
                 string guid = Request["guid"];
                //获取当前人的联系人（进行分页）
                 if (dic_Self.ContainsKey(guid))
                 {
                     int page_Index = Convert.ToInt32(Request["PageIndex"]);
                     int page_Size = Convert.ToInt32(Request["PageSize"]);
                                       
                     //进行分页
                     List<remind> list2 = GetPageByLinq(dic_Self[guid], page_Index, page_Size);
                     //对象集合转为dic集合列表
                     List<Dictionary<string, object>> dicList = ConverList<remind>.ListToDic(list2);
                     foreach (var item in dicList)
                     {
                     }
                     //返回数据
                     PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = list2.Count };
                     //数据库包（json格式）
                     jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
                 }

                 else
                 {
                     bool ispage = false;
                     if (!string.IsNullOrEmpty(context.Request["ispage"]))
                     {
                         ispage = Convert.ToBoolean(context.Request["ispage"]);
                     }
                     ht.Add("PageIndex", context.Request["PageIndex"] ?? "1");
                     ht.Add("PageSize", context.Request["PageSize"] ?? "10");
                     ht.Add("TableName", "remind");
                     //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                     string fileds = "id,dbo.get_convert_date(rem_date)as rem_date,rem_content,rem_status,rem_isopen,dbo.get_am_pm(rem_date) as am_pm,dbo.get_hour_min(rem_date) as hour_min";
                     //新加字段fileds，主要是为了方便使用
                     jsonModel = Constant.remind_S.GetPage(ht, fileds, ispage, GetWhere(context));
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
                string rem_userid = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" and rem_isdelete=0 ");//提醒
                if (!string.IsNullOrEmpty(context.Request["rem_userid"]))
                {
                    rem_userid = context.Request["rem_userid"];
                    sb.Append(" and rem_userid ='" + rem_userid + "'");//提醒
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

        #region 辅助方法【linq 分页】

        /// <summary>
        /// 辅助方法【linq 分页】
        /// </summary>
        /// <param name="lstPerson"></param>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static List<remind> GetPageByLinq(List<remind> lstPerson, int pageIndex, int PageSize)
        {
            List<remind> result = null;
            try
            {
                result = lstPerson.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        #endregion
    }
}