/*客户信息*/
var url = pageurl + "/LinkMan/cust_linkman_handle.ashx";
var id = getparam("id");
$(function () {
    //新增联系人
    dd_setRightMenu('AddLinkMan.html?id=' + id + '&dd_nav_bgcolor=FF6CB1FF&number=' + Math.random(), "", [{ "id": "1", "iconId": "edit", "text": "编辑", }]);
    initdata();
    //联系人详细
    //mui('#link_info').each(function () { //循环所有toggle
    //    this.on('tap', 'a',function (event) {
            
    //        var href = "SeeLinkMan.html?id=" + id + "&ss=dkjdkj&&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random();
    //        openwindow(href, href, 'slide-in-right');
    //    });
    //});
    $('#link_info>a').on('tap', function () {
        var href = "SeeLinkMan.html?id=" + id + "&ss=dkjdkj&&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random();
         openwindow(href, href, 'slide-in-right');
    })
    //查看联系人信息
    $("#show_customer").on('tap', function () {
        var cust_id = getparam("cust_id");
        if (cust_id != 0 && cust_id != "") {
            var href = "/CustomerInfo/SeeCustomer.html?id=" + cust_id + "&dsds=fdkjkj&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random();
            openwindow(href, href, 'slide-in-right');
        }
        else {
            dd_toast('您未添加客户信息', 'error', 0);
        }
    })
    hidePreloader();
})

//初始化数据
function initdata() {
    var data = {
        Func: "get_cust_linkman_detail",
        id: id,
        userid: userid,
        guid: userid
    }
    getajax_async(url, data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;

            $("#link_name").html(retData.cust_linkman.link_name);
            $("#link_username").html(retData.cust_linkman.link_username);
            //$("#link_position").html(retData.cust_linkman.link_position);
            $("#link_level").html(retData.cust_linkman.link_level_name);
            $("#link_phonenumber").html(retData.cust_linkman.link_phonenumber);
            $('#link_phonenumber').attr('href', 'tel:' + retData.cust_linkman.link_phonenumber);
            $("#link_email").html(retData.cust_linkman.link_email);
            debugger;
            $("#customer_name").html(retData.cust_linkman.customer_name == "" || retData.cust_linkman.customer_name ==null? "暂未指定客户" : retData.cust_linkman.customer_name);
            $("#link_usersname").html(retData.cust_linkman.link_usersname);
            if (retData.follow_up != null && retData.follow_up.length > 0) {
                $('#follow_list').append(ejs.render($('#follow_list_template').html(), { follow_up: retData.follow_up }));
            } else {
                $('#follow_list').html('<li style="margin:0.15rem 0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }
            if (retData.workplan != null && retData.workplan.length > 0) {
                $('#workplan_list').append(ejs.render($('#workplan_list_template').html(), { workplan: retData.workplan }));
            } else {
                $('#workplan_list').html('<li style="margin-top: 0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }
            if (retData.sign != null && retData.sign.length > 0) {
                $('#sign_list').append(ejs.render($('#sign_list_template').html(), { sign: retData.sign }));
            } else {
                $('#sign_list').html('<li style="margin-top: 0;font-size:0.28rem;color:#888;">暂无数据</li>');
            }
            $("#pub_color").css('background-color', getcolor(retData.cust_linkman.link_level));


            //匹配详情查看更多 通过客户ID 去进行查看【传入跟踪的客户ID方便样式更改】
            $("#a_followup").attr("href", "../FollowRecord/FollowupRecord.html?ddsj=fddfd&dd_nav_bgcolor=FF6CB1FF" + "&link_id=" + retData.cust_linkman.id + "&cust_id_forlink=" + retData.cust_linkman.link_cust_id);
            
            //匹配详情查看更多 通过客户ID 去进行查看
            $("#a_WorkPlan").attr("href", "../WorkPlan/WorkPlan.html?dd_nav_bgcolor=FF6CB1FF" + "&link_id=" + retData.cust_linkman.id );

            //匹配详情查看更多 通过客户ID 去进行查看
            $("#a_VisitSign").attr("href", "../VisitSign/VisitSign.html?dd_nav_bgcolor=FFFA676F" + "&cust_id=" + retData.cust_linkman.link_cust_id);          
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}

