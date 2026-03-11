using System;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;

namespace Com.ValuePlus.Archive.BLL
{
    /// <summary>
    /// ALOCATION:
    /// 0:主页面
    /// 1:明细页面
    /// 2:增加后
    /// 3:编辑后
    /// 4:删除后
    /// 5:增加前
    /// 6:删除前
    /// </summary>

    public class ArchiveActionBll
    {   
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取当前模板当前状态下不同显示位置的动作列表（同时已经作过参数处理）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="iLocationFlag"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="strIsHis"></param>
        /// <returns>ArrayList</returns>
        public ArrayList GetActionDetailList(string strTID, string strRID, string strSID, int iLocationFlag,string strKeyValue, string strUserId, bool isAdmin, string strIsHis)
        {
            ArrayList arrActionList =  new ArrayList();
            string strSql = "SELECT AID,ADESC,ADESCCHS,ATYPE,ISAUTOSAVE FROM TB_HRTMPSA WHERE TID='" + strTID + "' AND SID='" + strSID + "'AND ARIGHT=1 AND ALOCATION=" + iLocationFlag + " ORDER BY AORDER";

            try
            {
                
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        Entity_CreateAction entity = new Entity_CreateAction();
                        entity.TID = strTID;
                        entity.SID = strSID;
                        entity.AID = dr["AID"].ToString();
                        entity.ADESC = dr["ADESC"].ToString();
                        entity.ADESCCHS = dr["ADESCCHS"].ToString();
                        entity.ATYPE = dr["ATYPE"].ToString();
                        entity.ADETAIL = this.GetActionDetailPage(strTID, strRID, strSID, entity.AID, strKeyValue, strUserId, isAdmin, strIsHis);
                        entity.ISAUTOSAVE = dr["ISAUTOSAVE"]!=null?int.Parse(dr["ISAUTOSAVE"].ToString()):0;
                        arrActionList.Add(entity);
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("ArchiveActionBll.GetActionDetailList() error: Sql:" + strSql);
            }
            return arrActionList;
        }


