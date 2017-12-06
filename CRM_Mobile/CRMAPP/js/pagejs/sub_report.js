/*工作日报*/

//var url = pageurl + "/Report/workreport_handle.ashx";
//var pagesize_day = 8;
//var pageindex_day = 1;
//var RowCount_day = 0;
//var pagesize_week = 8;
//var pageindex_week = 1;
//var RowCount_week = 0;
//var pagesize_month = 8;
//var pageindex_month = 1;
//var RowCount_month = 0;
var report_reader = 0;
var fo_users = [];
var orgData = JSON.parse(localStorage.getItem("orgData"));
var myrole_name = "";
//筛选用的用户ID
var report_userid;
//下拉刷新
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
        
        if (page_parmeter.report_type == 1) {
          
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 1;
            initdata(page_parmeter);
            mui('#pullrefresh').pullRefresh().endPulldownToRefresh(); //refresh completed
        } else if (page_parmeter.report_type == 2) {
          
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 2;
            initdata(page_parmeter);
            mui('#pullrefresh').pullRefresh().endPulldownToRefresh(); //refresh completed
        } else if (page_parmeter.report_type == 3) {
           
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 3;
            initdata(page_parmeter);
            mui('#pullrefresh').pullRefresh().endPulldownToRefresh(); //refresh completed
        }
    }, 1500);
}

/**
 * 上拉加载具体业务实现
 */
function pullupRefresh() {
    setTimeout(function () {
      
        if (page_parmeter.report_type == 1) {
            mui('#pullrefresh').pullRefresh().endPullupToRefresh((page_parmeter.RowCount / page_parmeter.pagesize < page_parmeter.pageindex)); //参数为true代表没有更多数据了。
            page_parmeter.pageindex++;
            page_parmeter.report_type = 1;
            initdata(page_parmeter);
        } else if (page_parmeter.report_type == 2) {
            mui('#pullrefresh').pullRefresh().endPullupToRefresh((page_parmeter.RowCount / page_parmeter.pagesize < page_parmeter.pageindex)); //参数为true代表没有更多数据了。
            page_parmeter.pageindex++;
            page_parmeter.report_type = 2;
            initdata(page_parmeter);
        } else if (page_parmeter.report_type == 3) {
            mui('#pullrefresh').pullRefresh().endPullupToRefresh((page_parmeter.RowCount / page_parmeter.pagesize < page_parmeter.pageindex)); //参数为true代表没有更多数据了。
            page_parmeter.pageindex++;
            page_parmeter.report_type = 3;
            initdata(page_parmeter);
        }

    }, 1500);
}
if (mui.os.plus) {
    mui.plusReady(function () {
        setTimeout(function () {
            mui('#pullrefresh').pullRefresh().pullupLoading();
        }, 1000);

    });
}
$(function () {
    getii();
       
    initdata(page_parmeter);
    
    $('#report_lists').on('tap', 'li', function () {
        var id = $(this).children('input[type="hidden"]').val();
        // var type = $(this).children('input[name="type"]').val();
        mui.openWindow({
            id: 'CheckReport.html',
            url: 'CheckReport.html?id=' + id + "&dd_nav_bgcolor=FFF7A64F&number=" + Math.random(),
            show: {
                autoShow: true,
                aniShow: 'slide-in-right',
                duration: '200ms'
            },
            waiting: {
                autoShow: true
            }
        })
    })
})

var static_url = pageurl + "/Statistical/statistic_handle.ashx";

//获取下属
function getii() {

    var data = {
        Func: "GetMemmber",
        guid: userid
    }
    getajax_async(static_url, data, function (json) {
        if (json.result.errMsg == "success") {

            //alert(JSON.stringify(json))
            //我的下属
            var retdata = json.result.retData
            if (retdata != null) {
                for (var i = 0; i < retdata.length; i++) {
                    if (retdata[i].Name != username) {
                        $("#select_people").append("<p>" + retdata[i].Name + "<input  type='hidden' value='" + retdata[i].UniqueNo + "' /></p>");
                        fo_users.push("'" + retdata[i].UniqueNo + "'");
                        report_reader = fo_users.join(',');
                    }
                }
            }
        }
    });
    //if (myrole_name != "") {
    //员工筛选
    $('#select_people p').on('tap', function () {
        $(this).addClass('active').siblings().removeClass('active');
        var text = $(this).text();
        $('.tools a:eq(1)').find('b').text(text);
        $('.mark_wrap').css('display', 'none');
        $('#select_people').css('display', 'none');
        $('#report').css('display', 'none');
        report_reader = "'" + $(this).children("input[type='hidden']").val() + "'";
        //获取选择的员工ID
        report_userid = $(this).find("input[type='hidden']").val();

        if (text == '不限') {
            report_userid = '';
        }

        if (s_index == "0") {
           
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 1;
            initdata(page_parmeter);
        }
        else if (s_index == "1") {
           
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 1;
            initdata(page_parmeter);
        }
        else if (s_index == "2") {
            
            page_parmeter.pageindex = 1
            page_parmeter.report_type = 1;
            initdata(page_parmeter);
        }
    })   
}

//初始化数据
function initData_Others() {
    var data = {
        Func: "get_workreport_list",
        pagesize: page_parmeter.pagesize,
        pageindex: page_parmeter.pageindex,
        ispage: true,
        report_userid: report_userid,
        report_type: page_parmeter.report_type,
        type: 2,
        report_reader: report_reader,
        guid: userid,
        departmentID: page_parmeter.departmentID,
        memmberID: page_parmeter.memmberID
    }

    getajax_async(url, data, function (json) {
        if (json != null && json.result != null && json.result.errMsg != null & json.result.errMsg == "success") {
            var retData = json.result.retData.PagedData;
            page_parmeter.RowCount = json.result.retData.RowCount;
            $(".count").html(page_parmeter.RowCount + "条");

            if (page_parmeter.pageindex == 1) {
                $('#report_lists').empty();
            }
            disablePull(retData);
            
            switch (page_parmeter.report_type) {
                case 1:
                    if (page_parmeter.RowCount > 0) {
                        $('#report_lists').parent().css({ 'height': 'auto' });
                        $('#report_lists').append(ejs.render($('#day_list_template').html(), { retData: retData }));
                    } else {
                        $('#report_lists').parent().css({ 'height': '100%' });
                        $('#report_lists').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
                    }
                    break;
                case 2:
                    if (page_parmeter.RowCount > 0) {
                        $('#report_lists').parent().css({ 'height': 'auto' });
                        $('#report_lists').append(ejs.render($('#week_list_template').html(), { retData: retData }));
                    } else {
                        $('#report_lists').parent().css({ 'height': '100%' });
                        $('#report_lists').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
                    }
                    break;
                case 3:
                    if (page_parmeter.RowCount > 0) {
                        $('#report_lists').parent().css({ 'height': 'auto' });
                        $('#report_lists').append(ejs.render($('#month_list_template').html(), { retData: retData }));
                    } else {
                        $('#report_lists').parent().css({ 'height': '100%' });
                        $('#report_lists').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
                    }
                    break;
                default:
            }



        } else {
            $('#report_lists').parent().css({ 'height': '100%' });
            $('#report_lists').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
        }
    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}

function disablePull(retData) {
    //当列表数据过少是禁用下拉刷新，上拉加载更多
    if (retData != null && retData != '' && retData.length <= 0) {
        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
    }
}

