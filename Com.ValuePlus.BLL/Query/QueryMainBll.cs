using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.DAL.Query;
using Com.ValuePlus.Common;

namespace Com.ValuePlus.BLL.Query
{
    public class QueryMainBll
    {
        #region 通过视图名称，返回该视图对象的所有字段名到Hashtable
        /// <summary>
        /// 通过视图名称，返回该视图对象的所有字段名到Hashtable
        /// </summary>
        /// <param name="strViewName"></param>
        /// <returns>Hashtable</returns>
        public SortHashTable GetViewColInfo(String strViewName)
        {
            SortHashTable hsTable = new SortHashTable();
            QueryMainDao daoQueryMain = new QueryMainDao();
            DataTable dt = daoQueryMain.selectViewColInfo(strViewName);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                //DataRow row = dt.Rows[0];
                foreach (DataRow row in dt.Rows)
                {
                    //hsTable.Add(row["NAME"].ToString(), row["NAME"].ToString());
                    hsTable.Add(row["column_name"].ToString(), row["data_type"].ToString());
                }
            }
            return hsTable;
        }
        #endregion

        #region 通过视图名称，获取通过视图名称的输出列集合，返回到ArrayList
        /// <summary>
        /// 通过视图名称，获取通过视图名称的输出列集合，返回到ArrayList
        /// </summary>
        /// <param name="strViewName"></param>
        /// <returns>ArrayList</returns>
        public ArrayList GetViewColArrayList(String strViewName)
        {
            ArrayList arrList = new ArrayList();
            QueryMainDao daoQueryMain = new QueryMainDao();
            DataTable dt = daoQueryMain.selectViewColInfo(strViewName);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                int iCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    arrList.Add(row["column_name"].ToString() + "※" + row["data_type"].ToString());
                    iCount++;
                }
            }
            return arrList;
        }
        #endregion


        #region 通过视图名称，获取该视图的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，获取该视图的所有记录，返回dataset
        /// </summary>
        /// <param name="strViewName"></param>
        /// <param name="isHaveKey"></param>
        /// <returns>DataSet</returns>
        public DataSet GetViewAllDataInfo(String strViewName, bool isHaveKey)
        {
            QueryMainDao daoQueryMain = new QueryMainDao();
            DataSet ds = daoQueryMain.selectViewAllInfo(strViewName, isHaveKey);
            return ds;
        }
        #endregion

        #region 通过视图名称，在服务器端分页获取该视图的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，在服务器端分页获取该视图的所有记录，返回dataset
        /// </summary>
        /// <param name="strViewName"></param>
        /// <param name="isPageMode">是否分页</param>
        /// <param name="iPageIndex">当前页数</param>
        /// <param name="iPageSize">每页显示数</param>
        /// <param name="strFilterSql">查询条件</param>
        /// <param name="strDsSort">排序规则</param>
        /// <param name="iRecordCount">返回的总记录数</param>
        /// <returns>DataSet</returns>
        public DataSet GetViewDataInfoByPage(String strViewName, bool isPageMode, int iPageIndex, int iPageSize, String strFilterSql, String strDsSort, ref int iRecordCount)
        {
            QueryMainDao daoQueryMain = new QueryMainDao();
            DataSet ds = daoQueryMain.selectViewDataInfoByPage(strViewName,isPageMode, iPageIndex, iPageSize,strFilterSql,strDsSort, ref iRecordCount);
            return ds;
        }
        #endregion


    }
}
