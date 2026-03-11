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


public partial class AppFunction_HRSalary_PayRollReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "showWaitingDiv", "<script language=\"javascript\">ShowWaitingDiv();</script>");

        if (!Page.IsPostBack)
        {
            try
            {
                this.txtPageSize.Attributes.Add("onkeypress", "EnterPageSizeTextBox()");
                this.iPageSize = int.Parse(this.txtPageSize.Text);

                //清空导出excel相关的session
                Session["ExportDsViewDataViewState"] = null;
                Session["ExportDsViewDataViewState_Sql"] = null;
                Session["ExportDsViewDataViewState_Title"] = null;

                //从后台获取当前的薪资期间
                this.strYearMonth = this.getCurYearMonthFromDB();
                this.strSPName_Detail = this.lb_SPName_Detail.Text;
                this.strSPName_Summary = this.lb_SPName_Summary.Text;
                this.strTableName_Pre = this.lb_TableName_Pre.Text;
                this.strReportType = "Detail";

                //初始化薪资月份下拉框
                this.InitYearMonthDDList();
                //初始化报表类型下拉框
                this.InitReportTypeDDList();
                //通过存储过程加载全部数据
                this.BuildPayRollData_Detail();
                //填充DataGrid的数据
                this.strTableName = this.strTableName_Pre + "_" + this.iPageIndex.ToString();   
                this.BindDataGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('Load Failed!');</script>");
            }
        }

    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(typeof(Page), "hideWaitingDiv", "<script language=\"javascript\">HideWaitingDiv();</script>");
    }

    #region viewstate初始化区域
    private string strYearMonth
    {
        get
        {
            return ViewState["strYearMonth_ViewState"] as string;
        }
        set
        {
            ViewState["strYearMonth_ViewState"] = value;
        }
    }
    private string strReportType
    {
        get
        {
            return ViewState["strReportType_ViewState"] as string;
        }
        set
        {
            ViewState["strReportType_ViewState"] = value;
        }
    }
    private string strSPName_Detail
    {
        get
        {
            return ViewState["strSPName_ViewState"] as string;
        }
        set
        {
            ViewState["strSPName_ViewState"] = value;
        }
    }
    private string strSPName_Summary
    {
        get
        {
            return ViewState["strSPName_Summary_ViewState"] as string;
        }
        set
        {
            ViewState["strSPName_Summary_ViewState"] = value;
        }
    }
    private string strTableName_Pre
    {
        get
        {
            return ViewState["strTableName_Pre_ViewState"] as string;
        }
        set
        {
            ViewState["strTableName_Pre_ViewState"] = value;
        }
    }
    private string strTableName
    {
        get
        {
            return ViewState["strTableName_ViewState"] as string;
        }
        set
        {
            ViewState["strTableName_ViewState"] = value;
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
                return 1;
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
    #endregion

    /// <summary>
    /// 从后台获取当前的薪资期间
    /// </summary>
    /// <returns></returns>
    private String getCurYearMonthFromDB()
    {
        String strMonth = "";
        String strSql = "select TOP 1 * from KQPERD_1 WHERE PISNOW = '1'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            strMonth = dt.Rows[0]["PID"].ToString();
        }
        return strMonth;
    }

    /// <summary>
    /// 初始化薪资月份下拉框
    /// </summary>
    private void InitYearMonthDDList()
    {
        this.DDList_YearMonth.Items.Clear();
        this.DDList_YearMonth.Items.Add(new ListItem("YearMonth", ""));
        String strSql = "select * from [VW_Sys_YearMonth] where lid = 'YearMonth-PR'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if((dt!=null)&&(dt.Rows.Count>0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                this.DDList_YearMonth.Items.Add(new ListItem(dr["CID"].ToString(), dr["CID"].ToString()));
            }


        }

        this.DDList_YearMonth.ClearSelection();
        this.DDList_YearMonth.SelectedIndex = this.DDList_YearMonth.Items.IndexOf(this.DDList_YearMonth.Items.FindByValue(this.strYearMonth));
    }

    /// <summary>
    /// 初始化报表类型下拉框
    /// </summary>
    private void InitReportTypeDDList()
    {
        this.DDList_ReportType.Items.Clear();
        this.DDList_ReportType.Items.Add(new ListItem("Report Type", ""));
        this.DDList_ReportType.Items.Add(new ListItem("Detail", "Detail"));
        this.DDList_ReportType.Items.Add(new ListItem("Summary", "Summary"));

        this.DDList_ReportType.ClearSelection();
        this.DDList_ReportType.SelectedIndex = this.DDList_ReportType.Items.IndexOf(this.DDList_ReportType.Items.FindByValue(this.strReportType));
    }

    /// <summary>
    /// 选择月份下拉框事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDList_YearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DataGrid1.Visible = true;
        if (!String.IsNullOrEmpty(this.DDList_YearMonth.SelectedValue))
        {
            this.strYearMonth = this.DDList_YearMonth.SelectedValue;
            this.BuildPayRollData();
            this.DDList_YearMonth.SelectedIndex = this.DDList_YearMonth.Items.IndexOf(this.DDList_YearMonth.Items.FindByValue(this.strYearMonth));
            this.iPageIndex = 1;
            this.BindDataGrid();
        }
        else
        {
            this.DataGrid1.Visible = false;
        }

    }

    /// <summary>
    /// 选择月份下拉框事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDList_ReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DataGrid1.Visible = true;
        if (!String.IsNullOrEmpty(this.DDList_ReportType.SelectedValue))
        {
            this.strReportType = this.DDList_ReportType.SelectedValue;
            this.BuildPayRollData();
            this.DDList_ReportType.SelectedIndex = this.DDList_ReportType.Items.IndexOf(this.DDList_ReportType.Items.FindByValue(this.strReportType));
            this.iPageIndex = 1;
            this.BindDataGrid();
        }
        else
        {
            this.DataGrid1.Visible = false;
        }

    }

    /// <summary>
    /// 通过存储过程加载全部数据
    /// </summary>
    private void BuildPayRollData()
    {
        if (this.strReportType.Equals("Detail"))
        {
            this.BuildPayRollData_Detail();
        }
        else if (this.strReportType.Equals("Summary"))
        {
            this.BuildPayRollData_Summary();
        }
    }

    /// <summary>
    /// 通过存储过程加载全部明细数据
    /// </summary>
    private void BuildPayRollData_Detail()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("YearMonth",this.strYearMonth);
        hsTableParam.Add("iPageSize", this.iPageSize);
        SqlParamDao.ExcuteSP(this.strSPName_Detail, hsTableParam);
        //初始化分页页数下拉框
        this.InitTableCountDDList();
    }

    /// <summary>
    /// 通过存储过程加载全部汇总数据
    /// </summary>
    private void BuildPayRollData_Summary()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("YearMonth", this.strYearMonth);
        hsTableParam.Add("iPageSize", this.iPageSize);
        SqlParamDao.ExcuteSP(this.strSPName_Summary, hsTableParam);
        //初始化分页页数下拉框
        this.InitTableCountDDList();
    }

    /// <summary>
    /// 初始化分页页数下拉框
    /// </summary>
    private void InitTableCountDDList()
    {
        this.DDList_TableCount.Items.Clear();
        this.DDList_TableCount.Items.Add(new ListItem("Page", ""));
        String strSql = "select * from " + this.strTableName_Pre + "_Main";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            int iTableCount = int.Parse(dt.Rows[0]["TableCount"].ToString());
            for (int i = 1; i <=iTableCount; i++)
            {
                this.DDList_TableCount.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }


        }

        this.DDList_TableCount.ClearSelection();
        this.DDList_TableCount.SelectedIndex = this.DDList_TableCount.Items.IndexOf(this.DDList_TableCount.Items.FindByValue("1"));
    }

    /// <summary>
    /// 选择分页下拉框事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDList_TableCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DataGrid1.Visible = true;
        if (!String.IsNullOrEmpty(this.DDList_TableCount.SelectedValue))
        {
            this.iPageIndex = int.Parse(this.DDList_TableCount.SelectedValue);
            
            this.BindDataGrid();
        }
        else
        {
            this.DataGrid1.Visible = false;
        }
    }


    /// <summary>
    /// 跳转每页显示数量按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void aChangePageSize_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtPageSize.Text))
        {
            this.iPageSize = Convert.ToInt32(this.txtPageSize.Text);
            this.strYearMonth = this.DDList_YearMonth.SelectedValue;
            this.BuildPayRollData();
            this.DDList_ReportType.SelectedIndex = this.DDList_ReportType.Items.IndexOf(this.DDList_ReportType.Items.FindByValue(this.strReportType));
            this.DDList_YearMonth.SelectedIndex = this.DDList_YearMonth.Items.IndexOf(this.DDList_YearMonth.Items.FindByValue(this.strYearMonth));
            this.iPageIndex = 1;
            this.BindDataGrid();

        }
    }

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid()
    {
        if (this.strReportType.Equals("Detail"))
        {
            this.strTableName = this.strTableName_Pre + "_" + this.iPageIndex.ToString();
            Session["ExportDsViewDataViewState_Title"] = this.Title + "(" + this.iPageIndex.ToString() + ")";
        }
        else if (this.strReportType.Equals("Summary"))
        {
            this.strTableName = this.strTableName_Pre + "_CostCenter_Summary";
            Session["ExportDsViewDataViewState_Title"] = this.Title + "(Summary)";
        }
        String strSql = "select * from " + this.strTableName;
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        this.dsGridList = ds;
        this.DataGrid1.DataSource = ds;
        this.DataGrid1.DataBind();
        Session["ExportDsViewDataViewState"] = this.dsGridList;
        //String strExcelTitle = "PROPERTY NAME \r\n";
        //strExcelTitle = strExcelTitle + "DETAILED PAYROLL & RELATED BY DEPARTMENT \r\n";
        //strExcelTitle = strExcelTitle + "DEPARTMENT: \r\n";
        //strExcelTitle = strExcelTitle + "Month Of: "+this.strYearMonth+"\r\n";
        //Session["ExportDsViewDataViewState_Title"] = strExcelTitle;
    }
    #endregion

    #region DataGrid相关事件
    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        e.Item.Cells[0].Width = Unit.Pixel(30);
        e.Item.Cells[1].Width = Unit.Pixel(200);
        int iCount = e.Item.Cells.Count;
        for (int i = 2; i < iCount; i++)
        {
            e.Item.Cells[i].Width = Unit.Pixel(100);
        }
        if (e.Item.ItemType == ListItemType.Header)//标题行
        {
            e.Item.Style.Add("word-break", "keep-all");
            e.Item.Style.Add("word-wrap", "normal");

        }

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


}