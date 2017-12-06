
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
    /// 角色管理类
    /// </summary>
    public partial class RoleManagerService
    {
        MultiEvaluation_DAL.RoleManagerDal DAL = new MultiEvaluation_DAL.RoleManagerDal();
        BLLCommon common = new BLLCommon();
        public JsonModel GetMenuIdsForRoleId(Hashtable ht)
        {
            ht.Add("PageIndex", 1);
            ht.Add("PageSize", 99999);
            DataTable dt = DAL.GetMenuIdsForRoleId(ht);
            return GetDataTableToJsonModel(dt, ht);
        }

        public string GetMenuIdsForRoleId(int RoleId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("RoleId", RoleId);
            DataTable dt = DAL.GetMenuIdsForRoleId(ht);
            string mids = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    mids += row["MenuId"].ToString() + ",";
                }
                if (!string.IsNullOrWhiteSpace(mids))
                {
                    mids = "," + mids;
                }
            }
            return mids;
        }

        public JsonModel EditRoleMenu(Hashtable ht)
        {
            var result = DAL.EditRoleMenu(ht);
            JsonModel jsonModel = null;
            if (result)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    status = "ok"
                };
            }
            else
            {
                jsonModel = new JsonModel()
                {
                    errNum = 999,
                    errMsg = "fail",
                    status = "error"
                };
            }
            return jsonModel;
        }

        public JsonModel DelRole(Hashtable ht)
        {
            var result = DAL.DelRole(ht);
            JsonModel jsonModel = null;
            if (result)
            {
                jsonModel = new JsonModel()
                {
                    errNum = 0,
                    errMsg = "success",
                    status = "ok"
                };
            }
            else
            {
                jsonModel = new JsonModel()
                {
                    errNum = 999,
                    errMsg = "fail",
                    status = "error"
                };
            }
            return jsonModel;
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

    }
}
