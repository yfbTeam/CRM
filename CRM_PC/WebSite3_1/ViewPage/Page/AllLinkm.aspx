<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllLinkm.aspx.cs" Inherits="Page_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<title>全部联系人</title>
        <link href="../css/iconfont.css" rel="stylesheet" />
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
        <link href="../../Scripts/table/jquery.dataTables.min.css" rel="stylesheet" />
        <script src="../../Scripts/table/jquery.dataTables.min.js"></script>
    </head>
<body>
    <!--header-->
	<header class="repository_header_wrap manage_header">
		<div class="width repository_header clearfix">
			<a href="" class="logo fl"><img src="../images/logo.png" /></a>
			<div class="testsystem  fl"></div>
			<nav class="navbar menu_mid  fl">
				<ul>
					<li ><a href="AllCustomer.aspx">全部客户</a></li>
					<li  class="active"><a href="AllLinkman.aspx">全部联系人</a></li>
					<li><a href="salerkit.aspx">销售简报</a></li>
				</ul>
			</nav>
			<div class="search_account fr clearfix">
				<ul class="account_area fl">
					<li>
						<a href="javascript:;" class="login_area clearfix">
							<div class="avatar">
								<img src="images/teacher_img.png"/>
							</div>
							<h2>
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
					<select name="" class="select">
						<option value="0">北京圣邦天麒科技有限公司</option>
                        <option value="1">北京华人启星科技有限公司</option>
					</select>
				</div>
                <div class="clearfix fl course_select">
					<label for="">联系人等级：</label>
					<select name="" id="LinkManLevel" class="select" onchange="SelectData()">
						<option value="0">A类（VIP）</option>
                        <option value="1">B类（优秀）</option>
                        <option value="2">C类（普通）</option>
                        <option value="3">D类（潜在）</option>
					</select>
				</div>
								
                <div class="clearfix fl course_select">
					<label for="">销售负责人：</label>
					<select name="" id="department" class="select" onchange="GetUserinfoByDepartMent()">
                        <option value="0">部门</option>
						<option value="1">普教一部</option>
                        <option value="2">普教二部</option>
                        <option value="3">普教三步</option>
                        <option value="4">高教一部</option>
                        <option value="5">高教二部</option>
                         <option value="6">高教三部</option>
					</select>
                    <select name="" id="user" class="select ml10" onchange="SelectData()">
						<option value="index">人员</option>
                        
					</select>
				</div>
				<div class="fl pr search ml10">
                    <input type="text" name="" id="Name" value="" placeholder="请输入关键字" />
                    <i class="iconfont icon-sousuo_sousuo" onclick="getData(1,18)"></i>
                </div>

			</div>
            <div class="wrap mt10">
                <table id="ex">
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
                        <tr>
                            <td>1</td>
                            <td>孙开明</td>
                            <td>北京外国语大学</td>
                            <td>15802324387</td>
                            <td> A类（VIP）</td>
                            <td>李孝利</td>
                            <td>2016年11月12日</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>孙开明</td>
                            <td>北京外国语大学</td>
                            <td>15802324387</td>
                            <td> A类（VIP）</td>
                            <td>李孝利</td>
                            <td>2016年11月12日</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>孙开明</td>
                            <td>北京外国语大学</td>
                            <td>15802324387</td>
                            <td> A类（VIP）</td>
                            <td>李孝利</td>
                            <td>2016年11月12日</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>孙开明</td>
                            <td>北京外国语大学</td>
                            <td>15802324387</td>
                            <td> A类（VIP）</td>
                            <td>李孝利</td>
                            <td>2016年11月12日</td>
                        </tr>
                        <tr>
                            <td>1</td>
                            <td>孙开明</td>
                            <td>北京外国语大学</td>
                            <td>15802324387</td>
                            <td> A类（VIP）</td>
                            <td>李孝利</td>
                            <td>2016年11月12日</td>
                        </tr>
                    </tbody>
                </table>
                <input type="button" onclick="SelectWhere()" value="测试" />
                <input type="button" onclick="SetSelect()" value="测试" />
                 <div id="fenyeholder" class="holder">
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
      function Initcompanyname() {
      }
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
      
      function BindDataTo_InitLinkManLevel(bindData) {
          $("#LinkManLevel").empty();
          var index = "<option value='index'>" + "未筛选" + "</option>";
          $("#LinkManLevel").append(index);

          $(bindData).each(function ()
          {
              Map_LinkManLevel[this.pub_value] = this.pub_title;
              var str = "<option value='" + this.pub_value + "'>" + this.pub_title + "</option>";
              $("#LinkManLevel").append(str);
          });
      }
      var arr = new Array();
      //绑定销售负责人部门
      function Initdepartment() {
          arr = [];
          var postData = { func: "GetDepartMent", guid: Guid };
          $.ajax({
              type: "Post",
              url: WebIp + "Statistical/statistic_handle.ashx",
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
              var str = "<option value='" + this.ID + "'>" + this.Name + "</option>";
              $("#department").append(str);
              arr[this.ID] = this.UserInfo_List;
          });
      }
      function GetUserinfoByDepartMent() {
          var department = $("#department").val();
          $("#user").empty();
          var str = "<option value='index'>" + "无筛选" + "</option>";
          $("#user").append(str);
          $(arr[department]).each(function () {
              var str = "<option value='" + this.Name + "'>" + this.Name + "</option>";
              $("#user").append(str);

          });
      }
      function SelectCustomerByWhere() {
          //公司名称
          var companyname = $("#companyname").val();
          //客户等级
          var customerlevel = $("#customerlevel").val();
          //客户分类
          var customerclass = $("#customerclass").val();
          //部门
          var department = $("#department").val();
          //人员
          var user = $("#user").val();

          alert(companyname + "," + customerlevel + "," + customerclass + "," + user);
          //根据条件客户信息
          function GetCustomerinfos() {
              var postData = {};
              $.ajax({
                  type: "Get",
                  url: "url",
                  data: postData,
                  dataType: "json",
                  success: function (returnVal) {
                      //alert(returnVal);
                      returnVal = $.parseJSON(returnVal);
                      BindDataToUserInfo(returnVal);
                      //fenye(1);
                  },
                  error: function (errMsg) {
                      alert("失败");
                  }
              });
          }
      }
      //首次加载
      var CachbindData;
      function GetAllCustomer() {
          var postData = { func: "get_cust_linkman_list", guid: Guid, PageIndex: "1", PageSize: "1000000" };
          $.ajax({
              type: "Post",
              url: WebIp + "LinkMan/cust_linkman_handle.ashx",
              data: postData,
              dataType: "json",
              async: false,
              success: function (returnVal) {
              
                  if (returnVal.result.errMsg == "success")
                  {
                      CachbindData = returnVal.result.retData;
                      BindDataTo_GetAllCustomer(returnVal.result.retData)
                      //fenye(1);
                  }
              },
              error: function (errMsg) {
                  alert("失败2");
              }
          });
      }
      function BindDataTo_GetAllCustomer(bindData) {
          $("#ShowLinkManInfo").empty();
          var count = 1;
          $(bindData).each(function ()
          {          
              var str = "<tr>" +
                           "<td>" + count + "</td>" +
                            "<td>" + this.link_name + "</td>" +
                           "<td>" + this.link_cust_name + "</td>" +
                           "<td>" + this.link_telephone + "</td>" +
                            "<td>" + Map_LinkManLevel[this.link_level] + "</td>" +
                           "<td>" + this.link_usersname + "</td>" +
                           "<td>" + "跟进时间" + "</td>" +                        
                           "</tr>";
              $("#ShowLinkManInfo").append(str);
           
              count++;
          });
      }
      //根据条件查询数据
      function SelectData()
      {
          $("#ShowLinkManInfo").empty();
          var count = 1;
          $(CachbindData).each(function ()
          {
             
              //各个条件
              var flg = true;
              //联系人等级筛选
              var LinkManLevel = $("#LinkManLevel").val();      
              if (LinkManLevel != "index" && LinkManLevel != this.link_level) {
                  flg = false;
              }
              //人员筛选
              var user = $("#user").val();
              if (user != "index" && user != this.link_usersname) {
                  flg = false;
              }
              if (flg)
              {
                  
                  var str = "<tr>" +
                "<td>" + count + "</td>" +
                 "<td>" + this.link_name + "</td>" +
                "<td>" + this.link_cust_name + "</td>" +
                "<td>" + this.link_telephone + "</td>" +
                 "<td>" + Map_LinkManLevel[this.link_level] + "</td>" +
                "<td>" + this.link_usersname + "</td>" +
                "<td>" + "跟进时间" + "</td>" +
                "</tr>";
                  $("#ShowLinkManInfo").append(str);
                  count++;
              }

          });
          //fenye(1);
      }
      function SelectWhere()
      {
          var aaa = $("input[type='search']").val();
          alert(aaa);
      }
      function SetSelect()
      {
         $("input[type='search']").val("B");
          
      }
      $(document).ready(function () {
          //////初始化用户
          //InitUser();
          //////绑定客户级别
          //InitLinkManLevel();
          //GetAllCustomer();
      
          //$('#ex').dataTable({
          //    "bFilter": true,
          //    "oLanguage": {    
          //        "sLengthMenu": "每页显示 _MENU_ 条记录",    
          //        "sZeroRecords": "对不起，查询不到任何相关数据",    
          //        "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",    
          //        "sInfoEmtpy": "找不到相关数据",    
          //        "sInfoFiltered": "数据表中共为 _MAX_ 条记录)",    
          //        "sProcessing": "正在加载中...",    
          //        "sSearch": "搜索",    
          //        "sUrl": "", //多语言配置文件，可将oLanguage的设置放在一个txt文件中，例：Javascript/datatable/dtCH.txt    
          //        "oPaginate": {    
          //            "sFirst":    "第一页",    
          //            "sPrevious": " 上一页 ",    
          //            "sNext":     " 下一页 ",    
          //            "sLast":     " 最后一页 "   
          //        }    
  
          //    } 
          //});  
      });

      function TestHandle() {
          var postData = { func: "aaa" };
          $.ajax({
              type: "Post",
              url: "../../Handle/Handler_AllCustomer.ashx",
              data: postData,
              dataType: "json",
              async: false,
              success: function (returnVal) {
                  alert(returnVal);
              },
              error: function (errMsg) {
                  alert("失败3");
              }
          });
      }
      function insertCustomer() {
          OpenIFrameWindow('导入用户', 'InsertCustomer.aspx', '800px', '600px');
      }
    </script>
