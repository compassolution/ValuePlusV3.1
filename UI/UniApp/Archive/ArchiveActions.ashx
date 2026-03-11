<%@ WebHandler Language="C#" Class="ArchiveActions" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Property;

public class ArchiveActions : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]
        string strTID = WebCommon.GetJsonValue(strParamJson,"tid").ToString();
        string strRID = WebCommon.GetJsonValue(strParamJson,"rid").ToString();
        string strSID = WebCommon.GetJsonValue(strParamJson,"sid").ToString();
        string strGID = WebCommon.GetJsonValue(strParamJson,"gid").ToString();
        string strSPName = WebCommon.GetJsonValue(strParamJson,"spname").ToString();
        string strAID = WebCommon.GetJsonValue(strParamJson,"aid").ToString();
        string strKEY = WebCommon.GetJsonValue(strParamJson,"key").ToString();
        string strKEYVALUE = WebCommon.GetJsonValue(strParamJson,"keyvalue").ToString();
        string strGRIDKEY = WebCommon.GetJsonValue(strParamJson,"gridkey").ToString();
        string strGRIDKEYVALUE = WebCommon.GetJsonValue(strParamJson,"gridkeyvalue").ToString();
        string strIsLoadAllRole = WebCommon.GetJsonValue(strParamJson,"isloadallrole").ToString();
        string strActionLocation = WebCommon.GetJsonValue(strParamJson,"actionlocation").ToString();
        string strCondition = WebCommon.GetJsonValue(strParamJson,"condition").ToString();
        string strSql = WebCommon.GetJsonValue(strParamJson,"sql").ToString();
        string strIsInsert = WebCommon.GetJsonValue(strParamJson,"isinsert").ToString();
        string strPostJsonData = WebCommon.GetJsonObjectValue(strParamJson,"postjsondata").ToString();

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getactiondatalist"://获取特定模板特定角色场景下的动作列表数据
                this.GetActionDataList(context,strTID,strRID,strSID,strAID,strActionLocation,strRequestLanguage,strUserCode);
                break;
            case "getonespactionandparams"://根据存储过程类型的动作AID获取动作明细及对应参数
                this.GetOneSPActionAndParams(context,strTID,strRID,strSID,strAID,strKEYVALUE,strUserCode);
                break;
            case "executespaction"://执行存储过程类型的动作
                this.ExecuteSPAction(context,strTID,strRID,strSID,strAID,strSPName,strPostJsonData,strRequestLanguage,strUserCode);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 获取特定模板特定角色场景下的动作列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strLocation"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void GetActionDataList(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strLocation,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取特定模板特定角色场景下的动作列表数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
            //获取特定模板特定角色下的场景列表数据
            String strJsonData_ActionList = mobileArchiveAction.GetArchiveActionData(strUserCode,strTID,strRID,strSID,strAID,strLocation,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData_ActionList);

            if(!String.IsNullOrEmpty(strJsonData_ActionList)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData_ActionList);
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据存储过程类型的动作AID获取动作明细及对应参数
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strKeyValue"></param>
    /// <param name="strUserCode"></param>
    public void GetOneSPActionAndParams(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strKeyValue,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据存储过程类型的动作AID获取动作明细及对应参数";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
            DataTable dt = mobileArchiveAction.GetDataTable_ActionList(strTID,strRID,strSID,strAID,"",strUserCode);
            if(dt!=null && dt.Rows.Count>0)
            {
                DataRow drAction = dt.Rows[0];
                String strSPName = drAction["ADETAIL"].ToString();
                String strLocation = drAction["ALOCATION"].ToString();
                String strMOVENEXT = drAction["MOVENEXT"].ToString();

                ArchiveActionBll bllAction = new ArchiveActionBll();
                ArrayList arrListParam = bllAction.GetSpParamInfo(strSPName);
                int iSPParamsCount = arrListParam.Count;
                sbResultData.Append("\"TID\":\"" + strTID + "\"");
                sbResultData.Append(",\"RID\":\"" + strRID + "\"");
                sbResultData.Append(",\"SID\":\"" + strSID + "\"");
                sbResultData.Append(",\"AID\":\"" + strAID + "\"");
                sbResultData.Append(",\"ADESC\":\"" + drAction["ADESC"].ToString() + "\"");
                sbResultData.Append(",\"ADESCCHS\":\"" + drAction["ADESCCHS"].ToString() + "\"");
                sbResultData.Append(",\"ADETAIL\":\"" + strSPName + "\"");
                sbResultData.Append(",\"ALOCATION\":\"" + strLocation + "\"");
                sbResultData.Append(",\"MOVENEXT\":\"" + strMOVENEXT + "\"");
                sbResultData.Append(",\"Params\":[");
                if ((arrListParam != null) && (arrListParam.Count > 0))//有需输入的参数则显示参数设置页面
                {
                    //获取存储过程参数相应xml文件的数据
                    Hashtable hsTableXMLParams = mobileArchiveAction.ReadXmlParamToHashTable(strSPName);

                    for (int i = 0; i < arrListParam.Count; i++)
                    {
                        SpParamEntity paramProperty = (SpParamEntity)arrListParam[i];
                        String strParamName = paramProperty.strParamName;
                        String strParamValue = drAction["APARA"+i.ToString()].ToString();
                        //替换带%的参数
                        strParamValue = strParamValue.Replace("%TID%", strTID);
                        strParamValue = strParamValue.Replace("%RID%", strRID);
                        strParamValue = strParamValue.Replace("%SID%", strSID);
                        strParamValue = strParamValue.Replace("%AID%", strAID);
                        strParamValue = strParamValue.Replace("%KEY%", strKeyValue);
                        strParamValue = strParamValue.Replace("%USERCODE%", strUserCode);

                        String strIsNeedInput = strParamValue.Equals("")?"1":"0";
                        String strIsExistsXml = "0";
                        sbResultData.Append( i > 0 ? ",":"");
                        //sbResultData.Append("{\"P" + i.ToString() + "\":");
                        sbResultData.Append("{");
                        sbResultData.Append("\"paramName\":\"" + strParamName + "\"");
                        sbResultData.Append(",\"paramValue\":\"" + strParamValue + "\"");
                        sbResultData.Append(",\"xmlParam\":{");
                        //if ( hsTableXMLParams.ContainsKey(strParamName)){
                        if ((String.IsNullOrEmpty(strParamValue)) && hsTableXMLParams.ContainsKey(strParamName))
                        {
                            strIsNeedInput = "1";
                            strIsExistsXml = "1";
                            SpXmlEntity entityXml = (SpXmlEntity)hsTableXMLParams[strParamName];
                            sbResultData.Append("\"TID\":\"" + entityXml.strTid + "\"");
                            sbResultData.Append(",\"GID\":\"" + entityXml.strGid + "\"");
                            sbResultData.Append(",\"SID\":\"" + entityXml.strSid + "\"");
                            sbResultData.Append(",\"PID\":\"" + entityXml.strPid + "\"");
                            sbResultData.Append(",\"PDESC\":\"" + entityXml.strPDESC + "\"");
                            sbResultData.Append(",\"PDESCCHS\":\"" + entityXml.strPDESCCHS + "\"");
                            sbResultData.Append(",\"PCTRL\":\"" + entityXml.strPCTRLTYPE + "\"");
                            sbResultData.Append(",\"PCTRLID\":\"" + entityXml.strPCTRLID + "\"");
                            sbResultData.Append(",\"PCTRLD\":\"" + entityXml.strPCTRLSQL + "\"");
                            if (entityXml.strPCTRLTYPE.Equals("1") && (!String.IsNullOrEmpty(entityXml.strPCTRLID)))
                            {
                                //下拉框类型的，则拼写SQL语句              
                                TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(entityXml.strPCTRLID);
                                String strWhereBIsstop = "";
                                if (property_LSTD.TABLENAME.Equals("TB_HRLSTD"))
                                {
                                    strWhereBIsstop = " AND isnull(BISSTOP,'0') <> '1' ";//只显示正常使用的项目
                                }
                                String strPListSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "'" + strWhereBIsstop + " ORDER BY " + property_LSTD.ORDER;

                                sbResultData.Append(",\"PListSQL\":\"" + strPListSql + "\"");
                            }
                            else if (entityXml.strPCTRLTYPE.Equals("2") && (!String.IsNullOrEmpty(entityXml.strPCTRLID)) && (!String.IsNullOrEmpty(entityXml.strPCTRLSQL)))
                            {
                                sbResultData.Append(",\"PListSQL\":\"" + entityXml.strPCTRLSQL + "\"");
                            }else{
                                sbResultData.Append(",\"PListSQL\":\"\"");
                            }
                        }
                        sbResultData.Append("}");
                        sbResultData.Append(",\"needInput\":\"" + strIsNeedInput + "\"");
                        sbResultData.Append(",\"existsXml\":\"" + strIsExistsXml + "\"");
                        sbResultData.Append("}");
                        //sbResultData.Append("}");
                    }
                }
                sbResultData.Append("]");
            }

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 执行存储过程类型的动作
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strAID"></param>
    /// <param name="strSPName"></param>
    /// <param name="strPostJsonData"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void ExecuteSPAction(HttpContext context,String strTID,String strRID,String strSID,String strAID,String strSPName
        ,String strPostJsonData,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "执行存储过程类型的动作";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            int iReturValue = 1;
            MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
            String strExecuteResultMsg = mobileArchiveAction.DoExcuteSPAction(strTID, strRID, strSID, strAID, strSPName, strPostJsonData, strRequestLanguage, strUserCode,ref iReturValue);

            strReturnCode = iReturValue.ToString();
            strReturnMsg = strExecuteResultMsg;
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}