<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JsAPI.aspx.cs" Inherits="Enterprise_JsAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <%--<script type="text/javascript" src="http://g.alicdn.com/ilw/ding/0.5.1/scripts/dingtalk.js"></script>--%>
    <%--<script type="text/javascript" src="http://g.alicdn.com/dingding/open-develop/0.8.4/dingtalk.js"></script>--%>
    <script type="text/javascript" src="http://g.alicdn.com/dingding/open-develop/0.9.2/dingtalk.js"></script>
    <script src="js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="js/public.js"></script>
    <script type="text/javascript">
        var _ServerHttp = {
            uri: '<%=signPackage.ServerUri%>'
        }

        var _config = {
                appId: '<%=signPackage.appId%>',
                corpId: '<%=signPackage.corpId%>',
                timeStamp: '<%=signPackage.timestamp%>',
                nonce: '<%=signPackage.nonceStr%>',
                signature: '<%=signPackage.signature%>',
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

            };

            dd.config({
                appId: _config.appId,
                corpId: _config.corpId,
                timeStamp: _config.timeStamp,
                nonceStr: _config.nonce

            });

            dd.error(function (error) {
                /**
                 {
                    message:"错误信息",//message信息会展示出钉钉服务端生成签名使用的参数，请和您生成签名的参数作对比，找出错误的参数
                    errorCode:"错误码"
                 }
                **/

                alert(JSON.stringify(error));
            });

            dd.ready(function () {

               
                dd.runtime.permission.requestAuthCode({
                    corpId: _config.corpId,
                    onSuccess: function (result) {
                        
                       
                        localStorage.setItem("code", result["code"]);
                        location.href = _ServerHttp.uri + "?code=" + result["code"] + "&mode=mobile&dd_nav_bgcolor=FF6CB1FF&numer=" + Math.random();
                      
                    },
                    onFail: function (err) {
                    }
                });
            });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
