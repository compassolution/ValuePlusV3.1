using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Resources;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common;

using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Utils;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.Archive.Property;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class Archive_ArchiveMain : PageBase
{
    private DataSet dsCurRole = new DataSet();
    private DataSet dsCurScene = new DataSet();
    private Dictionary<string, string> dicTypeChangeCol = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
        this.DoCreateDataGridColumn();
        this.BuildGroupSearchArea(this.dtMasterLists);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Archive_ArchiveMainAjax), this.Page);

        //modify by sammen 20181130 使用相对地址，兼容https及外网地址映射的需求
        String strExportPage = "../Export/exportindex.aspx";
        this.aExportExcel.Attributes.Add("onclick", "javascript:exportexcel('" + strExportPage + "');return false;");
        //this.aExportExcel.Attributes.Add("onclick", "javascript:exportexcel('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");

        if (!Page.IsPostBack)
        {
            try
            {
                //清空导出excel相关的session
                Session["ExportDsViewDataViewState"] = null;
                Session["ExportDsViewDataViewState_Sql"] = null;
                Session["ExportDsViewDataViewState_Title"] = null;
                //增加三项导出excel时涉及的参数 add by sammen 20191226
                //【主要为了解决：1模板列表导出时列名导出名称；2隐藏的字段不显示】
                Session["ExportDsViewDataViewState_TID"] = null;
                Session["ExportDsViewDataViewState_SID"] = null;
                Session["ExportDsViewDataViewState_GID"] = null;

                //获取页面传递的参数
                this.GetRequestParam();
                //根据模板获取定义信息
                this.GetInfo_TB_HRTMPH(this.TID);
                //获取模板样式配置
                this.entityArchiveStyle = ArchiveStyleGetterBll.GetTemplateStylesEntity(this.TID);
                //页面设置
                this.PageSetting();

                //获取DataGrid数据的DataSet
                this.DataGridSetting_First();
                //设置获取DataGrid分页部分的数据及显示
                this.SetDataGridPageArea();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
    }

    //protected void Page_UnLoad(object sender, EventArgs e)
    //{
    //    //清空导出excel相关的session
    //    Session["ExportDsViewDataViewState"] = null;
    //    Session["ExportDsViewDataViewState_Sql"] = null;
    //    Session["ExportDsViewDataViewState_Title"] = null;
    //    Session["ExportDsViewDataViewState_TableName"] = null;
    //    Session["ExportDsViewDataViewState_TID"] = null;
    //    Session["ExportDsViewDataViewState_SID"] = null;
    //}

    #region viewstate初始化区域
    public string TID
    {
        get
        {
            return ViewState["TID_ViewState"] as string;
        }
        set
        {
            ViewState["TID_ViewState"] = value;
        }
    }
    public string RID
    {
        get
        {
            return ViewState["RID_ViewState"] as string;
        }
        set
        {
            ViewState["RID_ViewState"] = value;
        }
    }
    public string SID
    {
        get
        {
            return ViewState["SID_ViewState"] as string;
        }
        set
        {
            ViewState["SID_ViewState"] = value;
        }
    }
    public string KEY
    {
        get
        {
            return ViewState["KEY_ViewState"] as string;
        }
        set
        {
            ViewState["KEY_ViewState"] = value;
        }
    }
    public string KEYTYPE
    {
        get
        {
            return ViewState["KEYTYPE_ViewState"] as string;
        }
        set
        {
            ViewState["KEYTYPE_ViewState"] = value;
        }
    }
    private string strEdit
    {
        get
        {
            return ViewState["strEdit"] as string;
        }
        set
        {
            ViewState["strEdit"] = value;
        }
    }
    private string strDelete
    {
        get
        {
            return ViewState["strDelete"] as string;
        }
        set
        {
            ViewState["strDelete"] = value;
        }
    }
    private string strConfirm
    {
        get
        {
            return ViewState["strConfirm"] as string;
        }
        set
        {
            ViewState["strConfirm"] = value;
        }
    }
    private string strView
    {
        get
        {
            return ViewState["strView"] as string;
        }
        set
        {
            ViewState["strView"] = value;
        }
    }
    private string strRefcolumn
    {
        get
        {
            return ViewState["strRefcolumn"] as string;
        }
        set
        {
            ViewState["strRefcolumn"] = value;
        }
    }
    private string strTipHaveRole
    {
        get
        {
            return ViewState["strTipHaveRole"] as string;
        }
        set
        {
            ViewState["strTipHaveRole"] = value;
        }
    }
    private string strTipHaveScene
    {
        get
        {
            return ViewState["strTipHaveScene"] as string;
        }
        set
        {
            ViewState["strTipHaveScene"] = value;
        }
    }
    private string strTipHaveAction
    {
        get
        {
            return ViewState["strTipHaveAction"] as string;
        }
        set
        {
            ViewState["strTipHaveAction"] = value;
        }
    }
    private string strTipShowDetail
    {
        get
        {
            return ViewState["strTipShowDetail"] as string;
        }
        set
        {
            ViewState["strTipShowDetail"] = value;
        }
    }
    private string strTipDeleteSuccess
    {
        get
        {
            return ViewState["strTipDeleteSuccess"] as string;
        }
        set
        {
            ViewState["strTipDeleteSuccess"] = value;
        }
    }
    private string strTipDeleteFailed
    {
        get
        {
            return ViewState["strTipDeleteFailed"] as string;
        }
        set
        {
            ViewState["strTipDeleteFailed"] = value;
        }
    }
	
    private Hashtable hsCurRoleParamValue
    {
        get
        {
            if (this.ViewState["hsCurRoleParamValue"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)this.ViewState["hsCurRoleParamValue"];
        }
        set
        {
            this.ViewState["hsCurRoleParamValue"] = value;
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
    private string strCurUserCode
    {
        get
        {
            return ViewState["strCurUserCode"] as string;
        }
        set
        {
            ViewState["strCurUserCode"] = value;
        }
    }
    private bool IsHasAction
    {
        get
        {
            if (ViewState["IsHasAction"] != null)
            {
                return (bool)ViewState["IsHasAction"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsHasAction"] = value;
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

    public DataSet dsGridList
    {
        get
        {
            if (this.ViewState["dsGridList"] == null)
            {
                return new DataSet();
            }
            return (DataSet)this.ViewState["dsGridList"];
        }
        set
        {
            this.ViewState["dsGridList"] = value;
        }
    }
    private string strSqlAddtion
    {
        get
        {
            if (this.ViewState["strSqlAddtion"] == null)
            {
                return null;
            }
            return this.ViewState["strSqlAddtion"].ToString();
        }
        set
        {
            this.ViewState["strSqlAddtion"] = value;
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

    private string strMainGroupID
    {
        get
        {
            return ViewState["strMainGroupID"] as string;
        }
        set
        {
            ViewState["strMainGroupID"] = value;
        }
    }
    private string strMainGview
    {
        get
        {
            return ViewState["strMainGview"] as string;
        }
        set
        {
            ViewState["strMainGview"] = value;
        }
    }
    private string strMainGsql
    {
        get
        {
            return ViewState["strMainGsql"] as string;
        }
        set
        {
            ViewState["strMainGsql"] = value;
        }
    }

    private string strSSLCT
    {
        get
        {
            return ViewState["strSSLCT"] as string;
        }
        set
        {
            ViewState["strSSLCT"] = value;
        }
    }
    private int iAddFlag
    {
        get
        {
            if (this.ViewState["iAddFlag"] != null)
            {
                return (int)this.ViewState["iAddFlag"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iAddFlag"] = value;
        }
    }
    private int iDelFlag
    {
        get
        {
            if (this.ViewState["iDelFlag"] != null)
            {
                return (int)this.ViewState["iDelFlag"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iDelFlag"] = value;
        }
    }
    private int iEditFlag
    {
        get
        {
            if (this.ViewState["iEditFlag"] != null)
            {
                return (int)this.ViewState["iEditFlag"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iEditFlag"] = value;
        }
    }
    private int iSSize
    {
        get
        {
            if (this.ViewState["iSSize"] != null)
            {
                return (int)this.ViewState["iSSize"];
            }
            return 1;
        }
        set
        {
            this.ViewState["iSSize"] = value;
        }
    }
    private int iCformFlag
    {
        get
        {
            if (this.ViewState["iCformFlag"] != null)
            {
                return (int)this.ViewState["iCformFlag"];
            }
            return 1;
        }
        set
        {
            this.ViewState["iCformFlag"] = value;
        }
    }
    private string strQueryModeFlag
    {
        get
        {
            if (this.ViewState["strQueryModeFlag"] == null)
            {
                return "0";
            }
            return this.ViewState["strQueryModeFlag"].ToString();
        }
        set
        {
            this.ViewState["strQueryModeFlag"] = value;
        }
    }

    private bool IsCreatedGridCol
    {
        get
        {
            if (ViewState["IsCreatedGridCol"] != null)
            {
                return (bool)ViewState["IsCreatedGridCol"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsCreatedGridCol"] = value;
        }
    }
    private bool IsSortAscending
    {
        get
        {
            object obj2 = this.ViewState["IsSortAscending"];
            return ((obj2 == null) || ((bool)obj2));
        }
        set
        {
            this.ViewState["IsSortAscending"] = value;
        }
    }

    private int iPageIndex
    {
        get
        {
            if (ViewState["iPageIndex"] != null)
            {
                return (int)ViewState["iPageIndex"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iPageIndex"] = value;
        }
    }
    private int iPageSize
    {
        get
        {
            if (ViewState["iPageSize"] != null)
            {
                return (int)ViewState["iPageSize"];
            }
            else
            {
                return 10;
            }
        }
        set
        {
            ViewState["iPageSize"] = value;
        }
    }
    private int iPageCount
    {
        get
        {
            if (this.ViewState["iPageCount"] == null)
            {
                return 0;
            }
            return (int)this.ViewState["iPageCount"];
        }
        set
        {
            this.ViewState["iPageCount"] = value;
        }
    }
    private int iPageCurNum
    {
        get
        {
            if (this.ViewState["iPageCurNum"] == null)
            {
                return 1;
            }
            return (int)this.ViewState["iPageCurNum"];
        }
        set
        {
            this.ViewState["iPageCurNum"] = value;
        }
    }
    private int iRecordCount
    {
        get
        {
            if (this.ViewState["iRecordCount"] == null)
            {
                return 0;
            }
            return (int)this.ViewState["iRecordCount"];
        }
        set
        {
            this.ViewState["iRecordCount"] = value;
        }
    }

    public DataTable dtMasterLists
    {
        get
        {
            if (this.ViewState["dtMasterLists"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtMasterLists"];
        }
        set
        {
            this.ViewState["dtMasterLists"] = value;
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

    public string strColPosition
    {
        get
        {
            if (this.ViewState["strColPosition"] == null)
            {
                return null;
            }
            return this.ViewState["strColPosition"].ToString();
        }
        set
        {
            this.ViewState["strColPosition"] = value;
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

    public DataTable dtGroupQuerySettingList
    {
        get
        {
            if (this.ViewState["dtGroupQuerySettingList"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtGroupQuerySettingList"];
        }
        set
        {
            this.ViewState["dtGroupQuerySettingList"] = value;
        }
    }

    public String strJArrayGroupQuery
    {
        get
        {
            if (this.ViewState["strJArrayGroupQuery"] == null)
            {
                return "";
            }
            return this.ViewState["strJArrayGroupQuery"].ToString(); ;
        }
        set
        {
            this.ViewState["strJArrayGroupQuery"] = value;
        }
    }

    private string strPostGroupQueryFilter
    {
        get
        {
            if (this.ViewState["strPostGroupQueryFilter"] == null)
            {
                return null;
            }
            return this.ViewState["strPostGroupQueryFilter"].ToString();
        }
        set
        {
            this.ViewState["strPostGroupQueryFilter"] = value;
        }
    }
    #endregion

    #region 获取页面传递的参数
    /// <summary>
    /// 获取页面传递的参数
    /// </summary>
    private void GetRequestParam()
    {
        try
        {
            ////解密传递字符串并获取对应参数值
            Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            this.TID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
            this.RID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
            this.SID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
            this.strOpenStatus = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "status");//列表显示状态模式，select表示为默认查询状态
            this.strSingleRole = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "ROLE");//角色编码，由配置连接提供
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageIndex")))
            {
                this.iPageIndex = int.Parse(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageIndex"));
            }
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageSize")))//add by sammen 20140327
            {
                this.iPageSize = int.Parse(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pageSize"));
            }
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sqlAddtion")))//add by sammen 20130109
            {
                this.strSqlAddtion = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sqlAddtion").ToString();
            }
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "searchValue")))//add by sammen 20130109
            {
                this.strSearchValue = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "searchValue").ToString();
                this.txtSimpleSearchValue.Text = this.strSearchValue;
            }
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sortExp")))//add by sammen 20210516
            {
                this.strSortExp = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sortExp").ToString();
            }
            //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
            if (!String.IsNullOrEmpty(UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postGoupQueryFilter")))
            {
                this.strPostGroupQueryFilter = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postGoupQueryFilter").ToString();
                //add by sammen 20250121 防止查询条件里存在&等特殊字符而进行的加解密
                this.strPostGroupQueryFilter = UrlParamEncryption.Decrypt3des(this.strPostGroupQueryFilter, System.Text.Encoding.UTF8);
            }
            else
            {
                this.strSortExp = "";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            base.AlertMessageBox(this, "Request Url Params Error！");
        }
        //如果获取的场景值为空，则设置为该角色下的第一个场景
        if (String.IsNullOrEmpty(this.SID))
        {
            this.SID = this.GetRoleFirstScene(this.TID, this.RID);
        }
    }
    #endregion

    #region 根据模板获取定义信息
    /// <summary>
    /// 根据模板获取定义信息
    /// </summary>
    /// <param name="strRole"></param>
    /// <param name="strTemplate"></param>
    ///<param name="iRoleIndex"></param>
    private void GetInfo_TB_HRTMPH(String strTemplate)
    {
        string strSql = "SELECT * FROM TB_HRTMPH WHERE TID='" + strTemplate + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (this.Language.Equals("zh-cn"))
            {
                Session["ArchiveDesc"] = ds.Tables[0].Rows[0]["TDESCCHS"].ToString();
            }
            else
            {
                Session["ArchiveDesc"] = ds.Tables[0].Rows[0]["TDESC"].ToString();
            }
        }

    }
    #endregion

    #region 页面设置及数据准备
    /// <summary>
    /// 页面设置及数据准备
    /// </summary>
    private void PageSetting()
    {
        this.txtSimpleSearchValue.Attributes.Add("onkeypress", "EnterSimpleSearchTextBox()");
        this.txtPageSize.Attributes.Add("onkeypress", "EnterPageSizeTextBox()");

        //页面语言设置以及基础设置
        this.DoLanguageSetting();
        //设置角色列表
        this.RoleListSetting(this.TID);

        //选择某一角色前执行 add by sammen 20140626
        this.doBeforeLoadRole();

        //通过TID获取当前角色下所设置的参数值
        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
        this.hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(this.TID, this.RID, base.GetUserCode(), this.IsAdminstrator());

        //选择某一角色后执行 add by sammen 20140626
        this.doAfterLoadRole();

        //设置场景列表
        this.SceneListSetting(this.TID, this.RID);

        //选择某一状态前执行 add by sammen 20140626
        this.doBeforeLoadSence();

        //根据单据编码及当前场景编码获取对应的场景记录,并设置相应全局变量
        this.DoGetSenceParamFlag(this.TID, this.SID);

        //选择某一状态后执行 add by sammen 20140626
        this.doAfterLoadSence();

        //设置动作列表
        this.ActionListSetting(this.TID, this.SID);
        //根据角色和场景的个数隐藏对应区域
        this.HiddenRoleAndScene(this.dsCurRole, this.dsCurScene);

        //根据参数strOpenStatus设置页面相应区域
        this.SettingByOpenState();

        //设置新增按钮属性
        this.SetAddClick();

        //根据单据编码获取模板分组表TB_HRTMPG中主信息的数据
        this.DoGetGroupInfo(this.TID);
        //获取主显示的列名的DataTable
        this.DoGetMasterListDataTable(this.TID, this.SID, this.strMainGroupID);
        //加载查询区域
        this.DoBuildSimpleConditionDDList(this.dtMasterLists);
        this.BuildGroupSearchArea(this.dtMasterLists);
    }

    /// <summary>
    /// 页面语言设置以及基础设置
    /// </summary>
    private void DoLanguageSetting()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("Archive");

        if (this.Language.Equals("zh-cn"))
        {
            this.Page.Title = Session["ArchiveDesc"] + "列表";
        }
        else
        {
            this.Page.Title = Session["ArchiveDesc"] + " List";
        }

        this.Label_Add.Text = rmLocResourceManager.GetString("btnNew");
        this.Label_Operation.Text = rmLocResourceManager.GetString("btnOperation");
        this.Label_Refresh.Text = rmLocResourceManager.GetString("btnRefresh");
        this.Label_Search.Text = rmLocResourceManager.GetString("btnSimpleSearch");
        this.Label_ShowGroupSearch.Text = rmLocResourceManager.GetString("btnGroupSearch");
        this.Label_ShowDataList.Text = rmLocResourceManager.GetString("btnShowDataList");
        this.Label_FirstPage.Text = rmLocResourceManager.GetString("aFirstPage");
        this.Label_PrePage.Text = rmLocResourceManager.GetString("aPrePage");
        this.Label_NextPage.Text = rmLocResourceManager.GetString("aNextPage");
        this.Label_LastPage.Text = rmLocResourceManager.GetString("aLastPage");
        this.Label_Query.Text = rmLocResourceManager.GetString("btnGroupSearch");
        this.Label_Reset.Text = rmLocResourceManager.GetString("btnGroupReset");
        this.Label_ExportExcel.Text = rmLocResourceManager.GetString("btnExportExcel");

        this.Label_Page1.Text = rmLocResourceManager.GetString("lbPage1");
        this.Label_Page2.Text = rmLocResourceManager.GetString("lbPage2");
        this.Label_Page3.Text = rmLocResourceManager.GetString("lbPage1");
        this.Label_Page4.Text = rmLocResourceManager.GetString("lbPage3");
        this.Label_Page5.Text = rmLocResourceManager.GetString("lbPage4");
        this.Label_Page6.Text = rmLocResourceManager.GetString("lbPage5");

        this.strDelete = rmLocResourceManager.GetString("tipDelete");
        this.strConfirm = rmLocResourceManager.GetString("tipSureDelete");
        this.strTipHaveRole = rmLocResourceManager.GetString("tipHaveRole");
        this.strTipHaveScene = rmLocResourceManager.GetString("tipHaveScene");
        this.strTipHaveAction = rmLocResourceManager.GetString("tipHaveAction");
        this.strTipShowDetail = rmLocResourceManager.GetString("tipShowDetail");

        this.strTipDeleteSuccess = rmLocResourceManager.GetString("tipDeleteSuccess");
        this.strTipDeleteFailed = rmLocResourceManager.GetString("tipDeleteFailed");
    }

    /// <summary>
    /// 根据单据编码获取模板分组表TB_HRTMPG中主信息的数据
    /// </summary>
    /// <param name="strDocuName"></param>
    private void DoGetGroupInfo(string strDocuName)
    {
        //根据单据编码获取模板分组表TB_HRTMPG中主信息的数据
        DataSet ds = SqlParamDao.GetDataSetBySql("SELECT GID,GVIEW,GSQL ,islarge FROM TB_HRTMPG WHERE TID='" + strDocuName + "' AND GTYPE=0");
        this.strMainGroupID = ds.Tables[0].Rows[0][0].ToString();//分组编码
        this.strMainGview = ds.Tables[0].Rows[0][1].ToString();//是否是根据视图读取（0表示不是，1表示是）
        this.strMainGsql = ds.Tables[0].Rows[0][2].ToString();//视图读取的语句（如果前面加符号@，表示从外部数据源读取）
        //if (ds.Tables[0].Rows[0][3] == DBNull.Value)//获取设置值中是否服务器分页
        //{
        //    Session["islarge"] = "0";
        //}
        //else
        //{
        //    Session["islarge"] = ds.Tables[0].Rows[0][3].ToString();
        //}
        //根据单据编码获取模板主键及其类型
        this.DoGetMainKeyInfo(strDocuName, this.strMainGroupID);
    }

    /// <summary>
    /// 根据单据编码获取模板主键及其类型
    /// </summary>
    /// <param name="strDocuName"></param>
    private void DoGetMainKeyInfo(string strDocuName,string strMainGroupId)
    {
        //通过档案编码和主信息分组编码获取对应的字段明细表数据,判断该单据主信息表中是否有自定义字段
        DataSet ds = SqlParamDao.GetDataSetBySql("SELECT PID,PTYPE FROM TB_HRTMPD WHERE TID='" + strDocuName + "' AND PISKEY=1 AND GID='" + strMainGroupId + "'");
        if (ds.Tables[0].Rows.Count == 0)
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "errclientscript", "<script language=javascript>alert('Please set a key field!');</script>");
            return;
        }
        else
        {
            this.KEY = ds.Tables[0].Rows[0][0].ToString();
            this.KEYTYPE = ds.Tables[0].Rows[0][1].ToString();
        }
    }

    /// <summary>
    /// 根据单据编码及场景编码获取对应的场景记录,并设置相应全局变量
    /// </summary>
    /// <param name="strDoc"></param>
    /// <param name="strScene"></param>
    private void DoGetSenceParamFlag(String strDocuName, String strScene)
    {
        string strSql = "SELECT SSLCT,SDEL,SEDIT,SADD,SSIZE,SCFORM,SREF FROM TB_HRTMPS WHERE TID='" + strDocuName + "' AND SID='" + strScene + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if ((dr[0] != DBNull.Value) && (dr[0].ToString() != ""))//SSLCT过滤查询语句
            {
                this.strSSLCT = dr[0].ToString();
                this.strSSLCT = ParamOperationBll.ReplaceSceneSqlParam(this.strSSLCT, this.hsCurRoleParamValue);
                //去掉sql语句中得order by ,并设置排序字符串
                this.strSSLCT = this.RemoveOrderby(this.strSSLCT);

            }
            if (dr[1] != DBNull.Value)//SDEL是否可以删除
            {
                this.iDelFlag = (int)dr[1];
            }
            else
            {
                this.iDelFlag = 0;
            }
            if (dr[2] != DBNull.Value)//SEDIT是否可以编辑
            {
                this.iEditFlag = (int)dr[2];
            }
            else
            {
                this.iEditFlag = 0;
            }
            if (dr[3] != DBNull.Value)//SADD是否可以新增
            {
                this.iAddFlag = (int)dr[3];
            }
            else
            {
                this.iAddFlag = 0;
            }
            if (dr[4] != DBNull.Value)//SSIZE页面显示大小
            {
                this.iSSize = (int)dr[4];
            }
            else
            {
                this.iSSize = 1;
            }
            if (dr[5] != DBNull.Value)//SCFORM是否弹出窗体
            {
                this.iCformFlag = (int)dr[5];
            }
            else
            {
                this.iCformFlag = 0;
            }
            if (dr[6] != DBNull.Value)//SREF是否是查询模式
            {
                this.strQueryModeFlag = dr[6].ToString();
            }
            else
            {
                this.strQueryModeFlag = "0";
            }
            //如果URL参数传递的status不存在，则赋值为查询模式
            if (String.IsNullOrEmpty(this.strOpenStatus))
            {
                this.strOpenStatus = this.strQueryModeFlag.Equals("0") ? "" : "select";
            }
        }

        //判断是否显示新增按钮
        if ((this.iAddFlag == 1) & !this.IsHis)
        {
            this.aAdd.Visible = true;
        }
        else
        {
            this.aAdd.Visible = false;
        }

    }

    /// <summary>
    /// 获取主显示的列名的DataTable
    /// </summary>
    /// <param name="strDocuName"></param>
    /// <param name="strScene"></param>
    /// <param name="strGroupId"></param>
    private void DoGetMasterListDataTable(String strDocuName, String strScene, String strGroupId)
    {
        if ((this.dtMasterLists == null) || (this.dtMasterLists.Rows.Count == 0))
        {
            DataSet ds = new DataSet();
            string strSqlSmsd = "SELECT PID,PDESC,PDESCCHS,PCTRL,PDEFAULT,PCTRLID,PCTRLD,PISKEY,PSYS,PRIGHT,PTYPE,PWIDTH FROM TB_HRTMPSD WHERE PRIGHT<2 AND TID='" + strDocuName + "' AND SID='" + strScene + "' AND GID='" + strGroupId + "' AND PLIST=1 AND PTYPE<>'CH' AND PTYPE<>'CS'  ORDER BY PORDER";
            //strSqlSmsd = this.MastAuthorization(strSqlSmsd, strDocuName, strScene, this.GID);
            ds = SqlParamDao.GetDataSetBySql(strSqlSmsd);
            this.dtMasterLists = ds.Tables[0];
        }
    }

    #endregion

    #region 根据参数strOpenStatus设置页面相应区域
    /// <summary>
    /// 根据参数strOpenStatus设置页面相应区域
    /// </summary>
    private void SettingByOpenState()
    {
        //如果默认是查询而不显示列表就隐藏列表
        if (this.strOpenStatus.ToLower() == "select")
        {
            //this.trGrid.Visible = false;
            //this.DDListSimpleSearch.Visible = false;
            //this.txtSimpleSearchValue.Visible = false;
            //this.aSimpleSearch.Visible = false;
            //this.trPageArea.Visible = false;
            //this.aRefresh.Visible = false;
            //this.divActionNav.Visible = false;
            //this.aShowDataList.Visible = true;
            this.aShowGroupSearch.Visible = false;
            this.trGroupSearch.Visible = true;
        }
        else
        {
            //this.trGrid.Visible = true;
            //this.DDListSimpleSearch.Visible = true;
            //this.txtSimpleSearchValue.Visible = true;
            //this.aSimpleSearch.Visible = true;
            //this.trPageArea.Visible = true;
            //this.aRefresh.Visible = true;
            //this.divActionNav.Visible = true;
            //this.aShowDataList.Visible = false;
            this.aShowGroupSearch.Visible = true;
            this.trGroupSearch.Visible = false;
        }
    }
    #endregion

    #region 设置角色列表
    /// <summary>
    /// 设置角色列表
    /// </summary>
    private void RoleListSetting(String strTemplate)
    {
        String strSql;
        if (this.IsAdminstrator())
        {
            //如果是管理员账号
            strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r WHERE r.TID='" + strTemplate + "' ORDER BY RORDER";
            if (!String.IsNullOrEmpty(this.strSingleRole))
            {
                strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r WHERE r.TID='" + strTemplate + "' AND r.RID = '" + this.strSingleRole + "' ORDER BY RORDER";
            }
        }
        else
        {
            strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r,TB_HR_USERROLE ur WHERE r.TID=ur.TID AND r.RID=ur.RID AND ur.TID='" + strTemplate + "' AND ur.SUSERID='" + this.GetUserCode() + "' ORDER BY r.RORDER";
            if (!String.IsNullOrEmpty(this.strSingleRole))
            {
                strSql = "SELECT r.RID,r.RDESC,r.RDESCCHS FROM TB_HRTMPR r,TB_HR_USERROLE ur WHERE r.TID=ur.TID AND r.RID=ur.RID AND ur.TID='" + strTemplate + "' AND ur.SUSERID='" + this.GetUserCode() + "' AND r.RID = '" + this.strSingleRole + "' ORDER BY r.RORDER";
            }
        }
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        this.dsCurRole = ds;
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                String strShowName = "";
                if (this.Language == "zh-cn")
                {
                    strShowName = "RDESCCHS";
                }
                else
                {
                    strShowName = "RDESC";
                }
                StringBuilder strBuilderRole = new StringBuilder();
                strBuilderRole.Append("\r\n");
                if (ds.Tables[0].Rows.Count > 1)
                {
                    strBuilderRole.Append("                 <li><span>" + this.strTipHaveRole + "</span></li>\r\n");
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["RID"].ToString().Equals(this.RID))
                    {
                        this.Label_Role.Text = ds.Tables[0].Rows[i][strShowName].ToString();
                    }
                    else
                    {
                        //String strParamString = ArchiveCommon.GetBeforeTransmitParam(this.strDocuName, ds.Tables[0].Rows[i]["RID"].ToString(), "", this.strOpenStatus);
                        String strParamString = "TID=" + this.TID + "&RID=" + ds.Tables[0].Rows[i]["RID"].ToString() + "&SID=" + "" + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;
                        strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                        strBuilderRole.Append("                 <li><a href=\"javascript:selectOtherRole('" + strParamString + "');\" class=\"a_Left\">" + ds.Tables[0].Rows[i][strShowName].ToString() + "</a></li>\r\n");
                    }
                }
                //在页面显示
                this.subNav_aRole.InnerHtml = strBuilderRole.ToString();
            }
        }
    }
    #endregion

    #region 设置场景列表
    /// <summary>
    /// 设置场景列表
    /// </summary>
    private void SceneListSetting(String strTemplate, String strRole)
    {
        string strSql = "SELECT s.SID,s.SDESC,s.SDESCCHS FROM TB_HRTMPS s,TB_HRTMPRD rs WHERE s.TID=rs.TID AND s.SID=rs.SID AND rs.TID='" + strTemplate + "' AND rs.RID='" + strRole + "' ORDER BY s.SORDER";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        this.dsCurScene = ds;
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                String strShowName = "";
                if (this.Language == "zh-cn")
                {
                    strShowName = "SDESCCHS";
                }
                else
                {
                    strShowName = "SDESC";
                }
                StringBuilder strBuilderScene = new StringBuilder();
                strBuilderScene.Append("\r\n");
                if (ds.Tables[0].Rows.Count > 1)
                {
                    strBuilderScene.Append("                 <li><span>" + this.strTipHaveScene + "</span></li>\r\n");
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["SID"].ToString().Equals(this.SID))
                    {
                        this.Label_Scene.Text = ds.Tables[0].Rows[i][strShowName].ToString();
                    }
                    else
                    {
                        //String strParamString = ArchiveCommon.GetBeforeTransmitParam(this.strDocuName, this.strRole, ds.Tables[0].Rows[i]["SID"].ToString(), this.strOpenStatus);
                        String strParamString = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + ds.Tables[0].Rows[i]["SID"].ToString() + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole;
                        strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                        strBuilderScene.Append("                    <li><a href=\"javascript:selectOtherScene('" + strParamString + "');\" class=\"a_Left\">" + ds.Tables[0].Rows[i][strShowName].ToString() + "</a></li>\r\n");
                    }
                }
                //在页面显示
                this.subNav_aScene.InnerHtml = strBuilderScene.ToString();
            }
        }
    }
    #endregion

    #region 设置Action动作列表
    /// <summary>
    /// 设置Action动作列表
    /// </summary>
    private void ActionListSetting(String strTemplate, String strScene)
    {
        string strHis;
        if (this.IsHis != null && bool.Parse(this.IsHis.ToString()))
        {
            strHis = "1";
        }
        else
        {
            strHis = "0";
        }
        ArchiveActionBll bllAction = new ArchiveActionBll();
        ArrayList arrActionList = bllAction.GetActionDetailList(this.TID, this.RID, this.SID, 0, "", base.GetUserCode(), this.IsAdminstrator(), strHis);

        if ((arrActionList != null) && (arrActionList.Count > 0))
        {
            String strActionName = "";
            String strActionPage = "";
            String strActionType = "0";

            StringBuilder strBuilderAction = new StringBuilder();
            strBuilderAction.Append("\r\n");
            if (arrActionList.Count > 1)
            {
                strBuilderAction.Append("                 <li><span>" + this.strTipHaveAction + "</span></li>\r\n");
            }
            for (int i = 0; i < arrActionList.Count; i++)
            {
                Entity_CreateAction entityAction = (Entity_CreateAction)arrActionList[i];
                if (base.Language.Equals("en-us"))
                {
                    strActionName = entityAction.ADESC;
                }
                else
                {
                    strActionName = entityAction.ADESCCHS;
                }
                strActionPage = entityAction.ADETAIL;
                strActionType = entityAction.ATYPE;
                strBuilderAction.Append("                    <li><a href=\"#\" onclick=\"javascript:selectOneAction('" + strActionPage + "','"+ strActionType + "');\" class=\"a_Right\">" + strActionName + "</a></li>\r\n");
            }
            //在页面显示
            this.subNav_aAction.InnerHtml = strBuilderAction.ToString();
            this.divActionNav.Visible = true;
            this.IsHasAction = true;
        }
        else
        {
            this.divActionNav.Visible = false;
            this.IsHasAction = false;
        }

    }
    #endregion

    #region 返回某一个角色的第一个状态
    /// <summary>
    /// 返回某一个角色的第一个状态
    /// </summary>
    /// <param name="strTemplate"></param>
    /// <param name="strRole"></param>
    /// <returns></returns>
    private String GetRoleFirstScene(String strTemplate, String strRole)
    {
        String strFirstScene = "";
        string strSql = "SELECT s.SID,s.SDESC,s.SDESCCHS FROM TB_HRTMPS s,TB_HRTMPRD rs WHERE s.TID=rs.TID AND s.SID=rs.SID AND rs.TID='" + strTemplate + "' AND rs.RID='" + strRole + "' ORDER BY s.SORDER";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                strFirstScene = ds.Tables[0].Rows[0]["SID"].ToString();
            }
        }
        return strFirstScene;
    }
    #endregion

    #region 根据角色和场景的个数隐藏对应区域
    /// <summary>
    /// 根据角色和场景的个数隐藏对应区域
    /// </summary>
    private void HiddenRoleAndScene(DataSet dsRole, DataSet dsScene)
    {
        int iRoleCount = 0;
        int iSceneCount = 0;
        if (dsRole != null)
        {
            iRoleCount = dsRole.Tables[0].Rows.Count;
        }
        if (dsScene != null)
        {
            iSceneCount = dsScene.Tables[0].Rows.Count;
        }
        if ((iRoleCount <= 1) && (iSceneCount <= 1))
        {
            this.trFirst.Visible = false;
        }
    }
    #endregion

    #region 页面按钮点击事件
    /// <summary>
    /// 设置新增单据记录按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SetAddClick()
    {
        string str = "";
        switch (this.iSSize)
        {
            case 1:
                str = "bottommaximum";
                break;

            case 2:
                str = "bottomzoom";
                break;

            case 3:
                str = "topzoom";
                break;
        }
        StringBuilder sbParamString = new StringBuilder();
        sbParamString.Append("TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=");
        sbParamString.Append("&OPTYPE=add" + "&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue);
        //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
        sbParamString.Append("&postGoupQueryFilter=" + this.strJArrayGroupQuery);

        if (this.iCformFlag == 0)//当前窗口打开
        {
            sbParamString.Append("&pageIndex=" + this.iPageIndex.ToString() + "&pageSize=" + this.iPageSize.ToString() + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole);
        }
        String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
        this.aAdd.Attributes.Add("onclick", "OpenDetail('" + strParamString + "','" + this.iCformFlag + "');return false;");

    }

    /// <summary>
    /// 刷新数据集事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Refresh_Click(object sender, EventArgs e)
    {
        //this.strSqlAddtion = null;//delete by sammen 20130109
        //this.txtSimpleSearchValue.Text = "";//delete by sammen 20130109

        this.RefreshDataGridDataSet();
    }

    /// <summary>
    /// 重新刷新加载数据集
    /// </summary>
    private void RefreshDataGridDataSet()
    {
        try
        {
            //this.iPageIndex = 0; //DELETE BY WSM 页面刷新后保留到当前页，而不回到首页
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            this.DataGrid1.DataKeyField = this.KEY.ToString();
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid分页部分的数据及显示
            this.SetDataGridPageArea();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    /// <summary>
    /// 根据单个条件过滤数据集事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SimpleSearch_Click(object sender, EventArgs e)
    {
        String strFilterName = this.DDListSimpleSearch.SelectedValue;
        String strFilterValue = this.txtSimpleSearchValue.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilterName))//对某特定字段的过滤
        {
            String strFilterDataType = "varchar";
            String[] strArr = strFilterName.Split('*');
            if ((strArr != null) && (strArr.Length == 2))
            {
                strFilterName = strArr[0].ToString();
                strFilterDataType = strArr[1].ToString();
                //日期格式处理
                if (strFilterDataType.ToLower().Equals("date"))
                {
                    strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 23)";
                }
                else if (strFilterDataType.ToLower().Equals("datetime"))
                {
                    strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 20)";
                }
            }

            if(this.entityArchiveStyle.IsSearchByPY.Equals("1"))
            {
                //处理拼音检索汉字
                if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
                {
                    strFilterName = "dbo.fun_getPY(" + strFilterName + ") ";
                }
            }
            strFilterSql = strFilterName + " like '%" + strFilterValue + "%'";
        }
        else//对所有字段的过滤
        {
            int iCount = this.DDListSimpleSearch.Items.Count;
            for (int i = 1; i < iCount;i++ )
            {
                String strName = this.DDListSimpleSearch.Items[i].Value;
                if (!String.IsNullOrEmpty(strName))
                {
                    String strFilterDataType = "varchar";
                    String[] strArr = strName.Split('*');
                    if ((strArr != null) && (strArr.Length == 2))
                    {
                        strName = strArr[0].ToString();
                        strFilterDataType = strArr[1].ToString();
                        //日期格式处理
                        if (strFilterDataType.ToLower().Equals("date"))
                        {
                            strName = " CONVERT(varchar(100), " + strName + ", 23)";
                        }
                        else if (strFilterDataType.ToLower().Equals("datetime"))
                        {
                            strName = " CONVERT(varchar(100), " + strName + ", 20)";
                        }

                        if (this.entityArchiveStyle.IsSearchByPY.Equals("1"))
                        {
                            //处理拼音检索汉字
                            if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
                            {
                                strName = "dbo.fun_getPY(" + strName + ") ";
                            }
                        }
                        if (i == 1)
                        {
                            strFilterSql = strFilterSql + "(" + strName + " like '%" + strFilterValue + "%')";
                        }
                        else
                        {
                            strFilterSql = strFilterSql + " OR (" + strName + " like '%" + strFilterValue + "%')";
                        }
                    }
                }
            }
        }

        this.strSearchValue = this.txtSimpleSearchValue.Text ;
        this.strSqlAddtion = strFilterSql;
        //add by sammen 20250121 防止查询条件里存在&等特殊字符而进行的加解密
        this.strSqlAddtion = UrlParamEncryption.Encrypt3des(this.strSqlAddtion, System.Text.Encoding.UTF8);
        this.iPageIndex = 0;
        this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
        this.DataGrid1.DataKeyField = this.KEY.ToString();
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();

        //设置获取DataGrid分页部分的数据及显示
        this.SetDataGridPageArea();
    }

    /// <summary>
    /// 显示数据列表区域事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ShowDataList_Click(object sender, EventArgs e)
    {
        this.strOpenStatus = "";
        //根据参数strOpenStatus设置页面相应区域
        this.SettingByOpenState();
        this.RefreshDataGridDataSet();
        if (this.IsHasAction)//判断当前状态下是否有动作
        {
            this.divActionNav.Visible = true;
        }
        else
        {
            this.divActionNav.Visible = false;
        }
    }

    #endregion

    #region 查询条件初始化区域
    /// <summary>
    /// 自动加载简易查询条件下拉框
    /// </summary>
    /// <param name="strRole"></param>
    /// <param name="strTemplate"></param>
    private void DoBuildSimpleConditionDDList(DataTable dtMasterList)
    {
        //新增默认搜索字段的设置 add by sammen 20200818
        String strDefaultFilterPID = this.entityArchiveStyle.DefaultFilterPID;
        String strDefaultFilterItemValue = "";

        this.DDListSimpleSearch.Controls.Clear();
        this.DDListSimpleSearch.Items.Clear();
        this.DDListSimpleSearch.Items.Add(new ListItem("", ""));
        if ((dtMasterList != null) && (dtMasterList.Rows.Count > 0))
        {
            String str3 = "";
            if (this.Language == "zh-cn")
            {
                str3 = "PDESCCHS";
            }
            else
            {
                str3 = "PDESC";
            }
            for (int i = 0; i < dtMasterList.Rows.Count; i++)
            {
                String strItemValue = dtMasterList.Rows[i]["PID"].ToString()+"*"+ dtMasterList.Rows[i]["PTYPE"].ToString();//PID*PTYPE
                this.DDListSimpleSearch.Items.Add(new ListItem(dtMasterList.Rows[i][str3].ToString(), strItemValue));
                if (!String.IsNullOrEmpty(strDefaultFilterPID))
                {
                    if (dtMasterList.Rows[i]["PID"].ToString().ToLower().Equals(strDefaultFilterPID.ToLower()))
                    {
                        strDefaultFilterItemValue = strItemValue;
                    }
                }
            }
            this.DDListSimpleSearch.ClearSelection();
            this.DDListSimpleSearch.SelectedIndex = 0;
            if (!String.IsNullOrEmpty(strDefaultFilterItemValue))
            {
                this.DDListSimpleSearch.SelectedIndex = this.DDListSimpleSearch.Items.IndexOf(this.DDListSimpleSearch.Items.FindByValue(strDefaultFilterItemValue));
            }
            //this.DDListSimpleSearch.SelectedValue = "DCNO";
        }

    }

    /// <summary>
    /// 动态加载组合查询区域
    /// </summary>
    /// <param name="strFunCode"></param>
    private void BuildGroupSearchArea(DataTable dtMasterList)
    {

        /// 动态加载新版组合查询区域
        this.BuildNewGroupSearchArea();

        #region 旧版暂时屏蔽
        //int iShowCount = 4;//每行显示条件个数
        //if ((dtMasterList != null) && (dtMasterList.Rows.Count > 0))
        //{
        //    int iDtRowsCount = dtMasterList.Rows.Count;//功能列表记录数
        //    int iHtmlRowsCount = 1;//在页面显示列表图标的行数
        //    if (iDtRowsCount > iShowCount)
        //    {
        //        if (iDtRowsCount % iShowCount > 0)
        //        {
        //            iHtmlRowsCount = iDtRowsCount / iShowCount + 1;
        //        }
        //        else
        //        {
        //            iHtmlRowsCount = iDtRowsCount / iShowCount;
        //        }
        //    }

        //    StringBuilder strBuilderGroupSearch = new StringBuilder();
        //    String strPID = "";//条件编码
        //    String strPTYPE = "";//条件字段类型
        //    String strConditionName = "";//条件名称
        //    String strTextId = "";//条件文本框ID
        //    //先循环在页面写出从第一行到倒手第二行的
        //    for (int i = 1; i <= iHtmlRowsCount - 1; i++)
        //    {
        //        TableRow row = new TableRow();

        //        for (int j = (i - 1) * iShowCount; j <= i * iShowCount - 1; j++)
        //        {
        //            TableCell cellHead = new TableCell();
        //            TableCell cellContent = new TableCell();
        //            Label LabelAuditing = new Label();
        //            TextBox TextAuditing = new TextBox();

        //            DataRow dr = dtMasterList.Rows[j];
        //            strPID = dr["PID"].ToString();
        //            strPTYPE = dr["PTYPE"].ToString();
        //            strTextId = "txt_" + strPID;
        //            if (this.Language == "zh-cn")
        //            {
        //                strConditionName = dr["PDESCCHS"].ToString();
        //            }
        //            else
        //            {
        //                strConditionName = dr["PDESC"].ToString();
        //            }

        //            LabelAuditing.ID = "lab" + strPID;
        //            LabelAuditing.Text = strConditionName;
        //            TextAuditing.ID = strTextId;
        //            TextAuditing.Width = Unit.Percentage(95);
        //            TextAuditing.BorderWidth = 1;
        //            TextAuditing.Attributes.Add("onkeypress", "EnterGroupSearchTextBox('" + strTextId + "')");
        //            if (strPTYPE.ToLower().Equals("date"))
        //            {
        //                //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
        //                TextAuditing.Attributes.Remove("datetype");
        //                TextAuditing.Attributes.Add("datetype", "date");
        //            } else if  (strPTYPE.ToLower().Equals("datetime"))
        //            {
        //                //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
        //                TextAuditing.Attributes.Remove("datetype");
        //                TextAuditing.Attributes.Add("datetype", "datetime");
        //            }
        //            cellHead.Controls.Add(LabelAuditing);
        //            cellHead.Width = Unit.Percentage(100 / iShowCount * 0.4);
        //            cellContent.Controls.Add(TextAuditing);
        //            cellContent.Width = Unit.Percentage(100 / iShowCount * 0.6);

        //            row.Cells.Add(cellHead);
        //            row.Cells.Add(cellContent);


        //        }
        //        this.tbSearchCondition.Rows.Add(row);

        //    }
        //    //然后在页面写出最后一行
        //    int iDtRemainCount = iDtRowsCount - (iHtmlRowsCount - 1) * iShowCount;//最后一行还剩几条记录
        //    int iHtmlRemainCount = iShowCount - iDtRemainCount;//页面中最后一行还剩几个空位
        //    TableRow row1 = new TableRow();
        //    for (int j = (iHtmlRowsCount - 1) * iShowCount; j < iDtRowsCount; j++)
        //    {
        //        TableCell cellHead = new TableCell();
        //        TableCell cellContent = new TableCell();
        //        Label LabelAuditing = new Label();
        //        TextBox TextAuditing = new TextBox();

        //        DataRow dr = dtMasterList.Rows[j];
        //        strPID = dr["PID"].ToString();
        //        strPTYPE = dr["PTYPE"].ToString();
        //        strTextId = "txt_" + strPID;
        //        if (this.Language == "zh-cn")
        //        {
        //            strConditionName = dr["PDESCCHS"].ToString();
        //        }
        //        else
        //        {
        //            strConditionName = dr["PDESC"].ToString();
        //        }

        //        LabelAuditing.ID = "lab" + strPID;
        //        LabelAuditing.Text = strConditionName;
        //        TextAuditing.ID = strTextId;
        //        TextAuditing.Width = Unit.Percentage(95);
        //        TextAuditing.BorderWidth = 1;
        //        TextAuditing.Attributes.Add("onkeypress", "EnterGroupSearchTextBox('" + strTextId + "')");
        //        if (strPTYPE.ToLower().Equals("date"))
        //        {
        //            //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
        //            TextAuditing.Attributes.Remove("datetype");
        //            TextAuditing.Attributes.Add("datetype", "date");
        //        }
        //        else if (strPTYPE.ToLower().Equals("datetime"))
        //        {
        //            //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
        //            TextAuditing.Attributes.Remove("datetype");
        //            TextAuditing.Attributes.Add("datetype", "datetime");
        //        }
        //        cellHead.Controls.Add(LabelAuditing);
        //        cellHead.Width = Unit.Percentage(100/ iShowCount * 0.4);
        //        cellContent.Controls.Add(TextAuditing);
        //        cellContent.Width = Unit.Percentage(100 / iShowCount * 0.6);

        //        row1.Cells.Add(cellHead);
        //        row1.Cells.Add(cellContent);
        //    }

        //    this.tbSearchCondition.Rows.Add(row1);

        //}
        #endregion
    }

    /// <summary>
    /// 根据组合条件过滤数据集事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GroupSearch_Click(object sender, EventArgs e)
    {
        //【新版组合查询】根据组合条件过滤数据集事件
        this.DoGroupSearch(false);

        #region 旧版暂时屏蔽
        //String strFilterName = " ";
        //String strFilterValue = "";
        //String strFilterDataType = "";
        //String strFilterSql = "";

        //if ((this.dtMasterLists != null) && (this.dtMasterLists.Rows.Count > 0))
        //{
        //    String strPID = "";//条件编码
        //    String strTextId = "";//条件文本框ID
        //    for (int i = 0; i < this.dtMasterLists.Rows.Count; i++)
        //    {
        //        DataRow dr = this.dtMasterLists.Rows[i];
        //        strPID = dr["PID"].ToString();
        //        strFilterDataType = dr["PTYPE"].ToString();
        //        strTextId = "txt_" + strPID;
        //        TextBox txtUserControl = (TextBox)this.FindControl(strTextId);
        //        if (txtUserControl != null)
        //        {
        //            strFilterName = strPID;
        //            //日期格式处理
        //            if (strFilterDataType.ToLower().Equals("date"))
        //            {
        //                strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 23)";
        //            }
        //            else if (strFilterDataType.ToLower().Equals("datetime"))
        //            {
        //                strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 20)";
        //            }

        //            //处理拼音检索汉字
        //            if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
        //            {
        //                strFilterName = "dbo.fun_getPY(" + strFilterName + ") ";
        //            }

        //            strFilterValue = txtUserControl.Text;
        //            if (!String.IsNullOrEmpty(strFilterValue.Trim()))
        //            {
        //                if (strFilterSql.Equals(""))
        //                {
        //                    strFilterSql = strFilterName + " like '%" + strFilterValue + "%'";
        //                }
        //                else
        //                {
        //                    strFilterSql = strFilterSql + " AND " + strFilterName + " like '%" + strFilterValue + "%'";
        //                }
        //            }
        //        }
        //    }
        //}

        //if (!String.IsNullOrEmpty(strFilterSql))
        //{
        //    //this.strOpenStatus = "";
        //    //根据参数strOpenStatus设置页面相应区域
        //    this.SettingByOpenState();

        //    this.iPageIndex = 0;
        //    this.strSqlAddtion = strFilterSql;
        //    this.txtSimpleSearchValue.Text = "";
        //    this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
        //    this.DataGrid1.DataKeyField = this.KEY.ToString();
        //    if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        //    {
        //        this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        //    }
        //    this.DataGrid1.DataBind();

        //    //设置获取DataGrid分页部分的数据及显示
        //    this.SetDataGridPageArea();
        //}
        #endregion
    }

    /// <summary>
    /// 组合查询条件重置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GroupReset_Click(object sender, EventArgs e)
    {
        this.DoGroupSearch(true);
    }

    /// <summary>
    /// 显示组合查询区域事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ShowGroupSearch_Click(object sender, EventArgs e)
    {
        this.strOpenStatus = "select";
        //根据参数strOpenStatus设置页面相应区域
        this.SettingByOpenState();
    }
    #endregion

    #region 新版组合查询区域 add by sammen 20230303

    /// <summary>
    /// 【新版组合查询】动态加载新版组合查询区域
    /// </summary>
    /// <param name="strFunCode"></param>
    private void BuildNewGroupSearchArea()
    {
        this.dtGroupQuerySettingList = TMPSQSGetterBll.GetTemplateStylesEntity(this.TID,this.SID);

        this.tbSearchCondition.Rows.Clear();
        if ((this.dtGroupQuerySettingList != null) && (this.dtGroupQuerySettingList.Rows.Count > 0))
        {
            this.strOpenStatus = "select";
            int iShowCount = int.Parse(this.dtGroupQuerySettingList.Rows[0]["COLQTY"].ToString());//每行显示条件个数
            int iDtRowsCount = this.dtGroupQuerySettingList.Rows.Count;//功能列表记录数
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

            JArray jaDefaultFilter = new JArray();
            //从传递参数中获取默认的查询条件值
            if (!String.IsNullOrEmpty(this.strPostGroupQueryFilter))
            {
                jaDefaultFilter = (JArray)JsonConvert.DeserializeObject(this.strPostGroupQueryFilter);
            }

            StringBuilder strBuilderGroupSearch = new StringBuilder();
            //先循环在页面写出从第一行到倒手第二行的
            for (int i = 1; i <= iHtmlRowsCount - 1; i++)
            {
                TableRow row = new TableRow();

                for (int j = (i - 1) * iShowCount; j <= i * iShowCount - 1; j++)
                {
                    DataRow dr = this.dtGroupQuerySettingList.Rows[j];
                    /// 根据记录集生成表格的一行条件
                    row = BuildOneRowSearchCondition(row, dr, jaDefaultFilter, iShowCount);

                }
                this.tbSearchCondition.Rows.Add(row);
            }

            //然后在页面写出最后一行
            int iDtRemainCount = iDtRowsCount - (iHtmlRowsCount - 1) * iShowCount;//最后一行还剩几条记录
            int iHtmlRemainCount = iShowCount - iDtRemainCount;//页面中最后一行还剩几个空位
            TableRow row1 = new TableRow();
            for (int j = (iHtmlRowsCount - 1) * iShowCount; j < iDtRowsCount; j++)
            {
                DataRow dr = this.dtGroupQuerySettingList.Rows[j];
                /// 根据记录集生成表格的一行条件
                row1 = BuildOneRowSearchCondition(row1, dr, jaDefaultFilter, iShowCount);
            }
            this.tbSearchCondition.Rows.Add(row1);
        }else
        {
           this.strOpenStatus = "";
        }

        //根据参数strOpenStatus设置页面相应区域
        this.SettingByOpenState();
    }

    /// <summary>
    /// 【新版组合查询】根据记录集生成表格的一行条件
    /// </summary>
    /// <param name="tableRow"></param>
    /// <param name="dr"></param>
    /// <param name="jaDefaultFilter"></param>
    /// <param name="iShowCount"></param>
    /// <returns></returns>
    private TableRow BuildOneRowSearchCondition(TableRow tableRow, DataRow dr,JArray jaDefaultFilter, int iShowCount)
    {
        TableCell cellHead = new TableCell();
        TableCell cellContent = new TableCell();
        Label LabelAuditing = new Label();

        String strSEQNO = dr["SEQNO"].ToString();
        String strGID = dr["GID"].ToString();
        String strPID = dr["PID"].ToString();
        String strPTYPE = dr["PTYPE"].ToString();
        String strPCTRL = dr["PCTRL"].ToString();
        String strPCTRLID = dr["PCTRLID"].ToString();
        String strPMAST = dr["PMAST"].ToString();
        String strPORDER = dr["PORDER"].ToString();
        String strQueryRule = dr["QueryRule"].ToString();
        String strCtrlId = "ctrl_"+strPID+"_" + strSEQNO;
        String strConditionName = dr["SHOWDESC"].ToString();
        if (this.Language == "zh-cn")
        {
            strConditionName = dr["SHOWDESCCHS"].ToString();
        }

        //遍历获取默认查询值
        String strDefaultValue = "";
        if (jaDefaultFilter.Count > 0)
        {
            foreach (JObject itemJArray in jaDefaultFilter)
            {
                String strPostFilterName = itemJArray["filterName"].ToString();
                String strPostFilterRule = itemJArray["filterRule"].ToString();
                String strPostFilterValue = itemJArray["filterValue"].ToString();
                if (strPID.ToUpper().Equals(strPostFilterName.ToUpper())&& strQueryRule.ToUpper().Equals(strPostFilterRule.ToUpper()))
                {
                    strDefaultValue = strPostFilterValue;
                    break;
                }
            }
        }

        LabelAuditing.ID = "lab_" + strSEQNO;
        LabelAuditing.Text = strConditionName;
        LabelAuditing.CssClass = "label_edit_Archive";
        //下拉框控件
        if (strPCTRL.Equals("1")){
            DropDownList DDListAuditing = new DropDownList();
            DDListAuditing.ID = strCtrlId;
            DDListAuditing.Attributes.Add("PID", strPID);
            DDListAuditing.Attributes.Add("QueryRule", Microsoft.JScript.GlobalObject.encodeURIComponent(strQueryRule));
            DDListAuditing.Width = Unit.Percentage(95);
            DDListAuditing.BorderWidth = 1;
            DDListAuditing.Attributes.Add("onkeypress", "EnterGroupSearchTextBox('" + strCtrlId + "')");
            try
            {
                TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(strPCTRLID);
                String strWhereBIsstop = "";
                if (property_LSTD.TABLENAME.Equals("TB_HRLSTD"))
                {
                    strWhereBIsstop = " AND isnull(BISSTOP,'0') <> '1' ";//只显示正常使用的项目
                }
                String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "'" + strWhereBIsstop + " ORDER BY " + property_LSTD.ORDER;

                //如果存在主控字段
                String strMasterFiled = strPMAST;

                if (!String.IsNullOrEmpty(strMasterFiled))
                {
                    String strMasterValue = "";
                    strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "' and CUID ='" + strMasterValue + "' ORDER BY " + property_LSTD.ORDER;
                }

                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    ListItem item = new ListItem("", "");
                    DDListAuditing.Items.Add(item);
                    string strShowName = "";
                    if (base.Language.Equals("zh-cn"))
                    {
                        strShowName = "CDESCCHS";
                    }
                    else
                    {
                        strShowName = "CDESC";
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        item = new ListItem(dt.Rows[i][strShowName].ToString(), dt.Rows[i]["CID"].ToString());
                        //默认值回填
                        if (dt.Rows[i]["CID"].ToString().ToLower().Equals(strDefaultValue.ToLower()))
                        {
                            item.Selected = true;
                        }
                        DDListAuditing.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("模板组合查询生成下拉框控件失败：DropDownListID:" + DDListAuditing.ID);
            }

            cellContent.Controls.Add(DDListAuditing);
        }
        //普通文本框控件
        else
        {
            TextBox TextAuditing = new TextBox();
            TextAuditing.ID = strCtrlId;
            TextAuditing.Attributes.Add("PID", strPID);
            TextAuditing.Attributes.Add("QueryRule", Microsoft.JScript.GlobalObject.encodeURIComponent(strQueryRule));
            TextAuditing.Width = Unit.Percentage(95);
            TextAuditing.BorderWidth = 1;
            TextAuditing.Attributes.Add("onkeypress", "EnterGroupSearchTextBox('" + strCtrlId + "')");
            if (strPTYPE.ToLower().Equals("date"))
            {
                //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
                TextAuditing.Attributes.Remove("datetype");
                TextAuditing.Attributes.Add("datetype", "date");
            }
            else if (strPTYPE.ToLower().Equals("datetime"))
            {
                //TextAuditing.Attributes.Add("onfocus", "vpCalendar_ShowDate(this)");
                TextAuditing.Attributes.Remove("datetype");
                TextAuditing.Attributes.Add("datetype", "datetime");
            }
            TextAuditing.Text = strDefaultValue;
            cellContent.Controls.Add(TextAuditing);
        }
        cellHead.Controls.Add(LabelAuditing);
        cellHead.Width = Unit.Percentage(100 / iShowCount * 0.4);
        cellContent.Width = Unit.Percentage(100 / iShowCount * 0.6);
        tableRow.Cells.Add(cellHead);
        tableRow.Cells.Add(cellContent);

        return tableRow;
    }


    /// <summary>
    /// 【新版组合查询】根据组合条件过滤数据集事件
    /// </summary>
    /// <param name="isReset">是否是重置后查询</param>
    private void DoGroupSearch(bool isReset)
    {
        StringBuilder sbFilterSql = new StringBuilder();
        JArray ja = new JArray(); 

        if ((this.dtGroupQuerySettingList != null) && (this.dtGroupQuerySettingList.Rows.Count > 0))
        {
            for (int i = 0; i < this.dtGroupQuerySettingList.Rows.Count; i++)
            {
                DataRow dr = this.dtGroupQuerySettingList.Rows[i];

                String strSEQNO = dr["SEQNO"].ToString();
                String strPID = dr["PID"].ToString();
                String strPTYPE = dr["PTYPE"].ToString();
                String strPCTRL = dr["PCTRL"].ToString();
                String strPCTRLID = dr["PCTRLID"].ToString();
                String strQueryRule = dr["QueryRule"].ToString();
                String strCtrlId = "ctrl_" + strPID + "_" + strSEQNO;

                String strFilterName = strPID;
                String strFilterRule = strQueryRule;
                String strFilterValue = "";
                String strFilterText = "";
                String strFilterDataType = "";
                //下拉框控件
                if (strPCTRL.Equals("1"))
                {
                    DropDownList ddListUserControl = (DropDownList)this.FindControl(strCtrlId);
                    if (ddListUserControl != null)
                    {
                        if (isReset)
                        {
                            ddListUserControl.SelectedValue = "";
                            ddListUserControl.SelectedIndex = -1;
                        }
                        strFilterValue = ddListUserControl.SelectedItem.Value;
                        strFilterText = ddListUserControl.SelectedItem.Text;
                        //下拉框需用值去数据库匹配//delete by sammen 20240321
                        //strFilterText = strFilterValue;
                    }
                }
                else
                {
                    TextBox txtUserControl = (TextBox)this.FindControl(strCtrlId);
                    if (isReset) {
                        txtUserControl.Text = "";
                    }

                    if (txtUserControl != null)
                    {
                        strFilterName = strPID;
                        //日期格式处理
                        if (strFilterDataType.ToLower().Equals("date")|| strPCTRL.Equals("12"))
                        {
                            strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 23)";
                        }
                        else if (strFilterDataType.ToLower().Equals("datetime") || strPCTRL.Equals("112"))
                        {
                            strFilterName = " CONVERT(varchar(100), " + strFilterName + ", 20)";
                        }else
                        {
                            //处理拼音检索汉字
                            if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
                            {
                                strFilterName = "dbo.fun_getPY(" + strFilterName + ") ";
                            }
                        }

                        strFilterValue = txtUserControl.Text;
                        strFilterText = txtUserControl.Text;
                    }
                }

                strFilterText = strFilterText.Trim();
                if (!String.IsNullOrEmpty(strFilterText))
                {
                    if (strFilterRule.Contains("like"))
                    {
                        sbFilterSql.Append(" AND (");
                        sbFilterSql.Append(" (" + strFilterName + " " + strFilterRule + " '%" + strFilterValue + "%')");
                        sbFilterSql.Append(" or (" + strFilterName + " " + strFilterRule + " '%" + strFilterText + "%')");//add by sammen 20240321 下拉框是value和Text都可能要匹配
                        sbFilterSql.Append(" )");
                    }
                    else
                    {
                        sbFilterSql.Append(" AND (");
                        sbFilterSql.Append("  (" + strFilterName + " " + strFilterRule + " '" + strFilterValue + "')");
                        sbFilterSql.Append(" or (" + strFilterName + " " + strFilterRule + " '" + strFilterText + "')");//add by sammen 20240321 下拉框是value和Text都可能要匹配
                        sbFilterSql.Append(" )");
                    }

                    //记录下本次组合查询的规则
                    if (!isReset)
                    {
                        JObject jo = new JObject();
                        jo.Add("filterName", strPID);
                        jo.Add("filterRule", strFilterRule);
                        jo.Add("filterValue", strFilterValue);
                        jo.Add("filterText", strFilterText);
                        ja.Add(jo);
                    }
                }
            }
        }

        if (!String.IsNullOrEmpty(sbFilterSql.ToString()))
        {
            this.strSqlAddtion = " 1=1 " + sbFilterSql.ToString();
        }
        else
        {
            this.strSqlAddtion = "";
        }

        //add by sammen 20250121 防止查询条件里存在&等特殊字符而进行的加解密
        this.strSqlAddtion = UrlParamEncryption.Encrypt3des(this.strSqlAddtion, System.Text.Encoding.UTF8);
        //本次查询规则写入ViewState
        this.strJArrayGroupQuery = ja.Count > 0 ? ja.ToString() : "";
        //add by sammen 20250121 防止查询条件里存在&等特殊字符而进行的加解密
        this.strJArrayGroupQuery = UrlParamEncryption.Encrypt3des(this.strJArrayGroupQuery, System.Text.Encoding.UTF8);

        //根据参数strOpenStatus设置页面相应区域
        this.SettingByOpenState();

        this.iPageIndex = 0;
        this.txtSimpleSearchValue.Text = "";
        this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
        this.DataGrid1.DataKeyField = this.KEY.ToString();
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();

        //设置获取DataGrid分页部分的数据及显示
        this.SetDataGridPageArea();

    }


    #endregion

    #region DataGrid区域
    /// <summary>
    /// 设置列表DataGrid相关
    /// </summary>
    private void DataGridSetting_First()
    {
        this.DoCreateDataGridColumn();
        this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
        this.DataGrid1.DataKeyField = this.KEY.ToString();
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();

    }

    /// <summary>
    /// 创建DataGrid各动态列
    /// </summary>
    /// <param name="strDocuName"></param>
    /// <param name="strScene"></param>
    /// <param name="strRole"></param>
    protected void DoCreateDataGridColumn()
    {
        if ((this.dtMasterLists != null) && (this.dtMasterLists.Rows.Count > 0))
        {
            DataTable dtFiled = this.dtMasterLists;
            int iCount = dtFiled.Rows.Count;

            String strFieldName = "";
            String strHeadText = "";
            String strFieldType = "";
            String strCtrlType = "";
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dtFiled.Rows[i];
                strFieldName = dr["PID"].ToString();
                strFieldType = dr["PTYPE"].ToString().ToLower();
                strCtrlType = dr["PCTRL"].ToString().ToLower();
                if (this.Language.Equals("zh-cn"))
                {
                    strHeadText = dr["PDESCCHS"].ToString();
                }
                else
                {
                    strHeadText = dr["PDESC"].ToString();
                }
                
                BoundColumn tcColumn = new BoundColumn();
                tcColumn.HeaderText = strHeadText;
                tcColumn.SortExpression = strFieldName;
                tcColumn.DataField = strFieldName;
                tcColumn.FooterText = strCtrlType;//设置为控件类型
                Double dWidth = Double.Parse(dr["PWIDTH"].ToString());
                if (strFieldType.ToLower().Equals("date"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd}";
                }else if (strFieldType.ToLower().Equals("datetime"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd  HH:mm:ss}";
                }

                this.DataGrid1.Columns.Add(tcColumn);
            }
            this.IsCreatedGridCol = true;
        }
    }

    /// <summary>
    /// 加载DataGrid数据
    /// </summary>
    /// <param name="strDoc"></param>
    /// <param name="strScene"></param>
    /// <param name="strRole"></param>
    /// <param name="isHistory"></param>
    private void DoLoadDataGridData(string strDoc, string strRole, string strScene, bool isHistory)
    {
        string strSqlString = "";
        //通过单据编码获取其对应主分组的数据表的前缀
        string strDocMainTable = strDoc + "_" + this.strMainGroupID;

        if (!(this.strMainGview == "1") || !(this.strMainGsql.Substring(0, 1) == "@"))
        {
            strSqlString = "SELECT * FROM " + strDocMainTable;
          
            //如果该场景下存在过滤条件，则获取过滤语句，且已经替换好参数
            if (!String.IsNullOrEmpty(this.strSSLCT))
            {
                strSqlString = this.strSSLCT;
            }
            //去掉语句中的ORDER BY
            //strSqlString = this.RemovePartString(strSqlString, "ORDER BY");

            //如果是读取历史表
            if (isHistory)
            {
                strSqlString = strSqlString.Replace(strDocMainTable, strDocMainTable + "_H");
            }
        }
        else
        {
            strSqlString = this.strMainGsql;
        }


        try
        {
            int iRecordCount = 0;
            //******** BEGIN 此处捕获异常是为了防治替换sql失败时的终极处理
            String strSql_Replaced = "";
            String strSqlAddtionToQuery = this.strSqlAddtion;
            //add by sammen 20250121 防止查询条件里存在&等特殊字符而进行的加解密
            try
            {
                strSqlAddtionToQuery = UrlParamEncryption.Decrypt3des(strSqlAddtionToQuery, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                strSqlAddtionToQuery = this.strSqlAddtion;
            }
            try
            {
                //将sql语句中涉及字典表的替换成字典表相应字段
                ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
                strSql_Replaced = lstdReplace.GetSqlIncludeLSTHDetail(strSqlString, strDoc, strScene, this.strMainGroupID, this.Language);

                //查询过滤条件
                if (!string.IsNullOrEmpty(strSqlAddtionToQuery))
                {
                    strSql_Replaced = this.PagingSql(strSql_Replaced, strSqlAddtionToQuery, " asc");
                }

                //加载读取数据集，并返回记录数
                this.dsGridList = ArchiveMainDealBll.GetArchiveListByServerPaging(strSql_Replaced, this.iPageSize, this.iPageIndex, this.KEY, this.strSortExp, ref iRecordCount);
                strSqlString = strSql_Replaced;
            }
            catch (Exception e)
            {
                if (strSqlString.IndexOf('*') > -1)
                {
                    //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
                    String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(strDoc, strScene, this.strMainGroupID);
                    strSqlString = strSqlString.Replace("*", strFieldNameString);
                }
                //查询过滤条件
                if (!string.IsNullOrEmpty(strSqlAddtionToQuery))
                {
                    strSqlString = this.PagingSql(strSqlString, strSqlAddtionToQuery, " asc");
                }

                //加载读取数据集，并返回记录数
                this.dsGridList = ArchiveMainDealBll.GetArchiveListByServerPaging(strSqlString, this.iPageSize, this.iPageIndex, this.KEY, this.strSortExp, ref iRecordCount);
                log.Error(e);
                log.Error("将sql语句中涉及字典表的替换成字典表相应字段时出错或者获取界面语句出错ReplaceSqlIncludeTBLSTD.GetSqlIncludeLSTHDetail(),TID:" + strDoc);
                log.Error("将sql语句中涉及字典表的替换成字典表相应字段时出错或者获取界面语句出错ReplaceSqlIncludeTBLSTD.GetSqlIncludeLSTHDetail(),SQL:" + strSql_Replaced);
            }
            //********* END

            if (this.dsGridList != null && this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0] != null)
            {
                this.dsGridList.Tables[0].TableName = strDocMainTable;
            }
            this.iRecordCount = iRecordCount;

            //ViewState["DsViewDataViewState"] = SqlParamDao.GetDataSetBySql(strSqlString);//此时读取全部数据，大大影响列表加载性能
            //Session["ExportDsViewDataViewState"] = ViewState["DsViewDataViewState"];//主要是应用于导出EXCEL功能

            //点击导出以后才获取数据，优化了列表页面的加载性能 modify by sammen 20151112
            Session["ExportDsViewDataViewState_Sql"] = strSqlString;
            Session["ExportDsViewDataViewState_Title"] = this.TID+" Record Export";
            //增加三项导出excel时涉及的参数 add by sammen 20191226
            //【主要为了解决：1模板列表导出时列名导出名称；2隐藏的字段不显示】
            Session["ExportDsViewDataViewState_TID"] = this.TID;
            Session["ExportDsViewDataViewState_SID"] = this.SID;
            Session["ExportDsViewDataViewState_GID"] = this.strMainGroupID;

            if (this.Language.Equals("zh-cn"))
            {
                Session["ExportDsViewDataViewState_Title"] = this.TID + " 记录导出";
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this, "Get List Data Failed!");
            return;
        }

        try
        {
            if (this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0].Constraints.Count == 0)
            {
                this.dsGridList.Tables[0].Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[] { this.dsGridList.Tables[0].Columns[this.KEY.ToString()] }, true));
                this.dsGridList.Tables[0].Columns[this.KEY.ToString()].AllowDBNull = false;
                this.dsGridList.Tables[0].Columns[this.KEY.ToString()].Unique = true;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this,"DataGrid Set Error!");
            return;
        }
    }

    /// <summary>
    /// 去掉sql语句中得order by ,并设置排序字符串
    /// </summary>
    /// <param name="old"></param>
    /// <returns></returns>
    private string RemoveOrderby(string strOldSql)
    {
        int startIndex = strOldSql.ToUpper().IndexOf("ORDER BY", 0, StringComparison.InvariantCultureIgnoreCase);
        if (startIndex > -1)
        {
            //add by sammen 20210516,如果已经排序了，则不从sql中获取
            if (String.IsNullOrEmpty(this.strSortExp))
            {
                this.strSortExp = strOldSql.Substring(startIndex).ToUpper().Replace("ORDER BY", "");
            }
            strOldSql = strOldSql.Remove(startIndex);
        }
        return strOldSql;
    }

    /// <summary>
    /// DataGrid 列表项目创建事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)//如果可以删除，则显示编辑列，否则不显示
        {
            if ((this.iDelFlag == 1) & !this.IsHis)
            {
                this.DataGrid1.Columns[0].Visible = true;
            }
            else
            {
                this.DataGrid1.Columns[0].Visible = false;
            }
        }
        if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
        {
            ImageButton  button = (ImageButton)e.Item.FindControl("Imagebutton_Delete");
            if (button != null)
            {
                button.ToolTip = this.strDelete;
                button.Attributes.Add("onclick", "return confirm('" + this.strConfirm + "');");
                if ((this.iDelFlag == 1) & !this.IsHis)
                {
                    button.Visible = true;
                }
                else
                {
                    button.Visible = false;
                }
            }
        }
        //设置标题栏不换行
        if (e.Item.ItemType == ListItemType.Header)
        {
            // 在这里, e.Item返回的是当前创建行的对象
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {
                // e.Item.Cells[x]是当前行中的单元格
                e.Item.Cells[i].Wrap = false;
            }
        }

    }

    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            String strOpType = "";
            if (this.iEditFlag == 1)
            {
                //如果可编辑，则双击直接进入编辑页面
                strOpType = "edit";
            }
            else
            {
                //如果不可编辑，则双击进入查看页面
                strOpType = "readonly";
            }

            StringBuilder sbParamString = new StringBuilder();
            sbParamString.Append("TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + strCurKeyValue );
            sbParamString.Append("&OPTYPE=" + strOpType + "&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue + "&sortExp=" + this.strSortExp);
            //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
            sbParamString.Append("&postGoupQueryFilter=" + this.strJArrayGroupQuery );

            if (this.iCformFlag == 0)//当前窗口打开
            {
                sbParamString.Append("&pageIndex=" + this.iPageIndex.ToString() + "&pageSize=" + this.iPageSize.ToString() + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole);
            }
            String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());

            e.Item.Attributes.Add("ondblclick", "OpenDetail('" + strParamString + "','" + this.iCformFlag + "');return false;");
            e.Item.Attributes.Add("title", strCurKeyValue + "   " + this.strTipShowDetail);

            //为key字段添加超链接
            String strCurKeyValueFormat = strCurKeyValue;
            if (this.KEYTYPE.ToLower().Equals("date"))
            {
                DateTime dtCurKey = DateTime.Parse(strCurKeyValue);
                strCurKeyValueFormat = dtCurKey.ToString("yyyy-MM-dd");
            }
            else if (this.KEYTYPE.ToLower().Equals("datetime"))
            {
                DateTime dtCurKey = DateTime.Parse(strCurKeyValue);
                strCurKeyValueFormat = dtCurKey.ToString("yyyy-MM-dd HH:mm:ss");
            }
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {
                if (this.DataGrid1.Columns[i].SortExpression.ToString() == this.DataGrid1.DataKeyField.ToString())
                {
                    TableCellCollection tcl = e.Item.Cells;
                    HyperLink lbKeyFeild = new HyperLink();
                    lbKeyFeild.Text = strCurKeyValueFormat;
                    lbKeyFeild.ToolTip = strCurKeyValueFormat;
                    lbKeyFeild.CssClass = "a_Center";
                    lbKeyFeild.Attributes.Remove("onclick");
                    lbKeyFeild.Attributes.Add("onclick", "javascript:OpenDetail('" + strParamString + "','" + this.iCformFlag + "');return false;");

                    e.Item.Cells[i].Controls.Clear();
                    e.Item.Cells[i].Controls.Add(lbKeyFeild);
                }
            }

            //如果第一列是复选框类型，则动态生成复选框
            BoundColumn tcColumn0 = (BoundColumn)this.DataGrid1.Columns[1];
            String strCtrl0Type = tcColumn0.FooterText;
            String strFieldName = tcColumn0.DataField;
            if (strCtrl0Type.Equals("11"))//复选框
            {
                TableCellCollection tcl = e.Item.Cells;
                CheckBox cb = new CheckBox();
                cb.ID = "cbSelect_" + this.TID + "_" + this.strMainGroupID + "_" + strFieldName + "_" + this.KEY + "_" + strCurKeyValue;
                if (e.Item.Cells[1].Text.ToString().ToLower().Equals("true"))
                {
                    cb.Checked = true;
                }
                cb.Attributes.Add("KEYVALUE", strCurKeyValue);
                e.Item.Cells[1].Controls.Clear();
                e.Item.Cells[1].Controls.Add(cb);
            }

        }
        if (e.Item.ItemType == ListItemType.Header)//如果第一列是复选框类型，则动态生成全选复选框
        {
            String strCtrl0Type = this.DataGrid1.Columns[1].FooterText;
            if (strCtrl0Type.Equals("11"))//复选框
            {
                TableCellCollection tcl = e.Item.Cells;
                CheckBox cb = new CheckBox();
                cb.ID = "cbAll";
                cb.Attributes.Add("onclick", "checkAll('"+cb.ClientID+"');return false;");
                e.Item.Cells[1].Controls.Clear();
                e.Item.Cells[1].Controls.Add(cb);
            }
        }

    }

    /// <summary>
    /// DataGrid 列表项目事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        StringBuilder sbParamString = new StringBuilder();
        sbParamString.Append("TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY  );
        sbParamString.Append("&sqlAddtion=" + this.strSqlAddtion + "&searchValue=" + this.strSearchValue);
        //add by sammen 20230304 新版模板组合查询时增加过滤条件的参数传递
        sbParamString.Append("&postGoupQueryFilter=" + this.strJArrayGroupQuery);

        StringBuilder sbParamForPaging = new StringBuilder();
        sbParamForPaging.Append("&pageIndex=" + this.iPageIndex.ToString() + "&pageSize=" + this.iPageSize.ToString() + "&status=" + this.strOpenStatus + "&ROLE=" + this.strSingleRole);
        if (e.CommandName == "View")
        {
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            sbParamString.Append("&KEYVALUE=" + strCurKeyValue+"&OPTYPE=readonly") ;
            if (this.iCformFlag == 0)//当前窗口打开
            {
                sbParamString.Append(sbParamForPaging.ToString());
            }
            String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
            Page.ClientScript.RegisterStartupScript(typeof(Page), "detailclientscript", "<script language=javascript>OpenDetail('" + strParamString + "','" + this.iCformFlag + "');</script>");
        }
        else if (e.CommandName == "Edit")
        {
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            sbParamString.Append("&KEYVALUE=" + strCurKeyValue + "&OPTYPE=edit");
            if (this.iCformFlag == 0)//当前窗口打开
            {
                sbParamString.Append(sbParamForPaging.ToString());
            }
            String strParamString = UrlParamEncryption.EncryptionUrlParam(sbParamString.ToString());
            Page.ClientScript.RegisterStartupScript(typeof(Page), "detailclientscript", "<script language=javascript>OpenDetail('" + strParamString + "','"+this.iCformFlag+"');</script>");
        }
        else if (e.CommandName == "Delete")
        {
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            String strIsHis = "0";
            if (this.IsHis)
            {
                strIsHis = "1";
            }
            //先作删除前的判断
            int iPreDeleteSpCount = ArchiveMainDealBll.JudgeBeforeDelete(this.TID, this.RID, this.SID, strCurKeyValue, this.GetUserCode(), this.IsAdminstrator(), strIsHis);
            if (iPreDeleteSpCount==0)
            {
                int iDeleteCount = ArchiveMainDealBll.DeleteArchiveOneRecord(this.TID, this.RID, this.SID, this.KEY, strCurKeyValue, this.GetUserCode(), this.IsAdminstrator(), strIsHis);
                if (iDeleteCount <= 0)
                {
                    this.AlertMessageBox(this, this.strTipDeleteFailed);
                    this.RefreshDataGridDataSet();
                }
                else
                {
                    this.WriteDeleteDataLog(strCurKeyValue);
                    this.AlertMessageBox(this, this.strTipDeleteSuccess);
                    this.RefreshDataGridDataSet();
                }
            }
            else
            {
                ArchiveActionBll bllArchiveAction = new ArchiveActionBll();
                String strMsg = bllArchiveAction.GetPageTipAfterExcuteSp(this.TID,this.SID,6,iPreDeleteSpCount,this.Language);
                if(!String.IsNullOrEmpty(strMsg)){
                this.AlertMessageBox(this, strMsg);
                this.RefreshDataGridDataSet();
                }
            }
        }
    }


    /// <summary>
    /// DataGrid 排序事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (this.dsGridList != null && this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0] != null)
        {
            DataView defaultView = this.dsGridList.Tables[0].DefaultView;//取当前页的数据集
            //DataView defaultView = ((DataSet)ViewState["DsViewDataViewState"]).Tables[0].DefaultView;//取所有页的所有数据集

            #region 当前页排序
            //if (this.IsSortAscending)
            //{
            //    defaultView.Sort = e.SortExpression;
            //}
            //else
            //{
            //    defaultView.Sort = e.SortExpression + " DESC";
            //}
            //this.IsSortAscending = !this.IsSortAscending;

            ////填充DataGrid的数据

            ////设置全局dataset
            //DataSet dsTemp = new DataSet();
            //System.Data.DataTable dt = defaultView.ToTable();
            //dsTemp.Tables.Add(dt.Copy());
            //this.dsGridList = dsTemp;

            //this.DataGrid1.DataSource = defaultView;
            //this.DataGrid1.DataBind();
            #endregion

            #region 所有页排序
            if (this.IsSortAscending)
            {
                this.strSortExp = e.SortExpression;
                
            }
            else
            {
                this.strSortExp = e.SortExpression + " DESC";
            }
            //add by sammen 20210516,自定义排序后也加上对主键的排序+","+this.KEY，否则翻页后顺序会乱
            if (!e.SortExpression.ToUpper().Equals(this.KEY.ToUpper()))
            {
                this.strSortExp = this.strSortExp + "," + this.KEY;
            }
            this.IsSortAscending = !this.IsSortAscending;
            this.RefreshDataGridDataSet();
            #endregion

        }
    }


    #endregion

    #region 页面分页部分
    /// <summary>
    /// 设置获取DataGrid分页部分的数据及显示
    /// </summary>
    private void SetDataGridPageArea()
    {
        //设置获取DataGrid列表的页数
        this.SetPageCount();
        //初始化当前页数的下拉框
        this.InitCurPageDDList();
        //设置获取DataGrid列表的当前页数
        this.SetPageCurNum();
        this.Label_AllCount.Text = this.iRecordCount.ToString();
        this.txtPageSize.Text = this.iPageSize.ToString();
    }

    /// <summary>
    /// 设置获取DataGrid列表的页数
    /// </summary>
    private void SetPageCount()
    {
        if ((this.iRecordCount % this.iPageSize) == 0)
        {
            this.iPageCount = this.iRecordCount / this.iPageSize;
        }
        else
        {
            this.iPageCount = (this.iRecordCount / this.iPageSize) + 1;
        }
        this.Label_AllPage.Text = this.iPageCount.ToString();
    }

    /// <summary>
    /// 初始化当前页数的下拉框
    /// </summary>
    private void InitCurPageDDList()
    {
        if ((this.iRecordCount > 0) && (this.iPageCount > 0))
        {
            this.DDList_CurPage.Items.Clear();
            for (int i = 1; i <= this.iPageCount; i++)
            {
                this.DDList_CurPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            this.DDList_CurPage.ClearSelection();
            this.DDList_CurPage.SelectedIndex = this.iPageIndex;
        }
    }

    /// <summary>
    /// 设置获取DataGrid列表的当前页数,并根据当前页数设置按钮可用
    /// </summary>
    private void SetPageCurNum()
    {
        if (this.DDList_CurPage.Items.Count > 0)
        {
            this.DDList_CurPage.SelectedIndex = this.iPageIndex;
        }
        if (this.iPageIndex <= 0)
        {
            this.aFirstPage.Enabled = false;
            this.aPrePage.Enabled = false;
        }
        else
        {
            this.aFirstPage.Enabled = true;
            this.aPrePage.Enabled = true;
        }
        if (this.iPageIndex >= this.iPageCount - 1)
        {
            this.aNextPage.Enabled = false;
            this.aLastPage.Enabled = false;
        }
        else
        {
            this.aNextPage.Enabled = true;
            this.aLastPage.Enabled = true;
        }
    }

    /// <summary>
    /// 首页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = 0;
            //获取DataGrid数据的DataSet
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }

    }

    /// <summary>
    /// 上一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PrePage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = this.iPageIndex - 1;
            //获取DataGrid数据的DataSet
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 下一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NextPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageIndex + 1;
            //获取DataGrid数据的DataSet
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 末页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LastPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageCount - 1;
            //获取DataGrid数据的DataSet
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 选择页数下拉框事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDList_CurPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.iPageIndex = this.DDList_CurPage.SelectedIndex;
        //获取DataGrid数据的DataSet
        this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();
        //设置获取DataGrid列表的当前页数
        this.SetPageCurNum();
    }

    /// <summary>
    /// 跳转每页显示数量按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GoPageSize_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtPageSize.Text))
        {
            this.iPageIndex = 0;
            this.iPageSize = Convert.ToInt32(this.txtPageSize.Text);
            //获取DataGrid数据的DataSet
            this.DoLoadDataGridData(this.TID, this.RID, this.SID, false);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();

            this.SetDataGridPageArea();
        }
    }

    #endregion

    #region 写入日志
    /// <summary>
    /// 写入删除档案日志
    /// </summary>
    /// <param name="strKeyValue"></param>
    private void WriteDeleteDataLog(String strKeyValue)
    {
        if ((Session["ArchiveIsLog"] != null) && (Session["ArchiveIsLog"].ToString().Equals("1")))
        {
            Entity_HRLOG_2 entityLog2 = null;
            DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Delete, strKeyValue, entityLog2);
        }
    }
    #endregion

    #region 工具类
    /// <summary>
    /// 去掉字符串中的某字符
    /// </summary>
    /// <param name="strAll"></param>
    /// <param name="strPart"></param>
    /// <returns></returns>
    private string RemovePartString(String strAll, String strPart)
    {
        int startIndex = strAll.IndexOf(strPart, 0, StringComparison.InvariantCultureIgnoreCase);
        if (startIndex > -1)
        {
            strAll = strAll.Remove(startIndex);
        }
        return strAll;
    }

    /// <summary>
    /// 特殊拼语句
    /// </summary>
    /// <param name="strOldSql"></param>
    /// <param name="PID"></param>
    /// <param name="order"></param>
    /// <returns></returns>
    private string PagingSql(String strOldSql, String PID, String order)
    {
        string str = "SELECT * FROM (" + strOldSql + ") tempTable ";
        if (PID != "")
        {
            //return (str + " WHERE tempTable." + PID);
            return (str + " WHERE " + PID);
        }
        if (order != "")
        {
            str = str + " " + order;
        }
        return str;
    }
    #endregion

    #region 保存DataGrid中的复选框是否被选中的值（作废）
    protected void SaveChecked_Click(object sender, EventArgs e)
    {
        this.saveCheckedBox();
    }
    private void saveCheckedBox()
    {
        //*******已经由javascript和ajax实现
        //*******
        //String strAllValue = this.hfCheckBoxValue.Value.ToString();
        //if(String.IsNullOrEmpty(strAllValue))
        //{
        //    return;
        //}
        //String[] strArr1 = strAllValue.Split('*');
        //if(strArr1==null)
        //{
        //    return;
        //}
        //int iCount;
        //StringBuilder strBuilderSql = new StringBuilder();
        //for (int i = 0; i < strArr1.Length; i++)
        //{
        //    String strValue1 = strArr1[i].ToString();
        //    int index = strValue1.IndexOf("cbSelect");
        //    strValue1 = strValue1.Substring(index);
        //    String[] strArr2 = strValue1.Split(':');
        //    String strIDString = strArr2[0];
        //    String strPValue = strArr2[1];//是否被选中的值（true/false）
        //    String[] strArr3 = strIDString.Split('_');
        //    String strPid = strArr3[1];//字段名
        //    String strCurKeyValue = strArr3[2];//主键值

        //    String strMainTableName = this.TID + "_" + this.strMainGroupID;
        //    String strSql = "update " + strMainTableName + " set " + strPid + "='" + strPValue + "' WHERE " + this.KEY + "='" + strCurKeyValue + "'";
        //    strBuilderSql.Append(strSql + ";\r\n");
        //}
        //if (strBuilderSql!=null)
        //{
        //    iCount = SqlParamDao.ExecuteNonQueryBySql(strBuilderSql.ToString());
        //}

        //int iCount;
        //Control control;
        //if (this.DataGrid1.Items.Count == 0)
        //{
        //    return;
        //}
        ////如果第一列是复选框类型
        //String strCtrl0Type = this.DataGrid1.Columns[1].FooterText;
        //if (strCtrl0Type.Equals("11"))//复选框
        //{
        //    //control = this.DataGrid1.Items[0].Cells[1].Controls[0];
        //    String strValue = this.DataGrid1.Items[0].Cells[1].Text;
        //}
        //else
        //{
        //    return;
        //}
        //if (((control != null) && (control is CheckBox)) && ((CheckBox)control).Enabled)
        //{
        //    StringBuilder strBuilderSql = new StringBuilder();
        //    for (int i = 0; i < this.DataGrid1.Items.Count; i++)
        //    {
        //        if (this.DataGrid1.Items[i].ItemType == ListItemType.Item)
        //        {
        //            CheckBox cBox = (CheckBox)this.DataGrid1.Items[i].Cells[1].Controls[0];
        //            String strCboxId = cBox.ID;
        //            String[] strArrID = strCboxId.Split('_');
        //            String strTid = cBox.Attributes["TID"].ToString();
        //            String strGid = cBox.Attributes["GID"].ToString();
        //            String strPid = cBox.Attributes["PID"].ToString();
        //            String strKey = this.KEY;
        //            String strKeyValue = cBox.Attributes["KEYVALUE"].ToString();
        //            String strValue = "False";
        //            if (cBox.Checked)
        //            {
        //                strValue = "True";
        //            }
        //            String strMainTableName = strTid + "_" + strGid;
        //            String strSql = "update " + strMainTableName + " set " + strPid + "='" + strValue + "' WHERE " + strKey + "='" + strKeyValue + "'";
        //            strBuilderSql.Append(strSql +";\r\n");
        //        }
        //    }
        //    if (strBuilderSql!=null)
        //    {
        //        iCount = SqlParamDao.ExecuteNonQueryBySql(strBuilderSql.ToString());
        //    }
        //}
    }
    #endregion

    #region 加载角色或者场景前后执行存储过程 add by sammen 20140619
    /// <summary>
    /// 加载角色前执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doBeforeLoadRole()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_BeforeLoadRole(hsTableParam);
        return iCount;
    }

    /// <summary>
    /// 加载角色后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterLoadRole()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_AfterLoadRole(hsTableParam);
        return iCount;
    }

    /// <summary>
    /// 加载场景前执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doBeforeLoadSence()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_BeforeLoadSence(hsTableParam);
        return iCount;
    }

    /// <summary>
    /// 加载场景后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterLoadSence()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ActionMainActionBll.DoExcuteSP_AfterLoadSence(hsTableParam);
        return iCount;
    }

    #endregion

}
