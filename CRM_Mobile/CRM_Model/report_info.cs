using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    public class report_info
    {
        public long id { get; set; }
        public int report_type { get; set; }
        public string report_content { get; set; }
        public string report_startdate { get; set; }
        public string report_enddate { get; set; }
        public string report_plan { get; set; }
        public string praise_all { get; set; }
        public string report_username { get; set; }

        public string is_praise { get; set; }
        public string report_createdate { get; set; }

        public long work_report_id { get; set; }

        public int picture_count { get; set; }

    }
}
