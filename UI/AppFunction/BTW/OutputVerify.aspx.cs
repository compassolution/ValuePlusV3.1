using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Data;
using System.Text;
using System.Collections;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_BTW_OutputVerify : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //出库单号
            if (Request.Params["P0"] != null)
            {
                this.strOutCode = Request.Params["P0"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strOutCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOutCode);
                this.BindDataGrid(strOutCode);
            }
        }
    }

    #region viewstate初始化区域
    private string strOutCode
    {
        get
        {
            return ViewState["strOutCode_ViewState"] as string;
        }
        set
        {
            ViewState["strOutCode_ViewState"] = value;
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
    #endregion

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="strOutCode"></param>
    private void BindDataGrid(String strOutCode)
    {
        try
        {
            String strSql = "SELECT * FROM OUTWH_2 WHERE OUTNO = '" + strOutCode + "' ORDER BY POSCODE";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            this.dsGridList = ds;
            this.DataGrid1.DataSource = ds;
            this.DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutputVerify.BindDataGrid() Error!");
        }
    }
    #endregion

    #region DataGrid相关事件
    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            ////动态选择框
            //CheckBox cbox = (CheckBox)e.Item.Cells[0].FindControl("cbox");
            //cbox.Attributes.Add("onclick", "CheckRow(this.checked, this.id);");
            String strQty = e.Item.Cells[7].Text.ToString();
            String strMType = e.Item.Cells[6].Text.ToString();
            e.Item.ToolTip = strMType;
            //动态生成价格文本框
            TextBox txtPrice = (TextBox)e.Item.Cells[11].FindControl("txtPrice");
            txtPrice.Text = e.Item.Cells[14].Text.ToString();

            txtPrice.Attributes.Add("onkeyup", "VerifyPriceTextBox(this.id," + txtPrice.Text + "," + strQty + ");");
            //动态生成总金额文本框
            TextBox txtAmount = (TextBox)e.Item.Cells[12].FindControl("txtAmount");
            //txtAmount.Enabled = false;
            txtAmount.Text = e.Item.Cells[15].Text.ToString();
            //动态生成币制文本框
            TextBox txtCurrency = (TextBox)e.Item.Cells[13].FindControl("txtCurrency");
            txtCurrency.Text = e.Item.Cells[16].Text.ToString();

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
    /// 初审保存操作
    /// </summary>
    protected void btnSave1_Click(object sender, EventArgs e)
    {
        try
        {
            String strCustomNo = this.txtBGDH.Text.ToString();
            String strCustomDate = this.txtBGSJ.Text.ToString();
            if ((!String.IsNullOrEmpty(strCustomNo)) && (!String.IsNullOrEmpty(strCustomDate)))
            {

                String strSqlString = this.GetUpdateSqlString().ToString();
                if (!String.IsNullOrEmpty(strSqlString))
                {
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSqlString);
                    if (iCount > 0)
                    {
                        //数据库存储过程作相应后续处理
                        //this.ExecImportSp();//暂时屏蔽

                        this.AlertMessageBox(this.Page, "Successfully!");
                        this.BindDataGrid(this.strOutCode);
                        this.RefreshOpener(true);
                        this.Page.Focus();
                    }
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Please select record!");
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Please input Customs Number and Customs Date!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutputVerify.btnSave1_Click() Error!");
        }
    }


    /// <summary>
    /// 数据库存储过程作相应后续处理
    /// </summary>
    private void ExecImportSp()
    {
        Hashtable hsTable = new Hashtable();
        hsTable.Add("P0", this.strOutCode);
        SqlParamDao.ExcuteSPReturnStr("USP_AfterOutVerify", hsTable);
    }

    /// <summary>
    /// 根据界面中复选框获取序号的字符串
    /// </summary>
    /// <returns></returns>
    private String GetUpdatePosString()
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        String strPosString = "";
        int iIndex = 0;
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            chkSelect = (CheckBox)dgItem.FindControl("cbox");
            if (chkSelect.Checked)
            {
                String strSelectValue = dgItem.Cells[2].Text;
                if (iIndex == 0)
                {
                    strPosString = strPosString + "'" + strSelectValue + "'";
                }
                else
                {
                    strPosString = strPosString + ",'" + strSelectValue + "'";
                }
                iIndex++;
                continue;
            }
        }
        if (!String.IsNullOrEmpty(strPosString))
        {
            strPosString = "(" + strPosString + ")";
        }
        return strPosString;
    }


    /// <summary>
    /// 根据界面中复选框获取更新数据库的字符串
    /// </summary>
    /// <returns></returns>
    private StringBuilder GetUpdateSqlString()
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        System.Web.UI.WebControls.TextBox txtPrice;
        System.Web.UI.WebControls.TextBox txtAmount;
        System.Web.UI.WebControls.TextBox txtCurrency;
        StringBuilder strBuilder = new StringBuilder();
        String strCustomNo = this.txtBGDH.Text.ToString();
        String strCustomDate = this.txtBGSJ.Text.ToString();

        int iIndex = 1;
        StringBuilder strBuilder2 = new StringBuilder();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            if ((dgItem.ItemType == ListItemType.Item) || (dgItem.ItemType == ListItemType.AlternatingItem))
            {
                chkSelect = (CheckBox)dgItem.Cells[0].FindControl("cbox");
                if (chkSelect.Checked)
                {
                    //出库单号
                    String strOutCode = dgItem.Cells[1].Text.ToString().Trim();
                    //序号
                    String strPosCode = dgItem.Cells[2].Text.ToString().Trim();
                    //本次出仓价格
                    txtPrice = (TextBox)dgItem.Cells[11].FindControl("txtPrice");
                    if (String.IsNullOrEmpty(txtPrice.Text))
                    {
                        return null;
                    }
                    float fPrice = float.Parse(txtPrice.Text.ToString().Trim());
                    //本次出仓总金额
                    txtAmount = (TextBox)dgItem.Cells[12].FindControl("txtAmount");
                    if (String.IsNullOrEmpty(txtAmount.Text))
                    {
                        return null;
                    }
                    float fAmount = float.Parse(txtAmount.Text.ToString().Trim());
                    //本次出仓币制
                    txtCurrency = (TextBox)dgItem.Cells[13].FindControl("txtCurrency");
                    if (String.IsNullOrEmpty(txtCurrency.Text))
                    {
                        return null;
                    }

                    String strSqlString = "UPDATE OUTWH_2 SET OUTNUMBER = '" + strCustomNo + "', OUTDATE = '" + strCustomDate + "',MPRICE = " + fPrice + ",MAMOUNT = " + fAmount + ",CURRENCY = '" + txtCurrency.Text + "' WHERE OUTNO = '" + this.strOutCode + "' and POSCODE =" + strPosCode;
                    strBuilder2.Append(strSqlString + "\r\n");
                    iIndex++;
                }
            }
        }
        return strBuilder2;
    }


    /// <summary>
    /// 关闭操作
    /// </summary>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.RefreshOpener(true);
    }

    /// <summary>
    /// 刷新父页面
    /// </summary>
    private void RefreshOpener(bool isClose)
    {
        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    if(window.opener!=null){\r\n");
        strB.Append("        if(window.opener.document.getElementById(\"aRefreshDetail\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefreshDetail\").click();\r\n");
        if (isClose)
        {
            strB.Append("            window.close();\r\n");
        }
        strB.Append("        }\r\n");
        strB.Append("    }\r\n");
        strB.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString());
    }
    #endregion
}
