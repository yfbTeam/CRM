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
                    case "get_cust_customer_detail":
                        get_cust_customer_detail(context);
                        break;
                    case "get_cust_customer_list":
                        get_cust_customer_list(context);
                        break;
                    case "get_cust_customer_info":
                        get_cust_customer_info(context);
                        break;
                    case "edit_cust_customer":
                        edit_cust_customer(context);
                        break;
                    case "get_cust_customer_search":
                        get_cust_customer_search(context);
                        break;
                    case "get_cust_customer_parent":
                        get_cust_customer_parent(context);
                        break;
                    case "update_cust_customer_isdelete":
                        update_cust_customer_isdelete(context);
                        break;
                    case "cust_customer_nearby":
                        cust_customer_nearby(context);
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

        //签到
        #region 获取近距离的联系人(cust_users 当前用户ID)【签到     guid】

        /// <summary>
        /// 获取近距离的联系人(cust_users 当前用户ID)【签到】
        /// </summary>
        /// <param name="context"></param>
        public void cust_customer_nearby(HttpContext context)
        {
            try
            {
                HttpRequest Request = context.Request;
                //获取评论
                string sign_x = Request["sign_x"];
                string sign_y = Request["sign_y"];
                string guid = Request["guid"];

                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    //获取指定父ID的客户信息
                    //List<cust_customer> list1 = (from t in dic_Self[guid]
                    //                             where Constant.GetDistance(Convert.ToDouble(sign_y), Convert.ToDouble(sign_x), Convert.ToDouble(t.cust_y), Convert.ToDouble(t.cust_x)) <= 3000
                    //                             select t).ToList<cust_customer>();
                    if (dic_Self[guid].Count > 0)
                    {
                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = ConverList<cust_customer>.ListToDic(dic_Self[guid])
                        };
                    }
                }
                else
                {
                    string cust_users = Request["cust_users"];
                    DataTable comDt = Constant.bbc.cust_customer_nearby(sign_x, sign_y, cust_users);
                    List<Dictionary<string, object>> comList = new List<Dictionary<string, object>>();
                    comList = Constant.common.DataTableToList(comDt);
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = comList
                    };
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

        //客户详情（组合）
        #region 《多个集合通用方法》获取客户详细(页面有多个返回集合的时候，不需要前端逐次调每一个集合的接口)【客户列表】

        /// <summary>
        /// 《多个集合通用方法》获取客户详细(页面有多个返回集合的时候，不需要前端逐次调每一个集合的接口)【客户列表】
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_detail(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string id = Request["id"];
            string userid = Request["userid"];

            string guid = Request["guid"];
            string cust_name = "";
            try
            {
                customer cust_list = new customer();
                customer_info cust = new customer_info();

                #region 客户信息

                //从当前用户的缓存块里获取
                if (dic_Self.ContainsKey(guid))
                {
                    //获取指定的客户【在自己的客户列表里获取】
                    List<cust_customer> list1 = (from t in dic_Self[guid]
                                                 where t.id == Convert.ToInt64(id)
                                                 select t).ToList<cust_customer>();

                    if (list1.Count > 0)
                    {
                        cust_customer cst = list1[0];
                        cust.cust_name = cst.cust_name;
                        cust_name = cst.cust_name;
                        cust.cust_level = Convert.ToString(pub_param_handle.dic_customer_Level[Convert.ToString(cst.cust_level)]);
                        cust.cust_followdate = Convert.ToString(cst.cust_followdate);
                        cust.cust_usersname = cst.cust_usersname;
                        cust.cust_id = id;
                        cust_list.cust = cust;
                    }
                }
                else
                {
                    //获取客户信息
                    string where = " 1=1 and id=" + id + "";

                    cust = Constant.bbc.getcust_customer(cust, where);
                    cust.cust_name = cust.cust_name;
                    if (cust.cust_level != null)
                    {
                        cust.cust_level = cust.cust_level.ToString();
                    }

                    cust.cust_followdate = cust.cust_followdate.ToString();
                    cust.cust_usersname = cust.cust_usersname.ToString();

                    cust_list.cust = cust;
                }
                #endregion

                #region 联系人

                if (cust_linkman_handle.dic_Self.ContainsKey(guid))
                {
                    //获取指定的客户【在自己的客户列表里获取】
                    List<cust_linkman> list1 = (from t in cust_linkman_handle.dic_Self[guid]
                                                where t.link_cust_id == Convert.ToInt64(id)
                                                orderby t.link_createdate descending
                                                select t).Take(2).ToList<cust_linkman>();
                    List<Dictionary<string, object>> list = ConverList<cust_linkman>.ListToDic(list1);
                    foreach (var item in list)
                    {
                        //联系人等级
                        item["link_level_name"] = pub_param_handle.dic_linkMan_Grade[Convert.ToString(item["link_level"])];
                        //联系人客户名称
                        item["link_cust_name"] = cust_name;
                    }
                    cust_list.linkman = list;
                }
                else
                {
                    //获取联系人信息                
                    DataTable modList = Constant.bbc.getcust_linkman_list(id);
                    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                    list = Constant.common.DataTableToList(modList);
                    cust_list.linkman = list;
                }

                #endregion

                #region 工作计划

                if (workplan_handle.dic_Self.ContainsKey(guid))
                {
                    //获取指定的客户【在自己的客户列表里获取】
                    List<workplan> list1 = (from t in workplan_handle.dic_Self[guid]
                                            where t.wp_cust_id == Convert.ToInt64(id)
                                            orderby t.wp_plandate descending
                                            select t).Take(2).ToList<workplan>();
                    List<Dictionary<string, object>> list = ConverList<workplan>.ListToDic(list1);
                    foreach (var item in list)
                    {
                        //联系人和职位
                        List<cust_linkman> links = (from t in cust_linkman_handle.dic_Self[guid]
                                                    where t.id == Convert.ToInt64(item["wp_link_id"])
                                                    select t).ToList<cust_linkman>();
                        if (links != null && links.Count > 0)
                        {
                            item["link_name"] = links[0].link_name;
                            item["link_position"] = links[0].link_position;
                            item["wp_cust_name"] = links[0].link_cust_name;
                        }
                    }
                    cust_list.workplan = list;
                }
                else
                {
                    //获取工作计划             
                    DataTable workplanList = Constant.bbc.getworkplan_list(userid, id, "1");
                    List<Dictionary<string, object>> w_list = new List<Dictionary<string, object>>();
                    w_list = Constant.common.DataTableToList(workplanList);
                    cust_list.workplan = w_list;
                }

                #endregion

                #region 跟进记录

                if (follow_up_handle.dic_Self.ContainsKey(guid))
                {
                    //获取指定的客户【在自己的客户列表里获取】
                    List<follow_up> list1 = (from t in follow_up_handle.dic_Self[guid]
                                             where t.follow_cust_id == Convert.ToInt64(id)
                                             orderby t.follow_date descending
                                             select t).Take(2).ToList<follow_up>();
                    List<Dictionary<string, object>> list = ConverList<follow_up>.ListToDic(list1);
                    //foreach (var item in list)
                    //{

                    //}
                    cust_list.follow_up = list;
                }
                else
                {
                    //获取跟进记录         
                    DataTable followList = Constant.bbc.getfollow_up_list(userid, id, "1");
                    List<Dictionary<string, object>> f_list = new List<Dictionary<string, object>>();
                    f_list = Constant.common.DataTableToList(followList);
                    cust_list.follow_up = f_list;
                }

                #endregion

                #region 签到记录

                if (sign_in_handle.dic_Self.ContainsKey(guid))
                {
                    //获取指定的客户【在自己的客户列表里获取】
                    List<sign_in> list1 = (from t in sign_in_handle.dic_Self[guid]
                                           where t.sign_cust_id == Convert.ToInt64(id)
                                           orderby t.sign_date descending
                                           select t).Take(1).ToList<sign_in>();
                    List<Dictionary<string, object>> list = ConverList<sign_in>.ListToDic(list1);
                    foreach (var item in list)
                    {
                        //填入客户名称（签到）
                        item["sign_cust_name"] = cust.cust_name;
                    }
                    cust_list.sign = list;
                }
                else
                {
                    //获取签到记录      
                    DataTable signList = Constant.bbc.getsign_list(userid);
                    List<Dictionary<string, object>> s_list = new List<Dictionary<string, object>>();
                    s_list = Constant.common.DataTableToList(signList);
                    cust_list.sign = s_list;
                }

                #endregion

                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = cust_list
                };
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

        //搜索
        #region 按客户姓名搜索客户信息【搜索     guid】

        /// <summary>
        /// 按客户姓名搜索客户信息
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_search(HttpContext context)
        {
            Hashtable ht = new Hashtable();
            HttpRequest Request = context.Request;
            try
            {
                string guid = Request["guid"];
                string cust_name = Request["cust_name"];
                string cust_users = Request["cust_users"];
                string sign_x = Request["sign_x"] == null ? "" : Request["sign_x"];
                string sign_y = Request["sign_y"] == null ? "" : Request["sign_y"];
                string pageIndex = Request["PageIndex"] ?? "1";
                string pageSize = Request["PageSize"] ?? "10";
                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    int page_Index = Convert.ToInt32(Request["PageIndex"]);
                    int page_Size = Convert.ToInt32(Request["PageSize"]);
                    List<cust_customer> list1 = null;
                    //分支是因为一个纯粹通过客户名称查询，另外一个通过地址和客户名称查询
                    if (!string.IsNullOrEmpty(cust_name))
                    {
                        list1 = (from t in dic_Self[guid]
                                 where t.cust_name.Contains(cust_name)
                                 select t).ToList<cust_customer>();
                    }
                    else
                    {
                        list1 = dic_Self[guid];
                    }

                    bool isSign_Page = false;
                    if (!string.IsNullOrEmpty(Request["sign_x"]))
                    {
                        isSign_Page = true;
                    }
                    if (isSign_Page)
                    {
                        //进行分页
                        List<cust_customer> list2 = GetPageByLinq(list1, page_Index, page_Size);

                        List<Dictionary<string, object>> diis = ConverList<cust_customer>.ListToDic(list2);

                        //foreach (var item in diis)
                        //{
                        //    item.Add("distance", Convert.ToString(Constant.GetDistance(Convert.ToDouble(sign_x), Convert.ToDouble(sign_y), Convert.ToDouble(item["cust_x"]), Convert.ToDouble(item["cust_y"]))));
                        //}

                        //数据返回
                        PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = diis, PageIndex = page_Index, PageSize = page_Size, RowCount = list1.Count };
                        //数据返回
                        jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
                    }
                    else
                    {
                        //进行分页
                        List<cust_customer> list2 = GetPageByLinq(list1, page_Index, page_Size);
                        //数据返回
                        PagedDataModel<cust_customer> psd = new PagedDataModel<cust_customer>() { PagedData = list2, PageIndex = page_Index, PageSize = page_Size, RowCount = list1.Count };


                        //数据返回
                        jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };
                    }
                }
                else
                {
                    bool ispage = false;
                    if (!string.IsNullOrEmpty(Request["ispage"]))
                    {
                        ispage = Convert.ToBoolean(Request["ispage"]);
                    }
                    ht.Add("PageIndex", pageIndex);
                    ht.Add("PageSize", pageSize);
                    ht.Add("TableName", "cust_customer");
                    //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                    string order = "";
                    string fileds = "";
                    if (sign_x == "")
                    {
                        order = "round(dbo.fnGetDistance('" + sign_x + "','" + sign_y + "',cust_x,cust_y)*1000,0) asc";
                        fileds = "id,cust_name,cust_address,cust_location,round(dbo.fnGetDistance('" + sign_x + "','" + sign_y + "',cust_x,cust_y)*1000,0) as distance";
                    }
                    else
                    {
                        order = "round(dbo.fnGetDistance(" + sign_x + "," + sign_y + ",cust_x,cust_y)*1000,0) asc";
                        fileds = "id,cust_name,cust_address,cust_location,round(dbo.fnGetDistance(" + sign_x + "," + sign_y + ",cust_x,cust_y)*1000,0) as distance";
                    }
                    ht.Add("Order", "round(dbo.fnGetDistance('" + sign_x + "','" + sign_y + "',cust_x,cust_y)*1000,0) asc");
                    string where = " and cust_isdelete=0 and cust_users='" + cust_users + "'";
                    if (sign_x != "")
                    {
                        where += " and round(dbo.fnGetDistance(" + sign_x + "," + sign_y + ",cust_x,cust_y)*1000,0)<=3000";
                    }
                    if (!string.IsNullOrEmpty(cust_name))
                    {
                        where += " and cust_name like '%" + cust_name + "%'";
                    }
                    //新加字段fileds，主要是为了方便使用
                    jsonModel = Constant.cust_customer_S.GetPage(ht, fileds, ispage, where);
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
        //父级客户
        #region 获取父级客户【客户分为两级，父级和子级        guid】

        /// <summary>
        /// 获取父级客户【客户分为两级，父级和子级】
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_parent(HttpContext context)
        {
            HttpRequest Request = context.Request;
            Hashtable ht = new Hashtable();
            try
            {

                string guid = Request["guid"];
                string _cust_parent_id = Request["_cust_parent_id"];
                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    //获取指定父ID的客户信息
                    List<cust_customer> list1 = (from t in dic_Self[guid]
                                                 where t.cust_parent_id == Convert.ToInt64(_cust_parent_id)
                                                 select t).ToList<cust_customer>();


                    //提示
                    if (list1.Count > 0)
                    {
                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = ConverList<cust_customer>.ListToDic(list1)
                        };
                    }
                }
                else
                {
                    string cust_parent_id = "0";
                    string cust_user = "0";

                    ht.Add("TableName", "cust_customer");
                    //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                    string fileds = "id,cust_name";
                    //新加字段fileds，主要是为了方便使用

                    string where = " and cust_isdelete=0 ";
                    if (Request["cust_parent_id"] != null)
                    {
                        cust_parent_id = Request["cust_parent_id"];
                        where += " and cust_parent_id=" + cust_parent_id + "";
                    }

                    if (Request["cust_user"] != null)
                    {
                        cust_user = Request["cust_user"];
                        where += " and cust_users='" + cust_user + "'";
                    }

                    jsonModel = Constant.cust_customer_S.GetPage(ht, fileds, false, where);
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
        public void get_cust_customer_info(HttpContext context)
        {
            HttpRequest Request = context.Request;
            Hashtable ht = new Hashtable();
            string id = context.Request["id"];
            //string cust_user = Request["cust_user"];
            string guid = Request["guid"];
            try
            {
                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    //指定的一个客户
                    List<cust_customer> list1 = (from t in dic_Self[guid]
                                                 where t.id == Convert.ToInt64(id)
                                                 select t).ToList<cust_customer>();
                    List<Dictionary<string, object>> list2 = ConverList<cust_customer>.ListToDic(list1);
                    foreach (var item in list2)
                    {
                        item["cust_level_value"] = item["cust_level"];
                        item["cust_type_value"] = item["cust_type"];
                        item["cust_level"] = pub_param_handle.dic_customer_Level[Convert.ToString(item["cust_level"])];
                        item["cust_type"] = pub_param_handle.dic_customer_Type[Convert.ToString(item["cust_type"])];
                        List<cust_linkman> links = (from t in cust_linkman_handle.dic_Self[guid]
                                                    where t.link_cust_id == Convert.ToInt64(id)
                                                    select t).ToList<cust_linkman>();
                        string cust_links = "";
                        var linkids = "";
                        foreach (cust_linkman li_item in links)
                        {
                            cust_links += "、" + li_item.link_name;
                            linkids += "," + li_item.id;
                        }
                        item["cust_links"] = cust_links.Trim('、');
                        item["linkids"] = linkids.Trim('、');
                    }


                    if (list1.Count > 0)
                    {
                        jsonModel = new JsonModel()
                        {
                            errNum = 0,
                            errMsg = "success",
                            retData = list2
                        };
                    }
                }
                else
                {
                    ht.Add("TableName", "cust_customer");
                    //(select dbo.getlink_name(1)这个是在数据库中建的函数
                    string fileds = "id,cust_name,cust_level as cust_level_value,cust_type as cust_type_value,dbo.get_pub_param_title(cust_type,'客户类型') as cust_type,dbo.get_pub_param_title(cust_level,'客户级别') as cust_level,cust_address,cust_location,dbo.get_link_name(" + id + ") as link_names,dbo.get_link_ids(" + id + ") as linkids,dbo.getcustomer_name(cust_parent_id) as parent_customer_name,cust_parent_id";
                    //新加字段fileds，主要是为了方便使用
                    jsonModel = Constant.cust_customer_S.GetPage(ht, fileds, false, " and id=" + id + "");
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

        #region 添加客户信息

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="context"></param>
        public void add_cust_customer(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            string guid = request["guid"];
            try
            {
                #region 填充的客户信息

                //已经有的客户信息不允许再提交【以客户名称作为根据】
                cust_customer cust = new cust_customer();
              
                //目前添加的不可以和其他的人一样（客户名称）【可以添加、允许编辑，但防止和其他人一样】
                cust.cust_name = request["cust_name"];
                //填充的客户信息
                cust.cust_type = int.Parse(request["cust_type"]);
                cust.cust_address = request["cust_address"];
                cust.cust_location = request["cust_location"];
                cust.cust_parent_id = Convert.ToInt32(request["cust_parent_id"]);
                cust.cust_level = Convert.ToInt32(request["cust_level"]);
                cust.cust_parent_id =0;
                //x坐标
                string cust_x = Convert.ToString(request["cust_x"]);
                cust.cust_x = cust_x == "" ? 0 : Convert.ToDecimal(cust_x);
                //y坐标
                string cust_y = Convert.ToString(request["cust_y"]);
                cust.cust_y = cust_y == "" ? 0 : Convert.ToDecimal(cust_y);
                //负责人
                cust.cust_usersname = request["cust_usersname"];
                cust.cust_users = request["cust_users"];
                //时间
                cust.cust_followdate = DateTime.Now;
                cust.cust_createdate = DateTime.Now;
                cust.cust_updatedate = DateTime.Now;

                cust.cust_updateuser = 1;
                cust.cust_isdelete = "0";

                cust.cust_creator = 0;
               

                #endregion

                var exit_customer = (from _customer in list_All
                                     where _customer.cust_name == cust.cust_name
                                     select new
                                     {
                                         _customer
                                     });

                if (exit_customer.Count() > 0)
                {
                    //客户信息已存在【标示是某人】    
                    jsonModel = get_jsonmodel(0, "cust_isexit", exit_customer.ElementAt(0)._customer.cust_usersname);
                }
                else if (dic_Self.ContainsKey(guid))
                {
                    //缓存添加客户
                    dic_Self[guid].Add(cust);
                    //成功提示
                    jsonModel = get_jsonmodel(0, "success", 1);

                    new Thread(() =>
                    {

                        //通知领导进行添加
                        lead_add_hlper(request, guid, cust);

                        //联系人关联[添加客户]
                        relate_add_helper(request, guid, cust);

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
                string json = Constant.jss.Serialize(jsonModel);
                response.Write("{\"result\":" + json + "}");
            }
        }

        /// <summary>
        /// 通知领导进行添加
        /// </summary>
        /// <param name="request"></param>
        /// <param name="guid"></param>
        /// <param name="cust"></param>
        void lead_add_hlper(HttpRequest request, string guid, cust_customer cust)
        {
             try
            {
            #region 通知领导进行添加

            //添加客户
            if (!list_All.Contains(cust))
            {
                list_All.Add(cust);
            }

            //获取上级的guid
            List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];
            //上级列表
            commonAdmin_CustursID.ForEach(new Action<string>((unqeque) =>
            {
                //若领导在线，添加当前添加的用户【领导在线,而且该不为自己】
                if (dic_Self.ContainsKey(unqeque) && unqeque != guid)
                {
                    if (!dic_Self[unqeque].Contains(cust))
                    {
                        dic_Self[unqeque].Add(cust);
                    }
                }
            }));

            #endregion
                  }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
          
        }

        /// <summary>
        /// 联系人关联【添加】
        /// </summary>
        /// <param name="request"></param>
        /// <param name="guid"></param>
        /// <param name="cust"></param>
        void relate_add_helper(HttpRequest request, string guid, cust_customer cust)
        {
             
             try
            {
            //联系人ID
            string link_ids = request["link_ids"];
            jsonModel = Constant.bbc.add_cust_customer(cust, link_ids);
            //新增的客户ID
            cust.id = Convert.ToInt32(jsonModel.retData);
            //获取关联的联系人【与客户】
            if (!string.IsNullOrEmpty(link_ids))
            {
                //联系人的ID获取
                string[] str = link_ids.Split(new char[] { ',' });
                //联系人id列表
                List<string> link_list = str.ToList<string>();

                var relate_linkman = (from _linkman in cust_linkman_handle.dic_Self[guid]
                                      where link_list.Contains(Convert.ToString(_linkman.id))
                                      select new
                                      {
                                          _linkman
                                      });
                //将关联的联系人客户id设置为当前客户的ID
                if (relate_linkman.Count() > 0)
                {
                    foreach (var link_man in relate_linkman)
                    {
                        link_man._linkman.link_cust_id = cust.id;
                    }
                }
            }
                  }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }           
        }

        #endregion

        #region 编辑（指定客户ID变更客户信息  guid）

        /// <summary>
        /// 编辑客户信息
        /// </summary>
        /// <param name="context"></param>
        public void edit_cust_customer(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = request["id"];
            long id_long = id == "0" ? 0 : Convert.ToInt64(id);
            string guid = request["guid"];
            try
            {
                //修改《暂时修改功能》
                if (id_long > 0)
                {
                    if (dic_Self.ContainsKey(guid))
                    {
                        var select_customer = (from _customer in dic_Self[guid]
                                               where _customer.id == id_long
                                               select new
                                               {
                                                   _customer
                                               }
                                                   );
                        if (select_customer.Count() > 0)
                        {

                            cust_customer s_C = select_customer.ElementAt(0)._customer;

                            //删除状态、地址、等级、坐标、用户名称、用户类型【可变更】 用原有的数据进行变更，然后提交给数据库                          
                            s_C.cust_address = request["cust_address"];
                            s_C.cust_level = Convert.ToInt32(request["cust_level"]);
                            s_C.cust_location = request["cust_location"];
                            s_C.cust_name = request["cust_name"];
                            s_C.cust_type = int.Parse(request["cust_type"]);
                            //x坐标
                            string cust_x = Convert.ToString(request["cust_x"]);
                            s_C.cust_x = cust_x == "" ? 0 : Convert.ToDecimal(cust_x);

                            //y坐标
                            string cust_y = Convert.ToString(request["cust_y"]);
                            s_C.cust_y = cust_y == "" ? 0 : Convert.ToDecimal(cust_y);

                            s_C.cust_updatedate = DateTime.Now;

                            //成功提示                         
                            jsonModel = get_jsonmodel(0, "success", select_customer.Count());

                            //开启线程操作数据库
                            new Thread(() =>
                            {
                                //关联联系人【编辑】
                                relate_edit_helper(request, id_long, guid, s_C);

                            }) { IsBackground = true }.Start();
                        }
                    }

                }
                else
                {
                    add_cust_customer(context);
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
        /// 联系人关联【编辑】
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id_long"></param>
        /// <param name="guid"></param>
        /// <param name="s_C"></param>
        private void relate_edit_helper(HttpRequest request, long id_long, string guid, cust_customer s_C)
        {
            try
            {
                string link_ids = request["link_ids"];
                jsonModel = Constant.bbc.edit_cust_customer(s_C, link_ids);
                //获取关联的联系人【与客户】
                if (!string.IsNullOrEmpty(link_ids))
                {
                    //联系人的ID获取
                    string[] str = link_ids.Split(new char[] { ',' });

                    List<string> link_list = str.ToList<string>();

                    List<cust_linkman> links = (from t in cust_linkman_handle.dic_Self[guid]
                                                where link_list.Contains(Convert.ToString(t.id))
                                                select t).ToList<cust_linkman>();
                    if (links != null)
                    {
                        foreach (cust_linkman item in links)
                        {
                            item.link_cust_id = id_long;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }         
        }



        #endregion

        #region 删除 【客户信息,指定ID     guid】

        /// <summary>
        ///删除 【客户信息】
        /// </summary>
        /// <param name="context"></param>
        public void update_cust_customer_isdelete(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string id = context.Request["id"];

            //id号
            long id_long = Convert.ToInt64(id);
            try
            {
                //获取指定用户的客户
                string guid = request["guid"];
                if (id != "0")
                {

                    //缓存应用
                    if (dic_Self.ContainsKey(guid))
                    {
                        //删除指定的客户
                        List<cust_customer> list1 = (from t in dic_Self[guid]
                                                     where t.id == id_long
                                                     select t).ToList<cust_customer>();


                        //进行客户删除
                        if (list1.Count > 0)
                        {
                            //是自己的才能进行删除  判断有没有关联
                            if (list1[0].cust_users == guid)
                            {
                                //查看是否有联系人进行过关联
                                if (cust_linkman_handle.dic_Self.ContainsKey(guid) && cust_linkman_handle.dic_Self[guid].Count(item => item.link_cust_id == id_long) > 0)
                                {
                                    jsonModel = get_jsonmodel(5, "failed", "无法删除,关联了联系人");
                                }
                                else if (workplan_handle.dic_Self.ContainsKey(guid) && workplan_handle.dic_Self[guid].Count(item => item.wp_cust_id == id_long) > 0)
                                {
                                    jsonModel = get_jsonmodel(5, "failed", "无法删除,关联了工作计划");
                                }
                                else if (sign_in_handle.dic_Self.ContainsKey(guid) && sign_in_handle.dic_Self[guid].Count(item => item.sign_cust_id == id_long) > 0)
                                {
                                    jsonModel = get_jsonmodel(5, "failed", "无法删除,关联了签到记录");

                                }
                                else if (follow_up_handle.dic_Self.ContainsKey(guid) && follow_up_handle.dic_Self[guid].Count(item => item.follow_cust_id == id_long) > 0)
                                {
                                    jsonModel = get_jsonmodel(5, "failed", "无法删除,关联了跟进记录");
                                }

                                else if (workplan_handle.dic_Self.ContainsKey(guid) && workplan_handle.dic_Self[guid].Count(item => item.wp_cust_id == id_long) > 0)
                                {
                                    jsonModel = get_jsonmodel(5, "failed", "无法删除,关联了工作报告");
                                }
                                else
                                {
                                    #region 成功删除

                                    //删除客户需要两个地方
                                    dic_Self[guid].Remove(list1[0]);
                                    list_All.Remove(list1[0]);
                                    jsonModel = get_jsonmodel(0, "success", 1);
                                    //开启线程操作数据库
                                    new Thread(() =>
                                    {
                                        try
                                        {
                                            #region 通知领导进行删除

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
                                                        //删除指定的客户
                                                        List<cust_customer> li = (from t in dic_Self[item]
                                                                                  where t.id == id_long
                                                                                  select t).ToList<cust_customer>();
                                                        //进行客户删除
                                                        if (li.Count > 0)
                                                        {
                                                            //删除客户需要两个地方
                                                            dic_Self[item].Remove(li[0]);
                                                        }
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region 通知与客户管理的联系人去除客户信息

                                            //通知与客户管理的联系人去除客户信息
                                            List<cust_linkman> link_list = (from t in cust_linkman_handle.dic_Self[guid]
                                                                            where t.link_cust_id == Convert.ToInt64(id)
                                                                            select t).ToList<cust_linkman>();

                                            foreach (var item in link_list)
                                            {
                                                item.link_cust_name = string.Empty;
                                                //删除联系人关联的客户信息
                                                Constant.cust_linkman_S.Update(item);
                                            }

                                            #endregion

                                            #region 调用数据库

                                            cust_customer cust_customer = Constant.cust_customer_S.GetEntityById(Convert.ToInt32(id)).retData as cust_customer;
                                            cust_customer.cust_isdelete = request["cust_isdelete"];
                                            if (cust_customer != null)
                                            {
                                                jsonModel = Constant.cust_customer_S.Update(cust_customer);
                                            }

                                            #endregion
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.Error(ex);
                                        }
                                    }) { }.Start();

                                    #endregion
                                }
                            }
                            else
                            {
                                jsonModel = new JsonModel()
                                {
                                    errNum = 3,
                                    errMsg = "failed",
                                    retData = "未经授权,无法删除该客户"
                                };
                            }
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

        //客户列表
        #region  客户列表【率属于当前用户的客户       guid】


        /// <summary>
        /// 获取客户集合（带分页）备注：具体返回字段不全，只是为了测试
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_customer_list(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string guid = Request["guid"];
            try
            {
                //缓存应用
                if (dic_Self.ContainsKey(guid))
                {
                    //分页信息
                    int page_Index = Convert.ToInt32(Request["PageIndex"]);
                    int page_Size = Convert.ToInt32(Request["PageSize"]);

                    //进行分页
                    List<cust_customer> list2 = GetPageByLinq(dic_Self[guid], page_Index, page_Size);
                    //对象集合转为dic集合列表
                    List<Dictionary<string, object>> dicList = ConverList<cust_customer>.ListToDic(list2);
                    //数据对应
                    foreach (var item in dicList)
                    {

                        if (item.ContainsKey("cust_level") && pub_param_handle.dic_customer_Level.ContainsKey(Convert.ToString(item["cust_level"])))
                        {
                            //客户级别
                            item["cust_level"] = pub_param_handle.dic_customer_Level[Convert.ToString(item["cust_level"])];
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
                    }
                    //返回数据
                    PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = dic_Self[guid].Count };
                    //数据包（json格式）
                    jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };

                }
                else
                {
                    Hashtable ht = new Hashtable();

                    bool ispage = false;
                    if (!string.IsNullOrEmpty(Request["ispage"]))
                    {
                        ispage = Convert.ToBoolean(Request["ispage"]);
                    }
                    ht.Add("PageIndex", Request["PageIndex"] ?? "1");
                    ht.Add("PageSize", Request["PageSize"] ?? "10");

                    ht.Add("TableName", "cust_customer");

                    //(select dbo.getlink_name(1) 这个是在数据库中建的函数
                    string fileds = "id,cust_name,cust_usersname,dbo.get_pub_param_title(cust_level,'客户级别') as cust_level,dbo.get_follow_time(id) as cust_followdate,cust_address";
                    //新加字段fileds，主要是为了方便使用
                    jsonModel = Constant.cust_customer_S.GetPage(ht, fileds, ispage, GetWhere(Request));
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

        #endregion

        #region 辅助参数

        public string GetWhere(HttpRequest Request)
        {
            string result = string.Empty;
            try
            {
                string cust_users = "";
                StringBuilder sb = new StringBuilder();
                sb.Append(" and cust_isdelete=0");
                if (!string.IsNullOrEmpty(Request["cust_users"]))
                {
                    cust_users = Request["cust_users"];
                    sb.Append(" and cust_users='" + cust_users + "'");//跟进客户id
                }

                result = sb.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


        #endregion

        #region 辅助方法【linq 分页\页面回调】

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
                List<cust_customer> list = lstPerson.OrderByDescending(i => i.id).ToList<cust_customer>();
                result = list.Skip((pageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return result;
        }

        public static JsonModel get_jsonmodel(int errNum, string errMsg, object retData)
        {
            return new JsonModel()
            {
                errNum = errNum,
                errMsg = errMsg,
                retData = retData,
            };
        }

        #endregion


      
    }
}