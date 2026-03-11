using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common.Config;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common.Security;

public partial class Query_QuerySort : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (Request.Params["tbName"] != null)
                {
                    this.strTbName = Request.Params["tbName"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.strTbName = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTbName);
                    if (Request.Params["order"] != null)
                    {
                        this.strOrderString = Request.Params["order"].ToString();
                        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                        this.strOrderString = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOrderString);
                        this.hsTableSortString = this.GetSortStringToHstable(this.strOrderString);
                    }
                    this.BindDataGrid();
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
    private string strTbName
    {
        get
        {
            return ViewState["strVwName_ViewState"] as string;
        }
        set
        {
            ViewState["strVwName_ViewState"] = value;
        }
    }
    private string strOrderString
    {
        get
        {
            return ViewState["strOrderString_ViewState"] as string;
        }
        set
        {
            ViewState["strOrderString_ViewState"] = value;
        }
    }
    private Hashtable hsTableSortString
    {
        get
        {
            return ViewState["hsTableSortString_ViewState"] as Hashtable;
        }
        set
        {
            ViewState["hsTableSortString_ViewState"] = value;
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
    private int iRecordCount
    {
        get
        {
            if (this.ViewState["iRecordCount"] != null)
            {
                return (int)this.ViewState["iRecordCount"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iRecordCount"] = value;
        }
    }
    #endregion

    #region 解析排序字符串到Hashtable
    /// <summary>
    /// 解析排序字符串到Hashtable
    /// </summary>
    /// <param name="strSort"></param>
    /// <returns></returns>
    private Hashtable GetSortStringToHstable(String strSort)
    {
        Hashtable hsTable = new Hashtable();
        if (!String.IsNullOrEmpty(strSort))
        {
            String[] strArray = strSort.Split(',');
            if (strArray != null)
            {
                int iCount = strArray.Length;
                for (int i = 0; i < iCount; i++)
                {
                    String str1 = strArray[i];
                    String str2 = (i+1).ToString();
                    if (str1.ToUpper().IndexOf("DESC") >= 0)
                    {
                        str1 = str1.Replace(" DESC","");
                        str2 = str2 + ";DESC";
                    }
                    hsTable.Add(str1, str2);
                }
            }
        }
        return hsTable;
    }
    #endregion

    #region 绑定DataGrid数据
    /// <summary>
    /// 绑定DataGrid数据
    /// </summary>
    private void BindDataGrid()
    {
        String strViewKeyColName = BaseConfig.Instance.GetConfigValueByKey("KeyName_QueryView");
        String strSql = "SELECT * FROM VIEWLANG_1 WHERE VWNAME = '" + this.strTbName + "'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            String strCationName = "b.[caption]";
            if (this.Language.Equals("zh-cn"))
            {
                strCationName = "b.[captioncn]";
            }
            strCationName = "isnull(" + strCationName + ",a.column_name)";
            strSql = "SELECT a.column_name as [column]," + strCationName + " as [caption]  FROM INFORMATION_SCHEMA.COLUMNS a left join VIEWLANG_2 b on a.TABLE_NAME = b.VWNAME and a.column_name = b.cname where a.TABLE_NAME =   '" + this.strTbName + "' and a.column_name<>'" + strViewKeyColName + "' ORDER BY ORDINAL_POSITION";
        }
        else
        {
            strSql = "SELECT column_name as [column],column_name as [caption]  FROM INFORMATION_SCHEMA.COLUMNS  where TABLE_NAME =   '" + this.strTbName + "' and column_name<>'" + strViewKeyColName + "' ORDER BY ORDINAL_POSITION";
        }

        //String strSql = "SELECT a.column_name as [column]," + strCationName + " as [caption]  FROM INFORMATION_SCHEMA.COLUMNS a left join VIEWLANG_2 b on a.column_name = b.cname where a.TABLE_NAME =   '" + this.strTbName + "' and a.column_name<>'" + strViewKeyColName + "' ORDER BY ORDINAL_POSITION";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
            this.iRecordCount = ds.Tables[0].Rows.Count;
        }

        this.DataGrid1.DataSource = ds;
        this.DataGrid1.DataKeyField = "column";
        this.DataGrid1.DataBind();
        

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
        if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
        {
            int i = e.Item.ItemIndex;
            String sddd = e.Item.Cells[2].Text.ToString();
            int iCount = this.iRecordCount;

            DropDownList ddListNumber = (DropDownList)e.Item.Cells[0].FindControl("ddListNumber");
            String strColumnName = "[" + e.Item.Cells[1].Text.ToString() + "]";
            CheckBox cbRule = (CheckBox)e.Item.Cells[3].FindControl("cbOrder");

            ListItem lItem = new ListItem("", "");// 构造一项
            ddListNumber.Items.Insert(0, lItem);
            for (int j = 1; j <= iCount; j++)
            {
                lItem = new ListItem(j.ToString(), j.ToString());// 构造一项
                ddListNumber.Items.Insert(j, lItem);
            }
            //回填
            if (hsTableSortString.ContainsKey(strColumnName))
            {
                String strValue = hsTableSortString[strColumnName].ToString();
                if (!String.IsNullOrEmpty(strValue))
                {
                    String[] strArray = strValue.Split('※');
                    if (strArray != null)
                    {
                        //顺序or倒序
                        String strOrderNumber = strArray[0].ToString(); 
                        if (strValue.ToUpper().IndexOf("DESC") >= 0)
                        {
                            cbRule.Checked = true;
                            strOrderNumber = strOrderNumber.Replace(";DESC", "");
                        }
                        //排序序号
                        ddListNumber.SelectedIndex = ddListNumber.Items.IndexOf(ddListNumber.Items.FindByValue(strOrderNumber));
                    }
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

    /// <summary>
    /// 确定操作
    /// </summary>
    protected void OK_Click(object sender, EventArgs e)
    {
        try
        {
            System.Web.UI.WebControls.DropDownList ddListNumber;
            System.Web.UI.WebControls.CheckBox cbRule;
            Hashtable hsTable = new Hashtable();
            String strSelectValue = "";
            String strColumnName = "";
            String strSort = "";
            foreach (DataGridItem dgItem in this.DataGrid1.Items)
            {
                ddListNumber = (DropDownList)dgItem.Cells[0].FindControl("ddListNumber");
                cbRule = (CheckBox)dgItem.Cells[3].FindControl("cbOrder");
                if (!String.IsNullOrEmpty(ddListNumber.SelectedValue.ToString()))
                {
                    strSelectValue = ddListNumber.SelectedValue.ToString();
                    strColumnName = "[" + dgItem.Cells[1].Text.ToString() + "]";
                    if (cbRule.Checked)
                    {
                        strColumnName = strColumnName + " DESC";
                    }

                    if (hsTable.ContainsKey(strSelectValue))
                    {
                        this.AlertMessageBox(this.Page,"These is the same order ,please select it again!");
                        ddListNumber.Focus();
                        return;
                    }
                    else
                    {
                        hsTable.Add(strSelectValue, strColumnName);
                    }
                }
            }
            if (hsTable != null)
            {
                ArrayList list = new ArrayList(hsTable.Keys);
                list.Sort();
                int i = 1;
                foreach (string str in list)
                {
                    if (i == 1)
                    {
                        strSort = hsTable[str].ToString();
                    }
                    else
                    {
                        strSort = strSort + "," + hsTable[str].ToString();
                    }
                    i++;
                }
            }

            if (!String.IsNullOrEmpty(strSort))
            {
                this.strOrderString = strSort;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>SetOpenerSorting('" + strSort + "')</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('No sort rule setting!');</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Query_QuerySort.OK_Click() Error！");
        }
    }

}
