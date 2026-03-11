using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Data;
using System.Text;

public partial class AppFunction_BTW_AlterBooklet : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.btnSave.Attributes.Add("onclick","return confirm('Are you sure?');");
            this.BuildBookletDDList(this.strBookNo);
        }
    }

    #region viewstate初始化区域
    private string strBookNo
    {
        get
        {
            return ViewState["strBookNo_ViewState"] as string;
        }
        set
        {
            ViewState["strBookNo_ViewState"] = value;
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
    private int iListCount
    {
        get
        {
            if (this.ViewState["iListCount"] == null)
            {
                return 0;
            }
            return (int)this.ViewState["iListCount"];
        }
        set
        {
            this.ViewState["iListCount"] = value;
        }
    }
    private DataSet dsBookletList
    {
        get
        {
            return (DataSet)this.ViewState["dsBookletList"];
        }
        set
        {
            this.ViewState["dsBookletList"] = value;
        }
    }
    #endregion

    #region 加载手册号信息下拉框
    /// <summary>
    /// 加载手册号信息下拉框
    /// </summary>
    private void BuildBookletDDList(String strBookNo)
    {
        try
        {
            if (this.dsBookletList == null) 
            {
                String strSql = "SELECT * FROM BOOKLET_1 ORDER BY BOOKLETNO ";
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
                this.dsBookletList = ds;
            }
            //查询手册号下拉框
            this.ddListQuery.DataSource = this.dsBookletList;
            this.ddListQuery.DataTextField = "BOOKLETNO";
            this.ddListQuery.DataValueField = "BOOKLETNO";
            this.ddListQuery.DataBind();
            if (!String.IsNullOrEmpty(strBookNo))
            {
                this.ddListQuery.SelectedIndex = this.ddListQuery.Items.IndexOf(this.ddListQuery.Items.FindByValue(strBookNo));
            }
            //原手册号下拉框
            this.ddListOld.DataSource = this.dsBookletList;
            this.ddListOld.DataTextField = "BOOKLETNO";
            this.ddListOld.DataValueField = "BOOKLETNO";
            this.ddListOld.DataBind();
            if (!String.IsNullOrEmpty(strBookNo))
            {
                this.ddListOld.SelectedIndex = this.ddListOld.Items.IndexOf(this.ddListOld.Items.FindByValue(strBookNo));
                this.ddListOld.Enabled = false;
            }
            //现手册号下拉框
            this.ddListNew.DataSource = this.dsBookletList;
            this.ddListNew.DataTextField = "BOOKLETNO";
            this.ddListNew.DataValueField = "BOOKLETNO";
            this.ddListNew.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterBooklet.BuildLocationDDList() Error!");
        }
    }
    #endregion

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="strBookNo"></param>
    private void BindDataGrid(String strBookNo)
    {
        try
        {
            String strSql = "SELECT * FROM BATCHORDER_1 WHERE BOOKLETNO = '" + strBookNo + "' ORDER BY BCODE";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            this.dsGridList = ds;
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                this.iListCount = ds.Tables[0].Rows.Count;
            }
            this.lbListCount.Text = this.iListCount.ToString();
            this.DataGrid1.DataSource = ds;
            this.DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterBooklet.BindDataGrid() Error!");
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

    #region 按钮操作
    /// <summary>
    /// 查询定位操作
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        try
        {
            this.strBookNo = this.ddListQuery.SelectedValue.ToString().Trim();
            this.BindDataGrid(this.strBookNo);
            this.BuildBookletDDList(this.strBookNo);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterBooklet.btnQuery_Click() Error!");
        }
    }

    /// <summary>
    /// 变更保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            String strBookNoNew = this.ddListNew.SelectedValue.ToString();
            if (strBookNoNew.Equals(this.strBookNo))
            {
                this.AlertMessageBox(this.Page,"The Same Booklet NO.");
                return;
            }

            //变更序号
            DateTime dtNow = DateTime.Now;
            String strAlterCode = "AB" + dtNow.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(":", "").Replace(" ", "");

            String strInserSqlString = this.GetInsertSqlString(strAlterCode, strBookNoNew);;
            if (!String.IsNullOrEmpty(strInserSqlString))
            {
                String strSql = "INSERT INTO ALTERBOOKLET_1 (ALTERCODE,OLDBOOKNO,NEWBOOKNO,ALTERUSER,ALTERDATE) VALUES ('" + strAlterCode + "','" + this.strBookNo + "','" + strBookNoNew + "','" + this.GetUserCode() + "','" + dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "')\r\n";

                strSql = strSql + strInserSqlString;
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iCount > 0)
                {
                    this.AlertMessageBox(this.Page, "Successfully!");
                    this.BindDataGrid(this.strBookNo);
                    this.RefreshOpener();
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Please select record!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterBooklet.btnSave_Click() Error!");
        }
    }


    /// <summary>
    /// 根据界面中复选框获取插入SQL语句的字符串
    /// </summary>
    /// <returns></returns>
    private String GetInsertSqlString(String strAlterCode,String strNewBookNo)
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        StringBuilder sb = new StringBuilder();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            chkSelect = (CheckBox)dgItem.FindControl("cbox");
            if (chkSelect.Checked)
            {
                String strSelectValue = dgItem.Cells[1].Text;
                String strSql = "INSERT INTO ALTERBOOKLET_2([ALTERCODE] ,[BCODE] ,[FDATE] ,[AMOUNT] ,[WEIGHT] ,[BOOKLETNO] ,[DELIEVERY]) SELECT '" + strAlterCode + "',[BCODE] ,[FDATE] ,[AMOUNT] ,[WEIGHT] ,[BOOKLETNO] ,[DELIEVERY] FROM BATCHORDER_1 WHERE [BCODE] = '" + strSelectValue + "'\r\n";

                sb.Append(strSql);
                sb.Append("UPDATE BATCHORDER_1 SET BOOKLETNO = '"+strNewBookNo+"' where [BCODE]= '"+strSelectValue+"' \r\n");
                continue;
            }
        }
        return sb.ToString();
    }


    /// <summary>
    /// 关闭操作
    /// </summary>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.RefreshOpener();
    }

    /// <summary>
    /// 刷新父页面
    /// </summary>
    private void RefreshOpener()
    {
        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    if(window.opener!=null){\r\n");
        strB.Append("        if(window.opener.document.getElementById(\"aRefresh\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefresh\").click();\r\n");
        strB.Append("            window.close();\r\n");
        strB.Append("        }\r\n");
        strB.Append("    }\r\n");
        strB.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString());
    }
    #endregion
}
