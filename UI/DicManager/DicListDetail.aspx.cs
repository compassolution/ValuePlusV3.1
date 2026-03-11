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
using System.Resources;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Threading;
using Com.ValuePlus.Common.Security;

public partial class DicManager_DicListDetail : Com.ValuePlus.Web.PageBase
{

    #region 页面加载
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("DicList");
            String strErr1 = rmLocResourceManager.GetString("Err1");

            //自定义设置页面文字显示的中英文字符串
            this.Label1.Text = rmLocResourceManager.GetString("lb_addDicDetail");
            this.Label2.Text = rmLocResourceManager.GetString("DetailHeaderText1");
            this.Label3.Text = rmLocResourceManager.GetString("DetailHeaderText2");
            this.Label4.Text = rmLocResourceManager.GetString("DetailHeaderText3");
            this.Label5.Text = rmLocResourceManager.GetString("DetailHeaderText4");
            this.Label6.Text = rmLocResourceManager.GetString("lbParentId");
            this.Button1.Text = rmLocResourceManager.GetString("btnSave");
            this.Button2.Text = rmLocResourceManager.GetString("btnBack");
            this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("DetailHeaderText1");
            this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("DetailHeaderText2");
            this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("DetailHeaderText3");
            this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("DetailHeaderText4");
            this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("lbParentId");
            this.DataGrid1.Columns[6].HeaderText = rmLocResourceManager.GetString("DetailHeaderText5")+"(0:No;1:Yes)";
            this.DataGrid1.Columns[7].HeaderText = rmLocResourceManager.GetString("DetailHeaderText6");

            this.strBtn_Edit = rmLocResourceManager.GetString("imBtn_Edit");
            this.strBtn_Delete = rmLocResourceManager.GetString("imBtn_Delete");
            this.strBtn_Update = rmLocResourceManager.GetString("imBtn_Update");
            this.strBtn_Cancel = rmLocResourceManager.GetString("imBtn_Cancel");
            this.strBtn_Detail = rmLocResourceManager.GetString("imBtn_Detail");
            this.strTip_del_Confirm = rmLocResourceManager.GetString("Tip2");
            this.strTip_Failed = rmLocResourceManager.GetString("Tip4");
            this.strSuccessTip2 = rmLocResourceManager.GetString("Success3");
            this.strSuccessTip1 = rmLocResourceManager.GetString("Success2");

            this.strTip1 = rmLocResourceManager.GetString("Tip1");
            this.strTip3 = rmLocResourceManager.GetString("Tip3");
            this.strErr2 = rmLocResourceManager.GetString("Err2");
            this.strSuccessTip = rmLocResourceManager.GetString("Success1");

