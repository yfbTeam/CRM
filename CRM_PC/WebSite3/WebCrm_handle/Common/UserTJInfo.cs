using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCrm_handle.Common
{
    public class UserTJInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartMentId { get; set; }
        public string DepartMentName { get; set; }
        //签到
        public int count_sign { get; set; }
        //跟进
        public int count_follow { get; set; }
        //评论
        public int count_com_userid { get; set; }
        //客户
        public int count_cust_users { get; set; }
        //联系人
        public int count_link_users { get; set; }
        //工作计划
        public int count_wp_userid { get; set; }
        //工作报告
        public int count_report_userid { get; set; }
    }
}