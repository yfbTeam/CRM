$(document).ready(function () {
    //var FirstUrl = window.location.href;
    //FirstUrl = FirstUrl.substring(0, FirstUrl.indexOf("SitePages"));

    var formData = {};// 'HFoldUrl': GetRequest().HFoldUrl, 'UrlName': GetRequest().UrlName, 'hSubject': GetRequest().hSubject, 'hContent': GetRequest().hContent, 'HStatus': GetRequest().HStatus, 'capacity': GetRequest().capacity };
    $("#uploadify").uploadify({
        'swf': 'http://localhost:37227/Scripts/Uploadyfy/uploadify/uploadify.swf',
        'uploader': 'http://localhost:37227/CourseManage/upload.ashx',
        //'buttonCursor': 'hand',
        'buttonText': '选择文件',
        //'cancelImg': 'upload/uploadify-cancel.png',
        //'buttonImage': FirstUrl + '/_layouts/15/SVDigitalCampus/Image/bg.png',
        // 'folder': '/jxdBlog/photos',
        'auto': false,
        'multi': true,
        //'wmode': 'transparent',
        'queueID': 'fileQueue',
        'width': '100%',
        'height': 70,
        'fileSizeLimit': '100MB',
        'fileTypeDesc': '文件',       //图片选择描述  
        'queueSizeLimit': 10,                   //一个队列上传文件数限制  
        'removeTimeout': 1,                  //完成时清除队列显示秒数,默认3秒  
        'successTimeout': 20,                  //上传超时  
        //'fileTypeExts': '*.docx;*.doc;*.ppt;*.pptx;*.pdf;*.caj;*.txt;*.rar;*.zip;*.jpg;*.gif;',
        //'fileTypeDesc:': '请选择docx doc ppt pptx pdf caj txt rar zip jpg gif文件',
        'progressData': 'all',

        'formData': formData, //参数
        'removeCompleted': true,
        'overrideEvents': ['onUploadProgress'],
        onUploadSuccess: function (file, data, response) {
            //上传完成时触发（每个文件触发一次）
            //if (response.indexOf('错误提示') > -1) {
            //    $("#" + file.id).find(".data").html("上传失败：" + data);
            //}
            //else {
            if (data == "1") {
                $("#" + file.id).find(".data").html("上传成功");
                parent.Bind();
                parent.GetUpladed();
            }
            else if (data == "3") {
                alert("请选择要上传文件的文件夹");
            }
            else if (data == "2") {
                alert("网盘空间不足");
                parent.location.reload();
            }
            else {
                $("#" + file.id).find(".data").html("上传失败");
            }
            //}
            //}
        },
        //上传汇总  
        'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
            var persent = parseInt(100 * bytesUploaded / bytesTotal);

            $("#" + file.id).find(".data").html(persent + "%");
            $("#" + file.id).find(".uploadify-progress-bar").css("width", persent + "%");

        },
        //返回一个错误，选择文件的时候触发             
        'onUploadError': function (file, errorCode, errorMsg, errorString) {
            alert('文件 ' + file.name + '上传失败: ' + errorString);
        },
        //检测FLASH失败调用              
        'onFallback': function () {
            alert("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。");
        },
        'onAllComplete': function (event, data) {
            alert(data.filesUploaded + '个图片上传成功');
        }

    });
});
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串

    var theRequest = new Object();

    if (url.indexOf("?") != -1) {

        var str = url.substr(1);

        strs = str.split("&");

        for (var i = 0; i < strs.length; i++) {

            theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
        }

    }

    return theRequest;

}
