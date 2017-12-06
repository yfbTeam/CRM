<%@ Page Title="Home Page" Language="VB"  AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<script src="Scripts/jquery-1.10.2.min.js"></script>
<script>
    function aaa() {
        var data = {
            'grant_type': 'password',
            'username': "2017001",
            'password': "123456"
        };
        $.ajax({
            url: "http://192.168.10.79:9993/token",
            type: "POST",
            data: data,
            dataType: "json",
            success: function (data) {
                alert("1");
                alert(data.access_token);
                bbb(data.access_token);
                //$.cookie("token", data.access_token);
                //getOrders();
            },
            error: function (xmlHttpRequest) {
                alert(xmlHttpRequest.responseJSON.error_description)

            }
        });
    }
    function bbb(token) {
        $.ajax({
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            },
            url: 'http://192.168.10.79:9993/api/employee/getallemps',
            type: "GET",
            dataType: 'json',
            success: function (data) {
                alert("2");
                alert(data);
            }
        });
    }
    $(document).ready(function () {
        aaa();
    });
</script>


