using CRM_Common;
using CRM_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM_Handler.Admin
{
    /// <summary>
    /// admin_helper 的摘要说明
    /// </summary>
    public class admin_helper : IHttpHandler
    {
        #region 字段
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        JsonModel jsonModel = null;

        #endregion

        #region 中心入口点
        
        public void ProcessRequest(HttpContext context)
        {
            string func = context.Request["func"] ?? "";
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);

                switch (func)
                {
                    case "import_data":
                        import_cust_customer(context);
                        break;
                 
                  
                    default:
                        jsonModel = new JsonModel()
                        {
                            errNum = 5,
                            errMsg = "没有此方法",
                            retData = ""
                        };
                        context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex);
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 批量导入客户

        /// <summary>
        /// 批量导入客户
        /// </summary>
        /// <param name="request"></param>
        public void import_cust_customer(HttpContext context)
        {
            try
            {
            
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

    }
}