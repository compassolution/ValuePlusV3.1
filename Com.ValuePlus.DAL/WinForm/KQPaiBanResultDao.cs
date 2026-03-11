using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.WinForm
{
    public class KQPaiBanResultDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region 查询【KQRSSZ_1】和【KQRSSZ_2】的一条记录，获取gird列名，返回DataSet记录集
        /// <summary>
        /// 查询【KQRSSZ_1】和【KQRSSZ_2】的一条记录，获取gird列名，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findResultGridCol()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_getCol(), null);
            }
            return ds;
        }
        #endregion

        #region 对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// 对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <param name="strEmNo"></param>
        /// <param name="strDate"></param>
        /// <param name="dtDay"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findResultByEmnoADay(String strEmNo, String strDate, DateTime dtDay)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDate());
                param[0].Value = strEmNo;
                param[1].Value = strDate;
                param[2].Value = dtDay;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDate(), param);
            }
            return ds;
        }
        #endregion


        #region 根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// 根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <param name="strDate"></param>
        /// <param name="strUserId"></param>
        /// <param name="strFlagToNormal"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findResultByEmnoADayAUserId(String strDate, String strUserId, String strFlagToNormal)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDateAUserId());
                param[0].Value = strDate;
                param[1].Value = strUserId;
                param[2].Value = strFlagToNormal;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDateAUserId(), param);
            }
            return ds;
        }
        #endregion

        #region (获取总数)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回记录数
        /// <summary>
        /// (获取总数)根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <param name="strDate"></param>
        /// <param name="strUserId"></param>
        /// <param name="strFlagToNormal"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public int findCountByEmnoADayAUserId(String strDate, String strUserId, String strFlagToNormal)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_selectCount_byNoDateAUserId());
                param[0].Value = strDate;
                param[1].Value = strUserId;
                param[2].Value = strFlagToNormal;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_selectCount_byNoDateAUserId(), param);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
            }
            return iCount;
        }
        #endregion

        #region （分页获取）根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <summary>
        /// （分页获取）根据用户ID对数据表【KQRSSZ_1】和【KQRSSZ_2】的通过员工编号，月份及日期select操作，返回DataSet记录集
        /// <param name="strDate"></param>
        /// <param name="strUserId"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iStartIndex"></param>
        /// <param name="strBisToNormal"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findResultByEmnoADayAUserId(String strDate, String strUserId,int iPageSize,int iStartIndex,String strBisToNormal)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDateAUserId_MultiPage());
                param[0].Value = strDate;
                param[1].Value = strUserId;
                param[2].Value = iPageSize;
                param[3].Value = iStartIndex;
                param[4].Value = strBisToNormal;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQRSSZ_1A2_select_byNoDateAUserId_MultiPage(), param);
            }
            return ds;
        }
        #endregion

        #region 通过员工编号，月份及日期更新对数据表【KQRSSZ_2】，返回DataSet记录数
        /// <summary>
        /// 通过员工编号，月份及日期更新对数据表【KQRSSZ_2】，返回DataSet记录数
        /// <param name="strJbhour"></param>
        /// <param name="strTxhour"></param>
        /// <param name="strRemark"></param>
        /// <param name="strIsToNormal"></param>
        /// <param name="strIsAllow"></param>
        /// <param name="strEmNo"></param>
        /// <param name="dtDay"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateResultByEmnoADay(String strJbhour, String strTxhour, String strRemark,String strIsToNormal,String strIsAllow,String strEmNo,DateTime dtDay)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                String strUpdateSql = "UPDATE KQRSSZ_2 SET ";
                if ((strJbhour != null) && (!strJbhour.Equals("undefined")) && (!strJbhour.Equals("null")))
                {
                    strUpdateSql = strUpdateSql + "SJBHOUR = "+ strJbhour+",";
                }
                if ((strTxhour != null) && (!strTxhour.Equals("undefined")) && (!strTxhour.Equals("null")))
                {
                    strUpdateSql = strUpdateSql + "STXHOUR = " + strTxhour+",";
                }
                if ((strRemark != null) && (!strRemark.Equals("undefined")) && (!strRemark.Equals("null")))
                {
                    strUpdateSql = strUpdateSql + "SREMARK = '" + strRemark+"',";
                }
                if ((strIsToNormal != null) && (!strIsToNormal.Equals("undefined")) && (!strIsToNormal.Equals("null")))
                {
                    strUpdateSql = strUpdateSql + "BISTONORMAL = '" + strIsToNormal+"',";
                }
                strUpdateSql = strUpdateSql.TrimEnd(',') + " WHERE EM_NO ='"+strEmNo+"' AND r_Date =cast('"+dtDay+"' as datetime)";

                int iReturn = dao.ExecuteNonQuery(CommandType.Text, strUpdateSql, null);
                if (iReturn > 0) { iCount++; }
                //throw new Exception();

                //"UPDATE KQRSSZ_2 set SJBHOUR = @SJBHOUR ,STXHOUR=@STXHOUR,SREMARK=@SREMARK,BISTONORMAL=@BISTONORMAL,BISALLOW=@BISALLOW WHERE EM_NO = @EM_NO AND R_DATE = @R_DATE"

                //DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQRSSZ_2_update_byNoDate());
                //if ((strJbhour!=null) && (!strJbhour.Equals("undefined")) && (!strJbhour.Equals("null")))
                //{
                //    param[0].Value = strJbhour;
                //}
                //if ((strTxhour != null) && (!strTxhour.Equals("undefined")) && (!strTxhour.Equals("null")))
                //{
                //    param[1].Value = strTxhour;
                //}
                //if ((strRemark != null) && (!strRemark.Equals("undefined")) && (!strRemark.Equals("null")))
                //{
                //    param[2].Value = strRemark;
                //}
                //if ((strIsToNormal != null) && (!strIsToNormal.Equals("undefined")) && (!strIsToNormal.Equals("null")))
                //{
                //    param[3].Value = strIsToNormal;
                //}
                //param[4].Value = strIsAllow;
                //param[5].Value = strEmNo;
                //param[6].Value = dtDay;
                //count = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_KQRSSZ_2_update_byNoDate(), param);
            }
            return iCount;
        }
        #endregion

        #region 根据主键批量更新表KQRSSZ_2的记录,返回记录数
        /// <summary>
        /// 根据主键批量更新表KQRSSZ_2的记录,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns>DataTable</returns>
        public int updateBatchKQRSSZ2DataByKey(String strModifyResults)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();

                log.Error("@strModifyResults:" + strModifyResults);
                if (strModifyResults != null)
                {
                    String[] strArrRecord = strModifyResults.Split('!');//记录数数组
                    String strRecord = "";
                    String strSeqnoADate = "";
                    String strModifyItemAValues = "";
                    String strModifyItem = "";
                    String strModifyValue = "";
                    for (int i = 0; i < strArrRecord.Length; i++)
                    {
                        String strSeqno = "";
                        String strDate = "";
                        DateTime dtR_Date = new DateTime();

                        strRecord = strArrRecord[i].ToString();
                        if (!String.IsNullOrEmpty(strRecord))
                        {
                            String[] strArrSeqnoADate = strRecord.Split('$');//分开记录key(序列号和日期共同组成)和其对应的修改对象组
                            if ((strArrSeqnoADate != null) && (strArrSeqnoADate.Length > 1))
                            {
                                strSeqnoADate = strArrSeqnoADate[0].ToString();//序列号和日期的拼写字符串，中间用#分开
                                if (!String.IsNullOrEmpty(strSeqnoADate))
                                {
                                    String[] strArrEAY = strSeqnoADate.Split('#');
                                    if ((strArrEAY != null) && (strArrEAY.Length > 1))
                                    {
                                        strSeqno = strArrEAY[0].ToString();//序列号
                                        strDate = strArrEAY[1].ToString();//日期YY-MM-D
                                        dtR_Date = DateTime.Parse(strDate);
                                    }
                                }
                                strModifyItemAValues = strArrSeqnoADate[1].ToString();//每条记录中的修改对象组，各个对象组间用^隔开

                            }
                        }

                        //log.Error("@strRecord:" + strRecord);
                        //log.Error("@strDate:" + strDate);
                        //log.Error("@strSeqno:" + strSeqno);


                        if ((!String.IsNullOrEmpty(strSeqno)) && (dtR_Date!=null))
                        {
                            String strUpdateSql = "UPDATE KQRSSZ_2 SET ";

                            //DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQTOE_2_update_byKey());
                            //param[0].Value = strSeqno;
                            //param[1].Value = strDate;
                            if (!String.IsNullOrEmpty(strModifyItemAValues))
                            {
                                String[] strArrModifyItemAValue = strModifyItemAValues.Split('^');//每条记录中的修改对象组，各个对象组间用^隔开
                                if ((strArrModifyItemAValue != null) && (strArrModifyItemAValue.Length > 0))
                                {
                                    for (int j = 0; j < strArrModifyItemAValue.Length; j++)
                                    {
                                        String[] strArrTemp = strArrModifyItemAValue[j].Split('*');//每个对象组中的item和value，中间用*隔开
                                        if ((strArrTemp != null) && (strArrTemp.Length > 1))
                                        {
                                            strModifyItem = strArrTemp[0].ToString();
                                            if (strArrTemp[1].ToString().Equals("space"))
                                            {
                                                strModifyValue = "";
                                            }
                                            else
                                            {
                                                strModifyValue = strArrTemp[1].ToString();
                                            }

                                            ////根据ITEM的值拼写出param的需要更新的参数，strModifyItem的值为D1,D2,D3,D4......D32。
                                            //int iParamIndex = int.Parse(strModifyItem.Substring(1, strModifyItem.Length - 1));
                                            if ((strModifyValue == null) || (strModifyValue.ToLower().Equals("null")))
                                            {
                                                strModifyValue = "";
                                            }
                                            //param[iParamIndex+1].Value = strModifyValue;

                                            strUpdateSql = strUpdateSql + strModifyItem + " =@" + strModifyItem + ",";
                                            dao.AddParameter("@" + strModifyItem, strModifyValue, TypeDao.VarChar, 300);
                                        }
                                    }
                                }
                            }

                            dao.AddParameter("@SEQNO", strSeqno, TypeDao.VarChar, 20);
                            dao.AddParameter("@strDate", strDate, TypeDao.VarChar, 20);

                            DbParameter[] param = dao.GetParameters();
                            strUpdateSql = strUpdateSql + " SEQNO =@SEQNO  WHERE SEQNO =@SEQNO AND r_Date =cast(@strDate as datetime)";

                            int iReturn = dao.ExecuteNonQuery(CommandType.Text, strUpdateSql, param);
                            continue;
                            //if (iReturn > 0) { iCount++; continue; }
                            //else { throw new Exception(); }
                        }
                    }
                }

                //事务提交
                dao.Commit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iCount = 0;
                dao.RoolBack();

            }
            finally
            {
                dao.Dispose();
            }
            return iCount;
        }
        #endregion

    }
}
