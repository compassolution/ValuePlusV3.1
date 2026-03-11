using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Common.Config;
using System.Collections;
using System.Data;
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.SysParams
{
    /// <summary>
    /// 档案模板样式参数获取类
    /// </summary>
    public class ArchiveStyleParamsGetter
    {
        private static String strArchiveName = BaseConfig.Instance.GetConfigValueByKey("TID_ArchiveStyleParam");

        /// <summary>
        /// 通过模板编码获取其对应各样式参数值集合
        /// </summary>
        /// <param name="strParamName">参数名</param>
        /// <returns></returns>
        public static Hashtable GetArchiveStyleParams(String strTID)
        {
            Hashtable hsArchive = new Hashtable();
            if (!String.IsNullOrEmpty(strTID))
            {
                String strTableName = "";
                string strSql = "SELECT * FROM TB_HRTMPG WHERE TID='" + strArchiveName + "' AND GTYPE = '0' ORDER BY GORDER";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    String strMainG = dr["GID"].ToString();
                    strTableName = strArchiveName + "_" + strMainG;
                }

                if (!String.IsNullOrEmpty(strTableName))
                {
                    strSql = "SELECT * FROM " + strTableName + " where TID = '" + strTID + "'";
                    DataTable dtArchive = SqlParamDao.GetDataTableBySql(strSql);
                    if (dtArchive != null && dtArchive.Rows.Count > 0)
                    {
                        DataRow dr = dtArchive.Rows[0];

                        //获取表列集合
                        String strSqlCol = "select a.[name] from [syscolumns] a inner join [sysobjects] b on a.[id] = b.[id] and b.[name] = '" + strTableName + "' order by [colorder]";
                        DataTable dtCol = SqlParamDao.GetDataTableBySql(strSqlCol);
                        if (dtCol != null && dtCol.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtCol.Rows.Count; i++)
                            {
                                DataRow row = dtCol.Rows[i];

                                String strColTemp = row["name"].ToString();

                                hsArchive.Add(strColTemp, dr[strColTemp].ToString());
                            }
                        }

                        //hsArchive.Add("COLNUM", dr["COLNUM"].ToString());
                        //hsArchive.Add("TABLEWIDTH", dr["TABLEWIDTH"].ToString());
                        //hsArchive.Add("LABELWIDTH", dr["LABELWIDTH"].ToString());
                        //hsArchive.Add("PAGETYPE", dr["PAGETYPE"].ToString());
                        //hsArchive.Add("ISCLOSE", dr["ISCLOSE"].ToString());
                    }
                }
            }
            return hsArchive;
        }
    }
}
