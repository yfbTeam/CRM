using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
   public class report
    {
        //跟进记录类
        public report_info report_info { get; set; }

        public List<Dictionary<string, object>> comment { get; set; }
        public List<Dictionary<string, object>> picture { get; set; }

        public List<Dictionary<string, object>> praise { get; set; }
    }
}
