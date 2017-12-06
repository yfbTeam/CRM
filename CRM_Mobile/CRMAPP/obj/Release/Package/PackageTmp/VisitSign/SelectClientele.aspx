<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectClientele.aspx.cs" Inherits="CRMAPP.VisitSign.SelectClientele" %>

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>选择客户</title>
    <link href="/css/mui.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/css/iconfont.css" />
    <link rel="stylesheet" href="/css/style.css" />
    <script src="/js/mui.min.js"></script>
    <script src="/js/zepto.min.js"></script>
    <script src="/js/public.js?dsksd=d2sjk"></script>
    <script src="http://webapi.amap.com/maps?v=1.3&key=f07e75646876646ed34a84f363d51891&callback=init"></script>
    <script type="text/javascript" src="http://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>
    <script type="text/javascript" src="/js/ejs.min.js"></script>
    <script type="text/javascript">
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/common.js?number=" + Math.random() + "'></s" + "cript>");
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/dd_navigation.js?number=" + Math.random() + "'></s" + "cript>");
        document.write("<s" + "cript type='text/javascript' src='/js/pagejs/sign_in.js?fdfd=321&number=" + Math.random() + "'></s" + "cript>");
    </script>
    <style type="text/css">
        .mui-bar-nav ~ .mui-content .mui-pull-top-pocket {
            top: 0;
        }
    </style>
    <%--    <script type="text/template" id="list_template">
        <% $.each(retData,function(index,item){ %>
        <li><input type="hidden" name="cust_id" value="<%=item.id%>"><input type="hidden" name="cust_name" value="<%=item.cust_name%>"><input type="hidden" name="distance" value="<%=item.distance%>">
            <div class="clearfix"><span class="school fl"><%=item.cust_name%></span><span class="fr people" style='<%=item.distance>2000?"color:red":""%>'><%=item.distance%>m</span></div>
            <div class="clearfix"><span class="piancha fl"><i class="iconfont">&#xe68a;</i><%=item.cust_address%></span></div></li>
        <% }) %>
    </script>--%>
    <script>
        var VisitSign_needfresh = getQueryString("VisitSign_needfresh");
        if (VisitSign_needfresh == 'true') {
            localStorage.setItem('VisitSign_needfresh', 'true');
        }
    </script>
