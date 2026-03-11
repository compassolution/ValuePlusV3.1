using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.WebCtrls;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.DataLog.Entity;
using System.Resources;
using System.Text.RegularExpressions;
using Com.ValuePlus.Common.Config;

public partial class Archive_Detail_EditArchiveDetail : ArchivePageBase
{
    private ArrayList arrayObjectHRLOG2 = new ArrayList();
    private ArrayList arrListMyFreeTextBox = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Archive_Detail_ArchiveDetailAjax), this.Page);
        if (!Page.IsPostBack)
        {

            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.TID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
                this.RID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
                this.SID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
                this.KEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEY");
                this.KEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEYVALUE");
                this.OPTYPE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");//操作类型（add新增，edit编辑，readonly只读）
                this.strPageIndex = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageIndex");//列表页面传递过来的当前页码，主要为了返回页面时使用
                this.strPageSize = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageSize");//列表页面传递过来的当前页码，主要为了返回页面时使用/add by sammen 20140327
                this.strOpenStatus = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "status");////列表显示状态模式，select表示为默认查询状态，主要为了返回页面时使用
                this.strSingleRole = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "ROLE");//角色编码，由配置连接提供
                if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sqlAddtion")))//add by sammen 20130109 为了返回时主列表页面记录保持过滤状态
                {
                    this.strSqlAddtion = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sqlAddtion").ToString();
                }
                if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "searchValue")))//add by sammen 20130109
                {
                    this.strSearchValue = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "searchValue").ToString();
                }
                if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sortExp")))//add by sammen 20210516
                {
                    this.strSortExp = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sortExp").ToString();
                }

                //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
                if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postGoupQueryFilter")))
                {
                    this.strPostGoupQueryFilter = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postGoupQueryFilter").ToString();
                }
                //add by sammen 20240410 增加隐藏顶部工具栏模式，最初为批量审批页面MultiAppove.html页面打造
                if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "hideTopToolbar")))
                {
                    String hideTopToolbar = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "hideTopToolbar").ToString();
                    if(hideTopToolbar.Equals("1"))
                    {
                        this.divTopToolBar.Visible = false;
                    }
                }
                //页面语言设置
                this.DoLanguageSetting();

                this.hfIsSaveSuccess.Value = "1";//默认赋值
                this.hfIsAutoSave.Value = "0";//默认赋值
                
                /// 资产系统中，判断如果当前系统时间不处于当前月份，则进行提示
                /// add by sammen 20160726
                this.SetMonthlyNotSettleNotice();

                /// 设置新增单据记录按钮事件
                /// add by sammen 20170215
                this.SetAddClick();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, this.strTipUrlLoadError);
            }
            try
            {
                //清除服务器端控件客户端界面选择的值
                this.ClearServerCtrlClientSettingSession();

                if (!String.IsNullOrEmpty(this.strPageIndex))
                {
                    this.aBack.Visible = true;
                    this.aClose.Visible = false;
                    this.hfIsOpenAtCurPage.Value = "1";//是否在当前页面打开的标志位
                }
                else
                {
                    this.aBack.Visible = false;
                    this.aClose.Visible = true;
                    this.hfIsOpenAtCurPage.Value = "0";//是否在当前页面打开的标志位
                }
                if ((!String.IsNullOrEmpty(this.TID)) && (!String.IsNullOrEmpty(this.RID)) && (!String.IsNullOrEmpty(this.SID)) && (!String.IsNullOrEmpty(this.OPTYPE)))
                {
                    //首先设置该页面加载时需要的信息
                    this.SetCurPageInfo();

                    //加载数据前执行 add by sammen 20140626
                    this.doBeforeLoadDetail();

                    if (this.OPTYPE.ToLower().Equals("add"))//新增操作
                    {
                        //加载该模板该状态下所能新增的字段信息
                        this.ddListKeyValue.Visible = false;

                        //加载该模板该状态下的所有动作ACTION
                        ///// 加载当前模板在新增状态时当前场景下的Action动作区域 add by sammen 20170511
                        this.ActionAreaSetting();
                        
                        //加载该模板该状态下该keyvalue的所有信息
                        this.CreateGroupTabs();

                    }
                    else//编辑操作、只读操作
                    {
                        //加载该模板该状态下的所有动作ACTION
                        this.ActionAreaSetting();
                        //加载该模板该状态下的所有列表主键下拉框
                        this.ddListKeyValue.Visible = true;
                        this.BuildKeyValueDDList(this.strCurSceneSql);

                        //加载该模板该状态下该keyvalue的所有信息
                        this.CreateGroupTabs();
                    }
                    if (this.OPTYPE.ToLower().Equals("readonly"))
                    {
                        this.aSave.Visible = false;
                    }
                    //写查看日志
                    this.WriteDataLog(null, "readonly");

                    //动态加载页面中的客户端事件
                    this.BuildClientEventScript(this.TID,this.SID);

                    //针对FreeTextBoxCtrl控件修改字体及其大小
                    this.AddFontToFreeTextBoxCtrl(this.arrListMyFreeTextBox);

                    //获取模板表格类型的动作列表
                    ArchiveGridActionBll.GetArchiveGridActions();

                    //加载数据后执行 add by sammen 20140626
                    this.doAfterLoadDetail();


                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, this.strTipPageLoadError);
            }
        }
        //设置向前向后切换记录的按钮的可用性
        this.SetPreNextRecordButton();

        //页面标题设置,防止切换标签时标题显示为默认值的情况
        if (this.Language.Equals("zh-cn"))
        {
            this.Page.Title = Session["ArchiveDesc"] + "内容明细";
        }
        else
        {
            this.Page.Title = Session["ArchiveDesc"] + " Detail";
        }
    }

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
        this.DropGroupTab();
        this.CreateGroupTabs();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    /// <summary>
    /// 重新加载页面
    /// </summary>
    public void RefreshPage()
    {
        //加载数据前执行 add by sammen 20140626
        this.doBeforeLoadDetail();
        this.ActionAreaSetting();//重新加载动作区 add by 20111205
        this.DropGroupTab();
        this.CreateGroupTabs();
        //加载数据后执行 add by sammen 20140626
        this.doAfterLoadDetail();
    }

    #region viewstate初始化区域
    private string strPageType
    {
        get
        {
            return ViewState["strPageType_ViewState"] as string;
        }
        set
        {
            ViewState["strPageType_ViewState"] = value;
        }
    }
    private string strCurMainTableName
    {
        get
        {
            return ViewState["strCurMainTableName_ViewState"] as string;
        }
        set
        {
            ViewState["strCurMainTableName_ViewState"] = value;
        }
    }
    private string strCurSceneSql
    {
        get
        {
            return ViewState["strCurSceneSql_ViewState"] as string;
        }
        set
        {
            ViewState["strCurSceneSql_ViewState"] = value;
        }
    }
    private Entity_ArchiveStyle entityArchiveStyle
    {
        get
        {
            if (this.ViewState["entityArchiveStyle"] == null)
            {
                return new Entity_ArchiveStyle();
            }
            return (Entity_ArchiveStyle)this.ViewState["entityArchiveStyle"];
        }
        set
        {
            this.ViewState["entityArchiveStyle"] = value;
        }
    }
    private bool IsHis
    {
        get
        {
            if (ViewState["IsHis"] != null)
            {
                return (bool)ViewState["IsHis"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsHis"] = value;
        }
    }
    private ArrayList arrListCurGroups
    {
        get
        {
            if (this.ViewState["arrListCurGroups_viewState"] == null)
            {
                return new ArrayList();
            }
            return (ArrayList)this.ViewState["arrListCurGroups_viewState"];
        }
        set
        {
            this.ViewState["arrListCurGroups_viewState"] = value;
        }
    }
    private string strPageIndex
    {
        get
        {
            return ViewState["strPageIndex_ViewState"] as string;
        }
        set
        {
            ViewState["strPageIndex_ViewState"] = value;
        }
    }
    private string strPageSize
    {
        get
        {
            return ViewState["strPageSize_ViewState"] as string;
        }
        set
        {
            ViewState["strPageSize_ViewState"] = value;
        }
    }
    private string strOpenStatus
    {
        get
        {
            return ViewState["strOpenStatus"] as string;
        }
        set
        {
            ViewState["strOpenStatus"] = value;
        }
    }
    //传递进来的唯一角色
    private string strSingleRole
    {
        get
        {
            return ViewState["strSingleRole_ViewState"] as string;
        }
        set
        {
            ViewState["strSingleRole_ViewState"] = value;
        }
    }

    private int iShowGroupCount
    {
        get
        {
            if (this.ViewState["iShowGroupCount"] != null)
            {
                return (int)this.ViewState["iShowGroupCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iShowGroupCount"] = value;
        }
    }
    private int iRecordCount
    {
        get
        {
            if (this.ViewState["iRecordCount"] != null)
            {
                return (int)this.ViewState["iRecordCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iRecordCount"] = value;
        }
    }
    private string strTipUrlLoadError
    {
        get
        {
            return ViewState["strTipUrlLoadError"] as string;
        }
        set
        {
            ViewState["strTipUrlLoadError"] = value;
        }
    }
    private string strTipPageLoadError
    {
        get
        {
            return ViewState["strTipPageLoadError"] as string;
        }
        set
        {
            ViewState["strTipPageLoadError"] = value;
        }
    }
    private string strTipExsitSameKeyValue
    {
        get
        {
            return ViewState["strTipExsitSameKeyValue"] as string;
        }
        set
        {
            ViewState["strTipExsitSameKeyValue"] = value;
        }
    }
    private string strTipSaveFailed
    {
        get
        {
            return ViewState["strTipSaveFailed"] as string;
        }
        set
        {
            ViewState["strTipSaveFailed"] = value;
        }
    }
    private string strTipSaveSuccess
    {
        get
        {
            return ViewState["strTipSaveSuccess"] as string;
        }
        set
        {
            ViewState["strTipSaveSuccess"] = value;
        }
    }
    private string strTipErrorConfig
    {
        get
        {
            return ViewState["strTipErrorConfig"] as string;
        }
        set
        {
            ViewState["strTipErrorConfig"] = value;
        }
    }
    private string strSqlAddtion
    {
        get
        {
            return ViewState["strSqlAddtion_ViewState"] as string;
        }
        set
        {
            ViewState["strSqlAddtion_ViewState"] = value;
        }
    }
    private string strPostGoupQueryFilter
    {
        get
        {
            return ViewState["strGroupQueryFilter_ViewState"] as string;
        }
        set
        {
            ViewState["strGroupQueryFilter_ViewState"] = value;
        }
    }
    
    private string strSearchValue
    {
        get
        {
            if (this.ViewState["strSearchValue"] == null)
            {
                return null;
            }
            return this.ViewState["strSearchValue"].ToString();
        }
        set
        {
            this.ViewState["strSearchValue"] = value;
        }
    }
    public string strSortExp
    {
        get
        {
            if (this.ViewState["strSortExp"] == null)
            {
                return null;
            }
            return this.ViewState["strSortExp"].ToString();
        }
        set
        {
            this.ViewState["strSortExp"] = value;
        }
    }
    public int iCurTabIndex
    {
        get
        {
            if (this.ViewState["iCurTabIndex"] == null)
            {
                return 0;
            }
            return int.Parse(this.ViewState["iCurTabIndex"].ToString());
        }
        set
        {
            this.ViewState["iCurTabIndex"] = value;
        }
    }
    #endregion

    #region 页面语言设置
    /// <summary>
    /// 页面语言设置
    /// </summary>
    private void DoLanguageSetting()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("ArchiveDetail");
        //页面标题设置
        if (this.Language.Equals("zh-cn"))
        {
            this.Page.Title = Session["ArchiveDesc"] + "内容明细";
        }
        else
        {
            this.Page.Title = Session["ArchiveDesc"] + " Detail";
        }

        this.aBack.Text = rmLocResourceManager.GetString("aBack");
        this.aClose.Text = rmLocResourceManager.GetString("aClose");
        this.aSave.Text = rmLocResourceManager.GetString("aSave");
        this.aRefreshDetail.Text = rmLocResourceManager.GetString("aRefresh");
        //this.lbSelcetAll.Text = rmLocResourceManager.GetString("lbSelcetAll");
        this.lbSelcetAll.Text = "";

        this.strTipUrlLoadError = rmLocResourceManager.GetString("tipUrlLoadError");
        this.strTipPageLoadError = rmLocResourceManager.GetString("tipPageLoadError");
        this.strTipExsitSameKeyValue = rmLocResourceManager.GetString("tipExsitSameKeyValue");
        this.strTipErrorConfig = rmLocResourceManager.GetString("tipErrorConfig");
        this.strTipSaveFailed = rmLocResourceManager.GetString("tipSaveFailed");
        this.strTipSaveSuccess = rmLocResourceManager.GetString("tipSaveSuccess");
        this.hfIsSureLeaveWhenChanged.Value = rmLocResourceManager.GetString("tipIsSureLeaveWhenChanged");
    }
    #endregion

    #region 设置该页面加载时需要的信息
    /// <summary>
    /// 设置该页面加载时需要的信息
    /// </summary>
    private void SetCurPageInfo()
    {
        //if (String.IsNullOrEmpty(this.KEY))
        //{
            //如果KEY不存在，则先获取
            this.KEY = this.GetKeyNameByTID(this.TID);
        //}
        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
        //获取当前用户的对应参数值
        this.hsCurUserParamValue = bllGetArchiveSetting.GetUserParamValueByUserId(base.GetUserCode(), this.IsAdminstrator());
        //通过TID获取当前角色下所设置的参数值
        this.hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(this.TID, this.RID, base.GetUserCode(), this.IsAdminstrator(),this.hsCurUserParamValue);
        //通过TID及SID获取当前场景下的数据集SQL语句
        this.strCurSceneSql = bllGetArchiveSetting.GetSenceSqlByTidASid(this.TID, this.SID, this.strCurMainTableName, this.KEY);
        //替换相关参数
        this.strCurSceneSql = ParamOperationBll.ReplaceSceneSqlParam(this.strCurSceneSql, this.hsCurRoleParamValue);
        //获取当前模板的样式配置文件ArchiveStyle
        this.GetArchiveStyle(this.TID);

    }

    /// <summary>
    /// 通过TID获取其主键字段名称，同时获取对应主信息表的表名,字段类型等
    /// </summary>
    /// <param name="strTid"></param>
    private String GetKeyNameByTID(String strTid)
    {
        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
        DataTable dt = bllGetArchiveSetting.GetKeyInfoByTID(strTid);
        String strKeyName = "";
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            strKeyName = dt.Rows[0]["PID"].ToString();
            this.strCurMainTableName = ServerCtrlIDGetterBll.GetArchiveTableNameByGroup(strTid, dt.Rows[0]["GID"].ToString());
            this.KEYTYPE = dt.Rows[0]["PTYPE"].ToString();
        }
        return strKeyName;
    }

    /// <summary>
    /// 获取当前模板的样式配置文件ArchiveStyle
    /// </summary>
    private void GetArchiveStyle(String strTID)
    {
        this.entityArchiveStyle = ArchiveStyleGetterBll.GetTemplateStylesEntity(strTID);
        this.strPageType = this.entityArchiveStyle.PageStyle;//页面类型，0为平铺，1为页签
    }

    #endregion
    
    #region 加载当前模板当前场景下的Action动作区域
    /// <summary>
    /// 加载当前模板当前场景下的Action动作区域
    /// </summary>
    private void ActionAreaSetting()
    {
        string strHis;
        if (this.IsHis)
        {
            strHis = "1";
        }
        else
        {
            strHis = "0";
        }
        ArchiveActionBll bllAction = new ArchiveActionBll();
        ArrayList arrActionList = bllAction.GetActionDetailList(this.TID, this.RID, this.SID, 1, this.KEYVALUE, base.GetUserCode(),this.IsAdminstrator(), strHis);
        //获取在新增时也显示的模板动作 add by sammen 20170511
        Hashtable hsTableShowWhenAdd = this.GetShowActionWhenAdd();
        this.divActionArea.InnerHtml = "";

        if ((arrActionList != null) && (arrActionList.Count > 0))
        {
            String strActionName = "";
            String strActionPage = "";
            String strActionIsAutoSave = "0";
            String strActionType = "0";

            StringBuilder strBuilderAction = new StringBuilder();
            strBuilderAction.Append("\r\n");
            for (int i = 0; i < arrActionList.Count; i++)
            {
                Entity_CreateAction entityAction = (Entity_CreateAction)arrActionList[i];
                bool isShow = false;

                if (this.OPTYPE.ToLower().Equals("add"))//新增操作
                {
                    if (hsTableShowWhenAdd.ContainsKey(entityAction.TID+ entityAction.AID))
                    {
                        isShow = true;
                    }
                }
                else
                {
                    isShow = true;
                }
                if (isShow)
                {
                    if (base.Language.Equals("en-us"))
                    {
                        strActionName = entityAction.ADESC;
                    }
                    else
                    {
                        strActionName = entityAction.ADESCCHS;
                    }
                    strActionPage = entityAction.ADETAIL;
                    strActionIsAutoSave = entityAction.ISAUTOSAVE.ToString();//是否自动保存
                    strActionType = entityAction.ATYPE;

                    strBuilderAction.Append("                    <a href=\"#\" onclick=\"javascript:selectOneAction('" + strActionPage + "','" + strActionIsAutoSave + "','"+ strActionType + "');\" class=\"a_Left\">" + strActionName + "</a>\r\n");
                }
            }
            //在页面显示
            this.divActionArea.InnerHtml = strBuilderAction.ToString();
        }
    }

    /// <summary>
    /// 获取在新增时也显示的模板动作
    /// add by sammen 20170511
    /// </summary>
    /// <returns></returns>
    private Hashtable GetShowActionWhenAdd()
    {
        Hashtable hsTable = new Hashtable();

        StringBuilder sbSql = new StringBuilder();
        sbSql.Append("select * from TB_HRTMPSA WHERE TID = '" + this.TID + "' AND SID = '" + this.SID + "' AND ATYPE = '0' AND ALOCATION = '1' AND ARIGHT = '1' ");
        //判断是否显示按钮调用身份证读取页面，如果是，则在新增保存前也显示，与返回保存同时出现
        sbSql.Append(" AND (ADETAIL LIKE '%ReadIDCard/ReadIDCard.html%'");
        //首先判断表ActionWhenAdd_1是否存在
        String strSqlIsExists = "select count(*) from sysobjects where name = 'ActionWhenAdd_1' and xtype = 'U'";
        int iCount = SqlParamDao.ExecuteScalarBySql(strSqlIsExists);
        if (iCount > 0)
        {
            sbSql.Append(" or (TID+AID IN (SELECT TIDAID FROM ActionWhenAdd_1))");
        }
        sbSql.Append(")");

        String strJudgeSql = sbSql.ToString();
        DataTable dt = SqlParamDao.GetDataTableBySql(strJudgeSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                hsTable.Remove(dt.Rows[i]["TID"].ToString() + dt.Rows[i]["AID"].ToString());
                hsTable.Add(dt.Rows[i]["TID"].ToString() + dt.Rows[i]["AID"].ToString(), dt.Rows[i]["ADETAIL"].ToString());
            }
        }
        return hsTable;

    }

    #endregion

    #region 当前模板具有的所有记录主键值下拉列表操作区域
    /// <summary>
    /// 加载当前模板具有的所有记录主键值下拉列表
    /// </summary>
    /// <param name="strCurSceneSql"></param>
    private void BuildKeyValueDDList(String strCurSceneSql)
    {

        try
        {
            String strSql = strCurSceneSql;
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.ddListKeyValue.Items.Clear();
                this.ddListKeyValue.DataSource = dt.DefaultView;
                this.ddListKeyValue.DataTextField = this.KEY;
                this.ddListKeyValue.DataValueField = this.KEY;
                //设置显示类型(时间类型特殊设置)
                if (!String.IsNullOrEmpty(this.KEYTYPE) && (this.KEYTYPE.StartsWith("date")))
                {
                    this.ddListKeyValue.DataTextFormatString = @"{0:yyyy\-MM\-dd}";//还可以是格式@"MM\/dd\/yyyy"
                    //this.KEYVALUE = DateTime.Parse(this.KEYVALUE).ToString(@"yyyy\-MM\-dd");
                }
                this.ddListKeyValue.DataBind();
                //ListItem lItem = new ListItem("", "");
                //this.ddListKeyValue.Items.Add(lItem);
                //当前被选中
                if (!String.IsNullOrEmpty(this.KEYVALUE))
                {
                    this.ddListKeyValue.SelectedIndex = this.ddListKeyValue.Items.IndexOf(this.ddListKeyValue.Items.FindByValue(this.KEYVALUE));
                }
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    String strKeyValue = dt.Rows[i][this.KEY].ToString();

                //    ListItem lItem = new ListItem(strKeyValue, strKeyValue);
                //    this.ddListKeyValue.Items.Add(lItem);
                //}
                this.iRecordCount = dt.Rows.Count;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("加载当前模板具有的所有记录主键值下拉列表失败,方法BuildKeyValueDDList");
        }
    }

    /// <summary>
    /// 主键值下拉框选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListKeyValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.KEYVALUE = this.ddListKeyValue.SelectedValue;
            this.RefreshPage();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("主键值下拉框选择失败,方法ddListKeyValue_SelectedIndexChanged");
        }
    }

    /// <summary>
    /// 设置向前向后切换记录的按钮的可用性
    /// </summary>
    private void SetPreNextRecordButton()
    {
        try
        {
            this.ddListKeyValue.Attributes.Add("onchange", "ShowWaitingDiv();");
            if (this.ddListKeyValue.SelectedIndex <= 0)
            {
                this.imgBtnPre.Attributes.Add("onclick", "return false;");
            }
            else
            {
                this.imgBtnPre.Attributes.Add("onclick", "ShowWaitingDiv();");
            }
            if (this.ddListKeyValue.SelectedIndex >= this.iRecordCount - 1)
            {
                this.imgBtnNext.Attributes.Add("onclick", "return false;");
            }
            else
            {
                this.imgBtnNext.Attributes.Add("onclick", "ShowWaitingDiv();");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("设置向前向后切换记录的按钮的可用性失败,方法SetPreNextRecordButton");
        }
    }

    /// <summary>
    /// 切换到前一条记录的操作
    /// </summary>
    protected void imgBtnPre_Click(object sender, EventArgs e)
    {
        try
        {
            int iCur = this.ddListKeyValue.SelectedIndex;
            if (iCur > 0)
            {
                this.KEYVALUE = this.ddListKeyValue.Items[iCur - 1].Value;
                this.ddListKeyValue.SelectedIndex = iCur - 1;
                this.RefreshPage();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("切换到前一条记录的操作失败,方法imgBtnPre_Click");
        }
    }
    /// <summary>
    /// 切换到后一条记录的操作
    /// </summary>
    protected void imgBtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            int iCur = this.ddListKeyValue.SelectedIndex;
            if (iCur < this.iRecordCount - 1)
            {
                this.KEYVALUE = this.ddListKeyValue.Items[iCur + 1].Value;
                this.ddListKeyValue.SelectedIndex = iCur + 1;
                this.RefreshPage();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("切换到后一条记录的操作失败,方法imgBtnNext_Click");
        }
    }

    #endregion

    #region 当前模板当前状态下的当前记录，同时创建所有可视控件
    /// <summary>
    /// 创建页面明细内容
    /// </summary>
    private void CreateGroupTabs()
    {
        ArrayList arrListGroup = this.GetCurGroupArrayList(this.TID,this.SID);
        if (arrListGroup == null)
        {
            return;
        }
        int iShowGroupIndex = 0;
        for (int i = 0; i < arrListGroup.Count; i++)
        {
            Entity_TB_HRTMPSG entity = (Entity_TB_HRTMPSG)arrListGroup[i];
            if (entity.GRIGHT.Value < 8)//非不可见时创建页签
            {
                String strGroupId = entity.GID;
                String strGroupType = entity.GTYPE.Value.ToString();
                String strGroupName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strGroupName = entity.GDESCCHS;
                }
                else
                {
                    strGroupName = entity.GDESC;
                }

                DataTable dtRecord = this.GetRecordByTable(this.TID, strGroupId,strGroupType, this.KEY, this.KEYVALUE);

                if (this.strPageType.Equals("0"))//平铺页面
                {
                    this.CreateCheckBoxGroup(strGroupId, strGroupName);
                    this.CreatePanelPage(entity, dtRecord, strGroupId, strGroupName);
                }
                else if (this.strPageType.Equals("1"))//页签页面
                {
                    this.CreateMultiPageTabs(iShowGroupIndex,entity, dtRecord, strGroupId, strGroupName);
                    iShowGroupIndex++;
                }
            }
        }
        this.iShowGroupCount = arrListGroup.Count;
        this.SetGroupVisible(this.iShowGroupCount);

        //设置Edge浏览器兼容时新增设置
        if (this.strPageType.Equals("1"))//页签页面时的默认index
        {
            this.TabStrip1.SelectedIndex = this.iCurTabIndex;
        }

    }

    /// <summary>
    /// 根据显示类型控制页签或者平铺类型的控件是否显示
    /// </summary>
    /// <param name="iShowGroupCount"></param>
    private void SetGroupVisible(int iShowGroupCount)
    {
        if (this.strPageType.Equals("1"))//页签页面
        {
            this.divCheckGroup.Visible = false;
            this.Panel_View.Visible = false;
            this.tbTabs.Visible = true;
            if (iShowGroupCount <= 1)//如果只存在一个页签，则隐藏
            {
                this.TabStrip1.Visible = false;
            }
            else
            {
                this.TabStrip1.Visible = true;
            }
        }
        else//平铺页面
        {
            this.divCheckGroup.Visible = true;
            this.Panel_View.Visible = true;
            this.tbTabs.Visible = false;
            if (iShowGroupCount <= 1)
            {
                this.divCheckGroup.Visible = false;
            }
        }
    }

    /// <summary>
    /// 创建平铺页面类型
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="dtRecord"></param>
    private void CreatePanelPage(Entity_TB_HRTMPSG entity, DataTable dtRecord, String strGroupId, String strGroupName)
    {
        Panel pageView = new Panel();
        pageView = (Panel)this.CreatePageView(pageView, entity, dtRecord);

        //每个分组区域（将包括标题和内容）
        Panel panelOuter = new Panel();
        panelOuter.ID = ServerCtrlIDGetterBll.GetCtrlID_GroupDiv(this.TID,strGroupId);

        //每个分组标题区域
        Panel panelTitle = new Panel();
        panelTitle.ID = ServerCtrlIDGetterBll.GetCtrlID_GroupTitleDiv(this.TID, strGroupId);
        panelTitle.CssClass = "divGroupTitle";
        Table tbTitle = new Table();
        tbTitle.Width = Unit.Percentage(100);
        tbTitle.Height = Unit.Percentage(100);
        TableRow rowTitle = new TableRow();
        rowTitle.Width = Unit.Percentage(100);
        rowTitle.Height = Unit.Percentage(100);
        TableCell cellTitle = new TableCell();
        cellTitle.Width = Unit.Percentage(80);
        cellTitle.Height = Unit.Percentage(100);
        cellTitle.HorizontalAlign = HorizontalAlign.Left;

        System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
        image.ImageUrl = "../../common/images/down_list.gif";
        Label lbTitle = new Label();
        lbTitle.Text = strGroupName;
        lbTitle.ForeColor = Color.Red;
        cellTitle.Controls.Add(image);
        cellTitle.Controls.Add(lbTitle);

        TableCell cellTotop = new TableCell();
        cellTotop.HorizontalAlign = HorizontalAlign.Right;
        cellTotop.Height = Unit.Percentage(100);
        LinkButton lbToTop = new LinkButton();
        lbToTop.Text = "Top";
        lbToTop.ForeColor = Color.Red;
        lbToTop.Attributes.Add("href", "#divTop");
        cellTotop.Controls.Add(lbToTop);

        rowTitle.Controls.Add(cellTitle);
        rowTitle.Controls.Add(cellTotop);

        tbTitle.Controls.Add(rowTitle);
        panelTitle.Controls.Add(tbTitle);

        panelOuter.Controls.Add(panelTitle);
        panelOuter.Controls.Add(pageView);

        this.Panel_View.Controls.Add(panelOuter);
    }

    /// <summary>
    /// 创建页签类型
    /// </summary>
    /// <param name="iShowGroupIndex"></param>
    /// <param name="entity"></param>
    /// <param name="dtRecord"></param>
    /// <param name="strGroupId"></param>
    /// <param name="strGroupName"></param>
    private void CreateMultiPageTabs(int iShowGroupIndex,Entity_TB_HRTMPSG entity, DataTable dtRecord, String strGroupId, String strGroupName)
    {
        if (!base.IsPostBack)
        {
            Tab item = new Tab();
            item.TabIndex = short.Parse(iShowGroupIndex.ToString());
            item.Text = strGroupName;
            item.ID = ServerCtrlIDGetterBll.GetCtrlID_Tab(this.TID, strGroupId);
            item.ToolTip = strGroupName;
            this.TabStrip1.Items.Add(item);
        }
        PageView pageView = new PageView();
        pageView = (PageView)this.CreatePageView(pageView, entity, dtRecord);
        this.MultiPage1.Controls.Add(pageView);
    }

    /// <summary>
    /// 常见选择分组的多选框
    /// </summary>
    /// <param name="strGroupId"></param>
    /// <param name="strGroupName"></param>
    private void CreateCheckBoxGroup(String strGroupId, String strGroupName)
    {
        CheckBox ckBox = new CheckBox();
        ckBox.ID = ServerCtrlIDGetterBll.GetCtrlID_GroupCheckBox(this.TID, strGroupId); ;
        ckBox.Text = strGroupName;
        ckBox.Checked = true;
        ckBox.AutoPostBack = false;
        ckBox.InputAttributes.Add("onclick", "changeGroupCheckBox('" + ckBox.ID + "');");

        ckBox.LabelAttributes.Add("onclick", "locationDiv('" + ServerCtrlIDGetterBll.GetCtrlID_GroupDiv(this.TID, strGroupId) + "');return false;");
        ckBox.LabelAttributes.CssStyle.Add("text-decoration", "underline");
        ckBox.LabelAttributes.CssStyle.Add("cursor", "hand");
        ckBox.LabelAttributes.CssStyle.Add("font-size", "12px");

        this.tdGroupCheckBox.Controls.Add(ckBox);
    }

    /// <summary>
    /// Drop页签
    /// </summary>
    private void DropGroupTab()
    {
        if (!String.IsNullOrEmpty(this.strPageType))
        {
            if (this.strPageType.Equals("0"))//平铺页面
            {
                this.tdGroupCheckBox.Controls.Clear();
                this.Panel_View.Controls.Clear();
            }
            else if (this.strPageType.Equals("1"))//页签页面
            {
                this.MultiPage1.Controls.Clear();
            }
        }
        else//平铺页面
        {
            this.tdGroupCheckBox.Controls.Clear();
            this.Panel_View.Controls.Clear();
        }
    }

    /// <summary>
    /// 创建一个页签视图，返回该页面视图对象
    /// </summary>
    /// <param name="entityHRTMPSG"></param>
    /// <param name="dtRecord"></param>
    /// <returns>PageView</returns>
    private Control CreatePageView(Control pageView, Entity_TB_HRTMPSG entityHRTMPSG, DataTable dtRecord)
    {
        String strTID = entityHRTMPSG.TID;
        String strSID = entityHRTMPSG.SID;
        String strGID = entityHRTMPSG.GID;
        String strType = entityHRTMPSG.GTYPE.ToString();
        pageView.ID = ServerCtrlIDGetterBll.GetCtrlID_PageView(this.TID, strGID);

        switch (strType)
        {
            case "0"://主信息
                this.CreateNormalPage(pageView, entityHRTMPSG, dtRecord);
                break;
            case "1"://常规
                this.CreateNormalPage(pageView, entityHRTMPSG, dtRecord);
                break;
            case "2"://表格集合
            case "3"://列表集合
                this.CreateGridPage(pageView, entityHRTMPSG, dtRecord);
                break;
            //case "3"://列表集合
            //    this.CreateListPage(pageView, entityHRTMPSG, dtRecord);
            //    break;
        }

        return pageView;

    }

    /// <summary>
    /// 创建一个常规页面添加到页签视图，返回该页面视图对象
    /// </summary>
    /// <param name="pageView"></param>
    /// <param name="entityHRTMPSG"></param>
    /// <param name="dtRecord"></param>
    /// <returns>PageView</returns>
    private Control CreateNormalPage(Control pageView, Entity_TB_HRTMPSG entityHRTMPSG, DataTable dtRecord)
    {
        String strTID = entityHRTMPSG.TID;
        String strSID = entityHRTMPSG.SID;
        String strGID = entityHRTMPSG.GID;
        String strType = entityHRTMPSG.GTYPE.ToString();
        this.iTabelCellNumSetting = int.Parse(entityArchiveStyle.CellNum);//页面显示列数
        this.strLabelWidthSetting = entityArchiveStyle.LabelWidth;//label列的宽度

        int iCount = 0;
        String strSql = "select * from TB_HRTMPSD WHERE TID='" + strTID + "' AND SID = '" + strSID + "' AND GID = '" + strGID + "' AND  PRIGHT<2  ORDER BY PORDER";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            iCount = dt.Rows.Count;
            //创建表格
            Table tb = this.CreateHtmlTable(entityHRTMPSG,entityArchiveStyle.TableWidth);
            TableRow row = new TableRow();
            int iRowCount = 0;
            int iRowCellCount = 0;
            int i = 0;
            bool isNeedBuildPreAndAfterCS = false;//是否需要一行显示且前后分隔换行的控件
            bool isBuildPreCS = true;//是否已经在前面换行
            bool isBuildAfterCS = true;//是否已经在后面换行
            while (i < iCount)
            //for (int i = 0; i < iCount; i++)
            {
                //首先创建第一行
                if (i == 0)
                {
                    row = this.CreateTableRow(tb);
                    row.ID = ServerCtrlIDGetterBll.GetCtrlID_Row(entityHRTMPSG.TID, entityHRTMPSG.GID, iRowCount.ToString());
                    iRowCount++;
                    iRowCellCount = 0;
                }
                DataRow dr = dt.Rows[i];
                Entity_TB_HRTMPSD entityHRTMPSD = SetDataToEntityBll.SetDataToEntity_TB_HRTMPSD(dr);

                //需要一行显示且前后分隔换行的控件(多行文本3、宽文本6、富文本控件19)
                if ((entityHRTMPSD.PCTRL.Value == 19) || (entityHRTMPSD.PCTRL.Value == 6))
                {
                    isNeedBuildPreAndAfterCS = true;
                }
                else
                {
                    isNeedBuildPreAndAfterCS = false;
                }

                if (entityHRTMPSD.PTYPE.ToUpper().Equals("CH"))//表示为页眉
                {
                    //如果是页眉且不是第一个字段，则创建行
                    //if (i > 0)
                    //{
                        row = this.CreateTableRow(tb);
                        row.ID = ServerCtrlIDGetterBll.GetCtrlID_Row(entityHRTMPSG.TID, entityHRTMPSG.GID, iRowCount.ToString()) + "_CH";
                        //row.Style.Add("cursor", "hand");
                        //row.Attributes.Add("onclick", "HideRows('" + row.ID.ToString() + "')");//单击隐藏或展开

                        iRowCount++;
                        iRowCellCount = this.iTabelCellNumSetting / 2;
                    //}
                    TableCell cell = this.CreateTableCell(row);
                    //创建页眉label
                    Label lb = this.CreateLabel(cell, entityHRTMPSD, base.Language);
                    lb.Attributes.Add("onclick", "HideRows('" + row.ID.ToString() + "')");//单击隐藏或展开
                    isBuildPreCS = false;
                    isBuildAfterCS = false;
                    i++;
                }
                else if (entityHRTMPSD.PTYPE.ToUpper().Equals("CS"))//表示为分隔符
                {
                    iRowCellCount = this.iTabelCellNumSetting / 2;
                    isBuildPreCS = false;
                    isBuildAfterCS = false;
                    i++;
                }
                else//其他类型
                {
                    //在isNeedBuildPreAndAfterCS=true的控件前面换行
                    if (isNeedBuildPreAndAfterCS)
                    {
                        if (!isBuildPreCS)
                        {
                            iRowCellCount = this.iTabelCellNumSetting / 2;
                            isBuildPreCS = true;
                            continue;
                        }
                        else
                        {
                            isBuildPreCS = false;
                        }
                    }

                    if ((!isNeedBuildPreAndAfterCS) || (isNeedBuildPreAndAfterCS && !isBuildAfterCS))
                    {
                        //创建CELL
                        if (iRowCellCount == this.iTabelCellNumSetting / 2)
                        {
                            row = this.CreateTableRow(tb);
                            row.ID = ServerCtrlIDGetterBll.GetCtrlID_Row(entityHRTMPSG.TID, entityHRTMPSG.GID, iRowCount.ToString());
                            iRowCount++;
                            iRowCellCount = 0;
                        }

                        //新增label列
                        TableCell cell_lable = this.CreateTableCell(row);
                        cell_lable.CssClass = "td_Normal_Label";
                        Label lb = this.CreateLabel(cell_lable, entityHRTMPSD, base.Language);
                        //新增控件列
                        TableCell cell_control = this.CreateTableCell(row);
                        cell_control.CssClass = "td_Normal_Field";
                        //格式化大文本控件时，占一行
                        //多行文本框也占一行 add by sammen 20131119
                        if (isNeedBuildPreAndAfterCS)
                        {
                            cell_control.ColumnSpan = this.iTabelCellNumSetting - 1;
                        }
                        this.CreateWebControlToCell(cell_control, entityHRTMPSD, dtRecord, entityHRTMPSG.GTYPE.ToString());

                        //将所有富文本控件收集起来再进行处理
                        if (entityHRTMPSD.PCTRL.Value == 19)
                        {
                            MyFreeTextBox freeTemp = (MyFreeTextBox)cell_control.Controls[0];
                            this.arrListMyFreeTextBox.Add(freeTemp);

                        }

                        iRowCellCount++;
                    }

                    //在isNeedBuildPreAndAfterCS=true的控件后面换行
                    if (isNeedBuildPreAndAfterCS)
                    {
                        if (!isBuildAfterCS)
                        {
                            iRowCellCount = this.iTabelCellNumSetting / 2;
                            isBuildAfterCS = true;
                            continue;
                        }
                        else
                        {
                            isBuildPreCS = false;
                            isBuildAfterCS = false;
                            i++;
                        }
                    }
                    else
                    {
                        isBuildPreCS = false;
                        isBuildAfterCS = false;
                        i++;
                    }
                }
            }
            pageView.Controls.Add(tb);
        }

        return pageView;

    }

    /// <summary>
    /// 根据记录数和每行显示列数获取显示行数
    /// </summary>
    /// <param name="iRecordCount"></param>
    /// <param name="iCellCount"></param>
    /// <returns></returns>
    private int GetRowsCountByRecordCountACellCount(int iRecordCount, int iCellCount)
    {
        int iDtRowsCount = iRecordCount;//记录数
        int iShowCount = iCellCount;//每行显示列数
        int iHtmlRowsCount = 1;//在页面显示列表图标的行数
        if (iDtRowsCount > iShowCount)
        {
            if (iDtRowsCount % iShowCount > 0)
            {
                iHtmlRowsCount = iDtRowsCount / iShowCount + 1;
            }
            else
            {
                iHtmlRowsCount = iDtRowsCount / iShowCount;
            }
        }
        return iHtmlRowsCount;
    }

    /// <summary>
    /// 创建一个表格集合页面添加到页签视图，返回该页面视图对象
    /// </summary>
    /// <param name="pageView"></param>
    /// <param name="entityHRTMPSG"></param>
    /// <param name="dtRecord"></param>
    /// <returns>PageView</returns>
    private Control CreateGridPage(Control pageView, Entity_TB_HRTMPSG entityHRTMPSG, DataTable dtRecord)
    {
        String strTID = entityHRTMPSG.TID;
        String strSID = entityHRTMPSG.SID;
        String strGID = entityHRTMPSG.GID;
        String strType = entityHRTMPSG.GTYPE.ToString();

        String strSql = "select * from TB_HRTMPSD WHERE TID='" + strTID + "' AND SID = '" + strSID + "' AND GID = '" + strGID + "' AND  PRIGHT<2 AND PLIST = 1 ORDER BY PORDER";
        DataTable dtFiled = SqlParamDao.GetDataTableBySql(strSql);

        Table tb = new Table();
        tb.ID = ServerCtrlIDGetterBll.GetCtrlID_Table(entityHRTMPSG.TID, entityHRTMPSG.GID);
        if (!String.IsNullOrEmpty(this.hfGirdTableName.Value))
        {
            this.hfGirdTableName.Value = this.hfGirdTableName.Value + "*" + tb.ClientID;
        }
        else
        {
            this.hfGirdTableName.Value = tb.ClientID;
        }
        tb.CssClass = "table_archive";
        //设置表格宽度
        int iWidth = 100;
        if ((entityHRTMPSG.GWIDTH != null) && (!String.IsNullOrEmpty(entityHRTMPSG.GWIDTH.ToString())) && (entityHRTMPSG.GWIDTH.Value!=0))
        {
            iWidth = entityHRTMPSG.GWIDTH.Value;
        }
        tb.Width = Unit.Percentage(iWidth);
        //设置Edge浏览器兼容时加上这句
        tb.Style.Add("table-layout", "fixed");
        ////如果当前权限可以新增信息则先增加第一行
        //tb.Controls.Add(this.CreateGridPageFirstRow(strTID, strGID));
        if ((dtFiled != null) && (dtFiled.Rows.Count > 0))
        {
            //创建表格
            DataGrid dg = this.CreateDataGird(entityHRTMPSG, dtFiled, dtRecord);
            if (this.strPageType.Equals("1"))//页签页面
            {
                dg.HeaderStyle.CssClass = "fixGroupGridHeaderStyle";
            }

            TableCell cell = new TableCell();
            cell.CssClass = "td_Normal_Label";
            cell.Controls.Add(dg);
            cell.Width = Unit.Percentage(100);

            TableRow row = new TableRow();
            row.ID = ServerCtrlIDGetterBll.GetCtrlID_Row(entityHRTMPSG.TID, entityHRTMPSG.GID, "0");
            row.Width = Unit.Percentage(100);
            row.Controls.Add(cell);

            tb.Controls.Add(row);

            pageView.Controls.Add(tb);
        }

        return pageView;

    }

    /// <summary>
    /// 创建一个列表集合页面添加到页签视图，返回该页面视图对象
    /// </summary>
    /// <param name="pageView"></param>
    /// <param name="entityHRTMPSG"></param>
    /// <param name="dtRecord"></param>
    /// <returns>PageView</returns>
    private Control CreateListPage(Control pageView, Entity_TB_HRTMPSG entityHRTMPSG, DataTable dtRecord)
    {
        String strTID = entityHRTMPSG.TID;
        String strSID = entityHRTMPSG.SID;
        String strGID = entityHRTMPSG.GID;
        String strType = entityHRTMPSG.GTYPE.ToString();

        return pageView;

    }

    #endregion

    #region 创建表格集合页面相关控件（GTYPE=2）
    /// <summary>
    /// 创建DataGrid
    /// </summary>
    /// <param name="entityHRTMPSG"></param>
    /// <returns></returns>
    private DataGrid CreateDataGird(Entity_TB_HRTMPSG entityHRTMPSG, DataTable dtFiled, DataTable dtRecord)
    {
        String strID = ServerCtrlIDGetterBll.GetCtrlID_DataGrid(entityHRTMPSG.TID, entityHRTMPSG.GID);
        DataGrid dataGrid = WebControlCreateBll.CreateDataGrid(entityHRTMPSG, strID);

        dataGrid.ItemCreated += new DataGridItemEventHandler(dataGrid_ItemCreated);
        dataGrid.ItemDataBound += new DataGridItemEventHandler(dataGrid_ItemDataBound);
        //创建数据列
        dataGrid = this.CreateDataGridColumn(dataGrid, dtFiled);

        //dt排序
        String strGridDataKey = dataGrid.DataKeyField;
        if (!String.IsNullOrEmpty(strGridDataKey))
        {
            DataView dv = dtRecord.DefaultView;
            dv.Sort = strGridDataKey + " Asc";
            DataTable dt2 = dv.ToTable();
            //绑定数据源
            dataGrid.DataSource = dt2;
        }
        else
        {
            //绑定数据源
            dataGrid.DataSource = dtRecord;
        }
        dataGrid.DataBind();

        return dataGrid;
    }

    void dataGrid_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        DataGrid dataGrid = (DataGrid)sender;
        String strGID = ServerCtrlIDGetterBll.GetGIDByCtrlId(dataGrid.ID);
        String strGridKey = dataGrid.DataKeyField;
        String strGRight = dataGrid.Attributes["GRIGHT"].ToString();

        e.Item.Cells[0].Width = Unit.Pixel(36);
        e.Item.Cells[0].HorizontalAlign = HorizontalAlign.Center;

        if (e.Item.ItemType == ListItemType.Header)
        {
            //如果可以新增
            if ((!(this.OPTYPE.ToLower().Equals("readonly")))&&(!String.IsNullOrEmpty(this.KEYVALUE)))//只有当前页面已经存在keyvalue的时候且不是只读的时候才能进行新增
            {
                switch (strGRight)
                {
                    case "4"://增
                    case "5"://增编
                    case "6"://增删
                    case "7"://增删编
                        //单项新增
                        String strParam = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + this.KEYVALUE + "&OPTYPE=" + this.OPTYPE + "&GID=" + strGID + "&GRIDKEY=" + strGridKey + "&GRIDKEYVALUE=";
                        String strUrl = "ArchiveGridRecordDetail.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);
                        ImageButton imageBtn_detail = new ImageButton();
                        imageBtn_detail.ImageUrl = "../../common/images/add.png";
                        imageBtn_detail.ToolTip = "Add";
                        imageBtn_detail.Attributes.Add("onclick", "OpenGridDetailPage('" + strUrl + "');return false;");
                        e.Item.Cells[0].Controls.Clear();
                        e.Item.Cells[0].Controls.Add(imageBtn_detail);
                        e.Item.Cells[0].HorizontalAlign = HorizontalAlign.Left;

                        //批量新增（如果联合主键之一存在数据列表类型控件的字段则可以批量增加）
                        DataTable dt = new DataTable();
                        string strSql = "select top 1 * from TB_HRTMPSD where TID='" + this.TID + "' AND SID='" + this.SID + "' AND GID='" + strGID + "' AND PCTRL = '2' order by PORDER";
                        dt = SqlParamDao.GetDataTableBySql(strSql);
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["PISKEY"].ToString().Equals("1"))
                        {
                            //modify by sammen 20260213 出现批量增加的条件太宽松，当有些非空字段不在select语句字段中时，导致添加保存失败【未完成，先屏蔽】
                            //String strDBList_PCTRLD = dt.Rows[0]["PCTRLD"].ToString();
                            //GetArchiveSettingBll bll = new GetArchiveSettingBll();
                            //Hashtable hsTableRoleParams = bll.GetRoleParamValueByTidARid(this.TID, this.RID, this.GetUserCode(), this.IsAdminstrator());
                            //String strRecordSql_PCTRLD = "select top 1 from (" + ParamOperationBll.ReplacePctrlDSqlParam(strDBList_PCTRLD, "", hsTableRoleParams)+") a ";
                            //DataTable dt_Column_PCTRLD = SqlParamDao.GetDataTableBySql(strRecordSql_PCTRLD);


                            strParam = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + this.KEYVALUE + "&OPTYPE=" + this.OPTYPE + "&GID=" + strGID;
                            strUrl = "ArchiveDetailBatch.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);
                            ImageButton imageBtn_batch = new ImageButton();
                            imageBtn_batch.ImageUrl = "../../common/images/icon/icon_add.gif";
                            imageBtn_batch.ToolTip = "Batch Add";
                            imageBtn_batch.Attributes.Add("onclick", "OpenGridDetailPage('" + strUrl + "');return false;");
                            e.Item.Cells[0].Controls.Add(imageBtn_batch);
                            e.Item.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        }

                        break;
                }
            }

            //设置标题栏不换行
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {
                // e.Item.Cells[x]是当前行中的单元格
                e.Item.Cells[i].Wrap = false;
            }
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            String strGridKeyValue = dataGrid.DataKeys[e.Item.ItemIndex].ToString();

            String strParam = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + this.KEYVALUE + "&OPTYPE=" + this.OPTYPE + "&GID=" + strGID + "&GRIDKEY=" + strGridKey + "&GRIDKEYVALUE=" + strGridKeyValue;
            String strUrl = "ArchiveGridRecordDetail.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);

            ImageButton imageBtn_detail = new ImageButton();
            imageBtn_detail.ImageUrl = "../../common/images/search1.png";
            imageBtn_detail.ToolTip = "detail";
            imageBtn_detail.Attributes.Add("onclick", "OpenGridDetailPage('" + strUrl + "');return false;");

            e.Item.Cells[0].Controls.Clear();
            e.Item.Cells[0].Controls.Add(imageBtn_detail);

            //如果可以删除(非只读状态时)
            if (!(this.OPTYPE.ToLower().Equals("readonly")))
            {
                switch (strGRight)
                {
                    case "2"://删
                    case "3"://删编
                    case "6"://增删
                    case "7"://增删编
                        ImageButton imageBtn_del = new ImageButton();
                        imageBtn_del.ImageUrl = "../../common/images/icon/delete.gif";
                        imageBtn_del.ToolTip = "delete";
                        imageBtn_del.Attributes.Add("onclick", "return DeleteGridDetail('" + this.TID + "','" + strGID + "','" + this.KEY + "','" + this.KEYVALUE + "','" + strGridKey + "','" + strGridKeyValue + "')");

                        e.Item.Cells[0].Controls.Add(imageBtn_del);
                        e.Item.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                        break;
                }
            }

        }
    }

    void dataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataGrid dataGrid = (DataGrid)sender;
            String strGID = ServerCtrlIDGetterBll.GetGIDByCtrlId(dataGrid.ID);
            String strGridKey = dataGrid.DataKeyField;
            String strGridKeyValue = dataGrid.DataKeys[e.Item.ItemIndex].ToString();
            String strGRight = dataGrid.Attributes["GRIGHT"].ToString();

            String strParam = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + this.KEYVALUE + "&OPTYPE=" + this.OPTYPE + "&GID=" + strGID + "&GRIDKEY=" + strGridKey + "&GRIDKEYVALUE=" + strGridKeyValue;
            String strUrl = "ArchiveGridRecordDetail.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);
            e.Item.Attributes.Add("ondblclick", "OpenGridDetailPage('" + strUrl + "');return false;");

            //e.Item.Attributes.Add("onclick", "this.style.backgroundcolor=#fffeee ");
            //if (e.Item.ItemIndex % 2 == 0)//偶数行背景色
            //{
            //    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#E5F1F4");
            //}
            //else//奇数行背景色
            //{
            //    e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8FBFC");
            //}
        }

    }
    /// <summary>
    /// 创建DataGrid各动态列
    /// </summary>
    /// <param name="dataGrid"></param>
    /// <param name="dtFiled"></param>
    private DataGrid CreateDataGridColumn(DataGrid dataGrid, DataTable dtFiled)
    {
        if ((dtFiled != null) && (dtFiled.Rows.Count > 0))
        {
            //创建模板列(第一列)
            TemplateColumn tc = new TemplateColumn();
            //tc.HeaderText = "";
            //tc.HeaderStyle.Width = Unit.Pixel(5);
            //tc.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            //tc.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            dataGrid.Columns.AddAt(0, tc);

            int iCount = dtFiled.Rows.Count;

            String strFieldName = "";
            String strFieldType = "";
            String strHeadText = "";
            String strDataKeyField = "";
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dtFiled.Rows[i];
                strFieldName = dr["PID"].ToString();
                strFieldType = dr["PTYPE"].ToString().ToLower();
                if (this.Language.Equals("zh-cn"))
                {
                    strHeadText = dr["PDESCCHS"].ToString();
                }
                else
                {
                    strHeadText = dr["PDESC"].ToString();
                }
                //获取该定义的主键（除模板主键外的记录主键）
                if ((dr["PISKEY"].ToString().Equals("1")) && (!dr["PID"].ToString().Equals(this.KEY)))
                {
                    strDataKeyField = dr["PID"].ToString();
                }

                BoundColumn tcColumn = new BoundColumn();
                tcColumn.HeaderText = strHeadText;
                tcColumn.SortExpression = strFieldName;
                tcColumn.DataField = strFieldName;
                Double dWidth = Double.Parse(dr["PWIDTH"].ToString());
                if (strFieldType.ToLower().Equals("date"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd}";
                }
                else if (strFieldType.ToLower().Equals("datetime"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd  HH:mm:ss}";
                }
                //tcColumn.HeaderStyle.Width = Unit.Percentage(dWidth);

                dataGrid.Columns.Add(tcColumn);
            }
            if (!String.IsNullOrEmpty(strDataKeyField))
            {
                dataGrid.DataKeyField = strDataKeyField;
            }
            else
            {
                this.AlertMessageBox(this, this.strTipErrorConfig);
                return null;
            }
        }

        return dataGrid;
    }
    #endregion

    #region 获取某模板的某分组所对应表中的记录值

    /// <summary>
    /// 获取当前模板当前场景下的分组列表
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <returns></returns>
    private ArrayList GetCurGroupArrayList(String strTID,String strSID)
    {
        GetArchiveSettingBll bll = new GetArchiveSettingBll();
        ArrayList arrListGroup = new ArrayList();
        if ((this.arrListCurGroups != null) && (this.arrListCurGroups.Count > 0))
        {
            arrListGroup = (ArrayList)this.arrListCurGroups;
        }
        else
        {
            try
            {
                arrListGroup = bll.GetGroupListByTidARid(strTID, strSID);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("获取当前模板当前场景下的分组列表的操作失败，Method：GetCurGroupArrayList");
            }
            this.arrListCurGroups = arrListGroup;
        }
        return arrListGroup;
    }

    /// <summary>
    /// 获取某模板的某分组所对应表中的记录值
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strGID"></param>
    /// <param name="strGroupType"></param>
    /// <param name="strKeyField"></param>
    /// <param name="strKeyValue"></param>
    /// <returns></returns>
    private DataTable GetRecordByTable(String strTID, String strGID,String strGroupType, String strKeyField, String strKeyValue)
    {
        DataTable dt = new DataTable();
        String strTableName = strTID + "_" + strGID;
        //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
        String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(strTID, this.SID, strGID);

        string strSql = "select *  from " + strTableName + " where " + strKeyField + "='" + strKeyValue + "' order by " + strKeyField;
        //如果是列表分组
        if (strGroupType.Equals("2") || strGroupType.Equals("3"))
        {
            //将sql语句中涉及字典表的替换成字典表相应字段
            ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
            strSql = lstdReplace.GetSqlIncludeLSTHDetail(strSql, strTID, this.SID, strGID, this.Language);
        }
        else
        {
            strSql = strSql.Replace("*", strFieldNameString);
        }
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        catch (Exception ex)
        {
            strSql = strSql.Replace("*", strFieldNameString);
            dt = SqlParamDao.GetDataTableBySql(strSql);

            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取某模板的某分组所对应表中的记录值的操作失败，SQL：" + strSql);
        }
        return dt;
    }
    #endregion

    #region 按钮点击操作
    /// <summary>
    /// 返回操作
    /// </summary>
    protected void aBack_Click(object sender, EventArgs e)
    {
        try
        {
            //清除服务器端控件客户端界面选择的值
            this.ClearServerCtrlClientSettingSession();

            //String strParam = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&pageIndex=" + this.strPageIndex + "&pageSize=" + this.strPageSize + "&status" + this.strOpenStatus;
            //strParam = strParam + "&ROLE=" + this.strSingleRole + "&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue + "&sortExp="+this.strSortExp;

            StringBuilder sbParamString = new StringBuilder();
            sbParamString.Append("TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID);
            sbParamString.Append("&pageIndex=" + this.strPageIndex + "&pageSize=" + this.strPageSize + "&status" + this.strOpenStatus);
            sbParamString.Append("&ROLE=" + this.strSingleRole + "&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue + "&sortExp=" + this.strSortExp);
            //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
            sbParamString.Append("&postGoupQueryFilter=" + this.strPostGoupQueryFilter);

            String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
            String strUrl = "../ArchiveMain.aspx?" + strParamString;
            Response.Redirect(strUrl, false);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("返回操作失败,方法aBack_Click");
        }
    }
    /// <summary>
    /// 关闭操作
    /// </summary>
    protected void aClose_Click(object sender, EventArgs e)
    {
        try
        {
            //清除服务器端控件客户端界面选择的值
            this.ClearServerCtrlClientSettingSession();

            Response.Write("<script language=\"javascript\">window.close();</script>");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("关闭操作失败,方法aClose_Click");
        }
    }
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void aSave_Click(object sender, EventArgs e)
    {
        if ((this.strPageType.Equals("0"))&&(this.Panel_View.Controls.Count < 1))//平铺页面
        {
            return;
        }
        if ((this.strPageType.Equals("1")) && (this.MultiPage1.Controls.Count < 1))//页签页面
        {
            return;
        }
        int iGroupCount = this.arrListCurGroups.Count;
        StringBuilder strBuilderSql = new StringBuilder();
        //存储主键名称和主键值
        Hashtable hsTableKey = new Hashtable();
        hsTableKey.Add(this.KEY, this.KEYVALUE);
        //新增时的新keyvalue
        String strKeyValueAdded = ""; 

        int iExcuteDBCount = 0;
        for (int i = 0; i < iGroupCount; i++)
        {
            if ((this.strPageType.Equals("0")) && (this.Panel_View.Controls[i] == null))//平铺页面
            {
                return;
            }
            if ((this.strPageType.Equals("1")) && (this.MultiPage1.Controls[i] == null))//页签页面
            {
                return;
            }

            Entity_TB_HRTMPSG entityHRTMPSG = (Entity_TB_HRTMPSG)arrListCurGroups[i];
            //主信息和常规信息页面才需要保存
            //增加控制：对该状态下该分组的权限不为“只读”“不可见”的情况下才可以保存 add by sammen 20130917
            if ((entityHRTMPSG.GTYPE < 2) && (entityHRTMPSG.GRIGHT != 0) && (entityHRTMPSG.GRIGHT != 8))
            {
                String strGid = entityHRTMPSG.GID;
                String strGroupTableName = ServerCtrlIDGetterBll.GetArchiveTableNameByGroup(this.TID, strGid);
                String strPageViewId = ServerCtrlIDGetterBll.GetCtrlID_PageView(this.TID, strGid);
                String strSql = "";
                //判断当前页签是进行新增还是修改
                bool bIsInsert = ArchiveMainDealBll.JudgeIsCanInsert(strGroupTableName, this.OPTYPE, this.KEY, this.KEYVALUE);

                ArrayList arrListObject = new ArrayList();

                //获取当前页签中的所有控件值
                //首先获取pageview对象获取Panel对象
                Control pageView = new Control();
                if (this.strPageType.Equals("0"))//平铺页面
                {
                    Panel panelOuter = (Panel)this.Panel_View.Controls[i];
                    pageView = (Panel)panelOuter.FindControl(ServerCtrlIDGetterBll.GetCtrlID_PageView(this.TID, strGid));
                }
                else if (this.strPageType.Equals("1"))//页签页面
                {
                    pageView = (PageView)this.MultiPage1.Controls[i];
                }

                int iPvControlCount = pageView.Controls.Count;
                for (int j = 0; j < iPvControlCount; j++)
                {
                    //在获取pageview下的table对象
                    Table tb = (Table)pageView.Controls[j];
                    int iTbRows = 0;

                    //table中遍历每一行
                    while (iTbRows < tb.Rows.Count)
                    {
                        for (int cellCount = 1; cellCount < tb.Rows[iTbRows].Cells.Count; cellCount += 2)
                        {
                            TableCell cellTemp = tb.Rows[iTbRows].Cells[cellCount];//取控件列

                            Entity_HRLOG_2 entityLog2 = new Entity_HRLOG_2();//日志2对象
                            try
                            {
                                arrListObject = this.GetCtrlValueInCell(cellTemp, strGid, arrListObject, ref entityLog2);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                log.Error("保存档案信息，获取CELL中控件的值失败,控件列：" + cellTemp.ID);
                            }
                            if (arrListObject == null)
                            {
                                this.hfIsSaveSuccess.Value = "0";
                                return;
                            }
                            //准备需写日志的数据
                            this.GetDataLogDataInfo(entityLog2);
                        }
                        iTbRows++;
                        
                    }
                }
                if ((arrListObject != null) && (arrListObject.Count > 0))
                {
                    //最后处理自增字段的值(最后处理是为了在验证失败后不获取自增值，以保证自增值的流水号的连续性)
                    ///add by sammen 20140327
                    for (int j = 0; j < arrListObject.Count; j++)
                    {
                        Entity_ToDBObject entityDBObject1 = (Entity_ToDBObject)arrListObject[j];
                        if (entityDBObject1.FIELDTYPE.ToLower().Equals("ints") || (entityDBObject1.FIELDTYPE.ToLower().Equals("intc")))
                        {
                            String strFieldValue = this.GetIncreaceNo(entityDBObject1.GID, entityDBObject1.FIELDNAME, entityDBObject1.FIELDTYPE, entityDBObject1.GROUPTYPE, entityDBObject1.FIELDVALUE_NEW);
                            entityDBObject1.FIELDVALUE_NEW = strFieldValue;
                            arrListObject.RemoveAt(j);
                            arrListObject.Insert(j, entityDBObject1);
                        }
                    }

                    if (bIsInsert)
                    {
                        if (entityHRTMPSG.GTYPE.ToString().Equals("0"))
                        {
                            //获取新增的主键值
                            strKeyValueAdded = ArchiveMainDealBll.GetKeyValueAdded(arrListObject,this.KEY);
                        }else{
                            if (!String.IsNullOrEmpty(strKeyValueAdded))
                            {
                                //将其他分组中的主键字段设置为新键值
                                arrListObject = ArchiveMainDealBll.SetOtherGroupNewKeyValue(arrListObject, this.KEY, strKeyValueAdded);
                            }
                        }
                        strSql = ParamSqlStringGetterBll.GetInsertSqlString(strGroupTableName, arrListObject);
                    }
                    else
                    {
                        strSql = ParamSqlStringGetterBll.GetUpdateSqlString(strGroupTableName, arrListObject, hsTableKey);
                    }
                }
                
                if (!String.IsNullOrEmpty(strSql))
                {
                    strBuilderSql.Append(strSql + ";\r\n");
                }
                else
                {
                    break;
                }
            }
        }
        //根据SQL语句更新到数据库
        if ((strBuilderSql != null) && (!String.IsNullOrEmpty(strBuilderSql.ToString())))
        {
            try
            {
                String strIsHis = "0";
                if (this.IsHis)
                {
                    strIsHis = "1";
                }
                ArchiveActionBll bllAction = new ArchiveActionBll();
                if (this.OPTYPE.Equals("add"))
                {
                    //新增前检查，主键值是否重复
                    if (!ArchiveMainDealBll.IsHadKeyValue(this.strCurMainTableName, this.KEY, strKeyValueAdded))
                    {
                        //检查通过后，则进行新增操作
                        iExcuteDBCount = ArchiveMainDealBll.AddArchiveData(strBuilderSql, this.TID, this.RID, this.SID, strKeyValueAdded, this.GetUserCode(), this.IsAdminstrator(), strIsHis);

                        this.KEYVALUE = strKeyValueAdded;
                    }
                    else
                    {
                        this.hfIsSaveSuccess.Value = "0";
                        base.AlertMessageBox(this, this.strTipExsitSameKeyValue + "：" + this.KEY);
                        return;
                    }
                }
                else
                {
                    iExcuteDBCount = ArchiveMainDealBll.ModifyArchiveData(strBuilderSql, this.TID, this.RID, this.SID, this.KEYVALUE, this.GetUserCode(), this.IsAdminstrator(), strIsHis);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("保存模板数据时，根据SQL语句更新到数据库失败，SQL：" + strBuilderSql.ToString());
            }

            if (iExcuteDBCount > 0)
            {
                this.hfIsSaveSuccess.Value = "1";
                //this.OPTYPE = "edit";// modify by sammen 20170713 此行先执行再执行SuccessfullySaved delete by sammen 20220421 因为在此执行造成没有新增的日志写入
                this.SuccessfullySaved();
            }
            else
            {
                this.hfIsSaveSuccess.Value = "0";
                base.AlertMessageBox(this, this.strTipSaveFailed);
            }
        }
        else
        {
            this.hfIsSaveSuccess.Value = "0";
        }
    }

    /// <summary>
    /// 保存成功后操作
    /// <param name="isClose">是否关闭当前窗口</param>
    /// </summary>
    private void SuccessfullySaved()
    {
        //写操作日志
        this.WriteDataLog(this.arrayObjectHRLOG2, this.OPTYPE);
        // add by sammen 20220421 写完日志后，设置为编辑模式
        this.OPTYPE = "edit";
        //重新刷新页面
        this.RefreshPage();
        //清除服务器端控件客户端界面选择的值
        this.ClearServerCtrlClientSettingSession();

        //保存成功后的处理
        bool isCurPageOpen = false;
        bool isClose = false;
        if (this.hfIsOpenAtCurPage.Value.Equals("1"))//在当前页面打开的情况
        {
            isCurPageOpen = true;
        }
        if (this.entityArchiveStyle.IsCloseAfterSaved.Equals("1"))//配置文件获取保存后是否关闭的标志位
        {
            isClose = true;
        }
        this.DealParantPageAfterSuccessSave(isCurPageOpen, isClose);
        
    }

    /// <summary>
    /// 保存成功后对父窗口页面的操作
    /// <param name="isClose">是否关闭当前窗口</param>
    /// </summary>
    private void DealParantPageAfterSuccessSave(bool isCurPageOpen,bool isClose)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("<script language=\"javascript\">\r\n");
        sb.Append("    if(window.opener!=null){\r\n");
        sb.Append("        if(window.opener.document.getElementById('aRefresh')!=null){\r\n");
        sb.Append("           window.opener.document.getElementById('aRefresh').click();\r\n");
        sb.Append("        }\r\n");
        sb.Append("    }\r\n");
        if (isClose)
        {
            if (isCurPageOpen)
            {
                //这一句可能会影响FreeTextBox控件的提交操作。
                sb.Append("    window.document.getElementById('aBack').click();\r\n");
            }
            else
            {
                sb.Append("    window.close();\r\n");
            }
        }
        //else
        //{
        //    //add by sammen 20130701 防止新增后停留在当前页面时，右键刷新后有新增一条
        //    sb.Append("    window.document.getElementById('aRefreshDetail').click();\r\n");
        //}
        sb.Append("    alert('" + this.strTipSaveSuccess + "');\r\n");
        if (this.hfIsAutoSave.Value.Equals("1"))//如果是自动保存，则成功提示后直接进入动作操作页面
        {
            sb.Append("    doExcuteAction();\r\n");
        }
        sb.Append("</script>\r\n");

        Page.ClientScript.RegisterStartupScript(typeof(Page), this.strTipSaveSuccess, sb.ToString());
    }

    /// <summary>
    /// 重新加载数据
    /// </summary>
    protected void aRefresh_Click(object sender, EventArgs e)
    {
        this.RefreshPage();
    }
    
    #endregion

    #region 写操作日志区域
    /// <summary>
    /// 准备需写日志的数据
    /// </summary>
    /// <param name="arrListObject"></param>
    private void GetDataLogDataInfo(Entity_HRLOG_2 entityLog2)
    {
        if (!String.IsNullOrEmpty(entityLog2.PID))
        {
            this.arrayObjectHRLOG2.Add(entityLog2);
        }
    }

    /// <summary>
    /// 写入操作日志(增改查)
    /// </summary>
    /// <param name="arrayObjectHRLOG2">修改记录信息(可为空)</param>
    /// <param name="strOpType">页面操作类型，查看新增删除</param>
    private void WriteDataLog(ArrayList arrayObjectHRLOG2,String strOpType)
    {
        try
        {
            if ((Session["ArchiveIsLog"] != null) && (Session["ArchiveIsLog"].ToString().Equals("1")))
            {
                if ((arrayObjectHRLOG2 != null) && (arrayObjectHRLOG2.Count > 0))
                {
                    if (strOpType.Equals("add"))
                    {
                        DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Add, this.KEYVALUE, arrayObjectHRLOG2);
                    }
                    else if (strOpType.Equals("edit"))
                    {
                        DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Modify, this.KEYVALUE, arrayObjectHRLOG2);
                    }
                }
                else
                {
                    if (strOpType.Equals("readonly"))
                    {
                        Entity_HRLOG_2 entityLog2_View = new Entity_HRLOG_2();//日志2对象
                        entityLog2_View = DataLogWriter.SetDataToEntity_HRLOG_2(this.TID, null, null, null, null);

                        DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_View, this.KEYVALUE, entityLog2_View);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("写入操作日志(增改查)失败,方法WriteDataLog");
        }
    }

    #endregion

    #region Ajax方法区域
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strCtrlId"></param>
    /// <param name="strValue"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public void CreateGridDetailPageCtrl()
    {
        try
        {

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveDetailAjax.CreateGridDetailPageCtrl() Error！");
        }
    }
    #endregion

    #region 动态加载页面中的客户端事件
    /// <summary>
    /// 动态加载页面中的客户端事件
    /// </summary>
    /// <param name="strTid"></param>
    /// <param name="strSid"></param>
    private void BuildClientEventScript(String strTid,String strSid)
    {
        StringBuilder strBuilderContent = new StringBuilder();
        if ((String.IsNullOrEmpty(strTid)) || (String.IsNullOrEmpty(strSid)))
        {
            return ;
        }
        this.divClientEvent.InnerHtml = ArchiveEventBll.BuildClientEventScript(strTid, strSid,this.GetBrowserType());
    }

    #endregion

    #region 加载明细页面前后执行存储过程 add by sammen 20140626
    /// <summary>
    /// 加载明细数据前执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doBeforeLoadDetail()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_BeforeLoadDetail(hsTableParam);
        return iCount;
    }

    /// <summary>
    /// 加载明细数据后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterLoadDetail()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_AfterLoadDetail(hsTableParam);
        return iCount;
    }
    #endregion

    /// <summary>
    /// 资产系统中，判断如果当前系统时间不处于当前月份，则进行提示
    /// add by sammen 20160726
    /// </summary>
    private void SetMonthlyNotSettleNotice()
    {
        try
        {
            String strNeedDealTID = BaseConfig.Instance.GetConfigValueByKey("Monthly_Notice_TID");
            if (strNeedDealTID.IndexOf(this.TID + ";") > -1)
            {
                String strCurDateTime = DateTime.Now.ToString("yyyy-MM-dd");
                String strReturnDateTime = this.GetCurMonthlyLastDateTime();
                String strReturnMonth = strReturnDateTime.Substring(0, 7);
                if (!strCurDateTime.Equals(strReturnDateTime.Substring(0,10)))
                {
                    //如果日期不等，则判断为当前系统时间不处于当前月份
                    String strNotice = "警告：月份(" + strReturnMonth + ")尚未月结!\\n系统操作日期将统一为：" + strReturnDateTime;
                    if (this.Language.Equals("en-us"))
                    {
                        strNotice = "Warnning：Month(" + strReturnMonth + ")Had not settled!\\nNow system operation time must be：" + strReturnDateTime;
                    }
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alertNotice", "<script language=\"javascript\">alert('" + strNotice + "');</script>");
                    //Response.Write("<script language=\"javascript\">window.close();</script>");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }


    /// <summary>
    /// 设置新增单据记录按钮事件
    /// add by sammen 20170215
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SetAddClick()
    {
        //String strParamString = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + "" + "&OPTYPE=add" + "&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue;
        //strParamString = strParamString + "&pageIndex=" + this.strPageIndex.ToString() + "&pageSize=" + this.strPageSize.ToString() + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;

        StringBuilder sbParamString = new StringBuilder();
        sbParamString.Append("TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + "" + "&OPTYPE=add");
        sbParamString.Append("&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue);
        sbParamString.Append("&pageIndex=" + this.strPageIndex + "&pageSize=" + this.strPageSize + "&status" + this.strOpenStatus + "&ROLE=" + this.strSingleRole);
        //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
        sbParamString.Append("&postGoupQueryFilter=" + this.strPostGoupQueryFilter);

        String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
        this.aAddDetail.Attributes.Add("onclick", "AddDetail('" + strParamString + "','" + "0" + "');return false;");
    }

    protected void TabStrip1_SelectedIndexChange(object sender, EventArgs e)
    {
        //设置Edge浏览器兼容时新增设置
        this.iCurTabIndex = this.TabStrip1.SelectedIndex;
    }
}
