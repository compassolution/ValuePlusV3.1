using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Resources;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.Query;
using Com.ValuePlus.BLL.Export;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.DAL;
using System.Drawing;
using Com.ValuePlus.Common;
using System.IO;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.Utils;


public partial class Query_QueryMain : PageBase
{
    private ResourceManager rm_ViewColCaption;
    String strColumnMaxCount = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("ColumnMaxCount_QueryList");

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "showWaitingDiv", "<script language=\"javascript\">ShowWaitingDiv();</script>");

        //modify by sammen 20181130 使用相对地址，兼容https及外网地址映射的需求
        String strExportPage = "../Export/exportindex.aspx";
        this.btnExportExl.Attributes.Add("onclick", "javascript:exportexcel('" + strExportPage + "');return false;");
        this.btnExportPdf.Attributes.Add("onclick", "javascript:exportpdf('" + strExportPage + "');return false;");
        this.btnExportTxt.Attributes.Add("onclick", "javascript:exporttxt('" + strExportPage + "');return false;");
        //this.btnExportExl.Attributes.Add("onclick", "javascript:exportexcel('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");
        //this.btnExportPdf.Attributes.Add("onclick", "javascript:exportpdf('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");
        //this.btnExportTxt.Attributes.Add("onclick", "javascript:exporttxt('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");

        if (!Page.IsPostBack)
        {
            this.strLocalUrl = Server.UrlDecode(base.Request.Url.ToString());
            this.hfLocalUrl.Value = Server.UrlDecode(base.Request.Url.ToString());
            String strQRY = "";
            String strSP = "";

            //清空导出excel相关的session
            Session["ExportDsViewDataViewState"] = null;
            Session["ExportDsViewDataViewState_Sql"] = null;
            Session["ExportDsViewDataViewState_Title"] = null;
            //增加三项导出excel时涉及的参数 add by sammen 20191226
            //【主要为了解决：1模板列表导出时列名导出名称；2隐藏的字段不显示】
            Session["ExportDsViewDataViewState_TID"] = null;
            Session["ExportDsViewDataViewState_SID"] = null;
            Session["ExportDsViewDataViewState_GID"] = null;

            ResourceManager rmLocResourceManager = base.GetResourceManager("QueryMain");
            this.hsTableRequestParam = this.GetUrlAnalyse();
            if (this.hsTableRequestParam != null)
            {
                if (this.hsTableRequestParam["QRY"] != null)//普通视图查询
                {
                    strQRY = this.hsTableRequestParam["QRY"].ToString();
                    this.strViewName = strQRY;
                    this.strIsViewOrSp = "view";
                    //获取存储查询或者视图查询的标题
                    this.strViewOrSpTitleName = this.GetSpOrViewTitleName(this.strIsViewOrSp, strQRY);
                }
                else if (this.hsTableRequestParam["SP"] != null)//存储过程查询
                {
                    strSP = this.hsTableRequestParam["SP"].ToString();
                    this.strSPName = strSP;
                    this.strIsViewOrSp = "sp";
                    //获取存储查询或者视图查询的标题
                    this.strViewOrSpTitleName = this.GetSpOrViewTitleName(this.strIsViewOrSp, strSP);
                    //执行型存储查询
                    this.ExcuteSpQuery();
                }
                if (this.hsTableRequestParam["STATUS"] != null)//列表显示状态模式，select表示为默认查询状态
                {
                    this.strStatus = this.hsTableRequestParam["STATUS"].ToString();
                }
                else
                {
                    this.strStatus = "";
                }
            }

            //设置中英文
            this.SetLanguageLabel(rmLocResourceManager);

            //查询按钮点击不刷新，从而让查询区域可视
            this.btnFilterCon.Attributes.Add("onclick", "return false;");
            this.btnFilterCol.Attributes.Add("onclick", "return false;");
            this.btnOrder.Attributes.Add("onclick", "return false;");
            this.btnNewWindow.Attributes.Add("onclick", "return false;");

            this.txtPageSize.Attributes.Add("onkeypress", "EnterPageSizeTextBox()");
            this.txtCondition1.Attributes.Add("onkeypress", "EnterConditionTextBox('txtCondition1')");
            this.txtCondition2.Attributes.Add("onkeypress", "EnterConditionTextBox('txtCondition2')");
            this.txtCondition3.Attributes.Add("onkeypress", "EnterConditionTextBox('txtCondition3')");
            this.txtCondition4.Attributes.Add("onkeypress", "EnterConditionTextBox('txtCondition4')");
            this.txtCondition5.Attributes.Add("onkeypress", "EnterConditionTextBox('txtCondition5')");
            
            //从配置文件读取
            String strPageSize = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("PageSize_QueryList");
            this.iPageSize = int.Parse(strPageSize);

            try
            {
                this.arrListGridColumn = this.GetArrayListViewCol();
                //获取该视图各列对应的中英文
                this.hsTableColumnLanguage = this.GetViewColLanguge();
                // 根据视图字段加载下拉框列表
                this.BuildDropDownListItems();
                //根据视图字段加载列过滤ListBox控件列表
                this.BuildHideColumnListBoxItems();
                //填充DataGrid的数据
                //add by sammen 20201104 默认显示列表时才加载数据
                if(this.strViewOrSpIsShowDataList.Equals("1"))
                {
                    this.BindDataGrid(true);
                }
                //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
                this.SettingPageByIsHaveKey(this.bIsHaveKey);

                //报表查询日志写入 add by sammen 20250304
                DataLogWriter.Log_QueryReport(this.GetUserCode(), RequestUtils.GetIP(), this.strIsViewOrSp.Equals("sp")?this.strSPName:this.strViewName, this.hsTableRequestParam);

            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + rmLocResourceManager.GetString("errTipQueryResult") + "');</script>");
            }

            //列表显示状态模式重置
            this.strStatus = "";

        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    #region 设置中英文
    /// <summary>
    /// 设置中英文
    /// </summary>
    private void SetLanguageLabel(ResourceManager rmLocResourceManager)
    {
        this.btnQueryAll.Text = rmLocResourceManager.GetString("btnShowAll");
        this.btnFilterCon.Text = rmLocResourceManager.GetString("btnFilter");
        this.btnFilterCol.Text = rmLocResourceManager.GetString("btnFilterCol");
        this.btnOrder.Text = rmLocResourceManager.GetString("btnOrder");
        this.btnExportExl.Text = rmLocResourceManager.GetString("btnExportExcel");
        this.ImageBtnSearch.ToolTip = rmLocResourceManager.GetString("imgQuery");
        this.ImageBtnSearch.AlternateText = rmLocResourceManager.GetString("imgQuery");
        this.lbLabelResult.Text = rmLocResourceManager.GetString("lbQueryResultCount");
        this.btnNewWindow.Text = rmLocResourceManager.GetString("btnNewWindow");
        this.btnClose.Text = rmLocResourceManager.GetString("btnClose");
        this.btnExportTxt.Text = rmLocResourceManager.GetString("btnExportTxt");
        this.strTipToExportExcel = rmLocResourceManager.GetString("lbTipToExportExcel");
    }
    #endregion

    #region viewstate初始化区域
    private string strLocalUrl
    {
        get
        {
            return ViewState["Query_strLocalUrl_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strLocalUrl_ViewState"] = value;
        }
    }
    private string strStatus
    {
        get
        {
            return ViewState["Query_strStatus_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strStatus_ViewState"] = value;
        }
    }
    private bool bIsHaveKey
    {
        get
        {
            if (ViewState["bIsHaveKey"] != null)
            {
                return (bool)ViewState["bIsHaveKey"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["bIsHaveKey"] = value;
        }
    }
    private string strIsViewOrSp
    {
        get
        {
            return ViewState["Query_strIsViewOrSp_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strIsViewOrSp_ViewState"] = value;
        }
    }
    private string strViewName
    {
        get
        {
            return ViewState["Query_strViewName_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strViewName_ViewState"] = value;
        }
    }
    private string strSPName
    {
        get
        {
            return ViewState["Query_strSPName_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strSPName_ViewState"] = value;
        }
    }
    private string strViewOrSpTitleName
    {
        get
        {
            return ViewState["Query_strViewOrSpTitleName_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strViewOrSpTitleName_ViewState"] = value;
        }
    }
    private string strViewOrSpIsShowDataList
    {
        get
        {
            return ViewState["Query_strViewOrSpIsShowDataList_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strViewOrSpIsShowDataList_ViewState"] = value;
        }
    }
    private string strViewOrSpResultListOrderBy
    {
        get
        {
            return ViewState["Query_strViewOrSpResultListOrderBy_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strViewOrSpResultListOrderBy_ViewState"] = value;
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
    private ArrayList arrListGridColumn
    {
        get
        {
            return ViewState["arrListGridColumn_ViewState"] as ArrayList;
        }
        set
        {
            ViewState["arrListGridColumn_ViewState"] = value;
        }
    }
    private Hashtable hsTableColumnLanguage
    {
        get
        {
            return ViewState["hsTableColumnLanguage_ViewState"] as Hashtable;
        }
        set
        {
            ViewState["hsTableColumnLanguage_ViewState"] = value;
        }
    }
    private Hashtable hsTableGridColumn_Hide
    {
        get
        {
            if (ViewState["hsTableGridColumn_Hide_ViewState"] != null)
            {
                return ViewState["hsTableGridColumn_Hide_ViewState"] as Hashtable;
            }
            else
            {
                return new Hashtable();
            }
        }
        set
        {
            ViewState["hsTableGridColumn_Hide_ViewState"] = value;
        }
    }
    private bool bIsShowGridData
    {
        get
        {
            if (ViewState["bIsShowGridData"] != null)
            {
                return (bool)ViewState["bIsShowGridData"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["bIsShowGridData"] = value;
        }
    }
    private string strTipToExportExcel
    {
        get
        {
            return ViewState["strTipToExportExcel"] as string;
        }
        set
        {
            ViewState["strTipToExportExcel"] = value;
        }
    }
    private string strFilterSql
    {
        get
        {
            return ViewState["Query_strFilterSql_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strFilterSql_ViewState"] = value;
        }
    }
    private string strDsSort
    {
        get
        {
            return ViewState["Query_strDsSort_ViewState"] as string;
        }
        set
        {
            ViewState["Query_strDsSort_ViewState"] = value;
        }
    }
    private int iListColumnCount
    {
        get
        {
            if (ViewState["iListColumnCount"] != null)
            {
                return (int)ViewState["iListColumnCount"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iListColumnCount"] = value;
        }
    }

    private DataSet dsGridList
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
                return 20;
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
    #endregion
    
    #region 获取存储查询或者视图查询的标题
    /// <summary>
    /// 获取存储查询或者视图查询的标题
    /// </summary>
    /// <param name="strSP"></param>
    /// <returns></returns>
    private String GetSpOrViewTitleName(String strType,String strSPOrView)
    {
        //是否默认显示数据列表【默认为1即默认显示】add by sammen 20201104
        this.strViewOrSpIsShowDataList = "1";
        this.strViewOrSpResultListOrderBy = "";

        String strTitleName = strSPOrView;
        String strViewName = strSPOrView;
        if (strType.ToLower().Equals("sp"))
        {
            strViewName = "_" + strSPOrView;
        }
        try
        {
            String strSql = "select * from VIEWLANG_1 WHERE VWNAME = '" + strViewName + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                strTitleName = String.IsNullOrEmpty (dt.Rows[0]["VDESC"].ToString())? strTitleName : dt.Rows[0]["VDESC"].ToString();
                if (this.Language.Equals("zh-cn"))
                {
                    strTitleName = String.IsNullOrEmpty(dt.Rows[0]["VDESCCN"].ToString()) ? strTitleName : dt.Rows[0]["VDESCCN"].ToString();
                }

                try
                {
                    //是否默认显示数据列表add by sammen 20201104
                    if (dt.Columns.Contains("IsShowDataList"))
                    {
                        this.strViewOrSpIsShowDataList = dt.Rows[0]["IsShowDataList"].ToString();
                    }
                    //排序规则字段add by sammen 20210118
                    if (dt.Columns.Contains("OrderBy"))
                    {
                        this.strViewOrSpResultListOrderBy = dt.Rows[0]["OrderBy"].ToString();
                        this.strDsSort = this.strViewOrSpResultListOrderBy;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("VIEWLANG_1表中缺少是否默认显示数据列表的字段IsShowDataList");
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        this.lb_QueryTileName.Text = strTitleName;
        this.hfViewOrSpIsShowDataList.Value = this.strViewOrSpIsShowDataList;
        return strTitleName;
    }
    #endregion


    #region 获取可替换表格列名的资源文件（中英文）【暂时不用】
    /// <summary>
    //获取可替换表格列名的资源文件（中英文）
    /// </summary>
    /// <param name="bIsHave"></param>
    private ResourceManager GetColumnCaptionResourceM()
    {
        if (this.rm_ViewColCaption == null)
        {
            String strSourceName = "";
            //根据配置资源文件将DATAGRID中的标题按照当前中英文进行替换
            if (this.strIsViewOrSp.Equals("view"))
            {
                strSourceName = "RM_" + this.strViewName;
            }
            else
            {
                strSourceName = "RM_" + this.strSPName;
            }
            this.rm_ViewColCaption = base.GetResourceManager(strSourceName);

            //如果加载错误异常，则置为空
            try
            {
                this.rm_ViewColCaption.GetStream(strSourceName);
            }
            catch (Exception ex)
            {
                this.rm_ViewColCaption = null;
            }

        }

        return this.rm_ViewColCaption;
    }
    #endregion

    #region 获取该视图各列对应的中英文,并返回一个hashTable
    /// <summary>
    /// 获取该视图各列对应的中英文,并返回一个hashTable
    /// </summary>
    private Hashtable GetViewColLanguge()
    {
        Hashtable hsTable = new Hashtable();

        if (this.hsTableColumnLanguage == null)
        {
            String strViewName = "";
            
            if (this.strIsViewOrSp.Equals("view"))
            {
                strViewName = this.strViewName;
            }
            else
            {
                strViewName = "_" + this.strSPName;
            }
            this.hfTableName.Value = strViewName;

            String strCaptionFieldName = "CAPTION";
            if (this.Language.Equals("zh-cn"))
            {
                strCaptionFieldName = "CAPTIONCN";
            }


            try
            {
                String strSql = "SELECT * FROM VIEWLANG_2 WHERE VWNAME = '" + strViewName + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        String strColName = dr["CNAME"].ToString();
                        String strColCaption = dr[strCaptionFieldName].ToString();
                        hsTable.Add(strColName, strColCaption);
                    }
                }
                this.hsTableColumnLanguage = hsTable;
            }
            catch (Exception ex)
            {
                this.hsTableColumnLanguage = null;
            }

        }

        return hsTable;

    }
    #endregion

    #region 判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
    /// <summary>
    //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
    /// </summary>
    /// <param name="bIsHave"></param>
    private void SettingPageByIsHaveKey(bool bIsHave)
    {
        //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
        if (bIsHave)
        {
            this.trPageArea.Visible = true;
            this.trCountArea.Visible = false;
            //设置获取DataGrid分页部分的数据及显示
            this.SetDataGridPageArea();
        }
        else
        {
            this.trPageArea.Visible = false;
            this.trCountArea.Visible = true;
        }
    }
    #endregion

    #region 获取需显示的动态列名,并返回一个hashTable
    /// <summary>
    /// 获取需显示的动态列名,并返回一个hashTable
    /// </summary>
    private ArrayList GetArrayListViewCol()
    {
        String strViewKeyColName = BaseConfig.Instance.GetConfigValueByKey("KeyName_QueryView");
        ArrayList arrList = new ArrayList();
        String strArrayListObject = "";
        String[] strArray =null;
        String strColumnNameInArr = "";
        if (this.strIsViewOrSp.Equals("view"))
        {
            QueryMainBll bllQuery = new QueryMainBll();
            arrList = bllQuery.GetViewColArrayList(this.strViewName);
        }
        else if (this.strIsViewOrSp.Equals("sp"))
        {
            SPQueryBll bllSpQuery = new SPQueryBll();
            arrList = bllSpQuery.GetSpColArrayList(this.strSPName);
        }
        this.bIsHaveKey = false;
        if (arrList != null)
        {
            for (int i = 0; i < arrList.Count; i++)
            {
                strArrayListObject = arrList[i].ToString();
                strArray = strArrayListObject.Split('※');
                strColumnNameInArr = strArray[0];
                if (strColumnNameInArr.Equals(strViewKeyColName))
                {
                    this.bIsHaveKey = true;
                    arrList.RemoveAt(i);
                }
            }
        }

        //判断是否默认显示数据集
        this.iListColumnCount = arrList.Count;
        this.SetIsShowGridData(this.iListColumnCount);

        return arrList;

    }
    #endregion

    #region 判断是否默认显示数据集
    /// <summary>
    /// 判断是否默认显示数据集
    /// </summary>
    /// <param name="iListColumnCount"></param>
    private void SetIsShowGridData(int iListColumnCount)
    {
        if (iListColumnCount <= int.Parse(strColumnMaxCount))
        {
            this.bIsShowGridData = true;
        }
        else
        {
            this.bIsShowGridData = false;
        }
    }
    #endregion

    #region 动态生成DataGrid的列【暂时不用】
    /// <summary>
    /// 动态生成DataGrid的列
    /// </summary>
    //private void DataGrid1_CreateColumn()
    ////{
    //    try
    //    {
    //        this.DataGrid1.AllowSorting = true;
    //        this.DataGrid1.AutoGenerateColumns = false;

    //        //SortHashTable hsTableCol = this.arrListGridColumn;
    //        foreach (System.Collections.DictionaryEntry entity in hsTableCol)
    //        {
    //            String strColName = entity.Key.ToString();
    //            String strColCaption = strColName;

    //            if (this.rm_ViewColCaption != null)
    //            {
    //                if (!String.IsNullOrEmpty(this.rm_ViewColCaption.GetString(strColName)))
    //                {
    //                    strColCaption = this.rm_ViewColCaption.GetString(strColName);
    //                }
    //            }

    //            BoundColumn boundCol = new BoundColumn();
    //            boundCol.HeaderText = strColCaption;
    //            boundCol.DataField = strColName;
    //            boundCol.SortExpression = strColName;
    //            boundCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
    //            boundCol.HeaderStyle.Width = Unit.Percentage(2);
    //            DataGrid1.Columns.Add(boundCol);
    //        }

    //        this.DataGrid1.SortCommand += new DataGridSortCommandEventHandler(this.DataGrid1_SortCommand);
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }

    //}
    #endregion
    
    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        QueryMainBll bllQuery = new QueryMainBll();
        int iAllCount = 0;
        if (bFresh || (this.dsGridList == null))
        {
            if (this.strIsViewOrSp.Equals("view"))//普通视图查询
            {
                this.dsGridList = bllQuery.GetViewDataInfoByPage(this.strViewName, this.bIsHaveKey, this.iPageIndex, this.iPageSize, this.strFilterSql, this.strDsSort, ref iAllCount);
            }
            else if (this.strIsViewOrSp.Equals("sp"))//存储过程查询
            {
                this.dsGridList = this.GetSpQueryData(bIsHaveKey, ref iAllCount);
            }
        }

        this.iRecordCount = iAllCount;

        this.dsGridList = this.ReplaceDataSetColumnLanguage((DataSet)this.dsGridList);
        Session["ExportDsViewDataViewState"] = this.SetDataSetColumnCaption(this.dsGridList, this.hsTableColumnLanguage);
        Session["ExportDsViewDataViewState_Title"] = this.strViewOrSpTitleName;
        this.DataGrid1.DataSource = this.dsGridList;

        if (this.bIsShowGridData)
        {
            //如果是查询模式，则默认先不显示数据
            if (!this.strStatus.ToLower().Equals("select"))
            {
                this.DataGrid1.DataBind();
            }
            this.lbTipToExportExcel.Text = "";
        }
        else
        {
            this.lbTipToExportExcel.Text = this.strTipToExportExcel;
        }
        //设置结果数显示
        this.SetDsCountLabel();

    }
    #endregion

    #region DataGrid相关事件
    /// <summary>
    /// DataGrid ITEM create事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreate(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)//标题行
        {

            Hashtable hsTableColLanguage = this.hsTableColumnLanguage;
            if (hsTableColLanguage != null)
            {
                int iCount = e.Item.Cells.Count;
                for (int i = 0; i < iCount; i++)
                {
                    if (e.Item.Cells[i].Controls[0] is LinkButton)
                    {
                        LinkButton ctrl = (LinkButton)e.Item.Cells[i].Controls[0];
                        String strColName = ctrl.Text;
                        try
                        {
                            if(hsTableColLanguage.ContainsKey(strColName))
                            {
                                String strColCaption = hsTableColLanguage[strColName].ToString();
                                if (!String.IsNullOrEmpty(strColCaption))
                                {
                                    ctrl.Text = strColCaption;
                                }
                            }
                        }catch(Exception ex)
                        {}
                    }
                }
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
        if (e.Item.ItemType == ListItemType.Header)//标题行
        {
            e.Item.Style.Add("word-break", "keep-all");
            e.Item.Style.Add("word-wrap", "normal");

        }
        //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    if (e.Item.ItemIndex % 2 == 0)//偶数行背景色
        //    {
        //        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#E3EEFD");
        //    }
        //    else//奇数行背景色
        //    {
        //        e.Item.BackColor = System.Drawing.ColorTranslator.FromHtml("#f0f0f0");
        //    }
        //    ////当鼠标移到的时候设置该行颜色为 " "，   并保存原来的背景颜色 
        //    //e.Item.Attributes.Add("onmouseover", "this.style.backgroundColor= '#ffff66'; this.style.cursor='hand'");
        //    ////添加自定义属性，当鼠标移走时还原该行的背景色 
        //    //e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor='#f0f0f0'");
        //    ////鼠标单击时背景变色
        //    //e.Item.Attributes.Add("onclick", "this.style.backgroundColor=red ");
        //}

    }
    #endregion

    #region DataGrid排序
    /// <summary>
    /// DataGrid排序
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (this.dsGridList != null)
        {
            DataSet ds = (DataSet)this.dsGridList;
            DataView defaultView = ds.Tables[0].DefaultView;
            if (this.SortAscending)
            {
                defaultView.Sort = e.SortExpression;
            }
            else
            {
                defaultView.Sort = e.SortExpression + " DESC";
            }
            this.SortAscending = !this.SortAscending;

            //填充DataGrid的数据

            //设置全局dataset
            DataSet dsTemp = new DataSet();
            System.Data.DataTable dt = defaultView.ToTable();
            dsTemp.Tables.Add(dt.Copy());
            this.dsGridList = dsTemp;
            this.dsGridList = this.ReplaceDataSetColumnLanguage((DataSet)this.dsGridList);
            Session["ExportDsViewDataViewState"] = this.dsGridList;
            this.DataGrid1.DataSource = defaultView;
            this.DataGrid1.DataBind();
        }
    }

    /// <summary>
    /// 排序规则
    /// </summary>
    private bool SortAscending
    {
        get
        {
            object obj2 = this.ViewState["SortAscending"];
            return ((obj2 == null) || ((bool)obj2));
        }
        set
        {
            this.ViewState["SortAscending"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 执行存储查询的存储过程
    /// </summary>
    private void ExcuteSpQuery()
    {
        Hashtable hsTableParam = (Hashtable)this.hsTableRequestParam;
        hsTableParam.Remove("SP");
        hsTableParam.Remove("sp");
        hsTableParam.Remove("STATUS");
        hsTableParam.Remove("status");
        SqlParamDao.ExcuteSP(this.strSPName, hsTableParam);
    }

    #region 根据存储过程及传入参数获取存储查询数据
    /// <summary>
    /// 根据存储过程及传入参数获取存储查询数据
    /// </summary>
    /// <param name="bIsHaveKey"></param>
    /// <returns></returns>
    private DataSet GetSpQueryData(bool bIsHaveKey, ref int iAllCount)
    {
        DataSet ds = new DataSet();
        SPQueryBll bllSpQuery = new SPQueryBll();
        Hashtable hsTableParam = (Hashtable)this.hsTableRequestParam;
        ds = bllSpQuery.GetSpDataInfoByPage(this.strSPName, hsTableParam, this.bIsHaveKey, this.iPageIndex, this.iPageSize, this.strFilterSql, this.strDsSort, ref iAllCount);

        //if ((this.hsTableRequestParam != null) && (this.hsTableRequestParam.Count > 0))
        //{
        //    Hashtable hsTableParam = (Hashtable)this.hsTableRequestParam;
        //    hsTableParam.Remove("SP");
        //    if (bIsHaveKey)
        //    {
        //        ds = bllSpQuery.GetSpDataInfoByPage(this.strSPName, hsTableParam, this.iPageIndex, this.iPageSize, ref iAllCount);
        //    }
        //    else
        //    {
        //        ds = bllSpQuery.GetSpAllDataInfo(this.strSPName, hsTableParam, this.bIsHaveKey);
        //    }
        //}
        //else
        //{
        //if (bIsHaveKey)
        //{
        //    ds = bllSpQuery.GetSpDataInfoByPage(this.strSPName, hsTableParam, this.bIsHaveKey,this.iPageIndex, this.iPageSize, this.strFilterSql, this.strDsSort, ref iAllCount);
        //}
        //else
        //{
        //    ds = bllSpQuery.GetSpAllDataInfo(this.strSPName, hsTableParam, bIsHaveKey);
        //}
        //}

        return ds;
    }
    #endregion

    #region 根据视图字段加载下拉框列表
    /// <summary>
    /// 根据视图字段加载下拉框列表
    /// </summary>
    private void BuildDropDownListItems()
    {
        try
        {
            ArrayList arrListCol = this.arrListGridColumn; 
            Hashtable hsTableColLanguage = this.hsTableColumnLanguage;

            int iCount = 0;
            String strArrayListObject = "";
            String[] strArray = null;
            String strColumnNameInArr = "";
            String strColumnDataType = "";
            if (arrListCol != null)
            {
                for (int i = 0; i < arrListCol.Count; i++)
                {
                    strArrayListObject = arrListCol[i].ToString();
                    strArray = strArrayListObject.Split('※');
                    strColumnNameInArr = strArray[0];
                    strColumnDataType = strArray[1];
                    String strColCaption = strColumnNameInArr;
                    //获取中英文显示值
                    if ((hsTableColLanguage != null) && (hsTableColLanguage.ContainsKey(strColumnNameInArr)))
                    {
                        strColCaption = hsTableColLanguage[strColumnNameInArr].ToString();
                    }

                    System.Web.UI.WebControls.ListItem lItem = new System.Web.UI.WebControls.ListItem(strColCaption, strColumnNameInArr + "*" + strColumnDataType);// 构造一项
                    this.dList_Condition1.Items.Insert(iCount, lItem);
                    this.dList_Condition2.Items.Insert(iCount, lItem);
                    this.dList_Condition3.Items.Insert(iCount, lItem);
                    this.dList_Condition4.Items.Insert(iCount, lItem);
                    this.dList_Condition5.Items.Insert(iCount, lItem);
                    iCount++;
                }
            }

            //this.dList_Condition1.AutoPostBack = false;
            //this.dList_Condition1.Attributes.Add("onchange", "javascript:alert(document.getElementById('dList_Condition1').value)");
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 根据视图字段加载列过滤ListBox控件列表
    /// <summary>
    /// 根据视图字段加载列过滤ListBox控件列表
    /// </summary>
    private void BuildHideColumnListBoxItems()
    {
        try
        {
            ArrayList arrListCol = this.arrListGridColumn; 
            Hashtable hsTableColLanguage = this.hsTableColumnLanguage;
            String strArrayListObject = "";
            String[] strArray = null;
            String strColumnNameInArr = "";
            String strColumnDataType = "";
            if (arrListCol != null)
            {
                for (int i = 0; i < arrListCol.Count; i++)
                {
                    strArrayListObject = arrListCol[i].ToString();
                    strArray = strArrayListObject.Split('※');
                    strColumnNameInArr = strArray[0];
                    strColumnDataType = strArray[1];
                    String strColCaption = strColumnNameInArr;
                    //获取中英文显示值
                    if ((hsTableColLanguage != null) && (hsTableColLanguage.ContainsKey(strColumnNameInArr)))
                    {
                        strColCaption = hsTableColLanguage[strColumnNameInArr].ToString();
                    }

                    System.Web.UI.WebControls.ListItem lItem = new System.Web.UI.WebControls.ListItem(strColCaption, strColumnNameInArr);// 构造一项
                    this.lstBoxGridCol.Items.Insert(i, lItem);
                    this.lstBoxGridCol.Items[i].Selected = true;
                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 查询所有当前记录
    /// <summary>
    /// 查询所有当前记录
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnQueryAll_Click(object sender, EventArgs e)
    {
        try
        {
            this.strFilterSql = "";
            this.strDsSort = "";
            this.iPageIndex = 0;
            //填充DataGrid的数据
            this.BindDataGrid(true);
            //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
            this.SettingPageByIsHaveKey(this.bIsHaveKey);

            //点击查询以后，客户端设置为显示列表 add by sammen 20201104
            this.hfViewOrSpIsShowDataList.Value = "1";
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 根据查询条件查询
    /// <summary>
    /// 根据查询条件查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Boolean boolCheckBox1 = this.cb_search1.Checked;
        Boolean boolCheckBox2 = this.cb_search2.Checked;
        Boolean boolCheckBox3 = this.cb_search3.Checked;
        Boolean boolCheckBox4 = this.cb_search4.Checked;
        Boolean boolCheckBox5 = this.cb_search5.Checked;
        String strCondition1 = "";
        String strCondition2 = "";
        String strCondition3 = "";
        String strCondition4 = "";
        String strCondition5 = "";
        String strRule1 = "";
        String strRule2 = "";
        String strRule3 = "";
        String strRule4 = "";
        String strRule5 = "";
        String strCondValue1 = "";
        String strCondValue2 = "";
        String strCondValue3 = "";
        String strCondValue4 = "";
        String strCondValue5 = "";


        String strTempFilterSql = "";
        if (boolCheckBox1)
        {
            strCondition1 = this.dList_Condition1.SelectedValue ;
            strRule1 = this.DDList_rule1.SelectedValue;
            strCondValue1 = this.txtCondition1.Text.ToString().Trim();
            strTempFilterSql = this.GetFilterSqlStr(strCondition1, strRule1, strCondValue1, strTempFilterSql);
        }
        if (boolCheckBox2)
        {
            strCondition2 = this.dList_Condition2.SelectedValue;
            strRule2 = this.DDList_rule2.SelectedValue;
            strCondValue2 = this.txtCondition2.Text.ToString().Trim();
            strTempFilterSql = this.GetFilterSqlStr(strCondition2, strRule2, strCondValue2, strTempFilterSql);
        }
        if (boolCheckBox3)
        {
            strCondition3 = this.dList_Condition3.SelectedValue;
            strRule3 = this.DDList_rule3.SelectedValue;
            strCondValue3 = this.txtCondition3.Text.ToString().Trim();
            strTempFilterSql = this.GetFilterSqlStr(strCondition3, strRule3, strCondValue3, strTempFilterSql);
        }
        if (boolCheckBox4)
        {
            strCondition4 = this.dList_Condition4.SelectedValue;
            strRule4 = this.DDList_rule4.SelectedValue;
            strCondValue4 = this.txtCondition4.Text.ToString().Trim();
            strTempFilterSql = this.GetFilterSqlStr(strCondition4, strRule4, strCondValue4, strTempFilterSql);
        }
        if (boolCheckBox5)
        {
            strCondition5 = this.dList_Condition5.SelectedValue;
            strRule5 = this.DDList_rule5.SelectedValue;
            strCondValue5 = this.txtCondition5.Text.ToString().Trim();
            strTempFilterSql = this.GetFilterSqlStr(strCondition5, strRule5, strCondValue5, strTempFilterSql);
        }
        this.strFilterSql = strTempFilterSql;
        //this.BindDataGridFilter();
        this.iPageIndex = 0;
        this.BindDataGrid(true);
        //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
        this.SettingPageByIsHaveKey(this.bIsHaveKey);

        //点击查询以后，客户端设置为显示列表 add by sammen 20201104
        this.hfViewOrSpIsShowDataList.Value = "1";

    }
    #endregion

    #region 数据列过滤
    /// <summary>
    /// 数据列过滤
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageBtnFilterCol_Click(object sender, ImageClickEventArgs e)
    {
        Hashtable hsTable = new Hashtable();
        int iCount = this.lstBoxGridCol.Items.Count;
        for (int i = 0; i < iCount; i++)
        {
            if (this.lstBoxGridCol.Items[i].Selected == false)
            {
                String strColName = this.lstBoxGridCol.Items[i].Value;
                hsTable.Add(strColName, strColName);
                this.iListColumnCount--;
            }
        }
        this.hsTableGridColumn_Hide = hsTable;
        //判断是否默认显示数据集
        this.SetIsShowGridData(this.iListColumnCount);

        this.iPageIndex = 0;
        this.BindDataGrid(true);
        //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
        this.SettingPageByIsHaveKey(this.bIsHaveKey);

        //点击查询以后，客户端设置为显示列表 add by sammen 20201104
        this.hfViewOrSpIsShowDataList.Value = "1";

    }
    #endregion

    #region 排序确定点击事件
    /// <summary>
    /// 排序确定点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOrderOk_Click(object sender, EventArgs e)
    {
        if (this.dsGridList != null)
        {
            String strSortString = this.hfSortString.Value;
            if (!String.IsNullOrEmpty(strSortString))
            {
                #region 首先去掉已经隐藏的列名
                String[] strArr = strSortString.Split(',');
                if ((strArr != null) && (strArr.Length > 0))
                {
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        String str = strArr[i];
                        String strN = str.Replace("[", "").Replace("]", "");
                        if(str.IndexOf(" DESC")>0)
                        {
                            strN = strN.Replace(" DESC", "");
                            if (this.hsTableGridColumn_Hide.ContainsKey(strN))
                            {
                                if (i < strArr.Length - 1)
                                {
                                    strSortString = strSortString.Replace(str + ",", "");
                                }
                                else
                                {
                                    if (strSortString.IndexOf(",") > 0)
                                    {
                                        strSortString = strSortString.Replace("," + str, "");
                                    }
                                    else
                                    {
                                        strSortString = strSortString.Replace(str, "");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this.hsTableGridColumn_Hide.ContainsKey(strN))
                            {
                                if (i < strArr.Length - 1)
                                {
                                    strSortString = strSortString.Replace(str + ",", "");
                                }
                                else
                                {
                                    if (strSortString.IndexOf(",") > 0)
                                    {
                                        strSortString = strSortString.Replace("," + str, "");
                                    }
                                    else
                                    {
                                        strSortString = strSortString.Replace(str, "");
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                this.strDsSort = strSortString;
                this.iPageIndex = 0;
                this.BindDataGrid(true);

                //判断是否存在唯一键值列，如果有责服务器分页，如果没有则不分页
                this.SettingPageByIsHaveKey(this.bIsHaveKey);
            }
        }
    }
    #endregion

    #region 获取相应过滤条件sql
    /// <summary>
    /// 获取相应过滤条件sql
    /// </summary>
    /// <param name="strCondition"></param>
    /// <param name="strRule"></param>
    /// <param name="strCondValue"></param>
    /// <param name="strFilterSql"></param>
    /// <returns></returns>
    private String GetFilterSqlStr(String strConditionAndDataType, String strRule, String strCondValue, String strFilterSql)
    {
        String strTempFilter = "";
        String[] strArr = strConditionAndDataType.Split('*');
        String strCondition = "";//条件字段名
        String strCondDataType = "";//字段数据类型

        if ((strArr != null) && (strArr.Length == 2))
        {
            strCondDataType = strArr[1];
            strCondition = "[" + strArr[0] + "]";
        }

        if (strRule.Equals("like"))
        {
            //处理拼音检索汉字
            if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
            {
                strTempFilter = "dbo.fun_getPY(" + strCondition + ") " + " like '%" + strCondValue + "%'";
            }
            else
            {
                strTempFilter = strCondition + " like '%" + strCondValue + "%'";
            }
        }
        else
        {
            //根据不同数据类型拼写过滤条件语句
            if (strCondDataType.ToLower().Equals("datetime"))//时间字段类型
            {
                strTempFilter = "CONVERT(varchar(100)," + strCondition + ",23)" + strRule + "  '" + strCondValue + "'";
            }
            else
            {
                strTempFilter = strCondition + strRule + "  '" + strCondValue + "'";
            }
        }

        if (strFilterSql.Equals(""))
        {
            strFilterSql = strTempFilter;
        }
        else
        {
            strFilterSql = strFilterSql + " AND " + strTempFilter;
        }
        return strFilterSql;
    }
    #endregion

    #region 设置结果数显示
    /// <summary>
    /// 设置结果数显示
    /// </summary>
    private void SetDsCountLabel(){
        //设置结果数显示
        DataSet ds = (DataSet)this.dsGridList;
        int iCount = 0;
        if ((ds != null) && (ds.Tables.Count > 0))
        {
            iCount = ds.Tables[0].Rows.Count;
        }
        this.lbResultCount.Text = iCount.ToString();
    }
    #endregion 

    //#region 导出到excel文件
    ///// <summary>
    ///// 导出到excel文件
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnExportExl_Click(object sender, EventArgs e)
    //{
    //    if (this.dsGridList != null)
    //    {
    //        Session["ExportDsViewDataViewState"] = this.dsGridList;
    //        Page.ClientScript.RegisterStartupScript(typeof(Page), "errclientscript", "<script language=javascript>window.open('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "','_blank','toolbar=no,scrollbars=yes,location=no,resizable=yes,smenubar=no' )</script>");

    //        //动态生成DataGrid的列
    //        //this.DataGrid1_CreateColumn();
    //        //填充DataGrid的数据
    //        this.BindDataGrid(false);
    //    }
    //}
    //#endregion

    //#region 导出到txt文件
    ///// <summary>
    ///// 导出到txt文件
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnExportTxt_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (this.dsGridList != null)
    //        {
    //            DataSet ds = (DataSet)this.dsGridList;
    //            System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                int i = 0;
    //                for (int n = 0; n < ds.Tables[0].Rows.Count; n++)
    //                {
    //                    for (int m = 0; m < ds.Tables[0].Columns.Count; m++)
    //                    {
    //                        i++;
    //                        sb.Append(ds.Tables[0].Rows[n][m].ToString());
    //                    }
    //                    sb.Append("\r\n");
    //                }
    //                HttpContext.Current.Response.Clear();
    //                HttpContext.Current.Response.Buffer = true;
    //                HttpContext.Current.Response.Charset = "GB2312";
    //                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=abc.txt");
    //                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
    //                Response.ContentType = "text/plain";//设置输出文件类型为txt文件。 
    //                this.EnableViewState = false;
    //                System.Globalization.CultureInfo myCItrad = new System.Globalization.CultureInfo("ZH-CN", true);
    //                System.IO.StringWriter oStringWriter = new System.IO.StringWriter(myCItrad);
    //                HttpContext.Current.Response.Write(sb.ToString());
    //                HttpContext.Current.Response.End();
    //                HttpContext.Current.Response.Close();

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }
    //}
    //#endregion

    #region 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// <summary>
    /// 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// </summary>
    /// <returns>Hashtable</returns>
    private Hashtable GetUrlAnalyse()
    {
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
                        hsTable.Add(strArray3[0], strArray3[1]);
                    }
                }
            }
            else
            {
                String[] strArray4 = strUrl.Split('=');
                if (strArray4.Length == 2)
                {
                    hsTable.Add(strArray4[0], strArray4[1]);
                }
            }
        }
        return hsTable;
    }
    #endregion
    
    #region 生成查询区域 【暂时不用】
    /// <summary>
    /// 生成查询区域
    /// </summary>
    /// <returns></returns>
    private void BiuldSearchArea()
    {
        StringBuilder strBuilderTitle = new StringBuilder();
        StringBuilder strBuilderContent = new StringBuilder();

        int iCount = 5;
        for (int i = 0; i < iCount; i++)
        {
            String strCount = i.ToString();
            strBuilderContent.Append("								<tr style=\"width:100%\">\r\n");
            strBuilderContent.Append("								    <td width=\"5%\">\r\n");
            strBuilderContent.Append("									    <asp:CheckBox ID=\"cb_search\"" + strCount + " runat=\"server\" />\r\n");
            strBuilderContent.Append("									</td>\r\n");
            strBuilderContent.Append("			                        <td width=\"35%\">\r\n");
            strBuilderContent.Append("			          	                <asp:DropDownList ID=\"dList_Condition\"" + strCount + " runat=\"server\">\r\n");
            strBuilderContent.Append("                                      </asp:DropDownList>\r\n");
            strBuilderContent.Append("			                        </td>\r\n");
            strBuilderContent.Append("			                        <td width=\"10%\">\r\n");
            strBuilderContent.Append("			          	                <asp:DropDownList ID=\"DDList_rule\"" + strCount + " runat=\"server\">\r\n");
            strBuilderContent.Append("			          	                    <asp:ListItem Value=\"=\">=</asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\">\">></asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\">=\">>=</asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\"<\"><</asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\"<=\"><=</asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\"<>\"><></asp:ListItem>\r\n");
            strBuilderContent.Append("	                                        <asp:ListItem Value=\"like\">like</asp:ListItem>\r\n");
            strBuilderContent.Append("                                      </asp:DropDownList>\r\n");
            strBuilderContent.Append("			                        </td>\r\n");
            strBuilderContent.Append("			                        <td width=\"50%\">\r\n");
            strBuilderContent.Append("			          	                <asp:TextBox ID=\"txtCondition\"" + strCount + " runat=\"server\"  Width = \"90%\"></asp:TextBox>\r\n");
            strBuilderContent.Append("			          	            </td>\r\n");
            strBuilderContent.Append("			                    </tr>\r\n");

        }

        this.divSearchArea.InnerHtml = strBuilderTitle.ToString();
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
                this.DDList_CurPage.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
            this.DDList_CurPage.ClearSelection();
            this.DDList_CurPage.SelectedIndex = 0;
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
            this.BindDataGrid(true);
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
            this.BindDataGrid(true);
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
            this.BindDataGrid(true);
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
            this.BindDataGrid(true);
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
        this.BindDataGrid(true);
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
            this.BindDataGrid(true);
            //if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            //{
            //    this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            //}
            //this.DataGrid1.DataBind();

            this.SetDataGridPageArea();
        }
    }
    #endregion

    #region 根据视图的字段中英文显示设置替换Dataset中显示列名,同时去掉隐藏列
    /// <summary>
    /// 根据视图的字段中英文显示设置替换Dataset中显示列名,同时去掉隐藏列
    /// <param name="ds"></param>
    /// </summary>
    private DataSet ReplaceDataSetColumnLanguage(DataSet ds)
    {
        DataSet dsReturn = ds;
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
            try
            {
                ArrayList arrListCol = this.arrListGridColumn;
                Hashtable hsTableColLanguage = this.hsTableColumnLanguage;
                Hashtable hsTableColHide = this.hsTableGridColumn_Hide;
                String strArrayListObject = "";
                String[] strArray = null;
                String strColumnNameInArr = "";
                String strColumnDataType = "";
                if (arrListCol != null)
                {
                    for (int i = 0; i < arrListCol.Count; i++)
                    {
                        strArrayListObject = arrListCol[i].ToString();
                        strArray = strArrayListObject.Split('※');
                        strColumnNameInArr = strArray[0];
                        strColumnDataType = strArray[1];
                        String strColCaption = strColumnNameInArr;
                        //获取中英文显示值
                        if ((hsTableColLanguage != null) && (hsTableColLanguage.ContainsKey(strColumnNameInArr)))
                        {
                            strColCaption = hsTableColLanguage[strColumnNameInArr].ToString();
                            ds.Tables[0].Columns[strColumnNameInArr].Caption = strColCaption;
                        }
                        //去掉不需要显示的的列
                        if ((hsTableColHide != null) && (hsTableColHide.ContainsKey(strColumnNameInArr)))
                        {
                            ds.Tables[0].Columns.Remove(strColumnNameInArr);
                        }
                    }
                }
                dsReturn = ds;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        return dsReturn;
    }
    #endregion

    /// <summary>
    /// 根据列名集合修改对应数据集中的列名
    /// </summary>
    /// <param name="dsSource"></param>
    /// <param name="hsTableColumn"></param>
    /// <returns></returns>
    private DataSet SetDataSetColumnCaption(DataSet dsSource,Hashtable hsTableColumn)
    {
        DataSet dsAim = new DataSet();
        DataTable dt = dsSource.Tables[0];

        if (hsTableColumn != null)
        {
            int iCount = dt.Columns.Count;
            for (int i = 0; i < iCount; i++)
            {
                DataColumn dCol = dt.Columns[i];
                String strColName = dCol.ColumnName;
                if (hsTableColumn.ContainsKey(strColName))
                {
                    String strColCaption = hsTableColumn[strColName].ToString();
                    dCol.Caption = strColCaption == "" ? strColName : strColCaption;
                }

            }
        }
        dsAim = dsSource;
        return dsAim;
    }


}
