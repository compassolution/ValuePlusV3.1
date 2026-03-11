using Com.ValuePlus.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Com.ValuePlus.Web
{
    public class DicGetter
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 获取并返回字典信息
        /// <summary>
        /// 获取并返回字典信息
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="isContentStopped"></param>
        /// <param name="isEscape"></param>
        public static String GetDictionaryList(String strLid,bool isContentStopped,bool isEscape)
        {
            String strReturn = "";
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append("{");

                String[] arrList = strLid.Split(';');
                int iArrLength = arrList.Length;
                for (int t = 0; t < iArrLength; t++)
                {
                    if (t > 0)
                    {
                        sBuilder.Append(",");
                    }
                    String sLid = arrList[t];
                    String strTableName = "TB_HRLSTD";

                    //支持视图模式查询列表
                    if (sLid.StartsWith("@"))
                    {
                        String strTemp = sLid.TrimStart('@');
                        strTableName = strTemp.Split(':')[0];
                        sLid = strTemp.Split(':')[1];
                    }

                    String strSql = "select * from " + strTableName + " where LID= '" + sLid + "' ";
                    if (!isContentStopped)
                    {
                        strSql = strSql + " and BISSTOP <> '1'";
                    }
                    strSql = strSql + " ORDER BY ISNULL(P9,CID)";
                    log.Error("获取并返回字典信息:" + strSql);

                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt, "\""+sLid+ "\"", isEscape));
                }
                sBuilder.Append("}");
                strReturn = sBuilder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return strReturn;
        }
        #endregion

        #region 获取并返回字典信息
        /// <summary>
        /// 获取并返回字典信息
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="isContentStopped"></param>
        /// <param name="isEscape"></param>
        public static DataTable GetDictionaryListDataTable(String strLid, bool isContentStopped)
        {
            DataTable dt = new DataTable();
            try
            {
                String strTableName = "TB_HRLSTD";

                StringBuilder sbMultiLid = new StringBuilder();
                String[] arrList = strLid.Split(';');
                int iArrLength = arrList.Length;
                for (int t = 0; t < iArrLength; t++)
                {
                    if (t > 0)
                    {
                        sbMultiLid.Append(",");
                    }
                    String sLid = arrList[t];

                    //支持视图模式查询列表
                    if (sLid.StartsWith("@"))
                    {
                        String strTemp = sLid.TrimStart('@');
                        strTableName = strTemp.Split(':')[0];
                        sLid = strTemp.Split(':')[1];
                    }
                    sbMultiLid.Append("'"+ sLid + "'");

                }
                String strSql = "select * from " + strTableName + " where LID in (" + sbMultiLid.ToString() + ") ";
                if (!isContentStopped)
                {
                    strSql = strSql + " and BISSTOP <> '1'";
                }
                strSql = strSql + " ORDER BY ISNULL(P9,CID)";
                log.Error("获取并返回字典信息:" + strSql);
                dt = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }
        #endregion

    }
}
