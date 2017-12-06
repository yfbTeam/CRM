using CRM_BLL;
using CRM_Common;
using CRM_Handler.PubParam;
using CRM_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Handler.LinkMan;
using CRM_Handler.WorkPlan;
using CRM_Handler.Follow;
using CRM_Handler.SiginIn;
using CRM_Handler.Statistical;
using CRM_Handler.Custom;
using CRM_Handler.Common;


namespace CRM_Handler.LinkMan
{
    /// <summary>
    /// cust_linkman_handle 联系人 的摘要说明
    /// </summary>
    public class cust_linkman_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        /// 所有联系人
        /// </summary>
        public static List<cust_linkman> list_All = null;

        /// <summary>
        /// 指定某个用户的联系人群
        /// </summary>
        public static Dictionary<string, List<cust_linkman>> dic_Self = new Dictionary<string, List<cust_linkman>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.linkman;

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
                        case "get_cust_linkman_list":
                            get_cust_linkman_list(context, guid);
                            break;
                        case "get_cust_linkman_info":
                            get_cust_linkman_info(context, guid);
                            break;
                        case "edit_cust_linkman":
                            edit_cust_linkman(context, guid);
                            break;

                        case "get_cust_linkman_detail":
                            get_cust_linkman_detail(context, guid);
                            break;

                        case "update_cust_linkman_isdelete":
                            update_cust_linkman_isdelete(context, guid);
                            break;
                        case "get_cust_linkman_list_ALL":
                            get_cust_linkman_list_ALL(context, guid);
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
                        case "get_cust_linkman_list_ALL":
                            get_cust_linkman_list_ALL(context, guid);
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

        #region 删除联系人【guid】

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="context"></param>
        public void update_cust_linkman_isdelete(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;

            //id号
            long id_long = RequestHelper.long_transfer(Request, "id");
            try
            {
                if (id_long != 0)
                {
                    //联系人列表,当前用户
                    List<cust_linkman> cust_linkman_selfs = dic_Self[guid];
                    //获取当前用户的指定联系人
                    cust_linkman delete_linkman = cust_linkman_selfs.FirstOrDefault(item => item.id == id_long);
                    //删除指定ID的联系人
                    if (delete_linkman != null)
                    {
                        List<follow_up> follow_up_selfs = follow_up_handle.dic_Self[guid];

                        string[] has_the_Customer_users = Split_Hepler.str_to_stringss(delete_linkman.link_users);
                        if (has_the_Customer_users.Count() > 1)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了多个负责人");
                        }
                        else if (follow_up_selfs.Count(item => item.follow_link_id == id_long) > 0)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了跟进记录");
                        }

                        else
                        {
                            cust_linkman_selfs.Remove(delete_linkman);
                            //删除状态               
                            delete_linkman.link_isdelete = RequestHelper.string_transfer(Request, "link_isdelete");
                            jsonModel = Constant.get_jsonmodel(0, "success", 1);
                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                try
                                {
                                    //通知缓存的领导删除/数据库删除该联系人[进行数据库操作]
                                    admin_delete_linkman(guid, delete_linkman);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                }

                            }) { IsBackground = true }.Start();
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

        /// <summary>
        /// 通知缓存的领导删除/数据库删除该联系人
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="guid"></param>
        /// <param name="id_long"></param>
        /// <param name="delete_linkman"></param>
        private void admin_delete_linkman(string guid, cust_linkman delete_linkman)
        {
            try
            {
                //当前添加客户
                if (list_All.Contains(delete_linkman))
                {
                    list_All.Remove(delete_linkman);
                }

                string _guid = delete_linkman.link_users;
                //通知当事人进行删除  //多个负责人的联系人不让删除，在之前就已经限制
                if (dic_Self.ContainsKey(_guid))
                {
                    if (dic_Self[_guid].Contains(delete_linkman))
                    {
                        dic_Self[_guid].Remove(delete_linkman);
                    }
                }

                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];

                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，删除当前的联系人
                        if (dic_Self.ContainsKey(item))
                        {
                            //联系人列表,当前用户
                            List<cust_linkman> linkman_admins = dic_Self[item];
                            //判断领导是否存在该联系人
                            if (linkman_admins.Contains(delete_linkman))
                            {
                                //删除客户需要两个地方
                                linkman_admins.Remove(delete_linkman);
                            }
                        }
                    }
                }

