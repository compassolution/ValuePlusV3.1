using Com.ValuePlus.DAL;
using Com.ValuePlus.Labor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;

/// <summary>
/// LaborService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class LaborService : System.Web.Services.WebService
{
    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public LaborService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 接口测试
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public String TestService()
    {
        String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String strReturn = "0";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select count(1) from (select top 1 * from [LUser_1] A) a");
            String strSql = sbSql.ToString();

            log.Error("LaborService 接口测试，Sql:" + strSql);
            strReturn = SqlParamDao.ExecuteScalarBySql(strSql).ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            strReturn = "-1";
        }
        return strReturn;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="strAccount"></param>
    /// <param name="strPwd"></param>
    /// <returns></returns>
    [WebMethod]
    public String DoLogin(String strAccount,String strPwd)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        try
        {
            DataTable dt = LUser.GetUserInfoDataTable(strAccount, strAccount, strAccount, "");

            //log.Error("Labor App登录名查询数据结果数："+dt.Rows.Count);
            if ((dt != null) && (dt.Rows.Count == 1))
            {
                DataRow dr = dt.Rows[0];

                if ((dr["UserCode"] != null) && (!String.IsNullOrEmpty(dr["UserCode"].ToString())))
                {
                    strAccount = dr["UserCode"].ToString();
                }

                if (!dr["IsValid"].Equals("1"))
                {
                    strReturnCode = "-3";
                    strReturnMsg = "您的账号被禁用,请联系单位管理员";
                }
                else
                {
                    //如果有OpenId输入，则表示可免密登录,否则需要验证密码正确性
                    if (!dr["password"].Equals(strPwd))
                    {
                        strReturnCode = "-2";
                        strReturnMsg = "密码不正确";
                    }
                    else
                    {
                        sbResultData.Append(CommonJson.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
                        strReturnCode = "1";
                        strReturnMsg = "用户登录成功";
                    }
                }
            }
            else
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
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");
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
        return sbResult.ToString();
    }


    /// <summary>
    /// 发送订阅消息【工单状态变更时】
    /// </summary>
    /// <param name="strWONO"></param>
    /// <param name="strStatusTo"></param>
    /// <param name="IsReload">是否重新获取需推送记录【字符串1或者true时需重新获取】</param>
    /// <returns></returns>
    [WebMethod]
    public String SendWOPendingMsg(String strWONO, String strStatusTo, String IsReload)
    {
        String strNowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        String strReturn = "1";
        log.Error("调用WebService发送订阅消息,strWONO:" + strWONO + ";strStatusTo:" + strStatusTo + ";IsReload:" + IsReload);
        try
        {
            SendWXMsg.SendWOPendingMsg(strWONO, strStatusTo, IsReload);
        }
        catch (Exception ex)
        {
            log.Error("调用WebService发送订阅消息出错,strWONO:" + strWONO + ";strStatusTo:" + strStatusTo);
            log.Error(ex);
            strReturn = "-1";
        }
        return strReturn;
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

}
