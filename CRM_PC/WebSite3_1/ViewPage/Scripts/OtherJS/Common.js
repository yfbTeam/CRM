
//退出
$(function(){
    $('.settings').hover(function () {
        $(this).find('.setting_none').show();
    }, function () {
        $(this).find('.setting_none').hide();
    })
})
/**日期转换成时间字符串**/
/** date日期  **/
/** format要转换的字符串格式，默认为 "yyyy-MM-dd"格式，若没有需要的格式可自己添加 **/
function DateTimeConvert(date, format, iseval) {
    iseval = iseval == false ? false : true;
    if (iseval) {
        date = date.replace(/(^\s*)|(\s*$)/g, "");
        if (date == "") {
            return "";
        }
        if (date.indexOf("Date") != -1) {
            date = eval(date.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
        } else {
            date = new Date(Date.parse(date));
        }
    }
    var year = date.getFullYear();
    var month = (date.getMonth() + 1).toString();
    var twoMonth = month.length == 1 ? "0" + month : month; //月份为1位数时，前面加0
    var day = (date.getDate()).toString();
    var twoDay = day.length == 1 ? "0" + day : day; //天数为1位数时，前面加0
    var hour = (date.getHours()).toString();
    var twoHour = hour.length == 1 ? "0" + hour : hour; //小时数为1位数时，前面加0
    var minute = (date.getMinutes()).toString();
    var twoMinute = minute.length == 1 ? "0" + minute : minute; //分钟数为1位数时，前面加0
    var second = (date.getSeconds()).toString();
    var twoSecond = second.length == 1 ? "0" + second : second; //秒数为1位数时，前面加0
    var dateTime;
    if (format == "yyyy-MM-dd HH:mm:ss") {
        dateTime = year + "-" + twoMonth + "-" + twoDay + " " + twoHour + ":" + twoMinute + ":" + twoSecond;
    } else if (format == "yyyy-MM-dd HH:mm") {
        dateTime = year + "-" + twoMonth + "-" + twoDay + " " + twoHour + ":" + twoMinute;
    } else if (format == "年月日") {
        dateTime = year + "年" + month + "月" + day + "日";
    } else if (format == "yyyy-MM") {
        dateTime = year + "-" + twoMonth
    } else if (format == "MM-dd") {
        dateTime = twoMonth + "-" + twoDay
    }
    else if (format == "dd") {
        dateTime = twoDay;
    }
    else {
        dateTime = year + "-" + twoMonth + "-" + twoDay
    }
    return dateTime;
}


function getDateDiff(dateTimeStamp) {
    var minute = 1000 * 60;
    var hour = minute * 60;
    var day = hour * 24;
    var halfamonth = day * 15;
    var month = day * 30;
    var now = new Date().getTime();
    var diffValue = now - dateTimeStamp;
    if (diffValue < 0) {
        return "刚刚";
    }
    var monthC = diffValue / month;
    var weekC = diffValue / (7 * day);
    var dayC = diffValue / day;
    var hourC = diffValue / hour;
    var minC = diffValue / minute;
    if (monthC >= 1) {
        result = parseInt(monthC) + "个月前";
    }
    else if (weekC >= 1) {
        result = parseInt(weekC) + "周前";
    }
    else if (dayC >= 1) {
        result = parseInt(dayC) + "天前";
    }
    else if (hourC >= 1) {
        result = parseInt(hourC) + "个小时前";
    }
    else if (minC >= 1) {
        result = parseInt(minC) + "分钟前";
    } else
        result = "刚刚";
    return result;
}

/**根据IsDelete字段转换是否归档描述**/
/** int数字  **/
function DataState(IsDelete) {
    if (IsDelete == 0) {
        return "正常";
    } else if (IsDelete == 1) {
        return "删除";
    } else if (IsDelete == 2) {
        return "归档";
    }
}
//获取URL参数
function GetUrlDate() {
    var name, value;
    var str = location.href; //取得整个地址栏
    var num = str.indexOf("?")
    str = str.substr(num + 1); //取得所有参数   stringvar.substr(start [, length ]

    var arr = str.split("&"); //各个参数放到数组里
    for (var i = 0; i < arr.length; i++) {
        num = arr[i].indexOf("=");
        if (num > 0) {
            name = arr[i].substring(0, num);
            value = arr[i].substr(num + 1);
            this[name] = value;
        }
    }
}

/**改变启用禁用状态**/
/**obj当前超链接对象，itemid 选中行的主键，status 启用禁用的值，tablename 表名**/
function ChangeUseStatus(obj, itemid, status, tablename) {
    if (itemid != null && itemid != "") {
        jQuery.ajax({
            url: WebServiceUrl + "/Common/CommonFunction.asmx/ChangeUseStatus?jsoncallback=?",//random" + Math.random(),//方法所在页面和方法名
            type: "POST",
            data: { itemid: itemid, Status: status, tablename: tablename },
            success: function (json) {
                if (json.result != "0") {
                    //if (status == 0) {
                    //    $(obj).next().attr("class", "Disable");
                    //} else {
                    //    $(obj).prev().attr("class", "Disable");
                    //}
                    //$(obj).attr("class", "Enable");
                    getData(1);
                } else {
                    layer.msg("操作失败！");
                }
            },
            error: function (request) {
                layer.msg("操作失败");
            }
        });
    }
}
/*************************************IFrame弹框方法***********************************************************/
var curWinindex;
function OpenIFrameWindow(title, url, width, height) {
    //iframe层
    var index = layer.open({
        type: 2,
        title: title,
        shadeClose: false,
        shade: 0.2,
        area: [width, height],
        content: url //iframe的url
    });
    curWinindex = index;
}
function CloseIFrameWindow() {
    layer.close(curWinindex);
}
function layerMsg(title) { //msg信息框
    layer.msg(title, {
        time: 0 //不自动关闭
        , btn: ['确定']
        , yes: function (index) {
            layer.close(index);
        }
    });
}

function OpenIFrameWindow1(title, url, width, height) {
    //iframe层
    var index = layer.open({
        type: 2,
        title: title,
        shadeClose: false,
        shade: 0.2,
        area: [width, height],
        offset: '20px',
        content: url //iframe的url
    });
    curWinindex = index;
}
/****************************************结束********************************************************/

/**********************************************ZTree相关方法**********************************************/
//绑定Ztree树
function treeBind(treeId, url, data, setting) {
    /// <summary>绑定Ztree树</summary>
    /// <param name="treeId" type="String">树控件ul的id</param>
    /// <param name="url" type="String">JSON数据源路径</param>  
    /// <param name="data" type="String">传递的参数,可为''</param>
    /// <param name="setting" type="String">Ztree树的配置信息,可为''</param>
    /// <returns>zTree 对象</returns>
    if (setting == "") {
        setting = {
            view: {
                selectedMulti: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            }
        };
    }
    $.ajax({
        type: "post",
        url: url,
        data: data,
        dataType: "JSON",
        async: false,
        cache: false,
        success: function (json) {
            $("#" + treeId).html('');
            var result = json.result;
            if (result.errNum == 0 && result.retData.length > 0) {
                var zTreeNode = $.parseJSON(result.retData);
                var zTreeObj = $.fn.zTree.init($("#" + treeId), setting, zTreeNode); //返回树对象
                zTreeObj.expandNode(zTreeObj.getNodeByParam("id", 0, null), true, false, false); //展开第一个顶级节点
            } else {
                //layer.msg("您没有此权限!");
            }
        },
        error: function () {
            layer.msg("Ajax请求数据失败!");
        }
    });
    //return $.fn.zTree.init($("#" + treeId), setting, zTreeNode); //返回树对象
}
//获取选择节点集合(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )  
function getChildNodes(ulZtreeId) {
    /// <summary>获取选择节点集合(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )</summary>
    /// <param name="ulZtreeId" type="String">Ztree树Id</param>
    /// <param name="getEndChild" type="Bool">是否只获取最低级节点的值,默认获取全部的.</param>
    var getEndChild = arguments[1] || false;

    var treeObj = $.fn.zTree.getZTreeObj(ulZtreeId);
    var treeNode = treeObj.getCheckedNodes(true);
    var data = eval(treeNode);
    var str = "";
    $.each(data, function (n, value) {
        if (getEndChild) {
            if (value.check_Child_State == '-1')  //只获取最底级节点的值
                str += value.id + ',';
        }
        else
            str += value.id + ',';

    });
    return str = str.substr(0, str.length - 1);
}

//设置指定树的节点选中(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )  
function setNodesCheck(treeId, nodesList) {
    /// <summary>设置指定树的节点选中(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )</summary>
    /// <param name="nodesList" type="String">节点集合(例'1,2,3')</param>
    var treeObj = $.fn.zTree.getZTreeObj(treeId);
    var strArray = nodesList.split(',');
    var nodes = null;
    treeObj.checkAllNodes(false);
    $.each(strArray, function (i, n) {
        nodes = treeObj.getNodeByParam("id", n, null);
        treeObj.checkNode(nodes, true, false, false);
    });
}

//取消指定树的节点的选中状态(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )
function setNodesNoCheck(treeId, nodesList) {
    /// <summary>取消指定树的节点的选中状态(用于ztree插件,且依赖 jquery.ztree.excheck 扩展 js )</summary>
    /// <param name="nodesList" type="String">节点集合(例'1,2,3')</param>

    var treeObj = $.fn.zTree.getZTreeObj(treeId);
    var strArray = nodesList.split(',');
    var nodes = null;
    $.each(strArray, function (i, n) {
        nodes = treeObj.getNodeByParam("id", n, null);
        treeObj.checkNode(nodes, false, false, false);
    });
}



/**********************************************结束**********************************************/


//JS操作cookies****************************************
//获取
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}


