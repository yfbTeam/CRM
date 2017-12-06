/*工作计划列表页*/
var url = pageurl + "/LinkMan/cust_linkman_handle.ashx";
var linkArray = [];//联系人id
var linkNameArray = [];//联系人名称
var type = '';
$(function () {
    dd_setRightNone();
    initdata(page_parmeter);
    //监听搜索keyup事件
    var u = navigator.userAgent;
    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端\
    if (isAndroid == false) {
        //如果是ios 用blur事件
        $("#link_name").on("blur", function () {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        });
    }
    else {
        //如果是android 用keyup事件
        $("#link_name").on("keyup", function () {
            page_parmeter.pageindex = 1;
            initdata(page_parmeter);
        });
    }

    //隐藏加载框
    hidePreloader();
})

//数据
function initdata() {
    if ($('#list_template').html() != null) {
       
        //选择客户后进行联系人的检索
        var link_cust_id = localStorage.getItem("cust_id");
        var link_name = $("#link_name").val();
        var data = {
            Func: "get_cust_linkman_list",
            link_name: link_name,
            cust_users: userid,
            ispage: false,
            link_cust_id: link_cust_id,
            guid: userid
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;
                if (retData.length > 0) {
                    $('#list').append(ejs.render($('#list_template').html(), { retData: retData }));
                    //选择某一客户后返回
                    select();
                }
            }
            else {
                nomessage("list");
            }
        }, function () {
            dd_toast('接口错误，请联系管理员！', 'error', 0);
        })
    }

}

//选择某个客户
function select() {
    mui('#list li').each(function () {
        this.addEventListener('tap', function (event) {
            //获取隐藏域的id和名称
            var id = $(this).children("input[name='link_id']").val();
            var name = $(this).children("input[name='link_name']").val();
            if ($(this).hasClass('active') == false) {
                //追加到数组中
                linkArray.push(id);
                linkNameArray.push(name);
                //添加选中的类名
                $(this).addClass('active');
            }
            else {
                //移除要删除的唯一标识
                removeByValue(linkArray, id);
                //移除要删除的姓名
                removeByValue(linkNameArray, name);
                //移除选中的类名
                $(this).removeClass('active');
            }
        });
    });
}

//移除指定的元素
function removeByValue(arr, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == val) {
            arr.splice(i, 1);
            break;
        }
    }
}

//选择后提交后端
function save() {
    var cust_id = localStorage.getItem("cust_id");
    var linkman_ids = linkArray.join(',');
    var user_ids = localStorage.getItem("userArray");
    var user_names = localStorage.getItem("nameArray");
    //需要删除的【将以前指派d客户从这些人里进行删除，除总监和管理员外】 
    //var current_select_user_ids = localStorage.getItem("current_select_user_ids");
    var data = {
        Func: "relate_customer_linkmans",
        guid: userid,
        cust_id: cust_id,
        linkman_ids: linkman_ids,
        usernames: user_names,
        user_ids: user_ids
    }
    getajax(pageurl + "/Custom/cust_customer_handle.ashx", data, function (json) {
        if (json.result.errMsg == "success") {
            dd_toast('指派成功！', 'success', 0);
            localStorage.setItem("select_user_need_back", "true");
            mui.back();
        }
        else {

        }
    })


}