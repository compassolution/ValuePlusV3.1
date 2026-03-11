using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.DataLog.DAL;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog.Enum;
using System.Data;
using System.Collections;

namespace Com.ValuePlus.DataLog
{
    public class DataLogWriter
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 常规日志写入，适用普通情况(不写明细表HRLOG_2)
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strLogType">日志类型</param>
        /// <param name="strAtyped">模板ID</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <param name="strKey">模板主键值</param>
        /// <returns></returns>
        public static int Log_General(string strUserCode, string strLogType, string strAtyped, string strAref, string strActionType, string strKey)
        {
            return DataLogDAL.WriteDataLog(strUserCode, strLogType, strAtyped, strAref, strActionType, strKey);
        }
        /// <summary>
        /// 常规日志写入，适用普通情况(不写明细表HRLOG_2)
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strLogType">日志类型</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <returns></returns>
        public static int Log_General(string strUserCode, string strLogType, string strAref, string strActionType)
        {
            return DataLogDAL.WriteDataLog(strUserCode, strLogType, strAref, strActionType);
        }

        /// <summary>
        /// 登录日志写入(不写明细表HRLOG_2)
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <returns></returns>
        public static int Log_Login(string strUserCode, string strAref, string strActionType)
        {
            return DataLogDAL.WriteDataLog(strUserCode, LogType.Login, strAref, strActionType);
        }
        /// <summary>
        /// 注销日志写入(不写明细表HRLOG_2)
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <returns></returns>
        public static int Log_LogOut(string strUserCode, string strAref, string strActionType)
        {
            return DataLogDAL.WriteDataLog(strUserCode, LogType.LogOut, strAref, strActionType);
        }
        /// <summary>
        /// 用户管理日志写入(不写明细表HRLOG_2)
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <returns></returns>
        public static int Log_User(string strUserCode, string strActionType, string strAref)
        {
            return DataLogDAL.WriteDataLog(strUserCode, LogType.User, strAref, strActionType);
        }

        /// <summary>
        /// 档案操作日志写入
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strTID">模板ID</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <param name="strKeyValue">模板主键值</param>
        /// <param name="arrayObjectHRLOG2">操作明细列表</param>
        /// <returns></returns>
        public static int Log_Archive(string strUserCode, string strTID,string strAref,String strActionType, string strKeyValue, ArrayList arrayObjectHRLOG2)
        {
            int iCount = 0;
            try
            {
                iCount = DataLogDAL.WriteDataLog(strUserCode, LogType.Archive, strTID, strAref, strActionType, strKeyValue, arrayObjectHRLOG2);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Log_ArchiveModify档案操作日志写入失败");
            }
            return iCount;
        }

        /// <summary>
        /// 档案操作日志写入
        /// </summary>
        /// <param name="strUserCode">用户编号</param>
        /// <param name="strTID">模板ID</param>
        /// <param name="strAref">参考IP</param>
        /// <param name="strActionType">操作类别</param>
        /// <param name="strKeyValue">模板主键值</param>
        /// <param name="Entity_HRLOG_2">操作明细</param>
        /// <returns></returns>
        public static int Log_Archive(string strUserCode, string strTID, string strAref, String strActionType, string strKeyValue, Entity_HRLOG_2 entityLog2)
        {
            int iCount = 0;
            try
            {
                iCount = DataLogDAL.WriteDataLog(strUserCode, LogType.Archive, strTID, strAref, strActionType, strKeyValue, entityLog2);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Log_ArchiveModify档案操作日志写入失败");
            }
            return iCount;
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
        public static int Log_ExecuteAction(string strUserCode, string strAref, String strTID, String strSID, String strAID, String strKeyValue, string strSpName,Hashtable hsTableParam, bool IsSuccess)
        {
            return DataLogDAL.WriteDataLog_Execute(strUserCode, strAref, strTID,strSID, strAID, strKeyValue, strSpName, hsTableParam, IsSuccess);
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
        public static int Log_QueryReport(string strUserCode, string strAref, string strQueryName, Hashtable hsTableParam)
        {
            return DataLogDAL.WriteDataLog_QueryReport(strUserCode, strAref, strQueryName, hsTableParam);
        }

        /// <summary>
        /// 操作日志表对象赋值写入
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strOldValue"></param>
        /// <param name="strNewValue"></param>
        /// <returns>Entity_HRLOG_2</returns>
        public static Entity_HRLOG_2 SetDataToEntity_HRLOG_2(String strTID,String strGID,String strPID,String strOldValue,String strNewValue)
        {
            Entity_HRLOG_2 entity = new Entity_HRLOG_2();
            entity.TID = strTID;
            entity.GID = strGID;
            entity.PID = strPID;
            entity.POVAL = strOldValue;
            entity.PNVAL = strNewValue;
            return entity;
        }


    }
}
