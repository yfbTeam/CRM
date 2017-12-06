var url = pageurl + "/Remind/remind_setting_handle.ashx";
$(function () {
    dd_setRightNone();
    mui('body').on('shown', '.mui-popover', function (e) {
        //console.log('shown', e.detail.id);//detail为当前popover元素
    });
    mui('body').on('hidden', '.mui-popover', function (e) {
        //console.log('hidden', e.detail.id);//detail为当前popover元素
    });

    mui('body').on('tap', '.mui-popover-action li>a', function () {
        var a = this,
            parent;
        //根据点击按钮，反推当前是哪个actionsheet
        for (parent = a.parentNode; parent != document.body; parent = parent.parentNode) {
            if (parent.classList.contains('mui-popover-action')) {
                break;
            }
        }
        //关闭actionsheet
        mui('#' + parent.id).popover('toggle');
        text = a.innerHTML;
    })
    initdata();

    $("#plan a").on('tap', function () {
        submitData($(this).html(), 1);
    })
    $("#follow a").on('tap', function () {
        submitData($(this).html(), 2);
    })
    $("#birthday a").on('tap', function () {
        submitData($(this).html(), 3);
    })
    hidePreloader();
})

function submitData(remind_remark, remind_type) {
    if ($("#submit_data").attr("fls") == 'false') {
        $("#submit_data").attr("fls", 'true');
        var data = {
            Func: "edit_remind_setting",
            id: 1,
            remind_userid: userid,
            remind_type: remind_type,
            remind_remark: remind_remark,
            guid: userid
        }

        getajax(url, data, function (json) {
            if (json.result.errMsg == "success") {
                dd_toast('保存成功！', 'success', 0);
                initdata();
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
        dd_toast('不可重复保存', 'error', 0);
    }
}

//初始化数据
function initdata() {
    var data = {
        Func: "get_remind_setting_info",
        remind_userid: userid,
        guid: userid
    }
    getajax(url, data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;
            for (var i = 0; i < retData.length; i++) {
                $("#list" + retData[i].remind_type).html(retData[i].remind_remark);
                $("#hid" + retData[i].remind_type).html(retData[i].id);
            }
        }
        else {

        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}