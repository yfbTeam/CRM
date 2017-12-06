//设置右侧导航
function dd_setRight_Add(text, url) {
    dd.ready(function () {
        dd.biz.navigation.setRight({
            show: true,//控制按钮显示， true 显示， false 隐藏， 默认true
            control: true,//是否控制点击事件，true 控制，false 不控制， 默认false
            text: text,//控制显示文本，空字符串表示显示默认文本
            onSuccess: function (result) {
                mui.openWindow({
                    id: url,
                    url: url,
                    show: {
                        autoShow: true,
                        aniShow: 'slide-in-right',
                        duration: '200ms'
                    },
                    waiting: {
                        autoShow: true
                    }
                });
            },
            onFail: function (err) { }
        });
    })
}
//设置ios左侧返回按钮
(function dd_setLeft() {
    dd.biz.navigation.setLeft({
        show: true,//控制按钮显示， true 显示， false 隐藏， 默认true
        control: true,//是否控制点击事件，true 控制，false 不控制， 默认false
        showIcon: true,//是否显示icon，true 显示， false 不显示，默认true； 注：具体UI以客户端为准
        text: '',//控制显示文本，空字符串表示显示默认文本
        onSuccess: function (result) {
            //alert(1);
            //window.location.reload();
        },
        onFail: function (err) { }
    });
})();
//震动一下
function notification_vibrate() {
    dd.device.notification.vibrate({
        duration: 3000, //震动时间，android可配置 iOS忽略
        onSuccess: function (result) {
            /*
            {}
            */
        },
        onFail: function (err) { }
    })
}


//设置右侧导航
function dd_setRight_save(text) {
    dd.ready(function () {
        dd.biz.navigation.setRight({
            show: true,//控制按钮显示， true 显示， false 隐藏， 默认true
            control: true,//是否控制点击事件，true 控制，false 不控制， 默认false
            text: text,//控制显示文本，空字符串表示显示默认文本
            onSuccess: function (result) {
                submit();
            },
            onFail: function (err) { }
        });
    })
}
//上次图片
function dd_uploadImage() {
    dd.ready(function () {
        dd.biz.util.uploadImage({
            multiple: true, //是否多选，默认false
            onSuccess: function (result) {
                alert(result);
            },
            onFail: function (err) { }
        });
    })
}

function dd_setRightNone() {
    dd.ready(function () {
        dd.biz.navigation.setRight({
            show: false,//控制按钮显示， true 显示， false 隐藏， 默认true
            control: false//是否控制点击事件，true 控制，false 不控制， 默认false
        });
    })

}

function dd_setRightMenu(url, url1, items) {
    dd.ready(function () {
        dd.biz.navigation.setMenu({
            backgroundColor: "#ADD8E6",
            items: items,
            onSuccess: function (data) {
                var id = data.id;
                if (id == '2') {
                    url = url1;
                }
                var u = navigator.userAgent;
                var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
                if (isAndroid == false)
                {
                    //dd.device.notification.showPreloader({
                    //    text: "使劲加载中..", //loading显示的字符，空表示不显示文字
                    //    showIcon: true, //是否显示icon，默认true
                    //    onSuccess: function (result) {
                    //        /*{}*/
                    //    },
                    //    onFail: function (err) { }
                    //})
                }
                
                mui.openWindow({
                    id: url,
                    url: url,
                    show: {
                        autoShow: true,
                        aniShow: 'slide-in-right',
                        duration: '200ms'
                    },
                    waiting: {
                        autoShow: true
                    }
                });
            },
            onFail: function (err) {
            
            }
        });
    })
}

dd.ready(function () {
    dd.biz.navigation.setLeft({
        show: true,//控制按钮显示， true 显示， false 隐藏， 默认true
        control: true,//是否控制点击事件，true 控制，false 不控制， 默认false
        showIcon: true,//是否显示icon，true 显示， false 不显示，默认true； 注：具体UI以客户端为准
        text: '',//控制显示文本，空字符串表示显示默认文本
        onSuccess: function (result) {
            mui.back();
        },
        onFail: function (err) { }
    });
})



function showPreloader() {
    //var u = navigator.userAgent;
    //var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端\
    //if (isAndroid == false)
    //{
    //    dd.device.notification.showPreloader({
    //        text: "使劲加载中..", //loading显示的字符，空表示不显示文字
    //        showIcon: true, //是否显示icon，默认true
    //        onSuccess: function (result) {
    //            /*{}*/
    //        },
    //        onFail: function (err) { }
    //    })
    //}
  
}

function hidePreloader() {
   
    //dd.ready(function () {
    //    dd.device.notification.hidePreloader({
    //        onSuccess: function (result) {
    //            /*{}*/
    //        },
    //        onFail: function (err) { }
    //    })
    //});
}