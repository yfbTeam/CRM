<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InsertCustomer.aspx.cs" Inherits="Page_InsertCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
 <head runat="server">
        <meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<title>全部客户</title>
        <link href="/css/reset.css" rel="stylesheet" />
        <link href="../css/style.css" rel="stylesheet" />
        <script src="/Scripts/jquery-1.10.2.min.js"></script>
        <%-- 导航选中教师 --%>
        <script src="/Scripts/OtherJS/menu_top.js"></script>
        <%--公用js--%>
        <script src="/Scripts/OtherJS/Common.js"></script>
        <%-- 弹出层js --%>
        <script src="../Scripts/OtherJS/layer/layer.js"></script>
        <%--模板js--%>
        <script src="/Scripts/OtherJS/jquery.tmpl.js"></script>
        <%--分页--%>
        <script src="/Scripts/OtherJS/PageBar.js"></script>
             <script src="../../Scripts/pgscript/jPages.min.js"></script>
        <link href="../../Scripts/pgscript/jPages.css" rel="stylesheet" />
        <%-- 上传 --%>
     <link href="../Scripts/OtherJS/Uploadyfy/uploadify/uploadify.css" rel="stylesheet" />
     <script src="../Scripts/OtherJS/Uploadyfy/uploadify/jquery.uploadify-3.1.min.js"></script>
     <script src="../Scripts/init.js"></script>
    </head>
<body style="background: #F8FCFF;">
    <div style="padding:20px;">
        <div class="insert—content">
            <div class="row_dia clearfix">
                <div class="clearfix fl">
                    <label for="" class="row_label fl">资源文件:</label>
                    <div class="row_content">
                        <span class="fl data"></span>
                        <div class="upload_imga">
                            <input type="file" name="" id="uploadify" multiple="multiple" />
                        </div>
                    </div>
                </div>
                <div class="clearfix fl ml20">
                    <label for="" class="row_label fl" id="UserName" >负责人名称:</label>
                    <div class="row_content">
                        <input type="text" placeholder="姓名" class="text" onblur="SetFromData()" id="Name" />
                    </div>
                </div>
                <div class="stytem_select_right fl" style="margin:0px 0px 0px 20px;">
                    <a href="javascript:;" class="newcourse" id="icon-plus">
                        提取信息
                    </a>
                </div>
            </div>
            <h1 class="tip" id="tip_" style="display:none">导入提示：<span class="success" id="success">30个成功</span><span class="success" id="fail">4个失败</span><span class="fail" id="fail1" style="display:none">5个失败</span></h1>
            <div class="wrap mt10">
                <table>
                    <thead>
                        <tr>
                            <th width="40px;">编号</th>
                            <th>客户名称</th>
                            <th>负责人</th>
                            <th>导入结果</th>
                        </tr>
                    </thead>
                    <tbody id="msgshow">
                       <%-- <tr>
                            <td>1</td>
                            <td>北京外国语大学</td>
                            <td>李孝利</td>
                            <td class="fail">导入失败</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>北京外国语大学</td>
                            <td>李孝利</td>
                            <td class="repeat">导入重复</td>
                        </tr>--%>
                    </tbody>
                </table>
                <div id="fenyeholder" class="holder">
                <div style="display:none" class="page" id="pageBar">
                    <a href="">1</a>
					<a href="">2</a>
					<a href="">3</a>
					<a href="">4</a>
					<a href="">5</a>
					<a href="" class="on">6</a>
					<a href="">7</a>
					<a href="">8</a>
					<a href="">9</a>
					<a href="">10</a>
					<a href="" class="next">下一页</a>
					<a href="" class="end">尾页</a>
                </div>
		    </div>
        </div>
    </div>
    <script>
       
        function SetFromData()
        {
            $("#uploadify").uploadify("settings", "formData", { 'userName': $("#Name").val() });
          
        }
        $(function () {
            
            $("#uploadify").uploadify(
                {
                'auto': true,                      //是否自动上传
                'swf': '../Scripts/OtherJS/Uploadyfy/uploadify/uploadify.swf',
                'uploader': WebIp+'File/HandlerFileLeadIn.ashx',
                //'formData': { Func: "UplodExcel", userName:"宫天航" }, //参数
                'fileTypeExts': '*.xls;*.xlsx',
                'buttonText': '选择Excel',//按钮文字
                // 'cancelimg': 'uploadify/uploadify-cancel.png',
                'width': 80,
                'height': 32,
                //最大文件数量'uploadLimit':
                'multi': true,//单选            
                'fileSizeLimit': '10MB',//最大文档限制
                'queueSizeLimit': 1,  //队列限制
                'removeCompleted': true, //上传完成自动清空
                'removeTimeout': 0, //清空时间间隔
                //'overrideEvents': ['onDialogClose', 'onUploadSuccess', 'onUploadError', 'onSelectError'],
                'onUploadSuccess': function (file, data, response) 
                {
                    
                    data = $.parseJSON(data);
                    var Result_Success = data.Result_Success;
                    var add_customer_count = Result_Success.add_customer_count ;
                    var add_linkman_count = Result_Success.add_linkman_count;
                    var exit_customer_count = Result_Success.exit_customer_count;
                    var exit_linkman_count = Result_Success.exit_linkman_count;
                    $("#success").text(add_customer_count + "个成功");
                    $("#fail").text(exit_customer_count + "个客户已存在");
                    $("#fail1").text(exit_linkman_count + "个联系人已存在");
                    $("#tip_").show();
                    var Result_Fail = data.Result_Fail;                 
                    BindDataTo_GetAllCustomer(Result_Fail);
                    fenye(1);
                },
               
            });
        });
        function BindDataTo_GetAllCustomer(bindData) {
            $("#msgshow").empty();
            var count = 1;
            $(bindData).each(function () 
            {
                var str =  "<tr>"+
                            "<td>"+count+"</td>"+
                            "<td>"+this.cust_name+"</td>"+
                            "<td>"+this.cust_usersname+"</td>"+
                            "<td class='fail'>" + "导入失败" + this.msg + "</td>" +
                        "</tr>"
                $("#msgshow").append(str);

                count++;
            });
        }
        //分页
        var currentpage = 1;
        var allpages;//没用
        var addpage;
        var deletepage;
        function fenye(cp) {
            $("div.holder").jPages({
                containerID: "msgshow",
                first: '首页',
                last: '尾页',
                previous: "上一页",
                next: "下一页",
                perPage: 10,
                delay: 0,
                startPage: cp,
                callback: function (pages, items) {
                    currentpage = pages.current;
                    allpages = pages.count;
                    //"当前页面:" + pages.current);
                    //"页面总数:" + pages.count);
                    //"总数量:" + items.count);
                    //"每页数量:" + items.count / pages.count);
                    addpage = parseInt(items.count / 10) + 1;
                    if ((items.count - 1) % 10 == 0) {
                        deletepage = pages.current - 1;
                    }
                    else {
                        deletepage = pages.current;
                    }
                }
            });
        }
    </script>
</body>
</html>
