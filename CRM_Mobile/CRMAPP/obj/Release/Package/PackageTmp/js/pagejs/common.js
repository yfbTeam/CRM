var page_parmeter = new Object();
page_parmeter.pagesize = 8;
page_parmeter.pageindex = 1;
page_parmeter.RowCount = 0;
page_parmeter.type = 'pri';
page_parmeter.departmentID = '';
page_parmeter.memmberID = '';
page_parmeter.stardate = '';
page_parmeter.enddate = '';
page_parmeter.report_type = 1;
page_parmeter.self_report = '0';
var falg = true, falg1 = true, falg2 = true, falg3 = true;


//var scroll;


//多负责人
//page_parmeter.userArray = '';
//page_parmeter.nameArray = '';
mui.init({
    pullRefresh: {
        container: '#pullrefresh',
        down: {
            callback: pulldownRefresh
        },
        up: {
            contentrefresh: '正在加载...',
            callback: pullupRefresh
        }
    }
});
/**
 * 下拉刷新具体业务实现
 */
function pulldownRefresh() {

    setTimeout(function () {
        page_parmeter.pageindex = 1;
        //debugger;
        initdata(page_parmeter);
        mui('#pullrefresh').pullRefresh().endPulldownToRefresh(); //refresh completed

        //scroll = mui('#pullrefresh').scroll();
        //alert(scroll.y);

    }, 1000);
}

//$(function () {
//    var ob = $('#pullrefresh');

//    if (ob != null && ob != undefined) {
//        $('#pullrefresh').on('touchend', function (event) {
//            debugger;
//            // 如果这个元素的位置内只有一个手指的话
//            if (event.targetTouches.length == 1) {
//                var touch = event.targetTouches[0];
//                // 把元素放在手指所在的位置
//                //touchEnd.value = touch.pageX + ';' + touch.pageY;
//                alert(touch.pageY);
//            }
//        }, false);

//    }


//})




//function pullToPosition() {
//    mui('#pullrefresh').pullRefresh().scrollTo(0, -500, 0);
//    mui('#pullrefresh').pullRefresh().off
//}

/**
 * 上拉加载具体业务实现
 */
function pullupRefresh() {
    setTimeout(function () {
        mui('#pullrefresh').pullRefresh().endPullupToRefresh((page_parmeter.RowCount / page_parmeter.pagesize < page_parmeter.pageindex)); //参数为true代表没有更多数据了。
        page_parmeter.pageindex++;

        initdata(page_parmeter);

        //scroll = mui('#pullrefresh').scroll();

        //alert(scroll.y);
        //debugger;
    }, 0);
}
if (mui.os.plus) {
    mui.plusReady(function () {
        setTimeout(function () {
            mui('#pullrefresh').pullRefresh().pullupLoading();
        }, 0);

    });
}
//当列表数据过少是禁用下拉刷新，上拉加载更多



//恢复各项
function enable_top_pull() {
    mui('#pullrefresh').pullRefresh().refresh(true);//恢复滚动
    mui('#pullrefresh').pullRefresh().scrollTo(0, 0); //滚动置顶
}



//部门筛选 人员筛选————————————————————————————————————————————————————————————————————————————————————————


/*客户信息*/
var custmers_url = pageurl + "/Statistical/statistic_handle.ashx";
//部门列表
var department_list;
//成员列表
var user_list;

