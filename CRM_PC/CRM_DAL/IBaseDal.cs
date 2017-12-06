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
    public interface IBaseDal<T> where T : class ,new()
    {
        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add(T entity);

        /// <summary>
        /// 更新单挑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        bool Update(T entity, SqlTransaction trans = null);

        /// <summary>
        /// 伪删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteFalse(T entity, int id);

        /// <summary>
        /// 批量伪删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        int DeleteBatchFalse(T entity, params int[] ids);

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(T entity, int id);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        int DeleteBatch(T entity, params int[] ids);

        /// <summary>
        /// 根据Id获取单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntityById(T entity, int id);

        /// <summary>
        /// 根据id判读数据是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckInfoById(T entity, int id);

        /// <summary>
        /// 根据某个字段获取数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        List<T> GetEntityListByField(T entity, string filed, string value);


        /// <summary>
        /// 获取数据（返回datatable）
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="RowCount"></param>
        /// <param name="IsPage"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        DataTable GetListByPage(Hashtable ht, out int RowCount, bool IsPage = true, string Where = "");

    }
}
