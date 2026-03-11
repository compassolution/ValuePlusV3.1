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

public partial class Vacation_VacationSpList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.TextBox1.Attributes.Add("onkeypress", "EnterSearchTextBox()");

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
            String strCommentSql = "select * from VACATIONPARAM_1  where [paramName] like 'USP_%' and isStop = '2' order by [paramName]";
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
    private void BindSpListInfo(DataTable dt, String strFilter)
    {
        this.ListBox1.Items.Clear();

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
                    String strId = dr["paramName"].ToString();
                    String strName = dr["paramValue"].ToString();
                    lstItem.Value = strId;
                    lstItem.Text = strName;
                    this.ListBox1.Items.Add(lstItem);
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
        String strFilter = this.TextBox1.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            strFilterSql = "(paramName LIKE '%" + strFilter + "%') or (paramValue LIKE '%" + strFilter + "%')";
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
    protected void ListBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String strSpId = this.ListBox1.SelectedValue;
        String strSpName = this.ListBox1.SelectedItem.Text;

        this.FrmDetail.Attributes["src"] = "../Tools/SP/SPContent.aspx?SP=" + strSpName;


    }
    #endregion

}
