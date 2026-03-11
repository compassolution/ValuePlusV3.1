using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Entity.Notice;

namespace Com.ValuePlus.DAL.Notice
{
    public class NoticeDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询公告表所有记录，返回dateset记录集
        /// <summary>
        /// 查询公告表所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 查询公告表所有记录，返回DataTable记录集
        /// <summary>
        /// 查询公告表所有记录，返回DataTable记录集
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable findAllTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_selectAll(), null);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据主键查询公告表记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询公告表记录，返回dateset记录集
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKey)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byKey());
                param[0].Value = strKey;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byKey(), param);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询公告表记录，返回datatable记录
        /// <summary>
        /// 根据主键查询公告表记录，返回datatable记录
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>DataSet</returns>
        public DataTable findTableByKey(String strKey)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byKey());
                param[0].Value = strKey;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byKey(), param);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            return dt;
        }
        #endregion

        #region 根据是否停用标记查询公告表记录，返回dateset记录集
        /// <summary>
        /// 根据是否停用标记查询公告表记录，返回dateset记录集
        /// </summary>
        /// <param name="bIsStop"></param>
        /// <returns>DataSet</returns>
        public DataSet findByIsStop(String bIsStop)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byIsStop());
                param[0].Value = bIsStop;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_select_byIsStop(), param);
            }
            return ds;
        }
        #endregion

        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <param name="NoticeEntity"></param>
        /// <returns></returns>
        public int insertOneRow(NoticeEntity entityNotice)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_insert());
                param[0].Value = entityNotice.strSKEY;
                param[1].Value = entityNotice.strSTITLE;
                param[2].Value = entityNotice.strSCONTENT;
                param[3].Value = entityNotice.strSPUBLISHOR;
                param[4].Value = entityNotice.dtDTPUBLISHTIME;
                param[5].Value = entityNotice.strSSOURCE;
                param[6].Value = entityNotice.strSEDITOR;
                param[7].Value = entityNotice.dtDTEDITTIME;
                param[8].Value = entityNotice.strBISSTOP;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 删除用户公告表相应记录
        /// <summary>
        /// 删除用户公告表相应记录
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns>int</returns>
        public int deleteByKey(String strKey)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_delete_byKey());
                param[0].Value = strKey;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_delete_byKey(), param);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
            }
            return iCount;
        }
        #endregion

        #region 根据主键更新公告表记录，返回更新记录数
        /// <summary>
        /// 根据主键更新公告表记录，返回更新记录数
        /// </summary>
        /// <param name="NoticeEntity"></param>
        /// <returns>int</returns>
        public int updateByKey(NoticeEntity entityNotice)
        {
            int iCount = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_update_byKey());
                param[0].Value = entityNotice.strSKEY;
                param[1].Value = entityNotice.strSTITLE;
                param[2].Value = entityNotice.strSCONTENT;
                param[3].Value = entityNotice.strSPUBLISHOR;
                param[4].Value = entityNotice.dtDTPUBLISHTIME;
                param[5].Value = entityNotice.strSSOURCE;
                param[6].Value = entityNotice.strSEDITOR;
                param[7].Value = entityNotice.dtDTEDITTIME;
                param[8].Value = entityNotice.strBISSTOP;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HR_PUBLIC_NOTICE_update_byKey(), param);
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
