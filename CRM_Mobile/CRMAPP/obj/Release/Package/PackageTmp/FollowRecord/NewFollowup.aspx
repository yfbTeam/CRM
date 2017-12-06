<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewFollowup.aspx.cs" Inherits="CRMAPP.FollowRecord.NewFollowup" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>添加跟进</title>
    <link href="../css/mui.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/iconfont.css" />
    <link rel="stylesheet" type="text/css" href="../css/scrollbar.css" />
     <link rel="stylesheet" type="text/css" href="/css/mobiscroll.custom-3.0.0-beta2.min.css" />
    <link rel="stylesheet" href="../css/style.css" />
    <link href="/css/mui.picker.css" rel="stylesheet" />
    <link href="/css/mui.poppicker.css" rel="stylesheet" />
  
    <script type="text/javascript" src="https://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>
    <script src="/js/mui.min.js"></script>
    <script src="../js/jquery-1.8.3.min.js"></script>
    <script src="../js/public.js?dsj=dsj"></script>
    <script src="/js/mobiscroll.custom-3.0.0-beta2.min.js"></script>
    <script type="text/javascript">
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/dd_navigation.js?number=" + Math.random() + "'></s" + "cript>");
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/follow_up.js?gf=kd&number=" + Math.random() + "'></s" + "cript>");
    </script>
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
    <style>
        .mes_lists li .mes_wrap .mes_deta .img_wrap {
            height: auto;
        }
    </style>
</head>
<body>
    <header class="mui-bar mui-bar-nav Header bgblue">
        <a class="mui-action-back mui-icon mui-icon-left-nav mui-pull-left"></a>
        <h1 class="mui-title">添加跟进</h1>
        <span class="mui-pull-right header_button" id="submit">保存</span>
    </header>
    <div class="mui-content" style="padding-bottom: 1.1rem;">
        <section class="section_module mt20">
            <form class="mui-input-group">
                <input type="hidden" name="Func" value="add_follow_up">
                <input type="hidden" name="id" id="id" value="0">
                <input type="hidden" name="follow_userid" fl="用户id" id="follow_userid" value="">
                <input type="hidden" name="guid" id="guid" fl="唯一标识" value="">
                <input type="hidden" name="follow_username" fl="用户名" id="follow_username" value="">
                <input type="hidden" name="follow_status" fl="跟进状态" value="0">
                <input type="hidden" name="follow_date" fl="跟进日期" id="follow_date" value="2016-12-21">
                <input type="hidden" name="follow_cust_id" fl="客户名称" isrequired="true" id="follow_cust_id" value="">
                <input type="hidden" name="follow_link_id" fl="联系人" isrequired="true" id="follow_link_id" value="">
                <input type="hidden" name="picture" fl="图片" id="picture" value="">
                <input type="hidden" name="audio" fl="语音" value="2.jpg">
                <input type="hidden" name="follow_type" fl="跟进类型" id="follow_type" isrequired="true" value="" />
                <input type="hidden" name="follow_remaindate" fl="提醒时间" id="follow_remaindate" value="2016-12-21" />
                <div class="input-row clearfix">
                    <div class="icon bgqiangreen fl">
                        <i class="iconfont">&#xe614;</i>
                    </div>
                    <label for="" class="fl">
                        客户名称
                    </label>
                    <div class="input fr">
                        <input type="text" name="cust_customer" fl="客户名称" readonly="readonly" id="cust_customer" value="" class="text" placeholder="请选择客户名称[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix">
                    <div class="icon bgblue fl">
                        <i class="iconfont">&#xe615;</i>
                    </div>
                    <label for="" class="fl">
                        联系人
                    </label>
                    <div class="input fr">
                        <!--<input type="text" name="" id="" value="" class="text select" placeholder="请选择联系人" />-->
                        <input type="text" name="link_name" fl="联系人" id="link_name" readonly="readonly" value="" class="text" placeholder="请选择联系人[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix">
                    <div class="icon bgpink fl">
                        <i class="iconfont">&#xe692;</i>
                    </div>
                    <label for="" class="fl">
                        跟进类型
                    </label>
                    <div class="input fr">
                        <input type="text" name="select_type" id="select_type" fl="跟进类型" readonly="readonly" value="" class="text" placeholder="请选择跟进类型[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix">
                    <div class="icon fl" style="background: #92dd99;">
                        <i class="iconfont">&#xe626;</i>
                    </div>
                    <label for="" class="fl">
                        跟进时间
                    </label>
                    <div class="input fr">
                        <input type="text" name="follow_date_Select" fl="跟进时间" isrequired="true" id="follow_date_Select" value="" class="text" placeholder="请选择跟进时间[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
            </form>
        </section>
        <section class="section_module mt20">
            <form class="mui-input-group">
                <div class="row">
                    <textarea id='follow_content' name="follow_content" fl="跟进内容" isrequired="true" class="mui-input-clear question" placeholder="输入跟进内容[必填]"></textarea>
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
        <div class="save-btn">
            <span onclick="submit()" id="submit_data" fls="false" class="bgblue">保存</span>
        </div>
       
    </div>


