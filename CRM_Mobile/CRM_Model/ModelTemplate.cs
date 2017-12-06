  
using System;
namespace CRM_Model
{
    

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class workplan:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_title { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? wp_plandate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? wp_endplandate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? wp_reminddate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? wp_cust_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? wp_link_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? wp_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? wp_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? wp_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? wp_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string wp_isdelete { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class cust_linkman:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? link_cust_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_cust_name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_department { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_position { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? link_level { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_sex { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? link_birthday { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_phonenumber { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_telephone { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_email { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_users { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_usersname { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? link_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? link_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? link_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? link_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string link_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class audio:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string audio_en_table { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string audio_cn_table { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? audio_table_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string audio_url { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? audio_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? audio_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? audio_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? audio_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string audio_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string audio_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class comment:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? com_table_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? com_parent_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? com_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? com_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? com_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? com_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string com_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class cust_customer:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? cust_parent_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? cust_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? cust_level { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? cust_category { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_address { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_location { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_users { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_usersname { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? cust_followdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? cust_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? cust_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? cust_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? cust_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? cust_x { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? cust_y { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string cust_GaoDe { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class crm_log:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? t_users { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string t_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string action { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? s_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string s_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class follow_up:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? follow_cust_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? follow_link_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? follow_date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? follow_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? follow_remaindate { get; set; }
		/// <summary>
		/// 
		/// </summary>
        public DateTime follow_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? follow_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? follow_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? follow_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_remark { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string follow_address { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class param_rule:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rule_title { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rule_value { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rule_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? rule_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rule_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? rule_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rule_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rule_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class picture:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pic_en_table { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pic_cn_table { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pic_table_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pic_url { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? pic_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pic_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? pic_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pic_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pic_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pic_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class praise:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? praise_table_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string praise_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string praise_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string praise_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? praise_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? praise_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? praise_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? praise_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string praise_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string praise_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class pub_param:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pub_title { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? pub_parentid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pub_value { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? pub_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pub_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? pub_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? pub_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pub_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string pub_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class remind:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rem_date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_isopen { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rem_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? rem_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? rem_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? rem_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string rem_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class remind_setting:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string remind_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? remind_date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string remind_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? remind_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? remind_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? remind_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? remind_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string remind_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string remind_remark { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class report_sender:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? table_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_reader { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_senders { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_isdelete { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class share:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? table_id { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class sign_in:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sign_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sign_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sign_date { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public long? sign_cust_id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sign_location { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sign_address { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sign_offset { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? sign_x { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public decimal? sign_y { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sign_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sign_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sign_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sign_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sign_isdelete { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class statistic:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string s_users { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string s_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string s_remark { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_linkman_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_sign_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_bf_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_wrokplan { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_followup_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_workreport_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_comment { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_cust_customer_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_follow_up_all { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? s_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? s_creator { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class statistic_detail:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sd_users { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sd_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sd_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sd_week { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sd_count { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string sd_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sd_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sd_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? sd_table_id { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class Sys_UserInfo:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public int? Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string UniqueNo { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? UserType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Nickname { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? Sex { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Phone { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Birthday { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string LoginName { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string IDCard { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string HeadPic { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string RegisterOrg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? AuthenType { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Address { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Remarks { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CreateUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string EditUID { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? EditTime { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsEnable { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Byte? IsDelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string CheckMsg { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string KaNo { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string AbsHeadPic { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? IsFirstLogin { get; set; }
    }

	/// </summary>
	///	
	/// </summary>
	[Serializable]
    public partial class workreport:TableBase
    {

		/// <summary>
		/// 
		/// </summary>
		public long? id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_userid { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_username { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? report_type { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? report_startdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? report_enddate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_content { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_plan { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_reader { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_sender { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? report_createdate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? report_creator { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? report_updatedate { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? report_updateuser { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_isdelete { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_remark { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_status { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_cust_customer_array { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_cust_linkman_array { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_follow_up_array { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string report_sign_in_array { get; set; }
    }
}
