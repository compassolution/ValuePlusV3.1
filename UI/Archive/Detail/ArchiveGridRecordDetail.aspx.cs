using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.WebCtrls;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;
using System.Resources;

public partial class Archive_Detail_ArchiveGridRecordDetail : ArchivePageBase
{
    private ArrayList arrayObjectHRLOG2 = new ArrayList();
    private ArrayList arrListMyFreeTextBox = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Archive_Detail_ArchiveDetailAjax), this.Page);
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.TID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TID");
                this.RID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "RID");
                this.SID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "SID");
                this.GID = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GID");
                this.KEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEY");
                this.KEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "KEYVALUE");
                this.GRIDKEY = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GRIDKEY");
                this.GRIDKEYVALUE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "GRIDKEYVALUE");
                this.OPTYPE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");//操作类型（add新增，edit编辑，readonly只读）

                //页面语言设置
                this.DoLanguageSetting();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, this.strTipUrlLoadError);
            }
            try
            {
                if ((!String.IsNullOrEmpty(this.TID)) && (!String.IsNullOrEmpty(this.RID)) && (!String.IsNullOrEmpty(this.SID)) && (!String.IsNullOrEmpty(this.OPTYPE)))
                {
                    //加载数据前执行 add by sammen 20161015
                    this.doBeforeLoadGridDetail();

                    this.IsCanSave = this.JudgeIsCanSave(this.TID,this.SID, this.GID, this.OPTYPE);
                    if (!IsCanSave)
                    {
                        this.aSaveT.Visible = false;
                        this.aSaveF.Visible = false;
                        this.OPTYPE = "readonly";
                    }

                    this.dtCurGroupField = this.GetCurGroupFieldInfo();
                    this.dtCurRecord = this.GetCurRecord();
                    this.BuildKeyValueDDList();
                    this.CreateGridDetailCtrls();


                    //动态加载页面中的客户端事件
                    this.BuildClientEventScript(this.TID, this.SID);

                    //针对FreeTextBoxCtrl控件修改字体及其大小
                    this.AddFontToFreeTextBoxCtrl(this.arrListMyFreeTextBox);

                    //获取模板表格类型的动作列表
                    ArchiveGridActionBll.GetArchiveGridActions();

                    //加载数据后执行 add by sammen 20161015
                    this.doAfterLoadGridDetail();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, this.strTipPageLoadError);
            }
        }
        //设置向前向后切换记录的按钮的可用性
        this.SetPreNextRecordButton();
    }

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
        this.MultiPage1.Controls.Clear();
        this.CreateGridDetailCtrls();
    }

    #region viewstate初始化区域
    private bool IsCanSave
    {
        get
        {
            if (ViewState["IsCanSave"] != null)
            {
                return (bool)ViewState["IsCanSave"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsCanSave"] = value;
        }
    }
    private string strGroupType
    {
        get
        {
            return ViewState["strGroupType_ViewState"] as string;
        }
        set
        {
            ViewState["strGroupType_ViewState"] = value;
        }
    }
    private string strGridKeyType
    {
        get
        {
            return ViewState["strGridKeyType"] as string;
        }
        set
        {
            ViewState["strGridKeyType"] = value;
        }
    }
    private DataTable dtCurGroupField
    {
        get
        {
            if (this.ViewState["dtCurGroupField"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtCurGroupField"];
        }
        set
        {
            this.ViewState["dtCurGroupField"] = value;
        }
    }
    private DataTable dtCurRecord
    {
        get
        {
            if (this.ViewState["dtCurRecord"] == null)
            {
                return new DataTable();
            }
            return (DataTable)this.ViewState["dtCurRecord"];
        }
        set
        {
            this.ViewState["dtCurRecord"] = value;
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

    private string strTipUrlLoadError
    {
        get
        {
            return ViewState["strTipUrlLoadError"] as string;
        }
        set
        {
            ViewState["strTipUrlLoadError"] = value;
        }
    }
    private string strTipPageLoadError
    {
        get
        {
            return ViewState["strTipPageLoadError"] as string;
        }
        set
        {
            ViewState["strTipPageLoadError"] = value;
        }
    }
    private string strTipSaveFailed
    {
        get
        {
            return ViewState["strTipSaveFailed"] as string;
        }
        set
        {
            ViewState["strTipSaveFailed"] = value;
        }
    }
    private string strTipSaveSuccess
    {
        get
        {
            return ViewState["strTipSaveSuccess"] as string;
        }
        set
        {
            ViewState["strTipSaveSuccess"] = value;
        }
    }
    #endregion

    #region 页面语言设置
    /// <summary>
    /// 页面语言设置
    /// </summary>
    private void DoLanguageSetting()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("ArchiveDetail");
        //页面标题设置
        if (this.Language.Equals("zh-cn"))
        {
            this.Page.Title = Session["ArchiveDesc"] + "表格内容明细";
        }
        else
        {
            this.Page.Title = Session["ArchiveDesc"] + " Grid Detail";
        }
        
        this.aClose.Text = rmLocResourceManager.GetString("aClose");
        this.aSaveT.Text = rmLocResourceManager.GetString("aSaveT");
        this.aSaveF.Text = rmLocResourceManager.GetString("aSaveF");

        this.strTipUrlLoadError = rmLocResourceManager.GetString("tipUrlLoadError");
        this.strTipPageLoadError = rmLocResourceManager.GetString("tipPageLoadError");
        this.strTipSaveFailed = rmLocResourceManager.GetString("tipSaveFailed");
        this.strTipSaveSuccess = rmLocResourceManager.GetString("tipSaveSuccess");
    }
    #endregion

    /// <summary>
    /// 判断当前页签是否可以进行保存
    /// </summary>
    /// <param name="strTID"></param>
    /// <param name="strSID"></param>
    /// <param name="strGID"></param>
    /// <param name="strOpType"></param>
    /// <returns></returns>
    private bool JudgeIsCanSave(String strTID, String strSID, String strGID, String strOpType)
    {
        //判断当前页签是否可以进行保存
        bool bIsCanSave = false;
        String strSql = "select * from TB_HRTMPSG where TID='" + strTID + "' AND SID = '" + strSID + "' AND GID='" + strGID + "'";
        DataTable dtTemp = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtTemp != null) || (dtTemp.Rows.Count <= 0))
        {
            DataRow dr = dtTemp.Rows[0];
            if (strOpType.Equals("add") || strOpType.Equals("edit"))
            {
                if (dr["GRIGHT"].ToString().Equals("0"))//只读
                {
                    bIsCanSave = false;
                }
                else
                {
                    bIsCanSave = true;
                }
            }
            else
            {
                bIsCanSave = false;
            }
            this.strGroupType = dr["GTYPE"].ToString();
        }
        else
        {
            this.strGroupType = "2";
        }
        return bIsCanSave;
    }

    #region 所有记录主键值下拉列表操作区域
    /// <summary>
    /// 加载当前模板当前分组具有的所有记录主键值下拉列表
    /// </summary>
    /// <param name="strCurSceneSql"></param>
    private void BuildKeyValueDDList()
    {
        try
        {
            String strTableName = this.TID + "_" + this.GID;
            string strSql = "select " + this.GRIDKEY + " from " + strTableName + " where " + this.KEY + "='" + this.KEYVALUE + "' order by " + this.GRIDKEY;

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.ddListKeyValue.Items.Clear();
                this.ddListKeyValue.DataSource = dt.DefaultView;
                this.ddListKeyValue.DataTextField = this.GRIDKEY;
                this.ddListKeyValue.DataValueField = this.GRIDKEY;
                //设置显示类型(时间类型特殊设置)
                if (!String.IsNullOrEmpty(this.strGridKeyType))
                {
                    if (this.strGridKeyType.ToLower().Equals("date"))
                    {
                        this.ddListKeyValue.DataTextFormatString = @"{0:yyyy\-MM\-dd}";//还可以是格式@"MM\/dd\/yyyy"
                    }
                    else if (this.strGridKeyType.ToLower().Equals("datetime"))
                    {
                        this.ddListKeyValue.DataTextFormatString = @"{0:yyyy\-MM\-dd HH\:mm\:ss}";//还可以是格式@"MM\/dd\/yyyy"
                    }
                    //this.KEYVALUE = DateTime.Parse(this.KEYVALUE).ToString(@"yyyy\-MM\-dd");
                }
                this.ddListKeyValue.DataBind();

                //当前被选中
                if (!String.IsNullOrEmpty(this.GRIDKEYVALUE))
                {
                    this.ddListKeyValue.SelectedIndex = this.ddListKeyValue.Items.IndexOf(this.ddListKeyValue.Items.FindByValue(this.GRIDKEYVALUE));
                }
                this.iRecordCount = dt.Rows.Count;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "加载当前模板当前分组具有的所有记录主键值下拉列表,方法BuildKeyValueDDList!");
            //Response.Write("<script language=\"javascript\">alert('" + "加载当前模板当前分组具有的所有记录主键值下拉列表,方法BuildKeyValueDDList" + "');</script>");
        }
    }

    /// <summary>
    /// 主键值下拉框选择时间
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListKeyValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GRIDKEYVALUE = this.ddListKeyValue.SelectedValue;
        this.RefreshPage();

    }

    /// <summary>
    /// 设置向前向后切换记录的按钮的可用性
    /// </summary>
    private void SetPreNextRecordButton()
    {
        try
        {
            if (this.ddListKeyValue.SelectedIndex <= 0)
            {
                this.imgBtnPre.Attributes.Add("onclick", "return false;");
            }
            else
            {
                this.imgBtnPre.Attributes.Add("onclick", "return true;");
            }
            if (this.ddListKeyValue.SelectedIndex >= this.iRecordCount - 1)
            {
                this.imgBtnNext.Attributes.Add("onclick", "return false;");
            }
            else
            {
                this.imgBtnNext.Attributes.Add("onclick", "return true;");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("设置向前向后切换记录的按钮的可用性失败,方法SetPreNextRecordButton");
        }
    }

    /// <summary>
    /// 切换到前一条记录的操作
    /// </summary>
    protected void imgBtnPre_Click(object sender, EventArgs e)
    {
        try
        {
            int iCur = this.ddListKeyValue.SelectedIndex;
            if (iCur > 0)
            {
                this.GRIDKEYVALUE = this.ddListKeyValue.Items[iCur - 1].Value;
                this.ddListKeyValue.SelectedIndex = iCur - 1;
                this.RefreshPage();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("切换到前一条记录的操作失败,方法imgBtnPre_Click");
        }
    }
    /// <summary>
    /// 切换到后一条记录的操作
    /// </summary>
    protected void imgBtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            int iCur = this.ddListKeyValue.SelectedIndex;
            if (iCur < this.iRecordCount - 1)
            {
                this.GRIDKEYVALUE = this.ddListKeyValue.Items[iCur + 1].Value;
                this.ddListKeyValue.SelectedIndex = iCur + 1;
                this.RefreshPage();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("切换到后一条记录的操作失败,方法imgBtnNext_Click");
        }
    }
    #endregion

    /// <summary>
    /// 重新加载页面
    /// </summary>
    public void RefreshPage()
    {
        //加载数据前执行 add by sammen 20161015
        this.doBeforeLoadGridDetail();
        this.dtCurRecord = this.GetCurRecord();
        this.MultiPage1.Controls.Clear();
        this.CreateGridDetailCtrls();
        //加载数据后执行 add by sammen 20161015
        this.doAfterLoadGridDetail();
    }

    /// <summary>
    /// 获取当前模板当前分组的所有字段定义
    /// </summary>
    public DataTable GetCurGroupFieldInfo()
    {
        String strSqlD = "SELECT * FROM TB_HRTMPSD WHERE TID='" + this.TID + "' AND SID='" + this.SID + "' AND GID ='" + this.GID + "' AND PRIGHT <2 order by PORDER";
        DataTable dtD = SqlParamDao.GetDataTableBySql(strSqlD);
        return dtD;
    }

    /// <summary>
    /// 获取当前模板当前分组的当前记录的数据信息
    /// </summary>
    public DataTable GetCurRecord()
    {
        String strGridKey = this.GRIDKEY;
        String strGridKeyValue = this.GRIDKEYVALUE;

        String strTypeSql = "select PTYPE from TB_HRTMPD WHERE TID ='" + this.TID + "' AND GID = '" + this.GID + "' AND PID = '" + this.GRIDKEY + "'";
        DataTable dtType = SqlParamDao.GetDataTableBySql(strTypeSql);
        if ((dtType != null) && (dtType.Rows.Count > 0))
        {
            DataRow dr = dtType.Rows[0];
            String strType = dr["PTYPE"].ToString();
            if (!String.IsNullOrEmpty(strType))
            {
                //if (strType.ToLower().Equals("date"))
                //{
                //    strGridKey = "CONVERT(varchar, " + this.GRIDKEY + ",23)";
                //    strGridKeyValue = this.GRIDKEYVALUE.Replace("/", "-").Trim();
                //}else if (strType.ToLower().Equals("datetime"))
                //{
                //    strGridKey = "CONVERT(varchar, " + this.GRIDKEY + ",20)";
                //    strGridKeyValue = this.GRIDKEYVALUE.Replace("/", "-").Trim();
                //}
                this.strGridKeyType = strType;
            }
        }

        String strTableName = this.TID + "_" + this.GID;
        //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
        String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(this.TID, this.SID, this.GID);

        string strSql_record = "select " + strFieldNameString + " from " + strTableName + " where " + this.KEY + "='" + this.KEYVALUE + "' and " + strGridKey + "='" + strGridKeyValue + " '";
        DataTable dtRecord = SqlParamDao.GetDataTableBySql(strSql_record);
        return dtRecord;
    }

    /// <summary>
    /// 创建GRID列表记录明细页面控件
    /// </summary>
    private void CreateGridDetailCtrls()
    {
        Table tb = new Table();
        tb.ID = ServerCtrlIDGetterBll.GetCtrlID_Table(this.TID, this.GID);
        tb.CssClass = "table_archive";
        tb.Width = Unit.Percentage(98);
        //设置Edge浏览器兼容时加上这句
        tb.Style.Add("table-layout", "fixed");

        try
        {
            DataTable dtD = (DataTable)this.dtCurGroupField;
            DataTable dtRecord = (DataTable)this.dtCurRecord;

            Entity_TB_HRTMPSD entityHRTMPSD = new Entity_TB_HRTMPSD();
            if ((dtD != null) && (dtD.Rows.Count > 0))
            {
                for (int i = 0; i < dtD.Rows.Count; i++)
                {
                    DataRow dr = dtD.Rows[i];
                    entityHRTMPSD = SetDataToEntityBll.SetDataToEntity_TB_HRTMPSD(dr);
                    TableRow row = this.CreateTableRow(tb);
                    TableCell cell_lable = this.CreateTableCell(row);
                    this.CreateLabel(cell_lable, entityHRTMPSD, this.Language);
                    cell_lable.Width = Unit.Percentage(30);

                    TableCell cell_control = this.CreateTableCell(row);
                    this.CreateWebControlToCell(cell_control, entityHRTMPSD, dtRecord, this.strGroupType);

                    //将所有富文本控件收集起来再进行处理
                    if (entityHRTMPSD.PCTRL.Value == 19)
                    {
                        MyFreeTextBox freeTemp = (MyFreeTextBox)cell_control.Controls[0];
                        this.arrListMyFreeTextBox.Add(freeTemp);

                    }
                }
            }

            //this.panelFieldArea.Controls.Clear();
            //this.panelFieldArea.Controls.Add(tb);

            PageView pageView = new PageView();
            pageView.ID = ServerCtrlIDGetterBll.GetCtrlID_PageView(this.TID, this.GID);
            pageView.Controls.Add(tb);
            this.MultiPage1.Controls.Add(pageView);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("创建GRID列表记录明细页面控件CreateGridDetailCtrls失败，Table.ID：" + tb.ID);
        }
    }

    #region 按钮点击操作
    /// <summary>
    /// 重新加载
    /// </summary>
    protected void aReloadPage_Click(object sender, EventArgs e)
    {
        this.RefreshPage();
    }
    /// <summary>
    /// 保存并关闭操作
    /// </summary>
    protected void aSaveT_Click(object sender, EventArgs e)
    {
        this.DoSave(true);
    } 
    /// <summary>
    /// 保存后继续操作
    /// </summary>
    protected void aSaveF_Click(object sender, EventArgs e)
    {
        this.DoSave(false);
    }

    /// <summary>
    /// 保存操作
    /// </summary>
    /// <param name="bIsClose"></param>
    private void DoSave(bool bIsClose)
    {
        //在获取table对象
        PageView pageView = (PageView)this.MultiPage1.Controls[0];
        Table tb = (Table)pageView.Controls[0];
        if (tb != null)
        {
            int iTbRows = 0;
            ArrayList arrListObject = new ArrayList();
            String strTableName = this.TID + "_" + this.GID;
            //存储主键名称和主键值
            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add(this.KEY, this.KEYVALUE);
            hsTableKey.Add(this.GRIDKEY, this.GRIDKEYVALUE);

            //table中遍历每一行
            while (iTbRows < tb.Rows.Count)
            {
                TableCell cellTemp = tb.Rows[iTbRows].Cells[1];//取控件列
                Entity_HRLOG_2 entityLog2 = new Entity_HRLOG_2();//日志2对象
                arrListObject = this.GetCtrlValueInCell(cellTemp, this.GID, arrListObject, ref entityLog2);
                if (arrListObject == null)
                {
                    break;
                }
                //准备需写日志的数据
                this.GetDataLogDataInfo(entityLog2);
                iTbRows++;
            }

            String strSql = "";
            int iExcuteDBCount = 0;
            int iExcuteActionReturn = 1;
            String strActionMsg = "";//执行动作后的提示
            try
            {
                if ((arrListObject != null) && (arrListObject.Count > 0))
                {
                    //最后处理自增字段的值(最后处理是为了在验证失败后不获取自增值，以保证自增值的流水号的连续性)
                    ///add by sammen 20140327
                    for (int j = 0; j < arrListObject.Count; j++)
                    {
                        Entity_ToDBObject entityDBObject1 = (Entity_ToDBObject)arrListObject[j];
                        if (entityDBObject1.FIELDTYPE.ToLower().Equals("ints") || (entityDBObject1.FIELDTYPE.ToLower().Equals("intc")))
                        {
                            String strFieldValue = this.GetIncreaceNo(entityDBObject1.GID, entityDBObject1.FIELDNAME, entityDBObject1.FIELDTYPE, entityDBObject1.GROUPTYPE, entityDBObject1.FIELDVALUE_NEW);
                            entityDBObject1.FIELDVALUE_NEW = strFieldValue;
                            arrListObject.RemoveAt(j);
                            arrListObject.Insert(j,entityDBObject1);
                        }
                    }

                    //判断当前页签是进行新增还是修改
                    if ((this.dtCurRecord != null) && (this.dtCurRecord.Rows.Count > 0))
                    {
                        strSql = ParamSqlStringGetterBll.GetUpdateSqlString(strTableName, arrListObject, hsTableKey);
                        iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                        if (iExcuteDBCount > 0)
                        {
                            //编辑保存后执行存储过程 add by sammen 20131120
                            iExcuteActionReturn = this.doAfterEdit();
                            strActionMsg = ArchiveGridActionBll.GetArchiveGridActionTips(this.TID, this.GID, ArchiveGridActionBll.ActionID_AfterEdit, iExcuteActionReturn, this.Language);
                        }
                    }
                    else
                    {

                        //首先判断主键是否重复
                        String strNewGridKeyValue = "";
                        for (int i = 0; i < arrListObject.Count; i++)
                        {
                            Entity_ToDBObject entityDBObject = (Entity_ToDBObject)arrListObject[i];
                            if (entityDBObject.FIELDNAME.Equals(this.GRIDKEY))
                            {
                                strNewGridKeyValue = entityDBObject.FIELDVALUE_NEW;
                                break;
                            }
                        }

                        //新增前执行存储过程 add by sammen 20131120
                        iExcuteActionReturn = this.doBeforeAdd(strNewGridKeyValue);
                        strActionMsg = ArchiveGridActionBll.GetArchiveGridActionTips(this.TID, this.GID, ArchiveGridActionBll.ActionID_BeforeAdd, iExcuteActionReturn, this.Language);
                        if (iExcuteActionReturn == 1)//只有在返回为1时才继续执行
                        {

                            strSql = ParamSqlStringGetterBll.GetInsertSqlString(strTableName, arrListObject);
                            iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                            if (iExcuteDBCount > 0)
                            {
                                //新增后执行存储过程 add by sammen 20131120
                                iExcuteActionReturn = this.doAfterAdd(strNewGridKeyValue);
                                strActionMsg = ArchiveGridActionBll.GetArchiveGridActionTips(this.TID, this.GID, ArchiveGridActionBll.ActionID_AfterAdd, iExcuteActionReturn, this.Language);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("页面ArchiveGridRecordDetail.aspx中保存数据时对数据库操作失败，方法aSave_Click，SQL：" + strSql.ToString());
                base.AlertMessageBox(this, this.strTipSaveFailed);
            }


            if (iExcuteDBCount > 0)
            {
                if (iExcuteActionReturn == 1)
                {
                    if (!String.IsNullOrEmpty(strActionMsg))
                    {
                        this.strTipSaveSuccess = strActionMsg;
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(strActionMsg))
                    {
                        this.strTipSaveSuccess = this.strTipSaveSuccess+" \\n "+strActionMsg;
                    }
                }
                base.AlertMessageBox(this, this.strTipSaveSuccess);
                this.SuccessfullySaved(bIsClose);
            }
            else
            {
                if (!String.IsNullOrEmpty(strActionMsg))
                {
                    this.strTipSaveFailed = strActionMsg;
                }
                base.AlertMessageBox(this, this.strTipSaveFailed);
            }
        }
    }

    /// <summary>
    /// 保存成功操作
    /// </summary>
    /// <param name="bIsClose">是否关闭当前页面</param>
    private void SuccessfullySaved(bool bIsClose)
    {
        //写日志
        this.WriteDataLog(this.arrayObjectHRLOG2,this.OPTYPE);

        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        strB.Append("    if(window.opener!=null){\r\n");
        strB.Append("        if(window.opener.document.getElementById(\"aRefreshDetail\")!=null){\r\n");
        strB.Append("            window.opener.document.getElementById(\"aRefreshDetail\").click();\r\n");
        if (bIsClose)
        {
            strB.Append("            window.close();\r\n");
        }
        else
        {
            String strUrl = base.Request.Url.ToString();
            strB.Append("            window.location.href='" + strUrl + "';\r\n");
            strB.Append("            if(document.getElementById(\"aReloadPage\")!=null){\r\n");
            strB.Append("               document.getElementById(\"aReloadPage\").click();\r\n");
            strB.Append("            }\r\n");
        }
        strB.Append("        }\r\n");
        strB.Append("    }\r\n");
        strB.Append("</script>");

        Page.ClientScript.RegisterStartupScript(typeof(Page), "success", strB.ToString()); 
    }

    /// <summary>
    /// 新增前执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doBeforeAdd(String strNewGridKeyValue)
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("GID", this.GID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("GridKey", this.GRIDKEY);
        hsTableParam.Add("GridKeyValue", strNewGridKeyValue);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ArchiveGridActionBll.DoExcuteSP_BeforeAdd(hsTableParam);
        return iCount;
    }
    /// <summary>
    /// 新增后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterAdd(String strNewGridKeyValue)
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("GID", this.GID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("GridKey", this.GRIDKEY);
        hsTableParam.Add("GridKeyValue", strNewGridKeyValue);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ArchiveGridActionBll.DoExcuteSP_AfterAdd(hsTableParam);
        return iCount;
    }
    /// <summary>
    /// 编辑后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterEdit()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("GID", this.GID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("GridKey", this.GRIDKEY);
        hsTableParam.Add("GridKeyValue", this.GRIDKEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = ArchiveGridActionBll.DoExcuteSP_AfterEdit(hsTableParam);
        return iCount;
    }

    #endregion

    #region 写操作日志区域
    /// <summary>
    /// 准备需写日志的数据
    /// </summary>
    /// <param name="arrListObject"></param>
    private void GetDataLogDataInfo(Entity_HRLOG_2 entityLog2)
    {
        if (!String.IsNullOrEmpty(entityLog2.PID))
        {
            this.arrayObjectHRLOG2.Add(entityLog2);
        }
    }

    /// <summary>
    /// 写入操作日志
    /// </summary>
    /// <param name="arrayObjectHRLOG2">修改记录信息(可为空)</param>
    /// <param name="strOpType">页面操作类型，查看新增删除</param>
    private void WriteDataLog(ArrayList arrayObjectHRLOG2, String strOpType)
    {
        if ((Session["ArchiveIsLog"] != null) && (Session["ArchiveIsLog"].ToString().Equals("1")))
        {
            if ((arrayObjectHRLOG2 != null) && (arrayObjectHRLOG2.Count > 0))
            {
                if (strOpType.Equals("add"))
                {
                    DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Add, this.KEYVALUE, arrayObjectHRLOG2);
                }
                else if (strOpType.Equals("edit"))
                {
                    DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Modify, this.KEYVALUE, arrayObjectHRLOG2);
                }
            }
            else
            {
                if (strOpType.Equals("readonly"))
                {
                    DataLogWriter.Log_Archive(this.GetUserCode(), this.TID, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_View, this.KEYVALUE, arrayObjectHRLOG2);
                }
            }
        }
    }
    #endregion

    #region 动态加载页面中的客户端事件
    /// <summary>
    /// 动态加载页面中的客户端事件
    /// </summary>
    /// <param name="strTid"></param>
    /// <param name="strSid"></param>
    private void BuildClientEventScript(String strTid, String strSid)
    {
        StringBuilder strBuilderContent = new StringBuilder();
        if ((String.IsNullOrEmpty(strTid)) || (String.IsNullOrEmpty(strSid)))
        {
            return;
        }
        this.divClientEvent.InnerHtml = ArchiveEventBll.BuildClientEventScript(strTid, strSid,this.GetBrowserType());
    }

    #endregion

    #region 加载明细页面前后执行存储过程 add by sammen 20161015
    /// <summary>
    /// 加载明细数据前执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doBeforeLoadGridDetail()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("GID", this.GID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("GridKey", this.GRIDKEY);
        hsTableParam.Add("GridKeyValue", this.GRIDKEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = -1;
        try
        {
            iCount = ActionMainActionBll.DoExcuteSP_BeforeLoadGridDetail(hsTableParam);
        }
        catch (Exception ex)
        {
            log.Error(ex+"\r\n");
            log.Error("错误提示：本应用发布源码中不包括存储过程（USP_Archive_BeforeLoadGridDetail）");
        }
        return iCount;
    }

    /// <summary>
    /// 加载明细数据后执行存储过程
    /// </summary>
    /// <returns></returns>
    private int doAfterLoadGridDetail()
    {
        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", this.TID);
        hsTableParam.Add("RID", this.RID);
        hsTableParam.Add("SID", this.SID);
        hsTableParam.Add("GID", this.GID);
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("GridKey", this.GRIDKEY);
        hsTableParam.Add("GridKeyValue", this.GRIDKEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = -1;
        try
        {
            iCount = ActionMainActionBll.DoExcuteSP_AfterLoadGridDetail(hsTableParam);
        }
        catch (Exception ex)
        {
            log.Error(ex + "\r\n");
            log.Error("错误提示：本应用发布源码中不包括存储过程（USP_Archive_AfterLoadGridDetail）");
        }
        return iCount;
    }
    #endregion



}
