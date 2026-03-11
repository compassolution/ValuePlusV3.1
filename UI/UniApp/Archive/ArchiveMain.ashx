<%@ WebHandler Language="C#" Class="ArchiveMain" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class ArchiveMain : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        string strPID = WebCommon.GetJsonValue(strParamJson,"pid").ToString();
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

        String strPageSize = WebCommon.GetJsonValue(strParamJson,"pagesize").ToString();
        String strPageIndex = WebCommon.GetJsonValue(strParamJson,"pageindex").ToString();
        int iPageSize = String.IsNullOrEmpty(strPageSize)? 10 : int.Parse(strPageSize);
        int iPageIndex = String.IsNullOrEmpty(strPageSize)? 0 : int.Parse(strPageIndex);

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getroledatalist"://获取特定模板的角色的列表数据
                this.GetRoleDataList(context,strTID,strRID,strIsLoadAllRole,strUserCode);
                break;
            case "getscenedatalist"://获取特定模板特定角色下的场景列表数据
                this.GetSceneDataList(context,strTID,strRID,strSID,strUserCode);
                break;
            case "getbusinessdatalist"://获取特定模板特定角色特定场景下的业务数据
                this.GetBusinessDataList(context,strTID,strRID,strSID,strCondition,iPageSize,iPageIndex,strRequestLanguage, strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取特定模板的角色列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strIsLoadAllRole">是否加载所有角色[1是/0否]</param>
    /// <param name="strUserCode"></param>
    public void GetRoleDataList(HttpContext context,String strTID,String strRID,String strIsLoadAllRole,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取特定模板的角色列表数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            //获取特定模板的角色列表数据
            String strJsonData_RoleList = mobileArchive.GetJsonData_RoleList(strUserCode,strTID,strRID,strIsLoadAllRole);
            //log.Error(strMethodDesc+":"+strJsonData_RoleList);

            if(!String.IsNullOrEmpty(strJsonData_RoleList)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData_RoleList);
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
    /// 获取特定模板特定角色下的场景列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strUserCode"></param>
    public void GetSceneDataList(HttpContext context,String strTID,String strRID,String strSID,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取特定模板特定角色下的场景列表数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            //获取特定模板特定角色下的场景列表数据
            String strJsonData_SceneList = mobileArchive.GetJsonData_SceneList(strUserCode,strTID,strRID,strSID);
            //log.Error(strMethodDesc+":"+strJsonData_SceneList);

            if(!String.IsNullOrEmpty(strJsonData_SceneList)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData_SceneList);
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
    /// 获取特定模板特定角色特定场景下的业务数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strSID"></param>
    /// <param name="strCondition"></param>
    /// <param name="strRequestLanguage"></param>
    /// <param name="strUserCode"></param>
    public void GetBusinessDataList(HttpContext context,String strTID,String strRID,String strSID,String strCondition,int iPageSize,int iPageIndex,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取特定模板特定角色特定场景下的业务数据";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            MobileArchive mobileArchive = new MobileArchive();
            //获取特定模板特定角色特定场景下的业务数据
            String strJsonData_BusinessDataList = mobileArchive.GetJsonData_BusinessDataList(strUserCode,strTID,strRID,strSID,strCondition,iPageSize,iPageIndex,strRequestLanguage);
            //log.Error(strMethodDesc+":"+strJsonData_BusinessDataList);

            if(!String.IsNullOrEmpty(strJsonData_BusinessDataList)){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"获取数据失败";
            }
            sbResultData.Append(strJsonData_BusinessDataList);

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