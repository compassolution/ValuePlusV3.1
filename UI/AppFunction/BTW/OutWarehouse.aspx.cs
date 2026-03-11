using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_BTW_OutWarehouse : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                //发票号码
                if (Request.Params["BCODE"] != null)
                {
                    this.strBCode = Request.Params["BCODE"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strBCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strBCode);
                    this.txtBCODE.Text = this.strBCode;
                }
                //物料编码
                if (Request.Params["MCODE"] != null)
                {
                    this.strMCode = Request.Params["MCODE"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strMCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strMCode);
                    this.txtMCODE.Text = this.strMCode;
                }
                //this.BindDataGrid(this.strBCode, this.strMCode);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutWarehouse.Page_Load() Error!");
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
    private string strMCode
    {
        get
        {
            return ViewState["strMCode_ViewState"] as string;
        }
        set
        {
            ViewState["strMCode_ViewState"] = value;
        }
    }
    private string strBondedNo
    {
        get
        {
            return ViewState["strBondedNo_ViewState"] as string;
        }
        set
        {
            ViewState["strBondedNo_ViewState"] = value;
        }
    }
    private string strProjectNo
    {
        get
        {
            return ViewState["strProjectNo_ViewState"] as string;
        }
        set
        {
            ViewState["strProjectNo_ViewState"] = value;
        }
    }
    private string strBinNo
    {
        get
        {
            return ViewState["strBinNo_ViewState"] as string;
        }
        set
        {
            ViewState["strBinNo_ViewState"] = value;
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
    /// <param name="strBCode"></param>
    /// <param name="strMCode"></param>
    /// <param name="strProjectNo"></param>
    /// <param name="strQWNO"></param>
    /// <param name="strBinNo"></param>
    private void BindDataGrid(String strBCode, String strMCode, String strQWNO, String strProjectNo, String strBinNo)
    {
        try
        {
            String strSql = "SELECT A.*,B.MNAME,B.MNAMECHS,B.MTYPE FROM MATERIAL_2 A ,MATERIAL_1 B WHERE A.MCODE = B.MCODE AND (ISNULL(A.QTY,0)-ISNULL(A.PREQTY,0)-ISNULL(A.OUTQTY,0)>0) ";//可出库量小于等于0的不显示
            if (!String.IsNullOrEmpty(strBCode))
            {
                strSql = strSql + " AND A.BCODE = '" + strBCode + "'";
            }
            if (!String.IsNullOrEmpty(strMCode))
            {
                strSql = strSql + " AND A.MCODE = '" + strMCode + "'";
            }
            if (!String.IsNullOrEmpty(strQWNO))
            {
                strSql = strSql + " AND A.QWNO = '" + strQWNO + "'";
            }
            if (!String.IsNullOrEmpty(strProjectNo))
            {
                strSql = strSql + " AND A.PROJECTNO = '" + strProjectNo + "'";
            }
            if (!String.IsNullOrEmpty(strBinNo))
            {
                strSql = strSql + " AND A.WHCODE = '" + strBinNo + "'";
            }
            strSql = strSql + " ORDER BY A.QWNO,A.MCODE,A.BCODE ";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                this.iListCount = ds.Tables[0].Rows.Count;
            }
            this.lbListCount.Text = this.iListCount.ToString();

            this.dsGridList = ds;
            this.DataGrid1.DataSource = ds;
            this.DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutWarehouse.BindDataGrid() Error!");
        }
    }
    #endregion

    #region DataGrid相关事件
    /// <summary>
    /// DataGrid ITEM create事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemCreate(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)//标题行
        {

        }
    }

    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item)||(e.Item.ItemType == ListItemType.AlternatingItem))
        {
            e.Item.ToolTip = e.Item.Cells[4].Text.ToString();
            //动态选择框
            CheckBox cbox = (CheckBox)e.Item.Cells[0].FindControl("cbox");
            cbox.Attributes.Add("onclick", "CheckRow(this.checked, this.id);");
            //动态生成数量文本框
            TextBox txtCount = (TextBox)e.Item.Cells[9].FindControl("txtCount");
            txtCount.Enabled = false;
            float fCurCount = float.Parse(e.Item.Cells[6].Text.ToString()) - float.Parse(e.Item.Cells[7].Text.ToString()) - float.Parse(e.Item.Cells[8].Text.ToString());
            if (fCurCount < 0)
            {
                fCurCount = 0;
            }
            txtCount.Text = fCurCount.ToString();
            txtCount.Attributes.Add("onblur", "VerifyCountTextBox(this.id," + txtCount.Text + ");");
            //动态生成价格文本框
            //TextBox txtPrice = (TextBox)e.Item.Cells[0].FindControl("txtPrice");
            //txtPrice.Enabled = false;
            //txtPrice.Text = e.Item.Cells[7].Text.ToString();
            //txtPrice.Attributes.Add("onblur", "VerifyPriceTextBox(this.id," + txtPrice.Text + ");");
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
    /// 查询操作
    /// </summary>
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        this.strBCode = this.txtBCODE.Text.ToString().Trim();
        this.strMCode = this.txtMCODE.Text.ToString().Trim();
        this.strBondedNo = this.txtBondedNo.Text.ToString().Trim();
        this.strProjectNo = this.txtProject.Text.ToString().Trim();
        this.strBinNo = this.txtBin.Text.ToString().Trim();
        this.BindDataGrid(this.strBCode, this.strMCode, this.strBondedNo, this.strProjectNo, this.strBinNo);
    }
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            String strSql = this.GetUpdateSqlString().ToString();
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iCount > 0)
                {
                    this.AlertMessageBox(this.Page, "Successfully!");
                    this.BindDataGrid(this.strBCode, this.strMCode, this.strBondedNo, this.strProjectNo, this.strBinNo);
                    this.RefreshOpener(true);
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Failed!");
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Failed,No Article can out warehouse!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutWarehouse.btnSave_Click() Error!");
        }
    }

    /// <summary>
    /// 根据界面中复选框获取更新数据库的字符串
    /// </summary>
    /// <returns></returns>
    private StringBuilder GetUpdateSqlString()
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        System.Web.UI.WebControls.TextBox txtCount;
        System.Web.UI.WebControls.TextBox txtPrice;
        StringBuilder strBuilder = new StringBuilder();
        ////生成临时的出库单号
        //DateTime dtNow = DateTime.Now;
        //String strLSCKD = "LOUT" + dtNow.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(":", "").Replace(" ", "");
        //生成正式的出库单号
        String strLSCKD = AutoIncreaseFiledBll.GetServerAutoFiledNo("OUTNO");
        strBuilder.Append("INSERT INTO OUTWH_1(OUTNO,OUTSTATE) VALUES ('" + strLSCKD + "','001')\r\n");

        int iIndex = 1;
        StringBuilder strBuilder2 = new StringBuilder();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            if ((dgItem.ItemType == ListItemType.Item) || (dgItem.ItemType == ListItemType.AlternatingItem))
            {
                chkSelect = (CheckBox)dgItem.Cells[0].FindControl("cbox");
                if (chkSelect.Checked)
                {
                    //物料编码
                    String strMcode = dgItem.Cells[1].Text.ToString().Trim();
                    //查仓号
                    String strQwno = dgItem.Cells[5].Text.ToString().Trim();
                    //本次出仓数量
                    txtCount = (TextBox)dgItem.Cells[8].FindControl("txtCount");
                    String strCount = txtCount.Text.ToString().Trim();
                    //本次出仓价格
                    String strPrice = dgItem.Cells[10].Text.ToString().Trim();
                    if (float.Parse(strCount) > 0)
                    {
                        String strSqlString = "INSERT INTO OUTWH_2(OUTNO,POSCODE,MCODE,QTY,MPRICE,QWNO) VALUES ('" + strLSCKD + "','" + iIndex.ToString() + "','" + strMcode + "','" + strCount + "','" + strPrice + "','" + strQwno + "')";
                        strBuilder2.Append(strSqlString + "\r\n");
                        iIndex++;
                    }
                    continue;
                }
            }
        }
        if (!String.IsNullOrEmpty(strBuilder2.ToString()))
        {
            strBuilder.Append(strBuilder2.ToString() + "\r\n");
            //插入成功后执行存储过程
            strBuilder.Append("EXEC [USP_InitPreOutput] '" + strLSCKD + "' \r\n");
            return strBuilder;
        }
        else
        {
            return strBuilder2;
        }
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
    private void RefreshOpener(bool bIsClose)
    {
        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    if(window.opener!=null){\r\n");
        strB.Append("        if(window.opener.document.getElementById(\"aRefresh\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefresh\").click();\r\n");
        if (bIsClose)
        {
            strB.Append("          window.close();\r\n");
        }
        else
        {
            strB.Append("          document.getElementById(\"btnQuery\").click();\r\n");
        }
        strB.Append("       }\r\n");
        strB.Append("    }\r\n");
        strB.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString());
    }

    #endregion

}
