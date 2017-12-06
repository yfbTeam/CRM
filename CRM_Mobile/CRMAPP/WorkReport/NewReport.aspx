<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewReport.aspx.cs" Inherits="CRMAPP.WorkReport.NewReport" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>写报告</title>
    <link href="/css/mui.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/css/iconfont.css" />
    <link rel="stylesheet" href="/css/style.css" />
    <link href="/css/layer.css" rel="stylesheet" />
    <script src="/js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="https://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>
    <script type="text/javascript" src="/js/mui.min.js"></script>
    <script src="/js/public.js?kds=gff"></script>
    <script type="text/javascript" src="/js/ejs.min.js"></script>
    <script type="text/javascript">
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/dd_navigation.js?did=fdfd&number=" + Math.random() + "'></s" + "cript>");
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/new_report.js?ssf=11&number=" + Math.random() + "'></s" + "cript>");
    </script>
    <%--    
    <script type="text/template" id="list_template">
        <% $.each(retData,function(index,item){ %>
        <li><div class="mes_wrap clearfix"><div class="people_img fl"><img src="../images/people_img.jpg" />
                </div><div class="mes_deta fr"><div class="clearfix a"><span class="people fl"><%=item.wp_username%></span><span class="time fr"><%=item.wp_plandate%></span>
                    </div><div class="content"><%=item.wp_content%></div><div class="detail"><span><%=item.cust_name%></span><b>|</b><span><%=item.link_name%></span>
                    </div></div></div></li>
        <% }) %>
    </script>--%>
    <style>
        .mes_lists li .mes_wrap .mes_deta .img_wrap {
            height: auto;
        }
    </style>
