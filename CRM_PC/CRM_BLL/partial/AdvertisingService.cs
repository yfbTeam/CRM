
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
    /// 数据操作类：对新闻、菜单等列表数据的操作。
    /// </summary>
    public partial class AdvertisingService
    {

        MultiEvaluation_DAL.AdvertisingDal DAL = new MultiEvaluation_DAL.AdvertisingDal();
        BLLCommon common = new BLLCommon();

        public JsonModel GetDataInfo(Hashtable ht)
        {
            DataTable dt = DAL.GetDataInfo(ht);
            DataView dv = dt.DefaultView;
            dv.Sort = " SortId desc,CreateTime desc ";
            DataTable dt2 = dv.ToTable();
            return GetDataTableToJsonModel(dt2, ht);
        }

        public JsonModel GetAdvertData(Hashtable ht)
        {
            DataTable dt = DAL.GetAdvertData(ht);
            if (ht.ContainsKey("MenuId") && !string.IsNullOrWhiteSpace(Convert.ToString(ht["MenuId"])))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = " SortId desc,CreateTime desc ";
                DataTable dt2 = dv.ToTable();
                return GetDataTableToJsonModel(dt2, ht);
            }
            else
            {
                DataView dv = dt.DefaultView;
                dv.Sort = " SortId desc,CreateTime desc ";
                DataTable dt2 = dv.ToTable();
                return GetDataTableToJsonModel(dt2, ht);
            }
        }

        public int GetSortIdForAdvert(int id, int mid)
        {
            return DAL.GetSortIdForAdvert(id, mid);
        }

        public JsonModel UpdateAdertSort(Hashtable ht)
        {
            JsonModel jsonModel = new JsonModel();
            try
            {
                int num = DAL.UpdateAdertSort(ht);
                if (num == -1)
                {
                    jsonModel = new JsonModel()
                    {
                        errMsg = "max",
                    };
                }
                else if (num == -2)
                {
                    jsonModel = new JsonModel()
                    {
                        errMsg = "min",
                    };
                }
                else if (num == -999)
                {
                    jsonModel = new JsonModel()
                    {
                        errMsg = "error",
                    };
                }
                else
                {
                    jsonModel = new JsonModel()
                    {
                        errMsg = "success",
                    };
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel = new JsonModel()
                {
                    errMsg = ex.Message,
                };
                return jsonModel;
            }
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
