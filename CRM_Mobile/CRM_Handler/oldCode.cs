#region 插入签到信息

///// <summary>
///// 插入签到信息（估计是重复了）
///// </summary>
//public void insert_into_sign(HttpContext context)
//{
//    JsonModel jsonModel = new JsonModel();
//    HttpRequest request = context.Request;
//    bool canThrow = true;
//    try
//    {              
//        sign_in sign = new sign_in();
//        //用户ID 
//        int_Valied(request, "sign_userid", sign, new Action<bool, int>((s, intValue) =>
//            {
//                if (s)
//                {
//                    sign.sign_userid = intValue;
//                }
//                else
//                {
//                    canThrow = false;
//                }
//            }));

//        //用户名称
//        str_Valied(request, "sign_username", sign, new Action<bool, string>((s, strValue) =>
//            {
//                if (s)
//                {
//                    sign.sign_username = strValue;
//                }
//                else
//                {
//                    canThrow = false;
//                }
//            }));

//        //签到时间
//        sign.sign_date = DateTime.Now;

//        //客户ID
//        int_Valied(request, "sign_cust_id", sign, new Action<bool, int>((s, intValue) =>
//        {
//            if (s)
//            {
//                sign.sign_cust_id = intValue;
//            }
//            else
//            {
//                canThrow = false;
//            }
//        }));

//        //地理名称(大致)
//        str_Valied(request, "sign_location", sign, new Action<bool, string>((s, strValue) =>
//        {
//            if (s)
//            {
//                sign.sign_location = strValue;
//            }
//            else
//            {
//                canThrow = false;
//            }
//        }));

//        //地理名称(具体)
//        str_Valied(request, "sign_address", sign, new Action<bool, string>((s, strValue) =>
//        {
//            if (s)
//            {
//                sign.sign_address = strValue;
//            }
//            else
//            {
//                canThrow = false;
//            }
//        }));

//        //位置偏移量（与客户的距离）
//        int_Valied(request, "sign_offset", sign, new Action<bool, int>((s, intValue) =>
//        {
//            if (s)
//            {
//                sign.sign_offset = intValue;
//            }
//            else
//            {
//                canThrow = false;
//            }
//        }));
//        if (canThrow)
//        {
//            Constant.sign_in_S.Add(sign);
//            jsonModel = new JsonModel()
//            {
//                errNum = 0,
//                errMsg = "success",

//            };
//        }
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

///// <summary>
///// int数值验证
///// </summary>
///// <param name="request"></param>
///// <param name="name"></param>
///// <param name="sign"></param>
///// <param name="callBack"></param>
//private static void int_Valied(HttpRequest request, string name, sign_in sign, Action<bool, int> callBack)
//{
//    bool result = false;
//    int intValue = 0;
//    try
//    {
//        if (int.TryParse(request[name], out intValue))
//        {
//            result = true;
//        }
//    }
//    catch (Exception ex)
//    {
//    }
//    finally
//    {
//        callBack(result, intValue);
//    }
//}

///// <summary>
///// 字符串验证
///// </summary>
///// <param name="request"></param>
///// <param name="name"></param>
///// <param name="sign"></param>
///// <param name="callBack"></param>
//private static void str_Valied(HttpRequest request, string name, sign_in sign, Action<bool, string> callBack)
//{
//    bool result = false;
//    string strValue = Convert.ToString(request[name]);
//    try
//    {
//        if (!string.IsNullOrEmpty(strValue))
//        {
//            result = true;
//        }
//    }
//    catch (Exception ex)
//    {
//    }
//    finally
//    {
//        callBack(result, strValue);
//    }
//}


/*--------------------新增图片信息开始-----------------------*/
//string picture = request["picture"];
//if (picture != "")
//{
//    string[] pictures = picture.Split(',');
//    for (int i = 0; i < pictures.Length; i++)
//    {
//        picture p = new picture();
//        p.id = 0;
//        p.pic_cn_table = "工作计划";
//        p.pic_en_table = "workplan";
//        p.pic_url = pictures[i];
//        p.pic_isdelete = "0";
//        p.pic_creator = workplan.wp_userid;
//        p.pic_remark = "新增工作计划";
//        string wp_id = Model.retData.ToString();
//        if (workplan.id != 0)
//        {
//            wp_id = workplan.id.ToString();
//        }
//        p.pic_table_id = int.Parse(wp_id);
//        Constant.picture_S.Add(p);
//    }
//}

