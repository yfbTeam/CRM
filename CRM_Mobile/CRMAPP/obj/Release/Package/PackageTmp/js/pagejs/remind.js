/*提醒*/
var url = pageurl + "/Remind/remind_handle.ashx";

$(function () {
    //切换开关
    mui('.mui-content .mui-switch').each(function () { //循环所有toggle
        mui(this).switch();
        this.addEventListener('toggle', function (event) {
            $("input[name='rem_isopen']").val(Number(event.detail.isActive));
        });
    });


})

function initdata() {
    if ($('#list_template').html() != null) {
        var data = {
            Func: "get_remind_list",
            PageSize: pagesize,
            PageIndex: pageindex,
            ispage: true,
            rem_userid: userid,
            guid: userid
        }
        getajax(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData.PagedData;
                RowCount = json.result.retData.RowCount;
                if (pageindex == 1) {
                    $('#list').empty();
                }
                //当列表数据过少是禁用下拉刷新，上拉加载更多
                if (retData!=null && retData!='' && retData.length===0) {
                    mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                }
                $('#list').parent().css({ 'height': 'auto' });
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                nomessage("list");
                deleteRemind();
                toggle();
                detail();
            }
            else {
                nomessage("list");
            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }
}

//提交
function submit() {
    if ($("#submit_data").attr("fls") == 'false') {
        $("#submit_data").attr("fls", 'true');
        if (validateForm($("textarea,input[type='hidden'],input[type='text'],select")) == "0") {
            var data = getForm($("input[type='hidden'],input[type='text'],select,textarea"));
            getajax(url, data, function (json) {
                if (json.result.errMsg == "success") {
                    mui.back();
                    dd_toast('保存成功！', 'success', 0);
                }
                else {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('保存失败！', 'error', 0);
                }
            }, function () {
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

function toggle() {
    mui('.mui-content .mui-switch').each(function () { //循环所有toggle
        mui(this).switch();
        var id = $(this).children("input[type='hidden']").val();
        if (id != undefined) {
            this.addEventListener('toggle', function (event) {
                var data = {
                    Func: "update_remind_isopen",
                    rem_isopen: Number(event.detail.isActive),
                    id: id,
                    guid: userid
                }
                getajax(url, data, function (json) {
                }, function () {
                    dd_toast('接口错误，请联系管理员！', 'error', 0);
                })
            });
        }
    });
}


function deleteRemind() {
    var btnArray = ['确认', '取消'];
    $('.mui-btn-red').each(function () {
        this.addEventListener('tap', function (event) {
            var ele = this;
            mui.confirm('确认删除该条记录？', '', btnArray, function (e) {
                if (e.index == 0) {
                    var id = $(ele).parent().parent().children("input[name='rem_id']").val();
                    $(ele).parent().parent().remove();
                    var data = {
                        Func: "update_remind_isdelete",
                        id: id,
                        rem_isdelete: 1,
                        guid: userid
                    }
                    getajax(url, data, function (json) {
                        if (json.result.errMsg == "success") {
                           
                        }
                    }, function () {
                        dd_toast('接口错误，请联系管理员！', 'error', 0);
                    })
                }
            });

        });
    });
}

//详细
function detail() {
    $('.mui-slider-handle').on('tap', function () {
        var id = $(this).children("input[type='hidden']").val();
        var href = "SeeReminder.html?fdfd=dsjj&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random() + "&id=" + id;
        showPreloader();
        openwindow(href, href, 'fade-in');
    })
}

function info(id) {
    var data = {
        Func: "get_remind_info",
        id: id,
        guid: userid
    }
    getajax(pageurl + '/Remind/remind_handle.ashx', data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData[0];
            $('#question').val(retData.rem_content);
            $('#select_time').val(retData.rem_date);
            $("#id").val(retData.id);
            if (retData.rem_isopen == 1) {
                $("#tixing").addClass(" mui-active");
            }
            //$('#question').val();
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}