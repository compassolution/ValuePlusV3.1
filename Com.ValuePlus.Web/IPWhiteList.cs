using Com.ValuePlus.DAL;
using Com.ValuePlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
//using Com.ValuePlus.Labor;

namespace Com.ValuePlus.Web
{
    public class IPWhiteList
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 获取白名单列表并判断是否可以访问应用
        /// <summary>
        /// 获取白名单列表并判断是否可以访问应用
        /// </summary>
        /// <param name="strLid"></param>
        /// <param name="isContentStopped"></param>
        /// <param name="isEscape"></param>
        public static bool IsCanVisitByIPWhiteList(String strInputIP, String strUserId)
        {
            bool bIsCanVisit = false;
            try
            {
                //几种无需判断直接通过判断可访问
                //##########情况1：设置为不启用白名单功能直接通过判断
                String strIsEnableIPWhiteList = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("IsEnableIPWhiteList");
                if (!strIsEnableIPWhiteList.Equals("1"))
                {
                    log.Error("获取白名单列表并判断是否可以访问应用====设置为不启用白名单功能直接通过判断");
                    return true;
                }
                //##########情况2：管理员admin账号直接通过判断
                if (strUserId.ToLower().Equals("admin"))
                {
                    log.Error("获取白名单列表并判断是否可以访问应用====管理员admin账号直接通过判断");
                    return true;
                }
                //##########情况3：获取到的IP地址不正确即没有四段，则暂时通过判断
                String[] strArrayInputIP = strInputIP.Trim().Split('.');
                if (strArrayInputIP.Length != 4)
                {
                    log.Error("获取白名单列表并判断是否可以访问应用====获取到的IP地址不正确即没有四段，则暂时通过判断");
                    return true;
                }

                String strProjectId = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("ProjectId");
                String strCompanyCode = "";

                //目前只是外包工系统会用到CompanyCode去区分
                if (strProjectId.Length >= 8 && strProjectId.Substring(0, 8).ToLower().Equals("vp_labor"))
                {
                    log.Error("获取白名单列表并判断是否可以访问应用===ProjectId 为"+ strProjectId);
                    String strSql_CompanyCode = " select * from LUser_1 where UserCode = '" + strUserId + "'";
                    DataTable dt_CompanyCode = SqlParamDao.GetDataTableBySql(strSql_CompanyCode);
                    if (dt_CompanyCode != null && dt_CompanyCode.Rows.Count == 1)
                    {
                        strCompanyCode = dt_CompanyCode.Rows[0]["CompanyCode"].ToString();
                    }

                    //##########情况4：【Labor系统中】对应用人单位参数配置中设置为不启用白名单功能直接通过判断，则暂时通过判断
                    Hashtable hsTableEmployerConfig = GetEmployerConfigHashTable(strCompanyCode);
                    if (hsTableEmployerConfig == null || (hsTableEmployerConfig != null && hsTableEmployerConfig["IsEnableIPWhiteList"].ToString().Equals("false")))
                    {
                        log.Error("获取白名单列表并判断是否可以访问应用====【Labor系统中】对i应用人单位参数配置中设置为不启用白名单功能直接通过判断，则暂时通过判断");
                        return true;
                    }

                }

                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append(" select * from IPWhiteList_1 where ProjectId = '" + strProjectId + "'");
                if (!String.IsNullOrEmpty(strCompanyCode))
                {
                    sBuilder.Append(" and CompanyCode = '" + strCompanyCode + "'");
                }
                log.Error("获取白名单列表并判断是否可以访问应用====白名单SQL："+sBuilder.ToString());

                //读取白名单表进行判断
                DataTable dt = SqlParamDao.GetDataTableBySql(sBuilder.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        String strIPScope = dr["IPAddress"].ToString().Trim();
                        String strIsBlackList = dr["IsBlackList"].ToString();

                        String[] strArrayIPScope = strIPScope.Split('.');
                        if (strArrayIPScope.Length != 4)
                        {
                            //如果白名单列表中的IP地址设置有误，则设置为不通过
                            log.Error("获取白名单列表并判断是否可以访问应用====如果白名单列表中的IP地址设置有误，则设置为不通过");
                            bIsCanVisit = false;
                            break;
                        }
                        else
                        {
                            //如果4段都完全匹配才算通过[同时支持带星号的情况]
                            log.Error("获取白名单列表并判断是否可以访问应用====如果4段都完全匹配才算通过[同时支持带星号的情况]");
                            if (
                                (strArrayInputIP[0].Trim() == strArrayIPScope[0].Trim()|| strArrayIPScope[0].Trim().Equals("*"))
                                && (strArrayInputIP[1].Trim() == strArrayIPScope[1].Trim() || strArrayIPScope[1].Trim().Equals("*"))
                                && (strArrayInputIP[2].Trim() == strArrayIPScope[2].Trim() || strArrayIPScope[2].Trim().Equals("*"))
                                && (strArrayInputIP[3].Trim() == strArrayIPScope[3].Trim() || strArrayIPScope[3].Trim().Equals("*"))
                                )
                            {
                                if (strIsBlackList.Equals("1"))//如果是黑名单情况，则直接不通过
                                {
                                    log.Error("获取白名单列表并判断是否可以访问应用====如果是黑名单情况，则直接不通过并跳出循环");
                                    bIsCanVisit = false;
                                }
                                else
                                {
                                    log.Error("获取白名单列表并判断是否可以访问应用====如果是白名单情况，则直接通过并跳出循环");
                                    bIsCanVisit = true;

                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    log.Error("获取白名单列表并判断是否可以访问应用====白名单表没数据，无法通过判断");
                    bIsCanVisit = false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("获取白名单列表并判断是否可以访问应用失败");
            }
            return bIsCanVisit;
        }
        #endregion


        #region 【Labor外包工系统】
        /// <summary>
        /// 获取公司用人单位配置表信息数据表集合返回HashTable
        /// </summary>
        /// <param name="strCompanyCode"></param>
        /// <returns></returns>
        private static Hashtable GetEmployerConfigHashTable(String strCompanyCode)
        {
            Hashtable hsTableReturn = new Hashtable();
            try
            {
                StringBuilder sbSql = new StringBuilder();

                sbSql.Append("select * from [LEmployerConfig_1] A where 1=1 ");
                if (!String.IsNullOrEmpty(strCompanyCode))
                {
                    sbSql.Append(" and A.CompanyCode = '" + strCompanyCode+"'");
                }
                sbSql.Append(" order by A.CompanyCode");
                String strSql = sbSql.ToString();

                DataTable dtReturn = SqlParamDao.GetDataTableBySql(strSql);

                if ((dtReturn != null) && (dtReturn.Rows.Count == 1))
                {
                    DataRow dr = dtReturn.Rows[0];
                    for (int i = 0; i < dtReturn.Columns.Count; i++)
                    {
                        String strColumnName = dtReturn.Columns[i].ToString();
                        hsTableReturn.Remove(strColumnName);
                        hsTableReturn.Add(strColumnName, dr[strColumnName].ToString());
                    }
                }
                log.Error("Labor 获取公司用人单位配置表信息数据表集合返回HashTable，Sql:" + strSql);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return hsTableReturn;
        }

        #endregion
    }



}
