using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.BLL;

public partial class Archive_Detail_ArchiveDetailBatch : ArchivePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                //解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.TID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
                this.RID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
                this.SID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
                this.GID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GID");
                this.KEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEY");
                this.KEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEYVALUE");
                this.OPTYPE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");

                if (this.Language.Equals("zh-cn"))
                {
                    this.Page.Title = Session["ArchiveDesc"] + "数据批量多选列表";
                }
                else
                {
                    this.Page.Title = Session["ArchiveDesc"] + " Batch Select Data List";
                }

                //if (Request.Params["ISOK"] != null)
                //{
                //    this.IsConfirm = Request.Params["ISOK"].ToString().Equals("1") ? true : false;
                //}
                this.strTableName = this.TID + "_" + this.GID;
                this.txtFilter.Attributes.Add("onkeypress", "EnterFilterTextBox()");

                this.GetPDefineInfoRecordByPID(this.TID, this.SID, this.GID);
                //获取页面数据集的SQL语句
                this.GetRecordSqlString(this.strPCtrlDetail);
                this.GetRecordKeyField(this.strPCtrlIdSet);
                this.hsTablePIDKeyValueList = this.GetPIDKeyValueList();

                //分页获取数据集
                this.dtCurPageRecord = this.GetDBListData(0, "");

                //初始化分页部分
                this.SetPageCount();
                this.SetPageCurNum();

                this.BindDataGird(this.dtCurPageRecord);
                //如果同时点击确定
                if (this.IsConfirm)
                {
                    this.ConfirmSelected();
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
    private string strTableName
    {
        get
        {
            return ViewState["strTableName_ViewState"] as string;
        }
        set
        {
            ViewState["strTableName_ViewState"] = value;
        }
    }
    private string strPCtrlIdKey
    {
        get
        {
            return ViewState["strPIDKey_ViewState"] as string;
        }
        set
        {
            ViewState["strPIDKey_ViewState"] = value;
        }
    }
    private string strPMast
    {
        get
        {
            return ViewState["strPMast_ViewState"] as string;
        }
        set
        {
            ViewState["strPMast_ViewState"] = value;
        }
    }
    private string strCurMastValue
    {
        get
        {
            return ViewState["strCurMastValue_ViewState"] as string;
        }
        set
        {
            ViewState["strCurMastValue_ViewState"] = value;
        }
    }
    private bool IsConfirm
    {
        get
        {
            if (ViewState["IsConfirm"] != null)
            {
                return (bool)ViewState["IsConfirm"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsConfirm"] = value;
        }
    }
    private string strPCtrlIdSet
    {
        get
        {
            return ViewState["strPCtrlIdSet_ViewState"] as string;
        }
        set
        {
            ViewState["strPCtrlIdSet_ViewState"] = value;
        }
    }
    private string strPCtrlDetail
    {
        get
        {
            return ViewState["strPCtrlDetail_ViewState"] as string;
        }
        set
        {
            ViewState["strPCtrlDetail_ViewState"] = value;
        }
    }
    private string strRecordKeyField
    {
        get
        {
            return ViewState["strRecordKeyField_ViewState"] as string;
        }
        set
        {
            ViewState["strRecordKeyField_ViewState"] = value;
        }
    }
    private Hashtable hsTableColumn
    {
        get
        {
            if (this.ViewState["hsTableColumn"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)this.ViewState["hsTableColumn"];
        }
        set
        {
            this.ViewState["hsTableColumn"] = value;
        }
    }
    private Hashtable hsTablePIDKeyValueList
    {
        get
        {
            if (this.ViewState["hsTablePIDKeyValueList"] == null)
            {
                return new Hashtable();
            }
            return (Hashtable)this.ViewState["hsTablePIDKeyValueList"];
        }
        set
        {
            this.ViewState["hsTablePIDKeyValueList"] = value;
        }
    }

    public DataTable dtAllRecord
    {
        get
        {
            if (this.ViewState["dtCurPageRecord"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtCurPageRecord"];
        }
        set
        {
            this.ViewState["dtCurPageRecord"] = value;
        }
    }
    public DataTable dtCurPageRecord
    {
        get
        {
            if (this.ViewState["dtCurPageRecord"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtCurPageRecord"];
        }
        set
        {
            this.ViewState["dtCurPageRecord"] = value;
        }
    }
    private int iPageSize
    {
        get
        {
            if (ViewState["iPageSize"] != null)
            {
                return (int)ViewState["iPageSize"];
            }
            else
            {
                return 20;
            }
        }
        set
        {
            ViewState["iPageSize"] = value;
        }

    }
    private int iPageIndex
    {
        get
        {
            if (ViewState["iPageIndex"] != null)
            {
                return (int)ViewState["iPageIndex"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iPageIndex"] = value;
        }

    }
    private int iPageCount
    {
        get
        {
            if (ViewState["iPageCount"] != null)
            {
                return (int)ViewState["iPageCount"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iPageCount"] = value;
        }

    }
    private int iRecordCount
    {
        get
        {
            if (ViewState["iRecordCount"] != null)
            {
                return (int)ViewState["iRecordCount"];
            }
            else
            {
                return 0;
            }
        }
        set
        {
            ViewState["iRecordCount"] = value;
        }

    }
    private string strRecordSql
    {
        get
        {
            return ViewState["strRecordSql_ViewState"] as string;
        }
        set
        {
            ViewState["strRecordSql_ViewState"] = value;
        }
    }
    private string strFilterCondition
    {
        get
        {
            return ViewState["strFilterCondition_ViewState"] as string;
        }
        set
        {
            ViewState["strFilterCondition_ViewState"] = value;
        }
    }
    #endregion

    #region 获取某模板的某分组的第一个数据列表字段定义信息
    /// <summary>
    /// 获取某模板的某分组的第一个数据列表字段定义信息
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>、
    /// <returns></returns>
    private DataTable GetPDefineInfoRecordByPID(String strTID, String strSID, String strGID)
    {
        DataTable dt = new DataTable();
        string strSql = "select TOP 1 * from TB_HRTMPSD where TID='" + strTID + "' AND SID='" + strSID + "' AND GID='" + strGID  + "' AND PCTRL = '2' order by PORDER";
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["PWIDTH"] != null)
                {   //如果在字段定义中定义了PWIDTH字段的值，则将每页的大写设置为该值
                    this.iPageSize = int.Parse(dt.Rows[0]["PWIDTH"].ToString());
                }
                this.strPCtrlIdSet = dt.Rows[0]["PCTRLID"].ToString();
                this.strPCtrlDetail = dt.Rows[0]["PCTRLD"].ToString();
                //this.strPCtrlDetail = this.strPCtrlDetail.TrimStart().TrimEnd().ToUpper();
                this.strPCtrlDetail = this.strPCtrlDetail.TrimStart().TrimEnd();
                this.strPCtrlIdKey = dt.Rows[0]["PID"].ToString();
                this.strPMast = dt.Rows[0]["PMAST"].ToString();

                //if (this.strPCtrlIdSet.Contains(";"))
                //{
                //    int iIndex = this.strPCtrlIdSet.IndexOf(";");
                //    this.strPCtrlIdKey = this.strPCtrlIdSet.Substring(0, iIndex);
                //}
                //else
                //{
                //    this.strPCtrlIdKey = this.strPCtrlIdSet;
                //}

                if (!(this.strPCtrlDetail.Contains("top")) && !(this.strPCtrlDetail.ToLower().Contains("percent")))
                {
                    this.strPCtrlDetail = "select top 100 percent " + this.strPCtrlDetail.Remove(0, 6);
                }

                //this.strPIDKey = dt.Rows[0]["PID"].ToString();
                //this.strPCtrlIdSet = dt.Rows[0]["PCTRLID"].ToString();
                //this.strPCtrlDetail = dt.Rows[0]["PCTRLD"].ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取某模板的某分组的第一个数据列表字段定义信息的操作失败，SQL：" + strSql);
        }
        return dt;
    }
    #endregion
    
    #region 获取供选择的数据列表数据集
    /// <summary>
    /// 获取页面数据集的SQL语句
    /// </summary>
    /// <param name="strPCtrlDetail"></param>
    private void GetRecordSqlString(String strPCtrlDetail)
    {
        String strMastValue = "";
        try
        {
            DataTable dt = new DataTable();
            GetArchiveSettingBll bll = new GetArchiveSettingBll();
            Hashtable hsTableRoleParams = bll.GetRoleParamValueByTidARid(this.TID, this.RID, this.GetUserCode(), this.IsAdminstrator());

            if (!String.IsNullOrEmpty(strPMast))//如果存在可空字段
            {
                String strTableName = this.TID + "_" + this.GID;
                String strPid = strPMast;
                if ((strPMast.Contains(";")) && (strPMast.Split(';').Length == 2))
                {
                    strTableName = this.TID + "_" + strPMast.Split(';')[0];
                    strPid = strPMast.Split(';')[1];
                }
                String strSql_Mast = "select top 1 " + strPid + " from " + strTableName + " where " + this.KEY + " = '" + this.KEYVALUE + "'";
                try
                {
                    dt = SqlParamDao.GetDataTableBySql(strSql_Mast);
                    if ((dt != null) && (dt.Rows.Count == 1))
                    {
                        strMastValue = dt.Rows[0][strPid].ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.Error("\r\n");
                    log.Error("模板" + this.TID + "的分组" + this.GID + "的PMAST" + strPMast + "配置有误！");
                }

            }

            String strSql = ParamOperationBll.ReplacePctrlDSqlParam(strPCtrlDetail, strMastValue, hsTableRoleParams);
            this.strRecordSql = strSql;
            this.strCurMastValue = strMastValue;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取页面数据集的SQL语句的操作失败，SQL：" + strPCtrlDetail + "MastValue:" + strMastValue);
        }
    }
    #endregion


    #region 分页获取获取供选择的数据列表数据集
    /// <summary>
    /// 分页获取获取供选择的数据列表数据集
    /// </summary>
    /// <param name="iCurPageindex"></param>
    /// <param name="strFilter"></param>
    /// <returns></returns>
    private DataTable GetDBListData(int iCurPageindex, String strFilter)
    {
        DataTable dt = new DataTable();
        String strSql = "";
        try
        {
            String strTableName = "select * from (" + this.strRecordSql + ") as TEMP1";
            if (!String.IsNullOrEmpty(strFilter))
            {
                strTableName = strTableName + " where " + strFilter;
            }

            strSql = strSql + " SELECT * FROM (";
            strSql = strSql + "              SELECT TOP " + ((iCurPageindex + 1) * this.iPageSize).ToString() + "*, ROW_NUMBER() OVER(ORDER BY " + this.strRecordKeyField + " ASC) AS ROW_ID FROM (" + strTableName + ") as TEMP2";
            strSql = strSql + "             ) AS TEMP3";
            strSql = strSql + " WHERE ROW_ID>" + (iCurPageindex * this.iPageSize).ToString();

            dt = SqlParamDao.GetDataTableBySql(strSql);

            this.dtAllRecord = dt;
            //获取总数
            String strSql_AllCount = "select count(*) from (" + strTableName + ") [COUNT]";
            this.iRecordCount = SqlParamDao.ExecuteScalarBySql(strSql_AllCount);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取供选择的数据列表数据集的操作失败，SQL：" + this.strRecordSql);
        }
        return dt;
    }
    #endregion

    //#region 根据数据集获取数据集对应的字段列集合
    ///// <summary>
    ///// 根据数据集获取数据集对应的字段列集合
    ///// </summary>
    ///// <param name="strTID"></param>
    ///// <returns></returns>
    //private Hashtable GetColumnList(DataTable dtCurPageRecord)
    //{
    //    Hashtable hsTable = new Hashtable();
    //    if (dtCurPageRecord != null)
    //    {
    //        try
    //        {
    //            int iColCount = dtCurPageRecord.Columns.Count;
    //            for (int i = 0; i < iColCount; i++)
    //            {
    //                hsTable.Add(dtCurPageRecord.Columns[i].ColumnName, dtCurPageRecord.Columns[i].Caption);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            log.Error("根据数据集获取数据集对应的字段列集合失败GetColumnList()");
    //            log.Error("\r\n");
    //            log.Error(ex);
    //        }
    //    }
    //    return hsTable;
    //}
    //#endregion

    /// <summary>
    /// 获取列表记录的主键字段及对应字段
    /// </summary>
    /// <param name="strPCTRLID"></param>
    /// <returns></returns>
    private void GetRecordKeyField(String strPCTRLID)
    {
        if (!String.IsNullOrEmpty(strPCTRLID))
        {
            //获取对应字段加载到hstable中
            String[] strArr = strPCTRLID.Split(';');
            this.strRecordKeyField = strArr[0].ToString();//主键字段
            Hashtable hsTable = new Hashtable();
            if (strArr.Length > 1)
            {
                for (int i = 1; i < strArr.Length; i++)
                {
                    String[] strArr1 = strArr[i].Split('=');
                    if (strArr1.Length == 2)
                    {
                        hsTable.Add(strArr1[1].ToString(), strArr1[0].ToString());
                    }
                }
            }
            this.hsTableColumn = hsTable;

        }
    }


    #region 获取数据库中已经保存过的列表数据集
    /// <summary>
    /// 获取数据库中已经保存过的列表数据集
    /// </summary>
    /// <param name="strTID"></param>
    /// <returns></returns>
    private Hashtable GetPIDKeyValueList()
    {
        Hashtable hsTable = new Hashtable();
        DataTable dt = new DataTable();
        String strSql = "select * from " + this.strTableName + " WHERE " + this.KEY + "='" + this.KEYVALUE + "'";
        dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null)&&(dt.Rows.Count>0))
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    hsTable.Add(dt.Rows[i][this.strPCtrlIdKey].ToString(), dt.Rows[i][this.strPCtrlIdKey].ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error("获取数据库中已经保存过的列表数据集失败GetPIDKeyValueList()");
                log.Error("\r\n");
                log.Error(ex);
            }
        }
        return hsTable;
    }
    #endregion

    /// <summary>
    /// 绑定数据集到DataGrid
    /// </summary>
    /// <param name="dt"></param>
    private void BindDataGird(DataTable dt)
    {
        this.DataGrid1.DataSource = dt;
        this.DataGrid1.DataKeyField = this.strRecordKeyField;
        this.DataGrid1.DataBind();
    }

    /// <summary>
    /// DataGrid 数据绑定事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            CheckBox chkSelect = (CheckBox)e.Item.FindControl("cbox");
            chkSelect.Style.Add("cursor", "hand");
            String strCurValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

            for (int i = 1; i < e.Item.Cells.Count; i++)
            {
                //判断是否已经存在这条记录，如果存在则选中
                if ((this.dtCurPageRecord.Columns[i-1].ColumnName.Equals(this.strRecordKeyField)) && (this.hsTablePIDKeyValueList.ContainsKey(strCurValue)))
                {
                    chkSelect.Checked = true;
                    break;
                }
            }

        }
    }


    #region 按钮点击操作
    /// <summary>
    /// 过滤操作
    /// </summary>
    protected void Filter_Click(object sender, EventArgs e)
    {
        try
        {
            String strFilter = "";
            if (!String.IsNullOrEmpty(this.txtFilter.Text))
            {
                if (this.dtAllRecord != null)
                {
                    int iColCount = this.dtAllRecord.Columns.Count;
                    //if (iColCount > 3) iColCount = 3;//考虑性能原因，只对前三个字段进行模糊查询
                    for (int i = 0; i < iColCount; i++)
                    {
                        String strCaption = this.dtAllRecord.Columns[i].Caption;
                        if (!strCaption.Equals("ROW_ID"))
                        {
                            //strCaption = "convert(varchar(100)," + strCaption + ")";
                            if (this.dtAllRecord.Columns[i].DataType.FullName.ToLower().IndexOf("decimal") < 0)
                            {
                                if (i == 0)
                                {
                                    strFilter = strFilter + "(convert(varchar(50),[" + strCaption + "]) like '%" + this.txtFilter.Text + "%')";
                                }
                                else
                                {
                                    strFilter = strFilter + " OR (convert(varchar(50),[" + strCaption + "]) like '%" + this.txtFilter.Text + "%')";
                                }
                            }
                        }
                    }
                }
            }
            //dv.RowFilter = String.Format(strFilter);
            this.strFilterCondition = strFilter;


            //分页获取数据集
            this.dtCurPageRecord = this.GetDBListData(0, this.strFilterCondition);
            this.BindDataGird(this.dtCurPageRecord);

            //this.iRecordCount = dv.Count;
            this.iPageIndex = 0;
            //初始化分页部分
            this.SetPageCount();
            this.SetPageCurNum();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveDetailBatch.Filter_Click() Error！");
        }
    }
    /// <summary>
    /// 确定操作
    /// </summary>
    protected void OK_Click(object sender, EventArgs e)
    {
        this.ConfirmSelected();
    }

    /// <summary>
    /// 确定选择
    /// </summary>
    private void ConfirmSelected()
    {
        try
        {
            System.Web.UI.WebControls.CheckBox chkSelect;
            String strSelectValues = "'',";
            StringBuilder sbSql = new StringBuilder();
            foreach (DataGridItem dgItem in this.DataGrid1.Items)
            {
                chkSelect = (CheckBox)dgItem.FindControl("cbox");
                if (chkSelect.Checked)
                {
                    ////////模式1：先插入主键值，然后在更新其他字段
                    ////主键值
                    //String strSelectValue = dgItem.Cells[1].Text;
                    //strSelectValues = strSelectValues + "'"+strSelectValue+"',";
                    //sbSql.Append("IF NOT EXISTS (SELECT * FROM " + this.strTableName + " WHERE " + this.KEY + "='" + this.KEYVALUE + "' AND " + this.strPCtrlIdKey + "='" + strSelectValue + "')\r\n");
                    //sbSql.Append("INSERT INTO " + this.strTableName + "(" + this.KEY + "," + this.strPCtrlIdKey + ") VALUES ('" + this.KEYVALUE + "','" + strSelectValue + "')\r\n");

                    ////从第三列开始
                    //for (int i = 2; i < dgItem.Cells.Count; i++)
                    //{
                    //    if (this.hsTableColumn.ContainsKey(this.dtCurPageRecord.Columns[i - 1].ColumnName))
                    //    {
                    //        String strColumnName = this.hsTableColumn[this.dtCurPageRecord.Columns[i - 1].ColumnName].ToString();
                    //        String strColumnValue = dgItem.Cells[i].Text.ToString();
                    //        sbSql.Append("UPDATE " + this.strTableName + " SET " + strColumnName + "='" + strColumnValue + "'  WHERE " + this.KEY + "='" + this.KEYVALUE + "' AND " + this.strPCtrlIdKey + "='" + strSelectValue + "'\r\n");
                    //    }
                    //}
                    ////////模式1：先插入主键值，然后在更新其他字段

                    //////模式2：直接插入所有其他字段 modify by sammen 20180317
                    String strSelectValue = dgItem.Cells[1].Text;
                    strSelectValues = strSelectValues + "'" + strSelectValue + "',";
                    sbSql.Append("IF NOT EXISTS (SELECT * FROM " + this.strTableName + " WHERE " + this.KEY + "='" + this.KEYVALUE + "' AND " + this.strPCtrlIdKey + "='" + strSelectValue + "')\r\n");
                    //从第三列开始
                    StringBuilder sbColumnName = new StringBuilder();
                    StringBuilder sbColumnValue = new StringBuilder();
                    for (int i = 2; i < dgItem.Cells.Count; i++)
                    {
                        if (this.hsTableColumn.ContainsKey(this.dtCurPageRecord.Columns[i - 1].ColumnName))
                        {
                            String strColumnName = this.hsTableColumn[this.dtCurPageRecord.Columns[i - 1].ColumnName].ToString();
                            String strColumnValue = dgItem.Cells[i].Text.ToString();

                            // add by sammen 20220104 写入数据库是单引号替换成两个单引号
                            strColumnValue = strColumnValue.Replace("'", "''");
                            // add by sammen 20231213 写入数据库时页面空格&nbsp;替换成空
                            strColumnValue = strColumnValue.Replace("&nbsp;", "");

                            sbColumnName.Append("," + strColumnName);
                            sbColumnValue.Append(",'" + strColumnValue + "'");
                        }
                    }
                    if (!String.IsNullOrEmpty(sbColumnName.ToString()))
                    {
                        sbSql.Append("INSERT INTO " + this.strTableName + "(" + this.KEY + "," + this.strPCtrlIdKey + sbColumnName.ToString()+ ") VALUES ('" + this.KEYVALUE + "','" + strSelectValue + "'"+ sbColumnValue + ")\r\n");
                    }
                    else
                    {
                        sbSql.Append("INSERT INTO " + this.strTableName + "(" + this.KEY + "," + this.strPCtrlIdKey + ") VALUES ('" + this.KEYVALUE + "','" + strSelectValue + "')\r\n");
                    }
                    //////模式2：直接插入所有其他字段 modify by sammen 20180317
                }
            }
            strSelectValues = strSelectValues.TrimEnd(',');
            //删除在列表中不存在而数据库中已经存在的
            //sbSql.Append("delete FROM " + this.strTableName + " WHERE " + this.KEY + "='" + this.KEYVALUE + "' AND " + this.strPCtrlIdKey + " not in (" + strSelectValues + ")\r\n");
                    
            if (!String.IsNullOrEmpty(sbSql.ToString()))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

                try
                {
                    //批量执行新增后执行的数据库脚本 add by sammen 20180321
                    Hashtable hsTableParam = new Hashtable();
                    hsTableParam.Add("TID", this.TID);
                    hsTableParam.Add("GID", this.GID);
                    hsTableParam.Add("Key", this.KEY);
                    hsTableParam.Add("KeyValue", this.KEYVALUE);
                    hsTableParam.Add("GridKey", !String.IsNullOrEmpty(this.GRIDKEY) ? this.GRIDKEY : this.strPCtrlIdKey);
                    hsTableParam.Add("GridKeyValue", "");
                    hsTableParam.Add("UserId", this.GetUserCode());
                    int iCount_SPAfterAdd = ArchiveGridActionBll.DoExcuteSP_AfterAdd(hsTableParam);
                }
                catch (Exception ex)
                {
                    log.Error("批量新增Grid列表后执行新增后动作的存储过程失败！TID="+ this.TID+";GID="+this.GID);
                    log.Error(ex + "\r\n");
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>AfterSave()</script>");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('No record selected or input error!');</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveDetailBatch.ConfirmSelected() Error！");
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('Failed!');</script>");
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
        if (this.dtCurPageRecord != null)
        {
            DataTable dt = this.dtCurPageRecord;
            DataView defaultView = dt.DefaultView;
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
            DataTable dtTemp = new DataTable();
            dtTemp = dt.Copy();
            this.dtCurPageRecord = dtTemp;

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


    #region 页面分页部分
    /// <summary>
    /// 设置获取DataGrid分页部分的数据及显示
    /// </summary>
    private void SetDataGridPageArea()
    {
        //设置获取DataGrid列表的页数
        this.SetPageCount();
        //设置获取DataGrid列表的当前页数
        this.SetPageCurNum();
        this.Label_AllCount.Text = this.iRecordCount.ToString();
        //this.txtPageSize.Text = this.iPageSize.ToString();
    }

    /// <summary>
    /// 设置获取DataGrid列表的页数
    /// </summary>
    private void SetPageCount()
    {
        if ((this.iRecordCount % this.iPageSize) == 0)
        {
            this.iPageCount = this.iRecordCount / this.iPageSize;
        }
        else
        {
            this.iPageCount = (this.iRecordCount / this.iPageSize) + 1;
        }
        this.Label_AllPage.Text = this.iPageCount.ToString();
        this.Label_AllCount.Text = this.iRecordCount.ToString();
    }

    /// <summary>
    /// 设置获取DataGrid列表的当前页数,并根据当前页数设置按钮可用
    /// </summary>
    private void SetPageCurNum()
    {
        this.Label_CurPage.Text = (this.iPageIndex + 1).ToString();
        if (this.iPageIndex <= 0)
        {
            this.aFirstPage.Enabled = false;
            this.aPrePage.Enabled = false;
        }
        else
        {
            this.aFirstPage.Enabled = true;
            this.aPrePage.Enabled = true;
        }
        if (this.iPageIndex >= this.iPageCount - 1)
        {
            this.aNextPage.Enabled = false;
            this.aLastPage.Enabled = false;
        }
        else
        {
            this.aNextPage.Enabled = true;
            this.aLastPage.Enabled = true;
        }
    }

    /// <summary>
    /// 首页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FirstPage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = 0;
            //获取DataGrid数据的DataSet
            this.dtCurPageRecord = this.GetDBListData(this.iPageIndex, this.strFilterCondition);
            this.BindDataGird(this.dtCurPageRecord);

            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }

    }

    /// <summary>
    /// 上一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PrePage_Click(object sender, EventArgs e)
    {
        if (this.iPageIndex > 0)
        {
            this.iPageIndex = this.iPageIndex - 1;
            this.dtCurPageRecord = this.GetDBListData(this.iPageIndex, this.strFilterCondition);
            this.BindDataGird(this.dtCurPageRecord);
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 下一页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NextPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageIndex + 1;
            //获取DataGrid数据的DataSet
            this.dtCurPageRecord = this.GetDBListData(this.iPageIndex, this.strFilterCondition);
            this.BindDataGird(this.dtCurPageRecord);

            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    /// <summary>
    /// 末页按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LastPage_Click(object sender, EventArgs e)
    {
        if ((this.iPageCount > 0) && (this.iPageCount > this.iPageIndex + 1))
        {
            this.iPageIndex = this.iPageCount - 1;
            //获取DataGrid数据的DataSet
            this.dtCurPageRecord = this.GetDBListData(this.iPageIndex, this.strFilterCondition);
            this.BindDataGird(this.dtCurPageRecord);
            //设置获取DataGrid列表的当前页数
            this.SetPageCurNum();
        }
    }

    #endregion

}
