
using CRM_Common;
using CRM_Handler.Admin;
using CRM_Handler.Common;
using CRM_Handler.Custom;
using CRM_Handler.LinkMan;
using CRM_Handler.Statistical;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM_Handler.File
{
    /// <summary>
    /// HandlerFileLeadIn 的摘要说明
    /// </summary>
    public class HandlerFileLeadIn : IHttpHandler
    {

        #region 字段

        /// <summary>
        /// 高德获取经纬度接口
        /// </summary>
        static string gaode_api_get_position = "http://restapi.amap.com/v3/geocode/geo?key=f07e75646876646ed34a84f363d51891&s=rsv3&city=35&address=";

        /// <summary>
        /// 公共客户
        /// </summary>
        static string pub_cust_name = "公共客户";

        #endregion

        #region 显示页面信息

        static string _showPage_info = string.Empty;

        /// <summary>
        /// 成功添加的客户数量
        /// </summary>
        int add_customer_count = 0;
        /// <summary>
        /// 成功添加的联系人数量
        /// </summary>
        int add_linkman_count = 0;
        /// <summary>
        /// 已存在的客户数量
        /// </summary>
        int exit_customer_count = 0;
        /// <summary>
        /// 已存在的联系人数量
        /// </summary>
        int exit_linkman_count = 0;

        #endregion
        public List<string> reinfo = new List<string>();
        public List<Dictionary<string, string>> Failinfo = new List<Dictionary<string, string>>();
        public Dictionary<string, int> Successinfo = new Dictionary<string, int>();
        
        public void ProcessRequest(HttpContext context)
        {
            
            context.Response.ContentType = "text/plain";
            HttpRequest Request = context.Request;
            try
            {
                _showPage_info = string.Empty;

                string root = HttpContext.Current.Server.MapPath("~/download/人员模板.xlsx");
                string excelPath = root;
                

                if (context.Request.Files.Count > 0)
                {
                    HttpPostedFile file = context.Request.Files[0];
                    //上传word文件, 由于只是做示例，在这里不多做文件类型、大小、格式以及是否存在的判断
                    file.SaveAs(excelPath);


                    //从Excel中读取客户和联系人
                    List<DataTable> dtList = ExcelHelper.readExcel(excelPath);
                    if (dtList.Count > 0)
                    {
                        //数据转译
                        List<Dictionary<string, object>> dic_data = DataTableToList(dtList[0]);
                        //当前负责人名称

                        string userName = context.Request.Form["userName"];
                      

                        //开始进行更改
                        begin_cust_linkman(dic_data, userName);
                        string info = Constant.jss.Serialize(reinfo);
                        context.Response.Write(Constant.jss.Serialize(new { Result = info, Result_Success = Successinfo, Result_Fail = Failinfo }));
                    }
                }
                else
                {
                    //请选择文件
                    //reinfo = "请选择文件";
                }
            }
            catch (Exception ex)
            {
                //reinfo = "出现异常，请联系管理员";
                LogHelper.Error(ex);
            }
            finally
            {


                //TextBox2.Text = _showPage_info;

            }
        }
        private void begin_cust_linkman(List<Dictionary<string, object>> dic_data, string userName)
        {
            try
            {
                if (!string.IsNullOrEmpty(userName))
                {
                    if (Constant.dic_All_user == null)
                    {
                        Constant.Get_all_user();
                    }
                    ///保证缓存数据存在
                    if (cust_customer_handle.list_All == null || cust_linkman_handle.list_All == null)
                    {
                        Constant.Fill_All_Data(null);
                    }

                    //用户中心里的数据进行匹配
                    List<UserInfo> listU = Constant.dic_All_user.Where(item => item.Name == userName).ToList<UserInfo>();

                    //验证负责人在组织架构里
                    if ((listU != null && listU.Count > 0) || userName == pub_cust_name)
                    {
                        if (dic_data != null && dic_data.Count > 0)
                        {
                            if (dic_data[0].Count >= 14)
                            {

                                //添加客户和联系人【普通】
                                add_custom_linkman(dic_data, userName, listU);
                             
                                Successinfo.Add("add_customer_count", add_customer_count);
                                Successinfo.Add("add_linkman_count", add_linkman_count);
                                Successinfo.Add("exit_customer_count", exit_customer_count);
                                Successinfo.Add("exit_linkman_count", exit_linkman_count);

                                //Successinfo.Add("成功导入客户数量：" + add_customer_count + "。成功导入联系人数量：" + add_linkman_count + "。已存在客户数量：" + exit_customer_count + "。已存在联系人数量：" + exit_linkman_count);
                                
                            }
                            else
                            {
                              
                                reinfo.Add("excel模板不正确，请联系管理员");
                            }
                        }
                        else
                        {
                            reinfo.Add("未读取到excel数据");
                        }
                    }
                    else
                    {
                        reinfo.Add("用户不存在或未授权");
                        
                    }
                }
                else
                {
                    reinfo.Add("用户名不存在或未授权");
                  
                }
            }
            catch (Exception ex)
            {
                 reinfo.Add("出现异常，请联系管理员");
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }



        #region 添加客户和联系人

        /// <summary>
        /// 添加客户和联系人
        /// </summary>
        /// <param name="dic_data">excel拿出的数据</param>
        /// <param name="userName">用户名称</param>
        /// <param name="listU">用户中心数据</param>
        private void add_custom_linkman(List<Dictionary<string, object>> dic_data, string userName, List<UserInfo> listU)
        {
            try
            {
                string uniQ = string.Empty;
                if (userName == pub_cust_name)
                {
                    uniQ = "00000000-0000-0000-0000-00000000";
                }
                else
                {
                    uniQ = listU[0].UniqueNo;
                }
                for (int i = 3; i < dic_data.Count; i++)
                {
                    //增加客户
                    string customName = Convert.ToString(dic_data[i]["F2"]).Trim();
                    List<cust_customer> customers = null;
                    if (userName == pub_cust_name)
                    {
                        customers = cust_customer_handle.list_All.Where(item => item.cust_name.Trim() == customName && item.cust_category == 1).ToList<cust_customer>();
                    }
                    else
                    {
                        customers = cust_customer_handle.list_All.Where(item => item.cust_name.Trim() == customName).ToList<cust_customer>();
                    }
                    if (!string.IsNullOrEmpty(customName))
                    {
                        long customerID = 0;
                        if (customers.Count == 0)
                        {
                            //添加客户
                            customerID = custom_add(dic_data, userName, uniQ, i, customName, customerID);
                        }
                        else
                        {
                            //修改客户
                            customerID = custom_edit(dic_data, i, customers, customerID);
                        }
                        //添加联系人
                        add_edit_linkman_center(dic_data, userName, uniQ, i, customName, customerID);
                    }
                }
            }
            catch (Exception ex)
            {
           
                reinfo.Add("出现异常，请联系管理员");
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 客户添加
        /// </summary>
        /// <param name="dic_data"></param>
        /// <param name="i"></param>
        /// <param name="customers"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        private long custom_edit(List<Dictionary<string, object>> dic_data, int i, List<cust_customer> customers, long customerID)
        {
            cust_customer cust_select = customers[0];
            //获取联系人ID
            customerID = (long)cust_select.id;
            if (Constant.IsInput_CoverAddress)
            {
                //设置经纬度
                customer_position_setting(dic_data, i, cust_select);
            }
            //数据库更改客户信息
            Constant.cust_customer_S.Update(cust_select);
            exit_customer_count++;
            Dictionary<string, string> msg = new Dictionary<string, string>();
            msg.Add("cust_name", cust_select.cust_name);
            msg.Add("cust_usersname", cust_select.cust_usersname);
            msg.Add("msg", "该客户已存在");
            Failinfo.Add(msg);
            reinfo.Add(i + "、该客户已存在：" + cust_select.cust_name + "      负责人：" + customers[0].cust_usersname);           
            return customerID;
        }

        /// <summary>
        /// 客户编辑
        /// </summary>
        /// <param name="dic_data"></param>
        /// <param name="userName"></param>
        /// <param name="uniQ"></param>
        /// <param name="i"></param>
        /// <param name="customName"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        private long custom_add(List<Dictionary<string, object>> dic_data, string userName, string uniQ, int i, string customName, long customerID)
        {
            long custom_id = -1;
            try
            {
                cust_customer cust_customer = new CRM_Model.cust_customer()
                {
                    //客户名称
                    cust_name = customName,
                    cust_followdate = DateTime.Now,
                    cust_isdelete = "0",
                    cust_createdate = DateTime.Now,
                    cust_creator = 2,
                    //用户名称
                    cust_usersname = userName,
                    cust_level = 3,
                    cust_parent_id = 0,
                    cust_type = 1,
                    cust_updatedate = DateTime.Now,
                    cust_updateuser = 2,
                    cust_users = uniQ,
                    cust_category = 0,
                };
                if (userName == pub_cust_name)
                {
                    cust_customer.cust_category = 1;
                }

                //设置经纬度
                customer_position_setting(dic_data, i, cust_customer);

                //增加客户
                JsonModel jsModel = Constant.cust_customer_S.Add(cust_customer);
                customerID = Convert.ToInt64(jsModel.retData);

                cust_customer.id = customerID;
                custom_id = customerID;

                //添加用户【自身】
                if (cust_customer_handle.dic_Self.ContainsKey(uniQ))
                {
                    if (!cust_customer_handle.dic_Self[uniQ].Contains(cust_customer))
                    {
                        cust_customer_handle.dic_Self[uniQ].Add(cust_customer);
                    }
                }
                cust_customer_handle.admin_add_customer(uniQ, cust_customer);

                if (customerID > 0)
                {
                    add_customer_count++;
                    reinfo.Add(i + "、成功提交客户：" + customName + "      负责人：" + userName);     
                   
                  
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return custom_id;
        }

        /// <summary>
        /// 位置设置
        /// </summary>
        /// <param name="dic_data">excel拿出的数据</param>
        /// <param name="i">循环变量</param>
        /// <param name="cust_customer">客户</param>
        private void customer_position_setting(List<Dictionary<string, object>> dic_data, int i, cust_customer cust_customer)
        {
            try
            {
                string address = Convert.ToString(dic_data[i]["F12"]).Trim();

                if (!string.IsNullOrEmpty(address))
                {
                    String addressStr = gaode_api_get_position + address;

                    Position_Model data = GaoDeHelper.GetPosition(addressStr, "UTF-8");
                    if (data.geocodes.Count > 0)
                    {
                        string location = data.geocodes[0].location;
                        if (!string.IsNullOrEmpty(location))
                        {
                            cust_customer.cust_x = Convert.ToDecimal(location.Split(new char[] { ',' })[0]);
                            cust_customer.cust_y = Convert.ToDecimal(location.Split(new char[] { ',' })[1]);
                            if (cust_customer.cust_usersname == pub_cust_name)
                            {
                                cust_customer.cust_GaoDe = address;
                                cust_customer.cust_address = data.geocodes[0].formatted_address;
                            }
                            else
                            {
                                cust_customer.cust_GaoDe = data.geocodes[0].formatted_address;
                                cust_customer.cust_address = address;
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

        #region 添加或编辑联系人

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="dic_data">excel拿出的数据</param>
        /// <param name="userName">用户名称</param>
        /// <param name="uniQ">用户身份</param>
        /// <param name="i">循环变量</param>
        /// <param name="customName">客户名称</param>
        /// <param name="customerID">客户ID</param>
        private void add_edit_linkman_center(List<Dictionary<string, object>> dic_data, string userName, string uniQ, int i, string customName, long customerID)
        {
            try
            {
                string link_name = Convert.ToString(dic_data[i]["F5"]).Trim();

                //不可重复录入联系人，或者联系人名称为空不给予录入
                if (!string.IsNullOrEmpty(link_name))
                {
                    //手机号码
                    string link_phonenumber = Convert.ToString(dic_data[i]["F9"]).Trim();

                    #region 联系人生日和等级

                    //联系人生日
                    string birday = Convert.ToString(dic_data[i]["F7"]).Trim();
                    DateTime d_1 = Convert.ToDateTime("1800-01-01");
                    if (DateTime.TryParse(birday, out d_1))
                    {
                    }
                    else
                    {
                        d_1 = Convert.ToDateTime("1800-01-01");
                    }

                    //初始化联系人等级【默认潜在】
                    int link_level = 3;

                    string link_level_string = Convert.ToString(dic_data[i]["F13"]).Trim();
                    if (link_level_string.Contains("A") || link_level_string.Contains("a"))
                    {
                        link_level = 0;
                    }
                    else if (link_level_string.Contains("B") || link_level_string.Contains("b"))
                    {
                        link_level = 1;
                    }
                    else if (link_level_string.Contains("C") || link_level_string.Contains("c"))
                    {
                        link_level = 2;
                    }
                    else if (link_level_string.Contains("D") || link_level_string.Contains("d"))
                    {
                        link_level = 3;
                    }

                    #endregion

                    string sex = Convert.ToString(dic_data[i]["F6"]).Trim();
                    string position = Convert.ToString(dic_data[i]["F8"]).Trim();
                    string email = Convert.ToString(dic_data[i]["F11"]).Trim();
                    string telep = Convert.ToString(dic_data[i]["F10"]).Trim();
                    string remark = Convert.ToString(dic_data[i]["F14"]).Trim();



                    //获取客户名称一致,联系人名称一致，手机号码一致的联系人
                    List<cust_linkman> customers = cust_linkman_handle.list_All.Where(item => item.link_cust_name.Trim() == customName.Trim() && item.link_position.Trim() == position.Trim() &&
                       item.link_sex.Trim() == sex.Trim()
                        && item.link_name.Trim() == link_name && item.link_phonenumber.Trim() == link_phonenumber).ToList();
                    if (customers.Count == 0)
                    {
                        linkman_add(dic_data, userName, uniQ, i, customName, customerID, link_name, link_phonenumber, d_1, link_level, sex, position, email, telep, remark);
                    }
                    else
                    {
                        linkman_edit(dic_data, userName, i, link_name, d_1, link_level, sex, position, email, telep, remark, customers);
                    }
                }
            }
            catch (Exception ex)
            {
                reinfo.Add("出现异常，请联系管理员");
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="dic_data">excel数据</param>
        /// <param name="userName">用户名称</param>
        /// <param name="uniQ">用户guid</param>
        /// <param name="i">序号</param>
        /// <param name="customName">客户名称</param>
        /// <param name="customerID">客户id</param>
        /// <param name="link_name">联系人名称</param>
        /// <param name="link_phonenumber">手机号码</param>
        /// <param name="d_1">生日</param>
        /// <param name="link_level">联系人等级</param>
        /// <param name="sex">性别</param>
        /// <param name="position">职务</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="telep">电话号码</param>
        /// <param name="remark">联系人备注</param>
        private void linkman_add(List<Dictionary<string, object>> dic_data, string userName, string uniQ, int i, string customName, long customerID, string link_name, string link_phonenumber, DateTime d_1, int link_level, string sex, string position, string email, string telep, string remark)
        {
            try
            {
                cust_linkman linkman = new CRM_Model.cust_linkman()
                {
                    link_cust_id = customerID,
                    link_birthday = d_1,
                    link_createdate = DateTime.Now,
                    link_creator = 1,
                    link_cust_name = customName,

                    link_email = email,
                    link_isdelete = "0",
                    link_level = link_level,
                    link_name = Convert.ToString(dic_data[i]["F5"]).Trim(),
                    link_phonenumber = link_phonenumber,
                    link_position = position,
                    link_remark = remark,
                    link_sex = sex,
                    link_status = "0",
                    link_telephone = telep,
                    link_updatedate = DateTime.Now,
                    link_updateuser = 1,
                    link_users = uniQ,
                    link_usersname = userName
                };
                string dp1 = Convert.ToString(dic_data[i]["F3"]).Trim();
                string dp2 = Convert.ToString(dic_data[i]["F4"]).Trim();
                if (!string.IsNullOrEmpty(dp1))
                {
                    linkman.link_department = dp1;
                }
                else
                {
                    linkman.link_department = dp2;
                }
                //增加联系人
                JsonModel js_linkman = Constant.cust_linkman_S.Add(linkman);
                linkman.id = Convert.ToInt64(js_linkman.retData);
                //添加用户【所有】
                cust_linkman_handle.list_All.Add(linkman);
                //添加用户【自身】
                if (cust_linkman_handle.dic_Self.ContainsKey(uniQ))
                {
                    if (!cust_linkman_handle.dic_Self[uniQ].Contains(linkman))
                    {
                        cust_linkman_handle.dic_Self[uniQ].Add(linkman);
                    }
                }
                cust_linkman_handle.admin_add_linkman(uniQ, linkman);
                if (linkman.id > 0)
                {
                    add_linkman_count++;
                    reinfo.Add(i + "、成功提交联系人：" + link_name + "      负责人：" + userName);
                   

                }
            }
            catch (Exception ex)
            {
                _showPage_info = "出现异常，请联系管理员";
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 编辑联系人
        /// </summary>
        /// <param name="dic_data">excel数据</param>
        /// <param name="userName">用户名称</param>
        /// <param name="i">序号</param>
        /// <param name="link_name">联系人名称</param>
        /// <param name="d_1">生日</param>
        /// <param name="link_level">联系人等级</param>
        /// <param name="sex">性别</param>
        /// <param name="position">职务</param>
        /// <param name="email">邮箱地址</param>
        /// <param name="telep">电话号码</param>
        /// <param name="remark">联系人备注</param>
        /// <param name="customers">缓存联系人</param>
        private void linkman_edit(List<Dictionary<string, object>> dic_data, string userName, int i, string link_name, DateTime d_1, int link_level, string sex, string position, string email, string telep, string remark, List<cust_linkman> customers)
        {
            try
            {
                #region 修改联系人

                cust_linkman _linkman = customers[0];
                //覆盖联系人
                _linkman.link_birthday = d_1;
                _linkman.link_level = link_level;
                _linkman.link_updatedate = DateTime.Now;
                _linkman.link_sex = sex;
                _linkman.link_telephone = telep;
                _linkman.link_remark = remark;
                _linkman.link_position = position;
                _linkman.link_email = email;

                string dp1 = Convert.ToString(dic_data[i]["F3"]).Trim();
                string dp2 = Convert.ToString(dic_data[i]["F4"]).Trim();
                if (!string.IsNullOrEmpty(dp1))
                {
                    _linkman.link_department = dp1;
                }
                else
                {
                    _linkman.link_department = dp2;
                }
                Constant.cust_linkman_S.Update(_linkman);

                #endregion

                exit_linkman_count++;
                reinfo.Add(i + "、联系人已存在【客户名称、联系人名称、手机号相同】,联系人信息覆盖：" + link_name + "      负责人：" + userName);
            }
            catch (Exception ex)
            {
                reinfo.Add("出现异常，请联系管理员");
                LogHelper.Error(ex);
            }
            finally
            {

            }
        }

        #endregion



        /// <summary>
        /// DataTable转换成List
        /// </summary>
        /// <param name="dt">要转换的DataTable</param>        
        /// <returns>List<Dictionary<string, object>></returns>
        public List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        result.Add(dc.ColumnName, dr[dc].ToString());
                    }
                    list.Add(result);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}