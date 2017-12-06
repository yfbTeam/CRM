
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiEvaluation_Common;
using MultiEvaluation_Model;
using System.Configuration;


namespace MultiEvaluation_DAL
{

	/// </summary>
	///	
	/// </summary>
    public interface ICourseDal: IBaseDal<Course>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface IStudentDal: IBaseDal<Student>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface ITeacherDal: IBaseDal<Teacher>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface IMajorDal: IBaseDal<Major>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface ICollegeDal: IBaseDal<College>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface IDepartmentDal: IBaseDal<Department>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface IRoleDal: IBaseDal<Role>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface ID_ClassDal: IBaseDal<D_Class>
    {


		
    }

	/// </summary>
	///	
	/// </summary>
    public interface IUserInfoDal: IBaseDal<UserInfo>
    {


		
    }
}