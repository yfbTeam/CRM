using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    /// <summary>
    /// 统计详情辅助类
    /// </summary>
    public class statistic_info
    {
        /// <summary>
        /// 新增客户数
        /// </summary>
        public string s_cust_customer_count { get; set; }
        /// <summary>
        /// 新增联系人数
        /// </summary>
        public string custname { get; set; }
        /// <summary>
        /// 新增签到人数
        /// </summary>
        public string s_linkman_count { get; set; }

        /// <summary>
        /// 拜访次数
        /// </summary>
        public string linkname { get; set; }
        /// <summary>
        /// 拜访联系人数
        /// </summary>
        public string s_sign_count { get; set; }
        public string signcount { get; set; }
        public string s_bf_count { get; set; }
        public string follow_up_name { get; set; }
        public string followcount { get; set; }

    }
}

