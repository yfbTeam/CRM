using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEUtility
{
    public enum SysStatus
    {
        正常 = 0,
        删除 = 1,
        归档 = 2
    }  
    public enum Display
    {
        显示 = 0,
        隐藏 = 1
    }
    public enum AutoNotice
    {
        社团 = 0,
        活动 = 1,
        宿舍 = 2
    }
    public enum MessageStatus
    {
        未读 = 0,
        已读 = 1
    }
    public enum MessageIsSend
    {
        未发送 = 0,
        已发送 = 1
    }
    public enum MessageTiming
    {
        立即发送 = 0,
        定时发送 = 1
    }
    public enum FusionChartType
    {
        None = 0,
        /// <summary>
        /// 2D柱状图
        /// </summary>
        MSColumn2D = 1,

        /// <summary>
        /// 仪表盘
        /// </summary>
        AngularGauge = 2,

        /// <summary>
        /// 2D饼图
        /// </summary>
        Pie2D = 3,

        /// <summary>
        /// 漏斗图
        /// </summary>
        Funnel = 4,

        /// <summary>
        /// 气泡图
        /// </summary>
        Bubble = 5,

        /// <summary>
        /// 趋势线图
        /// </summary>
        Line = 6,
        ///<summary>
        ///环形图
        ///</summary>
        Doughnut2D = 7,
        ///<summary>
        ///多Y轴图
        ///</summary>
        MSCombiDY2D = 8,
        /// <summary>
        /// 两线图
        /// </summary>
        Line2 = 9,
        /// <summary>
        /// 3D饼图
        /// </summary>
        Pie3D = 10,
    }

    public enum enumUserType 
    {
        教师=1,
        学生=2,
        家长=3,
        员工=4
    }

    public enum AuthenType 
    {
        新用户注册=0,
        用户未激活=1,
        用户已激活=2
    }

    public enum isEnable 
    { 
        启用=0,
        禁用=1
    }
}