</body>
</html>

<link rel="stylesheet" type="text/css" href="/css/mobiscroll.custom-3.0.0-beta2.min.css" />
<script src="/js/mobiscroll.custom-3.0.0-beta2.min.js"></script>
<script src="/js/mui.zoom.js"></script>
<script src="/js/mui.previewimage.js"></script>

<script type="text/javascript">
    //跟进记录可刷新
    localStorage.setItem("FollowupRecord_needfresh", false);

    //如果是新增（id存在），先赋处事值
    var _id = getparam("id")
    if (_id != "" && _id != null && _id != undefined) {
        getfollowup();
    }
    //隐藏加载框
    hidePreloader();

    //图片数组存储的方式，并加载图片
    var pics = [];
    var p = localStorage.getItem("pic");

    if (p != null && p != "") {
        pics.push(p);
        var pic_len = p.split(',');
        $(".mes_lists").show();
        for (var i = 0; i < pic_len.length; i++) {

            $("#img").append('<span><img src="' + pic_len[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
        }
    }

    /**---------------------选择跟进类型后返回名称和id---------------------------------***/
    if (localStorage.getItem("pub_title") != "" && localStorage.getItem("pub_title") != null) {
        $("#select_type").val(localStorage.getItem("pub_title"));
        $("#follow_type").val(localStorage.getItem("pub_value"));
    }

    /**---------------------选择客户后返回名称和id---------------------------------***/
    if (localStorage.getItem("follow_cust_name") != null) {
        var follow_cust_name = localStorage.getItem("follow_cust_name")

        $("#cust_customer").val(follow_cust_name);
        $("#follow_cust_id").val(localStorage.getItem("follow_cust_id"));
    }
    /*联系人选择后的值*/
    if (localStorage.getItem("link_name") != null) {
        var link_name = localStorage.getItem("link_name")

        $("#link_name").val(link_name);
        $("#follow_link_id").val(localStorage.getItem("link_id"));
    }
    /**--------------------选择客户-----------------------------**/
    var cust_customer = document.querySelector('#cust_customer');
    if (cust_customer != null) {
        cust_customer.addEventListener('tap', function () {
            var id = this.id;
            localStorage.removeItem("pub_title");
            localStorage.removeItem("pub_value");
            localStorage.removeItem("follow_cust_name");
            localStorage.removeItem("follow_cust_id");
            openwindow(id, "/Custom/SelectCustom.html?page=3&type=1&dd_nav_bgcolor=FF6CB1FF&cust_category=0", "slide-in-right")
        });
    }
    dd_setRight_save('保存');
    var retData = [];

    //选择联系人，先保证选择了客户 通过localStorage 方式存储和获取
    var cust_customer = document.querySelector('#cust_customer');
    if (cust_customer != null) {
        $("#link_name").on('tap', function () {
            var cust_customer = $("#cust_customer").val();
            if (cust_customer == "") {
                dd_toast('请先选择客户名称！', 'error', 0);
            }
            else {
                //先删除联系人里的localStorage内容
                localStorage.removeItem("link_id");
                localStorage.removeItem("link_name");
                var follow_cust_id = localStorage.getItem("follow_cust_id");
                openwindow(id, "/Custom/SelectLinkman.html?link_cust_id=" + follow_cust_id + "&dd_nav_bgcolor=FF6CB1FF&number=" + Math.random(), "slide-in-right")
            }

        })
    }

    //给登陆用户赋值
    $("#follow_userid").val(userid);
    $("#guid").val(userid);
    $("#follow_username").val(username);

    //图片功能  钉钉获取
    $(".camera-area").on('tap', function () {
        dd_uploadImage();
    })
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
                onFail: function (err) { }
            });
        })
    }
</script>
<script type="text/javascript">

    //点击跟进记录，选择
    (function ($) {
        var select_type = document.querySelector('#select_type');
        select_type.addEventListener('tap', function () {
            var id = this.id;
            var href = 'SelectFollowup.html?dd_nav_bgcolor=FF6CB1FF&number=' + Math.random();
            openwindow(href, href, 'slide-in-right');
        });
    })(mui);
  
    var myDate = new Date();    
    var month = myDate.getMonth();;  
    var day = myDate.getDate();
    //debugger;
    //起始时间
    var now = new Date(),
        min = new Date(myDate.getFullYear(), month, day - 2);
    max = new Date(myDate.getFullYear(), month, day);
    
    //debugger;

    $('#follow_date_Select').mobiscroll().date({
        theme: 'mobiscroll',
        lang: 'zh',
        display: 'bottom',
        min: min,
        max: max,
        dateFormat: 'yy-mm-dd',
        onClose: function (valueText, inst) {
            $('#follow_date').val($('#follow_date_Select').val());
        },
    });

    
</script>
