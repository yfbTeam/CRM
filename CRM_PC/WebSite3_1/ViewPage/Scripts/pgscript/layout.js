// JavaScript Document
$(document).ready(function (e)
{
   
	$("body").height($(window).height()+'px');//获取页面为浏览器一屏的高度
	$(".lfcontent").height($(window).height()-66);//左侧导航满屏显示
	$(".rfcontentad").height($(window).height()-75);//右侧管理员设置页面满屏
	$(".login").height($(window).height()+'px');//个人登录页面获取满屏高度
	$(".searchClist").height($(window).height()-215);//搜索内容列表获取高度
	$(".rfc_height").height($(window).height()-180); //切换列表
	//智存空间里面个人空间右边内容切换展现形式
	$('.bg_qh_tb i').click(function ()
	{
	  
		$(this).addClass("active").siblings().removeClass("active");
		$k=$(".bg_qh_tb i").index(this);
		$(".bg_qh_tb_content").eq($k).css({display:"block"}).siblings().css({display:"none"});
		$(".wj_select").css({display:"block"});
		//$(".breadcrumb").css({display:"block"});
	});

    //智存空间>个人空间>列表显示鼠标放上去可显示图标点击显示更多可操作项
	$(document).on('click', '.hytb_menu_click', function ()
	{
	    $(this).parent().find('.hytb_menu_list').slideToggle(50);
	});
	$(document).on('mouseleave', '.hytb_menu', function () {
	    $(this).find('.hytb_menu_list').slideUp(50);
	});
	$(document).on('mouseleave', '.rfc_table table tr', function () {
	    $(this).find('.hytb_menu_list').css({ display: 'none' });
	});
	

	//$('.hytb_menu_click').click(function ()
	//{
	    
	//	$(this).parent().find('.hytb_menu_list').slideToggle(50);
	//});
	//$('.hytb_menu').mouseleave(function ()
	//{
	   
	//	$(this).find('.hytb_menu_list').slideUp(50);
	//});
	//$('.rfc_table table tr').mouseleave(function ()
	//{
	   
	//	$(this).find('.hytb_menu_list').css({display : 'none'});
	//});
	//导航：
	$('.topNav li').click(function ()
	{
	   
		$(this).addClass("active").siblings().removeClass("active");
	});
	//智存空间左侧子导航
	$('.lfNav li').click(function ()
	{
	    
		$(this).addClass("active").siblings().removeClass("active");
	});
	//智存空间左侧顶部子标题可切换高度变化
	// $('.lfNav_top').on('click','.lfNav_top_active',function(){
 //    if($(this).hasClass('lfNav_top_tb')){
 //    	$(this).parent().css("height","auto");
 //    	$(this).addClass('lfNav_top_tb_up').removeClass('lfNav_top_tb');
 //    }else{
 //    	$(this).parent().css("height","62px");
 //    	$(this).addClass('lfNav_top_tb').removeClass('lfNav_top_tb_up');
 //    }
 //  });
    //智存空间>个人空间>列表单击选择input复选框 可操作按钮组显示
  
 
  //智存空间>个人空间>视图单击选择input复选框 选中样式
  $("body").on('change', '.rfc_views li', function ()
  {
      
    if($(this).hasClass('active')){
    	$(this).removeClass('active');
    }else{
    	$(this).addClass('active');
    }
  });
  //智存空间>个人空间>列表单击选择input复选框 选中样式
  //$("body").on('change', '.rfc_table table tr', function ()
  //{
     
  //  if($(this).hasClass('active')){
  //  	$(this).removeClass('active');
  //  }else{
  //  	$(this).addClass('active');
  //  }
  //});
    //智存空间>会议空间>单击选择input复选框 选中样式

  

  $("body").on('change', '.meet_table table tr', function ()
  {
     
    if($(this).hasClass('active')){
    	$(this).removeClass('active');
    }else{
    	$(this).addClass('active');
    }
  });
  //智存空间里面会议空间"近期会议"和"时间轴"切换
  $('.ty_biaoti span').click(function ()
  {

		$(this).addClass("active").siblings().removeClass("active");
		$kty=$(".ty_biaoti span").index(this);
		$(".ty_biaoti_content").eq($kty).css({display:"block"}).siblings().css({display:"none"});
		$(".wj_select").css({display:"block"});
		});

	/* 时间轴页面 点击 滑动/展开 */
	$(".meet_views_title").click(function(){	
		var arrow = $(this).find("h3.mvt_h3");
	// alert(arrow);
		if(arrow.hasClass("up")){
			arrow.removeClass("up");
			arrow.addClass("down");
		}else if(arrow.hasClass("down")){
			arrow.removeClass("down");
			arrow.addClass("up");
		}
	
		$(this).next('.meet_views .meet_table').slideToggle();	
	});

	//时间轴点击切换年月份
	$(function(){
    //隐藏所有子栏目（除第一个栏目外）
	$(".year:not(:first)").find(".month").hide();
	//点击大栏目
	  $(".year>li").click(function(){
		  $("li.active",$(this).parent()).removeClass("active");
		  $(this).addClass("active");
		  $("ul>li:first",this).addClass("active");		  		  
	  });
	  $(".month>li").click(function(){
		  var $ul=$(this).parent();		  
		  $ul.find(".active").removeClass("active");		  
		  $(this).addClass("active");
          return false;		  
	  });
   });	
	//个人设置页面点击编辑收起
	$(function ()
	{
	    
		$("#slide").find(".order_click").click(function(){
			$(this).parent().toggleClass("selected").siblings().removeClass("selected");
			$(this).next().slideToggle("fast").end().parent().siblings().find(".order_con")
			.addClass(" ")	
			.slideUp("fast").end().find(".cspan").text("编辑");
			var t = $(this).find(".cspan").text();
			$(this).find(".cspan").text((t=="编辑"?"收起":"编辑"));
		});	
	});
	//管理员设置>用户设置>右侧用户信息,用户设置内容切换展现形式
	$('.suser_title a').click(function ()
	{
	  
		$(this).addClass("active").siblings().removeClass("active");
		$ki=$(".suser_title a").index(this);
		$(".suser_content").eq($ki).css({display:"block"}).siblings().css({display:"none"});
		$(".suser_title").css({display:"block"});
	});
    //管理员设置页面上导航点击切换：
	//$(document).on('click', '.adNav li', function ()
	//{
	//    $(this).addClass("active").siblings().removeClass("active");
	//    $adi = $(".adNav li").index(this);
	//    $(".adrf_content").eq($adi).css({ display: "block" }).siblings().css({ display: "none" });
	//    $(".adNav").css({ display: "block" });
	//});
	$('.adNav li').click(function ()
	{
	    
		$(this).addClass("active").siblings().removeClass("active");

		
	});
	//智存空间有关上传(弹窗)：
	$('.myBtn_uploadbg').on('click','.myBtn_upltit .glyphicon',function(){
		if($(this).hasClass('glyphicon-triangle-bottom')){
			$(this).parent().parent().css("height","38px");
    	$(this).addClass('glyphicon-triangle-top').removeClass('glyphicon-triangle-bottom');
		}else{
			$(this).parent().parent().css("height","auto");
    	$(this).addClass('glyphicon-triangle-bottom').removeClass('glyphicon-triangle-top');
		}
	});
	//搜索页面单击选择input复选框 选中样式
  $("body").on('change','.searchClist table',function()
  {
     
    if($(this).hasClass('active')){
    	$(this).removeClass('active');
    }else{
    	$(this).addClass('active');
    }
  });
  $(document).on('change', '.check', function ()
  {
      var flg = false;
      for (var i = 0, len = $('.check').length; i < len; i++) {
          if ($('.check').eq(i).is(':checked'))
          {
              flg = true;
             
          }
      }
      if (flg)
      {
          $('.wj_select_btnz').show();
      }
      else
      {
          $('.wj_select_btnz').hide();
      }
  });
});

//全选


