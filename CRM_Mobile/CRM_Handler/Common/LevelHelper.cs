using CRM_Common;
using CRM_Handler.PubParam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM_Handler.Common
{
    public static class LevelHelper
    {
        #region 客户

        /// <summary>
        /// 获取客户等级显示
        /// </summary>
        public static string GetCustom_level(int custom_level_int)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_customer_Level != null)
                {
                    //客户等级
                    string custom_level_str = Convert.ToString(custom_level_int);
                    result = !pub_param_handle.dic_customer_Level.ContainsKey(custom_level_str) ? "" : pub_param_handle.dic_customer_Level[custom_level_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string GetCustom_level(string custom_level_str)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_customer_Level != null)
                {
                    //联系人等级              
                    result = !pub_param_handle.dic_customer_Level.ContainsKey(custom_level_str) ? "" : pub_param_handle.dic_customer_Level[custom_level_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string GetCustom_Type(string custom_type_str)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_customer_Type != null)
                {
                    //联系人等级              
                    result = !pub_param_handle.dic_customer_Type.ContainsKey(custom_type_str) ? "" : pub_param_handle.dic_customer_Type[custom_type_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string GetCustom_Property(string custom_Property_str)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_customer_Property != null)
                {
                    //联系人等级              
                    result = !pub_param_handle.dic_customer_Property.ContainsKey(custom_Property_str) ? "" : pub_param_handle.dic_customer_Property[custom_Property_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 联系人

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string Getlink_level(int link_level_int)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_linkMan_Grade != null)
                {
                    //联系人等级
                    string link_level_str = Convert.ToString(link_level_int);
                    result = !pub_param_handle.dic_linkMan_Grade.ContainsKey(link_level_str) ? "" : pub_param_handle.dic_linkMan_Grade[link_level_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string Getlink_level(string link_level_str)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_linkMan_Grade != null)
                {
                    //联系人等级              
                    result = !pub_param_handle.dic_linkMan_Grade.ContainsKey(link_level_str) ? "" : pub_param_handle.dic_linkMan_Grade[link_level_str];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        #endregion

        #region 跟进

        /// <summary>
        ///获取联系人等级显示
        /// </summary>
        public static string Getfollow_level(string follow_level_str)
        {
            string result = string.Empty;
            try
            {
                if (pub_param_handle.dic_follow_Level != null)
                {
                    //联系人等级              
                    result = !pub_param_handle.dic_follow_Level.ContainsKey(follow_level_str) ? "" : pub_param_handle.dic_follow_Level[follow_level_str];
                }
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