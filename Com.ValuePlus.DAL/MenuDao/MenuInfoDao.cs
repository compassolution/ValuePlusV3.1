using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class MenuInfoDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询栏目表所有记录，返回dateset记录集
        /// <summary>
        /// 查询栏目表所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 查询栏目表所有记录，返回DataTable记录集
        /// <summary>
        /// 查询栏目表所有记录，返回DataTable记录集
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable findAllTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectAll(), null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据主键查询栏目表记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询栏目表记录，返回dateset记录集
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public DataSet findByMenuCode(String strMenuCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode());
                param[0].Value = strMenuCode;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据顺序号获取表TB_HR_MENU记录，返回dateset记录集
        /// <summary>
        /// 根据顺序号获取表TB_HR_MENU记录，返回dateset记录集
        /// </summary>
        /// <param name="strOrder"></param>
        /// <returns>DataSet</returns>
        public DataSet findByOrder(String strOrder)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byOrder());
                param[0].Value = strOrder;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byOrder(), param);
            }
            return ds;
        }
        #endregion

        #region 根据栏目编码查询子目录记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询栏目表记录，返回dateset记录集
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public DataSet findSubLevelByMenuCode(String strMenuCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubLevel_byMenuCode());
                param[0].Value = strMenuCode;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubLevel_byMenuCode(), param);
            }
            return ds;
        }
        #endregion

        #region 根据栏目编码以及用户编码查询该用户具有的子目录记录，返回dateset记录集
        /// <summary>
        /// 根据栏目编码以及用户编码查询该用户具有的子目录记录，返回dateset记录集
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <param name="strUserId"></param>
        /// <returns>DataSet</returns>
        public DataSet findSubLevelByMenuCodeAUserId(String strMenuCode,String strUserId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubLevel_byMenuCodeAUserId());
                param[0].Value = strMenuCode;
                param[1].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubLevel_byMenuCodeAUserId(), param);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询栏目表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询栏目表记录，返回datatable记录
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public DataTable findTableByMenuCode(String strMenuCode)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode());
                param[0].Value = strMenuCode;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <param name="strSMENUCODE"></param>
        /// <param name="strSMENUNAME"></param>
        /// <param name="strSMENUNAMECN"></param>
        /// <param name="strSURLDETAIL"></param>
        /// <param name="strSMENUTYPE"></param>
        /// <param name="strSIMAGE"></param>
        /// <param name="strSPARENTCODE"></param>
        /// <param name="strSLEVEL"></param>
        /// <param name="strSORDER"></param>
        /// <param name="strBISSTOP"></param>
        /// <param name="strSSHOWLOCATION"></param>
        /// <returns></returns>
        public int insertOneRow(String strSMENUCODE, String strSMENUNAME, String strSMENUNAMECN, String strSURLDETAIL, String strSMENUTYPE, String strSIMAGE, String strSPARENTCODE, String strSLEVEL, String strSORDER, String strBISSTOP,String strSSHOWLOCATION)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_insert());
                param[0].Value = strSMENUCODE;
                param[1].Value = strSMENUNAME;
                param[2].Value = strSMENUNAMECN;
                param[3].Value = strSURLDETAIL;
                param[4].Value = strSMENUTYPE;
                param[5].Value = strSIMAGE;
                param[6].Value = strSPARENTCODE;
                param[7].Value = strSLEVEL;
                param[8].Value = strSORDER;
                param[9].Value = strBISSTOP;
                param[10].Value = strSSHOWLOCATION;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据栏目编码删除用户栏目表一条或者多条记录,同时删除用户栏目表相应记录
        /// <summary>
        /// 根据栏目编码删除用户栏目表一条或者多条记录,同时删除用户栏目表相应记录
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public int deleteByMenuCode(String strMenuCode)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();
                //先删除用户栏目表
                DbParameter[] param1 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byMenuCode());
                param1[0].Value = strMenuCode;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byMenuCode(), param1);
                if (obj != null)
                {
                    //再删除栏目表
                    DbParameter[] param2 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_delete_byMenuCode());
                    param2[0].Value = strMenuCode;
                    int iReturn = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_delete_byMenuCode(), param2);
                    if (iReturn > 0) 
                    { 
                        iCount++; 
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                //事务提交
                dao.Commit();
            }
            catch (Exception ex)
            {
                dao.RoolBack();
                log.Error(ex);
                iCount = 0;
            }
            finally
            {
                dao.Dispose();
            }
            return iCount;
        }
        #endregion

        #region 根据主键更新栏目表记录，返回更新记录数
        /// <summary>
        /// 根据主键更新栏目表记录，返回更新记录数
        /// </summary>
        /// <param name="strSMENUCODE"></param>
        /// <param name="strSMENUNAME"></param>
        /// <param name="strSMENUNAMECN"></param>
        /// <param name="strSURLDETAIL"></param>
        /// <param name="strSMENUTYPE"></param>
        /// <param name="strSIMAGE"></param>
        /// <param name="strSPARENTCODE"></param>
        /// <param name="strSLEVEL"></param>
        /// <param name="strSORDER"></param>
        /// <param name="strBISSTOP"></param>
        /// <param name="strSSHOWLOCATION"></param>
        /// <returns>DataSet</returns>
        public int updateByMenuCode(String strSMENUCODE, String strSMENUNAME, String strSMENUNAMECN, String strSURLDETAIL, String strSMENUTYPE, String strSIMAGE, String strSPARENTCODE, String strSLEVEL, String strSORDER, String strBISSTOP,String strSSHOWLOCATION)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();
                //获取原来记录
                DbParameter[] param2 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode());
                param2[0].Value = strSMENUCODE;
                DataSet ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_select_byMenuCode(), param2);
                //如果该栏目存在子栏目且排序序号发生变化，则需要更新其所有子栏目的相应序号
                String strOldOrder = "";
                if ((ds != null) && (ds.Tables.Count > 0))
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        strOldOrder = dt.Rows[0]["SORDER"].ToString();
                    }
                }

                //先更新本条栏目信息
                DbParameter[] param1 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_update_byMenuCode());
                param1[0].Value = strSMENUCODE;
                param1[1].Value = strSMENUNAME;
                param1[2].Value = strSMENUNAMECN;
                param1[3].Value = strSURLDETAIL;
                param1[4].Value = strSMENUTYPE;
                param1[5].Value = strSIMAGE;
                param1[6].Value = strSPARENTCODE;
                param1[7].Value = strSLEVEL;
                param1[8].Value = strSORDER;
                param1[9].Value = strBISSTOP;
                param1[10].Value = strSSHOWLOCATION;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_update_byMenuCode(), param1);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);

                    if (!strOldOrder.Equals(strSORDER))//判断排序序号是否发生变化，如果改变了则更新
                    {
                        int iMenuOrderLength = strSORDER.Length;
                        DbParameter[] param3 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubOrder_byParentOrder());
                        param3[0].Value = iMenuOrderLength;
                        param3[1].Value = strOldOrder;
                        DataSet ds1 = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_selectSubOrder_byParentOrder(), param3);

                        if ((ds1 != null) && (ds1.Tables.Count > 0))
                        {
                            DataTable dt1 = ds1.Tables[0];
                            if (dt1.Rows.Count > 1)//大于一说明其有子栏目
                            {
                                foreach (DataRow row in dt1.Rows)
                                {
                                    String strSubMenuCode = row["SMENUCODE"].ToString();
                                    String strSubMenuOrder = row["SORDER"].ToString();
                                    String strOrderSubStr = strSubMenuOrder.Substring(0,iMenuOrderLength);
                                    //判断父栏目排序序号是否发生变化，如果没有变化则跳出循环，结束
                                    if (strOrderSubStr.Equals(strSORDER))
                                    {
                                        break;
                                    }
                                    else//如有有变化则需要更新所有子目录的排序序号
                                    {
                                        if (!String.IsNullOrEmpty(strOrderSubStr))
                                        {
                                            String strLastOrder = strSubMenuOrder.Substring(strSubMenuOrder.Length - iMenuOrderLength,2);
                                            String strNewOrder = strSORDER + strLastOrder;
                                            DbParameter[] param4 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_updateOrder_byOrder());
                                            param4[0].Value = strNewOrder;
                                            param4[1].Value = strSubMenuOrder;
                                            DataSet ds2 = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_MENU_updateOrder_byOrder(), param4);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                //事务提交
                dao.Commit();
            }
            catch (Exception ex)
            {
                dao.RoolBack();
                log.Error(ex);
                iCount = 0;
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
