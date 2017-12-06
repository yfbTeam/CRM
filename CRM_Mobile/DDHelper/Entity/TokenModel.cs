using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDHelper.Entity
{
    public class TokenModel
    {
        public TokenModel()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string access_token  { get; set; }

        public string errcode { get; set; }

        public string errmsg { get; set; }
    }
}
