<%@ WebHandler Language="C#" Class="MineHandler" %>

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

public class MineHandler : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonObjectValue(strParamJson, "param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson, "usercode").ToString();//用户名
        string strAimUserCode = WebCommon.GetJsonValue(strParamJson, "aimusercode").ToString();//用户名
        string strOldMobileNo = WebCommon.GetJsonValue(strParamJson, "oldmobileno").ToString();//旧手机号
        string strNewMobileNo = WebCommon.GetJsonValue(strParamJson, "newmobileno").ToString();//新手机号
        string strVerifyCode = WebCommon.GetJsonValue(strParamJson, "verifycode").ToString();//手机短信验证码
        string strUserType = WebCommon.GetJsonValue(strParamJson, "usertype").ToString();//用户类型

        string strOldPassword = WebCommon.GetJsonValue(strParamJson, "oldpassword").ToString();//旧密码
        string strNewPassword = WebCommon.GetJsonValue(strParamJson, "newpassword").ToString();//新密码

        string strPassword = WebCommon.GetJsonValue(strParamJson, "password").ToString();//密码
        string strNickName = WebCommon.GetJsonValue(strParamJson, "nickname").ToString();//用户名
        string strCompanyCode = WebCommon.GetJsonValue(strParamJson, "companycode").ToString();//公司编码
        string strDeptCode = WebCommon.GetJsonValue(strParamJson, "deptcode").ToString();//部门编码
        string strIsNewInfo = WebCommon.GetJsonValue(strParamJson, "isnew").ToString();//是否是新增的标识
        string strNeedOpAccount = WebCommon.GetJsonValue(strParamJson, "needopaccount").ToString();//被操作的账号编码
        string strSetIsValid = WebCommon.GetJsonValue(strParamJson, "setisvalid").ToString();//设置是否有效标识
        string strPostDataObject = WebCommon.GetJsonObjectValue(strParamJson, "postdataobj").ToString();//用户类型

        StringBuilder sbImportParam = new StringBuilder();
        sbImportParam.Append("param:" + param);
        sbImportParam.Append(",strOldMobileNo:" + strOldMobileNo);
        sbImportParam.Append(",strPassword:" + strPassword);
        sbImportParam.Append(",strUserCode:" + strUserCode);
        sbImportParam.Append(",strUserType:" + strUserType);
        sbImportParam.Append(",strCompanyCode:" + strCompanyCode);
        sbImportParam.Append(",strPostDataObject:" + strPostDataObject);
        log.Error("Labor/MPHandler/MineHandler.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "changemobileno":
                this.ChangeMobileNo(context,strAimUserCode, strOldMobileNo,strNewMobileNo,strVerifyCode, strUserCode);
                break;
            case "changepassword":
                this.ChangePassword(context,strAimUserCode, strOldPassword,strNewPassword, strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 修改登录密码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAimUserCode">需修改的目标用户编码</param>
    /// <param name="strOldPassword"></param>
    /// <param name="strNewPassword"></param>
    /// <param name="strOpUserCode">当前操作的用户编码</param>
    public void ChangePassword(HttpContext context,String strAimUserCode,String strOldPassword,String strNewPassword,String strOpUserCode)
    {
        log.Error("修改登录密码传入参数:" + strAimUserCode.ToString()+",to mobile no:"+strNewPassword);
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            int iCount = LUser.ChangePasswordByUserCode(strAimUserCode,strOldPassword,strNewPassword,strOpUserCode);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "修改登录密码成功";
                sbResultData.Append("\"ResultData\":{");
                sbResultData.Append("   \"AimUserCode\":\"" + strAimUserCode + "\"");
                sbResultData.Append("   ,\"OldPassword\":\"" + strOldPassword + "\"");
                sbResultData.Append("   ,\"NewPassword\":\"" + strNewPassword + "\"");
                sbResultData.Append("   ,\"OpUserCode\":\"" + strOpUserCode + "\"");
                sbResultData.Append("}");
            }
            else
            {
                strReturnCode = "-2";
                strReturnMsg = "修改登录密码失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "修改登录密码出错,请稍候重试";
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
        log.Error("修改登录密码时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// 修改手机号
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAimUserCode">需修改的目标用户编码</param>
    /// <param name="strOldMobileNo"></param>
    /// <param name="strNewMobileNo"></param>
    /// <param name="strVerifyCode"></param>
    /// <param name="strOpUserCode">当前操作的用户编码</param>
    public void ChangeMobileNo(HttpContext context,String strAimUserCode,String strOldMobileNo,String strNewMobileNo,String strVerifyCode,String strOpUserCode)
    {
        log.Error("修改手机号传入参数:" + strAimUserCode.ToString()+",to mobile no:"+strNewMobileNo);
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            String strUserCode = LUser.GetUserCodeByMobileNo(strNewMobileNo);
            if (!String.IsNullOrEmpty(strUserCode))
            {
                strReturnCode = "-1";
                strReturnMsg = "该手机号已注册";
            }else
            {
                int iCount = LUser.ChangeChangeMobileNo(strAimUserCode, strOldMobileNo, strNewMobileNo, strOpUserCode);
                if (iCount > 0)
                {
                    strReturnCode = "1";
                    strReturnMsg = "修改手机号成功";
                    sbResultData.Append("\"ResultData\":{");
                    sbResultData.Append("   \"AimUserCode\":\"" + strAimUserCode + "\"");
                    sbResultData.Append("   ,\"OldMobileNo\":\"" + strOldMobileNo + "\"");
                    sbResultData.Append("   ,\"NewMobileNo\":\"" + strNewMobileNo + "\"");
                    sbResultData.Append("   ,\"VerifyCode\":\"" + strVerifyCode + "\"");
                    sbResultData.Append("   ,\"OpUserCode\":\"" + strOpUserCode + "\"");
                    sbResultData.Append("}");
                }
                else
                {
                    strReturnCode = "-2";
                    strReturnMsg = "修改手机号失败";
                }
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "修改手机号出错,请稍候重试";
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
        log.Error("修改手机号时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}