using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Utils.Cache;
using System.Threading;
using System.Data;
using Com.ValuePlus.DAL;
using System.Text;
using System.Collections;
using System.Resources;

public partial class Asset_BoundTag : PageBase
{
    private int iMaxCount = 20;
    private string strTableName = "需打印标签资产清单";
    private ResourceManager rmLocResourceManager;

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Asset_BoundTag));
        if (!Page.IsPostBack)
        {
            try
            {
                rmLocResourceManager = base.GetResourceManager("AssetBound");
                this.SetLanguageShow();

                CacheHelper.SetCache("EpcIdFromMoblie", "");
                this.txtFilterACode.Attributes.Add("onkeypress", "FilterEvent('资产编码')");
                this.txtFilterBarCode.Attributes.Add("onkeypress", "FilterEvent('标签ID')");
                this.txtFilterName.Attributes.Add("onkeypress", "FilterEvent('资产名称')");
                this.txtFilterNameChs.Attributes.Add("onkeypress", "FilterEvent('资产中文名')");
                this.txtFilterModel.Attributes.Add("onkeypress", "FilterEvent('资产型号')");
                this.txtFilterDept.Attributes.Add("onkeypress", "FilterEvent('使用部门')");
                this.txtFilterLocationChs.Attributes.Add("onkeypress", "FilterEvent('存放地址')");

                this.strFilterSql = "";
                this.BindDataGrid(this.strFilterSql);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }


    #region viewstate初始化区域
    private String strEPCIDFromMolie
    {
        get
        {
            return ViewState["strEPCIDFromMolie"] as string;
        }
        set
        {
            ViewState["strEPCIDFromMolie"] = value;
        }
    }
    private string strFilterSql
    {
        get
        {
            return ViewState["strFilterSql_ViewState"] as string;
        }
        set
        {
            ViewState["strFilterSql_ViewState"] = value;
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
    /// 设置界面中英文显示
    /// </summary>
    private void SetLanguageShow()
    {
        this.btnBatchBound.Text = rmLocResourceManager.GetString("btnBatchSave");
        this.btnClose.Text = rmLocResourceManager.GetString("btnClose");

        this.cbIsNow.Text = rmLocResourceManager.GetString("cbNowBound");

        this.txtGetEPCID.Text = rmLocResourceManager.GetString("txtWaitingBound");
        this.txtFilterACode.ToolTip = rmLocResourceManager.GetString("txtFilterACode");
        this.txtFilterBarCode.ToolTip = rmLocResourceManager.GetString("txtFilterBarCode");
        this.txtFilterName.ToolTip = rmLocResourceManager.GetString("txtFilterName");
        this.txtFilterNameChs.ToolTip = rmLocResourceManager.GetString("txtFilterNameChs");
        this.txtFilterModel.ToolTip = rmLocResourceManager.GetString("txtFilterModel");
        this.txtFilterDept.ToolTip = rmLocResourceManager.GetString("txtFilterDept");
        this.txtFilterLocationChs.ToolTip = rmLocResourceManager.GetString("txtFilterLocationChs");

        this.lbNowTip1.Text = rmLocResourceManager.GetString("lbBoundTip1");
        this.lbNowTip2.Text = rmLocResourceManager.GetString("lbBoundTip2");
        this.lbCountLabel.Text = rmLocResourceManager.GetString("lbCountLabel");
        this.lbMaxCount.Text = rmLocResourceManager.GetString("lbMaxCount");

        this.hfConfirmIsBinding.Value = rmLocResourceManager.GetString("tipIsConfirmBound");

        this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("lbACode");
        this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("lbBarCode");
        this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("lbAName");
        this.DataGrid1.Columns[5].HeaderText = rmLocResourceManager.GetString("lbANameChs");
        this.DataGrid1.Columns[6].HeaderText = rmLocResourceManager.GetString("lbModel");
        this.DataGrid1.Columns[7].HeaderText = rmLocResourceManager.GetString("lbUseDept");
        this.DataGrid1.Columns[8].HeaderText = rmLocResourceManager.GetString("lbLocationChs");
        this.DataGrid1.Columns[9].HeaderText = rmLocResourceManager.GetString("lbRemark");
    }

    /// <summary>
    /// 根据是否即时绑定设置客户端控件
    /// </summary>
    private void SetClietByIsNow(String strMsg)
    {
        String bIsNow = "true";
        if (this.hfIsNow.Value.Equals("0"))
        {
            bIsNow = "false";
        }else if (this.hfIsNow.Value.Equals("1"))
        {
            bIsNow = "true";
        }
        String strScript = "<script language=javascript>CheckIsNow(" + bIsNow + ");</script>";
            ; 
        if (!String.IsNullOrEmpty(strMsg))
        {
            strScript = "<script language=javascript>CheckIsNow(" + bIsNow + ");alert('" + strMsg + "');</script>";
        }
        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", strScript);
    }

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    /// <param name="strBCode"></param>
    private void BindDataGrid(String strFilter)
    {
        try
        {
            //String strSql = "SELECT top " + iMaxCount + " A.*,B.SNAME AS LOCATION,B.SNAMECN AS LOCATIONCHS FROM AMASSETS_1 A,AMLOCATION_1 B WHERE A.SLCODE=B.SLCODE ";

            String strSql = "SELECT * FROM  " + strTableName+" where 1=1 ";
            if (!String.IsNullOrEmpty(strFilter))
            {
                strSql = strSql + " " + strFilter;
            }
            strSql = strSql + " ORDER BY 资产编码";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            this.dsGridList = ds;
            this.DataGrid1.DataSource = this.dsGridList;
            this.DataGrid1.DataBind();
            this.lbCount.Text = ds.Tables[0].Rows.Count.ToString();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("Asset_BoundTag.BindDataGrid() Error!");
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
            e.Item.ToolTip = e.Item.Cells[1].Text.ToString();

            TextBox txtBarCode = (TextBox)e.Item.Cells[2].FindControl("txtBarCode");
            txtBarCode.Text = e.Item.Cells[3].Text.ToString().Replace("&nbsp;", "");

            CheckBox chkSelect = (CheckBox)e.Item.FindControl("cBox");
            chkSelect.Style.Add("cursor", "hand");
            chkSelect.Attributes.Add("OnClick", "SetCheckBoxState()");
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

            //根据是否即时绑定设置客户端控件
            this.SetClietByIsNow(null);
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

    #region 保存单个绑定
    /// <summary>
    /// 保存单个绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SingleBoundClick(object sender, EventArgs e)
    {
        this.SaveSingleBound();
    }

    /// <summary>
    /// 保存单个绑定
    /// </summary>
    private void SaveSingleBound()
    {
        try
        {
            System.Web.UI.WebControls.CheckBox chkSelect;
            System.Web.UI.WebControls.TextBox txtBarCode;
            String strSelectACode = "";
            String strBoundEPCID = "";
            foreach (DataGridItem dgItem in this.DataGrid1.Items)
            {
                chkSelect = (CheckBox)dgItem.FindControl("cBox");
                txtBarCode = (TextBox)dgItem.FindControl("txtBarCode");
                if (chkSelect.Checked)
                {
                    strSelectACode = dgItem.Cells[1].Text;
                    strBoundEPCID = txtBarCode.Text.Trim();
                    break;
                }
            }
            if (!String.IsNullOrEmpty(strSelectACode))
            {
                String strSql = "update AMASSETS_1 SET SBARCODE = '" + strBoundEPCID + "' WHERE SACODE = '" + strSelectACode + "';";
                strSql = strSql + "UPDATE " + strTableName + " SET 标签ID = '" + strBoundEPCID + "' where 资产编码 = '" + strSelectACode + "';";
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                //this.BindDataGrid(this.strFilterSql);
                //根据是否即时绑定设置客户端控件
                this.SetClietByIsNow("Successfully!");

            }
            else
            {
                //根据是否即时绑定设置客户端控件
                this.SetClietByIsNow("No Asset selected!");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Asset_BoundTag.SaveSingleBound() Error！");
        }
    }
    #endregion
    
    #region 保存批量绑定
    /// <summary>
    /// 保存批量绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BatchBoundClick(object sender, EventArgs e)
    {
        try
        {
            String strUpdateSql = this.GetUpdateSqlString().ToString();
            if (!String.IsNullOrEmpty(strUpdateSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strUpdateSql);
                if (iCount > 0)
                {
                    //根据是否即时绑定设置客户端控件
                    this.SetClietByIsNow("Successfully!");
                }
            }
            else
            {
                //根据是否即时绑定设置客户端控件
                this.SetClietByIsNow("Please select record!");
            }
            
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("Asset_BoundTag.BatchBoundClick() Error!");
        }
        //this.BindDataGrid(this.strFilterSql);

    }


    /// <summary>
    /// 根据界面中复选框获取更新数据库的字符串
    /// </summary>
    /// <returns></returns>
    private StringBuilder GetUpdateSqlString()
    {
        System.Web.UI.WebControls.CheckBox chkSelect;
        System.Web.UI.WebControls.TextBox txtBarCode;

        int iIndex = 1;
        StringBuilder strBuilder2 = new StringBuilder();
        Hashtable hsBwNo = new Hashtable();
        foreach (DataGridItem dgItem in this.DataGrid1.Items)
        {
            if ((dgItem.ItemType == ListItemType.Item) || (dgItem.ItemType == ListItemType.AlternatingItem))
            {
                chkSelect = (CheckBox)dgItem.Cells[0].FindControl("cBox");
                if (chkSelect.Checked)
                {
                    String strACode = dgItem.Cells[1].Text.ToString().Replace("&nbsp;", "").Replace("\r\n", "");
                    txtBarCode = (TextBox)dgItem.Cells[2].FindControl("txtBarCode");
                    String strBarCode = txtBarCode.Text.ToString().Trim();

                    String strSqlString = "UPDATE AMASSETS_1 SET SBARCODE = '" + strBarCode + "' where SACODE = '" + strACode + "'\r\n";
                    strBuilder2.Append("UPDATE " + strTableName + " SET 标签ID = '" + strBarCode + "' where 资产编码 = '" + strACode + "'\r\n");
                    strBuilder2.Append(strSqlString + "\r\n");
                    iIndex++;

                    continue;
                }
            }
        }
        return strBuilder2;
    }

    #endregion

    #region 点击过滤记录集
    /// <summary>
    /// 点击过滤记录集
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FilterClick(object sender, EventArgs e)
    {
        String strFilterField = this.hfFilterField.Value;
        //switch (strFilterField)
        //{
        //    case "SACODE":
        //        this.strFilterSql = " A.SACODE LIKE '%" + this.txtFilterACode.Text + "%' ";
        //        break;
        //    case "SBARCODE":
        //        this.strFilterSql = " A.SBARCODE LIKE '%" + this.txtFilterBarCode.Text + "%' ";
        //        break;
        //    case "SANAME":
        //        this.strFilterSql = " A.SANAME LIKE '%" + this.txtFilterName.Text + "%' ";
        //        break;
        //    case "SANAMECHS":
        //        this.strFilterSql = " A.SANAMECHS LIKE '%" + this.txtFilterNameChs.Text + "%' ";
        //        break;
        //    case "LOCATIONCHS":
        //        this.strFilterSql = " B.SNAMECN LIKE '%" + this.txtFilterLocationChs.Text + "%' ";
        //        break;
        //}
        this.strFilterSql = "";
        if (!String.IsNullOrEmpty(this.txtFilterACode.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 资产编码 LIKE '%" + this.txtFilterACode.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterBarCode.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 标签ID LIKE '%" + this.txtFilterBarCode.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterName.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 资产名称 LIKE '%" + this.txtFilterName.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterNameChs.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 资产中文名 LIKE '%" + this.txtFilterNameChs.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterModel.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 资产型号 LIKE '%" + this.txtFilterModel.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterDept.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 使用部门 LIKE '%" + this.txtFilterDept.Text.ToString().Trim() + "%' ";
        }
        if (!String.IsNullOrEmpty(this.txtFilterLocationChs.Text.ToString().Trim()))
        {
            this.strFilterSql = this.strFilterSql + " AND 存放地址 LIKE '%" + this.txtFilterLocationChs.Text.ToString().Trim() + "%' ";
        }

        this.BindDataGrid(this.strFilterSql);

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>CheckIsNow(true);</script>"); 
    }
    #endregion

    #region Ajax技术处理区域
    /// <summary>
    /// 从cache中获取由手持终端即时传递的EPCID
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public String GetEpcIdFromMoblieCache()
    {
        String strReturn = "";
        Object obj = CacheHelper.GetCache("EpcIdFromMoblie");
        if (obj != null)
        {
            strReturn = obj.ToString();
        }
        return strReturn;
    }


    /// <summary>
    /// test
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public void test()
    {
        CacheHelper.SetCache("EpcIdFromMoblie", DateTime.Now.ToString());
    }



    #endregion


}
