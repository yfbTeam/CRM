/*工作日报*/


var url = pageurl + "/Report/workreport_handle.ashx";


function initReport_Self() {
    var data = {
        Func: "get_workreport_list",
        pagesize: page_parmeter.pagesize,
        pageindex: page_parmeter.pageindex,
        ispage: true,
        report_userid: report_userid,
        report_type: page_parmeter.report_type,
        type: 1,
        guid: userid,
        //departmentID: page_parmeter.departmentID,
        //memmberID: page_parmeter.memmberID
    }
    getajax_async(url, data, function (json) {
        //debugger;
        if (json.result.errMsg == "success") {
            //debugger;
            var retData = json.result.retData.PagedData;
            page_parmeter.RowCount = json.result.retData.RowCount;
            $(".count").html(page_parmeter.RowCount + "条");
            
            disablePull(retData);
            //debugger;
            switch (page_parmeter.report_type) {
                case 1:
                   
                    if (page_parmeter.pageindex == 1) {
                        $('#day_list').empty();                     
                   
                    }                  
                    $('#day_list').append(ejs.render($('#day_list_template').html(), { retData: retData }));
                    nomes('day_list', page_parmeter.RowCount, page_parmeter.pageindex);

                    break;
                case 2:
                   
                    if (page_parmeter.pageindex == 1) {
                        $('#week_list').empty();

                    }                 
                    $('#week_list').append(ejs.render($('#week_list_template').html(), { retData: retData }));
                    nomes('week_list', page_parmeter.RowCount, page_parmeter.pageindex);

                    break;
                case 3:
               
                    if (page_parmeter.pageindex == 1) {
                        $('#month_list').empty();

                    }
                
                    $('#month_list').append(ejs.render($('#month_list_template').html(), { retData: retData }));
                    nomes('month_list', page_parmeter.RowCount, page_parmeter.pageindex);
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
    if (retData != null && retData.length == 0) {
        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
    }
}

function nomes(id, rowcount, pageindex) {
    if (pageindex == 1 && rowcount == 0) {
        $('#' + id + '').parent().css({ 'height': '100%' });
        $('#' + id + '').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
    }
}

function err(id, rowcount, pageindex) {
    if (pageindex == 1 && rowcount == 0) {
        $('#' + id + '').parent().css({ 'height': '100%' });
        $('#' + id + '').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/error.png) no-repeat center;background-size: 3rem auto;"></div>');
    }
}
