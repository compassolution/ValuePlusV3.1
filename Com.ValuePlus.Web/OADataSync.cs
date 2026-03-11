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
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.Web
{
    /// <summary>
    ///OADataSync 的摘要说明
    ///主要用于内外网数据同步时的公共类
    /// </summary>
    public class OADataSync
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 数据同步临时表名前缀-->从内网到外网
        /// </summary>
        public const string prefix_InnerToOuter = "DS_FI_";

        /// <summary>
        /// 数据同步临时表名前缀-->从外网到内网
        /// </summary>
        public const string prefix_OuterToInner = "DS_FO_";

        /// <summary>
        /// 创建临时表结构，并新增最后日期和最后用户字段
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CreateTempTable(string strTableNameFrom, string strTableNameTo)
        {
            string result = "";
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + strTableNameTo + "]') AND type in (N'U'))\r\n");
                sbSql.Append("DROP TABLE [dbo].[" + strTableNameTo + "];\r\n");
                //sbSql.Append("GO\r\n");
                sbSql.Append("select top 1 * into " + strTableNameTo + " from "+ strTableNameFrom + ";\r\n");
                sbSql.Append("delete from " + strTableNameTo + ";\r\n");
                sbSql.Append("alter table " + strTableNameTo + " add LASTINSERTTIME VARCHAR(30) NULL;\r\n");
                sbSql.Append("alter table " + strTableNameTo + " add LASTINSERTUSER VARCHAR(30) NULL;\r\n");
                sbSql.Append("\r\n");
                //sbSql.Append("GO\r\n");
            }
            catch (Exception ex)
            {
                log.Error("创建临时表结构，并新增最后日期和最后用户字段出错:" + ex);
                log.Error("创建临时表结构，并新增最后日期和最后用户字段出错:" + sbSql.ToString());
            }
            return sbSql.ToString();
        }

        /// <summary>
        /// 将JSON字符串转成对应表名的insert语句
        /// </summary>
        /// <param name="json"></param>
        /// <param name="tempTableName">临时表名</param>
        /// <param name="strAimTableName">目标表名</param>
        /// <returns></returns>
        public static String ConvertJsonToInsertSql(string json, string tempTableName,String strAimTableName)
        {
            //从syscolumn中获取表结构中的列集合
            String strColumnsSql = "select B.name from sysobjects a inner join syscolumns b on a.id = b.id where a.name = '" + strAimTableName + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strColumnsSql);

            //判断当前表是否存在标识列，如果存在则在插入前和插入后做打开IDENTITY_INSERT和关闭IDENTITY_INSERT处理
            String strSql_Identity = "SELECT count(1) FROM sys.columns c JOIN sys.tables t ON c.object_id = t.object_id WHERE t.name = '" + strAimTableName + "' AND c.is_identity = 1";
            int iCount_Identity = SqlParamDao.ExecuteScalarBySql(strSql_Identity);

            StringBuilder sbSqlInsert = new StringBuilder();
            List<string> insertSql = new List<string>();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(json);
            int iCount = jArray.Count;
            foreach (JObject jObject in jArray)
            {
                List<string> columns = new List<string>();
                List<string> values = new List<string>();
                foreach (JProperty property in jObject.Properties())
                {
                    string propertyName = property.Name;
                    //JToken propertyValue = property.Value;
                    string propertyValue = property.Value.ToString();
                    if(strAimTableName.ToUpper().Equals("TB_HR_USER")&&propertyName.ToUpper().Equals("SPWD"))
                    {
                        propertyValue = Microsoft.JScript.GlobalObject.unescape(propertyValue);
                        //byte[] btValue = Convert.FromBase64String(propertyValue);
                        //base64字符串转二进制
                        propertyValue = "CAST('"+ propertyValue + "' AS XML).value('.', 'VARBINARY(MAX)')";
                    }
                    else
                    {
                        //如果是空字符串则设置为null，防止有些类型如数值型无法写入空字符串
                        if (String.IsNullOrEmpty(propertyValue))
                        {
                            propertyValue = "null";
                        }else
                        {
                            propertyValue = "'" + Microsoft.JScript.GlobalObject.unescape(propertyValue).Replace("'", "''") + "'";
                        }
                    }

                    //判断此列名是否在表结构中存在，如果存在才进行插入
                    DataRow[] filteredRows = dt.Select("name = '"+ propertyName + "'");
                    if(filteredRows.Length==1)
                    {
                        columns.Add(propertyName);
                        values.Add(propertyValue);
                    }
                }
                string columnList = string.Join(", ", columns.ToArray());
                string valueList = string.Join(", ", values.ToArray());

                string insertStatement = $"INSERT INTO {tempTableName} ({columnList}) VALUES ({valueList});";
                insertStatement = insertStatement + ";\r\n\r\n";

                if(iCount_Identity>0)
                {
                    //"-- 开启 IDENTITY_INSERT\r\n
                    sbSqlInsert.Append("SET IDENTITY_INSERT " + tempTableName + " ON;");
                }

                sbSqlInsert.Append(insertStatement);
                if (iCount_Identity > 0)
                {
                    //"-- 关闭 IDENTITY_INSERT\r\n
                    sbSqlInsert.Append("SET IDENTITY_INSERT " + tempTableName + " OFF;");
                }
                //insertSql.Add(insertStatement);
            }

            //String strReturn = string.Join("; ", insertSql.ToArray());
            String strReturn = sbSqlInsert.ToString();
            return strReturn;
        }

        ///// <summary>
        ///// 根据同步编码获取配置中的表名及条件中的数据并拼接成json数据字符串【暂时不用】
        ///// </summary>
        ///// <param name="strDSCode"></param>
        ///// <returns></returns>
        //public static String GetDataJsonByDSCode(String strDSCode)
        //{
        //    StringBuilder sBuilder = new StringBuilder();
        //    String strSql = "select * from OADataSync_2 where DSCODE = '"+ strDSCode + "' ORDER BY SEQNO";
        //    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        //    sBuilder.Append("{");
        //    if(dt!=null && dt.Rows.Count>0){
        //        for(int i=0;i<dt.Rows.Count;i++)
        //        {
        //            String strTableName = dt.Rows[i]["TABLENAME"].ToString();
        //            String strFilterSql = dt.Rows[i]["FILTERSQL"].ToString();

        //            String strSql_Data = "select * from " + strTableName + " A where 1=1 ";
        //            //如果有过滤条件SQL语句，则加载
        //            if (!String.IsNullOrEmpty(strFilterSql))
        //            {
        //                strSql_Data = strSql_Data + " AND "+ strFilterSql;
        //            }
        //            DataTable dt_Data = SqlParamDao.GetDataTableBySql(strSql_Data);
        //            sBuilder.Append((i > 0 ? "," : "") + WebCommon.GetJsonStringByDataTable(dt_Data, "\"" + strOpType + "" + strTableName + "\"", true));
        //        }
        //    }
        //    sBuilder.Append("}");

        //    return sBuilder.ToString();
        //}

        ///// <summary>
        ///// 将数据集json写入到对应的临时表中，并且更新最后操作时间及用户字段【暂时不用】
        ///// </summary>
        ///// <param name="strDSCode"></param>
        ///// <param name="jsonObject"></param>
        ///// <param name="strUserId"></param>
        ///// <returns></returns>
        //public static int InsertDataToTempTableByDSCode(String strDSCode, JObject jsonObject,String strOpType, String strUserId)
        //{
        //    int iReturnResult = 0;
        //    StringBuilder sbSql = new StringBuilder();
        //    String strSql = "select * from OADataSync_2 where DSCODE = '" + strDSCode + "' ORDER BY SEQNO";
        //    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            String strTableName = dt.Rows[i]["TABLENAME"].ToString();
        //            String strJson_FromInner = jsonObject[strOpType + strTableName].ToString();
        //            String strCreateTable_Outer = OADataSync.CreateTempTable(strTableName, strOpType + strTableName);
        //            String strInsertSql_Outer = OADataSync.ConvertJsonToInsertSql(strJson_FromInner, strOpType + strTableName, strTableName);
        //            String strUpdateLast_Outer = "UPDATE " + strOpType + strTableName + " SET LASTINSERTTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',LASTINSERTUSER = '" + strUserId + "'";
        //            sbSql.Append(strCreateTable_Outer);
        //            sbSql.Append(strInsertSql_Outer);
        //            sbSql.Append(strUpdateLast_Outer);
        //        }
        //    }
        //    if(!String.IsNullOrEmpty(sbSql.ToString()))
        //    {
        //        iReturnResult = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
        //    }

        //    return iReturnResult;
        //}

        /// <summary>
        /// 根据同步编码获取配置中的表名及条件中的数据并拼接成json数据字符串
        /// </summary>
        /// <param name="strDSCode"></param>
        /// <param name="strOpType">prefix_InnerToOuter/prefix_OuterToInner</param>
        /// <returns></returns>
        public static String GetDataJsonByOADataSync2Config(String strDSCode, JObject jsonObject, String strOpType)
        {
            StringBuilder sBuilder = new StringBuilder();
            String strJson_FromInner = jsonObject["OADataSync_2"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(strJson_FromInner);
            sBuilder.Append("{");
            if (jArray.Count > 0)
            {
                int i = 0;
                foreach (JObject jObject in jArray)
                {
                    String strTableName = "";
                    String strFilterSql = "";
                    foreach (JProperty property in jObject.Properties())
                    {
                        switch (property.Name.ToUpper())
                        {
                            case "TABLENAME":
                                strTableName = property.Value.ToString();
                                break;
                            case "FILTERSQL":
                                strFilterSql = Microsoft.JScript.GlobalObject.unescape(property.Value.ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    String strSql_Data = "select * from " + strTableName + " A where 1=1 ";
                    //如果有过滤条件SQL语句，则加载
                    if (!String.IsNullOrEmpty(strFilterSql))
                    {
                        strSql_Data = strSql_Data + " AND " + strFilterSql;
                    }
                    DataTable dt_Data = SqlParamDao.GetDataTableBySql(strSql_Data);
                    sBuilder.Append((i > 0 ? "," : "") + WebCommon.GetJsonStringByDataTable(dt_Data, "\"" + strOpType + "" + strTableName + "\"", true));

                    i++;
                }
            }
            sBuilder.Append("}");

            return sBuilder.ToString();
        }

        /// <summary>
        /// 根据同步编码获取配置中的表名及条件将数据集json写入到对应的临时表中，并且更新最后操作时间及用户字段
        /// </summary>
        /// <param name="strDSCode"></param>
        /// <param name="jsonSyncConfig2Object"></param>
        /// <param name="jsonSyncDataObject"></param>
        /// <param name="strOpType">prefix_InnerToOuter/prefix_OuterToInner</param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public static int InsertDataToTempTableByOADataSync2Config(String strDSCode, JObject jsonSyncConfig2Object, JObject jsonSyncDataObject,String strOpType, String strUserId,ref StringBuilder sbSql)
        {
            int iReturnResult = 0;
            String strJson_OADataSync_2 = jsonSyncConfig2Object["OADataSync_2"].ToString();
            JArray jArray = (JArray)JsonConvert.DeserializeObject(strJson_OADataSync_2);

            if (jArray.Count > 0)
            {
                int i = 0;
                foreach (JObject jObject in jArray)
                {
                    String strTableName = "";
                    foreach (JProperty property in jObject.Properties())
                    {
                        switch (property.Name.ToUpper())
                        {
                            case "TABLENAME":
                                strTableName = property.Value.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                    String strJson_FromInner = jsonSyncDataObject[strOpType + strTableName].ToString();
                    String strCreateTable_Outer = OADataSync.CreateTempTable(strTableName, strOpType + strTableName);
                    String strInsertSql_Outer = OADataSync.ConvertJsonToInsertSql(strJson_FromInner, strOpType + strTableName, strTableName);
                    String strUpdateLast_Outer = "UPDATE " + strOpType + strTableName + " SET LASTINSERTTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',LASTINSERTUSER = '" + strUserId + "'";
                    sbSql.Append(strCreateTable_Outer);
                    sbSql.Append(strInsertSql_Outer);
                    sbSql.Append(strUpdateLast_Outer);

                    i++;
                }
            }
            if (!String.IsNullOrEmpty(sbSql.ToString()))
            {
                iReturnResult = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }

            return iReturnResult;
        }

    }
}
