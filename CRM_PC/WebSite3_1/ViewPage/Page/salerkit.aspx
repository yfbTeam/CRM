<%@ Page Language="C#" AutoEventWireup="true" CodeFile="salerkit.aspx.cs" Inherits="ViewPage_Page_salerkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta charset="utf-8" />
		<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
		<title>销售简报</title>
        <link href="../css/iconfont.css" rel="stylesheet" />
        <link href="../css/reset.css" rel="stylesheet" />
        <link href="../css/style.css" rel="stylesheet" />
        <script src="../Scripts/jquery-1.10.2.min.js"></script>
        <script src="../Scripts/jquery.cookie.js"></script>
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
        <script src="../Scripts/init.js"></script>
        <script src="../Scripts/linq.js"></script>
                <script src="../Scripts/pgscript/jPages.min.js"></script>
        <link href="../Scripts/pgscript/jPages.css" rel="stylesheet" />
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
					<li currentClass="active"><a href="AllLinkman.aspx">全部联系人</a></li>
					<li currentClass="active"><a href="salerkit.aspx">销售简报</a></li>
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
					<label for="">销售负责人：</label>
					<select name="" class="select" id="department" onchange="GetUserByDepartment()">
                        <option value="0">部门</option>
						<option value="1">普教一部</option>
                        <option value="2">普教二部</option>
                        <option value="3">普教三步</option>
                        <option value="4">高教一部</option>
                        <option value="5">高教二部</option>
                         <option value="6">高教三部</option>
					</select>
                    <select name="" class="select ml10" id="user" onchange="SelectDataTest()" >
						<option value="index">人员</option>
					</select>
				</div>
				<div class="fl clearfix none" id="show"> 
					<div class="stytem_select_right fl  ml10">
						<a href="javascript:;" class="newcourse" id="PrevWeek">
							上一周
						</a>
					</div>
					<div class="stytem_select_right fl  ml10">
						<a href="javascript:;" class="newcourse" id="CurWeek">
							当前周
						</a>
					</div>
					<div class="stytem_select_right fl  ml10">
						<a href="javascript:;" class="newcourse" id="NextWeek">
							下一周
						</a>
					</div>
                    <span style="line-height:35px;font-size:15px;color:#333;margin-left:10px;" id="alertdate">
                        2017-01-01至2018-01-01
                    </span>
				</div>
			</div>
			<div class="wrap mt10">
				<div class="header clearfix">
					<span>编号</span>
					<span>销售负责人</span>
					<span>签到次数</span>
					<span>跟进次数</span>
					<span>总监点评</span>
					<span>新增客户</span>
					<span>新增联系人</span>
					<span>工作计划</span>
					<span>工作报告</span>
					<span></span>
				</div>
				<ul class="kit_lists" id="lists">
				
				</ul>
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
	<script type="text/javascript">
	    var currentFirstDate;
	    var addDate = function (date, n) {
	        date.setDate(date.getDate() + n);
	        return date;
	    };
	    var setDate = function (date) {
	        var week = date.getDay() - 1;
	        date = addDate(date, week * -1);
	        currentFirstDate = new Date(date);
	        
	        var start = getNowFormatDate(currentFirstDate);
	        var end = getNextDay(start);
	        //alert(start + "|" + end);
	        $("#alertdate").text(start + " 至 " + end);
	    };
	  
	    setDate(new Date());	   
	    $('#PrevWeek').click(function () {
	        setDate(addDate(currentFirstDate, -7));
	        //alert(getNowFormatDate(currentFirstDate))
	        GetAllUserTJInfo();
	        $("#department").val("index");
	        $("#user").val("index");
	    })
	    $('#CurWeek').click(function () {
	        setDate(new Date());
	        //alert(getNowFormatDate(currentFirstDate))
	        GetAllUserTJInfo();
	        $("#department").val("index");
	        $("#user").val("index");
	        
	    })
	    $('#NextWeek').click(function () {
	        setDate(addDate(currentFirstDate, 7));
	        //alert(getNowFormatDate(currentFirstDate))
	        GetAllUserTJInfo();
	        $("#department").val("index");
	        $("#user").val("index");
	    })
	    function getNextDay(d) {
	        d = new Date(d);
	        d = +d + 1000 * 60 * 60 * 24 * 6;
	        d = new Date(d);
	        //return d;
	        //格式化
	        return d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();

	    }
	    function getNowFormatDate(date) {

	            var seperator1 = "-";
	            var seperator2 = ":";
	            var month = date.getMonth() + 1;
	            var strDate = date.getDate();
	            if (month >= 1 && month <= 9) {
	                month = "0" + month;
	            }
	            if (strDate >= 0 && strDate <= 9) {
	                strDate = "0" + strDate;
	            }
	            var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
	            return currentdate;	        
	    }


	</script>