function limit_setting_page() {
    var role = localStorage.getItem("role");
    //debugger;
    switch (role) {
        case 'Super_Admin':
            $('#tools').css('display', 'block');
            $('#tools1').css({ 'display': 'block', 'top': '1.65rem' });
            $('#department_panel').css('display', 'block');
            $('.workplan_pull').css('top', '0.8rem');

            get_department();
            department_tap_binding();
            get_memmber();
            //重新绑定事件
            sale_tap_binding();

            break;
        case 'Common_Admin':
            $('#tools').css('display', 'block');
            $('#tools1').css({ 'display': 'block', 'top': '1.65rem' });

            $('.workplan_pull').css('top', '0.8rem');
            $('#department_panel').css('display', 'block');

            $('#tools a:eq(0)').off('tap');
            $('.subreport_tools a:eq(1)').off('tap');

            get_self_depatment();
            //获取自己所属的部门【总监使用】
            function get_self_depatment() {
                var data = {
                    Func: "Get_Self_DepartMent",
                    guid: userid,

                }
                getajax(custmers_url, data, function (json) {
                    //debugger;
                    if (json.result.errMsg == "success") {
                        var retData = json.result.retData;
                        var department_name = retData.Name;
                        if (department_name != null && department_name != undefined && department_name != '') {
                            $('#department_panel>span').html(department_name);
                        } else {
                            $('#department_panel>span').html('部门');
                        }

                    }
                    //else if (json.result.errMsg == "failed") {
                    //    dd_toast(json.result.retData, 'error', 0);
                    //}
                })
            }
            get_memmber();
            //重新绑定事件
            sale_tap_binding();

            break;

        case 'Common_Memmber':
            $('#tools').hide();
            $('.linkman_pull').css('top', '1.65rem');
            break;
        case '':
            $('#tools').hide();
            break;

        default:
    }
}


//获取部门信息【超级管理员使用】
function get_department() {
    if (department_list == null || department_list == '' && department_list == undefined) {
        var data = {
            Func: "GetDepartMent",
            guid: userid,
        }
        getajax(custmers_url, data, function (json) {

            if (json.result.errMsg == "success") {
                department_list = json.result.retData;
                //debugger;               
                $('#depart_dele').append(ejs.render($('#department_template').html(), { retData: department_list }));

            }
            else if (json.result.errMsg == "failed") {
                dd_toast(json.result.retData, 'error', 0);
            }
        })
    }

}
//获取成员信息
function get_memmber() {
    if (user_list == null || user_list == '' && user_list == undefined) {
        var data = {
            Func: "GetMemmber",
            guid: userid,
        }
        getajax(custmers_url, data, function (json) {

            if (json.result.errMsg == "success") {
                user_list = json.result.retData;

                $('#saler_sele').append(ejs.render($('#sales_template').html(), { retData: user_list }));

            }
            else if (json.result.errMsg == "failed") {
                dd_toast(json.result.retData, 'error', 0);
            }
        })
    }
}

//部门事件绑定
function department_tap_binding() {
    $('#depart_dele>p').on('tap', function () {
        $(this).addClass('active').siblings().removeClass('active');

        page_parmeter.departmentID = $(this).children('input[type=hidden]').val();
        page_parmeter.memmberID = '';

        for (var i = 0; i < department_list.length; i++) {
            if (department_list[i].ID == page_parmeter.departmentID) {
                //debugger;
                //debugger;
                $('#saler_sele').html('');
                $('#saler_sele').append('<p>不限</p>');
                $('#saler_sele').append(ejs.render($('#sales_template').html(), { retData: department_list[i].UserInfo_List }));
                break;
            }
        }

        page_parmeter.pageindex = 1;

        page_parmeter.self_report = '1';

        //获取列表
        initdata(page_parmeter);
        enable_top_pull();

        var text = $(this).text();
        $('#department_panel').find('span').html(text + '<i class="iconfont">&#xe61e;</i>');
        $('.mark_wrap').css('display', 'none');
        $('#depart_dele').css({ 'display': 'none' });
        $('#report_type').css('display', 'none');
        falg = true;
        $('#saler_sele').hide();
        if (text == '不限') {
            $('#saler_sele').append(ejs.render($('#sales_template').html(), { retData: user_list }));
        }
        $('#sales_panel').find('span').html('不限' + '<i class="iconfont">&#xe61e;</i>');
        //重新绑定事件
        sale_tap_binding();
    });
}

