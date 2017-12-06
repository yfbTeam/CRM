/*工作计划列表页*/
var url = pageurl + "/Statistical/statistic_handle.ashx";
var userArray = [];//存储选择后的用户标识
var nameArray = [];//存储选择后的用户标识
var current_select_user_names;
var current_select_user_ids;



$(function () {
    dd_setRightNone();
    //指派人时，把当前已添加的放进去
    //userArray.push(userid);
    //nameArray.push(username);


    //来自 customer_info.js   initdata()
    current_select_user_names = localStorage.getItem("current_select_user_names");
    current_select_user_ids = localStorage.getItem("current_select_user_ids");


    //debugger;
    var names_list = current_select_user_names.split(",");
    var id_list = current_select_user_ids.split(",");




    //将已有的名称加入
    for (var i = 0; i < names_list.length; i++) {
        if (names_list[i] != null && names_list[i] != '' && names_list[i] != undefined) {
            nameArray.push(names_list[i]);
        }
    }
    //将已有的id加入
    for (var i = 0; i < id_list.length; i++) {
        if (id_list[i] != null && id_list[i] != '' && id_list[i] != undefined) {
            userArray.push(id_list[i]);
        }
    }

    localStorage.removeItem("current_select_user_names");
    localStorage.removeItem("current_select_user_ids");

    initdata();
    //隐藏加载框
    hidePreloader();

    mui('#select_people p').each(function () {
        //标示已选择的用户
        var name = $(this).children("input[name='username']").val();
        if (names_list.indexOf(name) > -1) {
            //添加选中的类名
            $(this).addClass('active');
        }
    });
})

//数据
function initdata() {
    if ($('#list_template').html() != null) {
        var data = {
            Func: "GetMemmber",
            guid: userid
        }
        getajax_async(url, data, function (json) {
            if (json.result.errMsg == "success") {
                var retData = json.result.retData;

                if (retData.length > 0) {
                    $('#select_people').append(ejs.render($('#list_template').html(), { retData: retData }));
                    select();
                }
                else {
                    //无数据页
                    $('#select_people').parent().css({ 'height': '100%' });
                    $('#select_people').html('<div style="width: 100%;height:100%;position:fixed;background: #fff url(/images/nomessage.png) no-repeat center;background-size: 3rem auto;"></div>');
                }
            }
            else {
                nomessage("select_people");
            }
        }, function () {
            dd_toast('当前设备网络不稳定，稍后尝试!', 'error', 0);
        })
    }

}

//选择某个负责人
function select() {
    mui('#select_people p').each(function () {
        this.addEventListener('tap', function (event) {
            //debugger;
            var uniqueno = $(this).children("input[name='uniqueno']").val();
            if ($(this).hasClass('active') == false) {

                //追加到数组中
                userArray.push(uniqueno);
                nameArray.push($(this).children("input[name='username']").val());


                //添加选中的类名
                $(this).addClass('active');
            }
            else {
                //移除要删除的唯一标识
                removeByValue(userArray, uniqueno);
                //移除要删除的姓名
                removeByValue(nameArray, $(this).children("input[name='username']").val());
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

//选择后提交存储在localStorage中
function save() {

    if (userArray.length > 0) {

        localStorage.setItem("userArray", userArray.join(','));
        localStorage.setItem("nameArray", nameArray.join(','));

        //localStorage.setItem("userArray", userArray);
        //localStorage.setItem("nameArray", nameArray);


        mui.back();
    }
    else {
        dd_toast('请至少指派一个负责人！', 'error', 0);
    }
}