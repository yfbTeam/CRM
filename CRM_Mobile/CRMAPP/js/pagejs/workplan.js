/*工作计划*/
var url = pageurl + "/WorkPlan/workplan_handle.ashx";

//指定客户的联系人ID【如果有url传值方式传值，接收，并设置为筛选条件】
var wp_cust_id = getQueryString("cust_id");

//指定客户的联系人ID【如果有url传值方式传值，接收，并设置为筛选条件】
var link_id = getQueryString("link_id");



//初始化数据
function initdata(page_parmeter) {
    var data = {
        Func: "get_workplan_list",
        wp_userid: userid,
        pagesize: page_parmeter.pagesize,
        pageindex: page_parmeter.pageindex,
        ispage: true,
        guid: userid,
        wp_cust_id: wp_cust_id,
        link_id: link_id,
        departmentID: page_parmeter.departmentID,
        memmberID: page_parmeter.memmberID
    }
    getajax_async(url, data, function (json) {
        //debugger;
        if (json.result.errMsg == "success") {
            var retData = json.result.retData.PagedData;    
            page_parmeter.RowCount = json.result.retData.RowCount;
            $(".count").html(page_parmeter.RowCount + "条");
            if (page_parmeter.pageindex == 1) {
                $('#list').empty();
            }          
            //当列表数据过少是禁用下拉刷新，上拉加载更多
            if (retData != null && retData != '' && retData.length <= 0) {
                mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
            }
            $('#list').parent().css({ 'height': 'auto' });
            $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
            nomessage("list");
            deleteplan();
            toggle();           
            detail();
        }       
        else {
            errormessage("list");
        }

    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}
//开关
function update_isopen(id) {

    var data = {
        Func: "update_workplan_isopen",
        wp_status: Number(event.detail.isActive),
        id: id,
        guid: userid
    }
    getajax(url, data, function (json) {
    })
}
//开关
function toggle() {
    mui('.mui-content .mui-switch').each(function () {
        var id = $(this).children("input[type='hidden']").val();
        mui(this).switch();
        this.addEventListener('toggle', function (event) {
            update_isopen(id);
        });
    });
}
//详细
function detail(IsAdmin) {
    $('.mui-slider-handle').on('tap', function () {
        var id = $(this).children("input[name='wp_id']").val();
        var wp_userid = $(this).children("input[name='wp_userid']").val();      
        var href = "SeeWorkPlan.html?fdfd=ds112jj&dd_nav_bgcolor=FF3CCDAB" + "&id=" + id + "&wp_userid=" + wp_userid;
        //设置场景
        save_history();

        openwindow(href, href, 'fade-in');
    })
}
//删除
var ff = 0;//删除出现多个屏蔽
function deleteplan() {
    mui('.mui-btn-red').each(function () {
        var id = $(this).children("input[type='hidden']").val();
        this.addEventListener('tap', function (event) {
            ff++;
            if (ff <= 1) {
                var btnArray = ['确认', '取消'];
                var ele = this;
                mui.confirm('确认删除该条记录？', '', btnArray, function (e) {
                    var id = $(ele).parent().parent().children("input[name='wp_id']").val();
                    if (e.index == 0) {                       
                        var data = {
                            Func: "update_workplan_isdelete",
                            id: id,
                            wp_isdelete: 1,
                            guid: userid
                        }
                        getajax(url, data, function (json) {
                            debugger;
                            ff = 0;
                            if (json.result.errMsg == "success") {
                              
                                $(ele).parent().parent().remove();
                                 page_parmeter.pageindex = 1;
                                initdata(page_parmeter);
                            }
                            else if (json.result.errMsg == "failed") {
                                dd_toast(json.result.retData, 'error', 0);
                                ff = 0;
                            }
                        }, function () {
                            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
                        })
                    }
                    else {
                        ff = 0;
                    }
                });
            }
        });
    });
}

//详细
function info(id) {

    var data = {
        Func: "get_workplan_info",
        id: id,
        guid: userid
    }
    getajax_async(pageurl + '/WorkPlan/workplan_handle.ashx', data, function (json) {

        if (json.result.errMsg != null && json.result.errMsg == "success") {
            var retData = json.result.retData;
            //debugger
            //数据绑定
            $('#workplan_info').html(ejs.render($('#info').html(), { retData: retData }));
            //alert(retData.pic)
            if (retData.pic != null && retData.pic != "") {
                var pic = retData.pic.split(',');
                for (var i = 0; i < pic.length; i++) {
                    $("#img").append('<span><img src="' + pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                }
            }                     
            toggle();
        }
    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}

