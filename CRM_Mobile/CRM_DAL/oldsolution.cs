
///// <summary>
///// 新增签到
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_sign_in(sign_in model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", model.id),
//            new SqlParameter("@sign_userid", model.sign_userid),
//            new SqlParameter("@sign_username", model.sign_username),
//            new SqlParameter("@sign_date", model.sign_date),
//            new SqlParameter("@sign_cust_id", model.sign_cust_id),
//            new SqlParameter("@sign_location", model.sign_location),
//            new SqlParameter("@sign_address", model.sign_address),
//            new SqlParameter("@sign_offset", model.sign_offset)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_sign_in", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}
///// <summary>
///// 新增跟进记录
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_follow_up(follow_up model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", SqlDbType.Int),
//            new SqlParameter("@follow_userid", model.follow_userid),
//            new SqlParameter("@follow_username", model.follow_username),
//            new SqlParameter("@follow_cust_id", model.follow_cust_id),
//            new SqlParameter("@follow_link_id", model.follow_link_id),
//            new SqlParameter("@follow_date", model.follow_date),
//            new SqlParameter("@follow_content", model.follow_content),
//            new SqlParameter("@follow_type", model.follow_type),
//            new SqlParameter("@follow_status", model.follow_status),
//            new SqlParameter("@follow_remaindate", model.follow_remaindate),
//            new SqlParameter("@follow_address", model.follow_address)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_follow_up", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}

///// <summary>
///// 编辑联系人
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_cust_linkman(cust_linkman model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", SqlDbType.Int),
//            new SqlParameter("@link_cust_id", model.link_cust_id),
//            new SqlParameter("@link_name", model.link_name),
//            new SqlParameter("@link_department", model.link_department),
//            new SqlParameter("@link_position", model.link_position),
//            new SqlParameter("@link_level", model.link_level),
//            new SqlParameter("@link_sex", model.link_sex),
//            new SqlParameter("@link_birthday", model.link_birthday),
//            new SqlParameter("@link_phonenumber", model.link_phonenumber),
//            new SqlParameter("@link_telephone", model.link_telephone),
//            new SqlParameter("@link_email", model.link_email),
//            new SqlParameter("@link_status", model.link_status),
//            new SqlParameter("@link_users", model.link_users),
//            new SqlParameter("@link_usersname", model.link_usersname)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_cust_linkman", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}


///// <summary>
///// 编辑工作计划
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_workplan(workplan model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", SqlDbType.Int),
//            new SqlParameter("@wp_userid", model.wp_userid),
//            new SqlParameter("@wp_username", model.wp_username),
//            new SqlParameter("@wp_content", model.wp_content),
//            new SqlParameter("@wp_plandate", model.wp_plandate),
//            new SqlParameter("@wp_reminddate", model.wp_reminddate),
//            new SqlParameter("@wp_cust_id", model.wp_cust_id),
//            new SqlParameter("@wp_link_id", model.wp_link_id),
//            new SqlParameter("@wp_status", model.wp_status)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_workplan", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}

///// <summary>
///// 编辑工作报告
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_workreport(workreport model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", SqlDbType.Int),
//            new SqlParameter("@report_userid", model.report_userid),
//            new SqlParameter("@report_username", model.report_username),
//            new SqlParameter("@report_type", model.report_type),
//            new SqlParameter("@report_startdate", model.report_startdate),
//            new SqlParameter("@report_enddate", model.report_enddate),
//            new SqlParameter("@report_content", model.report_content),
//            new SqlParameter("@report_plan", model.report_plan),
//            new SqlParameter("@report_reader", model.report_reader),
//            new SqlParameter("@report_sender", model.report_sender)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_workreport", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}

///// <summary>
///// 编辑客户
///// </summary>
///// <param name="model"></param>
///// <returns></returns>
//public static int edit_cust_customer(cust_customer model)
//{
//    int result = 0;
//    SqlParameter[] param = {
//            new SqlParameter("@id", SqlDbType.Int),
//            new SqlParameter("@cust_parent_id", model.cust_parent_id),
//            new SqlParameter("@cust_name", model.cust_name),
//            new SqlParameter("@cust_type", model.cust_type),
//            new SqlParameter("@cust_level", model.cust_level),
//            new SqlParameter("@cust_address", model.cust_address),
//            new SqlParameter("@cust_location", model.cust_location),
//            new SqlParameter("@cust_users", model.cust_users),
//            new SqlParameter("@cust_usersname", model.cust_usersname)
//    };
//    param[0].Direction = ParameterDirection.Output;
//    object obj = SQLHelp.ExecuteNonQuery("edit_cust_customer", CommandType.StoredProcedure, param);
//    result = Convert.ToInt32(param[0].Value.ToString());
//    return result;
//}
