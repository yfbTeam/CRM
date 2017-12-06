using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CRM_BLL;
using System.Collections;
using CRM_Handler;
using System.Data;
using CRM_Common;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using CRM_Handler.LinkMan;
using CRM_Handler.PubParam;
using CRM_Model;
using CRM_Handler.WorkPlan;
using CRM_Handler.Follow;
using CRM_Handler.SiginIn;
using CRM_Handler.Statistical;
using CRM_Handler.Common;

namespace CRM_Handler.Custom
{
    /// <summary>
    /// cust_customer_handle 的摘要说明
    /// </summary>
    public class cust_customer_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        /// 客户集合
        /// </summary>
        public static List<cust_customer> list_All = null;

        /// <summary>
        /// 指定某个用户的客户群
        /// </summary>
        public static Dictionary<string, List<cust_customer>> dic_Self = new Dictionary<string, List<cust_customer>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.custom;

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
                        case "get_cust_customer_detail":
                            get_cust_customer_detail(context, guid);
                            break;
                        case "get_cust_customer_list":
                            get_cust_customer_list(context, guid);
                            break;
                        case "get_cust_customer_info":
                            get_cust_customer_info(context, guid);
                            break;
                        case "edit_cust_customer":
                            edit_cust_customer(context, guid);
                            break;
                        case "get_cust_customer_search":
                            get_cust_customer_search(context, guid);
                            break;
                        case "get_cust_customer_parent":
                            get_cust_customer_parent(context, guid);
                            break;
                        case "update_cust_customer_isdelete":
                            update_cust_customer_isdelete(context, guid);
                            break;
                        case "relate_customer_linkmans":
                            relate_customer_linkmans(context, guid);
                            break;
                        case "get_cust_customer_info_by_AllCustomer":
                            get_cust_customer_info_by_AllCustomer(context);
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
                        case "get_cust_customer_info_by_AllCustomer":
                            get_cust_customer_info_by_AllCustomer(context);
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

        //客户详情（组合）
        #region 《多个集合通用方法》获取客户详细(页面有多个返回集合的时候，不需要前端逐次调每一个集合的接口)【客户列表】

