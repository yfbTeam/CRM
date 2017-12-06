/*工作计划列表页*/
var url = pageurl + "/LinkMan/cust_linkman_handle.ashx";
var type = '';
$(function () {
    dd_setRightNone();
     initdata(page_parmeter);
    //监听搜索keyup事件
    var u = navigator.userAgent;
    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端\
    if (isAndroid == false) {
        //如果是ios 用blur事件
        $("#link_name").on("blur", function () {
             page_parmeter.pageindex = 1;
                                initdata(page_parmeter);
        });
    }
    else {
        //如果是android 用keyup事件
        $("#link_name").on("keyup", function () {
             page_parmeter.pageindex = 1;
                                initdata(page_parmeter);
        });
    }

    //隐藏加载框
    hidePreloader();
})

//数据
function initdata(page_parmeter) {
    if ($('#list_template').html() != null) {
        //选择客户后进行联系人的检索
        var link_cust_id = getparam("link_cust_id");
        var link_name = $("#link_name").val();
        var data = {
            Func: "get_cust_linkman_list",
            link_name: link_name,
            cust_users: userid,
            PageSize: page_parmeter.pagesize,
            PageIndex: page_parmeter.pageindex,
            ispage: true,
            link_cust_id:link_cust_id,
            guid: userid,
            type: page_parmeter.type
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData.PagedData;
                if (retData.length > 0) {
                    page_parmeter.RowCount = json.result.retData.RowCount;
                    //如果是首页就清空列表
                    if (page_parmeter.pageindex == 1) {
                        $('#list').empty();
                    }
                    //当列表数据过少是禁用下拉刷新，上拉加载更多
                    if (retData != null && retData != '' && retData.length <= 0) {
                        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                    }
                    $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
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
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }
    
}

//选择某个客户
function select() {   
    mui('#list li').each(function () {
        this.removeEventListener('tap', select_event, false)

        this.addEventListener('tap', select_event, false);
    });
}

function select_event(event) {

    //获取隐藏域的id和名称
    var id = $(this).children("input[name='link_id']").val();
    var name = $(this).children("input[name='link_name']").val();
    mui.back();
    //存储id和名称
    localStorage.setItem("link_name", name);
    localStorage.setItem("link_id", id);
}