//销售人员点击事件
function sale_tap_binding() {
    $('#saler_sele>p').on('tap', function () {
        $('#sales_panel').find('span').html('');
        $(this).addClass('active').siblings().removeClass('active');
        page_parmeter.memmberID = $(this).children('input[type=hidden]').val();
        page_parmeter.pageindex = 1;
        page_parmeter.self_report = '1';
        //获取当前部门的计划列表
        initdata(page_parmeter);
        enable_top_pull();
        var text = $(this).text();
        localStorage.removeItem('memmberID');
        localStorage.setItem('memmberID', page_parmeter.memmberID);
        $('#sales_panel').find('span').html(text + '<i class="iconfont">&#xe61e;</i>');
        falg1 = true;
        $('.mark_wrap').css('display', 'none');
        $('#saler_sele').css({ 'display': 'none' });
        $('#report_type').css('display', 'none');
        $('#depart_dele').hide();       
    });
}

//点击页面蒙版关闭
$(document).on('tap', function (e) {
    var target = e.target;
    var tagname = target.tagName;
    if (tagname == 'DIV' && target.className == 'mark_wrap') {
        $('.mark_wrap').css('display', 'none');
        //跟进记录切换
        $('#depart_dele').css('display', 'none');
        //时间切换
        $('#saler_sele').css('display', 'none');
        //
        $('#time_sele').hide();
        $('#person_sele').hide();
    }
})
//!(function ($) {
//    $.fn.selectslide = function (options) {
//        var defaults = {
//            event: event,
//        };

//        var options = $.extend(defaults.event, options || {});
//        return this.each(function () {
//            var obj = $(this);
//            var index = obj.index();
//            defaults[falg + index] = true;
//            obj.on(defaults.event, function () {

//                if (defaults[falg + index]) {
//                    $('.mark_wrap').show();
//                    $('#layer' + index).show();

//                    defaults[falg + index] = false;
//                } else {
//                    $('.mark_wrap').hide();

//                    defaults[falg + index] = true;
//                }
//            })
//        })
//    }
//})(Zepto);


//设置场景
function save_history() {
    //设置场景
    localStorage.setItem("save_department_select", page_parmeter.departmentID);
    localStorage.setItem("save_sale_select", page_parmeter.memmberID);
}

//场景还原
function return_history() {
    //获取当前场景
    var save_department_select = localStorage.getItem("save_department_select");
    var save_sale_select = localStorage.getItem("save_sale_select");
    //debugger;
    if (save_department_select != null && save_department_select != undefined && save_department_select != '' && department_list != undefined && department_list.length > 0) {
        //场景还原
        for (var i = 0; i < department_list.length; i++) {
            if (department_list[i].ID == save_department_select) {
                //debugger;
                //debugger;
                page_parmeter.departmentID = save_department_select;

                $('#department_panel').find('span').html(department_list[i].Name + '<i class="iconfont">&#xe61e;</i>');

                //人员信息的重新绑定
                $('#saler_sele').html('');
                $('#saler_sele').append('<p>不限</p>');
                $('#saler_sele').append(ejs.render($('#sales_template').html(), { retData: department_list[i].UserInfo_List }));

                //重新绑定事件
                sale_tap_binding();

                break;
            }
        }
    }
    
    if (save_sale_select != null && save_sale_select != undefined && save_sale_select != '' && user_list != undefined && user_list.length > 0) {
        //场景还原
        for (var i = 0; i < user_list.length; i++) {
            if (user_list[i].UniqueNo == save_sale_select) {
                //debugger;
                //debugger;
                page_parmeter.memmberID = save_sale_select;

                $('#sales_panel').find('span').html(user_list[i].Name + '<i class="iconfont">&#xe61e;</i>');

                break;
            }
        }
    }   
}

//function remove_history_data()
//{
//    localStorage.removeItem("save_department_select");
//    localStorage.removeItem("save_sale_select");
//}