        /// <summary>
        /// 《多个集合通用方法》获取客户详细(页面有多个返回集合的时候，不需要前端逐次调每一个集合的接口)【客户列表】
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_detail(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                customer cust_list = new customer();
                //客户信息
                cust_customer cst = customer_info_fill(guid, id, cust_list);
                if (cst != null)
                {
                    //more联系人
                    more_linkman(guid, cust_list, cst);
                    //more跟进记录[2]
                    more_follow(guid, cust_list, cst);
                    //more签到记录
                    more_sign(guid, cust_list, cst);
                    jsonModel = Constant.get_jsonmodel(0, "success", cust_list);
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(0, "failed", "指定的客户不存在");
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
        /// <summary>
        /// 获取指定客户信息
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link_list"></param>   
        /// <returns></returns>
        private static cust_customer customer_info_fill(string guid, long id, customer cust_list)
        {
            IEnumerable<cust_customer> cust_customer_selfs = dic_Self[guid];
            //获取指定的客户【在自己的客户列表里获取】
            cust_customer cst = (from t in cust_customer_selfs
                                 where t.id == id
                                 select t).FirstOrDefault();
            try
            {
                if (cst != null)
                {
                    customer_info cust = new customer_info();
                    cust.cust_name = cst.cust_name;
                    cust.cust_level = LevelHelper.GetCustom_level((int)cst.cust_level);
                    cust.cust_followdate = Convert.ToString(cst.cust_followdate);
                    cust.cust_usersname = cst.cust_usersname;
                    cust.cust_users_id = cst.cust_users;
                    cust.cust_id = Convert.ToString(id);
                    cust_list.cust = cust;
                    cust.cust_category = (int)cst.cust_category;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return cst;
        }

        /// <summary>
        /// 更多联系人
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <param name="cust_name"></param>
        /// <param name="cust_list"></param>
        private static void more_linkman(string guid, customer cust_list, cust_customer cst)
        {
            try
            {
                List<cust_linkman> list_linkman_selfs = cust_linkman_handle.dic_Self[guid];

                //获取指定的客户【在自己的客户列表里获取】
                IEnumerable<cust_linkman> list_linkmans = (from t in list_linkman_selfs
                                                           where t.link_cust_id == cst.id
                                                           orderby t.link_createdate descending
                                                           select t);
                int count = list_linkmans.Count();
                list_linkmans = list_linkmans.Take(2);

                List<Dictionary<string, object>> list = ConvertToENU<cust_linkman>.ListToDic(list_linkmans);
                foreach (var item in list)
                {
                    //联系人等级
                    item["link_level_name"] = LevelHelper.Getlink_level(Convert.ToString(item["link_level"]));
                    //联系人客户名称
                    item["link_cust_name"] = cst.cust_name;
                }
                cust_list.linkman = list;
                cust_list.Count_All_linkman = count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// more跟进记录[2]
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="link_list"></param>
        /// <param name="li"></param>
        private static void more_follow(string guid, customer link_list, cust_customer cst)
        {
            try
            {
                //跟进记录
                List<follow_up> follow_ups_selfs = follow_up_handle.dic_Self[guid];



                //获取指定的跟进记录【在自己的跟进记录列表里获取，通过跟进时间做降序排列，只选取最上面的两条记录】
                IEnumerable<follow_up> follow_ups = (from t in follow_ups_selfs
                                                     where t.follow_cust_id == cst.id
                                                     orderby t.follow_date descending
                                                     select t);
                int count = follow_ups.Count();
                follow_ups = follow_ups.Take(2);

                link_list.follow_up = ConvertToENU<follow_up>.ListToDic(follow_ups);
                link_list.Count_All_follow = count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 更多签到
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cust_list"></param>
        /// <param name="cust"></param>
        private static void more_sign(string guid, customer cust_list, cust_customer cust)
        {
            try
            {
                //签到列表
                List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];
                //获取指定的客户【在自己的客户列表里获取】
                IEnumerable<sign_in> sign_ins = (from t in sign_in_selfs
                                                 where t.sign_cust_id == cust.id
                                                 orderby t.sign_date descending
                                                 select t);
                int count = sign_ins.Count();
                sign_ins = sign_ins.Take(2);

                List<Dictionary<string, object>> dic_sign_ins = ConvertToENU<sign_in>.ListToDic(sign_ins);
                foreach (var item in dic_sign_ins)
                {
                    //填入客户名称（签到）
                    item["sign_cust_name"] = cust.cust_name;
                }
                cust_list.sign = dic_sign_ins;
                cust_list.Count_All_sign = count;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        #endregion

        //搜索
        #region 按客户姓名搜索客户信息【搜索     guid】

        /// <summary>
        /// 按客户姓名搜索客户信息
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_search(HttpContext context, string guid)
        {

            HttpRequest Request = context.Request;
            try
            {
                string cust_name = RequestHelper.string_transfer(Request, "cust_name");
                string cust_users = RequestHelper.string_transfer(Request, "cust_users");

                string cust_category = RequestHelper.string_transfer(Request, "cust_category");

                string sign_x = RequestHelper.string_transfer(Request, "sign_x");
                string sign_y = RequestHelper.string_transfer(Request, "sign_y");
                string pageIndex = RequestHelper.string__double(Request, "PageIndex", "1");
                string pageSize = RequestHelper.string__double(Request, "PageSize", "10");
                //缓存应用
                int page_Index = Convert.ToInt32(pageIndex);
                int page_Size = Convert.ToInt32(pageSize);


                //客户列表,当前用户
                List<cust_customer> cust_customer_selfs = dic_Self[guid];

                //判断是否是获取普通用户【跟进记录目前使用这种方式】
                if (cust_category == "0")
                {
                    cust_customer_selfs = (from c in cust_customer_selfs
                                           where c.cust_category == 0
                                           select c).ToList();
                }

                //分支是因为一个纯粹通过客户名称查询，另外一个通过地址和客户名称查询
                List<cust_customer> list_cust_customer = (from t in cust_customer_selfs
                                                          where t.cust_name.Contains(cust_name)
                                                          orderby t.cust_followdate descending
                                                          select t).ToList();
                //是否分页
                bool isSign_Page = !string.IsNullOrEmpty(sign_x) ? true : false;

                if (isSign_Page)
                {
                    //进行分页
                    List<cust_customer> list_customer_page = GetPageByLinq(list_cust_customer, page_Index, page_Size);
                    //对象集合转为dic集合列表
                    List<Dictionary<string, object>> dic_customer_page = ConverList<cust_customer>.ListToDic(list_customer_page);
                    //数据返回
                    PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dic_customer_page, PageIndex = page_Index, PageSize = page_Size, RowCount = list_cust_customer.Count };
                    //数据返回
                    jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
                }
                else
                {
                    //进行分页
                    List<cust_customer> list_customer_page = GetPageByLinq(list_cust_customer, page_Index, page_Size);
                    //数据返回
                    PagedDataModel<cust_customer> psd = new PagedDataModel<cust_customer>() { PagedData = list_customer_page, PageIndex = page_Index, PageSize = page_Size, RowCount = list_cust_customer.Count };

                    //数据返回
                    jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
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

        //父级客户
        #region 获取父级客户【客户分为两级，父级和子级        guid】

        /// <summary>
        /// 获取父级客户【客户分为两级，父级和子级】
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_parent(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            Hashtable ht = new Hashtable();
            try
            {

                long _cust_parent_id = RequestHelper.long_transfer(Request, "_cust_parent_id");

                //客户列表,当前用户
                List<cust_customer> cust_customer_selfs = dic_Self[guid];
                //获取指定父ID的客户信息
                cust_customer cust_customer = (from t in cust_customer_selfs
                                               where t.cust_parent_id == _cust_parent_id
                                               select t).FirstOrDefault();
                //提示
                if (cust_customer != null)
                {
                    jsonModel = Constant.get_jsonmodel(0, "success", ConverList<cust_customer>.T_ToDic(cust_customer));
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

        //指定客户ID进行增删改查
        #region 获取客户信息【根据客户ID获取指定的客户信息     guid】

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_info(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                //客户列表,当前用户
                List<cust_customer> cust_customer_selfs = dic_Self[guid];
                //指定的一个客户
                cust_customer cust_customer = (from t in cust_customer_selfs
                                               where t.id == id
                                               select t).FirstOrDefault();
                if (cust_customer != null)
                {
                    Dictionary<string, object> dic_customer = ConverList<cust_customer>.T_ToDic(cust_customer);
                    //客户等级
                    string cust_level = Convert.ToString(dic_customer["cust_level"]);
                    //客户类型
                    string cust_type = Convert.ToString(dic_customer["cust_type"]);
                    //客户属性
                    string cust_category = Convert.ToString(dic_customer["cust_category"]);

                    dic_customer["cust_level_value"] = cust_level;
                    dic_customer["cust_type_value"] = cust_type;

                    dic_customer["cust_level"] = LevelHelper.GetCustom_level(cust_level);
                    dic_customer["cust_type"] = LevelHelper.GetCustom_Type(cust_type);

                    dic_customer["cust_category_value"] = string.IsNullOrEmpty(cust_category) ? "1" : cust_category;
                    dic_customer["cust_category"] = LevelHelper.GetCustom_Property(cust_category);

                    //联系人列表,当前用户
                    List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                    List<cust_linkman> cust_linkmans = (from t in cust_linkman_selfs
                                                        where t.link_cust_id == id
                                                        select t).ToList();
                    string cust_links = "";
                    var linkids = "";
                    foreach (cust_linkman li_item in cust_linkmans)
                    {
                        cust_links += "、" + li_item.link_name;
                        linkids += "," + li_item.id;
                    }
                    dic_customer["cust_links"] = cust_links.Trim('、');
                    dic_customer["linkids"] = linkids.Trim('、');

                    jsonModel = Constant.get_jsonmodel(0, "success", dic_customer);
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

        #region 编辑或添加客户信息（指定客户ID变更客户信息  guid）

        //public void Add_cust_Then(HttpContext context, string guid)
        //{
        //    HttpRequest Request = context.Request;
        //    HttpResponse response = context.Response;
        //    long id = RequestHelper.long_transfer(Request, "id");

        //    try
        //    {
        //        cust_customer cust = new cust_customer();
        //        //目前添加的不可以和其他的人一样（客户名称）【可以添加、允许编辑，但防止和其他人一样】
        //        cust.cust_name = RequestHelper.string_transfer(Request, "cust_name");
        //        // 填充的客户信息 已经有的客户信息不允许再提交【以客户名称作为根据】
        //        //填充的客户信息
        //        cust.cust_type = RequestHelper.int_transfer(Request, "cust_type");
        //        cust.cust_address = RequestHelper.string_transfer(Request, "cust_address");
        //        cust.cust_location = RequestHelper.string_transfer(Request, "cust_location");
        //        cust.cust_isdelete = "0";
        //        cust.cust_parent_id = RequestHelper.int_transfer(Request, "cust_parent_id");
        //        cust.cust_level = RequestHelper.int_transfer(Request, "cust_level");
        //        cust.cust_category = RequestHelper.int_transfer(Request, "cust_category");
        //        cust.cust_x = RequestHelper.decimal_transfer(Request, "cust_x");
        //        cust.cust_y = RequestHelper.decimal_transfer(Request, "cust_y");
        //        cust.cust_followdate = DateTime.Now;
        //        cust.cust_updatedate = DateTime.Now;
        //        cust.cust_updateuser = 0;
        //        string link_ids = RequestHelper.string_transfer(Request, "link_ids");

        //        //多负责人
        //        string cust_leaders = RequestHelper.string_transfer(Request, "cust_leaders");
        //        string cust_leadernames = RequestHelper.string_transfer(Request, "cust_leadernames");

        //        //修改《暂时修改功能》
        //        if (id > 0)
        //        {
        //            //编辑客户
        //            edit_customer(guid, id, cust, link_ids, cust_leaders, cust_leadernames);
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(cust_leaders))
        //            {
        //                cust.cust_usersname = RequestHelper.string_transfer(Request, "cust_usersname");
        //                cust.cust_users = RequestHelper.string_transfer(Request, "cust_users");
        //            }
        //            else
        //            {
        //                cust.cust_users = cust_leaders;
        //                cust.cust_usersname = cust_leadernames;
        //            }
        //            cust.cust_createdate = DateTime.Now;
        //            //添加客户
        //            add_customer(guid, cust, cust_leaders, link_ids);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex);
        //        jsonModel = Constant.ErrorGetData(ex);
        //    }
        //    finally
        //    {
        //        string json = Constant.jss.Serialize(jsonModel);
        //        response.Write("{\"result\":" + json + "}");
        //    }
        //}

        /// <summary>
        /// 编辑客户信息
        /// </summary>
        /// <param name="context"></param>
        public void edit_cust_customer(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");

            try
            {
                cust_customer cust = new cust_customer();
                //目前添加的不可以和其他的人一样（客户名称）【可以添加、允许编辑，但防止和其他人一样】
                cust.cust_name = RequestHelper.string_transfer(Request, "cust_name");
                // 填充的客户信息 已经有的客户信息不允许再提交【以客户名称作为根据】
                //填充的客户信息
                cust.cust_type = RequestHelper.int_transfer(Request, "cust_type");
                cust.cust_address = RequestHelper.string_transfer(Request, "cust_address");
                cust.cust_location = RequestHelper.string_transfer(Request, "cust_location");
                cust.cust_parent_id = RequestHelper.int_transfer(Request, "cust_parent_id");
                cust.cust_level = RequestHelper.int_transfer(Request, "cust_level");
                cust.cust_category = RequestHelper.int_transfer(Request, "cust_category");
                cust.cust_x = RequestHelper.decimal_transfer(Request, "cust_x");
                cust.cust_y = RequestHelper.decimal_transfer(Request, "cust_y");

                string link_ids = RequestHelper.string_transfer(Request, "link_ids");

                //多负责人
                string cust_leaders = RequestHelper.string_transfer(Request, "cust_leaders");
                string cust_leadernames = RequestHelper.string_transfer(Request, "cust_leadernames");

                if (string.IsNullOrEmpty(cust_leaders))
                {
                    cust.cust_users = RequestHelper.string_transfer(Request, "cust_users");
                    cust.cust_usersname = RequestHelper.string_transfer(Request, "cust_usersname");
                }
                else
                {
                    cust.cust_users = cust_leaders;
                    cust.cust_usersname = cust_leadernames;
                }

                //修改《暂时修改功能》
                if (id > 0)
                {
                    if (!string.IsNullOrEmpty(cust_leaders) && !string.IsNullOrEmpty(cust_leadernames))
                    {
                        link_ids = string.Empty;
                        //继承客户的联系人
                        var linkds_list = (from t in cust_linkman_handle.dic_Self[guid] where t.link_cust_id == id select Convert.ToString(t.id)).ToList();

                        foreach (var link_ID in linkds_list)
                        {
                            link_ids += link_ID + ",";
                        }
                        if (!string.IsNullOrEmpty(link_ids))
                        {
                            link_ids = link_ids.Substring(0, link_ids.Length - 1);
                        }
                    }
                    //编辑客户
                    edit_customer(guid, id, cust, link_ids, cust_leaders, cust_leadernames);
                }
                else if (id == 0)
                {
                    cust.cust_createdate = DateTime.Now;
                    cust.cust_followdate = DateTime.Now;
                    cust.cust_updatedate = DateTime.Now;
                    cust.cust_updateuser = 0;
                    cust.cust_isdelete = "0";


                    cust.cust_createdate = DateTime.Now;
                    //添加客户
                    add_customer(guid, cust, cust_leaders, link_ids);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                string json = Constant.jss.Serialize(jsonModel);
                response.Write("{\"result\":" + json + "}");
            }
        }

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cust"></param>
        /// <param name="link_ids"></param>
        private void add_customer(string guid, cust_customer cust, string cust_leaders, string link_ids)
        {
            try
            {
                //判断客户是否已经存在
                cust_customer cust_customer = (from t in list_All
                                               where t.cust_name.Trim() == cust.cust_name.Trim()
                                               select t).FirstOrDefault();
                if (cust_customer != null)
                {
                    //客户信息已存在                       
                    jsonModel = Constant.get_jsonmodel(0, "error_have_cust", cust_customer.cust_usersname);
                }
                else
                {
                    //公共属性设置字段【非管理员无法修改或添加，所有人可以使用】
                    if (cust.cust_category == 1)
                    {
                        cust.cust_users = "00000000-0000-0000-0000-00000000";
                        cust.cust_usersname = "公共客户";
                    }
                    if (!dic_Self[guid].Contains(cust))
                    {
                        //缓存添加客户
                        dic_Self[guid].Add(cust);

                        //在此进行添加，为了返回客户的ID,能够关联联系人【】【】【】【】
                        jsonModel = Constant.bbc.add_cust_customer(cust, link_ids);
                        //jsonModel = Constant.get_jsonmodel(0, "success", 1);

                        new Thread(() =>
                        {
                            try
                            {


                                //设置客户ID
                                cust.id = Convert.ToInt32(jsonModel.retData);

                                //关联联系人【先设置完客户的id ,才能进行与联系人的关联】
                                relate_linkmans(guid, cust, link_ids);

                                //通知领导进行添加
                                admin_add_customer(guid, cust);

                                //若有被指派，则进行客户的再分配
                                Add_Other_Users(cust, cust_leaders);

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
            }

        }

        /// <summary>
        /// 通知领导添加用户
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cust"></param>
        public static void admin_add_customer(string guid, cust_customer cust)
        {
            try
            {
                //当前添加客户
                if (!list_All.Contains(cust))
                {
                    list_All.Add(cust);
                }

                //管理员的添加操作不用通知普通成员   1、属于自己的客户,最高权限,  2、超管可以删别人的客户，但是不能给其他人通过这种方式添加

                //通知领导我已添加用户
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，添加当前添加的用户
                        if (dic_Self.ContainsKey(item))
                        {
                            //客户列表,当前用户
                            List<cust_customer> cust_customer_admins = dic_Self[item];
                            if (!cust_customer_admins.Contains(cust))
                            {
                                cust_customer_admins.Add(cust);
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
        /// 编辑客户信息
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="id"></param>
        /// <param name="cust"></param>
        /// <param name="link_ids"></param>
        private void edit_customer(string guid, long id, cust_customer cust, string link_ids, string cust_leaders, string cust_leadernames)
        {
            try
            {
                cust_customer edit_customer = (from t in list_All
                                               where t.id == id
                                               select t).FirstOrDefault();
                if (edit_customer != null)
                {
                    edit_customer.cust_level = cust.cust_level;
                    edit_customer.cust_name = cust.cust_name;
                    edit_customer.cust_parent_id = cust.cust_parent_id;
                    edit_customer.cust_type = cust.cust_type;
                    edit_customer.cust_category = cust.cust_category;
                    edit_customer.cust_updatedate = DateTime.Now;
                    edit_customer.cust_updateuser = 1;
                    //查看是否有多负责人的情况
                    if (!string.IsNullOrEmpty(cust_leaders) && !string.IsNullOrEmpty(cust_leadernames))
                    {
                        string[] cust_Users = Split_Hepler.str_to_stringss(edit_customer.cust_users);
                        for (int k = 0; k < cust_Users.Count(); k++)
                        {
                            string cust_user = cust_Users[k];
                            //判断该用户是否在线 然后进行移除
                            if (!Constant.dicLimit_P.ContainsKey(cust_user) && dic_Self.ContainsKey(cust_user))
                            {
                                if (dic_Self[cust_user].Contains(edit_customer))
                                {
                                    dic_Self[cust_user].Remove(edit_customer);
                                }
                            }
                        }

                        edit_customer.cust_users = cust_leaders;
                        edit_customer.cust_usersname = cust_leadernames;
                    }
                    //不需要的参数【验证】
                    edit_customer.cust_x = cust.cust_x == 0 ? edit_customer.cust_x : cust.cust_x;
                    edit_customer.cust_y = cust.cust_y == 0 ? edit_customer.cust_y : cust.cust_y;
                    edit_customer.cust_location = string.IsNullOrEmpty(cust.cust_location) ? edit_customer.cust_location : cust.cust_location;
                    edit_customer.cust_address = cust.cust_address;
                    //成功提示                           
                    jsonModel = Constant.get_jsonmodel(0, "success", edit_customer.id);
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            //关联联系人
                            relate_linkmans(guid, edit_customer, link_ids);

                            jsonModel = Constant.bbc.edit_cust_customer(edit_customer, link_ids);

                            //通知其他负责人进行添加
                            Add_Other_Users(edit_customer, cust_leaders);

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
            }

        }

        /// <summary>
        /// 关联联系人
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="cust"></param>
        /// <param name="link_ids"></param>
        private static void relate_linkmans(string guid, cust_customer cust, string link_ids)
        {
            try
            {
                //获取关联的联系人【与客户】
                long[] arri = Split_Hepler.str_to_longs(link_ids);
                //联系人列表,当前用户
                List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                List<cust_linkman> links = (from t in cust_linkman_selfs
                                            where arri.Contains((long)t.id)
                                            select t).ToList();
                if (links != null)
                {
                    //先进行移除，再进行第二步的指定
                    for (int i = 0; i < links.Count; i++)
                    {
                        //找到该联系人
                        cust_linkman man = links[i];

                        string[] links_Users = Split_Hepler.str_to_stringss(man.link_users);
                        for (int k = 0; k < links_Users.Count(); k++)
                        {
                            string link_user = links_Users[k];
                            //判断该用户是否在线 然后进行移除
                            if (!Constant.dicLimit_P.ContainsKey(link_user) && cust_linkman_handle.dic_Self.ContainsKey(link_user))
                            {
                                if (cust_linkman_handle.dic_Self[link_user].Contains(man))
                                {
                                    cust_linkman_handle.dic_Self[link_user].Remove(man);
                                }
                            }
                        }

                    }

                    //进行第二步的指定
                    foreach (cust_linkman item in links)
                    {
                        item.link_users = cust.cust_users;
                        item.link_usersname = cust.cust_usersname;

                        string[] links_Users_new = Split_Hepler.str_to_stringss(item.link_users);
                        for (int k = 0; k < links_Users_new.Count(); k++)
                        {
                            string link_user = links_Users_new[k];
                            //判断该用户是否在线 然后进行移除
                            if (!Constant.dicLimit_P.ContainsKey(link_user) && cust_linkman_handle.dic_Self.ContainsKey(link_user))
                            {
                                if (!cust_linkman_handle.dic_Self[link_user].Contains(item))
                                {
                                    cust_linkman_handle.dic_Self[link_user].Add(item);
                                }
                            }
                        }

                        Constant.cust_linkman_S.Update(item);
                    }
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
        public void Add_Other_Users(cust_customer custer, string cust_leaders)
        {
            try
            {
                //若有被指派，则进行客户的再分配
                if (!string.IsNullOrEmpty(cust_leaders))
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
                                if (!dic_Self[guid].Contains(custer))
                                {
                                    dic_Self[guid].Add(custer);
                                }
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

        #region 删除 【客户信息,指定ID     guid】

        /// <summary>
        ///删除 【客户信息】
        /// </summary>
        /// <param name="context"></param>
        public void update_cust_customer_isdelete(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                if (id > 0)
                {
                    //客户列表,当前用户
                    List<cust_customer> cust_customer_selfs = dic_Self[guid];
                    //删除指定的客户
                    cust_customer delete_customer = (from t in cust_customer_selfs
                                                     where t.id == id
                                                     select t).FirstOrDefault();
                    //进行客户删除
                    if (delete_customer != null)
                    {
                        List<cust_linkman> linkman_selfs = cust_linkman_handle.dic_Self[guid];
                        List<follow_up> follow_up_selfs = follow_up_handle.dic_Self[guid];
                        List<sign_in> sign_in_selfs = sign_in_handle.dic_Self[guid];

                        string[] has_the_Customer_users = Split_Hepler.str_to_stringss(delete_customer.cust_users);
                        //获取当前人的权限
                        LimitType limitType = Constant.Get_self_limit(guid);

                        if (delete_customer.cust_category == 1 && limitType != LimitType.Super_Admin)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除公共客户");
                        }
                        else if (has_the_Customer_users.Count() > 1)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了多个负责人");
                        }
                        //查看是否有联系人进行过关联
                        else if (linkman_selfs.Count(item => item.link_cust_id == id) > 0)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了联系人");
                        }
                        else if (follow_up_selfs.Count(item => item.follow_cust_id == id) > 0)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了跟进记录");
                        }
                        else if (sign_in_selfs.Count(item => item.sign_cust_id == id) > 0)
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "无法删除,关联了签到记录");
                        }
                        else
                        {
                            //删除客户需要两个地方
                            cust_customer_selfs.Remove(delete_customer);

                            jsonModel = Constant.get_jsonmodel(0, "success", 1);
                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                try
                                {
                                    //删除状态        
                                    delete_customer.cust_isdelete = RequestHelper.string_transfer(Request, "cust_isdelete");
                                    //通知缓存的领导删除/数据库删除该客户[进行数据库操作]
                                    admin_delete_customer(guid, delete_customer);
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
        /// 通知缓存的领导删除/数据库删除该客户[进行数据库操作]
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="delete_customer"></param>
        private static void admin_delete_customer(string guid, cust_customer delete_customer)
        {
            try
            {
                //删除当前客户
                if (list_All.Contains(delete_customer))
                {
                    list_All.Remove(delete_customer);
                }

                string _guid = delete_customer.cust_users;
                //通知当事人进行删除  //多个负责人的客户不让删除，在之前就已经限制
                if (dic_Self.ContainsKey(_guid))
                {
                    if (dic_Self[_guid].Contains(delete_customer))
                    {
                        dic_Self[_guid].Remove(delete_customer);
                    }
                }


                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，删除指定的用户
                        if (dic_Self.ContainsKey(item) && item != guid)
                        {
                            //联系人列表,当前用户
                            List<cust_customer> customer_admins = dic_Self[item];
                            //判断领导是否存在该联系人
                            if (customer_admins.Contains(delete_customer))
                            {
                                //删除客户需要两个地方
                                customer_admins.Remove(delete_customer);
                            }
                        }
                    }
                }
                Constant.cust_customer_S.Update(delete_customer);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        #endregion

        //客户列表
        #region  客户列表【率属于当前用户的客户       guid】


        /// <summary>
        /// 获取客户集合（带分页）备注：具体返回字段不全，只是为了测试
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //分页信息
                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");
                string cust_name = RequestHelper.string_transfer(Request, "cust_name").Trim();

                string type = RequestHelper.string_transfer(Request, "type").Trim();

                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");
                //客户列表,当前用户   
                var cust_customer_selfs = (from t in dic_Self[guid]
                                           where t.cust_name.Contains(cust_name)
                                           select t);

                cust_customer_selfs = Check_And_Get_List_dep(departmentID, memmberID, cust_customer_selfs);
                switch (type)
                {
                    case "pri":
                        cust_customer_selfs = (from t in cust_customer_selfs
                                               where t.cust_category != null && t.cust_category != 1
                                               orderby t.cust_followdate descending
                                               select t);
                        break;

                    case "pub":
                        cust_customer_selfs = (from t in cust_customer_selfs
                                               where t.cust_category != null && t.cust_category == 1
                                               orderby t.cust_followdate descending
                                               select t);
                        break;
                    default:
                        break;
                }

                List<cust_customer> list_customers = cust_customer_selfs.ToList();
                int all_count = list_customers.Count;
                list_customers = GetPageByLinq(list_customers, page_Index, page_Size);
                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dic_cust_customers = ConverList<cust_customer>.ListToDic(list_customers);
                //数据对应
                foreach (var item in dic_cust_customers)
                {

                    //客户级别
                    item["cust_level"] = LevelHelper.GetCustom_level(Convert.ToString(item["cust_level"]));
                    DateTime lastFollowDate = Convert.ToDateTime(item["cust_followdate"]);
                    if (DateTime.Now.Year == lastFollowDate.Year && DateTime.Now.DayOfYear == lastFollowDate.DayOfYear)
                    {
                        item["cust_followdate"] = "今天联系";
                    }
                    else
                    {
                        item["cust_followdate"] = ((DateTime.Now - lastFollowDate).Days + 1) + "天未联系";
                    }
                }
                //返回数据
                PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dic_cust_customers, PageIndex = page_Index, PageSize = page_Size, RowCount = all_count };
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
                string result = Constant.jss.Serialize(jsonModel);
                context.Response.Write("{\"result\":" + result + "}");
            }
        }


        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<cust_customer> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<cust_customer> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.cust_users.Contains(memmberID)
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
                                      where UniqueNo_string.Contains(w.cust_users)
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

        #region 指派给其他人客户和联系人

        /// <summary>
        /// 指派给其他人客户和联系人
        /// </summary>
        public void relate_customer_linkmans(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            //客户ID
            long cust_id = RequestHelper.long_transfer(Request, "cust_id");
            //联系人IDS
            string linkman_ids = RequestHelper.string_transfer(Request, "linkman_ids");
            //用户名称【可能是列表】
            string user_names = RequestHelper.string_transfer(Request, "usernames");
            //用户ids
            string user_ids = RequestHelper.string_transfer(Request, "user_ids");
            //之前d用户id
            string before_select_user_ids = RequestHelper.string_transfer(Request, "before_select_user_ids");
            try
            {
                //获取指定客户
                cust_customer select_customer = cust_customer_handle.dic_Self[guid].FirstOrDefault(t => t.id == cust_id);
                if (select_customer != null)
                {
                    //更改客户的责任人
                    bool is_sussessed = update_customer_user_ids(select_customer, user_names, user_ids);
                    if (is_sussessed)
                    {
                        //分割转换
                        long[] linkman_ids_longs = Split_Hepler.str_to_longs(linkman_ids);
                        //更改联系人的负责人
                        bool reuslt = update_linkman_user_ids(guid, linkman_ids_longs, user_names, user_ids);
                        if (reuslt)
                        {
                            jsonModel = Constant.get_jsonmodel(3, "success", 1);
                        }
                        else
                        {
                            jsonModel = Constant.get_jsonmodel(3, "success", "联系人指派失败");
                        }
                    }
                    else
                    {
                        jsonModel = Constant.get_jsonmodel(0, "failed", "客户指派失败");
                    }
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(3, "failed", "指定的客户不存在");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                string result = Constant.jss.Serialize(jsonModel);
                context.Response.Write("{\"result\":" + result + "}");
            }
        }

        /// <summary>
        ///更改客户的责任人
        /// </summary>
        public static bool update_customer_user_ids(cust_customer select_customer, string user_names, string user_ids)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(user_names) && !string.IsNullOrEmpty(user_ids))
                {
                    //取出之前的id通知当前人进行删除
                    string before_cust_users = select_customer.cust_users;

                    select_customer.cust_users = user_ids;
                    select_customer.cust_usersname = user_names;


                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            //本地缓存进行变更
                            local_update_customers(select_customer, user_ids, before_cust_users);

                            Constant.cust_customer_S.Update(select_customer);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }

                    }) { IsBackground = true }.Start();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 本地缓存进行变更
        /// </summary>
        /// <param name="select_customer"></param>
        /// <param name="user_ids"></param>
        /// <param name="before_cust_users"></param>
        private static void local_update_customers(cust_customer select_customer, string user_ids, string before_cust_users)
        {
            try
            {
                //缓存判断删除
                string[] user_ids_befores = Split_Hepler.str_to_stringss(before_cust_users);
                foreach (var guid in user_ids_befores)
                {
                    //管理员不要删除
                    if (!Constant.dicLimit_P.ContainsKey(guid))
                    {
                        //判断是否已经存在
                        if (cust_customer_handle.dic_Self.ContainsKey(guid))
                        {
                            List<cust_customer> current_list = cust_customer_handle.dic_Self[guid];
                            if (current_list.Contains(select_customer))
                            {
                                current_list.Remove(select_customer);
                            }
                        }
                    }
                }

                //缓存判断添加
                string[] user_ids_strings = Split_Hepler.str_to_stringss(user_ids);
                foreach (var guid in user_ids_strings)
                {
                    //判断是否已经存在
                    if (cust_customer_handle.dic_Self.ContainsKey(guid))
                    {
                        List<cust_customer> current_list = cust_customer_handle.dic_Self[guid];
                        if (!current_list.Contains(select_customer))
                        {
                            current_list.Add(select_customer);
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
        ///批量更改联系人的责任人
        /// </summary>
        public static bool update_linkman_user_ids(string guid, long[] linkman_ids_longs, string user_names, string user_ids)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(user_names) && !string.IsNullOrEmpty(user_ids))
                {
                    //获取所有指定的联系人
                    List<cust_linkman> linkman_list = cust_linkman_handle.dic_Self[guid].Where(t => linkman_ids_longs.Contains((long)t.id)).ToList();
                    if (linkman_list.Count > 0)
                    {
                        result = true;
                    }
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            foreach (var linkman in linkman_list)
                            {
                                //本地缓存联系人变更
                                local_update_linkman(user_names, user_ids, linkman);
                                Constant.cust_linkman_S.Update(linkman);
                            }
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
            return result;
        }

        /// <summary>
        /// 本地缓存联系人变更
        /// </summary>
        /// <param name="user_names"></param>
        /// <param name="user_ids"></param>
        /// <param name="linkman"></param>
        private static void local_update_linkman(string user_names, string user_ids, cust_linkman linkman)
        {
            try
            {
                //取出之前的id通知当前人进行删除
                string before_linkman_users = linkman.link_users;

                linkman.link_users = user_ids;
                linkman.link_usersname = user_names;

                //缓存判断
                string[] user_ids_befores = Split_Hepler.str_to_stringss(before_linkman_users);
                foreach (var guid in user_ids_befores)
                {
                    if (!Constant.dicLimit_P.ContainsKey(guid))
                    {
                        //判断是否已经存在
                        if (cust_linkman_handle.dic_Self.ContainsKey(guid))
                        {
                            List<cust_linkman> current_linkmans = cust_linkman_handle.dic_Self[guid];
                            current_linkmans.Remove(linkman);
                        }
                    }
                }

                //缓存判断添加
                string[] user_ids_strings = Split_Hepler.str_to_stringss(user_ids);
                foreach (var guid in user_ids_strings)
                {
                    //判断是否已经存在
                    if (cust_linkman_handle.dic_Self.ContainsKey(guid))
                    {
                        List<cust_linkman> current_linkmans = cust_linkman_handle.dic_Self[guid];
                        if (!current_linkmans.Contains(linkman))
                        {
                            current_linkmans.Add(linkman);
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
        public static List<cust_customer> GetPageByLinq(List<cust_customer> lstPerson, int pageIndex, int PageSize)
        {
            List<cust_customer> result = null;
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

        #region PC端使用

        public static void get_cust_customer_info_by_AllCustomer(HttpContext context)
        {
            var customer = cust_customer_handle.list_All;
            Dictionary<string, string> paramters_Type = pub_param_handle.dic_customer_Type;
            Dictionary<string, string> paramters_Level = pub_param_handle.dic_customer_Level;

            var cust_linkman = cust_linkman_handle.list_All;
            var follow_up = follow_up_handle.list_All;



            //customer左外follow_up得到c_f
            var query1 = from c in customer
                         join f in follow_up on c.id equals f.follow_cust_id into gj
                         from subpet in gj.DefaultIfEmpty()

                         select new
                         {

                             id = c.id,
                             cust_name = c.cust_name,
                             cust_type = paramters_Type[c.cust_type.ToString()],
                             cust_level = paramters_Level[c.cust_level.ToString()],
                             cust_usersname = c.cust_usersname,
                             follow_date = (subpet == null ? new DateTime() : subpet.follow_date)
                         };
            //c_f左外cust_linkman
            var query2 = from c_f in query1
                         join l in cust_linkman on c_f.id equals l.link_cust_id
                         into cfl
                         from cfl_ in cfl.DefaultIfEmpty()

                         select new
                         {

                             id = c_f.id,
                             cust_name = c_f.cust_name,
                             cust_type = c_f.cust_type,
                             cust_level = c_f.cust_level,
                             cust_usersname = c_f.cust_usersname,
                             follow_date = c_f.follow_date,
                             link_name = (cfl_ == null ? String.Empty : cfl_.link_name),
                             link_telephone = (cfl_ == null ? String.Empty : cfl_.link_telephone)
                         };
            //按follow_date分组
            var query3 = from p in query2
                         group p by p.id into allg
                         select new
                         {

                             cust_name = allg.Max(p => p.cust_name),
                             link_name = allg.Max(p => p.link_name),
                             link_telephone = allg.Max(p => p.link_telephone),
                             cust_type = allg.Max(p => p.cust_type),
                             cust_level = allg.Max(p => p.cust_level),
                             cust_usersname = allg.Max(p => p.cust_usersname),
                             follow_date = allg.Max(p => p.follow_date)
                         };
            context.Response.Write(Constant.jss.Serialize(query3));

        }
        public string Settime(string str)
        {
            str = Regex.Replace(str, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return str;
        }
        #endregion
    }
}