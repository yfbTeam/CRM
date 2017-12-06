using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    /// <summary>
    /// 联系人辅助类
    /// </summary>
    public class linkman
    {
        /// <summary>
        /// 获取联系人
        /// </summary>
        public linkman_info cust_linkman { get; set; }
        /// <summary>
        /// 获取工作计划
        /// </summary>
        public List<Dictionary<string, object>> workplan { get; set; }
        /// <summary>
        /// 跟进记录
        /// </summary>
        public List<Dictionary<string, object>> follow_up { get; set; }
        /// <summary>
        /// 签到记录
        /// </summary>
        public List<Dictionary<string, object>> sign { get; set; }

    }
}
