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
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Handler.Statistical;
using CRM_Handler.Share;
using CRM_Handler.Custom;
using CRM_Handler.LinkMan;
using CRM_Handler.Follow;
using CRM_Handler.SiginIn;
using CRM_Handler.Common;

namespace CRM_Handler.Report
{
    /// <summary>
    /// workreport_handle 的摘要说明
    /// </summary>
    public class workreport_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///工作报告群
        /// </summary>
        public static List<workreport> list_All = null;

        /// <summary>
        /// 指定某个用户的工作报告设置
        /// </summary>
        public static Dictionary<string, List<workreport>> dic_Self = new Dictionary<string, List<workreport>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.report;

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
                //当前用户ID
                string guid = RequestHelper.string_transfer(Request, "guid");

                if (Data_check_helper.check_Self(handertype, guid))
                {
                    switch (func)
                    {
                        case "get_workreport_list":
                            get_workreport_list(context, guid);
                            break;
                        case "get_report_tongji_time":
                            get_report_tongji_time(context, guid);
                            break;
                        case "edit_workreport":
                            edit_workreport(context, guid);
                            break;
                        case "workreport_info":
                            workreport_info(context, guid);
                            break;
                        case "get_report_tongji_type":
                            get_report_tongji_type(context, guid);
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
                jsonModel = Constant.ErrorGetData(ex);
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 获取报告列表【guid】

        /// <summary>
        /// 获取报告列表
        /// </summary>
        /// <param name="context"></param>
        public void get_workreport_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");

                int type = RequestHelper.int_transfer(Request, "type");
                string report_reader = RequestHelper.string_transfer(Request, "report_reader");
                int report_type = RequestHelper.int_transfer(Request, "report_type");

                //用户ID【指定自己下属的】
                string report_userid = RequestHelper.string_transfer(Request, "report_userid");

                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");

                //筛选报告
                var workreports = select_report_list(guid, type, report_type, report_userid);

                workreports = Check_And_Get_List_dep(departmentID, memmberID, workreports);

                int workreport_count = workreports.Count();

                //进行分页
                List<workreport> list_workreport = GetPageByLinq(workreports.ToList(), page_Index, page_Size);
                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = ConverList<workreport>.ListToDic(list_workreport);

                foreach (var item in dicList)
                {
                    item["report_startdate"] = Convert.ToDateTime(item["report_startdate"]).ToString("yyyy-MM-dd");
                    item["report_enddate"] = Convert.ToDateTime(item["report_enddate"]).ToString("yyyy-MM-dd");
                    item["report_createdate"] = Convert.ToDateTime(item["report_createdate"]).ToString("yyyy-MM-dd");
                }

                //返回数据
                PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = workreport_count };
                //数据库包（json格式）              
                jsonModel = Constant.get_jsonmodel(0, "success", psd);
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                //管理者  若为1 代表当前人为管理员  若为0则为普通员工
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = "IsAdmin";
                }
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<workreport> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<workreport> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.report_userid == memmberID
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
                                      where UniqueNo_string.Contains(w.report_userid)
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
        /// 筛选报告
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="type"></param>
        /// <param name="report_type"></param>
        /// <param name="report_userid"></param>
        /// <param name="list1"></param>
        /// <returns></returns>
        private static IEnumerable<workreport> select_report_list(string guid, int type, int report_type, string report_userid)
        {
            IEnumerable<workreport> workreports = null;
            try
            {
                //工作报告,当前用户
                workreports = from t in dic_Self[guid] select t;
                //我的报告
                if (type == 1)
                {
                    //我提交的
                    workreports = (from t in workreports
                                   where t.report_userid == guid && t.report_type == report_type
                                   orderby t.report_createdate descending
                                   select t);
                }
                //我的下属，当选择不限时，不包括自己的报告
                else if (type == 2)
                {
                    //是否筛选
                    if (!string.IsNullOrEmpty(report_userid))
                    {
                        //别人提交给我的
                        workreports = (from t in workreports
                                       where t.report_userid == report_userid && t.report_type == report_type
                                       orderby t.report_createdate descending
                                       select t);
                    }
                    else
                    {
                        //不限时，不包括自己的
                        workreports = (from t in workreports
                                       where t.report_userid != guid && t.report_type == report_type
                                       orderby t.report_createdate descending
                                       select t);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return workreports;
        }

        #endregion

        #region 工作报告详情【ID  guid】

        /// <summary>
        /// /工作报告详情【分享圈也使用】
        /// </summary>
        /// <param name="context"></param>
        public void workreport_info(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                //指定一个报告
                workreport workreport = (from t in list_All
                                         where t.id == id
                                         select t).FirstOrDefault();
                if (workreport != null)
                {
                    Dictionary<string, object> dic_workreport = ConverList<workreport>.T_ToDic(workreport);

                    //添加点评
                    add_comment(workreport, dic_workreport);

                    //添加图片
                    add_img(workreport, dic_workreport);

                    jsonModel = Constant.get_jsonmodel(0, "success", dic_workreport);
                }

            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                //若为1 代表当前人为管理员  若为0则为普通员工
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = "IsAdmin";
                }
                string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
                context.Response.Write("{\"result\":" + jsonString + "}");
            }
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="workreport"></param>
        /// <param name="dic_workreport"></param>
        private static void add_img(workreport workreport, Dictionary<string, object> dic_workreport)
        {
            try
            {
                string t_pic = string.Empty;
                string m_pic = string.Empty;

                string remark1 = "1";
                //t.com_type  （2为点评【工作报告】  1为评论【跟进记录】）
                List<picture> pictureList = (from t in Constant.list_picture_All
                                             where workreport.id == t.pic_table_id && t.pic_remark == remark1
                                             select t).ToList();
                int t_count = pictureList.Count;
                for (int i = 0; i < t_count; i++)
                {
                    if (i == t_count - 1)
                    {
                        t_pic += pictureList[i].pic_url;
                    }
                    else
                    {
                        t_pic += pictureList[i].pic_url + ",";
                    }
                }
                dic_workreport.Add("t_pic", t_pic);

                string remark2 = "2";
                List<picture> m_pictureList = (from t in Constant.list_picture_All
                                               where workreport.id == t.pic_table_id && t.pic_remark == remark2
                                               select t).ToList();
                int m_count = m_pictureList.Count;
                for (int i = 0; i < m_count; i++)
                {
                    if (i == m_count - 1)
                    {
                        m_pic += m_pictureList[i].pic_url;
                    }
                    else
                    {
                        m_pic += m_pictureList[i].pic_url + ",";
                    }
                }
                dic_workreport.Add("m_pic", m_pic);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 添加点评
        /// </summary>
        /// <param name="list1"></param>
        /// <param name="dic_list"></param>
        private static void add_comment(workreport workreport, Dictionary<string, object> dic_workreport)
        {
            try
            {
                string commentArray = string.Empty;
                //t.com_type  （2为点评【工作报告】  1为评论【跟进记录】）
                List<comment> commentList = (from t in comment_handle.list_All
                                             where workreport.id == t.com_table_id && t.com_type == "2"
                                             select t).ToList();
                for (int i = 0; i < commentList.Count; i++)
                {
                    if (i == commentList.Count - 1)
                    {
                        commentArray += commentList[i].com_username + "：" + commentList[i].com_content;
                    }
                    else
                    {
                        commentArray += commentList[i].com_username + "：" + commentList[i].com_content + ",";
                    }
                }
                dic_workreport.Add("dianping", commentArray);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 新增报告【guid】

        /// <summary>
        /// 新增报告
        /// </summary>
        /// <param name="context"></param>
        public void edit_workreport(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");

            try
            {
                workreport workreport = new workreport();
                workreport.id = Convert.ToInt32(id);
                workreport.report_type = RequestHelper.int_transfer(Request, "report_type");
                workreport.report_startdate = RequestHelper.DateTime_transfer(Request, "report_startdate");
                workreport.report_enddate = RequestHelper.DateTime_transfer(Request, "report_enddate").AddDays(1);
                workreport.report_content = RequestHelper.string_transfer(Request, "report_content");
                workreport.report_plan = RequestHelper.string_transfer(Request, "report_plan");
                workreport.report_reader = RequestHelper.string_transfer(Request, "report_reader_name");
                workreport.report_sender = RequestHelper.string_transfer(Request, "report_sender_name");

                workreport.report_remark = "新增工作报告";

                string t_picture = RequestHelper.string_transfer(Request, "t_picture");
                string m_picture = RequestHelper.string_transfer(Request, "m_picture");

                //添加关联的信息【新增客户、新增联系人、跟进记录、签到记录】
                relate_other_herlper(guid, workreport);

                //修改《暂时修改功能》
                if (id > 0)
                {
                    //修改工作报告
                    edit_report_helper(id, guid, workreport, t_picture, m_picture);
                }
                else if (id == 0)
                {
                    workreport.report_userid = RequestHelper.string_transfer(Request, "report_userid");
                    workreport.report_username = RequestHelper.string_transfer(Request, "report_username");
                    workreport.report_isdelete = "0";
                    workreport.report_createdate = DateTime.Now;
                    workreport.report_updatedate = DateTime.Now;
                    workreport.report_status = "0";
                    //添加工作报告
                    add_report_helper(guid, workreport, t_picture, m_picture);
                }

            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        /// 添加关联的信息【新增客户、新增联系人、跟进记录、签到记录】
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="workreport"></param>
        private static void relate_other_herlper(string guid, workreport workreport)
        {
            //关联的新增客户
            List<cust_customer> list_cust_customer = cust_customer_handle.dic_Self[guid].Where(item => item.cust_users.Contains(guid) && item.cust_createdate > workreport.report_startdate && item.cust_createdate < workreport.report_enddate).ToList<cust_customer>();
            for (int i = 0; i < list_cust_customer.Count; i++)
            {
                workreport.report_cust_customer_array += list_cust_customer[i].id + ",";
                //去除最后一个字符
                if (i == list_cust_customer.Count - 1)
                {
                    workreport.report_cust_customer_array = workreport.report_cust_customer_array.Substring(0, workreport.report_cust_customer_array.Length - 1);
                }
            }

            //关联的新增联系人
            List<cust_linkman> list_cust_linkman = cust_linkman_handle.dic_Self[guid].Where(item => item.link_users.Contains(guid) && item.link_createdate > workreport.report_startdate && item.link_createdate < workreport.report_enddate).ToList<cust_linkman>();
            for (int i = 0; i < list_cust_linkman.Count; i++)
            {
                workreport.report_cust_linkman_array += list_cust_linkman[i].id + ",";
                if (i == list_cust_linkman.Count - 1)
                {
                    workreport.report_cust_linkman_array = workreport.report_cust_linkman_array.Substring(0, workreport.report_cust_linkman_array.Length - 1);
                }
            }

            //关联的跟进记录
            List<follow_up> list_follow_up = follow_up_handle.dic_Self[guid].Where(item => item.follow_userid == guid && item.follow_date > workreport.report_startdate && item.follow_date < workreport.report_enddate).ToList<follow_up>();
            for (int i = 0; i < list_follow_up.Count; i++)
            {
                workreport.report_follow_up_array += list_follow_up[i].id + ",";
                if (i == list_follow_up.Count - 1)
                {
                    workreport.report_follow_up_array = workreport.report_follow_up_array.Substring(0, workreport.report_follow_up_array.Length - 1);
                }
            }

            //关联的签到
            List<sign_in> list_sign_in = sign_in_handle.dic_Self[guid].Where(item => item.sign_userid == guid && item.sign_date > workreport.report_startdate && item.sign_date < workreport.report_enddate).ToList<sign_in>();
            for (int i = 0; i < list_sign_in.Count; i++)
            {
                workreport.report_sign_in_array += list_sign_in[i].id + ",";
                if (i == list_sign_in.Count - 1)
                {
                    workreport.report_sign_in_array = workreport.report_sign_in_array.Substring(0, workreport.report_sign_in_array.Length - 1);
                }
            }
        }

        /// <summary>
        /// 添加工作报告辅助
        /// </summary>
        /// <param name="request"></param>
        /// <param name="guid"></param>
        /// <param name="workreport"></param>
        /// <param name="table_id"></param>
        /// <returns></returns>
        private void add_report_helper(string guid, workreport workreport, string t_picture, string m_picture)
        {
            //缓存添加客户
            dic_Self[guid].Add(workreport);

            jsonModel = Constant.get_jsonmodel(0, "success", 1);
            new Thread(() =>
            {
                try
                {
                    #region 通知领导进行添加

                    //通知领导我已添加用户
                    if (Constant.dic_custs_users.ContainsKey(guid))
                    {
                        if (!list_All.Contains(workreport))
                        {
                            //当前添加客户
                            list_All.Add(workreport);
                        }

                        //获取上级的guid
                        List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
                        //上级列表
                        foreach (var item in commonAdmin_CustursID)
                        {
                            //若领导在线，添加当前添加的用户
                            if (dic_Self.ContainsKey(item))
                            {
                                //报告列表,当前跟进
                                List<workreport> workreport_admins = dic_Self[item];
                                if (!workreport_admins.Contains(workreport))
                                {
                                    workreport_admins.Add(workreport);
                                }
                            }
                        }
                    }

                    #endregion

                    jsonModel = Constant.bbc.add_workreport(workreport, t_picture, m_picture);
                    long id = Convert.ToInt64(jsonModel.retData);

                    workreport.id = id;

                    //插入图片
                    workport_picture_add(workreport, t_picture, m_picture);
                    //抄送人
                    //if ((request["report_reader"] != null) && request["report_sender"] != null)
                    //{
                    //    Constant.bbc.edit_report_sender(table_id, request["report_reader"].ToString(), request["report_sender"].ToString());
                    //}
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }) { }.Start();
        }

        /// <summary>
        /// 工作计划图片添加
        /// </summary>
        /// <param name="workreport"></param>
        /// <param name="t_picture"></param>
        /// <param name="m_picture"></param>
        private void workport_picture_add(workreport workreport, string t_picture, string m_picture)
        {
            try
            {

                //今日总结的图片
                Constant.list_picture_All.Add(new picture()
                {
                    pic_en_table = "workreport",
                    pic_cn_table = "工作报告",
                    pic_table_id = (int)workreport.id,
                    pic_url = t_picture,
                    pic_createdate = DateTime.Now,
                    pic_creator = 1,
                    pic_updatedate = DateTime.Now,
                    pic_updateuser = 1,
                    pic_isdelete = "0",
                    pic_remark = "1",
                });
                //明日计划的图片
                Constant.list_picture_All.Add(new picture()
                {
                    pic_en_table = "workreport",
                    pic_cn_table = "工作报告",
                    pic_table_id = (int)workreport.id,
                    pic_url = m_picture,
                    pic_createdate = DateTime.Now,
                    pic_creator = 1,
                    pic_updatedate = DateTime.Now,
                    pic_updateuser = 1,
                    pic_isdelete = "0",
                    pic_remark = "2",
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 修改辅助
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Model"></param>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <param name="workreport"></param>
        /// <param name="table_id"></param>
        /// <returns></returns>
        private void edit_report_helper(long id, string guid, workreport _newworkreport, string t_picture, string m_picture)
        {
            try
            {
                //工作报告,当前用户
                List<workreport> workreport_selfs = dic_Self[guid];
                workreport workreport = workreport_selfs.FirstOrDefault(item => item.id == id);
                if (workreport != null)
                {
                    workreport.report_type = _newworkreport.report_type;
                    workreport.report_startdate = _newworkreport.report_startdate;
                    workreport.report_enddate = _newworkreport.report_enddate;
                    workreport.report_plan = _newworkreport.report_plan;
                    workreport.report_isdelete = _newworkreport.report_isdelete;
                    workreport.report_remark = _newworkreport.report_remark;
                    //成功提示              
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);
                    //开启线程操作数据库
                    new Thread(() =>
                    {

                        Constant.bbc.edit_workreport(workreport, t_picture, m_picture);
                        //今日总结的图片
                        picture edit_t_picture = Constant.list_picture_All.FirstOrDefault(item => item.pic_table_id == id);
                        if (edit_t_picture != null)
                        {
                            edit_t_picture.pic_url = t_picture;
                        }
                        //明日计划的图片
                        picture edit_m_picture = Constant.list_picture_All.FirstOrDefault(item => item.pic_table_id == id);
                        if (edit_m_picture != null)
                        {
                            edit_m_picture.pic_url = m_picture;
                        }

                        //抄送人
                        //if ((request["report_reader"] != null) && request["report_sender"] != null)
                        //{
                        //    Constant.bbc.edit_report_sender(id, request["report_reader"].ToString(), request["report_sender"].ToString());
                        //}
                    }) { }.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 获取工作报告统计【日、周、月】 根据id 串获取

        /// <summary>
        /// 今日统计
        /// </summary>
        /// <param name="context"></param>
        public void get_report_tongji_type(HttpContext context, string guid)
        {
            try
            {
                HttpRequest Request = context.Request;

                //客户
                string report_cust_customer_array = RequestHelper.string_transfer(Request, "report_cust_customer_array");
                //联系人
                string report_cust_linkman_array = RequestHelper.string_transfer(Request, "report_cust_linkman_array");
                //跟进
                string report_follow_up_array = RequestHelper.string_transfer(Request, "report_follow_up_array");
                //签到
                string report_sign_in_array = RequestHelper.string_transfer(Request, "report_sign_in_array");

                //新增客户             
                List<long> list_cust_long = list_string_to_list_long(report_cust_customer_array);
                //新增联系人                
                List<long> list_link_long = list_string_to_list_long(report_cust_linkman_array);
                //跟进              
                List<long> list_follow_long = list_string_to_list_long(report_follow_up_array);
                //签到
                List<long> list_sign_long = list_string_to_list_long(report_sign_in_array);

                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
                //普通员工
                UserInfo userInfo = Constant.dic_All_user.Where(item => item.UniqueNo == guid).FirstOrDefault();

                if (userInfo != null)
                {
                    //新增客户数量
                    List<cust_customer> s_cust_customer_list = cust_customer_handle.list_All.Where(child => list_cust_long.Contains((long)child.id)).ToList<cust_customer>();

                    //新增联系人数量
                    List<cust_linkman> s_linkman_list = cust_linkman_handle.list_All.Where(child => list_link_long.Contains((long)child.id)).ToList<cust_linkman>();
                    //跟进数量
                    List<follow_up> s_followup_list = follow_up_handle.list_All.Where(child => list_follow_long.Contains((long)child.id)).ToList<follow_up>();
                    //签到数量
                    List<sign_in> s_sign_list = sign_in_handle.list_All.Where(child => list_sign_long.Contains((long)child.id)).ToList<sign_in>();

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("s_users", userInfo.UniqueNo);
                    dic.Add("s_username", userInfo.Name);

                    //客户统计
                    customer_tongji(s_cust_customer_list, dic);
                    //联系人统计
                    link_man_tongji(s_linkman_list, dic);
                    //跟进统计
                    follow_tongji(s_followup_list, dic);
                    //签到统计
                    sign_tongji(s_sign_list, dic);
                    //字典集合添加字典
                    dicList.Add(dic);

                }

                //数据包（json格式）              
                jsonModel = Constant.get_jsonmodel(0, "success", dicList);
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
        /// 签到统计
        /// </summary>
        /// <param name="s_sign_list"></param>
        /// <param name="dic"></param>
        private static void sign_tongji(List<sign_in> s_sign_list, Dictionary<string, object> dic)
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
                    dic["sign_in"] += "," + item.Key + " " + item.Value + "次";
                }
                else
                {
                    dic.Add("sign_in", item.Key + " " + item.Value + "次");
                }
            }
            //无新增签到
            if (!dic.ContainsKey("sign_in"))
            {
                dic.Add("sign_in", "");
            }
        }

        /// <summary>
        /// 跟进统计
        /// </summary>
        /// <param name="s_followup_list"></param>
        /// <param name="dic"></param>
        private static void follow_tongji(List<follow_up> s_followup_list, Dictionary<string, object> dic)
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
                        dic["baifang"] += "," + item.Key + " " + item.Value + "次";
                    }
                    else
                    {
                        dic.Add("baifang", item.Key + " " + item.Value + "次");
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
        /// 联系人统计
        /// </summary>
        /// <param name="s_linkman_list"></param>
        /// <param name="dic"></param>
        private static void link_man_tongji(List<cust_linkman> s_linkman_list, Dictionary<string, object> dic)
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
        /// 客户统计
        /// </summary>
        /// <param name="s_cust_customer_list"></param>
        /// <param name="dic"></param>
        private static void customer_tongji(List<cust_customer> s_cust_customer_list, Dictionary<string, object> dic)
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

        /// <summary>
        /// string 转List<long>
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static List<long> list_string_to_list_long(string array)
        {
            List<long> long_list = new List<long>();
            try
            {
                if (!string.IsNullOrEmpty(array) && array != "undefined")
                {
                    //签到数量
                    List<string> list = array.Split(new char[] { ',' }).ToList<string>();
                    long_list = new List<long>(list.Select(x => long.Parse(x)));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return long_list;
        }

        #endregion

        #region 报告统计【添加时使用】

        /// <summary>
        /// 报告统计【添加时使用】
        /// </summary>
        /// <param name="context"></param>
        public void get_report_tongji_time(HttpContext context, string guid)
        {
            try
            {
                HttpRequest Request = context.Request;

                string username = RequestHelper.string_transfer(Request, "username");
                //1 为本日 2 为本周 3为本月
                string type = RequestHelper.string_transfer(Request, "type");

                //客户数量
                int cust_count = 0;
                //联系人数量
                int link_count = 0;
                //新增跟进
                int follow_count = 0;
                //拜访次数
                int sigin_count = 0;

                //开始日期
                DateTime d_start = RequestHelper.DateTime_transfer(Request, "s_startdate");
                //结束日期
                DateTime d_end = RequestHelper.DateTime_transfer(Request, "s_enddate").AddDays(1);

                Dictionary<string, int> diclist = new Dictionary<string, int>();
                if (d_start <= d_end)
                {
                    //客户列表,当前用户
                    List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                    //客户数量
                    cust_count = cust_customer_selfs.Count(item => item.cust_users.Contains(guid) && Convert.ToDateTime(item.cust_createdate) >= d_start && Convert.ToDateTime(item.cust_createdate) < d_end);
                    //联系人列表,当前用户
                    List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                    //联系人数量
                    link_count = cust_linkman_selfs.Count(item => item.link_users.Contains(guid) && Convert.ToDateTime(item.link_createdate) > d_start && Convert.ToDateTime(item.link_createdate) < d_end);
                    //跟进列表
                    List<follow_up> follow_selfs = follow_up_handle.dic_Self[guid];
                    //新增跟进
                    follow_count = follow_selfs.Count(item => item.follow_userid == guid && Convert.ToDateTime(item.follow_date) >= d_start && Convert.ToDateTime(item.follow_date) < d_end);
                    //签到
                    List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];
                    //拜访次数
                    sigin_count = sign_in_selfs.Count(item => item.sign_userid == guid && Convert.ToDateTime(item.sign_date) >= d_start && Convert.ToDateTime(item.sign_date) < d_end);



                    diclist.Add("s_cust_customer_count", cust_count);
                    diclist.Add("s_linkman_count", link_count);
                    diclist.Add("s_followup_count", follow_count);
                    diclist.Add("s_sign_count", sigin_count);
                }

                jsonModel = Constant.get_jsonmodel(0, "success", diclist);
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
        public static List<workreport> GetPageByLinq(List<workreport> lstPerson, int pageIndex, int PageSize)
        {
            List<workreport> result = null;
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