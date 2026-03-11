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
using System.Threading;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.Query;
using Com.ValuePlus.Common;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;

public partial class Query_QuerySelect : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.strQuerySql = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "sql");
                this.strKeyField = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "key");
                this.strOpElement = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "element");
                this.hdFieldElement.Value = this.strOpElement;

                this.txtCondition.Attributes.Add("onkeypress", "EnterFilterTextBox()");

                if (!String.IsNullOrEmpty(this.strQuerySql))
                {
                    //更新SQL查询语句中的用户参数
                    if ((this.strQuerySql.IndexOf("@") > 1)|| (this.strQuerySql.IndexOf("?") > 1))
                    {
                        GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                        Hashtable hsTableParam = bllGetArchiveSetting.GetUserParamValueByUserId(this.GetUserCode(), this.IsAdminstrator());
                        this.strQuerySql = this.strQuerySql.Replace("?", "'"+this.GetUserCode() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@USERCODE@", "'" + this.GetUserCode() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P0@", "'" + hsTableParam["P0"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P1@", "'" + hsTableParam["P1"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P2@", "'" + hsTableParam["P2"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P3@", "'" + hsTableParam["P3"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P4@", "'" + hsTableParam["P4"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P5@", "'" + hsTableParam["P5"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P6@", "'" + hsTableParam["P6"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P7@", "'" + hsTableParam["P7"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P8@", "'" + hsTableParam["P8"].ToString() + "'");
                        this.strQuerySql = this.strQuerySql.Replace("@P9@", "'" + hsTableParam["P9"].ToString() + "'");

                        log.Error("Query_QuerySelect页面查询语句：" + this.strQuerySql);
                    }

                    //绑定DataGrid数据
                    this.BindDataGrid(true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, "Load Error！");
            }
        }
    }

    #region viewstate初始化区域
    private string strKeyField
    {
        get
        {
            return ViewState["QuerySelect_strKeyField_ViewState"] as string;
        }
        set
        {
            ViewState["QuerySelect_strKeyField_ViewState"] = value;
        }
    }
    private string strOpElement
    {
        get
        {
            return ViewState["QuerySelect_strOpElement_ViewState"] as string;
        }
        set
        {
            ViewState["QuerySelect_strOpElement_ViewState"] = value;
        }
    }
    private string strQuerySql
    {
        get
        {
            return ViewState["QuerySelect_strSql_ViewState"] as string;
        }
        set
        {
            ViewState["QuerySelect_strSql_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        QuerySelectBll bllQuery = new QuerySelectBll();
        if (bFresh)
        {
            ViewState["DsAllDataViewState"] = SqlParamDao.GetDataSetBySql(this.strQuerySql);
            
        }
        else
        {
            if (ViewState["DsAllDataViewState"] == null)
            {
                ViewState["DsAllDataViewState"] = SqlParamDao.GetDataSetBySql(this.strQuerySql);
            }
        }
        this.DataGrid1.DataSource = ViewState["DsAllDataViewState"];
        this.DataGrid1.DataKeyField = this.strKeyField;
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
        if (ViewState["DsAllDataViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["DsAllDataViewState"];
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
            //DataSet dsTemp = new DataSet();
            //System.Data.DataTable dt = defaultView.ToTable();
            //dsTemp.Tables.Add(dt.Copy());
            //this.dsGridList = dsTemp;
            //this.dsGridList = this.ReplaceDataSetColumnLanguage((DataSet)this.dsGridList);

            this.DataGrid1.DataSource = defaultView;
            this.DataGrid1.DataKeyField = this.strKeyField;
            this.DataGrid1.DataBind();
        }
    }
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

    #region 每行数据加载事件
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        int iIndex = e.Item.ItemIndex;
        int i2= this.DataGrid1.DataKeys.Count;
        String strKeyValue = "";
        if (iIndex >= 0)
        {
            strKeyValue = (String)this.DataGrid1.DataKeys[iIndex];
        }
        e.Item.Attributes.Add("ondblclick", "selectRow('" + strKeyValue + "');"); 
        if(e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item ) 
        {
            e.Item.Attributes.Add("onmouseover", "this.oldcolor=this.style.backgroundColor;this.style.backgroundColor='#FFFF00';this.style.cursor='hand'"); 
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.oldcolor"); 
        }

    }
    #endregion

    #region 根据关键字过滤
    protected void ImageBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        String strKeyValue = this.txtCondition.Text;
        QuerySelectBll bllQuery = new QuerySelectBll();
        DataTable dt = bllQuery.GetDataInfoBySql(this.strQuerySql);
        if (!strKeyValue.Equals(""))
        {
            String strFilterSql = "";
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                String strCaption = dt.Columns[i].Caption;
                //strCaption = "convert(varchar(100)," + strCaption + ")";
                if (dt.Columns[i].DataType.FullName.ToLower().IndexOf("decimal") < 0)
                {
                    if (i == 0)
                    {
                        strFilterSql = strFilterSql + "([" + strCaption + "] like '%" + this.txtCondition.Text + "%')";
                    }
                    else
                    {
                        strFilterSql = strFilterSql + " OR ([" + strCaption + "] like '%" + this.txtCondition.Text + "%')";
                    }
                }
            }
            dt.DefaultView.RowFilter = String.Format(strFilterSql);
            this.DataGrid1.DataSource = dt.DefaultView;

        }
        else
        {
            this.DataGrid1.DataSource = dt;
        }
        this.DataGrid1.DataKeyField = this.strKeyField;
        this.DataGrid1.DataBind();
    }
    #endregion


}
