/*工作计划*/
var url = pageurl + "/LinkMan/cust_linkman_handle.ashx";

//指定客户的联系人ID【如果有url传值方式传值，接收，并设置为筛选条件】
var link_cust_id = getQueryString("cust_id");


//初始化
function initdata(page_parmeter) {
    var link_name = $("#link_name").val();
    if ($('#list_template').html() != null) {
        var data = {
            Func: "get_cust_linkman_list",
            wp_userid: userid,
            pagesize: page_parmeter.pagesize,
            pageindex: page_parmeter.pageindex,
            ispage: true,
            link_users: userid,
            link_cust_id:link_cust_id,
            guid: userid,
            link_name: link_name,
            type: page_parmeter.type,
            departmentID: page_parmeter.departmentID,
            memmberID: page_parmeter.memmberID
        }
        getajax_async(url, data, function (json) {
            //debugger;
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
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                nomessage("list");
                detail();
                deletelink();
            } else {
                $('#list').parent().css({ 'height': '100%' });
                nomessage("list");
            }
        }, function () {
            errormessage('list');
        })
    }
}

//删除
var ff = 0;//删除出现多个屏蔽
function deletelink() {

    mui('.mui-btn-red').each(function () {
        this.addEventListener('tap', function (event) {
            ff++;
            if (ff <= 1) {
                var ele = this;
                var btnArray = ['确认', '取消'];
                mui.confirm('确认删除该条记录？', '', btnArray, function (e) {
                    if (e.index == 0) {
                        var id = $(ele).children("input[type='hidden']").val();
                       
                        var data = {
                            Func: "update_cust_linkman_isdelete",
                            id: id,
                            link_isdelete: 1,
                            guid: userid
                        }
                        getajax(url, data, function (json) {
                            ff = 0;
                            if (json.result.errMsg == "success") {
                                page_parmeter.pageindex = 1;
                                initdata(page_parmeter);
                                $(ele).parent().parent().remove();
                            }
                            else if (json.result.errMsg == "failed")
                            {
                                dd_toast(json.result.retData, 'error', 0);
                            }
                        }, function () {
                            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
                        })
                    }
                    else {
                        ff = 0;
                    }
                });
            }

        });
    });
}

//进入详细
function detail() {
    mui('.mui-slider-handle').each(function () { //循环所有toggle
        var link_id = $(this).children("input[name='link_id']").val();
        var cust_id = $(this).children("input[name='cust_id']").val();
        this.addEventListener('tap', function (event) {
            var href = "LinkMan_detail.html?id=" + link_id + "&cust_id=" + cust_id + "&number=" + Math.random() + "&djj=dskj&dd_nav_bgcolor=FF6CB1FF";
            //设置场景
            save_history();
            openwindow(href, href, 'slide-in-right');
        });
    });
}
//详细
function dataInfo(id) {
    if ($('#info_template').html() != null) {
        var data = {
            Func: "get_cust_linkman_info",
            id: id,
            guid: userid
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                //debugger;
                $('#info_list').append(ejs.render($('#info_template').html(), { retData: retData }));
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }
}
//详情【编辑联系人】
function info(id) {
    var data = {
        Func: "get_cust_linkman_info",
        id: id,
        guid: userid
    }
    getajax_async(url, data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData;
            $("input[name='id']").val(retData.id);
            $("#link_level").val(retData.link_level);
            $("input[name='link_cust_name']").val(retData.link_cust_name);
            $("input[name='link_sex']").val(retData.link_sex);
            $("input[name='link_name']").val(retData.link_name);
            $("input[name='link_department']").val(retData.link_department);
            $("input[name='link_birthday']").val(retData.link_birthday);
            $("input[name='link_position']").val(retData.link_position);
            $("input[name='link_phonenumber']").val(retData.link_phonenumber);
            //$("#link_phonenumber").attr("href", 'tel:' + retData.link_phonenumber);
            if (retData.link_telephone != "") {
                var html = '<div class="input-row" style="width:5.35rem;"><label for=""  class="fl">电话</label><div class="input fr">';
                html += '<span class="text">' + retData.link_telephone + '</span>';
                html += '</div></div>';
                $("#link_telephone_div").html(html);
            }

            $("input[name='link_email']").val(retData.link_email);
            $("input[name='link_remark']").val(retData.link_remark);
            $("input[name='link_remark']").val(retData.link_remark);
            $("#cust_name").val(retData.link_cust_name);
            if (retData.link_sex == "男") {
                $("#nan").attr("checked", "checked");
            }
            else {
                $("#nv").attr("checked", "checked");
            }

            $("#cust_parent_id").val(retData.cust_parentname == "" ? "-" : retData.cust_parentname);
            $("#link").val(retData.link_levelName);
            $("#cust_type").val(retData.cust_type);
            $("#link_cust_id").val(retData.link_cust_id);
        }
        else {
            //mui.toast('无数据')
        }
    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}

//提交
function submit() {
    if ($("#submit_data").attr("fls") == 'false') {
        $("#submit_data").attr("fls", 'true');
        if ($("#link_cust_id").val() == "") {
            $("#link_cust_id").val(p_id);
        }
        $("#link_sex").val($("input[name='sex']:checked").val());
        if (validateForm($("input[type='hidden'],input[type='text'],select,textarea")) == "0") {
            var data = getForm($("input[type='hidden'],input[type='text'],select,textarea"));
            getajax(url, data, function (json) {
                if (json.result.errMsg == "success") {
                    localStorage.setItem('link_id', json.result.retData);
                    localStorage.setItem('link_name', $("input[name='link_name']").val());
                    dd_toast('保存成功！', 'success', 0);
                    mui.back();                  
                }
                else if(json.result.errMsg == "failed")
                {
                    dd_toast(json.result.retData, 'error', 0);
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


//获取客户类型
function getcust_type(pub_title) {
    var data = {
        Func: "get_pub_param",
        pub_title: pub_title,
        guid: userid
    }
    getajax_async(pageurl + "/PubParam/pub_param_handle.ashx", data, function (json) {
        if (json.result.errMsg == "success") {
            //retData = json.result;

            var retLen = json.result.retData.length;
            for (var i = 0; i < retLen; i++) {
                cust_level_retData.push({ "text": json.result.retData[i].pub_title, "value": json.result.retData[i].pub_value });
            }
        }
        else {

        }
    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}


//获取上级父客户
function getcust_parentname(cust_parent_id) {
    var data = {
        Func: "get_cust_customer_parent",
        cust_user: userid,
        guid: userid
    }
    getajax_async(pageurl + "/Custom/cust_customer_handle.ashx", data, function (json) {

        if (json.result.errMsg == "success") {
            var retLen = json.result.retData.length;
            if (retLen > 0) {
                for (var i = 0; i < retLen; i++) {
                    cust_name.push({ "text": json.result.retData[i].cust_name, "value": json.result.retData[i].id });
                }
            }
            else {
                cust_name = [];
                cust_name.push({ "text": "请选择客户名称", "value": '' });
            }

        }
        else {
            cust_name = [];
            cust_name.push({ "text": "请选择客户名称", "value": '' });
        }
    }, function () {
        dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
    })
}