#region 开启或关闭跟进(废弃)

//public void update_follow_isopen(HttpContext context)
//{
//    HttpRequest request = context.Request;
//    JsonModel Model = null;
//    HttpResponse response = context.Response;
//    string id = context.Request["id"];
//    try
//    {
//        if (id != "0")
//        {
//            follow_up follow_up = Constant.follow_up_S.GetEntityById(Convert.ToInt32(id)).retData as follow_up;
//            follow_up.follow_status = request["follow_status"];
//            if (follow_up != null)
//            {
//                Model = Constant.follow_up_S.Update(follow_up);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        Model = Constant.ErrorGetData(ex);
//    }
//    finally
//    {
//        response.Write("{\"result\":" + Constant.jss.Serialize(Model) + "}");
//    }
//}

#endregion

#region 根据id获取职位id【废弃】

/// <summary>
/// 根据id获取职位id
/// </summary>
/// <param name="context"></param>
//public void get_link_position_by_id(HttpContext context)
//{
//    try
//    {
//        string id = context.Request["id"];
//        DataTable dt = Constant.bbc.get_link_position_by_id(id);
//        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
//        list = Constant.common.DataTableToList(dt);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = "success",
//            retData = list
//        };
//    }
//    catch (Exception ex)
//    {
//        LogHelper.Error(ex);
//        jsonModel = Constant.ErrorGetData(ex);
//    }
//    finally
//    {
//        string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
//        context.Response.Write("{\"result\":" + jsonString + "}");
//    }
//}

#endregion

#region 删除联系人【废弃】

///// <summary>
///// 删除联系人
///// </summary>
///// <param name="context"></param>
//public void del_cust_linkman(HttpContext context)
//{
//    HttpRequest request = context.Request;
//    HttpResponse response = context.Response;
//    string id = context.Request["id"];
//    if (!string.IsNullOrWhiteSpace(id))
//    {
//        try
//        {
//            cust_linkman advert = Constant.cust_linkman_S.GetEntityById(Convert.ToInt32(id)).retData as cust_linkman;
//            if (advert != null)
//            {
//                if (!string.IsNullOrWhiteSpace(request["IsDelete"]))
//                {
//                    advert.link_isdelete = request["IsDelete"];
//                }
//                jsonModel = Constant.cust_linkman_S.Update(advert);
//                jsonModel.status = "yes";
//            }

//        }
//        catch (Exception ex)
//        {
//            LogHelper.Error(ex);
//            jsonModel = Constant.ErrorGetData(ex);
//        }
//        finally
//        {
//            response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//        }
//    }
//}

#endregion

#region 销售简报【废弃】

///// <summary>
///// 销售简报
///// </summary>
///// <param name="context"></param>
//public void get_statistic_list_1(HttpContext context)
//{
//    Hashtable ht = new Hashtable();
//    bool ispage = false;
//    if (!string.IsNullOrEmpty(context.Request["ispage"]))
//    {
//        ispage = Convert.ToBoolean(context.Request["ispage"]);
//    }
//    ht.Add("PageIndex", context.Request["PageIndex"] ?? "1");
//    ht.Add("PageSize", context.Request["PageSize"] ?? "10");
//    try
//    {
//        ht.Add("TableName", "statistic");
//        //(select dbo.getlink_name(1) 这个是在数据库中建的函数
//        string fileds = "*,s_follow_up_all,CONVERT(varchar(100), s_createdate, 23) as createdate";
//        //新加字段fileds，主要是为了方便使用
//        jsonModel = Constant.statistic_S.GetPage(ht, fileds, ispage, GetWhere(context, 2));
//    }
//    catch (Exception ex)
//    {
//        LogHelper.Error(ex);
//        jsonModel = Constant.ErrorGetData(ex);
//    }
//    finally
//    {
//        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//    }
//}

#endregion


#region 获取用户是否签到【】

