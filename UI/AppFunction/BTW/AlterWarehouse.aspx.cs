using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Data;
using System.Text;


public partial class AppFunction_BTW_AlterWarehouse : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.btnSave.Attributes.Add("onclick", "return confirm('Are you sure?');");
            this.BuildWarehouseDDList(this.strBinNo);
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
    private DataSet dsWareHouseList
    {
        get
        {
            return (DataSet)this.ViewState["dsWareHouseList"];
        }
        set
        {
            this.ViewState["dsWareHouseList"] = value;
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
    private string strOldWHString
    {
        get
        {
            return ViewState["strOldWHString_ViewState"] as string;
        }
        set
        {
            ViewState["strOldWHString_ViewState"] = value;
        }
    }
    #endregion

    #region 加载库位信息下拉框
    /// <summary>
    /// 加载库位信息下拉框
    /// </summary>
    private void BuildWarehouseDDList(String strWHNo)
    {
        try
        {
            if (this.dsWareHouseList == null)
            {
                String strSql = "SELECT * FROM WHPOS_1 ORDER BY WHCODE ";
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
                this.dsWareHouseList = ds;
            }
            //查询库位号下拉框
            this.ddListQuery.Controls.Clear();
            this.ddListQuery.Items.Clear();
            this.ddListQuery.Items.Add(new ListItem("", ""));
            if ((this.dsWareHouseList != null) && (this.dsWareHouseList.Tables.Count > 0))
            {
                DataTable dt = this.dsWareHouseList.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String strItemValue = dt.Rows[i]["WHCODE"].ToString();
                    this.ddListQuery.Items.Add(new ListItem(strItemValue, strItemValue));
                }
                this.ddListQuery.ClearSelection();
                this.ddListQuery.SelectedIndex = 0;
                if (!String.IsNullOrEmpty(strWHNo))
                {
                    this.ddListQuery.SelectedIndex = this.ddListQuery.Items.IndexOf(this.ddListQuery.Items.FindByValue(strWHNo));
                }
            }

            //现库位号下拉框
            this.ddListNew.DataSource = this.dsWareHouseList;
            this.ddListNew.DataTextField = "WHCODE";
            this.ddListNew.DataValueField = "WHCODE";
            this.ddListNew.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterWarehouse.BuildLocationDDList() Error!");
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
            String strSql = "SELECT A.* FROM MATERIAL_2 A WHERE 1=1 ";
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
            strSql = strSql + " ORDER BY A.WHCODE,A.BCODE,A.MCODE,A.QWNO ";
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
            log.Error("AppFunction_BTW_AlterWarehouse.BindDataGrid() Error!");
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
            this.strBCode = this.txtBCODE.Text.ToString().Trim();
            this.strMCode = this.txtMCODE.Text.ToString().Trim();
            this.strBondedNo = this.txtBondedNo.Text.ToString().Trim();
            this.strProjectNo = this.txtProject.Text.ToString().Trim();
            this.strBinNo = this.ddListQuery.SelectedValue.ToString();
            this.BindDataGrid(this.strBCode, this.strMCode, this.strBondedNo, this.strProjectNo, this.strBinNo);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_AlterWarehouse.btnQuery_Click() Error!");
        }
    }

    /// <summary>
    /// 变更保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            String strWHCodeNew = this.ddListNew.SelectedValue.ToString();

            //变更序号
            DateTime dtNow = DateTime.Now;
            String strAlterCode = "AW" + dtNow.ToString("yyyy-MM-dd HH:mm:ss").Replace("-", "").Replace(":", "").Replace(" ", "");

            String strInserSqlString = this.GetInsertSqlString(strAlterCode, strWHCodeNew); ;
            if (!String.IsNullOrEmpty(strInserSqlString))
            {
                String strSql = "INSERT INTO ALTERWH_1 (ALTERCODE,OLDWH,NEWWH,ALTERUSER,ALTERDATE) VALUES ('" + strAlterCode + "','"+this.strOldWHString+"','" + strWHCodeNew + "','" + this.GetUserCode() + "','" + dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "')\r\n";

                strSql = strSql + strInserSqlString;
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iCount > 0)
                {
                    this.AlertMessageBox(this.Page, "Successfully!");
                    this.BindDataGrid(this.strBCode, this.strMCode, this.strBondedNo, this.strProjectNo, this.strBinNo);
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
            log.Error("AppFunction_BTW_AlterWarehouse.btnSave_Click() Error!");
        }
    }


    /// <summary>
    /// 根据界面中复选框获取插入SQL语句的字符串
    /// </summary>
    /// <returns></returns>
    private String GetInsertSqlString(String strAlterCode, String strNewWHNo)
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        StringBuilder sb = new StringBuilder();
        int iIndex = 1;
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            chkSelect = (CheckBox)dgItem.FindControl("cbox");
            if (chkSelect.Checked)
            {
                String strSelectQwNo = dgItem.Cells[1].Text;//查仓号
                String strBookLetNo = strSelectQwNo.Split('/')[0].ToString();//手册号
                String strOldWh = dgItem.Cells[2].Text;//库位
                //拼写“从库位”的字符串
                if (String.IsNullOrEmpty(this.strOldWHString))
                {
                    this.strOldWHString = strOldWh;
                }
                else
                {
                    if (this.strOldWHString.IndexOf(strOldWh)<0)
                    {
                        this.strOldWHString = this.strOldWHString + "," + strOldWh;
                    }
                }

                sb.Append("INSERT INTO ALTERWH_2([ALTERCODE],[POSCODE],[MCODE],[BOOKLETNO],[QWNO],[BCODE],[QTY],[PREQTY],[OUTQTY],[MPRICE],[MAMOUNT],[CURRENCY],[ALLWT],[PROJECTNO],[PONO],[CUSDATE],[CUSNUMBER],[RKDATE],[WHCODE]) ");
                sb.Append("SELECT '" + strAlterCode + "','" + iIndex.ToString() + "',[MCODE],'" + strBookLetNo + "',[QWNO],[BCODE],[QTY],[PREQTY],[OUTQTY],[MPRICE],[MAMOUNT],[CURRENCY],[ALLWT],[PROJECTNO],[PONO],[CUSDATE],[CUSNUMBER],[RKDATE],[WHCODE] FROM MATERIAL_2 WHERE [QWNO] = '" + strSelectQwNo + "'\r\n");

                sb.Append("UPDATE BATCHORDER_2 SET WHCODE = '" + strNewWHNo + "' where [QWNO]= '" + strSelectQwNo + "' \r\n");
                sb.Append("UPDATE MATERIAL_2 SET WHCODE = '" + strNewWHNo + "' where [QWNO]= '" + strSelectQwNo + "' \r\n");
                sb.Append("UPDATE BONDEDNUMBER_1 SET WHCODE = '" + strNewWHNo + "' where [QWNO]= '" + strSelectQwNo + "' \r\n");
                sb.Append("UPDATE OUTWH_2 SET WHCODE = '" + strNewWHNo + "' where [QWNO]= '" + strSelectQwNo + "' \r\n");
                iIndex++;
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
