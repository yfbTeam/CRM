<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewWorkPlan.aspx.cs" Inherits="CRMAPP.WorkPlan.NewWorkPlan" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>新增计划</title>
    <link href="/css/mui.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/css/iconfont.css" />
    <link rel="stylesheet" type="text/css" href="/css/mobiscroll.custom-3.0.0-beta2.min.css" />
    <link rel="stylesheet" href="/css/style.css?dds=fdk" />
    <link href="/css/mui.picker.css" rel="stylesheet" />
    <link href="/css/mui.poppicker.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script src="/js/mui.min.js"></script>
    <script type="text/javascript" src="https://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>
    <script src="/js/public.js?jdj=fgf"></script>
    <script src="/js/mobiscroll.custom-3.0.0-beta2.min.js"></script>
    <script type="text/javascript" src="/js/ejs.min.js"></script>

    <script type="text/javascript">
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/dd_navigation.js?did=gf&number=" + Math.random() + "'></s" + "cript>");
    </script>
    <style>
        .mes_lists li .mes_wrap .mes_deta .img_wrap {
            height: auto;
        }
    </style>


    <script src="/js/mui.picker.min.js"></script>
    <script src="/js/mui.picker.js"></script>
    <script src="/js/mui.poppicker.js"></script>
    <style>
        #link_name {
            border: none;
            top: 0;
            height: 0.4rem;
            display: block;
            padding: 0;
            line-height: 0.4rem;
            color: #5d6e7a;
            text-align: left;
            font-size: 0.28rem;
            font-family: "microsoft yahei";
        }
    </style>
</head>
<body>
    <header class="mui-bar mui-bar-nav Header bggreen">
        <a class="mui-action-back mui-icon mui-icon-left-nav mui-pull-left"></a>
        <h1 class="mui-title">新增计划</h1>
        <span id="submit" class="mui-pull-right header_button">保存</span>
    </header>
    <div class="mui-content" style="padding-bottom: 1.1rem;">
        <input type="hidden" name="Func" value="edit_workplan">
        <input type="hidden" name="id" value="0">
        <input type="hidden" name="wp_userid" id="wp_userid" fl="用户id" value="">
        <input type="hidden" name="guid" id="guid" fl="唯一标识" value="">
        <input type="hidden" name="wp_username" id="wp_username" fl="用户名" value="">
        <input type="hidden" name="wp_cust_id" id="wp_cust_id" fl="客户名称" value="">
        <input type="hidden" name="wp_link_id" id="wp_link_id" fl="联系人" value="">
        <input type="hidden" name="wp_status" value="0">
        <!--<input type="hidden" name="wp_plandate" value="2016-12-08">-->
        <input type="hidden" name="picture" id="picture" value="">
       
        <section class="section_module mt20">
            <form class="mui-input-group">
                <div class="input-row clearfix">
                    <div class="icon fl" style="background: #92dd99;">
                        <i class="iconfont">&#xe626;</i>
                    </div>
                    <label for="" class="fl">
                        开始时间
                    </label>
                    <div class="input fr">
                        <input type="text" name="wp_plandate" fl="开始时间" isrequired="true" id="s_startdate" value="" class="text select" placeholder="请选择开始时间[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix">
                    <div class="icon fl" style="background: #92dd99;">
                        <i class="iconfont">&#xe626;</i>
                    </div>
                    <label for="" class="fl">
                        结束时间
                    </label>
                    <div class="input fr">
                        <input type="text" name="wp_endplandate" fl="开始时间" isrequired="true" id="s_enddate" value="" class="text select" placeholder="请选择结束时间[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="row">
                    <textarea id='question' name="wp_content" fl="计划内容" isrequired="true" class="mui-input-clear question" placeholder="输入计划内容[必填]"></textarea>
                </div>
                <div class="camera-area">
                    <div class="clearfix">
                        <div class="imgicon fl">
                        </div>
                        <div class="fl wen">
                            添加图片
                        </div>
                    </div>
                </div>
                <ul class="mes_lists" style="display: none">
                    <li>
                        <div class="mes_wrap clearfix">
                            <div class="mes_deta">
                                <div class="img_wrap">
                                    <div class="img clearfix" id="img">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </form>
        </section>
        <section class="section_module mt20">
            <div class="mui-input-group">
                <div class="clearfix remind">
                    <div class="icon bgqiangreen fl">
                        <i class="iconfont">&#xe628;</i>
                    </div>
                    <div class="fl">
                        <div class="input-row clearfix" style="width: 5.35rem;">
                            <label for="" class="fl">
                                提醒
                            </label>
                            <div id="tixing" class="mui-switch mui-switch-mini fr mui-switch-green">
                                <div class="mui-switch-handle"></div>
                            </div>
                        </div>
                        <div class="input-row clearfix" style="width: 5.35rem;">
                            <label for="" class="fl">
                                提醒时间
                            </label>
                            <div class="input fr">
                                <input type="text" name="wp_reminddate" fl="提醒时间" readonly="readonly" id="select_time" value="" class="text select" placeholder="请选择时间" />
                                <i class="iconfont">&#xe64c;</i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <div class="save-btn">
        <span onclick="submit()" id="submit_data" fls="false" class="bggreen">保存</span>
    </div>
