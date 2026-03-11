using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class DicListDao
    {
        #region 查询菜单定义表所有记录，返回dateset记录集
        /// <summary>
        /// 查询菜单定义表所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_selectAll(), null);
            }
            return ds;
        }
        #endregion


        #region 根据主键查询菜单定义表记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询菜单定义表记录，返回dateset记录集
        /// </summary>
        /// <param name="strLid"></param>
        /// <returns>DataSet</returns>
        public DataSet findById(String strLid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_select_byLid());
                param[0].Value = strLid;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_select_byLid(), param);
            }
            return ds;
        }
        #endregion


        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="strDesc"></param>
        /// <param name="strDescChs"></param>
        /// <param name="strIsStop"></param>
        /// <returns></returns>
        public int insertOneRow(String strLid, String strDesc,String strDescChs,String strIsStop)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_insert());
                param[0].Value = strLid;
                param[1].Value = strDesc;
                param[2].Value = strDescChs;
                param[3].Value = strIsStop;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion


        #region 根据主键删除菜单定义表一条或者多条记录，返回删除记录数
        /// <summary>
        /// 根据主键删除菜单定义表一条或者多条记录，返回删除记录数
        /// </summary>
        /// <param name="strLid"></param>
        /// <returns>DataSet</returns>
        public int deleteById(String strLid)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_delete_byLid());
                param[0].Value = strLid;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_delete_byLid(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion


        #region 根据主键更新菜单定义表记录，返回更新记录数
        /// <summary>
        /// 根据主键更新菜单定义表记录，返回更新记录数
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="strDesc"></param>
        /// <param name="strDescChs"></param>
        /// <param name="strIsStop"></param>
        /// <returns>DataSet</returns>
        public int updateById(String strLid,String strDesc,String strDescchs,String strIsstop)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_update_byLid());
                param[0].Value = strLid;
                param[1].Value = strDesc;
                param[2].Value = strDescchs;
                param[3].Value = strIsstop;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRLSTH_update_byLid(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion



    }
}
