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

public partial class Flow_FlowManage_FlowDefineList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //加载流程定义表所有数据到页面中的列表中显示
            try
            {
                //获取流程定义表所有数据dataset
                this.BindDataGrid(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "加载流程定义列表失败！");
            }
        }
    }

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            ViewState["FlowListViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["FlowListViewState"] == null)
            {
                ViewState["FlowListViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["FlowListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取流程定义表数据
    /// <summary>
    /// 获取流程定义表数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetDsFromDb()
    {
        String strSql = "select * from TB_FLOW_DEFINE";
        DataTable dtAll = SqlParamDao.GetDataTableBySql(strSql);
        return dtAll;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("imgBtnDetail");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "编辑";
        }
        ImBtn = (ImageButton)e.Item.FindControl("imgBtnDel");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "删除";
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '是否确定要删除？ '); ");
        }
        ImBtn = (ImageButton)e.Item.FindControl("imgBtnInstance");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "流程实例";
        }
        ImBtn = (ImageButton)e.Item.FindControl("imgBtnPost");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "岗位列表";
        }
        ImBtn = (ImageButton)e.Item.FindControl("imgBtnCreate");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "创建流程";
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '是否确定要创建该流程？ '); ");
        }

    }
    #endregion

    #region 删除流程定义表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        String strSql_deleteByFlowId = "delete from TB_FLOW_DEFINE where SFLOWCODE = '" + strFlowId + "'";
        //执行操作之前需检验其他子表中是否存在记录


        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql_deleteByFlowId);
            if (iCount > 0)
            {
                this.AlertMessageBox(this.Page, "删除流程定义成功！");
            }
            else
            {
                this.AlertMessageBox(this.Page, "删除流程定义失败！");
            }
            //返回页面
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "删除流程定义失败！");
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
        if (ViewState["FlowListViewState"] != null)
        {
            DataTable dt = (DataTable)ViewState["FlowListViewState"];
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
            ViewState["FlowListViewState"] = dtTemp;

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

    #region 对流程定义的系列操作定义
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("EditFlowDefine.aspx?flowId=" + strFlowId);
        }
        else if (e.CommandName == "Instance")
        {
            string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("FlowInstanceList.aspx?flowId=" + strFlowId);
        }
        else if (e.CommandName == "Post")
        {
            string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("FlowPostList.aspx?flowId=" + strFlowId);

        }
        else if (e.CommandName == "Create")
        {
            string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            this.CreateFlowDefine(strFlowId);

        }
    }
    #endregion

    #region 创建流程定义
    /// <summary>
    /// 创建流程定义
    /// </summary>
    /// <param name="strFlowId"></param>
    private void CreateFlowDefine(String strFlowId)
    {
        try
        {
            Hashtable hsTable = new Hashtable();
            hsTable.Add("FlowId", strFlowId);
            int iReturn = SqlParamDao.ExcuteSP("FLOW_CreateFlow", hsTable);
            if (iReturn < 0)
            {
                this.AlertMessageBox(this.Page, "创建流程定义失败！");
            }
            else
            {
                this.AlertMessageBox(this.Page, "创建流程定义成功！");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "创建流程定义失败！");
        }
        Response.Redirect("FlowDefineList.aspx");
       
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditFlowDefine.aspx");
    }
    #endregion


}
