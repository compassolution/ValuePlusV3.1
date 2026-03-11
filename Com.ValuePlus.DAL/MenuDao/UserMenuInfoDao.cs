using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class UserMenuInfoDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 根据栏目编码删除用户栏目表一条或者多条记录，返回删除记录数
        /// <summary>
        /// 根据栏目编码删除用户栏目表一条或者多条记录，返回删除记录数
        /// </summary>
        /// <param name="strMenuCode"></param>
        /// <returns>DataSet</returns>
        public int deleteByMenuCode(String strMenuCode)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byMenuCode());
                param[0].Value = strMenuCode;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byMenuCode(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据用户ID查询该用户具有的栏目信息,返回datatable记录
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
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_select_byUserId());
                param[0].Value = strUserId;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_select_byUserId(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 插入用户ID及其对应的一组栏目,返回记录数
        /// <summary>
        /// 插入用户ID及其对应的一组栏目,返回记录数
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strMenuCode[]"></param>
        /// <returns>DataTable</returns>
        public int insertUserMenuRecord(String strUserId, String[] strMenuCode)
        {
            int iCount = 0;
            IDatabaseDAO dao = DALFactory.CreateSqlServerDAO();
            try
            {
                //事务开始
                dao.BeginTransaction();
                //先删除
                DbParameter[] param1 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byUserId());
                param1[0].Value = strUserId;
                DataSet ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_delete_byUserId(), param1);

                //然后后插入一组数据
                if (strMenuCode!=null)
                {
                    for (int i = 0; i < strMenuCode.Length; i++)
                    {
                        String strKey = Guid.NewGuid().ToString();
                        String strCode = strMenuCode[i];
                        if (!String.IsNullOrEmpty(strCode))
                        {
                            DbParameter[] param2 = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_insert());
                            param2[0].Value = strKey;
                            param2[1].Value = strUserId;
                            param2[2].Value = strCode;
                            param2[3].Value = "1";
                            int iReturn = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_USERMENU_insert(), param2);
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
    }
}
