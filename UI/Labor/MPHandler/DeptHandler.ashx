<%@ WebHandler Language="C#" Class="DeptHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Labor;
using Com.ValuePlus.Web;

public class DeptHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonObjectValue(strParamJson,"param").ToString();//请求类型参数
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();//登录名
        string strPassword = WebCommon.GetJsonValue(strParamJson,"password").ToString();//密码
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户名
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码
        string strDeptCode = WebCommon.GetJsonValue(strParamJson,"deptcode").ToString();//部门编码
        string strNeedOpAccount = WebCommon.GetJsonValue(strParamJson,"needopaccount").ToString();//被操作的账号编码
        string strSetIsValid = WebCommon.GetJsonValue(strParamJson,"setisvalid").ToString();//设置是否有效标识
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//用户类型

        switch (param.ToLower().ToString())
        {
            case "savedept":
                this.SaveDeptInfo(context, strCompanyCode, strDeptCode, strPostDataObject);
                break;
            case "deletedept":
                this.DeleteDeptInfo(context,strCompanyCode,strDeptCode,strSetIsValid,strUserCode);
                break;
            case "getdeptlist":
                this.GetCompanyDeptList(context,strCompanyCode);
                break;
            case "getdeptandaccount":
                this.GetCompanyDeptAndAccountInfo(context,strCompanyCode);
                break;
            case "saveaccount":
                this.SaveAccountInfo(context,strUserCode,strPostDataObject);
                break;
            case "deleteaccount":
                this.DeleteAccountInfo(context,strUserCode,strNeedOpAccount,strSetIsValid);
                break;
        }
    }

    /// <summary>
    /// 通过公司编码获取公司所有部门及账号信息数据表集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    public void GetCompanyDeptAndAccountInfo(HttpContext context,String strCompanyCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            sbResultData.Append(LCompany.GetDeptAndAccountJsonData(strCompanyCode,"",false));

            //尚未分配部门的账号
            DataTable dtUser_NoDept = LUser.GetUserInfoDataTableByDept(strCompanyCode, "", "");
            sbResultData.Append("," + WebCommon.GetJsonStringByDataTable(dtUser_NoDept, "\"NoDeptAccountList\"", false));

            strReturnCode = "1";
            strReturnMsg = "通过公司编码获取公司所有部门及账号信息数据表集合成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过公司编码获取公司所有部门及账号信息数据表集合出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("通过公司编码获取公司所有部门及账号信息数据表集合时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存账号信息（由管理员操作非自行注册）
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strOPUserCode"></param>
    /// <param name="strPostDataObject"></param>
    public void SaveAccountInfo(HttpContext context, String strOPUserCode,String strPostDataObject)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string strTableName = "LUser_1";
            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);
            String strNeedSaveAccount = jo["UserCode"].ToString();
            String strCompanyCode = jo["CompanyCode"].ToString();
            String strDeptCode = jo["DeptCode"].ToString();
            String strUserName = jo["UserName"].ToString();
            String strUserType = jo["UserType"].ToString();
            String strUserRole = jo["UserRole"].ToString();
            String strMobileNo = jo["MobileNo"].ToString();
            String strPassword = jo["Password"].ToString();
            String strPosiName = jo["PosiName"].ToString();
            String strIsAdministrator = jo["IsAdministrator"].ToString();

            Hashtable hsTableKey = new Hashtable();

            int iCount = 0;
            if (String.IsNullOrEmpty(strNeedSaveAccount))
            {
                log.Error("新增账号SaveAccountInfo:"+strPostDataObject);
                //新增账号
                strNeedSaveAccount = LUser.GenerateUserCode(strUserType);
                hsTableKey.Add("UserCode", strNeedSaveAccount);

                iCount = LUser.InsertOneRegUser(strNeedSaveAccount,strMobileNo,strPassword,strUserType,strUserRole,strIsAdministrator,strUserName,strPosiName,strCompanyCode,strDeptCode,strOPUserCode);
            }else
            {
                log.Error("修改账号信息SaveAccountInfo:"+strPostDataObject);
                hsTableKey.Add("UserCode", strNeedSaveAccount);
                //修改账号信息
                iCount = JObjectToDB.UpdateJObjectDataToTable(jo, strTableName, hsTableKey);
            }


            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "保存账号成功:"+strNeedSaveAccount;
                sbResultData.Append("\"ResultData\":{\"UserCode\":\""+strNeedSaveAccount+"\"}");
            }else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存账号失败,请稍候重试";
            }

        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存账号时服务器端出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("保存账号时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除账号信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strNeedOpAccount"></param>
    /// <param name="strSetIsValid"></param>//为空时直接删除
    public void DeleteAccountInfo(HttpContext context,String strUserCode,String strNeedOpAccount,String strSetIsValid)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LUser.DeleteAccountInfo(strUserCode,strNeedOpAccount,strSetIsValid);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "删除账号信息成功";
                sbResultData.Append("\"ResultData\":{\"UserCode\":\"" + strNeedOpAccount + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "删除账号信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "删除账号信息出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("删除账号信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 删除公司部门信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strDeptCode"></param>
    /// <param name="strSetIsValid"></param>//为空时直接删除
    /// <param name="strUserCode"></param>
    public void DeleteDeptInfo(HttpContext context,String strCompanyCode,String strDeptCode,String strSetIsValid,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LCompany.DeleteDeptInfo(strCompanyCode,strDeptCode,strSetIsValid,strUserCode);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "删除公司部门信息成功";
                sbResultData.Append("\"ResultData\":{\"CompanyCode\":\"" + strCompanyCode + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "删除公司部门信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "删除公司部门信息出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("删除公司部门信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存公司部门信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strDeptCode"></param>
    /// <param name="strPostDataObject"></param>
    public void SaveDeptInfo(HttpContext context,String strCompanyCode,String strDeptCode ,String strPostDataObject)
    {
        log.Error("保存公司部门信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string strTableName = "LCompany_2";

            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("CompanyCode", strCompanyCode);
            hsTableKey.Add("DeptCode", strDeptCode);

            int iCount = 0;
            //判断这个主键是否存在，如果
            if (JObjectToDB.JudgeRecordIsExists(strTableName, hsTableKey))
            {
                iCount = JObjectToDB.UpdateJObjectDataToTable(jo, strTableName, hsTableKey);
            }else
            {
                iCount = JObjectToDB.InsertJObjectDataToTable(jo, strTableName, hsTableKey);
            }

            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "保存公司部门信息成功";
                sbResultData.Append("\"ResultData\":{\"CompanyCode\":\"" + strCompanyCode + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存公司部门信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存公司部门信息出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("保存公司部门信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 通过公司编码获取公司所有部门的信息数据表集合
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    public void GetCompanyDeptList(HttpContext context,String strCompanyCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LCompany.GetDeptInfoDataTable(strCompanyCode, "");

            strReturnCode = "1";
            strReturnMsg = "通过公司编码获取公司所有部门的信息数据表集合成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过公司编码获取公司所有部门的信息数据表集合出错,请稍候重试";
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        log.Error("通过公司编码获取公司所有部门的信息数据表集合时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}