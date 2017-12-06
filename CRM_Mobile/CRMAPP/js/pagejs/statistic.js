/*统计*/
var url = pageurl + "/Statistical/statistic_handle.ashx";
var contentWebview = null;
var type = '';
$(function () {
    //page_parmeter.pageindex = 1;
    //initdata(page_parmeter);
  
    $("#s_username").on('change', function () {
        page_parmeter.pageindex = 1;
        initdata(page_parmeter);
        //if (contentWebview == null) {
        //    contentWebview = plus.webview.currentWebview().children()[0];
        //}
        ////内容区滚动到顶部
        //contentWebview.evalJS("mui('#pullrefresh').pullRefresh().scrollTo(0,0,100)");
        //下拉可用并置顶
        enable_top_pull();
    })
    $("#s_startdate").on('keyup', function () {
        initdata(page_parmeter);
    })
    $("#s_enddate").on('keyup', function () {
        page_parmeter.pageindex = 1;
        initdata(page_parmeter);
    })


})
//查看销售简报详情
function detail() {
    mui('#list li').each(function () { //循环所有toggle
        var userid = $(this).children("input[name='userid']").val()
        var username = $(this).children("input[name='username']").val();
        var startdate = $("#s_startdate").val();
        var enddate = $("#s_enddate").val();
        this.addEventListener('tap', function (event) {
            showPreloader();
            //debugger;

             //设置场景
            save_history();         

            localStorage.setItem("sale_startdate", startdate);
            localStorage.setItem("sale_enddate", enddate);
            openwindow('SalesKit', '/SalesKit/SalesKitDeatil.html?dd_nav_bgcolor=FF6CB1FF&s1=2&userid=' + userid + '&username=' + escape(username) + '&startdate=' + startdate + '&enddate=' + enddate, 'slide-in-right');

        });
    });
}

function initdata(page_parmeter) {
    //alert(pagesize);
    //alert(pageindex);
    //alert($("#s_startdate").val());

    //alert($("#s_enddate").val());

    //alert(userid);

    if ($('#list_template').html() != null) {
        var data = {
            Func: "get_statistic_list",
            PageSize:page_parmeter. pagesize,
            PageIndex: page_parmeter.pageindex,
            ispage: true,
            stardate: page_parmeter.stardate,
            enddate: page_parmeter.enddate,
            s_username: $("#s_username").val(),
            guid: userid,
            type: page_parmeter.type,
            departmentID: page_parmeter.departmentID,
            memmberID: page_parmeter.memmberID
        }
        getajax_async(url, data, function (json) {
            //alert(JSON.stringify(json))
            if (json.result.errMsg == "success") {

                var retData = json.result.retData.PagedData;
                page_parmeter.RowCount = json.result.retData.RowCount;
                if (page_parmeter.pageindex == 1) {
                    $('#list').empty();
                }
                //当列表数据过少是禁用下拉刷新，上拉加载更多
                if (retData != null && retData != '' && retData.length <= 0) {
                    mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                }
                $('#list').parent().css({ 'height': 'auto' });
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                detail();
                nomessage("list");
                //alert(JSON.stringify(json))


                $(".count").html(page_parmeter.RowCount + "人");
            }
            else {
                //$('#list').empty();
                nomessage("list");
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }
}
