using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_BTW_PackageArticle : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                //物料业务编号
                if (Request.Params["P0"] != null)
                {
                    this.strMBOCODE = Request.Params["P0"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strMBOCODE = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strMBOCODE);
                }
                this.GetHadCode();

            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutWarehouse.Page_Load() Error!");
        }

    }


    #region viewstate初始化区域
    private string strMBOCODE
    {
        get
        {
            return ViewState["MBOCODE_ViewState"] as string;
        }
        set
        {
            ViewState["MBOCODE_ViewState"] = value;
        }
    }
    private string strMboType
    {
        get
        {
            return ViewState["strMboType_ViewState"] as string;
        }
        set
        {
            ViewState["strMboType_ViewState"] = value;
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
    private Hashtable hsTableHad
    {
        get
        {
            if (this.ViewState["hsTableHad"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)this.ViewState["hsTableHad"];
        }
        set
        {
            this.ViewState["hsTableHad"] = value;
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
    
    /// <summary>
    /// 根据当前MBOCODE获取已经具有的物料
    /// </summary>
    private void GetHadCode()
    {
        try
        {
            Hashtable hsTable = new Hashtable();

            String strSql = "select A.MBOTYPE,A.MBOCODE,B.SUBMCODE from MBO_1 A LEFT JOIN MBO_3 B ON A.MBOCODE = B.MBOCODE WHERE A..MBOCODE = '" + this.strMBOCODE + "'";

            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    if (!hsTable.ContainsKey(dr["MBOCODE"].ToString() + dr["SUBMCODE"].ToString()))
                    {
                        hsTable.Add(dr["MBOCODE"].ToString() + dr["SUBMCODE"].ToString(), dr["MBOCODE"].ToString() + dr["SUBMCODE"].ToString());
                    }
                }
                this.strMboType = ds.Tables[0].Rows[0]["MBOTYPE"].ToString();
            }
            this.hsTableHad = hsTable;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_PackageArticle.GetHadCode() Error!");
        }
    }

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="strSMCode"></param>
    /// <param name="strMCode"></param>
    /// <param name="strNameChs"></param>
    private void BindDataGrid(String strSMCode, String strMCode, String strNameChs)
    {
        try
        {
            //维修业务的“拆解部件”和“采购部件”
            String strSql = "select A.* from MBO_1 A WHERE A.ISCONFIRM = '1' AND A.MBOTYPE  = '" + this.strMboType + "' and A.MPROPERTY IN ('002','004')";

            if (!String.IsNullOrEmpty(strSMCode))
            {
                strSql = strSql + " AND A.SMCODE = '" + strSMCode + "'";
            }
            if (!String.IsNullOrEmpty(strMCode))
            {
                strSql = strSql + " AND A.MCODE = '" + strMCode + "'";
            }
            if (!String.IsNullOrEmpty(strNameChs))
            {
                strSql = strSql + " AND A.MNAMECHS LIKE '%" + strNameChs + "%'";
            }
            strSql = strSql + " ORDER BY a.MBOCODE,a.SMCODE,a.MCODE ";
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
            log.Error("AppFunction_BTW_PackageArticle.BindDataGrid() Error!");
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
        if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        {
            //动态选择框
            CheckBox cbox = (CheckBox)e.Item.Cells[0].FindControl("cbox");
            cbox.Attributes.Add("onclick", "CheckRow(this.checked, this.id);");
            //动态生成数量文本框
            TextBox txtCount = (TextBox)e.Item.Cells[8].FindControl("txtCount");
            //txtCount.Enabled = false;
            float fCurCount = float.Parse(e.Item.Cells[7].Text.ToString());
            if (fCurCount < 0)
            {
                fCurCount = 0;
            }
            txtCount.Text = fCurCount.ToString();
            txtCount.Attributes.Add("onblur", "VerifyCountTextBox(this.id," + txtCount.Text + ");");

            //反写已经在数据中选择的项目
            String strSubMBOCODE = this.strMBOCODE + e.Item.Cells[2].Text.ToString().Trim();
            if (this.hsTableHad.ContainsKey(strSubMBOCODE))
            {
                cbox.Checked = true;

            }

            //采购来源中文显示
            String strPurchaseSource = e.Item.Cells[9].Text.ToString().Trim();
            if (strPurchaseSource.Equals("001"))
            {
                e.Item.Cells[9].Text = "国外进口";
            } else if (strPurchaseSource.Equals("002"))
            {
                e.Item.Cells[9].Text = "国内采购";
            }


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
        String strSMCode = this.txtSMCODE.Text.ToString().Trim();
        String strMCode = this.txtMCODE.Text.ToString().Trim();
        String strNamcChs = this.txtChinese.Text.ToString().Trim();
        this.BindDataGrid(strSMCode, strMCode, strNamcChs);

    }
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //获取所选记录
            System.Web.UI.WebControls.CheckBox chkSelect;
            System.Web.UI.WebControls.TextBox txtCount;

            int iSelectCount = 1;
            StringBuilder strBuilder = new StringBuilder();
            StringBuilder strBuilder2 = new StringBuilder();
            strBuilder2.Append("DELETE FROM [TB_MultiPackageArticle] where MBOCODE = '"+this.strMBOCODE+"'\r\n");
            foreach (DataGridItem dgItem in this.DataGrid1.Items)
            {
                if ((dgItem.ItemType == ListItemType.Item) || (dgItem.ItemType == ListItemType.AlternatingItem))
                {
                    chkSelect = (CheckBox)dgItem.Cells[0].FindControl("cbox");
                    if (chkSelect.Checked)
                    {
                        //物料业务编码
                        String strMBOcode_Selected = dgItem.Cells[1].Text.ToString().Trim();
                        //物料编码
                        String strMCode_Selected = dgItem.Cells[2].Text.ToString().Trim();
                        //本次出仓数量
                        txtCount = (TextBox)dgItem.Cells[8].FindControl("txtCount");
                        String strCount = txtCount.Text.ToString().Trim();
                        if (float.Parse(strCount) > 0)
                        {
                            String strSqlString = "INSERT INTO [TB_MultiPackageArticle](MBOCODE,SEQNO,SUBMBOCODE,SUBMCODE,QTY,DTSTR) VALUES ('" + this.strMBOCODE + "','" + iSelectCount.ToString() + "','" + strMBOcode_Selected + "','" + strMCode_Selected + "','" + strCount + "','" + DateTime.Now.ToString() + "')";
                            strBuilder2.Append(strSqlString + "\r\n");
                            iSelectCount++;
                        }
                        continue;

                    }
                }
            }

            if (!String.IsNullOrEmpty(strBuilder2.ToString()))
            {
                strBuilder.Append(strBuilder2.ToString() + "\r\n");
                //插入成功后执行存储过程
                strBuilder.Append("EXEC [USP_PackageArticle] '" + this.strMBOCODE + "', '" + this.GetUserCode() + "' \r\n");

            }

            if (!String.IsNullOrEmpty(strBuilder.ToString()))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strBuilder.ToString());
                if (iCount > 0)
                {
                    this.AlertMessageBox(this.Page, "Successfully!");
                    this.BindDataGrid(this.txtSMCODE.Text, this.txtMCODE.Text, this.txtChinese.Text);
                    this.RefreshOpener(true);
                }
                else
                {
                    this.AlertMessageBox(this.Page, "Failed!");
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "Failed,No Article be Packaged!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("AppFunction_BTW_OutWarehouse.btnSave_Click() Error!");
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
        strB.Append("        if(window.opener.document.getElementById(\"aRefreshDetail\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefreshDetail\").click();\r\n");
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