using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class CommonJson
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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

    }
}
