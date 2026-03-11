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
using Com.ValuePlus.Flow.BLL.Flow;
using Com.ValuePlus.Common;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Flow.Config;
using System.Text;

public partial class Flow_WorkFlow_PendingList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //加载当前用户的待办事项列表
            try
            {
                //填充流程类型类型区域查询选项
                this.BuildFlowDefineArea();
                //添加页面控件的键盘相关事件
                this.SetControlKeyEvent();
                //加载当前用户的待办事项列表dataset
                this.strCondition = " 1=1 ";
                this.BindPendingListDataGrid(true);
                //设置获取DataGrid分页部分的数据及显示
                this.SetDataGridPageArea();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载当前用户的待办事项列表失败" + "');</script>");
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
    private DataSet dsGridList
    {
        get
        {
            if (this.ViewState["DsPendingListViewState"] == null)
            {
                return new DataSet();
            }
            return (DataSet)this.ViewState["DsPendingListViewState"];
        }
        set
        {
            this.ViewState["DsPendingListViewState"] = value;
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
                return 0;
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
                return System.Convert.ToInt16(Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("defaultPendingListPageSize")); ;
            }
        }
        set
        {
            ViewState["iPageSize"] = value;
        }
    }
    private int iPageCount
    {
        get
        {
            if (this.ViewState["iPageCount"] == null)
            {
                return 0;
            }
            return (int)this.ViewState["iPageCount"];
        }
        set
        {
            this.ViewState["iPageCount"] = value;
        }
    }
    private int iPageCurNum
    {
        get
        {
            if (this.ViewState["iPageCurNum"] == null)
            {
                return 1;
            }
            return (int)this.ViewState["iPageCurNum"];
        }
        set
        {
            this.ViewState["iPageCurNum"] = value;
        }
    }
    private int iRecordCount
    {
        get
        {
            if (this.ViewState["iRecordCount"] == null)
            {
                return 0;
            }
            return (int)this.ViewState["iRecordCount"];
        }
        set
        {
            this.ViewState["iRecordCount"] = value;
        }
    }
    private string strCbFlowDefineFilterSql
    {
        get
        {
            return ViewState["strCbFlowDefineFilterSql_ViewState"] as string;
        }
        set
        {
            ViewState["strCbFlowDefineFilterSql_ViewState"] = value;
        }
    }
    private string strCondition
    {
        get
        {
            return ViewState["strCondition_ViewState"] as string;
        }
        set
        {
            ViewState["strCondition_ViewState"] = value;
        }
    }
    #endregion

    #region 添加页面控件的键盘相关事件
    /// <summary>
    /// 添加页面控件的键盘相关事件
    /// </summary>
    private void SetControlKeyEvent()
    {
        this.txtSendUser.Attributes.Add("onkeypress", "EnterSearchTextBox('" + this.txtSendUser.ClientID + "')");
        this.txtSendDate_Start.Attributes.Add("onkeypress", "EnterSearchTextBox('" + this.txtSendDate_Start.ClientID + "')");
        this.txtSendDate_End.Attributes.Add("onkeypress", "EnterSearchTextBox('" + this.txtSendDate_End.ClientID + "')");
        this.txtFlowName.Attributes.Add("onkeypress", "EnterSearchTextBox('" + this.txtFlowName.ClientID + "')");
        this.txtPageSize.Attributes.Add("onkeypress", "EnterPageSizeTextBox()"); 
    }
    #endregion

    #region 填充流程类型区域查询选项
    /// <summary>
    /// 填充流程类型区域查询选项
    /// </summary>
    private void BuildFlowDefineArea()
    {
        FlowDefineBll bllDefine = new FlowDefineBll();
        DataSet ds = bllDefine.GetAllFlowDefineInfo();
        if ((ds != null) & (ds.Tables.Count > 0)&&(ds.Tables[0].Rows.Count > 0))
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                String strDefineCode = ds.Tables[0].Rows[i]["SFLOWCODE"].ToString();
                String strHtmlCtrlId = "cb" + strDefineCode;
                String strDefineName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strDefineName = ds.Tables[0].Rows[i]["SFLOWNAMECN"].ToString();
                }
                else
                {
                    strDefineName = ds.Tables[0].Rows[i]["SFLOWNAME"].ToString();
                }
                ListItem listItem = new ListItem();
                listItem.Value = strDefineCode;
                listItem.Text = strDefineName;
                this.CheckBoxList1.Items.Add(listItem);
            }
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindPendingListDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            this.dsGridList = GetPendingListDsFromDb(this.iPageSize, this.iPageIndex, this.strCondition);
        }
        else
        {
            if (this.dsGridList == null)
            {
                this.dsGridList = GetPendingListDsFromDb(this.iPageSize, this.iPageIndex, strCondition);
            }
        }
        this.DataGrid1.DataSource = this.dsGridList;
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 当前用户的待办事项数据
    /// <summary>
    /// 当前用户的待办事项数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetPendingListDsFromDb(int iPageSize,int iPageIndex,String strCondition)
    {
        String strUserId = base.GetUserCode().ToString();
        FlowInstanceBll bll = new FlowInstanceBll();
        int iAllCount = 0;
        DataSet dsAll = bll.GetPendingFlowListByUserId(strUserId, iPageSize, iPageIndex, strCondition, ref iAllCount);
        this.iRecordCount = iAllCount;
        return dsAll;
    }
    #endregion

    #region DataGrid系列事件
    /// <summary>
    /// DataGrid 列表项目创建事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)//如果可以删除，则显示编辑列，否则不显示
        {
            
        }
        if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
        {
           
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
        }

    }

    /// <summary>
    /// DataGrid 列表项目事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "toDeal")
        {
            string strFlowId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if (!String.IsNullOrEmpty(strFlowId))
            {
                String strParam = "flowId=" + strFlowId;
                Response.Redirect("FlowDealPage.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam));
            }
        }
    }

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

            //填充DataGrid的数据

            //设置全局dataset
            DataSet dsTemp = new DataSet();
            System.Data.DataTable dt = defaultView.ToTable();
            dsTemp.Tables.Add(dt.Copy());
            this.dsGridList = dsTemp;

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

    #region 页面分页部分
    /// <summary>
    /// 设置获取DataGrid分页部分的数据及显示
    /// </summary>
    private void SetDataGridPageArea()
    {
        //设置获取DataGrid列表的页数
        this.SetPageCount();
        //初始化当前页数的下拉框
        this.InitCurPageDDList();
        //设置获取DataGrid列表的当前页数
        this.SetPageCurNum();
        this.Label_AllCount.Text = this.iRecordCount.ToString();
        this.txtPageSize.Text = this.iPageSize.ToString();
    }

    /// <summary>
    /// 设置获取DataGrid列表的页数
    /// </summary>
    private void SetPageCount()
    {
        if ((this.iRecordCount % this.iPageSize) == 0)
        {
            this.iPageCount = this.iRecordCount / this.iPageSize;
        }
        else
        {
            this.iPageCount = (this.iRecordCount / this.iPageSize) + 1;
        }
        this.Label_AllPage.Text = this.iPageCount.ToString();
    }

    /// <summary>
    /// 初始化当前页数的下拉框
    /// </summary>
    private void InitCurPageDDList()
    {
        if ((this.iRecordCount > 0) && (this.iPageCount > 0))
        {
            this.DDList_CurPage.Items.Clear();
            for (int i = 1; i <= this.iPageCount; i++)
            {
                this.DDList_CurPage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            this.DDList_CurPage.ClearSelection();
            this.DDList_CurPage.SelectedIndex = this.iPageIndex;
        }
    }

    /// <summary>
    /// 设置获取DataGrid列表的当前页数,并根据当前页数设置按钮可用
    /// </summary>
    private void SetPageCurNum()
    {
        if (this.DDList_CurPage.Items.Count > 0)
        {
            this.DDList_CurPage.SelectedIndex = this.iPageIndex;
        }
        if (this.iPageIndex <= 0)
        {
            this.aFirstPage.Enabled = false;
            this.aPrePage.Enabled = false;
        }
        else
        {
            this.aFirstPage.Enabled = true;
            this.aPrePage.Enabled = true;
        }
        if (this.iPageIndex >= this.iPageCount - 1)
        {
            this.aNextPage.Enabled = false;
            this.aLastPage.Enabled = false;
        }
        else
        {
            this.aNextPage.Enabled = true;
            this.aLastPage.Enabled = true;
        }
    }

    /// <summary>
    /// 首页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = 0;
            //获取DataGrid数据的DataSet
            this.BindPendingListDataGrid(true);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }

    }

    /// <summary>
    /// 上一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PrePage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = this.iPageIndex - 1;
            //获取DataGrid数据的DataSet
            this.BindPendingListDataGrid(true);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 下一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NextPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageIndex + 1;
            //获取DataGrid数据的DataSet
            this.BindPendingListDataGrid(true);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 末页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LastPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageCount - 1;
            //获取DataGrid数据的DataSet
            this.BindPendingListDataGrid(true);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 选择页数下拉框事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DDList_CurPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.iPageIndex = this.DDList_CurPage.SelectedIndex;
        //获取DataGrid数据的DataSet
        this.BindPendingListDataGrid(true);
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();
        //设置获取DataGrid列表的当前页数
        this.SetPageCurNum();
    }

    /// <summary>
    /// 跳转每页显示数量按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GoPageSize_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtPageSize.Text))
        {
            this.iPageIndex = 0;
            this.iPageSize = Convert.ToInt32(this.txtPageSize.Text);
            //获取DataGrid数据的DataSet
            this.BindPendingListDataGrid(true);
            if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
            {
                this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
            }
            this.DataGrid1.DataBind();

            this.SetDataGridPageArea();
        }
    }

    #endregion

    #region 页面按钮点击事件
    /// <summary>
    /// 查询按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Search_Click(object sender, EventArgs e)
    {
        try
        {
            String strFilterSql = this.GetCBDefineFilterSql();
            if (!String.IsNullOrEmpty(this.txtSendUser.Text.Trim()))
            {
                strFilterSql = strFilterSql + "AND (b.SSOURUSERNAME LIKE '%" + this.txtSendUser.Text.Trim() + "%' OR b.SSOURUSERNAMECN LIKE '%" + this.txtSendUser.Text.Trim() + "%' ) ";
            }
            if (!String.IsNullOrEmpty(this.txtSendDate_Start.Text.Trim()))
            {
                strFilterSql = strFilterSql + "AND (b.DTSOURDATE >='" + this.txtSendDate_Start.Text.Trim() + "') ";
            }
            if (!String.IsNullOrEmpty(this.txtSendDate_End.Text.Trim()))
            {
                strFilterSql = strFilterSql + "AND (b.DTSOURDATE <='" + this.txtSendDate_End.Text.Trim() + "') ";
            }
            if (!String.IsNullOrEmpty(this.txtFlowName.Text.Trim()))
            {
                strFilterSql = strFilterSql + "AND (b.SWORKFLOWNAME LIKE '%" + this.txtFlowName.Text.Trim() + "%' OR b.SWORKFLOWNAMECN LIKE '%" + this.txtFlowName.Text.Trim() + "%' ) ";
            }
            if (!String.IsNullOrEmpty(strFilterSql) && (strFilterSql.StartsWith("AND")))
            {
                strFilterSql = strFilterSql.Substring(3, strFilterSql.Length - 3);
            }

            //重新加载根据查询条件的DATATABLE和DATASET
            if (!String.IsNullOrEmpty(strFilterSql))
            {
                this.strCondition = strFilterSql;
            }
            else
            {
                this.strCondition = " 1=1 ";
            }
            this.iPageIndex = 0;
            //重新加载当前用户的待办事项列表dataset
            this.BindPendingListDataGrid(true);
            //设置获取DataGrid分页部分的数据及显示
            this.SetDataGridPageArea();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    #region 获取页面查询流程定义区域的复选框的值
    /// <summary>
    /// 获取页面查询流程定义区域的复选框的值
    /// </summary>
    private String GetCBDefineFilterSql()
    {
        String strSql = "";
        if (this.CheckBoxList1.Items.Count > 0)
        {
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    String strFlowDefineCode = CheckBoxList1.Items[i].Value;
                    strSql = strSql+"OR A.SFLOWCODE ='" + strFlowDefineCode + "' ";
                }
            }
        }
        if (!String.IsNullOrEmpty(strSql) && (strSql.StartsWith("OR")))
        {
            strSql = strSql.Substring(2, strSql.Length - 2);
            strSql = "AND (" + strSql + ")";
        }
        return strSql;
    }
    #endregion


    /// <summary>
    /// 处理按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Deal_Click(object sender, EventArgs e)
    {
        String strParam = "flowId=";
        Response.Redirect("FlowDealPage.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam));
    }

    /// <summary>
    /// 重置按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Reset_Click(object sender, EventArgs e)
    {
        this.test();
    }

    private void test()
    {
        try
        {
            FlowInstanceBll bll = new FlowInstanceBll();

            //获取当前用户信息
            UserInfo entityUser = base.GetUserInfo();
            //设置流程实例信息
            Entity_TB_FLOW_WORK_INSTANCE entityInstance = new Entity_TB_FLOW_WORK_INSTANCE();

            entityInstance.SWORKFLOWCODE = System.Guid.NewGuid().ToString();
            entityInstance.SFLOWCODE = "SALARY";
            entityInstance.SWORKFLOWNAME = "2010年10月份薪资计算";
            entityInstance.SWORKFLOWNAMECN = "2010年10月份薪资计算";
            entityInstance.SENTITYID = "";
            entityInstance.SENTITYNAME = "2010年10月份薪资计算";
            entityInstance.SENTITYNAMECN = "2010年10月份薪资计算";
            entityInstance.SFLOWACCEPTNO = "";
            entityInstance.DTSTARTDATE = DateTime.Now;
            entityInstance.SUSERID = entityUser.SUSERID;
            entityInstance.SDEPTID = entityUser.SDEPT;
            entityInstance.SFLOWMOVECODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_Start");
            entityInstance.SWKSCODE = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowState_Start");
            entityInstance.NCOUNTWORKDAY = 0;
            entityInstance.NSUBNUMBER = 0;
            entityInstance.SBIZSTATUS = Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("defaultBizState");
            entityInstance.BISSUBFLOW = "0";
            entityInstance.SPARENTCODE = null;
            entityInstance.SPENDINGUSERID = entityUser.SUSERID;


            String strFlowId = bll.CreateWorkFlow(entityInstance,entityUser);
            this.Label_AllCount.Text = strFlowId;
            Response.Write("<script language=\"javascript\">alert('" + "发起流程成功" + "');</script>");
            Response.Redirect("PendingList.aspx");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + "发起流程失败" + "');</script>");
        }
    }

    #endregion

}