/// <summary>
/// 获取用户是否签到
/// </summary>
/// <param name="context"></param>
//public void get_is_sign(HttpContext context)
//{
//    try
//    {
//        int userid = 0;
//        if (!string.IsNullOrEmpty(context.Request["sign_userid"]))
//        {
//            userid = Convert.ToInt32(context.Request["sign_userid"]);
//        }
//        string sign_date = "";
//        if (!string.IsNullOrEmpty(context.Request["sign_date"]))
//        {
//            sign_date = context.Request["sign_date"];
//        }
//        DataTable dt = Constant.bbc.get_is_sign(sign_date, userid);
//        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
//        list = Constant.common.DataTableToList(dt);

//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = "success",
//            retData = list
//        };
//    }
//    catch (Exception ex)
//    {
//        LogHelper.Error(ex);
//        jsonModel = Constant.ErrorGetData(ex);
//    }
//    finally
//    {
//        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//    }
//}

#endregion

#endregion

#region 分享圈列表获取
//public void get_share_list(HttpContext context)
//{
//    HttpRequest Request = context.Request;

//    //当前用户ID
//    string guid = context.Request["guid"];
//    try
//    {
//        //缓存应用
//        if (dic_Self.ContainsKey(guid))
//        {
//            //请求参数
//            bool ispage = false;
//            //是否分页
//            if (!string.IsNullOrEmpty(Request["ispage"]))
//            {
//                ispage = Convert.ToBoolean(Request["ispage"]);
//            }
//            //每一页包含的数量
//            int PageSize = Convert.ToInt32(Request["PageSize"]);
//            //第几页
//            int PageIndex = Convert.ToInt32(Request["PageIndex"]);

//            //封装到PagedDataModel的元数据
//            List<report> report_List = new List<report>();


//            List<share> list2 = GetPageByLinq(dic_Self[guid], PageIndex, PageSize);

//            //数据对应
//            foreach (var fp in list2)
//            {
//                List<workreport> list1 = (from t in workreport_handle.dic_Self[guid]
//                                          where t.id == Convert.ToInt64(fp.table_id)
//                                          select t).Take(2).ToList<workreport>();
//                //跟进记录（对应页面的实体类型）
//                report report = new report();
//                List<praise> list_praise = praise_handle.dic_Self[guid].Where(p => p.praise_table_id == Convert.ToInt64(fp.id)).Where(p => p.praise_userid == guid).ToList<praise>();
//                string is_praise = "";
//                if (list_praise.Count > 0)
//                {
//                    is_praise = "1";
//                }
//                else
//                {
//                    is_praise = "0";
//                }
//                //附件跟进记录（对应页面的实体类型【通过数据库映射填充实体所需部分信息】）
//                foreach (var work_report in list1)
//                {
//                    report.report_info = new report_info()
//                    {
//                        report_content = work_report.report_content,
//                        report_username = work_report.report_username,
//                        report_createdate = Convert.ToDateTime(work_report.report_createdate).ToString("yyyy-MM-dd"),
//                        work_report_id = Convert.ToInt64(work_report.id),
//                        report_startdate = Convert.ToDateTime(work_report.report_startdate).ToString("yyyy-MM-dd"),
//                        report_enddate = Convert.ToDateTime(work_report.report_enddate).ToString("yyyy-MM-dd"),
//                        report_plan = work_report.report_plan,
//                        is_praise = is_praise,
//                        //report_content = fp.,
//                        //follow_cust_id = Convert.ToInt64(fp.follow_cust_id),
//                        //follow_date = ((DateTime)fp.follow_date).ToString("yyyy-MM-dd"),
//                        //follow_link_id = Convert.ToInt64(fp.follow_link_id),
//                        //follow_username = fp.follow_username,
//                        //is_praise = is_praise,
//                        //id = Convert.ToInt64(fp.id),

//                    };
//                }

//                #region 获取图片

//                if (Constant.list_picture_All != null)
//                {
//                    //获取指定的图片【类型 和ID】
//                    List<picture> list_picture = (from t in Constant.list_picture_All
//                                                  where t.pic_en_table == "follow_up" && t.pic_table_id == Convert.ToInt32(fp.id)
//                                                  select t).ToList<picture>();
//                    List<Dictionary<string, object>> list_picture_1 = ConverList<picture>.ListToDic(list_picture);
//                    report.picture = list_picture_1;
//                }
//                #endregion

