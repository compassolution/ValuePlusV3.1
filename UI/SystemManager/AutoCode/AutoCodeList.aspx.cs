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

public partial class SystemManager_AutoCode_AutoCodeList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("AutoCodeManager");

            //自定义设置页面文字显示的中英文字符串
            this.Button1.Text = rmLocResourceManager.GetString("btnAdd");
            this.Button2.Text = rmLocResourceManager.GetString("btnSearch");
            this.dList_Condition1.Items[0].Text = rmLocResourceManager.GetString("lbAid");
            this.dList_Condition1.Items[1].Text = rmLocResourceManager.GetString("lbAdesc");
            this.dList_Condition1.Items[2].Text = rmLocResourceManager.GetString("lbAdescChs");
            this.dList_Condition1.Items[3].Text = rmLocResourceManager.GetString("lbAprefix");
            this.dList_Condition1.Items[4].Text = rmLocResourceManager.GetString("lbAdate");
            this.dList_Condition2.Items[0].Text = rmLocResourceManager.GetString("lbAid");
            this.dList_Condition2.Items[1].Text = rmLocResourceManager.GetString("lbAdesc");
            this.dList_Condition2.Items[2].Text = rmLocResourceManager.GetString("lbAdescChs");
            this.dList_Condition2.Items[3].Text = rmLocResourceManager.GetString("lbAprefix");
            this.dList_Condition2.Items[4].Text = rmLocResourceManager.GetString("lbAdate");
            this.ImageBtnSearch.ToolTip = rmLocResourceManager.GetString("btnSearch");
            this.ImageBtnSearch.AlternateText = rmLocResourceManager.GetString("btnSearch");
            this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("lbAid");
            this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("lbAdesc");
            this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("lbAdescChs");
            this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("lbAprefix");
            this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("lbAdate");
            this.DataGrid1.Columns[5].HeaderText = rmLocResourceManager.GetString("lbAlength");
            this.DataGrid1.Columns[6].HeaderText = rmLocResourceManager.GetString("lbAnextNo");
            this.DataGrid1.Columns[7].HeaderText = rmLocResourceManager.GetString("lbAlastDate");
            this.DataGrid1.Columns[8].HeaderText = rmLocResourceManager.GetString("imBtn_Edit");

            //viewstate设置
            this.strBtn_Edit = rmLocResourceManager.GetString("imBtn_Edit");
            this.strBtn_Delete = rmLocResourceManager.GetString("imBtn_Delete");
            this.strBtn_Update = rmLocResourceManager.GetString("imBtn_Update");
            this.strBtn_Cancel = rmLocResourceManager.GetString("imBtn_Cancel");
            this.strTip_del_Confirm = rmLocResourceManager.GetString("Tip_sureDel");
            this.strTip_Failed = rmLocResourceManager.GetString("Tip_failed");
            this.strSuccess_Del = rmLocResourceManager.GetString("Success_del");
            this.strSuccess_Update = rmLocResourceManager.GetString("Success_update");
            this.strErr_ReadList = rmLocResourceManager.GetString("Err_readList");
            this.strErr_SaveFaild = rmLocResourceManager.GetString("Err_saveFailed");

            //查询按钮点击不刷新，从而让查询区域可视
            this.Button2.Attributes.Add("onclick", "return false;");

            //加载自动编号表所有数据到页面中的列表中显示
            try
            {
                //获取自动编号表所有数据dataset
                this.BindDataGrid(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + strErr_ReadList + "');</script>");
            }
        }
            
    }

    #region viewstate初始化区域
    private string strBtn_Edit
    {
        get
        {
            return ViewState["autoList_strBtn_Edit_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strBtn_Edit_ViewState"] = value;
        }
    }
    private string strBtn_Delete
    {
        get
        {
            return ViewState["autoList_strBtn_Delete_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strBtn_Delete_ViewState"] = value;
        }
    }
    private string strBtn_Update
    {
        get
        {
            return ViewState["autoList_strBtn_Update_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strBtn_Update_ViewState"] = value;
        }
    }
    private string strBtn_Cancel
    {
        get
        {
            return ViewState["autoList_strBtn_Cancel_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strBtn_Cancel_ViewState"] = value;
        }
    }
    private string strTip_del_Confirm
    {
        get
        {
            return ViewState["autoList_strTip_del_Confirm_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strTip_del_Confirm_ViewState"] = value;
        }
    }
    private string strTip_Failed
    {
        get
        {
            return ViewState["autoList_strTip_Failed_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strTip_Failed_ViewState"] = value;
        }
    }
    private string strSuccess_Del
    {
        get
        {
            return ViewState["autoList_strSuccess_Del_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strSuccess_Del_ViewState"] = value;
        }
    }
    private string strSuccess_Update
    {
        get
        {
            return ViewState["autoList_strSuccess_Update_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strSuccess_Update_ViewState"] = value;
        }
    }
    private string strErr_ReadList
    {
        get
        {
            return ViewState["autoList_strErr_ReadList_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strErr_ReadList_ViewState"] = value;
        }
    }
    private string strErr_SaveFaild
    {
        get
        {
            return ViewState["autoList_strErr_SaveFaild_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strErr_strErr_SaveFaild_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定列表数据
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            ViewState["AutoListListViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["AutoListListViewState"] == null)
            {
                ViewState["AutoListListViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["AutoListListViewState"];
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
        if (ViewState["AutoListListViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["AutoListListViewState"];
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
            ViewState["AutoListListViewState"] = dsTemp;

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

    #region 获取自动编号表全部数据
    /// <summary>
    /// 获取自动编号表全部数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb()
    {
        AutoCodeManagerBll bllAuto = new AutoCodeManagerBll();
        return bllAuto.GetAllAutoInfo();
    }
    #endregion

    #region dataGrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("Imagebutton1");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strBtn_Edit;
        }
        ImBtn = (ImageButton)e.Item.FindControl("Imagebutton2");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strBtn_Delete;
            ImBtn.Attributes.Add("onclick ", "return window.confirm( '" + strTip_del_Confirm + " '); ");
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton3");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strBtn_Update;
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton4");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strBtn_Cancel;
        }

    }
    #endregion

    #region dataGrid中下拉框的绑定
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        DropDownList dDList = (DropDownList)e.Item.FindControl("DropDownList3");
        if (dDList != null)
        {
            int num = dDList.Items.IndexOf(dDList.Items.FindByValue(((DataRowView)e.Item.DataItem)["ADATE"].ToString()));
            dDList.SelectedIndex = num;
        }
    }
    #endregion

    #region 根据查询条件查询自动编号表信息
    protected void ImageBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Boolean boolCheckBox1 = this.cb_search1.Checked;
        Boolean boolCheckBox2 = this.cb_search2.Checked;
        String strCondition1 = "";
        String strCondition2 = "";
        String strCondValue1 = "";
        String strCondValue2 = "";

        DataSet ds = GetDsFromDb();
        String strFilterSql = "";
        if (boolCheckBox1)
        {
            strCondition1 = this.dList_Condition1.SelectedValue;
            strCondValue1 = this.txtCondition1.Text.ToString();
            if (strFilterSql.Equals(""))
            {
                strFilterSql = strCondition1 + " like '%" + strCondValue1 + "%'";
            }
            else
            {
                strFilterSql = strFilterSql + " AND " + strCondition1 + " like '%" + strCondValue1 + "%'";
            }
        }
        if (boolCheckBox2)
        {
            strCondition2 = this.dList_Condition2.SelectedValue;
            strCondValue2 = this.txtCondition2.Text.ToString();
            if (strFilterSql.Equals(""))
            {
                strFilterSql = strCondition2 + " like '%" + strCondValue2 + "%'";
            }
            else
            {
                strFilterSql = strFilterSql + " AND " + strCondition2 + " like '%" + strCondValue2 + "%'";
            }
        }
        //重新加载根据查询条件的DATATABLE和DATASET
        if (!strFilterSql.Equals(""))
        {
            ds.Tables[0].DefaultView.RowFilter = strFilterSql;
            this.DataGrid1.DataSource = ds.Tables[0].DefaultView;

            //设置全局dataset
            DataSet dsTemp = new DataSet();
            DataView dv = ds.Tables[0].DefaultView;
            System.Data.DataTable dt = dv.ToTable();
            dsTemp.Tables.Add(dt.Copy());
            ViewState["AutoListListViewState"] = dsTemp;
        }
        else
        {
            this.DataGrid1.DataSource = ds;
        }
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 编辑当前记录
    protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
    {
        this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
        this.BindDataGrid(false);

    }
    #endregion

    #region 取消编辑当前记录
    protected void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
    {
        this.DataGrid1.EditItemIndex = -1;
        this.BindDataGrid(false);
    }
    #endregion

    #region 删除清单定义表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        string strAid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        //执行删除操作
        AutoCodeManagerBll bllAuto = new AutoCodeManagerBll();
        try
        {
            int iCount = bllAuto.DeleteAutoInfoByAid(strAid);
            Response.Write("<script language=javascript> alert('" + this.strSuccess_Del + "') </script>");
            
            //返回页面
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + this.strTip_Failed + "');</script>");
        }
    }
    #endregion

    #region 提交更新
    protected void DataGrid1_UpdateCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        TextBox tbAdesc = (TextBox)e.Item.Cells[1].Controls[0];
        TextBox tbAdescChs = (TextBox)e.Item.Cells[2].Controls[0];
        TextBox tbAprefix = (TextBox)e.Item.Cells[3].Controls[0];
        DropDownList ddLAdate = (DropDownList)e.Item.Cells[4].Controls[1];
        TextBox tbAlength = (TextBox)e.Item.Cells[5].Controls[0];
        TextBox tbAnextNo = (TextBox)e.Item.Cells[6].Controls[0];
        TextBox tbAlastDate = (TextBox)e.Item.Cells[7].Controls[0];
        string strAID = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
        String strADESC = tbAdesc.Text.ToString();
        String strADESCCHS = tbAdescChs.Text.ToString();
        String strAPREFIX = tbAprefix.Text.ToString();
        String strADATE = ddLAdate.SelectedValue.ToString();
        int iALENGTH = 0;
        if (!String.IsNullOrEmpty(tbAlength.Text.ToString()))
        {
            iALENGTH = int.Parse(tbAlength.Text.ToString());
        }
        int iANEXTNO = 0;
        if (!String.IsNullOrEmpty(tbAnextNo.Text.ToString()))
        {
            iANEXTNO = int.Parse(tbAnextNo.Text.ToString());
        }
        String strALASTDATE = tbAlastDate.Text.ToString();


        //执行更新操作
        AutoCodeManagerBll bllAuto = new AutoCodeManagerBll();
        try
        {
            int iCount = bllAuto.UpdateAutoInfoByAid(strAID, strADESC, strADESCCHS, strAPREFIX, strADATE, iALENGTH, iANEXTNO, strALASTDATE);
            Response.Write("<script language=javascript> alert('" + this.strSuccess_Update + "') </script>");

            //返回页面
            this.DataGrid1.EditItemIndex = -1;
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + this.strErr_SaveFaild + "');</script>");
        }

    }
    #endregion


}
