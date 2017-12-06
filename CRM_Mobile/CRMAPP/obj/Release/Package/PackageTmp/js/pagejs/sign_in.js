/*签到*/
var sign_url = pageurl + "/SiginIn/sign_in_handle.ashx";

//指定客户的客户ID【如果有url传值方式传值，接收，并设置为筛选条件】
var sign_cust_id = getQueryString("cust_id");
//针对某个人的具体制定位置
var sin_first_date = getQueryString("sing_first_date");


var type = '';
/**---------------------初始化数据---------------------------------***/
function initdata(page_parmeter) {

    //有指定时间的，某个客户详情里获取的签到，有可能不是默认当天的，需要重新设置时间
    if (sin_first_date != null && sin_first_date != '' && sin_first_date != undefined) {

        page_parmeter.stardate = sin_first_date;
        page_parmeter.enddate = sin_first_date;
        $("#s_startdate").val(page_parmeter.stardate);
        $("#s_enddate").val(page_parmeter.stardate);
    }

    //debugger;
    var data = {
        Func: "get_sign_list",
        PageSize: page_parmeter.pagesize,
        PageIndex: page_parmeter.pageindex,
        ispage: true,
        sign_userid: userid,
        guid: userid,
        sign_cust_id: sign_cust_id,
        stardate: page_parmeter.stardate,
        enddate: page_parmeter.enddate,
        departmentID: page_parmeter.departmentID,
        memmberID: page_parmeter.memmberID
    }
    getajax_async(sign_url, data, function (json) {

        if (json.result.errMsg == "success") {

            var retData = json.result.retData.PagedData;
            page_parmeter.RowCount = json.result.retData.RowCount;
            $(".count").html('总共' + page_parmeter.RowCount + "条");
            if (page_parmeter.pageindex == 1) {
                $('#list').empty();
            }
            //当列表数据过少是禁用下拉刷新，上拉加载更多
            if (retData != null && retData != '' && retData.length <= 0) {
                mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
            }
            $('#list').parent().css({ 'height': 'auto' });

            //不同的角色，签到信息显示的不一样
            var role = localStorage["role"];
            if (role == "Super_Admin" || role == "Common_Admin") {
                $('#list').append(ejs.render($('#list_template_admin').html(), { retData: retData }));
            }
            else {
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
            }
            nomessage("list");
        }
        else {
            page_parmeter.RowCount = 0;
            $('#list').parent().css({ 'height': '100%' });
            //$('#list').empty();
            nomessage("list");
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}
var result = true;

function saveData(sign_userid, sign_username, sign_cust_id, sign_x, sign_y, sign_address, sign_offset) {

    var data = {
        Func: "edit_sign_in",
        id: 0,
        sign_userid: sign_userid,
        sign_username: sign_username,
        sign_cust_id: sign_cust_id,
        sign_x: sign_x,
        sign_y: sign_y,
        sign_address: sign_address,
        sign_offset: sign_offset,
        sign_location: sign_x + sign_y,
        guid: userid
    }

    getajax(sign_url, data, function (json) {
        //alert(JSON.stringify(data));
        if (json.result.errNum == "0") {
            if (result) {
                result = false;
                dd_toast('签到成功！', 'sucess', 0);
                //mui.back();
                //document.location.href("http://www.baidu.com");
                mui.back();
            }

        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}

