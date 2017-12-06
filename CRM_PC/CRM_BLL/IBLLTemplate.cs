
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiEvaluation_Model;


namespace MultiEvaluation_BLL
{

	/// </summary>
	///	
	/// </summary>
    public interface ICourseService:IBaseService<Course>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface IStudentService:IBaseService<Student>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface ITeacherService:IBaseService<Teacher>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface IMajorService:IBaseService<Major>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface ICollegeService:IBaseService<College>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface IDepartmentService:IBaseService<Department>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface IRoleService:IBaseService<Role>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface ID_ClassService:IBaseService<D_Class>
    {

    }
	

	/// </summary>
	///	
	/// </summary>
    public interface IUserInfoService:IBaseService<UserInfo>
    {

    }
	
}