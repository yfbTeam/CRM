var pics = [];
var id = getparam("id");
if (id != "") {
    document.title = '新增计划';
    info(id);
}
else {
    document.title = '编辑计划';
}


//提交
function submit() {
    var s_startdate = $("#s_startdate").val();
    var s_enddate = $("#s_enddate").val();
    if (s_startdate <= s_enddate) {
        if ($("#submit_data").attr("fls") == 'false') {
            $("#submit_data").attr("fls", 'true');
            var url = pageurl + "/WorkPlan/workplan_handle.ashx";
            $("#picture").val(pics.join(','));
            if (validateForm($("input[type='text'],input[type='hidden'],select,textarea")) == "0") {
                var data = getForm($("input[type='hidden'],input[type='text'],select,textarea"));
                getajax(url, data, function (json) {
                    if (json.result.errMsg == "success") {
                        mui.back();
                        dd_toast('保存成功！', 'success', 0);
                    }
                    else {
                        dd_toast('保存失败！', 'error', 0);
                    }
                }, function () {
                    dd_toast('接口错误，请联系管理员！', 'error', 0);
                })
            }
            else {
                $("#submit_data").attr("fls", 'false');
            }
            if (s_startdate <= s_enddate) {

            }

        }
        else {
            dd_toast('不可重复提交', 'error', 0);
        }
    }
    else {
        dd_toast('开始时间不能大于结束时间', 'error', 0);
    }
}

//获取联系人
function initdata() {
    retData = [];
    //var wp_cust_id = localStorage.getItem("wp_cust_id");
    //if (wp_cust_id != "" && wp_cust_id != null) {


    //}
    //else {
    //    dd_toast('您还没有联系人！', 'error', 0);
    //    picker.hide();
    //}
    var data = {
        Func: "get_cust_linkman_list",
        pagesize: 99999999,
        pageindex: 1,
        ispage: false,
        link_users: userid,
        //link_cust_id: wp_cust_id,//暂时先去掉客户id  包括上边注释的内容
        guid: userid
    }
    var np_url = pageurl + "/LinkMan/cust_linkman_handle.ashx";
    getajax_async(np_url, data, function (json1) {
        var retLen = 0;
        if (json1.result.errMsg == "success") {
            retLen = json1.result.retData.length;
            for (var i = 0; i < retLen; i++) {
                retData.push({ "text": json1.result.retData[i].link_name, "value": json1.result.retData[i].link_position, "id": json1.result.retData[i].id });
            }
        }
        if (retLen == 0) {
            dd_toast('暂无联系人！', 'error', 0);
            picker.hide();
        }

    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}

//详细
function info(id) {
    if (id != null && id != "" && id != undefined) {
        var data = {
            Func: "get_workplan_info",
            id: id,
            guid: userid
        }
        getajax_async(pageurl + '/WorkPlan/workplan_handle.ashx', data, function (json) {

            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                var cust_name = retData.cust_name;
                $("input[name='id']").val(retData.id);
                $("#question").html(retData.wp_content);

                var d1 = (retData.wp_plandate != null && retData.wp_plandate != "" && retData.wp_plandate != '1800/1/1 0:00:00') ? retData.wp_plandate.formatTime("{0}-{1}-{2}") : "";
                var d2 = (retData.wp_endplandate != null && retData.wp_endplandate != "" && retData.wp_endplandate != '1800/1/1 0:00:00') ? retData.wp_endplandate.formatTime("{0}-{1}-{2}") : "";
                var d3 = (retData.wp_reminddate != null && retData.wp_reminddate != "" && retData.wp_reminddate != '1800/1/1 0:00:00') ? retData.wp_reminddate.formatTime("{0}-{1}-{2}") : "";

                //$("#cust_customer").val(retData.cust_name);
                //$("#link_name").val(retData.link_name);
                $("#link_position").html(retData.link_position);
                $("#s_startdate").val(d1);
                $("#s_enddate").val(d2);

                //== '1800-01-01' ? "" : retData.wp_reminddate

                $("#select_time").val(d3);
                //$("#wp_cust_id").val(retData.wp_cust_id);
                //$("#wp_link_id").val(retData.wp_link_id);
                if (retData.wp_status == 1) {
                    $("#tixing").addClass("mui-active");
                }

                if (retData.pic != null && retData.pic != "") {
                    $(".mes_lists").show();
                    var pic = retData.pic.split(',');
                    for (var i = 0; i < pic.length; i++) {
                        $("#img").append('<span><img src="' + pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                        pics.push(pic[i]);
                    }
                    mui.previewImage();
                }
            }

        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }
}