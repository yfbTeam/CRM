using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    public class follow_up_info
    {
        public long id { get; set; }
        public long follow_cust_id { get; set; }
        public long follow_link_id { get; set; }
        public string follow_cust_name { get; set; }
        public string follow_link_name { get; set; }
        public string follow_username { get; set; }
        public string follow_date { get; set; }
        public string follow_content { get; set; }
        public string praise_all { get; set; }
        public string is_praise { get; set; }

        public int picture_count { get; set; }

        public string IsAdmin { get; set; }
    }
}
