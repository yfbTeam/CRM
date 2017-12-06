<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyMap.aspx.cs" Inherits="CRMAPP.VisitSign.MyMap" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,minimum-scale=1,maximum-scale=1,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <title>拜访签到</title>
    <link href="../css/mui.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../css/iconfont.css" />
    <link rel="stylesheet" href="../css/style.css" />
    <script src="../js/mui.min.js"></script>
    <script type="text/javascript" src="../js/zepto.min.js"></script>

    <link rel="stylesheet" href="http://cache.amap.com/lbs/static/main1119.css" />

</head>
<body>
    <header class="mui-bar mui-bar-nav Header bgred">
        <a class="mui-action-back mui-icon mui-icon-left-nav mui-pull-left"></a>
        <h1 class="mui-title">拜访签到</h1>
    </header>
    <div class="mui-content map" style="position: absolute; width: 100%; top: 0rem; bottom: 0.5rem;">
        <div id="container" style="width: 100%; height: 100%;"></div>
        <div class="sign_clientele" id="sign_clientele">
            <div class="clientele">
                <h2 id="cust_name">请选择签到客户</h2>
                <%--<p>您附近有<span id="cust_count">0</span>个客户</p>--%>
            </div>
            <i class="iconfont">&#xe64c;</i>
        </div>
        <input type="button"  onclick="saveData()" fls="false" value="签到" id="signbtn" />
    </div>

</body>
</html>

<script src="http://webapi.amap.com/maps?v=1.3&key=f07e75646876646ed34a84f363d51891&callback=init"></script>
<script type="text/javascript" src="https://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>
<!--<script type="text/javascript" src="https://g.alicdn.com/ilw/ding/0.9.9/scripts/dingtalk.js"></script>-->
<script src="../js/public.js?du=fdjj"></script>
<script type="text/javascript">
    document.write("<s" + "cript type='text/javascript' src='/js/pagejs/dd_navigation.js?number=" + Math.random() + "'></s" + "cript>");
    document.write("<s" + "cript type='text/javascript' src='/js/pagejs/sign_in.js?fdfd=dskjkj&number=" + Math.random() + "'></s" + "cript>");
</script>
<script>
    dd.ready(function () {
        dd_setRightNone();
    });
    function show() {
        var cust_name = localStorage.getItem("cust_name");
        var distance = localStorage.getItem("distance");
        if (cust_name != null)
        {
            $("#cust_name").html("签到客户：" + cust_name);
        }
        
    }
    function show2(str) {
        trace(str);
    }
    setInterval(show, 1000);
    var map;
    var gdRetunResult;
    var xself;
    var yself;



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
    dd_toast('钉钉异常！', 'error', 0);
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
                //initdata();
            }
            //alert(JSON.stringify(result.longitude));
            map = new AMap.Map("container", {
                resizeEnable: true,
                center: [xself, yself],
                zoom: 17
            });
            if (localStorage.getItem("cust_name") != null && localStorage.getItem("cust_name") != "") {
                add = localStorage.getItem("cust_name") + "(" + localStorage.getItem("distance") + ")";
            }
            else {
                add = result.address;
            }
            add = result.address;
            addMarker(xself, yself, add);


            gdRetunResult = result;
        },
        onFail: function (err) {

            dd_toast('钉钉异常！', 'error', 0);
        }
    });
})

   
   // 添加自己的坐标
   function addMarker(x, y, title) {
       var marker = new AMap.Marker({
           position: [x, y]
       });
       marker.setMap(map);
       var circle = new AMap.Circle({
           center: [x, y],
           radius: 40,
           fillOpacity: 0.2,
           strokeWeight: 1,
           strokeColor: '#6CB1FF',
           fillColor: '#6CB1FF'
       })
       circle.setMap(map);
       map.setFitView()
       var info = new AMap.InfoWindow({
           content: title,
           offset: new AMap.Pixel(0, -22)
       })
       info.open(map, marker.getPosition())
   }

function addCustomMarker(x, y, title) {
    var makr = new AMap.Marker({
        icon: "http://webapi.amap.com/theme/v1.3/markers/n/mark_b.png",
        position: [x, y],
    });
    makr.setMap(map);

    // 设置鼠标划过点标记显示的文字提示
    makr.setTitle(title);

    // 设置label标签
    makr.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
        offset: new AMap.Pixel(-22, -22),//修改label相对于maker的位置
        content: title
    });

    //AMap.addEventListener(makr, MOUCE_CLICK, custommarkonclick);

    //AMap.event.addDomListener(document.getElementById('makr'), 'tap', function () {
    //    alert("2");
    //}, false);
}

function custommarkonclick() {
    dd_toast('点击事件！', 'success', 0);
}

//签到
$('#signbtn').on('tap', function () {
    var sign_cust_id = localStorage.getItem("sign_cust_id");
    var distance = localStorage.getItem("distance");
    
    if (sign_cust_id != null && sign_cust_id != "") {
       
        saveData(userid, username, sign_cust_id, xself, yself, gdRetunResult.address, distance);
    }
    else {
        dd_toast('您未选择客户！', 'error', 0);
    }

    //// 设置label标签
    //markerslef.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
    //    offset: new AMap.Pixel(-22, -22),//修改label相对于maker的位置
    //    content: "我当前的位置(签到成功)"          
    //});


});

function initdata() {
    var data = {
        Func: "cust_customer_nearby",
        cust_users: userid,
        sign_x: xself,
        sign_y: yself,
        guid: userid
    }
    var url = pageurl + '/Custom/cust_customer_handle.ashx';
    getajax(url, data, function (json) {
        if (json.result.errMsg == "success") {
            var retData = json.result.retData[0];
            $("#cust_count").html(retData.cust_count);
        }
    }, function () {
        dd_toast('接口错误，请联系管理员！', 'error', 0);
    })
}

$(".clientele").on('tap', function () {
    var href = "SelectClientele.html?dd_nav_bgcolor=FFFA676F&number=" + Math.random() + "&ss=hggh&xself=" + xself + "&yself=" + yself;
    openwindow(href, href, 'slide-in-right');
})
hidePreloader();
</script>
