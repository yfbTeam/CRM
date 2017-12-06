
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_Common;
using CRM_DAL;
using CRM_Model;
using CRM_BLL;



namespace CRM_BLL
{

	/// </summary>
	///	
	/// </summary>
    public partial class cust_customerService:BaseService<cust_customer>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getcust_customerDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class workplanService:BaseService<workplan>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetworkplanDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class audioService:BaseService<audio>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetaudioDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class commentService:BaseService<comment>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetcommentDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class crm_Test_logService:BaseService<crm_Test_log>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getcrm_Test_logDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class cust_linkmanService:BaseService<cust_linkman>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getcust_linkmanDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class follow_upService:BaseService<follow_up>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getfollow_upDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class param_ruleService:BaseService<param_rule>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getparam_ruleDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class pictureService:BaseService<picture>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetpictureDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class praiseService:BaseService<praise>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetpraiseDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class pub_paramService:BaseService<pub_param>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getpub_paramDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class remindService:BaseService<remind>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetremindDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class remind_settingService:BaseService<remind_setting>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getremind_settingDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class report_senderService:BaseService<report_sender>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getreport_senderDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class shareService:BaseService<share>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetshareDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class sign_inService:BaseService<sign_in>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getsign_inDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class statisticService:BaseService<statistic>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetstatisticDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class statistic_detailService:BaseService<statistic_detail>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.Getstatistic_detailDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class Sys_UserInfoService:BaseService<Sys_UserInfo>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetSys_UserInfoDal();
        }
		
		 
    }
	

	/// </summary>
	///	
	/// </summary>
    public partial class workreportService:BaseService<workreport>

    {
	 public override void SetCurrentDal()
        {
            CurrentDal = DalFactory.GetworkreportDal();
        }
		
		 
    }
	
}