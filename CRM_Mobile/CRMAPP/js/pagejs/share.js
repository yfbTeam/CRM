/*统计*/
var url = pageurl + "/Share/circle_share_handle.ashx";
var type = '';
//分享圈进入
$(function () {
    dd_setRightNone();
    mui.previewImage();
    page_parmeter.pageindex = 1;
    initdata(page_parmeter);
    hidePreloader();

})

function initPraise() {
    //点赞
    $('#list li').each(function () {
        var _this = this;
        $(_this).on('tap', '.dianzan', function () {
            var id = $(this).children('input[type="hidden"]').val();
            if ($("#ispraise" + id + "").is('.active')) {
                $("#ispraise" + id + "").removeClass('active');
            }
            else {
                $("#ispraise" + id + "").addClass('active');
            }
            var data1 = {
                Func: "edit_praise",
                id: 0,
                praise_table_id: id,
                praise_userid: userid,
                praise_username: username,
                praise_type: 2,
                guid: userid
            }
            getajax(pageurl + '/Share/praise_handle.ashx', data1, function (json1) {
                if (json1.result.errMsg == "success") {
                    var retData = json1.result.retData;
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
                dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
            })
        })

    })
}
function initdata(page_parmeter) {
    if ($('#list_template').html() != null) {
        //alert($("#s_startdate").val());
        var data = {
            Func: "get_share_list",
            PageSize: page_parmeter.pagesize,
            PageIndex: page_parmeter.pageindex,
            ispage: true,
            userid: userid,
            guid: userid,
            type: page_parmeter.type
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                // debugger;
                var retData = json.result.retData.PagedData;
                page_parmeter.RowCount = json.result.retData.RowCount;
                if (page_parmeter.pageindex == 1) {
                    $('#list').empty();
                }
                //当列表数据过少是禁用下拉刷新，上拉加载更多
                if (retData != null && retData != '' && retData.length <= 0) {
                    mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                }
                $('#list').parent().css({ 'height': 'auto' });
                //alert(JSON.stringify(json))
                for (var i = 0; i < retData.length; i++) {
                    //alert(retData[i].report_info.report_content)
                    var type = retData[i].report_info.report_type;
                    var v1 = retData[i].report_info.report_content;
                    var v2 = retData[i].report_info.report_plan;
                    switch (type) {
                        case 1:
                            retData[i].report_info.report_content = '今日总结：' + v1;
                            retData[i].report_info.report_plan = '明日计划：' + v2;
                            break;
                        case 2:
                            retData[i].report_info.report_content = '本周总结：' + v1;
                            retData[i].report_info.report_plan = '下周计划：' + v2;
                            break;
                        case 3:
                            retData[i].report_info.report_content = '本月总结：' + v1;
                            retData[i].report_info.report_plan = '下月计划：' + v2;
                            break;
                        default:

                    }

                }
                //alert(JSON.stringify(retData))
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));

                $('.content').on('tap', function () {

                    var id = $(this).children('input[type="hidden"]').val();
                    var href = '/WorkReport/CheckReport.html?id=' + id + "&dd=sdk&dd_nav_bgcolor=FF73C2FF&number=" + Math.random();
                    openwindow(href, href, 'slide-in-right');
                })

                nomessage("list");
                initPraise();
                com_save();
            }
            else {
                nomessage("list");
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }
}

var ff = 0;
function com_save() {
    $(".com").on('tap', function () {
        var com_table_id = $(this).children('input[type="hidden"]').val();
        input_plain(0, com_table_id, "评论一下吧！")
    })
    $(".com3").on('tap', function () {
        var com_table_id = $(this).children('input[type="hidden"]').val();
        input_plain(0, com_table_id, "评论一下吧！")
    })
    $(".com_2").on('tap', function () {
        ff++;
        var com_table_id = $(this).children('input[name="table_id"]').val();
        var com_parent_id = $(this).children('input[name="parent_id"]').val();

        var com_id = $(this).children('input[name="com_id"]').val();
        var com_userid = $(this).children('input[name="com_userid"]').val();
        if (com_userid == userid) {
            if (ff <= 1) {
                dd.ready(function () {
                    dd.device.notification.actionSheet({
                        cancelButton: '取消', //取消按钮文本
                        otherButtons: ["删除"],
                        onSuccess: function (result) {
                            ff = 0;
                            if (result.buttonIndex != -1) {
                                if (result.buttonIndex == 0) {
                                    var data1 = {
                                        Func: "update_comment_isdelete",
                                        id: com_id,
                                        com_isdelete: 1,
                                        guid: userid
                                    }
                                    $("#item1_" + com_id).remove();
                                    getajax(pageurl + "/Share/comment_handle.ashx", data1, function (json1) {

                                    }, function () {
                                        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
                                    })
                                }

                            }
                        },
                        onFail: function (err) { }
                    })
                });
            }
        }
        else {
            input_plain(com_id, com_table_id, '回复一下吧！')
        }
    })

    $(".com4").on('tap', function () {
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
                                    dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
                                })
                            }

                        }
                    },
                    onFail: function (err) { }
                })
            });
        }
    })
}

//添加评论
function input_plain(com_parent_id, com_table_id, text) {
   
   
        dd.ui.input.plain({
            placeholder: text, //占位符
            text: '', //默认填充文本
            onSuccess: function (data) {
                var data = {
                    Func: "edit_comment",
                    id: 0,
                    com_table_id: com_table_id,
                    com_parent_id: com_parent_id,
                    com_content: data.text,
                    com_type: 3,
                    com_userid: userid,
                    com_username: username,
                    guid: userid
                }
                
                if (text == '' || text == '评论一下吧！' || text == null || text == undefined) {

                    dd_toast('请输入内容！', 'error', 0);
                    return;
                }
              
               
                getajax(pageurl + '/Share/comment_handle.ashx', data, function (json) {
                    if (json.result.errMsg == "success") {
                        //dd_toast('发送成功！', 'success', 0);
                        getcomment(com_table_id);
                    }
                    else {
                        dd_toast('发送失败！', 'error', 0);
                    }
                }, function () {
                    dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
                })
            },
            onFail: function () {

            }
        })
    
}

function getcomment(id) {
    var data = {
        Func: "get_comment",
        id: id,
        type: 3,
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
                        com_list.push('<p class="com_2" id="item1_' + retData[i].id + '"><input type="hidden" name="com_userid" value=' + retData[i].com_userid + ' /><input type="hidden" name="parent_id" value="' + retData[i].com_parent_id + '" /><input type="hidden" name="com_id" value="' + retData[i].id + '" /><input type="hidden" name="table_id" value="' + retData[i].com_table_id + '" /><span>' + retData[i].com_username + '：</span>' + retData[i].com_content + '</p>');
                        for (var j = 0; j < len; j++) {
                            if (retData[j].com_parent_id == retData[i].id && retData[j].com_parent_id != "0") {
                                com_list.push('<p class="com4" id="item2_' + retData[j].id + '"><input type="hidden" name="com_userid" value="' + retData[j].com_userid + '" /><input type="hidden" name="parent_id" value="' + retData[j].id + '" /><span> ' + retData[j].com_username + ' 回复 ' + retData[i].com_username + '：</span>' + retData[j].com_content + '</p>');
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
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}