//                #region 获取评论

//                //获取指定的图片【类型 和ID】
//                List<comment> list_p = (from t in comment_handle.dic_Self[guid]
//                                        where t.com_table_id == Convert.ToInt32(fp.id) && t.com_isdelete == "0"
//                                        select t).ToList<comment>();
//                List<Dictionary<string, object>> list = ConverList<comment>.ListToDic(list_p);
//                report.comment = list;

//                #endregion

//                #region 获取点赞人

//                List<praise> list_praises = (from t in praise_handle.dic_Self[guid]
//                                             where t.praise_table_id == Convert.ToInt32(fp.id)
//                                             select t).ToList<praise>();
//                List<Dictionary<string, object>> list_p1 = ConverList<praise>.ListToDic(list_praises);
//                report.praise = list_p1;

//                #endregion

//                report_List.Add(report);
//            }

//            //返回数据
//            PagedDataModel<report> psd = new PagedDataModel<report>()
//            {
//                PagedData = report_List,
//                PageIndex = PageIndex,
//                PageSize = PageSize,
//                RowCount = dic_Self[guid].Count
//            };

//            //数据包（json格式）
//            jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };

//        }
//        else
//        {
//            #region 数据库方式操作【缓存机制下不需要】
//            Hashtable ht = new Hashtable();
//            PagedDataModel<report_list> pagedDataModel = null;
//            JsonModel1 jsonModel1 = null;
//            //请求参数
//            bool ispage = false;
//            if (!string.IsNullOrEmpty(context.Request["ispage"]))
//            {
//                ispage = Convert.ToBoolean(context.Request["ispage"]);
//            }
//            int PageSize = Convert.ToInt32(context.Request["PageSize"]);
//            int PageIndex = Convert.ToInt32(context.Request["PageIndex"]);
//            ht.Add("PageIndex", context.Request["PageIndex"] ?? "1");
//            ht.Add("PageSize", context.Request["PageSize"] ?? "10");
//            ht.Add("TableName", "v_share");
//            ht = Constant.common.AddStartEndIndex(ht);
//            List<report_list> flist = new List<CRM_Model.report_list>();
//            report_list list = new report_list();
//            List<report> f = new List<report>();

//            int RowCount = 0;
//            //获取follow_up集合
//            string report_userid = "";
//            string where = "";
//            if (context.Request["report_userid"] != null)
//            {
//                report_userid = context.Request["report_userid"];
//                where += " and report_userid='" + report_userid + "'";
//            }

//            //DataTable modList = Constant.bbc.getlistpage(ht, "id,dbo.get_picture_count('workreport',work_report_id) as picture_count,work_report_id,report_username,dbo.get_is_praise(work_report_id,report_userid,2) as is_praise,report_plan,CONVERT(varchar(100), report_enddate, 20) as report_enddate,CONVERT(varchar(100), report_startdate, 20) as report_startdate,CONVERT(varchar(100), report_createdate, 20) as report_createdate,report_content,dbo.get_praise_all(work_report_id,2) as praise_all", out RowCount, true, where);
//            string userid = "";
//            if (context.Request["userid"] != null)
//            {
//                userid = context.Request["userid"];
//            }
//            DataTable modList = Constant.bbc.getlistpage(ht, "id,dbo.get_picture_count('workreport',work_report_id) as picture_count,work_report_id,report_username,dbo.get_is_praise(work_report_id,'" + userid + "',2) as is_praise,report_plan,CONVERT(varchar(100), report_enddate, 20) as report_enddate,CONVERT(varchar(100), report_startdate, 20) as report_startdate,CONVERT(varchar(100), report_createdate, 20) as report_createdate,report_content,dbo.get_praise_all(work_report_id,2) as praise_all", out RowCount, true, where);

