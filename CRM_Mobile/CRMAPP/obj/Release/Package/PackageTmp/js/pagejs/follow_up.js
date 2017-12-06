var s_url = pageurl + "/PubParam/pub_param_handle.ashx";
var url = pageurl + "/Follow/follow_up_handle.ashx";

var fo_users = [];
var myrole_name = '';//主要看是不是上级，若是头部的显示，否则隐藏，主要用于距离上部的高度
var type = '';
/*-------------------绑定跟进记录-------------------------------*/
function initdata(page_parmeter) {
    if ($('#list_template').html() != null) {
        var data = {
            Func: "get_follow_up_list",
            follow_userid: fo_users.join(','),
            pagesize: page_parmeter.pagesize,
            pageindex: page_parmeter.pageindex,
            ispage: true,
            riqi: riqi,
            follow_cust_id: follow_cust_id,
            link_id: link_id,
            guid: userid,
            is_self_guid: is_self_guid,
            type: page_parmeter.type,
            departmentID: page_parmeter.departmentID,
            memmberID: page_parmeter.memmberID
        }
        getajax_async(url, data, function (json) {
            //debugger;
          
            if (json.result.errMsg == "success") {               
                var retData = json.result.retData.PagedData;
                if (retData.length > 0) {
                    page_parmeter.RowCount = json.result.retData.RowCount;
                    $(".count").html(page_parmeter.RowCount + "条");
                    if (page_parmeter.pageindex == 1) {
                        $('#list').empty();
                    }
                    //当列表数据过少是禁用下拉刷新，上拉加载更多
                    if (retData != null && retData != '' && retData.length <= 0) {
                        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                    }


                    //debugger;
                    $('#list').parent().css({ 'height': 'auto' });
                    $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                    nomessage("list");//无数据时
                    initPraise();//点赞
                    detail();//暂时隐藏
                    com_save();
                 
                    initCust();
                    CustomerSele();
                   
                }
                else {
                    page_parmeter.RowCount = json.result.retData.RowCount;
                    $(".count").html(page_parmeter.RowCount + "条");
                    nomessage("list");
                }
            }
            else {
                nomessage("list");
            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }

}

//客户选择
function CustomerSele() {
    $('#person_sele p').on('tap', function () {

        $(this).addClass('active').siblings().removeClass('active');
        var text = $(this).children("input[type='hidden']").val();
        follow_cust_id = text;
        var val = $(this).text();
        $('#tools_flow a:eq(1)').find('span').html(val + '<i class="iconfont">&#xe61e;</i>');
        $('.mark_wrap').css('display', 'none');
        $('.time_sele').css('display', 'none');
        $('.tou_sele').css('display', 'none');
        page_parmeter.pageindex = 1;
        initdata(page_parmeter);
        falg3 = true;
        //切换客户之后将那些先天的筛选条件去掉
        cust_id_forlink = "";
        follow_cust_id = "";
        link_id = "";


    })
}

function getii() {
    var orgData = JSON.parse(localStorage.getItem("orgData"));
    if (orgData != "" && orgData != null) {
        var orglen = orgData.length;
        fo_users.push("'" + userid + "'");
        for (var i = 0; i < orglen; i++) {
            if (orgData[i].Name == username) {
                myrole_name = orgData[i].RoleName;
            }
            if (orgData[i].Name != username && myrole_name != "") {
                fo_users.push("'" + orgData[i].UniqueNo + "'");
            }
        }
        if (myrole_name == "") {
            $('.choose_wrap').css('display', 'none');
            $(".tools").css('top', '0');
            $("#pullrefresh").css('top', '0.8rem');
            $('#time_sele').css({ 'top': '0.8rem' });
        }
    }

}

//评论或回复
function com_save() {
    $(".com").on('tap', function () {
        var com_table_id = $(this).children('input[type="hidden"]').val();
        input_plain(0, com_table_id, "评论一下吧！")
    })
    $(".com_2").on('tap', function () {
        var com_table_id = $(this).children('input[name="table_id"]').val();//评论外键表的id
        var com_parent_id = $(this).children('input[name="parent_id"]').val();//评论表的父id
        var com_userid = $(this).children('input[name="com_userid"]').val();//评论人
        //如果评论人是当前用户的话，就执行删除的功能
        if (com_userid == userid) {
            dd.ready(function () {
                dd.device.notification.actionSheet({
                    cancelButton: '取消', //取消按钮文本
                    otherButtons: ["删除"],
                    onSuccess: function (result) {
                        if (result.buttonIndex != -1) {
                            if (result.buttonIndex == 0) {
                                var data1 = {
                                    Func: "update_comment_isdelete",
                                    id: com_parent_id,
                                    com_isdelete: 1,
                                    guid: userid
                                }
                                $("#item1_" + com_parent_id).remove();
                                getajax(pageurl + "/Share/comment_handle.ashx", data1, function (json1) {

                                }, function () {
                                    dd_toast('接口错误，请联系管理员！', 'error', 0);
                                })
                            }

                        }
                    },
                    onFail: function (err) { }
                })
            });
        }
        else {
            input_plain(com_parent_id, com_table_id, '回复一下吧！')
        }

    })
    $(".com3").on('tap', function () {
        var com_parent_id = $(this).children('input[name="parent_id"]').val();
        var com_userid = $(this).children('input[name="com_userid"]').val();
        if (com_userid == userid) {
            dd.ready(function () {
                dd.device.notification.actionSheet({
                    cancelButton: '取消', //取消按钮文本
                    otherButtons: ["删除"],
                    onSuccess: function (result) {
                        if (result.buttonIndex != -1) {
                            if (result.buttonIndex == 0) {
                                var data = {
                                    Func: "update_comment_isdelete",
                                    id: com_parent_id,
                                    com_isdelete: 1,
                                    guid: userid
                                }
                                getajax(pageurl + "/Share/comment_handle.ashx", data, function (json) {
                                    $("#item2_" + com_parent_id).remove();
                                }, function () {
                                    dd_toast('接口错误，请联系管理员！', 'error', 0);
                                })
                            }

                        }
                    },
                    onFail: function (err) { }
                })
            });
        }
    })
    $(".comment").on('tap', function () {
        var com_table_id = $(this).children('input[type="hidden"]').val();
        input_plain(0, com_table_id, "评论一下吧！")
    })
}

//获取跟进详细  通过模板来赋值的
function follow_up_info(id) {
    if ($('#info_template').html() != null) {
        var data = {
            Func: "get_follow_up",
            id: id,
            guid: userid
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                //debugger;
                $('.mui-content').html(ejs.render($('#info_template').html(), { retData: retData }));
                if (json.result.retData.pic != null && json.result.retData.pic != "") {
                    var pic = json.result.retData.pic.split(',');
                    for (var i = 0; i < pic.length; i++) {
                        $("#img").append('<span><img src="' + pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                    }
                }
                //toggle();
            }
            else {
                //mui.toast('无数据')
            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }
}

//新增详情
function getfollowup() {
    var id = getparam("id");
    $("#id").val(id);
    var data = {
        Func: "get_follow_up",
        id: id,
        guid: userid
    }
    getajax_async(url, data, function (json) {

        //debugger;
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;
            $("#follow_userid").val(retData.follow_userid);
            $("#guid").val(retData.follow_userid);
            $("#follow_username").val(retData.follow_username);
            $("#follow_cust_id").val(retData.follow_cust_id);
            $("#follow_link_id").val(retData.follow_link_id);
            $("#follow_type").val(retData.follow_type);
            $("#cust_customer").val(retData.follow_cust_name);
            $("#link_name").val(retData.follow_link_name);
            $("#select_type").val(retData.follow_type_name);
            $("#follow_content").val(retData.follow_content);
            if (retData.pic != null && retData.pic != "") {
                $(".mes_lists").show();
                var pic = retData.pic.split(',');
                for (var i = 0; i < pic.length; i++) {
                    $("#img").append('<span><img src="' + pic[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                }
            }
            //toggle();
        }
        else {
            //mui.toast('无数据')
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}



function detail() {
    mui('.content').each(function () { //循环所有toggle
        var id = $(this).children("input[type='hidden']").val();
        this.addEventListener('tap', function (event) {
            var href = "SeeFollowupRecord.html?id=" + id + "&dd_nav_bgcolor=FF6CB1FF&a=a";
             //设置场景
            save_history();
            localStorage.setItem("riqi", riqi);

            mui.openWindow({
                id: href,
                url: href,
                show: {
                    autoShow: true,
                    aniShow: 'slide-in-right',
                    duration: '200ms'
                },
                waiting: {
                    autoShow: true
                }
            });
        });
    });
}

//保存跟进记录
function submit() {
    $("#picture").val(pics.join(','));
    if ($("#submit_data").attr("fls") == 'false') {
        $("#submit_data").attr("fls", 'true');
        if (validateForm($("input[type='hidden'],input[type='text'],select,textarea")) == "0") {
            var data = getForm($("input[type='hidden'],input[type='text'],select,textarea"));
            getajax(url, data, function (json) {
                if (json.result.errMsg == "success") {
                    dd_toast('保存成功！', 'success', 0);
                    mui.back();
                }
                else {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('保存失败！', 'error', 0);
                }
            }, function () {
                $("#submit_data").attr("fls", 'false');
                dd_toast('接口错误，请联系管理员！', 'error', 0);
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

//重新加载评论的内容  type 为1表示是跟进评论，此处不用改动，程序代码自己约定
function getcomment(id) {
    var data = {
        Func: "get_comment",
        id: id,
        type: 1,
        guid: userid
    }
    getajax_async(pageurl + '/Share/comment_handle.ashx', data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;
            var com_list = [];
            var len = retData.length;
            if (len > 0) {
                for (var i = 0; i < len; i++) {
                    com_list.push('<input type="hidden" name="table_id" value="' + retData[i].com_table_id + '" />');
                    if (retData[i].com_parent_id == "0") {
                        //评论内容
                        com_list.push('<p class="com_2" id="item1_' + retData[i].id + '"><input type="hidden" name="com_userid" value=' + retData[i].com_userid + ' /><input type="hidden" name="parent_id" value="' + retData[i].id + '" /><span>' + retData[i].com_username + '：</span>' + retData[i].com_content + '</p>');
                        for (var j = 0; j < len; j++) {
                            if (retData[j].com_parent_id == retData[i].id && retData[j].com_parent_id != "0") {
                                //回复内容
                                com_list.push('<p  class="com3" id="item2_' + retData[j].id + '"><span> ' + retData[j].com_username + ' 回复 ' + retData[i].com_username + '：</span>' + retData[j].com_content + '</p>');
                            }
                        }
                    }
                }
                $("#comment_list_" + id).html(com_list.join(''));
                com_save();
            }



        }
        else {
            //mui.toast('无数据')
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}

//点赞
function initPraise() {
    //点赞
    $('.mes_lists li').each(function () {

        var _this = this;
        $(_this).on('tap', '.dianzan', function () {

            var id = $(this).children('input[type="hidden"]').val();
            if ($("#ispraise" + id + "").is('.active')) {
                $("#ispraise" + id + "").removeClass('active');
            }
            else {
                $("#ispraise" + id + "").addClass('active');
            }
            var data = {
                Func: "edit_praise",
                id: 0,
                praise_table_id: id,
                praise_userid: userid,
                praise_username: username,
                praise_type: 1,
                guid: userid
            }
            getajax(pageurl + '/Share/praise_handle.ashx', data, function (json) {
               
                if (json.result.errMsg == "success") {
                    //debugger;
                    var retData = json.result.retData;
                    if (retData.length > 0) {
                        $("#praise" + id + "").empty();
                        var shtml = '';
                        for (var i = 0; i < retData.length; i++) {
                            shtml += retData[i].praise_username + " ";

                        }
                        $("#praise" + id + "").html(shtml);
                    }
                    else {
                        $("#praise" + id + "").html('还没有人点赞');
                    }
                                  
                }
                else {
                    //mui.toast('无数据')
                }
            }, function () {
                dd_toast('接口错误，请联系管理员！', 'error', 0);
            })
        })

    })
}

//筛选 客户根据跟进记录里有的客户进行筛选
function initCust() {
    //debugger;
    if ($('#cust_template').html() != null) {
        var data = {
            Func: "get_cust_list",
            userid: fo_users.join(','),
            guid: userid,
            riqi: riqi,
            is_self_guid: is_self_guid,
            departmentID: page_parmeter.departmentID,
            memmberID: page_parmeter.memmberID
        }
        getajax(url, data, function (json) {

            if (json.result != null) {

                if (json.result.errMsg == "success") {

                    $('#person_sele').html('');

                    $('#tools_flow a:eq(1)').find('span').html('不限' + '<i class="iconfont">&#xe61e;</i>');

                    var retData = json.result.retData;
                    //使用模板的方式进行数据绑定                  
                    $('#person_sele').append(ejs.render($('#cust_template').html(), { retData: retData }));
                    
                }
                else {
                    $('#person_sele').append('<p class="active">不限</p>');
                }
            }
            else {
                $('#person_sele').append('<p class="active">不限</p>');
                //alert(2);
            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }
}

//获取跟进类型
function follow_type() {
    if ($('#s_list_template').html() != null) {
        var data = {
            Func: "get_pub_param",
            pub_title: "跟进类型",
            guid: userid
        }
        getajax(s_url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                $('#s_list').append(ejs.render($('#s_list_template').html(), { retData: retData }));
            }
            else {

            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }
}

//暂时不需要以下代码
//function initlink() {
//    var follow_cust_id = localStorage.getItem("follow_cust_id");
//    if (follow_cust_id != "" && follow_cust_id != null) {
//        var data = {
//            Func: "get_cust_linkman_list",
//            pagesize: 99999999,
//            pageindex: 1,
//            ispage: false,
//            link_users: userid,
//            link_cust_id: follow_cust_id,
//            guid: userid
//        }
//        var np_url = pageurl + "/LinkMan/cust_linkman_handle.ashx";
//        getajax(np_url, data, function (json) {
//            var retLen = 0;
//            if (json.result.errMsg == "success") {
//                retLen = json.result.retData.length;
//                for (var i = 0; i < retLen; i++) {
//                    retData.push({ "text": json.result.retData[i].link_name, "value": json.result.retData[i].link_position, "id": json.result.retData[i].id });
//                }
//            }

//            if (retLen == 0) {
//                dd_toast('暂无联系人！', 'error', 0);
//                picker.hide();
//            }
//        }, function () {
//            dd_toast('接口错误，请联系管理员！', 'error', 0);
//        })

//    }
//}