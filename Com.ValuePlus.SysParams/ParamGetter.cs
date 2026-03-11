using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Com.ValuePlus.DAL;

namespace Com.ValuePlus.SysParams
{
    public class ParamGetter
    {
        /// <summary>
        /// 通过系统参数库及参数名称获取其对应参数值
        /// </summary>
        /// <param name="strParamAchive">参数库名称（模板ID）</param>
        /// <param name="strParamName">参数名</param>
        /// <returns></returns>
        public static String GetParamValue(String strParamAchive, String strParamName)
        {
            String strParamValue = "";
            if ((!String.IsNullOrEmpty(strParamAchive)) && (!String.IsNullOrEmpty(strParamName)))
            {
                Hashtable hsArchive = GetParamAchive(strParamAchive);
                if ((hsArchive != null) && (hsArchive.ContainsKey(strParamName)))
                {
                    strParamValue = hsArchive[strParamName].ToString();
                }
            }
            return strParamValue;
        }

        /// <summary>
        /// 通过系统参数库名获取其对应的各个参数，存放到HASHTABLE对象中
        /// </summary>
        /// <param name="strParamArchive"></param>
        /// <returns></returns>
        public static Hashtable GetParamAchive(String strParamArchive)
        {
            Hashtable hsArchive = new Hashtable();
            if (!String.IsNullOrEmpty(strParamArchive))
            {
                String strTableName = "";
                string strSql = "SELECT * FROM TB_HRTMPG WHERE TID='" + strParamArchive + "' AND GTYPE = '0' ORDER BY GORDER";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    String strMainG = dr["GID"].ToString();
                    strTableName = strParamArchive + "_" + strMainG;
                }
                if (!String.IsNullOrEmpty(strTableName))
                {
                    strSql = "SELECT * FROM " + strTableName + " where ISSTOP <> '1'";
                    DataTable dtArchive = SqlParamDao.GetDataTableBySql(strSql);
                    if (dtArchive != null && dtArchive.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtArchive.Rows.Count; i++)
                        {
                            DataRow dr = dtArchive.Rows[i];
                            String strParamName = dr["paramName"].ToString();
                            String strParamValue = dr["paramValue"].ToString();
                            hsArchive.Add(strParamName, strParamValue);
                        }
                    }
                }
            }
            return hsArchive;
        }


    }
}