//            //if 跟进记录不存在的情况
//            if (modList == null || modList.Rows.Count <= 0)
//            {
//                jsonModel1 = ErrorGetData("记录不存在");
//            }
//            else
//            {
//                List<report_info> s = ConverList<report_info>.ConvertToList(modList);
//                foreach (report_info fp in s)
//                {
//                    report fw = new report();
//                    fw.report_info = fp;
//                    //获取评论
//                    DataTable comDt = Constant.bbc.getcomment_list(fp.id.ToString(), "2");
//                    List<Dictionary<string, object>> comList = new List<Dictionary<string, object>>();
//                    comList = Constant.common.DataTableToList(comDt);
//                    fw.comment = comList;
//                    //获取图片信息
//                    DataTable picDt = Constant.bbc.getpicture_list("workreport", fp.work_report_id.ToString());
//                    List<Dictionary<string, object>> picList = new List<Dictionary<string, object>>();
//                    picList = Constant.common.DataTableToList(picDt);
//                    fw.picture = picList;
//                    f.Add(fw);

//                }
//                list.reportlist = f;
//                flist.Add(list);

//                //总页数
//                int PageCount = (int)Math.Ceiling(RowCount * 1.0 / PageSize);
//                //将数据封装到PagedDataModel分页数据实体中
//                pagedDataModel = new PagedDataModel<report_list>()
//                {
//                    PageCount = PageCount,
//                    PagedData = flist,
//                    PageIndex = PageIndex,
//                    PageSize = PageSize,
//                    RowCount = RowCount
//                };
//                //将分页数据实体封装到JSON标准实体中
//                jsonModel1 = new JsonModel1()
//                {
//                    errNum = 0,
//                    errMsg = "success",
//                    retData = pagedDataModel
//                };
//            }
//            #endregion
//        }

//    }
//    catch (Exception ex)
//    {
//        jsonModel = Constant.ErrorGetData(ex.Message);
//    }
//    finally
//    {
//        #region 没有缓存机制的情况下用的

//        //string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
//        //context.Response.Write("{\"result\":" + jsonString + "}");

//        #endregion

//        string result = Constant.jss.Serialize(jsonModel);
//        context.Response.Write("{\"result\":" + result + "}");
//    }
//}
#endregion

#region comment_handle 旧方案

//using CRM_BLL;
//using CRM_Common;
//using CRM_Handler.Report;
//using CRM_Handler;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading;
//using System.Web;
//using System.Web.Script.Serialization;
//using CRM_Model;

//namespace CRM_Handler.Share
//{
//    /// <summary>
//    /// comment_handle 评论 的摘要说明
//    /// </summary>
//    public class comment_handle : IHttpHandler
//    {
//        #region 字段

//        JsonModel jsonModel = null;

//        /// <summary>
//        ///评论群
//        /// </summary>
//        public static List<comment> list_All = null;

//        /// <summary>
//        /// 指定某个用户的评论设置
//        /// </summary>
//        public static Dictionary<string, List<comment>> dic_Self = new Dictionary<string, List<comment>>();

//        #endregion

//        #region 中心入口点

//        public void ProcessRequest(HttpContext context)
//        {
//            string func = context.Request["func"] ?? "";
//            try
//            {
//                //全局初始化
//                Constant.Fill_All_Data(context);

//                switch (func)
//                {
//                    case "edit_comment":
//                        edit_comment(context);
//                        break;
//                    case "get_comment":
//                        get_comment(context);
//                        break;
//                    case "update_comment_isdelete":
//                        update_comment_isdelete(context);
//                        break;
//                    default:
//                        jsonModel = new JsonModel()
//                        {
//                            errNum = 5,
//                            errMsg = "没有此方法",
//                            retData = ""
//                        };
//                        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.Error(ex);
//                jsonModel = Constant.ErrorGetData(ex);
//                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//            }
//        }

//        #endregion

//        #region 获取评论【tableID Type guid】

//        /// <summary>
//        /// 获取评论
//        /// </summary>
//        /// <param name="context"></param>
//        public void get_comment(HttpContext context)
//        {
//            HttpRequest Request = context.Request;
//            string guid = Request["guid"];

