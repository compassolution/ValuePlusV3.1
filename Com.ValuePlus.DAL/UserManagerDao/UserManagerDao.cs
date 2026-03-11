using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class UserManagerDao
    {
        #region 查询系统用户信息表所有记录，返回dateset记录集
        /// <summary>
        /// 查询栏目表所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 查询系统用户信息表所有记录，返回DataTable记录集
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
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_selectAll(), null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据用户ID查询该用户信息,返回dataset记录
        /// <summary>
        /// 根据用户ID查询该用户信息，返回dataset记录
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>DataSet</returns>
        public DataSet findByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byUserId(), param);
                
            }
            return ds;
        }
        #endregion

        #region 根据用户ID查询该用户信息,返回datatable记录
        /// <summary>
        /// 根据用户ID查询该用户信息，返回datatable记录
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>DataTable</returns>
        public DataTable findTableByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据用户登录帐号查询该用户信息,返回DataSet记录
        /// <summary>
        /// 根据用户登录帐号查询该用户信息，返回DataSet记录
        /// </summary>
        /// <param name="strAccountId"></param>
        /// <returns>DataSet</returns>
        public DataSet findByAccountId(String strAccountId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byAccountId());
                param[0].Value = strAccountId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byAccountId(), param);
                
            }
            return ds;
        }
        #endregion

        #region 根据用户登录帐号查询该用户信息,返回datatable记录
        /// <summary>
        /// 根据用户登录帐号查询该用户信息，返回datatable记录
        /// </summary>
        /// <param name="strAccountId"></param>
        /// <returns>DataTable</returns>
        public DataTable findTableByAccountId(String strAccountId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byAccountId());
                param[0].Value = strAccountId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_select_byAccountId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据用户ID删除系统用户信息表一条记录，返回删除记录数
        /// <summary>
        /// 根据用户ID删除菜单定义表一条或者多条记录，返回删除记录数
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>DataSet</returns>
        public int deleteById(String strUserId)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_delete_byUserId());
                param[0].Value = strUserId;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_delete_byUserId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 插入系统用户表一条记录
        /// <summary>
        /// 插入系统用户表一条记录
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strSACCOUNTID"></param>
        /// <param name="strSPWD"></param>
        /// <param name="strSTAFFNO"></param>
        /// <param name="strSUSERNAME"></param>
        /// <param name="strSUSERNAMECN"></param>
        /// <param name="strSDEPT"></param>
        /// <param name="strSDEPTCN"></param>
        /// <param name="strSPOSI"></param>
        /// <param name="strSPOSICN"></param>
        /// <param name="strSTREECLR"></param>
        /// <param name="strSWORKCLR"></param>
        /// <param name="strBISALERT"></param>
        /// <param name="strBISGROUPUSER"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int insertOneRow(String strSUSERID,String strSACCOUNTID,String strSPWD,String strSTAFFNO,String strSUSERNAME,String strSUSERNAMECN,String strSDEPT,String strSDEPTCN,String strSPOSI,String strSPOSICN,String strSTREECLR,String strSWORKCLR,String strBISALERT,String strBISGROUPUSER,String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_insert());
                param[0].Value =  strSUSERID;
                param[1].Value =  strSACCOUNTID;
                param[2].Value =  strSPWD;
                param[3].Value =  strSTAFFNO;
                param[4].Value =  strSUSERNAME;
                param[5].Value =  strSUSERNAMECN;
                param[6].Value =  strSDEPT;
                param[7].Value =  strSDEPTCN;
                param[8].Value =  strSPOSI;
                param[9].Value =  strSPOSICN;
                param[10].Value =  strSTREECLR;
                param[11].Value =  strSWORKCLR;
                param[12].Value =  strBISALERT;
                param[13].Value =  strBISGROUPUSER;
                param[14].Value = strBISSTOP;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion
        
        #region 根据用户ID更新系统用户表一条记录
        /// <summary>
        /// 根据用户ID更新系统用户表一条记录
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strSACCOUNTID"></param>
        /// <param name="strSPWD"></param>
        /// <param name="strSTAFFNO"></param>
        /// <param name="strSUSERNAME"></param>
        /// <param name="strSUSERNAMECN"></param>
        /// <param name="strSDEPT"></param>
        /// <param name="strSDEPTCN"></param>
        /// <param name="strSPOSI"></param>
        /// <param name="strSPOSICN"></param>
        /// <param name="strSTREECLR"></param>
        /// <param name="strSWORKCLR"></param>
        /// <param name="strBISALERT"></param>
        /// <param name="strBISGROUPUSER"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int updateByUserId(String strSUSERID, String strSACCOUNTID, String strSPWD, String strSTAFFNO, String strSUSERNAME, String strSUSERNAMECN, String strSDEPT, String strSDEPTCN, String strSPOSI, String strSPOSICN, String strSTREECLR, String strSWORKCLR, String strBISALERT, String strBISGROUPUSER, String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_update_byUserId());
                param[0].Value = strSUSERID;
                param[1].Value = strSACCOUNTID;
                param[2].Value = strSPWD;
                param[3].Value = strSTAFFNO;
                param[4].Value = strSUSERNAME;
                param[5].Value = strSUSERNAMECN;
                param[6].Value = strSDEPT;
                param[7].Value = strSDEPTCN;
                param[8].Value = strSPOSI;
                param[9].Value = strSPOSICN;
                param[10].Value = strSTREECLR;
                param[11].Value = strSWORKCLR;
                param[12].Value = strBISALERT;
                param[13].Value = strBISGROUPUSER;
                param[14].Value = strBISSTOP;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_update_byUserId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion


        #region 根据用户ID，修改其密码字段
        /// <summary>
        /// 根据用户ID，修改其密码字段
        /// </summary>
        /// <param name="strPwd"></param>
        /// <param name="strUserId"></param>
        /// <returns>int</returns>
        public int updatePwdByUserId(String strPwd,String strUserId)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_changPwd_byUserId());
                param[0].Value = strPwd;
                param[1].Value = strUserId;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USER_changPwd_byUserId(), param);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
            }
            return iCount;
        }
        #endregion

    }
}
