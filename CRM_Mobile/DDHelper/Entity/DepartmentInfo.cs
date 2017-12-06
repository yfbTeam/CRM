using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace DDHelper.Entity
{
    /// <summary>
    /// DepartmentInfo 的摘要说明
    /// </summary>
    public class DepartmentInfo
    {
        public DepartmentInfo()
        {

        }

        public int errcode { get; set; }

        public string errmsg { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public int order { get; set; }

        public int parentid { get; set; }

        public bool createDeptGroup { get; set; }

        public bool autoAddUser { get; set; }

        public bool deptHiding { get; set; }

        public object deptPerimits { get; set; }

        public object userPerimits { get; set; }

        public bool outerDept { get; set; }

        public object outerPermitDepts { get; set; }

        public string orgDeptOwner { get; set; }

        public object deptManagerUseridList { get; set; }
        

        
    }


    //  "name": "钉钉事业部",
    //"order" : 10,
    //"parentid": 1,
    //"createDeptGroup": true,
    //"autoAddUser": true,
    //"deptHiding" : true,
    //"deptPerimits" : "3|4",
    //"userPerimits" : "userid1|userid2",
    //"outerDept" : true,
    //"outerPermitDepts" : "1|2",
    //"outerPermitUsers" : "userid3|userid4",
    //"orgDeptOwner" : "manager1122",
    //"deptManagerUseridList" : "manager1122|manager3211"

}