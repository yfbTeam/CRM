using CRM_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace CRM_Handler.Common
{
    public static class RequestHelper
    {
        /// <summary>
        ///string转
        /// </summary>
        public static string string_transfer(HttpRequest request, string string_name)
        {
            string result = string.Empty;
            try
            {
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && request[string_name] != null)
                {
                    result = request[string_name];
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///string转【】
        /// </summary>
        public static string string__double(HttpRequest request, string string_name,string value)
        {
            string result = string.Empty;
            try
            {
                result = request[string_name] ?? value;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///int转
        /// </summary>
        public static int int_transfer(HttpRequest request, string string_name)
        {
            int result =0;
            try
            {
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && !string.IsNullOrEmpty(request[string_name]))
                {
                    string data = request[string_name];
                    bool has_value = int.TryParse(data, out result);
                    if (!has_value)
                    {
                        result = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///DateTime转
        /// </summary>
        public static DateTime DateTime_transfer(HttpRequest request, string string_name)
        {
            DateTime result = Convert.ToDateTime("1800-01-01");
            try
            {
                var st = request[string_name];
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && !string.IsNullOrEmpty(st))
                {
                    string data = request[string_name];
                    bool has_value = DateTime.TryParse(data, out result);
                    if (!has_value)
                    {
                        result = Convert.ToDateTime("1800-01-01");
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///long转
        /// </summary>
        public static long long_transfer(HttpRequest request, string string_name)
        {
            long result = 0;
            try
            {
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && !string.IsNullOrEmpty(request[string_name]))
                {
                    string data = request[string_name];
                    bool has_value = long.TryParse(data, out result);
                    if (!has_value)
                    {
                        result =0;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        ///bool转
        /// </summary>
        public static bool bool_transfer(HttpRequest request, string string_name)
        {
            bool result = false;
            try
            {
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && !string.IsNullOrEmpty(request[string_name]))
                {
                    string data = request[string_name];
                    bool has_value = bool.TryParse(data, out result);
                    if (!has_value)
                    {
                        result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// decimal转
        /// </summary>
        /// <param name="request"></param>
        /// <param name="string_name"></param>
        /// <returns></returns>
        public static decimal decimal_transfer(HttpRequest request, string string_name)
        {
            decimal result = 0;
            try
            {
                //确保传入值和request保存的值在
                if (!string.IsNullOrEmpty(string_name) && !string.IsNullOrEmpty(request[string_name]))
                {
                    string data = request[string_name];
                    bool has_value = decimal.TryParse(data, out result);
                    if (!has_value)
                    {
                        result = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
    }
}