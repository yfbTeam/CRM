<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="ViewPage_Page_test" %>


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
    </head>
<body>
    <!--header-->
	<header class="repository_header_wrap manage_header">
		<div class="width repository_header clearfix">
			<a href="" class="logo fl"><img src="../images/logo.png" /></a>
			<div class="testsystem  fl"></div>
			<nav class="navbar menu_mid  fl">
				<ul>
					<li  currentClass="active"><a href="AllCustomer.html">全部客户</a></li>
					<li  currentClass="active"><a href="AllLinkman.html">全部联系人</a></li>
					<li  currentClass="active"><a href="salerkit.html">销售简报</a></li>
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
					<select name="" class="select">
						<option value="0">A类（VIP）</option>
                        <option value="1">B类（优秀）</option>
                        <option value="2">C类（普通）</option>
                        <option value="3">D类（潜在）</option>
					</select>
				</div>
				<div class="clearfix fl course_select">
					<label for="">客户等级：</label>
					<select name="" class="select">
						<option value="0">VIP客户</option>
                        <option value="1">普通客户</option>
                        <option value="2">开拓客户</option>
					</select>
				</div>
				<div class="clearfix fl course_select">
					<label for="">客户分类：</label>
					<select name="" class="select">
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
					<select name="" class="select">
                        <option value="0">部门</option>
						<option value="1">普教一部</option>
                        <option value="2">普教二部</option>
                        <option value="3">普教三步</option>
                        <option value="4">高教一部</option>
                        <option value="5">高教二部</option>
                         <option value="6">高教三部</option>
					</select>
                    <select name="" class="select ml10">
						<option value="0">人员</option>
                        
					</select>
				</div>
				<div class="fl pr search ml10">
                    <input type="text" name="" id="Name" value="" placeholder="请输入关键字" />
                    <i class="iconfont icon-sousuo_sousuo" onclick="getData(1,18)"></i>
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
                    <tbody>
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