//写cookies
function setCookie(name, value) {
    var Days = 1;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}
//删除
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

//序号
var pageNum = 1;
function pageIndex() {
    return pageNum++;
}

//列表名称长度修正
function NameLengthUpdate(Name, Length) {
    if (Name.length > Length) {
        return Name.substr(0, Length) + "...";
    }
    return Name;
}
/** 
* js截取字符串，中英文都能用 
* @param str：需要截取的字符串 
* @param len: 需要截取的长度 
*/
function cutstr(str, len) {
    var str_length = 0;
    var str_len = 0;
    str_cut = new String();
    str_len = str.length;
    for (var i = 0; i < str_len; i++) {
        a = str.charAt(i);
        str_length++;
        if (escape(a).length > 4) {
            //中文字符的长度经编码之后大于4  
            str_length++;
        }
        str_cut = str_cut.concat(a);
        if (str_length >= len) {
            str_cut = str_cut.concat("...");
            return str_cut;
        }
    }
    //如果给定字符串小于指定长度，则返回源字符串；  
    if (str_length < len) {
        return str;
    }
}

function OnError(XMLHttpRequest, textStatus, errorThrown) { }

/*
*@param Title 邮箱或者消息的标题
*@param Contents 邮箱内容或者消息的内容
*@param Type 详细值可查看SMSUtility 类库中的 SysEnums.cs 下的 AutoNotice枚举 
*@param Creator 发件人（IDCard）
*@param Receiver 收件人（IDCard）
*@param ReceiverEmail 邮箱地址（可以为空）
*@param Href 内容信息中的超链接（可以为空）
*@param CreatorName 发件人名称
*@param ReceiverName 收件人名称
@param Timing 是否是定时发送 0 立即发送， 1 定时发送
*/
function addSysNotice(Title, Contents, Type, Creator, Receiver, ReceiverEmail, Href, CreatorName, ReceiverName, Timing) {
    $.ajax({
        url: "/Common.ashx",
        type: "post",
        async: false,
        dataType: "json",
        data: { PageName: "PortalManage/MessageHandler.ashx", Func: "EditMessage", Title: Title, Contents: Contents, Type: Type, Creator: Creator, Receiver: Receiver, ReceiverEmail: ReceiverEmail, Href: Href, CreatorName: CreatorName, ReceiverName: ReceiverName, Timing: Timing },
        success: function (json) {
            if (json.result.errNum.toString() == "0") {
                //layer.msg("操作成功！");
            }
            else { //layer.msg('操作失败！'); 
            }
        },
        error: function (errMsg) {
            //layer.msg('操作失败！');
        }
    });
}
/*
*@param Title 邮箱或者消息的标题
*@param Contents 邮箱内容或者消息的内容
*@param Type 详细值可查看SMSUtility 类库中的 SysEnums.cs 下的 AutoNotice枚举 
*@param Creator 发件人（IDCard）
*@param CreatorName 发件人名称
*@param ReceiversArray 收件人json数组（[{ Receiver:"收件人", ReceiverEmail: "邮箱地址（可以为空）", ReceiverName: "收件人名称"}]）
*@param Href 内容信息中的超链接（默认为空）
*@param Timing 是否是定时发送 0 立即发送， 1 定时发送（默认为 0）
*@param TimingDate 定时发送的时间（默认为空）
*@param FilePath 邮箱文件（默认为空）
*/
function Notice_MoreSendMessage(Title, Contents, Type, Creator, CreatorName, ReceiversArray, Href, Timing, TimingDate, FilePath, isSendEmail) {
    Href = arguments[6] || "";
    Timing = arguments[7] || 0;
    TimingDate = arguments[8] || "";
    FilePath = arguments[9] || "";
    isSendEmail = arguments[10] || "false";
    $.ajax({
        url: "/Common.ashx",
        type: "post",
        async: false,
        dataType: "json",
        data: {
            PageName: "PortalManage/MessageHandler.ashx", Func: "MoreSendMessage",
            Title: Title, Contents: Contents, Type: Type,
            Creator: Creator, CreatorName: CreatorName,
            Receivers: JSON.stringify(ReceiversArray),
            Timing: Timing, CreateTime: TimingDate,
            Href: Href, FilePath: FilePath,
            isSendEmail: isSendEmail
        },
        success: function (json) {
            if (json.result.errNum.toString() == "0") {
                //layer.msg("发送成功！");
            }
            else { }
        },
        error: function (errMsg) { }
    });
}
/*
function addMonnitor(RequestType, RequestSourceID, RequestSourceName, RequestUserType, Creator, IDCard) {
    $.post("/Common.ashx", { PageName: "PortalManage/MonitorRecordHandler.ashx", Func: "AddRecord", RequestType: RequestType, RequestSourceID: RequestSourceID, RequestSourceName: RequestSourceName, RequestUrl: window.location.href, RequestUserType: RequestUserType, Creator: Creator, IDCard: IDCard }, function (data) {

    });
}
*/
function enumType(type) {
    var msg = "";
    switch (type) {
        case "通知公告":
            msg = 0;
            break;
        case "学校新闻":
            msg = 1;
            break;
        case "媒体报道":
            msg = 2;
            break;
        case "招聘信息":
            msg = 3;
            break;
    }
    return msg;
}