//            //获取评论
//            string id = context.Request["id"];
//            string type = context.Request["type"];
//            try
//            {
//                if (dic_Self.ContainsKey(guid))
//                {
//                    //指定的一个客户
//                    List<comment> list1 = (from t in dic_Self[guid]
//                                           where t.com_table_id == Convert.ToInt64(id) && t.com_type == type && t.com_isdelete == "0"
//                                           select t).ToList<comment>();
//                    //List<Dictionary<string, object>> comList = ConverList<comment>.ListToDic(list1);

//                    if (list1.Count > 0)
//                    {
//                        jsonModel = new JsonModel()
//                        {
//                            errNum = 0,
//                            errMsg = "success",
//                            retData = ConverList<comment>.ListToDic(list1)
//                        };
//                    }

//                }
//                else
//                {
//                    DataTable comDt = Constant.bbc.getcomment_list(id.ToString(), type);
//                    List<Dictionary<string, object>> comList = new List<Dictionary<string, object>>();
//                    comList = Constant.common.DataTableToList(comDt);
//                    jsonModel = new JsonModel()
//                    {
//                        errNum = 0,
//                        errMsg = "success",
//                        retData = comList
//                    };
//                    context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.Error(ex);
//            }
//            finally
//            {
//                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//            }
//        }

//        #endregion

//        #region 添加或编辑评论

//        /// <summary>
//        /// 新增评论
//        /// </summary>
//        /// <param name="context"></param>
//        public void edit_comment(HttpContext context)
//        {
//            HttpRequest request = context.Request;
//            HttpResponse response = context.Response;
//            string id = request["id"];
//            string guid = request["guid"];
//            try
//            {
//                comment comment = new comment();
//                comment.id = Convert.ToInt32(id);
//                comment.com_table_id = int.Parse(request["com_table_id"].ToString());
//                comment.com_parent_id = int.Parse(request["com_parent_id"]);
//                comment.com_content = request["com_content"];
//                comment.com_type = request["com_type"];
//                comment.com_userid = request["com_userid"];
//                comment.com_username = request["com_username"];
//                comment.com_isdelete = "0";

//                //workreport w = new workreport();
//                //w.report_status = "1";
//                //Constant.workreport_S.Update(w);
//                //修改《暂时修改功能》
//                if (id != "0")
//                {
//                    if (dic_Self.ContainsKey(guid))
//                    {
//                        List<comment> dic2 = dic_Self[guid].Where(item => item.id == Convert.ToInt64(id)).ToList<comment>();
//                        if (dic2.Count > 0)
//                        {
//                            dic2[0].com_table_id = comment.com_table_id;
//                            dic2[0].com_parent_id = comment.com_parent_id;
//                            dic2[0].com_content = comment.com_content;
//                            dic2[0].com_type = comment.com_type;
//                            dic2[0].com_userid = comment.com_userid;
//                            dic2[0].com_username = comment.com_username;
//                            dic2[0].com_isdelete = comment.com_isdelete;

//                            //成功提示
//                            jsonModel = new JsonModel()
//                            {
//                                errNum = 0,
//                                errMsg = "success",
//                                retData = dic2.Count
//                            };
//                            //开启线程操作数据库
//                            new Thread(() =>
//                            {
//                                comment advert = Constant.comment_S.GetEntityById(Convert.ToInt32(id)).retData as comment;
//                                if (advert != null)
//                                {
//                                    jsonModel = Constant.comment_S.Update(comment);
//                                }
//                            }) { }.Start();
//                        }
//                    }
//                }
//                else
//                {
//                    if (dic_Self.ContainsKey(guid))
//                    {
//                        //缓存添加签到
//                        dic_Self[guid].Add(comment);


//                        jsonModel = new JsonModel()
//                        {
//                            errNum = 0,
//                            errMsg = "success",
//                            retData = 1
//                        };
//                        new Thread(() =>
//                        {
//                            //数据库标示
//                            jsonModel = Constant.comment_S.Add(comment);
//                            //已阅读则进行标示【报告】
//                            List<workreport> dic2 = workreport_handle.dic_Self[guid].Where(item => item.id == comment.com_table_id).ToList<workreport>();
//                            //标示
//                            if (dic2.Count > 0)
//                            {
//                                dic2[0].report_status = "1";

