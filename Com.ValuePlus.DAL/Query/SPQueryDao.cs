using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.Query
{
    public class SPQueryDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static String strViewKeyColName = BaseConfig.Instance.GetConfigValueByKey("KeyName_QueryView");

        #region 通过存储过程名称，获取该存储过程的输入参数
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输入参数
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>DataTable</returns>
        public DataTable selectSpParamInfo(String strSpName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //String strSql = "SELECT B.NAME AS PARAM FROM sysobjects A,syscolumns B WHERE A.xtype = 'P' AND A.ID = B.ID AND A.NAME = '" + strSpName + "'";
            String strSql = "SELECT SUBSTRING(PARAMETER_NAME,2,50) AS PARAM ,DATA_TYPE AS TYPE FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME= '" + strSpName + "' ORDER BY ORDINAL_POSITION";
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

        #region 通过存储过程名称，获取该存储过程的输出列集合
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输出列集合
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>DataTable</returns>
        public DataTable selectSpColInfo(String strSpName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            String strSql = "SELECT column_name,data_type  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =  '_" + strSpName + "' ORDER BY ORDINAL_POSITION";
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

        #region 通过存储过程名称极其参数值，执行该存储过程，并将所有记录返回dataset
        /// <summary>
        /// 通过存储过程名称极其参数值，执行该存储过程，并将所有记录返回dataset
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <param name="isHaveKey"></param>
        /// <returns>DataSet</returns>
        public DataSet selectSpAllDataInfo(String strSpName,Hashtable hsTableParam,bool isHaveKey)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                String strSql = "select * from _" + strSpName;
                if ((hsTableParam != null) && (hsTableParam.Count > 0))
                {
                    if (isHaveKey)
                    {
                        strSql = strSql + " order by " + strViewKeyColName;
                    }
                    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
                    {
                        String strParamName = entity.Key.ToString();
                        String strParamValue = hsTableParam[strParamName].ToString();
                        dao.AddParameter("@" + strParamName, strParamValue, TypeDao.VarChar, 100);
                    }
                    DbParameter[] param = dao.GetParameters();
                    int iCount = dao.ExecuteNonQuery(CommandType.StoredProcedure, strSpName, param);

                    ds = dao.ExecuteDataSet(CommandType.Text, strSql, param);
                }
                else
                {
                    int iCount = dao.ExecuteNonQuery(CommandType.StoredProcedure, strSpName, null);
                    ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
                }

            }
            return ds;
        }
        #endregion

        #region 通过存储过程名称极其参数值，执行该存储过程，并将所有记录返回dataset
        /// <summary>
        /// 通过存储过程名称极其参数值，执行该存储过程，并将所有记录返回dataset
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <param name="isPageMode">是否分页</param>
        /// <param name="iPageIndex">当前页数</param>
        /// <param name="iPageSize">每页显示数</param>
        /// <param name="strFilterSql">查询条件</param>
        /// <param name="strDsSort">排序规则</param>
        /// <param name="iRecordCount">返回的总记录数</param>
        /// <returns>DataSet</returns>
        public DataSet selectSpDataInfoByPage(String strSpName, Hashtable hsTableParam,bool isPageMode, int iPageIndex, int iPageSize, String strFilterSql, String strDsSort, ref int iRecordCount)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                //获取全部记录数
                iRecordCount = 0;

                String strTableName = "_" + strSpName;
                String strSql = "";
                if (!isPageMode)//不分页查询
                {
                    strSql = "select * from " + strTableName + " where 1=1 ";
                    if (!String.IsNullOrEmpty(strFilterSql))
                    {
                        strSql = strSql + " AND " + strFilterSql;
                    }
                    if (!String.IsNullOrEmpty(strDsSort))
                    {
                        strSql = strSql + " ORDER BY " + strDsSort;
                    }
                }
                else//不分页查询
                {
                    String strOverOrder = strViewKeyColName;
                    if (!String.IsNullOrEmpty(strDsSort))
                    {
                        strOverOrder = strDsSort;
                    }
                    strSql = "select top " + iPageSize.ToString() + " * from (select top 100 percent *, ROW_NUMBER() OVER(ORDER BY " + strOverOrder + " ) AS ROW_ID  from " + strTableName + " where 1=1 ";
                    if (!String.IsNullOrEmpty(strFilterSql))
                    {
                        strSql = strSql + " AND " + strFilterSql;
                    }
                    if (!String.IsNullOrEmpty(strDsSort))
                    {
                        strSql = strSql + " ORDER BY " + strDsSort;
                    }
                    strSql = strSql + ") SpQueryTemp1 WHERE ROW_ID > " + ((iPageIndex) * iPageSize).ToString();
                    

                }
                //DbParameter[] param = null;
                //if ((hsTableParam != null) && (hsTableParam.Count > 0))
                //{
                //    foreach (System.Collections.DictionaryEntry entity in hsTableParam)
                //    {
                //        String strParamName = entity.Key.ToString();
                //        String strParamValue = hsTableParam[strParamName].ToString();
                //        dao.AddParameter("@" + strParamName, strParamValue, TypeDao.VarChar, 100);
                //    }
                //    param = dao.GetParameters();

                //}
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null );

                if (!isPageMode)//不分页查询
                {
                    if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                    {
                        iRecordCount = ds.Tables[0].Rows.Count;
                    }
                }
                else
                {
                    String strSql_All = "select count(*) from " + strTableName + " where 1=1 ";
                    if (!String.IsNullOrEmpty(strFilterSql))
                    {
                        strSql_All = strSql_All + " AND " + strFilterSql;
                    }
                    iRecordCount = (int)dao.ExecuteScalar(CommandType.Text, strSql_All, null);
                    
                }

                //同时删除列strViewKeyColName信息
                if (ds.Tables[0].Columns.Contains(strViewKeyColName))
                {
                    ds.Tables[0].Columns.Remove(strViewKeyColName);
                }
                if (ds.Tables[0].Columns.Contains("ROW_ID"))
                {
                    ds.Tables[0].Columns.Remove("ROW_ID");
                }

            }

            return ds;
        }
        #endregion
    }
}
