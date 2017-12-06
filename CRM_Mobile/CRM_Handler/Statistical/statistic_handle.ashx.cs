using CRM_BLL;
using CRM_Common;
using CRM_Model;
using CRM_Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Handler.Custom;
using CRM_Handler.LinkMan;
using CRM_Handler.Follow;
using CRM_Handler.SiginIn;
using CRM_Handler.WorkPlan;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CRM_Handler.Report;
using CRM_Handler.Share;
using CRM_Handler.Common;

namespace CRM_Handler.Statistical
{
    /// <summary>
    /// statistic_handle 的摘要说明
    /// </summary>
    public class statistic_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        DataTable s_dt = new DataTable();

        #endregion

        #region 中心入口点

        /// <summary>
        /// 中心入口点
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                //当前用户ID
                string guid = RequestHelper.string_transfer(Request, "guid");
                if (func == "DataInit")
                {
                    //填充所有数据
                    Constant.Fill_All_Data(context);
                    //角色初始化
                    Constant.Role_Cacle_Init(context);
                }
                else if (Data_check_helper.check_Self(HanderType.custom, guid))
                {
                    switch (func)
                    {
                        case "get_statistic_list":
                            get_statistic_list(context, guid);
                            break;
                        case "GetDepartMent":
                            GetDepartMent(context);
                            break;

                        case "Get_Self_DepartMent":
                            Get_Self_DepartMent(context);
                            break;
                        case "get_statistic_today":
                            get_statistic_today(context, guid);
                            break;
                        case "get_statistic_detail":
                            get_statistic_detail(context, guid);
                            break;
                        case "get_today_matters":
                            get_today_matters(context, guid);
                            break;
                        case "GetMemmber":
                            GetMemmber(context, guid);
                            break;
                        case "Get_All_Memmber":
                            Get_All_Memmber(context, guid);
                            break;
                        case "Orgization_Reflesh":
                            //带身份才能刷新
                            Orgization_Reflesh();
                            break;
                        case "get_statistic_detail_PC":
                            //带身份才能进行获取
                            get_statistic_detail_PC(context, guid);
                            break;


                        default:
                            jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                            break;
                    }
                }
                else if (Constant.Chek_Someone_Is_Valied(guid))
                {
                    switch (func)
                    {
                        case "get_statistic_detail_PC":
                            //带身份才能进行获取
                            get_statistic_detail_PC(context, guid);
                            break;
                        default:
                            jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                            break;
                    }
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(5, "请到首页进行刷新", "");
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

        #region 销售简报

        /// <summary>
        /// 销售简报
        /// </summary>
        /// <param name="context"></param>
        public void get_statistic_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //分页信息
                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");
                //模糊名称查询
                string like_name = RequestHelper.string_transfer(Request, "like_name");

                //開始日期
                DateTime stardate = RequestHelper.DateTime_transfer(Request, "stardate");
                //結束日期
                DateTime enddate = RequestHelper.DateTime_transfer(Request, "enddate").AddDays(1);
                //模糊查询【用户名称】
                string s_username = RequestHelper.string_transfer(Request, "s_username");

                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");

                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

                List<UserInfo> list = null;
                //管理层
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    switch (Constant.dicLimit_P[guid].LimitType)
                    {
                        case LimitType.Super_Admin:
                            list = Constant.List_All_RelateUserInfo;

                            break;
                        case LimitType.Common_Admin:
                            list = Constant.dicLimit_P[guid].List_Uni_UserName;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //普通员工
                    list = Constant.dic_All_user.Where(item => item.UniqueNo == guid).ToList<UserInfo>();
                }

                list = Check_And_Get_List_dep(departmentID, memmberID, list).ToList();
              
                all_get_count(guid, stardate, enddate, s_username, dicList, list);
                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList_real = GetPageByLinq(dicList, page_Index, page_Size);
                //返回数据
                PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList_real, PageIndex = page_Index, PageSize = page_Size, RowCount = dicList.Count };
                //数据包（json格式）
                jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };

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

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<UserInfo> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<UserInfo> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.UniqueNo == memmberID
                                  select w);
                }
                else if (!string.IsNullOrEmpty(departmentID))
                {
                    //先获取部门信息
                    DepartMent department = Constant.DepartMent_List.FirstOrDefault(d => d.ID == departmentID);

                    var UniqueNo_string = (from userInfo in department.UserInfo_List
                                           select userInfo.UniqueNo);

                    //取到部门，获取下列信息
                    if (department != null)
                    {
                        Data_selfs = (from w in Data_selfs
                                      where UniqueNo_string.Contains(w.UniqueNo)
                                      select w);
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Data_selfs;
        }


        /// <summary>
        /// 详情获取辅助
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="d_start"></param>
        /// <param name="d_end"></param>
        /// <param name="s_username"></param>
        /// <param name="dicList"></param>
        /// <param name="list"></param>
        private static void all_get_count(string guid, DateTime d_start, DateTime d_end, string s_username, List<Dictionary<string, object>> dicList, List<UserInfo> list)
        {
            try
            {               
                foreach (var item in list)
                {
                    if (item.Name.Contains(s_username))
                    {
                        string uniqueno = item.UniqueNo;
                        //客户列表,当前用户
                        List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                        //新增客户数量
                        int s_cust_customer_count = cust_customer_selfs.Count(child => child.cust_users.Contains(uniqueno) && child.cust_createdate >= d_start && child.cust_createdate < d_end);
                        //联系人列表,当前用户
                        List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                        //新增联系人数量
                        int s_linkman_count = cust_linkman_selfs.Count(child => child.link_users.Contains(uniqueno) && child.link_createdate >= d_start && child.link_createdate < d_end);
                        //跟进列表
                        List<follow_up> follow_selfs = follow_up_handle.dic_Self[guid];
                        //跟进数量
                        int s_followup_count = follow_selfs.Count(child => child.follow_userid == uniqueno && child.follow_date >= d_start && child.follow_date < d_end);
                        //签到列表,当前用户
                        List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];
                        //签到数量
                        int s_sign_count = sign_in_selfs.Count(child => child.sign_userid == uniqueno && child.sign_date >= d_start && child.sign_date < d_end);
                        //工作计划,当前用户
                        List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                        //工作计划
                        int s_wrokplan = workplan_selfs.Count(child => child.wp_userid == uniqueno && child.wp_createdate >= d_start && child.wp_createdate < d_end);
                        //工作报告,当前用户
                        List<workreport> workreport_selfs = workreport_handle.dic_Self[guid];
                        //工作报告
                        int s_workreport_count = workreport_selfs.Count(child => child.report_userid == uniqueno && child.report_createdate >= d_start && child.report_createdate < d_end);

                        //总监点评
                        var s_comment = (from com in comment_handle.list_All
                                         join foll in follow_up_handle.list_All on com.com_table_id equals foll.id
                                         where foll.follow_userid == uniqueno && com.com_type == "1" && com.com_createdate >= d_start && com.com_createdate < d_end 
                                         && uniqueno != com.com_userid
                                         select com.com_table_id).Count();

                        s_comment += (from com in comment_handle.list_All
                                      join work in workreport_handle.list_All on com.com_table_id equals work.id
                                      where work.report_userid == uniqueno && com.com_type == "2" && com.com_createdate >= d_start && com.com_createdate < d_end
                                       && uniqueno != com.com_userid
                                      select com.com_table_id).Count();

                        //总监点评
                        //int s_comment = comment_handle.list_All.Count(child => child.com_userid == uniqueno && child.com_createdate > d_start && child.com_createdate < d_end);

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("s_users", uniqueno);
                        dic.Add("s_username", item.Name);

                        dic.Add("s_cust_customer_count", s_cust_customer_count);
                        dic.Add("s_linkman_count", s_linkman_count);
                        dic.Add("s_followup_count", s_followup_count);
                        dic.Add("s_sign_count", s_sign_count);
                        dic.Add("s_wrokplan", s_wrokplan);
                        dic.Add("s_workreport_count", s_workreport_count);
                        dic.Add("s_bf_count", s_followup_count + s_sign_count);
                        dic.Add("s_comment", s_comment);

                        //字典集合添加字典
                        dicList.Add(dic);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 今日、本周、本月统计

        /// <summary>
        /// 今日统计
        /// </summary>
        /// <param name="context"></param>
        public void get_statistic_today(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                if (!string.IsNullOrEmpty(guid))
                {
                    string username = RequestHelper.string_transfer(Request, "username");
                    //1 为本日 2 为本周 3为本月
                    string type = RequestHelper.string_transfer(Request, "type");

                    //客户列表,当前用户
                    List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                    //联系人列表,当前用户
                    List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                    //跟进列表
                    List<follow_up> follow_selfs = follow_up_handle.dic_Self[guid];
                    //签到列表,当前用户
                    List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];

                    int cust_count;
                    int link_count;
                    int follow_count;
                    int sigin_count;
                    //今日统计
                    count_today(guid, type, cust_customer_selfs, cust_linkman_selfs, follow_selfs, sign_in_selfs, out cust_count, out link_count, out follow_count, out sigin_count);

                    Dictionary<string, int> diclist = new Dictionary<string, int>();
                    diclist.Add("s_cust_customer_count", cust_count);
                    diclist.Add("s_linkman_count", link_count);
                    diclist.Add("s_followup_count", follow_count);
                    diclist.Add("s_sign_count", sigin_count);
                    jsonModel = Constant.get_jsonmodel(0, "success", diclist);

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                //管理者
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = Convert.ToString(Constant.dicLimit_P[guid].LimitType);
                }
                else
                {
                    jsonModel.status = Convert.ToString(LimitType.Common_Memmber);
                }
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        ///  //今日统计
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="type"></param>
        /// <param name="cust_customer_selfs"></param>
        /// <param name="cust_linkman_selfs"></param>
        /// <param name="follow_selfs"></param>
        /// <param name="sign_in_selfs"></param>
        /// <param name="cust_count"></param>
        /// <param name="link_count"></param>
        /// <param name="follow_count"></param>
        /// <param name="sigin_count"></param>
        private static void count_today(string guid, string type, List<cust_customer> cust_customer_selfs, List<cust_linkman> cust_linkman_selfs, List<follow_up> follow_selfs, List<sign_in> sign_in_selfs, out int cust_count, out int link_count, out int follow_count, out int sigin_count)
        {

            DateTime dt_now = DateTime.Now;
            //客户数量
            cust_count = 0;
            //联系人数量
            link_count = 0;
            //新增跟进
            follow_count = 0;
            //拜访次数
            sigin_count = 0;

            if (cust_customer_selfs == null || cust_linkman_selfs == null || follow_selfs == null || sign_in_selfs == null)
            {
                return;
            }

            try
            {
                switch (type)
                {
                    //当日
                    case "1":
                        //客户数量
                        cust_count = cust_customer_selfs.Count(item => item.cust_users.Contains(guid) && Convert.ToDateTime(item.cust_createdate).Year == dt_now.Year && Convert.ToDateTime(item.cust_createdate).DayOfYear == dt_now.DayOfYear);

                        //联系人数量
                        link_count = cust_linkman_selfs.Count(item => item.link_users.Contains(guid) && Convert.ToDateTime(item.link_createdate).Year == dt_now.Year && Convert.ToDateTime(item.link_createdate).DayOfYear == dt_now.DayOfYear);
                        //新增跟进
                        follow_count = follow_selfs.Count(item => item.follow_userid == guid && Convert.ToDateTime(item.follow_date).Year == dt_now.Year && Convert.ToDateTime(item.follow_date).DayOfYear == dt_now.DayOfYear);
                        //拜访次数
                        sigin_count = sign_in_selfs.Count(item => item.sign_userid == guid && Convert.ToDateTime(item.sign_date).Year == dt_now.Year && Convert.ToDateTime(item.sign_date).DayOfYear == dt_now.DayOfYear);

                        break;
                    //当周
                    case "2":
                        //客户数量 
                        cust_count = cust_customer_selfs.Count(item => !string.IsNullOrEmpty(item.cust_users) && item.cust_users.Contains(guid) && Constant.IsInSameWeek(Convert.ToDateTime(item.cust_createdate), dt_now));
                        //联系人数量
                        link_count = cust_linkman_selfs.Count(item => !string.IsNullOrEmpty(item.link_users) && item.link_users.Contains(guid) && Constant.IsInSameWeek(Convert.ToDateTime(item.link_createdate), dt_now));
                        //新增跟进
                        follow_count = follow_selfs.Count(item => !string.IsNullOrEmpty(item.follow_userid) && item.follow_userid == guid && Constant.IsInSameWeek(Convert.ToDateTime(item.follow_date), dt_now));
                        //拜访次数
                        sigin_count = sign_in_selfs.Count(item => !string.IsNullOrEmpty(item.sign_userid) && item.sign_userid == guid && Constant.IsInSameWeek(Convert.ToDateTime(item.sign_date), dt_now));


                        break;
                    //当月
                    case "3":
                        //客户数量
                        cust_count = cust_customer_selfs.Count(item => item.cust_users.Contains(guid) && Convert.ToDateTime(item.cust_createdate).Year == dt_now.Year && Convert.ToDateTime(item.cust_createdate).Month == dt_now.Month);
                        //联系人数量
                        link_count = cust_linkman_selfs.Count(item => item.link_users.Contains(guid) && Convert.ToDateTime(item.link_createdate).Year == dt_now.Year && Convert.ToDateTime(item.link_createdate).Month == dt_now.Month);
                        //新增跟进
                        follow_count = follow_selfs.Count(item => item.follow_userid == guid && Convert.ToDateTime(item.follow_date).Year == dt_now.Year && Convert.ToDateTime(item.follow_date).Month == dt_now.Month);
                        //拜访次数
                        sigin_count = sign_in_selfs.Count(item => item.sign_userid == guid && Convert.ToDateTime(item.sign_date).Year == dt_now.Year && Convert.ToDateTime(item.sign_date).Month == dt_now.Month);

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }



        #endregion

        #region 今日事项

        /// <summary>
        /// 今日事项
        /// </summary>
        /// <param name="context"></param>
        public void get_today_matters(HttpContext context, string guid)
        {
            try
            {
                if (!string.IsNullOrEmpty(guid))
                {
                    DateTime dt_now = DateTime.Now;
                    //工作计划,当前用户
                    List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                    if (workplan_selfs != null && workplan_selfs.Count > 0)
                    {
                        List<workplan> workPlay_List = workplan_selfs.Where(item => item.wp_userid == guid && Convert.ToDateTime(item.wp_createdate).Year == dt_now.Year
                            &&dt_now.DayOfYear >= Convert.ToDateTime(item.wp_plandate).DayOfYear
                            && dt_now.DayOfYear <= Convert.ToDateTime(item.wp_endplandate).DayOfYear 
                            ).ToList();
                        //跟进列表
                        List<follow_up> follow_selfs = follow_up_handle.dic_Self[guid];
                        List<follow_up> foolow_up_List = follow_selfs.Where(item => item.follow_userid == guid && Convert.ToDateTime(item.follow_date).Year == dt_now.Year
                            &&dt_now.DayOfYear == Convert.ToDateTime(item.follow_date).DayOfYear                             
                            ).ToList();

                        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

                        foreach (var item in workPlay_List)
                        {
                            Dictionary<string, object> diclist = new Dictionary<string, object>();
                            diclist.Add("content", "工作计划：" + item.wp_content);
                            list.Add(diclist);
                        }

                        foreach (var item in foolow_up_List)
                        {
                            Dictionary<string, object> diclist = new Dictionary<string, object>();
                            diclist.Add("content", "跟进记录：" + item.follow_content);
                            list.Add(diclist);
                        }
                        //成功返回               
                        jsonModel = Constant.get_jsonmodel(0, "success", list);
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
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 统计详情

        /// <summary>
        /// 统计详情
        /// </summary>
        /// <param name="context"></param>
        public void get_statistic_detail(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            //开始日期
            DateTime d_start = RequestHelper.DateTime_transfer(Request, "s_startdate");
            //结束日期
            DateTime d_end = RequestHelper.DateTime_transfer(Request, "s_enddate").AddDays(1);

            string userid = RequestHelper.string_transfer(Request, "userid");
            try
            {               
                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                //普通员工
                List<UserInfo> list = Constant.dic_All_user.Where(item => item.UniqueNo == userid).ToList();

                if (list.Count > 0)
                {
                    string uniqueno = list[0].UniqueNo;

                    //新增客户数量
                    List<cust_customer> s_cust_customer_list = cust_customer_handle.list_All.Where(child => child.cust_users.Contains(uniqueno) && child.cust_createdate >= d_start && child.cust_createdate < d_end).ToList();

                    //新增联系人数量
                    List<cust_linkman> s_linkman_list = cust_linkman_handle.list_All.Where(child => child.link_users.Contains(uniqueno) && child.link_createdate >= d_start && child.link_createdate < d_end).ToList();
                    //跟进数量
                    List<follow_up> s_followup_list = follow_up_handle.list_All.Where(child => child.follow_userid == uniqueno && child.follow_date >= d_start && child.follow_date < d_end).ToList();
                    //签到数量
                    List<sign_in> s_sign_list = sign_in_handle.list_All.Where(child => child.sign_userid == uniqueno && child.sign_date >= d_start && child.sign_date < d_end).ToList();

                      //工作计划数量
                    List<workplan> s_workplan_list = workplan_handle. list_All.Where(child => child.wp_userid == uniqueno && child.wp_createdate >= d_start && child.wp_createdate < d_end).ToList();
                    //工作报告数量
                    List<workreport> s_workreport_list =  workreport_handle. list_All.Where(child => child.report_userid == uniqueno && child.report_createdate >= d_start && child.report_createdate < d_end).ToList();


                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("s_users", uniqueno);
                    dic.Add("s_username", list[0].Name);

                    //客户统计
                    detail_customer(s_cust_customer_list, dic);
                    //联系人统计
                    detail_linkman(s_linkman_list, dic);
                    //跟进统计
                    detail_follow(s_followup_list, dic);
                    //签到统计
                    detail_sign(s_sign_list, dic);
                    //工作计划统计
                    detail_workplane(s_workplan_list, dic);
                    //工作总结统计
                    detail_workreport(s_workreport_list, dic);

                    #region 总监点评

                    //总监点评
                    var select1 = (from com in comment_handle.list_All
                                   join foll in follow_up_handle.list_All on com.com_table_id equals foll.id
                                   where foll.follow_userid == uniqueno && com.com_type == "1" && com.com_createdate > d_start && com.com_createdate < d_end
                                    && uniqueno != com.com_userid
                                   select com).ToList();

                    var select2 = (from com in comment_handle.list_All
                                   join work in workreport_handle.list_All on com.com_table_id equals work.id
                                   where work.report_userid == uniqueno && com.com_type == "2" && com.com_createdate > d_start && com.com_createdate < d_end
                                    && uniqueno != com.com_userid
                                   select com).ToList();

                    select1.AddRange(select2);
                    //总监点评详情
                    detail_comment(select1, dic);
                    
                    #endregion
                   
                    //字典集合添加字典
                    dicList.Add(dic);

                }

                //数据包（json格式）
                jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = dicList };
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

        /// <summary>
        /// 点评详情
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="dic"></param>
        private static void detail_comment(List<comment> s_comment_list, Dictionary<string, object> dic)
        {
            try
            {
                dic.Add("s_comment_count", s_comment_list.Count);
                //点评统计
                Dictionary<string, int> dic_comment = new Dictionary<string, int>();
                foreach (var item in s_comment_list)
                {
                    string date = ((DateTime)item.com_createdate).ToString("yyyy年MM月dd日") + "--点评人："+ item.com_username ;
                    if (dic_comment.ContainsKey(date))
                    {
                        dic_comment[date] += 1;
                    }
                    else
                    {
                        dic_comment.Add(date, 1);
                    }
                }
                //
                foreach (var item in dic_comment)
                {
                    if (dic.ContainsKey("comment"))
                    {
                        dic["comment"] += "," + item.Key + "  " + item.Value + "次";
                    }
                    else
                    {
                        dic.Add("comment", item.Key + "  " + item.Value + "次");
                    }
                }
                //无新增点评
                if (!dic.ContainsKey("comment"))
                {
                    dic.Add("comment", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 工作总结详情
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="dic"></param>
        private static void detail_workreport(List<workreport> s_workreport_list, Dictionary<string, object> dic)
        {
            try
            {
                dic.Add("s_workreport_count", s_workreport_list.Count);
                //报告统计
                Dictionary<string, int> dic_workreport = new Dictionary<string, int>();
                foreach (var item in s_workreport_list)
                {
                    string date = ((DateTime)item.report_createdate).ToString("yyyy年MM月dd日");
                    if (dic_workreport.ContainsKey(date))
                    {
                        dic_workreport[date] += 1;
                    }
                    else
                    {
                        dic_workreport.Add(date, 1);
                    }
                }
                //
                foreach (var item in dic_workreport)
                {
                    if (dic.ContainsKey("workreport"))
                    {
                        dic["workreport"] += "," + item.Key + "  " + item.Value + "个";
                    }
                    else
                    {
                        dic.Add("workreport", item.Key + "  " + item.Value + "个");
                    }
                }
                //无新增报告
                if (!dic.ContainsKey("workreport"))
                {
                    dic.Add("workreport", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 工作计划详情
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="dic"></param>
        private static void detail_workplane(List<workplan> s_workplan_list, Dictionary<string, object> dic)
        {
            try
            {
                dic.Add("s_workplan_count", s_workplan_list.Count);
                //工作计划统计
                Dictionary<string, int> dic_workplan = new Dictionary<string, int>();
                foreach (var item in s_workplan_list)
                {
                    string date = ((DateTime)item.wp_createdate).ToString("yyyy年MM月dd日");
                    if (dic_workplan.ContainsKey(date))
                    {
                        dic_workplan[date] += 1;
                    }
                    else
                    {
                        dic_workplan.Add(date, 1);
                    }
                }
                //
                foreach (var item in dic_workplan)
                {
                    if (dic.ContainsKey("workplan"))
                    {
                        dic["workplan"] += "," + item.Key + "  " + item.Value + "个";
                    }
                    else
                    {
                        dic.Add("workplan", item.Key + "  " + item.Value + "个");
                    }
                }
                //无新增工作计划
                if (!dic.ContainsKey("workplan"))
                {
                    dic.Add("workplan", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 签到详情
        /// </summary>
        /// <param name="s_sign_list"></param>
        /// <param name="dic"></param>
        private static void detail_sign(List<sign_in> s_sign_list, Dictionary<string, object> dic)
        {
            try
            {
                dic.Add("s_sign_count", s_sign_list.Count);
                //签到统计
                Dictionary<string, int> dic_sign = new Dictionary<string, int>();
                foreach (var item in s_sign_list)
                {
                    string date = ((DateTime)item.sign_date).ToString("yyyy年MM月dd日");
                    if (dic_sign.ContainsKey(date))
                    {
                        dic_sign[date] += 1;
                    }
                    else
                    {
                        dic_sign.Add(date, 1);
                    }
                }
                //
                foreach (var item in dic_sign)
                {
                    if (dic.ContainsKey("sign_in"))
                    {
                        dic["sign_in"] += "," + item.Key + "  " + item.Value + "次";
                    }
                    else
                    {
                        dic.Add("sign_in", item.Key + "  " + item.Value + "次");
                    }
                }
                //无新增签到
                if (!dic.ContainsKey("sign_in"))
                {
                    dic.Add("sign_in", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 跟进详情
        /// </summary>
        /// <param name="s_followup_list"></param>
        /// <param name="dic"></param>
        private static void detail_follow(List<follow_up> s_followup_list, Dictionary<string, object> dic)
        {
            try
            {
                dic.Add("s_followup_count", s_followup_list.Count);
                //跟进统计
                Dictionary<string, int> dic_fllow = new Dictionary<string, int>();
                foreach (var item in s_followup_list)
                {
                    string date = ((DateTime)item.follow_date).ToString("yyyy年MM月dd日");
                    if (dic_fllow.ContainsKey(date))
                    {
                        dic_fllow[date] += 1;
                    }
                    else
                    {
                        dic_fllow.Add(date, 1);
                    }
                }
                foreach (var item in dic_fllow)
                {
                    if (dic.ContainsKey("baifang"))
                    {
                        dic["baifang"] += "," + item.Key + "  " + item.Value + "次";
                    }
                    else
                    {
                        dic.Add("baifang", item.Key + "  " + item.Value + "次");
                    }
                }
                //无新增跟进
                if (!dic.ContainsKey("baifang"))
                {
                    dic.Add("baifang", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 联系人详情
        /// </summary>
        /// <param name="s_linkman_list"></param>
        /// <param name="dic"></param>
        private static void detail_linkman(List<cust_linkman> s_linkman_list, Dictionary<string, object> dic)
        {
            try
            {
                //联系人统计
                dic.Add("s_linkman_count", s_linkman_list.Count);
                foreach (var item in s_linkman_list)
                {
                    if (dic.ContainsKey("linkname"))
                    {
                        dic["linkname"] += "," + item.link_name;
                    }
                    else
                    {
                        dic.Add("linkname", item.link_name);
                    }
                }
                //无新增联系人
                if (!dic.ContainsKey("linkname"))
                {
                    dic.Add("linkname", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 客户详情
        /// </summary>
        /// <param name="s_cust_customer_list"></param>
        /// <param name="dic"></param>
        private static void detail_customer(List<cust_customer> s_cust_customer_list, Dictionary<string, object> dic)
        {

            try
            {
                //客户统计
                dic.Add("s_cust_customer_count", s_cust_customer_list.Count);
                foreach (var item in s_cust_customer_list)
                {
                    if (dic.ContainsKey("cust_customer"))
                    {
                        dic["cust_customer"] += "," + item.cust_name;
                    }
                    else
                    {
                        dic.Add("cust_customer", item.cust_name);
                    }
                }
                //无新增客户
                if (!dic.ContainsKey("cust_customer"))
                {
                    dic.Add("cust_customer", "");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 获取下属成员

        /// <summary>
        /// 获取下属成员
        /// </summary>
        public void GetMemmber(HttpContext context, string guid)
        {
            try
            {
                List<UserInfo> UserInfo_list = null;
                //取出它的成员
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    Admin_CS admin_cs = Constant.dicLimit_P[guid];
                    LimitType limitType = Constant.dicLimit_P[guid].LimitType;

                    switch (limitType)
                    {
                        case LimitType.Super_Admin:
                            UserInfo_list = Constant.List_All_RelateUserInfo;
                            break;
                        case LimitType.Common_Admin:
                            UserInfo_list = new List<UserInfo>();
                            foreach (var item in (admin_cs.List_Uni_UserName))
                            {
                                if (item.UniqueNo != guid)
                                {
                                    UserInfo_list.Add(item);
                                }
                            }
                            //集合转换

                            break;

                        default:
                            break;
                    }
                }

                //返回数据
                jsonModel = Constant.get_jsonmodel(0, "success", UserInfo_list);
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

        /// <summary>
        /// 获取本部门所有成员
        /// </summary>
        public void Get_All_Memmber(HttpContext context, string guid)
        {
            try
            {
                List<UserInfo> UserInfo_list = null;
                //取出它的成员
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    Admin_CS admin_cs = Constant.dicLimit_P[guid];
                    LimitType limitType = Constant.dicLimit_P[guid].LimitType;

                    switch (limitType)
                    {
                        case LimitType.Super_Admin:
                            UserInfo_list = Constant.dic_All_user;
                            break;
                        case LimitType.Common_Admin:
                            UserInfo_list = new List<UserInfo>();
                            foreach (var item in (admin_cs.List_Uni_UserName))
                            {
                                UserInfo_list.Add(item);
                            }
                            //集合转换

                            break;

                        default:
                            break;
                    }
                }

                //返回数据
                jsonModel = Constant.get_jsonmodel(0, "success", UserInfo_list);
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

        #region 强制刷新组织机构


        /// <summary>
        ///强制刷新组织机构
        /// </summary>
        public static void Orgization_Reflesh()
        {
            try
            {
                Constant.Get_all_user();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 获取部门

        /// <summary>
        /// 超级管理员使用
        /// </summary>
        /// <param name="context"></param>
        public void GetDepartMent(HttpContext context)
        {
            HttpRequest Request = context.Request;

            try
            {
                if (Constant.DepartMent_List.Count > 0)
                {
                    jsonModel = Constant.get_jsonmodel(0, "success", Constant.DepartMent_List);
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(0, "failed", "未获取到部门信息");
                }
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        /// 获取自己的部门
        /// </summary>
        public void Get_Self_DepartMent(HttpContext context)
        {

            HttpRequest Request = context.Request;
            string guid = RequestHelper.string_transfer(Request, "guid");
            try
            {
                if (Constant.DepartMent_List.Count > 0)
                {
                    DepartMent self_dp = Constant.DepartMent_List.FirstOrDefault(d => d.Leader_Guid == guid);
                    if (self_dp != null)
                    {
                        jsonModel = Constant.get_jsonmodel(0, "success", self_dp);
                    }
                    else
                    {
                        //配合前端测试
                        DepartMent department = new DepartMent() { Name = "部门" };
                        //jsonModel = Constant.get_jsonmodel(3, "failed", "未获取到该部门的信息");
                        jsonModel = Constant.get_jsonmodel(0, "success", department);
                    }

                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(3, "failed", "未获取到部门信息");
                }
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }



        #endregion

        #region 获取部门成员

        //public void GetDepartMent_Memmber(HttpContext context)
        //{
        //    HttpRequest Request = context.Request;

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        jsonModel = Constant.ErrorGetData(ex);
        //    }
        //    finally
        //    {
        //        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
        //    }
        //}

        #endregion

        #region 辅助方法

        /// <summary>
        /// 辅助方法【linq 分页】
        /// </summary>
        /// <param name="lstPerson"></param>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetPageByLinq(List<Dictionary<string, object>> listDic, int pageIndex, int PageSize)
        {
            List<Dictionary<string, object>> result = null;
            try
            {
                result = listDic.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList<Dictionary<string, object>>();
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

        #region PC使用


        /// <summary>
        ///获取销售简报（详细，按当前周的每一天进行统计）
        /// </summary>
        public void get_statistic_detail_PC(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            //获取当前人身份         
            try
            {
                Dictionary<string, object> diclist_All = new Dictionary<string, object>();


                List<Dictionary<string, string>> diclistinfo = new List<Dictionary<string, string>>();

                DateTime select_date = RequestHelper.DateTime_transfer(Request, "select_date");

                DateTime dt1 = select_date.AddDays((Convert.ToInt32(select_date.DayOfWeek) - 1) * (-1));
                DateTime dt2 = dt1.AddDays(1);
                DateTime dt3 = dt2.AddDays(1);
                DateTime dt4 = dt3.AddDays(1);
                DateTime dt5 = dt4.AddDays(1);
                DateTime dt6 = dt5.AddDays(1);
                DateTime dt7 = dt6.AddDays(1);

                //获取从星期一到星期日的所有数据【已进行筛选】
                var select_1 = get_statistic_detail_PC_Helper(guid, dt1, dt1.AddHours(24));
                var select_2 = get_statistic_detail_PC_Helper(guid, dt2, dt2.AddHours(24));
                var select_3 = get_statistic_detail_PC_Helper(guid, dt3, dt3.AddHours(24));
                var select_4 = get_statistic_detail_PC_Helper(guid, dt4, dt4.AddHours(24));
                var select_5 = get_statistic_detail_PC_Helper(guid, dt5, dt5.AddHours(24));
                var select_6 = get_statistic_detail_PC_Helper(guid, dt6, dt6.AddHours(24));
                var select_7 = get_statistic_detail_PC_Helper(guid, dt7, dt7.AddHours(24));

                //周一的各项统计数据
                Dictionary<string, string> diclist1 = new Dictionary<string, string>();
                //加入周一签到次数
                diclist1.Add("Sign_in_List", select_1.Sign_in_List.Count.ToString());
                //加入周一的跟进数量
                diclist1.Add("Follow_up_List", select_1.Follow_up_List.Count.ToString());
                //加入周一的点评数量
                diclist1.Add("Comment_List", select_1.Comment_List.Count.ToString());
                //加入周一的新增客户数量
                diclist1.Add("Cust_customer_List", select_1.Cust_customer_List.Count.ToString());
                //加入周一的新增联系人数量
                diclist1.Add("Cust_linkman_List", select_1.Cust_linkman_List.Count.ToString());
                //加入周一的工作计划
                diclist1.Add("Workplan_List", select_1.Workplan_List.Count.ToString());
                //加入周一的工作报告
                diclist1.Add("Workreport_List", select_1.Workreport_List.Count.ToString());
                diclist1.Add("Date", "周一(" + dt1.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist1);

                //周二的各项统计数据
                Dictionary<string, string> diclist2 = new Dictionary<string, string>();
                //加入周二签到次数
                diclist2.Add("Sign_in_List", select_2.Sign_in_List.Count.ToString());
                //加入周二的跟进数量
                diclist2.Add("Follow_up_List", select_2.Follow_up_List.Count.ToString());
                //加入周二的点评数量
                diclist2.Add("Comment_List", select_2.Comment_List.Count.ToString());
                //加入周二的新增客户数量
                diclist2.Add("Cust_customer_List", select_2.Cust_customer_List.Count.ToString());
                //加入周二的新增联系人数量
                diclist2.Add("Cust_linkman_List", select_2.Cust_linkman_List.Count.ToString());
                //加入周二的工作计划
                diclist2.Add("Workplan_List", select_2.Workplan_List.Count.ToString());
                //加入周二的工作报告
                diclist2.Add("Workreport_List", select_2.Workreport_List.Count.ToString());
                diclist2.Add("Date", "周二(" + dt2.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist2);

                //周三的各项统计数据
                Dictionary<string, string> diclist3 = new Dictionary<string, string>();
                //加入周三签到次数
                diclist3.Add("Sign_in_List", select_3.Sign_in_List.Count.ToString());
                //加入周三的跟进数量
                diclist3.Add("Follow_up_List", select_3.Follow_up_List.Count.ToString());
                //加入周三的点评数量
                diclist3.Add("Comment_List", select_3.Comment_List.Count.ToString());
                //加入周三的新增客户数量
                diclist3.Add("Cust_customer_List", select_3.Cust_customer_List.Count.ToString());
                //加入周三的新增联系人数量
                diclist3.Add("Cust_linkman_List", select_3.Cust_linkman_List.Count.ToString());
                //加入周三的工作计划
                diclist3.Add("Workplan_List", select_3.Workplan_List.Count.ToString());
                //加入周三的工作报告
                diclist3.Add("Workreport_List", select_3.Workreport_List.Count.ToString());
                diclist3.Add("Date", "周三(" + dt3.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist3);

                //周四的各项统计数据
                Dictionary<string, string> diclist4 = new Dictionary<string, string>();
                //加入周四签到次数
                diclist4.Add("Sign_in_List", select_4.Sign_in_List.Count.ToString());
                //加入周四的跟进数量
                diclist4.Add("Follow_up_List", select_4.Follow_up_List.Count.ToString());
                //加入周四的点评数量
                diclist4.Add("Comment_List", select_4.Comment_List.Count.ToString());
                //加入周四的新增客户数量
                diclist4.Add("Cust_customer_List", select_4.Cust_customer_List.Count.ToString());
                //加入周四的新增联系人数量
                diclist4.Add("Cust_linkman_List", select_4.Cust_linkman_List.Count.ToString());
                //加入周四的工作计划
                diclist4.Add("Workplan_List", select_4.Workplan_List.Count.ToString());
                //加入周四的工作报告
                diclist4.Add("Workreport_List", select_4.Workreport_List.Count.ToString());
                diclist4.Add("Date", "周四(" + dt4.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist4);

                //周五的各项统计数据
                Dictionary<string, string> diclist5 = new Dictionary<string, string>();
                //加入周五签到次数
                diclist5.Add("Sign_in_List", select_5.Sign_in_List.Count.ToString());
                //加入周五的跟进数量
                diclist5.Add("Follow_up_List", select_5.Follow_up_List.Count.ToString());
                //加入周五的点评数量
                diclist5.Add("Comment_List", select_5.Comment_List.Count.ToString());
                //加入周五的新增客户数量
                diclist5.Add("Cust_customer_List", select_5.Cust_customer_List.Count.ToString());
                //加入周五的新增联系人数量
                diclist5.Add("Cust_linkman_List", select_5.Cust_linkman_List.Count.ToString());
                //加入周五的工作计划
                diclist5.Add("Workplan_List", select_5.Workplan_List.Count.ToString());
                //加入周五的工作报告
                diclist5.Add("Workreport_List", select_5.Workreport_List.Count.ToString());
                diclist5.Add("Date", "周五(" + dt5.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist5);

                //周六的各项统计数据
                Dictionary<string, string> diclist6 = new Dictionary<string, string>();
                //加入周六签到次数
                diclist6.Add("Sign_in_List", select_6.Sign_in_List.Count.ToString());
                //加入周六的跟进数量
                diclist6.Add("Follow_up_List", select_6.Follow_up_List.Count.ToString());
                //加入周六的点评数量
                diclist6.Add("Comment_List", select_6.Comment_List.Count.ToString());
                //加入周六的新增客户数量
                diclist6.Add("Cust_customer_List", select_6.Cust_customer_List.Count.ToString());
                //加入周六的新增联系人数量
                diclist6.Add("Cust_linkman_List", select_6.Cust_linkman_List.Count.ToString());
                //加入周六的工作计划
                diclist6.Add("Workplan_List", select_6.Workplan_List.Count.ToString());
                //加入周六的工作报告
                diclist6.Add("Workreport_List", select_6.Workreport_List.Count.ToString());
                diclist6.Add("Date", "周六(" + dt6.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist6);

                //周日的各项统计数据
                Dictionary<string, string> diclist7 = new Dictionary<string, string>();
                //加入周一签到次数
                diclist7.Add("Sign_in_List", select_7.Sign_in_List.Count.ToString());
                //加入周一的跟进数量
                diclist7.Add("Follow_up_List", select_7.Follow_up_List.Count.ToString());
                //加入周一的点评数量
                diclist7.Add("Comment_List", select_7.Comment_List.Count.ToString());
                //加入周一的新增客户数量
                diclist7.Add("Cust_customer_List", select_7.Cust_customer_List.Count.ToString());
                //加入周一的新增联系人数量
                diclist7.Add("Cust_linkman_List", select_7.Cust_linkman_List.Count.ToString());
                //加入周一的工作计划
                diclist7.Add("Workplan_List", select_7.Workplan_List.Count.ToString());
                //加入周一的工作报告
                diclist7.Add("Workreport_List", select_7.Workreport_List.Count.ToString());
                diclist7.Add("Date", "周日(" + dt7.ToString("yyyy-MM-dd") + ")");
                diclistinfo.Add(diclist7);





                //diclist.Add(dt1.ToString("yyyy-MM-dd"), select_1);
                //diclist.Add(dt2.ToString("yyyy-MM-dd"), select_2);
                //diclist.Add(dt3.ToString("yyyy-MM-dd"), select_3);
                //diclist.Add(dt4.ToString("yyyy-MM-dd"), select_4);
                //diclist.Add(dt5.ToString("yyyy-MM-dd"), select_5);
                //diclist.Add(dt6.ToString("yyyy-MM-dd"), select_6);
                //diclist.Add(dt7.ToString("yyyy-MM-dd"), select_7);


                //一周的签到数量
                int sign_count = select_1.Sign_in_List.Count + select_2.Sign_in_List.Count + select_3.Sign_in_List.Count + select_4.Sign_in_List.Count + select_5.Sign_in_List.Count
                    + select_6.Sign_in_List.Count + select_7.Sign_in_List.Count;
                //一周的跟进数量
                int follow_count = select_1.Follow_up_List.Count + select_2.Follow_up_List.Count + select_3.Follow_up_List.Count + select_4.Follow_up_List.Count + select_5.Follow_up_List.Count
                    + select_6.Follow_up_List.Count + select_7.Follow_up_List.Count;
                //一周的点评数量
                int comment_count = select_1.Comment_List.Count + select_2.Comment_List.Count + select_3.Comment_List.Count + select_4.Comment_List.Count + select_5.Comment_List.Count
                    + select_6.Comment_List.Count + select_7.Comment_List.Count;
                //一周的新增客户数量
                int cust_count = select_1.Cust_customer_List.Count + select_2.Cust_customer_List.Count + select_3.Cust_customer_List.Count + select_4.Cust_customer_List.Count + select_5.Cust_customer_List.Count
                    + select_6.Cust_customer_List.Count + select_7.Cust_customer_List.Count;
                //一周的新增联系人数量
                int linkman_count = select_1.Cust_linkman_List.Count + select_2.Cust_linkman_List.Count + select_3.Cust_linkman_List.Count + select_4.Cust_linkman_List.Count + select_5.Cust_linkman_List.Count
                    + select_6.Cust_linkman_List.Count + select_7.Cust_linkman_List.Count;
                //一周的工作计划
                int workplan_count = select_1.Workplan_List.Count + select_2.Workplan_List.Count + select_3.Workplan_List.Count + select_4.Workplan_List.Count + select_5.Workplan_List.Count
                    + select_6.Workplan_List.Count + select_7.Workplan_List.Count;
                //一周的工作报告
                int workreport_count = select_1.Workreport_List.Count + select_2.Workreport_List.Count + select_3.Workreport_List.Count + select_4.Workreport_List.Count + select_5.Workreport_List.Count
                    + select_6.Workreport_List.Count + select_7.Workreport_List.Count;

                diclist_All.Add("sign_count", sign_count);
                diclist_All.Add("follow_count", follow_count);
                diclist_All.Add("comment_count", comment_count);
                diclist_All.Add("cust_count", cust_count);
                diclist_All.Add("linkman_count", linkman_count);
                diclist_All.Add("workplan_count", workplan_count);
                diclist_All.Add("workreport_count", workreport_count);


                diclist_All.Add("all_list", diclistinfo);

                jsonModel = Constant.get_jsonmodel(0, "success", diclist_All);
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }



        /// <summary>
        ///统计详情获取辅助
        /// </summary>
        public static Data_T get_statistic_detail_PC_Helper(string guid, DateTime start_D, DateTime end_D)
        {
            Data_T dt = new Data_T();
            try
            {

                //签到列表
                var select = (from _sign_in in sign_in_handle.list_All
                              where _sign_in.sign_userid == guid && _sign_in.sign_date >= start_D && _sign_in.sign_date <= end_D
                              select _sign_in).ToList();
                dt.Sign_in_List = select;
                //跟进列表
                var select2 = (from _follow_up in follow_up_handle.list_All
                               where _follow_up.follow_userid == guid && _follow_up.follow_date >= start_D && _follow_up.follow_date <= end_D
                               select _follow_up).ToList();
                dt.Follow_up_List = select2;
                //点评列表
                var select3 = (from _comment in comment_handle.list_All
                               where _comment.com_userid == guid && _comment.com_createdate >= start_D && _comment.com_createdate <= end_D
                               select _comment).ToList();
                dt.Comment_List = select3;

                //客户列表
                var select4 = (from _cust_customer in cust_customer_handle.list_All
                               where _cust_customer.cust_users.Contains(guid) && _cust_customer.cust_createdate >= start_D && _cust_customer.cust_createdate <= end_D
                               select _cust_customer).ToList();
                dt.Cust_customer_List = select4;

                //联系人列表
                var select5 = (from _cust_linkman in cust_linkman_handle.list_All
                               where _cust_linkman.link_users.Contains(guid) && _cust_linkman.link_createdate >= start_D && _cust_linkman.link_createdate <= end_D
                               select _cust_linkman).ToList();
                dt.Cust_linkman_List = select5;
                //工作计划列表
                var select6 = (from _workplan in workplan_handle.list_All
                               where _workplan.wp_userid == guid && _workplan.wp_createdate >= start_D && _workplan.wp_createdate <= end_D
                               select _workplan).ToList();
                dt.Workplan_List = select6;
                //工作报告列表
                var select7 = (from _workreport in workreport_handle.list_All
                               where _workreport.report_userid == guid && _workreport.report_createdate >= start_D && _workreport.report_createdate <= end_D
                               select _workreport).ToList();
                dt.Workreport_List = select7;


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {

            }
            return dt;
        }

        #endregion

    }

    #region 辅助类

    [Serializable]
    public class Data_T
    {
        public List<sign_in> Sign_in_List = new List<sign_in>();

        public List<follow_up> Follow_up_List = new List<follow_up>();

        public List<comment> Comment_List = new List<comment>();

        public List<cust_customer> Cust_customer_List = new List<cust_customer>();

        public List<cust_linkman> Cust_linkman_List = new List<cust_linkman>();

        public List<workplan> Workplan_List = new List<workplan>();

        public List<workreport> Workreport_List = new List<workreport>();
    }


    public class RetData
    {
        /// <summary>
        /// 1
        /// </summary>
        public string rowNum { get; set; }
        /// <summary>
        /// 1673
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 6D2E7E84-F257-49C2-9931-EF70BD477115
        /// </summary>
        public string UniqueNo { get; set; }
        /// <summary>
        /// 3
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 韩亚培
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 韩亚培
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 1
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 18254157367
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 2017/1/17 0:00:00
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// hanyapei
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// e10adc3949ba59abbe56e057f20f883e
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 130990199309091145
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HeadPic { get; set; }
        /// <summary>
        /// 1000
        /// </summary>
        public string RegisterOrg { get; set; }
        /// <summary>
        /// 2
        /// </summary>
        public string AuthenType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CreateUID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EditUID { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string IsEnable { get; set; }
        /// <summary>
        /// 0
        /// </summary>
        public string IsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CheckMsg { get; set; }
        /// <summary>
        /// http://222.35.226.155:8080/
        /// </summary>
        public string AbsHeadPic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
    }

    public class OrgData
    {
        /// <summary>
        /// 00000000000000000X
        /// </summary>
        public string UniqueNo { get; set; }
        /// <summary>
        /// 超级管理员
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RoleName { get; set; }
    }

    public class Result
    {
        /// <summary>
        /// RetData
        /// </summary>
        public List<RetData> retData { get; set; }
        /// <summary>
        /// OrgData
        /// </summary>
        public List<OrgData> orgData { get; set; }
        /// <summary>
        /// success
        /// </summary>
        public string errMsg { get; set; }
        /// <summary>
        /// ErrNum
        /// </summary>
        public int errNum { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string status { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// Result
        /// </summary>
        public Result result { get; set; }
    }

    /// <summary>
    /// 部门信息
    /// </summary>
    public class DepartMent
    {
        /// <summary>
        /// 部门内用户列表
        /// </summary>
        public List<UserInfo> UserInfo_List = new List<UserInfo>();

        /// <summary>
        /// 部门ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门ID一样的认定为属于同一部门
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is DepartMent)
            {
                DepartMent dp = obj as DepartMent;
                if (dp.ID == this.ID)
                {
                    result = true;
                }
            }

            return result;
        }

        public string Leader_Guid { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        public string UniqueNo { get; set; }

        public string Name { get; set; }

        public string RoleName { get; set; }

        public string RegisterOrg { get; set; }
    }

    /// <summary>
    /// 管理者【子级用户】
    /// </summary>
    public class Admin_CS
    {
        /// <summary>
        /// 组成员
        /// </summary>
        public List<string> List_Memmber = new List<string>();

        /// <summary>
        /// 组成员名称
        /// </summary>
        public List<UserInfo> List_Uni_UserName = new List<UserInfo>();

        /// <summary>
        /// 类型
        /// </summary>
        public LimitType LimitType { get; set; }
    }

    /// <summary>
    /// 权限级别
    /// </summary>
    public enum LimitType
    {
        Super_Admin,
        Common_Admin,
        Common_Memmber
    }

    #endregion
}