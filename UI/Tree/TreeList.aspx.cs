using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;

using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.Tree;
using Com.ValuePlus.Archive.BLL;

public partial class Tree_TreeList : PageBase
{
    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
        this.DoCreateDataGridColumn();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //modify by sammen 20181130 使用相对地址，兼容https及外网地址映射的需求
        String strExportPage = "../Export/exportindex.aspx";
        this.btnExportExl.Attributes.Add("onclick", "javascript:exportexcel('" + strExportPage + "');return false;");
        //this.btnExportExl.Attributes.Add("onclick", "javascript:exportexcel('" + String.Format(this.GetSiteSchema() + "://{0}/Export/exportindex.aspx", Com.ValuePlus.Utils.RequestUtils.GetCurrentFullHost()) + "');return false;");
        
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                String strUrlQuery = base.Request.Url.Query.ToString();
                strUrlQuery = strUrlQuery.Split('&')[0].ToString();
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(strUrlQuery);
                this.strTid = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TREE");
                this.strCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "CODE");
                this.strOpType = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");//add/edit/readonly


                //树形模板编码
                if (!String.IsNullOrEmpty(this.strTid))
                {
                    this.Label_Code.Text = this.strCode;

                    if (String.IsNullOrEmpty(this.strOpType))
                    {
                        this.strOpType = "readonly";
                    }
                    //add是才能新增
                    if (this.strOpType.ToLower().Equals("add"))
                    {
                        this.aAddSub.Visible = true;
                    }
                    else
                    {
                        this.aAddSub.Visible = false;
                    }

                    //设置中英文显示
                    this.SetLanaguageShow();

                    //加载当前编码的信息
                    this.LoadCurTreeCodeInfo();

                    //获取主显示的列名的DataTable
                    this.DoGetMasterListDataTable(this.strTid);
                    //获取DataGrid数据的DataSet
                    this.DataGridSetting_First();

                    //获取树形模板页面固定动作
                    TreeActionBll.GetTreeActions();


                    /// 主要是准备导出的数据到sessoin中
                    this.GetTreeListDS(this.strTid);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
    }

    #region viewstate初始化区域
    private string strTid
    {
        get
        {
            return ViewState["TID_ViewState"] as string;
        }
        set
        {
            ViewState["TID_ViewState"] = value;
        }
    }
    private string strCode
    {
        get
        {
            return ViewState["CODE_ViewState"] as string;
        }
        set
        {
            ViewState["CODE_ViewState"] = value;
        }
    }
    private string strParentCode
    {
        get
        {
            return ViewState["PARENTCODE_ViewState"] as string;
        }
        set
        {
            ViewState["PARENTCODE_ViewState"] = value;
        }
    }
    private string strOpType
    {
        get
        {
            return ViewState["opType_ViewState"] as string;
        }
        set
        {
            ViewState["opType_ViewState"] = value;
        }
    }
    private DataTable dtMasterLists
    {
        get
        {
            if (this.ViewState["dtMasterLists"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtMasterLists"];
        }
        set
        {
            this.ViewState["dtMasterLists"] = value;
        }
    }
    private DataSet dsGridList
    {
        get
        {
            if (this.ViewState["dsGridList"] == null)
            {
                return new DataSet();
            }
            return (DataSet)this.ViewState["dsGridList"];
        }
        set
        {
            this.ViewState["dsGridList"] = value;
        }
    }
    private bool IsSortAscending
    {
        get
        {
            object obj2 = this.ViewState["IsSortAscending"];
            return ((obj2 == null) || ((bool)obj2));
        }
        set
        {
            this.ViewState["IsSortAscending"] = value;
        }
    }
    #endregion

    #region 加载页面中固定表单的中英文显示信息
    private void SetLanaguageShow()
    {
        String strSql1 = "SELECT * FROM TB_HRTREEH WHERE TID = '" + this.strTid + "' "; 
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql1);
        if ((dt!= null) && (dt.Rows.Count > 0))
        {
            if (this.Language.Equals("zh-cn"))
            {
                this.Label_Title.Text = "当前" + dt.Rows[0]["TDESCCHS"].ToString() ;
                this.Label_SubTitle.Text = "当前" + dt.Rows[0]["TDESCCHS"].ToString() + "下的子" + dt.Rows[0]["TDESCCHS"].ToString();
                this.aRefresh.Text = "重新加载";
                this.aDetail.Text = "查看明细";
                this.aAddSub.Text = "新增子" + dt.Rows[0]["TDESCCHS"].ToString();
                Session["ExportDsViewDataViewState_Title"] = dt.Rows[0]["TDESCCHS"].ToString();
            }
            else
            {
                this.Label_Title.Text = "Current " + dt.Rows[0]["TDESC"].ToString();
                this.Label_SubTitle.Text = "Subsidiary  " + dt.Rows[0]["TDESC"].ToString() + " Lists";
                this.aRefresh.Text = "Refresh";
                this.aDetail.Text = "Detail";
                this.aAddSub.Text = "Add Subsidiary";
                Session["ExportDsViewDataViewState_Title"] = dt.Rows[0]["TDESC"].ToString();
            }
        }
    }
    #endregion

    #region 加载当前编码的信息
    private void LoadCurTreeCodeInfo()
    {
        //修改当前的操作
        String strParamString_Edit = "TREE=" + this.strTid + "&CODE=" + this.strCode + "&PCODE=&OPTYPE=" + this.strOpType;
        strParamString_Edit = UrlParamEncryption.EncryptionUrlParam(strParamString_Edit);
        this.aDetail.Attributes.Add("onclick", "javascript:OpenDetail('" + strParamString_Edit + "');return false;");
        //新增子目录的操作
        String strParamString_Add = "TREE=" + this.strTid + "&CODE=&PCODE=" + this.strCode + "&OPTYPE=" + this.strOpType;
        strParamString_Add = UrlParamEncryption.EncryptionUrlParam(strParamString_Add);
        this.aAddSub.Attributes.Add("onclick", "javascript:OpenDetail('" + strParamString_Add + "');return false;");
        //获取当前编码的相关信息并填入界面
        String strSql1 = "select * from [TREE_" + this.strTid+"] WHERE TREECODE = '" + this.strCode + "'";
        DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
        if ((dt1 != null) && (dt1.Rows.Count > 0))
        {
            this.Label_Name.Text = this.Language.Equals("zh-cn") ? dt1.Rows[0]["TREENAMECHS"].ToString() : dt1.Rows[0]["TREENAME"].ToString();
        }

        if (this.strCode.ToUpper().Equals("ROOT"))
        {
            this.btnExportExl.Visible = true;
            this.btnExportExl.Text = this.Language.Equals("zh-cn") ? "导出清单" : "Export Excel";
        }
        else
        {
            this.btnExportExl.Visible = false;
        }
    }
    #endregion

    /// <summary>
    /// 获取主显示的列名的DataTable
    /// </summary>
    /// <param name="strTid"></param>
    private void DoGetMasterListDataTable(String strTid)
    {
        if ((this.dtMasterLists == null) || (this.dtMasterLists.Rows.Count == 0))
        {
            DataSet ds = new DataSet();
            string strSqlSmsd = "SELECT PID,PDESC,PDESCCHS,PCTRL,PDEFAULT,PCTRLID,PCTRLD,PISKEY,PSYS,PRIGHT,PTYPE,PWIDTH FROM TB_HRTREED WHERE  TID='" + strTid + "' AND PLIST=1 AND PTYPE<>'CH' AND PTYPE<>'CS' and PRIGHT IN ('0','1') ORDER BY PORDER";
            //strSqlSmsd = this.MastAuthorization(strSqlSmsd, strDocuName, strScene, this.GID);
            ds = SqlParamDao.GetDataSetBySql(strSqlSmsd);
            this.dtMasterLists = ds.Tables[0];
        }
    }

    /// <summary>
    /// 重新加载页面
    /// </summary>
    public void RefreshPage()
    {
        this.DoLoadDataGridData(this.strTid, this.strCode);
        this.DataGrid1.DataKeyField = "TREECODE";
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();
    }

    /// <summary>
    /// 重新加载父页面
    /// </summary>
    public void RefreshParentPage()
    {

        //同时加载树形结构
        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    RefreshParent();\r\n");
        strB.Append("</script>");
        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString());
    }

    #region DataGrid区域
    /// <summary>
    /// 设置列表DataGrid相关
    /// </summary>
    private void DataGridSetting_First()
    {
        this.DoCreateDataGridColumn();
        this.DoLoadDataGridData(this.strTid, this.strCode);
        this.DataGrid1.DataKeyField = "TREECODE";
        if ((this.dsGridList != null) && (this.dsGridList.Tables.Count > 0))
        {
            this.DataGrid1.DataSource = this.dsGridList.Tables[0].DefaultView;
        }
        this.DataGrid1.DataBind();

    }

    /// <summary>
    /// 创建DataGrid各动态列
    /// </summary>
    /// <param name="strDocuName"></param>
    /// <param name="strScene"></param>
    /// <param name="strRole"></param>
    protected void DoCreateDataGridColumn()
    {
        if ((this.dtMasterLists != null) && (this.dtMasterLists.Rows.Count > 0))
        {
            DataTable dtFiled = this.dtMasterLists;
            int iCount = dtFiled.Rows.Count;

            String strFieldName = "";
            String strHeadText = "";
            String strFieldType = "";
            String strCtrlType = "";
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dtFiled.Rows[i];
                strFieldName = dr["PID"].ToString();
                strFieldType = dr["PTYPE"].ToString().ToLower();
                strCtrlType = dr["PCTRL"].ToString().ToLower();
                if (this.Language.Equals("zh-cn"))
                {
                    strHeadText = dr["PDESCCHS"].ToString();
                }
                else
                {
                    strHeadText = dr["PDESC"].ToString();
                }

                BoundColumn tcColumn = new BoundColumn();
                tcColumn.HeaderText = strHeadText;
                tcColumn.SortExpression = strFieldName;
                tcColumn.DataField = strFieldName;
                tcColumn.FooterText = strCtrlType;//设置为控件类型
                Double dWidth = Double.Parse(dr["PWIDTH"].ToString());
                if (strFieldType.ToLower().Equals("date"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd}";
                }
                else if (strFieldType.ToLower().Equals("datetime"))
                {
                    tcColumn.DataFormatString = "{0:yyyy-MM-dd  HH:mm:ss}";
                }

                this.DataGrid1.Columns.Add(tcColumn);
            }
        }
    }

    /// <summary>
    /// 加载DataGrid数据
    /// </summary>
    /// <param name="strTreeId"></param>
    /// <param name="strParentCode"></param>
    private void DoLoadDataGridData(string strTreeId, string strParentCode)
    {
        //通过单据编码获取其对应主分组的数据表的前缀
        string strDocMainTable = "TREE_" + strTreeId ;
        string strSqlString = "SELECT * FROM " + strDocMainTable +" ORDER BY TREEORDER";

        if (!String.IsNullOrEmpty(strParentCode))
        {
            strSqlString = "SELECT * FROM " + strDocMainTable + " WHERE PARENTCODE = '" + strParentCode + "' ORDER BY TREEORDER";
        }
        try
        {
            //将sql语句中涉及字典表的替换成字典表相应字段
            ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
            String strSql_Replaced = lstdReplace.GetSqlIncludeLSTHDetail_Tree(strSqlString, strTreeId, this.Language);

            //******** BEGIN 此处捕获异常是为了防治替换sql失败时的终极处理
            try
            {
                //加载读取数据集，并返回记录数
                this.dsGridList = SqlParamDao.GetDataSetBySql(strSql_Replaced);
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            //********* END

            if (this.dsGridList != null && this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0] != null)
            {
                this.dsGridList.Tables[0].TableName = strDocMainTable;
            }
            strSqlString = strSql_Replaced;
            ViewState["DsViewDataViewState"] = SqlParamDao.GetDataSetBySql(strSqlString);//wsm
            Session["ExportDsViewDataViewState"] = ViewState["DsViewDataViewState"];//主要是应用于导出EXCEL功能
        }
        catch (Exception ex)
        {
            //加载读取数据集，并返回记录数
            this.dsGridList = SqlParamDao.GetDataSetBySql(strSqlString);

            log.Error(ex);
            log.Error("将sql语句中涉及字典表的替换成字典表相应字段时出错或者获取界面语句出错ReplaceSqlIncludeTBLSTD.GetSqlIncludeLSTHDetail_Tree(),TID:" + strTreeId);
            this.AlertMessageBox(this, "Get List Data Failed!");
            return;
        }

        try
        {
            if (this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0].Constraints.Count == 0)
            {
                this.dsGridList.Tables[0].Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[] { this.dsGridList.Tables[0].Columns["TREECODE"] }, true));
                this.dsGridList.Tables[0].Columns["TREECODE"].AllowDBNull = false;
                this.dsGridList.Tables[0].Columns["TREECODE"].Unique = true;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this, "DataGrid Set Error!");
            return;
        }
    }

    /// <summary>
    /// DataGrid 列表项目创建事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)//如果可以删除，则显示编辑列，否则不显示
        {
            if (this.strOpType.ToLower().Equals("readonly") || this.strOpType.ToLower().Equals("edit"))
            {
                this.DataGrid1.Columns[0].Visible = false;
            }
            else
            {
                this.DataGrid1.Columns[0].Visible = true;
            }
        }
        if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
        {
            ImageButton button = (ImageButton)e.Item.FindControl("Imagebutton_Delete");
            if (button != null)
            {
                button.ToolTip = "Delete";
                button.Attributes.Add("onclick", "return confirm('" + "Delete it ,Are you sure?" + "');");
            }
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
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

            String strParamString = "TREE=" + this.strTid + "&CODE=" + strCurKeyValue + "&PCODE=" + this.strCode + "&OPTYPE=" + this.strOpType;

            strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
            e.Item.Attributes.Add("ondblclick", "OpenDetail('" + strParamString + "');return false;");

            //为key字段添加超链接
            for (int i = 0; i < e.Item.Cells.Count; i++)
            {
                if (this.DataGrid1.Columns[i].SortExpression.ToString().Equals(this.DataGrid1.DataKeyField.ToString()))
                {
                    TableCellCollection tcl = e.Item.Cells;
                    HyperLink lbKeyFeild = new HyperLink();
                    lbKeyFeild.Text = strCurKeyValue;
                    lbKeyFeild.ToolTip = strCurKeyValue;
                    lbKeyFeild.CssClass = "a_Center";
                    lbKeyFeild.Attributes.Remove("onclick");
                    lbKeyFeild.Attributes.Add("onclick", "javascript:OpenDetail('" + strParamString + "');return false;");

                    e.Item.Cells[i].Controls.Clear();
                    e.Item.Cells[i].Controls.Add(lbKeyFeild);
                }
                
                if(this.DataGrid1.Columns[i].SortExpression.ToString().Equals("BISSTOP"))
                {
                    if (e.Item.Cells[i].Text.Equals("1"))
                    {
                        e.Item.Cells[i].Text = "是";
                    } else if (e.Item.Cells[i].Text.Equals("2"))
                    {
                        e.Item.Cells[i].Text = "否";
                    }
                }

            }

        }

    }

    /// <summary>
    /// DataGrid 列表项目事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCommand(object sender, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "View")
        {
            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            String strParamString = "TREE=" + this.strTid + "&CODE=" + strCurKeyValue + "&PCODE=" + this.strCode + "&OPTYPE=" + this.strOpType;

            strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);

            Page.ClientScript.RegisterStartupScript(typeof(Page), "detailclientscript", "<script language=javascript>OpenDetail('" + strParamString + "'');</script>");
        }
        else if (e.CommandName == "Delete")
        {

            String strCurKeyValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            //先执行删除前的动作执行 add by sammen 20131120
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("TID", this.strTid);
            hsTableParam.Add("Key", "TREECODE");
            hsTableParam.Add("KeyValue", strCurKeyValue);
            hsTableParam.Add("UserId", this.GetUserCode());
            String strDelSql = "";
            String strMsg = "";
            try
            {
                int iCount = TreeActionBll.DoExcuteSP_BeforeDelete(hsTableParam);
                strMsg = TreeActionBll.GetTreeActionTips(strTid, TreeActionBll.ActionID_BeforeDelete, iCount, this.Language);
                if (iCount == 1)///返回值为1时才可以继续执行
                {
                    strDelSql = "DELETE FROM TREE_" + this.strTid + " WHERE TREECODE = '" + strCurKeyValue + "'";
                    int iDeleteCount = SqlParamDao.ExecuteNonQueryBySql(strDelSql);
                    if (iDeleteCount > 0)
                    {
                        //先执行删除后的动作执行 
                        iCount = TreeActionBll.DoExcuteSP_AfterDelete(hsTableParam);
                        strMsg = TreeActionBll.GetTreeActionTips(strTid, TreeActionBll.ActionID_AfterDelete, iCount, this.Language);
                        if (String.IsNullOrEmpty(strMsg))
                        {
                            strMsg = "Delete Successfully!";
                        }
                        base.AlertMessageBox(this, strMsg);
                        this.RefreshPage();
                        this.RefreshParentPage();
                    }
                }
                else
                {
                    log.Error("页面Tree_TreeList.aspx中删除前执行动作失败，方法Delete，树：" + this.strTid + "(TREECODE = '" + strCurKeyValue + "')；返回值" + iCount.ToString() + ";返回提示：" + strMsg);
                    if (String.IsNullOrEmpty(strMsg))
                    {
                        strMsg = "Failed Deleted";
                    }
                    base.AlertMessageBox(this, strMsg);
                    this.RefreshPage();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("页面Tree_TreeList.aspx中删除列表失败，方法Delete，SQL：" + strDelSql.ToString());
                if (String.IsNullOrEmpty(strMsg))
                {
                    strMsg = "Failed Deleted";
                }
                base.AlertMessageBox(this, strMsg);
                this.RefreshPage();
            }
        }
    }

    /// <summary>
    /// DataGrid 排序事件
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (this.dsGridList != null && this.dsGridList.Tables.Count > 0 && this.dsGridList.Tables[0] != null)
        {
            DataView defaultView = this.dsGridList.Tables[0].DefaultView;//取当前页的数据集

            if (this.IsSortAscending)
            {
                defaultView.Sort = e.SortExpression;
            }
            else
            {
                defaultView.Sort = e.SortExpression + " DESC";
            }
            this.IsSortAscending = !this.IsSortAscending;

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

    #region 按钮点击操作
    /// <summary>
    /// 刷新操作
    /// </summary>
    protected void aRefresh_Click(object sender, EventArgs e)
    {
        this.RefreshPage();
        //重新加载父页面
        this.RefreshParentPage();
    }


    /// <summary>
    /// 保存成功操作
    /// </summary>
    protected void aAdd_Click(object sender, EventArgs e)
    {
    }
    #endregion

    private void GetTreeListDS(String strTid)
    {
        //清空导出excel相关的session
        Session["ExportDsViewDataViewState"] = null;
        Session["ExportDsViewDataViewState_Sql"] = null;

        StringBuilder sbSql = new StringBuilder();
        sbSql.Append("select ");

        String strSql_TreeConfig = "select * from TB_HRTREED WHERE TID = '" + strTid + "' order by PORDER";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql_TreeConfig);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                String strPID = dr["PID"].ToString();
                String strPDESC = this.Language.Equals("zh-cn") ? dr["PDESCCHS"].ToString() : dr["PDESC"].ToString();
                if (i == 0)
                {
                    sbSql.Append(strPID + " AS [" + strPDESC + "]");
                }
                else
                {
                    sbSql.Append(","+strPID + " AS [" + strPDESC + "]");
                }
            }
        }
        else
        {
            sbSql.Append(" * ");
        }

        sbSql.Append(" from TREE_" + strTid + " ORDER BY TREEORDER");
        String strSql = sbSql.ToString();
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

        Session["ExportDsViewDataViewState"] = ds;
        Session["ExportDsViewDataViewState_Sql"] = strSql;
    }
}
