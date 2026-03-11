using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Data;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Common;

using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common.Security;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.Utils;

public partial class Archive_Action_DoAction : PageBase
{
    protected String strXmlFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ActionXmlFile");//存储过程参数对应xml文件相对路径

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "showWaitingDiv", "<script language=\"javascript\">ShowWaitingDiv();</script>");
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "saveCheckedBox", "<script language=\"javascript\">saveCheckedBox();</script>");//先保存列表中复选框
        if (!Page.IsPostBack)
        {
            ////解密传递字符串并获取对应参数值
            //Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            //String strSP = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SP");
            //this.strTID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
            //this.strRID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
            //this.strSID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
            //this.strAID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "AID");
            String strSP = Request.Params["SP"] == null ? "" : Request.Params["SP"].ToString();
            this.strTID = Request.Params["TID"] == null ? "" : Request.Params["TID"].ToString();
            this.strRID = Request.Params["RID"] == null ? "" : Request.Params["RID"].ToString();
            this.strSID = Request.Params["SID"] == null ? "" : Request.Params["SID"].ToString();
            this.strAID = Request.Params["AID"] == null ? "" : Request.Params["AID"].ToString();

            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strSP = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSP);
            this.strTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTID);
            this.strRID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strRID);
            this.strSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSID);
            this.strAID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strAID);
            this.strKeyValue = Request.Params["KEYVALUE"] == null ? "" : Request.Params["KEYVALUE"].ToString();

            ResourceManager rmLocResourceManager = base.GetResourceManager("QueryMain");
            this.strBtnOK = rmLocResourceManager.GetString("btnOK");
            this.strBtnClose = rmLocResourceManager.GetString("btnClose");
            this.strLbSure = rmLocResourceManager.GetString("lbSure");
            this.strLbExecuteSuccess = rmLocResourceManager.GetString("lbExecuteSuccess");
            this.strLbExecuteFailed = rmLocResourceManager.GetString("lbExecuteFailed");
            this.strKeyValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strKeyValue);
            //this.btnDoSubmit.Attributes.Add("onclick", "saveCheckedBox();");

            try
            {
                //设置xml文件的读写路径及文件名以及一些全局变量
                this.SetFilePathAndName(strSP);
                //获取动作名称
                this.SetActionDesc();

                if (!String.IsNullOrEmpty(strSP))
                {
                    this.strSPName = strSP;
                    strResultForwardPage = "SubmitAction.aspx?SP=" + this.strSPName + "&TID=" + this.strTID + "&RID=" + this.strRID + "&SID=" + this.strSID + "&AID=" + this.strAID + "&MOVENEXT=" + this.strActionMoveNextFlag;

                    //获取存储过程对应需传入的参数
                    ArchiveActionBll bllAction = new ArchiveActionBll();
                    ArrayList arrListParam = bllAction.GetSpParamInfo(this.strSPName);
                    if ((arrListParam != null) && (arrListParam.Count > 0))//有需输入的参数则显示参数设置页面
                    {
                        this.BiuldParamSettingArea(arrListParam);
                    }
                    else//无需输入的参数则直接提示是否执行动作
                    {
                        this.DoHtmlAction();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return;
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
            return ViewState["DoAction_strSPName_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strSPName_ViewState"] = value;
        }
    }
    private String strTID
    {
        get
        {
            return ViewState["DoAction_strTID_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strTID_ViewState"] = value;
        }
    }
    private String strRID
    {
        get
        {
            return ViewState["DoAction_strRID_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strRID_ViewState"] = value;
        }
    }
    private String strSID
    {
        get
        {
            return ViewState["DoAction_strSID_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strSID_ViewState"] = value;
        }
    }
    private String strAID
    {
        get
        {
            return ViewState["DoAction_strAID_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strAID_ViewState"] = value;
        }
    }
    private String strActionDesc
    {
        get
        {
            return ViewState["DoAction_strActionDesc_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strActionDesc_ViewState"] = value;
        }
    }
    private String strActionMoveNextFlag
    {
        get
        {
            return ViewState["DoAction_strActionMoveNextFlag_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strActionMoveNextFlag_ViewState"] = value;
        }
    }
    private String strResultForwardPage
    {
        get
        {
            return ViewState["strResultForwardPage_ViewState"] as String;
        }
        set
        {
            ViewState["strResultForwardPage_ViewState"] = value;
        }
    }
    private String strKeyValue
    {
        get
        {
            return ViewState["DoAction_KeyValue_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_KeyValue_ViewState"] = value;
        }
    }
    private String strBtnOK
    {
        get
        {
            return ViewState["DoAction_strBtnOK_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strBtnOK_ViewState"] = value;
        }
    }
    private String strBtnClose
    {
        get
        {
            return ViewState["DoAction_strBtnClose_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strBtnClose_ViewState"] = value;
        }
    }
    private String strLbSure
    {
        get
        {
            return ViewState["DoAction_strLbSure_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strLbSure_ViewState"] = value;
        }
    }
    private string strLbExecuteSuccess
    {
        get
        {
            return ViewState["strLbExecuteSuccess"] as string;
        }
        set
        {
            ViewState["strLbExecuteSuccess"] = value;
        }
    }
    private string strLbExecuteFailed
    {
        get
        {
            return ViewState["strLbExecuteFailed"] as string;
        }
        set
        {
            ViewState["strLbExecuteFailed"] = value;
        }
    }
	
    private String strRequestUrl
    {
        get
        {
            return ViewState["DoAction_strRequestUrl_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strRequestUrl_ViewState"] = value;
        }
    }
    private String strRequestUrlQuery
    {
        get
        {
            return ViewState["DoAction_strRequestUrlQuery_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strRequestUrlQuery_ViewState"] = value;
        }
    }
    private String strXmlFilePathAndName
    {
        get
        {
            return ViewState["DoAction_strXmlFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["DoAction_strXmlFilePathAndName_ViewState"] = value;
        }
    }
    private Hashtable hsTableSPParamAValue
    {
        get
        {
            return ViewState["Query_hsTableSPParamAValue_ViewState"] as Hashtable;
        }
        set
        {
            ViewState["Query_hsTableSPParamAValue_ViewState"] = value;
        }
    }
    #endregion

    #region 设置xml文件的读写路径及文件名
    /// <summary>
    /// 设置xml文件的读写路径及文件名
    /// </summary>
    private void SetFilePathAndName(String strSP)
    {
        //传入的报表参数中的报表名称字符串
        this.strRequestUrl = Server.UrlDecode(base.Request.Url.ToString());
        this.strRequestUrlQuery = Server.UrlDecode(base.Request.Url.Query.ToString());
        this.strSPName = strSP;
        String strUserId = base.GetUserCode();

        String strXmlFilePath = base.MapPath(strXmlFileRelaTivePath);
        if (!Directory.Exists(strXmlFilePath))
        {
            Directory.CreateDirectory(strXmlFilePath);
        }

        //this.strFilePathAndName = base.MapPath(strSP);
        this.strXmlFilePathAndName = strXmlFilePath + "\\" + this.strSPName + ".xml";
    }
    #endregion

    #region 根据特殊算法分析url连接，返回参数及其值的ArrayList
    /// <summary>
    /// 根据特殊算法分析url连接，返回参数及其值的ArrayList
    /// </summary>
    /// <param name="strUrl"></param>
    /// <returns>Hashtable</returns>
    private ArrayList GetUrlAnalyse()
    {
        ArrayList list = new ArrayList();
        for (int num = 0; num < (base.Request.QueryString.Count - 1); num++)
        {
            if (base.Request["P" + num.ToString()] != null)
            {
                String strP = base.Request["P" + num.ToString()].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                strP = SQLInjectionDefense.ReplaceSQLReservedKeyword(strP);
                list.Add(strP);
            }
        }

        return list;
    }
    #endregion

    #region 读取存储过程参数相应xml文件,存储在hashTable中
    /// <summary>
    /// 读取存储过程参数相应xml文件,存储在hashTable中
    /// </summary>
    /// <returns>Hashtable</returns>
    private Hashtable ReadXmlFile(String strXmlFilePathAndName)
    {
        Hashtable hsTable = new Hashtable();
        try
        {
            // 打开一个 XML 文件 
            if (File.Exists(strXmlFilePathAndName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(strXmlFilePathAndName);
                XmlNode xmlNode = xmlDoc.SelectSingleNode("//Fields");
                XmlNodeList nodeList = xmlDoc.SelectNodes("//Fields");
                int iCount = nodeList.Count;

                for (int i = 0; i < iCount; i++)
                {
                    SpXmlEntity entityXml = new SpXmlEntity();
                    xmlNode = nodeList[i];
                    entityXml.strAlias = xmlNode.ChildNodes[0].InnerText.ToString();
                    entityXml.strTid = xmlNode.ChildNodes[1].InnerText.ToString();
                    entityXml.strGid = xmlNode.ChildNodes[2].InnerText.ToString();
                    entityXml.strSid = xmlNode.ChildNodes[3].InnerText.ToString();
                    entityXml.strPid = xmlNode.ChildNodes[4].InnerText.ToString();
                    hsTable.Add(entityXml.strAlias, entityXml);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "【" + this.strSPName + "】xml文件读取失败！");
            //Response.Write("<script language=\"javascript\">alert('【" + this.strSPName + "】'xml文件读取失败！');</script>");
        }
        return hsTable;
    }
    #endregion

    #region 生成参数设置的页面区域
    /// <summary>
    /// 生成参数设置的页面区域
    /// </summary>
    /// <param name="arrListParam"></param>
    /// <returns></returns>
    private void BiuldParamSettingArea(ArrayList arrListParam)
    {
        String strRequestUrl = strResultForwardPage;

        String strParamName = "";
        if ((arrListParam != null) && (arrListParam.Count > 0))
        {
            //读取xml文件
            Hashtable hsTableXmlParam = this.ReadXmlFile(this.strXmlFilePathAndName);
            ArrayList arrListUrlParam = this.GetUrlAnalyse();
            int iUrlParamCount = 0;
            if (arrListUrlParam == null)
            {
                iUrlParamCount = 0;
            }
            else
            {
                iUrlParamCount = arrListUrlParam.Count;
            }
            if (iUrlParamCount < arrListParam.Count)//如果url链接未提供参数或者提供的参数不够则页面生成参数输入框
            {

                StringBuilder strBuilderAll = new StringBuilder();
                StringBuilder strBuilderVar = new StringBuilder();
                StringBuilder strBuilderParamAndValue = new StringBuilder();

                //如果链接中存在参数的提供
                for (int i = 0; i < iUrlParamCount; i++)
                {
                    SpParamEntity paramProperty = (SpParamEntity)arrListParam[i];
                    strParamName = paramProperty.strParamName;
                    String strParamValue = arrListUrlParam[i].ToString();

                    strBuilderParamAndValue.Append("&" + strParamName + "=" + strParamValue);//弹出新窗口时用
                }
                String strNowDate = DateTime.Now.ToString("yyyy-MM-dd");

                strBuilderAll.Append("\r\n");
                strBuilderAll.Append("        <table border=\"0\" class=\"table\" width=\"60%\" id=\"tb1\" align=\"center\" style=\"height:auto;width:60%\">\r\n");
                strBuilderAll.Append("          <tr height=\"10\" align=\"center\">\r\n");
                strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
                strBuilderAll.Append("              </td>\r\n");
                strBuilderAll.Append("          </tr>\r\n");
                //参数及其值区域
                for (int i = iUrlParamCount; i < arrListParam.Count; i++)
                {
                    SpParamEntity paramProperty = (SpParamEntity)arrListParam[i];
                    strParamName = paramProperty.strParamName;
                    String strDataType = paramProperty.strParamDataType;//数据类型

                    StringBuilder strBuilder = new StringBuilder();
                    String strHtmlCtrlId = "txt" + strParamName;//控件ID
                    String strLabelCaption = strParamName;//显示内容
                    String strCtrlType = "";//控件类型
                    String strCtrlId = "";//控件关键字
                    String strCtrlSql = "";//控件语句
                    String strDefaultTextHtml = "                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" maxlength=\"40\" style=\"width:80%;\" />\r\n";

                    SpXmlEntity entityXml = new SpXmlEntity();
                    ArchiveActionBll bllAction = new ArchiveActionBll();
                    if ((hsTableXmlParam.ContainsKey(strParamName)) || (hsTableXmlParam.ContainsKey(strParamName.ToLower())) || (hsTableXmlParam.ContainsKey(strParamName.ToUpper())))//从xml文件中读取相关参数的配置
                    {
                        entityXml = (SpXmlEntity)hsTableXmlParam[strParamName];
                        if (entityXml == null)
                        {
                            entityXml = (SpXmlEntity)hsTableXmlParam[strParamName.ToUpper()];
                        }
                        if (entityXml == null)
                        {
                            entityXml = (SpXmlEntity)hsTableXmlParam[strParamName.ToLower()];
                        }
                        entityXml = bllAction.GetSpXmlEntityInfoByParam(entityXml);
                        if (base.Language.Equals("zh-cn"))
                        {
                            strLabelCaption = entityXml.strPDESCCHS;
                        }
                        else
                        {
                            strLabelCaption = entityXml.strPDESC;
                        }
                        strCtrlType = entityXml.strPCTRLTYPE;
                        strCtrlId = entityXml.strPCTRLID;
                        strCtrlSql = entityXml.strPCTRLSQL;
                        if (!strDataType.Equals("datetime"))
                        {
                            strDataType = entityXml.strPDATATYPE;
                        }
                    }
                    else
                    {
                        strCtrlType = "0";
                        if (!strDataType.Equals("datetime"))
                        {
                            strDataType = "varchar";
                        }
                    }

                    strBuilder.Append("           <tr>\r\n");
                    strBuilder.Append("              <td class=\"edit_label\"align = \"center\" Width=\"30%\">\r\n");
                    strBuilder.Append("                 <span id=\"Label1\">" + strLabelCaption + "<font color=red>*</font></span>\r\n");
                    strBuilder.Append("              </td>\r\n");
                    strBuilder.Append("              <td>\r\n");

                    if (!String.IsNullOrEmpty(strCtrlType))
                    {
                        if (strCtrlType.Equals("0"))//独立文本框
                        {
                            //if ((strDataType.ToLower().Equals("date")) || (strDataType.ToLower().Equals("datetime")))//如果是时间类型
                            //{
                            //    strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                            //}
                            if ((strDataType.ToLower().Equals("date")) || (strDataType.ToLower().Equals("datetime")))//如果是时间类型
                            {
                                strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" datetype=\"date\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                            }
                            else
                            {
                                strBuilder.Append(strDefaultTextHtml);
                            }
                        }
                        else if (strCtrlType.Equals("1"))//下拉框列表类型
                        {
                            if (!String.IsNullOrEmpty(strCtrlId))
                            {
                                strBuilder.Append("                 <select id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" style=\"width:80%;\" />\r\n");
                                strBuilder.Append(this.GetSelectOptionHtml(strCtrlId, base.Language));
                                strBuilder.Append("                 </select>");
                            }
                            else
                            {
                                strBuilder.Append(strDefaultTextHtml);
                            }
                        }
                        else if (strCtrlType.Equals("2"))//文本框类型，且弹出选择框
                        {
                            strBuilder.Append(strDefaultTextHtml);
                            if (!String.IsNullOrEmpty(strCtrlSql))
                            {
                                //如果字段包括;分隔符，则取第一个字段
                                //add by sammen 20140717
                                if (strCtrlId.Contains(";"))
                                {
                                    String[] arrTemp = strCtrlId.Split(';');
                                    strCtrlId = arrTemp[0].ToString();
                                }

                                String strParamString = "sql=" + strCtrlSql + "&key=" + strCtrlId + "&element=" + strHtmlCtrlId;
                                strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                                strBuilder.Append("                 <img src=\"../../common/images/search1.png\" width=\"14\" height=\"14\" style=\"cursor:hand\" onclick=\"javascript:showOpenWindow('" + strParamString + "')\">\r\n");
                            }
                        }
                        else if (strCtrlType.Equals("6"))//宽行文本框
                        {
                            strBuilder.Append("                 <textarea id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" maxlength=\"200\" type=\"text\" style=\"width:80%;height:50px;\"></textarea>\r\n");
                        }
                        else if (strCtrlType.Equals("12"))//日期选择框
                        {
                            //strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                            strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" datetype=\"date\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                        }
                        else if (strCtrlType.Equals("112"))//日期时间选择框
                        {
                            //strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                            strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" datetype=\"datetime\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                        }
                        else//其他类型都默认文本框
                        {
                            strBuilder.Append(strDefaultTextHtml);
                        }
                    }
                    else
                    {
                        if ((strDataType.ToLower().Equals("date")) || (strDataType.ToLower().Equals("datetime")))//如果是时间类型
                        {
                            //strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                            strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" datetype=\"date\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                        }
                        else
                        {
                            strBuilder.Append(strDefaultTextHtml);
                        }
                    }

                    strBuilder.Append("              </td>\r\n");
                    strBuilder.Append("           </tr>\r\n");

                    strBuilderVar.Append("      var " + strHtmlCtrlId + "=document.form1." + strHtmlCtrlId + ".value;\r\n");
                    //strBuilderParamAndValue.Append("&" + strParamName + "=\"+" + strHtmlCtrlId + "+\"");//当前页面打开时用
                    strBuilderParamAndValue.Append("&" + strParamName + "='+" + strHtmlCtrlId + "+'");//弹出新窗口时用
                    strBuilderAll.Append(strBuilder);


                }
                if ((strBuilderParamAndValue.ToString().Substring(strBuilderParamAndValue.Length - 1, 1) == "'") && (strBuilderParamAndValue.ToString().Substring(strBuilderParamAndValue.Length - 2, 1) == "+"))
                {
                    strBuilderParamAndValue = strBuilderParamAndValue.Remove(strBuilderParamAndValue.Length - 2, 2);
                }


                //提交脚本区域
                strBuilderAll.Append("<script type=\"text/javascript\">\r\n");
                strBuilderAll.Append("  function doSubmitParam(){\r\n");
                strBuilderAll.Append(strBuilderVar);
                //strBuilderAll.Append("      alert('" + strRequestUrl + strBuilderParamAndValue + "');\r\n");
                strBuilderAll.Append("      if('" + this.strLbSure + this.strActionDesc + "？'){\r\n");
                strBuilderAll.Append("          saveCheckedBox();\r\n");    ////先保存列表中复选框
                strBuilderAll.Append("          window.location.href='" + strRequestUrl + strBuilderParamAndValue + ";\r\n");
                strBuilderAll.Append("      }else{\r\n");
                strBuilderAll.Append("          window.close();\r\n");
                strBuilderAll.Append("      }\r\n");
                strBuilderAll.Append("   }\r\n");
                strBuilderAll.Append("</script>\r\n");

                String strOnclickEvent1 = "javascript:doSubmitParam();";
                String strOnclickEvent2 = "javascript:window.close();";

                //提交按钮区域
                strBuilderAll.Append("          <tr height=\"18\" align=\"center\">\r\n");
                strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
                strBuilderAll.Append("                      <input type=\"button\" name=\"Button1\" value=\"" + this.strBtnOK + "\" id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
                strBuilderAll.Append("                      <input type=\"reset\" name=\"Button2\" value=\"" + this.strBtnClose + "\" id=\"Button2\" onclick=\"" + strOnclickEvent2 + "\" class=\"btn_2k3\" style=\"height:25px;width:100px\" />\r\n");
                strBuilderAll.Append("              </td>\r\n");
                strBuilderAll.Append("          </tr>\r\n");

                strBuilderAll.Append("        </table>\r\n");
                this.divParamArea.InnerHtml = strBuilderAll.ToString();

            }
            else
            {
                //一一匹配链接中的参数和存储过程参数
                this.SetSpParamValue(arrListUrlParam, arrListParam);
                this.DoHtmlAction();
            }
        }
        else//无需输入的参数则直接跳转到查询结果页面
        {
            this.DoHtmlAction();
        }
    }
    #endregion

    #region 转查询结果页面
    /// <summary>
    /// 转查询结果页面
    /// </summary>
    /// <param name="hsTableParam"></param>
    /// <returns></returns>
    private void RedirectResulPage()
    {
        Response.Redirect(strResultForwardPage, false);
    }
    #endregion

    #region 根据列表清单编号加载下拉框列表
    /// <summary>
    /// 根据列表清单编号加载下拉框列表
    /// </summary>
    /// <param name="strListId"></param>
    /// <param name="strLanguage"></param>
    /// <returns></returns>
    private StringBuilder GetSelectOptionHtml(String strListId, String strLanguage)
    {
        StringBuilder strBuilderSeleteHtml = new StringBuilder();
        DataSet ds = new DataSet();
        if (strListId.Substring(0, 1).Equals("@"))
        {
            strListId = strListId.Replace("@", "").ToUpper();
            string[] strArray = strListId.Split(new char[] { ';' });
            if ((strArray != null) && (strArray.Length == 2))
            {
                String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + strArray[0] + " WHERE LID= '" + strArray[1] + "' ORDER BY CID";

                ds = SqlParamDao.GetDataSetBySql(strSql);
            }
        }
        else
        {
            DicManagerBll bllDic = new DicManagerBll();
            ds = bllDic.GetDicDetailInfoByLId(strListId);
        }

        strBuilderSeleteHtml.Append("                      <option value=\"" + "" + "\">All</option>\r\n");
        if ((ds != null) && (ds.Tables.Count > 0))
        {
            DataTable dt = ds.Tables[0];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                String strValue = "";
                String strCaption = "";
                foreach (DataRow r in dt.Rows)
                {
                    strValue = r["CID"].ToString();
                    if (!String.IsNullOrEmpty(strLanguage))
                    {
                        if (strLanguage.Equals("zh-cn"))
                        {
                            strCaption = r["CDESCCHS"].ToString();
                        }
                        else
                        {
                            strCaption = r["CDESC"].ToString();
                        }
                    }
                    else
                    {
                        strCaption = r["CDESCCHS"].ToString();
                    }
                    //动态加载下拉列表
                    strBuilderSeleteHtml.Append("                      <option value=\"" + strValue + "\">" + strCaption + "</option>\r\n");
                }
            }
        }
        return strBuilderSeleteHtml;
    }
    #endregion

    #region 获取动作名称
    /// <summary>
    /// 获取动作名称
    /// </summary>
    /// <returns></returns>
    private void SetActionDesc()
    {
        string strActionName;
        if (this.Language == "zh-cn")
        {
            strActionName = "ADESCCHS";
        }
        else
        {
            strActionName = "ADESC";
        }
        string sqlstring = "SELECT " + strActionName + ",MOVENEXT FROM TB_HRTMPSA WHERE TID='" + this.strTID + "' AND SID ='"+this.strSID+"' AND AID='" + this.strAID + "'";
        DataSet set = SqlParamDao.GetDataSetBySql(sqlstring);
        if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
        {
            this.strActionDesc = set.Tables[0].Rows[0][0].ToString();
            this.strActionMoveNextFlag = set.Tables[0].Rows[0][1].ToString();
        }
        else
        {
            this.strActionDesc = "";
            this.strActionMoveNextFlag = "1";
        }
    }
    #endregion

    #region 从url链接参数一一匹配设置存储过程参数值
    /// <summary>
    /// 从url链接参数一一匹配设置存储过程参数值
    /// <param name="hsTableParam"></param>
    /// <param name="strSpName"></param>
    /// </summary>
    private void SetSpParamValue(ArrayList arrListUrlParam, ArrayList arrListSpParam)
    {
        Hashtable hs = new Hashtable();
        //存储过程的参数命名无须规则限制，而是根据参数的顺序与字段APARA0、APARA1、……APARA9顺序匹配，最多支持10个参数
        for (int i = 0; i < arrListSpParam.Count; i++)
        {
            SpParamEntity paramProperty = (SpParamEntity)arrListSpParam[i];
            String strParamName = paramProperty.strParamName;
            String strParamValue = arrListUrlParam[i].ToString();

            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strParamValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strParamValue);

            hs.Add(strParamName, strParamValue);
        }
        this.hsTableSPParamAValue = hs;
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
        int iCount = ArchiveActionBll.DoExcuteSP(this.strSPName, this.hsTableSPParamAValue);
        this.SetPageTipAfterExcuteSp(iCount);

        //执行动作的日志写入 add by sammen 20250304
        DataLogWriter.Log_ExecuteAction(this.GetUserCode(), RequestUtils.GetIP(),this.strTID,this.strSID,this.strAID,strKeyValue, this.strSPName, this.hsTableSPParamAValue, (iCount > 0 ? true : false));
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
            if (iReturn < 0)
            {
                strMsg = this.strLbExecuteFailed;
            }
            else
            {
                strMsg = this.strLbExecuteSuccess;
            }
        }
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "doActionSuccess", "<script language=\"javascript\">reloadOpenerPages('" + strMsg + "','"+this.strActionMoveNextFlag+"');</script>");

        String strExecJsScripts = "reloadOpenerPages('" + strMsg + "','" + this.strActionMoveNextFlag + "');";
        //add by sammen 20240428 根据FLUser_6的配置进行短信发送或者公众号推送
        if (iReturn > -1)
        {
            //动作执行成功，则根据FLUser_6的配置进行短信发送
            Com.ValuePlus.Archive.Flow.FLUserSMS.SendSMSAfterActionExecute(this.strTID, this.strRID, this.strSID, this.strAID,this.strKeyValue, "1");
            //动作执行成功，则根据FLUser_6的配置进行公众号的推送【先调用客户端脚本以便去调用远程公众号服务器】
            strExecJsScripts = strExecJsScripts + "pushMsgAfterAction('" + this.strTID + "','" + this.strRID + "','" + this.strSID + "','" + this.strAID + "','" + this.strKeyValue + "','" + this.GetUserCode() + "');";
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "doActionSuccess", "<script language=\"javascript\">" + strExecJsScripts + "</script>");

    }
    #endregion

    /// <summary>
    /// html页面预处理
    /// </summary>
    private void DoHtmlAction()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("<script language=\"javascript\">\r\n");
        sb.Append("    if(confirm('" + strLbSure + "" + this.strActionDesc + "？')){\r\n");
        sb.Append("        saveCheckedBox();\r\n");    ////先保存列表中复选框
        sb.Append("        javascript:__doPostBack('btnDoSubmit','');\r\n");
        sb.Append("    }else{window.close();}");
        sb.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "addConfirm", sb.ToString());
    }
}
