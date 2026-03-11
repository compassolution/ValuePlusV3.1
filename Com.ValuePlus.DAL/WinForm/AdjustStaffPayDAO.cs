using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.WinForm
{
    public class AdjustStaffPayDAO
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region 查询薪资库员工列表一条记录获取列名，返回列名DataSet记录集
        /// <summary>
        /// 查询薪资库员工列表一条记录获取列名，返回列名DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllStaffList_Col()
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList_Col");
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 查询薪资库员工列表记录数，返回记录数
        /// <summary>
        /// 查询薪资库员工列表记录数，返回记录数
        /// </summary>
        /// <returns>int</returns>
        public int findAllStaffListCount()
        {
            int iCount = 0;
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList_Count");
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                object obj = dao.ExecuteScalar(CommandType.Text, strSql, null);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
            }
            return iCount;
        }
        #endregion

        #region 查询薪资库员工列表所有记录，返回DataSet记录集
        /// <summary>
        /// 查询薪资库员工列表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllStaffList()
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList") + " order by A.EMPNO";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 分页查询薪资库员工列表所有记录，返回DataSet记录集
        /// <summary>
        /// 分页查询薪资库员工列表所有记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllStaffListMultiPage(int iPageSize, int iStartIndex)
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList_MultiPage").Replace("@PAGESIZE", iPageSize.ToString()).Replace("@STARTINDEX", iStartIndex.ToString()) + "";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <summary>
        /// 根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <param name="strFilterSql">AND 1=1</param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findStaffListByCondition(String strFilterSql)
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList") + strFilterSql+ " order by A.EMPNO";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 分页根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// <summary>
        /// 分页根据条件查询薪资库员工列表相关记录，返回DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAllStaffListMultiPageByCondition(int iPageSize, int iStartIndex, String strFilterSql)
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffList_MultiPage").Replace("@PAGESIZE", iPageSize.ToString()).Replace("@STARTINDEX", iStartIndex.ToString()) + strFilterSql + " order by A.EMPNO";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 查询员工需调整的薪资列表的一条记录，返回列名DataSet记录集
        /// <summary>
        /// 查询员工需调整的薪资列表的一条记录，返回列名DataSet记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findPayItemList_Col()
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffPayValue_Col");
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 根据员工编号查询其对应的需调整的薪资列表，返回DataSet记录集
        /// <summary>
        /// 根据员工编号查询其对应的需调整的薪资列表，返回DataSet记录集
        /// <param name="strYearMonthDcno"></param>
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findPayItemListByStaffNo(String strYearMonthDCNO)
        {
            DataSet ds = new DataSet();
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_GetStaffPayValue").Replace("@yearmonthdcno", "'"+strYearMonthDCNO+"'");
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

            }
            return ds;
        }
        #endregion

        #region 根据员工编号及薪资项目更新该员工新调整值，返回记录数
        /// <summary>
        /// 根据员工编号及薪资项目更新该员工新调整值，返回记录数
        /// <param name="strYearMonthDcno"></param>
        /// <param name="strItemCode"></param>
        /// <param name="fValue"></param>
        /// </summary>
        /// <returns>int</returns>
        public int updatePayValueByStaffNoAItemCode(String strYearMonthDcno,String strItemCode,float fValue)
        {
            int iCount = 0;
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_UpdateStaffPayValue").Replace("@yearmonth", "'" + strYearMonthDcno + "'").Replace("@itemcode", "'" + strItemCode + "'").Replace("@itemevalue", fValue.ToString());
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                iCount = dao.ExecuteNonQuery(CommandType.Text, strSql, null);

            }
            return iCount;
        }
        #endregion

        #region 批量更新表员工薪资变更调整信息（表prempl_2）,返回记录数
        /// <summary>
        /// 批量更新表员工薪资变更调整信息（表prempl_2）,返回记录数
        /// </summary>
        /// <param name="strModifyResults"></param>
        /// <returns>int</returns>
        public int updateStaffPayValueBatch(String strModifyResults)
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
                    String strStaffNoAItemCode = "";
                    String strYearMonthDcno = "";
                    String strItemCode = "";
                    String strModifyItemAValues = "";
                    String strModifyItem = "";
                    String strModifyValue = "";
                    for (int i = 0; i < strArrRecord.Length; i++)
                    {
                        strRecord = strArrRecord[i].ToString();
                        if (!String.IsNullOrEmpty(strRecord))
                        {
                            String[] strArrStaffAItemCode = strRecord.Split('$');//分开记录key(序列号和日期共同组成)和其对应的修改对象组
                            if ((strArrStaffAItemCode != null) && (strArrStaffAItemCode.Length > 1))
                            {
                                strStaffNoAItemCode = strArrStaffAItemCode[0].ToString();//员工编号和薪资项编码的拼写字符串，中间用#分开
                                if (!String.IsNullOrEmpty(strStaffNoAItemCode))
                                {
                                    String[] strArrEAY = strStaffNoAItemCode.Split('#');
                                    if ((strArrEAY != null) && (strArrEAY.Length > 1))
                                    {
                                        strYearMonthDcno = strArrEAY[0].ToString();//员工编号
                                        strItemCode = strArrEAY[1].ToString();//项目编码
                                    }
                                }
                                strModifyItemAValues = strArrStaffAItemCode[1].ToString();//每条记录中的修改对象组，各个对象组间用^隔开

                            }
                        }

                        if ((!String.IsNullOrEmpty(strYearMonthDcno)) && (!String.IsNullOrEmpty(strItemCode)) && (!String.IsNullOrEmpty(strModifyItemAValues)))
                        {
                            String strUpdateSql = "update prempl_2 set ";

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

                                            ////根据ITEM的值拼写出param的需要更新的参数
                                            if ((strModifyValue == null) || (strModifyValue.ToLower().Equals("null")))
                                            {
                                                strModifyValue = "";
                                            }

                                            strUpdateSql = strUpdateSql + strModifyItem + " =@" + strModifyItem + ",";
                                            dao.AddParameter("@" + strModifyItem, strModifyValue, TypeDao.VarChar, 300);
                                        }
                                    }
                                }
                            }

                            dao.AddParameter("@yearmonthdcno", strYearMonthDcno, TypeDao.VarChar, 20);
                            dao.AddParameter("@itemcode", strItemCode, TypeDao.VarChar, 20);
                            DbParameter[] param = dao.GetParameters();
                            strUpdateSql = strUpdateSql + " yearmonthdcno =@yearmonthdcno WHERE yearmonthdcno =@yearmonthdcno AND itemcode =@itemcode";

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
