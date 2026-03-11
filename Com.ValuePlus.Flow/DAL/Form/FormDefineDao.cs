using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;

namespace Com.ValuePlus.Flow.DAL.Form
{
    public class FormDefineDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表TB_FORM_DEFINE所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FORM_DEFINE所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FORM_DEFINE的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FORM_DEFINE的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKeyValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_selectByKey());
                param[0].Value = strKeyValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 根据表单编码查询TB_FORM_DEFINE的相应记录，返回dateset记录集
        /// <summary>
        /// 根据表单编码查询TB_FORM_DEFINE的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByCode(String strCodeValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_selectByCode());
                param[0].Value = strCodeValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_selectByCode(), param);
            }
            return ds;
        }
        #endregion

        #region 新增一条记录到表TB_FORM_DEFINE中，返回成功新增记录数
        /// <summary>
        /// 新增一条记录到表TB_FORM_DEFINE中，返回成功新增记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int insertOneRow(String strSFORMID, String strSFORMCODE, String strSFORMNAME, String strSFORMAMECN, String strSFORMDESC, String strSFORMDESCCN, String strSPLUGINAFTERSVAE, String strBISVERSION, String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_insert());
                param[0].Value = strSFORMID;
                param[1].Value = strSFORMCODE;
                param[2].Value = strSFORMNAME;
                param[3].Value = strSFORMAMECN;
                param[4].Value = strSFORMDESC;
                param[5].Value = strSFORMDESCCN;
                param[6].Value = strSPLUGINAFTERSVAE;
                param[7].Value = strBISVERSION;
                param[8].Value = strBISSTOP;
                object obj = dao.ExecuteScalar(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新表TB_FORM_DEFINE一条记录，返回成功更新记录数
        /// <summary>
        /// 根据主键更新表TB_FORM_DEFINE一条记录，返回成功更新记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateByKey(String strSFORMID, String strSFORMCODE, String strSFORMNAME, String strSFORMAMECN, String strSFORMDESC, String strSFORMDESCCN, String strSPLUGINAFTERSVAE, String strBISVERSION, String strBISSTOP)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_updateByKey());
                param[0].Value = strSFORMID;
                param[1].Value = strSFORMCODE;
                param[2].Value = strSFORMNAME;
                param[3].Value = strSFORMAMECN;
                param[4].Value = strSFORMDESC;
                param[5].Value = strSFORMDESCCN;
                param[6].Value = strSPLUGINAFTERSVAE;
                param[7].Value = strBISVERSION;
                param[8].Value = strBISSTOP;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_updateByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键删除表TB_FORM_DEFINE的相应记录，返回成功删除记录数
        /// <summary>
        /// 根据主键删除表TB_FORM_DEFINE的相应记录，返回成功删除记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int deleteByKey(String strKeyValue)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_deleteByKey());
                param[0].Value = strKeyValue;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FORM_DEFINE_deleteByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

    }
}
