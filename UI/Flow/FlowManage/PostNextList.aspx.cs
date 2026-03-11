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
using Com.ValuePlus.Utils;

public partial class Flow_FlowManage_PostNextList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strCurFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString();
            String strPostCode = Request.Params["postCode"] == null ? "" : Request.Params["postCode"].ToString();
            this.strCurPathCode = Request.Params["pathCode"] == null ? "" : Request.Params["pathCode"].ToString();
            this.strCurPostCode = strPostCode;

            this.txtPathCode.Text = this.strCurPathCode;
            this.lbPostCode.Text = strPostCode;
            this.txtCurPostCode.Text = strPostCode;
            //加载下一岗位表所有数据到页面中的列表中显示
            try
            {
                //获取下一岗位表所有数据dataset
                this.BindDataGrid(strPostCode, true);
                //绑定所有岗位信息到下拉框
                this.BuildPostToDDList();
                if (!String.IsNullOrEmpty(this.strCurPathCode))
                {
                    this.BindPostNextInfo(true, this.strCurPathCode);
                    ViewState["opKey"] = "modify";
                    this.lbOption.Text = "修改";
                }
                else
                {
                    ViewState["opKey"] = "add";
                    this.lbOption.Text = "新增";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "加载岗位的下一岗位列表失败!");
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
    private string strCurPathCode
    {
        get
        {
            return ViewState["strCurPathCode_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPathCode_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定列表数据
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(String strPostCode, bool bFresh)
    {
        if (bFresh)
        {
            ViewState["PostNextListViewState"] = GetDsFromDb(strPostCode);
        }
        else
        {
            if (ViewState["PostNextListViewState"] == null)
            {
                ViewState["PostNextListViewState"] = GetDsFromDb(strPostCode);
            }
        }
        this.DataGrid1.DataSource = ViewState["PostNextListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取下一岗位表数据
    /// <summary>
    /// 获取下一岗位表数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetDsFromDb(String strPostCode)
    {
        String strSql = "select * from TB_FLOW_PATH where SFLOWCODE = '"+this.strCurFlowId+"' and SPOSTCODE_PRE = '" + strPostCode + "' order by NINDEX";
        DataTable dtAll = SqlParamDao.GetDataTableBySql(strSql);
        return dtAll;
    }
    #endregion

    #region 绑定岗位数据到下拉列表
    /// <summary>
    /// 绑定岗位数据到下拉列表
    /// </summary>
    private void BuildPostToDDList()
    {
        DataTable dt = this.GetAllPostByFlowId(this.strCurFlowId);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            this.DropDownList1.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strPostCode = dt.Rows[i]["SPOSTCODE"].ToString(); ;
                String strPostName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strPostName = dt.Rows[i]["SPOSTNAMECN"].ToString();
                }
                else
                {
                    strPostName = dt.Rows[i]["SPOSTNAME"].ToString();
                }
                ListItem lItem = new ListItem(strPostName, strPostCode);
                this.DropDownList1.Items.Insert(i, lItem);
            }
        }
    }
    #endregion

    #region 获取某流程定义的岗位表所有数据
    /// <summary>
    /// 获取某流程定义的岗位表所有数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetAllPostByFlowId(String strFlowId)
    {
        String strSql = "select * from TB_FLOW_POST_DEFINE where SFLOWCODE = '" + this.strCurFlowId + "' order by SPOSTCODE";
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

    #region 删除下一岗位表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strPostPathCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        String strSql_deleteByPostNextCode = "delete from TB_FLOW_PATH where SFLOWPATHCODE = '" + strPostPathCode + "' and SFLOWCODE ='" + this.strCurFlowId + "'";
        //执行操作之前需检验其他子表中是否存在记录


        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql_deleteByPostNextCode);
            if (iCount > 0)
            {
                this.AlertMessageBox(this.Page, "删除下一岗位成功!");
            }
            else
            {
                this.AlertMessageBox(this.Page, "删除下一岗位失败!");
            }
            //返回页面
            Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode, false);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "删除下一岗位失败!");
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
        if (ViewState["PostNextListViewState"] != null)
        {
            DataTable dt = (DataTable)ViewState["PostNextListViewState"];
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
            ViewState["PostNextListViewState"] = dtTemp;

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

    #region 对路径信息的系列操作定义
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strPostPathCode = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode + "&pathCode=" + strPostPathCode);
        }
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode);
        this.Button1.Text = "新增";
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
        Response.Redirect("PostActorList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode);
    }
    #endregion

    #region 岗位活动列表按钮操作
    /// <summary>
    /// 岗位活动列表按钮操作
    /// </summary>
    protected void Button4_Click(object sender, EventArgs e)
    {
        Response.Redirect("PostActionList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode);
    }
    #endregion

    #region 绑定岗位下岗位信息数据到明细区域
    /// <summary>
    /// 绑定岗位下岗位信息数据到明细区域
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindPostNextInfo(bool bFresh, String strPathCode)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["PostNextViewState"] = GetNextPostInfo(strPathCode);
        }
        else
        {
            if (ViewState["PostNextViewState"] == null)
            {
                ViewState["PostNextViewState"] = GetNextPostInfo(strPathCode);
            }
        }
        dt = (DataTable)ViewState["PostNextViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["SPOSTCODE_NEXT"].ToString()));
            this.txtWorkDay.Text = dt.Rows[0]["NWORKDAY"].ToString();
            this.txtDesc.Text = dt.Rows[0]["SWAYDESC"].ToString();
            this.txtDescCN.Text = dt.Rows[0]["SWAYDESCCN"].ToString();
            this.txtIndex.Text = dt.Rows[0]["NINDEX"].ToString();
            this.txtBizStatus.Text = dt.Rows[0]["SBIZSTATUS"]==null?"0":dt.Rows[0]["SBIZSTATUS"].ToString();
        }
    }
    #endregion

    #region 获取相应岗位活动的明细信息
    /// <summary>
    /// 获取相应岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetNextPostInfo(String strPathCode)
    {
        String strSql = "select * from TB_FLOW_PATH where SFLOWPATHCODE = '" + strPathCode + "' order by NINDEX";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        return dt;
    }
    #endregion

    #region 保存路径操作
    /// <summary>
    /// 保存路径操作
    /// </summary>
    protected void Button5_Click(object sender, EventArgs e)
    {
        String strNextPostCode = this.DropDownList1.SelectedValue.ToUpper();
        String strWorkDay = this.txtWorkDay.Text.Trim();
        String strDesc = this.txtDesc.Text.Trim();
        String strDescCN = this.txtDescCN.Text.Trim();
        String strIndex = this.txtIndex.Text.Trim();
        String strBizStatus = this.txtBizStatus.Text.Trim();

        if (String.IsNullOrEmpty(strNextPostCode))
        {
            this.AlertMessageBox(this.Page, "请选择下一岗位!");
            this.DropDownList1.Focus();
            //返回页面
        }
        else if (String.IsNullOrEmpty(strIndex))
        {
            this.AlertMessageBox(this.Page, "请输入显示顺序!");
            this.txtIndex.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strIndex))
        {
            this.AlertMessageBox(this.Page, "显示顺序应该为正整数!");
            this.txtIndex.Focus();
            //返回页面
        }
        else
        {
            try
            {
                if (ViewState["opKey"].Equals("modify"))
                {

                    this.UpdatePostNextInfo(this.strCurPathCode, this.strCurFlowId, this.strCurPostCode,strNextPostCode, strWorkDay, strDesc, strDescCN, strIndex, strBizStatus);
                    
                    this.AlertMessageBox(this.Page, "下一岗位路径信息更新成功!");
                    Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode + "&pathCode=" + this.strCurPathCode, false);

                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    String strPathCode = Guid.NewGuid().ToString();
                    this.AddPostNextInfo(strPathCode, this.strCurFlowId, this.strCurPostCode, strNextPostCode, strWorkDay, strDesc, strDescCN, strIndex, strBizStatus);
                    this.AlertMessageBox(this.Page, "下一岗位路径信息新增成功!");
                    Response.Redirect("PostNextList.aspx?flowId=" + this.strCurFlowId + "&postCode=" + this.strCurPostCode,false);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "下一岗位路径信息保存出错!");
            }
        }
    }
    #endregion

    #region 新增岗位活动的明细信息
    /// <summary>
    /// 获取相应岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private void AddPostNextInfo(String strPathCode, String strFlowId, String strCurPostCode, String strNextPostCode, String strWorkDay, String strDesc, String strDescCN, String strIndex, String strBizStatus)
    {
        String strSql = "insert into TB_FLOW_PATH ( SFLOWPATHCODE,SFLOWCODE,SPOSTCODE_PRE,SPOSTCODE_NEXT,NWORKDAY,SWAYDESC,SWAYDESCCN,NINDEX,SBIZSTATUS) values('" + strPathCode + "','" + strFlowId + "','" + strCurPostCode + "','" + strNextPostCode + "'," + strWorkDay + ",'" + strDesc + "','" + strDescCN + "'," + strIndex + ",'" + strBizStatus + "')";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion

    #region 根据岗位活动编码更新岗位活动的明细信息
    /// <summary>
    /// 根据岗位活动编码更新岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private void UpdatePostNextInfo(String strPathCode, String strFlowId, String strCurPostCode, String strNextPostCode, String strWorkDay, String strDesc, String strDescCN, String strIndex, String strBizStatus)
    {
        String strSql = "update TB_FLOW_PATH set SFLOWCODE='" + strFlowId + "',SPOSTCODE_PRE='" + strCurPostCode + "',SPOSTCODE_NEXT='" + strNextPostCode + "',NWORKDAY=" + strWorkDay + ",SWAYDESC='" + strDesc + "',SWAYDESCCN='" + strDescCN + "',NINDEX=" + strIndex + ",SBIZSTATUS='" + strBizStatus + "' where SFLOWPATHCODE='" + strPathCode + "'";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion

}
