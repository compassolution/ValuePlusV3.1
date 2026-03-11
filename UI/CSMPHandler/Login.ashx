<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Security;

public class Login : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);

        //string param = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//请求类型参数
        //string strUserID = hsTableUrlQuery["userid"] == null ? string.Empty : hsTableUrlQuery["userid"].ToString();//登录名
        //string strPassword = hsTableUrlQuery["password"] == null ? string.Empty : hsTableUrlQuery["password"].ToString();//密码
        //string strRequestLanguage = hsTableUrlQuery["language"] == null ? string.Empty : hsTableUrlQuery["language"].ToString();//终端请求时的语言

        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//请求类型参数
        string strUserID = context.Request["userid"] == null ? string.Empty : context.Request["userid"].ToString();//登录名
        string strPassword = context.Request["password"] == null ? string.Empty : context.Request["password"].ToString();//密码
        string strNewPassword = context.Request["newpassword"] == null ? string.Empty : context.Request["newpassword"].ToString();//新密码
        string strRequestLanguage = context.Request["language"] == null ? string.Empty : context.Request["language"].ToString();//终端请求时的语言
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strUserID = SQLInjectionDefense.ReplaceSQLReservedKeyword(strUserID);
        strPassword = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPassword);
        strNewPassword = SQLInjectionDefense.ReplaceSQLReservedKeyword(strNewPassword);
        strRequestLanguage = SQLInjectionDefense.ReplaceSQLReservedKeyword(strRequestLanguage);

        switch (param.ToLower().ToString())
        {
            case "cslogin":
                this.JudgeLogin(context, strUserID, strPassword);
                break;
        }
    }

    #region 验证登录
    /// <summary>
    /// 验证登录
    /// </summary>
    public void JudgeLogin(HttpContext context, String strUserId, String strPwd)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from WXUser_1 where MobileNo = '"+strUserId+"'");//员工编号
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count >1))
            {
                if(dt.Rows.Count>1){
                    strReturnCode = "-2";
                    strReturnMsg = "手机号进行了多次注册绑定！";
                }else{
                    DataRow dr = dt.Rows[0];
                    if (!dr["Password"].Equals(strPwd))
                    {
                        strReturnCode = "-3";
                        strReturnMsg = "输入密码错误,请重新输入！";
                    }else
                    {
                        strReturnCode = "1";
                        strReturnMsg = "登录成功";
                        sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",false));
                    }
                }
            }else
            {
                strReturnCode = "-1";
                strReturnMsg = "手机号尚未注册绑定";
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
    #endregion



    public bool IsReusable {
        get {
            return false;
        }
    }

}