//                                //数据库标示
//                                workreport workreport = Constant.workreport_S.GetEntityById(Convert.ToInt32(comment.com_table_id)).retData as workreport;
//                                if (workreport != null)
//                                {
//                                    workreport.report_status = "1";
//                                    Constant.workreport_S.Update(workreport);
//                                }
//                            }
//                        }) { }.Start();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.Error(ex);
//                jsonModel = Constant.ErrorGetData(ex);
//            }
//            finally
//            {
//                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//            }
//        }

//        #endregion

//        #region 删除评论

//        /// <summary>
//        ///删除 
//        /// </summary>
//        /// <param name="context"></param>
//        public void update_comment_isdelete(HttpContext context)
//        {
//            HttpRequest request = context.Request;
//            HttpResponse response = context.Response;
//            string id = request["id"];
//            string guid = request["guid"];
//            try
//            {
//                if (id != "0")
//                {
//                    //删除指定的评论
//                    List<comment> list1 = (from t in dic_Self[guid]
//                                           where t.id == Convert.ToInt64(id)
//                                           select t).ToList<comment>();
//                    //进行评论删除
//                    if (list1.Count > 0)
//                    {
//                        //删除客户需要两个地方
//                        dic_Self[guid].Remove(list1[0]);
//                        list_All.Remove(list1[0]);
//                        jsonModel = new JsonModel()
//                        {
//                            errNum = 0,
//                            errMsg = "success",
//                            retData = 1
//                        };
//                        //开启线程操作数据库
//                        new Thread(() =>
//                        {
//                            try
//                            {
//                                comment comment = Constant.comment_S.GetEntityById(Convert.ToInt32(id)).retData as comment;
//                                comment.com_isdelete = request["com_isdelete"];
//                                if (comment != null)
//                                {
//                                    jsonModel = Constant.comment_S.Update(comment);
//                                }
//                            }
//                            catch (Exception ex)
//                            {
//                                LogHelper.Error(ex);
//                            }
//                        }) { }.Start();
//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.Error(ex);
//                jsonModel = Constant.ErrorGetData(ex);
//            }
//            finally
//            {
//                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//            }
//        }

//        #endregion

//        #region 辅助字段

//        public bool IsReusable
//        {
//            get
//            {
//                return false;
//            }
//        }

//        #endregion
//    }
//}

#endregion


#region 统计详情

///// <summary>
///// 统计详情
///// </summary>
///// <param name="context"></param>
//public void get_statistic_detail(HttpContext context)
//{
//    string userid = "";
//    if (!string.IsNullOrEmpty(context.Request["userid"]))
//    {
//        userid = context.Request["userid"];
//    }
//    string username = "";
//    if (!string.IsNullOrEmpty(context.Request["username"]))
//    {
//        username = context.Request["username"];
//    }
//    string startdate = "";
//    if (!string.IsNullOrEmpty(context.Request["startdate"]))
//    {
//        startdate = context.Request["startdate"];
//    }
//    string enddate = "";
//    if (!string.IsNullOrEmpty(context.Request["enddate"]))
//    {
//        enddate = context.Request["enddate"];
//    }
//    try
//    {
//        DataTable slist_dt = Constant.bbc.get_statistic(userid, username, startdate, enddate);
//        List<Dictionary<string, object>> sList = new List<Dictionary<string, object>>();
//        sList = Constant.common.DataTableToList(slist_dt);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = "success",
//            retData = sList
//        };
//    }
//    catch (Exception ex)
//    {
//        LogHelper.Error(ex);
//        jsonModel = Constant.ErrorGetData(ex);
//    }
//    finally
//    {
//        string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
//        context.Response.Write("{\"result\":" + jsonString + "}");
//    }
//}

#endregion

//签到
#region 获取近距离的联系人(cust_users 当前用户ID)【签到     guid】

///// <summary>
///// 获取近距离的联系人(cust_users 当前用户ID)【签到】
///// </summary>
///// <param name="context"></param>
//public void cust_customer_nearby(HttpContext context)
//{
//    try
//    {
//        HttpRequest Request = context.Request;
//        //获取评论
//        string sign_x = Request["sign_x"];
//        string sign_y = Request["sign_y"];
//        string guid = Request["guid"];

