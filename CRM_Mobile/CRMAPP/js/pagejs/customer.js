/*客户信息*/
var url = pageurl + "/Custom/cust_customer_handle.ashx";

//初始化数据【CustomerInfo.html】
function initdata(page_parmeter) {
    //debugger;
    var cust_name = $("#cust_name").val();
    if ($('#list_template').html() != null) {
        var data = {
            Func: "get_cust_customer_list",
            cust_users: userid,
            pagesize: page_parmeter.pagesize,
            pageindex: page_parmeter.pageindex,
            ispage: true,
            guid: userid,
            cust_name: cust_name,
            type: page_parmeter.type,
            departmentID: page_parmeter.departmentID,
            memmberID: page_parmeter.memmberID
        }
        getajax_async(url, data, function (json) {

            if (json.result.errMsg == "success") {
                //debugger;
                var retData = json.result.retData.PagedData;
                page_parmeter.RowCount = json.result.retData.RowCount;
                $(".count").html(page_parmeter.RowCount + "条");
                if (page_parmeter.pageindex == 1) {
                    $('#list').empty();
                }
                //debugger;
                //当列表数据过少是禁用下拉刷新，上拉加载更多
                if (retData != null && retData != '' && retData.length <= 0) {
                    mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                }
                $('#list').parent().css({ 'height': 'auto' });
                $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                nomessage("list");
                detail();
                deletecust();

            }
            else {
                $('#list').parent().css({ 'height': '100%' });
                nomessage("list");

            }
        })
    }
}


