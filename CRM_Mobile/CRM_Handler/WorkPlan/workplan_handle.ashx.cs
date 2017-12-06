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
using CRM_Handler.LinkMan;
using CRM_Handler.Custom;
using CRM_Handler.Statistical;
using CRM_Handler.Common;

namespace CRM_Handler.WorkPlan
{
    /// <summary>
    /// workplan_handle 工作计划的摘要说明
    /// </summary>
    public class workplan_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        /// 工作计划集合
        /// </summary>
        public static List<workplan> list_All = null;

        /// <summary>
        /// 指定某个用户的工作计划
        /// </summary>
        public static Dictionary<string, List<workplan>> dic_Self = new Dictionary<string, List<workplan>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.workplane;

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
                        case "get_workplan_today":
                            get_workplan_today(context, guid);
                            break;
                        case "get_workplan_list":
                            get_workplan_list(context, guid);
                            break;
                        case "get_workplan_info":
                            get_workplan_info(context, guid);
                            break;
                        case "edit_workplan":
                            edit_workplan(context, guid);
                            break;
                        case "update_workplan_isopen":
                            update_workplan_isopen(context, guid);
                            break;
                        case "update_workplan_isdelete":
                            update_workplan_isdelete(context, guid);
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

        #region 获取工作计划【通过ID获取 guid】

