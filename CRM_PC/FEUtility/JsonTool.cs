using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEUtility
{
    public static class JsonTool
    {
        public static string GetStringVal(JObject node, string key)
        {
            if (node[key] == null) return default(string);
            return ((JValue)node[key]).Value.ToString();
        }

        public static JObject GetObjVal(JObject node, string key)
        {
            if (node[key] == null) return default(JObject);
            return (JObject)node[key];
        }

        public static JArray GetArryVal(JObject node, string key)
        {
            if (node[key] == null) return default(JArray);
            return (JArray)node[key];
        }
    }
}