//进入详细【CustomerInfo.html】
function detail() {
    mui('.mui-slider-handle').each(function () { //循环所有toggle
        var id = $(this).children("input[type='hidden']").val();
        this.addEventListener('tap', function (event) {
            showPreloader();
            var href = "SeeInfo.html?id=" + id + "&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random();
            //设置场景
            save_history();

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

//删除【CustomerInfo.html】
var ff = 0;//删除出现多个屏蔽
function deletecust() {
    var btnArray = ['确认', '取消'];
    mui('.mui-btn-red').each(function () {
        this.addEventListener('tap', function (event) {
            ff++;
            if (ff <= 1) {
                var ele = this;
                mui.confirm('确认删除该条记录？', '', btnArray, function (e) {
                    if (e.index == 0) {
                        var id = $(ele).parent().parent().children("input[name='cust_id']").val();

                        var data = {
                            Func: "update_cust_customer_isdelete",
                            id: id,
                            cust_users: userid,
                            cust_isdelete: 1,
                            guid: userid
                        }
                        getajax(url, data, function (json) {
                            if (json.result.errMsg == "success") {
                                page_parmeter.pageindex = 1;
                                initdata(page_parmeter);
                                $(ele).parent().parent().remove();
                                ff = 0;
                            }
                            else if (json.result.errMsg == "failed") {
                                dd_toast(json.result.retData, 'error', 0);
                                ff = 0;
                            }
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


var sign_X;
var sign_Y;
//客户编辑有效使用【SeeCustomer.html】
function info(id) {
    var data = {
        Func: "get_cust_customer_info",
        id: id,
        guid: userid
    }
    getajax_async(url, data, function (json) {
        var role = localStorage["role"];
        if (role == "Super_Admin") {
            $("#cust_category_div").css("display", "block");
        }
        if (json.result.errMsg == "success") {
            
            var retData = json.result.retData;         
            sign_X = retData.cust_x;
            sign_Y = retData.cust_y;
            //alert(JSON.stringify(retData))

            $("#cust_id").val(retData.id);
            $("input[name='cust_parent_id']").val(retData.cust_parent_id);
            $("input[name='cust_type']").val(retData.cust_type_value);

            $("input[name='cust_category']").val(retData.cust_category_value);
            $("input[name='cust_level']").val(retData.cust_level_value);
            $("input[name='cust_name']").val(retData.cust_name);
            $("#cust_parent_id_1").val(retData.parent_customer_name == "" ? "-" : retData.parent_customer_name);
            $("#cust_parent_id").val(retData.cust_parent_id);
            //有地址就隐藏请选择的项
            if (retData.cust_address != '' && retData.cust_address != null) {
                $("#address_display").html(retData.cust_address);
                $("#cust_address").val(retData.cust_address);
                $("#cust_type_2").hide();
            }
            //alert(retData.cust_usersname)
            //总监需要看到自己的指派人
            var role = localStorage["role"];
            if (role == "Common_Admin"||role =='Super_Admin') {
                //赋予指派负责人的查看
                $('#leader').val(retData.cust_users);
                $('#leader_names').val(retData.cust_usersname);                
            }

            $("#cust_type").val(retData.cust_type);
            $("#cust_level").val(retData.cust_level);
            $("#cust_category").val(retData.cust_category);
            $("#links").html(retData.cust_links);
            $("#link_ids").val(retData.linkids);
            if (retData.linkids != "" && retData.linkids != undefined) {
                link_name.push(retData.cust_links);
                links_id.push(retData.linkids);
            }
            $("#link_cust_name").val(retData.cust_name);

            //$("#cust_address").innerText = retData.cust_address;
        }
        else {
            //mui.toast('无数据')
        }
    })
}

//详细
function dataInfo(id) {
    var id = getparam("id");
    if ($('#info_template').html() != null) {
        var data = {
            Func: "get_cust_customer_info",
            id: id,
            guid: userid
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                //debugger;
                $('#info_list').append(ejs.render($('#info_template').html(), { retData: retData }));
            }
        })
    }
}

//提交【保存客户】
function submit() {
    var address = $("#address_display").html();
    if (address == '' || address == null) {
        dd_toast('请指定客户地址：', 'error', 0);
    }

    else if ($("#submit_data").attr("fls") == 'false') {
        $("#submit_data").attr("fls", 'true');
        if (validateForm($("#cust_customer input[type='hidden'],#cust_customer input[type='text'],#cust_customer select,#cust_customer textarea")) == "0") {
            var data = getForm($("#cust_customer input[type='hidden'],#cust_customer input[type='text'],#cust_customer select,#cust_customer textarea"));
            //alert(JSON.stringify(data));
            getajax(url, data, function (json) {
                if (json.result.errMsg == "success") {
                   
                    dd_toast('客户保存成功！', 'success', 0);
                    mui.back();

                }
                else if (json.result.errMsg == "error_have_cust") {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('客户已存在,当前持有人：' + json.result.retData, 'error', 0);
                }
                else {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('保存失败！', 'error', 0);
                }
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

//保存客户（添加联系人之前进行操作）
function submit_customer_for_addLinkman() {
  
             
        if (validateForm($("#cust_customer input[type='hidden'],#cust_customer input[type='text'],#cust_customer select,#cust_customer textarea")) == "0") {
            var address = $("#address_display").html();
            if (address == '' || address == null) {
                dd_toast('请指定客户地址：', 'error', 0);
                return;
            }

            var data = getForm($("#cust_customer input[type='hidden'],#cust_customer input[type='text'],#cust_customer select,#cust_customer textarea"));
            //alert(JSON.stringify(data));
            getajax(url, data, function (json) {
                debugger;
                if (json.result.errMsg == "success") {                                                     
                    cust_id_return_from_service = json.result.retData;
                    $('#cust_id').val(cust_id_return_from_service);
                    dd_toast('客户保存成功！', 'success', 0);

                    $('#submit_data').html("返回到客户列表");
                    //客户保存
                    $('#submit_data').off('tap', submit_customer_for_addLinkman);
                    $('#submit_data').on('tap', function () {
                        mui.back();
                    });
                }
                else if (json.result.errMsg == "error_have_cust") {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('客户已存在,当前持有人：' + json.result.retData, 'error', 0);
                }
                else {
                    $("#submit_data").attr("fls", 'false');
                    dd_toast('保存失败！', 'error', 0);
                }
            })
        }
        else {
            $("#submit_data").attr("fls", 'false');
           
        }  
}


//获取客户类型【NewCustomer.html】
function getcust_type(pub_title, flag) {
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
                if (flag == 1) {
                    cusy_type_retData.push({ "text": json.result.retData[i].pub_title, "value": json.result.retData[i].pub_value });
                }
                else {
                    cusy_level_retData.push({ "text": json.result.retData[i].pub_title, "value": json.result.retData[i].pub_value });
                }
            }

        }
        else {

        }
    })
}

//获取成员【NewCustomer.html】
function getmemmber_type() {
    var data = {
        Func: "GetMemmber",
        guid: userid
    }
    getajax_async(pageurl + "/Statistical/statistic_handle.ashx", data, function (json) {

        if (json.result.errMsg == "success") {
            //retData = json.result;
            //alert(JSON.stringify(json))
            //alert(JSON.stringify(json.result.retData));
            var retLen = json.result.retData.length;

            for (var i = 0; i < retLen; i++) {

                cust_leader_retData.push({ "text": json.result.retData[i].Name, "value": json.result.retData[i].UniqueNo });

                //alert(JSON.stringify(json.result.retData[i]))
            }
            //$('#leader_names_div').append(ejs.render($('#cust_relate_template').html(), { retData: cust_leader_retData }));
        }
        else {

        }
    })
}

//获取父客户【NewCustomer.html】
function getcust_parent() {
    var data = {
        Func: "get_cust_customer_parent",
        cust_user: userid,
        guid: userid
    }
    getajax_async(pageurl + "/Custom/cust_customer_handle.ashx", data, function (json) {
        if (json.result.errMsg == "success") {
            //retData = json.result;

            var retLen = json.result.retData.length;
            for (var i = 0; i < retLen; i++) {
                cusy_parent_retData.push({ "text": json.result.retData[i].cust_name, "value": json.result.retData[i].id });
            }
        }
        else {

        }
    })
}


//指派联系人（将指定的客户，和客户下的某些联系人指派给某个用户）
function relate_customer_linkmans(cust_id, linkman_ids, user_names, user_ids) {
    var data = {
        Func: "relate_customer_linkmans",
        guid: userid,
        cust_id: cust_id,
        linkman_ids: linkman_ids,
        user_names: user_names,
        user_ids: user_ids
    }
    getajax(pageurl + "/Custom/cust_customer_handle.ashx", data, function (json) {
        if (json.result.errMsg == "success") {


        }
        else {

        }
    })
}




