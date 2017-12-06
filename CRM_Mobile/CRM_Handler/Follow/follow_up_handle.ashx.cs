using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CRM_BLL;
using System.Collections;
using CRM_Handler;
using System.Data;
using CRM_Common;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using CRM_Model;
using CRM_Handler.Statistical;
using CRM_Handler.LinkMan;
using CRM_Handler.Share;
using CRM_Handler.Custom;
using CRM_Handler.PubParam;
using CRM_Handler.Common;
using CRM_Handler.Entity;

namespace CRM_Handler.Follow
{
    /// <summary>
    ///跟进记录
    /// </summary>
    public class follow_up_handle : IHttpHandler
    {
        #region 字段

        JsonModel jsonModel = null;

        /// 跟进记录集合
        /// </summary>
        public static List<follow_up> list_All = null;

        /// <summary>
        /// 指定某个用户的跟进记录
        /// </summary>
        public static Dictionary<string, List<follow_up>> dic_Self = new Dictionary<string, List<follow_up>>();

        /// <summary>
        /// 当前类型
        /// </summary>
        static HanderType handertype = HanderType.follow;

        #endregion

        #region 中心入口点

        /// <summary>
        /// /中心入口点
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest Request = context.Request;
            string func = RequestHelper.string_transfer(Request, "func");
            try
            {
                //全局初始化
                Constant.Fill_All_Data(context);
                //当前用户ID
                string guid = RequestHelper.string_transfer(Request, "guid");
                if (Data_check_helper.check_Self(handertype, guid))
                {
                    switch (func)
                    {
                        case "get_follow_up_list":
                            get_follow_up_list(context, guid);
                            break;
                        case "get_follow_up":
                            get_follow_up(context, guid);
                            break;
                        case "add_follow_up":
                            add_follow_up(context, guid);
                            break;
                        case "get_cust_list":
                            get_cust_list(context, guid);
                            break;
                        default:
                            jsonModel = Constant.get_jsonmodel(5, "没有此方法", "");
                            context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
                            break;
                    }
                }
                else
                {
                    jsonModel = Constant.get_jsonmodel(5, "请到首页进行刷新", "");
                }
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
                context.Response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        #endregion

        #region 获取客户列表【跟进记录使用  combolist】【guid】

        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <param name="context"></param>
        public void get_cust_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //我的跟进GUID               
                string is_self_guid = RequestHelper.string_transfer(Request, "is_self_guid");

                string riqi = RequestHelper.string_transfer(Request, "riqi");
                string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                string memmberID = RequestHelper.string_transfer(Request, "memmberID");
                if (Data_check_helper.check_Self(handertype, guid))
                {
                    //对象集合转为dic集合列表
                    List<cust_customer> cust_customers = null;

                    //客户列表,当前用户
                    List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                    //跟进列表
                    IEnumerable<follow_up> follow_selfs = GetPage_Helper(dic_Self[guid], 0, 0, riqi, is_self_guid);
                    follow_selfs = Check_And_Get_List_dep(departmentID, memmberID, follow_selfs);
                    if (!string.IsNullOrEmpty(is_self_guid))
                    {
                        //对象集合转为dic集合列表
                        cust_customers = (from t in cust_customer_selfs
                                          join c in follow_selfs on t.id equals c.follow_cust_id
                                          where c.follow_userid == is_self_guid
                                          select t
                                     ).Distinct().ToList();
                    }
                    else
                    {
                        //对象集合转为dic集合列表
                        cust_customers = (from t in cust_customer_selfs
                                          join c in follow_selfs on t.id equals c.follow_cust_id
                                          where c.follow_cust_id == t.id
                                          select t
                                     ).Distinct().ToList();
                    }
                    jsonModel = Constant.get_jsonmodel(0, "success", cust_customers);
                }
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                //时间转换
                string jsonString = Constant.bbc.ConverDatetime(Constant.jss.Serialize(jsonModel));
                context.Response.Write("{\"result\":" + jsonString + "}");
            }
        }



        #endregion

        #region 获取跟进记录【指定ID guid】

        /// <summary>
        /// 获取根据记录【指定ID】
        /// </summary>
        /// <param name="context"></param>

