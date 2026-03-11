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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.Report;
using Com.ValuePlus.Entity.Report;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.Utils;

/// <summary>
/// modify log
/// 20121120：增加报表支持存储查询的功能，相应存储过程的名称为"USP_RPT_"+报表名称
/// </summary>

public partial class Report_ReportMain : PageBase
{
    protected  String strRptFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ReortFile") + "/rpt";//报表rpt文件相对路径
    protected  String strXmlFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ReortFile") + "/xml";//报表xml文件相对路径
    protected  String strPrnFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ReortFile") + "/print";//报表prn文件相对路径
    protected  String strDataFileRelaTivePath = BaseConfig.Instance.GetConfigValueByKey("PATH_ReortFile") + "/data";//报表data文件相对路径

    //protected ReportDocument reportDocument1;
    protected HtmlTable htmTable;
    protected System.Web.UI.WebControls.Table webTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "showWaitingDiv", "<script language=\"javascript\">ShowWaitingDiv();</script>");
        if (!Page.IsPostBack)
        {
            this.strRPT = Request.Params["RPT"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strRPT = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strRPT);

            //设置xml/data/prn文件的读写路径及文件名以及一些全局变量
            this.SetFilePathAndName(strRPT);

            ResourceManager rmLocResourceManager = base.GetResourceManager("ReportMain");
            //this.Label1.Text = rmLocResourceManager.GetString("lbQueryParam");
            //this.Label2.Text = rmLocResourceManager.GetString("lbQueryValue");
            this.strTitle = rmLocResourceManager.GetString("lbReportTitle");
            this.strTipPathErr = rmLocResourceManager.GetString("tipPathErr");
            this.strTipNoMatchXmlErr = rmLocResourceManager.GetString("tipNoMatchXmlErr");
            this.strTipLoadReportErr = rmLocResourceManager.GetString("tipLoadReportErr");
            this.strTipReportDBAccessErr = rmLocResourceManager.GetString("tipReportDBAccessErr");
            this.strBtnOK = rmLocResourceManager.GetString("btnOK");
            this.strBtnReset = rmLocResourceManager.GetString("btnReset");

            //设置报表数据库访问
            SetReportDataAccess();
            //报表查询日志写入 add by sammen 20250304
            Hashtable hsTableParam = new Hashtable();
            DataLogWriter.Log_QueryReport(this.GetUserCode(), RequestUtils.GetIP(), this.strRPT, hsTableParam);
        }
        //初始化crystal相关组件
        InitializeComponent();
        //设置打印表头
        this.SetPrinter();
        //加载报表
        this.LoadReport();

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    #region viewstate初始化区域
    private ReportDocument reportDocument1
    {
        get
        {
            return ViewState["reportMain_ReportDocument1_ViewState"] as ReportDocument;
        }
        set
        {
            ViewState["reportMain_ReportDocument1_ViewState"] = value;
        }
    }
    private String strRPT
    {
        get
        {
            return ViewState["reportMain_strRPT_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strRPT_ViewState"] = value;
        }
    }
    
    private String strTitle
    {
        get
        {
            return ViewState["reportMain_strTitle_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strTitle_ViewState"] = value;
        }
    }
    private String strTipPathErr
    {
        get
        {
            return ViewState["reportMain_strTipPathErr_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strTipPathErr_ViewState"] = value;
        }
    }
    private String strTipNoMatchXmlErr
    {
        get
        {
            return ViewState["reportMain_strTipNoMatchXmlErr_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strTipNoMatchXmlErr_ViewState"] = value;
        }
    }
    private String strTipLoadReportErr
    {
        get
        {
            return ViewState["reportMain_strTipLoadReportErr_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strTipLoadReportErr_ViewState"] = value;
        }
    }
    private String strTipReportDBAccessErr
    {
        get
        {
            return ViewState["reportMain_strTipReportDBAccessErr_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strTipReportDBAccessErr_ViewState"] = value;
        }
    }
    private String strBtnOK
    {
        get
        {
            return ViewState["reportMain_strBtnOK_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strBtnOK_ViewState"] = value;
        }
    }
    private String strBtnReset
    {
        get
        {
            return ViewState["reportMain_strBtnReset_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strBtnReset_ViewState"] = value;
        }
    }
    private String strRequestUrl
    {
        get
        {
            return ViewState["reportMain_strRequestUrl_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strRequestUrl_ViewState"] = value;
        }
    }
    private String strRequestUrlQuery
    {
        get
        {
            return ViewState["reportMain_strRequestUrlQuery_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strRequestUrlQuery_ViewState"] = value;
        }
    }
    private String strRptFileName
    {
        get
        {
            return ViewState["reportMain_strRptFileName_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strRptFileName_ViewState"] = value;
        }
    }
    private String strRptFilePathAndName
    {
        get
        {
            return ViewState["reportMain_strRptFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strRptFilePathAndName_ViewState"] = value;
        }
    }
    private String strDataFilePathAndName
    {
        get
        {
            return ViewState["reportMain_strDataFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strDataFilePathAndName_ViewState"] = value;
        }
    }
    private String strXmlFilePathAndName
    {
        get
        {
            return ViewState["reportMain_strXmlFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strXmlFilePathAndName_ViewState"] = value;
        }
    }
    private String strPrnFilePathAndName
    {
        get
        {
            return ViewState["reportMain_strPrnFilePathAndName_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strPrnFilePathAndName_ViewState"] = value;
        }
    }
    private String strDbServer
    {
        get
        {
            return ViewState["reportMain_strDbServer_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strDbServer_ViewState"] = value;
        }
    }
    private String strDbDatabase
    {
        get
        {
            return ViewState["reportMain_strDbDatabase_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strDbDatabase_ViewState"] = value;
        }
    }
    private String strDbUser
    {
        get
        {
            return ViewState["reportMain_strDbUser_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strDbUser_ViewState"] = value;
        }
    }
    private String strDbPwd
    {
        get
        {
            return ViewState["reportMain_strDbPwd_ViewState"] as String;
        }
        set
        {
            ViewState["reportMain_strDbPwd_ViewState"] = value;
        }
    }
    #endregion

    #region 初始化crystal相关组件
    /// <summary>
    /// 初始化crystal相关组件
    /// </summary>
    private void InitializeComponent()
    {
        if (this.CrystalReportViewer1 == null)
        {
            this.CrystalReportViewer1 = new CrystalReportViewer();
        }
        this.reportDocument1 = new ReportDocument();
        this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA3;
        this.reportDocument1.PrintOptions.PaperSource = CrystalDecisions.Shared.PaperSource.Auto;
        this.reportDocument1.PrintOptions.PrinterDuplex = PrinterDuplex.Default;
        this.reportDocument1.PrintOptions.PrinterName = "EPSON LQ-1600KIII";
    }
    #endregion

    #region 设置报表数据库链接
    /// <summary>
    /// 设置报表数据库链接
    /// </summary>
    private void SetReportDataAccess()
    {
        DBConSortStr dbConnection = new DBConSortStr();
        this.strDbDatabase = dbConnection.Database;
        this.strDbServer = dbConnection.Server;
        this.strDbUser = dbConnection.User;
        this.strDbPwd = dbConnection.Password;
    }
    #endregion

    #region 设置xml/data/prn文件的读写路径及文件名
    /// <summary>
    /// 设置xml/data/prn文件的读写路径及文件名
    /// </summary>
    private void SetFilePathAndName(String strRPT)
    {
        //传入的报表参数中的报表名称字符串
        //this.strRequestUrl = Server.UrlDecode(base.Request.Url.ToString());

        //modify by sammen 20181130 只取路径和参数部门，不获取主机头部分，因为在外网映射并带端口时无法获取到端口号造成错误
        this.strRequestUrl = Server.UrlDecode(base.Request.Url.PathAndQuery.ToString());
        if (base.Request.ApplicationPath.Equals("/"))
        {
            //如果是无应用上下文
            this.strRequestUrl = this.strRequestUrl.Replace("/Report/", "");
        }

        log.Error("水晶报表加载链接原始地址:"+ strRequestUrl);

        this.strRequestUrlQuery = Server.UrlDecode(base.Request.Url.Query.ToString());
        this.strRptFileName = strRPT.Substring(strRPT.LastIndexOf("/") + 1, strRPT.LastIndexOf(".") - strRPT.LastIndexOf("/") - 1);
        String strUserId = base.GetUserCode();

        String strRptFilePath = base.MapPath(strRptFileRelaTivePath);
        if (!Directory.Exists(strRptFilePath))
        {
            Directory.CreateDirectory(strRptFilePath);
        }
        String strXmlFilePath = base.MapPath(strXmlFileRelaTivePath) ;
        if (!Directory.Exists(strXmlFilePath))
        {
            Directory.CreateDirectory(strXmlFilePath);
        }
        String strDataFilePath = base.MapPath(strDataFileRelaTivePath) + "\\" + strUserId ;
        if (!Directory.Exists(strDataFilePath))
        {
            Directory.CreateDirectory(strDataFilePath);
        }
        String strPrnFilePath = base.MapPath(strPrnFileRelaTivePath);
        if (!Directory.Exists(strPrnFilePath))
        {
            Directory.CreateDirectory(strPrnFilePath);
        }

        this.strRptFilePathAndName = strRptFilePath + "\\" + strRptFileName + ".rpt";
        this.strXmlFilePathAndName = strXmlFilePath + "\\" + strRptFileName + ".xml";
        this.strDataFilePathAndName = strDataFilePath + "\\" + strRptFileName + ".data";
        this.strPrnFilePathAndName = strPrnFilePath + "\\" + strRptFileName + ".prn";
    }
    #endregion

    #region 设置打印表头
    /// <summary>
    /// 设置打印表头
    /// </summary>
    public void SetPrinter()
    {
        try
        {
            String[] strArray = new String[3];
            if (!File.Exists(this.strPrnFilePathAndName))
            {
                this.reportDocument1.PrintOptions.PrinterName = "";
                this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                return;
            }
            StreamReader readerStream = File.OpenText(this.strPrnFilePathAndName);
            String strLine = readerStream.ReadLine();
            if (strLine != null)
            {
                strArray = strLine.Split(new char[] { ';' });
            }
            readerStream.Close();
            String strPrintName = strArray[0];
            String strPageType = strArray[1];
            String strPrintType = strArray[2];
            if (String.IsNullOrEmpty(strPrintType))
            {
                strPrintType = "1";//默认纵向
            }
            this.reportDocument1.PrintOptions.PrinterName = strPrintName;

            if (!String.IsNullOrEmpty(strPageType))
            {
                switch (strPageType)
                {
                    case "A3":
                        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA3;
                        break;
                    case "A4":
                        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        break;
                    case "A5":
                        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;
                        break;
                    case "B4":
                        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperB4;
                        break;
                    case "B5":
                        this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperB5;
                        break;
                    //case "HRCARD"://防止遍历造成速度慢，目前本地可以，但是服务器上尺寸无效。待继续研究
                    //    System.Drawing.Printing.PaperSize size_HRCARD = new System.Drawing.Printing.PaperSize("HRCARD", 860, 540);
                    //    this.reportDocument1.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)size_HRCARD.RawKind;
                    //    break;
                    default:
                        PrinterSettings settingPrint = new PrinterSettings();
                        if (!String.IsNullOrEmpty(strPrintName))
                        {
                            settingPrint.PrinterName = strPrintName;
                        }
                        //遍历服务器端对应打印机的打印纸张
                        for (int i = 0; i < settingPrint.PaperSizes.Count; i++)
                        {
                            System.Drawing.Printing.PaperSize size = settingPrint.PaperSizes[i];
                            if (size.PaperName == strPageType)
                            {
                                //this.reportDocument1.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)i;
                                this.reportDocument1.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)size.RawKind;
                                log.Error("报表:【"+ this.strRptFilePathAndName + "】使用了自定义纸张："+ strPageType);
                                break;
                            }
                        }
                        break;
                }
            }
            switch (strPrintType)
            {
                case "1":
                    this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
                    break;

                case "2":
                    this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                    break;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, this.strRptFileName + "报表打印机设置失败！");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
            return;
        }
    }

    ///// <summary>
    ///// 设置打印表头
    ///// </summary>
    //public void SetPrinter()
    //{
    //    try
    //    {
    //        String[] strArray = new String[3];
    //        if (!File.Exists(this.strPrnFilePathAndName))
    //        {
    //            this.reportDocument1.PrintOptions.PrinterName = "";
    //            this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
    //            this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
    //            return;
    //        }
    //        StreamReader readerStream = File.OpenText(this.strPrnFilePathAndName);
    //        String strLine = readerStream.ReadLine();
    //        if (strLine != null)
    //        {
    //            strArray = strLine.Split(new char[] { ';' });
    //        }
    //        readerStream.Close();
    //        this.reportDocument1.PrintOptions.PrinterName = strArray[0];
    //        String strPageType = strArray[1];
    //        if (strPageType != null)
    //        {
    //            if (!(strPageType == "A3"))
    //            {
    //                if (strPageType == "A4")
    //                {
    //                    this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
    //                    goto Label_01AC;
    //                }
    //                if (strPageType == "A5")
    //                {
    //                    this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;
    //                    goto Label_01AC;
    //                }
    //                if (strPageType == "B4")
    //                {
    //                    this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperB4;
    //                    goto Label_01AC;
    //                }
    //                if (strPageType == "B5")
    //                {
    //                    this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperB5;
    //                    goto Label_01AC;
    //                }
    //            }
    //            else
    //            {
    //                this.reportDocument1.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA3;
    //                goto Label_01AC;
    //            }
    //        }
    //        PrinterSettings settingPrint = new PrinterSettings();
    //        settingPrint.PrinterName = strArray[0];
    //        for (int i = 0; i < settingPrint.PaperSizes.Count; i++)
    //        {
    //            System.Drawing.Printing.PaperSize size = settingPrint.PaperSizes[i];
    //            if (size.PaperName == strArray[1])
    //            {
    //                //this.reportDocument1.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)i;
    //                this.reportDocument1.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)size.RawKind;
    //                break;
    //            }
    //        }
    //    Label_01AC:
    //        if (strArray.Length <= 2)
    //        {
    //            this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
    //        }
    //        else
    //        {
    //            switch (strArray[2])
    //            {
    //                case "1":
    //                    this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
    //                    return;

    //                case "2":
    //                    this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
    //                    return;
    //            }
    //            this.reportDocument1.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        this.AlertMessageBox(this.Page, this.strRptFileName + "报表打印机设置失败！");
    //        Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
    //        return;
    //    }
    //}
    #endregion

    #region 装载报表
    /// <summary>
    /// 装载报表
    /// </summary>
    public void LoadReport()
    {
        if (!File.Exists(this.strRptFilePathAndName))
        {
            this.AlertMessageBox(this.Page, this.strTipPathErr);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
        }
        else
        {
            try
            {
                this.reportDocument1.Load(this.strRptFilePathAndName);
                //读取xml文件ReportDocument
                Hashtable hsTableXmlParam = ReadXmlFile(this.strXmlFilePathAndName);

                //ParameterFields crParameterFields = this.reportDocument1.ParameterFields;

                //int iParamFieldCount = crParameterFields.Count;

                //判断xml文件的参数个数与报表文件中参数个数是否匹配（不是每个报表都要有对应xml文件）
                //if (hsTableXmlParam != null)
                //{
                //    int iXmlParamCount = hsTableXmlParam.Count;
                //    if (iXmlParamCount != iParamFieldCount)
                //    {
                //        Response.Write("<script language=\"javascript\">alert('" + "【" + this.strRptFileName + "】" + this.strTipNoMatchXmlErr + "');</script>");
                //        return;
                //    }
                //}

                //如果从url已经获取到参数则设置已经获取到的报表参数，并返回未设置值的参数hashTable
                Hashtable hsTableParamValues = this.GetUrlAnalyse(this.strRequestUrlQuery);
                //Hashtable hsTableNoValueParam = new Hashtable(); 
                ArrayList arrListTableNoValueParam = new ArrayList();
                if (hsTableParamValues != null)
                {
                    arrListTableNoValueParam = this.SetReportParamValues(hsTableParamValues);
                }

                //判断是否存在未获取值的参数，有则显示参数设置页面，无则直接显示报表
                this.SetParamAreaIsVisible(arrListTableNoValueParam, hsTableXmlParam,hsTableParamValues);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, this.strRptFileName + this.strTipLoadReportErr);
                Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
                return;
            }

            
        }
    }
    #endregion

    #region 设置报表中涉及的所有表的数据访问
    /// <summary>
    /// 设置报表中涉及的所有表的数据访问
    /// </summary>
    private void SetReportDataBase()
    {
        try
        {
            //设置报表文件对数据库的访问
            this.reportDocument1.SetDatabaseLogon(this.strDbUser, this.strDbPwd, this.strDbServer, this.strDbDatabase);
            foreach (CrystalDecisions.CrystalReports.Engine.Table table in this.reportDocument1.Database.Tables)
            {
                TableLogOnInfo infoTableLogOn = new TableLogOnInfo();
                infoTableLogOn = table.LogOnInfo;
                infoTableLogOn.ConnectionInfo.ServerName = this.strDbServer;
                infoTableLogOn.ConnectionInfo.DatabaseName = this.strDbDatabase;
                infoTableLogOn.ConnectionInfo.UserID = this.strDbUser;
                infoTableLogOn.ConnectionInfo.Password = this.strDbPwd;
                table.ApplyLogOnInfo(infoTableLogOn);
                //if (table.Location != "Command")
                //{
                //    table.Location = table.Location;
                //}
            }
            this.CrystalReportViewer1.ReportSource = this.reportDocument1;
            this.CrystalReportViewer1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "【" + this.strRptFileName + "】" + this.strTipReportDBAccessErr);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
            return;
        }
    }
    #endregion

    #region 读取报表相应xml文件,存储在hashTable中
    /// <summary>
    /// 读取报表相应xml文件,存储在hashTable中
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
            this.AlertMessageBox(this.Page, "【" + this.strRptFilePathAndName + "】xml文件读取失败！");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
        }
        return hsTable;
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
                    if ((!strArray3[0].ToLower().Equals("?rpt")) && (!strArray3[0].ToLower().Equals("rnd")))//（rpt,rnd）
                    {
                        if (!hsTable.Contains(strArray3[0]))
                        {
                            //如果参数值是一种类型（%P0%、%P1%、%P2%、、、、%P0%、%P9%）则从视图中获取相应值
                            String strValue = strArray3[1];
                            if (strValue.ToUpper().Equals("%USERCODE%"))
                            {
                                strValue = strArray3[1].Replace("%USERCODE%", base.GetUserCode());
                                this.strRequestUrl = this.strRequestUrl.Replace(strArray3[1], strValue);
                            }else if ((strValue.Length == 4) && (strValue.Substring(0, 2).ToUpper().Equals("%P")))
                            {
                                strValue = bllReportMain.GetUserParamReplaced(strArray3[1], base.GetUserCode());
                                this.strRequestUrl = this.strRequestUrl.Replace(strArray3[1], strValue);
                            }
                            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                            strValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(strValue);
                            log.Error("水晶报表加载链接替换掉参数后的地址:" + strRequestUrl);
                            hsTable.Add(strArray3[0], strValue);
                        }
                    }
                }
            }
        }
        return hsTable;
    }
    #endregion

    #region 设置报表参数值，并返回未设置值的参数到ArrayList
    /// <summary>
    /// 设置报表参数值，并返回未设置值的参数到ArrayList
    /// </summary>
    /// <param name="hsTableParamValue"></param>
    /// <returns>arrListNoValueParam</returns>
    private ArrayList SetReportParamValues(Hashtable hsTableParamValue)
    {
        //Hashtable hsTableNoValueParam = new Hashtable();
        ArrayList arrListNoValueParam = new ArrayList();
        if (hsTableParamValue != null)
        {
            ParameterFields crParameterFields;
            ParameterField crParameterField;
            ParameterValues crParameterValues;
            ParameterDiscreteValue crParameterDiscreteValue;
            String strParamName;
            String strParamTextTip;
            ParameterValueKind ptParamDataType;
            String strParamDataType;

            crParameterFields = this.reportDocument1.ParameterFields;
            int iParamFieldCount = crParameterFields.Count;
            

            if (iParamFieldCount > 0)
            {
                ParameterFields crParameterFields_new = new ParameterFields();
                for (int i = 0; i < iParamFieldCount; i++)
                {
                    crParameterField = crParameterFields[i];
                    strParamName = crParameterField.Name;
                    String strReortName = crParameterField.ReportName;
                    if (strReortName.Equals(""))//如果是主报表
                    {
                        crParameterValues = crParameterField.CurrentValues;
                        crParameterDiscreteValue = new ParameterDiscreteValue();
                        if (hsTableParamValue[strParamName] != null)//如果存在某个参数的值
                        {
                            crParameterDiscreteValue.Value = hsTableParamValue[strParamName];
                            crParameterValues.Add(crParameterDiscreteValue);

                            //**如此设置不能翻第三页
                            //crParameterField.CurrentValues = crParameterValues;
                            //crParameterFields_new.Add(crParameterField);
                            //**如此设置不能翻第三页

                            //**如此设置可翻第三页
                            this.reportDocument1.SetParameterValue(strParamName, crParameterValues);
                            //**如此设置可翻第三页
                        }
                        else if ((strParamName.Substring(0,1).Equals("@")) && (hsTableParamValue[strParamName.Substring(1,strParamName.Length-1)] != null))//如果存在某个参数的值(如果报表调用的是存储过程，则报表参数前面第一位自动带@)
                        {
                            crParameterDiscreteValue.Value = hsTableParamValue[strParamName.Substring(1, strParamName.Length - 1)];
                            crParameterValues.Add(crParameterDiscreteValue);

                            //**如此设置不能翻第三页
                            //crParameterField.CurrentValues = crParameterValues;
                            //crParameterFields_new.Add(crParameterField);
                            //**如此设置不能翻第三页

                            //**如此设置可翻第三页
                            this.reportDocument1.SetParameterValue(strParamName, crParameterValues);
                            //**如此设置可翻第三页
                        }
                        else//如果该参数并未获取到值,则设置参数的提示文本
                        {
                            ReportParamProperty paramProperty = new ReportParamProperty();
                            strParamTextTip = crParameterField.PromptText;
                            ptParamDataType = crParameterField.ParameterValueType;
                            strParamDataType = ptParamDataType.ToString();

                            paramProperty.strParamName = strParamName;
                            paramProperty.strParamTextTip = strParamTextTip;
                            paramProperty.strParamDataType = strParamDataType;

                            //hsTableNoValueParam.Add(strParamName, paramProperty);
                            arrListNoValueParam.Add(paramProperty);
                        }
                    }

                }
                //**如此设置不能翻第三页
                //this.CrystalReportViewer1.ParameterFieldInfo = crParameterFields_new;
                //**如此设置不能翻第三页

            }
        }
        return arrListNoValueParam;
    }
    #endregion

    #region 设置是否显示报表界面，是怎显示报表界面，否则显示参数界面
    /// <summary>
    /// 设置是否显示报表界面，是怎显示报表界面，否则显示参数界面
    /// </summary>
    /// <param name="bIsShowReport"></param>
    private void ShowReportOrParamArea(bool bIsShowReport)
    {
        if (bIsShowReport)
        {
            this.CrystalReportViewer1.Visible = true;
            this.divParamArea.Visible = false;
        }
        else
        {
            this.CrystalReportViewer1.Visible = false;
            this.divParamArea.Visible = true;
        }
    }
    #endregion

    #region 判断是否存在未获取值的参数，有则显示参数设置页面，无则直接显示报表(同时如果需要执行存储过程则首先执行)
    /// <summary>
    /// 判断是否存在未获取值的参数，有则显示参数设置页面，无则直接显示报表
    /// </summary>
    /// <param name="arrListTableNoValueParam"></param>
    /// <param name="hsTableXmlParam"></param>
    /// <param name="hsTableParamValues"></param>
    private void SetParamAreaIsVisible(ArrayList arrListTableNoValueParam, Hashtable hsTableXmlParam,Hashtable hsTableParamValues)
    {
        //如果还有参数尚未设置参数值，则许显示参数页面，否则则直接显示报表
        if (arrListTableNoValueParam == null)
        {
            //如果需要则首先执行存储过程(add by sammen 20121120)
            this.ExecuteProcedureFirst(hsTableParamValues);

            //设置显示报表区域
            this.ShowReportOrParamArea(true);
            this.SetReportDataBase();
        }
        else
        {
            if (arrListTableNoValueParam.Count > 0)
            {
                this.ShowReportOrParamArea(false);//设置显示参数设置区域
                this.BiuldParamSettingArea(arrListTableNoValueParam, hsTableXmlParam);
            }
            else
            {
                //如果需要则首先执行存储过程(add by sammen 20121120)
                this.ExecuteProcedureFirst(hsTableParamValues);

                this.ShowReportOrParamArea(true);//设置显示报表区域
                this.SetReportDataBase();
            }
        }
    }
    #endregion

    #region 生成参数设置的页面区域
    /// <summary>
    /// 生成参数设置的页面区域
    /// </summary>
    /// <param name="arrListTableNoValueParam"></param>
    /// <param name="hsTableXmlParam"></param>
    /// <returns></returns>
    private void BiuldParamSettingArea(ArrayList arrListTableNoValueParam, Hashtable hsTableXmlParam)
    {
        ReportMainBll bllReport = new ReportMainBll();

        String strParamName = "";
        if ((arrListTableNoValueParam != null) && (arrListTableNoValueParam.Count > 0))
        {
            //int iNoValueParamCount = hsTableNoValueParam.Count;
            StringBuilder strBuilderAll = new StringBuilder();
            StringBuilder strBuilderVar = new StringBuilder();
            StringBuilder strBuilderParamAndValue = new StringBuilder();
            String strNowDate = DateTime.Now.ToString("yyyy-MM-dd");

            strBuilderAll.Append("\r\n");
            strBuilderAll.Append("        <table border=\"0\" class=\"table\" width=\"60%\" id=\"tb1\" align=\"center\" style=\"height:auto;width:60%\">\r\n");
            strBuilderAll.Append("          <tr height=\"10\" align=\"center\">\r\n");
            strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
            strBuilderAll.Append("              </td>\r\n");
            strBuilderAll.Append("          </tr>\r\n");
            //参数及其值区域
            for (int i = 0; i < arrListTableNoValueParam.Count;i++ )
            {
                ReportParamProperty paramProperty = (ReportParamProperty)arrListTableNoValueParam[i];
                StringBuilder strBuilder = new StringBuilder();
                String strHtmlCtrlId = "txt" + strParamName;//控件ID
                String strLabelCaption = "";//显示内容
                String strDataType = "";//数据类型
                String strCtrlType = "";//控件类型
                String strCtrlId = "";//控件关键字
                String strCtrlSql = "";//控件语句
                String strDefaultTextHtml = "                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" maxlength=\"40\"style=\"width:80%;\" />\r\n";
                if (paramProperty != null)
                {
                    strParamName = paramProperty.strParamName.ToString();
                    ReportXmlEntity entityXml = new ReportXmlEntity();
                    //strHtmlCtrlId = strHtmlCtrlId.Replace("@", "");
                    if ((hsTableXmlParam.ContainsKey(strParamName.ToLower())) || (hsTableXmlParam.ContainsKey(strParamName.ToUpper())) || (hsTableXmlParam.ContainsKey(strParamName)))
                    {
                        entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToLower()];
                        if (entityXml == null)
                        {
                            entityXml = (ReportXmlEntity)hsTableXmlParam[strParamName.ToUpper()];
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
                        strDataType = entityXml.strPDATATYPE;
                    }
                    else
                    {
                        strLabelCaption = paramProperty.strParamTextTip;
                        strDataType = paramProperty.strParamDataType;
                        strCtrlType = "0";
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
                            String strParamString = "sql=" + strCtrlSql + "&key=" + strCtrlId + "&element=" + strHtmlCtrlId;
                            strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                            strBuilder.Append("                 <img src=\"../common/images/search1.png\" width=\"14\" height=\"14\" style=\"cursor:hand\" onclick=\"javascript:showOpenWindow('" + strParamString + "')\">\r\n");
                        }
                    }
                    //else if (strCtrlType.Equals("12"))//时间选择框
                    //{
                    //    strBuilder.Append("                 <input id=\"" + strHtmlCtrlId + "\" name=\"" + strHtmlCtrlId + "\" runat=\"server\" type=\"text\" value = \"" + strNowDate + "\" onfocus=\"vpCalendar_ShowDate(this);\"  maxlength=\"40\" style=\"width:80%;\" />\r\n");
                    //}
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
                strBuilderParamAndValue.Append("&" + strParamName + "=\"+" + strHtmlCtrlId + "+\"");
                strBuilderAll.Append(strBuilder);

            }
            //提交脚本区域
            log.Error("水晶报表加载链接替换掉参数后准备加载数据的地址:" + this.strRequestUrl+ strBuilderParamAndValue);
            strBuilderAll.Append("<script type=\"text/javascript\">\r\n");
            strBuilderAll.Append("  function doSubmitParam(){\r\n");
            strBuilderAll.Append(strBuilderVar);
            //strBuilderAll.Append("      alert('" + strRequestUrl + strBuilderParamAndValue + "');\r\n");
            strBuilderAll.Append("      ShowWaitingDiv();\r\n");
            strBuilderAll.Append("      window.location.href =\"" + this.strRequestUrl + strBuilderParamAndValue + "\";\r\n");
            strBuilderAll.Append("  }\r\n");
            strBuilderAll.Append("</script>\r\n");

            String strOnclickEvent1 = "javascript:doSubmitParam();";

            //提交按钮区域
            strBuilderAll.Append("          <tr height=\"18\" align=\"center\">\r\n");
            strBuilderAll.Append("              <td align =\"center\" colspan=\"2\">\r\n");
            //strBuilderAll.Append("                      <input type=\"button\" name=\"Button1\" value=\"" + this.strBtnOK + "\" id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
            strBuilderAll.Append("                      <a id=\"Button1\" onclick=\"" + strOnclickEvent1 + "\" class=\"a_Center\"/>"+this.strBtnOK+"</a>\r\n");
            //strBuilderAll.Append("                      <input type=\"reset\" name=\"Button2\" value=\"" + this.strBtnReset + "\" id=\"Button2\" class=\"btn_2k3\" style=\"height:25px;width:100px;\" />\r\n");
            strBuilderAll.Append("              </td>\r\n");
            strBuilderAll.Append("          </tr>\r\n");

            strBuilderAll.Append("        </table>\r\n");
            this.divParamArea.InnerHtml = strBuilderAll.ToString();
        }
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

    #region 如果需要执行存储过程则先执行存储过程(add by sammen 20121120)
    /// <summary>
    /// 如果需要执行存储过程则先执行存储过程
    /// </summary>
    /// <param name="hsTableParamValues"></param>
    private void ExecuteProcedureFirst(Hashtable hsTableParamValues)
    {
        String strSPName = "USP_RPT_"+this.strRptFileName;
        try
        {
            String strSql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + strSPName + "]') AND type in (N'P', N'PC')";
            int iCount = SqlParamDao.ExecuteScalarBySql(strSql);
            if (iCount > 0)
            {
                String strReturn = SqlParamDao.ExcuteSPReturnStr(strSPName, hsTableParamValues);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "【" + strSPName + "】Failed！");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "go(-1)", "<script language=\"javascript\">history.go(-1);</script>");
        }
    }
    #endregion

    protected void Page_Unload(object sender, EventArgs e)
    {
        if (this.reportDocument1 != null)
        {
            this.reportDocument1.Dispose();
        }
    }

}
