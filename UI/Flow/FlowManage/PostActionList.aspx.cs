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
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL;
using Com.ValuePlus.Flow.DAL;

public partial class Flow_FlowManage_PostActionList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strCurFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString();
            String strPostCode = Request.Params["postCode"] == null ? "" : Request.Params["postCode"].ToString();
            this.strCurPostCode = strPostCode;
            //this.strCurFlowId = strFlowId;
            this.lbPostCode.Text = strPostCode;
            //加载岗位活动表所有数据到页面中的列表中显示
            try
            {
                //获取岗位活动表所有数据dataset
                this.BindDataGrid(strPostCode, true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "加载岗位活动列表信息错误!");
            }
        }
    }

    #region viewstate初始化区域
    private string strCurFlowId
    {
        get
        {
            return ViewState["strCurFlowId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFlowId_ViewState"] = value;
        }
    }
    private string strCurPostCode
    {
        get
        {
            return ViewState["strCurPostCode_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPostCode_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(String strPostCode, bool bFresh)
    {
        if (bFresh)
        {
            ViewState["PostActionListViewState"] = GetDsFromDb(strPostCode);
        }
        else
        {
            if (ViewState["PostActionListViewState"] == null)
            {
                ViewState["PostActionListViewState"] = GetDsFromDb(strPostCode);
            }
        }
        this.DataGrid1.DataSource = ViewState["PostActionListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取岗位活动表数据
    /// <summary>
    /// 获取岗位活动表数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetDsFromDb(String strPostCode)
    {
        String strSql = "select * from TB_FLOW_POST_ACTION where SPOSTCODE = '" + strPostCode + "' order by NINDEX";
        DataTable dtAll = SqlParamDao.GetDataTableBySql(strSql);
        return dtAll;
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

    }
    #endregion

    #region 删除岗位活动表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strPostActionCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        String strSql_deleteByPostActionCode = "delete from TB_FLOW_POST_ACTION where SFLOWACTIONCODE = '" + strPostActionCode + "'";
        //执行操作之前需检验其他子表中是否存在记录


        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql_deleteByPostActionCode);
            if (iCount > 0)
            {
                this.AlertMessageBox(this.Page, "删除岗位活动成功!");
            }
            else
            {
                this.AlertMessageBox(this.Page, "删除岗位活动失败!");
            }
            //返回页面
            this.BindDataGrid(this.strCurPostCode, true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "删除岗位活动失败!");
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
        if (ViewState["PostActionListViewState"] != null)
        {
            DataTable dt = (DataTable)ViewState["PostActionListViewState"];
            DataView defaultView = dt.DefaultView;
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
            DataTable dtTemp = new DataTable();
            dtTemp = dt.Copy();
            ViewState["PostActionListViewState"] = dtTemp;

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

    #region 对表单的系列操作定义
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strPostActionCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("EditPostActionInfo.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode + "&actionCode=" + strPostActionCode);
        }
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditPostActionInfo.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode);
    }
    #endregion

    #region 返回按钮操作
    /// <summary>
    /// 返回按钮操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("FlowPostList.aspx?flowId=" + this.strCurFlowId);
    }
    #endregion

    #region 岗位参与者按钮操作
    /// <summary>
    /// 岗位参与者按钮操作
    /// </summary>
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("PostActorList.aspx?flowId="+this.strCurFlowId+"&postCode=" + this.strCurPostCode);
    }
    #endregion

    #region 下岗位列表按钮操作
    /// <summary>
    /// 返回按钮操作
    /// </summary>
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode);
    }
    #endregion

}
