using CRM_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRM_Handler.Custom;
using CRM_Handler.Follow;
using CRM_Handler.LinkMan;
using CRM_Handler.Remind;
using CRM_Handler.WorkPlan;
using CRM_Handler.Share;
using CRM_Handler.Report;

namespace CRM_Handler.Common
{
    public static class Data_check_helper
    {

        /// <summary>
        /// 判断当前人的缓存数据在不在
        /// </summary>
        public static bool check_Self(HanderType HanderType, string guid)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(guid))
                {
                    switch (HanderType)
                    {
                        case HanderType.custom:
                            if (cust_customer_handle.dic_Self.ContainsKey(guid))
                            {
                                result = true;
                            }

                            break;
                        case HanderType.linkman:
                            if (cust_linkman_handle.dic_Self.ContainsKey(guid))
                            {
                                result = true;
                            }
                            break;
                        case HanderType.report:
                            if (workreport_handle.dic_Self.ContainsKey(guid))
                            {
                                result = true;
                            }
                            break;

                        case HanderType.workplane:
                            if (workplan_handle.dic_Self.ContainsKey(guid))
                            {
                                result = true;
                            }
                            break;
                        case HanderType.follow:
                            if ( follow_up_handle. dic_Self.ContainsKey(guid))
                            {
                                result = true;
                            }
                            break;
                        default:
                            break;
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