using System;
using System.Text;
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
using Com.ValuePlus.BLL.Notice;
using Com.ValuePlus.Common.Security;

public partial class Notice_NoticeList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strOpType = "view";
            if (Request.Params["opType"] != null)
            {//view表示查看，manage表示管理
                this.strOpType = Request.Params["opType"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpType);
            }

            //初始化页面相关组件
            this.InitComponetByOpType();

            //查询按钮点击不刷新，从而让查询区域可视
            this.Button2.Attributes.Add("onclick", "return false;");
            try
            {
                //获取公告表所有数据dataset
                this.BindDataGrid(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
            }
        }
    }

    #region viewstate初始化区域
    private string strOpType
    {
        get
        {
            return ViewState["NoticeList_strOpType"] as string;
        }
        set
        {
            ViewState["NoticeList_strOpType"] = value;
        }
    }

    private string strTipIsToDelete
    {
        get
        {
            return ViewState["NoticeList_tipIsToDelete"] as string;
        }
        set
        {
            ViewState["NoticeList_tipIsToDelete"] = value;
        }
    }
    private string strTipFaildLoadData
    {
        get
        {
            return ViewState["NoticeList_tipFaildLoadData"] as string;
        }
        set
        {
            ViewState["NoticeList_tipFaildLoadData"] = value;
        }
    }
    private string strTipSuccessDelete
    {
        get
        {
            return ViewState["NoticeList_tipSuccessDelete"] as string;
        }
        set
        {
            ViewState["NoticeList_tipSuccessDelete"] = value;
        }
    }    
    private string strTipFaildDataOp
    {
        get
        {
            return ViewState["NoticeList_tipFaildDataOp"] as string;
        }
        set
        {
            ViewState["NoticeList_tipFaildDataOp"] = value;
        }
    }
    private string strImgBtnView
    {
        get
        {
            return ViewState["NoticeList_imgBtnView"] as string;
        }
        set
        {
            ViewState["NoticeList_imgBtnView"] = value;
        }
    }
    private string strImgBtnEdit
    {
        get
        {
            return ViewState["NoticeList_imgBtnEdit"] as string;
        }
        set
        {
            ViewState["NoticeList_imgBtnEdit"] = value;
        }
    }
    private string strImgBtnDelete
    {
        get
        {
            return ViewState["NoticeList_imgBtnDelete"] as string;
        }
        set
        {
            ViewState["NoticeList_imgBtnDelete"] = value;
        }
    }
    #endregion

    #region 初始化页面相关组件
    /// <summary>
    /// 初始化页面相关组件
    /// </summary>
    private void InitComponetByOpType()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("PublicNotice");

        this.Button1.Text = rmLocResourceManager.GetString("btnAdd").ToString();
        this.Button2.Text = rmLocResourceManager.GetString("btnSearch");

        this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("lbNoticeTitle");
        this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("lbPublishor");
        this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("lbPublishDate");
        this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("lbIsStop");
        this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("lbGridOP");

        this.dList_Condition1.Items[0].Text = rmLocResourceManager.GetString("lbNoticeTitle");
        this.dList_Condition1.Items[1].Text = rmLocResourceManager.GetString("lbPublishor");
        this.dList_Condition1.Items[2].Text = rmLocResourceManager.GetString("lbPublishDate");
        this.dList_Condition1.Items[3].Text = rmLocResourceManager.GetString("lbIsStop");
        this.dList_Condition2.Items[0].Text = rmLocResourceManager.GetString("lbNoticeTitle");
        this.dList_Condition2.Items[1].Text = rmLocResourceManager.GetString("lbPublishor");
        this.dList_Condition2.Items[2].Text = rmLocResourceManager.GetString("lbPublishDate");
        this.dList_Condition2.Items[3].Text = rmLocResourceManager.GetString("lbIsStop");

        this.strTipFaildDataOp = rmLocResourceManager.GetString("tipFaildDataOp").ToString();
        this.strTipFaildLoadData = rmLocResourceManager.GetString("tipFaildLoadData").ToString();
        this.strTipIsToDelete = rmLocResourceManager.GetString("tipIsToDelete").ToString();
        this.strTipSuccessDelete = rmLocResourceManager.GetString("tipSuccessDelete").ToString();
        this.strImgBtnView = rmLocResourceManager.GetString("imgBtnView").ToString();
        this.strImgBtnEdit = rmLocResourceManager.GetString("imgBtnEdit").ToString();
        this.strImgBtnDelete = rmLocResourceManager.GetString("imgBtnDelete").ToString();

        if (this.strOpType.Equals("view"))
        {
            this.divAdd.Visible = false;
            this.DataGrid1.Columns[1].Visible = false;
            this.DataGrid1.Columns[3].Visible = false;
            //this.DataGrid1.Columns[4].Visible = false;
        }
        else
        {
            this.divAdd.Visible = true;
            this.DataGrid1.Columns[1].Visible = false;
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
            ViewState["NoticeList_DataSetViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["NoticeList_DataSetViewState"] == null)
            {
                ViewState["NoticeList_DataSetViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["NoticeList_DataSetViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取公告信息表数据
    /// <summary>
    /// 获取公告信息表数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb()
    {
        NoticeBll bllNotice = new NoticeBll();
        DataSet dsNoticeInfo = new DataSet();
        if (!String.IsNullOrEmpty(this.strOpType))
        {
            switch (this.strOpType)
            {
                case "view"://获取停用的公告
                    dsNoticeInfo = bllNotice.GetNoticInfoByIsStop("0");
                    break;
                case "manage"://获取全部公告信息
                    dsNoticeInfo = bllNotice.GetAllNoticInfo();
                    break;
            }
        }
        return dsNoticeInfo;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("Imagebutton1");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strImgBtnView;
            if (this.strOpType.Equals("view"))
            {
                ImBtn.Visible = true;
            }
        }
        ImBtn = (ImageButton)e.Item.FindControl("Imagebutton2");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strImgBtnEdit;
            if (this.strOpType.Equals("view"))
            {
                ImBtn.Visible = false;
            }
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton3");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = this.strImgBtnDelete;
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '" + this.strTipIsToDelete + " '); ");
            if (this.strOpType.Equals("view"))
            {
                ImBtn.Visible = false;
            }
        }

    }
    #endregion

    #region datagrid Item DataBound
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //String strNoticeKey = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        //if (this.strOpType.Equals("view"))
        //{
        //    e.Item.Attributes.Add("onclick", "return alert('dd');");
        //}

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
        if (ViewState["NoticeList_DataSetViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["NoticeList_DataSetViewState"];
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
            ViewState["NoticeList_DataSetViewState"] = dsTemp;

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

    #region 触发新增操作
    /// <summary>
    /// 触发新增操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("NoticeEdit.aspx?opType=add&noticeKey=");
    }
    #endregion

    #region 根据查询条件查询
    /// <summary>
    /// 根据查询条件查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ImageBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        Boolean boolCheckBox1 = this.cb_search1.Checked;
        Boolean boolCheckBox2 = this.cb_search2.Checked;
        String strCondition1 = "";
        String strCondition2 = "";
        String strRule1 = "";
        String strRule2 = "";
        String strCondValue1 = "";
        String strCondValue2 = "";

        NoticeBll bllNotice = new NoticeBll();
        DataSet dsViewInfo = this.GetDsFromDb();

        String strFilterSql = "";
        if (boolCheckBox1)
        {
            strCondition1 = this.dList_Condition1.SelectedValue;
            strRule1 = this.DDList_rule1.SelectedValue;
            strCondValue1 = this.txtCondition1.Text.ToString().Trim();
            strFilterSql = this.GetFilterSqlStr(strCondition1, strRule1, strCondValue1, strFilterSql);
        }
        if (boolCheckBox2)
        {
            strCondition2 = this.dList_Condition2.SelectedValue;
            strRule2 = this.DDList_rule2.SelectedValue;
            strCondValue2 = this.txtCondition2.Text.ToString().Trim();
            strFilterSql = this.GetFilterSqlStr(strCondition2, strRule2, strCondValue2, strFilterSql);
        }

        try
        {
            //重新加载根据查询条件的DATATABLE和DATASET
            if (!strFilterSql.Equals(""))
            {
                dsViewInfo.Tables[0].DefaultView.RowFilter = strFilterSql;
                this.DataGrid1.DataSource = dsViewInfo.Tables[0].DefaultView;

                //设置全局dataset
                DataSet dsTemp = new DataSet();
                DataView dv = dsViewInfo.Tables[0].DefaultView;
                System.Data.DataTable dt = dv.ToTable();
                dsTemp.Tables.Add(dt.Copy());
                ViewState["NoticeList_DataSetViewState"] = dsTemp;
            }
            else
            {
                this.DataGrid1.DataSource = dsViewInfo;
                ViewState["NoticeList_DataSetViewState"] = dsViewInfo;
            }
            this.DataGrid1.DataBind();
            //设置结果数显示
            //this.SetDsCountLabel();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取相应过滤条件sql
    /// <summary>
    /// 获取相应过滤条件sql
    /// </summary>
    /// <param name="strCondition"></param>
    /// <param name="strRule"></param>
    /// <param name="strCondValue"></param>
    /// <param name="strFilterSql"></param>
    /// <returns></returns>
    private String GetFilterSqlStr(String strCondition, String strRule, String strCondValue, String strFilterSql)
    {
        String strTempFilter = "";
        if (strRule.Equals("like"))
        {
            strTempFilter = strCondition + " like '%" + strCondValue + "%'";
        }
        else
        {
            strTempFilter = strCondition + strRule + "  '" + strCondValue + "'";
        }

        if (strFilterSql.Equals(""))
        {
            strFilterSql = strTempFilter;
        }
        else
        {
            strFilterSql = strFilterSql + " AND " + strTempFilter;
        }
        return strFilterSql;
    }
    #endregion

    #region Item相关事件操作
    /// <summary>
    /// 查看、编辑、删除
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            String strNoticeKey = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strNoticeKey != null) && (!strNoticeKey.Equals("")))
            {
                StringBuilder strBuilderAll = new StringBuilder();
                strBuilderAll.Append("<script language=javascript> showNoticeInfo('" + strNoticeKey + "') ;</script>");
                this.divTemp.InnerHtml = strBuilderAll.ToString();
            }
        }
        else if (e.CommandName == "Edit")
        {
            String strNoticeKey = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strNoticeKey != null) && (!strNoticeKey.Equals("")))
            {
                Response.Redirect("NoticeEdit.aspx?opType=edit&noticeKey=" + strNoticeKey);
            }
        }
        else if (e.CommandName == "Delete")
        {
            String strNoticeKey = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strNoticeKey != null) && (!strNoticeKey.Equals("")))
            {
                this.DeleteNoticeInfo(strNoticeKey);
            }
        }


    }
    #endregion

    #region 删除公告信息表当前记录
    /// <summary>
    /// 删除公告信息表当前记录
    /// </summary>
    /// <param name="strNoticeKey"></param>
    private void DeleteNoticeInfo(String strNoticeKey)
    {
        //执行删除操作
        NoticeBll bllNotice = new NoticeBll();
        try
        {
            int iCount = bllNotice.deleteNoticeInfo(strNoticeKey);
            if (iCount > 0)
            {
                Response.Write("<script language=javascript> alert('" + this.strTipSuccessDelete + "') </script>");
            }
            else
            {
                Response.Write("<script language=javascript> alert('" + this.strTipFaildDataOp + "') </script>");
            }
            //返回页面
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=javascript> alert('" + this.strTipFaildDataOp + "') </script>");
        }
    }
    #endregion

}
