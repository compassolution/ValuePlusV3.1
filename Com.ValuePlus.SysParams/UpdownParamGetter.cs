using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.ValuePlus.DAL;
using System.Data;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.SysParams
{
    /// <summary>
    /// 文件上传下载功能配置数据获取类
    /// </summary>
    public class UpdownParamGetter
    {
        private static String strArchiveName = BaseConfig.Instance.GetConfigValueByKey("TID_UpdownConfigParam");

        /// <summary>
        /// 通过配置编码（上传下载功能编号）获取其对应参数值配置
        /// </summary>
        /// <param name="strFolder">配置编码</param>
        /// <returns></returns>
        public static Hashtable GetUpdownParams(String strFolder)
        {
            Hashtable hsArchive = new Hashtable();
            if (!String.IsNullOrEmpty(strFolder))
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
                    strSql = "SELECT * FROM " + strTableName + " where FID = '" + strFolder + "'";
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
                    }
                }
            }
            return hsArchive;
        }

    }
}
