/*工作计划*/
var url = pageurl + "/Statistical/statistic_handle.ashx";
var wp_url = pageurl + '/WorkPlan/workplan_handle.ashx';
var report_url = pageurl + '/Report/workreport_handle.ashx';
var command_url = pageurl + '/Share/comment_handle.ashx';
var share_url = pageurl + '/Share/circle_share_handle.ashx';
var orgData = JSON.parse(localStorage.getItem("orgData"));
var id = "";
var myrole_name = "";
var r_type = 0;
$(function () {

    if (getparam("id") != "") {
        id = getparam("id");
    }
    //提交点评
    $("#submit_comment").on('tap', function () {
        submit_comment();
    })

    //添加信息才初始化统计信息
    var add_info = getQueryString("add_info");
    if (add_info != null) {
        httpData.initData(1);
    }

    httpData.initData_plan();
    httpData.initData_SeeDetail();

    httpData.reportInfo();

})

function submit_comment() {
    //if ($("#submit_comment").attr("fls") == 'false') {
    //    $("#submit_comment").attr("fls", 'true');
    var com_content = $('#com_content').val();
    if (com_content == "") {
        dd_toast('点评内容未填写！', 0);
        $("#submit_data").attr("fls", 'false');
    }
    else if (com_content.length < 10) {
        dd_toast('点评内容少于10个字,无法进行点评！', 0);
    }
    else {
        var data = {
            Func: "edit_comment",
            id: 0,
            com_table_id: id,
            com_parent_id: 0,
            com_content: $('#com_content').val(),
            com_userid: userid,
            com_username: username,
            com_type: 2,
            guid: userid
        }
        getajax_async(command_url, data, function (json) {
            
            if (json.result.errMsg == "success") {
                //点评完清空文本框内容
                $("#com_content").val('');
                var dianping_C = $("#dianping_C")

                //点评完直接显示点评内容
                if (dianping_C != null && dianping_C != '' && dianping_C != undefined) {

                    $("#dianping_C").append('<p  style="color:#7f9bec">' + username + ':' + com_content + '</p>');
                    $("#dianping_C").css({ 'line-height': '0.3rem', 'padding': '0.2rem' });
                }
            }
            else {
                dd_toast('点评失败！', 'error', 0);
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
        //}
        //} else {
        //    dd_toast('不可重复提交', 'error', 0);
    }

}

//添加报告
function submit() {
    if ($("#submit_data").attr("fls") == 'false') {

        $("#submit_data").attr("fls", 'true');
        $("#picture").val(today_pics.join(','));
        $("#picture_t").val(tomorrow_pics.join(','));
        $("#report_senders").val(report_senders.join(','));
        $("#report_readers").val(report_readers.join(','));

        var enddate = $("#report_enddate").attr("value");
        var startdate = $("#report_startdate").attr("value");

        var type = $("#report_type").attr("value");

        //若为周报，开始时间与结束时间一样
        if (type == "1") {

            $("#report_enddate").attr("value", startdate);
        }
        //$("#report_enddate").val('2016-12-20');
        $("#report_reader").val();
        // $("#report_sender").val(report_senders_name.join('、'));
        $("#report_sender").val();

        if (validateForm($("input[type='hidden'],input[type='text'],select,textarea")) == "0") {
            //alert($("#report_sender").val())
            var data = getForm($("input[type='hidden'],input[type='text'],select,textarea"));
            getajax(report_url, data, function (json) {
                if (json.result.errMsg == "success") {
                    mui.back();
                    dd_toast('保存成功！', 'success', 0);
                }
                else {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('保存失败！', 'error', 0);
                }
            }, function () {
                dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
            })
        }
        else {

            $("#submit_data").attr("fls", 'false');
        }
    }
    else {
        dd_toast('不可重复保存', 'error', 0);
    }
}
//初始化数据【数量统计】
var httpData = {
    initData: function (type, s_startdate, s_enddate) {

        if (type == 1) {
            s_enddate = s_startdate;
        }
        var data = {
            Func: "get_report_tongji_time",
            userid: userid,
            username: username,
            type: type,
            s_startdate: s_startdate,
            s_enddate: s_enddate,
            guid: userid
        }
        getajax_async(report_url, data, function (json) {

            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                //$("#s_bf_count1").html(retData.s_bf_count);
                //$("#s_linkman_count").html(retData.s_linkman_count);
                //$("#s_sign_count").html(retData.s_sign_count);
                //$("#s_bf_count").html(retData.s_bf_count);
                //$("#s_cust_customer_count").html(retData.s_cust_customer_count);

                $("#cust_customer_count").html(retData.s_cust_customer_count);
                $("#cust_linkman_count").html(retData.s_linkman_count);
                $("#follow_up_count").html(retData.s_followup_count);
                $("#sign_in_count").html(retData.s_sign_count);

            }
            else {
                //$("#s_bf_count1").html("0");
                //$("#s_linkman_count").html("0");
                //$("#s_sign_count").html("0");
                //$("#s_bf_count").html("0");
                //$("#s_cust_customer_count").html("0");

                $("#cust_customer_count").html(0);
                $("#cust_linkman_count").html(0);
                $("#follow_up_count").html(0);
                $("#sign_in_count").html(0);
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    },
    initData_plan: function () { /**---------------------计划列表---------------------------------***/
        var data = {
            Func: "get_workplan_today",
            wp_userid: userid,
            guid: userid
        }
        getajax_async(wp_url, data, function (json) {
            //debugger;
            if (json.result != null && json.result.errMsg == "success") {
                var retData = json.result.retData;
                if (retData != null && retData.length > 0) {
                    var list = [];
                    list.push(' <li><div class="mes_wrap clearfix"><div class="people_img fl"><img src="../images/people_img.jpg" />');
                    list.push('</div><div class="mes_deta fr"><div class="clearfix a"><span class="people fl">' + retData[0].wp_username + '</span><span class="time fr">' + retData[0].wp_plandate + '</span>');
                    list.push('</div><div class="content">' + retData[0].wp_content + '</div><div ><span>' + '</span><span>' + '</span>');
                    list.push('</div></div></div></li>');
                    $('#list').append(list.join(''));
                }
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    },
    initData_SeeDetail: function () {
        if ($('#cust_customer_template').html() != null) {
            var data = {
                Func: "get_statistic_detail",
                userid: userid,
                guid: userid
            }
            getajax_async(url, data, function (json) {

                if (json.result.errMsg == "success") {
                    var retData = json.result.retData;
                    $("#cust_linkmancount").html("（" + retData.cust_linkmancount.count + "）");
                    $("#cust_customer_count").html("（" + retData.cust_customer_count.count + "）");
                    $("#sign_in_count").html("（" + retData.sign_in_count.count + "）");
                    $("#baifang_link_count").html("（" + retData.baifang_link_count.count + "）");
                    $("#baifang_count").html("（" + retData.baifang_count.count + "）");
                    $('#cust_customer').append(ejs.render($('#cust_customer_template').html(), { retData: retData.cust_customer_count.statistic_detail }));
                    $('#cust_linkman').append(ejs.render($('#cust_linkman_template').html(), { retData: retData.cust_linkmancount.statistic_detail }));
                    $('#sign_in').append(ejs.render($('#sign_in_template').html(), { retData: retData.sign_in_count.statistic_detail }));
                    $('#baifang_link').append(ejs.render($('#baifang_link_template').html(), { retData: retData.baifang_link_count.statistic_detail }));
                    $('#baifang').append(ejs.render($('#baifang_template').html(), { retData: retData.baifang_count.statistic_detail }));

                }
            }, function () {
                dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
            })
        }
    },
    reportInfo: function () {
        var data = {
            Func: "workreport_info",
            id: id,
            guid: userid
        }
        //工作报告详细信息
        getajax_async(report_url, data, function (json) {
            if (json.result.errMsg == "success") {
                //debugger;
                var retData = json.result.retData;
                var role = localStorage["role"];
                if (role == "Super_Admin" || role == "Common_Admin") {
                    $(".commit").show();
                }
                var report_type = retData.report_type;
                r_type = report_type;

                var report_content_tip;
                var report_plan_tip;
                var re_type = "";
                if (report_type == 1) {
                    re_type = "日报";
                    report_content_tip = "今日总结：";
                    report_plan_tip = "明日计划：";
                }
                else if (report_type == 2) {
                    re_type = "周报";
                    report_content_tip = "本周总结：";
                    report_plan_tip = "下周计划：";
                }
                else {
                    re_type = "月报";
                    report_content_tip = "本月总结：";
                    report_plan_tip = "下月计划：";
                }
                var report_enddate = "";
                if (report_type != 1) {
                    report_enddate = retData.report_enddate;
                    report_enddate = "~" + report_enddate;
                }
                //alert(report_enddate);
                $("#title").html(retData.report_username + "的" + re_type + " " + retData.report_startdate.substr(0, 11) + report_enddate.substr(0, 11));
                $("#report_content").html(report_content_tip + retData.report_content);
                $("#report_plan").html(report_plan_tip + retData.report_plan);
                $("#report_createdate").html(retData.report_createdate);
                $("#report_reader").html(retData.report_reader);
                $("#report_sender").html(retData.report_sender);
                //alert(JSON.stringify(retData))
                //debugger;
                //新增客户数量 && retData.report_cust_customer_array.indexOf(",") != -1
                var cust_customer_count;
                if (retData.report_cust_customer_array != "" && retData.report_cust_customer_array != null) {
                    report_cust_customer_array = retData.report_cust_customer_array.split(","); //字符分割 
                    cust_customer_count = report_cust_customer_array.length;
                    $("#cust_customer_count").html(cust_customer_count);
                }
                //新增联系人数量 && retData.report_cust_linkman_array.indexOf(",") != -1
                var cust_linkman_count;
                if (retData.report_cust_linkman_array != "" && retData.report_cust_linkman_array != null) {
                    report_cust_linkman_array = retData.report_cust_linkman_array.split(","); //字符分割 
                    cust_linkman_count = report_cust_linkman_array.length;
                    $("#cust_linkman_count").html(cust_linkman_count);
                }
                //跟进次数 && retData.report_follow_up_array.indexOf(",") != -1
                var follow_up_count;
                if (retData.report_follow_up_array != "" && retData.report_follow_up_array != null) {
                    report_follow_up_array = retData.report_follow_up_array.split(","); //字符分割 
                    follow_up_count = report_follow_up_array.length;
                    $("#follow_up_count").html(follow_up_count);
                }
                //签到次数  && retData.report_sign_in_array.indexOf(",") != -1
                var sign_in_count;
                if (retData.report_sign_in_array != "" && retData.report_sign_in_array != null) {
                    report_sign_in_array = retData.report_sign_in_array.split(","); //字符分割 
                    sign_in_count = report_sign_in_array.length;
                    $("#sign_in_count").html(sign_in_count);
                }

                var dianping = retData.dianping;
                //alert(JSON.stringify(dianping))
                if (dianping != "") {
                    $("#dianping_C").css({ 'line-height': '0.3rem', 'padding': '0.2rem' });
                    var dianpings = dianping.split(',');
                    for (var i = 0; i < dianpings.length; i++) {
                        $("#dianping_C").append('<p style="color:#7f9bec">' + dianpings[i] + '</p>');
                    }
                }
                else {
                    //$("#dianping_C").parent().empty();
                }
                isshare = retData.isshare;
                if (json.result.retData.t_pic != null && json.result.retData.t_pic != "") {
                    $("#t_img_wrap").show();
                    var t_pic = json.result.retData.t_pic.split(',');
                    for (var i = 0; i < t_pic.length; i++) {
                        $("#t_img").append('<span><img src="' + t_pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                    }
                }
                if (json.result.retData.m_pic != null && json.result.retData.m_pic != "") {
                    $("#m_img_wrap").show();
                    var m_pic = json.result.retData.m_pic.split(',');
                    for (var i = 0; i < m_pic.length; i++) {
                        $("#m_img").append('<span><img src="' + m_pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                    }
                }
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }

}

