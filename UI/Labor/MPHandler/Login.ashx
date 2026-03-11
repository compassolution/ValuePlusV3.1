<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Labor;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Login : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strAccount = WebCommon.GetJsonValue(strParamJson,"account").ToString();//登录名
        string strPassword = WebCommon.GetJsonValue(strParamJson,"password").ToString();//密码
        string strNewPassword = WebCommon.GetJsonValue(strParamJson,"newpassword").ToString();//新密码
        string strRequestLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();//终端请求时的语言
        string strOpenId = WebCommon.GetJsonValue(strParamJson,"openid").ToString();//密码


        //StringBuilder sbImportParam = new StringBuilder();
        //sbImportParam.Append("param:" + param);
        //sbImportParam.Append(",strAccount:" + strAccount);
        //sbImportParam.Append(",strPassword:" + strPassword);
        //sbImportParam.Append(",strNewPassword:" + strNewPassword);
        //sbImportParam.Append(",strRequestLanguage:" + strRequestLanguage);
        //log.Error("Labor/MPHandler/Login.ashx传入参数:" + sbImportParam.ToString());

        switch (param.ToLower().ToString())
        {
            case "login":
                this.JudgeLogin(context, strAccount, strPassword,strOpenId);
                break;
            case "changepassword":
                this.ChangePassword(context, strAccount, strPassword, strNewPassword);
                break;
            case "getuserinfo":
                this.GetUserInfoByUserCode(context, strAccount);
                break;
            case "logout":
                this.DoLogout(context, strUserCode);
                break;
            default:
                break;
        }
    }
        
    /// <summary>
    /// Labor App验证登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccount"></param>
    /// <param name="strPwd"></param>
    /// <param name="strOpenId">如果有OpenId输入，则表示可免密登录</param>
    public void JudgeLogin(HttpContext context, String strAccount, String strPwd,String strOpenId)
    {
        log.Error("Labor App验证登录,登录账号:" + strAccount);
        log.Error("Labor App验证登录,IP地址为:"+this.GetClientIPAddress());
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetUserInfoDataTable(strAccount,strAccount,strAccount,"");
            if (!String.IsNullOrEmpty(strOpenId))
            {
                //如果有OpenId输入，则表示可免密登录
                dt = LUser.GetUserInfoDataTableByOpenId(strOpenId,"");
            }

            //log.Error("Labor App登录名查询数据结果数："+dt.Rows.Count);
            if ((dt != null) && (dt.Rows.Count ==1))
            {
                DataRow dr = dt.Rows[0];

                if ((dr["UserCode"] != null) && (!String.IsNullOrEmpty(dr["UserCode"].ToString())))
                {
                    strAccount = dr["UserCode"].ToString();
                }

                if (!dr["IsValid"].Equals("1"))
                {
                    strReturnCode = "-3";
                    strReturnMsg = "您的账号被禁用";
                }else
                {
                    //如果有OpenId输入，则表示可免密登录,否则需要验证密码正确性
                    if (String.IsNullOrEmpty(strOpenId)&&!dr["password"].Equals(strPwd))
                    {
                        strReturnCode = "-2";
                        strReturnMsg = "密码不正确";
                    }else
                    {
                        //登录成功后的后续操作
                        Hashtable hsTableParams = new Hashtable();
                        hsTableParams.Add("UserCode",dr["UserCode"].ToString());
                        hsTableParams.Add("IsLogin","1");
                        hsTableParams.Add("LoginMode","1");
                        hsTableParams.Add("LoginFlag",strAccount);
                        LUser.DoAfterUserLogin(hsTableParams);

                        sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
                        strReturnCode = "1";
                        strReturnMsg = "用户登录成功";
                    }
                }
            }else
            {
                strReturnCode = "-1";
                strReturnMsg = "账号不存在";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-4";
            strReturnMsg = "用户登录验证时服务器端出错";
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
                sbResult.Append("," + sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error("Labor App验证登录结果："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }
        
    /// <summary>
    /// 退出登录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void DoLogout(HttpContext context, String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();

        StringBuilder sbResult = new StringBuilder();
        try
        {
            Hashtable hsTableParams = new Hashtable();
            hsTableParams.Add("UserCode",strUserCode);
            hsTableParams.Add("IsLogin","0");
            hsTableParams.Add("LoginMode","1");
            hsTableParams.Add("LoginFlag",strUserCode);

            int iCount = LUser.DoAfterUserLogin(hsTableParams);
            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = "退出登录成功";
                sbResultData.Append("\"ResultData\":{\"UserCode\":\""+strUserCode+"\"}");
            }else
            {
                strReturnCode = "-2";
                strReturnMsg = "退出登录失败";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = "退出登录出错";
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
        log.Error("退出登录时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    /// <summary>
    /// //登录成功后,执行存储过程,目前主要是写入应用主机地址
    /// </summary>
    private void ExecuteAfterLogin(String strAccount, String strPwd)
    {
        String strSpName = "p_tb_userlogin";
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("SACCOUNTID", strAccount);
        hsTableParam.Add("pwd", strPwd);
        hsTableParam.Add("guid", Guid.NewGuid().ToString());
        hsTableParam.Add("ip", this.GetClientIPAddress());
        try
        {
            if (!String.IsNullOrEmpty(strSpName))
            {
                SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("登录成功后,执行存储过程失败: SPNAME:" + strSpName);
        }

    }

    /// <summary>
    /// 获取所有用户信息
    /// </summary>
    /// <param name="context"></param>
    public void GetUserInfoByUserCode(HttpContext context, String strAccount)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_Moblie_User] where UserCode = " + JObjectToDB.GetColumnEncryptDataString("LUser_1", "UserCode", strAccount));
            String strSql = sbSql.ToString();

            DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql);

            string json = WebCommon.GetDataRowJsonString(dtResultData, "ResultData", "");
            //json = "2";
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strAccount"></param>
    /// <param name="strOldPassword"></param>
    /// <param name="strNewPassword"></param>
    private void ChangePassword(HttpContext context, String strAccount, String strOldPassword, String strNewPassword)
    {
        String strResult = "0";
        try
        {
            UserManagerBll bllUser = new UserManagerBll();
            int iCount = bllUser.ChangeUserPwd(strNewPassword, strAccount);
            if (iCount > 0)
            {
                //写修改密码记录
                String strSpName = "USP_Sys_RecordPasswordHis";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("UserCode", strAccount);
                hsTableParam.Add("OldPwd", strOldPassword);
                hsTableParam.Add("NewPwd", strNewPassword);

                try
                {
                    iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }


            }
            strResult = iCount.ToString();
        }
        catch (Exception ex)
        {
            strResult = "-1";
            log.Error(ex);
            log.Error("修改密码失败");
        }
        context.Response.Write(strResult);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}