                //数据库标示数据删除
                Constant.cust_linkman_S.Update(delete_linkman);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 获取联系人列表【guid】

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_linkman_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {

                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");
                //通过客户信息ID号获取对应的联系人
                long link_cust_id = RequestHelper.long_transfer(Request, "link_cust_id");
                ////指定客户ID获取联系人【获取所有联系人】
                //List<cust_linkman> list_linkman = null;

                //按联系人名称进行模糊搜索
                string link_name = RequestHelper.string_transfer(Request, "link_name");

                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");

                //联系人列表,当前用户
                var cust_linkman_selfs = from t in dic_Self[guid] select t;

                cust_linkman_selfs = Check_And_Get_List_dep(departmentID, memmberID, cust_linkman_selfs);

                if (link_cust_id > 0)
                {
                    cust_linkman_selfs = (from t in cust_linkman_selfs
                                          where t.link_cust_id == link_cust_id && t.link_name.Contains(link_name)
                                          orderby t.id descending
                                          select t);
                }
                else
                {
                    cust_linkman_selfs = (from t in cust_linkman_selfs
                                          where t.link_name.Contains(link_name)
                                          orderby t.id descending
                                          select t);

                }
                List<cust_linkman> list_linkmans = cust_linkman_selfs.ToList();
                int all_count = list_linkmans.Count;
                //是否为页面
                bool ispage = RequestHelper.bool_transfer(Request, "ispage");
                if (ispage)
                {
                    //进行分页
                    List<cust_linkman> list_linkman_page = GetPageByLinq(list_linkmans, page_Index, page_Size);
                    //对象集合转为dic集合列表
                    List<Dictionary<string, object>> dic_list_linkman_page = ConverList<cust_linkman>.ListToDic(list_linkman_page);
                    foreach (var item in dic_list_linkman_page)
                    {
                        string link_level = Convert.ToString(item["link_level"]);
                        item["link_level_name"] = LevelHelper.Getlink_level(link_level);
                    }

                    //返回数据
                    PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>()
                    {
                        PagedData = dic_list_linkman_page,
                        PageIndex = page_Index,
                        PageSize = page_Size,
                        RowCount = all_count
                    };
                    //数据库包（json格式）                     
                    jsonModel = Constant.get_jsonmodel(0, "success", psd);
                }
                else
                {
                    //数据库包（json格式）item["link_levelName"] = pub_param_handle.dic_linkMan_Grade[Convert.ToString(item["link_level"])];                      
                    jsonModel = Constant.get_jsonmodel(0, "success", list_linkmans);
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

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<cust_linkman> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<cust_linkman> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.link_users.Contains(memmberID)
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
                                      where UniqueNo_string.Contains(w.link_users)
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

        #endregion

        #region 获取联系人信息【ID  guid】

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_linkman_info(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            long id = RequestHelper.int_transfer(Request, "id");
            try
            {
                //联系人列表,当前用户
                List<cust_linkman> cust_linkman_selfs = dic_Self[guid];
                //指定的一个联系人
                cust_linkman select_linkman = (from t in cust_linkman_selfs
                                               where t.id == id
                                               select t).FirstOrDefault();
                if (select_linkman != null)
                {
                    Dictionary<string, object> item = ConverList<cust_linkman>.T_ToDic(select_linkman);
                    //联系人名称
                    string link_levelName = Convert.ToString(item["link_level"]);
                    item["link_levelName"] = LevelHelper.Getlink_level(link_levelName);
                    //生日
                    DateTime datetime_P = Convert.ToDateTime(item["link_birthday"]);
                    item["link_birthday"] = datetime_P.ToString("yyyy-MM-dd") == "1800-01-01" ? "" : datetime_P.ToString("yyyy-MM-dd");

                    jsonModel = Constant.get_jsonmodel(0, "success", item);
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

        #region 新增、编辑联系人【guid】

        /// <summary>
        /// 新增联系人
        /// </summary>
        /// <param name="context"></param>
        public void edit_cust_linkman(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                cust_linkman new_linkman = new cust_linkman();
                new_linkman.link_birthday = RequestHelper.DateTime_transfer(Request, "link_birthday");
                new_linkman.link_department = RequestHelper.string_transfer(Request, "link_department");
                new_linkman.link_email = RequestHelper.string_transfer(Request, "link_email");
                new_linkman.link_level = RequestHelper.int_transfer(Request, "link_level");
                new_linkman.link_name = RequestHelper.string_transfer(Request, "link_name");
                new_linkman.link_position = RequestHelper.string_transfer(Request, "link_position");
                new_linkman.link_remark = RequestHelper.string_transfer(Request, "link_remark");
                new_linkman.link_sex = RequestHelper.string_transfer(Request, "link_sex");
                new_linkman.link_status = RequestHelper.string_transfer(Request, "link_status");
                new_linkman.link_telephone = RequestHelper.string_transfer(Request, "link_telephone");
                new_linkman.link_phonenumber = RequestHelper.string_transfer(Request, "link_phonenumber");
                new_linkman.link_cust_id = RequestHelper.long_transfer(Request, "link_cust_id");
                new_linkman.link_cust_name = new_linkman.link_cust_id > 0 ? RequestHelper.string_transfer(Request, "link_cust_name") : string.Empty;

                //多负责人
                string cust_leaders = RequestHelper.string_transfer(Request, "cust_leaders");
                string cust_leadernames = RequestHelper.string_transfer(Request, "cust_leadernames");

                //是否为领导者
                if (string.IsNullOrEmpty(cust_leaders))
                {
                    new_linkman.link_users = RequestHelper.string_transfer(Request, "link_users");
                    new_linkman.link_usersname = RequestHelper.string_transfer(Request, "link_usersname");//用户姓名
                }
                else
                {
                    new_linkman.link_users = cust_leaders;
                    new_linkman.link_usersname = cust_leadernames;
                }

                //修改《暂时修改功能》
                if (id > 0)
                {
                    //编辑联系人
                    edit_linkman(id, guid, new_linkman, cust_leaders, cust_leadernames);
                }
                else if (id == 0)
                {
                    new_linkman.link_updatedate = DateTime.Now;
                    new_linkman.link_createdate = DateTime.Now;
                    new_linkman.link_isdelete = "0";

                    //添加联系人
                    add_linkman(guid, cust_leaders, new_linkman);
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
        /// 添加联系人
        /// </summary>
        /// <param name="Request"></param>      
        /// <param name="guid">用户id</param>            
        private void add_linkman(string guid, string cust_leaders, cust_linkman new_linkman)
        {
            try
            {
                cust_customer customer = cust_customer_handle.list_All.FirstOrDefault(t => t.cust_category == 1 && t.id == new_linkman.link_cust_id);
                if (
                    customer == null)
                {
                    //姓名、部门、手机号码、性别一致的认定为重复联系人
                    cust_linkman linkman = cust_linkman_handle.list_All.FirstOrDefault(t => !string.IsNullOrEmpty(t.link_phonenumber) && t.link_phonenumber == new_linkman.link_phonenumber

                        );
                    if (linkman == null)
                    {
                        if (!dic_Self[guid].Contains(new_linkman))
                        {
                            //缓存添加客户
                            dic_Self[guid].Add(new_linkman);
                            //不需要开启线程操作数据库
                            jsonModel = Constant.bbc.add_cust_linkman(new_linkman);
                            new_linkman.id = Convert.ToInt32(jsonModel.retData);

                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                try
                                {
                                    //通知领导进行添加
                                    admin_add_linkman(guid, new_linkman);

                                    //通知其他负责人进行添加
                                    Add_Other_Users(new_linkman, cust_leaders);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                }

                            }) { IsBackground = true }.Start();
                        }
                    }
                    else
                    {
                        jsonModel = Constant.get_jsonmodel(0, "failed", "联系人已存在，不可重复添加");
                    }
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(0, "failed", "您没有权限添加公共联系人");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 通知领导进行添加
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link"></param>
        public static void admin_add_linkman(string guid, cust_linkman new_linkman)
        {
            try
            {
                //通知领导我已添加用户
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    if (!list_All.Contains(new_linkman))
                    {
                        //当前添加客户
                        list_All.Add(new_linkman);
                    }

                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];

                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，添加当前添加的用户
                        if (dic_Self.ContainsKey(item))
                        {
                            //联系人列表,当前领导
                            List<cust_linkman> cust_linkman_admins = dic_Self[item];
                            //防止重复提交
                            if (!cust_linkman_admins.Contains(new_linkman))
                            {
                                cust_linkman_admins.Add(new_linkman);
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
        /// 编辑联系人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid">用户id</param>       
        private void edit_linkman(long id, string guid, cust_linkman new_linkman, string cust_leaders, string cust_leadernames)
        {
            try
            {
                //联系人列表,当前用户
                List<cust_linkman> cust_linkman_selfs = dic_Self[guid];
                cust_linkman edit_linkman = cust_linkman_selfs.FirstOrDefault(item => item.id == id);
                if (edit_linkman != null)
                {
                    edit_linkman.link_birthday = new_linkman.link_birthday;
                    edit_linkman.link_cust_id = new_linkman.link_cust_id;
                    edit_linkman.link_cust_name = new_linkman.link_cust_name;
                    edit_linkman.link_department = new_linkman.link_department;
                    edit_linkman.link_email = new_linkman.link_email;
                    edit_linkman.link_level = new_linkman.link_level;
                    edit_linkman.link_name = new_linkman.link_name;
                    edit_linkman.link_phonenumber = new_linkman.link_phonenumber;
                    edit_linkman.link_position = new_linkman.link_position;
                    edit_linkman.link_remark = new_linkman.link_remark;
                    edit_linkman.link_sex = new_linkman.link_sex;
                    edit_linkman.link_telephone = new_linkman.link_telephone;

                    //查看是否有多负责人的情况
                    if (!string.IsNullOrEmpty(cust_leaders) && !string.IsNullOrEmpty(cust_leadernames))
                    {
                        string[] link_Users = Split_Hepler.str_to_stringss(edit_linkman.link_users);
                        for (int k = 0; k < link_Users.Count(); k++)
                        {
                            string link_user = link_Users[k];
                            //判断该用户是否在线 然后进行移除
                            if (!Constant.dicLimit_P.ContainsKey(link_user) && dic_Self.ContainsKey(link_user))
                            {
                                if (dic_Self[link_user].Contains(edit_linkman))
                                {
                                    dic_Self[link_user].Remove(edit_linkman);
                                }
                            }
                        }

                        edit_linkman.link_users = cust_leaders;
                        edit_linkman.link_usersname = cust_leadernames;
                    }

                    //成功提示                      
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);

                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        jsonModel = Constant.bbc.edit_cust_linkman(edit_linkman);

                        //通知其他负责人进行添加
                        Add_Other_Users(edit_linkman, cust_leaders);

                    }) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }


        /// <summary>
        /// 通知其他负责人进行添加
        /// </summary>
        /// <param name="custer"></param>
        public void Add_Other_Users(cust_linkman linkman, string cust_leaders)
        {
            try
            {
                //多负责人添加                       
                string[] arri = Split_Hepler.str_to_stringss(cust_leaders);
                foreach (var guid in arri)
                {
                    if (!string.IsNullOrEmpty(guid))
                    {
                        //如果不包含则添加
                        if (dic_Self.ContainsKey(guid))
                        {
                            if (!dic_Self[guid].Contains(linkman))
                            {
                                dic_Self[guid].Add(linkman);
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

        #region 联系人详细【查看更多信息使用,导航链接】

        /// <summary>
        /// 联系人详细
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_linkman_detail(HttpContext context, string guid)
        {
            try
            {
                HttpRequest Request = context.Request;
                //主体承载
                linkman link_list = new linkman();
                long id = RequestHelper.long_transfer(Request, "id");
                string userid = RequestHelper.string_transfer(Request, "userid");
                //联系人
                cust_linkman li = linkman_info_fill(guid, link_list, id);
                if (li != null)
                {
                    //more跟进记录[2]
                    more_follow(guid, link_list, li);
                    //more签到记录[2]
                    more_sign(guid, link_list, li);
                }

                jsonModel = Constant.get_jsonmodel(0, "success", link_list);
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
        /// 获取指定联系人信息
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link_list"></param>
        /// <param name="id"></param>      
        /// <returns></returns>
        private static cust_linkman linkman_info_fill(string guid, linkman link_list, long id)
        {
            //联系人列表,当前用户
            List<cust_linkman> cust_linkman_selfs = dic_Self[guid];
            //获取指定的联系人【在自己的联系人列表里获取】
            cust_linkman li = (from t in cust_linkman_selfs
                               where t.id == id
                               select t).FirstOrDefault();
            try
            {
                if (li != null)
                {
                    //联系人展示实体
                    linkman_info cl = new linkman_info();

                    cl.customer_name = li.link_cust_name;
                    cl.link_email = li.link_email;
                    cl.link_level = li.link_level.ToString();
                    cl.link_level_name = pub_param_handle.dic_linkMan_Grade[Convert.ToString(li.link_level)];
                    cl.link_name = li.link_name;
                    cl.link_phonenumber = li.link_phonenumber;
                    cl.link_position = li.link_position;
                    cl.link_username = li.link_usersname;
                    //联系人ID
                    cl.id = Convert.ToString(li.id);
                    //联系人客户ID
                    cl.link_cust_id = Convert.ToString(li.link_cust_id);
                    link_list.cust_linkman = cl;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return li;
        }

        /// <summary>
        /// more跟进记录[2]
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link_list"></param>
        /// <param name="li"></param>
        private static void more_follow(string guid, linkman link_list, cust_linkman li)
        {
            try
            {
                //跟进记录
                List<follow_up> follow_ups_selfs = follow_up_handle.dic_Self[guid];

                //获取指定的跟进记录【在自己的跟进记录列表里获取，通过跟进时间做降序排列，只选取最上面的两条记录】
                List<follow_up> follow_ups = (from t in follow_ups_selfs
                                              where t.follow_cust_id == li.link_cust_id && t.follow_link_id == li.id
                                              orderby t.follow_date descending
                                              select t).Take(2).ToList();
                link_list.follow_up = ConverList<follow_up>.ListToDic(follow_ups);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// more签到记录[2]
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link_list"></param>
        /// <param name="li"></param>
        private static void more_sign(string guid, linkman link_list, cust_linkman li)
        {
            try
            {
                //签到列表
                List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];
                //获取指定的客户【在自己的客户列表里获取】
                List<sign_in> sign_ins = (from t in sign_in_selfs
                                          where t.sign_cust_id == li.link_cust_id
                                          orderby t.sign_date descending
                                          select t).Take(2).ToList();
                List<Dictionary<string, object>> dic_sign_ins = ConverList<sign_in>.ListToDic(sign_ins);
                foreach (var item in dic_sign_ins)
                {
                    //填入客户名称（签到）
                    item["sign_cust_name"] = li.link_cust_name;
                }
                link_list.sign = dic_sign_ins;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 辅助参数

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
        public static List<cust_linkman> GetPageByLinq(List<cust_linkman> lstPerson, int pageIndex, int PageSize)
        {
            List<cust_linkman> result = null;
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

        #region pc端使用

        public void get_cust_linkman_list_ALL(HttpContext context, string guid)
        {

            Dictionary<string, string> paramters_linkMan = pub_param_handle.dic_linkMan_Grade;
            var cust_linkman = cust_linkman_handle.list_All;
            var follow_up = follow_up_handle.list_All;
            //cust_linkman左外follow_up得到lf
            var query1 = from l in cust_linkman
                         join f in follow_up on l.id equals f.follow_link_id into gj
                         from lf in gj.DefaultIfEmpty()

                         select new
                         {

                             id = l.id,
                             link_name = l.link_name,
                             link_cust_name = l.link_cust_name,
                             link_telephone = l.link_telephone,
                             link_level = paramters_linkMan[l.link_level.ToString()],
                             link_usersname = l.link_usersname,
                             follow_date = (lf == null ? new DateTime() : lf.follow_date)
                         };
            //按follow_date分组
            var query2 = from p in query1
                         group p by p.id into allg
                         select new
                         {

                             link_name = allg.Max(p => p.link_name),
                             link_cust_name = allg.Max(p => p.link_cust_name),
                             link_telephone = allg.Max(p => p.link_telephone),
                             link_level = allg.Max(p => p.link_level),
                             link_usersname = allg.Max(p => p.link_usersname),
                             follow_date = allg.Max(p => p.follow_date)
                         };
            context.Response.Write(Constant.jss.Serialize(query2));
        }

        #endregion
    }
}