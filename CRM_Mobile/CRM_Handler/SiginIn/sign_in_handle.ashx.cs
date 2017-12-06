using CRM_BLL;
using CRM_Common;
using CRM_Model;
using CRM_Handler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using CRM_Handler.Custom;
using CRM_Handler.Statistical;
using CRM_Handler.Common;
using CRM_Handler.Admin;

namespace CRM_Handler.SiginIn
{
    /// <summary>
    /// sign_in_handle 签到的摘要说明
    /// </summary>
    public class sign_in_handle : IHttpHandler
    {

        #region 字段

        JsonModel jsonModel = null;

        /// <summary>
        ///签到群
        /// </summary>
        public static List<sign_in> list_All = null;

        /// <summary>
        /// 指定某个用户的签到群
        /// </summary>
        public static Dictionary<string, List<sign_in>> dic_Self = new Dictionary<string, List<sign_in>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.follow;

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
                        case "edit_sign_in":
                            edit_sign_in(context, guid);
                            break;
                        case "get_sign_list":
                            get_sign_list(context, guid);
                            break;
                        case "get_is_sign":
                            //get_is_sign(context);
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

        #region 添加或修改签到【guid】

        public void edit_sign_in(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;
            long id = RequestHelper.long_transfer(Request, "id");
            try
            {
                sign_in si = new sign_in();
                si.id = id;
                si.sign_date = DateTime.Now;
                si.sign_cust_id = RequestHelper.long_transfer(Request, "sign_cust_id");
                si.sign_location = RequestHelper.string_transfer(Request, "sign_location");
                si.sign_x = RequestHelper.decimal_transfer(Request, "sign_x");
                si.sign_y = RequestHelper.decimal_transfer(Request, "sign_y");
                si.sign_address = RequestHelper.string_transfer(Request, "sign_address");
                si.sign_offset = Convert.ToInt32(RequestHelper.decimal_transfer(Request, "sign_offset"));
                si.sign_isdelete = "0";
                si.sign_createdate = DateTime.Now;
                si.sign_updatedate = DateTime.Now;

                //修改《暂时修改功能》
                if (id > 0)
                {
                }
                else
                {
                    //放在此位置的原因是  领导修改时不更改当前编辑人的信息
                    si.sign_userid = RequestHelper.string_transfer(Request, "sign_userid");
                    si.sign_username = RequestHelper.string_transfer(Request, "sign_username");

                    if (!dic_Self[guid].Contains(si))
                    {
                        //同一个人  同一个客户  【五分钟之内】
                        int count = dic_Self[guid].Count(s => s.sign_userid == si.sign_userid
                            && s.sign_cust_id == si.sign_cust_id && ((DateTime)s.sign_date).Year == DateTime.Now.Year && ((DateTime)s.sign_date).DayOfYear == DateTime.Now.DayOfYear
                            && ((DateTime)si.sign_date - (DateTime)s.sign_date).Minutes < Constant.SignLimitTime);

                        if (count == 0)
                        {
                            if(string.IsNullOrEmpty(si.sign_address))
                            {
                                var model = PositionHelper.GeoCoder(Convert.ToString(si.sign_y),Convert.ToString(si.sign_x)); 
                                if(model!= null)
                                {
                                    si.sign_address = model.Result.Formatted_Address;
                                }                               
                            }

                            //缓存添加客户
                            dic_Self[guid].Add(si);
                            jsonModel = Constant.get_jsonmodel(0, "success", 1);
                            new Thread(() =>
                            {
                                try
                                {
                                    //通知领导进行添加
                                    admin_add_sign(guid, si);
                                    jsonModel = Constant.sign_in_S.Add(si);
                                    si.id = Convert.ToInt32(jsonModel.retData);
                                    //客户列表,当前用户【更改拜访记录】
                                    List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                                    cust_customer customer_sign = cust_customer_selfs.FirstOrDefault(item => item.id == si.sign_cust_id);
                                    if (customer_sign != null)
                                    {
                                        customer_sign.cust_followdate = DateTime.Now;
                                        Constant.cust_customer_S.Update(customer_sign);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex);
                                }
                            }) { IsBackground = true }.Start();
                        }
                        else
                        {
                            jsonModel = Constant.get_jsonmodel(5, "failed", "同一客户，禁止在5分钟之内重复签到");
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
        /// 通知领导进行添加
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="si"></param>
        private static void admin_add_sign(string guid, sign_in si)
        {
            try
            {
                //通知领导我已添加用户
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    if (!list_All.Contains(si))
                    {
                        //当前添加客户
                        list_All.Add(si);
                    }
                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];

                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，添加当前添加的用户
                        if (dic_Self.ContainsKey(item))
                        {
                            //跟进列表,当前跟进
                            List<sign_in> sign_in_admins = dic_Self[item];
                            if (!sign_in_admins.Contains(si))
                            {
                                sign_in_admins.Add(si);
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

        #region 获取签到列表

        /// <summary>
        /// 签到列表
        /// </summary>
        /// <param name="context"></param>
        public void get_sign_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //分页信息
                int page_Index = RequestHelper.int_transfer(Request, "PageIndex");
                int page_Size = RequestHelper.int_transfer(Request, "PageSize");
                //開始日期
                DateTime stardate = RequestHelper.DateTime_transfer(Request, "stardate");
                //結束日期
                DateTime enddate = RequestHelper.DateTime_transfer(Request, "enddate").AddDays(1);

                //部门的ID号【传参 】
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");

                var sign_in_selfs = from t in dic_Self[guid] select t;

                //指定签到客户
                long sign_cust_id = RequestHelper.long_transfer(Request, "sign_cust_id");

                //获取签到列表【筛选】
                sign_in_selfs = get_signs_helper(stardate, enddate, sign_in_selfs, sign_cust_id);

                sign_in_selfs = Check_And_Get_List_dep(departmentID, memmberID, sign_in_selfs);

                int sign_count = sign_in_selfs.Count();
                //进行分页
                List<sign_in> list_sign_in_page = GetPageByLinq(sign_in_selfs.ToList(), page_Index, page_Size);

                //对象集合转为dic集合列表
                List<Dictionary<string, object>> dicList = ConverList<sign_in>.ListToDic(list_sign_in_page);
                //客户列表,当前用户
                List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                foreach (var item in dicList)
                {
                    item["sign_date"] = Convert.ToDateTime(item["sign_date"]).ToString("yyyy-MM-dd  HH:mm");
                    long sign_cust_id_c = Convert.ToInt64(item["sign_cust_id"]);
                    cust_customer _cust_customer = (from t in cust_customer_selfs
                                                    where sign_cust_id_c == (long)t.id
                                                    select t).FirstOrDefault();
                    if (_cust_customer != null)
                    {
                        item.Add("cust_name", _cust_customer.cust_name);
                    }
                }
                string status = string.Empty;

                //返回数据
                PagedDataModel<Dictionary<string, object>> psd = new PagedDataModel<Dictionary<string, object>>() { PagedData = dicList, PageIndex = page_Index, PageSize = page_Size, RowCount = sign_count };
                //数据包（json格式）
                jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd, status = status };

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

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<sign_in> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<sign_in> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.sign_userid == memmberID
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
                                      where UniqueNo_string.Contains(w.sign_userid)
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
        /// 获取签到列表【时间筛选】
        /// </summary>
        /// <param name="sign_date"></param>
        /// <param name="sign_in_selfs"></param>
        /// <param name="sign_ins"></param>
        /// <param name="sign_cust_id"></param>
        /// <returns></returns>
        private static IEnumerable<sign_in> get_signs_helper(DateTime startdate, DateTime enddate, IEnumerable<sign_in> sign_in_selfs, long sign_cust_id)
        {
            try
            {
                //判断有没有指定客户并判断是不是有效的ID
                if (sign_cust_id > 0)
                {
                    //获取指定客户的签到列表
                    sign_in_selfs = (from t in sign_in_selfs
                                     where t.sign_cust_id == sign_cust_id && ((DateTime)t.sign_date) > startdate && ((DateTime)t.sign_date) < enddate
                                     orderby t.id descending
                                     select t);
                }
                else
                {
                    //有日期的签到筛选
                    sign_in_selfs = (from t in sign_in_selfs
                                     where ((DateTime)t.sign_date) > startdate && ((DateTime)t.sign_date) < enddate
                                     orderby t.id descending
                                     select t);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return sign_in_selfs;
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
        public static List<sign_in> GetPageByLinq(List<sign_in> lstPerson, int pageIndex, int PageSize)
        {
            List<sign_in> result = null;
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