</body>

</html>
<script src="../js/mui.zoom.js"></script>
<script src="../js/mui.previewimage.js"></script>
<script type="text/javascript">
    document.write("<s" + "cript type='text/javascript' src='/js/pagejs/new_workplan.js?fddf=iii&number=" + Math.random() + "'></s" + "cript>");
</script>

<script>
    //工作计划可刷新
    localStorage.setItem("WorkPlan_needfresh", true);
    mui('.mui-content .mui-switch').each(function () { //循环所有toggle
        this.addEventListener('toggle', function (event) {
            //alert(Number(event.detail.isActive));
            $("input[name='wp_status']").val(Number(event.detail.isActive));
            if (Number(event.detail.isActive) == 1) {
                $("#select_time").attr('isrequired', 'true');
            }
            else {
                $("#select_time").attr('isrequired', 'false');
            }
        });
    });
    dd_setRight_save('保存');
    /*若存在图片，加加载图片*/
    var p = localStorage.getItem("pic");
    if (p != null && p != "") {
        pics.push(p);
        var pic_len = p.split(',');
        $(".mes_lists").show();
        for (var i = 0; i < pic_len.length; i++) {

            $("#img").append('<span><img src="' + pic_len[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
        }
    }

    //var cust_customer = document.querySelector('#cust_customer');
    //if (cust_customer != null) {
    //    cust_customer.addEventListener('tap', function () {
    //        var id = this.id;
    //        localStorage.removeItem("wp_cust_name");
    //        localStorage.removeItem("wp_cust_id");
    //        openwindow(id, "/Custom/SelectCustom.html?page=1&type=1&number=" + Math.random() + "&dd_nav_bgcolor=FF3CCDAB", "slide-in-right")
    //    });
    //}

    //客户名称
    //var wp_cust_name = localStorage.getItem("wp_cust_name");
    //if (wp_cust_name != null && wp_cust_name != undefined) {
    //    if ($("#cust_customer").val() != wp_cust_name) {//如果选择的
    //        $("#link_name").val("");
    //        $("#link_position").html("");
    //        $("#wp_link_id").val("");
    //    }
    //    $("#cust_customer").val(wp_cust_name);
    //    $("#wp_cust_id").val(localStorage.getItem("wp_cust_id"));
    //}

    //var retData = [];
    //$("#link_name").on('tap', function () {
    //    mui.init();
    //    var picker = new mui.PopPicker();
    //    initdata();
    //    picker.setData(retData);
    //    picker.show(function (items) {
    //        var text = items[0].text == undefined ? "" : items[0].text;
    //        var value = items[0].value == undefined ? "" : items[0].value;
    //        var id_l = items[0].id == undefined ? "" : items[0].id;
    //        $("#link_name").val(text);
    //        $("#link_position").html(value);
    //        $("#wp_link_id").val(id_l);
    //    });
    //})
    //起始时间
    var now = new Date(),
        min = new Date(now.getFullYear(), 1, 01);
                max = new Date(now.getFullYear(), 11, 31);

                $('#s_startdate').mobiscroll().date({
        theme: 'mobiscroll',
        lang: 'zh',
        display: 'bottom',
        min:min,
        max: max,
        mode: "scroller",
        dateFormat: 'yy-mm-dd',
        onClose: function (valueText, inst) {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        },
    });
    //
   
    $('#s_enddate').mobiscroll().date({
        theme: 'mobiscroll',
        lang: 'zh',
        display: 'bottom',
        min:min,
        max: max,
        dateFormat: 'yy-mm-dd',
        onClose: function (valueText, inst) {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        },
    });

    $("#wp_userid").val(userid);
    $("#guid").val(userid);
    $("#wp_username").val(username);
    //隐藏提示
    hidePreloader();
    //点击调用图片上传【钉钉】
    $(".camera-area").on('tap', function () {
        dd_uploadImage();
    })
    //调用钉钉高级接口进行图片上传
    var _config = {
        appId: '<%=signPackage.appId%>',
        corpId: '<%=signPackage.corpId%>',
        timeStamp: '<%=signPackage.timestamp%>',
        nonce: '<%=signPackage.nonceStr%>',
        signature: '<%=signPackage.signature%>'
    };
    dd.config({
        appId: _config.appId,
        corpId: _config.corpId,
        timeStamp: _config.timeStamp,
        nonceStr: _config.nonce,
        signature: _config.signature,
        jsApiList: ["runtime.info",
        "device.notification.alert",
        "device.notification.confirm",
        "device.base.getUUID",
        "device.notification.modal",
        "device.geolocation.get",
         "runtime.permission.requestAuthCode",
         "biz.user.get",
         "biz.contact.choose",
         "device.notification.prompt",
         "biz.ding.post",
         "biz.util.openLink",
         "biz.navigation.setMenu",
          "device.base.getInterface",
          "device.nfc.nfcRead",
          "device.launcher.checkInstalledApps",
          "biz.util.uploadImage",
        "biz.map.view"]
    });


    function dd_uploadImage() {
        //上传图片
        dd.ready(function () {
            dd.biz.util.uploadImage({
                multiple: true, //是否多选，默认false
                max: 6,
                onSuccess: function (result) {
                    pics.push(result);
                    var result_len = result.length;
                    $(".mes_lists").show();
                    for (var i = 0; i < result_len; i++) {
                        $("#img").append('<span><img src="' + result[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                    }
                    localStorage.setItem("pic", pics);
                    mui.previewImage();
                },
                onFail: function (err) {

                }
            });
        })
        //钉钉异常
        dd.error(function (error) {

            alert(JSON.stringify(error));
        });
    }
    //获取当前时间
    var now = new Date(),
        minDate = new Date(now.getFullYear() - 10, now.getMonth(), now.getDate()),
        maxDate = new Date(now.getFullYear() + 10, now.getMonth(), now.getDate());

    //激活时间控件【通过这个插件激活控件】
    var instance = mobiscroll.datetime('#select_time', {
        theme: 'mobiscroll',  // Specify theme like: theme: 'ios' or omit setting to use default
        lang: 'zh',           // Specify language like: lang: 'pl' or omit setting to use default
        display: 'bottom',    // Specify display mode like: display: 'bottom' or omit setting to use default
        dateFormat: 'yy-mm-dd'         // More info about max: https://docs.mobiscroll.com/3-0-0_beta6/javascript/datetime#!opt-max
    });

</script>
