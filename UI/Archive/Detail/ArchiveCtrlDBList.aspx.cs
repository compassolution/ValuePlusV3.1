using System;
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
using Com.ValuePlus.Common.Security;

public partial class Archive_Detail_ArchiveCtrlDBList : ArchivePageBase
{
    //private int iPageSize = 10 ;//每页显示条数
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                //Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                //this.strCurTID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
                //this.strCurRID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
                //this.strCurSID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
                //this.strCurGID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GID");
                //this.strCurPID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "PID");
                //this.strCurMastValue = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "MASTVALUE");
                ////this.strCurKeyValue = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEYVALUE");
                if (Request.Params["TID"] != null)
                {
                    this.strCurTID = Request.Params["TID"].ToString();
                }
                if (Request.Params["RID"] != null)
                {
                    this.strCurRID = Request.Params["RID"].ToString();
                }
                if (Request.Params["SID"] != null)
                {
                    this.strCurSID = Request.Params["SID"].ToString();
                }
                if (Request.Params["GID"] != null)
                {
                    this.strCurGID = Request.Params["GID"].ToString();
                }
                if (Request.Params["PID"] != null)
                {
                    this.strCurPID = Request.Params["PID"].ToString();
                }
                if (Request.Params["MASTVALUE"] != null)
                {
                    this.strCurMastValue = Request.Params["MASTVALUE"].ToString();
                }
                if (Request.Params["KEYVALUE"] != null)
                {
                    this.strCurKeyValue = Request.Params["KEYVALUE"].ToString();
                }
                if (Request.Params["ISOK"] != null)
                {
                    this.IsConfirm = Request.Params["ISOK"].ToString().Equals("1")?true:false;
                }
                if (Request.Params["TextCtrlId"] != null)
                {
                    this.strTextCtrlId = Request.Params["TextCtrlId"].ToString();
                }

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strCurTID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurTID);
                this.strCurRID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurRID);
                this.strCurSID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurSID);
                this.strCurGID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurGID);
                this.strCurPID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurPID);
                this.strCurMastValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurMastValue);
                this.strCurKeyValue = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurKeyValue);
                this.strTextCtrlId = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTextCtrlId);

                if (this.Language.Equals("zh-cn"))
                {
                    this.Page.Title = Session["ArchiveDesc"] + "数据选择列表";
                }
                else
                {
                    this.Page.Title = Session["ArchiveDesc"] + " Data Select List";
                }

                this.txtFilter.Attributes.Add("onkeypress", "EnterFilterTextBox()");

                this.GetPDefineInfoRecordByPID(this.strCurTID, this.strCurSID, this.strCurGID, this.strCurPID);
                //获取页面数据集的SQL语句
                this.GetRecordSqlString(this.strPCtrlDetail, this.strCurMastValue);

                //分页获取数据集
                this.dtCurPageRecord = this.GetDBListData(0,"");

                //初始化分页部分
                this.SetPageCount();
                this.SetPageCurNum();

                //
                this.hsTableColumn = this.GetColumnList(this.dtCurPageRecord);
                this.strRecordKeyField = this.GetRecordKeyField(this.strPCtrlIdSet);
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
    public string strTextCtrlId
    {
        get
        {
            return ViewState["strTextCtrlId_ViewState"] as string;
        }
        set
        {
            ViewState["strTextCtrlId_ViewState"] = value;
        }
    }
    private string strCurTID
    {
        get
        {
            return ViewState["strCurTID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurTID_ViewState"] = value;
        }
    }
    private string strCurRID
    {
        get
        {
            return ViewState["strCurRID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurRID_ViewState"] = value;
        }
    }
    private string strCurSID
    {
        get
        {
            return ViewState["strCurSID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurSID_ViewState"] = value;
        }
    }
    private string strCurGID
    {
        get
        {
            return ViewState["strCurGID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurGID_ViewState"] = value;
        }
    }
    private string strCurPID
    {
        get
        {
            return ViewState["strCurPID_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPID_ViewState"] = value;
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
    private string strCurKeyValue
    {
        get
        {
            return ViewState["strCurKeyValue_ViewState"] as string;
        }
        set
        {
            ViewState["strCurKeyValue_ViewState"] = value;
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
    private string strPCtrlIdKey
    {
        get
        {
            return ViewState["strPCtrlIdKey_ViewState"] as string;
        }
        set
        {
            ViewState["strPCtrlIdKey_ViewState"] = value;
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
    public DataTable dtAllRecord
    {
        get
        {
            if (this.ViewState["dtRecord"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtRecord"];
        }
        set
        {
            this.ViewState["dtRecord"] = value;
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

    #region 获取某模板的某分组某字段定义信息
    /// <summary>
    /// 获取某模板的某分组某字段定义信息
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strPID"></param>、
    /// <returns></returns>
    private DataTable GetPDefineInfoRecordByPID(String strTID, String strSID, String strGID, String strPID)
    {
        DataTable dt = new DataTable();
        string strSql = "select * from TB_HRTMPSD where TID='" + strTID + "' AND SID='" + strSID + "' AND GID='" + strGID + "' AND PID='" + strPID + "'";
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
                this.strPCtrlDetail = this.strPCtrlDetail.TrimStart().TrimEnd().ToUpper();

                if (this.strPCtrlIdSet.Contains(";"))
                {
                    int iIndex = this.strPCtrlIdSet.IndexOf(";");
                    this.strPCtrlIdKey = this.strPCtrlIdSet.Substring(0,iIndex);
                }
                else
                {
                    this.strPCtrlIdKey = this.strPCtrlIdSet;
                }

                if (!(this.strPCtrlDetail.Contains("top")) && !(this.strPCtrlDetail.ToLower().Contains("percent")))
                { 
                    this.strPCtrlDetail = "select top 100 percent "+this.strPCtrlDetail.Remove(0,6);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取某模板的某分组某字段定义信息的操作失败，SQL：" + strSql);
        }
        return dt;
    }
    #endregion

    #region 获取供选择的数据列表数据集

    /// <summary>
    /// 获取页面数据集的SQL语句
    /// </summary>
    /// <param name="strPCtrlDetail"></param>
    /// <param name="strMastValue"></param>
    private void GetRecordSqlString(String strPCtrlDetail, String strMastValue)
    {
        try
        {
            GetArchiveSettingBll bll = new GetArchiveSettingBll();
            Hashtable hsTableRoleParams = bll.GetRoleParamValueByTidARid(this.strCurTID, this.strCurRID, this.GetUserCode(), this.IsAdminstrator());
            this.strRecordSql = ParamOperationBll.ReplacePctrlDSqlParam(strPCtrlDetail, strMastValue, hsTableRoleParams);
            //if (this.iRecordCount <= 0)
            //{
            //    DataTable dt = new DataTable();
            //    dt = SqlParamDao.GetDataTableBySql(this.strRecordSql);
            //    if ((dt != null) && (dt.Rows.Count > 0))
            //    {
            //        this.iRecordCount = dt.Rows.Count;
            //    }
            //    this.dtAllRecord = dt;
            //}
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取页面数据集的SQL语句的操作失败，SQL：" + strPCtrlDetail + "MastValue:" + strMastValue);
        }
    }

    /// <summary>
    /// 分页获取获取供选择的数据列表数据集
    /// </summary>
    /// <param name="iCurPageindex"></param>
    /// <param name="strFilter"></param>
    /// <returns></returns>
    private DataTable GetDBListData(int iCurPageindex,String strFilter) 
    {
        String strSql = "";
        DataTable dt = new DataTable();
        try
        {
            String strTableName = "select * from (" + this.strRecordSql + ") as TEMP1";
            if (!String.IsNullOrEmpty(strFilter))
            {
                strTableName = strTableName + " where " + strFilter;
            }
            
            strSql = strSql+ " SELECT * FROM (";
            strSql = strSql + "              SELECT TOP " + ((iCurPageindex + 1) * this.iPageSize).ToString() + "*, ROW_NUMBER() OVER(ORDER BY " + this.strPCtrlIdKey + " ASC) AS ROW_ID FROM (" + strTableName + ") as TEMP2";
            strSql = strSql+ "             ) AS TEMP3";
            strSql = strSql + " WHERE ROW_ID>" + (iCurPageindex*this.iPageSize).ToString();

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
            log.Error("获取供选择的数据列表数据集的操作失败，SQL：" + strSql);
        }
        return dt;
    }
    #endregion

    #region 根据数据集获取数据集对应的字段列集合
    /// <summary>
    /// 根据数据集获取数据集对应的字段列集合
    /// </summary>
    /// <param name="strTID"></param>
    /// <returns></returns>
    private Hashtable GetColumnList(DataTable dtRecord)
    {
        Hashtable hsTable = new Hashtable();
        if (dtRecord != null)
        {
            try
            {
                int iColCount = dtRecord.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    hsTable.Add(dtRecord.Columns[i].ColumnName, dtRecord.Columns[i].Caption);
                }
            }
            catch (Exception ex)
            {
                log.Error("根据数据集获取数据集对应的字段列集合失败GetColumnList()");
                log.Error("\r\n");
                log.Error(ex);
            }
        }
        return hsTable;
    }
    #endregion

    /// <summary>
    /// 获取列表记录的主键字段
    /// </summary>
    /// <param name="strPCTRLID"></param>
    /// <returns></returns>
    private String GetRecordKeyField(String strPCTRLID)
    {
        String strKeyField = "";
        if (!String.IsNullOrEmpty(strPCTRLID))
        {
            strKeyField = strPCTRLID.Split(';')[0].ToString();
        }
        return strKeyField;
    }

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
    /// 获取页面控件赋值
    /// </summary>
    /// <param name="dr"></param>
    /// <param name="strPCTRLID"></param>
    /// <returns></returns>
    private String GetCtrlValueSet(DataTable dt, String strKeyField, String strKeyValue, String strPCTRLID)
    {
        String strResult = "";
        try
        {
            if (!String.IsNullOrEmpty(strPCTRLID))
            {
                String strFilter = strKeyField + "='" + strKeyValue + "'";
                DataRow[] drs = dt.Select(strFilter);
                //DataRow dr = drs[0];
                //if (dr != null)
                //{
                //}
                String[] strArray = strPCTRLID.Split(';');
                if ((strArray != null) && (strArray.Length > 0))
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        String strTemp = strArray[i].ToString();
                        String[] strArrayValue = strTemp.Split('=');
                        String strNeedSetPID = "";
                        String strCtrlId = "";
                        String strCtrlValueSet = "";
                        if (strArrayValue.Length == 1)
                        {
                            strNeedSetPID = this.strCurPID;
                            strCtrlValueSet = drs.Length>0?drs[0][strArrayValue[0]].ToString():"";
                        }
                        else if (strArrayValue.Length == 2)
                        {
                            strNeedSetPID = strArrayValue[0];
                            strCtrlValueSet = drs.Length > 0 ? drs[0][strArrayValue[1]].ToString() : "";
                        }
                        if (!String.IsNullOrEmpty(strNeedSetPID))
                        {
                            String strSql = "select PCTRL FROM TB_HRTMPD WHERE TID = '" + this.strCurTID + "' AND GID = '" + this.strCurGID + "' AND PID = '" + strNeedSetPID + "'";
                            DataTable dtTemp = SqlParamDao.GetDataTableBySql(strSql);
                            if (dtTemp != null && dtTemp.Rows.Count > 0)
                            {
                                String strCtrlType = dtTemp.Rows[0]["PCTRL"].ToString();
                                if ((strCtrlType.Equals("0")) || (strCtrlType.Equals("3")) || (strCtrlType.Equals("4")) || (strCtrlType.Equals("6")) || (strCtrlType.Equals("9")))
                                {
                                    //文本框
                                    strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(this.strCurTID, this.strCurGID, strNeedSetPID);
                                }
                                else if ((strCtrlType.Equals("1"))||(strCtrlType.Equals("7")))//下拉框
                                {
                                    strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DropDownList(this.strCurTID, this.strCurGID, strNeedSetPID);
                                }
                                else if ((strCtrlType.Equals("2"))||(strCtrlType.Equals("8")))//数据列表
                                {
                                    strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DBTextBox(this.strCurTID, this.strCurGID, strNeedSetPID);
                                }
                                if (!String.IsNullOrEmpty(strCtrlId))
                                {
                                    //hsTable.Add(strCtrlId, strCtrlValueSet);
                                    if (i == 0)
                                    {
                                        strResult = strCtrlId + "＄" + strCtrlValueSet;
                                    }
                                    else
                                    {
                                        strResult = strResult + "＆" + strCtrlId + "＄" + strCtrlValueSet;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("获取页面控件赋值操作失败，可能是配置文件中PCTRLID和PCTRLD字段不匹配，GetCtrlValueSet()");
        }
        return strResult;
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
            CheckBox chkSelect = (CheckBox)e.Item.FindControl("CheckBox1");
            chkSelect.Style.Add("cursor", "hand");
            chkSelect.Attributes.Add("OnClick", "SetCheckBoxState()");
            chkSelect.Attributes.Add("OnDblClick", "DbClickCheckBox()");

            String strCurValue = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            if((!String.IsNullOrEmpty(this.strCurKeyValue))&&(strCurValue.Equals(this.strCurKeyValue)))
            {
                chkSelect.Checked = true;
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
            log.Error("Archive_Detail_ArchiveCtrlDBList.Filter_Click() Error！");
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
            String strSelectValue = "";
            foreach (DataGridItem dgItem in this.DataGrid1.Items)
            {
                chkSelect = (CheckBox)dgItem.FindControl("CheckBox1");
                if (chkSelect.Checked)
                {
                    strSelectValue = dgItem.Cells[1].Text;
                    break;
                }
            }
            //if (!String.IsNullOrEmpty(strSelectValue))
            //{
            String strResult = this.GetCtrlValueSet(this.dtAllRecord, this.strRecordKeyField, strSelectValue, this.strPCtrlIdSet);
            // add by sammen 20220104 单引号'替换成”，然后在js方法SetCtrlsValue中替换回来。
            strResult = strResult.Replace("'", "”");
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>SetCtrlsValue('" + strResult + "');window.close();</script>");
            //}
            //else
            //{
            //    //Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>alert('No record selected or input error!');</script>");
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>window.close();</script>");
            //}
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveCtrlDBList.ConfirmSelected() Error！");
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
        this.Label_CurPage.Text = (this.iPageIndex+1).ToString(); 
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

    #region DataGrid排序
    /// <summary>
    /// DataGrid排序
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (this.dtAllRecord!= null)
        {
            DataTable dt = this.dtAllRecord;
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
            this.dtAllRecord = dtTemp;

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

    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        for (int i=0; i < dtAllRecord.Columns.Count; i++)
        {
            if (dtAllRecord.Columns[i].ColumnName == "PINYIN")
            {
                if (e.Item.Cells.Count > 1)
                {
                    e.Item.Cells[i+1].Visible = false;
                }
            }
        }
    }
}