function ShowNewsType(str) {
    var msg = "";
    switch (str) {
        case "0":
            msg = "通知公告";
            break;
        case "1":
            msg = "学校新闻";
            break;
        case "2":
            msg = "媒体报道";
            break;
        case "3":
            msg = "招聘信息";
            break;
    }
    return msg;
}

function enumSystemType(type) {
    var msg = "";
    switch (type) {
        case "学校简介": msg = 11; break;
        case "网站简介": msg = 66; break;
        case "校园风貌": msg = 12; break;
        case "师资力量": msg = 49; break;
    }
    return msg;
}

//js 获取地址栏参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]);
    return null;
}

function uniqueArry(elem) {
    var res = [], hash = {};
    for (var i = 0, elem; (elem = this[i]) != null; i++) {
        if (!hash[elem]) {
            res.push(elem);
            hash[elem] = true;
        }
    }
    return res;
}
function logOut() {
    window.location.href = "/AppManage/Index.aspx?action=loginOut";
}

/**
 * Created by 刘丽华 on 2016/11/29.
 */

var second = 0;
window.setInterval(function () {
    second++;
}, 1000);
/*
window.onbeforeunload = function () {
    var ToUrl = location.href;
    var SkinLong = second;
    var FromUrl = getReferrer();
    var LoginCookie = $.cookie('LoginCookie_Author');
    var UniqueNo = '';
    var LoginName = '';
    if (LoginCookie != undefined && LoginCookie != 'null' && LoginCookie != "" && LoginCookie != null) {
        LoginCookie = decodeURIComponent(LoginCookie);
        UniqueNo = window.JSON.parse(LoginCookie)["UniqueNo"];
        LoginName = window.JSON.parse(LoginCookie)["LoginName"];
    }
    $.ajax({
        url: "/SystemSettings/UserInfo.ashx",
        type: "post",
        async: false,
        dataType: "json",
        data: {
            Func: "UserSkim",
            UniqueNo: UniqueNo,
            ToUrl: ToUrl,
            SkinLong: SkinLong,
            FromUrl: FromUrl,
            LoginName: LoginName,
            SkinLong: SkinLong,
            WebSite: "Sinptech_Study_System"
        },
        success: function (json) {

        },
        error: function (errMsg) {
        }
    });
};
function getReferrer() {
    var referrer = '';
    try {
        referrer = window.top.document.referrer;
    } catch (e) {
        if (window.parent) {
            try {
                referrer = window.parent.document.referrer;
            } catch (e2) {
                referrer = '';
            }
        }
    }
    if (referrer === '') {
        referrer = document.referrer;
    }
    return referrer;
}
*/
$(function () {
    if (myBrowser() == "IE55" || myBrowser() == "IE6" || myBrowser() == "IE7" || myBrowser() == "IE8") {
        window.location.href = "/error.html";
    }
})

