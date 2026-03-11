<%@ WebHandler Language="C#" Class="CompanyHandler" %>

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

public class CompanyHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonObjectValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户名
        string strUserType = WebCommon.GetJsonValue(strParamJson,"usertype").ToString();//用户类型
        string strAimUserType = WebCommon.GetJsonValue(strParamJson,"aimusertype").ToString();//目标用户类型
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson,"companycode").ToString();//公司编码
        string strIsValid = WebCommon.GetJsonValue(strParamJson,"isvalid").ToString();//是否有效
        string strFocusType = WebCommon.GetJsonValue(strParamJson,"focustype").ToString();//关注类型
        string strIsFocus = WebCommon.GetJsonValue(strParamJson,"isfocus").ToString();//是否是关注
        int iTopRows = String.IsNullOrEmpty(WebCommon.GetJsonValue(strParamJson,"toprows").ToString())?0:int.Parse(WebCommon.GetJsonValue(strParamJson,"toprows").ToString());//获取前多少条记录
        string strOrderBy = WebCommon.GetJsonObjectValue(strParamJson,"orderby").ToString();//排序
        string strConditionSql = WebCommon.GetJsonObjectValue(strParamJson,"conditionsql").ToString();//查询条件语句
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson,"postdataobj").ToString();//


        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        sbImportParam.Append(",strUserType:" + strUserType);
        sbImportParam.Append(",strCompanyCode:" + strCompanyCode);
        sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        log.Error("Labor/MPHandler/Partner.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "regcompany":
                this.RegistCompanyInfo(context,strUserCode,strUserType,strCompanyCode, strPostDataObject);
                break;
            case "getcompanyinfo":
                this.GetCompanyInfo(context,strCompanyCode);
                break;
            case "getcompanylist":
                this.GetCompanyList(context,strUserCode,strConditionSql,iTopRows,strOrderBy);
                break;
            case "getmypartnercompanylist":
                this.GetMyPartnerCompanyList(context,strUserCode,strAimUserType,strFocusType,strIsValid,strConditionSql,iTopRows,strOrderBy);
                break;
            case "getmypartnercompanycodearray":
                //仅返回公司编码数组
                this.GetMyPartnerCompanyCodeArray(context,strUserCode,strAimUserType,strFocusType,strIsValid,strConditionSql,iTopRows,strOrderBy);
                break;
            case "focusoutsourced":
                this.FocusOutsourced(context, strUserCode, strCompanyCode,strIsFocus);
                break;
            case "getcompanydiningtimeinfo":
                //通过公司编码获取公司就餐时间信息数据
                this.GetCompanyDiningTimeInfo(context,strCompanyCode);
                break;
            case "savecompanydiningtimedata":
                //保存公司就餐时间信息数据
                this.SaveCompanyDiningTimeData(context,strUserCode,strCompanyCode,strPostDataObject);
                break;
        }

    }


    /// <summary>
    /// 用人单位添加关注/取消关注某一外包公司
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strCompanyCode"></param>
    /// <param name="strIsFocus"></param>
    public void FocusOutsourced(HttpContext context,String strUserCode,String strCompanyCode, String strIsFocus)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode", strUserCode);//当前用户
            hsTableParams.Add("BeObjectType", "2");//被关注对象类型
            hsTableParams.Add("BeObjectCode", strCompanyCode);//被关注对象编码
            hsTableParams.Add("FocusType", "2");//关注类型（1、合作/2主动关注）
            hsTableParams.Add("IsFocus", strIsFocus);//是否是关注（1、关注，0,、取消关注）

            int iCount = LUser.DoFocusObject(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "用人单位添加关注/取消关注某一外包公司成功";
                sbResultData.Append("\"ResultData\":{\"UserCode\":\"" + strUserCode + "\"");
                sbResultData.Append("   ,\"CompanyCode\":\"" + strCompanyCode + "\"");
                sbResultData.Append("   ,\"IsFocus\":\"" + strIsFocus + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "用人单位添加关注/取消关注某一外包公司失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "用人单位添加关注/取消关注某一外包公司出错,请稍候重试";
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
        log.Error("用人单位添加关注/取消关注某一外包公司时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 加载已关注的外包服务公司数组,仅返回公司编码数组
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strAimUserType"></param>
    /// <param name="strFocusType"></param>
    /// <param name="strIsValid"></param>
    /// <param name="strCondition"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetMyPartnerCompanyCodeArray(HttpContext context,String strUserCode,String strAimUserType,String strFocusType,String strIsValid,String strCondition,int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            //log.Error("GetMyPartnerCompanyCodeArray返回公司编码数组，strUserCode："+strUserCode+";strAimUserType:"+strAimUserType+";strFocusType:"+strFocusType);
            sbResultData.Append("\"ResultData\":[");
            //DataTable dt = LCompany.GetMyPartnerCompanyDataTable(strUserCode,strAimUserType,strFocusType,strIsValid,"",0,"");
            DataTable dt = LCompany.GetMyPartnerCompanyDataTable(strUserCode,strAimUserType,strFocusType,strIsValid,strCondition,iTopRows,strOrderBy);
            if (dt != null & dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    String strOutsourcedCode = dr["CompanyCode"].ToString();
                    if (i == 0)
                    {
                        sbResultData.Append("\"" + strOutsourcedCode + "\"");
                    }else
                    {
                        sbResultData.Append(",\"" + strOutsourcedCode + "\"");
                    }
                }
            }
            sbResultData.Append("]");

            strReturnCode = "1";
            strReturnMsg = "加载已关注的外包服务公司数组,仅返回公司编码数组成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "加载已关注的外包服务公司数组,仅返回公司编码数组出错,请稍候重试";
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
        log.Error("加载已关注的外包服务公司数组,仅返回公司编码数组时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 根据用户编码获取我的伙伴公司列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strAimUserType">目标公司对象类型</param>
    /// <param name="strFocusType">关注类型仅我是用人单位是有效</param>
    /// <param name="strIsValid"></param>
    /// <param name="strCondition"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetMyPartnerCompanyList(HttpContext context,String strUserCode,String strAimUserType,String strFocusType,String strIsValid,String strCondition,int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LCompany.GetMyPartnerCompanyDataTable(strUserCode,strAimUserType,strFocusType,strIsValid,strCondition,iTopRows,strOrderBy);

            strReturnCode = "1";
            strReturnMsg = "根据用户编码获取我的伙伴公司列表成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据用户编码获取我的伙伴公司列表出错,请稍候重试";
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
        log.Error("根据用户编码获取我的伙伴公司列表时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 保存公司信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPostDataObject"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strUserType"></param>
    public void RegistCompanyInfo(HttpContext context,String strUserCode,String strUserType,String strCompanyCode,String strPostDataObject)
    {
        log.Error("保存公司信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            bool IsRegCompany = false;
            if (String.IsNullOrEmpty(strCompanyCode))
            {
                //如果公司编码不存在，则自动生成一个新编码
                strCompanyCode = LCompany.GenerateCompanyCode(strUserType);
                IsRegCompany = true;
            }
            string strTableName = "LCompany_1";

            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("CompanyCode", strCompanyCode);

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
                if (IsRegCompany)
                {
                    //成功后同时将此公司和当前用户关联
                    LUser.ConnectUserToCompany(strUserCode,strCompanyCode);
                }

                //保存公司信息后的后续操作
                Hashtable hsTableParams = new Hashtable();
                hsTableParams.Add("RegCompanyCode",strCompanyCode);
                hsTableParams.Add("OpUserCode",strUserCode);
                LCompany.DoAfterRegCompany(hsTableParams);

                strReturnCode = "1";
                strReturnMsg = "保存公司信息成功";
                sbResultData.Append("\"ResultData\":{\"CompanyCode\":\"" + strCompanyCode + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存公司信息失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存公司信息出错,请稍候重试";
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
        log.Error("保存公司信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
        
    /// <summary>
    /// 保存就餐时间信息数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strPostDataObject"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strUserType"></param>
    public void SaveCompanyDiningTimeData(HttpContext context,String strUserCode,String strCompanyCode,String strPostDataObject)
    {
        log.Error("保存就餐时间信息的数据："+strPostDataObject.ToString());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            string strTableName = "LCompany_4";

            //json字符串转json的JObject对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(strPostDataObject);

            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add("CompanyCode", strCompanyCode);

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
                strReturnMsg = "保存就餐时间信息数据成功";
                sbResultData.Append("\"ResultData\":{\"CompanyCode\":\"" + strCompanyCode + "\"}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "保存就餐时间信息数据失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "保存就餐时间信息数据出错,请稍候重试";
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
        log.Error("保存就餐时间信息数据时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 根据公司编码获取公司信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    public void GetCompanyInfo(HttpContext context,String strCompanyCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LCompany.GetCompanyInfoDataTable(strCompanyCode,"");

            strReturnCode = "1";
            strReturnMsg = "根据公司编码获取公司信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据公司编码获取公司信息出错,请稍候重试";
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
        log.Error("根据公司编码获取公司信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
        
    /// <summary>
    /// 通过公司编码获取公司就餐时间信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strCompanyCode"></param>
    public void GetCompanyDiningTimeInfo(HttpContext context,String strCompanyCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LCompany.GetDiningTimeInfoDataTable(strCompanyCode);

            strReturnCode = "1";
            strReturnMsg = "通过公司编码获取公司就餐时间信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "通过公司编码获取公司就餐时间信息出错,请稍候重试";
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
        log.Error("通过公司编码获取公司就餐时间信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 根据相应查询条件获取公司信息
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    /// <param name="strConditionSql"></param>
    /// <param name="iTopRows"></param>
    /// <param name="strOrderBy"></param>
    public void GetCompanyList(HttpContext context,String strUserCode,String strConditionSql, int iTopRows,String strOrderBy)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LCompany.GetCompanyInfoDataTableFromCondition(strUserCode,strConditionSql,iTopRows,strOrderBy);

            strReturnCode = "1";
            strReturnMsg = "根据相应查询条件获取公司信息成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "根据相应查询条件获取公司信息出错,请稍候重试";
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
        log.Error("根据相应查询条件获取公司信息时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}