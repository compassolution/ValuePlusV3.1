using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.DAL.Query;
using Com.ValuePlus.Entity.Report;
using Com.ValuePlus.Common;

namespace Com.ValuePlus.BLL.Query
{
    public class SPQueryBll
    {
        //#region 通过存储过程名称，获取该存储过程的输入参数，返回到Hashtable
        ///// <summary>
        ///// 通过存储过程名称，获取该存储过程的输入参数，返回到Hashtable
        ///// </summary>
        ///// <param name="strSpName"></param>
        ///// <returns>Hashtable</returns>
        //public Hashtable GetSpParamInfo(String strSpName)
        //{
        //    Hashtable hsTable = new Hashtable();
        //    SPQueryDao daoSpQuery = new SPQueryDao();
        //    DataTable dt = daoSpQuery.selectSpParamInfo(strSpName);
        //    if ((dt != null) && (dt.Rows.Count > 0))
        //    {
        //        int iCount = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            hsTable.Add(row["PARAM"].ToString(), row["TYPE"].ToString());
        //            iCount++;
        //        }
        //    }
        //    return hsTable;
        //}
        //#endregion

        #region 通过存储过程名称，获取该存储过程的输入参数，返回到ArrayList
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输入参数，返回到ArrayList
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>Hashtable</returns>
        public ArrayList GetSpParamInfo(String strSpName)
        {
            ArrayList arrList = new ArrayList();
            SPQueryDao daoSpQuery = new SPQueryDao();
            DataTable dt = daoSpQuery.selectSpParamInfo(strSpName);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                int iCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    ReportParamProperty paramProperty = new ReportParamProperty();
                    paramProperty.strParamName = row["PARAM"].ToString();
                    paramProperty.strParamDataType = row["TYPE"].ToString();
                    arrList.Add(paramProperty);
                    iCount++;
                }
            }
            return arrList;
        }
        #endregion

        #region 通过存储过程名称，获取该存储过程的输出列集合，返回到Hashtable
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输出列集合，返回到Hashtable
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>Hashtable</returns>
        public SortHashTable GetSpColInfo(String strSpName)
        {
            SortHashTable hsTable = new SortHashTable();
            SPQueryDao daoSpQuery = new SPQueryDao();
            DataTable dt = daoSpQuery.selectSpColInfo(strSpName);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                int iCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    hsTable.Add(row["column_name"].ToString(), row["data_type"].ToString());
                    iCount++;
                }
            }
            return hsTable;
        }
        #endregion

        #region 通过存储过程名称，获取该存储过程的输出列集合，返回到ArrayList
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输出列集合，返回到ArrayList
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>ArrayList</returns>
        public ArrayList GetSpColArrayList(String strSpName)
        {
            ArrayList arrList = new ArrayList();
            SPQueryDao daoSpQuery = new SPQueryDao();
            DataTable dt = daoSpQuery.selectSpColInfo(strSpName);
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

        #region 通过视图名称，获取该存储查询的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，获取该存储查询的所有记录，返回dataset
        /// </summary>
        /// <param name="strSpName"></param>
        /// <param name="hsTableParam"></param>
        /// <param name="isHaveKey"></param>
        /// <returns>DataSet</returns>
        public DataSet GetSpAllDataInfo(String strSpName, Hashtable hsTableParam, bool isHaveKey)
        {
            SPQueryDao daoSpQuery = new SPQueryDao();
            DataSet ds = daoSpQuery.selectSpAllDataInfo(strSpName, hsTableParam,isHaveKey);
            return ds;
        }
        #endregion

        #region 通过视图名称，在服务器端分页获取该存储查询的所有记录，返回dataset
        /// <summary>
        /// 通过视图名称，在服务器端分页获取该存储查询的所有记录，返回dataset
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
        public DataSet GetSpDataInfoByPage(String strSpName, Hashtable hsTableParam, bool isPageMode, int iPageIndex, int iPageSize, String strFilterSql, String strDsSort, ref int iRecordCount)
        {
            SPQueryDao daoSpQuery = new SPQueryDao();
            DataSet ds = daoSpQuery.selectSpDataInfoByPage(strSpName, hsTableParam,isPageMode, iPageIndex, iPageSize,strFilterSql,strDsSort, ref iRecordCount);
            return ds;
        }
        #endregion

    }
}
