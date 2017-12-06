
//分享圈
var url_share = pageurl + "/Share/circle_share_handle.ashx";


var httpData_share =
    {
        add_share: function ( table_id, type) {
            var data =
                {                   
                    table_id: table_id,
                    type: type,
                    Func:'AddShare'
                }
            pub.getAjax(url_share, data, function (json) {
                if (json.result.errMsg == "success") {
                    var retData = json.result.retData.PagedData;
                  
                   
                }
            })
        },
    }