//        //获取指定父ID的客户信息                 
//        if (dic_Self[guid].Count > 0)
//        {
//            jsonModel = new JsonModel()
//            {
//                errNum = 0,
//                errMsg = "success",
//                retData = ConverList<cust_customer>.ListToDic(dic_Self[guid])
//            };

//        }
//    }
//    catch (Exception ex)
//    {
//        LogHelper.Error(ex);
//    }
//    finally
//    {
//        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
//    }
//}

#endregion

#region 工作计划

///// <summary>
//      /// more工作计划[2]
//      /// </summary>
//      /// <param name="guid"></param>
//      /// <param name="link_list"></param>
//      /// <param name="li"></param>
//      private static void more_workplane(string guid, linkman link_list, cust_linkman li)
//      {
//          try
//          {
//              //获取指定的工作计划【在自己的工作计划列表里获取 默认两条数据】
//              List<workplan> workplans = (from t in workplan_handle.dic_Self[guid]
//                                          where t.wp_cust_id == li.link_cust_id && t.wp_link_id == li.id
//                                          orderby t.wp_plandate descending
//                                          select t).Take(2).ToList();
//              List<Dictionary<string, object>> dic_workplans = ConverList<workplan>.ListToDic(workplans);
//              foreach (var item in dic_workplans)
//              {
//                  //联系人和职位
//                  cust_linkman links = (from t in cust_linkman_handle.dic_Self[guid]
//                                        where t.id == Convert.ToInt64(item["wp_link_id"])
//                                        select t).FirstOrDefault();
//                  if (links != null)
//                  {
//                      item["link_name"] = links.link_name;
//                      item["link_position"] = links.link_position;
//                      item["wp_cust_name"] = links.link_cust_name;
//                  }
//              }
//              link_list.workplan = dic_workplans;
//          }
//          catch (Exception ex)
//          {
//              LogHelper.Error(ex);
//          }

//      }

#endregion

//签到是不让修改的 ok
#region oldsolution

//List<sign_in> dic2 = dic_Self[guid].Where(item => item.id == Convert.ToInt64(id)).ToList<sign_in>();
//if (dic2.Count > 0)
//{
//    dic2[0].sign_userid = si.sign_userid;
//    dic2[0].sign_username = si.sign_username;
//    dic2[0].sign_date = si.sign_date;
//    dic2[0].sign_location = si.sign_location;
//    dic2[0].sign_y = si.sign_y;
//    dic2[0].sign_address = si.sign_address;
//    dic2[0].sign_offset = si.sign_offset;
//    dic2[0].sign_isdelete = si.sign_isdelete;

//    //成功提示
//    jsonModel = new JsonModel()
//    {
//        errNum = 0,
//        errMsg = "success",
//        retData = dic2.Count
//    };
//    //开启线程操作数据库
//    new Thread(() =>
//    {
//        sign_in advert = Constant.sign_in_S.GetEntityById(Convert.ToInt32(id)).retData as sign_in;
//        if (advert != null)
//        {
//            jsonModel = Constant.sign_in_S.Update(si);
//        }
//    }) { }.Start();
//}

#endregion

#region old solution  指定客户的工作计划

////指定客户的工作计划
//string wp_cust_id = Request["wp_cust_id"];

//long wp_cust_id_long = 0;

////指定联系人的工作计划
//string link_id = Request["link_id"];

//long link_id_long = 0;

////判断有没有指定客户并判断是不是有效的ID
//if (!string.IsNullOrEmpty(wp_cust_id) && long.TryParse(wp_cust_id, out wp_cust_id_long))
//{
//    //获取指定客户的工作计划
//    list2 = (from t in dic_Self[guid]
//             where t.wp_cust_id == wp_cust_id_long
//             select t).ToList<workplan>();
//}
//else if (!string.IsNullOrEmpty(link_id) && long.TryParse(link_id, out link_id_long))
//{
//    //获取指定客户的工作计划
//    list2 = (from t in dic_Self[guid]
//             where t.wp_link_id == link_id_long
//             select t).ToList<workplan>();
//}
//else
//{
//}

#endregion