        /// <summary>
        /// 获取活动明细信息，返回页面链接或者存储过程名称或者报表文件名称（同时已经作过参数处理）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isHis"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public String GetActionDetailPage(string strTID, string strRID, string strSID, string strAID,string strKeyValue, string strUserId, bool isAdmin,string isHis)
        {
            string strActionPage = "";
            string strSql = "SELECT ADETAIL,APARA0,APARA1,APARA2,APARA3,APARA4,APARA5,APARA6,APARA7,APARA8,APARA9,ATYPE,ALOCATION FROM TB_HRTMPSA WHERE TID='" + strTID + "' AND SID='" + strSID + "' AND AID='" + strAID + "'";

            try
            {
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow row = dt.Rows[0];
                    if (row["ADETAIL"] == DBNull.Value)
                    {
                        return "";
                    }
                    //动作明细
                    String strDetail = row["ADETAIL"].ToString();
                    //动作类型（0页面，1报表，2存储过程,3存储查询）
                    string strActionType = row["ATYPE"].ToString();

                    if (!String.IsNullOrEmpty(strActionType))
                    {
                        //switch (strActionType)
                        //{
                        //    case "0"://页面
                        //        strActionPage = String.Format("http://{0}/", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + strDetail;
                        //        break;
                        //    case "1"://报表
                        //        strActionPage = String.Format("http://{0}/", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "Report/ReportMain.aspx?RPT=" + strDetail;
                        //        break;
                        //    case "2"://存储过程
                        //        String strParams = "SP=" + strDetail + "&TID=" + strTID + "&RID=" + strRID + "&SID=" + strSID + "&AID=" + strAID;
                        //        strActionPage = String.Format("http://{0}/", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "Archive/Action/DoAction.aspx?" + strParams;
                        //        break;
                        //    case "3"://存储查询
                        //        strActionPage = String.Format("http://{0}/", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "Query/SPQuery.aspx?SP=" + strDetail;
                        //        break;

                        //}

                        //modify by sammen 20181130 使用相对地址，兼容https及外网地址映射的需求
                        String strPathPre = "";
                        if (row["ALOCATION"].ToString().Equals("0"))//模板主列表页面
                        {
                            strPathPre = "../";
                        }
                        else
                        {
                            strPathPre = "../../";
                        }
                        switch (strActionType)
                        {
                            case "0"://页面
                                strActionPage = strPathPre + strDetail;
                                break;
                            case "1"://报表
                                strActionPage = strPathPre + "Report/ReportMain.aspx?RPT=" + strDetail;
                                break;
                            case "2"://存储过程 
                                //add by sammen 20240429 增加KEYVALUE的传递
                                String strParams = "SP=" + strDetail + "&TID=" + strTID + "&RID=" + strRID + "&SID=" + strSID + "&AID=" + strAID + "&KEYVALUE=" + strKeyValue;
                                strActionPage = strPathPre + "Archive/Action/DoAction.aspx?" + strParams;
                                break;
                            case "3"://存储查询
                                strActionPage = strPathPre + "Query/SPQuery.aspx?SP=" + strDetail;
                                break;
                        }
                    }

                    if ((row["APARA0"] != DBNull.Value) && (row["APARA0"].ToString() != ""))
                    {
                        if (row["ATYPE"].ToString().Equals("0"))
                        {
                            if (strActionPage.IndexOf("?") < 0)
                            {
                                strActionPage = strActionPage + "?";
                            }else
                            {
                                strActionPage = strActionPage + "&";
                            }
                        }
                        else
                        {
                            strActionPage = strActionPage + "&";
                        }
                        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                        //通过TID获取当前角色下所设置的参数值
                        Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserId, isAdmin);
                        for (int i = 0; i < 9; i++)
                        {
                            String strAparaFieldName = "APARA" + i.ToString();
                            if ((row[strAparaFieldName] != DBNull.Value) && (row[strAparaFieldName].ToString() != ""))
                            {
                                String strAparaFieldValue = row[strAparaFieldName].ToString();
                                if (i > 0)
                                {
                                    strActionPage = strActionPage + "&";
                                }
                                strActionPage = strActionPage + "P" + i.ToString() + "=";
                                strAparaFieldValue = ParamOperationBll.ReplaceParamToValue(strAparaFieldValue, hsCurRoleParamValue);

                                strAparaFieldValue = ParamOperationBll.ReplaceActionParam(strAparaFieldValue, strTID, strRID, strSID, strAID, strKeyValue, strUserId, isHis);
                                strActionPage = strActionPage + strAparaFieldValue;

                                //add by sammen 20181122 链接到页面型的动作，自动替换参数
                                if (row["ATYPE"].ToString().Equals("0"))
                                {
                                    //如果链接中自带了@P0@/@P0@....@P9@
                                    strActionPage = strActionPage.Replace("@P" + i.ToString() + "@", strAparaFieldValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("ArchiveActionBll.GetActionDetailPage() error: Sql:" + strSql);
            }
            return strActionPage;
        }

        /// <summary>
        /// 执行ACTION对应的存储过程
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isHis"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public int ExcuteActionDetailSP(string strTID, string strRID, string strSID, string strAID, string strKeyValue, string strUserId, bool isAdmin, string isHis)
        {
            int iCount = 0;
            string strSql = "SELECT ADETAIL,APARA0,APARA1,APARA2,APARA3,APARA4,APARA5,APARA6,APARA7,APARA8,APARA9,ATYPE FROM TB_HRTMPSA WHERE TID='" + strTID + "' AND SID='" + strSID + "' AND AID='" + strAID + "'";

            try
            {
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow row = dt.Rows[0];
                    if (row["ADETAIL"] == DBNull.Value)
                    {
                        return 0;
                    }
                    //动作类型（0页面，1报表，2存储过程,3存储查询）
                    string strActionType = row["ATYPE"].ToString();
                    if ((!String.IsNullOrEmpty(strActionType)) && (strActionType.Equals("2")))
                    {
                        //动作明细(存储过程名称)
                        String strSpName = row["ADETAIL"].ToString();
                        //获取该存储过程对应的参数信息
                        ArrayList arrListSPParam = this.GetSpParamInfo(strSpName);
                        Hashtable hsSpParamValue = new Hashtable();

                        if ((row["APARA0"] != DBNull.Value) && (row["APARA0"].ToString() != ""))
                        {
                            GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                            //通过TID获取当前角色下所设置的参数值
                            Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserId, isAdmin);

                            if ((arrListSPParam != null) && (arrListSPParam.Count > 0))
                            {
                                for (int i = 0; i < arrListSPParam.Count; i++)
                                {
                                    SpParamEntity paramProperty = (SpParamEntity)arrListSPParam[i];
                                    String strParamName = paramProperty.strParamName;

                                    String strAparaFieldName = "APARA" + i.ToString();
                                    if ((row[strAparaFieldName] != DBNull.Value) && (row[strAparaFieldName].ToString() != ""))
                                    {
                                        String strAparaFieldValue = row[strAparaFieldName].ToString();
                                        strAparaFieldValue = ParamOperationBll.ReplaceParamToValue(strAparaFieldValue, hsCurRoleParamValue);

                                        strAparaFieldValue = ParamOperationBll.ReplaceActionParam(strAparaFieldValue, strTID, strRID, strSID, strAID, strKeyValue, strUserId, isHis);
                                        hsSpParamValue.Add(strParamName, strAparaFieldValue);
                                    }
                                }
                            }
                        }
                        //执行存储过程
                        iCount = DoExcuteSP(strSpName, hsSpParamValue);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("ArchiveActionBll.ExcuteActionDetailSP() error: Sql:" + strSql);
            }
            return iCount;
        }

        /// <summary>
        /// 根据LOCATION字段的配置执行ACTION对应的存储过程
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isHis"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public int ExcuteActionDetailSPByLocation(string strTID, string strRID, string strSID, int iLocationFlag, string strKeyValue, string strUserId, bool isAdmin, string isHis )
        {
            int iCount = 0;
            string strSql = "SELECT AID,ADESC,ADESCCHS,ATYPE FROM TB_HRTMPSA WHERE TID='" + strTID + "' AND SID='" + strSID + "'AND ARIGHT=1 AND ALOCATION=" + iLocationFlag + " ORDER BY AORDER";

            try
            {
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count == 1))
                {
                    String strAID = dt.Rows[0]["AID"].ToString();
                    iCount = this.ExcuteActionDetailSP(strTID, strRID, strSID, strAID, strKeyValue, strUserId, isAdmin, isHis);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("ArchiveActionBll.ExcuteActionDetailSPByLocation() error: Sql:" + strSql);
            }
            return iCount;
        }

        /// <summary>
        /// 根据执行存储过程的结果返回页面提示
        /// </summary>
        public String GetPageTipAfterExcuteSp(String strTid, String strSid, int iALocaction, int iReturn, String strLanguage)
        {
            String strMsgName = "";
            String strMsg = "";
            if (strLanguage == "zh-cn")
            {
                strMsgName = "MESSCHS";
            }
            else
            {
                strMsgName = "MESSENG";
            }
            String strSql = "SELECT " + strMsgName + " FROM TB_HRTMPSA A,TB_HRTMPAR B WHERE A.TID = B.TID AND A.AID = B.AID AND A.TID='" + strTid + "' AND A.SID='" + strSid + "' AND A.ARIGHT=1 AND A.ALOCATION=" + iALocaction + " AND ECFROM<=" + iReturn + " AND ECTO>=" + iReturn;
            DataSet set = SqlParamDao.GetDataSetBySql(strSql);
            if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
            {
                strMsg = set.Tables[0].Rows[0][0].ToString().Replace("@S@", iReturn.ToString());
            }
            return strMsg;
        }

        #region 涉及存储过程相关操作
        /// <summary>
        /// 通过存储过程名称，获取该存储过程的输入参数，返回到ArrayList
        /// </summary>
        /// <param name="strSpName"></param>
        /// <returns>Hashtable</returns>
        public ArrayList GetSpParamInfo(String strSpName)
        {
            ArrayList arrList = new ArrayList(); 
            String strSql = "SELECT SUBSTRING(PARAMETER_NAME,2,50) AS PARAM ,DATA_TYPE AS TYPE FROM INFORMATION_SCHEMA.PARAMETERS WHERE SPECIFIC_NAME= '" + strSpName + "' ORDER BY ORDINAL_POSITION";
               
            try
            {
                 DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    int iCount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        SpParamEntity paramProperty = new SpParamEntity();
                        paramProperty.strParamName = row["PARAM"].ToString();
                        paramProperty.strParamDataType = row["TYPE"].ToString();
                        arrList.Add(paramProperty);
                        iCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("ArchiveActionBll---GetSpParamInfo error: Sql:" + strSql);
            }
            return arrList;
        }

        /// <summary>
        /// 根据实体SpXmlEntity从TB_HRTMPSD中获取对应信息并返回实体类SpXmlEntity
        /// </summary>
        /// <param name="entityXml"></param>
        /// <returns></returns>
        public SpXmlEntity GetSpXmlEntityInfoByParam(SpXmlEntity entityXml)
        {
            if (entityXml != null)
            {
                String strSql = "SELECT PID,PDESC,PDESCCHS,PCTRL,PCTRLID,PCTRLD,PTYPE,PDEFAULT,PSYS,PRIGHT,PMAST FROM TB_HRTMPSD  WHERE TID ='" + entityXml.strTid + "' AND GID ='" + entityXml.strGid + "' AND SID ='" + entityXml.strSid + "' AND PID ='" + entityXml.strPid + "'";
                    
                try
                {
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        entityXml.strPID = dt.Rows[0]["PID"].ToString();
                        entityXml.strPCTRLTYPE = dt.Rows[0]["PCTRL"].ToString();
                        entityXml.strPDESCCHS = dt.Rows[0]["PDESCCHS"].ToString();
                        entityXml.strPDESC = dt.Rows[0]["PDESC"].ToString();
                        entityXml.strPCTRLID = dt.Rows[0]["PCTRLID"].ToString();
                        entityXml.strPCTRLSQL = dt.Rows[0]["PCTRLD"].ToString();
                        entityXml.strPDATATYPE = dt.Rows[0]["PTYPE"].ToString();
                        entityXml.strPDEFAULT = dt.Rows[0]["PDEFAULT"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Error("ArchiveActionBll---GetSpXmlEntityInfoByParam error: Sql:" + strSql);
                }
            }
            return entityXml;
        }

        /// <summary>
        /// 执行动作存储过程
        /// <param name="hsTableParam"></param>
        /// <param name="strSpName"></param>
        /// </summary>
        public static int DoExcuteSP(String strSpName, Hashtable hsTableParam)
        {
            int iCount = 0;
            try
            {
                if (!String.IsNullOrEmpty(strSpName))
                {
                    iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
            }
            catch (Exception ex)
            {
                iCount = -1;
                log.Error(ex);
                log.Error("ArchiveActionBll---DoExcuteSP error: SPNAME:" + strSpName);
            }
            return iCount;
        }
        #endregion

    }
}
