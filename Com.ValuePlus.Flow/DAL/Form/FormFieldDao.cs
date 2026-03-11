using System;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Flow.Config;
using System.Data;
using System.Data.Common;

namespace Com.ValuePlus.Flow.DAL.Form
{
    public class FormFieldDao
    {

        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 查询表TB_FORM_FIELD所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FORM_FIELD所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findAll()
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectAll(), null);
            }
            return ds;
        }
        #endregion

        #region 根据主键查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByKey(String strKeyValue)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByKey());
                param[0].Value = strKeyValue;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByKey(), param);
            }
            return ds;
        }
        #endregion

        #region 根据外键表单定义ID查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// <summary>
        /// 根据外键表单定义ID查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByFormId(String strFormId)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByFormId());
                param[0].Value = strFormId;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByFormId(), param);
            }
            return ds;
        }
        #endregion

        #region 根据表单定义ID和字段编码组合查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// <summary>
        /// 根据表单定义ID和字段编码组合查询TB_FORM_FIELD的相应记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet findByCodeAndFormId(String strFormId, String strFieldCode)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByKeyAFormId());
                param[0].Value = strFormId;
                param[1].Value = strFieldCode;
                ds = dao.ExecuteDataSet(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_selectByKeyAFormId(), param);
            }
            return ds;
        }
        #endregion

        #region 新增一条记录到表TB_FORM_FIELD中，返回成功新增记录数
        /// <summary>
        /// 新增一条记录到表TB_FORM_FIELD中，返回成功新增记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int insertOneRow(String strSFIELDID, String strSFORMID, String strSFIELDCODE, String strSFIELDNAME, String strSFIELDNAMECN, String strBISKEY, String strSFIELDTYPECODE, String strNFIELDLENGTH, String strSFIELDPRECISION, String strBISNULL, String strSDEFAULTVALUE, String strSCTRLTYPECODE, String strSCTRLDSSQL, String strNORDER, String strBISMAINVIEW, String strNCTRLLENGTH, String strBISMUST, String strSTIPDESC, String strSTIPDESCCN, String strSCTRLRIGHTTYPECODE, String strBISFKEY, String strSFKEYTABLE, String strSFKEYFIELD)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_insert());
                param[0].Value = strSFIELDID;
                param[1].Value = strSFORMID;
                param[2].Value = strSFIELDCODE;
                param[3].Value = strSFIELDNAME;
                param[4].Value = strSFIELDNAMECN;
                param[5].Value = strBISKEY;
                param[6].Value = strSFIELDTYPECODE;
                param[7].Value = System.Convert.ToDecimal(strNFIELDLENGTH);
                param[8].Value = strSFIELDPRECISION;
                param[9].Value = strBISNULL;
                param[10].Value = strSDEFAULTVALUE;
                param[11].Value = strSCTRLTYPECODE;
                param[12].Value = strSCTRLDSSQL;
                param[13].Value = System.Convert.ToDecimal(strNORDER);
                param[14].Value = strBISMAINVIEW;
                param[15].Value = System.Convert.ToDecimal(strNCTRLLENGTH);
                param[16].Value = strBISMUST;
                param[17].Value = strSTIPDESC;
                param[18].Value = strSTIPDESCCN;
                param[19].Value = strSCTRLRIGHTTYPECODE;
                param[20].Value = strBISFKEY;
                param[21].Value = strSFKEYTABLE;
                param[22].Value = strSFKEYFIELD;
                object obj = dao.ExecuteScalar(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_insert(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键更新表TB_FORM_FIELD一条记录，返回成功更新记录数
        /// <summary>
        /// 根据主键更新表TB_FORM_FIELD一条记录，返回成功更新记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int updateByKey(String strSFIELDID, String strSFORMID, String strSFIELDCODE, String strSFIELDNAME, String strSFIELDNAMECN, String strBISKEY, String strSFIELDTYPECODE, String strNFIELDLENGTH, String strSFIELDPRECISION, String strBISNULL, String strSDEFAULTVALUE, String strSCTRLTYPECODE, String strSCTRLDSSQL, String strNORDER, String strBISMAINVIEW, String strNCTRLLENGTH, String strBISMUST, String strSTIPDESC, String strSTIPDESCCN, String strSCTRLRIGHTTYPECODE, String strBISFKEY, String strSFKEYTABLE, String strSFKEYFIELD)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_updateByKey());
                param[0].Value = strSFIELDID;
                param[1].Value = strSFORMID;
                param[2].Value = strSFIELDCODE;
                param[3].Value = strSFIELDNAME;
                param[4].Value = strSFIELDNAMECN;
                param[5].Value = strBISKEY;
                param[6].Value = strSFIELDTYPECODE;
                param[7].Value = System.Convert.ToDecimal(strNFIELDLENGTH);
                param[8].Value = strSFIELDPRECISION;
                param[9].Value = strBISNULL;
                param[10].Value = strSDEFAULTVALUE;
                param[11].Value = strSCTRLTYPECODE;
                param[12].Value = strSCTRLDSSQL;
                param[13].Value = System.Convert.ToDecimal(strNORDER);
                param[14].Value = strBISMAINVIEW;
                param[15].Value = System.Convert.ToDecimal(strNCTRLLENGTH);
                param[16].Value = strBISMUST;
                param[17].Value = strSTIPDESC;
                param[18].Value = strSTIPDESCCN;
                param[19].Value = strSCTRLRIGHTTYPECODE;
                param[20].Value = strBISFKEY;
                param[21].Value = strSFKEYTABLE;
                param[22].Value = strSFKEYFIELD;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_updateByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion

        #region 根据主键删除表TB_FORM_FIELD的相应记录，返回成功删除记录数
        /// <summary>
        /// 根据主键删除表TB_FORM_FIELD的相应记录，返回成功删除记录数
        /// </summary>
        /// <returns>DataSet</returns>
        public int deleteByKey(String strKeyValue)
        {
            int count = 0;
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                DbParameter[] param = dao.MakeParameter(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_deleteByKey());
                param[0].Value = strKeyValue;
                object obj = dao.ExecuteNonQuery(FlowSqlConfig.Instance.GetSqlForTB_FORM_FIELD_deleteByKey(), param);
                if (obj != null)
                {
                    count = Convert.ToInt32(obj); ;
                }
            }
            return count;
        }
        #endregion
        
        #region 通过数据表名称，获取该表的所有字段
        /// <summary>
        /// 通过数据表名称，获取该表的所有字段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns>DataTable</returns>
        public DataTable selectCoulmnInfoFromTable(String strTableName)
        {
            DataSet ds = new DataSet(); 
            DataTable dt = new DataTable();
            String strSql = "SELECT B.NAME as columns FROM sysobjects A,syscolumns B WHERE A.xtype = 'U' AND A.name = '" + strTableName + "' AND A.ID = B.ID order by B.colorder";
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
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
