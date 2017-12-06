using MultiEvaluation_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEvaluation_DAL
{
    public partial class RoleManagerDal
    {
        public DataTable GetMenuIdsForRoleId(Hashtable ht)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                sbSql4org.Append(@"select rm.MenuId from  [dbo].[RoleOfMenu]  rm inner join [dbo].[PortalTreeData] pt on pt.Id=rm.MenuId
inner join [dbo].[RoleInfo] ri on rm.RoleId=ri.Id  where 1=1 and ri.Id=@RoleId and pt.IsDelete=@IsDelete");
                SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@RoleId",ht["RoleId"].ToString()),
                    new SqlParameter("@IsDelete",((int)SysStatus.正常).ToString())
                 };
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, param);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool EditRoleMenu(Hashtable ht)
        {
            using (SqlTransaction trans = SQLHelp.BeginTransaction())
            {
                try
                {
                    StringBuilder sbSql4org = new StringBuilder();
                    List<SqlParameter> List = new List<SqlParameter>();
                    sbSql4org.Append("delete from RoleOfMenu where RoleId=@RoleId");
                    SqlParameter[] param = new SqlParameter[]{
                    new SqlParameter("@RoleId",ht["RoleId"].ToString())
                 };
                    int number = SQLHelp.ExecuteNonQuery(trans, CommandType.Text, sbSql4org.ToString(), param);
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(ht["MenuIds"])))
                    {
                        string[] MenuIds = ht["MenuIds"].ToString().Split(',');
                        StringBuilder sbSql4org_ins = new StringBuilder();
                        sbSql4org_ins.Append(" INSERT INTO RoleOfMenu(RoleId,MenuId)");
                        for (int i = 0; i < MenuIds.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(MenuIds[i]))
                            {
                                if (i + 1 == MenuIds.Length) sbSql4org_ins.Append(" SELECT @RoleId" + i + ",@MenuId" + i);
                                else sbSql4org_ins.Append(" SELECT @RoleId" + i + ",@MenuId" + i + " union");
                                List.Add(new SqlParameter("@RoleId" + i, ht["RoleId"].ToString()));
                                List.Add(new SqlParameter("@MenuId" + i, MenuIds[i]));
                            }
                        }
                        int num = SQLHelp.ExecuteNonQuery(trans, CommandType.Text, sbSql4org_ins.ToString(), List.ToArray());
                        if (num > 0)
                        {
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        trans.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }

        }


        public bool DelRole(Hashtable ht)
        {
            using (SqlTransaction trans = SQLHelp.BeginTransaction())
            {
                try
                {
                    StringBuilder sbSql4org = new StringBuilder();

                    if (ht.ContainsKey("Id") && !string.IsNullOrWhiteSpace(ht["Id"].SafeToString()))
                    {
                        sbSql4org.Append("delete from RoleInfo where Id=@Id");
                        SqlParameter[] param = new SqlParameter[] { new SqlParameter("@Id", ht["Id"].SafeToString()) };
                        int number = SQLHelp.ExecuteNonQuery(trans, System.Data.CommandType.Text, sbSql4org.ToString(), param);
                        if (number > 0)
                        {
                            StringBuilder sbSql4org2 = new StringBuilder();
                            sbSql4org2.Append("delete from RoleOfMenu where RoleId=@RoleId");
                            SqlParameter[] param2 = new SqlParameter[] { new SqlParameter("@RoleId", ht["Id"].SafeToString()) };
                            int num = SQLHelp.ExecuteNonQuery(trans, System.Data.CommandType.Text, sbSql4org2.ToString(), param2);
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    return false;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
    }
}
