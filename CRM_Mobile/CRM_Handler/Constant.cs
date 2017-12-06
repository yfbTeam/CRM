using CRM_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_BLL;
using CRM_Common;
using System.Data;
using CRM_Handler.Custom;
using CRM_Handler.LinkMan;
using CRM_Handler.WorkPlan;
using CRM_Handler.PubParam;
using CRM_Handler.Follow;
using CRM_Handler.Remind;
using CRM_Handler.Report;
using CRM_Handler.Share;
using CRM_Handler.SiginIn;
using CRM_Handler.Statistical;
using System.Web.Script.Serialization;
using System.Timers;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using CRM_Handler.Common;
using System.Configuration;

namespace CRM_Handler
{
    public static class Constant
    {
        #region 字段

        /// <summary>
        /// 是否已经进行了全局的初始化
        /// </summary>
        static bool isAllDataInit = false;

        /// <summary>
        /// 所有用户数据
        /// </summary>
        public static string All_user_data = null;

        /// <summary>
        /// 所有用户数据 【统计详情，批量导入数据，】
        /// </summary>
        public static List<UserInfo> dic_All_user = new List<UserInfo>();

        /// <summary>
        /// 指定部门的数据 【统计详情，批量导入数据，】
        /// </summary>
        public static List<UserInfo> List_All_RelateUserInfo = new List<UserInfo>();

        /// <summary>
        /// 部门列表
        /// </summary>
        public static List<DepartMent> DepartMent_List = new List<DepartMent>();

        /// <summary>
        /// 用户中心
        /// </summary>
        public static string service_url = ConfigurationManager.AppSettings["userCenter"] + "&IsStu=false&OrgNo=" + ConfigurationManager.AppSettings["OrgNo"];

        /// <summary>
        /// 是否开启测试模式
        /// </summary>
        public static bool IsTestMode = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsTestMode"]);

        /// <summary>
        /// 批量导入时是否覆盖原有客户地址
        /// </summary>
        public static bool IsInput_CoverAddress = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsInput_CoverAddress"]);

        /// <summary>
        /// 销售部门组织ID
        /// </summary>
        public static string Sale_DepartmentList_Value = System.Configuration.ConfigurationManager.AppSettings["Sale_DepartmentList_Value"];
        /// <summary>
        /// 销售部门组织
        /// </summary>
        public static string Sale_DepartmentList = System.Configuration.ConfigurationManager.AppSettings["Sale_DepartmentList"];



        #region 权限管理

        /// <summary>
        /// 权限组【适用于管理员、总监】
        /// </summary>
        public static Dictionary<string, Admin_CS> dicLimit_P = new Dictionary<string, Admin_CS>();

        /// <summary>
        /// 所有用户
        /// </summary>
        public static Dictionary<string, List<string>> dic_custs_users = new Dictionary<string, List<string>>();

        #endregion

        //客户信息
        public static cust_customerService cust_customer_S = new cust_customerService();
        //联系人
        public static cust_linkmanService cust_linkman_S = new cust_linkmanService();
        //语音
        public static audioService audio_S = new audioService();
        //图片
        public static pictureService picture_S = new pictureService();
        //评论
        public static commentService comment_S = new commentService();
        //跟进记录
        public static follow_upService follow_up_S = new follow_upService();
        //公共参数
        public static pub_paramService pub_param_S = new pub_paramService();
        //规则
        public static param_ruleService param_rule_S = new param_ruleService();
        //点赞
        public static praiseService praise_S = new praiseService();
        //提醒
        public static remindService remind_S = new remindService();
        //提醒设置
        public static remind_settingService remind_setting_S = new remind_settingService();
        //签到
        public static sign_inService sign_in_S = new sign_inService();
        //工作计划
        public static workplanService workplan_S = new workplanService();
        //工作报告
        public static workreportService workreport_S = new workreportService();
        //统计
        public static statisticService statistic_S = new statisticService();
        //分享圈
        public static shareService share_S = new shareService();

        /// <summary>
        /// js辅助
        /// </summary>
        public static JavaScriptSerializer jss = new JavaScriptSerializer();

        //全局参数
        public static BLLCommon common = new BLLCommon();
        public static BLLBaseCommon bbc = new BLLBaseCommon();


        //所有联系人数据，不管删除没删除
        //public static List<cust_linkman> linkman_all_delete_or_nodelete = new List<cust_linkman>();

        #region 全局数据

        /// 跟进记录集合
        /// </summary>
        public static List<picture> list_picture_All = null;

        #endregion

        #endregion

        #region 表名称

        //客户表
        public static string cust_customerTable = "cust_customer";

        //联系人
        public static string cust_linkmanTable = "cust_linkman";

        //语音
        public static string audioTable = "audio";

        //图片
        public static string pictureTable = "picture";

        //评论
        public static string commentTable = "comment";

