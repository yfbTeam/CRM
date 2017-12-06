<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllLinkman.aspx.cs" Inherits="Page_Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<title>全部联系人</title>
        <link href="../css/iconfont.css" rel="stylesheet" />
        <link href="../css/reset.css" rel="stylesheet" />
        <link href="../css/style.css" rel="stylesheet" />
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <%-- 导航选中教师 --%>
        <script src="../Scripts/OtherJS/menu_top.js"></script>
        <%--公用js--%>
        <script src="../Scripts/OtherJS/Common.js"></script>
        <%-- 弹出层js --%>
        <script src="../Scripts/OtherJS/layer/layer.js"></script>
        <%--模板js--%>
        <script src="../Scripts/OtherJS/jquery.tmpl.js"></script>
        <%--分页--%>
        <script src="../Scripts/OtherJS/PageBar.js"></script>
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
					<li  currentClass="active"><a href="AllCustomer.aspx">全部客户</a></li>
					<li  currentClass="active"><a href="AllLinkman.aspx">全部联系人</a></li>
					<li  currentClass="active"><a href="salerkit.aspx">销售简报</a></li>
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
								徐世红
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
					<select name="" id="companyname" class="select">
                      <%-- <option value="index">为筛选</option>--%>
						<option value="1">北京圣邦天麒科技有限公司</option>
                       <%--<option value="0">北京华人启星科技有限公司</option>--%>
					</select>
				</div>
                <div class="clearfix fl course_select">
					<label for="">联系人等级：</label>
					<select name="" id="LinkManLevel" class="select" onchange="SelectDataTest()">
                        <option value="index">未筛选</option>
						<option value="A类（VIP）">A类（VIP）</option>
                        <option value="B类（优秀）">B类（优秀）</option>
                        <option value="C类（普通）">C类（普通）</option>
                        <option value="D类（潜在）">D类（潜在）</option>
					</select>
				</div>								
                <div class="clearfix fl course_select">
					<label for="">销售负责人：</label>
					<select name="" id="department" class="select" onchange="GetUserByDepartment()">
                        <option value="0">部门</option>
						<option value="1">普教一部</option>
                        <option value="2">普教二部</option>
                        <option value="3">普教三步</option>
                        <option value="4">高教一部</option>
                        <option value="5">高教二部</option>
                         <option value="6">高教三部</option>
					</select>
                    <select name="" id="user" class="select ml10" onchange="SelectDataTest()">
						<option value="index">人员</option>
                        
					</select>
				</div>
				<div class="fl pr search ml10">
                    <input type="text" name="" id="KeyName" value="" placeholder="请输入关键字" />
                    <i class="iconfont icon-sousuo_sousuo" onclick="SelectDataByKey()"></i>
                    
                </div>
			</div>
            <div class="wrap mt10">
                <table>
                    <thead>
                        <tr>
                            <th width="40px;">编号</th>
                            <th>联系人</th>
                            <th>客户名称</th>
                            <th>电话</th>
                            <th>联系人等级</th>
                            <th>销售负责人</th>
                            <th>上次跟进时间</th>
                        </tr>
                    </thead>
                    <tbody id="ShowLinkManInfo">
                       
                    </tbody>
                </table>  
                 <div id="fenyeholder" class="holder"></div>          
		    </div>
        </div>
    </div>
	 <footer id="footer">
		<div class="footer clearfix width">
			<div class="footer_right">
				<p>咨询电话: 010-83068508&nbsp;&nbsp;地址: 北京市丰台区南四环西路128号诺德中心3座7层 </p>
			</div>
		</div>
	</footer>
