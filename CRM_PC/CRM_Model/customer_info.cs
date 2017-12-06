using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    public class customer_info
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string cust_id { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string cust_name { get; set; }
        /// <summary>
        /// 客户等级
        /// </summary>
        public string cust_level { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string cust_usersname { get; set; }
        /// <summary>
        /// 姓名id
        /// </summary>
        public string cust_users_id { get; set; }
        /// <summary>
        /// 最后跟进时间
        /// </summary>
        public string cust_followdate { get; set; }

        /// <summary>
        /// 客户属性
        /// </summary>
        public int cust_category { get; set; }
      
    }
}
