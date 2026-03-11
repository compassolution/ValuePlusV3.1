using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Web.Script.Serialization;

namespace Com.ValuePlus.Weixin
{/// <summary>
    /// Json序列化和反序列化辅助类 
    /// </summary>
    public class JsonHelper
    {
        /// <summary> 
        /// Json序列化 
        /// </summary> 
        public static string JsonSerializer<T>(T obj)
        {
            string jsonString = string.Empty;
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    jsonString = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch
            {
                jsonString = string.Empty;
            }
            return jsonString;
        }

        /// <summary> 
        /// Json反序列化
        /// </summary> 
        public static T JsonDeserialize<T>(string jsonString)
        {
            T obj = Activator.CreateInstance<T>();
            try
            {
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());//typeof(T)
                    T jsonObject = (T)ser.ReadObject(ms);
                    ms.Close();

                    return jsonObject;
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 将 DataTable 序列化成 json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return "\"\"";
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    result.Add(dc.ColumnName, dr[dc].ToString());
                }
                list.Add(result);
            }
            return myJson.Serialize(list);
        }

        /// <summary>
        ///  将对象序列化成 json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.Serialize(obj);
        }

        /// <summary>
        /// 将 json 字符串反序列化成对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object JsonToObject(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.DeserializeObject(json);
        }

        /// <summary>
        /// 将 json 字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }
            JavaScriptSerializer myJson = new JavaScriptSerializer();

            return myJson.Deserialize<T>(json);
        }

        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }

    }
}
