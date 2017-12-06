using CRM_Common;
using CRM_Handler.Common;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebCrm_handle.Common;

namespace WebCrm_handle.Salerkit
{
    /// <summary>
    /// SalerkitHandler 的摘要说明
    /// </summary>
    public class SalerkitHandler : IHttpHandler
    {
        JsonModel jsonModel = new JsonModel();
        public void ProcessRequest(HttpContext context)
        {
            string Date_Start = "2016-01-01";
            string Date_End = "2018-01-01";
            string Type_Value = "签到";
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                switch (func)
                {

                    case "Get_AllUserTJInfo": Get_AllUserTJInfo(Request,context); break;
                    case "Get_OneUserTJInfo": Get_OneUserTJInfo(Request, context); break;
                    //case "Get_CustomerByDepartment": Get_CustomerByDepartment(Request, context); break;

                    //case "Get_saleDepartment": Get_saleDepartment(context); break;
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
        public void Get_OneUserTJInfo(HttpRequest Request, HttpContext context)
        {
            DateTime select_date = RequestHelper.DateTime_transfer(Request, "select_date");
            string userid = RequestHelper.string_transfer(Request, "userid");
            DateTime dt1 = select_date.AddDays((Convert.ToInt32(select_date.DayOfWeek) - 1) * (-1));
            DateTime dt2 = dt1.AddDays(1);
            DateTime dt3 = dt2.AddDays(1);
            DateTime dt4 = dt3.AddDays(1);
            DateTime dt5 = dt4.AddDays(1);
            DateTime dt6 = dt5.AddDays(1);
            DateTime dt7 = dt6.AddDays(1);

            Dictionary<string, string> Usermap1 = GetUsermap(dt1, userid);
            Dictionary<string, string> Usermap2 = GetUsermap(dt2, userid);
            Dictionary<string, string> Usermap3 = GetUsermap(dt3, userid);
            Dictionary<string, string> Usermap4 = GetUsermap(dt4, userid);
            Dictionary<string, string> Usermap5 = GetUsermap(dt5, userid);
            Dictionary<string, string> Usermap6 = GetUsermap(dt6, userid);
            Dictionary<string, string> Usermap7 = GetUsermap(dt7, userid);

            List<Dictionary<string, string>> ReMap = new List<Dictionary<string, string>>();
            ReMap.Add(Usermap1);
            ReMap.Add(Usermap2);
            ReMap.Add(Usermap3);
            ReMap.Add(Usermap4);
            ReMap.Add(Usermap5);
            ReMap.Add(Usermap6);
            ReMap.Add(Usermap7);
            jsonModel = Constant.get_jsonmodel(0, "success", ReMap);
            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");

        }
        public  Dictionary<string, string> GetUsermap(DateTime dt,string userid)
        {
            string WeekName = dt.DayOfWeek.ToString();
            if (WeekName == "Sunday") { WeekName = "周日"; }
            else if (WeekName == "Monday") { WeekName = "周一"; }
            else if (WeekName == "Tuesday") { WeekName = "周二"; }
            else if (WeekName == "Wednesday") { WeekName = "周三"; }
            else if (WeekName == "Thursday") { WeekName = "周四"; }
            else if (WeekName == "Friday") { WeekName = "周五"; }
            else if (WeekName == "Saturday") { WeekName = "周六"; }

            Dictionary<string, string> Usermap = new Dictionary<string, string>();
            Usermap.Add("date", dt.ToShortDateString() + "(" + WeekName+")"); 
            Usermap = Get_OneUserTJInfo_sign(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "签到", userid, Usermap);
            Usermap = Get_OneUserTJInfo_follow(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "跟进", userid, Usermap);
            Usermap = Get_OneUserTJInfo_com_userid(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "评论", userid, Usermap);
            Usermap = Get_OneUserTJInfo_cust_users(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "客户", userid, Usermap);
            Usermap = Get_OneUserTJInfo_link_users(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "联系人", userid, Usermap);
            Usermap = Get_OneUserTJInfo_wp_userid(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "工作计划", userid, Usermap);
            Usermap = Get_OneUserTJInfo_report_userid(dt.ToShortDateString(), dt.AddHours(24).ToShortDateString(), "工作报告", userid, Usermap);
            return Usermap;
        }
        //签到1
        public  Dictionary<string, string> Get_OneUserTJInfo_sign(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”                   
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_sign", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_sign", "0");
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
            return Usermap;
        }
        //跟进1
        public  Dictionary<string, string> Get_OneUserTJInfo_follow(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_follow", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_follow", "0");
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
            return Usermap;
        }
        //评论1
        public  Dictionary<string, string> Get_OneUserTJInfo_com_userid(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_com_userid", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_com_userid", "0");
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
            return Usermap;
        }
        //客户1
        public  Dictionary<string, string> Get_OneUserTJInfo_cust_users(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”                 
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_cust_users", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_cust_users", "0");
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
            return Usermap;
        }
        //联系人1
        public  Dictionary<string, string> Get_OneUserTJInfo_link_users(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”                  
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_link_users", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_link_users", "0");
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
            return Usermap;
        }
        //工作计划1
        public  Dictionary<string, string> Get_OneUserTJInfo_wp_userid(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_wp_userid", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_wp_userid", "0");
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
            return Usermap;
        }
        //工作报告1
        public  Dictionary<string, string> Get_OneUserTJInfo_report_userid(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        {

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
                    cmd.CommandText = "GetTJInfoByUserId";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
                    cmd.Parameters.Add(new SqlParameter("@User_id", userid));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string countbywhere = reader["countbywhere"].ToString();
                            Usermap.Add("count_report_userid", countbywhere);
                        }
                    }
                    else
                    {
                        Usermap.Add("count_report_userid", "0");
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
            return Usermap;
        }
        //__________________________________________________
        //public Dictionary<string, string> Get_OneUserTJInfo_sign(string Date_Start, string Date_End, string Type_Value, string userid, Dictionary<string, string> Usermap)
        //{

        //    SqlConnection con = null;
        //    try
        //    {
        //        con = Constant.GetNewConnection();
        //        //创建数据库连接对象
        //        using (SqlCommand cmd = new SqlCommand())
        //        {

        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "GetTJInfoByUserId";
        //            cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
        //            cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
        //            cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));
        //            cmd.Parameters.Add(new SqlParameter("@User_id", userid));

        //            //创建读取器（只进只读对象）
        //            SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
        //            while (reader.Read())
        //            {
        //                string countbywhere = reader["countbywhere"].ToString();
        //                Usermap.Add("sign", countbywhere);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return Usermap;
        //}
        public void Get_AllUserTJInfo(HttpRequest Request,HttpContext context)
        {
            DateTime select_date = RequestHelper.DateTime_transfer(Request, "select_date");
            DateTime dt1 = select_date.AddDays((Convert.ToInt32(select_date.DayOfWeek) - 1) * (-1));
            DateTime dt2 = dt1.AddDays(1);
            DateTime dt3 = dt2.AddDays(1);
            DateTime dt4 = dt3.AddDays(1);
            DateTime dt5 = dt4.AddDays(1);
            DateTime dt6 = dt5.AddDays(1);
            DateTime dt7 = dt6.AddDays(1);
            string Date_Start = dt1.ToShortDateString();
            string Date_End = dt7.ToShortDateString();
            //string Date_Start = "2016-01-01";
            //string Date_End = "2018-01-01";
            Dictionary<string, UserTJInfo> Usermap = GetAllUser();
            Usermap = GetAllUser_sign(Date_Start, Date_End, "签到", Usermap);
            Usermap = GetAllUser_follow(Date_Start, Date_End, "跟进", Usermap);
            Usermap = GetAllUser_com(Date_Start, Date_End, "评论", Usermap);
            Usermap = GetAllUser_cust(Date_Start, Date_End, "客户", Usermap);
            Usermap = GetAllUser_link(Date_Start, Date_End, "联系人", Usermap);
            Usermap = GetAllUser_wp(Date_Start, Date_End, "工作计划", Usermap);
            Usermap = GetAllUser_report(Date_Start, Date_End, "工作报告", Usermap);
            jsonModel = Constant.get_jsonmodel(0, "success", Usermap.Values);
            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
        }
        public Dictionary<string, UserTJInfo> GetAllUser() 
        {
            Dictionary<string, UserTJInfo> map = new Dictionary<string, UserTJInfo>();
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
                    cmd.CommandText = "GetAllUser";
                    //cmd.Parameters.Add(new SqlParameter("@Title_Type", "客户级别"));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        UserTJInfo UserTJInfo_ = new UserTJInfo();
                        UserTJInfo_.Id = Convert.ToString(reader["userid"]);
                        UserTJInfo_.Name = Convert.ToString(reader["username"]);
                        UserTJInfo_.DepartMentName = Convert.ToString(reader["departmentname"]);
                        UserTJInfo_.DepartMentId = Convert.ToString(reader["departmentid"]);

                        map.Add(reader["userid"].ToString(), UserTJInfo_);
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
        //获取签到统计
        public Dictionary<string, UserTJInfo> GetAllUser_sign(string Date_Start, string Date_End,string Type_Value,Dictionary<string, UserTJInfo> Usermap)
        {
          
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_sign = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取跟进统计
        public Dictionary<string, UserTJInfo> GetAllUser_follow(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
            
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_follow = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取评论统计
        public Dictionary<string, UserTJInfo> GetAllUser_com(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
           
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_com_userid = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取客户统计
        public Dictionary<string, UserTJInfo> GetAllUser_cust(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
           
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_cust_users = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取联系人统计
        public Dictionary<string, UserTJInfo> GetAllUser_link(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
           
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_link_users = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取工作计划统计
        public Dictionary<string, UserTJInfo> GetAllUser_wp(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
           
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_wp_userid = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
        }
        //获取工作报告统计
        public Dictionary<string, UserTJInfo> GetAllUser_report(string Date_Start, string Date_End, string Type_Value, Dictionary<string, UserTJInfo> Usermap)
        {
           
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
                    cmd.CommandText = "GetCountByDate";
                    cmd.Parameters.Add(new SqlParameter("@Date_Start", Date_Start));
                    cmd.Parameters.Add(new SqlParameter("@Date_End", Date_End));
                    cmd.Parameters.Add(new SqlParameter("@Type_Value", Type_Value));

                    //创建读取器（只进只读对象）
                    SqlDataReader reader = cmd.ExecuteReader();		//执行SQL，返回一个“流”
                    while (reader.Read())
                    {
                        string userid = reader["userid"].ToString();
                        Usermap[userid].count_report_userid = Convert.ToInt32(reader["countbywhere"]);
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
            return Usermap;
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