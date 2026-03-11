using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class DicDetailDao
    {
        #region 根据菜单定义ID查询菜单内容明细表记录，返回dateset记录集
        /// <summary>
        /// 根据菜单定义ID查询菜单内容明细表记录，返回dateset记录集
        /// </summary>
        /// <param name="strLid"></param>
        /// <returns>DataSet</returns>
        public DataSet findByLid(String strLid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_select_byLid());
                param[0].Value = strLid;
                ds = dao.ExecuteDataSet( SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_select_byLid(), param);
            }
            return ds;
        }
        #endregion

        #region 根据菜单定义ID以及明细表ID查询菜单内容明细表记录，返回dateset记录集
        /// <summary>
        /// 根据菜单定义ID以及明细表ID查询菜单内容明细表记录，返回dateset记录集
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="strCid"></param>
        /// <returns>DataSet</returns>
        public DataSet findByLidACid(String strLid,String strCid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_select_byLidCid());
                param[0].Value = strLid;
                param[1].Value = strCid;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_select_byLidCid(), param);
            }
            return ds;
        }
        #endregion

        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <param name="strLID"></param>
        /// <param name="strCID"></param>
        /// <param name="strCDESC"></param>
        /// <param name="strCDESCCHS"></param>
        /// <param name="strCUID"></param>
        /// <param name="strP0"></param>
        /// <param name="strP1"></param>
        /// <param name="strP2"></param>
        /// <param name="strP3"></param>
        /// <param name="strP4"></param>
        /// <param name="strP5"></param>
        /// <param name="strP6"></param>
        /// <param name="strP7"></param>
        /// <param name="strP8"></param>
        /// <param name="strP9"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int insertOneRow(String strLID, String strCID, String strCDESC, String strCDESCCHS, String strCUID, String strP0, String strP1, String strP2, String strP3, String strP4, String strP5, String strP6, String strP7, String strP8, String strP9, String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_insert());
                param[0].Value = strLID;
                param[1].Value = strCID;
                param[2].Value = strCDESC;
                param[3].Value = strCDESCCHS;
                param[4].Value = strCUID;
                param[5].Value = strP0;
                param[6].Value = strP1;
                param[7].Value = strP2;
                param[8].Value = strP3;
                param[9].Value = strP4;
                param[10].Value = strP5;
                param[11].Value = strP6;
                param[12].Value = strP7;
                param[13].Value = strP8;
                param[14].Value = strP9;
                param[15].Value = strBISSTOP;
                object obj = dao.ExecuteScalar(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion


        #region 根据菜单定义表ID删除菜单内容明细表一条或者多条记录，返回删除记录数
        /// <summary>
        /// 根据菜单定义表ID删除菜单内容明细表一条或者多条记录，返回删除记录数
        /// </summary>
        /// <param name="strLid"></param>
        /// <returns>DataSet</returns>
        public int deleteByLid(String strLid)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_delete_byLid());
                param[0].Value = strLid;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_delete_byLid(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据菜单定义表ID和内容明细表ID删除菜单内容明细表一条记录，返回删除记录数
        /// <summary>
        /// 根据菜单定义表ID和内容明细表ID删除菜单内容明细表一条记录，返回删除记录数
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="strCid"></param>
        /// <returns>DataSet</returns>
        public int deleteByLidACid(String strLid, String strCid)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_delete_byLidCid());
                param[0].Value = strLid;
                param[1].Value = strCid;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_delete_byLidCid(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据菜单定义表ID和内容明细表I更新菜单内容明细表记录，返回更新记录数
        /// <summary>
        /// 根据菜单定义表ID和内容明细表I更新菜单内容明细表记录，返回更新记录数
        /// </summary>
        /// <param name="strLID"></param>
        /// <param name="strCID"></param>
        /// <param name="strCDESC"></param>
        /// <param name="strCDESCCHS"></param>
        /// <param name="strCUID"></param>
        /// <param name="strP0"></param>
        /// <param name="strP1"></param>
        /// <param name="strP2"></param>
        /// <param name="strP3"></param>
        /// <param name="strP4"></param>
        /// <param name="strP5"></param>
        /// <param name="strP6"></param>
        /// <param name="strP7"></param>
        /// <param name="strP8"></param>
        /// <param name="strP9"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns>DataSet</returns>
        public int updateByLidACid(String strLID, String strCID, String strCDESC, String strCDESCCHS, String strCUID, String strP0, String strP1, String strP2, String strP3, String strP4, String strP5, String strP6, String strP7, String strP8, String strP9, String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_update_byLidCid());
                param[0].Value = strLID;
                param[1].Value = strCID;
                param[2].Value = strCDESC;
                param[3].Value = strCDESCCHS;
                param[4].Value = strCUID;
                param[5].Value = strP0;
                param[6].Value = strP1;
                param[7].Value = strP2;
                param[8].Value = strP3;
                param[9].Value = strP4;
                param[10].Value = strP5;
                param[11].Value = strP6;
                param[12].Value = strP7;
                param[13].Value = strP8;
                param[14].Value = strP9;
                param[15].Value = strBISSTOP;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRLSTD_update_byLidCid(), param);
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