</body>
</html>
<script>
    $(document).ready(function ()
    {
        $('#show').show();
        Get_saleDepartment();
        GetAllUserTJInfo()
    });
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

        var postData = { func: "Get_saleDepartment" };
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
    var CachbindData;
    function GetAllUserTJInfo()
    {
       var currentFirstDatevalue=getNowFormatDate(currentFirstDate)
       var postData = { func: "Get_AllUserTJInfo", select_date: currentFirstDatevalue };
        $.ajax({
            type: "Post",
            url: WebIp + "/Salerkit/SalerkitHandler.ashx",
            data: postData,
            dataType: "json",
            async: false,
            success: function (returnVal) {
               
                if (returnVal.result.errMsg == "success") {
                    CachbindData = returnVal.result.retData;
                    BindDataTo_GetOneInfo(returnVal.result.retData)
                    //week();
                }
            },
            error: function (errMsg) {
                alert("失败2");
            }
        });
    }
    function BindDataTo_GetOneInfo(bindData) {
        $("#lists").empty();
        var index = 1;
        $(bindData).each(function ()
        {
           var str= "<li>" +
                         "<dt class='clearfix' onclick=\"Get_OneUserTJInfo('" + this.Id + "',this);\">" +
                             "<span>" + index + "</span>" +
                             "<span>" + this.Name + "</span>" +
                             "<span>" + this.count_sign + "</span>" +
                             "<span>" + this.count_follow + "</span>" +
                             "<span>" + this.count_com_userid + "</span>" +
                             "<span>" + this.count_cust_users + "</span>" +
                             "<span>" + this.count_link_users + "</span>" +
                             "<span>" + this.count_wp_userid + "</span>" +
                             "<span>" + this.count_report_userid + "</span>" +                            
                             "<span><i class='iconfont icon-l2'></i></span>" +
                         "</dt>" +
                        "<dd class='clearfix'>" +
                            "<div class='kit fl'>"+
                                "<h2 style='text-align:left;text-indent:20px;'>类型</h2>" +
                                "<div>"+
                                "<p style='text-align:left;text-indent:20px;'>签到次数</p>" +
                                "<p style='text-align:left;text-indent:20px;'>跟进次数</p>" +
                                "<p style='text-align:left;text-indent:20px;'>点评次数</p>" +
                                "<p style='text-align:left;text-indent:20px;'>新增客户个数</p>" +
                                "<p style='text-align:left;text-indent:20px;'>新增联系人个数</p>" +
                                "<p  style='text-align:left;text-indent:20px;'>工作计划条数</p>" +
                                "<p  style='text-align:left;text-indent:20px;'>工作报告条数</p>" +
                                "</div>" +
                            "</div>" +
                            "<div id='kitbody' class='fl'></div>" +
                        "</dd>" +
		        "</li>";
           $("#lists").append(str);
           index++;
        });
        fenye(1);      
        //加载完就展开           
    }
    function test()
    {
        var dd = $(a).next();

        //<div class="kit">
		//						<h2></h2>
		//						<div>
		//							<p>签到（0）</p>
		//							<p>跟进（0）</p>
		//							<p>点评（0）</p>
		//							<p>新增客户（0）</p>
		//							<p>新增联系人（0）</p>
		//							<p>工作计划（0）</p>
		//							<p>工作报告（0）</p>
		//						</div>
		//					</div>
    }
    function Get_OneUserTJInfo(id, a)
    {
        var currentFirstDatevalue = getNowFormatDate(currentFirstDate)
        var dd = $(a).next().find('#kitbody');
        var postData = { func: "Get_OneUserTJInfo", select_date: currentFirstDatevalue, userid: id };
        $.ajax({
            type: "Post",
            url: WebIp + "/Salerkit/SalerkitHandler.ashx",
            data: postData,
            dataType: "json",
            async: false,
            success: function (returnVal) {

                if (returnVal.result.errMsg == "success")
                {
                    
                    BindDataTo_Get_OneUserTJInfo(returnVal.result.retData, dd)                  
                }
            },
            error: function (errMsg) {
                alert("失败2");
            }
        });
    }
    function BindDataTo_Get_OneUserTJInfo(bindData, dom)
    {
       
        dom.empty();
        var index = 1;
        $(bindData).each(function () {         
            var str=   "<div class='kit'>"+
                                       "<h2>" + this.date + "</h2>" +
                                       "<div>"+
                                           "<p>" + this.count_sign + "次</p>" +
                                           "<p>" + this.count_follow + "次</p>" +
                                           "<p>" + this.count_com_userid + "次</p>" +
                                           "<p>" + this.count_cust_users + "个</p>" +
                                           "<p>" + this.count_link_users + "个</p>" +
                                           "<p>" + this.count_wp_userid + "条</p>" +
                                           "<p>" + this.count_report_userid + "条</p>" +
                                       "</div>"+
                                   "</div>";
            dom.append(str);
            index++;
        });
        if (dom.parent().is(':hidden')) {
            dom.parent().stop().slideDown();
            dom.parents('li').addClass('active');
        } else {
            dom.parent().slideUp();
            dom.parents('li').removeClass('active');
        }
    }
    var currentpage = 1;
    var allpages;//没用
    var addpage;
    var deletepage;
    function fenye(cp) {
        $("div.holder").jPages({
            containerID: "lists",
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
    function SelectDataTest() {
       
        var arrRes = Enumerable.From(CachbindData).Where(function (i) {
            //各个条件
            var flg = true;          
            var department = $("#department").val();
          
            if (department != "index" && department != i.DepartMentId) {
                flg = false;
            }
           
            var user = $("#user").val();
            if (user != "index" && user != i.Name) {
                flg = false;
            }
            if (flg) { return i }
        }).ToArray();
        BindDataTo_GetOneInfo(arrRes);
        //关键字筛选项设为空
     
    }
</script>



