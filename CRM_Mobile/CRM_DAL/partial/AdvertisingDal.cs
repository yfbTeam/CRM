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
    public partial class AdvertisingDal
    {
        public DataTable GetDataInfo(Hashtable ht)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                sbSql4org.Append("select * from Advertising where 1=1 ");
                if (ht.ContainsKey("MenuIds") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["MenuIds"])))
                {
                    string ids = string.Empty;
                    if (ht.ContainsKey("ChildId") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["ChildId"])))
                    {
                        ids = ht["MenuIds"].ToString();
                        sbSql4org.Append(" and [MenuId] in (" + ids + ")");
                    }
                    else
                    {
                        ids = GetMenuInfoByParentID(Convert.ToInt32(ht["MenuIds"]));
                        sbSql4org.Append(" and [MenuId] in (" + ids + ")");
                    }
                }
                if (ht.ContainsKey("isPush") && !string.IsNullOrWhiteSpace(ht["isPush"].SafeToString()))
                {
                    sbSql4org.Append(" and isPush=" + ht["isPush"].SafeToString());
                }
                if (ht.ContainsKey("IsDelete") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["IsDelete"])))
                {
                    sbSql4org.Append(" and IsDelete=@IsDelete");
                    List.Add(new SqlParameter("@IsDelete", ht["IsDelete"].ToString()));
                }
                if (ht.ContainsKey("ImgUrl") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["ImgUrl"])))
                {
                    sbSql4org.Append(" and (ImageUrl is not null and ImageUrl!='')");
                }
                if (ht.ContainsKey("NotItem") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["NotItem"])))
                {
                    sbSql4org.Append(" and ([MenuId] not in (" + ht["NotItem"].ToString() + "))");
                }
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetMenuInfoByParentID(int pid)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                string ids = string.Empty;
                sbSql4org.Append("select * from PortalTreeData where PId=@PId and IsDelete=@IsDelete and (BeforeAfter=@Before or BeforeAfter=@BeforeAfter) Order by SortId desc");
                List.Add(new SqlParameter("@PId", pid));
                List.Add(new SqlParameter("@IsDelete", ((int)SysStatus.正常).ToString()));
                List.Add(new SqlParameter("@Before", ((int)BeforeAfter.前台展示).ToString()));
                List.Add(new SqlParameter("@BeforeAfter", (int)BeforeAfter.前后台展示));
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string itemId = row["Id"].ToString();
                        ids += itemId + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(ids)) ids = ids.Substring(0, ids.Length - 1);
                }
                return ids;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataTable GetAdvertData(Hashtable ht)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                sbSql4org.Append("select a.*,p.Name from Advertising  a left join [dbo].[PortalTreeData] p on a.[MenuId]=p.Id where 1=1");
                if (ht.ContainsKey("MenuId") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["MenuId"])))
                {
                    sbSql4org.Append(" and a.MenuId in (" + ht["MenuId"].ToString() + ")");
                    // List.Add(new SqlParameter("@MenuId", ht["MenuId"].ToString()));
                }
                sbSql4org.Append(" and a.IsDelete=@IsDelete");
                List.Add(new SqlParameter("@IsDelete", ((int)SysStatus.正常).ToString()));
                sbSql4org.Append(" and p.IsDelete=@IsDelete2");
                List.Add(new SqlParameter("@IsDelete2", ((int)SysStatus.正常).ToString()));

                sbSql4org.Append(" and (p.BeforeAfter=" + ((int)BeforeAfter.前台展示) + " or p.BeforeAfter=" + ((int)BeforeAfter.前后台展示) + ") ");

                if (ht.ContainsKey("isPush") && !string.IsNullOrWhiteSpace(ht["isPush"].SafeToString()))
                {
                    sbSql4org.Append(" and a.isPush=@isPush");
                    List.Add(new SqlParameter("@isPush", ht["isPush"].SafeToString()));
                }
                //if (ht.ContainsKey("MenuId") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["MenuId"])))
                //    sbSql4org.Append(" Order by  a.SortId desc ,a.CreateTime desc ");
                //else
                //    sbSql4org.Append(" Order by  a.MenuId asc,a.SortId desc ,a.CreateTime desc ");
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetSortIdForAdvert(int id, int mid)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                sbSql4org.Append(@" select max(SortId) as SortId from Advertising where MenuId in (
 select id from PortalTreeData where pid=(
select pid from [dbo].[PortalTreeData] where id=@mid)
And  Display=@Display And (BeforeAfter=@BA or BeforeAfter=@BeforeAfter))");
                List.Add(new SqlParameter("@mid", mid));
                List.Add(new SqlParameter("@Display", ((int)Display.显示).ToString()));
                List.Add(new SqlParameter("@BA", ((int)BeforeAfter.前后台展示).ToString()));
                List.Add(new SqlParameter("@BeforeAfter", ((int)BeforeAfter.前台展示).ToString()));
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    int number = Convert.ToInt32(dt.Rows[0]["SortId"]);
                    return number;
                }
                else
                {
                    return 0;
                }
                //    StringBuilder sbSql4org = new StringBuilder();
                //    List<SqlParameter> List = new List<SqlParameter>();
                //    sbSql4org.Append("select top 1 * from Advertising where 1=1 and MenuId=@MenuId and IsDelete=@IsDelete and id<@id order by id desc");
                //    SqlParameter[] param = new SqlParameter[] 
                //{
                //    new SqlParameter("@MenuId",mid),
                //    new SqlParameter("@IsDelete",((int)SysStatus.正常).ToString()),
                //    new SqlParameter("@id",id)
                //};
                //    DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, param);
                //    if (dt != null && dt.Rows.Count == 1)
                //    {
                //        int sortId = Convert.ToInt32(dt.Rows[0]["SortId"]);
                //        return sortId;
                //    }
                //    else
                //    {
                //        return 0;//没有上一级
                //    }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public int UpdateAdertSort(Hashtable ht)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                StringBuilder sbSql4org_up = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                string opertion = ht["upOrdown"].ToString();
                //int mid = Convert.ToInt32(ht["MenuId"]);
                //string mids = GetAdvertMenuIds(mid);
                //if (string.IsNullOrWhiteSpace(mids)) return -999;
                int id = Convert.ToInt32(ht["Id"]);
                int sid = Convert.ToInt32(ht["SortId"]);
                string mwhere = string.Empty;
                if (opertion == "up")//SortID值变大 找下一条
                {

                    if (!string.IsNullOrWhiteSpace(ht["MenuId"].SafeToString()))
                    {
                        mwhere += " and MenuId in (" + ht["MenuId"].SafeToString() + ")";
                    }
                    if (ht.ContainsKey("isPush") && !string.IsNullOrWhiteSpace(ht["isPush"].SafeToString()))
                    {
                        mwhere += " and isPush=" + ht["isPush"].SafeToString();
                    }
                    // 求的下一级
                    sbSql4org.Append(@";with TB as (select top 999999 *,row_number() over(order by SortId desc) as rowid from Advertising where  IsDelete=@IsDelete " + mwhere + " order by SortId desc)"
+ " select * from tb where rowid=(select rowid-1 from TB where Id=@id " + mwhere + ")");
                    //sbSql4org.Append("select top 1 * from Advertising where MenuId in (" + mids + ") and IsDelete=@IsDelete and id>@id order by SortId asc");
                    SqlParameter[] param = new SqlParameter[] 
                    {
                        new SqlParameter("@IsDelete",((int)SysStatus.正常).ToString()),
                        new SqlParameter("@id",id)
                    };
                    DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, param);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return -1;///已经是最大值
                    }
                    else
                    {
                        sbSql4org_up.Append("update Advertising set SortId=@SortId where Id=@CID;update Advertising set SortId=@PSortId where Id=@PCID");
                        SqlParameter[] param2 = new SqlParameter[] 
                        {
                            new SqlParameter("@SortId",dt.Rows[0]["SortId"]),
                            new SqlParameter("@CID",id),
                            new SqlParameter("@PSortId",sid),
                            new SqlParameter("@PCID",dt.Rows[0]["Id"])
                        };
                        int number = SQLHelp.ExecuteNonQuery(sbSql4org_up.ToString(), CommandType.Text, param2);
                        return number;
                    }
                }
                else if (opertion == "down")
                {
                    if (!string.IsNullOrWhiteSpace(ht["MenuId"].SafeToString()))
                    {
                        mwhere += " and MenuId in (" + ht["MenuId"].SafeToString() + ")";
                    }
                    if (ht.ContainsKey("isPush") && !string.IsNullOrWhiteSpace(ht["isPush"].SafeToString()))
                    {
                        mwhere += " and isPush=" + ht["isPush"].SafeToString();
                    }
                    sbSql4org.Append(@"with TB as (select top 999999  *,row_number() over(order by SortId desc) as rowid from Advertising where  IsDelete=@IsDelete " + mwhere + " order by SortId desc)"
+ " select * from tb where rowid=(select rowid+1 from TB where Id=@id" + mwhere + ")");
                    SqlParameter[] param = new SqlParameter[] 
            {
                new SqlParameter("@IsDelete",((int)SysStatus.正常).ToString()),
                new SqlParameter("@id",id)
            };
                    DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, param);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return -2;///已经是最小值
                    }
                    else
                    {
                        sbSql4org_up.Append("update Advertising set SortId=@SortId where Id=@CID;update Advertising set SortId=@PSortId where Id=@PCID");
                        SqlParameter[] param2 = new SqlParameter[] 
            {
                new SqlParameter("@SortId",sid),
                new SqlParameter("@CID",dt.Rows[0]["Id"]),
                new SqlParameter("@PSortId",dt.Rows[0]["SortId"]),
                new SqlParameter("@PCID",id)
            };
                        int number = SQLHelp.ExecuteNonQuery(sbSql4org_up.ToString(), CommandType.Text, param2);
                        return number;
                    }
                }

                return -999;
            }
            catch (Exception)
            {
                return -999;
            }

        }


        public string GetAdvertMenuIds(int mid)
        {
            try
            {
                string ids = string.Empty;
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                sbSql4org.Append(@"select id from PortalTreeData where pid=(
select pid from [dbo].[PortalTreeData] where id=@mid)
And  Display=@Display And (BeforeAfter=@BA or BeforeAfter=@BeforeAfter)");
                List.Add(new SqlParameter("@mid", mid));
                List.Add(new SqlParameter("@Display", ((int)Display.显示).ToString()));
                List.Add(new SqlParameter("@BA", ((int)BeforeAfter.前后台展示).ToString()));
                List.Add(new SqlParameter("@BeforeAfter", ((int)BeforeAfter.前台展示).ToString()));
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ids += row["Id"] + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(ids)) ids = ids.Substring(0, ids.Length - 1);
                }
                return ids;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        public int GetMaxSortId(int mid)
        {
            try
            {
                StringBuilder sbSql4org = new StringBuilder();
                List<SqlParameter> List = new List<SqlParameter>();
                sbSql4org.Append(@" select max(SortId) as SortId from Advertising where MenuId in (
 select id from PortalTreeData where pid=(
select pid from [dbo].[PortalTreeData] where id=@mid)
And  Display=@Display And (BeforeAfter=@BA or BeforeAfter=@BeforeAfter))");
                List.Add(new SqlParameter("@mid", mid));
                List.Add(new SqlParameter("@Display", ((int)Display.显示).ToString()));
                List.Add(new SqlParameter("@BA", ((int)BeforeAfter.前后台展示).ToString()));
                List.Add(new SqlParameter("@BeforeAfter", ((int)BeforeAfter.前台展示).ToString()));
                DataTable dt = SQLHelp.ExecuteDataTable(sbSql4org.ToString(), CommandType.Text, List.ToArray());
                if (dt != null && dt.Rows.Count > 0)
                {
                    int number = Convert.ToInt32(dt.Rows[0]["SortId"]);
                    return number;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