            //加载清单内容明细表所有数据到页面中的列表中显示
            try
            {
                //获取清单明细表相应数据并绑定
                if (!String.IsNullOrEmpty(Request.Params["lid"]))
                {
                    String strLid = Request.Params["lid"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    strLid = SQLInjectionDefense.ReplaceSQLReservedKeyword(strLid);
                    this.TextBox1.Text = strLid;
                    this.GetListDefine(strLid);
                    this.BindDataGrid(true, strLid);
                }

            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, strErr1);
            }
        }
    }
    #endregion

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
    private string strTip1
    {
        get
        {
            return ViewState["dicDefine_strTip1_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strTip1_ViewState"] = value;
        }
    }
    private string strTip3
    {
        get
        {
            return ViewState["dicDefine_strTip3_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strTip3_ViewState"] = value;
        }
    }
    private string strErr2
    {
        get
        {
            return ViewState["dicDefine_strErr2_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strErr2_ViewState"] = value;
        }
    }
    private string strSuccessTip
    {
        get
        {
            return ViewState["dicDefine_strSuccessTip_ViewState"] as string;
        }
        set
        {
            ViewState["dicDefine_strSuccessTip_ViewState"] = value;
        }
    }
    #endregion

    #region 清单定义名称显示
    private void GetListDefine(String strLid)
    {
        String strSql = "Select * from TB_HRLSTH WHERE LID = '"+strLid+"'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            DataRow dr = dt.Rows[0];
            if (this.Language.Equals("zh-cn"))
            {
                this.lbLDESC.Text = dr["LDESCCHS"].ToString();
            }
            else
            {
                this.lbLDESC.Text = dr["LDESC"].ToString();
            }
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh, String strLid)
    {
        if (bFresh)
        {
            ViewState["DicDetailViewState"] = GetDsFromDb(strLid);
        }
        else
        {
            if (ViewState["DicDetailViewState"] == null)
            {
                ViewState["DicDetailViewState"] = GetDsFromDb(strLid);
            }
        }
        this.DataGrid1.DataSource = ViewState["DicDetailViewState"];
        this.DataGrid1.DataBind();
        this.TextBox6.Text = "10";
    }
    #endregion

    #region 获取数据
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb(String strLid)
    {
        DicManagerBll bllDic = new DicManagerBll();
        DataSet dsDicDetail = bllDic.GetDicDetailInfoByLId(strLid);
        return dsDicDetail;
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
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '" + this.strTip_del_Confirm + " '); ");
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

    #region DataGrid排序
    /// <summary>
    /// DataGrid排序
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (ViewState["DicDetailViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["DicDetailViewState"];
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
            ViewState["DicDetailViewState"] = dsTemp;

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

    #region 编辑清单内容明细表当前记录
    protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
    {
        this.DataGrid1.EditItemIndex = e.Item.ItemIndex;
        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
        this.BindDataGrid(false,strLid);
    }
    #endregion

    #region 删除清单内容明细表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
        String strCid = e.Item.Cells[1].Text.ToString();

        //执行删除操作
        DicManagerBll bllDic = new DicManagerBll();
        int iCount = bllDic.deleteDicDetailInfo(strLid, strCid);
        if (iCount > 0)
        {
            this.AlertMessageBox(this.Page, strSuccessTip1);
        }
        else
        {
            this.AlertMessageBox(this.Page, strTip_Failed);
        }
        //返回页面
        this.BindDataGrid(true, strLid);
    }
    #endregion

    #region 取消编辑
    protected void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        this.DataGrid1.EditItemIndex = -1;
        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
        this.BindDataGrid(false, strLid);
    }
    #endregion

    #region 提交更新
    protected void DataGrid1_UpdateCommand(object source, DataGridCommandEventArgs e)
    {
        //获取DATAGRID的当前行
        TextBox tbDesc = (TextBox)e.Item.Cells[2].Controls[0];
        TextBox tbDescChs = (TextBox)e.Item.Cells[3].Controls[0];
        TextBox tbCUID = (TextBox)e.Item.Cells[4].Controls[0];
        TextBox tbP9 = (TextBox)e.Item.Cells[5].Controls[0];
        TextBox tbStop = (TextBox)e.Item.Cells[6].Controls[0];

        string strLid = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString(); ;
        String strCid = e.Item.Cells[1].Text.ToString();
        String strDesc = tbDesc.Text.ToString(); ;
        String strDescChs = tbDescChs.Text.ToString();
        String strCuid = tbCUID.Text.ToString();
        String strP9 = tbP9.Text.ToString();
        String strIsStop = tbStop.Text.ToString();

        //执行更新操作
        DicManagerBll bllDic = new DicManagerBll();
        int iCount = bllDic.updateDicDetailInfo(strLid, strCid, strDesc, strDescChs, strCuid, null, null, null, null, null, null, null, null, null, strP9, strIsStop);
        if (iCount > 0)
        {
            this.AlertMessageBox(this.Page, this.strSuccessTip);
        }
        else
        {
            this.AlertMessageBox(this.Page, this.strTip_Failed);
        }
        //返回页面
        this.DataGrid1.EditItemIndex = -1;
        this.BindDataGrid(true, strLid);

    }
    #endregion

    #region 新增保存操作
    /// <summary>
    /// 新增保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strLid = this.TextBox1.Text.Trim();//清单编码
        String strCid = this.TextBox2.Text.Trim();//明细编码
        String strCDesc = this.TextBox3.Text.Trim();//明细英文名
        String strCDescChs = this.TextBox4.Text.Trim();//明细英文名
        String strCCUid = this.TextBox5.Text.Trim();//Cuid
        String strCP9 = this.TextBox6.Text.Trim();//参数

        if ((strLid == null) || (strLid.Equals("")))
        {
            //返回页面
            this.BindDataGrid(false, strLid);
        }
        else if ((strCid == null) || (strCid.Equals("")))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            //返回页面
            this.BindDataGrid(false, strLid);
        }
        else if ((strCDesc == null) || (strCDesc.Equals("")))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            //返回页面
            this.BindDataGrid(false, strLid);
        }
        else if ((strCDescChs == null) || (strCDescChs.Equals("")))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            //返回页面
            this.BindDataGrid(false, strLid);
        }
        else
        {
            DicManagerBll bllDic = new DicManagerBll();
            //首先判断清单编码的唯一性
            if (bllDic.IsExsitLIDAndCID(strLid, strCid))
            {
                this.AlertMessageBox(this.Page, this.strTip1);
                //返回页面
                this.BindDataGrid(false, strLid);
            }
            else
            {
                try
                {
                    bllDic.AddDicDetailInfo(strLid, strCid, strCDesc, strCDescChs, strCCUid, strCP9, "0");
                    this.AlertMessageBox(this.Page, this.strSuccessTip);
                    //返回页面
                    this.BindDataGrid(true, strLid);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    this.AlertMessageBox(this.Page, this.strErr2);
                }
            }
        }
    }
    #endregion

    #region 返回操作
    /// <summary>
    /// 返回操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=\"javascript\">window.location.href='DicDefineList.aspx';</script>");
    }
    #endregion


}
