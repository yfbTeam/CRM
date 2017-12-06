using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    public class linkman_info
    {
        /// <summary>
        /// 联系人名称
        /// </summary>
        public string link_name { get; set; }
        /// <summary>
        /// 联系人职位
        /// </summary>
        public string link_position { get; set; }
        /// <summary>
        /// 联系人等级
        /// </summary>
        public string link_level { get; set; }
        /// <summary>
        /// 联系人等级名称
        /// </summary>
        public string link_level_name { get; set; }
        /// <summary>
        /// <summary>
        /// 电话号码
        /// </summary>
        public string link_phonenumber { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string link_email { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string customer_name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string link_username { get; set; }

        /// <summary>
        /// 联系人ID
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 联系人所属的客户ID
        /// </summary>
        public string link_cust_id { get; set; }


    }
}
