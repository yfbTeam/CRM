using CRM_Common;
using CRM_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRM_BLL_DAL
{
    /// <summary>
    /// 使用此类主要是写存储过程，并且页面有多个列表的时候
    /// </summary>
    public class DALBaseCommon
    {
        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static customer_info getcust_customer(customer_info entity, string where)
        {
            try
            {
                SqlParameter[] param = {
                        new SqlParameter("@where",where)
                };
                SqlDataReader reader = SQLHelp.ExecuteReader("getcust_customer", CommandType.StoredProcedure, param);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {

                        foreach (PropertyInfo item in pros)
                        {
                            item.SetValue(entity, reader.IsDBNull(reader.GetOrdinal(item.Name)) ? null : reader[item.Name]);
                        }
                    }
                }
                else { entity = null; }
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static workreport workreport_info(workreport entity, string id)
        {
            try
            {
                SqlParameter[] param = {
                        new SqlParameter("@id",id)
                };
                SqlDataReader reader = SQLHelp.ExecuteReader("get_workreport_info", CommandType.StoredProcedure, param);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {

                        foreach (PropertyInfo item in pros)
                        {
                            item.SetValue(entity, reader.IsDBNull(reader.GetOrdinal(item.Name)) ? null : reader[item.Name]);
                        }
                    }
                }
                else { entity = null; }
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取联系人类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static linkman_info getcust_linkman(linkman_info entity, string id)
        {
            try
            {
                SqlParameter[] param = {
                        new SqlParameter("@Id",id)
                };
                SqlDataReader reader = SQLHelp.ExecuteReader("getcust_linkman", CommandType.StoredProcedure, param);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {

                        foreach (PropertyInfo item in pros)
                        {
                            item.SetValue(entity, reader.IsDBNull(reader.GetOrdinal(item.Name)) ? null : reader[item.Name]);
                        }
                    }
                }
                else { entity = null; }
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 获取联系人信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getcust_linkman_list(string link_cust_id)
        {
            SqlParameter[] param = {
                    new SqlParameter("@link_cust_id",link_cust_id)
            };
            return SQLHelp.ExecuteDataTable("getcust_linkman_list", CommandType.StoredProcedure, param);
        }

        public static DataTable get_cust_customer_by_custname(string cust_name, long cust_id)
        {
            SqlParameter[] param = {
                    new SqlParameter("@cust_name",cust_name),
                    new SqlParameter("@cust_id",cust_id)
            };
            return SQLHelp.ExecuteDataTable("get_cust_customer_by_custname", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        public static DataTable get_statistic(string userid, string username, string startdate, string enddate)
        {
            SqlParameter[] param = {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@username",username),
                    new SqlParameter("@startdate",startdate),
                    new SqlParameter("@enddate",enddate)
            };
            return SQLHelp.ExecuteDataTable("proc_getstatic_detail", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取今日事项
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DataTable get_today_matters(string userid)
        {
            SqlParameter[] param = {
                    new SqlParameter("@userid",userid)
            };
            return SQLHelp.ExecuteDataTable("get_today_matters", CommandType.StoredProcedure, param);
        }

        public static DataTable get_report_info(string id)//get_praise
        {
            SqlParameter[] param = {
                    new SqlParameter("@id",id)
            };
            return SQLHelp.ExecuteDataTable("get_report_info", CommandType.StoredProcedure, param);
        }

        public static DataTable get_praise(string id)//
        {
            SqlParameter[] param = {
                    new SqlParameter("@id",id)
            };
            return SQLHelp.ExecuteDataTable("get_praise", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取日历是否签到
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DataTable get_is_sign(string sign_date, int userid)
        {
            SqlParameter[] param = {
                    new SqlParameter("@sign_date",sign_date),
                    new SqlParameter("@userid",userid)
            };
            return SQLHelp.ExecuteDataTable("get_is_sign", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        public static DataTable get_statistic_detail(string userid, string sd_type)
        {
            SqlParameter[] param = {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@sd_type",sd_type)
            };
            return SQLHelp.ExecuteDataTable("get_statistic_detail", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取工作计划第一条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getworkplan_list(string wp_userid, string table_id, string type)
        {
            SqlParameter[] param = {
                    new SqlParameter("@wp_userid",wp_userid),
                    new SqlParameter("@table_id",table_id),
                    new SqlParameter("@type",type)
            };
            return SQLHelp.ExecuteDataTable("getworkplan_list", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取签到记录第一条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getsign_list(string sign_userid)
        {
            SqlParameter[] param = {
                    new SqlParameter("@sign_userid",sign_userid)
            };
            return SQLHelp.ExecuteDataTable("getsign_list", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 根据id获取联系人职位
        /// </summary>
        /// <param name="sign_userid"></param>
        /// <returns></returns>
        public static DataTable get_link_position_by_id(string id)
        {
            SqlParameter[] param = {
                    new SqlParameter("@id",id)
            };
            return SQLHelp.ExecuteDataTable("get_link_position_by_id", CommandType.StoredProcedure, param);
        }
        //跟进记录获取所有跟进客户
        public static DataTable get_cust_list(string userid)
        {
            SqlParameter[] param = {
                    new SqlParameter("@userid",userid)
            };
            return SQLHelp.ExecuteDataTable("get_cust_list", CommandType.StoredProcedure, param);
        }

        /// <summary>
        /// 获取签到记录第一条信息
        /// </summary>
        /// <returns></returns>
        public static DataTable getfollow_up_list(string follow_userid, string tableid, string type)
        {
            SqlParameter[] param = {
                    new SqlParameter("@follow_userid",follow_userid),
                    new SqlParameter("@tableid",tableid),
                    new SqlParameter("@type",type)
            };
            return SQLHelp.ExecuteDataTable("getfollow_up_list", CommandType.StoredProcedure, param);
        }


        //获取图片集合
        public static DataTable getpicture_list(string pic_en_table, string pic_table_id)
        {
            SqlParameter[] param = {
                    new SqlParameter("@pic_en_table",pic_en_table),
                    new SqlParameter("@pic_table_id",pic_table_id)
            };
            return SQLHelp.ExecuteDataTable("getpicture_list", CommandType.StoredProcedure, param);
        }

        public static DataTable getcomment_list(string com_table_id, string com_type)
        {
            SqlParameter[] param = {
                    new SqlParameter("@com_table_id",com_table_id),
                    new SqlParameter("@com_type",com_type)
            };
            return SQLHelp.ExecuteDataTable("getcomment_list", CommandType.StoredProcedure, param);
        }

        public static DataTable cust_customer_nearby(string sign_x, string sign_y, string cust_users)
        {
            SqlParameter[] param = {
                    new SqlParameter("@sign_x",sign_x),
                    new SqlParameter("@sign_y",sign_y),
                    new SqlParameter("@cust_users",cust_users)
            };
            return SQLHelp.ExecuteDataTable("cust_customer_nearby", CommandType.StoredProcedure, param);
        }
        public static DataTable get_statistic_today(string userid, string username, string type)
        {
            SqlParameter[] param = {
                    new SqlParameter("@userid",userid),
                    new SqlParameter("@username",username),
                    new SqlParameter("@type",type)
            };
            return SQLHelp.ExecuteDataTable("get_static_today", CommandType.StoredProcedure, param);
        }
        /// <summary>
        /// 获取提醒设置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remind_userid"></param>
        /// <param name="remind_type"></param>
        /// <returns></returns>
        public static remind_setting get_remind_setting_info(remind_setting entity, string remind_userid, string remind_type)
        {
            try
            {
                SqlParameter[] param = {
                    new SqlParameter("@remind_userid",remind_userid),
                    new SqlParameter("@remind_type",remind_type)
            };
                SqlDataReader reader = SQLHelp.ExecuteReader("get_remind_setting_info", CommandType.StoredProcedure, param);
                if (reader.HasRows)
                {
                    PropertyInfo[] pros = entity.GetType().GetProperties();
                    while (reader.Read())
                    {

                        foreach (PropertyInfo item in pros)
                        {
                            item.SetValue(entity, reader.IsDBNull(reader.GetOrdinal(item.Name)) ? null : reader[item.Name]);
                        }
                    }
                }
                else { entity = null; }
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static int edit_remind_setting(remind_setting model)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@remind_userid", model.remind_userid==null?"": model.remind_userid),
                    new SqlParameter("@remind_type", model.remind_type==null?"":model.remind_type),
                    new SqlParameter("@remind_remark", model.remind_remark==null?"":model.remind_remark)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("edit_remind_setting", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }


        public static int edit_report_sender(int table_id, string report_reader, string report_sender)
        {
            SqlParameter[] param = {
                    new SqlParameter("@table_id", table_id),
                    new SqlParameter("@report_reader", report_reader),
                    new SqlParameter("@report_sender", report_sender)
            };
            object obj = SQLHelp.ExecuteNonQuery("edit_report_sender", CommandType.StoredProcedure, param);
            return Convert.ToInt32(obj); ;
        }

        public static int edit_sign_in(sign_in model)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@sign_userid", model.sign_userid==null?"":model.sign_userid),
                    new SqlParameter("@sign_username",model.sign_username==null?"":model.sign_username),
                    new SqlParameter("@sign_date", model.sign_date==null?DateTime.Now:model.sign_date),
                    new SqlParameter("@sign_cust_id",model.sign_cust_id==null?0:model.sign_cust_id),
                    new SqlParameter("@sign_location",model.sign_location==null?"":model.sign_location),
                    new SqlParameter("@sign_address",model.sign_address==null?"":model.sign_address),
                    new SqlParameter("@sign_offset",model.sign_offset==null?0:model.sign_offset),
                    new SqlParameter("@sign_x",model.sign_x==null?0: model.sign_x),
                    new SqlParameter("@sign_y",model.sign_y==null?0:model.sign_y)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("edit_sign_in", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        public static DataTable edit_praise(praise model)
        {
            string result = "";
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@praise_table_id", model.praise_table_id==null?0:model.praise_table_id),
                    new SqlParameter("@praise_userid", model.praise_userid==null?"":model.praise_userid),
                    new SqlParameter("@praise_username", model.praise_username==null?"":model.praise_username),
                    new SqlParameter("@praise_type",model.praise_type==null?"":model.praise_type)
            };

            return SQLHelp.ExecuteDataTable("edit_praise", CommandType.StoredProcedure, param);
        }

        ///// <summary>
        ///// 新增签到
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public static int edit_sign_in(sign_in model)
        //{
        //    int result = 0;
        //    SqlParameter[] param = {
        //            new SqlParameter("@id", model.id),
        //            new SqlParameter("@sign_userid", model.sign_userid),
        //            new SqlParameter("@sign_username", model.sign_username),
        //            new SqlParameter("@sign_date", model.sign_date),
        //            new SqlParameter("@sign_cust_id", model.sign_cust_id),
        //            new SqlParameter("@sign_location", model.sign_location),
        //            new SqlParameter("@sign_address", model.sign_address),
        //            new SqlParameter("@sign_offset", model.sign_offset)
        //    };
        //    param[0].Direction = ParameterDirection.Output;
        //    object obj = SQLHelp.ExecuteNonQuery("edit_sign_in", CommandType.StoredProcedure, param);
        //    result = Convert.ToInt32(param[0].Value.ToString());
        //    return result;
        //}
        /// <summary>
        /// 新增跟进记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int add_follow_up(follow_up model, string picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", SqlDbType.Int),
                    new SqlParameter("@follow_userid", model.follow_userid==null?"":model.follow_userid),
                    new SqlParameter("@follow_username", model.follow_username==null?"":model.follow_username),
                    new SqlParameter("@follow_cust_id", model.follow_cust_id==null?0:model.follow_cust_id),
                    new SqlParameter("@follow_link_id", model.follow_link_id==null?0:model.follow_link_id),
                    new SqlParameter("@follow_date", model.follow_date==null?DateTime.Now:model.follow_date),
                    new SqlParameter("@follow_content", model.follow_content==null?"":model.follow_content),
                    new SqlParameter("@follow_type", model.follow_type==null?0:model.follow_type),
                    new SqlParameter("@follow_status", model.follow_status==null?"":model.follow_status),
                    new SqlParameter("@follow_remaindate", model.follow_remaindate==null?DateTime.Now:model.follow_remaindate),
                    new SqlParameter("@follow_address", model.follow_address==null?"":model.follow_address),
                    new SqlParameter("@picture", picture==null?"":picture)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("add_follow_up", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        public static int edit_follow_up(follow_up model, string picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@follow_userid", model.follow_userid==null?"":model.follow_userid),
                    new SqlParameter("@follow_username", model.follow_username==null?"":model.follow_username),
                    new SqlParameter("@follow_cust_id", model.follow_cust_id==null?0:model.follow_cust_id),
                    new SqlParameter("@follow_link_id", model.follow_link_id==null?0:model.follow_link_id),
                    new SqlParameter("@follow_date", model.follow_date==null?DateTime.Now:model.follow_date),
                    new SqlParameter("@follow_content", model.follow_content==null?"":model.follow_content),
                    new SqlParameter("@follow_type", model.follow_type==null?0:model.follow_type),
                    new SqlParameter("@follow_status", model.follow_status==null?"":model.follow_status),
                    new SqlParameter("@follow_remaindate", model.follow_remaindate==null?DateTime.Now:model.follow_remaindate),
                    new SqlParameter("@follow_address", model.follow_address==null?"":model.follow_address),
                    new SqlParameter("@picture", picture==null?"":picture)
            };
            result = Convert.ToInt32(SQLHelp.ExecuteScalar("edit_cust_customer", System.Data.CommandType.StoredProcedure, param.ToArray()));
            return result;
        }

        /// <summary>
        /// 编辑联系人edit_cust_linkman
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int add_cust_linkman(cust_linkman model)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", SqlDbType.Int),
                    new SqlParameter("@link_cust_id", model.link_cust_id==null?0:model.link_cust_id),
                    new SqlParameter("@link_name", model.link_name==null?"":model.link_name),
                    new SqlParameter("@link_department", model.link_department==null?"":model.link_department),
                    new SqlParameter("@link_position", model.link_position==null?"":model.link_position),
                    new SqlParameter("@link_level", model.link_level==null?0:model.link_level),
                    new SqlParameter("@link_sex", model.link_sex==null?"":model.link_sex),
                    new SqlParameter("@link_birthday", model.link_birthday),
                    new SqlParameter("@link_phonenumber", model.link_phonenumber==null?"":model.link_phonenumber),
                    new SqlParameter("@link_telephone", model.link_telephone==null?"":model.link_telephone),
                    new SqlParameter("@link_email", model.link_email==null?"":model.link_email),
                    new SqlParameter("@link_status", model.link_status==null?"":model.link_status),
                    new SqlParameter("@link_users", model.link_users==null?"0":model.link_users),
                    new SqlParameter("@link_usersname", model.link_usersname==null?"":model.link_usersname),
                    new SqlParameter("@link_cust_name", model.link_cust_name==null?"":model.link_cust_name)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("add_cust_linkman", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        /// <summary>
        /// 编辑联系人edit_cust_linkman
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int edit_cust_linkman(cust_linkman model)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@link_cust_id", model.link_cust_id==null?0:model.link_cust_id),
                    new SqlParameter("@link_name", model.link_name==null?"":model.link_name),
                    new SqlParameter("@link_department", model.link_department==null?"":model.link_department),
                    new SqlParameter("@link_position", model.link_position==null?"":model.link_position),
                    new SqlParameter("@link_level", model.link_level==null?0:model.link_level),
                    new SqlParameter("@link_sex", model.link_sex==null?"":model.link_sex),
                    new SqlParameter("@link_birthday", model.link_birthday),
                    new SqlParameter("@link_phonenumber", model.link_phonenumber==null?"":model.link_phonenumber),
                    new SqlParameter("@link_telephone", model.link_telephone==null?"":model.link_telephone),
                    new SqlParameter("@link_email", model.link_email==null?"":model.link_email),
                    new SqlParameter("@link_status", model.link_status==null?"":model.link_status),
                    new SqlParameter("@link_users", model.link_users==null?"0":model.link_users),
                    new SqlParameter("@link_usersname", model.link_usersname==null?"":model.link_usersname),
                    new SqlParameter("@link_isdelete", model.link_isdelete),
                    new SqlParameter("@link_cust_name", model.link_cust_name==null?"":model.link_cust_name)
            };
            result = Convert.ToInt32(SQLHelp.ExecuteScalar("edit_cust_linkman", System.Data.CommandType.StoredProcedure, param.ToArray()));
            return result;
        }


        /// <summary>
        /// 编辑工作计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int add_workplan(workplan model, string picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", SqlDbType.Int),
                    new SqlParameter("@wp_userid", model.wp_userid==null?"":model.wp_userid),
                    new SqlParameter("@wp_username", model.wp_username==null?"": model.wp_username),
                    new SqlParameter("@wp_content", model.wp_content==null?"":model.wp_content),
                    new SqlParameter("@wp_plandate", model.wp_plandate==null?Convert.ToDateTime("1800-01-01"):model.wp_plandate),
                    new SqlParameter("@wp_endplandate", model.wp_endplandate==null?Convert.ToDateTime("1800-01-01"):model.wp_endplandate),
                    new SqlParameter("@wp_reminddate", model.wp_reminddate==null?Convert.ToDateTime("1800-01-01"):model.wp_reminddate),
                    new SqlParameter("@wp_cust_id", model.wp_cust_id==null?0:model.wp_cust_id),
                    new SqlParameter("@wp_link_id", model.wp_link_id==null?0: model.wp_link_id),
                    new SqlParameter("@wp_status", model.wp_status==null?"":model.wp_status),
                    new SqlParameter("@picture", picture==null?"":picture)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("add_workplan", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        /// <summary>
        /// 编辑工作计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int edit_workplan(workplan model, string picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@wp_userid", model.wp_userid==null?"":model.wp_userid),
                    new SqlParameter("@wp_username", model.wp_username==null?"": model.wp_username),
                    new SqlParameter("@wp_content", model.wp_content==null?"":model.wp_content),
                    new SqlParameter("@wp_plandate", model.wp_plandate==null?Convert.ToDateTime("1800-01-01"):model.wp_plandate),
                    new SqlParameter("@wp_endplandate", model.wp_endplandate==null?Convert.ToDateTime("1800-01-01"):model.wp_endplandate),
                    new SqlParameter("@wp_reminddate", model.wp_reminddate==null?Convert.ToDateTime("1800-01-01"):model.wp_reminddate),
                    new SqlParameter("@wp_cust_id", model.wp_cust_id==null?0:model.wp_cust_id),
                    new SqlParameter("@wp_link_id", model.wp_link_id==null?0: model.wp_link_id),
                    new SqlParameter("@wp_status", model.wp_status==null?"":model.wp_status),
                    new SqlParameter("@picture", picture==null?"":picture)
            };
            result = Convert.ToInt32(SQLHelp.ExecuteScalar("edit_workplan", System.Data.CommandType.StoredProcedure, param.ToArray()));
            return result;
        }

        /// <summary>
        /// 编辑工作报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int add_workreport(workreport model, string t_picture, string m_picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", SqlDbType.Int),
                    new SqlParameter("@report_userid", model.report_userid==null?"":model.report_userid),
                    new SqlParameter("@report_username", model.report_username==null?"":model.report_username),
                    new SqlParameter("@report_type", model.report_type==null?1:model.report_type),
                    new SqlParameter("@report_startdate", model.report_startdate==null?DateTime.Now:model.report_startdate),
                    new SqlParameter("@report_enddate", model.report_enddate==null?DateTime.Now:model.report_enddate),
                    new SqlParameter("@report_content", model.report_content==null?"":model.report_content),
                    new SqlParameter("@report_plan", model.report_plan==null?"":model.report_plan),
                     new SqlParameter("@report_cust_customer_array", model.report_cust_customer_array==null?"":model.report_cust_customer_array),
                      new SqlParameter("@report_cust_linkman_array", model.report_cust_linkman_array==null?"":model.report_cust_linkman_array),
                       new SqlParameter("@report_follow_up_array", model.report_follow_up_array==null?"":model.report_follow_up_array),
                        new SqlParameter("@report_sign_in_array", model.report_sign_in_array==null?"":model.report_sign_in_array),
                    new SqlParameter("@report_reader", model.report_reader==null?"":model.report_reader),
                    new SqlParameter("@report_sender", model.report_sender==null?"":model.report_sender),
                    new SqlParameter("@t_picture", t_picture==null?"":t_picture),
                    new SqlParameter("@m_picture",m_picture==null?"":m_picture)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("add_workreport", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        /// <summary>
        /// 编辑工作报告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int edit_workreport(workreport model, string t_picture, string m_picture)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@report_userid", model.report_userid==null?"":model.report_userid),
                    new SqlParameter("@report_username", model.report_username==null?"":model.report_username),
                    new SqlParameter("@report_type", model.report_type==null?1:model.report_type),
                    new SqlParameter("@report_startdate", model.report_startdate==null?DateTime.Now:model.report_startdate),
                    new SqlParameter("@report_enddate", model.report_enddate==null?DateTime.Now:model.report_enddate),
                    new SqlParameter("@report_content", model.report_content==null?"":model.report_content),
                    new SqlParameter("@report_plan", model.report_plan==null?"":model.report_plan),
                    new SqlParameter("@report_reader", model.report_reader==null?"":model.report_reader),
                    new SqlParameter("@report_sender", model.report_sender==null?"":model.report_sender),
                    new SqlParameter("@t_picture", t_picture==null?"":t_picture),
                    new SqlParameter("@m_picture",m_picture==null?"":m_picture)
            };
            result = Convert.ToInt32(SQLHelp.ExecuteScalar("edit_workreport", System.Data.CommandType.StoredProcedure, param.ToArray()));
            return result;
        }

        /// <summary>
        /// 编辑客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int add_cust_customer(cust_customer model, string link_ids)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", SqlDbType.Int),
                    new SqlParameter("@cust_parent_id", model.cust_parent_id==null?0:model.cust_parent_id),
                    new SqlParameter("@cust_name", model.cust_name==null?"":model.cust_name),
                    new SqlParameter("@cust_type", model.cust_type==null?0:model.cust_type),
                    new SqlParameter("@cust_level", model.cust_level==null?0:model.cust_level),
                    new SqlParameter("@cust_category", model.cust_category==null?0:model.cust_category),
                    new SqlParameter("@cust_address", model.cust_address==null?"":model.cust_address),
                    new SqlParameter("@cust_location", model.cust_location==null?"":model.cust_location),
                    new SqlParameter("@cust_users", model.cust_users==null?"":model.cust_users),
                    new SqlParameter("@cust_usersname", model.cust_usersname),
                    new SqlParameter("@cust_x", model.cust_x==null?0:model.cust_x),
                    new SqlParameter("@cust_y", model.cust_y==null?0:model.cust_y),
                    new SqlParameter("@link_ids", link_ids==null?"":link_ids)
            };
            param[0].Direction = ParameterDirection.Output;
            object obj = SQLHelp.ExecuteNonQuery("add_cust_customer", CommandType.StoredProcedure, param);
            result = Convert.ToInt32(param[0].Value.ToString());
            return result;
        }

        public static int edit_cust_customer(cust_customer model, string link_ids)
        {
            int result = 0;
            SqlParameter[] param = {
                    new SqlParameter("@id", model.id),
                    new SqlParameter("@cust_parent_id", model.cust_parent_id==null?0:model.cust_parent_id),
                    new SqlParameter("@cust_name", model.cust_name==null?"":model.cust_name),
                    new SqlParameter("@cust_type", model.cust_type==null?0:model.cust_type),
                    new SqlParameter("@cust_level", model.cust_level==null?0:model.cust_level),
                    new SqlParameter("@cust_category", model.cust_category==null?0:model.cust_category),
                    new SqlParameter("@cust_address", model.cust_address==null?"":model.cust_address),
                    new SqlParameter("@cust_location", model.cust_location==null?"":model.cust_location),
                    new SqlParameter("@cust_users", model.cust_users==null?"":model.cust_users),
                    new SqlParameter("@cust_usersname", model.cust_usersname),
                    new SqlParameter("@cust_x", model.cust_x==null?0:model.cust_x),
                    new SqlParameter("@cust_y", model.cust_y==null?0:model.cust_y),
                    new SqlParameter("@link_ids", link_ids==null?"":link_ids),
                    new SqlParameter("@cust_isdelete", model.cust_isdelete)
            };
            result = Convert.ToInt32(SQLHelp.ExecuteScalar("edit_cust_customer", System.Data.CommandType.StoredProcedure, param.ToArray()));
            return result;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="ht">HashTable</param>
        /// <param name="fileds">字段</param>
        /// <param name="RowCount">返回的行数</param>
        /// <param name="IsPage">是否分页</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public static DataTable getlistpage(Hashtable ht, string fileds, out int RowCount, bool IsPage, string Where = "")
        {
            RowCount = 0;
            SqlParameter[] parms4org = { };
            int StartIndex = 0;
            int EndIndex = 0;
            string order = "";
            if (IsPage)
            {
                StartIndex = Convert.ToInt32(ht["StartIndex"].ToString());
                EndIndex = Convert.ToInt32(ht["EndIndex"].ToString());
            }
            try
            {
                if (ht.ContainsKey("Order") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["Order"]))) order = ht["Order"].ToString();
                DataTable dt = SQLHelp.GetListByPage((string)ht["TableName"], fileds, Where, order, StartIndex, EndIndex, IsPage, parms4org, out RowCount);

                //DataTable dt = SQLHelp.GetListByPage(ht, parms4org);
                return dt;
            }
            catch (Exception ex)
            {
                //写入日志
                //throw;
                return null;
            }
        }

    }
}