        public void get_workplan_info(HttpContext context, string guid)
        {
            /// <summary>
            /// 返回的数据
            /// </summary>
            Dictionary<string, object> dic_info = new Dictionary<string, object>();

            HttpRequest Request = context.Request;
            //获取工作计划ID
            string id = RequestHelper.string_transfer(Request, "id");

            int ind_id = -1;
            bool hasID = int.TryParse(id, out ind_id);

            try
            {
                //获取指定的图片【类型 和ID】
                List<picture> listp = new List<picture>();

                //工作计划,当前用户
                List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                //指定一个工作计划
                var workplaneQuery = (from t in workplan_selfs
                                      where t.id == ind_id
                                      select new
                                      {
                                          dic_info = ConverList<workplan>.T_ToDic(t),
                                      }
                             );
                //获取指定图片
                var picQuery = (from t in Constant.list_picture_All
                                where t.pic_en_table == "workplan" && t.pic_table_id == ind_id
                                select new
                                {
                                    img = t,
                                }
                               );

                if (workplaneQuery.Count() > 0)
                {
                    //指定工作计划数据
                    dic_info = workplaneQuery.ElementAt(0).dic_info;
                    string wp_endplandate = Convert.ToString(dic_info["wp_endplandate"]);
                    string wp_reminddate = Convert.ToString(dic_info["wp_reminddate"]);

                    dic_info["wp_endplandate"] = string.IsNullOrEmpty(wp_endplandate) ? "1800-01-01 00:00:00.000" : wp_endplandate;
                    dic_info["wp_reminddate"] = string.IsNullOrEmpty(wp_reminddate) ? "1800-01-01 00:00:00.000" : wp_reminddate;

                    string pic = "";
                    foreach (var item in picQuery)
                    {
                        pic += item.img.pic_url + ',';
                    }
                    //通过这种方式移除最后一个逗号
                    dic_info["pic"] = pic.Trim(',');

                    jsonModel = Constant.get_jsonmodel(0, "success", dic_info);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel) + "}"));
            }
        }

        #endregion

        #region 获取今日工作计划（不分页）【guid】

        /// <summary>
        /// 获取今日工作计划（不分页）
        /// </summary>
        /// <param name="context"></param>
        public void get_workplan_today(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                DateTime now = DateTime.Now;
                //工作计划,当前用户
                List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                //指定今日的工作计划
                List<workplan> list_workplan = (from t in workplan_selfs
                                                where (Convert.ToDateTime(t.wp_plandate)).Year == now.Year && (Convert.ToDateTime(t.wp_plandate)).DayOfYear == now.DayOfYear
                                                select t).ToList();
                List<Dictionary<string, object>> dic_workplan = ConverList<workplan>.ListToDic(list_workplan);
                //数据对应
                foreach (var item in dic_workplan)
                {
                    DateTime datetime_P = Convert.ToDateTime(item["wp_plandate"]);
                    int pc = datetime_P.Hour;

                    if (pc > 12)
                    {
                        item["wp_plandate"] = datetime_P.ToString("yyyy年MM月dd日");
                    }
                    else
                    {
                        item["wp_plandate"] = datetime_P.ToString("yyyy年MM月dd日");
                    }
                }
                jsonModel = Constant.get_jsonmodel(0, "success", dic_workplan);
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

        #region 获取工作计划（分页）【guid】

        /// <summary>
        /// 获取工作计划（分页）
        /// </summary>
        /// <param name="context"></param>
        public void get_workplan_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //分页信息
                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");
                long wp_link_id = RequestHelper.long_transfer(Request, "wp_link_id");
                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");
                //工作计划,当前用户
                List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];

                //通过部门获取数据【或者纯粹获取某个成员的工作计划】
                workplan_selfs = Check_And_Get_List_dep(departmentID, memmberID, workplan_selfs).ToList();
                int workplan_count = workplan_selfs.Count;
                //进行分页
                List<workplan> workplans = GetPageByLinq(workplan_selfs, page_Index, page_Size);
                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = ConverList<workplan>.ListToDic(workplans);

                if (workplans.Count > 0)
                {
                    //数据对应
                    foreach (var item in dicList)
                    {
                        DateTime datetime_P = Convert.ToDateTime(item["wp_plandate"]);
                        int pc = datetime_P.Hour;

                        //工作计划具体时间   下午hh:mm   上午hh:mm
                        if (pc > 12)
                        {
                            item["wp_plandate"] = datetime_P.ToString("yyyy年 MM月dd日");
                        }
                        else
                        {
                            item["wp_plandate"] = datetime_P.ToString("yyyy年 MM月dd日");
                        }

                        //联系人列表,当前用户
                        List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                        //联系人和职位
                        cust_linkman linkman = (from t in cust_linkman_selfs
                                                where t.id == wp_link_id
                                                select t).FirstOrDefault();
                        if (linkman != null)
                        {
                            item["link_name"] = linkman.link_name;
                            item["link_position"] = linkman.link_position;
                            item["cust_name"] = linkman.link_cust_name;
                        }
                    }
                }
                //返回数据
                PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = workplan_count };
                //数据包（json格式）               
                jsonModel = Constant.get_jsonmodel(0, "success", psd);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                string status = string.Empty;
                //管理者
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = "IsAdmin";
                }
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的工作计划】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="workplan_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<workplan> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<workplan> workplan_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    workplan_selfs = (from w in workplan_selfs
                                      where memmberID == w.wp_userid
                                      select w).ToList();
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
                        workplan_selfs = (from w in workplan_selfs
                                          where UniqueNo_string.Contains(w.wp_userid)
                                          select w);
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return workplan_selfs;
        }

        #endregion

        #region 新增工作计划【guid】

        /// <summary>
        /// 新增工作计划
        /// </summary>
        /// <param name="context"></param>
        public void edit_workplan(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;

            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                workplan workplan = new workplan();
                workplan.id = id;
                workplan.wp_content = RequestHelper.string_transfer(Request, "wp_content");
                workplan.wp_plandate = RequestHelper.DateTime_transfer(Request, "wp_plandate");
                workplan.wp_endplandate = RequestHelper.DateTime_transfer(Request, "wp_endplandate");
                workplan.wp_reminddate = RequestHelper.DateTime_transfer(Request, "wp_reminddate");
                workplan.wp_status = RequestHelper.string_transfer(Request, "wp_status");

                string picture = RequestHelper.string_transfer(Request, "picture");
                //修改《暂时修改功能》
                if (id > 0)
                {
                    //编辑工作计划
                    edit_workplane(guid, id, workplan, picture);
                }
                else if (id == 0)
                {
                    workplan.wp_userid = RequestHelper.string_transfer(Request, "wp_userid");
                    workplan.wp_username = RequestHelper.string_transfer(Request, "wp_username");
                    workplan.wp_isdelete = "0";
                    workplan.wp_createdate = DateTime.Now;
                    workplan.wp_updatedate = DateTime.Now;
                    if (!dic_Self[guid].Contains(workplan))
                    {
                        //缓存添加工作计划
                        dic_Self[guid].Add(workplan);
                        jsonModel = Constant.get_jsonmodel(0, "success", 1);
                        //开启线程操作数据库
                        new Thread(() =>
                        {
                            //添加工作计划
                            add_workplane(guid, workplan, picture);

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

        /// <summary>
        /// 添加工作计划
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="Request"></param>
        /// <param name="workplan"></param>
        /// <param name="picture"></param>
        private void add_workplane(string guid, workplan workplan, string picture)
        {
            try
            {
                //通知领导进行添加
                admin_add_workplane(guid, workplan);

                jsonModel = Constant.bbc.add_workplan(workplan, picture);
                workplan.id = Convert.ToInt32(jsonModel.retData);

                if (picture != "")
                {
                    string[] pictures = picture.Split(',');
                    for (int i = 0; i < pictures.Length; i++)
                    {
                        Constant.list_picture_All.Add(new picture()
                        {
                            pic_en_table = "workplan",
                            pic_cn_table = "工作计划",
                            pic_table_id = Convert.ToInt32(workplan.id),
                            pic_url = pictures[i],
                            pic_createdate = DateTime.Now,
                            pic_creator = 1,
                            pic_updatedate = DateTime.Now,
                            pic_updateuser = 1,
                            pic_isdelete = "0",
                            pic_remark = "新增工作计划",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 通知领导
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="workplan"></param>
        private static void admin_add_workplane(string guid, workplan workplan)
        {
            try
            {
                //通知领导我已添加用户
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    if (!list_All.Contains(workplan))
                    {
                        //当前添加工作计划
                        list_All.Add(workplan);
                    }

                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，添加当前添加的用户
                        if (dic_Self.ContainsKey(item))
                        {
                            //工作计划,当前用户
                            List<workplan> workplan_selfs = dic_Self[item];
                            if (!workplan_selfs.Contains(workplan))
                            {
                                workplan_selfs.Add(workplan);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 编辑工作计划
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <param name="workplan"></param>
        /// <param name="picture"></param>
        private void edit_workplane(string guid, long id, workplan workplan, string picture)
        {
            try
            {
                //工作计划,当前用户
                List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                workplan edit_workplan = workplan_selfs.Where(item => item.id == id).FirstOrDefault();
                if (edit_workplan != null)
                {
                    edit_workplan.wp_content = workplan.wp_content;
                    edit_workplan.wp_plandate = workplan.wp_plandate;
                    edit_workplan.wp_endplandate = workplan.wp_endplandate;
                    edit_workplan.wp_reminddate = workplan.wp_reminddate;
                    edit_workplan.wp_status = workplan.wp_status;
                    //成功提示                           
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        Constant.bbc.edit_workplan(edit_workplan, picture);
                        workplan.id = Convert.ToInt32(jsonModel.retData);
                        edit_workplane_picture(id, picture);
                    }) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 编辑图片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="picture"></param>
        private static void edit_workplane_picture(long id, string picture)
        {
            try
            {
                List<picture> li = (from t in Constant.list_picture_All
                                    where t.pic_table_id == id
                                    select t).ToList<picture>();
                for (int i = 0; i < li.Count; i++)
                {
                    Constant.list_picture_All.Remove(li[i]);
                }

                if (picture != "")
                {
                    //picture 是以逗号分隔的，需要分个进行缓存
                    string[] pictures = picture.Split(',');
                    for (int i = 0; i < pictures.Length; i++)
                    {
                        Constant.list_picture_All.Add(new picture()
                        {
                            pic_en_table = "workplan",
                            pic_cn_table = "工作计划",
                            pic_table_id = Convert.ToInt32(id),
                            pic_url = pictures[i],
                            pic_createdate = DateTime.Now,
                            pic_creator = 1,
                            pic_updatedate = DateTime.Now,
                            pic_updateuser = 1,
                            pic_isdelete = "0",
                            pic_remark = "新增工作计划",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 更改工作计划【guid】

        public void update_workplan_isopen(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                if (id > 0)
                {
                    //工作计划,当前用户
                    List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                    workplan workplan = workplan_selfs.Where(item => item.id == id).FirstOrDefault();
                    if (workplan != null)
                    {
                        workplan.wp_status = RequestHelper.string_transfer(Request, "wp_status");
                        //成功提示                           
                        jsonModel = Constant.get_jsonmodel(0, "success", 1);
                        //开启线程操作数据库
                        new Thread(() =>
                        {
                            try
                            {
                                if (workplan != null)
                                {
                                    jsonModel = Constant.workplan_S.Update(workplan);
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

        #region 删除工作计划【guid】

        public void update_workplan_isdelete(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");

            try
            {
                if (id > 0)
                {
                    //工作计划,当前用户
                    List<workplan> workplan_selfs = workplan_handle.dic_Self[guid];
                    //删除指定的工作计划
                    workplan delete_workplan = (from t in workplan_selfs
                                                where t.id == id
                                                select t).FirstOrDefault();
                    //进行工作计划删除
                    if (delete_workplan != null)
                    {
                        //自己只能删除自己的计划
                        if (delete_workplan.wp_userid == guid)
                        {
                            if (workplan_selfs.Contains(delete_workplan))
                            {
                                //删除工作计划需要两个地方
                                workplan_selfs.Remove(delete_workplan);
                            }
                            if (list_All.Contains(delete_workplan))
                            {
                                //删除工作计划需要两个地方
                                list_All.Remove(delete_workplan);
                            }
                            jsonModel = Constant.get_jsonmodel(0, "success", 1);

                            #region 开启线程操作数据库

                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                try
                                {
                                    //通知领导进行删除
                                    admin_delete_workplane(guid, delete_workplan);
                                    delete_workplan.wp_isdelete = RequestHelper.string_transfer(Request, "wp_isdelete");
                                    Constant.workplan_S.Update(delete_workplan);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                }

                            }) { }.Start();

                            #endregion
                        }
                        else
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除其他人的工作计划");
                        }
                    }
                    else
                    {
                        jsonModel = Constant.get_jsonmodel(5, "failed", "工作计划不存在");
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

        /// <summary>
        /// 通知领导
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="delete_workreport"></param>
        private static void admin_delete_workplane(string guid, workplan delete_workreport)
        {
            try
            {
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，删除当前删除的工作计划
                        if (dic_Self.ContainsKey(item))
                        {
                            //工作计划,当前用户
                            List<workplan> workplan_admins = workplan_handle.dic_Self[item];
                            if (workplan_admins.Contains(delete_workreport))
                            {
                                workplan_admins.Remove(delete_workreport);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
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
        public static List<workplan> GetPageByLinq(List<workplan> lstPerson, int pageIndex, int PageSize)
        {
            List<workplan> result = null;
            try
            {
                List<workplan> list = lstPerson.OrderByDescending(i => i.id).ToList<workplan>();
                result = list.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
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