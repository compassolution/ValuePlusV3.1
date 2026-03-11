using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Text;

public partial class Tools_DataView : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //modify by sammen 20181130 使用相对地址，兼容https及外网地址映射的需求
        String strExportPage = "../Export/exportindex.aspx";
        this.btnExport.Attributes.Add("onclick", "javascript:exportexcel('" + strExportPage + "');return false;");
        //this.btnExport.Attributes.Add("onclick", "javascript:exportexcel('" + String.Format(this.GetSiteSchema()+"://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");

        if (!Page.IsPostBack)
        {
            //清空导出excel相关的session
            Session["ExportDsViewDataViewState"] = null;
            Session["ExportDsViewDataViewState_Sql"] = null;
            Session["ExportDsViewDataViewState_Title"] = null;

            this.txtSql.Attributes.Add("onkeypress", "EnterSqlTextBox()");
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    #region DataGrid操作
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    private void BindDataGrid()
    {
        try
        {
            this.DataGrid1.Visible = true;
            if ((!String.IsNullOrEmpty(this.txtSql.Text)) && (this.txtSql.Text.Trim().ToUpper().StartsWith("SELECT")))
            {
                String strSql = this.txtSql.Text;
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
                this.DataGrid1.DataSource = ds;
                this.DataGrid1.DataBind();
                ViewState["DsViewDataViewState"] = ds;
                Session["ExportDsViewDataViewState"] = ds;//主要是应用于导出EXCEL功能

                if ((ds != null) && (ds.Tables.Count > 0))
                {
                    int iCount = ds.Tables[0].Rows.Count;
                    this.lbCount.Text = "查询记录数为：" + iCount.ToString();
                }
            }
            else
            {
                this.lbCount.Text = "请填写正确的查询语句！";
            }
        }
        catch (Exception ex)
        {
            this.lbCount.Text = "sql语句执行失败,请检查sql查询语句的有效性";
            this.DataGrid1.Visible = false;
        }

    }

    /// <summary>
    /// DataGrid 列表项目创建事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
        {
            ImageButton button = (ImageButton)e.Item.FindControl("Imagebutton_Edit");
            if (button != null)
            {
                //String strRand = System.Guid.NewGuid().ToString();
                button.AlternateText = e.Item.ItemIndex.ToString();

                button.ToolTip = "点击保存";
                button.Attributes.Add("onclick", "return confirm('是否确定要修改？');");
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
            for (int i = 1; i < e.Item.Cells.Count; i++)
            {
                //BoundColumn tcColumn0 = (BoundColumn)this.DataGrid1.Columns[i];
                //String strHeader = tcColumn0.HeaderText.Trim();

                TextBox txt = new TextBox();
                txt.Text = e.Item.Cells[i].Text;
                if (txt.Text.Equals("&nbsp;"))
                {
                    txt.Text = "";
                }
                //txt.Attributes.Add("FIELD", strHeader);

                e.Item.Cells[i].Controls.Clear();
                e.Item.Cells[i].Controls.Add(txt);
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
        if (e.CommandName == "Edit")
        {
            for (int i = 1; i < e.Item.Cells.Count; i++)
            {
                if (e.Item.Cells[i].Controls[0] is TextBox)
                {
                    TextBox txt = (TextBox)e.Item.Cells[i].Controls[0];


                    BoundColumn tcColumn0 = (BoundColumn)this.DataGrid1.Columns[i];
                    String strHeader = tcColumn0.HeaderText.Trim();


                }
            }

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
        if (ViewState["DsViewDataViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["DsViewDataViewState"];
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
            ViewState["DsViewDataViewState"] = dsTemp;
            Session["ExportDsViewDataViewState"] = ViewState["DsViewDataViewState"];
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