</body>
</html>
  <script>
      var Guid = "0C2D5119-85D2-4DFC-AB93-9C2F1125BF56";
      //分页
      var currentpage = 1;
      var allpages;//没用
      var addpage;
      var deletepage;
      function fenye(cp) {
          $("div.holder").jPages({
              containerID: "ShowLinkManInfo",
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
      //初始化用户
      function InitUser() {

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
                  alert("失败");
              }
          });
      }
     
      //绑定联系人等级
      var Map_LinkManLevel = new Array();
      function InitLinkManLevel()
      {
          Map_LinkManLevel = [];
          var postData = { func: "get_pub_param", pub_title: "联系人级别", guid: Guid };
          $.ajax({
              type: "Post",
              url: WebIp + "PubParam/pub_param_handle.ashx",
              data: postData,
              dataType: "json",
              async: false,
              success: function (returnVal) {
                  if (returnVal.result.errMsg == "success") {
                      BindDataTo_InitLinkManLevel(returnVal.result.retData);
                  }
              },
              error: function (errMsg) {
                  alert("失败");
              }
          });
      }
      //绑定联系人等级
      function BindDataTo_InitLinkManLevel(bindData) {
          $("#LinkManLevel").empty();
          var index = "<option value='index'>" + "未筛选" + "</option>";
          $("#LinkManLevel").append(index);

          $(bindData).each(function ()
          {
              Map_LinkManLevel[this.pub_value] = this.pub_title;
              var str = "<option value='" + this.pub_title + "'>" + this.pub_title + "</option>";
              $("#LinkManLevel").append(str);
          });
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

                  if (returnVal.result.errMsg == "success") {
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
      
      //首次加载获取数据
      var CachbindData;
      function GetAllLinkMan() {
          var postData = { func: "Get_AllLinkMan"};
          $.ajax({
              type: "Post",
              url: WebIp + "/LinkMan/AllLinkManHandler.ashx",
              data: postData,
              dataType: "json",
              async: false,
              success: function (returnVal)
              {
                  if (returnVal.result.errMsg == "success") {
                      //alert(JSON.stringify(returnVal.result.retData))
                      CachbindData = returnVal.result.retData;
                      BindDataTo_GetAllLinkMan(returnVal.result.retData);

                  }
              },
              error: function (errMsg) {
                  alert("失败2");
              }
          });
      }
      //绑定首次加载获取数据
      function BindDataTo_GetAllLinkMan(bindData) {
          $("#ShowLinkManInfo").empty();
          var count = 1;
          $(bindData).each(function ()
          {
              if (this.link_name == null) {
                  this.link_name = "";
              }
              if (this.link_cust_name == null) {
                  this.link_cust_name = "";
              }
              if (this.link_telephone == null) {
                  this.link_telephone = "";
              }
              if (this.link_level == null) {
                  this.link_level = "";
              }
              if (this.link_usersname == null) {
                  this.link_usersname = "";
              }           
              var str = "<tr>" +
                           "<td>" + count + "</td>" +
                            "<td>" + this.link_name + "</td>" +
                           "<td>" + this.link_cust_name + "</td>" +
                           "<td>" + this.link_telephone + "</td>" +
                            "<td>" + this.link_level + "</td>" +
                           "<td>" + this.link_usersname + "</td>" +
                            "<td>" + data_string(this.follow_date) + "</td>" +
                           "</tr>";
              $("#ShowLinkManInfo").append(str);
           
              count++;
          });
          fenye(1);
      }
      //根据条件查询数据
      function SelectDataTest()
      {
        ;
          var LinkManLevel = $("#LinkManLevel").val();
          var department = $("#department").val();
          var user = $("#user").val();
          
       
          var arrRes = Enumerable.From(CachbindData).Where(function (i)
          {
             
              //各个条件
              var flg = true;
              //联系人等级筛选
              var LinkManLevel = $("#LinkManLevel").val();
              if (LinkManLevel != "index" && LinkManLevel != i.link_level) {
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
              //    if (cn != "0" && (i.OrganNo == "22" || i.OrganNo == "22" || i.OrganNo == "22")) {
              //        flg = false;
              //    }
              //    if (cn == "0" && (i.OrganNo != "22" && i.OrganNo != "22" && i.OrganNo != "22")) {
              //        flg = false;
              //    }
              //}
              if (flg) { return i}
          }).ToArray();
      
        
          BindDataTo_GetAllLinkMan(arrRes);
          //关键字筛选项设为空
          $("#KeyName").val("");
      }
     
      //根据关键字查询数据
      function SelectDataByKey()
      {
          $("#ShowLinkManInfo").empty();
          var count = 1;
          var KeyName = $("#KeyName").val();
          $(CachbindData).each(function () {
                      
              if (this.link_name.indexOf(KeyName) != -1 || this.link_cust_name.indexOf(KeyName) != -1 || this.link_telephone.indexOf(KeyName) != -1
                  || this.link_level.indexOf(KeyName) != -1 || this.link_usersname.indexOf(KeyName) != -1)
              {
                  var str = "<tr>" +
                "<td>" + count + "</td>" +
                            "<td>" + this.link_name + "</td>" +
                           "<td>" + this.link_cust_name + "</td>" +
                           "<td>" + this.link_telephone + "</td>" +
                            "<td>" + this.link_level + "</td>" +
                           "<td>" + this.link_usersname + "</td>" +
                           "<td>" + data_string(this.follow_date) + "</td>" +
                "</tr>";
                  $("#ShowLinkManInfo").append(str);
                  count++;
              }
          });
          fenye(1);
          //其他筛选项设置为未筛选
          $("#LinkManLevel").val("index");
          $("#department").val("index");
          $("#user").val("index");
      }
      function GetinfoByGSname() {
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
          else {
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
      $(document).ready(function () {
          ////初始化用户
          //InitUser();
          ////绑定客户级别
          //InitLinkManLevel();        
          ////绑定部门     
          //Initdepartment();
          //展示主数据
          Get_saleDepartment();
          GetAllLinkMan();
      });

      //时间格式化
      function data_string(str)
      {
          if (str == null) { return ""; }
          var d = eval('new ' + str.substr(1, str.length - 2));
          var ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
          for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
          return ar_date.join('-');
          function dFormat(i) { return i < 10 ? "0" + i.toString() : i; }
      }

    </script>