        public void get_follow_up(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                long id = RequestHelper.long_transfer(Request, "id");
                long follow_cust_id = RequestHelper.long_transfer(Request, "follow_cust_id");
                long follow_link_id = RequestHelper.long_transfer(Request, "follow_link_id");
                string follow_type = RequestHelper.string_transfer(Request, "follow_type");
                //跟进列表
                List<follow_up> follow_selfs = dic_Self[guid];
                //指定的一个客户
                follow_up follow_up = (from t in follow_selfs
                                       where t.id == id
                                       select t).FirstOrDefault();
                Dictionary<string, object> dic_follow_up = ConverList<follow_up>.T_ToDic(follow_up);
                //联系人列表,当前用户
                List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.dic_Self[guid];
                //联系人名称和客户名称
                cust_linkman cust_linkman = (from t in cust_linkman_selfs
                                             where t.id == follow_up.follow_link_id
                                             select t).FirstOrDefault();
                if (cust_linkman != null)
                {
                    dic_follow_up["follow_link_name"] = cust_linkman.link_name;
                    dic_follow_up["follow_cust_name"] = cust_linkman.link_cust_name;
                }
                //跟进类型
                dic_follow_up["follow_type_name"] = LevelHelper.Getfollow_level(follow_up.follow_type.ToString());
                if (Constant.list_picture_All != null)
                {
                    //获取指定的图片【类型 和ID】
                    List<picture> listp = (from t in Constant.list_picture_All
                                           where t.pic_en_table == "follow_up" && t.pic_table_id == id
                                           select t).ToList();
                    string pic = "";
                    foreach (picture p in listp)
                    {
                        pic += p.pic_url + ',';
                    }
                    dic_follow_up["pic"] = pic.Trim(',');
                }
                jsonModel = Constant.get_jsonmodel(0, "success", dic_follow_up);
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                string jsonString = Constant.jss.Serialize(jsonModel);
                context.Response.Write("{\"result\":" + jsonString + "}");
            }
        }

        #endregion

        #region 获取跟进记录列表

        /// <summary>
        /// 获取跟进记录列表
        /// </summary>
        /// <param name="context"></param>
        public void get_follow_up_list(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            try
            {
                //缓存应用
                if (Data_check_helper.check_Self(handertype, guid))
                {
                    //是否分页                   
                    bool ispage = RequestHelper.bool_transfer(Request, "ispage");
                    //每一页包含的数量
                    int PageSize = RequestHelper.int_transfer(Request, "PageSize");
                    //第几页
                    int PageIndex = RequestHelper.int_transfer(Request, "PageIndex");

                    //封装到PagedDataModel的元数据
                    List<follow> follow_List = new List<follow>();

                    //跟进客户的客户ID
                    int follow_cust_id = RequestHelper.int_transfer(Request, "follow_cust_id");
                    //跟进客户的联系人ID
                    int link_l_id = RequestHelper.int_transfer(Request, "link_id");
                    //筛选日期                 
                    string riqi = RequestHelper.string_transfer(Request, "riqi");

                    //我的跟进GUID
                    string is_self_guid = RequestHelper.string_transfer(Request, "is_self_guid");

                    //部门的ID号【传参 】
                    string departmentID = RequestHelper.string_transfer(Request, "departmentID");
                    string memmberID = RequestHelper.string_transfer(Request, "memmberID");

                    var data_list = from t in dic_Self[guid] select t;

                    data_list = Check_And_Get_List_dep(departmentID, memmberID, data_list);

                    List<follow_up> follow_ups = GetPage_Helper(data_list, follow_cust_id, link_l_id, riqi, is_self_guid).ToList();

                    int follow_count = follow_ups.Count;
                    follow_ups = follow_ups.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

                    //数据对应
                    foreach (var fp in follow_ups)
                    {
                        //跟进记录（对应页面的实体类型）
                        follow follow = new follow();
                        //评论数量
                        int list_praise_count = praise_handle.list_All.Count(t => t.praise_table_id == (long)fp.id && t.praise_userid == guid && t.praise_type == "1");
                        //是否包含评论
                        string is_praise = list_praise_count > 0 ? "1" : "0";

                        //附件跟进记录（对应页面的实体类型【通过数据库映射填充实体所需部分信息】）
                        int picture_count = 0;
                        if (Constant.list_picture_All != null)
                        {
                            //获取指定的图片【类型 和ID】                       
                            picture_count = Constant.list_picture_All.Count(t => t.pic_en_table == "follow_up" && t.pic_table_id == Convert.ToInt32(fp.id));
                        }
                        follow.follow_up_info = new follow_up_info()
                        {
                            follow_content = fp.follow_content,
                            follow_cust_id = (long)fp.follow_cust_id,
                            follow_date = ((DateTime)fp.follow_date).ToString("yyyy-MM-dd"),
                            follow_link_id = (long)fp.follow_link_id,
                            follow_username = fp.follow_username,
                            is_praise = is_praise,
                            id = (long)fp.id,
                            picture_count = picture_count,
                            follow_createdate = Convert.ToString(fp.follow_createdate),
                        };

                        //获取跟进类型
                        string type_string = Convert.ToString(fp.follow_type);
                        if (!string.IsNullOrEmpty(type_string) && pub_param_handle.dic_follow_Level.ContainsKey(type_string))
                        {
                            follow.follow_up_info.follow_type = pub_param_handle.dic_follow_Level[type_string];
                        }


                        //获取客户和联系人
                        get_customer_linkman(guid, fp, follow);
                        //获取图片评论和点赞
                        get_picture_praise_comment(fp, follow);
                        follow_List.Add(follow);
                    }

                    //返回数据
                    PagedDataModel<follow> psd = new PagedDataModel<follow>()
                    {
                        PagedData = follow_List,
                        PageIndex = PageIndex,
                        PageSize = PageSize,
                        RowCount = follow_count
                    };
                    //数据包（json格式）
                    jsonModel = new JsonModel() { errNum = 0, errMsg = "success", retData = psd };

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                jsonModel = Constant.ErrorGetData(ex.Message);
            }
            finally
            {
                if (Constant.dicLimit_P.ContainsKey(guid))
                {
                    jsonModel.status = "IsAdmin";
                }
                string result = Constant.jss.Serialize(jsonModel);
                context.Response.Write("{\"result\":" + result + "}");
            }
        }

        /// <summary>
        /// 通过部门获取数据【或者纯粹获取某个成员的】
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Data_selfs"></param>
        /// <returns></returns>
        private static IEnumerable<follow_up> Check_And_Get_List_dep(string departmentID, string memmberID, IEnumerable<follow_up> Data_selfs)
        {
            try
            {
                if (!string.IsNullOrEmpty(memmberID))
                {
                    Data_selfs = (from w in Data_selfs
                                  where w.follow_userid == memmberID
                                  select w);
                }
                else if (!string.IsNullOrEmpty(departmentID))
                {
                    //先获取部门信息
                    DepartMent department = Constant.DepartMent_List.FirstOrDefault(d => d.ID == departmentID);

                    var UniqueNo_string = (from userInfo in department.UserInfo_List
                                           select userInfo.UniqueNo);

                    //取到部门，获取下列信息
                    if (department != null)
                    {
                        Data_selfs = (from w in Data_selfs
                                      where UniqueNo_string.Contains(w.follow_userid)
                                      select w);
                    }


                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return Data_selfs;
        }

        /// <summary>
        /// 获取客户和联系人姓名
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="fp"></param>
        /// <param name="follow"></param>
        private static void get_customer_linkman(string guid, follow_up fp, follow follow)
        {
            try
            {
                #region 获取客户信息\联系人

                //联系人列表,当前用户
                List<cust_linkman> cust_linkman_selfs = cust_linkman_handle.list_All;

                //获取联系人信息
                string link_id = fp.follow_link_id.ToString();
                long link_id_long = Convert.ToInt64(link_id);
                //获取指定的客户【在自己的客户列表里获取】
                cust_linkman cust_linkman = (from t in cust_linkman_selfs
                                             where t.id == link_id_long
                                             select t).FirstOrDefault(); if (cust_linkman != null)
                {
                    follow.follow_up_info.follow_cust_name = cust_linkman.link_cust_name;
                    follow.follow_up_info.follow_link_name = cust_linkman.link_name;
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获取图片评论和点赞
        /// </summary>
        /// <param name="fp"></param>
        /// <param name="follow"></param>
        private static void get_picture_praise_comment(follow_up fp, follow follow)
        {
            try
            {
                #region 获取图片

                if (Constant.list_picture_All != null)
                {
                    //获取指定的图片【类型 和ID】
                    List<picture> list_picture = (from t in Constant.list_picture_All
                                                  where t.pic_en_table == "follow_up" && t.pic_table_id == (int)fp.id
                                                  select t).ToList();
                    follow.picture = ConverList<picture>.ListToDic(list_picture);
                }

                #endregion

                #region 获取评论

                //获取指定的评论
                List<comment> list_comment = (from t in comment_handle.list_All
                                              where t.com_table_id == (int)fp.id && t.com_isdelete == "0" && t.com_type == "1"
                                              select t).ToList();
                follow.comment = ConverList<comment>.ListToDic(list_comment);

                #endregion

                #region 获取点赞人

                List<praise> list_praise = (from t in praise_handle.list_All
                                            where t.praise_table_id == (int)fp.id && t.praise_type == "1"
                                            select t).ToList();
                follow.praise = ConverList<praise>.ListToDic(list_praise);

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 新增跟进记录【guid】

        /// <summary>
        /// 新增跟进记录
        /// </summary>
        /// <param name="context"></param>
        public void add_follow_up(HttpContext context, string guid)
        {
            HttpRequest Request = context.Request;
            HttpResponse response = context.Response;

            try
            {
                long id = RequestHelper.long_transfer(Request, "id");

                follow_up newfollow_up = new follow_up();
                newfollow_up.follow_cust_id = RequestHelper.int_transfer(Request, "follow_cust_id");
                newfollow_up.follow_link_id = RequestHelper.int_transfer(Request, "follow_link_id");
                newfollow_up.follow_type = int.Parse(RequestHelper.string_transfer(Request, "follow_type"));
                newfollow_up.follow_content = RequestHelper.string_transfer(Request, "follow_content");
                newfollow_up.follow_status = RequestHelper.string_transfer(Request, "follow_status");
                newfollow_up.follow_remaindate = RequestHelper.DateTime_transfer(Request, "follow_remaindate");
                newfollow_up.follow_address = RequestHelper.string_transfer(Request, "follow_address");
                string _pictures = RequestHelper.string_transfer(Request, "picture");
                //修改《暂时修改功能》
                if (id > 0)
                {
                    //编辑跟进记录
                    edit_follow(guid, id, newfollow_up, _pictures);
                }
                else if (id == 0)
                {
                    newfollow_up.follow_userid = RequestHelper.string_transfer(Request, "follow_userid");//用户编号
                    newfollow_up.follow_username = RequestHelper.string_transfer(Request, "follow_username");//用户姓名
                    newfollow_up.follow_createdate = DateTime.Now;
                    newfollow_up.follow_date = RequestHelper.DateTime_transfer(Request, "follow_date");
                    newfollow_up.follow_date = newfollow_up.follow_date == RequestHelper.default_DateTime ? DateTime.Now : newfollow_up.follow_date;
                    newfollow_up.follow_isdelete = "0";

                    if (!dic_Self[guid].Contains(newfollow_up))
                    {

                        //缓存添加客户
                        dic_Self[guid].Add(newfollow_up);

                        jsonModel = Constant.get_jsonmodel(0, "success", 1);
                        new Thread(() =>
                 {
                     try
                     {
                         //添加跟进
                         add_follow(guid, newfollow_up, _pictures);
                     }
                     catch (Exception ex)
                     {
                         LogHelper.Error(ex);
                     }

                 }) { IsBackground = true }.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                jsonModel = Constant.ErrorGetData(ex);
            }
            finally
            {
                response.Write("{\"result\":" + Constant.jss.Serialize(jsonModel) + "}");
            }
        }

        /// <summary>
        /// 添加跟进记录
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="newfollow_up"></param>
        /// <param name="_pictures"></param>
        private void add_follow(string guid, follow_up newfollow_up, string _pictures)
        {
            try
            {
                //通知领导进行添加
                admin_add_follow(guid, newfollow_up);

                //添加跟进记录
                jsonModel = Constant.bbc.add_follow_up(newfollow_up, _pictures);
                newfollow_up.id = Convert.ToInt64(jsonModel.retData);
                //缓存添加图片
                AddPicture_helper(newfollow_up, _pictures);

                //客户列表,当前用户【更改拜访记录】
                List<cust_customer> cust_customer_selfs = cust_customer_handle.dic_Self[guid];
                cust_customer customer_follow = cust_customer_selfs.FirstOrDefault(item => item.id == newfollow_up.follow_cust_id);
                if (customer_follow != null)
                {
                    customer_follow.cust_followdate = DateTime.Now;
                    Constant.cust_customer_S.Update(customer_follow);
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// 通知领导我已添加跟进
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="follow_up"></param>
        private static void admin_add_follow(string guid, follow_up follow_up)
        {
            try
            {
                //通知领导我已添加跟进
                if (Constant.dic_custs_users.ContainsKey(guid))
                {
                    if (!list_All.Contains(follow_up))
                    {
                        //当前添加跟进
                        list_All.Add(follow_up);
                    }

                    //获取上级的guid
                    List<string> commonAdmin_CustursID = Constant.dic_custs_users[guid];

                    //上级列表
                    foreach (var item in commonAdmin_CustursID)
                    {
                        //若领导在线，添加当前添加的跟进
                        if (dic_Self.ContainsKey(item))
                        {
                            //跟进列表,当前跟进
                            List<follow_up> follow_up_admins = dic_Self[item];
                            if (!follow_up_admins.Contains(follow_up))
                            {
                                follow_up_admins.Add(follow_up);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// 编辑跟进记录
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="Request"></param>
        /// <param name="id"></param>
        /// <param name="follow_up"></param>
        private void edit_follow(string guid, long id, follow_up follow_up, string _pictures)
        {
            try
            {
                //跟进列表
                List<follow_up> follow_selfs = dic_Self[guid];
                follow_up edit_follow_up = follow_selfs.FirstOrDefault(item => item.id == id);
                if (edit_follow_up != null)
                {
                    edit_follow_up.follow_cust_id = follow_up.follow_cust_id;
                    edit_follow_up.follow_link_id = follow_up.follow_link_id;
                    edit_follow_up.follow_content = follow_up.follow_content;
                    edit_follow_up.follow_status = follow_up.follow_status;
                    edit_follow_up.follow_remaindate = follow_up.follow_remaindate;
                    edit_follow_up.follow_isdelete = follow_up.follow_isdelete;
                    edit_follow_up.follow_address = follow_up.follow_address;

                    //成功提示                            
                    jsonModel = Constant.get_jsonmodel(0, "success", 1);
                    //开启线程操作数据库
                    new Thread(() =>
                    {
                        try
                        {
                            editpicture_helper(id, _pictures, edit_follow_up);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                        }
                    }) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 编辑图片，若没有则添加
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_pictures"></param>
        /// <param name="edit_follow_up"></param>
        private static void editpicture_helper(long id, string _pictures, follow_up edit_follow_up)
        {
            try
            {
                picture edit_picture = Constant.list_picture_All.FirstOrDefault(item => item.id == id);
                if (edit_picture != null)
                {
                    edit_picture.pic_updatedate = DateTime.Now;
                    edit_picture.pic_url = _pictures;

                    Constant.follow_up_S.Update(edit_follow_up);
                    Constant.picture_S.Update(edit_picture);
                }
                else
                {
                    picture pictureModel = AddPicture_helper(edit_follow_up, _pictures);
                    Constant.picture_S.Add(pictureModel);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 缓存添加图片
        /// </summary>
        /// <param name="newfollow_up"></param>
        /// <param name="_pictures"></param>
        private static picture AddPicture_helper(follow_up newfollow_up, string _pictures)
        {
            picture picturemodel = null;
            try
            {
                //图片之前已经用存储过程和跟进记录一起插入

                if (!string.IsNullOrEmpty(_pictures))
                {
                    string[] _pics = _pictures.Split(',');
                    for (int i = 0; i < _pics.Length; i++)
                    {
                        picturemodel = new picture()
                        {
                            pic_en_table = "follow_up",
                            pic_cn_table = "跟进记录",
                            pic_table_id = Convert.ToInt32(newfollow_up.id),
                            pic_url = _pics[i],
                            pic_createdate = DateTime.Now,
                            pic_creator = 1,
                            pic_updatedate = DateTime.Now,
                            pic_updateuser = 1,
                            pic_isdelete = "0",
                            pic_remark = "新增跟进记录",
                        };
                        Constant.list_picture_All.Add(picturemodel);

                    }

                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return picturemodel;
        }


        #endregion

        #region 获取辅助

        /// <summary>
        /// 辅助方法【linq 分页】
        /// </summary>
        /// <param name="lstPerson"></param>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public static IEnumerable<follow_up> GetPage_Helper(IEnumerable<follow_up> lstPerson, int follow_cust_id, int link_id, string riqi, string is_self_guid)
        {
            IEnumerable<follow_up> list = null;
            try
            {
                list = lstPerson.OrderByDescending(i => i.id).ToList();
                //根据当前人的跟进记录去获取
                if (!string.IsNullOrEmpty(is_self_guid))
                {
                    list = list.Where(p => p.follow_userid == is_self_guid);
                }
                //根据客户ID取
                if (follow_cust_id != 0)
                {
                    list = list.Where(p => p.follow_cust_id == follow_cust_id);
                }
                //根据联系人ID取
                if (link_id != 0)
                {
                    list = list.Where(p => p.follow_link_id == link_id);
                }
                //日期差
                int d = 0;
                //是否为范围（对比范围，或对比日）
                DataCountType dataCountType = default(DataCountType);
                if (riqi != "不限" && riqi != "")
                {
                    if (riqi == "今天")
                    {
                        dataCountType = DataCountType.day;
                        d = 0;
                    }
                    else if (riqi == "昨天")
                    {
                        dataCountType = DataCountType.day;
                        d = -1;
                    }
                    else if (riqi == "本周")
                    {
                        dataCountType = DataCountType.thisweek;
                        d = 0;
                    }
                    else if (riqi == "上周")
                    {
                        dataCountType = DataCountType.lastweek;
                        d = 0;
                    }
                    else if (riqi == "本月")
                    {
                        dataCountType = DataCountType.month;
                        d = 0;
                    }
                    DateTime d_EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(d);
                    switch (dataCountType)
                    {
                        //匹配日期获取
                        case DataCountType.day:
                            list = list.Where(p => Convert.ToDateTime(p.follow_date).Year == d_EndTime.Year && Convert.ToDateTime(p.follow_date).DayOfYear == d_EndTime.DayOfYear);
                            break;
                        case DataCountType.thisweek:
                            list = list.Where(p => Constant.IsInSameWeek(Convert.ToDateTime(p.follow_date), d_EndTime));
                            break;
                        case DataCountType.lastweek:

                            int dayofweek = (int)d_EndTime.DayOfWeek == 0 ? 7 : (int)d_EndTime.DayOfWeek;
                            //上星期周一
                            DateTime startdate = d_EndTime.AddDays(-dayofweek - 7 + 1);
                            //去掉时分秒
                            startdate = Convert.ToDateTime(startdate.ToString("yyyy-MM-dd"));
                            //上星期周末
                            DateTime enddate = d_EndTime.AddDays(-dayofweek + 1);
                            //去掉时分秒
                            enddate = Convert.ToDateTime(enddate.ToString("yyyy-MM-dd"));

                            list = list.Where(p => Convert.ToDateTime(p.follow_date) > startdate && Convert.ToDateTime(p.follow_date) < enddate);
                            break;
                        case DataCountType.month:
                            list = list.Where(p => Convert.ToDateTime(p.follow_date).Year == d_EndTime.Year && Convert.ToDateTime(p.follow_date).Month == d_EndTime.Month);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return list;
        }

        #endregion

        #region 辅助字段

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion



    }

    #region 辅助类

    /// <summary>
    /// 计算日期范围的枚举
    /// </summary>
    public enum DataCountType
    {
        //按天计算
        day,
        //上周
        lastweek,
        //本周
        thisweek,
        //按月计算
        month,
    }



    #endregion
}