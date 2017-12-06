using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDHelper.Entity
{
    public class UserModel
    {
        public UserModel()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string userid { get; set; }

        public string deviceId { get; set; }

        public string is_sys { get; set; }

        public string sys_level { get; set; }
    }
}
