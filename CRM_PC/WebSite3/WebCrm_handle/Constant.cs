using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_BLL;
using CRM_DAL;
using CRM_Model;
using CRM_Common;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;

namespace WebCrm_handle
{
    public static class Constant
    {
        public static JavaScriptSerializer jss = new JavaScriptSerializer();

        public static JsonModel get_jsonmodel(int errNum, string errMsg, object retData)
        {
            return new JsonModel()
            {
                errNum = errNum,
                errMsg = errMsg,
                retData = retData,
            };
        }
        public static Dictionary<string, object> ExcuteSql_Get_Param(string Title_Type)
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
                    cmd.Parameters.Add(new SqlParameter("@Title_Type", Title_Type));

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
        public static SqlConnection GetNewConnection()
        {
            //创建数据库对象
            SqlConnection conn = new SqlConnection("Data Source=192.168.100.242;Initial Catalog=CRM;User ID=sa;password=sa@2016");
            return conn;
        }

        /// <summary>
        /// 执行查询操作（数据库表单实体化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <param name="errorString"></param>
        /// <param name="args"></param>
        /// <returns></returns>       
    }
}