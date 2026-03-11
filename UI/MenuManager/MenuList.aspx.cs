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
using System.Threading;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.SysManager;

public partial class MenuManager_MenuList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {

            ResourceManager rmLocResourceManager = base.GetResourceManager("MenuManager");
            String strErr1 = "Data Error!";

            if (rmLocResourceManager.GetString("Err1") != null)
            {
                strErr1 = rmLocResourceManager.GetString("Err1");
            }
            //自定义设置页面文字显示的中英文字符串
            this.Button1.Text = rmLocResourceManager.GetString("btnAdd");
            this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("lbMenuCode");
            this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("lbMenuName");
            this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("lbMenuNameChs");
            this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("lbUrlDetail");
            this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("lbMenuType");
            this.DataGrid1.Columns[5].HeaderText = rmLocResourceManager.GetString("lbParentCode");
            this.DataGrid1.Columns[6].HeaderText = rmLocResourceManager.GetString("lbLevel");
            this.DataGrid1.Columns[7].HeaderText = rmLocResourceManager.GetString("lbOrder");
            this.DataGrid1.Columns[8].HeaderText = rmLocResourceManager.GetString("lbEdit");

            String strImBtn_Detail = rmLocResourceManager.GetString("imBtn1");

            //加载栏目表所有数据到页面中的列表中显示
            try
            {
                //获取栏目表所有数据dataset
                this.BindDataGrid(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + strErr1 + "');</script>");
            }
        }

    }

    #region viewstate初始化区域
    private string strImBtn_Detail
    {
        get
        {
            return ViewState["menuDetail_strImBtn_Detail_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strImBtn_Detail_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            ViewState["MenuInfoViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["MenuInfoViewState"] == null)
            {
                ViewState["MenuInfoViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["MenuInfoViewState"];
        this.DataGrid1.DataBind();
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
        if (ViewState["MenuInfoViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["MenuInfoViewState"];
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
            ViewState["MenuInfoViewState"] = dsTemp;

            this.DataGrid1.DataSource = defaultView;
            this.DataGrid1.DataBind();
        }
    }
    #endregion

    #region 排序规则
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

    #region 获取菜单定义表数据
    /// <summary>
    /// 获取菜单定义表数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb()
    {
        MenuManagerBll bllMenu = new MenuManagerBll();
        DataSet dsDicAll = bllMenu.GetAllMenuInfo();
        return dsDicAll;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
         ImageButton ImBtn = (ImageButton)e.Item.FindControl("Imagebutton1");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strImBtn_Detail;
        }

    }
    #endregion

    #region 进入当前栏目的明细内容页面
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            String strMenuCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strMenuCode != null) && (!strMenuCode.Equals("")))
            {
                Response.Redirect("MenuDetail.aspx?menuCode=" + strMenuCode);
            }
        }
    }
    #endregion

    #region 触发新增操作
    /// <summary>
    /// 触发新增操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuDetail.aspx?menuCode=");
    }
    #endregion

}
