<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllCustomer.aspx.cs" Inherits="Page_AllCustomer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<title>全部客户</title>
        <link href="../css/iconfont.css" rel="stylesheet" />
        <link href="../css/reset.css" rel="stylesheet" />
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
        <script src="../Scripts/pgscript/jPages.min.js"></script>
        <link href="../Scripts/pgscript/jPages.css" rel="stylesheet" />
        <script src="../Scripts/init.js"></script>
        <script src="../Scripts/linq.js"></script>
    </head>
<body>
    <!--header-->
	<header class="repository_header_wrap manage_header">
		<div class="width repository_header clearfix">
			<a href="" class="logo fl"><img src="../images/logo.png" /></a>
			<div class="testsystem  fl"></div>
			<nav class="navbar menu_mid  fl">
				<ul>
					<li  class="active"><a href="AllCustomer.aspx">全部客户</a></li>
					<li><a href="AllLinkman.aspx">全部联系人</a></li>
					<li><a href="salerkit.aspx">销售简报</a></li>
				</ul>
			</nav>
			<div class="search_account fr clearfix">
				<ul class="account_area fl">
					<li>
						<a href="javascript:;" class="login_area clearfix">
							<div class="avatar">
								<img src="../images/3.jpg" />
							</div>
							<h2 id="Ln">
								管理员
							</h2>
						</a>
					</li>
				</ul>
				<div class="settings fl">
					<a href="javascript:;">
						<i class="iconfont icon-shezhi"></i>
					</a>
                    <div class="setting_none">
                        <span onclick="logOut()">退出</span>
                    </div>
				</div>
			</div>
		</div>
	</header>
    <div class="wrap_content width pt90">
        <div class="course_manage bordshadrad">
            <div class="newcourse_select clearfix">
                <div class="clearfix fl course_select">
					<label for="">公司名称：</label>
					<select name="" id="companyname" class="select" >
                     <%--  <option value="index">为筛选</option>--%>
						<option value="1">北京圣邦天麒科技有限公司</option>
              <%--         <option value="0">北京华人启星科技有限公司</option>--%>
					</select>
				</div>
				<div class="clearfix fl course_select">
					<label for="">客户等级：</label>
					<select name="" id="customerlevel" class="select" onchange="SelectDataTest()">
						<option value="0">VIP客户</option>
                        <option value="1">普通客户</option>
                        <option value="2">开拓客户</option>
					</select>
				</div>
				<div class="clearfix fl course_select">
					<label for="">客户分类：</label>
					<select name="" id="customerclass" class="select" onchange="SelectDataTest()">
						<option value="0">学校</option>
                        <option value="1">集成商</option>
                        <option value="2">经销商</option>
                        <option value="3">供应商</option>
                        <option value="4">政府</option>
                         <option value="5">医疗</option>
					</select>
				</div>
                <div class="clearfix fl course_select">
					<label for="">销售负责人：</label>
					<select name="" id="department" class="select" onchange="GetUserByDepartment()">
                        <option value="index">部门未筛选</option>
						<option value="普教一部">普教一部</option>
                        <option value="普教二部">普教二部</option>
                        <option value="普教三部">普教三部</option>
                        <option value="高教一部">高教一部</option>
                        <option value="高教二部">高教二部</option>
                         <option value="高教三部">高教三部</option>
					</select>
                    <select name="" id="user" class="select ml10" onchange="SelectDataTest()">
						<option value="index">人员</option>
                        
					</select>
				</div>
				<div class="fl pr search ml10">
                    <input type="text" name="" id="KeyName" value="" placeholder="请输入关键字" />
                    <i class="iconfont icon-sousuo_sousuo" onclick="SelectDataByKey()"></i>
                </div>
                <div class="stytem_select_right fr">
                    <a href="javascript:;" class="newcourse" onclick="insertCustomer()" id="icon-plus">
                        导入客户
                    </a>
                </div>
			</div>
            <div class="wrap mt10">
                <table>
                    <thead>
                        <tr>
                           <th width="40px;">编号</th>
                            <th>客户名称</th>
                            <th>联系人</th>
                            <th>联系人电话</th>
                            <th>客户分类</th>
                            <th>客户等级</th>
							<th>销售负责人</th>
							<th>上次跟进时间</th>
                        </tr>
                    </thead>
                    <tbody id="ShowCustomerInfo">
                        
                    </tbody>
                </table>
                <div id="fenyeholder" class="holder">

                </div>
                <div class="page" id="pageBar" style="display:none">
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

        //jQuery.support.cors = true;       
        var Guid = "0C2D5119-85D2-4DFC-AB93-9C2F1125BF56";
        //分页
        var currentpage = 1;
        var allpages;//没用
        var addpage;
        var deletepage;
        function fenye(cp) {
            $("div.holder").jPages({
                containerID: "ShowCustomerInfo",
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
        //用户初始化
        function InitUser()
        {
            
            var postData = { func: "DataInit", cust_users: Guid };
            $.ajax({
                type: "Post",
                url: WebIp + "Statistical/statistic_handle.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal) {
                  
                },
                error: function (errMsg) {
                    alert("失败1");
                }
            });
        }
        //绑定客户级别
        function Initcustomerlevel() {

            var postData = { func: "get_pub_param", pub_title: "客户级别", guid: Guid };
            $.ajax({
                type: "Post",
                url: WebIp + "PubParam/pub_param_handle.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal) {
                    if (returnVal.result.errMsg == "success") {
                        BindDataTo_Initcustomerlevel(returnVal.result.retData);
                    }
                },
                error: function (errMsg) {
                    alert("失败2");
                }
            });
        }
        function BindDataTo_Initcustomerlevel(bindData) {
            $("#customerlevel").empty();
            var index = "<option value='index'>" + "未筛选" + "</option>";
            $("#customerlevel").append(index);

            $(bindData).each(function () {
                var str = "<option value='" + this + "'>" + this + "</option>";
                $("#customerlevel").append(str);
            });
        }
      
        //绑定客户类型
        function Initcustomerclass() {
            var postData = { func: "get_pub_param", pub_title: "客户类型", guid: Guid };
            $.ajax({
                type: "Post",
                url: WebIp + "PubParam/pub_param_handle.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal) {
                    if (returnVal.result.errMsg == "success") {
                        BindDataTo_Initcustomerclass(returnVal.result.retData);
                    }
                },
                error: function (errMsg) {
                    alert("失败3");
                }
            });
        }
        function BindDataTo_Initcustomerclass(bindData)
        {
            $("#customerclass").empty();
            var index = "<option value='index'>" + "未筛选" + "</option>";
            $("#customerclass").append(index);

            $(bindData).each(function () {
                var str = "<option value='" + this + "'>" + this + "</option>";
                $("#customerclass").append(str);
            });
          
        }
        var arr = new Array();
       
      
        var arr_user = [];
        function GetUserinfoByDepartMent()
        {
            arr_user = [];
            var department = $("#department").val();
            $("#user").empty();
            var str = "<option value='index'>" + "未筛选" + "</option>";
            $("#user").append(str);
            $(arr[department]).each(function ()
            {
                arr_user.push(this.Name);
                var str = "<option value='" + this.Name + "'>" + this.Name + "</option>";
                $("#user").append(str);
               
            });
            //筛选
            SelectData();
        }
          
        var CachbindData;
        //根据条件查询数据
        function SelectData()
        {
        
            $("#ShowCustomerInfo").empty();
            var count = 1;
            $(CachbindData).each(function ()
            {
                var str = "<tr>" +
                            "<td>" + count + "</td>" +
                             "<td>" + this.cust_name + "</td>" +
                            "<td>" + this.link_name + "</td>" +
                            "<td>" + this.link_telephone + "</td>" +
                             "<td>" + this.cust_type + "</td>" +
                            "<td>" + this.cust_level + "</td>" +
                            "<td>" + this.cust_usersname + "</td>" +
                            "<td>" + data_string(this.follow_date) + "</td>" +
                            "</tr>";
             
                //各个条件
                var flg = true;
                alert(flg)
                //客户级别筛选
                var customerlevel = $("#customerlevel").val();
                //alert(customerlevel);
                //alert(this.cust_level);
                if (customerlevel != "index" && customerlevel != this.cust_level)
                {
                    flg = false;
                }
                //alert("客户级别筛选"+flg);
                //客户类型筛选
                var customerclass = $("#customerclass").val();
                //alert(customerclass);
                //alert(this.cust_type);
                if (customerclass != "index" && customerclass != this.cust_type)
                {
                    flg = false;
                }
                //alert("客户类型筛选" + flg);
                //部门人员筛选
                var department = $("#department").val();
                //alert(department);
                if (department != "index" && $.inArray(this.cust_usersname, arr_user) == -1) {
                    flg = false;
                }
                //alert("部门人员筛选" + flg);
                //人员筛选
                var user = $("#user").val();
                if ( user != "index" && user != this.cust_usersname) {
                    flg = false;
                }
                if (flg)
                {
                    
                    $("#ShowCustomerInfo").append(str);
                    count++;
                }            
               
            });
            fenye(1);
            //关键字筛选项设为空
            $("#KeyName").val("");
        }
        //根据关键字查询数据
        function SelectDataByKey() {
            $("#ShowCustomerInfo").empty();
            var count = 1;
            var KeyName = $("#KeyName").val();
            $(CachbindData).each(function () {

                if (this.cust_name.indexOf(KeyName) != -1 || this.link_name.indexOf(KeyName) != -1 || this.link_telephone.indexOf(KeyName) != -1
                    || this.cust_type.indexOf(KeyName) != -1 || this.cust_level.indexOf(KeyName) != -1 || this.cust_usersname.indexOf(KeyName) != -1
                    || this.follow_date.indexOf(KeyName) != -1) {
                    var str = "<tr>" +
                                 "<td>" + count + "</td>" +
                              "<td>" + this.cust_name + "</td>" +
                             "<td>" + this.link_name + "</td>" +
                             "<td>" + this.link_telephone + "</td>" +
                              "<td>" + this.cust_type + "</td>" +
                             "<td>" + this.cust_level + "</td>" +
                             "<td>" + this.cust_usersname + "</td>" +
                             "<td>" + data_string(this.follow_date) + "</td>" +
                               "</tr>";
                    $("#ShowCustomerInfo").append(str);
                    count++;
                }
            });
            fenye(1);
            //其他筛选项设置为未筛选
            $("#customerlevel").val("index");
            $("#customerclass").val("index");
            $("#department").val("index");
            $("#user").val("index");
        }
        //时间格式化
        function data_string(str)
        {
            if (str == null || str == "") { return "";}
            var d = eval('new ' + str.substr(1, str.length - 2));
            var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
            for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
            return ar_date.join('-');

            function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
        }
        $(document).ready(function ()
        {
            ////初始化用户
            //InitUser();
            //绑定客户级别
            //Initcustomerlevel();
            //绑定客户类型
            //Initcustomerclass();
            ////绑定部门    
            Get_saleDepartment();
            ////获取客户
            Getall();
            
        });

       
        function insertCustomer() {
            OpenIFrameWindow('导入用户', 'InsertCustomer.aspx', '800px', '600px');
        }
        //绑定销售负责人
        function GetUserByDepartment() {
            var department = $("#department").val();
            var postData = { func: "Get_CustomerByDepartment", DepartmentId: department };
            $.ajax({
                type: "Post",
                url: WebIp + "/Customer/AllCustomerHandler.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal) {

                    if (returnVal.result.errMsg == "success") {
                       
                        BindDataTo_GetUserinfoByDepartMent(returnVal.result.retData);
                    }
                },
                error: function (errMsg) {
                    alert("失败");
                }
            });
        }
        function BindDataTo_GetUserinfoByDepartMent(bindData) {
            var department = $("#department").val();
            $("#user").empty();
            var str = "<option value='index'>" + "未筛选" + "</option>";
            $("#user").append(str);
            $(bindData).each(function () {
                var str = "<option value='" + this + "'>" + this + "</option>";
                $("#user").append(str);

            });            //筛选
            SelectDataTest();
        }
        //绑定销售负责人部门
        function Get_saleDepartment() {

            var postData = { func: "Get_saleDepartment", guid: Guid };
            $.ajax({
                type: "Post",
                url: WebIp + "/Customer/AllCustomerHandler.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal) {

                    if (returnVal.result.errMsg == "success")
                    {
           
                        BindDataTo_Initdepartment(returnVal.result.retData);
                       
                    }
                },
                error: function (errMsg) {
                    alert("失败");
                }
            });
        }
       
        function BindDataTo_Initdepartment(bindData) {
            $("#department").empty();
            var index = "<option value='index'>" + "未筛选" + "</option>";
            $("#department").append(index);

            $(bindData).each(function () {
                var str = "<option value='" + this.id + "'>" + this.name + "</option>";
                $("#department").append(str);              
            });
        }
        //首次加载
        function Getall()
        {
            var postData = { func: "Get_AllCustomer"};
            $.ajax({
                type: "Post",
                url: WebIp + "/Customer/AllCustomerHandler.ashx",
                data: postData,
                dataType: "json",
                async: false,
                success: function (returnVal)
                {
                    
               
                    if (returnVal.result.errMsg == "success")
                    {
                        CachbindData = returnVal.result.retData.AllCustomer
                        //绑定客户信息
                        BindDataTo_GetAllCustomer1(returnVal.result.retData.AllCustomer);
                        //绑定cust_level
                        BindDataTo_Initcustomerlevel(returnVal.result.retData.cust_level)
                        //绑定cust_type
                        BindDataTo_Initcustomerclass(returnVal.result.retData.cust_type)
                    }

                    //CachbindData=returnVal;
                    //BindDataTo_GetAllCustomer1(returnVal)
                    //fenye(1);
                },
                error: function (errMsg) {
                    alert("失败4");
                }
            });
        }
        function GetinfoByGSname()
        {
            var cn = $("#companyname").val();
            if (cn == "0") {

                $("#department").empty();
                var index = "<option value='index'>" + "未筛选" + "</option>";
                $("#department").append(index);
                var str = "<option value='23'>销售三部</option>";
                $("#department").append(str);
                str = "<option value='22'>销售一部</option>";
                $("#department").append(str);
                str = "<option value='26'>销售二部</option>";
                $("#department").append(str);


            }
            else
            {
                $("#department").empty();
                var index = "<option value='index'>" + "未筛选" + "</option>";
                $("#department").append(index);
                var str = "<option value='05'>普教一部</option>";
                $("#department").append(str);
                str = "<option value='06'>普教二部</option>";
                $("#department").append(str);
                str = "<option value='07'>普教三部</option>";
                $("#department").append(str);
                str = "<option value='09'>高教一部</option>";
                $("#department").append(str);
                str = "<option value='10'>高教二部</option>";
                $("#department").append(str);
                str = "<option value='11'>高教三部</option>";
                $("#department").append(str);
            }
            SelectDataTest();
        }
        function BindDataTo_GetAllCustomer1(bindData)
        {
            $("#ShowCustomerInfo").empty();
            var count = 1;
            $(bindData).each(function ()
            {
                var follow_date = this.follow_date
                if (follow_date == null) {
                    follow_date = "";
                }
                else
                {
                    follow_date = data_string(this.follow_date);
                }
                if (this.cust_name == null) {
                    this.cust_name = "";
                }
                if (this.link_name == null) {
                    this.link_name = "";
                }
                if (this.link_telephone == null) {
                    this.link_telephone = "";
                }
                if (this.cust_type == null) {
                    this.cust_type = "";
                }
                if (this.cust_level == null) {
                    this.cust_level = "";
                }
                if (this.cust_usersname == null) {
                    this.cust_usersname = "";
                }
                if (this.follow_date == null) {
                    this.follow_date = "";
                }
                var str = "<tr>" +
                             "<td>" + count + "</td>" +
                              "<td>" + this.cust_name + "</td>" +
                             "<td>" + this.link_name + "</td>" +
                             "<td>" + this.link_telephone + "</td>" +
                              "<td>" + this.cust_type + "</td>" +
                             "<td>" + this.cust_level + "</td>" +
                             "<td>" + this.cust_usersname + "</td>" +
                             //"<td>" + this.OrganNo + "</td>" +
                               "<td>" + follow_date + "</td>" +
                             "</tr>";
                $("#ShowCustomerInfo").append(str);
                count++;
            });
            fenye(1)
        }
        //根据条件查询数据
        function SelectDataTest() {       
            //var customerlevel = $("#customerlevel").val();
            //var customerclass = $("#customerclass").val();
            //var department = $("#department").val();
            //var user = $("#user").val();          
            var arrRes = Enumerable.From(CachbindData).Where(function (i) {
                //各个条件
                var flg = true;               
                var customerlevel = $("#customerlevel").val();
                //alert(customerlevel + "|" + i.cust_level);
                if (customerlevel != "index" && customerlevel != i.cust_level) {
                    flg = false;
                }               
                var customerclass = $("#customerclass").val();               
                if (customerclass != "index" && customerclass != i.cust_type) {
                    flg = false;
                }
                var department = $("#department").val();
             
                if (department != "index" && department != i.OrganNo) {
                    flg = false;
                }
                var user = $("#user").val();
                if (user != "index" && user != i.cust_usersname) {
                    flg = false;
                }
                //公司
                //var cn = $("#companyname").val();
                //if (cn != "index") {
                //    if (cn != "0" && (i.OrganNo == "22" || i.OrganNo == "22" || i.OrganNo == "22"))
                //    {
                //        flg = false;
                //    }                  
                //    if (cn == "0" && (i.OrganNo != "22" && i.OrganNo != "22" && i.OrganNo != "22")) {
                //        flg = false;
                //    }
                //}
                if (flg) { return i }
            }).ToArray();


            BindDataTo_GetAllCustomer1(arrRes);
            //关键字筛选项设为空
            $("#KeyName").val("");
        }

    </script>
</body>
</html>
