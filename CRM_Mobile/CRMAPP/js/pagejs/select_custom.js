/*工作计划列表页*/
var url = pageurl + "/Custom/cust_customer_handle.ashx";
var type = '';
$(function () {
    dd_setRightNone();
    initdata(page_parmeter);
    //监听搜索keyup事件
    var u = navigator.userAgent;
    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端\
    if (isAndroid == false) {
        $("#cust_name").on("blur", function () {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        });
    }
    else {
        $("#cust_name").on("keyup", function () {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        });
    }
    hidePreloader();
})

//数据
function initdata(page_parmeter) {

    var cust_category = getQueryString('cust_category');
    if (cust_category == null || cust_category == undefined || cust_category == '') {
        cust_category = 2
    }
    debugger;
    if ($('#list_template').html() != null) {
        var cust_name = $("#cust_name").val();
        var data = {
            Func: "get_cust_customer_search",
            cust_name: cust_name,
            cust_users: userid,
            sign_x: xself,
            sign_y: yself,
            PageSize: page_parmeter.pagesize,
            PageIndex: page_parmeter.pageindex,
            ispage: true,
            guid: userid,
            type: page_parmeter.type,
            cust_category: cust_category
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData.PagedData;
                //debugger;
                if (retData.length > 0) {
                    page_parmeter.RowCount = json.result.retData.RowCount;
                    if (page_parmeter.pageindex == 1) {
                        $('#list').empty();
                    }

                    //当列表数据过少是禁用下拉刷新，上拉加载更多
                    if (retData != null && retData != '' && retData.length <= 0) {
                        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                    }
                    $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                    //nomessage("list");

                    //选择某一客户后返回
                    select();

                }
                else {
                    nomessage("list");

                }
            }
            else {
                nomessage("list");
            }
        }, function () {
            debugger;
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }

}

//选择某个客户
function select() {
  
    mui('#list li').each(function () {

        this.removeEventListener('tap', select_event, false)

        this.addEventListener('tap', select_event,false);
    });
}


function select_event(event)
{
    var id = $(this).children("input[name='cust_id']").val();
    var name = $(this).children("input[name='cust_name']").val();
    var type = getparam("type");
    var page = getparam("page");
    mui.back();
    //debugger;
    if (page == 1) {//工作计划
        localStorage.setItem("wp_cust_name", name);
        localStorage.setItem("wp_cust_id", id);
    }
    else if (page == 2) {//联系人
        localStorage.setItem("link_cust_name", name);
        localStorage.setItem("link_cust_id", id);
    }
    else {//跟进记录               
        localStorage.setItem("follow_cust_name", name);
        localStorage.setItem("follow_cust_id", id);
    }
       
}




