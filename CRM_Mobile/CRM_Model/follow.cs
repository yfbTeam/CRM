using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Model
{
    public class follow
    {
        //跟进记录类
        public follow_up_info follow_up_info { get; set; }
              
       
        //联系人
        public List<Dictionary<string, object>> cust_linkman { get; set; }
        //图片
        public List<Dictionary<string, object>> picture { get; set; }
        ////语音
        //private List<Dictionary<string, object>> audio;
        public List<Dictionary<string, object>> comment { get; set; }

        public List<Dictionary<string, object>> praise { get; set; }
    }
}
