using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.DataLog.BLL;
using System.Text;
using System.Collections;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog.Enum;
using System.Globalization;

namespace Com.ValuePlus.DataLog.DAL
{
    /// <summary>
    /// 数据库操作日志入库类
    /// </summary>
    public class DataLogDAL
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAtype"></param>
        /// <param name="strAref"></param>
        /// <param name="strAction"></param>
        /// <returns></returns>
        public static int WriteDataLog(string strUserCode, string strAtype, string strAref, string strAction)
        {
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            string strSql = "INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,REFER,ACTION) VALUES('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + strAtype + "','" + strAref + "','" + strAction + "')";

            try
            {
                return SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog写日志失败，Sql：" + strSql);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAtype"></param>
        /// <param name="strAtyped"></param>
        /// <param name="strAref"></param>
        /// <param name="strAction"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static int WriteDataLog(string strUserCode, string strAtype, string strAtyped, string strAref, string strAction, string strKey)
        {
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            string strSql = "INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,TYPED,REFER,ACTION,AKEY) VALUES('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + strAtype + "','" + strAtyped + "','" + strAref + "','" + strAction + "','" + strKey + "')";
                 
            try
            {
                return SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog写日志失败，Sql：" + strSql);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAtype"></param>
        /// <param name="strAtyped"></param>
        /// <param name="strAref"></param>
        /// <param name="strAction"></param>
        /// <param name="strKey"></param>
        /// <param name="arrayObjectHRLOG2"></param>
        /// <returns></returns>
        public static int WriteDataLog(string strUserCode, string strAtype, string strAtyped, string strAref, string strAction, string strKey, ArrayList arrayObjectHRLOG2)
        {
            StringBuilder sbSql = new StringBuilder();
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            string strSql = "INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,TYPED,REFER,ACTION,AKEY) VALUES('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + strAtype + "','" + strAtyped + "','" + strAref + "','" + strAction + "','" + strKey + "')";
            sbSql.Append(strSql + ";\r\n");
                
            try
            {
                if ((arrayObjectHRLOG2 != null) && (arrayObjectHRLOG2.Count > 0))
                {
                    for (int i = 0; i < arrayObjectHRLOG2.Count; i++)
                    {
                        Entity_HRLOG_2 entityLog2 = (Entity_HRLOG_2)arrayObjectHRLOG2[i];
                        String strSql2 = "INSERT INTO HRLOG_2(CID,LNO,TID,GID,PID,POVAL,PNVAL) VALUES('" + strNewLogNo + "','" + (i+1).ToString() + "','" + entityLog2.TID + "','" + entityLog2.GID + "','" + entityLog2.PID + "','" + entityLog2.POVAL + "','" + entityLog2.PNVAL + "')";

                        sbSql.Append(strSql2 + ";\r\n");
                    }
                }
                return SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog写日志失败，Sql：" + sbSql.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAtype"></param>
        /// <param name="strAtyped"></param>
        /// <param name="strAref"></param>
        /// <param name="strAction"></param>
        /// <param name="strKey"></param>
        /// <param name="Entity_HRLOG_2"></param>
        /// <returns></returns>
        public static int WriteDataLog(string strUserCode, string strAtype, string strAtyped, string strAref, string strAction, string strKey, Entity_HRLOG_2 entityLog2)
        {
            StringBuilder sbSql = new StringBuilder();
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            string strSql = "INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,TYPED,REFER,ACTION,AKEY) VALUES('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + strAtype + "','" + strAtyped + "','" + strAref + "','" + strAction + "','" + strKey + "')";
            sbSql.Append(strSql + ";\r\n");

            try
            {
                if (entityLog2 != null)
                {
                    String strSql2 = "INSERT INTO HRLOG_2(CID,LNO,TID,GID,PID,POVAL,PNVAL) VALUES('" + strNewLogNo + "','" + "0" + "','" + entityLog2.TID + "','" + entityLog2.GID + "','" + entityLog2.PID + "','" + entityLog2.POVAL + "','" + entityLog2.PNVAL + "')";

                    sbSql.Append(strSql2 + ";\r\n");
                }
                return SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog写日志失败，Sql：" + sbSql.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 动作执行日志写入
        /// add by sammen 20250304
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAref"></param>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public static int WriteDataLog_Execute(string strUserCode, string strAref,String strTID,String strSID,String strAID,String strKeyValue, String strSpName, Hashtable hsTableParam, bool IsSuccess)
        {
            StringBuilder sbSql = new StringBuilder();
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            //获取动作名称
            String strActionName = strSpName;
            String strSql_GetActionName = "select * from TB_HRTMPSA WHERE TID = '"+ strTID + "' AND SID = '"+ strSID + "' AND AID = '" + strAID + "'";
            DataTable dt_GetActionName = SqlParamDao.GetDataTableBySql(strSql_GetActionName);
            if(dt_GetActionName!=null && dt_GetActionName.Rows.Count>0){
                strActionName = dt_GetActionName.Rows[0]["ADESCCHS"].ToString();
            }
            sbSql.Append("INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,TYPED,REFER,ACTION,AKEY,DETAIL) VALUES");
            sbSql.Append("('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + LogActionType.ActionExecute + "','" + strActionName + "'");
            sbSql.Append(",'" + strAref + "','" + (IsSuccess?LogActionType.ActionExecute_Success:LogActionType.ActionExecute_Failed) + "'");
            sbSql.Append(",'" + strTID + ":" + strKeyValue + ":"+ strActionName + ":"+ strSpName + "'");
            sbSql.Append(",'" + HashTableToString(hsTableParam).Replace("'","''") + "')");
            sbSql.Append(";\r\n");

            try
            {
                return SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog_Execute写日志失败，Sql：" + sbSql.ToString());
                return -1;
            }
        }

        /// <summary>
        /// 报表查询日志写入 
        /// add by sammen 20250304
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strAref"></param>
        /// <param name="strQueryName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns></returns>
        public static int WriteDataLog_QueryReport(string strUserCode, string strAref, String strQueryName, Hashtable hsTableParam)
        {
            StringBuilder sbSql = new StringBuilder();
            String strNewLogNo = AutoNoGetter.GetServerAutoFiledNo("LOG");
            sbSql.Append("INSERT INTO HRLOG_1(CID,USERCODE,LOGTIME,TYPE,TYPED,REFER,ACTION,DETAIL) VALUES");
            sbSql.Append("('" + strNewLogNo + "','" + strUserCode + "',getdate(),'" + LogActionType.ReportView + "','" + strQueryName + "'");
            sbSql.Append(",'" + strAref + "','" + LogActionType.ReportView + "'");
            sbSql.Append(",'" + HashTableToString(hsTableParam).Replace("'", "''") + "')");
            sbSql.Append(";\r\n");

            try
            {
                return SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WriteDataLog_Execute写日志失败，Sql：" + sbSql.ToString());
                return -1;
            }
        }


        /// <summary>
        /// hashtable转字符串
        /// </summary>
        /// <param name="hsTableParam"></param>
        /// <returns></returns>
        public static String HashTableToString(Hashtable hsTableParam)
        {
            try
            {
                if (hsTableParam != null)
                {
                    string result = "{";
                    foreach (DictionaryEntry entry in hsTableParam)
                    {
                        result += string.Format("\"{0}\": \"{1}\", ", entry.Key, entry.Value);
                    }
                    if (result.EndsWith(", ")) // 移除最后一个逗号和空格
                    {
                        result = result.Substring(0, result.Length - 2);
                    }
                    result += "}";

                    return result;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
