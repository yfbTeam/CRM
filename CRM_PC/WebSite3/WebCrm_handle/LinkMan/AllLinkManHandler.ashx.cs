using CRM_Common;
using CRM_Handler.Common;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebCrm_handle.LinkMan
{
    /// <summary>
    /// AllLinkManHandler 的摘要说明
    /// </summary>
    public class AllLinkManHandler : IHttpHandler
    {
        JsonModel jsonModel = new JsonModel();
        Dictionary<string, object> map_Level = new Dictionary<string, object>();
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                switch (func)
                {
                    //获取学年学期
                    case "Get_AllLinkMan": Get_AllLinkMan(context); break;
                    //获取指标库
                    //case "Get_Indicator": Get_Indicator(context); break;
                    //新增指标库
                    //case "Add_Indicator": Add_Indicator(context); break;
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
        public Dictionary<string, object> ExcuteSql_LinkMan_level()
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
                    cmd.Parameters.Add(new SqlParameter("@Title_Type", "联系人级别"));

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
        public void Get_AllLinkMan(HttpContext context)
        {
            try
            {
                map_Level = ExcuteSql_LinkMan_level();
                var query = ExcuteSql();
                jsonModel = Constant.get_jsonmodel(0, "success", query);
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
        public List<Dictionary<string, object>> ExcuteSql()
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
                    cmd.CommandText = "AllLinkManForWeb";
                    // cmd.Parameters.AddRange(args);

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        Dictionary<string, object> map = new Dictionary<string, object>();

                        string link_level = Convert.ToString(reader["link_level"]);

                        //把cust_level由(1,2,3,4)转为对应的文字
                        if (link_level != null && map_Level.ContainsKey(link_level))
                        {
                            map.Add("link_level", map_Level[link_level]);
                        }

                        map.Add("id", reader["id"]);
                        map.Add("link_name", reader["link_name"]);
                        map.Add("link_cust_name", reader["link_cust_name"]);
                        map.Add("link_telephone", reader["link_telephone"]);
                        //map.Add("link_level", reader["link_level"]);
                        map.Add("link_usersname", reader["link_usersname"]);                      
                        map.Add("follow_date", reader["follow_date"]);
                        map.Add("linkuser_department", reader["linkuser_department"]);
                        map.Add("OrganNo", reader["OrganNo"]);
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