function myBrowser() {
    var userAgent = navigator.userAgent; //取得浏览器的userAgent字符串
    var isOpera = userAgent.indexOf("Opera") > -1; //判断是否Opera浏览器
    var isIE = userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera; //判断是否IE浏览器
    var isFF = userAgent.indexOf("Firefox") > -1; //判断是否Firefox浏览器
    var isSafari = userAgent.indexOf("Safari") > -1; //判断是否Safari浏览器
    if (isIE) {
        var IE5 = IE55 = IE6 = IE7 = IE8 = false;
        var reIE = new RegExp("MSIE (\\d+\\.\\d+);");
        reIE.test(userAgent);
        var fIEVersion = parseFloat(RegExp["$1"]);
        IE55 = fIEVersion == 5.5;
        IE6 = fIEVersion == 6.0;
        IE7 = fIEVersion == 7.0;
        IE8 = fIEVersion == 8.0;
        if (IE55) {
            return "IE55";
        }
        if (IE6) {
            return "IE6";
        }
        if (IE7) {
            return "IE7";
        }
        if (IE8) {
            return "IE8";
        }
    }//isIE end
    if (isFF) {
        return "FF";
    }
    if (isOpera) {
        return "Opera";
    }
}//myBrowser() end

function SetPageButton(curUniqueNo) {
    //element = arguments[2] || "span";
    var menucode = '';
    //var UrlDate = new GetUrlDate();
    //if (UrlDate && UrlDate.btncls) { menucode = UrlDate.btncls }
    var host = 'http://' + window.location.host;
    var href = window.location.href;
    var cururl = href.split(host)[1];
    if (cururl.indexOf("?") != -1) {
        var queindex = cururl.lastIndexOf("?");
        cururl = cururl.substring(0, queindex);
    }
    if (cururl.indexOf("Login.aspx") != -1 || cururl.indexOf("CommonPage/header.aspx") != -1) {
        return;
    }
    $.ajax({
        url: "/Hander/ModelHander.ashx",
        type: "post",
        async: false,
        dataType: "json",
        data: {
            Func: "GetSubButtonByUrl",
            Url: cururl,
            UniqueNo: curUniqueNo,
            MenuCode: menucode,
        },
        success: function (json) {
            if (json.result.errNum.toString() == "0") {
                var curpage = json.result.retData[0];
                var btnfield = curpage.ButtonField;
                if (btnfield != undefined && btnfield.length) {
                    var btnArray = btnfield.split(',');
                    for (var btn in btnArray) {
                        var curbtn = btnArray[btn].split('|');
                        var $spanObj = $(element + "[btncls='" + curbtn[2] + "']");
                        if ($spanObj) {
                            $spanObj.show();
                        }
                    }
                }
            } else if (json.result.errMsg != "1") {
                window.location.href = "/NoPower.html";
            }
        },
        error: function (errMsg) {
            layer.msg(errMsg);
        }
    });
}