</head>
<body>
    <header class="mui-bar mui-bar-nav Header bgred">
        <a class="mui-action-back mui-icon mui-icon-left-nav mui-pull-left"></a>
        <h1 class="mui-title">选择客户</h1>
    </header>
    <div class="mui-content">
        <div class="search_wrap" style="top: 0">
            <input type="text" name="cust_name" id="cust_name" placeholder="请输入客户名称" class="search" />
            <i class="mui-icon-search mui-icon"></i>
        </div>
        <div id="pullrefresh" class="mui-content mui-scroll-wrapper customer_pull" style="top: 0.75rem; bottom: 1.1rem;">
            <div class="mui-scroll">
                <ul class="sign_lists" id="list"></ul>
            </div>
        </div>
    </div>
    <div class="save-btn">
        <span id="submit_data" fls="false" class="bgred">签到</span>
    </div>
    <script type="text/javascript">
        var xself;
        var address;

        var yself;
        var id, distance, cust_name, distance;
        dd_setRightNone();
        /*工作计划列表页*/
        var url = pageurl + "/Custom/cust_customer_handle.ashx";
        var u = navigator.userAgent;
        var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
        var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
        //if (isAndroid) {
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

        //钉钉接口异常捕获
        dd.error(function (error) {
            //dd_toast('钉钉异常！', 'error', 0);
        });

        var add = "";
        //钉钉
        dd.ready(function () {

            dd.device.geolocation.get({
                targetAccuracy: 200,
                coordinate: 0,
                withReGeocode: true,
                onSuccess: function (result) {
                    /* 高德坐标 result 结构
                    {
                        longitude : Number,
                        latitude : Number,
                        accuracy : Number,
                        address : String,
                        province : String,
                        city : String,
                        district : String,
                        road : String,
                        netType : String,
                        operatorType : String,
                        errorMessage : String,
                        errorCode : Number,
                        isWifiEnabled : Boolean,
                        isGpsEnabled : Boolean,
                        isFromMock : Boolean,
                        provider : wifi|lbs|gps,
                        accuracy : Number,
                        isMobileEnabled : Boolean
                    }
                    */
                    // alert(JSON.stringify(result));


                    if (result.errorMessage != 'success') {
                        xself = result.longitude + 0.0060487513753;
                        yself = result.latitude + 0.00102650632146;
                        //initdata();
                    }
                    else {
                        xself = result.longitude;
                        yself = result.latitude;


                    }
                    page_parmeter.pageindex = 1;
                    initdata(page_parmeter);
                    //监听搜索keyup事件
                    if (isAndroid == false) {
                        $("#cust_name").on("blur", function () {
                            page_parmeter.pageindex = 1;
                            initdata(page_parmeter);
                        });
                    }
                    else {
                        $("#cust_name").on("keyup", function () {
                            page_parmeter.pageindex = 1;
                            initdata(page_parmeter);
                        });
                    }
                    //hidePreloader();
                    gdRetunResult = result;
                  
                    address = gdRetunResult.address;
                    if (address == null || address == undefined || address == '') {
                        if (gdRetunResult.province != undefined && gdRetunResult.city != undefined && gdRetunResult.district != undefined && gdRetunResult.road != undefined) {
                            address = gdRetunResult.province + gdRetunResult.city + gdRetunResult.district + gdRetunResult.road;
                        }
                    }
                },
                onFail: function (err) {

                    dd_toast('钉钉异常！', 'error', 0);
                }
            });
        })

        //}
        //else {

        //    var map = new AMap.Map("container");
        //    map.plugin('AMap.Geolocation', function () {
        //        geolocation = new AMap.Geolocation({
        //            enableHighAccuracy: true,//是否使用高精度定位，默认:true
        //            timeout: "100000",
        //            GeoLocationFirst: true,
        //            zoomToAccuracy: true,
        //            isSupported: true,
        //            convert: true,
        //            useNative: true,
        //            buttonOffset: new AMap.Pixel(10, 20),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
        //            showMarker: true,        //定位成功后在定位到的位置显示点标记，默认：true
        //            showCircle: true,        //定位成功后用圆圈表示定位精度范围，默认：true
        //            panToLocation: true,     //定位成功后将定位到的位置作为地图中心点，默认：true
        //            zoomToAccuracy: true,     //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
        //            buttonPosition: 'RB',
        //            complete: function () {
        //                //alert('定位成功');
        //            },
        //            error: function () {
        //                // alert('定位失败');
        //            }
        //        });


        //        map.addControl(geolocation);
        //        geolocation.getCurrentPosition();
        //        AMap.event.addListener(geolocation, 'complete', onComplete);//返回定位信息
        //        AMap.event.addListener(geolocation, 'error', onError);      //返回定位出错信息


        //    });
        //}
        //解析定位结果
        function onComplete(data) {
            var st = data.addressComponent;
            //alert(JSON.stringify(st));
            address = st.province + st.city + st.district + st.street;
            xself = data.position.getLng();
            yself = data.position.getLat();
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);

        }


        //解析定位错误信息
        function onError(data) {
            //document.getElementById('tip').innerHTML = '定位失败';
        }
        //选择的签到客户
        var select_id;
        //选择的用户距离
        var select_distance;
        //数据
        function initdata(page_parmeter) {
            var cust_name = $("#cust_name").val();
            var data = {
                Func: "get_cust_customer_search",
                cust_name: cust_name,
                cust_users: userid,
                sign_x: xself,
                sign_y: yself,
                ispage: true,
                pagesize: page_parmeter.pagesize,
                pageindex: page_parmeter.pageindex,
                guid: userid
            }
            getajax_async(url, data, function (json) {
                debugger;
                if (json.result.errMsg == "success") {
                    var retData = json.result.retData.PagedData;
                    page_parmeter.RowCount = json.result.retData.RowCount;
                    if (page_parmeter.pageindex == 1) {
                        $('#list').empty();
                    }
                    //当列表数据过少是禁用下拉刷新，上拉加载更多
                    if (retData != null && retData != '' && retData.length <= 0) {
                        mui('#pullrefresh').pullRefresh().disablePullupToRefresh();
                    }
                    nomessage("list");
                    var len = retData.length;
                    for (var i = 0; i < len; i++) {
                        var distance;
                        var display_distance;

                        if (retData[i].cust_x != null && retData[i].cust_x != undefined && retData[i].cust_x != "" && retData[i].cust_x != '0' &&
                            retData[i].cust_y != null && retData[i].cust_y != undefined && retData[i].cust_y != "" && retData[i].cust_y != '0'
                            ) {

                            distance = getGreatCircleDistance(xself, yself, retData[i].cust_x, retData[i].cust_y);
                            display_distance = (distance / 1000).toFixed(2) + "km";
                        }
                        else {

                            distance = '';
                            display_distance = "无客户地址";
                        }
                        //样式选择
                        var distance_style;
                        var custname_style;
                        var cust_address = '未知';

                        //不同角色显示的签到方式或记录都不同
                        var role = localStorage["role"];
                        if (role == "Super_Admin" || role == "Common_Admin") {
                            if (distance == '') {
                                distance_style = '</span><span class="fr people2" >';
                                custname_style = '<span class="school2 fl">';
                            }
                            else if (distance < 2000) {

                                distance_style = '</span><span class="fr people" >';
                                custname_style = '<span class="school fl">';
                                cust_address = retData[i].cust_address;
                            }

                            else {
                                distance_style = '</span><span class="fr people2" >';
                                custname_style = '<span class="school2 fl">';
                                cust_address = retData[i].cust_address;
                            }
                        }
                        else {
                            //非管理员
                            custname_style = '<span class="school fl">';
                            if (distance == '') {
                                distance_style = '</span><span class="fr people2" >';

                            }
                            else {
                                distance_style = '</span><span style="display:none" >';
                                cust_address = retData[i].cust_address;
                            }
                        }

                        //数据绑定
                        var html = '<li><input type="hidden" name="cust_id" value="' + retData[i].id + '"><input type="hidden"  name="cust_name" value="' + retData[i].cust_name + '"><input type="hidden" name="distance" value="' + distance + '">';
                        html += '<div class="clearfix">' + custname_style + retData[i].cust_name + distance_style + display_distance + '</span></div>';
                        html += '<div class="clearfix"><span class="piancha fl"><i class="iconfont">&#xe68a;</i>' + cust_address + '</span></div></li>';
                        $('#list').append(html);
                    }


                    //选择某一客户后返回
                    mui('#list li').each(function () {
                        this.addEventListener('tap', function (event) {
                            //$(this).addClass('bggray').siblings().removeClass('bggray');
                            select_id = $(this).children("input[name='cust_id']").val();

                            var distance = $(this).children("input[name='distance']").val();
                            var cust_name = $(this).children("input[name='cust_name']").val();

                            //var distance = $(this).children("input[name='distance']").val();
                            $(this).css('background', '#dcdcdc').siblings().css('background', '#fff');

                            select_distance = $(this).children("input[name='distance']").val();
                        });
                    });
                } else {
                    error("list");
                }
            }, function () {
                dd_toast('接口错误，请联系管理员！', 'error', 0);
            })

        }

        //签到
        $('#submit_data').on('tap', function () {

            if (select_id != null && select_id != "") {

                if (select_distance != '') {
                    if (xself != undefined && yself != undefined) {
                        saveData(userid, username, select_id, xself, yself, address, select_distance)
                    }
                    else {
                        dd_toast('请检查手机定位是否开启', 'error', 0);
                    }
                }
                else {
                    dd_toast('该客户未填写地址,无法签到！', 'error', 0);
                }
            }
            else {
                dd_toast('您未选择客户！', 'error', 0);
            }
        });

        var btnArray = ['确认', '取消'];
        mui('.plan_lists').on('tap', '.mui-btn', function (event) {
            var elem = this;
            mui.confirm('确认删除该条记录？', '', btnArray, function (e) {
                if (e.index == 0) {
                    var li = elem.parentNode.parentNode;
                    li.parentNode.removeChild(li);
                }
            });
        });
        hidePreloader();


        ///【计算经纬度】
        var EARTH_RADIUS = 6378137.0;    //单位M
        var PI = Math.PI;
        function getRad(d) {
            return d * PI / 180.0;
        }

        /**    
         * caculate the great circle distance    
         * @param {Object} lat1    
         * @param {Object} lng1    
         * @param {Object} lat2    
         * @param {Object} lng2    
         */

        function getGreatCircleDistance(lat1, lng1, lat2, lng2) {
            var radLat1 = getRad(lat1);
            var radLat2 = getRad(lat2);
            var a = radLat1 - radLat2;
            var b = getRad(lng1) - getRad(lng2);
            var s = 2 * Math.asin(Math.sqrt(Math.pow(Math.sin(a / 2), 2) + Math.cos(radLat1) * Math.cos(radLat2) * Math.pow(Math.sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.round(s * 10000) / 10000.0;
            return s;

        }


    </script>
</body>
</html>