        //跟进记录
        public static string follow_upTable = "follow_up";

        //公共参数
        public static string pub_paramTable = "pub_param";

        //参数规则
        public static string param_ruleTable = "param_rule";

        //点赞
        public static string praiseTable = "praise";

        //遗忘提醒
        public static string remindTable = "remind";

        //提醒设置
        public static string remind_settingTable = "remind_setting";

        //签到
        public static string sign_inTable = "sign_in";

        //工作计划
        public static string workplanTable = "workplan";

        //工作报告
        public static string workreportTable = "workreport";

        //统计
        public static string statisticTable = "statistic";

        #endregion

        #region 辅助函数

        /// <summary>
        /// 页转HT
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="request"></param>
        public static void SizeToHT(Hashtable ht, HttpRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request["PageIndex"]))
            {
                ht.Add("PageIndex", Convert.ToString(request["PageIndex"]));
            }
            if (!string.IsNullOrWhiteSpace(request["PageSize"]))
            {
                ht.Add("PageSize", Convert.ToString(request["PageSize"]));
            }
        }

        #endregion

        #region 异常处理（返回数据包）

        public static JsonModel ErrorGetData(Exception ex)
        {
            JsonModel jsonModel = new JsonModel()
            {
                errMsg = ex.Message,
                retData = "",
                status = "no"
            };

            return jsonModel;
        }

        public static JsonModel ErrorGetData(string ex)
        {
            JsonModel jsonModel = new JsonModel()
            {
                errMsg = ex,
                retData = "",
                status = "no"
            };

            return jsonModel;
        }

        #endregion

        #region 通用获取一页数据

        public static void GetPageCommon(HttpContext context, ModelEnum modelEnum, string TableName)
        {
            Hashtable ht = new Hashtable();
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string where = string.Empty;
            JsonModel jsonModel = null;
            try
            {
                SizeToHT(ht, request);
                ht.Add("TableName", TableName);
                jsonModel = GetPageHelper(modelEnum, ht, where);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                response.Write("{\"result\":" + jss.Serialize(jsonModel) + "}");
            }
        }

        public static void GetPageCommon(HttpContext context, ModelEnum modelEnum, string TableName, string where)
        {
            Hashtable ht = new Hashtable();
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            JsonModel jsonModel = null;
            try
            {
                SizeToHT(ht, request);
                ht.Add("TableName", TableName);
                jsonModel = GetPageHelper(modelEnum, ht, where);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = ErrorGetData(ex);
            }
            finally
            {
                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        public static JsonModel GetPageHelper(ModelEnum modelEnum, Hashtable ht, string where)
        {
            JsonModel Model = null;
            switch (modelEnum)
            {
                case ModelEnum.cust_customer:
                    Model = cust_customer_S.GetPage(ht, "", true, where);
                    break;
                default:
                    break;
            }

            return Model;
        }

        #endregion

        #region 获取所有数据（Common）

        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <returns></returns>
        public static List<cust_customer> Get_all_Customer()
        {
            List<cust_customer> dicCustomer = new List<cust_customer>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "cust_customer");
                DataTable dt = Constant.cust_customer_S.GetData(hs, false, "and cust_isdelete =0");
                dicCustomer = ConverList<cust_customer>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return dicCustomer;
        }

        /// <summary>
        /// 获取所有参数
        /// </summary>
        /// <returns></returns>
        public static List<pub_param> Get_all_pub_param()
        {
            List<pub_param> list = new List<pub_param>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "pub_param");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and pub_isdelete =0");
                list = ConverList<pub_param>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有联系人
        /// </summary>
        /// <returns></returns>
        public static List<cust_linkman> Get_all_linkman()
        {
            List<cust_linkman> list = new List<cust_linkman>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "cust_linkman");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and link_isdelete =0");
                list = ConverList<cust_linkman>.ConvertToList(dt);
                //linkman_all_delete_or_nodelete = list;
                //list = list.Where(t => t.link_isdelete == "0").ToList();

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有工作计划
        /// </summary>
        /// <returns></returns>
        public static List<workplan> Get_all_workplan()
        {
            List<workplan> list = new List<workplan>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "workplan");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and wp_isdelete =0");
                list = ConverList<workplan>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有跟进记录
        /// </summary>
        /// <returns></returns>
        public static List<follow_up> Get_all_Follow()
        {
            List<follow_up> list = new List<follow_up>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "follow_up");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and follow_isdelete =0");
                list = ConverList<follow_up>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有提醒
        /// </summary>
        /// <returns></returns>
        public static List<remind> Get_all_remind()
        {
            List<remind> list = new List<remind>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "remind");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and rem_isdelete =0");
                list = ConverList<remind>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有提醒设置
        /// </summary>
        /// <returns></returns>
        public static List<remind_setting> Get_all_remind_setting()
        {
            List<remind_setting> list = new List<remind_setting>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "remind_setting");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and remind_isdelete =0");
                list = ConverList<remind_setting>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有工作报告
        /// </summary>
        /// <returns></returns>
        public static List<workreport> Get_all_workreport()
        {
            List<workreport> list = new List<workreport>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "workreport");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and report_isdelete =0");
                list = ConverList<workreport>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有共享圈内容
        /// </summary>
        /// <returns></returns>
        public static List<share> Get_all_share()
        {
            List<share> list = new List<share>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "share");
                DataTable dt = Constant.share_S.GetData(hs, false, "");
                list = ConverList<share>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有评论内容
        /// </summary>
        /// <returns></returns>
        public static List<comment> Get_all_comment()
        {
            List<comment> list = new List<comment>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "comment");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and com_isdelete =0");
                list = ConverList<comment>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有点赞内容
        /// </summary>
        /// <returns></returns>
        public static List<praise> Get_all_praise()
        {
            List<praise> list = new List<praise>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "praise");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "");
                list = ConverList<praise>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有签到内容
        /// </summary>
        /// <returns></returns>
        public static List<sign_in> Get_all_sign_in()
        {
            List<sign_in> list = new List<sign_in>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "sign_in");
                DataTable dt = Constant.pub_param_S.GetData(hs, false, "and sign_isdelete =0");
                list = ConverList<sign_in>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取所有图片内容
        /// </summary>
        /// <returns></returns>
        public static List<picture> Get_all_picture()
        {
            List<picture> list = new List<picture>();
            try
            {
                Hashtable hs = new Hashtable();
                hs.Add("TableName", "picture");
                DataTable dt = Constant.picture_S.GetData(hs, false, " and pic_isdelete =0");
                list = ConverList<picture>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        #endregion

        #region 配置所有数据【】

        /// <summary>
        /// 数据初始化
        /// </summary>
        public static void Fill_All_Data(HttpContext context)
        {
            try
            {
                //if (!isAllDataInit)
                //{
                //已经初始化
                isAllDataInit = true;
                //客户 联系人数据填充
                customer_linkman_Fil();

                //参数填充
                pub_paramters_fill_center();

                #region 获取工作计划

                if (workplan_handle.list_All == null)
                {
                    workplan_handle.list_All = Constant.Get_all_workplan();
                }

                #endregion

                #region 获取跟进记录

                if (follow_up_handle.list_All == null)
                {
                    follow_up_handle.list_All = Constant.Get_all_Follow();
                }

                #endregion

                #region 获取签到内容

                if (sign_in_handle.list_All == null)
                {
                    sign_in_handle.list_All = Constant.Get_all_sign_in();
                }

                #endregion

                #region 获取提醒

                if (remind_handle.list_All == null)
                {
                    remind_handle.list_All = Constant.Get_all_remind();
                }

                #endregion

                #region 获取提醒设置

                if (remind_setting_handle.list_All == null)
                {
                    remind_setting_handle.list_All = Constant.Get_all_remind_setting();
                }

                #endregion

                #region 获取工作报告

                if (workreport_handle.list_All == null)
                {
                    workreport_handle.list_All = Constant.Get_all_workreport();
                }

                #endregion

                #region 获取分享圈

                if (circle_share_handle.list_All == null)
                {
                    circle_share_handle.list_All = Constant.Get_all_share();
                }

                #endregion

                #region 获取评论

                if (comment_handle.list_All == null)
                {
                    comment_handle.list_All = Constant.Get_all_comment();
                }

                #endregion

                #region 获取点赞

                if (praise_handle.list_All == null)
                {
                    praise_handle.list_All = Constant.Get_all_praise();
                }

                #endregion

                #region 获取所有图片

                if (list_picture_All == null)
                {
                    list_picture_All = Constant.Get_all_picture();
                }

                #endregion

                //定时释放垃圾
                Dispose_All();

                if (string.IsNullOrEmpty(All_user_data))
                {
                    //获取用户中心所有用户
                    Get_all_user();
                }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 参数填充
        /// </summary>
        private static void pub_paramters_fill_center()
        {
            try
            {
                if (pub_param_handle.dicCustomer == null)
                {
                    pub_param_handle.dicCustomer = Get_all_pub_param();

                    if (pub_param_handle.dic_customer_Level == null)
                    {
                        //跟进类型、联系人级别
                        List<pub_param> pub_param_parent = link_type_level_fill();
                        //客户级别 客户类型 客户属性
                        customer_level_grade_property_fill(pub_param_parent);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 客户级别 客户类型 客户属性
        /// </summary>
        /// <param name="pub_param_parent"></param>
        private static void customer_level_grade_property_fill(List<pub_param> pub_param_parent)
        {
            try
            {
                //客户级别
                pub_param pub_customer_level = pub_param_parent.FirstOrDefault(t => t.pub_title == "客户级别");
                if (pub_customer_level != null)
                {
                    List<pub_param> list_customer_level = (from t in pub_param_handle.dicCustomer
                                                           where t.pub_parentid == pub_customer_level.id
                                                           orderby t.pub_value ascending
                                                           select t).ToList();
                    pub_param_handle.dic_customer_Level_TC = ConverList<pub_param>.ListToDic(list_customer_level);
                    pub_param_handle.dic_customer_Level = ParamtersToDic(list_customer_level);
                }
                //客户类型
                pub_param pub_customer_type = pub_param_parent.FirstOrDefault(t => t.pub_title == "客户类型");
                if (pub_customer_type != null)
                {
                    List<pub_param> list_customer_type = (from t in pub_param_handle.dicCustomer
                                                          where t.pub_parentid == pub_customer_type.id
                                                          orderby t.pub_value ascending
                                                          select t).ToList();
                    pub_param_handle.dic_customer_Type_TC = ConverList<pub_param>.ListToDic(list_customer_type);
                    pub_param_handle.dic_customer_Type = ParamtersToDic(list_customer_type);
                }
                //客户属性
                pub_param pub_customer_property = pub_param_parent.FirstOrDefault(t => t.pub_title == "客户属性");
                if (pub_customer_property != null)
                {
                    List<pub_param> list_customer_property = (from t in pub_param_handle.dicCustomer
                                                              where t.pub_parentid == pub_customer_property.id
                                                              orderby t.pub_value ascending
                                                              select t).ToList();
                    pub_param_handle.dic_customer_Property_TC = ConverList<pub_param>.ListToDic(list_customer_property);
                    pub_param_handle.dic_customer_Property = ParamtersToDic(list_customer_property);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 跟进类型、联系人级别
        /// </summary>
        /// <returns></returns>
        private static List<pub_param> link_type_level_fill()
        {
            List<pub_param> pub_param_parent = pub_param_handle.dicCustomer.Where(t => t.pub_parentid == 0).ToList();
            try
            {
                //跟进类型
                pub_param pub_fllow_type = pub_param_parent.FirstOrDefault(t => t.pub_title == "跟进类型");
                if (pub_fllow_type != null)
                {
                    List<pub_param> list__fllow_type = (from t in pub_param_handle.dicCustomer
                                                        where t.pub_parentid == pub_fllow_type.id
                                                        select t).OrderBy(x => x.id).ToList();
                    pub_param_handle.dic_follow_Level_TC = ConverList<pub_param>.ListToDic(list__fllow_type);
                    pub_param_handle.dic_follow_Level = ParamtersToDic(list__fllow_type);
                }


                //联系人级别
                pub_param pub_linkman_grade = pub_param_parent.FirstOrDefault(t => t.pub_title == "联系人级别");
                if (pub_linkman_grade != null)
                {
                    List<pub_param> list_linkman_grade = (from t in pub_param_handle.dicCustomer
                                                          where t.pub_parentid == pub_linkman_grade.id
                                                          orderby t.pub_value ascending
                                                          select t).ToList();
                    pub_param_handle.dic_linkMan_Grade_TC = ConverList<pub_param>.ListToDic(list_linkman_grade);
                    pub_param_handle.dic_linkMan_Grade = ParamtersToDic(list_linkman_grade);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return pub_param_parent;
        }

        /// <summary>
        /// 客户 联系人数据填充
        /// </summary>
        private static void customer_linkman_Fil()
        {
            try
            {
                //通过该方式进行缓存应用【客户信息】
                if (cust_customer_handle.list_All == null)
                {
                    cust_customer_handle.list_All = Constant.Get_all_Customer();
                }

                if (cust_linkman_handle.list_All == null)
                {
                    cust_linkman_handle.list_All = Constant.Get_all_linkman();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 信息初始化

        public static void Role_Cacle_Init(HttpContext context)
        {
            HttpRequest Request = context.Request;
            try
            {
                string guid = RequestHelper.string_transfer(Request, "cust_users");
                if (!string.IsNullOrEmpty(guid))
                {
                    List<UserInfo> _UserInfoL;
                    List<string> Common_Admin_List;
                    List<string> list_C;
                    //组织架构准备
                    org_data_prepare(Request, out _UserInfoL, out Common_Admin_List, out list_C);
                    //是否需要强制进行数据刷新【当前用户角色变更时需要重新绑定数据源】
                    bool Need_force_Reflesh = false;
                    //将所有同一部门的人加入
                    foreach (var item in _UserInfoL)
                    {
                        //角色名称
                        string roleName = item.RoleName;
                        //用户编号
                        string uniqueNo = item.UniqueNo;
                        //管理者添加
                        if (roleName.Contains("超级管理员-CRM"))
                        {
                            //管理者添加
                            Need_force_Reflesh = super_admin_collect(guid, Common_Admin_List, Need_force_Reflesh, uniqueNo);
                        }
                        else if (roleName.Contains("部门总监-CRM"))
                        {
                            //部门总监-CRM
                            Need_force_Reflesh = common_admin_collect(guid, _UserInfoL, Common_Admin_List, list_C, Need_force_Reflesh, uniqueNo);
                        }
                        ////普通成员【给每个用户指定总监列表】
                        //else if (string.IsNullOrEmpty(roleName))
                        //{
                        //    //普通成员
                        //    Need_force_Reflesh = common_memmber_collect(guid, Common_Admin_List, Need_force_Reflesh, uniqueNo);
                        //}
                        else
                        {
                            //普通成员
                            Need_force_Reflesh = common_memmber_collect(guid, Common_Admin_List, Need_force_Reflesh, uniqueNo);
                        }
                    }

                    //角色数据分配
                    role_data_fill(guid, Need_force_Reflesh);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 组织架构准备
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="_UserInfoL"></param>
        /// <param name="Common_Admin_List"></param>
        /// <param name="list_C"></param>
        private static void org_data_prepare(HttpRequest Request, out List<UserInfo> _UserInfoL, out List<string> Common_Admin_List, out List<string> list_C)
        {
            //组织架构
            string orgData = Request["orgData"];
            //用户信息
            _UserInfoL = JsonToList<UserInfo>(orgData);
            //总监 UserID
            Common_Admin_List = new List<string>();
            //所有成员的ID收集和总监的收集
            list_C = new List<string>();

            foreach (var item2 in _UserInfoL)
            {
                //只提交总监
                if (item2.RoleName.Contains("部门总监-CRM"))
                {
                    Common_Admin_List.Add(item2.UniqueNo);
                }
                //所有总监和普通员工
                list_C.Add(item2.UniqueNo);
            }
        }

        /// <summary>
        /// 普通成员
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="Common_Admin_List"></param>
        /// <param name="Need_force_Reflesh"></param>
        /// <param name="uniqueNo"></param>
        /// <returns></returns>
        private static bool common_memmber_collect(string guid, List<string> Common_Admin_List, bool Need_force_Reflesh, string uniqueNo)
        {
            try
            {
                //普通成员 【可以重新设置】
                if (!dic_custs_users.ContainsKey(uniqueNo))
                {
                    dic_custs_users.Add(uniqueNo, Common_Admin_List);
                }
                else
                {
                    //重置
                    dic_custs_users[uniqueNo] = Common_Admin_List;
                }
                if (dicLimit_P.ContainsKey(uniqueNo))
                {
                    //若管理者位置存在则进行移除【说明角色已经有变动】
                    dicLimit_P.Remove(uniqueNo);

                    //进行重新绑定数据源
                    if (uniqueNo == guid)
                    {
                        Need_force_Reflesh = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Need_force_Reflesh;
        }

        /// <summary>
        /// 部门总监
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="_UserInfoL"></param>
        /// <param name="Common_Admin_List"></param>
        /// <param name="list_C"></param>
        /// <param name="Need_force_Reflesh"></param>
        /// <param name="uniqueNo"></param>
        /// <returns></returns>
        private static bool common_admin_collect(string guid, List<UserInfo> _UserInfoL, List<string> Common_Admin_List, List<string> list_C, bool Need_force_Reflesh, string uniqueNo)
        {
            try
            {
                //管理者
                Admin_CS admin_cs = new Admin_CS();
                if (!dicLimit_P.ContainsKey(uniqueNo))
                {
                    //加入管理者
                    dicLimit_P.Add(uniqueNo, admin_cs);
                    //将所有部门内的成员交与部门总监
                    admin_cs.List_Memmber = list_C;
                    admin_cs.List_Uni_UserName = _UserInfoL;

                    if (uniqueNo == guid)
                    {
                        //总监第一次加载或者由普通成员转变
                        Need_force_Reflesh = true;
                    }
                }
                else
                {
                    //进行重新绑定数据源
                    if (uniqueNo == guid && dicLimit_P[uniqueNo].LimitType == LimitType.Super_Admin)
                    {
                        //设置为总监
                        dicLimit_P[uniqueNo].LimitType = LimitType.Common_Admin;
                        Need_force_Reflesh = true;
                    }
                }
                //设置级别
                admin_cs.LimitType = LimitType.Common_Admin;

                if (!dic_custs_users.ContainsKey(uniqueNo))
                {
                    //管理员加载一个空列表
                    dic_custs_users.Add(uniqueNo, Common_Admin_List);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Need_force_Reflesh;
        }

        /// <summary>
        /// 超级管理员数据分配
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="Common_Admin_List"></param>
        /// <param name="Need_force_Reflesh"></param>
        /// <param name="uniqueNo"></param>
        /// <returns></returns>
        private static bool super_admin_collect(string guid, List<string> Common_Admin_List, bool Need_force_Reflesh, string uniqueNo)
        {
            try
            {
                //管理者
                Admin_CS admin_cs = new Admin_CS();
                if (!dicLimit_P.ContainsKey(uniqueNo))
                {
                    //加入管理者
                    dicLimit_P.Add(uniqueNo, admin_cs);

                    if (uniqueNo == guid)
                    {
                        //管理者第一次加载或者由普通成员转变
                        Need_force_Reflesh = true;
                    }
                }
                else
                {
                    //进行重新绑定数据源
                    if (uniqueNo == guid && dicLimit_P[uniqueNo].LimitType == LimitType.Common_Admin)
                    {
                        //设置为管理者
                        dicLimit_P[uniqueNo].LimitType = LimitType.Super_Admin;
                        Need_force_Reflesh = true;
                    }
                }
                //设置级别
                admin_cs.LimitType = LimitType.Super_Admin;

                if (!dic_custs_users.ContainsKey(uniqueNo))
                {
                    //管理员加载一个空列表
                    dic_custs_users.Add(uniqueNo, Common_Admin_List);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Need_force_Reflesh;
        }

        /// <summary>
        /// 角色数据分配
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="Need_force_Reflesh"></param>
        private static void role_data_fill(string guid, bool Need_force_Reflesh)
        {
            try
            {
                //角色类型
                LimitType limitType = LimitType.Common_Memmber;

                //当前若为管理员，拿出级别与成员【朝官】
                List<string> parameter_List = new List<string>();
                if (dicLimit_P.ContainsKey(guid))
                {
                    limitType = dicLimit_P[guid].LimitType;
                    //总监级别，直接给下属的名单列表
                    if (limitType == LimitType.Common_Admin)
                    {
                        parameter_List = dicLimit_P[guid].List_Memmber;
                    }
                }
                //局部初始化【超级管理员、普通管理员、普通成员】
                Constant.DataInit_Current_User_Using(guid, limitType, parameter_List, Need_force_Reflesh);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        #endregion

        #region 初始化当前程序使用数据

        /// <summary>
        /// 当前用户使用者
        /// </summary>
        /// <param name="guid">用户身份GUID</param>
        /// <param name="limitType">用户身份</param>
        /// <param name="stringLi">成员列表，普通用户和超级管理员不需要</param>
        /// <param name="Need_force_Reflesh">是否进行强制重新数据绑定,根据用户身份是否变更为判断依据</param>
        public static void DataInit_Current_User_Using(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //局部客户
                part_customer_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部联系人
                part_linkman_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部工作计划
                part_workplane_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部跟进记录
                part_follow_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部提醒
                part_remind_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部提醒设置
                part_remind_setting_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部工作报告
                part_workport_relate(guid, limitType, stringLi, Need_force_Reflesh);

                //局部签到
                part_sign_relate(guid, limitType, stringLi, Need_force_Reflesh);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_sign_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的签到列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的签到，直接获取
                    if (!sign_in_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<sign_in> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = sign_in_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in sign_in_handle.list_All
                                         where stringLi.Contains(t.sign_userid)
                                         select t).ToList();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in sign_in_handle.list_All
                                         where t.sign_userid == guid
                                         select t).ToList();
                                break;
                            default:
                                break;
                        }
                        if (sign_in_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            sign_in_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            sign_in_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_workport_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的工作报告列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的工作报告，直接获取
                    if (!workreport_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<workreport> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = workreport_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in workreport_handle.list_All
                                         where stringLi.Contains(t.report_userid)
                                         select t).ToList();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in workreport_handle.list_All
                                         where t.report_userid == guid
                                         select t).ToList();
                                break;
                            default:
                                break;
                        }
                        if (workreport_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            workreport_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            workreport_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_remind_setting_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的提醒列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的提醒，直接获取
                    if (!remind_setting_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<remind_setting> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = remind_setting_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in remind_setting_handle.list_All
                                         where stringLi.Contains(t.remind_userid)
                                         select t).ToList<remind_setting>();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in remind_setting_handle.list_All
                                         where t.remind_userid == guid
                                         select t).ToList<remind_setting>();
                                break;
                            default:
                                break;
                        }
                        if (remind_setting_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            remind_setting_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            remind_setting_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_remind_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的提醒列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的提醒，直接获取
                    if (!remind_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<remind> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = remind_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in remind_handle.list_All
                                         where stringLi.Contains(t.rem_userid)
                                         select t).ToList<remind>();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in remind_handle.list_All
                                         where t.rem_userid == guid
                                         select t).ToList<remind>();
                                break;
                            default:
                                break;
                        }
                        if (remind_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            remind_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            remind_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_follow_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的提醒列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的提醒，直接获取
                    if (!follow_up_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<follow_up> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = follow_up_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in follow_up_handle.list_All
                                         where stringLi.Contains(t.follow_userid)
                                         select t).ToList();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in follow_up_handle.list_All
                                         where t.follow_userid == guid
                                         select t).ToList();
                                break;
                            default:
                                break;
                        }
                        if (follow_up_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            follow_up_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            follow_up_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_workplane_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的工作计划列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的工作计划，直接获取
                    if (!workplan_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<workplan> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = workplan_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in workplan_handle.list_All
                                         where stringLi.Contains(t.wp_userid)
                                         select t).ToList();
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in workplan_handle.list_All
                                         where t.wp_userid == guid
                                         select t).ToList();
                                break;
                            default:
                                break;
                        }

                        if (workplan_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            workplan_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            workplan_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_linkman_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的联系人列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的联系人，直接获取
                    if (!cust_linkman_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<cust_linkman> list1 = null;
                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = cust_linkman_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in cust_linkman_handle.list_All
                                         from user in stringLi
                                         where t.link_users.Contains(user)
                                         select t).Distinct().ToList();

                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in cust_linkman_handle.list_All
                                         where t.link_users.Contains(guid)
                                         select t).ToList();
                                break;
                            default:
                                break;
                        }
                        if (cust_linkman_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            cust_linkman_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            cust_linkman_handle.dic_Self.Add(guid, list1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private static void part_customer_relate(string guid, LimitType limitType, List<string> stringLi, bool Need_force_Reflesh)
        {
            try
            {
                //全局获取当前人的客户列表
                if (!string.IsNullOrEmpty(guid))
                {
                    //若有当前人的客户，直接获取
                    if (!cust_customer_handle.dic_Self.ContainsKey(guid) || Need_force_Reflesh)
                    {
                        List<cust_customer> list1 = null;

                        switch (limitType)
                        {
                            //超级管理员
                            case LimitType.Super_Admin:
                                list1 = cust_customer_handle.list_All;
                                break;
                            //普通管理员
                            case LimitType.Common_Admin:
                                list1 = (from t in cust_customer_handle.list_All
                                         from user in stringLi
                                         where t.cust_users.Contains(user)
                                         select t).Distinct().ToList();
                                List<cust_customer> lst_pub = Constant.get_customer_customer();
                                foreach (var item in lst_pub)
                                {
                                    if (!list1.Contains(item))
                                    {
                                        list1.Add(item);
                                    }
                                }
                                break;
                            //普通员工
                            case LimitType.Common_Memmber:
                                list1 = (from t in cust_customer_handle.list_All
                                         where t.cust_users.Contains(guid)
                                         select t).ToList();
                                List<cust_customer> lst_pub2 = Constant.get_customer_customer();
                                foreach (var item in lst_pub2)
                                {
                                    if (!list1.Contains(item))
                                    {
                                        list1.Add(item);
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        if (cust_customer_handle.dic_Self.ContainsKey(guid))
                        {
                            //身份变更重新绑定【使用半途身份变更】
                            cust_customer_handle.dic_Self[guid] = list1;
                        }
                        else
                        {
                            //第一次绑定数据（改用户）
                            cust_customer_handle.dic_Self.Add(guid, list1);
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

        #region 初始化（提升性能）

        /// <summary>
        /// 辅助方法【公共参数】
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParamtersToDic(List<pub_param> list)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                foreach (var item in list)
                {
                    result.Add(Convert.ToString(item.pub_value), item.pub_title);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;

        }

        #endregion

        #region 定时释放垃圾

        static System.Timers.Timer Glob_Timer = null;

        /// <summary>
        /// 释放用不上的缓存（计算机运算时产生的缓存垃圾）【不包含从数据库去除的缓存数据】
        /// </summary>
        public static void Dispose_All()
        {
            if (Glob_Timer == null)
            {
                Glob_Timer = new System.Timers.Timer();
                Glob_Timer.Interval = 1000 * 60 * 10;
                Glob_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    GC_Helper();
                };
            }
        }

        public static void GC_Helper()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// JSON格式字符转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseFormByJson<T>(string jsonStr)
        {
            T obj = Activator.CreateInstance<T>();
            using (System.IO.MemoryStream ms =
            new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonStr)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer =
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Json转换成List集合，返回对象List
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="jsonString">反序列化字符串</param>
        /// <returns>反序列化后的值</returns>
        public static List<T> JsonToList<T>(string jsonString)
        {
            return ParseFormByJson<List<T>>(jsonString);
        }

        /// <summary>
        ///页面数据回调
        /// </summary>
        /// <param name="errNum"></param>
        /// <param name="errMsg"></param>
        /// <param name="retData"></param>
        /// <returns></returns>
        public static JsonModel get_jsonmodel(int errNum, string errMsg, object retData)
        {
            return new JsonModel()
            {
                errNum = errNum,
                errMsg = errMsg,
                retData = retData,
            };
        }

        /// <summary>
        ///页面数据回调
        /// </summary>
        /// <param name="errNum"></param>
        /// <param name="errMsg"></param>
        /// <param name="retData"></param>
        /// <returns></returns>
        public static JsonModel get_jsonmodel(int errNum, string errMsg, object retData, string status)
        {
            return new JsonModel()
            {
                errNum = errNum,
                errMsg = errMsg,
                retData = retData,
                status = status
            };
        }

        #endregion

        #region 获取经纬度的实际距离

        private const double EARTH_RADIUS = 6378.137;//地球半径
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        #endregion

        #region 从用户中心获取所有用户

        /// <summary>
        /// 从用户中心获取所有用户
        /// </summary>
        public static void Get_all_user()
        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(service_url + "&OrganNo=1000" + "&Status=1");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 10000;
            request.AllowAutoRedirect = false;
            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                    //获取所有用户
                    All_user_data = responseStr;
                    Root root = JsonConvert.DeserializeObject<Root>(responseStr);

                    //获取所有组织机构
                    if (root.result.orgData != null)
                    {
                        //先清除数据
                        dic_All_user.Clear();
                        DepartMent_List.Clear();

                        List<RetData> retData = root.result.retData;

                        //部门列表
                        string[] depart = Split_Hepler.str_to_stringss(Constant.Sale_DepartmentList);
                        //部门ID
                        string[] depart_value = Split_Hepler.str_to_stringss(Constant.Sale_DepartmentList_Value);
                        //遍历的方式填充所有用户列表
                        foreach (var item in retData)
                        {
                            UserInfo userInfo = new UserInfo() { RegisterOrg = item.RegisterOrg, UniqueNo = item.UniqueNo, Name = item.Name, RoleName = item.rowNum };
                            if (depart_value.Contains(item.RegisterOrg))
                            {
                                List_All_RelateUserInfo.Add(userInfo);
                            }
                            dic_All_user.Add(userInfo);
                        }
                        //获取部门信息
                        if (depart.Count() == depart_value.Count())
                        {
                            for (int i = 0; i < depart.Count(); i++)
                            {
                                //设置并获取部门
                                DepartMent departMent = new DepartMent() { ID = depart_value[i], Name = depart[i] };
                                departMent.UserInfo_List = (from t in dic_All_user
                                                            where t.RegisterOrg == departMent.ID
                                                            select t).ToList();
                                //获取该部门的总监
                                UserInfo user_leader = departMent.UserInfo_List.FirstOrDefault(u => u.RegisterOrg == departMent.ID && u.RoleName.Contains("部门总监-CRM"));
                                if (user_leader != null)
                                {
                                    departMent.Leader_Guid = user_leader.UniqueNo;
                                }
                                DepartMent_List.Add(departMent);

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }
        }

        #endregion

        #region 判断是否同一个星期


        /// <summary>   
        /// 判断两个日期是否在同一周   
        /// </summary>   
        /// <param name="dtmS">开始日期</param>   
        /// <param name="dtmE">结束日期</param>  
        /// <returns></returns>   
        public static bool IsInSameWeek(DateTime dtmS, DateTime dtmE)
        {
            bool result = false;
            int s1 = (int)dtmS.DayOfWeek;
            if (s1 == 0)
            {
                s1 = 7;
            }
            int s2 = (int)dtmE.DayOfWeek;
            if (s2 == 0)
            {
                s2 = 7;
            }
            DateTime temp1 = dtmS.AddDays(-s1).Date;
            DateTime temp2 = dtmE.AddDays(-s2).Date;
            if (temp1 == temp2)
            {
                result = true;
            }
            return result;
        }

        #endregion

        #region 获取公共客户


        /// <summary>
        ///获取公共客户
        /// </summary>
        public static List<cust_customer> get_customer_customer()
        {
            List<cust_customer> cust_customer_pub = null;
            try
            {
                cust_customer_pub = cust_customer_handle.list_All.Where(t => t.cust_category == 1).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                if (cust_customer_pub == null)
                {
                    cust_customer_pub = new List<cust_customer>();
                }
            }

            return cust_customer_pub;
        }

        #endregion

        #region 判断某人是否在组织机构

        public static bool Chek_Someone_Is_Valied(string guid)
        {
            bool result = false;
            try
            {
                var is_valied = Constant.dic_All_user.FirstOrDefault(v => v.UniqueNo == guid);
                if (is_valied != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 获取当前人的权限

        public static LimitType Get_self_limit(string guid)
        {
            LimitType LimitType = LimitType.Common_Memmber;
            try
            {
                //取出它的成员
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    Admin_CS admin_cs = Constant.dicLimit_P[guid];
                    LimitType = Constant.dicLimit_P[guid].LimitType;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return LimitType;
        }

        #endregion

    }
}