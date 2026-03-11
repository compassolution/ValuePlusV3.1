using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;

public partial class News_Manage_NewsList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if(Request.Params["plateId"]!=null)
            {
                this.strPlateId = Request.Params["plateId"].ToString();
                this.GetPlateDetailInfo(this.strPlateId);
                this.hfPlateId.Value = this.strPlateId;
            }

            this.btnDeletePlate.Attributes.Add("onclick", "return window.confirm( 'Are you sure delete it? '); ");

            //加载新闻内容表所有数据到页面中的列表中显示
            try
            {
                //获取新闻内容表所有数据dataset
                this.BindDataGrid(true,strPlateId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }

    #region viewstate初始化区域
    private string strPlateId
    {
        get
        {
            return ViewState["strPlateId_ViewState"] as string;
        }
        set
        {
            ViewState["strPlateId_ViewState"] = value;
        }
    }
    #endregion

    #region 获取相应版块的明细信息
    /// <summary>
    /// 获取相应版块的明细信息
    /// </summary>
    /// <returns></returns>
    public void GetPlateDetailInfo(String strPlateId)
    {
        String strSql = "SELECT * FROM TB_NEWS_PLATE WHERE SPLATEID = '" + strPlateId + "'";
        DataTable dtPlateInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtPlateInfo != null) && (dtPlateInfo.Rows.Count > 0))
        {
            this.lbCurPlateId.Text = dtPlateInfo.Rows[0]["SPLATEID"].ToString() + "【" + dtPlateInfo.Rows[0]["SPLATENAMECHS"].ToString() + "】";
            if (this.Language.Equals("en-us"))
            {
                this.lbCurPlateId.Text = dtPlateInfo.Rows[0]["SPLATEID"].ToString() + "【" + dtPlateInfo.Rows[0]["SPLATENAME"].ToString() + "】";
            }
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh,String strPlateId)
    {
        if (bFresh)
        {
            ViewState["NewsInfoViewState"] = GetDsFromDb(strPlateId);
        }
        else
        {
            if (ViewState["NewsInfoViewState"] == null)
            {
                ViewState["NewsInfoViewState"] = GetDsFromDb(strPlateId);
            }
        }
        this.DataGrid1.DataSource = ViewState["NewsInfoViewState"];
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
        if (ViewState["NewsInfoViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["NewsInfoViewState"];
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
            ViewState["NewsInfoViewState"] = dsTemp;

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

    #region 获取新闻内容数据
    /// <summary>
    /// 获取新闻内容数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb(String strPlateId)
    {
        String strSql = "SELECT * FROM TB_NEWS_CONTENT WHERE SPLATEID = '"+strPlateId+"'";
        DataSet dsDicAll = SqlParamDao.GetDataSetBySql(strSql);
        return dsDicAll;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("imgDelete");
        if (ImBtn != null)
        {
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '" + "Are you sure to Delete it？" + " '); ");
        }

    }
    #endregion

    #region 进入当前栏目的明细内容页面
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            String strNewsId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strNewsId != null) && (!strNewsId.Equals("")))
            {
                String strUrl = "EditNews.aspx?newsId=" + strNewsId + "&plateId=" + this.strPlateId;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>showOpenWindow('" + strUrl + "')</script>"); 
            }
        }
        if (e.CommandName == "Delete")
        {
            String strNewsId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if ((strNewsId != null) && (!strNewsId.Equals("")))
            {
                String strSql = "delete from TB_NEWS_CONTENT WHERE SNEWSID = '" + strNewsId + "'";
                try
                {
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    if (iCount > 0)
                    {
                        this.AlertMessageBox(this.Page, "Successfully！");
                    }
                    else
                    {
                        this.AlertMessageBox(this.Page, "Failed!");
                    }
                    //获取新闻内容表所有数据dataset
                    this.BindDataGrid(true, this.strPlateId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    this.AlertMessageBox(this.Page, "Failed!");
                }
            }
        }
    }
    #endregion

    #region 触发操作
    /// <summary>
    /// 删除操作
    /// </summary>
    protected void btnDeletePlate_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.strPlateId))
        {
            try
            {
                if ((!IsExsitSubLevel(this.strPlateId))&&(!this.IsExsitNews(this.strPlateId)))
                {
                    String strSql = "DELETE FROM TB_NEWS_PLATE WHERE SPLATEID = '" + this.strPlateId + "'";
                    int iDeleteCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    this.AlertMessageBox(this.Page, "Successfully！");
                    Response.Write("<script language=\"javascript\">window.parent.location.reload();;</script>");
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Failed,Can not be delete if have SubPlate OR News!");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "Failed!");
            }
        }
    }

    /// <summary>
    /// 修改当前版块操作
    /// </summary>
    protected void btnModifyPlate_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlateDetail.aspx?plateId=" + this.strPlateId + "&parentId=");
    }
    /// <summary>
    /// 刷新内容列表操作
    /// </summary>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        //获取新闻内容表所有数据dataset
        this.BindDataGrid(true, this.strPlateId);
    }
    /// <summary>
    /// 新增子版块操作
    /// </summary>
    protected void btnAddSubPlate_Click(object sender, EventArgs e)
    {
        Response.Redirect("PlateDetail.aspx?plateId=&parentId=" + this.strPlateId);
    }

    #endregion
    
    #region 判断版块编码是否已经存在下级子版块
    /// <summary>
    /// 判断版块编码是否已经存在下级子版块
    /// </summary>
    /// <param name="strPlateId"></param>
    /// <returns></returns>
    private Boolean IsExsitSubLevel(String strPlateId)
    {
        Boolean bIsExsit = true;
        String strSql = "select * from TB_NEWS_PLATE WHERE SPARENTID = '" + strPlateId + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds == null)//ds为空
        {
            bIsExsit = false;
        }
        else
        {
            if (ds.Tables.Count == 0)//ds中没有表
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                {
                    bIsExsit = false;
                }
            }
        }
        return bIsExsit;
    }
    #endregion

    #region 判断版块编码是否已经存在新闻
    /// <summary>
    /// 判断版块编码是否已经存在新闻
    /// </summary>
    /// <param name="strPlateId"></param>
    /// <returns></returns>
    private Boolean IsExsitNews(String strPlateId)
    {
        Boolean bIsExsit = true;
        String strSql = "select * from TB_NEWS_CONTENT WHERE SPLATEID = '" + strPlateId + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        if (ds == null)//ds为空
        {
            bIsExsit = false;
        }
        else
        {
            if (ds.Tables.Count == 0)//ds中没有表
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                {
                    bIsExsit = false;
                }
            }
        }
        return bIsExsit;
    }
    #endregion
}
