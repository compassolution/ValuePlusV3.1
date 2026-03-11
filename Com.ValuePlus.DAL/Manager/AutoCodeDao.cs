using System;
using System.Data;
using System.Data.Common;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL
{
    public class AutoCodeDao
    {
        #region 查询自动编号表所有记录，返回dateset记录集
        /// <summary>
        /// 查询自动编号表所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询自动编号表所有记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询自动编号表所有记录，返回dateset记录集
        /// </summary>
        /// <param name="strAid"></param>
        /// <returns>DataSet</returns>
        public DataSet findByAid(String strAid)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_select_byAid());
                param[0].Value = strAid;
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_select_byAid(), param);
            }
            return ds;
        }
        #endregion

        #region 插入自动编号表一条记录
        /// <summary>
        /// 插入自动编号表一条记录
        /// </summary>
        /// <param name="strAID"></param>
        /// <param name="strADESC"></param>
        /// <param name="strADESCCHS"></param>
        /// <param name="strAPREFIX"></param>
        /// <param name="strADATE"></param>
        /// <param name="iALENGTH"></param>
        /// <param name="iANEXTNO"></param>
        /// <param name="strALASTDATE"></param>
        /// <returns></returns>
        public int insertOneRow(String strAID, String strADESC, String strADESCCHS, String strAPREFIX, String strADATE, int iALENGTH, int iANEXTNO, String strALASTDATE)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_insert());
                param[0].Value = strAID;
                param[1].Value = strADESC;
                param[2].Value = strADESCCHS;
                param[3].Value = strAPREFIX;
                param[4].Value = strADATE;
                param[5].Value = iALENGTH;
                param[6].Value = iANEXTNO;
                param[7].Value = strALASTDATE;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新自动编号表一条记录
        /// <summary>
        /// 根据主键更新自动编号表一条记录
        /// </summary>
        /// <param name="strAID"></param>
        /// <param name="strADESC"></param>
        /// <param name="strADESCCHS"></param>
        /// <param name="strAPREFIX"></param>
        /// <param name="strADATE"></param>
        /// <param name="iALENGTH"></param>
        /// <param name="iANEXTNO"></param>
        /// <param name="strALASTDATE"></param>
        /// <returns></returns>
        public int updateByAid(String strAID, String strADESC, String strADESCCHS, String strAPREFIX, String strADATE, int iALENGTH, int iANEXTNO, String strALASTDATE)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_update_byAid());
                param[0].Value = strAID;
                param[1].Value = strADESC;
                param[2].Value = strADESCCHS;
                param[3].Value = strAPREFIX;
                param[4].Value = strADATE;
                param[5].Value = iALENGTH;
                param[6].Value = iANEXTNO;
                param[7].Value = strALASTDATE;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_update_byAid(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 根据主键删除自动编号表一条或者多条记录，返回删除记录数
        /// <summary>
        /// 根据主键删除自动编号表一条或者多条记录，返回删除记录数
        /// </summary>
        /// <param name="strAid"></param>
        /// <returns>DataSet</returns>
        public int deleteById(String strAid)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_delete_byAid());
                param[0].Value = strAid;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_HRAUTO_delete_byAid(), param);
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
