/*客户详情*/
var url = pageurl + "/Custom/cust_customer_handle.ashx";
var id = getparam("id");
$(function () {
    //debugger;
    var  cust_category = initdata();
    mui('#info_list li').each(function () { //循环所有toggle
        this.addEventListener('tap', function (event) {
            var href = "SeeCustomer.html?id=" + id + "&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random();
            showPreloader();
            openwindow(href, href, 'slide-in-right');
        });
    });


    $('.info_wrap').on('tap', 'a', function () {
        var id = this.getAttribute('href');
        var href = this.href + "&number=" + Math.random();
        openwindow(href, href, 'slide-in-right');
    })
    //工作计划开关
    mui('.mui-content .mui-switch').each(function () { //循环所有toggle
        var id = $(this).children("input[type='hidden']").val();
        this.addEventListener('toggle', function (event) {
            update_isopen(id);
        });
    });

    $('#a_user').on('tap', function () {
        var id_h = this.getAttribute('href');
        var href = this.href;
        localStorage.setItem("cust_id", id);       
        openwindow(id_h, href, 'slide-in-right');
    })
   
    //隐藏加载框
    hidePreloader();
})


//初始化数据【详情】
function initdata() {
    var data = {
        Func: "get_cust_customer_detail",
        id: id,
        userid: userid,
        guid: userid
    }
    getajax_async(url, data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;
            var role = localStorage["role"];            
            if (retData.cust.cust_category == 1) {//
                if (role == 'Super_Admin') {
                    //钉钉右侧                   
                    dd_setRightMenu('NewCustomer.html?dd_nav_bgcolor=FF6CB1FF&id=' + id + '&number=' + Math.random(), "", [{ "id": "1", "iconId": "edit", "text": "编辑", }]);
                }
                else {
                    dd_setRightNone();
                }
            }
            else {
                //只有总监可以指派
                if (role == "Common_Admin") {
                    $("#commen_role").css("display", "block");
                }
                //钉钉右侧
                dd_setRightMenu('NewCustomer.html?dd_nav_bgcolor=FF6CB1FF&id=' + id + '&number=' + Math.random(), "", [{ "id": "1", "iconId": "edit", "text": "编辑", }]);
            }          

            //$("#custom_id").val(retData.cust.cust_id);
            $("#cust_name").html( retData.cust.cust_name);
            //存储当前用户名称和用户id号，方便后期指派使用
            localStorage.setItem("current_select_user_names", retData.cust.cust_usersname );
            localStorage.setItem("current_select_user_ids",  retData.cust.cust_users_id );


            $("#cust_level").html(retData.cust.cust_level);
            $("#cust_followdate").html(retData.cust.cust_followdate);
            $("#cust_usersname").html(retData.cust.cust_usersname);

            //debugger;
            //客户详情，统计数量
            $('#f_count').html("(" + retData.Count_All_follow + "条)");
            $('#l_count').html("(" + retData.Count_All_linkman + "个)");
            $('#s_count').html("(" + retData.Count_All_sign + "条)");

            if (retData.follow_up != null && retData.follow_up.length > 0) {
                $('#follow_list').append(ejs.render($('#follow_list_template').html(), { follow_up: retData.follow_up }));
            } else {
                $('#follow_list').html('<li style="margin:0.15rem 0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }
            if (retData.linkman != null && retData.linkman.length > 0) {
                $('#linkman_list').append(ejs.render($('#linkman_list_template').html(), { linkman: retData.linkman }));
            } else {
                $('#linkman_list').html('<li style="margin:0.15rem 0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }
            
            //工作计划已经不关联了
            //if (retData.workplan != null && retData.workplan.length > 0) {
            //    $('#workplan_list').append(ejs.render($('#workplan_list_template').html(), { workplan: retData.workplan }));
            //} else {
            //    $('#workplan_list').html('<li style="margin-top:0;font-size:0.28rem;color:#888;">暂无数据</li>');
            //}
            if (retData.sign != null && retData.sign.length > 0) {
                $('#sign_list').append(ejs.render($('#sign_list_template').html(), { sign: retData.sign }));
            } else {
                $('#sign_list').html('<li style="margin-top:0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }

            //匹配详情查看更多 通过客户ID 去进行查看
            $("#a_followup").attr("href", "../FollowRecord/FollowupRecord.html?ddsj=fddfd&dd_nav_bgcolor=FF6CB1FF" + "&cust_id=" + retData.cust.cust_id);

            //匹配详情查看更多 通过客户ID 去进行查看
            $("#a_LinkMan").attr("href", "../LinkMan/LinkMan.html?dd_nav_bgcolor=FF6CB1FF" + "&cust_id=" + retData.cust.cust_id);

            //工作计划已经不关联了
            ////匹配详情查看更多 通过客户ID 去进行查看
            //$("#a_WorkPlan").attr("href", "../WorkPlan/WorkPlan.html?dd_nav_bgcolor=FF6CB1FF" + "&cust_id=" + retData.cust.cust_id);

            var sin_first_date = retData.sign.sign_date;
            //匹配详情查看更多 通过客户ID 去进行查看
            $("#a_VisitSign").attr("href", "../VisitSign/VisitSign.html?dd_nav_bgcolor=FFFA676F" + "&cust_id=" + retData.cust.cust_id + "&sin_first_date" + sin_first_date);
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
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
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}