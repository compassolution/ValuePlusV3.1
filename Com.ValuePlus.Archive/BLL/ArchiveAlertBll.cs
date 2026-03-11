using System;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using System.Text.RegularExpressions;

namespace Com.ValuePlus.Archive.BLL
{
    public class ArchiveAlertBll
    { 
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strLanguage"></param>
        /// <param name="isAdmintrator"></param>
        /// <param name="isRealTimeAlertMode">是否实时提醒模式(即是否需要弹出框实时提醒)</param>
        /// <returns></returns>
        public static DataSet getPendingItemLists(string strUserId, string strLanguage,bool isAdmintrator,bool isRealTimeAlertMode)
        {
            string str = "";
            if (strLanguage == "zh-cn")
            {
                str = "CHS";
            }
            string sqlstring = "SELECT rd.TID,t.TDESC" + str + " AS [TDESCRIPTION],rd.RID,r.RDESC" + str + " AS [RDESCRIPTION],rd.SID,s.SDESC" + str + " AS [SDESCRIPTION],0 AS [TCOUNT] FROM TB_HRTMPRD rd,TB_HR_USERROLE ur,TB_HRTMPS s,TB_HRTMPH t,TB_HRTMPR r WHERE ur.SUSERID='" + strUserId + "' AND rd.TID=ur.TID AND rd.RID=ur.RID AND s.TID=ur.TID AND s.SID=rd.SID AND s.SALERT=1 AND t.TID=ur.TID AND r.TID=ur.TID AND r.RID=ur.RID ";
            
            DataSet set = SqlParamDao.GetDataSetBySql(sqlstring);
            for (int i = set.Tables[0].Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = set.Tables[0].Rows[i];
                int iCount = GetPendingDetailCount(row["TID"].ToString(), row["RID"].ToString(), row["SID"].ToString(), strUserId, isAdmintrator, isRealTimeAlertMode);
                if (iCount == 0)
                {
                    set.Tables[0].Rows.Remove(row);
                }
                else
                {
                    row["TCOUNT"] = iCount;
                }
            }
            return set;
        }

        /// <summary>
        /// 计算总数
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserId"></param>
        /// <param name="bIsAdminstrator"></param>
        /// <param name="isRealTimeAlertMode">是否实时提醒模式(即是否需要弹出框实时提醒)</param>
        /// <returns></returns>
        private static int GetPendingDetailCount(string strTID, string strRID, string strSID, string strUserId, bool bIsAdminstrator, bool isRealTimeAlertMode)
        {
            int iCount = 0;
            Entity_ArchiveStyle entityArchiveStyle = ArchiveStyleGetterBll.GetTemplateStylesEntity(strTID);
            //再判断是否需要实时提醒模式 
            if (!((isRealTimeAlertMode)&&(!entityArchiveStyle.IsRealTimeAlert.Equals("1"))))
            {
                string strMainGID = SqlParamDao.GetDataTableBySql("SELECT GID FROM TB_HRTMPG WHERE TID='" + strTID + "' AND GTYPE=0").Rows[0][0].ToString();
                string strMainTable = strTID + "_" + strMainGID;
                DataSet set = SqlParamDao.GetDataSetBySql("SELECT SSLCT FROM TB_HRTMPS WHERE TID='" + strTID + "' AND SID='" + strSID + "'");
                string strSql = "SELECT * FROM " + strMainTable;
                if (set.Tables[0].Rows.Count > 0)
                {
                    DataRow row = set.Tables[0].Rows[0];
                    if ((row[0] != DBNull.Value) && (row[0].ToString() != ""))
                    {
                        strSql = removeOrderby(row[0].ToString());

                        //通过TID获取当前角色下所设置的参数值
                        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                        Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserId, bIsAdminstrator);

                        strSql = ParamOperationBll.ReplaceSceneSqlParam(strSql, hsCurRoleParamValue);
                    }
                }
                Regex regex = new Regex(@"[0-9a-zA-Z]+\.\*", RegexOptions.Compiled);
                if (regex.IsMatch(strSql))
                {
                    strSql = Regex.Replace(strSql, @"(?<alias>[0-9a-zA-Z])+\.\*", "COUNT('')");
                }
                else
                {
                    strSql = strSql.Replace("*", "COUNT('')");
                }

                Object obj = SqlParamDao.ExecuteScalarBySql(strSql);
                if (obj != null)
                {
                    iCount = Convert.ToInt32(obj);
                }
            }
            return iCount;
        }

        /// <summary>
        /// 获取待办事项实时提醒项目中的所有明细的条数总和
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strLanguage"></param>
        /// <param name="isAdmintrator"></param>
        /// <returns></returns>
        public static int GetAlertDetailCount(string strUserId, string strLanguage, bool isAdmintrator)
        {
            int iCount = 0;
            DataSet ds = getPendingItemLists(strUserId, strLanguage, isAdmintrator, true);
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    iCount = iCount + int.Parse(row["TCOUNT"].ToString());
                }
            }
            return iCount;
        }

        private static string removeOrderby(string _str)
        {
            int index = _str.IndexOf("ORDER BY", StringComparison.InvariantCultureIgnoreCase);
            if (index > 0)
            {
                return _str.Substring(0, index);
            }
            return _str;
        }

    }
}
