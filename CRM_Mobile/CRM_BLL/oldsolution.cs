///// <summary>
///// 编辑签到
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public JsonModel edit_sign_in(sign_in model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_sign_in(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}

///// <summary>
///// 新增跟进记录
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public JsonModel edit_follow_up(follow_up model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_follow_up(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}

///// <summary>
///// 新增客户
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public JsonModel edit_cust_customer(cust_customer model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_cust_customer(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}

//public JsonModel edit_cust_linkman(cust_linkman model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_cust_linkman(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}

//public JsonModel edit_workplan(workplan model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_workplan(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}

//public JsonModel edit_workreport(workreport model)
//{
//    JsonModel jsonModel = new JsonModel();
//    try
//    {
//        int result = DALBaseCommon.edit_workreport(model);
//        jsonModel = new JsonModel()
//        {
//            errNum = 0,
//            errMsg = result == 0 ? "success" : "",
//            retData = result
//        };
//        return jsonModel;
//    }
//    catch (Exception ex)
//    {
//        jsonModel = new JsonModel()
//        {
//            errNum = 400,
//            errMsg = ex.Message,
//            retData = ""
//        };
//        return jsonModel;
//    }
//}