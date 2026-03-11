using Com.ValuePlus.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Com.ValuePlus.Labor
{
    public class LEmployerConfig
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取公司用人单位配置表信息数据表集合
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        public static DataTable GetEmployerConfigDataTable(String strCompanyCode)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select * from [LEmployerConfig_1] A where 1=1 ");
                if (!String.IsNullOrEmpty(strCompanyCode))
                {
                    sbSql.Append(" and A.CompanyCode = '" + strCompanyCode + "'");
                }
                sbSql.Append(" order by A.CompanyCode");
                String strSql = sbSql.ToString();

                log.Error("Labor 获取公司用人单位配置表信息数据表集合，Sql:" + strSql);
                dtReturn = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dtReturn;
        }

        /// <summary>
        /// 获取公司用人单位配置表信息数据表集合返回HashTable
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        public static Hashtable GetEmployerConfigHashTable(String strCompanyCode)
        {
            Hashtable hsTableReturn = new Hashtable();
            try
            {
                DataTable dtReturn = GetEmployerConfigDataTable(strCompanyCode);
                if((dtReturn != null) && (dtReturn.Rows.Count == 1))
                {
                    DataRow dr = dtReturn.Rows[0];
                    for(int i=0; i < dtReturn.Columns.Count; i++)
                    {
                        String strColumnName = dtReturn.Columns[i].ToString();
                        hsTableReturn.Remove(strColumnName);
                        hsTableReturn.Add(strColumnName,dr[strColumnName].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return hsTableReturn;
        }


    }
}
