using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDHelper.Entity
{
    public class UserInfo
    {
        public UserInfo()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string tel { get; set; }
        public string workPlace { get; set; }
        public string remark { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public bool active { get; set; }
        public object orderInDepts { get; set; }
        public bool isAdmin { get; set; }
        public bool isBoss { get; set; }
        public string dingId { get; set; }
        public object isLeaderInDepts { get; set; }
        public bool isHide { get; set; }
        public object department { get; set; }
        public string position { get; set; }
        public string avatar { get; set; }
        public string jobnumber { get; set; }
        public object extattr { get; set; }

    }




    //    "errcode": 0,
    //"errmsg": "ok",
    //"userid": "zhangsan",
    //"name": "张三",
    //"tel" : "010-123333",
    //"workPlace" :"",
    //"remark" : "",
    //"mobile" : "13800000000",
    //"email" : "dingding@aliyun.com",
    //"active" : true,
    //"orderInDepts" : "{1:10, 2:20}",
    //"isAdmin" : false,
    //"isBoss" : false,
    //"dingId" : "WsUDaq7DCVIHc6z1GAsYDSA",
    //"isLeaderInDepts" : "{1:true, 2:false}",
    //"isHide" : false,
    //"department": [1, 2],
    //"position": "工程师",
    //"avatar": "dingtalk.com/abc.jpg",
    //"jobnumber": "111111",
    //"extattr": 
}