</head>
<body>
    <header class="mui-bar mui-bar-nav Header bgshenhuang">
        <a class="mui-action-back mui-icon mui-icon-left-nav mui-pull-left"></a>
        <h1 class="mui-title">
            <div><span>写日报</span><i class="iconfont">&#xe61e;</i></div>
        </h1>
        <span class="mui-pull-right header_button" id="submit">提交</span>
    </header>
    <div class="mui-content" style="padding-bottom: 1.1rem;">
        <section class="section_module mt20">
            <form class="mui-input-group">
                <input type="hidden" name="Func" value="edit_workreport">
                <input type="hidden" name="id" value="0">
                <input type="hidden" name="report_userid" fl="用户id" id="report_userid" value="">
                <input type="hidden" name="report_username" fl="用户名" id="report_username" value="">
                <input type="hidden" name="guid" id="guid" fl="唯一标识" value="">
                <input type="hidden" name="report_type" fl="报告类型" id="report_type" value="">
                <input type="hidden" name="t_picture" fl="今日图片" id="picture" value="">
                <input type="hidden" name="m_picture" fl="明日计划" id="picture_t" value="">
                <%-- <input type="hidden" name="report_reader_name" fl="发送人" id="report_reader_name" value="">
                <input type="hidden" name="report_sender_name" fl="抄送人" id="report_sender_name" value="">--%>
                <div class="input-row clearfix">
                    <div class="icon fl" style="background: #87d8ef;">
                        <i class="iconfont">&#xe60c;</i>
                    </div>
                    <label for="" class="fl">
                        报告类型
                    </label>
                    <div class="input fr">
                        <input type="text" name="link" fl="报告类型" id="link" isrequired="true" value="" readonly="readonly" placeholder="请选择报告类型[必填]" class="text select" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix" id="report_startdate_div">
                    <div class="icon bgqiangreen fl">
                        <i class="iconfont">&#xe626;</i>
                    </div>
                    <label for="" class="fl">
                        开始时间
                    </label>
                    <div class="input fr">
                        <input type="text" name="report_startdate" fl="开始时间"  id="report_startdate"isrequired="true" value="" class="text select" placeholder="请选择时间[必填]" />
                        <i class="iconfont" id="time_one">&#xe64c;</i>
                    </div>
                </div>
                <div class="input-row clearfix" id="report_enddate_div" style="display: none">
                    <div class="icon bgqiangreen fl">
                        <i class="iconfont">&#xe626;</i>
                    </div>
                    <label for="" class="fl">
                        结束时间
                    </label>
                    <div class="input fr">
                        <input type="text" name="report_enddate" fl="结束时间" id="report_enddate" isrequired="true"  value="" class="text select" placeholder="请选择时间[必填]" />
                        <i class="iconfont">&#xe64c;</i>
                    </div>
                </div>
            </form>
        </section>
        <div class="report_detail">
            <a class="clearfix" id="see">
                <div>
                    <p>新增客户</p>
                    <p id="cust_customer_count">0</p>
                </div>
                 <div>
                    <p>新增联系人</p>
                    <p id="cust_linkman_count">0</p>
                </div>
                <div>
                    <p>跟进次数</p>
                    <p id="follow_up_count">0</p>
                </div>
                <div>
                    <p>签到次数</p>
                    <p id="sign_in_count">0</p>
                </div>
            </a>
            <i class="iconfont">&#xe64c;</i>
        </div>
        <section class="section_module mt20">
            <form class="mui-input-group">
                <div class="row">
                    <textarea id='report_content' isrequired="true" fl="今日总结" name="report_content" class="mui-input-clear question" placeholder="今日总结"></textarea>
                </div>
                <div class="camera-area" id="Summarytoday">
                    <div class="clearfix">
                        <div class="imgicon fl">
                        </div>
                        <div class="fl wen">
                            添加图片
                        </div>
                    </div>

                </div>
                <ul class="mes_lists" id="mes_t" style="display: none">
                    <li>
                        <div class="mes_wrap clearfix">
                            <div class="mes_deta">
                                <div class="img_wrap">
                                    <div class="img clearfix" id="img_today">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </form>
        </section>
        <section class="section_module mt20">
            <form class="mui-input-group">
                <div class="row">
                    <textarea id='report_plan' isrequired="true" name="report_plan" fl="明日计划" class="mui-input-clear question" placeholder="明日计划"></textarea>
                </div>
                <div class="camera-area" id="Tomorrowplan">
                    <div class="clearfix">
                        <div class="imgicon fl">
                        </div>
                        <div class="fl wen">
                            添加图片
                        </div>
                    </div>

                </div>
                <ul class="mes_lists" id="mes_l" style="display: none">
                    <li>
                        <div class="mes_wrap clearfix">
                            <div class="mes_deta">
                                <div class="img_wrap">
                                    <div class="img clearfix" id="img_tomorrow">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </li>
                </ul>
            </form>
        </section>
        <div style="display:none" class="report_ri mt20">
            <ul class="mes_lists" id="list"></ul>
        </div>
        <form class="mui-input-group mt20">
            <div class="input-row clearfix">
                <div class="icon fl" style="background: #62c7c5;">
                    <i class="iconfont">&#xe617;</i>
                </div>
                <label for="" class="fl">
                    批阅人
                </label>
                <div class="input fr">
                    <input type="text" name="report_reader_name" id="report_reader" readonly="readonly"  fl="请选择批阅人"  value="" placeholder="" class="text" />
                    <input type="hidden" isrequired="false" name="report_reader" fl="请选择批阅人11111" id="report_readers" value="F4C7BE25-6482-4028-90E3-3BC5D8268338,"  placeholder="" class="text" />
                    <i class="iconfont" id="cust_customer">&#xe64c;</i>
                </div>
            </div>
            <div class="input-row clearfix">
                <div class="icon fl" style="background: #8399cb;">
                    <i class="iconfont">&#xe6d4;</i>
                </div>
                <label for="" class="fl">
                    抄送人
                </label>
                <div class="input fr">
                    <input type="text" name="report_sender_name" fl="请选择抄送人" readonly="readonly" id="report_sender" value="" class="text" />
                    <input type="hidden" isrequired="false" fl="请选择抄送人1111" name="report_sender" id="report_senders" value="F4C7BE25-6482-4028-90E3-3BC5D8268338," placeholder="请选择抄送人" class="text" />
                    <i class="iconfont">&#xe64c;</i>
                </div>
            </div>
        </form>
        <div class="save-btn">
             <span onclick="submit()" class="bgshenhuang" id="submit_data" fls="false">提交</span>
        </div>
    </div>
    <!--蒙版层-->
    <div class="mark_wrap1"></div>
    <style type="text/css">
        .cs_style {
            text-align: center;
            width: 80%;
            margin: 0 auto;
            height: 0.8rem;
            line-height: 0.8rem;
        }

            .cs_style.active {
                color: #ff0000;
            }

            .mark_wrap1 {
  width: 100%;
  position: fixed;
            top: 0rem;
  bottom: 0;
  background: rgba(162, 163, 165, 0.5);
  z-index: 9998;
  display: none;
}

        .fs_show {
            display: none;
            position: fixed;
            bottom: 0;
            left: 0;
            width: 100%;
            padding: 10px 0;
            border: 1px solid #ccc;
            z-index: 10000;
            background: #fff;
        }
    </style>
    <div id="cs" class="fs_show">
    </div>
    <div id="fs" class="fs_show">
    </div>
    <!--日报周报切换-->
    <!--<div class="tou_sele" id="report_type">
        <p class="active"><span>写日报 </span><i class="iconfont">&#xe650;</i></p>
        <p><span>写周报</span><i class="iconfont">&#xe650;</i></p>
        <p><span>写月报</span><i class="iconfont">&#xe650;</i></p>
    </div>-->

    <link rel="stylesheet" type="text/css" href="/css/mobiscroll.custom-3.0.0-beta2.min.css" />
    <script src="/js/mobiscroll.custom-3.0.0-beta2.min.js"></script>
    <script src="/js/mui.zoom.js"></script>
    <script src="/js/mui.previewimage.js"></script>
    <script type="text/javascript">

      
        //工作报告可刷新
        localStorage.setItem("WorkReport_needfresh", true);

        var today_pics = [];
        var tomorrow_pics = [];
        var report_senders = [];
        var report_senders_name = [];
        var report_readers = [];
        var report_readers_name = [];
        var report_type = 1;
        hidePreloader();
        dd_setRight_save('提交');
        //人员信息 组织架构 当前部分
        if (localStorage.getItem("orgData") != 'undefined') {
            var orgData = JSON.parse(localStorage.getItem("orgData"));
            //console.log(as.orgData)
            //orgData.errMsg;
            debugger;
            for (var i = 0; i < orgData.length; i++) {
                var rolename  = orgData[i].RoleName.split(',');
                rolename = rolename[0];
               
                //alert(JSON.stringify(orgData.length));
                if (username == orgData[i].Name) {
                    if (rolename != "" && rolename == "部门总监-CRM") {
                      
                        $('#report_reader').val("刘坤");
                        $('#report_readers').val("410FB978-44CA-4DAD-8898-BCB9627BD58E");
                        $('#report_sender').val("刘峻嵩、姜玲娜");
                        $('#report_senders').val("5AA4BCF7-B092-4146-8BAD-C70C8DEB1A1C、F4C7BE25-6482-4028-90E3-3BC5D8268338");
                    } else if (rolename == "") {
                        for (var j = 0; j < orgData.length; j++) {
                            if (orgData[j].RoleName != "") {
                                var rolename1 = orgData[j].RoleName.split(',')
                                rolename1 = rolename1[0];
                                if (rolename1 == "部门总监-CRM") {
                                    $('#report_reader').val(orgData[j].Name);
                                    $('#report_readers').val(orgData[j].UniqueNo);
                                
                                }
                            }
                        }
                        $('#report_sender').val("刘峻嵩、刘坤、姜玲娜");
                        $('#report_senders').val("5AA4BCF7-B092-4146-8BAD-C70C8DEB1A1C、410FB978-44CA-4DAD-8898-BCB9627BD58E、F4C7BE25-6482-4028-90E3-3BC5D8268338");
                    }
                }
                
            }
            //$("#cs").append("<div class=\"cs_style\" ontouchend=\"show(this,'F4C7BE25-6482-4028-90E3-3BC5D8268338','姜玲娜')\" userid='F4C7BE25-6482-4028-90E3-3BC5D8268338'>姜玲娜</div><div class=\"cs_style\" ontouchend=\"show(this,'410FB978-44CA-4DAD-8898-BCB9627BD58E','刘坤')\" userid='410FB978-44CA-4DAD-8898-BCB9627BD58E'>刘坤</div><div class=\"cs_style\" ontouchend=\"show(this,'5AA4BCF7-B092-4146-8BAD-C70C8DEB1A1C','刘峻嵩')\" userid='5AA4BCF7-B092-4146-8BAD-C70C8DEB1A1C'>刘峻嵩</div>");
            
            //for (var i = 0; i < orglen; i++) {
            //    if (orglen <= 3) {
            //        $("#cs").append("<div class=\"cs_style\"></div>");
            //        $("#cs").append("<div class=\"cs_style\"></div>");
            //        $("#cs").append("<div class=\"cs_style\"></div>");
            //    }
            //    if (orgData[i].RoleName != "" && orgData[i].RoleName != "超级管理员") {
            //        $("#cs").append("<div class=\"cs_style\" ontouchend=\"show(this,'" + orgData[i].UniqueNo + "','" + orgData[i].Name + "')\">" + orgData[i].Name + "</div>");
            //    }
            //}                  
            //$('#report_sender').each(function (index) {
               
            //    //report_senders.push(id);
            //    //report_senders_name.push(name);
            //    //$(_this).append('<i class="iconfont" style="color:#1296db">&nbsp;&#xe650;</i>');
            //    //$("#report_sender").val(report_senders_name.join('、'));               
            //});
           

        }

        var now = new Date();
        $('#report_startdate').mobiscroll().date({
            theme: 'mobiscroll',
            lang: 'zh',
            display: 'bottom',
            dateFormat: 'yy-mm-dd',
            onClose: function (valueText, inst) {
                s_startdate = $(this).val();
                httpData.initData(report_type, s_startdate, s_enddate);

            },
        });
        $('#report_enddate').mobiscroll().date({
            theme: 'mobiscroll',
            lang: 'zh',
            display: 'bottom',
            dateFormat: 'yy-mm-dd',
            onClose: function (valueText, inst) {

                s_enddate = $(this).val();
                if (report_type == 1) {
                    s_enddate = s_startdate;
                }
                httpData.initData(report_type, s_startdate, s_enddate);

            },
        });
        var s_startdate;
        var s_enddate;
        

        //选择周报 日报 月报
        $("#link").on('tap', function () {
            dd.device.notification.actionSheet({
                title: "请选择报告类型", //标题
                cancelButton: '取消', //取消按钮文本
                otherButtons: ["日报", "周报", "月报"],
                onSuccess: function (result) {
                    //onSuccess将在点击button之后回调
                    /*{
                        buttonIndex: 0 //被点击按钮的索引值，Number，从0开始, 取消按钮为-1
                    }*/
                    if (result.buttonIndex != -1) {
                        var b_type = "";
                        if (result.buttonIndex == 0) {
                            b_type = '日报';
                            $("#report_content").attr('placeholder', '今日总结');
                            $("#report_plan").attr('placeholder', '明日计划');
                            $("#report_content").attr('fl', '今日总结');
                            $("#report_plan").attr('fl', '明日计划');
                            
                            s_enddate = s_startdate;
                            httpData.initData(1, s_startdate, s_enddate);
                            report_type = 1;
                          
                        } else if (result.buttonIndex == 1) {
                            b_type = '周报';
                            $("#report_content").attr('placeholder', '本周总结');
                            $("#report_plan").attr('placeholder', '下周计划');
                            $("#report_content").attr('fl', '本周总结');
                            $("#report_plan").attr('fl', '下周计划');
                            httpData.initData(2, s_startdate, s_enddate);
                            report_type = 2;
                           
                        } else if (result.buttonIndex == 2) {
                            b_type = '月报';
                            $("#report_content").attr('placeholder', '本月总结');
                            $("#report_plan").attr('placeholder', '下月计划');
                            $("#report_content").attr('fl', '本月总结');
                            $("#report_plan").attr('fl', '下月计划');
                            httpData.initData(3, s_startdate, s_enddate);
                            report_type = 3;
                           
                        }
                        else {
                            b_type = "请选择报告类型";
                        }
                        $("#link").val(b_type);
                        $("#report_type").val(result.buttonIndex + 1);
                        if (result.buttonIndex == 0) {
                            min = new Date(now.getFullYear(), now.getMonth(), now.getDate()-2),
		                    max = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                            $('#report_startdate').mobiscroll().date({
                                theme: 'mobiscroll',
                                lang: 'zh',
                                display: 'bottom',
                                min:min,
                                max: max,
                                dateFormat: 'yy-mm-dd',
                                onClose: function (valueText, inst) {
                                    s_startdate = $(this).val();
                                    httpData.initData(report_type, s_startdate, s_enddate);

                                },
                            });
                            $("#report_enddate_div").hide();
                            $("#report_enddate_div").val();
                        }
                        else {
                            $("#report_enddate_div").show();
                            
                        }
                    }

                },
                onFail: function (err) { }
            })
        })
        //点击批阅区域 暂时移除
        //$("#report_sender").on('tap', function () {
        //    $("#cs").show();
        //    $('.mark_wrap1').css('display', 'block');
        //    $("#cs i").remove();
        //    report_senders_name = [];
        //})
        //点击抄送区域
        //$("#report_reader").on('tap', function () {
        //    report_readers_name = [];
        //    $("#fs").show();
        //    $('.mark_wrap1').css('display', 'block');
        //    var name1;
        //    var value = $('#report_senders').val();
        //    if(value != ''){
        //        value = value.split('、');
        //        value.each(function (index, elem) {
        //            name1 = $(elem);
        //        })
        //        $('#cs .cs_style').each(function (index,elem) {
        //            if(name1 == $(elem)){
        //                $(elem).append('<i class="iconfont" style="color:#1296db">&nbsp;&#xe650;</i>');
        //            }
        //        })
                
        //    }
        //})

        //选择或者移除批阅人
        function show(_this, id, name) {
            if ($(_this).has('i').length > 0) {
              
                $(_this).children('i').remove();
                report_senders.splice($.inArray(id, report_senders), 1);                
                report_senders_name.splice($.inArray(name, report_senders_name), 1);               
                $("#report_sender").val(report_senders_name.join('、'));               
            }
            else {
                report_senders.push(id);
                report_senders_name.push(name);
                $(_this).append('<i class="iconfont" style="color:#1296db">&nbsp;&#xe650;</i>');
                $("#report_sender").val(report_senders_name.join('、'));
                //$("#sp_2").html("&" + name + '-' + report_senders_name.join('、'));
                
            }
        }

        //选择或移除抄送人
        function showfs(_this, id, name) {
            if ($(_this).has('i').length > 0) {             
                $(_this).children('i').remove();             
                report_readers.splice($.inArray(id, report_readers), 1);
                report_readers_name.splice($.inArray(name, report_readers_name), 1);                
                $("#report_reader").val(report_readers_name.join('、'));               
            }
            else {
                $(_this).append('&nbsp;<i class="iconfont" style="color:#1296db">&#xe650;</i>');
                report_readers.push(id);
                report_readers_name.push(name);
                $("#report_reader").val(report_readers_name.join('、'));
            }
        }


        $(document).on('tap', function (e) {
            var target = e.target;
            var tagname = target.tagName;
            if (tagname == 'DIV' && target.className == 'mark_wrap1') {
                $('.mark_wrap1').css('display', 'none');
                $("#cs").css('display', 'none');
                $("#fs").css('display', 'none');
                $("#fs i").empty();
                $("#cs i").empty();
                
            }
        })
        //查看详情
        $("#see").on('tap', function () {
            //openwindow('SeeDetail.html?dd_nav_bgcolor=FFF7A64F&report_type=' + report_type + '&number=' + Math.random(), 'SeeDetail.html?dd_nav_bgcolor=FFF7A64F&report_type=' + report_type + '&' + Math.random(), 'slide-in-right');

        })


        $("#report_username").val(username);
        $("#report_userid").val(userid);
        $("#guid").val(userid);
        $("#Summarytoday").on('tap', function () {
            dd_uploadImage();
        })
        $("#Tomorrowplan").on('tap', function () {
            dd_uploadImage_t();
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

        //今日总结图片
        function dd_uploadImage() {
            dd.ready(function () {
                dd.biz.util.uploadImage({
                    multiple: true, //是否多选，默认false
                    max: 6,
                    onSuccess: function (result) {
                        today_pics.push(result);
                        var result_len = result.length;
                        $("#mes_t").show();
                        for (var i = 0; i < result_len; i++) {
                            $("#img_today").append('<span><img src="' + result[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                        }

                        mui.previewImage();
                    },
                    onFail: function (err) { }
                });
            })
        }
        //明日计划图片
        function dd_uploadImage_t() {
            dd.ready(function () {
                dd.biz.util.uploadImage({
                    multiple: true, //是否多选，默认false
                    max: 6,
                    onSuccess: function (result) {
                        tomorrow_pics.push(result);
                        var result_len = result.length;
                        $("#mes_l").show();
                        for (var i = 0; i < result_len; i++) {
                            $("#img_tomorrow").append('<span><img src="' + result[i] + '" alt="" data-preview-src="" data-preview-group="1"></span>');
                        }

                        mui.previewImage();
                    },
                    onFail: function (err) { }
                });
            })
        }
    </script>
</body>
</html>
