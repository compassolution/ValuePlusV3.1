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

public partial class Flow_FlowManage_FlowPostList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString();
            this.strCurFlowId = strFlowId;
            this.lbFlowId.Text = strFlowId;
            //加载流程岗位表所有数据到页面中的列表中显示
            try
            {
                //获取流程岗位表所有数据dataset
                this.BindDataGrid(strFlowId,true);
                if (!this.IsExsitStartPost())
                {
                    this.lbTipStartPost.Text = "（温馨提示：该流程定义尚未设置起始岗位！！）"; 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "加载流程岗位列表失败!");
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
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(String strFlowId,bool bFresh)
    {
        if (bFresh)
        {
            ViewState["PostListViewState"] = GetDsFromDb(strFlowId);
        }
        else
        {
            if (ViewState["PostListViewState"] == null)
            {
                ViewState["PostListViewState"] = GetDsFromDb(strFlowId);
            }
        }
        this.DataGrid1.DataSource = ViewState["PostListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取流程岗位表数据
    /// <summary>
    /// 获取流程岗位表数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetDsFromDb(String strFlowId)
    {
        String strSql = "select * from TB_FLOW_POST_DEFINE where SFLOWCODE = '" + strFlowId + "'";
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
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton3");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "下一岗位";
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton4");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "岗位参与者";
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton5");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "岗位活动";
        }

    }
    #endregion

    #region 删除流程岗位表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strPostCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        String strSql_deleteByPostCode = "delete from TB_FLOW_POST_DEFINE where SPOSTCODE = '" + strPostCode + "'";
        //执行操作之前需检验其他子表中是否存在记录


        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql_deleteByPostCode);
            if (iCount > 0)
            {
                this.AlertMessageBox(this.Page, "删除流程岗位成功!");
            }
            else
            {
                this.AlertMessageBox(this.Page, "删除流程岗位失败!");
            }
            //返回页面
            this.BindDataGrid(this.strCurFlowId,true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "删除流程岗位失败!");
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
        if (ViewState["PostListViewState"] != null)
        {
            DataTable dt = (DataTable)ViewState["PostListViewState"];
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
            ViewState["PostListViewState"] = dtTemp;

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

    #region 对流程岗位列表的系列操作定义
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strPostCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("EditPostInfo.aspx?flowId=" + this.strCurFlowId + "&postCode=" + strPostCode);
        }
        else if (e.CommandName == "Next")
        {
            string strPostCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + strPostCode);
        }
        else if (e.CommandName == "Actor")
        {
            string strPostCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("PostActorList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + strPostCode);
        }
        else if (e.CommandName == "Action")
        {
            string strPostCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("PostActionList.aspx?flowId="+this.strCurFlowId+"&postCode=" + strPostCode);
        }
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditPostInfo.aspx?flowId="+this.strCurFlowId);
    }
    #endregion

    #region 返回按钮操作
    /// <summary>
    /// 返回按钮操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("FlowDefineList.aspx");
    }
    #endregion

    #region 判断是否已经存在起始岗位
    /// <summary>
    /// 判断是否已经存在起始岗位
    /// </summary>
    /// <returns></returns>
    private bool IsExsitStartPost()
    {
        bool bIs = false;
        String strSql = "select * from TB_FLOW_POST_DEFINE where SFLOWCODE = '" + this.strCurFlowId + "' and BISSTARTPOST = '1'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            bIs = true;
        }
        return bIs;
    }
    #endregion

}
