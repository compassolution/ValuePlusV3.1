using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.DAL.Query
{
    public class QueryMainDao
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static String strViewKeyColName = BaseConfig.Instance.GetConfigValueByKey("KeyName_QueryView");

        #region 通过视图名称，获取该视图字段名
        /// <summary>
        /// 通过视图名称，获取该视图字段名
        /// </summary>
        /// <param name="strViewName"></param>
        /// <returns>DataTable</returns>
        public DataTable selectViewColInfo(String strViewName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //String strSql = "select top 1 * from " + strViewName;
            //String strSql = "SELECT B.NAME FROM sysobjects A,syscolumns B WHERE A.xtype = 'v' AND A.ID = B.ID AND A.NAME = '" + strViewName + "'";
            String strSql = "SELECT column_name,data_type  FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =  '" + strViewName + "' ORDER BY ORDINAL_POSITION";
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

        #region 通过视图名称，获取该视图的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，获取该视图的所有记录，返回dataset
        /// </summary>
        /// <param name="strViewName"></param>
        /// <param name="isHaveKey"></param>
        /// <returns>DataSet</returns>
        public DataSet selectViewAllInfo(String strViewName, bool isHaveKey)
        {
            DataSet ds = new DataSet();
            String strSql = "select * from " + strViewName;
            if (isHaveKey)
            {
                strSql = strSql + " order by " + strViewKeyColName;
            }
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
            }
            return ds;
        }
        #endregion

        #region 通过视图名称，服务器端分页获取该视图的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，服务器端分页获取该视图的所有记录，返回dataset
        /// </summary>
        /// <param name="strViewName"></param>
        /// <param name="isPageMode">是否分页</param>
        /// <param name="iPageIndex">当前页数</param>
        /// <param name="iPageSize">每页显示数</param>
        /// <param name="strFilterSql">查询条件</param>
        /// <param name="strDsSort">排序规则</param>
        /// <param name="iRecordCount">返回的总记录数</param>
        /// <returns>DataSet</returns>
        public DataSet selectViewDataInfoByPage(String strViewName, bool isPageMode, int iPageIndex, int iPageSize, String strFilterSql, String strDsSort, ref int iRecordCount)
        {
            DataSet ds = new DataSet();
            using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            {
                //获取全部记录数
                iRecordCount = 0;

                String strTableName = "(select * from " + strViewName + ") ViewQueryTemp1";
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
                    strSql = strSql + ") ViewQueryTemp2 WHERE ROW_ID > " + ((iPageIndex) * iPageSize).ToString();


                }
                ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);

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

            //DataSet ds = new DataSet();
            //int iStartIndex = iPageIndex * iPageSize;
            //String strSql = "select top " + iPageSize.ToString() + " * from " + strViewName + " where " + strViewKeyColName + " NOT IN (SELECT TOP " + iStartIndex.ToString() + " " + strViewKeyColName + " FROM " + strViewName + " ORDER BY " + strViewKeyColName + ")" + " ORDER BY " + strViewKeyColName;
            //using (IDatabaseDAO dao = DALFactory.CreateSqlServerDAO())
            //{
            //    ds = dao.ExecuteDataSet(CommandType.Text, strSql, null);
            //    //同时删除列strViewKeyColName信息
            //    if (ds.Tables[0].Columns.Contains(strViewKeyColName))
            //    {
            //        ds.Tables[0].Columns.Remove(strViewKeyColName);
            //    }
            //}
            ////获取全部记录数
            //iRecordCount = 0;
            //DataSet ds1 = selectViewAllInfo(strViewName,true);
            //if (ds1 != null)
            //{
            //    iRecordCount=ds1.Tables[0].Rows.Count;
            //}

            return ds;
        }
        #endregion


    }
}
