using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Entity.Regist;

namespace Com.ValuePlus.DAL.Regist
{
    public class RegistDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 获取表TB_VP_REGIST所有记录
        /// <summary>
        /// 获取表TB_VP_REGIST所有记录
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet selectRegistInfo()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(SqlConfig_wsm.Instance.GetSql_TB_VP_REGIST_select(), null);
            }
            return ds;
        }
        #endregion

        #region 插入TB_VP_REGIST表一条记录
        /// <summary>
        /// 插入TB_VP_REGIST表一条记录
        /// </summary>
        /// <param name="entityRegist"></param>
        /// <returns>int</returns>
        /// <returns>int</returns>
        public int insertOneRow(RegistInfoEntity entityRegist)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_VP_REGIST_insert());
                param[0].Value = entityRegist.strSKEY;
                param[1].Value = entityRegist.strSCLIENTNAME;
                param[2].Value = entityRegist.strSCONTACTOR;
                param[3].Value = entityRegist.strSCONTACTWAY;
                param[4].Value = entityRegist.strSGROUPNAME;
                param[5].Value = entityRegist.strSREGISTSTR;
                param[6].Value = entityRegist.strSASSIGNSTR;
                param[7].Value = entityRegist.dtDTREGISTDATA;
                param[8].Value = entityRegist.strSREQUESTIP;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_VP_REGIST_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj);
                }
            }
            return count;
        }
        #endregion

        #region 更新TB_VP_REGIST表记录
        /// <summary>
        /// 更新TB_VP_REGIST表记录
        /// </summary>
        /// <param name="entityRegist"></param>
        /// <returns>int</returns>
        public int updateRegistInfo(RegistInfoEntity entityRegist)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(SqlConfig_wsm.Instance.GetSql_TB_VP_REGIST_update());
                param[0].Value = entityRegist.strSKEY;
                param[1].Value = entityRegist.strSCLIENTNAME;
                param[2].Value = entityRegist.strSCONTACTOR;
                param[3].Value = entityRegist.strSCONTACTWAY;
                param[4].Value = entityRegist.strSGROUPNAME;
                param[5].Value = entityRegist.strSREGISTSTR;
                param[6].Value = entityRegist.strSASSIGNSTR;
                param[7].Value = entityRegist.dtDTREGISTDATA;
                param[8].Value = entityRegist.strSREQUESTIP;
                object obj = dao.ExecuteNonQuery(SqlConfig_wsm.Instance.GetSql_TB_VP_REGIST_update(), param);
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
