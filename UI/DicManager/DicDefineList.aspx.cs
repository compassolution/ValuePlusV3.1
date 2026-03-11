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

public partial class DicManager_DicDefineList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("DicList");
            String strErr1 = "Data Error!";

            if (rmLocResourceManager.GetString("Err1") != null)
            {
                strErr1 = rmLocResourceManager.GetString("Err1");
            }
            //自定义设置页面文字显示的中英文字符串
            this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("DGHeaderText1");
            this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("DGHeaderText2");
            this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("DGHeaderText3");
            this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("DGHeaderText4");
            this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("DGHeaderText5");
            this.dList_Condition1.Items[0].Text = rmLocResourceManager.GetString("DGHeaderText1");
            this.dList_Condition1.Items[1].Text = rmLocResourceManager.GetString("DGHeaderText2");
            this.dList_Condition1.Items[2].Text = rmLocResourceManager.GetString("DGHeaderText3");
            this.dList_Condition1.Items[3].Text = rmLocResourceManager.GetString("DGHeaderText4");
            this.dList_Condition2.Items[0].Text = rmLocResourceManager.GetString("DGHeaderText1");
            this.dList_Condition2.Items[1].Text = rmLocResourceManager.GetString("DGHeaderText2");
            this.dList_Condition2.Items[2].Text = rmLocResourceManager.GetString("DGHeaderText3");
            this.dList_Condition2.Items[3].Text = rmLocResourceManager.GetString("DGHeaderText4");
            this.Button1.Text = rmLocResourceManager.GetString("btnAdd");
            this.Button2.Text = rmLocResourceManager.GetString("btnSearch");
            this.ImageBtnSearch.ToolTip = rmLocResourceManager.GetString("btnSearch");
            this.ImageBtnSearch.AlternateText = rmLocResourceManager.GetString("btnSearch");

            this.strBtn_Edit = rmLocResourceManager.GetString("imBtn_Edit");
            this.strBtn_Delete = rmLocResourceManager.GetString("imBtn_Delete");
            this.strBtn_Update = rmLocResourceManager.GetString("imBtn_Update");
            this.strBtn_Cancel = rmLocResourceManager.GetString("imBtn_Cancel");
            this.strBtn_Detail = rmLocResourceManager.GetString("imBtn_Detail");
            this.strTip_del_Confirm = rmLocResourceManager.GetString("Tip2");
            this.strTip_Failed = rmLocResourceManager.GetString("Tip4");
            this.strSuccessTip2 = rmLocResourceManager.GetString("Success3");
            this.strSuccessTip1 = rmLocResourceManager.GetString("Success2");

            //查询按钮点击不刷新，从而让查询区域可视
            this.Button2.Attributes.Add("onclick", "return false;");

            //加载清单定义表所有数据到页面中的列表中显示
            try
            {
                //获取清单字典表所有数据dataset
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
    private string strBtn_Edit
    {
        get
        {
            return ViewState["dicDefine_strBtn_Edit_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strBtn_Edit_ViewState"] = value;
        }
    }
    private string strBtn_Delete
    {
        get
        {
            return ViewState["dicDefine_strBtn_Delete_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strBtn_Delete_ViewState"] = value;
        }
    }
    private string strBtn_Update
    {
        get
        {
            return ViewState["dicDefine_strBtn_Update_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strBtn_Update_ViewState"] = value;
        }
    }
    private string strBtn_Cancel
    {
        get
        {
            return ViewState["dicDefine_strBtn_Cancel_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strBtn_Cancel_ViewState"] = value;
        }
    }
    private string strBtn_Detail
    {
        get
        {
            return ViewState["dicDefine_strBtn_Detail_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strBtn_Detail_ViewState"] = value;
        }
    }
    private string strTip_del_Confirm
    {
        get
        {
            return ViewState["dicDefine_strTip_del_Confirm_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strTip_del_Confirm_ViewState"] = value;
        }
    }
    private string strTip_Failed
    {
        get
        {
            return ViewState["dicDefine_strTip_Failed_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strTip_Failed_ViewState"] = value;
        }
    }
    private string strSuccessTip1
    {
        get
        {
            return ViewState["dicDefine_strSuccessTip1_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strSuccessTip1_ViewState"] = value;
        }
    }
    private string strSuccessTip2
    {
        get
        {
            return ViewState["dicDefine_strSuccessTip2_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strSuccessTip2_ViewState"] = value;
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
            ViewState["DicDefineListViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["DicDefineListViewState"] == null)
            {
                ViewState["DicDefineListViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["DicDefineListViewState"];
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
        if (ViewState["DicDefineListViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["DicDefineListViewState"];
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
            ViewState["DicDefineListViewState"] = dsTemp;

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
        DicManagerBll bllDic = new DicManagerBll();
        DataSet dsDicAll = bllDic.GetAllDicListInfo();
        return dsDicAll;
    }
    #endregion

    #region datagrid Item Created
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
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '" + strTip_del_Confirm + " '); ");
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
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton5");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strBtn_Detail;
        }

    }
    #endregion

    #region 根据查询条件查询清单定义信息
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
            if (strFilterSql.Equals("")){
                strFilterSql = strCondition1 + " like '%" + strCondValue1 + "%'";
            }else{
                strFilterSql = strFilterSql + " AND " + strCondition1 + " like '%" + strCondValue1+"%'";
            }
        } 
        if (boolCheckBox2)
        {
            strCondition2 = this.dList_Condition2.SelectedValue;
            strCondValue2 = this.txtCondition2.Text.ToString();
            if (strFilterSql.Equals("")){
                strFilterSql = strCondition2 + " like '%" + strCondValue2 + "%'";
            }else{
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
            ViewState["DicDefineListViewState"] = dsTemp;
        }
        else
        {
            this.DataGrid1.DataSource = ds;
        }
        this.DataGrid1.DataBind();
    }
    #endregion
    
    #region 编辑清单定义表当前记录
    protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
    {
        this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
        this.BindDataGrid(false);

    }
    #endregion

    #region 取消编辑
    protected void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
    {
        this.DataGrid1.EditItemIndex = -1;
        this.BindDataGrid(false);
    }
    #endregion

    #region 提交更新
    protected void DataGrid1_UpdateCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        TextBox tbDesc = (TextBox)e.Item.Cells[1].Controls[0];
        TextBox tbDescChs = (TextBox)e.Item.Cells[2].Controls[0];
        TextBox tbStop = (TextBox)e.Item.Cells[3].Controls[0];
        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
        String strDesc = tbDesc.Text.ToString();
        String strDescChs = tbDescChs.Text.ToString();
        String strStop = tbStop.Text.ToString();


        //执行更新操作
        DicManagerBll bllDic = new DicManagerBll();
        try
        {
            int iCount = bllDic.updateDicListDefine(strLid, strDesc, strDescChs, strStop);
            if (iCount > 0)
            {
                Response.Write("<script language=javascript> alert('" + this.strSuccessTip2 + "') </script>");
            }
            else
            {
                Response.Write("<script language=javascript> alert('" + this.strTip_Failed + "') </script>");
            }
            this.DataGrid1.EditItemIndex = -1;
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + this.strTip_Failed + "');</script>");
        }

    }
    #endregion

    #region 进入当前清单定义的明细内容页面
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strLid != null) && (!strLid.Equals("")))
            {
                Response.Redirect("DicListDetail.aspx?lid=" + strLid);
            }
        }
    }
    #endregion

    #region 删除清单定义表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        //执行删除操作
        DicManagerBll bllDic = new DicManagerBll();
        try
        {
            int iCount = bllDic.deleteDicListDefine(strLid);
            if (iCount > 0)
            {
                Response.Write("<script language=javascript> alert('" + this.strSuccessTip1 + "') </script>");
            }
            else
            {
                Response.Write("<script language=javascript> alert('" + this.strTip_Failed + "') </script>");
            }
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


}
