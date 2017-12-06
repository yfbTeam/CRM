using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FEUtility
{
    public class ConverList<T> where T : new()
    {
        public static List<T> ConvertToList(DataTable dt)
        {

            // 定义集合  
            List<T> ts = new List<T>();

            try
            {
                // 获得此模型的类型  
                Type type = typeof(T);
                //定义一个临时变量  
                string tempName = string.Empty;
                //遍历DataTable中所有的数据行  
                foreach (DataRow dr in dt.Rows)
                {
                    T t = new T();
                    // 获得此模型的公共属性  
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    //遍历该对象的所有属性  
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;//将属性名称赋值给临时变量  
                        //检查DataTable是否包含此列（列名==对象的属性名）    
                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter  
                            if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                            //取值  
                            object value = dr[tempName];
                            //如果非空，则赋给对象的属性  
                            if (value != DBNull.Value)
                                pi.SetValue(t, value, null);
                        }
                    }
                    //对象添加到泛型集合中  
                    ts.Add(t);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return ts;

        }

        /// <summary>
        /// 将list对象集合转为dic
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ListToDic(List<T> list)
        {
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            try
            {
                //定义一个临时变量  
                string tempName = string.Empty;
                Type t = typeof(T);
                PropertyInfo[] propInfos = t.GetProperties();
                foreach (var u1 in list)
                {
                    Dictionary<string, object> dd = new Dictionary<string, object>();
                    foreach (var pi in propInfos)
                    {
                        string name = pi.Name;
                        //if (pi.PropertyType == typeof(DateTime?))
                        //{
                        //    string value = ((DateTime)pi.GetValue(u1, null)).ToString("yyyy-MM-dd HH:mm:ss");
                        //    dd.Add(name, value);
                        //}

                        object value = pi.GetValue(u1, null);
                        dd.Add(name, value);

                    }
                    dic.Add(dd);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dic;
        }

        /// <summary>
        /// 将list对象集合转为dic
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static Dictionary<string, object> T_ToDic(T obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                //定义一个临时变量  
                string tempName = string.Empty;
                Type t = typeof(T);
                PropertyInfo[] propInfos = t.GetProperties();
                foreach (var pi in propInfos)
                {
                    string name = pi.Name;
                    //if (pi.PropertyType == typeof(DateTime?))
                    //{
                    //    string value = ((DateTime)pi.GetValue(u1, null)).ToString("yyyy-MM-dd HH:mm:ss");
                    //    dd.Add(name, value);
                    //}

                    object value = pi.GetValue(obj, null);
                    dic.Add(name, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dic;
        }

    }
}
