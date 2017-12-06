using MultiEvaluation_Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEvaluation_BLL
{
    /// <summary>
    /// 系统管理类：操作后台管理等功能。
    /// </summary>
    public partial class AdminManagerService
    {

        MultiEvaluation_DAL.AdminManagerDal DAL = new MultiEvaluation_DAL.AdminManagerDal();
        BLLCommon common = new BLLCommon();
        public DataTable GetLeftNavigationMenu(Hashtable ht)
        {
            return DAL.GetLeftNavigationMenu(ht);
        }
        public int UpdatePortalTreeData(Hashtable ht, System.Data.SqlClient.SqlTransaction trans)
        {
            return DAL.UpdatePortalTreeData(ht, trans);
        }

        public DataTable GetPortalTreeData(Hashtable ht, System.Data.SqlClient.SqlTransaction trans)
        {
            return DAL.GetPortalTreeData(ht, trans);
        }
        public JsonModel GetPortalTreeDataForChildId(Hashtable ht)
        {
            JsonModel jsonModel = null;
            try
            {
                DataTable dt = DAL.GetPortalTreeDataForChildId(ht);
                if (dt != null)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = common.DataTableToList(dt)
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = ""
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel GetPortalTreeDataForParentId(Hashtable ht)
        {
            JsonModel jsonModel = null;
            try
            {
                DataTable dt = DAL.GetPortalTreeDataForParentId(ht);
                if (dt != null)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = common.DataTableToList(dt)
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = ""
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel GetMenuParent(Hashtable ht)
        {
            DataTable dt = DAL.GetMenuParent(ht);
            return GetDataTableToJsonModel(dt, ht);
        }

        public JsonModel GetTreeOfRole(Hashtable ht)
        {
            ht.Add("PageIndex", 1);
            ht.Add("PageSize", 1000);
            DataTable dt = DAL.GetTreeOfRole(ht);
            return GetDataTableToJsonModel(dt, ht);
        }

        public JsonModel GetDataTableToJsonModel(DataTable modList, Hashtable ht)
        {
            JsonModel jsonModel = null;
            try
            {
                PagedDataModel<Dictionary<string, object>> pagedDataModel = null;
                int PageIndex = int.Parse(Convert.ToString(ht["PageIndex"]));
                int PageSize = int.Parse(Convert.ToString(ht["PageSize"]));
                int RowCount = 0;
                if (modList == null)
                {
                    jsonModel = new JsonModel()
                    {
                        status = "null",
                        errMsg = "无数据",
                        errNum = 999
                    };
                    return jsonModel;
                }
                RowCount = modList.Rows.Count;
                if (RowCount <= 0)
                {
                    jsonModel = new JsonModel()
                    {
                        status = "null",
                        errMsg = "无数据",
                        errNum = 999
                    };
                    return jsonModel;
                }
                List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                DataTable newDt = GetPagedTable(modList, PageIndex, PageSize);
                list = common.DataTableToList(newDt);
                int PageCount = (int)Math.Ceiling(RowCount * 1.0 / PageSize);
                //将数据封装到PagedDataModel分页数据实体中
                pagedDataModel = new PagedDataModel<Dictionary<string, object>>()
                {
                    PageCount = PageCount,
                    PagedData = list,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                    RowCount = RowCount
                };
                //将分页数据实体封装到JSON标准实体中
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    retData = pagedDataModel,
                    status = "ok"
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)
        {
            if (PageIndex == 0) { return dt; }
            DataTable newdt = dt.Copy();
            newdt.Clear();
            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
            { return newdt; }

            if (rowend > dt.Rows.Count)
            { rowend = dt.Rows.Count; }
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        public JsonModel GetVoteDetails(Hashtable ht)
        {
            JsonModel jsonModel = null;
            try
            {
                DataTable dt = DAL.GetVoteDetails(ht);
                if (dt != null)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success",
                        retData = common.DataTableToList(dt)
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "无数据",
                        retData = ""
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }

        public JsonModel DelVote(Hashtable ht)
        {
            JsonModel jsonModel = null;
            try
            {
                bool dt = DAL.DelVote(ht);
                if (dt)
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 0,
                        errMsg = "success"
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errNum = 999,
                        errMsg = "fuil",
                        retData = ""
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 400,
                    errMsg = ex.Message,
                    retData = ""
                };
                return jsonModel;
            }
        }
    }
}
