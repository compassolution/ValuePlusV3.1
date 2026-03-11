using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using System.Collections;
using Com.ValuePlus.Utils;

namespace Com.ValuePlus.DAL.WinForm
{
    public class KQPaiBanDAO
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表【KQTOE_1】和【KQTOE_2】一条记录，获取gird列名，返回DataSet记录集
        /// <summary>
        /// 查询表【KQTOE_1】和【KQTOE_2】一条记录，获取gird列名，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findKQTOE1A2GridCol()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQTOE_1A2_select_getCol(), null);
            }
            return ds;
        }
        #endregion

        #region 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <summary>
        /// 通过YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <param name="strYearMonth"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findKQTOE1A2ByYearMonth(String strYearMonth)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQTOE_1A2_select_byYearMonth());
                param[0].Value = strYearMonth;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQTOE_1A2_select_byYearMonth(), param);

            }
            return ds;
        }
        #endregion

        #region 根据用户ID和班次编码以及过滤视图/YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <summary>
        /// 根据用户ID和班次编码以及过滤视图/YEARMONTH查询表【KQTOE_1】和【KQTOE_2】所有记录，返回DataSet记录集
        /// <param name="strYearMonth"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findKQTOE1A2ByYearMonthAUserId(String strYearMonth,String strUserId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQTOE_1A2_select_byYearMonthAndUserId());
                param[0].Value = strYearMonth;
                param[1].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQTOE_1A2_select_byYearMonthAndUserId(), param);

            }
            return ds;
        }
        #endregion

        #region 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// <summary>
        /// 根据主键批量更新表KQTOE_2的记录,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns>DataTable</returns>
        public int updateBatchKQTOE2DataByKey(String strModifyResults)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();

                if (strModifyResults != null)
                {
                    String[] strArrRecord = strModifyResults.Split('!');//记录数数组
                    String strRecord = "";
                    String strEmAYear = "";
                    String strEmployee = "";
                    String strYearMonth = "";
                    String strModifyItemAValues = "";
                    String strModifyItem = "";
                    String strModifyValue = "";
                    for (int i = 0; i < strArrRecord.Length; i++)
                    {
                        strRecord = strArrRecord[i].ToString();
                        if (!String.IsNullOrEmpty(strRecord))
                        {
                            String[] strArrEmAYear = strRecord.Split('$');//分开记录key(员工编号和月份共同组成)和其对应的修改对象组
                            if ((strArrEmAYear != null) && (strArrEmAYear.Length > 1))
                            {
                                strEmAYear = strArrEmAYear[0].ToString();//员工编号和月份的拼写字符串，中间用#分开
                                if (!String.IsNullOrEmpty(strEmAYear))
                                {
                                    String[] strArrEAY = strEmAYear.Split('#');
                                    if ((strArrEAY != null) && (strArrEAY.Length > 1))
                                    {
                                        strEmployee = strArrEAY[0].ToString();//员工编号
                                        strYearMonth = strArrEAY[1].ToString();//月份
                                    }
                                }
                                strModifyItemAValues = strArrEmAYear[1].ToString();//每条记录中的修改对象组，各个对象组间用^隔开
                                
                            }
                        }

                        if ((!String.IsNullOrEmpty(strEmployee)) && (!String.IsNullOrEmpty(strYearMonth)))
                        {
                            String strUpdateSql = "UPDATE KQTOE_2 SET ";

                            //DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQTOE_2_update_byKey());
                            //param[0].Value = strEmployee;
                            //param[1].Value = strYearMonth;
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

                                            //根据ITEM的值拼写出param的需要更新的参数，strModifyItem的值为D1,D2,D3,D4......D32。
                                            int iParamIndex = int.Parse(strModifyItem.Substring(1, strModifyItem.Length-1));
                                            if ((strModifyValue == null) || (strModifyValue.ToLower().Equals("null")))
                                            {
                                                strModifyValue = "";
                                            }
                                            //param[iParamIndex+1].Value = strModifyValue;

                                            strUpdateSql = strUpdateSql + strModifyItem + " =@" + strModifyItem + ",";
                                            dao.AddParameter("@" + strModifyItem, strModifyValue, TypeDao.VarChar,20);
                                        }
                                    }
                                }
                            }

                            dao.AddParameter("@EMPLOYEE", strEmployee, TypeDao.VarChar, 20);
                            dao.AddParameter("@YEARMONTH", strYearMonth, TypeDao.VarChar, 8);
                            DbParameter[] param = dao.GetParameters();
                            strUpdateSql = strUpdateSql + " EMPLOYEE =@EMPLOYEE  WHERE EMPLOYEE =@EMPLOYEE AND YEARMONTH =@YEARMONTH";

                            int iReturn = dao.ExecuteNonQuery(CommandType.Text, strUpdateSql, param);
                            continue;
                            //if (iReturn > 0) { iCount++; continue; }
                            //throw new Exception();
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

        #region 查询考勤班次表【KQSHIF_1】一条记录，获取gird列名，返回DataSet记录集
        /// <summary>
        /// 查询考勤班次表【KQSHIF_1】一条记录，获取gird列名，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findKQShifGridCol()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQSHIF_1_select_getCol(), null);

            }
            return ds;
        }
        #endregion

        #region 查询考勤班次表【KQSHIF_1】所有记录，返回DataSet记录集
        /// <summary>
        /// 查询考勤班次表【KQSHIF_1】所有记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllKQShif()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQSHIF_1_selectAll(), null);

            }
            return ds;
        }
        #endregion

        #region 根据用户ID和班次编码以及过滤视图获取数据表【KQSHIF_1】的部分记录，返回DataSet记录集
        /// <summary>
        /// 根据用户ID和班次编码以及过滤视图获取数据表【KQSHIF_1】的部分记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findKQShifInfoByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_KQSHIF_1_select_ByUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_KQSHIF_1_select_ByUserId(), param);

            }
            return ds;
        }
        #endregion

        #region 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// <summary>
        /// 根据存储过程名称及其参数执行存储过程，返回影响记录数
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <returns>DataSet</returns>
        public int ExcuteSP(String strSpName, Hashtable hsTableParam)
        {
            int iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            //using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            //{
            //    if ((hsTableParam != null) && (hsTableParam.Count>0))
            //    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
            //    {
            //        String strParamName = entity.Key.ToString();
            //        String strParamValue = hsTableParam[strParamName].ToString();
            //        dao.AddParameter("@" + strParamName, strParamValue, TypeDao.VarChar, 100);
            //    }
            //    DbParameter[] param = dao.GetParameters();
            //    iCount = dao.ExecuteNonQuery(CommandType.StoredProcedure, strSpName, param);
            //}
            return iCount;
        }
        #endregion


    }
}
