using CRM_Common;
using CRM_Handler.Common;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace WebCrm_handle.Customer
{
    /// <summary>
    /// AllCustomerHandler 的摘要说明
    /// </summary>
    public class AllCustomerHandler : IHttpHandler
    {
        JsonModel jsonModel = new JsonModel();
        Dictionary<string, object> Dic_cust_level=new Dictionary<string,object>();
        Dictionary<string, object> Dic_cust_type = new Dictionary<string, object>();
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                switch (func)
                {

                    case "Get_AllCustomer": Get_AllCustomer(context); break;

                    case "Get_CustomerByDepartment": Get_CustomerByDepartment( Request,context); break;

                    case "Get_saleDepartment": Get_saleDepartment(context); break;
                    default:
                        jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                        break;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.get_jsonmodel(7, "出现异常,请通知管理员", "");
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }
        public void Get_saleDepartment(HttpContext context)
        {
            List<Dictionary<string, string>> List_map = new List<Dictionary<string, string>>();
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("id", "05");
            map.Add("name", "普教一部");
            Dictionary<string, string> map1 = new Dictionary<string, string>();
            map1.Add("id", "06");
            map1.Add("name", "普教二部");
            Dictionary<string, string> map2 = new Dictionary<string, string>();
            map2.Add("id", "07");
            map2.Add("name", "普教三部");
            Dictionary<string, string> map3 = new Dictionary<string, string>();
            map3.Add("id", "09");
            map3.Add("name", "高教一部");
            Dictionary<string, string> map4 = new Dictionary<string, string>();
            map4.Add("id", "10");
            map4.Add("name", "高教二部");
            Dictionary<string, string> map5 = new Dictionary<string, string>();
            map5.Add("id", "11");
            map5.Add("name", "高教三部");
            //Dictionary<string, string> map6 = new Dictionary<string, string>();
            //map6.Add("id", "23");
            //map6.Add("name", "销售三部");
            //Dictionary<string, string> map7 = new Dictionary<string, string>();
            //map7.Add("id", "22");
            //map7.Add("name", "销售一部");
            //Dictionary<string, string> map8 = new Dictionary<string, string>();
            //map8.Add("id", "26");
            //map8.Add("name", "销售二部");
            List_map.Add(map);
            List_map.Add(map1);
            List_map.Add(map2);
            List_map.Add(map3);
            List_map.Add(map4);
            List_map.Add(map5);
            //List_map.Add(map6);
            //List_map.Add(map7);
            //List_map.Add(map8);
            jsonModel = Constant.get_jsonmodel(0, "success", List_map);
            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
        }
        public void Get_CustomerByDepartment(HttpRequest Request, HttpContext context) 
        {
            string DepartmentId = RequestHelper.string_transfer(Request, "DepartmentId");
           List<string> list=new List<string>();
            SqlConnection con = null;
            try
            {
                con = Constant.GetNewConnection();
                //创建数据库连接对象
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetUserByDepartmentId";
                    cmd.Parameters.Add(new SqlParameter("@DepartmentId", DepartmentId));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”

                    while (reader.Read())
                    {
                        if (reader["Name"] != null) 
                        {
                            list.Add(reader["Name"].ToString());
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
                if (con != null)
                {
                    con.Close();
                }
            }
            jsonModel = Constant.get_jsonmodel(0, "success", list);
            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
        }
        public void Get_AllCustomer(HttpContext context)
        {
            try
            {
                Dic_cust_level =Constant.ExcuteSql_Get_Param("客户级别");
                Dic_cust_type = Constant.ExcuteSql_Get_Param("客户类型");
                var query = ExcuteSql_AllAllCustomerInfo();
                Dictionary<string, object> map = new Dictionary<string, object>();
                map.Add("cust_level", Dic_cust_level.Values);
                map.Add("cust_type", Dic_cust_type.Values);
                map.Add("AllCustomer", query);
                jsonModel = Constant.get_jsonmodel(0, "success", map);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                //无论后端出现什么问题，都要给前端有个通知【为防止jsonModel 为空 ,全局字段 jsonModel 特意声明之后进行初始化】
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }
       
        public Dictionary<string, object> ExcuteSql_cust_level() 
        {
            Dictionary<string, object> map = new Dictionary<string, object>(); 
            SqlConnection con = null;
            try
            { 
                con = Constant.GetNewConnection();
                //创建数据库连接对象
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetDicValueByType";
                    cmd.Parameters.Add(new SqlParameter("@Title_Type", "客户级别")); 

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                   
                    while (reader.Read())
                    {
                        map.Add(reader["value"].ToString(), reader["title"]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return map;
        }
        public Dictionary<string, object> ExcuteSql_cust_type()
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            SqlConnection con = null;
            try
            {
                con = Constant.GetNewConnection();
                //创建数据库连接对象
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "GetDicValueByType";
                    cmd.Parameters.Add(new SqlParameter("@Title_Type", "客户类型"));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”

                    while (reader.Read())
                    {
                        map.Add(reader["value"].ToString(), reader["title"]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return map;
        }
        public List<Dictionary<string, object>> ExcuteSql_AllAllCustomerInfo()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            SqlConnection con = null;
            try
            {
                con = Constant.GetNewConnection();
                //创建数据库连接对象
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "AllCusTomerForWeb";
                    // cmd.Parameters.AddRange(args);

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        Dictionary<string, object> map = new Dictionary<string, object>();
                        string cust_level = Convert.ToString(reader["cust_level"]);
                       
                        //把cust_level由(1,2,3,4)转为对应的文字
                        if (cust_level!=null&&Dic_cust_level.ContainsKey(cust_level)) 
                        {
                            map.Add("cust_level", Dic_cust_level[cust_level]);
                        }
                        //把cust_type由(1,2,3,4)转为对应的文字
                        string cust_type = Convert.ToString(reader["cust_type"]);
                        if (cust_type!=null&&Dic_cust_type.ContainsKey(cust_type))
                        {
                            map.Add("cust_type", Dic_cust_type[cust_type]);
                        }
                       
                        map.Add("cust_name", reader["cust_name"]);
                        //map.Add("cust_type", reader["cust_type"]);
                        //map.Add("cust_level", reader["cust_level"]);
                        map.Add("cust_usersname", reader["cust_usersname"]);
                        map.Add("follow_date", reader["follow_date"]);
                        map.Add("link_name", reader["link_name"]);
                        map.Add("link_telephone", reader["link_telephone"]);
                        map.Add("custuser_department", reader["custuser_department"]);
                        map.Add("OrganNo", reader["OrganNo"]);
                        map.Add("id", reader["id"]);                     
                        list.Add(map);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
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