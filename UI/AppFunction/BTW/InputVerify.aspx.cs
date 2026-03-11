using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Data;
using System.Text;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_BTW_InputVerify : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //发票号码
            if (Request.Params["P0"] != null)
            {
                this.strBCode = Request.Params["P0"].ToString();
                this.ddListAll.Attributes.Add("onchange", "selectSameWhCode(this.id);");
                //审核类别(1/2)
                if (Request.Params["P1"] != null)
                {
                    this.strVerifyType = Request.Params["P1"].ToString();
                    if (this.strVerifyType.Equals("1"))
                    {
                        this.divForm1.Visible = true;
                        this.divForm2.Visible = false;
                        //this.btnSave1.Attributes.Add("onclick", "javascript:checkSave1();return false;");
                    }
                    else if (this.strVerifyType.Equals("2"))
                    {
                        this.divForm2.Visible = true;
                        this.divForm1.Visible = false;
                        //this.btnSave2.Attributes.Add("onclick", "javascript:checkSave2();return false;");
                        //加载库位信息下拉框
                        this.BuildLocationDDList();
                    }
                }
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strBCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strBCode);
                this.strVerifyType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strVerifyType);

                this.BindDataGrid(strBCode);
            }
        }
    }

    #region viewstate初始化区域
    private string strBCode
    {
        get
        {
            return ViewState["strBCode_ViewState"] as string;
        }
        set
        {
            ViewState["strBCode_ViewState"] = value;
        }
    }
    private string strVerifyType
    {
        get
        {
            return ViewState["strVerifyType_ViewState"] as string;
        }
        set
        {
            ViewState["strVerifyType_ViewState"] = value;
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
    private DataSet dsGridWhCode
    {
        get
        {
            if (this.ViewState["dsGridWhCode"] == null)
            {
                return new DataSet();
            }
            return (DataSet)this.ViewState["dsGridWhCode"];
        }
        set
        {
            this.ViewState["dsGridWhCode"] = value;
        }
    }
    #endregion

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="strBCode"></param>
    private void BindDataGrid(String strBCode)
    {
        try
        {
            String strSql = "SELECT * FROM BATCHORDER_2 WHERE BCODE = '" + strBCode + "' ORDER BY POSCODE";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            this.dsGridList = ds;
            this.DataGrid1.DataSource = ds;
            this.DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_InputVerify.BindDataGrid() Error!");
        }
    }
    #endregion

    #region 加载库位信息下拉框
    /// <summary>
    /// 加载库位信息下拉框
    /// </summary>
    private void BuildLocationDDList()
    {
        try
        {
            String strSql = "SELECT * FROM WHPOS_1 ORDER BY WHCODE ";
            this.dsGridWhCode = SqlParamDao.GetDataSetBySql(strSql);
            this.ddListAll.DataSource = this.dsGridWhCode;
            this.ddListAll.DataTextField = "WHCODE";
            this.ddListAll.DataValueField = "WHCODE";
            this.ddListAll.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_InputVerify.BuildLocationDDList() Error!");
        }
    }
    #endregion

    #region DataGrid 数据绑定事件
    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            e.Item.ToolTip = e.Item.Cells[7].Text.ToString();
            if (this.strVerifyType.Equals("1"))
            {
                this.DataGrid1.Columns[3].Visible = true;
                this.DataGrid1.Columns[4].Visible = false;
                this.DataGrid1.Columns[15].Visible = false;
                this.DataGrid1.Columns[16].Visible = true;
                TextBox txtBwNo = (TextBox)e.Item.Cells[3].FindControl("txtBwNo");
                txtBwNo.Text = e.Item.Cells[4].Text.ToString().Replace("&nbsp;", "");
                txtBwNo.Attributes.Add("onblur", "VerifyCountTextBox(this.id);");
            }
            else if (this.strVerifyType.Equals("2"))
            {
                this.DataGrid1.Columns[3].Visible = false;
                this.DataGrid1.Columns[4].Visible = true; ;
                this.DataGrid1.Columns[15].Visible = true;
                this.DataGrid1.Columns[16].Visible = false;

                DropDownList ddList_One = (DropDownList)e.Item.Cells[15].FindControl("ddList_One");
                ddList_One.DataSource = this.dsGridWhCode;
                ddList_One.DataTextField = "WHCODE";
                ddList_One.DataValueField = "WHCODE";
                ddList_One.DataBind();
                String strWHNo = e.Item.Cells[16].Text.ToString();
                if (!String.IsNullOrEmpty(strWHNo))
                {
                    ddList_One.SelectedIndex = ddList_One.Items.IndexOf(ddList_One.Items.FindByValue(strWHNo));
                }
            }
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

                String strUpdateSql = this.GetUpdateSqlString().ToString();
                if (!String.IsNullOrEmpty(strUpdateSql))
                {
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strUpdateSql);
                    //保存审核人信息
                    this.SaveVerifierInfo("1");
                    if (iCount > 0)
                    {
                        this.AlertMessageBox(this.Page, "Successfully!");
                        this.BindDataGrid(this.strBCode);
                        this.RefreshOpener();
                    }
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Please select record!");
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Please input Customs No. and Customs Date!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_InputVerify.btnSave1_Click() Error!");
        }
    }


    /// <summary>
    /// 二审保存操作
    /// </summary>
    protected void btnSave2_Click(object sender, EventArgs e)
    {
        try
        {
            //String strWHCode = this.ddListAll.SelectedValue.ToString();
            String strRkDate = this.txtRKSJ.Text.ToString();

            if (!String.IsNullOrEmpty(strRkDate))
            {
                String strPosString = this.GetUpdatePosString(strRkDate).ToString();
                if (!String.IsNullOrEmpty(strPosString))
                {
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strPosString);
                    //保存审核人信息
                    this.SaveVerifierInfo("2");

                    if (iCount > 0)
                    {
                        this.AlertMessageBox(this.Page, "Successfully!");
                        this.BindDataGrid(this.strBCode);
                        this.RefreshOpener();
                    }
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Please select record!");
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Please input Storage Date!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_InputVerify.btnSave1_Click() Error!");
        }
    }

    /// <summary>
    /// 根据界面中复选框获取序号的字符串
    /// </summary>
    /// <returns></returns>
    private StringBuilder GetUpdatePosString(String strRkDate)
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        int iIndex = 0;
        StringBuilder strBuilder2 = new StringBuilder();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            chkSelect = (CheckBox)dgItem.FindControl("cbox");
            if (chkSelect.Checked)
            {
                String strSelectValue = dgItem.Cells[2].Text;
                //if (iIndex == 0)
                //{
                //    strPosString = strPosString + "'" + strSelectValue + "'";
                //}
                //else
                //{
                //    strPosString = strPosString + ",'" + strSelectValue + "'";
                //}
                //iIndex++;
                //continue;

                String strPos = dgItem.Cells[2].Text.ToString().Trim();
                DropDownList ddList_One = (DropDownList)dgItem.Cells[15].FindControl("ddList_One");
                String strWhCode = ddList_One.SelectedValue.ToString().Trim();
                String strSqlString = "UPDATE BATCHORDER_2 SET WHCODE = '" + strWhCode + "', RKDATE = '" + strRkDate + "' WHERE BCODE = '" + this.strBCode + "' AND POSCODE = '" + strPos + "'";

                strBuilder2.Append(strSqlString + ";\r\n");
                iIndex++;
                continue;
            }
        }
        return strBuilder2;
    }

    /// <summary>
    /// 根据界面中复选框获取更新数据库的字符串
    /// </summary>
    /// <returns></returns>
    private StringBuilder GetUpdateSqlString()
    {
        String strCustomNo = this.txtBGDH.Text.ToString();
        String strCustomDate = this.txtBGSJ.Text.ToString();

        System.Web.UI.WebControls.CheckBox chkSelect;
        System.Web.UI.WebControls.TextBox txtBwNo;

        int iIndex = 1;
        StringBuilder strBuilder2 = new StringBuilder();
        Hashtable hsBwNo = new Hashtable();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            if ((dgItem.ItemType == ListItemType.Item) || (dgItem.ItemType == ListItemType.AlternatingItem))
            {
                chkSelect = (CheckBox)dgItem.Cells[0].FindControl("cbox");
                if (chkSelect.Checked)
                {
                    //Pos
                    String strPos = dgItem.Cells[2].Text.ToString().Trim();
                    //账册序号
                    txtBwNo = (TextBox)dgItem.Cells[3].FindControl("txtBwNo");
                    String strBwNo = txtBwNo.Text.ToString().Trim();
                    //if (hsBwNo.ContainsKey(strBwNo))
                    //{
                    //    AlertMessageBox(this.Page,"There is same BW No.");
                    //    return null;
                    //}
                    //else
                    //{
                        //hsBwNo.Add(strBwNo, strBwNo);
                    //}
                    if (float.Parse(strBwNo) > 0)
                    {
                        String strSqlString = "UPDATE BATCHORDER_2 SET CUSNUMBER = '" + strCustomNo + "', CUSDATE = '" + strCustomDate + "', BWNO = '" + strBwNo + "' where BCODE = '" + this.strBCode + "' AND POSCODE = '" + strPos + "'";
                        strBuilder2.Append(strSqlString + "\r\n");
                        iIndex++;
                    }
                    continue;
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
        this.RefreshOpener();
    }

    /// <summary>
    /// 保存审核人信息
    /// </summary>
    private void SaveVerifierInfo(String strVerifyType)
    {
        if (!String.IsNullOrEmpty(strVerifyType))
        {
            String strTableName = "BATCHORDER_3";
            if (strVerifyType.Equals("1"))
            {
                strTableName = "BATCHORDER_3";
            }
            else if (strVerifyType.Equals("2"))
            {
                strTableName = "BATCHORDER_4";
            }
            DateTime dtNow = DateTime.Now;
            String strSql = "select * from " + strTableName+" where BCODE = '"+this.strBCode+"'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                strSql = "update " + strTableName + " set [USER] ='" + this.GetUserCode() + "',[VDATE] = '" + dtNow + "' where [BCODE] = '" + this.strBCode + "'";
            }
            else
            {
                strSql = "insert into "+ strTableName + "([BCODE],[USER],[VDATE]) VALUES ('"+this.strBCode+"','"+this.GetUserCode()+"','"+dtNow+"')";
            }
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
        }
    }

    /// <summary>
    /// 刷新父页面
    /// </summary>
    private void RefreshOpener() 
    {
        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    if(window.opener!=null){\r\n");
        strB.Append("        if(window.opener.document.getElementById(\"aRefreshDetail\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefreshDetail\").click();\r\n");
        strB.Append("            window.close();\r\n");
        strB.Append("        }\r\n");
        strB.Append("    }\r\n");
        strB.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString());
    }
    #endregion
}
