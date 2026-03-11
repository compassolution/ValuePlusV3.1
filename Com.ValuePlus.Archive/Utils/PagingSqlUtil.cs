using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Archive.Utils
{
    public class PagingSqlUtil
    {
        /// <summary>
        /// 获取分页获取记录集的sql语句
        /// </summary>
        /// <param name="strOldSql">常规SQL语句</param>
        /// <param name="iPageSize">每页显示记录数</param>
        /// <param name="iPageIndex">当前页索引</param>
        /// <param name="strKey">主键</param>
        /// <returns></returns>
        public static string GetPagingSql(string strOldSql, int iPageSize, int iPageIndex, string strKey)
        {
            //int num = iPageSize * iPageIndex;
            //String strNewSql = "SELECT TOP " + iPageSize.ToString() + " * FROM (" + strOldSql + ") A  WHERE " + strKey + " NOT IN (SELECT TOP " + num.ToString() + " " + strKey + " FROM (" + strOldSql + ") A)";
            //return strNewSql;

            //GTYPE=2时的两个主键列 add by sammen 20211103
            String strMultiKey = strKey;
            String strMultiKeyFieldString = "A." + strKey + " = B." + strKey;
            String[] strArrayKey = strKey.Split(',');
            if (strArrayKey.Length == 2)
            {
                strMultiKey = strArrayKey[0] + "," + strArrayKey[1];
                strMultiKeyFieldString = "A." + strArrayKey[0] + " = B." + strArrayKey[0] + " AND A." + strArrayKey[1] + " = B." + strArrayKey[1];
            }

            int iBeforeNum = iPageSize * iPageIndex;
            int iAfterNum = iPageSize * (iPageIndex+1);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.* FROM (" + strOldSql + ") A");
            sbSql.Append(" ,(SELECT TOP "+ iAfterNum .ToString()+ " row_number() OVER (ORDER BY " + strKey + " ) as Num, " + strMultiKey + " FROM (" + strOldSql + ") tempTable) B");
            sbSql.Append("  WHERE "+ strMultiKeyFieldString + " AND B.Num > "+ iBeforeNum.ToString()+"");
            String strNewSql = sbSql.ToString();
            return strNewSql;
        }

        /// <summary>
        /// 获取分页获取记录集的sql语句
        /// </summary>
        /// <param name="strOldSql">常规SQL语句</param>
        /// <param name="iPageSize">每页显示记录数</param>
        /// <param name="iPageIndex">当前页索引</param>
        /// <param name="strKey">主键</param>
        /// <param name="strSort">排序规则</param>
        /// <returns></returns>
        public static string GetPagingSql(string strOldSql, int iPageSize, int iPageIndex, string strKey, string strSort)
        {
            //int num = iPageSize * iPageIndex;
            //String strNewSql = "SELECT TOP " + iPageSize.ToString() + " * FROM (" + strOldSql + ") A  WHERE " + strKey + " NOT IN (SELECT TOP " + num.ToString() + " " + strKey + " FROM (" + strOldSql + ") B order by " + strSort + " ) order by " + strSort;
            //return strNewSql;

            //GTYPE=2时的两个主键列 add by sammen 20211103
            String strMultiKey = strKey;
            String strMultiKeyFieldString = "A." + strKey + " = B." + strKey;
            String[] strArrayKey = strKey.Split(',');
            if (strArrayKey.Length == 2)
            {
                strMultiKey = strArrayKey[0] + "," + strArrayKey[1];
                strMultiKeyFieldString = "A." + strArrayKey[0] + " = B." + strArrayKey[0] + " AND A." + strArrayKey[1] + " = B." + strArrayKey[1];
            }

            int iBeforeNum = iPageSize * iPageIndex;
            int iAfterNum = iPageSize * (iPageIndex + 1);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT * FROM (");
            sbSql.Append("SELECT A.* FROM (" + strOldSql + ") A");
            sbSql.Append(" ,(SELECT TOP " + iAfterNum.ToString() + " row_number() OVER (ORDER BY " + strSort + " ) as Num, " + strMultiKey + " FROM (" + strOldSql + ") tempTable order by "+ strSort + ") B");
            sbSql.Append("  WHERE "+ strMultiKeyFieldString + " AND B.Num > " + iBeforeNum.ToString() + "");
            sbSql.Append(") tb order by "+ strSort);
            String strNewSql = sbSql.ToString();
            return strNewSql;

        }

        /// <summary>
        /// 根据sql语句获取其对应记录数的sql语句
        /// </summary>
        /// <param name="strOldSql"></param>
        /// <returns></returns>
        public static string GetCountSql(string strOldSql)
        {
            String strNewSql = ("SELECT count(1) FROM (" + strOldSql + ") A ");
            return strNewSql;
        }
    }
}
