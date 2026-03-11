using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DAL;
using Newtonsoft.Json;
using System.Data;

namespace Com.ValuePlus.Labor
{
    public class JObjectToDB
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 将JObject对象中的数据update更新到指定表中
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="strTableName"></param>
        /// <param name="hsTableKey"></param>
        /// <returns></returns>
        public static int UpdateJObjectDataToTable(JObject jo,String strTableName,Hashtable hsTableKey)
        {
            int iCount = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("update "+strTableName+" set ");

                int iIndex = 0;
                foreach (var item in jo)
                {
                    string strColumnName = item.Key;
                    string strColumnValue = item.Value.ToString();

                    //判断字段是否需要加密的处理
                    string strColumnDataString = GetColumnEncryptDataString(strTableName, strColumnName, strColumnValue);

                    //if (JudgeTableColumnIsExists(strTableName, strColumnName))
                    if (JudgeTableColumnIsExists(strColumnName))
                    {
                        if (iIndex == 0)
                        {
                            sbSql.Append(strColumnName + " = " + strColumnDataString);
                        }
                        else
                        {
                            sbSql.Append(" ," + strColumnName + " = " + strColumnDataString);
                        }
                        iIndex++;
                    }
                }
                sbSql.Append(" where 1=1 ");                
                foreach (string key in hsTableKey.Keys)
                {
                    //判断字段是否需要加密的处理
                    string strKeyColumnDataString = GetColumnEncryptDataString(strTableName, key, hsTableKey[key].ToString());

                    sbSql.Append(" and " + key + " = " + strKeyColumnDataString);
                }

                log.Error("将JObject对象中的数据update更新到指定表中,SQL:"+ sbSql.ToString());
                iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error("将JObject对象中的数据update更新到指定表中,SQL:" + sbSql.ToString());
                log.Error(ex);
            }
            return iCount;

        }

        /// <summary>
        /// 将JObject对象中的数据新增插入到指定表中
        /// </summary>
        /// <param name="jo"></param>
        /// <param name="strTableName"></param>
        /// <param name="hsTableKey"></param>
        /// <returns></returns>
        public static int InsertJObjectDataToTable(JObject jo, String strTableName, Hashtable hsTableKey)
        {
            int iCount = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                StringBuilder sbColumnName = new StringBuilder();
                StringBuilder sbColumnValue = new StringBuilder();
                sbSql.Append("insert into " + strTableName );

                int iIndex = 0;
                foreach (var item in jo)
                {
                    string strColumnName = item.Key;
                    string strColumnValue = item.Value.ToString();
                    if (hsTableKey.ContainsKey(strColumnName))
                    {
                        strColumnValue = hsTableKey[strColumnName].ToString();
                    }
                    //if (JudgeTableColumnIsExists(strTableName, strColumnName))
                    if (JudgeTableColumnIsExists(strColumnName))
                    {
                        //判断字段是否需要加密的处理
                        string strColumnDataString = GetColumnEncryptDataString(strTableName, strColumnName, strColumnValue);

                        if (iIndex == 0)
                        {
                            sbColumnName.Append(strColumnName);
                            sbColumnValue.Append(strColumnDataString);
                        }
                        else
                        {
                            sbColumnName.Append(" ," + strColumnName);
                            sbColumnValue.Append(" ," + strColumnDataString);
                        }
                        iIndex++;
                    }
                }
                sbSql.Append("(" + sbColumnName.ToString() + ") values (" + sbColumnValue.ToString() + ")");

                log.Error("将JObject对象中的数据新增插入到指定表中,SQL:" + sbSql.ToString());
                iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error("将JObject对象中的数据新增插入到指定表中,SQL:" + sbSql.ToString());
                log.Error(ex);
            }
            return iCount;

        }

        /// <summary>
        /// 判断某记录在表中是否存在
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="hsTableKey"></param>
        /// <returns></returns>
        public static bool JudgeRecordIsExists(String strTableName, Hashtable hsTableKey)
        {
            bool IsRecordExists = true;
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(" select count(1) from " + strTableName + " where 1=1 ");
                foreach (string key in hsTableKey.Keys)
                {
                    //判断字段是否需要加密的处理
                    string strKeyColumnDataString = GetColumnDecodeDataString(strTableName, key, hsTableKey[key].ToString());

                    sbSql.Append(" and " + key + " = " + strKeyColumnDataString );
                }

                log.Error("判断某记录在表中是否存在,SQL:" + sbSql.ToString());
                int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
                if (iCount <= 0)
                {
                    IsRecordExists = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return IsRecordExists;
        }

        /// <summary>
        /// 判断某记录在表中是否存在返回json
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strFiledValueJsonObject">条件的json对象</param>
        /// <param name="strExceptKeyJsonObject">例外的主键键值对json对象</param>
        /// <returns></returns>
        public static String JudgeRecordIsExistsReturnJson(String strTableName, String strFiledValueJsonObject, String strExceptKeyJsonObject)
        {
            String strReturnCode = "0";
            String strReturnMsg = "";
            StringBuilder sbResultData = new StringBuilder();

            StringBuilder sbResult = new StringBuilder();
            try
            {
                sbResultData.Append("\"ResultData\":{\"1\":\"1\"");

                //判断是否需要除了某些记录以外的判断
                StringBuilder sbSql_ExceptKey = new StringBuilder();
                if (!String.IsNullOrEmpty(strExceptKeyJsonObject))
                {
                    //json字符串转json的JObject对象
                    JObject jo_ExceptKey = (JObject)JsonConvert.DeserializeObject(strExceptKeyJsonObject);
                    sbSql_ExceptKey.Append(" and not (1=1 ");
                    foreach (var item in jo_ExceptKey)
                    {
                        string strKeyName = item.Key;
                        string strKeyValue = item.Value.ToString();

                        //判断字段是否需要加密的处理
                        string strKeyColumnDataString = GetColumnDecodeDataString(strTableName, strKeyName, strKeyValue);

                        sbSql_ExceptKey.Append(" and " + strKeyName + " = " + strKeyColumnDataString);
                    }
                    sbSql_ExceptKey.Append(" )");
                }

                //json字符串转json的JObject对象
                JObject jo = (JObject)JsonConvert.DeserializeObject(strFiledValueJsonObject);
                foreach (var item in jo)
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append(" select count(1) from " + strTableName + " where 1=1 ");

                    String strReturnIs = "0";//默认不存在
                    string strColumnName = item.Key;
                    string strColumnValue = item.Value.ToString();

                    //判断字段是否需要加密的处理
                    string strColumnDataString = GetColumnDecodeDataString(strTableName, strColumnName, strColumnValue);
                    sbSql.Append(" and " + strColumnName + " = " + strColumnDataString);
                    if (!String.IsNullOrEmpty(sbSql_ExceptKey.ToString()))
                    {
                        sbSql.Append(sbSql_ExceptKey.ToString());
                    }

                    int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());

                    if (iCount > 0)
                    {
                        strReturnIs = "1";
                    }
                    sbResultData.Append("   ,\"" + strColumnName + "\":\"" + strReturnIs + "\"");
                    //sbResultData.Append(",\"" + strColumnName + "\":{\"" + strColumnValue + "\":\"\",\"IsExists\":\"" + strReturnIs +"\"}");

                }
                sbResultData.Append("}");
                strReturnCode = "1";
                strReturnMsg = "成功判断某记录在表"+ strTableName + "中是否存在";
                log.Error("成功判断某记录在表" + strTableName + "中是否存在" + sbResultData.ToString());
            }
            catch (Exception ex)
            {
                strReturnCode = "-99";
                strReturnMsg = "判断某记录在表" + strTableName + "中是否存在出错";
                log.Error(ex);
            }
            finally
            {
                StringBuilder sbResultStatus = new StringBuilder();
                sbResultStatus.Append("\"ReturnStatus\":{");
                sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
                sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
                sbResultStatus.Append("}");

                sbResult.Append("{");
                sbResult.Append(sbResultStatus.ToString());
                if (!String.IsNullOrEmpty(sbResultData.ToString()))
                {
                    sbResult.Append("," + sbResultData.ToString());
                }
                sbResult.Append("}");
            }
            log.Error("判断某记录在表" + strTableName + "中是否存在时返回数据：" + sbResult.ToString());
            return sbResult.ToString();
        }
        
        /// <summary>
        /// 判断某字段在某表中是否存在【从实际数据表中遍历后进行判断进行获取】
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        public static bool JudgeTableColumnIsExists(String strTableName,String strColumnName)
        {
            bool IsExists = false;
            String strSql = "select count(1) from sysobjects a inner join syscolumns b on a.id = b.id where a.name = '" + strTableName + "' and b.name = '" + strColumnName + "' ";
            int iCount = SqlParamDao.ExecuteScalarBySql(strSql);
            if (iCount > 0)
            {
                IsExists = true;
            }
            return IsExists;
        }

        /// <summary>
        /// 判断某字段在某表中是否存在【根据字段命名规则判断】
        /// 如果字段名没有下横杠_则表示是该表的实际字段名
        /// </summary>
        /// <param name="strColumnName"></param>
        /// <returns></returns>
        public static bool JudgeTableColumnIsExists(String strColumnName)
        {
            bool IsExists = false;
            String[] strArray = strColumnName.Split('_');
            if (strArray.Length > 1)
            {
                IsExists = false;
            }else
            {
                IsExists = true;
            }
            return IsExists;
        }

        /// <summary>
        /// 判断某表的某字段是否设置为需要加密,并返回数据类型
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <param name="strDataType"></param>
        /// <returns></returns>
        public static bool JudgeColumnIsNeedEncrypted(String strTableName,String strColumnName, ref String strDataType)
        {
            bool IsNeedEncrypted = false;
            strDataType = "";
            String[] strArray = strTableName.Split('_');
            if (strArray.Length ==2)
            {
                String strTID = strArray[0];
                String strGID = strArray[1];
                String strPID = strColumnName;
                String strSql = "select * from TB_HRTMPD WHERE TID = '"+ strTID + "' AND GID = '" + strGID + "' AND PID = '" + strPID +"'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow dr = dt.Rows[0];
                    strDataType = dr["PTYPE"].ToString();
                    String strSave = dr["PSAVE"].ToString();
                    if(strSave.Equals("1"))
                    {
                        IsNeedEncrypted = true;
                    }
                }
            }

            return IsNeedEncrypted;
        }

        /// <summary>
        /// 根据字段及值获取加密字符串SQL语句段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        public static String GetColumnEncryptDataString(String strTableName ,String strColumnName ,String strColumnValue)
        {
            //如果字段需要加密
            string strColumnDataString = "'" + strColumnValue + "'";
            string strColumnDataType = "";
            if (JudgeColumnIsNeedEncrypted(strTableName, strColumnName, ref strColumnDataType))
            {
                switch (strColumnDataType.ToString().ToLower())
                {
                    case "int":
                        strColumnDataString = "[dbo].[Fun_Encrypt_Int]('" + strColumnValue + "')";
                        break;
                    case "numeric":
                        strColumnDataString = "[dbo].[Fun_Encrypt_Decimal]('" + strColumnValue + "') ";
                        break;
                    case "datetime":
                    case "date":
                        strColumnDataString = "[dbo].[Fun_Encrypt_Datetime]('" + strColumnValue + "') ";
                        break;
                    default:
                        strColumnDataString = "[dbo].[Fun_Encrypt_String]('" + strColumnValue + "') ";
                        break;
                }
            }
            return strColumnDataString;
        }

        /// <summary>
        /// 根据字段及值获取解密字符串SQL语句段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strColumnName"></param>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        public static String GetColumnDecodeDataString(String strTableName, String strColumnName, String strColumnValue)
        {
            //如果字段需要加密
            string strColumnDataString = "'" + strColumnValue + "'";
            string strColumnDataType = "";
            if (JudgeColumnIsNeedEncrypted(strTableName, strColumnName, ref strColumnDataType))
            {
                switch (strColumnDataType.ToString().ToLower())
                {
                    case "int":
                        strColumnDataString = "[dbo].[Fun_Decode_Int]('" + strColumnValue + "')";
                        break;
                    case "numeric":
                        strColumnDataString = "[dbo].[Fun_Decode_Decimal]('" + strColumnValue + "') ";
                        break;
                    case "datetime":
                    case "date":
                        strColumnDataString = "[dbo].[Fun_Decode_Datetime]('" + strColumnValue + "') ";
                        break;
                    default:
                        strColumnDataString = "[dbo].[Fun_Decode_String]('" + strColumnValue + "') ";
                        break;
                }
            }
            return strColumnDataString;
        }

    }
}
