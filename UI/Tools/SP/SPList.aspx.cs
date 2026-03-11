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
using Com.ValuePlus.DAL;
using System.Text;

public partial class Tools_SP_SPList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.txt_Search.Attributes.Add("onkeypress", "EnterSearchTextBox()");

            try
            {
                this.dtList = this.GetSpListData();
                this.BindSpListInfo(this.dtList, "");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }

    #region viewstate初始化区域
    private String strObjectType
    {
        get
        {
            return ViewState["SPContent_strObjectType_ViewState"] as String;
        }
        set
        {
            ViewState["SPContent_strObjectType_ViewState"] = value;
        }
    }
    private DataTable dtList
    {
        get
        {
            if (this.ViewState["dtList"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtList"];
        }
        set
        {
            this.ViewState["dtList"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 获取存储过程数据集
    /// </summary>
    /// <returns></returns>
    private DataTable GetSpListData()
    {
        DataTable dt = new DataTable();
        try
        {
            this.strObjectType = this.rdBtnListObject.SelectedValue;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from sysobjects where [CATEGORY] = '0' ");
            if(this.strObjectType.ToLower().Equals("sp"))
            {
                sbSql.Append(" and [XTYPE] in ('P')");
            }else if (this.strObjectType.ToLower().Equals("fun"))
            {
                sbSql.Append(" and [XTYPE] in ('FN','TF')");
            }
            else if (this.strObjectType.ToLower().Equals("view"))
            {
                sbSql.Append(" and [XTYPE] in ('V')");
            }
            sbSql.Append(" order by [XTYPE] ,[NAME]");
            String strCommentSql = sbSql.ToString();
            dt = SqlParamDao.GetDataTableBySql(strCommentSql);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="strFilter"></param>
    private void BindSpListInfo(DataTable dt ,String strFilter)
    {
        this.listBoxObjectList.Items.Clear();

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            String strFilterSql = "0=0";
            if (!String.IsNullOrEmpty(strFilter))
            {
                strFilterSql = strFilterSql + " AND " + strFilter;
            }
            DataRow[] drs = dt.Select(strFilterSql);
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strId = dr["ID"].ToString();
                    String strName = dr["NAME"].ToString();
                    lstItem.Value = strId;
                    lstItem.Text = strName;
                    this.listBoxObjectList.Items.Add(lstItem);
                }
            }
        }
    }
    #endregion

    #region 过滤操作
    /// <summary>
    /// 过滤操作
    /// </summary>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        String strFilter = this.txt_Search.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            strFilterSql = "(NAME LIKE '%" + strFilter + "%')";
        }
        this.BindSpListInfo(this.dtList, strFilterSql);

    }
    #endregion

    #region 重新加载操作
    /// <summary>
    /// 重新加载操作
    /// </summary>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.dtList = this.GetSpListData();
    }
    #endregion

    #region ListBox框变更事件
    protected void listBoxObjectList_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String strSpId = this.listBoxObjectList.SelectedValue;
        String strSpName = this.listBoxObjectList.SelectedItem.Text;

        this.FrmDetail.Attributes["src"] = "SPContent.aspx?type=" + this.strObjectType +"&name=" + strSpName;


    }
    #endregion


    protected void rdBtnListObject_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            this.dtList = this.GetSpListData();
            this.BindSpListInfo(this.dtList, "");
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
}
