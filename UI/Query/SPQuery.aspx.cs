using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Data;
using System.Web;
using System.Text;
using System.Web.Security;
using System.Drawing.Printing;
using System.Xml;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.Entity.Report;
using Com.ValuePlus.BLL.Query;
using Com.ValuePlus.BLL.Report;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class Query_SPQuery : PageBase
{
    protected String strXmlFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_SPXmlFile");//存储过程参数对应xml文件相对路径

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strSP = Request.Params["SP"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strSP = SQLInjectionDefense.ReplaceSQLReservedKeyword(strSP);

            String strRequestUrl = base.Request.Url.ToString();
            this.strSPTitleName = this.GetSpTitleName(strSP);

            ResourceManager rmLocResourceManager = base.GetResourceManager("QueryMain");
            this.strBtnOK = rmLocResourceManager.GetString("btnOK");
            this.strBtnReset = rmLocResourceManager.GetString("btnReset");

            try
            {
                //设置xml文件的读写路径及文件名以及一些全局变量
                this.SetFilePathAndName(strSP);

                if (!String.IsNullOrEmpty(strSP))
                {
                    this.strSPName = strSP;
                    strResultForwardPage = "QueryMain.aspx?SP=" + this.strSPName;

                    //获取存储过程对应需传入的参数
                    SPQueryBll bllSpQuery = new SPQueryBll();
                    //Hashtable hsTableParam = bllSpQuery.GetSpParamInfo(this.strSPName);
                    //if ((hsTableParam != null) && (hsTableParam.Count > 0))//有需输入的参数则显示参数设置页面
                    //{
                    //    this.BiuldParamSettingArea(hsTableParam);
                    //}
                    ArrayList arrListParam = bllSpQuery.GetSpParamInfo(this.strSPName);
                    if ((arrListParam != null) && (arrListParam.Count > 0))//有需输入的参数则显示参数设置页面
                    {
                        this.BiuldParamSettingArea(arrListParam);
                    }
                    else//无需输入的参数则直接跳转到查询结果页面
                    {
                        this.RedirectResulPage();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "【" + this.strSpName + "】" + rmLocResourceManager.GetString("errTipSpQueryInit") + "');</script>");
                return;
            }
        }
    }

    #region viewstate初始化区域
    private String strSPName
    {
        get
        {
            return ViewState["SPQuery_strSPName_ViewState"] as String;
        }
        set
        {
            ViewState["SPQuery_strSPName_ViewState"] = value;
        }
    }
    private String strSPTitleName
    {
        get
        {
            return ViewState["SPQuery_strSPTitleName_ViewState"] as String;
        }
        set
        {
            ViewState["SPQuery_strSPTitleName_ViewState"] = value;
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
    private String strBtnOK
    {
        get
        {
            return ViewState["spQuery_strBtnOK_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strBtnOK_ViewState"] = value;
        }
    }
    private String strBtnReset
    {
        get
        {
            return ViewState["spQuery_strBtnReset_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strBtnReset_ViewState"] = value;
        }
    }
    private String strRequestUrl
    {
        get
        {
            return ViewState["spQuery_strRequestUrl_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strRequestUrl_ViewState"] = value;
        }
    }
    private String strRequestUrlQuery
    {
        get
        {
            return ViewState["spQuery_strRequestUrlQuery_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strRequestUrlQuery_ViewState"] = value;
        }
    }
    private String strSpName
    {
        get
        {
            return ViewState["spQuery_strSpName_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strSpName_ViewState"] = value;
        }
    }
    private String strXmlFilePathAndName
    {
        get
        {
            return ViewState["spQuery_strXmlFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["spQuery_strXmlFilePathAndName_ViewState"] = value;
        }
    }
    #endregion

    #region 获取存储查询的标题
    /// <summary>
    /// 获取存储查询的标题
    /// </summary>
    /// <param name="strSP"></param>
    /// <returns></returns>
    private String GetSpTitleName(String strSP)
    {
        String strTitleName = strSP;
        try{
            String strSql = "select * from VIEWLANG_1 WHERE VWNAME = '_" + strSP + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt!=null)&&(dt.Rows.Count>0))
            {
                strTitleName = String.IsNullOrEmpty(dt.Rows[0]["VDESC"].ToString()) ? strTitleName : dt.Rows[0]["VDESC"].ToString();
                if (this.Language.Equals("zh-cn"))
                {
                    strTitleName = String.IsNullOrEmpty(dt.Rows[0]["VDESCCN"].ToString()) ? strTitleName : dt.Rows[0]["VDESCCN"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        this.lb_SPTileName.Text = strTitleName;
        return strTitleName;
    }
    #endregion

    #region 设置xml/data/prn文件的读写路径及文件名
    /// <summary>
    /// 设置xml/data/prn文件的读写路径及文件名
    /// </summary>
    private void SetFilePathAndName(String strSP)
    {
        //传入的报表参数中的报表名称字符串
        this.strRequestUrl = Server.UrlDecode(base.Request.Url.ToString());
        this.strRequestUrlQuery = Server.UrlDecode(base.Request.Url.Query.ToString());
        this.strSpName = strSP;
        String strUserId = base.GetUserCode();

        String strXmlFilePath = base.MapPath(strXmlFileRelaTivePath);
        if (!Directory.Exists(strXmlFilePath))
        {
            Directory.CreateDirectory(strXmlFilePath);
        }

        //this.strFilePathAndName = base.MapPath(strSP);
        this.strXmlFilePathAndName = strXmlFilePath + "\\" + this.strSpName + ".xml";
    }
    #endregion

    #region 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// <summary>
    /// 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// </summary>
    /// <param name="strUrl"></param>
    /// <returns>Hashtable</returns>
    private Hashtable GetUrlAnalyse(String strUrl)
    {
        int num = 20;
        //strUrl = strUrl.Replace("%","@");
        Hashtable hsTable = new Hashtable();

        String[] strArray = new String[num];
        if (strUrl.IndexOf("&") > 0)
        {
            ReportMainBll bllReportMain = new ReportMainBll();
            String[] strArray2 = strUrl.Split('&');
            for (int i = 0; i < strArray2.Length; i++)
            {
                String[] strArray3 = strArray2[i].Split('=');
                if (strArray3.Length == 2)
                {
                    if ((!strArray3[0].ToLower().Equals("?sp")) && (!strArray3[0].ToLower().Equals("rnd")))//（sp,rnd）
                    {
                        if (!hsTable.Contains(strArray3[0]))
                        {
                            //如果参数值是一种类型（%P0%、%P1%、%P2%、、、、%P0%、%P9%）则从视图中获取相应值
                            String strValue = strArray3[1];
                            if ((strValue.Length == 4) && (strValue.Substring(0, 2).ToUpper().Equals("%P")))
                            {
                                strValue = bllReportMain.GetUserParamReplaced(strValue.ToUpper(), base.GetUserCode());
                                this.strRequestUrl = this.strRequestUrl.Replace(strValue.ToUpper(), strValue);
                            }
                            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                            strValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strValue);
                            hsTable.Add(strArray3[0], strValue);
                        }
                    }
                }
            }
        }
        return hsTable;
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
                    ReportXmlEntity entityXml = new ReportXmlEntity();
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
            Response.Write("<script language=\"javascript\">alert('【" + this.strSpName + "】'xml文件读取失败！');</script>");
        }
        return hsTable;
    }
    #endregion

    //#region 生成参数设置的页面区域（hashTable暂时不用）
    ///// <summary>
    ///// 生成参数设置的页面区域
    ///// </summary>
    ///// <param name="hsTableParam"></param>
    ///// <returns></returns>
    //private void BiuldParamSettingArea(Hashtable hsTableParam)
    //{
    //    SPQueryBll bllSpQuery = new SPQueryBll();
    //    String strRequestUrl = strResultForwardPage;


    //    String strParamName = "";
    //    if ((hsTableParam != null) && (hsTableParam.Count > 0))
    //    {
    //        //读取xml文件
    //        Hashtable hsTableXmlParam = ReadXmlFile(this.strXmlFilePathAndName);
    //        Hashtable hsTableUrlParam = this.GetUrlAnalyse(this.strRequestUrlQuery);

    //        StringBuilder strBuilderAll = new StringBuilder();
    //        StringBuilder strBuilderVar = new StringBuilder();
    //        StringBuilder strBuilderParamAndValue = new StringBuilder();
    //        String strNowDate = DateTime.Now.ToString("yyyy-MM-dd");

    //        strBuilderAll.Append("\r\n");
    //        strBuilderAll.Append("        <table border=\"0\" class=\"warp_table\" width=\"60%\" id=\"tb1\" align=\"center\" style=\"height:auto\">\r\n");
    //        strBuilderAll.Append("          <tr height=\"10\" align=\"center\">\r\n");
    //        strBuilderAll.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //        strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
    //        strBuilderAll.Append("              </td>\r\n");
    //        strBuilderAll.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //        strBuilderAll.Append("          </tr>\r\n");
    //        //参数及其值区域
    //        foreach (System.Collections.DictionaryEntry entity in hsTableParam)
    //        {
    //            strParamName = entity.Key.ToString();
    //            if (!hsTableUrlParam.ContainsKey(strParamName))//url参数中未设置的参数才需要动态生成输入框
    //            {
    //                StringBuilder strBuilder = new StringBuilder();
    //                String strHtmlCtrlId = "txt" + strParamName;//控件ID
    //                String strLabelCaption = strParamName;//显示内容
    //                String strDataType = "";//数据类型
    //                String strCtrlType = "";//控件类型
    //                String strCtrlId = "";//控件关键字
    //                String strCtrlSql = "";//控件语句
    //                String strDefaultTextHtml = "                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" maxlength=\"40\"style=\"width:80%;\" />\r\n";

    //                ReportXmlEntity entityXml = new ReportXmlEntity();
    //                ReportMainBll bllReport = new ReportMainBll();
    //                if ((hsTableXmlParam.ContainsKey(strParamName.ToLower())) || (hsTableXmlParam.ContainsKey(strParamName.ToUpper())))//从xml文件中读取相关参数的配置
    //                {
    //                    entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToLower()];
    //                    if (entityXml == null)
    //                    {
    //                        entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToUpper()];
    //                    }
    //                    entityXml = bllReport.GetEntityInfoByParam(base.Language, entityXml);
    //                    if (base.Language.Equals("zh-cn"))
    //                    {
    //                        strLabelCaption = entityXml.strPDESCCHS;
    //                    }
    //                    else
    //                    {
    //                        strLabelCaption = entityXml.strPDESC;
    //                    }
    //                    strCtrlType = entityXml.strPCTRLTYPE;
    //                    strCtrlId = entityXml.strPCTRLID;
    //                    strCtrlSql = entityXml.strPCTRLSQL;
    //                    strDataType = entityXml.strPDATATYPE;
    //                }
    //                else
    //                {
    //                    strCtrlType = "0";
    //                    strDataType = "varchar";
    //                }

    //                strBuilder.Append("           <tr>\r\n");
    //                strBuilder.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //                strBuilder.Append("              <td class=\"edit_label\"align = \"center\" Width=\"40%\">\r\n");
    //                strBuilder.Append("                 <span id=\"Label1\">" + strLabelCaption + "<font color=red>*</font></span>\r\n");
    //                strBuilder.Append("              </td>\r\n");
    //                strBuilder.Append("              <td>\r\n");

    //                if (!String.IsNullOrEmpty(strCtrlType))
    //                {
    //                    if (strCtrlType.Equals("0"))//独立文本框
    //                    {
    //                        if ((strDataType.ToLower().Equals("date")) || (strDataType.ToLower().Equals("date")))//如果是时间类型
    //                        {
    //                            strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  value = \"" + strNowDate + "\" onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
    //                        }
    //                        else
    //                        {
    //                            strBuilder.Append(strDefaultTextHtml);
    //                        }
    //                    }
    //                    else if (strCtrlType.Equals("1"))//下拉框列表类型
    //                    {
    //                        if (!String.IsNullOrEmpty(strCtrlId))
    //                        {
    //                            strBuilder.Append("                 <select id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" style=\"width:80%;\" />\r\n");
    //                            strBuilder.Append(this.GetSelectOptionHtml(strCtrlId, base.Language));
    //                            strBuilder.Append("                 </select>");
    //                        }
    //                        else
    //                        {
    //                            strBuilder.Append(strDefaultTextHtml);
    //                        }
    //                    }
    //                    else if (strCtrlType.Equals("2"))//文本框类型，且弹出选择框
    //                    {
    //                        strBuilder.Append(strDefaultTextHtml);
    //                        if (!String.IsNullOrEmpty(strCtrlSql))
    //                        {
    //                            strBuilder.Append("                 <img src=\"../common/images/search1.png\" width=\"14\" height=\"14\" style=\"cursor:hand\" onclick=\"javascript:showOpenWindow('" + strCtrlSql + "','" + strCtrlId + "','" + strHtmlCtrlId + "')\">\r\n");
    //                        }
    //                    }
    //                    else if (strCtrlType.Equals("12"))//时间选择框
    //                    {
    //                        strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\"  onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
    //                    }
    //                    else//其他类型都默认文本框
    //                    {
    //                        strBuilder.Append(strDefaultTextHtml);
    //                    }
    //                }
    //                else
    //                {
    //                    if ((strDataType.ToLower().Equals("date")) || (strDataType.ToLower().Equals("date")))//如果是时间类型
    //                    {
    //                        strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" value = \"" + strNowDate + "\" onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
    //                    }
    //                    else
    //                    {
    //                        strBuilder.Append(strDefaultTextHtml);
    //                    }
    //                }

    //                strBuilder.Append("              </td>\r\n");
    //                strBuilder.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //                strBuilder.Append("           </tr>\r\n");

    //                strBuilderVar.Append("      var " + strHtmlCtrlId + "=document.form1." + strHtmlCtrlId + ".value;\r\n");
    //                //strBuilderParamAndValue.Append("&" + strParamName + "=\"+" + strHtmlCtrlId + "+\"");//当前页面打开时用
    //                strBuilderParamAndValue.Append("&" + strParamName + "='+" + strHtmlCtrlId + "+'");//弹出新窗口时用
    //                strBuilderAll.Append(strBuilder);
    //            }
                
    //        }
    //        //提交脚本区域
    //        strBuilderAll.Append("<script type=\"text/javascript\">\r\n");
    //        strBuilderAll.Append("  function doSubmitParam(){\r\n");
    //        strBuilderAll.Append(strBuilderVar);
    //        //strBuilderAll.Append("      alert('" + strRequestUrl + strBuilderParamAndValue + "');\r\n");
    //        strBuilderAll.Append("      window.open('" + strRequestUrl + strBuilderParamAndValue + "','newwindow', 'width=1000,height=600,top=100,left=200, center:1, toolbar=no, menubar=no, scrollbars=yes,resizable=yes,location=no, status=no');\r\n");
    //        strBuilderAll.Append("  }\r\n");
    //        strBuilderAll.Append("</script>\r\n");

    //        String strOnclickEvent1 = "javascript:doSubmitParam();";

    //        //提交按钮区域
    //        strBuilderAll.Append("          <tr height=\"18\" align=\"center\">\r\n");
    //        strBuilderAll.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //        strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
    //        strBuilderAll.Append("                  <div class=\"top_table_area\">\r\n");
    //        strBuilderAll.Append("                      <input type=\"button\" name=\"Button1\" value=\"" + this.strBtnOK + "\" id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
    //        strBuilderAll.Append("                      <input type=\"reset\" name=\"Button2\" value=\"" + this.strBtnReset + "\" id=\"Button2\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
    //        strBuilderAll.Append("                  </div>\r\n");
    //        strBuilderAll.Append("              </td>\r\n");
    //        strBuilderAll.Append("              <td background=\"../common/images/welcome/mail_rightbg.gif\">&nbsp;</td>\r\n");
    //        strBuilderAll.Append("          </tr>\r\n");

    //        strBuilderAll.Append("        </table>\r\n");
    //        this.divParamArea.InnerHtml = strBuilderAll.ToString();
    //    }
    //}
    //#endregion

    #region 生成参数设置的页面区域
    /// <summary>
    /// 生成参数设置的页面区域
    /// </summary>
    /// <param name="arrListParam"></param>
    /// <returns></returns>
    private void BiuldParamSettingArea(ArrayList arrListParam)
    {
        SPQueryBll bllSpQuery = new SPQueryBll();
        String strRequestUrl = strResultForwardPage;
        int iUrlParamCount = 0;

        String strParamName = "";
        if ((arrListParam != null) && (arrListParam.Count > 0))
        {
            //读取xml文件
            Hashtable hsTableXmlParam = ReadXmlFile(this.strXmlFilePathAndName);
            Hashtable hsTableUrlParam = this.GetUrlAnalyse(this.strRequestUrlQuery);

            StringBuilder strBuilderAll = new StringBuilder();
            StringBuilder strBuilderVar = new StringBuilder();
            StringBuilder strBuilderParamAndValue = new StringBuilder();
            String strNowDate = DateTime.Now.ToString("yyyy-MM-dd");

            strBuilderAll.Append("\r\n");
            //strBuilderAll.Append("        <table border=\"0\" class=\"table\" width=\"60%\" id=\"tb1\" align=\"center\" style=\"height:auto;width:60%\">\r\n");
            strBuilderAll.Append("          <tr height=\"10\" align=\"center\">\r\n");
            strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
            strBuilderAll.Append("              </td>\r\n");
            strBuilderAll.Append("          </tr>\r\n");
            //参数及其值区域
            for (int i = 0; i < arrListParam.Count; i++)
            {
                ReportParamProperty paramProperty = (ReportParamProperty)arrListParam[i];
                strParamName = paramProperty.strParamName;
                String strDataType = paramProperty.strParamDataType;//数据类型
                if ((!hsTableUrlParam.ContainsKey(strParamName.ToUpper()))&&(!hsTableUrlParam.ContainsKey(strParamName.ToLower())))//url参数中未设置的参数才需要动态生成输入框
                {
                    StringBuilder strBuilder = new StringBuilder();
                    String strHtmlCtrlId = "txt" + strParamName;//控件ID
                    String strLabelCaption = strParamName;//显示内容
                    String strCtrlType = "";//控件类型
                    String strCtrlId = "";//控件关键字
                    String strCtrlSql = "";//控件语句
                    String strDefaultTextHtml = "                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" maxlength=\"40\"style=\"width:80%;\" />\r\n";

                    ReportXmlEntity entityXml = new ReportXmlEntity();
                    ReportMainBll bllReport = new ReportMainBll();
                    if ((hsTableXmlParam.ContainsKey(strParamName)) || (hsTableXmlParam.ContainsKey(strParamName.ToLower())) || (hsTableXmlParam.ContainsKey(strParamName.ToUpper())))//从xml文件中读取相关参数的配置
                    {
                        entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToLower()];
                        if (entityXml == null)
                        {
                            entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToUpper()];
                        }
                        if (entityXml == null)
                        {
                            entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName];
                        }
                        entityXml = bllReport.GetEntityInfoByParam(base.Language, entityXml);
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
                    strBuilder.Append("              <td class=\"edit_label\"align = \"center\" Width=\"40%\">\r\n");
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

                                String strParamString = "sql="+strCtrlSql+"&key="+strCtrlId+"&element="+strHtmlCtrlId;
                                strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                                strBuilder.Append("                 <img src=\"../common/images/search1.png\" width=\"14\" height=\"14\" style=\"cursor:hand\" onclick=\"javascript:showOpenWindow('" + strParamString + "')\">\r\n");
                            }
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
                else
                {
                    //如果链接中存在参数的提供
                    String strParamValue = "";
                    if (hsTableUrlParam[strParamName.ToLower()] != null)
                    {
                        strParamValue = hsTableUrlParam[strParamName.ToLower()].ToString();
                    }
                    else
                    {
                        strParamValue = hsTableUrlParam[strParamName.ToUpper()].ToString();
                    }
                    strBuilderParamAndValue.Append("&" + strParamName + "=" + strParamValue);//弹出新窗口时用
                    iUrlParamCount++;
                }

            }
            if (iUrlParamCount != arrListParam.Count)
            {
                //提交脚本区域
                strBuilderAll.Append("<script type=\"text/javascript\">\r\n");
                strBuilderAll.Append("  function doSubmitParam(){\r\n");
                strBuilderAll.Append(strBuilderVar);
                //strBuilderAll.Append("      alert('" + strRequestUrl + strBuilderParamAndValue + "');\r\n");
                strBuilderAll.Append("      window.open('" + strRequestUrl + strBuilderParamAndValue + "','spQueryResult', 'left=0,top=0,width='+ (screen.availWidth - 10) +',height='+ (screen.availHeight-50) +',scrollbars,resizable=yes,location=no,toolbar=no');\r\n");
                strBuilderAll.Append("  }\r\n");
                strBuilderAll.Append("</script>\r\n");

                String strOnclickEvent1 = "javascript:doSubmitParam();";

                //提交按钮区域
                strBuilderAll.Append("          <tr height=\"18\" align=\"center\">\r\n");
                strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
                //strBuilderAll.Append("                      <input type=\"button\" name=\"Button1\" value=\"" + this.strBtnOK + "\" id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
                strBuilderAll.Append("                      <a id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"a_Center\"/>" + this.strBtnOK + "</a>\r\n");
                //strBuilderAll.Append("                      <input type=\"reset\" name=\"Button2\" value=\"" + this.strBtnReset + "\" id=\"Button2\" class=\"btn_2k3\" style=\"height:25px;width:100px\" />\r\n");
                strBuilderAll.Append("              </td>\r\n");
                strBuilderAll.Append("          </tr>\r\n");

                //strBuilderAll.Append("        </table>\r\n");
                this.divParamArea.InnerHtml = strBuilderAll.ToString();
            }
            else//如果存储过程中所需参已经在链接中提供则直接跳转到查询结果页面            
            {
                this.strResultForwardPage = this.strResultForwardPage + strBuilderParamAndValue;
                this.RedirectResulPage();
            }
        }
        else//无需输入的参数则直接跳转到查询结果页面
        {
            this.RedirectResulPage();
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
        Response.Redirect(strResultForwardPage,false);
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
        //if (strListId.Substring(0, 1).Equals("@"))
        //{
        //    strListId = strListId.Replace("@", "").ToUpper();
        //    string[] strArray = strListId.Split(new char[] { ';' });
        //    if ((strArray != null) && (strArray.Length == 2))
        //    {
        //        String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + strArray[0] + " WHERE LID= '" + strArray[1] + "' ORDER BY CID";

        //        ds = SqlParamDao.GetDataSetBySql(strSql);
        //    }
        //}
        //else
        //{
        //    DicManagerBll bllDic = new DicManagerBll();
        //    ds = bllDic.GetDicDetailInfoByLId(strListId);
        //}

        TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(strListId);
        String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "' ORDER BY " + property_LSTD.ORDER;
        ds = SqlParamDao.GetDataSetBySql(strSql);

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
}
