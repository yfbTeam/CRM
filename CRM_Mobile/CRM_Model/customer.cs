using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    /// <summary>
    /// 客户辅助类，主要是客户详情信息里面的辅助类
    /// </summary>
    public class customer
    {
        //客户信息
        public customer_info cust { get; set; }
        //联系人信息
        public List<Dictionary<string, object>> linkman { get; set; }
        ///// <summary>
        ///// 获取工作计划
        ///// </summary>
        //public List<Dictionary<string, object>> workplan { get; set; }
        /// <summary>
        /// 跟进记录
        /// </summary>
        public List<Dictionary<string, object>> follow_up { get; set; }
        /// <summary>
        /// 签到记录
        /// </summary>
        public List<Dictionary<string, object>> sign { get; set; }

        public int Count_All_follow { get; set; }

        public int Count_All_linkman { get; set; }

        public int Count_All_sign{ get; set; }
    }
}
