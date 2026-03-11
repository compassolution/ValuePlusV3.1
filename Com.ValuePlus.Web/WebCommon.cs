using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Com.ValuePlus.Common.Security;

namespace Com.ValuePlus.Web
{
    /// <summary>
    ///WebCommon 的摘要说明
    ///主要用于从浏览器客户端请求数据时服务器端的处理
    /// </summary>
    public class WebCommon
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 根据特殊算法分析url连接，返回参数及其值的hashtable
        /// <summary>
        /// 根据特殊算法分析url连接，返回参数及其值的hashtable
        /// </summary>
        /// <param name="strUrlQueryString"></param>
        /// <returns>Hashtable</returns>
        public static Hashtable GetUrlAnalyse(String strUrlQueryString)
        {
            Hashtable hsTable = new Hashtable();
            if (!String.IsNullOrEmpty(strUrlQueryString) && (strUrlQueryString.IndexOf('?') == 0))
            {
                //首先去掉首位?
                strUrlQueryString = strUrlQueryString.Substring(1, strUrlQueryString.Length - 1);

                String[] strArray = strUrlQueryString.Split('&');
                for (int i = 0; i < strArray.Length; i++)
                {
                    String[] strArray3 = strArray[i].Split('=');
                    if (strArray3.Length >= 2)
                    {
                        //防止有多个=号的存在，则取第一个=号前的都为参数，后面的所有都为参数值
                        String strParam = strArray3[0];
                        String strParamValue = strArray[i].Substring(strParam.Length + 1, strArray[i].Length - strParam.Length - 1);

                        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                        strParamValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strParamValue);

                        hsTable.Remove(strParam);
                        hsTable.Add(strParam, strParamValue);
                    }
                }
            }
            return hsTable;
        }
        #endregion

        /// <summary>
        /// 根据数据集输出行的Json格式字符串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="strNodeName">根节点名称</param>
        /// <param name="strColumnJson">列名json字符串</param>
        /// <returns></returns>
        public static String GetDataRowJsonString(DataTable dt, String strNodeName, String strColumnsJson)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dt.Columns.Count;
                sBuilder.Append("{" + strNodeName + ":[ ");
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            String strColName = dt.Columns[j].ColumnName;
                            String strColType = dt.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                            if (!String.IsNullOrEmpty(strColValue))
                            {
                                switch (strColType)
                                {
                                    case "System.DateTime":
                                        strColValue = DateTime.Parse(strColValue).ToString("yyyy-MM-dd HH:mm:ss");
                                        //如果是短日期
                                        if (strColValue.EndsWith("00:00:00"))
                                        {
                                            strColValue = strColValue.Substring(0, strColValue.Length - 9);
                                        }
                                        break;
                                    case "System.Byte[]":
                                        //如果是二进制的类型，则转化成Base64编码
                                        byte[] btValue = (byte[])dt.Rows[i][dt.Columns[j].ColumnName];
                                        strColValue = Convert.ToBase64String(btValue);
                                        //if (strColName.Equals("SIMAGE"))
                                        //{
                                        //    log.Error("SIMAGE Data Type:" + strColType);
                                        //    log.Error("SIMAGE Base64:" + strColValue);
                                        //}
                                        break;
                                }
                            }
                            strColValue = Microsoft.JScript.GlobalObject.escape(strColValue);

                            if (j == 0)
                            {
                                sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                            else
                            {
                                sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                        }

                        sBuilder.Append("}");
                    }

                }
                sBuilder.Append("]");
                if (!String.IsNullOrEmpty(strColumnsJson))
                {
                    if (strColumnsJson.Substring(0, 1).Equals("{"))
                    {
                        strColumnsJson = strColumnsJson.TrimStart('{');
                    }
                    if (strColumnsJson.Substring(strColumnsJson.Length - 1, 1).Equals("}"))
                    {
                        strColumnsJson = strColumnsJson.TrimEnd('}');
                    }
                    sBuilder.Append("," + strColumnsJson);
                }
                sBuilder.Append("}");

                json = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }

        /// <summary>
        /// 根据数据集输出列的Json格式字符串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <returns></returns>
        public static String GetColumnJsonString(DataTable dt)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dt.Columns.Count;
                sBuilder.Append("{colNames:[ ");
                sBuilder.Append("{");

                for (int j = 0; j < iColCount; j++)
                {
                    String strColName = dt.Columns[j].ColumnName;
                    String strColValue = dt.Columns[j].ColumnName;

                    if (j == 0)
                    {
                        //sBuilder.Append(strColName + ":'" + strColValue + "'");
                        sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                    }
                    else
                    {
                        //sBuilder.Append("," + strColName + ":'" + strColValue + "'");
                        sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                    }

                }
                sBuilder.Append("}");
                sBuilder.Append("]");
                sBuilder.Append("}");

                json = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }
        
        /// <summary>
        /// 根据数据集输出行的Json格式字符串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="strNodeName">根节点名称</param>
        /// <param name="isEscape">是否Escape值</param>
        /// <returns></returns>
        public static String GetJsonStringByDataTable(DataTable dt, String strNodeName, bool isEscape)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dt.Columns.Count;
                //sBuilder.Append("\"" + strNodeName + "\":[ ");
                sBuilder.Append(strNodeName + ":[ ");
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            String strColName = dt.Columns[j].ColumnName;
                            String strColType = dt.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                            if (!String.IsNullOrEmpty(strColValue))
                            {
                                switch (strColType)
                                {
                                    case "System.DateTime":
                                        strColValue = DateTime.Parse(strColValue).ToString("yyyy-MM-dd HH:mm:ss");
                                        //如果是短日期
                                        if (strColValue.EndsWith("00:00:00"))
                                        {
                                            strColValue = strColValue.Substring(0, strColValue.Length - 9);
                                        }
                                        break;
                                    case "System.Byte[]":
                                        //如果是二进制的类型，则转化成Base64编码
                                        byte[] btValue = (byte[])dt.Rows[i][dt.Columns[j].ColumnName];
                                        strColValue = Convert.ToBase64String(btValue);
                                        break;
                                }
                            }
                            if (isEscape)
                            {
                                strColValue = Microsoft.JScript.GlobalObject.escape(strColValue);
                            }

                            if (j == 0)
                            {
                                sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                            else
                            {
                                sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                        }

                        sBuilder.Append("}");
                    }

                }
                sBuilder.Append("]");

                json = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }

        /// <summary>
        /// 根据数据集输出行的Json格式字符串 add by sammen 增加是否字段名转大小写
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="strNodeName">根节点名称</param>
        /// <param name="isEscape">是否Escape值</param>
        /// <param name="isColumnUpper">字段名是否转大写</param>
        /// <returns></returns>
        public static String GetJsonStringByDataTable(DataTable dt, String strNodeName, bool isEscape,bool isColumnUpper)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dt.Columns.Count;
                //sBuilder.Append("\"" + strNodeName + "\":[ ");
                sBuilder.Append(strNodeName + ":[ ");
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            String strColName = dt.Columns[j].ColumnName;
                            if (isColumnUpper) {
                                strColName = strColName.ToUpper();
                            }
                            String strColType = dt.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                            if (!String.IsNullOrEmpty(strColValue))
                            {
                                switch (strColType)
                                {
                                    case "System.DateTime":
                                        strColValue = DateTime.Parse(strColValue).ToString("yyyy-MM-dd HH:mm:ss");
                                        //如果是短日期
                                        if (strColValue.EndsWith("00:00:00"))
                                        {
                                            strColValue = strColValue.Substring(0, strColValue.Length - 9);
                                        }
                                        break;
                                    case "System.Byte[]":
                                        //如果是二进制的类型，则转化成Base64编码
                                        byte[] btValue = (byte[])dt.Rows[i][dt.Columns[j].ColumnName];
                                        strColValue = Convert.ToBase64String(btValue);
                                        break;
                                }
                            }
                            if (isEscape)
                            {
                                strColValue = Microsoft.JScript.GlobalObject.escape(strColValue);
                            }

                            if (j == 0)
                            {
                                sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                            else
                            {
                                sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                        }

                        sBuilder.Append("}");
                    }

                }
                sBuilder.Append("]");

                json = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }
        
        /// <summary>
        /// 根据数据集输出行的Json格式字符串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="strNodeName">根节点名称</param>
        /// <returns></returns>
        public static String GetJsonStringByDataTable(DataTable dt, String strNodeName)
        {
            return GetJsonStringByDataTable(dt, strNodeName, true);
        }

        /// <summary>
        /// 根据数据集输出第一行的Json格式字符串从{开始到}结束
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="isEscape">是否Escape值</param>
        /// <returns></returns>
        public static String GetJsonStringByFirstDataRow(DataTable dt,bool isEscape)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dt.Columns.Count;
                if ((dt != null) && (dt.Rows.Count >0))
                {
                    DataRow dr = dt.Rows[0];
                    sBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String strColName = dt.Columns[j].ColumnName;
                        String strColType = dt.Columns[j].DataType.ToString();
                        //对值进行编码处理特殊字符，如引号等
                        String strColValue = dr[dt.Columns[j].ColumnName].ToString();
                        if (!String.IsNullOrEmpty(strColValue))
                        {
                            switch (strColType)
                            {
                                case "System.DateTime":
                                    strColValue = DateTime.Parse(strColValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    //如果是短日期
                                    if (strColValue.EndsWith("00:00:00"))
                                    {
                                        strColValue = strColValue.Substring(0, strColValue.Length - 9);
                                    }
                                    break;
                                case "System.Byte[]":
                                    //如果是二进制的类型，则转化成Base64编码
                                    byte[] btValue = (byte[])dr[dt.Columns[j].ColumnName];
                                    strColValue = Convert.ToBase64String(btValue);
                                    break;
                            }
                        }
                        if (isEscape)
                        {
                            strColValue = Microsoft.JScript.GlobalObject.escape(strColValue);
                        }

                        if (j == 0)
                        {
                            sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                        }
                        else
                        {
                            sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                        }
                    }

                    sBuilder.Append("}");
                }

                json = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }

        /// <summary>
        /// 获取HttpContext请求参数并返回json字符串
        /// 【主要用于url中传递的对象】
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonParamsFromContext(HttpContext context)
        {
            try
            {
                //解析客户端传递过来的json data
                StreamReader reader = new StreamReader(context.Request.InputStream);
                //String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());//测试用这个urlDecode会有乱码特别遇上%时
                String strParamJson = reader.ReadToEnd();
                //log.Error("获取HttpContext请求参数中的json data：" + strParamJson);

                return strParamJson;
            }
            catch (Exception ex)
            {
                String strCatchErroTitle = "获取Json字符串某节点的值 时出错，出错说明这个节点不存在则返回空字符串 \r\n";
                String strCatchErroMsg = ex.ToString();
                log.Error(strCatchErroTitle + strCatchErroMsg);

                return "";
            }
        }

        /// <summary>
        /// 获取Json字符串某节点的值
        /// 【主要用于url中传递的对象】
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = "";
            try
            {
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
                        result = result.Replace("\r\n","");
                        result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                        result = SQLInjectionDefense.ReplaceSQLReservedKeyword(result);
                    }
                }
            }
            catch (Exception ex)
            {
                //log.Error("获取Json字符串某节点的值错误:" + ex);
                //log.Error("出错说明这个节点不存在，则返回空字符串！");
            }
            result = Microsoft.JScript.GlobalObject.unescape(result);
            return result;
        }

        /// <summary>
        /// 获取Json字符串某节点的值
        /// 【主要用于url中传递的对象】
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonObjectValue(string jsonStr, string key)
        {
            string result = "";
            try
            {
                //JArray jsonArray = (JArray)JsonConvert.DeserializeObject(jsonStr);
                //JObject jo = (JObject)jsonArray[0];

                JObject jo = JObject.Parse(jsonStr);    //paramsStr - json字符串名字

                result = jo[key] == null ? "": jo[key].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                result = SQLInjectionDefense.ReplaceSQLReservedKeyword(result);
            }
            catch (Exception ex)
            {                
                //log.Error("获取Json字符串某节点的值错误:" + ex);
                //log.Error("出错说明这个节点不存在，则返回空字符串！");
            }
            return result;
        }


    }
}
