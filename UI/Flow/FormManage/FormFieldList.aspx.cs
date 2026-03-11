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
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL.Form;

public partial class Flow_FormManage_FormFieldList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFormId = Request.Params["formId"] == null ? "" : Request.Params["formId"].ToString();
            this.lbFormCode.Text = "【" + strFormId + "】";
            //加载特定表单的字段所有数据到页面中的列表中显示
            try
            {
                if (!String.IsNullOrEmpty(strFormId))
                {
                    this.strCurFormId = strFormId;
                    //获取特定表单字段信息所有数据dataset
                    this.BindDataGrid(true, strFormId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "err" + "');</script>");
            }
        }
    }

    #region viewstate初始化区域
    private string strCurFormId
    {
        get
        {
            return ViewState["strCurFormId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFormId_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh,String strFormId)
    {
        if (bFresh)
        {
            ViewState["FormFieldListViewState"] = GetDsFromDb(strFormId);
        }
        else
        {
            if (ViewState["FormFieldListViewState"] == null)
            {
                ViewState["FormFieldListViewState"] = GetDsFromDb(strFormId);
            }
        }
        this.DataGrid1.DataSource = ViewState["FormFieldListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取表单字段表数据
    /// <summary>
    /// 获取表单字段表数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb(String strFormId)
    {
        FormFieldBll bll = new FormFieldBll();
        DataSet dsAll = bll.GetFromFieldInfoByFormId(strFormId);
        return dsAll;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("Imagebutton1");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "编辑";
        }
        ImBtn = (ImageButton)e.Item.FindControl("Imagebutton2");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "删除";
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '是否确定要删除？ '); ");
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton3");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "表单结构";
        }

    }
    #endregion

    #region 删除表单字段当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strFieldId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        FormFieldBll bllFormField = new FormFieldBll();
        try
        {
            int iCount = bllFormField.deleteFormFieldInfo(strFieldId);
            if (iCount > 0)
            {
                Response.Write("<script language=javascript> alert('" + "删除表单字段成功！" + "') </script>");
            }
            else
            {
                Response.Write("<script language=javascript> alert('" + "删除表单字段失败！" + "') </script>");
            }
            //返回页面
            this.BindDataGrid(true,this.strCurFormId);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + "删除表单字段失败！" + "');</script>");
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
        if (ViewState["FormFieldListViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["FormFieldListViewState"];
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
            ViewState["FormFieldListViewState"] = dsTemp;

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

    #region 进入当前表单字段的明细内容页面
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            ////获取DATAGRID的当前行
            string strFieldId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("FormFieldInfo.aspx?formId=" + this.strCurFormId + "&fieldId=" + strFieldId);
        }
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormFieldInfo.aspx?formId=" + this.strCurFormId);
    }
    #endregion

    #region 返回按钮操作
    /// <summary>
    /// 返回按钮操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("FormList.aspx?");
    }
    #endregion

}
