using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.ValuePlus.DAL;
using System.Data;


namespace Com.ValuePlus.SysTask
{
    public class SysTaskBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取系统计划任务列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetSysTaskList()
        {
            ArrayList arrList = new ArrayList();

            try
            {
                String strSql = "select * from SYSTASK_1 WHERE BISSTOP = '2' order by STIME";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Entity_SysTask entity = new Entity_SysTask();
                        DataRow dr = dt.Rows[i];
                        entity.TaskCode = dr["TASKCODE"] == null ? "" : dr["TASKCODE"].ToString();
                        entity.TaskName = dr["TASKNAME"] == null ? "" : dr["TASKNAME"].ToString();
                        entity.TaskDesc = dr["TASKDESC"] == null ? "" : dr["TASKDESC"].ToString();
                        entity.ExecType = dr["EXECTYPE"] == null ? "" : dr["EXECTYPE"].ToString();
                        entity.Month = dr["SMONTH"] == null ? "" : dr["SMONTH"].ToString();
                        entity.Day = dr["SDAY"] == null ? "" : dr["SDAY"].ToString();
                        entity.Time = dr["STIME"] == null ? "" : dr["STIME"].ToString();
                        entity.TaskDetail = dr["TASKDETAIL"] == null ? "" : dr["TASKDETAIL"].ToString();
                        entity.IsStop = dr["BISSTOP"] == null ? "" : dr["BISSTOP"].ToString();

                        arrList.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取计划任务列表失败："+ex.Message.ToString());
            }

            return arrList;
        }

        /// <summary>
        /// 获取并执行系统计划任务
        /// </summary>
        /// <returns></returns>
        public static void ExecuteSysTaskList(DateTime dtCurTime)
        {

            try
            {
                String strSql = "select * from SYSTASK_1 WHERE BISSTOP = '2' order by STIME";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Entity_SysTask entity = new Entity_SysTask();
                        DataRow dr = dt.Rows[i];
                        entity.TaskCode = dr["TASKCODE"] == null ? "" : dr["TASKCODE"].ToString();
                        entity.TaskName = dr["TASKNAME"] == null ? "" : dr["TASKNAME"].ToString();
                        entity.TaskDesc = dr["TASKDESC"] == null ? "" : dr["TASKDESC"].ToString();
                        entity.ExecType = dr["EXECTYPE"] == null ? "" : dr["EXECTYPE"].ToString();
                        entity.Month = dr["SMONTH"] == null ? "" : dr["SMONTH"].ToString();
                        entity.Day = dr["SDAY"] == null ? "" : dr["SDAY"].ToString();
                        entity.Time = dr["STIME"] == null ? "" : dr["STIME"].ToString();
                        entity.TaskDetail = dr["TASKDETAIL"] == null ? "" : dr["TASKDETAIL"].ToString();
                        entity.IsStop = dr["BISSTOP"] == null ? "" : dr["BISSTOP"].ToString();

                        ExecuteSysTaskDetail(entity, dtCurTime);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("获取并执行计划任务列表失败：" + ex.Message.ToString());
            }

        }

        /// <summary>
        /// 执行任务内容并写入相应日志
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dtCurTime"></param>
        public static void ExecuteSysTaskDetail(Entity_SysTask entity,DateTime dtCurTime)
        {
            if (entity != null)
            {
                int iYear = dtCurTime.Year;
                int iMonth = dtCurTime.Month;
                int iDay = dtCurTime.Day;
                String strEventTime = dtCurTime.ToString("HH:mm");
                int iHour = dtCurTime.Hour;
                int iMinute = dtCurTime.Minute;
                int iSecond = dtCurTime.Second;
                bool isNeedExecute = false;
                //首先判断是否需要执行
                if (entity.ExecType.Equals("010"))//每年
                {
                    if ((!String.IsNullOrEmpty(entity.Month)) && (int.Parse(entity.Month) == iMonth) && (!String.IsNullOrEmpty(entity.Day)) && (int.Parse(entity.Day) == iDay))
                    {
                        if ((!String.IsNullOrEmpty(entity.Time)) && (entity.Time.Contains(strEventTime)))//时间字段可以支持用分号;分开的多个时间段
                        {
                            isNeedExecute = true;
                        }
                    }
                }else if (entity.ExecType.Equals("020"))//每月
                {
                    if ((!String.IsNullOrEmpty(entity.Day)) && (int.Parse(entity.Day) == iDay))
                    {
                        if ((!String.IsNullOrEmpty(entity.Time)) && (entity.Time.Contains(strEventTime)))//时间字段可以支持用分号;分开的多个时间段
                        {
                            isNeedExecute = true;
                        }
                    }
                }
                else if (entity.ExecType.Equals("030"))//每天
                {
                    if ((!String.IsNullOrEmpty(entity.Time)) && (entity.Time.Contains(strEventTime)))//时间字段可以支持用分号;分开的多个时间段
                    {
                        isNeedExecute = true;
                    }
                }
                else if (entity.ExecType.Equals("040"))//每小时
                {
                    if (iMinute == 0 && iSecond == 0)
                    {
                        isNeedExecute = true;
                    }
                }
                else if (entity.ExecType.Equals("050"))//每分钟
                {
                    if (iSecond == 0)
                    {
                        isNeedExecute = true;
                    }
                }


                if (isNeedExecute)
                {
                    log.Error("开始执行计划任务：[" + entity.TaskCode + "] 执行脚本：" + entity.TaskDetail + "\r\n");
                    StringBuilder sbSqlLog = new StringBuilder();

                    sbSqlLog.Append("INSERT INTO SYSTASK_2 (TASKCODE,DTTIME,DTSTARTTIME,ISSUCCESS,SDESC,DTENDTIME) VALUES ('" + entity.TaskCode + "','" + dtCurTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'");
                    try
                    {
                        int iCount = SqlParamDao.ExecuteNonQueryBySql(entity.TaskDetail);
                        sbSqlLog.Append(",'true','Successfully!','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
                    }
                    catch (Exception ex)
                    {
                        sbSqlLog.Append(",'false','" + ex.Message.Replace("'", "''") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");
                    }

                    sbSqlLog.Append("UPDATE SYSTASK_1 SET DTLASTTIME = '" + dtCurTime.ToString("yyyy-MM-dd HH:mm:ss") + "' where TASKCODE = '" + entity.TaskCode + "';");
                    SqlParamDao.ExecuteNonQueryBySql(sbSqlLog.ToString());
                }

            }
        }
    }
}
