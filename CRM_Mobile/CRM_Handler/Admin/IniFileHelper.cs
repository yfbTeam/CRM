using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using CRM_Model;
using System.Text;

namespace CRM_Handler.Admin
{

    public class IniFileHelper
    {
        [DllImport("kernel32")]
        //插入数据
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        public static void write(HanderType HanderType, TableBase tableBase)
        {
            string section = Convert.ToString(HanderType);
           StringBuilder  builder_string = new StringBuilder();
            switch (HanderType)
            {
                case HanderType.custom:
                    cust_customer cs = tableBase as cust_customer;

                    string info = "插入客户：" + cs.cust_name + " ";
                    //WritePrivateProfileString(section,"add","")
                    break;
                case HanderType.linkman:
                    break;
                case HanderType.report:
                    break;
                case HanderType.share:
                    break;
                case HanderType.workplane:
                    break;
                default:
                    break;
            }
        }
    }

   


}