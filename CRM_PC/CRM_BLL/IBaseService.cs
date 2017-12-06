using MultiEvaluation_Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiEvaluation_BLL
{
    public interface IBaseService<T> where T : class ,new()
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        JsonModel Add(T entity);

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        JsonModel Update(T entity, SqlTransaction trans = null);

        /// <summary>
        /// 伪删除单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel DeleteFalse(int id);

        /// <summary>
        /// 批量伪删除数据 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        JsonModel DeleteBatchFalse(params int[] ids);

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel Delete(int id);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        JsonModel DeleteBatch(params int[] ids);

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JsonModel GetEntityById(int id);

        /// <summary>
        /// 根据某个字段获取信息
        /// </summary>
        /// <param name="filed"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        JsonModel GetEntityListByField(string filed, string value);

        /// <summary>
        /// 指定页获取信息（根据条件)【返回的是jsmodel数据】
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="IsPage"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        JsonModel GetPage(Hashtable ht, bool IsPage = true, string Where = "");

        /// <summary>
        /// 指定页获取信息（根据条件)【返回的是Dataset数据】
        /// </summary>
        /// <param name="ht"></param>
        /// <param name="IsPage"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        DataTable GetData(Hashtable ht, bool IsPage = true, string Where = "");

        /// <summary>
        /// 判断是否含有某条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckInfoById(int id);

    }
}
