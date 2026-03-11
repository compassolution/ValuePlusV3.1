using Com.ValuePlus.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class LWorkOrder
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 自动生成工单编号
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        public static string GenerateWONO(string strCompanyCode)
        {
            string strPreFix = "W";
            string strDateTime = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            Random rad = new Random();//实例化随机数产生器rad；
            //2位随机数
            String strRandomCode = rad.Next(10, 100).ToString();
            //string strNewWONO = strPreFix + strCompanyCode + strDateTime;
            string strNewWONO = strPreFix + strDateTime + strRandomCode;
            return strNewWONO;
        }

        /// <summary>
        /// 通过特定条件获取工单信息数据表集合【获取全部，不分页】
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strCondition"></param>
        /// <param name="iTopRows">为0时则获取全部</param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderInfoDataTable(String strUserCode,String strCondition,int iTopRows, String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {

                if (iTopRows == 0)
                {
                    sbSql.Append("select * from [VW_Labor_LWorkOrderMain] A where 1=1");
                }
                else
                {
                    sbSql.Append("select top " + iTopRows.ToString() + " * from [VW_Labor_LWorkOrderMain] A where 1=1");
                }
                sbSql.Append(" and A.WONO IN (SELECT WONO FROM [dbo].[Fun_Labor_GetWorkOrderList_ByUserCode]('" + strUserCode + "'))");
                if (!String.IsNullOrEmpty(strCondition))
                {
                    sbSql.Append(strCondition);
                }
                //通过用人单位编码、工单号、用户类型判断该工单是否可显示可见
                sbSql.Append(" and [dbo].[Fun_Labor_JudgeWorkOrderIsVisible](EmployCompany,WONO,'"+ strUserCode + "') = '1'");

                strOrderBy = String.IsNullOrEmpty(strOrderBy) ? " WONO DESC" : strOrderBy;
                sbSql.Append(" order by " + strOrderBy);

                String strSql = sbSql.ToString();

                log.Error("Labor 通过特定条件获取工单信息数据表集合【获取全部，不分页】，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过特定条件获取工单信息数据表集合【获取全部，不分页】出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过特定条件获取工单信息数据表集合【分页获取】
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strCondition"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize">分页查询时必须大于0</param>
        /// <param name="strOrderBy">分页查询时必须有值</param>
        /// <param name="hsTableRefValues"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderInfoDataTable(String strUserCode,String strCondition, int iPageIndex, int iPageSize, String strOrderBy,ref Hashtable hsTableRefValues)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            StringBuilder sbSql_AllCount = new StringBuilder();
            try
            {
                
                sbSql.Append("select *  from [VW_Labor_LWorkOrderMain] A where 1=1 ");
                sbSql.Append(" and A.WONO IN (SELECT WONO FROM [dbo].[Fun_Labor_GetWorkOrderList_ByUserCode]('" + strUserCode + "'))");
                if (!String.IsNullOrEmpty(strCondition))
                {
                    sbSql.Append(strCondition);
                }
                //通过用人单位编码、工单号、用户类型判断该工单是否可显示可见
                sbSql.Append(" and [dbo].[Fun_Labor_JudgeWorkOrderIsVisible](EmployCompany,WONO,'" + strUserCode + "') = '1'");
                //求总数的语句
                sbSql_AllCount.Append(sbSql.ToString());

                strOrderBy = String.IsNullOrEmpty(strOrderBy) ? " WONO DESC" : strOrderBy;
                sbSql.Append(" order by " + strOrderBy);

                //如果存入了大于0的每页数量值，则说明是要进行分页查询
                if (iPageSize > 0)
                {
                    sbSql.Append(" offset ("+ iPageIndex .ToString()+ " * " + iPageSize.ToString() + ") row ");
                    sbSql.Append(" fetch next " + iPageSize.ToString() + " row only");
                }

                String strSql = sbSql.ToString();

                log.Error("Labor 通过特定条件获取工单信息数据表集合【分页获取】，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
                //求总数
                String strSql_AllCount = "select count(1) from (" + sbSql_AllCount.ToString() + ") A";
                log.Error("Labor 通过特定条件获取工单信息数据表集合【分页获取】获取总数的，strSql_AllCount:" + strSql_AllCount);
                int iTotalCount = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);

                //计划用工人数汇总
                strSql_AllCount = "select isnull(sum(EmployLaborCount),0) from (" + sbSql_AllCount.ToString() + ") A";
                int SummaryEmployLaborCount = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);
                //实际到岗人数汇总
                strSql_AllCount = "select isnull(sum(ActualLaborCount),0) from (" + sbSql_AllCount.ToString() + ") A";
                int SummaryActualLaborCount = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);
                //总房间数汇总
                strSql_AllCount = "select isnull(sum(TotalRoomsCount),0) from (" + sbSql_AllCount.ToString() + ") A";
                int SummaryTotalRoomsCount = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);
                //用工总小时数汇总
                strSql_AllCount = "select convert(decimal(18,0),isnull(sum(TotalHours),0.00)) from (" + sbSql_AllCount.ToString() + ") A";
                Decimal SummaryTotalHours = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);
                //额外补贴合计汇总
                strSql_AllCount = "select convert(decimal(18,2),isnull(sum(TotalSubsidy),0.00)) from (" + sbSql_AllCount.ToString() + ") A";
                Decimal SummaryTotalSubsidy = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);
                //总费用汇总
                strSql_AllCount = "select convert(decimal(18,2),isnull(sum(TotalCost),0.00)) from (" + sbSql_AllCount.ToString() + ") A";
                Decimal SummaryTotalCost = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);

                //返回的值对象
                hsTableRefValues["iTotalRowCount"] = iTotalCount;

                hsTableRefValues["SummaryEmployLaborCount"] = SummaryEmployLaborCount;//计划用工人数汇总
                hsTableRefValues["SummaryActualLaborCount"] = SummaryActualLaborCount;//实际到岗人数汇总
                hsTableRefValues["SummaryTotalHours"] = SummaryTotalHours;//用工总小时数汇总
                hsTableRefValues["SummaryTotalRoomsCount"] = SummaryTotalRoomsCount;//总房间数汇总
                hsTableRefValues["SummaryTotalSubsidy"] = SummaryTotalSubsidy;//额外补贴合计汇总
                hsTableRefValues["SummaryTotalCost"] = SummaryTotalCost;//总费用汇总

            }
            catch (Exception ex)
            {
                log.Error("Labor 通过特定条件获取工单信息数据表集合【分页获取】出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 获取工单信息数据表包括对应可执行动作集合,返回json数据对象
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="dtWorkOrder"></param>
        /// <param name="isEscape"></param>
        /// <returns></returns>
        public static String GetWorkOrdersAndActionsJsonData(String strUserCode, DataTable dtWorkOrder, bool isLoadActionList, bool isEscape)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                int iColCount = dtWorkOrder.Columns.Count;
                //sBuilder.Append("\"" + strNodeName + "\":[ ");
                sBuilder.Append("\"ResultData\"" + ":[ ");
                if ((dtWorkOrder != null) && (dtWorkOrder.Rows.Count > 0))
                {
                    for (int i = 0; i < dtWorkOrder.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }

                        String strCurEmployerCode = "";
                        String strWONO = "";
                        for (int j = 0; j < dtWorkOrder.Columns.Count; j++)
                        {
                            String strColName = dtWorkOrder.Columns[j].ColumnName;
                            String strColType = dtWorkOrder.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dtWorkOrder.Rows[i][dtWorkOrder.Columns[j].ColumnName].ToString();
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

                            //工单对应的可进行操作的动作信息
                            if (strColName.ToLower().Equals("wono"))
                            {
                                strWONO = strColValue;
                            }
                            if (strColName.ToLower().Equals("employcompany"))
                            {
                                strCurEmployerCode = strColValue;
                            }
                        }
                        if (isLoadActionList &&(!String.IsNullOrEmpty(strCurEmployerCode)) && (!String.IsNullOrEmpty(strWONO)))
                        {
                            DataTable dtAction = GetWorkOrderActionsDataTable(strWONO, strUserCode);
                            sBuilder.Append("," + CommonJson.GetJsonStringByDataTable(dtAction, "\"OrderAction_List\"", isEscape));
                        }

                        sBuilder.Append("}");
                    }

                }
                sBuilder.Append("]");

                json = sBuilder.ToString();
                log.Error("获取工单信息数据表包括对应可执行动作集合,返回json数据对象:" + json);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }
        
        /// <summary>
        /// 通过用户获取工单状态Tab集合,返回json数据对象
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="isEscape"></param>
        /// <returns></returns>
        public static String GetWorkOrderStatusTabsJsonData(String strUserCode, bool isEscape)
        {
            StringBuilder sBuilder = new StringBuilder();
            string json = "";
            try
            {
                String strSql = "select * from [dbo].[Fun_Labor_GetTabs_WorkOrderList]('" + strUserCode + "')";
                DataTable dtStatusTabs = SqlParamDao.GetDataTableBySql(strSql);
                int iColCount = dtStatusTabs.Columns.Count;
                //sBuilder.Append("\"" + strNodeName + "\":[ ");
                sBuilder.Append("\"ResultData\"" + ":[ ");
                if ((dtStatusTabs != null) && (dtStatusTabs.Rows.Count > 0))
                {
                    for (int i = 0; i < dtStatusTabs.Rows.Count; i++)
                    {
                        if (i > 0)
                        {
                            sBuilder.Append(",{");
                        }
                        else
                        {
                            sBuilder.Append("{");
                        }
                        for (int j = 0; j < dtStatusTabs.Columns.Count; j++)
                        {
                            String strColName = dtStatusTabs.Columns[j].ColumnName;
                            String strColType = dtStatusTabs.Columns[j].DataType.ToString();
                            //对值进行编码处理特殊字符，如引号等
                            String strColValue = dtStatusTabs.Rows[i][dtStatusTabs.Columns[j].ColumnName].ToString();
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

                            if (j > 0)
                            {
                                sBuilder.Append(",");
                            }
                            if (strColName.Equals("TabStatusArrary")|| strColName.ToLower().Equals("tabstatusarrary"))
                            {
                                sBuilder.Append("\"" + strColName + "\":[" + strColValue + "]");
                            }
                            else
                            {
                                sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                            }
                        }

                        sBuilder.Append("}");
                    }

                }
                sBuilder.Append("]");

                json = sBuilder.ToString();
                log.Error("通过用户获取工单状态Tab集合,返回json数据对象:" + json);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return json;
        }
        
        /// <summary>
        /// 通过用人工单号、用户类型获取该工单可进行操作的动作数据表集合
        /// </summary>
        /// <param name="@WONO"></param>
        /// <param name="@UserCode"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderActionsDataTable(String strWONO, String strUserCode)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_GetList_WorkOrderAction";
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("WONO", strWONO);
            hsTableParams.Add("UserCode", strUserCode);
            String strSql = "select * from [_USP_Labor_GetList_WorkOrderAction] where UserCode = " + strUserCode + " and WONO = '"+ strWONO + "'";
            try
            {
                log.Error("Labor 通过用人单位编码、工单号、用户类型获取该工单可进行操作的动作数据表集合，Sql:" + strSql);
                int iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过用人单位编码、工单号、用户类型获取该工单可进行操作的动作数据表集合出错，Sql:" + sbSql.ToString());
                log.Error(ex);

            }
            
            return dtReturn;
        }

        /// <summary>
        /// 通过工单号获取该工单的状态配置列表
        /// </summary>
        /// <param name="@EmployerCode"></param>
        /// <param name="@WONO"></param>
        /// <param name="@UserCode"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderStatusConfig(String strWONO, String strUserCode)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select *  from [dbo].[Fun_Labor_GetWorkOrderStatusConfig]('" + strWONO + "','" + strUserCode + "') A where 1=1 ");
                String strSql = sbSql.ToString();

                log.Error("Labor 通过工单号获取该工单的状态配置列表，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过工单号获取该工单的状态配置列表出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过工单号获取工单外包工列表集合
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderLaborsDataTable(String strWONO,String strUserCode, String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select *  from  [VW_Labor_LWorkOrderLabors] A where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "UserCode", strWONO));
                if (!String.IsNullOrEmpty(strUserCode))
                {
                    //sbSql.Append(" and UserCode = '" + strUserCode+"'");
                    sbSql.Append(" and UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode));
                }
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by " + strOrderBy);
                }
                else
                {
                    sbSql.Append(" order by WONO,SelectTime DESC ");
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过工单号获取工单外包工列表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过工单号获取工单外包工列表集合出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过工单号获取时间轴信息集合
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strOrderBy"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderTimelineDataTable(String strWONO,String strOrderBy)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select *  from [LWorkOrder_3] A where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "UserCode", strWONO));
                if (!String.IsNullOrEmpty(strOrderBy))
                {
                    sbSql.Append(" order by "+ strOrderBy);
                }else
                {
                    sbSql.Append(" order by SEQNO DESC ");
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过工单号获取时间轴信息集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过工单号获取时间轴信息集合出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过工单号及评价者、评价对象获取评价信息
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strBeObjectCode"></param>
        /// <param name="strDoObjectCode"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderAppraiseDataTable(String strWONO,String strBeObjectType, String strBeObjectCode, String strDoObjectType, String strDoObjectCode)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select *  from [VW_Labor_LWorkOrderAppraise] A where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "UserCode", strWONO));
                if (!String.IsNullOrEmpty(strBeObjectType))
                {
                    sbSql.Append(" AND BeObjectType = " + JObjectToDB.GetColumnEncryptDataString("LUser_6", "BeObjectType", strBeObjectType));
                }
                if (!String.IsNullOrEmpty(strBeObjectCode))
                {
                    sbSql.Append(" AND BeObjectCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_6", "BeObjectCode", strBeObjectCode));
                }
                if (!String.IsNullOrEmpty(strDoObjectType))
                {
                    sbSql.Append(" AND DoObjectType = " + JObjectToDB.GetColumnEncryptDataString("LUser_6", "DoObjectType", strDoObjectType));
                }
                if (!String.IsNullOrEmpty(strDoObjectCode))
                {
                    sbSql.Append(" AND DoObjectCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_6", "DoObjectCode", strDoObjectCode));
                }
                String strSql = sbSql.ToString();

                log.Error("Labor 通过工单号及评价者、评价对象获取评价信息，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过工单号及评价者、评价对象获取评价信息出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 通过用户获取其涉及工单的年度月度列表
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="strGetType"></param>
        /// <returns></returns>
        public static DataTable GetYearMonthDataTable(String strUserCode, String strGetType)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select *  from  dbo.[Fun_Labor_GetYearMonthList_ByUserCode]('" + strUserCode + "','" + strGetType + "')");
                String strSql = sbSql.ToString();

                log.Error("Labor 通过用户获取其涉及工单的年度月度列表，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 通过用户获取其涉及工单的年度月度列表出错，Sql:" + sbSql.ToString());
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 根据用户编码获取统计数据
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static DataTable GetWorkOrderSummaryData(String strUserCode, Hashtable hsTableParams)
        {
            DataTable dtReturn = new DataTable();
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_Data_GetSummaryData";
            String strSql = "select * from [_USP_Labor_Data_GetSummaryData] where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strUserCode);
            try
            {
                int iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("Labor 根据用户编码获取统计数据出错" + ex);
                log.Error("根据用户编码获取统计数据失败:strSql:" + strSql);
                log.Error(ex);

            }
            return dtReturn;
        }

        /// <summary>
        /// 删除工单信息
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <param name="strDeptCode"></param>
        /// <param name="strSetIsValid"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public static int DeleteWorkOrderInfo(String strUserCode, String strWONO, String strSetIsValid)
        {
            String strCurDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strSetIsValid))
            {
                sbSql.Append("update LWorkOrder_1 set IsValid = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "IsValid", strSetIsValid));
                sbSql.Append("  ,LastModifyUser = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "LastModifyUser", strUserCode));
                sbSql.Append("  ,LastModifyTime = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "LastModifyTime", strCurDataTime));
                sbSql.Append(" where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
            }
            else
            {
                sbSql.Append("delete from LWorkOrder_5 where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
                sbSql.Append("delete from LWorkOrder_4 where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
                sbSql.Append("delete from LWorkOrder_3 where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
                sbSql.Append("delete from LWorkOrder_2 where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
                sbSql.Append("delete from LWorkOrder_1 where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_1", "WONO", strWONO));
            }
            log.Error("删除工单信息:sql:" + sbSql.ToString());
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
            return iCount;

        }

        /// <summary>
        /// 保存工单后的后续逻辑操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoAfterSaveWorkOrder(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_AfterSaveWorkOrder";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }catch(Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 保存工单后的后续逻辑操作出错" + ex);
                log.Error("保存工单后的后续逻辑操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }

        /// <summary>
        /// 设置工单状态
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int SetWorkOrderStatus(Hashtable hsTableParams)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_WorkOrder_SetStatus";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
                if (iReturn == 1)
                {
                    //工单状态变化时发送订阅消息
                    String strWONO = hsTableParams["WONO"].ToString();
                    String strStatusTo = hsTableParams["StatusTo"].ToString();
                    SendWXMsg.SendWOPendingMsg(strWONO, strStatusTo,"1");
                }
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 设置工单状态失败出错" + ex);
                log.Error("设置工单状态失败:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());

            }
            return iReturn;

        }

        /// <summary>
        /// 工单中选择或者取消选择外包工
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int SelectWorkOrderLabors(Hashtable hsTableParams)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_WorkOrderSelectLabors";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
                if (iReturn == 1)
                {
                    //发送订阅消息
                    String strWONO = hsTableParams["WONO"].ToString();
                    String strStatusTo = "";
                    SendWXMsg.SendWOPendingMsg(strWONO, strStatusTo,"1");
                }
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 工单中选择或者取消选择外包工出错" + ex);
                log.Error("工单中选择或者取消选择外包工失败:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 外包工确认接单
        /// </summary>
        /// <param name="strWONO"></param>
        /// <param name="strLaborUserCode"></param>
        /// <param name="IsToConfirm"></param>
        /// <returns></returns>
        public static int LaborConfirmAcceptWorkOrder(String strWONO,String strLaborUserCode,String IsToConfirm)
        {
            int iReturn = 0;
            String strSql = "";
            try
            {
                String strCurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //String strSql = "UPDATE LWorkOrder_2 SET ConfirmTime = '"+ strCurTime + "' where WONO = '" + strWONO + "' AND UserCode = '" + strLaborUserCode + "' ";

                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("UPDATE LWorkOrder_2 SET ConfirmTime = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_2", "ConfirmTime", strCurTime));
                sbSql.Append(" where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_2", "WONO", strWONO));
                sbSql.Append(" and UserCode = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_2", "UserCode", strLaborUserCode));
                strSql = sbSql.ToString();

                if (!IsToConfirm.Equals("1"))
                {
                    //strSql = "UPDATE LWorkOrder_2 SET ConfirmTime = null where WONO = '" + strWONO + "' AND UserCode = '" + strLaborUserCode + "' ";

                    sbSql = new StringBuilder();
                    sbSql.Append("UPDATE LWorkOrder_2 SET ConfirmTime = null");
                    sbSql.Append(" where WONO = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_2", "WONO", strWONO));
                    sbSql.Append(" and UserCode = " + JObjectToDB.GetColumnEncryptDataString("LWorkOrder_2", "UserCode", strLaborUserCode));
                    strSql = sbSql.ToString();
                }
                iReturn = SqlParamDao.ExecuteNonQueryBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 外包工确认接单出错" + ex);
                log.Error("外包工确认接单确认失败:SQL:" + strSql);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 根据用户编码获取待办工单数量
        /// </summary>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        public static int GetWorkOrderPendingQty(String strUserCode)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                String strSql = "select [dbo].[Fun_Labor_GetWorkOrderPendingQty]('"+ strUserCode + "')";
                iReturn = SqlParamDao.ExecuteScalarBySql(strSql);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 根据用户编码获取待办工单数量出错" + ex);
                log.Error("根据用户编码获取待办工单数量失败:strUserCode:" + strUserCode);
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 保存外包工打卡记录
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int SaveLaborAttRecord(Hashtable hsTableParams)
        {
            int iReturn = 0;
            StringBuilder sbSql = new StringBuilder();
            String strSPName = "USP_Labor_SaveLaborAttRecord";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 保存外包工打卡记录失败出错" + ex);
                log.Error("保存外包工打卡记录失败:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
                log.Error(ex);

            }
            return iReturn;

        }

        /// <summary>
        /// 保存工单评价后的后续逻辑操作
        /// </summary>
        /// <param name="hsTableParams"></param>
        /// <returns></returns>
        public static int DoAfterSaveWorkOrderAppraise(Hashtable hsTableParams)
        {
            int iReturn = 0;
            String strSPName = "USP_Labor_AfterSaveWorkOrderAppraise";
            try
            {
                iReturn = SqlParamDao.ExcuteSP(strSPName, hsTableParams);
            }
            catch (Exception ex)
            {
                iReturn = -1;
                log.Error("Labor 保存工单评价后的后续逻辑操作出错" + ex);
                log.Error("保存工单评价后的后续逻辑操作出错:SPName:" + strSPName + ",Params:" + hsTableParams.ToString());
            }
            return iReturn;
        }


    }
}
