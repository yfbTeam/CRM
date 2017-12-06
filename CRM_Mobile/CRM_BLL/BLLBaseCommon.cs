using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_BLL_DAL;
using CRM_Model;
using System.Collections;
using System.Text.RegularExpressions;

namespace CRM_BLL
{
    /// <summary>
    /// 此类主要是调用DAL的类，特殊情况下使用
    /// </summary>
    public class BLLBaseCommon
    {
        //获取客户信息
        public customer_info getcust_customer(customer_info entity,string where)
        {
            return DALBaseCommon.getcust_customer(entity, where);
        }

        public workreport workreport_info(workreport entity,string id)
        {
            return DALBaseCommon.workreport_info(entity, id);
        }

        public DataTable get_cust_customer_by_custname(string cust_name,long custid)
        {
            return DALBaseCommon.get_cust_customer_by_custname(cust_name, custid);
        }

        //获取联系人信息
        public DataTable getcust_linkman_list(string link_cust_id)
        {
            return DALBaseCommon.getcust_linkman_list(link_cust_id);
        }
        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        public  DataTable get_statistic(string userid,string username,string startdate,string enddate)
        {
            return DALBaseCommon.get_statistic(userid, username, startdate,enddate);
        }

        /// <summary>
        /// 获取今日事项
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable get_today_matters(string userid)
        {
            return DALBaseCommon.get_today_matters(userid);
        }

        /// <summary>
        /// 获取日报详情
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable get_report_info(string id)
        {
            return DALBaseCommon.get_report_info(id);
        }

        public DataTable get_praise(string id) {
            return DALBaseCommon.get_praise(id);
        }

        /// <summary>
        /// 获取日历是否签到
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable get_is_sign(string sign_date, int userid)
        {
            return DALBaseCommon.get_is_sign(sign_date, userid);
        }


        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns></returns>
        public  DataTable get_statistic_detail(string userid, string sd_type)
        {
            return DALBaseCommon.get_statistic_detail(userid, sd_type);
        }

        //获取联系人信息
        public linkman_info getcust_linkman(linkman_info entity,string id)
        {
            return DALBaseCommon.getcust_linkman(entity,id);
        }

        /// <summary>
        /// 获取工作计划第一条信息
        /// </summary>
        /// <returns></returns>
        public  DataTable getworkplan_list(string wp_userid,string table_id,string type)
        {
            return DALBaseCommon.getworkplan_list(wp_userid, table_id,type);
        }

        /// <summary>
        /// 获取签到记录第一条信息
        /// </summary>
        /// <returns></returns>
        public  DataTable getsign_list(string sign_userid)
        {
            return DALBaseCommon.getsign_list(sign_userid);
        }

        /// <summary>
        /// 根据id获取联系人职位
        /// </summary>
        /// <param name="sign_userid"></param>
        /// <returns></returns>
        public DataTable get_link_position_by_id(string id)
        {
            return DALBaseCommon.get_link_position_by_id(id);
        }
        /// <summary>
        /// 跟进记录获取所有跟进客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable get_cust_list(string userid)
        {
            return DALBaseCommon.get_cust_list(userid);
        }

        /// <summary>
        /// 获取签到记录第一条信息
        /// </summary>
        /// <returns></returns>
        public DataTable getfollow_up_list(string follow_userid,string tableid,string type)
        {
            return DALBaseCommon.getfollow_up_list(follow_userid, tableid,type);
        }

        /// <summary>
        /// 获取图片集合
        /// </summary>
        /// <param name="pic_en_table"></param>
        /// <param name="pic_table_id"></param>
        /// <returns></returns>
        public DataTable getpicture_list(string pic_en_table,string pic_table_id)
        {
            return DALBaseCommon.getpicture_list(pic_en_table,pic_table_id);
        }

        public DataTable getcomment_list(string com_table_id,string com_type)
        {
            return DALBaseCommon.getcomment_list(com_table_id, com_type);
        }
        public DataTable cust_customer_nearby(string sign_x,string sign_y,string cust_users) {
            return DALBaseCommon.cust_customer_nearby(sign_x, sign_y, cust_users);
        }
        public DataTable get_statistic_today(string userid, string username, string type)
        {
            return DALBaseCommon.get_statistic_today(userid, username, type);
        }
        /// <summary>
        /// 获取分页的DataTable
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="fileds"></param>
        /// <param name="RowCount"></param>
        /// <param name="IsPage"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        public  DataTable getlistpage(Hashtable ht, string fileds, out int RowCount, bool IsPage, string Where = "")
        {
            return DALBaseCommon.getlistpage(ht,fileds,out RowCount,IsPage,Where);
        }

        /// <summary>
        /// 获取提醒设置信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="remind_userid"></param>
        /// <param name="remind_type"></param>
        /// <returns></returns>
        public remind_setting get_remind_setting_info(remind_setting entity,string remind_userid, string remind_type)
        {
            return DALBaseCommon.get_remind_setting_info(entity,remind_userid, remind_type);
        }

        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public string ConverDatetime(string jsonString)
        {
            jsonString = Regex.Replace(jsonString, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return jsonString;
        }

        /// <summary>
        /// 编辑签到
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonModel edit_remind_setting(remind_setting model)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_remind_setting(model);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_report_sender(int table_id,string report_reader,string report_sender)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_report_sender(table_id, report_reader, report_sender);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        /// <summary>
        /// 编辑签到
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonModel edit_sign_in(sign_in model)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_sign_in(model);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }
        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable edit_praise(praise model)
        {
            return DALBaseCommon.edit_praise(model);
        }

        ///// <summary>
        ///// 编辑签到
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public JsonModel edit_sign_in(sign_in model)
        //{
        //    JsonModel jsonModel = new JsonModel();
        //    try
        //    {
        //        int result = DALBaseCommon.edit_sign_in(model);
        //        jsonModel = new JsonModel()
        //        {
        //            errNum = 0,
        //            errMsg = result == 0 ? "success" : "",
        //            retData = result
        //        };
        //        return jsonModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonModel = new JsonModel()
        //        {
        //            errNum = 400,
        //            errMsg = ex.Message,
        //            retData = ""
        //        };
        //        return jsonModel;
        //    }
        //}

        /// <summary>
        /// 新增跟进记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonModel add_follow_up(follow_up model,string picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.add_follow_up(model, picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_follow_up(follow_up model, string picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_follow_up(model, picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonModel add_cust_customer(cust_customer model,string link_ids)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.add_cust_customer(model, link_ids);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };             
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_cust_customer(cust_customer model, string link_ids)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_cust_customer(model, link_ids);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel add_cust_linkman(cust_linkman model)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.add_cust_linkman(model);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_cust_linkman(cust_linkman model)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_cust_linkman(model);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel add_workplan(workplan model,string picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.add_workplan(model, picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_workplan(workplan model, string picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_workplan(model, picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel add_workreport(workreport model, string t_picture, string m_picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.add_workreport(model, t_picture, m_picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel edit_workreport(workreport model, string t_picture, string m_picture)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int result = DALBaseCommon.edit_workreport(model, t_picture, m_picture);
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg ="success",
                    retData = result
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }
    }
}
