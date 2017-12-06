/*首页*/
var url = pageurl + "/Statistical/statistic_handle.ashx";
var orgData;
$(function () {
    httpData.getUserInfo();
    if (userid != null ) {
        httpData.weekData();
        httpData.submitData();
    }
})

//初始化数据
var httpData = {
    weekData: function () {
        /*统计数据请求*/
        var data = {
            Func: "get_statistic_today",
            userid: userid,
            username: username,
            guid:userid,
            type: 2
        }
        getajax_async(url, data, function (json) {
          
            if (json != null && json.result != null && json.result.errMsg != null && json.result.errMsg == "success") {
                //debugger;
                var status = json.result.status;
                //设置当前为管理【这里并非指的是管理员】
                localStorage.setItem("role", status)


                var retData = json.result.retData;
                //alert(retData.s_followup_count);
                // alert(retData.s_cust_customer_count);
                $("#cust_customer_count").html(retData.s_cust_customer_count);
                $("#cust_linkman_count").html(retData.s_linkman_count);
                $("#follow_up_count").html(retData.s_followup_count);
                $("#sign_in_count").html(retData.s_sign_count);

             

                //if (retData.is_remin == 1)//如果有需要
                //{
                //    dd.ready(function () {
                //        dd.device.notification.vibrate({
                //            duration: 3000, //震动时间，android可配置 iOS忽略
                //            onSuccess: function (result) {
                //                /*
                //                {}
                //                */
                //            },
                //            onFail: function (err) { }
                //        });
                //    })
                //}
            }
            else {
                $("#cust_customer_count").html('0');
                $("#cust_linkman_count").html('0');
                $("#follow_up_count").html('0');
                $("#sign_in_count").html('0');
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    },
    submitData: function () {
        /*今日事项数据请求*/
        var data = {
            Func: "get_today_matters",
            userid: userid,
            guid: userid,
        }
        getajax_async(url, data, function (json) {
            
            if (json != null && json.result != null && json.result.errMsg != null & json.result.errMsg == "success") {

                var retData = json.result.retData;
                console.log(retData[0]);
                if (retData != null && retData.length > 0) {
                    $('#list').html(ejs.render($('#list_template').html(), { retData: retData }));
                } else {
                    $('#list').html('<div style="width:100%;height:3rem;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 2rem auto;"></div>');
                }

            }
            else {
                $('#list').html('<div class="width:100%;background: #fff url(/images/error.png) no-repeat center;background-size: 2rem auto;height:3rem;">');
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }, getUserInfo: function () {
      
        getajax_async(pageurl1 + "/Login.aspx", { code: localStorage.getItem("code"), mode: "mobile" }, function (json) {
            if (json != null) {
                var retData = json.result.retData;
               
                dd.ready(function () {
                    if (retData == "")//若平台未返回信息跳转错误页面
                    {
                        dd_toast('您没有权限进入该系统,请联系管理员', 'error', 0);
                        dd.biz.navigation.close({
                            onSuccess: function (result) {
                                /*result结构
                                {}
                                */
                            },
                            onFail: function (err) { }
                        })                                       
                    }
                    else {
                        //$("#body_Main").css("display", "inline")
                    }
                });
                localStorage.setItem("username", retData[0].Name);
                username = retData[0].Name;
                localStorage.setItem("userid", retData[0].UniqueNo);
                userid = retData[0].UniqueNo;

                localStorage.setItem("Phone", retData[0].Phone);
                localStorage.setItem("orgData", JSON.stringify(json.result.orgData));

                orgData = JSON.stringify(json.result.orgData);
                //userid = localStorage.getItem("userid");
                //username = localStorage.getItem("username");

                //var roots = ['D59CD2E5-2BA8-4803-A09F-C342B5D658EC', 'E197ABC0-E071-4DAA-9299-9FB316720B09', 'F4C7BE25-6482-4028-90E3-3BC5D8268338', '2EB80A19-E6D4-489A-82E0-E7FED8908109'];
                //分别为刘云诚、彭会平、姜玲娜、黄泳绮
               
                $("#username").html(retData[0].Name);

                if (userid == null || userid == undefined) {
                    userid = localStorage.getItem("userid")
                }
                //var isHighLimit = false;
                //if (userid == 'D59CD2E5-2BA8-4803-A09F-C342B5D658EC' || userid == 'E197ABC0-E071-4DAA-9299-9FB316720B09' || userid == 'F4C7BE25-6482-4028-90E3-3BC5D8268338' || userid == '2EB80A19-E6D4-489A-82E0-E7FED8908109')
                //{
                //    isHighLimit = true;
                //}
                /*统计数据请求*/
                var data = {
                    Func: "DataInit",
                    cust_users: userid,
                    orgData: orgData,
                    guid: userid,
                }


                getajax_async(url, data, function (json) {

                    httpData.weekData();
                    httpData.submitData();
                });

            }
        }, function () {
                dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
            })

            if (username != null || username != undefined) {
                $("#username").html(username);
            }
        }   
}

