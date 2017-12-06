/*--String.prototype--*/
~function (pro) {
    //->queryURLParameter:get url parameter or hash
    function queryURLParameter() {
        var reg = /([^?=&#]+)=([^?=&#]+)/g,
            obj = {};
        this.replace(reg, function () {
            obj[arguments[1]] = arguments[2];
        });

        //->HASH
        reg = /#([^?=&#]+)/g;
        this.replace(reg, function () {
            obj['HASH'] = arguments[1];
        });
        return obj;
    }

    //->formatTime:format time by template
    function formatTime(template) {
        template = template || '{0}年{1}月{2}日{3}时{4}分{5}秒';
        var reg = /\d+/g,
            ary = this.match(reg);
        template = template.replace(/\{(\d+)\}/g, function () {
            var index = arguments[1],
                res = ary[index] || '00';
            res.length < 2 ? res = '0' + res : null;
            return res;
        });
        return template;
    }

    pro.formatTime = formatTime;
    pro.queryURLParameter = queryURLParameter;
}(String.prototype);
~function () {
    function fn() {
        var desW = 640;
        var winW = document.documentElement.clientWidth;
        document.documentElement.style.fontSize = winW / desW * 100 + 'px';
    }
    fn();
    $(window).resize(function () {
        fn();
    })
}();
var pageurl = 'http://192.168.50.129:18099/Service';
var pageurl1 = 'http://192.168.50.129:18099';
//var userid = 'D59CD2E5-2BA8-4803-A09F-C342B5D658EC';
//var username = '刘云诚';
var userid = localStorage.getItem("userid");
var username = localStorage.getItem("username");

//var rs = [1438, 1491, 1563, 1566, 1613];
//var roots = ['D59CD2E5-2BA8-4803-A09F-C342B5D658EC', 'E197ABC0-E071-4DAA-9299-9FB316720B09', 'F4C7BE25-6482-4028-90E3-3BC5D8268338', '2EB80A19-E6D4-489A-82E0-E7FED8908109'];
//分别为刘云诚、彭会平、姜玲娜、黄泳绮

var IsIOS;

var needfresh;

function getajax(url, data, fn, fn1) {
    //ajax请求
    $.ajax({
        url: url,
        type: "post",
        async: false,
        dataType: "json",
        data: data,
        success: fn,
        error: fn1
    });
}
function getajax_async(url, data, fn, fn1) {
    //ajax请求
    $.ajax({
        url: url,
        type: "post",
        async: false,
        dataType: "json",
        data: data,
        success: fn,
        error: fn1
    });
}
function getparam(name) {
    //获取url请求参数
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function openwindow(id, url, aniShow) {
    //跳转页面
    mui.openWindow({
        id: id,
        url: url,
        show: {
            autoShow: true,
            aniShow: aniShow,
            duration: '200ms'
        },
        waiting: {
            autoShow: true,
            title: '正在加载...', //等待对话框上显示的提示内容 
            options: {
                width: 100, //等待框背景区域宽度，默认根据内容自动计算合适宽度 
                height: 100, //等待框背景区域高度，默认根据内容自动计算合适高度 

            }
        }
    });
}

function getForm(elements) {
    //获取表单的值
    var o = {};
    jQuery.each(elements, function (i, fields) {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            else {
                o[this.name].push(this.value || '');
            }

        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

function validateForm(elements) {
    var flag = 0;
    jQuery.each(elements, function (i, fields) {
        if ($(this).attr("isrequired") == "true") {
            if ($(this).val() == "") {
                flag++;
                if (flag == 1) {
                    dd_toast($(this).attr("fl") + '不允许为空！', 'error', 0);
                }
            }
        }
        if ($(this).attr("regtype") == "email") {
            var reg = /^\w+@[a-zA-Z0-9]+(\.[a-z]{2,3}){1,2}$/;
            if ($(this).val() != "" && reg.test($(this).val()) == false) {
                dd_toast('邮箱格式错误！', 'error', 0);
                flag++;
            }
        }
        if ($(this).attr("regtype") == "telphone") {
            var reg = /^((0\d{2,3}-\d{7,8})|(1[35784]\d{9}))$/;
            if ($(this).val() != "" && reg.test($(this).val()) == false) {
                dd_toast('电话格式错误！', 'error', 0);
                flag++;
            }
        }
    });
    return flag;
}


function setcookie(name, value) {

    //设置名称为name,值为value的Cookie
    var expdate = new Date();   //初始化时间
    expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间
    document.cookie = name + "=" + value + ";expires=" + expdate.toGMTString() + ";path=/";

    //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
}

function getcookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function getitem(name) {
    localStorage.getItem(name);
}
mui.init({
    swipeBack: false,//启用右滑关闭功能
    keyEventBind: {
        backbutton: true  //启动back按键监听
    }
});
window.onload = function () {
    //从服务器获取数据
    mui.plusReady(function () {
        //关闭等待框
        plus.nativeUI.closeWaiting();
        //显示当前页面
        mui.currentWebview.show();
    });
}


$(function () {
    var reg1 = /AppleWebKit.*Mobile/i, reg2 = /MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/;
    if (reg1.test(navigator.userAgent) || reg2.test(navigator.userAgent)) {
        $('.Header').css({ 'display': 'none' });
    } else {
        $('.Header').css({ 'display': 'block' });
        $('.mui-bar-nav ~ .mui-content').css({ 'paddingTop': '0.88rem' });
        //工作计划
        $('.workplan_pull').css({ 'top': '0.88rem' });
        //map
        $('.map').css({ 'top': '0.88rem', 'paddingTop': '0' });
        //跟进记录
        $('.followup').css({ 'top': '1.63rem' });
        $('.followup_pull').css({ 'top': '2.43rem' });
        //客户信息
        $('.customer_pull').css({ 'top': '0.88rem' });
        //联系人
        $('.linkman_pull').css({ 'top': '0.88rem' });
        //遗忘提醒
        $('.remind_pull').css({ 'top': '0.88rem' });
        //分享圈
        $('.share_pull').css({ 'top': '0.88rem' });
        //
        $('.subreport_tools').css({ 'top': '1.63rem' });
        $('.subreport_toolss').css({ 'top': '2.43rem' });
    }
})

//type是平台
function dd_toast(text,icon, type) {
    dd.device.notification.toast({
        global: true,
        icon:icon,
        text: text,
        duration: 2,
        delay:0
    });
}

function getgetLevel(flg) {
    var returnval = "";
    switch (flg)
    {
        case 1:
            returnval = 'VIP客户';
            break;
        case 2:
            returnval = '开拓客户';
            break;
    }
}

function getType(flg) {
    var returnval = "";
    switch (flg) {
        case 1:
            returnval = '学校';
            break;
        case 2:
            returnval = '集成商';
            break;
        case 3:
            returnval = '经销商';
            break;
        case 4:
            returnval = '供应商';
            break;
    }
}


function nomessage(id) {
    if (page_parmeter.pageindex == 1 && page_parmeter.RowCount == 0) {
        $('#' + id + '').parent().css({ 'height': '100%' });
        $('#' + id + '').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
    }
    
}

function getcolor(value) {
    var color = ["#F75A63", "#F79558", "#73C2FE", "#A77ADF"];
    var returns = '';
    if (value == 0) {
        returns = color[0];
    }
    else if (value == 1) {
        returns = color[1];
    }
    else if (value == 2) {
        returns = color[2];
    }
    else if (value == 3) {
        returns = color[3];
    }
    else {
        returns = color[0];
    }
    return returns;
}
function errormessage(id) {
    if (pageindex == 1 && RowCount == 0) {
        $('#' + id + '').parent().css({ 'height': '100%' });
        $('#' + id + '').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/error.png) no-repeat center;background-size: 3rem auto;"></div>');
    }
}
//获取url所传的参数
function getQueryString(name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return unescape(r[2]);
    }
    return null;
}



