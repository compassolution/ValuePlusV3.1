using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.DAL.Query;

namespace Com.ValuePlus.BLL.Query
{
    public class QuerySelectBll
    {
        #region 通过sql语句，获取对应数据集
        /// <summary>
        /// 通过sql语句，获取对应数据集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>Hashtable</returns>
        public DataTable GetDataInfoBySql(String strSql)
        {
            DataTable dt = new DataTable();
            if (strSql.ToLower().Contains("select"))
            {
                QuerySelectDao daoQuerySelect = new QuerySelectDao();
                dt = daoQuerySelect.selectDataBySql(strSql);
            }
            return dt;
        }
        #endregion
    }


}
