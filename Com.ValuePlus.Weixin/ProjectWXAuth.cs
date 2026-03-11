using Com.ValuePlus.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.ValuePlus.Weixin
{

    /// <summary>
    /// 项目的微信功能授权查询类类
    /// </summary>
    public class ProjectWXAuth
    {

        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 判断某项目的某个微信功能在某个时间是否有效
        /// </summary>
        /// <param name="strProjectId"></param>
        /// <param name="strWXMPModule"></param>
        /// <param name="JudgeDateTime"></param>
        /// <returns></returns>
        public static bool JudgeWXAuthIsValidByProjectId(String strProjectId, String strWXMPModule, String JudgeDateTime)
        {
            bool IsValid = false;
            StringBuilder sbSql = new StringBuilder();
            try
            {
                if (String.IsNullOrEmpty(JudgeDateTime)) {
                    JudgeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                sbSql.Append("select * from WXProjectAuth_1 where ProjectId = '"+ strProjectId+"' ");
                sbSql.Append(" and ('" + JudgeDateTime + "' between PeriodFrom" + strWXMPModule + " and PeriodTo" + strWXMPModule + ") ");
                sbSql.Append(" and IsValid = '1'");
                DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    IsValid = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("判断某项目的某个微信功能在某个时间是否有效时出错：" + ex);
                log.Error("判断某项目的某个微信功能在某个时间是否有效出错的SQL语句：" + sbSql.ToString());

            }
            return IsValid;
        }

        /// <summary>
        /// 获取某项目的某个微信功能的有效期间(返回值为“yyyy-MM-dd HH:mm:ss;yyyy-MM-dd HH:mm:ss”)
        /// 如无期限则返回为空字符串
        /// </summary>
        /// <param name="strProjectId"></param>
        /// <param name="strWXMPModule"></param>
        /// <returns></returns>
        public static String GetWXAuthPeriodByProjectId(String strProjectId, String strWXMPModule)
        {
            String strReturn = "";
            StringBuilder sbSql = new StringBuilder();
            try
            {
                sbSql.Append("select * from WXProjectAuth_1 where ProjectId = '" + strProjectId + "' and IsValid = '1'");
                DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    String strPeriodFrom = dt.Rows[0]["PeriodFrom"+ strWXMPModule].ToString();
                    String strPeriodTo = dt.Rows[0]["PeriodTo" + strWXMPModule].ToString();
                    strReturn = strPeriodFrom+";"+ strPeriodTo;
                }
            }
            catch (Exception ex)
            {
                log.Error("获取某项目的某个微信功能的有效期间时出错：" + ex);
                log.Error("获取某项目的某个微信功能的有效期间出错的SQL语句：" + sbSql.ToString());

            }
            return strReturn;
        }

    }
}
