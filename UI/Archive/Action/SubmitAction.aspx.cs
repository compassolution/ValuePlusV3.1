using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Common.Security;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.DataLog;
using Org.BouncyCastle.Asn1.Ocsp;
using Com.ValuePlus.Utils;

public partial class Archive_Action_SubmitAction : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "showWaitingDiv", "<script language=\"javascript\">ShowWaitingDiv();</script>");
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "saveCheckedBox", "<script language=\"javascript\">saveCheckedBox();</script>");//先保存列表中复选框
        if (!Page.IsPostBack)
        {
            try
            {
                this.strSPName = Request.Params["SP"] == null ? "" : Request.Params["SP"].ToString();
                this.strTID = Request.Params["TID"] == null ? "" : Request.Params["TID"].ToString();
                this.strRID = Request.Params["RID"] == null ? "" : Request.Params["RID"].ToString();
                this.strSID = Request.Params["SID"] == null ? "" : Request.Params["SID"].ToString();
                this.strAID = Request.Params["AID"] == null ? "" : Request.Params["AID"].ToString();
                this.strActionMoveNextFlag = Request.Params["MOVENEXT"] == null ? "" : Request.Params["MOVENEXT"].ToString();
                this.strKeyValue = Request.Params["KEYVALUE"] == null ? "" : Request.Params["KEYVALUE"].ToString();

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strSPName = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSPName);
                this.strTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTID);
                this.strSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSID);
                this.strAID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strAID);
                this.strActionMoveNextFlag = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strActionMoveNextFlag);
                this.strKeyValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strKeyValue);

                if (!String.IsNullOrEmpty(this.strSPName))
                {
                    this.hsTableRequestParam = this.GetUrlAnalyse();
                    this.DoExcuteSP(this.strSPName, this.hsTableRequestParam);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    #region viewstate初始化区域
    private String strSPName
    {
        get
        {
            return ViewState["strSPName_ViewState"] as String;
        }
        set
        {
            ViewState["strSPName_ViewState"] = value;
        }
    }
    private String strTID
    {
        get
        {
            return ViewState["strTID_ViewState"] as String;
        }
        set
        {
            ViewState["strTID_ViewState"] = value;
        }
    }
    private String strRID
    {
        get
        {
            return ViewState["strRID_ViewState"] as String;
        }
        set
        {
            ViewState["strRID_ViewState"] = value;
        }
    }
    private String strSID
    {
        get
        {
            return ViewState["strSID_ViewState"] as String;
        }
        set
        {
            ViewState["strSID_ViewState"] = value;
        }
    }
    private String strAID
    {
        get
        {
            return ViewState["strAID_ViewState"] as String;
        }
        set
        {
            ViewState["strAID_ViewState"] = value;
        }
    }
    private String strActionMoveNextFlag
    {
        get
        {
            return ViewState["strActionMoveNextFlag_ViewState"] as String;
        }
        set
        {
            ViewState["strActionMoveNextFlag_ViewState"] = value;
        }
    }   
    private String strKeyValue
    {
        get
        {
            return ViewState["ActionSubmit_KeyValue_ViewState"] as String;
        }
        set
        {
            ViewState["ActionSubmit_KeyValue_ViewState"] = value;
        }
    }
    private Hashtable hsTableRequestParam
    {
        get
        {
            return ViewState["Query_hsTableRequestParam_ViewState"] as Hashtable;
        }
        set
        {
            ViewState["Query_hsTableRequestParam_ViewState"] = value;
        }
    }
    #endregion

    #region 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// <summary>
    /// 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// </summary>
    /// <returns>Hashtable</returns>
    private Hashtable GetUrlAnalyse()
    {
        String strArr = "SP&TID&RID&SID&AID&MOVENEXT";
        String strUrl = Server.UrlDecode(base.Request.Url.Query.ToString().Replace("?", ""));
        Hashtable hsTable = new Hashtable();
        if (!String.IsNullOrEmpty(strUrl))
        {
            int num = 20;

            String[] strArray = new String[num];
            if (strUrl.IndexOf("&") > 0)
            {
                String[] strArray2 = strUrl.Split('&');
                for (int i = 0; i < strArray2.Length; i++)
                {
                    String[] strArray3 = strArray2[i].Split('=');
                    if (strArray3.Length == 2)
                    {
                        if (strArr.IndexOf(strArray3[0])<0)
                        {
                            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                            String strValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strArray3[1]);
                            hsTable.Add(strArray3[0], strValue);
                        }
                    }
                }
            }
            else
            {
                String[] strArray4 = strUrl.Split('=');
                if (strArray4.Length == 2)
                {
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    String strValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strArray4[1]);
                    hsTable.Add(strArray4[0], strValue);
                }
            }
        }
        return hsTable;
    }
    #endregion

    #region 执行存储过程事件
    /// <summary>
    /// 执行存储过程事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DoExcuteSP_Click(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "saveCheckedBox", "<script language=\"javascript\">saveCheckedBox();</script>");//先保存列表中复选框
        this.DoExcuteSP(this.strSPName, this.hsTableRequestParam);
    }
    /// <summary>
    /// 执行动作存储过程
    /// <param name="hsTableParam"></param>
    /// <param name="strSpName"></param>
    /// </summary>
    private void DoExcuteSP(String strSpName, Hashtable hsTableParam)
    {
        int iCount = ArchiveActionBll.DoExcuteSP(strSpName, hsTableParam);
        this.SetPageTipAfterExcuteSp(iCount);
        //执行动作的日志写入 add by sammen 20250304
        DataLogWriter.Log_ExecuteAction(this.GetUserCode(), RequestUtils.GetIP(),this.strTID, this.strSID, this.strAID, strKeyValue, strSpName, hsTableParam,(iCount>0?true:false));

    }
    #endregion

    #region 根据执行存储过程的结果返回页面提示
    /// <summary>
    /// 根据执行存储过程的结果返回页面提示
    /// </summary>
    private void SetPageTipAfterExcuteSp(int iReturn)
    {
        String strMsgName = "";
        String strMsg = "";
        if (this.Language == "zh-cn")
        {
            strMsgName = "MESSCHS";
        }
        else
        {
            strMsgName = "MESSENG";
        }
        String strSql = "SELECT " + strMsgName + " FROM TB_HRTMPAR WHERE TID='" + this.strTID + "' AND AID='" + this.strAID + "' AND ECFROM<=" + iReturn + " AND ECTO>=" + iReturn;
        DataSet set = SqlParamDao.GetDataSetBySql(strSql);
        if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
        {
            strMsg = set.Tables[0].Rows[0][0].ToString().Replace("@S@", iReturn.ToString());
        }
        else
        {
            strMsg = "操作执行成功！";
        }
        String strExecJsScripts = "reloadOpenerPages('" + strMsg + "','" + this.strActionMoveNextFlag + "');";
        //add by sammen 20240428 根据FLUser_6的配置进行短信发送或者公众号推送
        if (iReturn>-1)
        {
            //动作执行成功，则根据FLUser_6的配置进行短信发送
            Com.ValuePlus.Archive.Flow.FLUserSMS.SendSMSAfterActionExecute(this.strTID, this.strRID, this.strSID, this.strAID, this.strKeyValue, "1");
            //动作执行成功，则根据FLUser_6的配置进行公众号的推送【先调用客户端脚本以便去调用远程公众号服务器】
            strExecJsScripts = strExecJsScripts + "pushMsgAfterAction('" + this.strTID + "','" + this.strRID + "','" + this.strSID + "','" + this.strAID + "','"+ this.strKeyValue + "','" + this.GetUserCode() + "');";
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "doActionSuccess", "<script language=\"javascript\">"+ strExecJsScripts + "</script>");

        //再判断是否定位到下一个记录
        //String strSql2 = "SELECT MOVENEXT FROM TB_HRTMPSA WHERE TID='" + this.strTID + "' AND SID='" + this.strSID + "' AND AID='" + this.strAID + "'";
        //set2 = SqlParamDao.GetDataSetBySql(strSql2);
        //if (set2.Tables[0].Rows.Count > 0)
        //{
        //    str4 = set2.Tables[0].Rows[0][0].ToString();
        //}
        //if (str4 == "1")//需要跳转到下条记录
        //{

        //}
    }
    #endregion
}
