using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class UserRoleDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 根据用户ID查询该用户角色信息,返回dataset记录
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
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_select_byUserId(), param);

            }
            return ds;
        }
        #endregion

        #region 根据用户ID查询该用户角色,返回datatable记录
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
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_select_byUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 插入用户角色表一条记录
        /// <summary>
        /// 插入用户角色表一条记录
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <returns></returns>
        public int insertOneRow(String strSUSERID, String strTID, String strRID)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_insert());
                param[0].Value = strSUSERID;
                param[1].Value = strTID;
                param[2].Value = strRID;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据用户ID更新用户角色表信息
        /// <summary>
        /// 根据用户ID更新用户角色表信息
        /// </summary>
        /// <param name="strSUSERID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <returns></returns>
        public int updateByUserId(String strSUSERID, String strTID, String strRID)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_update_byUserId());
                param[0].Value = strSUSERID;
                param[1].Value = strTID;
                param[2].Value = strRID;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_update_byUserId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据用户ID删除用户角色表一条记录，返回删除记录数
        /// <summary>
        /// 根据用户ID删除用户角色表一条记录，返回删除记录数
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>int</returns>
        public int deleteById(String strUserId)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_delete_byUserId());
                param[0].Value = strUserId;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_delete_byUserId(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据用户ID查询该用户角色的详细信息(TB_HRTMPR),返回datatable记录
        /// <summary>
        /// 根据用户ID查询该用户角色的详细信息,返回datatable记录
        /// </summary>
        /// <param name="strUserId"></param>
        /// <returns>DataTable</returns>
        public DataTable findRoleInfoTableByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_byUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据当前用户ID查询该用户角色且要设置的用户不具备的角色的详细信息(TB_HRTMPR),返回datatable记录
        /// <summary>
        /// 根据当前用户ID查询该用户角色且要设置的用户不具备的角色的详细信息,返回datatable记录
        /// </summary>
        /// <param name="strCurUserId"></param>
        /// <param name="strSelUserId"></param>
        /// <returns>DataTable</returns>
        public DataTable findRoleInfoTableByCurUserIdAndSelUserId(String strCurUserId, String strSelUserId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_byCurUserIdAndSelUserId());
                param[0].Value = strCurUserId;
                param[1].Value = strSelUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_byCurUserIdAndSelUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 插入用户ID及其对应的一组角色,返回记录数
        /// <summary>
        /// 插入用户ID及其对应的一组角色,返回记录数
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strRidArr[]"></param>
        /// <returns>DataTable</returns>
        public int insertUserRoleRecord(String strUserId, String[] strRidArr)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();
                //先删除
                DbParameter[] param1 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_delete_byUserId());
                param1[0].Value = strUserId;
                DataSet ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_delete_byUserId(), param1);

                //然后后插入一组数据
                String[] strArrTemp =new String[2];
                String strLid = "";
                String strRid = "";
                if (strRidArr!=null)
                {
                    for (int i = 0; i < strRidArr.Length; i++)
                    {
                        strArrTemp = strRidArr[i].Split('*');
                        strLid = strArrTemp[0].ToString();
                        strRid = strArrTemp[1].ToString();
                        if ((!String.IsNullOrEmpty(strLid)) && (!String.IsNullOrEmpty(strRid)))
                        {
                            DbParameter[] param2 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_insert());
                            param2[0].Value = strUserId;
                            param2[1].Value = strLid;
                            param2[2].Value = strRid;
                            int iReturn = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USERROLE_insert(), param2);
                            if (iReturn > 0) { iCount++; continue; }
                            throw new Exception();
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

        #region 查询角色表TB_HRTMPR所有记录的部分字段信息，返回DataTable记录集
        /// <summary>
        /// 查询角色表TB_HRTMPR所有记录的部分字段信息，返回DataTable记录集
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable findPartColOfAllRole()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_part(), null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据获取表TB_HRTMPR中所有记录部分字段，同时过滤掉已经存在于特定用户的角色，返回DataTable记录集
        /// <summary>
        /// 根据获取表TB_HRTMPR中所有记录部分字段，同时过滤掉已经存在于特定用户的角色，返回DataTable记录集
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable findPartColOfNotInUserRoleByUserId(String strUserId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_partNotIn_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRTMPR_select_partNotIn_byUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion
    }
}
