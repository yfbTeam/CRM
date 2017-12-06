说明：enterprise中是企业的开发，主要有oauth授权与jsapi授权，服务器端调用，客户端调用；
isv中是isv的对接；
开发环境是visual stdio 2013


   #region oldsolution

            //拿到access_token之后。可以参照钉钉开发文档中的-服务端开发文档进行其它api的测试。关于服务端的回调接口，将在isv的开发中提到具体的用法。
            //这里只写一个接口测试。
            //string url = "https://oapi.dingtalk.com/department/create?access_token=" + access_token;
            //string param = "{\"access_token\":\"" + access_token + "\",\"name\":\"新增部门测试\",\"parentid\":\"1\",\"order\":\"3\",\"createDeptGroup\":\"false\"}";
            //string callback = HttpHelper.Post(url, param);
            //Helper.WriteLog("创建部门：" + callback);

            #endregion
