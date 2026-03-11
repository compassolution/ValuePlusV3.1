using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Com.ValuePlus.Web;
using System.Resources;
using Com.ValuePlus.Common;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.Entity;
using Microsoft.Web.UI.WebControls;
using Com.ValuePlus.Archive.BLL;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog;
using System.Text;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.Archive.WebCtrls;
using Com.ValuePlus.Archive.Tree;

public partial class Tree_TreeDetail : ArchivePageBase
{
    private ArrayList arrayObjectHRLOG2 = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.strTid = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "TREE");
                this.strCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "CODE");
                this.strParentCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "PCODE");
                this.OPTYPE = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "OPTYPE");//操作类型（add新增，edit编辑，readonly只读）

                this.TID = this.strTid;
                this.GID = "0";
                this.KEY = "TREECODE";
                this.KEYVALUE = this.strCode;

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
                if ((!String.IsNullOrEmpty(this.strTid))  && (!String.IsNullOrEmpty(this.OPTYPE)))
                {
                    this.strTableName = "TREE_" + this.strTid;

                    //获取父节点级别
                    this.iParentLevel = 0;//默认值
                    String strSql1 = "select TREELEVEL from " + this.strTableName + " WHERE TREECODE = '" + this.strParentCode + "'";
                    DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
                    if ((dt1 != null) && (dt1.Rows.Count > 0))
                    {
                        this.iParentLevel = int.Parse(dt1.Rows[0]["TREELEVEL"].ToString());
                    }
                    //如果超过了定义中的最高级别，则不提供新增功能
                    if ((this.iParentLevel + 1 >= this.iMaxLevel) && (String.IsNullOrEmpty(this.strCode)))
                    {
                        this.aSave.Enabled = false;
                    }
                        

                    if (this.OPTYPE.ToLower().Equals("readonly"))
                    {
                        this.aSave.Visible = false;
                    }

                    this.dtCurGroupField = this.GetCurFieldInfo();
                    this.dtCurRecord = this.GetCurRecord();
                    this.BuildKeyValueDDList();
                    this.CreateGridDetailCtrls();
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
    private string strTid
    {
        get
        {
            return ViewState["TID_ViewState"] as string;
        }
        set
        {
            ViewState["TID_ViewState"] = value;
        }
    }
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
    private string strPREFIX
    {
        get
        {
            return ViewState["strPREFIX_ViewState"] as string;
        }
        set
        {
            ViewState["strPREFIX_ViewState"] = value;
        }
    }
    private string strIsAutoCode
    {
        get
        {
            return ViewState["strIsAutoCode_ViewState"] as string;
        }
        set
        {
            ViewState["strIsAutoCode_ViewState"] = value;
        }
    }
    private int iCodeLevelLength
    {
        get
        {
            if (this.ViewState["iCodeLevelLength_ViewState"] != null)
            {
                return (int)this.ViewState["iCodeLevelLength_ViewState"];
            }
            return 3;
            
        }
        set
        {
            this.ViewState["iCodeLevelLength_ViewState"] = value;
        }
    }
    private int iMaxLevel
    {
        get
        {
            if (this.ViewState["iMaxLevel_ViewState"] != null)
            {
                return (int)this.ViewState["iMaxLevel_ViewState"];
            }
            return 3;

        }
        set
        {
            this.ViewState["iMaxLevel_ViewState"] = value;
        }
    }

    private string strCode
    {
        get
        {
            return ViewState["CODE_ViewState"] as string;
        }
        set
        {
            ViewState["CODE_ViewState"] = value;
        }
    }
    private string strParentCode
    {
        get
        {
            return ViewState["PARENTCODE_ViewState"] as string;
        }
        set
        {
            ViewState["PARENTCODE_ViewState"] = value;
        }
    }
    private string strOpType
    {
        get
        {
            return ViewState["opType_ViewState"] as string;
        }
        set
        {
            ViewState["opType_ViewState"] = value;
        }
    }
    private int iParentLevel
    {
        get
        {
            if (this.ViewState["iParentLevel"] != null)
            {
                return (int)this.ViewState["iParentLevel"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iParentLevel"] = value;
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

    private string strTipExsitSameKeyValue
    {
        get
        {
            return ViewState["strTipExsitSameKeyValue"] as string;
        }
        set
        {
            ViewState["strTipExsitSameKeyValue"] = value;
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
        this.aClose.Text = rmLocResourceManager.GetString("aClose");
        this.aSave.Text = rmLocResourceManager.GetString("aSave");

        this.strTipExsitSameKeyValue = rmLocResourceManager.GetString("tipExsitSameKeyValue");
        this.strTipUrlLoadError = rmLocResourceManager.GetString("tipUrlLoadError");
        this.strTipPageLoadError = rmLocResourceManager.GetString("tipPageLoadError");
        this.strTipSaveFailed = rmLocResourceManager.GetString("tipSaveFailed");
        this.strTipSaveSuccess = rmLocResourceManager.GetString("tipSaveSuccess");
        //页面标题设置
        this.Page.Title = rmLocResourceManager.GetString("lbGridTitle");
        String strSql = "SELECT * FROM TB_HRTREEH WHERE TID = '" + this.strTid + "' ";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            if (this.Language.Equals("zh-cn"))
            {
                this.Page.Title = dt.Rows[0]["TDESCCHS"].ToString() + ":" + this.strCode;
            }
            else
            {
                this.Page.Title = dt.Rows[0]["TDESC"].ToString() + ":" + this.strCode;
            }
            this.strPREFIX = dt.Rows[0]["TDESC"] == null ? "" : dt.Rows[0]["TDESC"].ToString();//编号前缀
            this.strIsAutoCode = dt.Rows[0]["TISAUTO"] == null ? "1" : dt.Rows[0]["TISAUTO"].ToString();//编号是否自动编号
            this.iCodeLevelLength = String.IsNullOrEmpty(dt.Rows[0]["TLEN"].ToString()) ? 3 : int.Parse(dt.Rows[0]["TLEN"].ToString());//每级编号长度
            this.iMaxLevel = String.IsNullOrEmpty(dt.Rows[0]["TLEVEL"].ToString())? 3 : int.Parse(dt.Rows[0]["TLEVEL"].ToString());//最高级别（级别从0开始）
        }

    }
    #endregion

    #region 所有记录主键值下拉列表操作区域
    /// <summary>
    /// 加载当前模板当前分组具有的所有记录主键值下拉列表
    /// </summary>
    /// <param name="strCurSceneSql"></param>
    private void BuildKeyValueDDList()
    {
        try
        {
            string strSql = "select TREECODE from " + this.strTableName + " order by TREEORDER";
            if (!String.IsNullOrEmpty(this.strParentCode))
            {
                strSql = "select TREECODE from " + this.strTableName + " where PARENTCODE = '" + this.strParentCode + "' order by TREEORDER";
            }

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.ddListKeyValue.Items.Clear();
                this.ddListKeyValue.DataSource = dt.DefaultView;
                this.ddListKeyValue.DataTextField = this.KEY;
                this.ddListKeyValue.DataValueField = this.KEY;
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
                if (!String.IsNullOrEmpty(this.KEYVALUE))
                {
                    this.ddListKeyValue.SelectedIndex = this.ddListKeyValue.Items.IndexOf(this.ddListKeyValue.Items.FindByValue(this.KEYVALUE));
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
    /// 主键值下拉框选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListKeyValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.KEYVALUE = this.ddListKeyValue.SelectedValue;
        this.strCode = this.ddListKeyValue.SelectedValue;
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
                this.KEYVALUE = this.ddListKeyValue.Items[iCur - 1].Value;
                this.strCode = this.ddListKeyValue.Items[iCur - 1].Value;
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
                this.KEYVALUE = this.ddListKeyValue.Items[iCur + 1].Value;
                this.strCode = this.ddListKeyValue.Items[iCur + 1].Value;
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
        this.dtCurRecord = this.GetCurRecord();
        this.MultiPage1.Controls.Clear();
        this.CreateGridDetailCtrls();
    }

    /// <summary>
    /// 获取当前模板当前分组的所有字段定义
    /// </summary>
    public DataTable GetCurFieldInfo()
    {
        String strSqlD = "SELECT * FROM TB_HRTREED WHERE TID='" + this.strTid + "' and PRIGHT IN ('0','1') order by PORDER";
        DataTable dtD = SqlParamDao.GetDataTableBySql(strSqlD);
        return dtD;
    }

    /// <summary>
    /// 获取当前模板当前分组的当前记录的数据信息
    /// </summary>
    public DataTable GetCurRecord()
    {
        String strTypeSql = "select PTYPE from TB_HRTREED WHERE TID ='" + this.strTid + "' AND PID = '" + this.KEY + "'";
        DataTable dtType = SqlParamDao.GetDataTableBySql(strTypeSql);
        if ((dtType != null) && (dtType.Rows.Count > 0))
        {
            DataRow dr = dtType.Rows[0];
            String strType = dr["PTYPE"].ToString();
            if (!String.IsNullOrEmpty(strType))
            {
                //if (strType.ToLower().Equals("date"))
                //{
                //    strGridKey = "CONVERT(varchar, " + this.KEY + ",23)";
                //    strGridKeyValue = this.KEYVALUE.Replace("/", "-").Trim();
                //}else if (strType.ToLower().Equals("datetime"))
                //{
                //    strGridKey = "CONVERT(varchar, " + this.KEY + ",20)";
                //    strGridKeyValue = this.KEYVALUE.Replace("/", "-").Trim();
                //}
                this.strGridKeyType = strType;
            }
        }

        string strSql_record = "select * from " + this.strTableName + " where TREECODE ='" + this.strCode + " '";
        DataTable dtRecord = SqlParamDao.GetDataTableBySql(strSql_record);
        return dtRecord;
    }

    /// <summary>
    /// 创建GRID列表记录明细页面控件
    /// </summary>
    private void CreateGridDetailCtrls()
    {
        Table tb = new Table();
        tb.ID = ServerCtrlIDGetterBll.GetCtrlID_Table(this.strTid, this.GID);
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
                    entityHRTMPSD = this.SetDataToEntity_TB_HRTMPSD(dr);
                    TableRow row = this.CreateTableRow(tb);
                    TableCell cell_lable = this.CreateTableCell(row);
                    this.CreateLabel(cell_lable, entityHRTMPSD, this.Language);
                    cell_lable.Width = Unit.Percentage(30);

                    TableCell cell_control = this.CreateTableCell(row);
                    //如果当前编码为根节点ROOT则不生成父节点控件
                    if (!((entityHRTMPSD.PID.Equals("PARENTCODE")) && (this.strCode.ToLower().Equals("root"))))
                    {
                        this.CreateWebControlToCell(cell_control, entityHRTMPSD, dtRecord, "0");

                        #region 如果是新增，则默认一些默认值
                    if ((String.IsNullOrEmpty(this.strCode)) && (!String.IsNullOrEmpty(this.strParentCode)))
                    {
                        String strAutoCode = "";
                        if (this.strIsAutoCode.Equals("1"))
                        {
                            strAutoCode = this.GetAutoCode(this.strTid, this.strParentCode);
                        }

                        //编码
                        if (entityHRTMPSD.PID.Equals("TREECODE"))
                        {
                            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                            GeneralTextBox txtBox = (GeneralTextBox)cell_control.Controls[0];
                            
                            //如果是新增时，且为自动编号，则自动获取编号
                            if (this.strIsAutoCode.Equals("1"))
                            {
                                txtBox.SetCtrlReadOnly(true);
                                txtBox.Text = strAutoCode;
                            }
                            else
                            {
                                txtBox.Text = this.strParentCode;
                            }
                        }
                        //上级编码
                        else if (entityHRTMPSD.PID.Equals("PARENTCODE"))
                        {
                            try
                            {//首先支持下拉框
                                String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DropDownList(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                                GeneralDropDownList txtBox = (GeneralDropDownList)cell_control.Controls[0];
                                txtBox.SelectedValue = this.strParentCode;
                                txtBox.SetCtrlReadOnly(true);
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    //如果是不是下拉框，则支持文本框
                                    String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                                    GeneralTextBox txtBox = (GeneralTextBox)cell_control.Controls[0];
                                    txtBox.Text = this.strParentCode;
                                    txtBox.SetCtrlReadOnly(true);
                                }
                                catch (Exception e)
                                {
                                    //最后支持数据选择文本框
                                    String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_DBTextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                                    DBTextBox txtBox = (DBTextBox)cell_control.Controls[0];
                                    txtBox.Text = this.strParentCode;
                                    txtBox.SetCtrlReadOnly(true);
                                }
                            }
                        }
                        //级别
                        else if (entityHRTMPSD.PID.Equals("TREELEVEL"))
                        {
                            //获取对应级别
                            int iLevel = this.iParentLevel+1;
                            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                            GeneralTextBox txtBox = (GeneralTextBox)cell_control.Controls[0];
                            txtBox.Text = iLevel.ToString();
                            txtBox.SetCtrlReadOnly(true);

                        }
                        //排序号
                        else if (entityHRTMPSD.PID.Equals("TREEORDER"))
                        {
                            ////获取顺序号
                            //int iOrder = 1;
                            //String strSql1 = "select MAX(TREEORDER) AS TREEORDER from " + this.strTableName + " WHERE PARENTCODE = '" + this.strParentCode + "'";
                            //DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
                            //if ((dt1 != null) && (dt1.Rows.Count > 0))
                            //{
                            //    if (!String.IsNullOrEmpty(dt1.Rows[0]["TREEORDER"].ToString()))
                            //    {
                            //        iOrder = int.Parse(dt1.Rows[0]["TREEORDER"].ToString()) + 1;
                            //    }
                            //}
                            String strCtrlId = ServerCtrlIDGetterBll.GetCtrlID_TextBox(entityHRTMPSD.TID, entityHRTMPSD.GID, entityHRTMPSD.PID);
                            GeneralTextBox txtBox = (GeneralTextBox)cell_control.Controls[0];

                            if (this.strIsAutoCode.Equals("1"))
                            {
                                txtBox.Text = strAutoCode;
                            }
                        }


                    }

                    #endregion
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

    private void DoSpecialDealAfterCreateCtrls()
    {
        
    }

    #region 按钮点击操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void aSave_Click(object sender, EventArgs e)
    {
        //在获取table对象
        PageView pageView = (PageView)this.MultiPage1.Controls[0];
        Table tb = (Table)pageView.Controls[0];
        if (tb != null)
        {
            int iTbRows = 0;
            ArrayList arrListObject = new ArrayList();
            //存储主键名称和主键值
            Hashtable hsTableKey = new Hashtable();
            hsTableKey.Add(this.KEY, this.KEYVALUE);

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
                    //判断当前页签是进行新增还是修改
                    if ((this.dtCurRecord != null) && (this.dtCurRecord.Rows.Count > 0))
                    {
                        strSql = ParamSqlStringGetterBll.GetUpdateSqlString(this.strTableName, arrListObject, hsTableKey);
                        iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                        if (iExcuteDBCount > 0)
                        {
                            //编辑保存后执行存储过程
                            iExcuteActionReturn = this.doAfterEdit();
                            strActionMsg = TreeActionBll.GetTreeActionTips(this.TID, TreeActionBll.ActionID_AfterEdit, iExcuteActionReturn, this.Language);
                        }
                    }
                    else
                    {
                        //首先判断主键是否重复
                        String strNewTreeCode = "";
                        for (int i = 0; i < arrListObject.Count; i++)
                        {
                            Entity_ToDBObject entityDBObject = (Entity_ToDBObject)arrListObject[i];
                            if (entityDBObject.FIELDNAME.Equals("TREECODE"))
                            {
                                /////如果是新增时，且为自动编号，则自动获取编号
                                //if (this.strIsAutoCode.Equals("1"))
                                //{
                                //    entityDBObject.FIELDVALUE_NEW = this.GetAutoCode(this.strTid, this.strParentCode);
                                    
                                //    arrListObject.RemoveAt(i);
                                //    arrListObject.Insert(i, entityDBObject);
                                //}
                                strNewTreeCode = entityDBObject.FIELDVALUE_NEW;
                                break;
                            }
                        }
                        this.KEYVALUE = strNewTreeCode;
                        String strSqlJudge = "select * from " + this.strTableName + " where TREECODE = '" + strNewTreeCode+"'";
                        DataTable dt = SqlParamDao.GetDataTableBySql(strSqlJudge);
                        if ((dt != null) && (dt.Rows.Count > 0))
                        {
                            base.AlertMessageBox(this, this.strTipExsitSameKeyValue + "：" + strNewTreeCode);
                            return;
                        }
                        //新增前执行存储过程
                        iExcuteActionReturn = this.doBeforeAdd(strNewTreeCode);
                        strActionMsg = TreeActionBll.GetTreeActionTips(this.TID, TreeActionBll.ActionID_BeforeAdd, iExcuteActionReturn, this.Language);
                        if (iExcuteActionReturn == 1)//只有在返回为1时才继续执行
                        {
                            strSql = ParamSqlStringGetterBll.GetInsertSqlString(this.strTableName, arrListObject);
                            iExcuteDBCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                            if (iExcuteDBCount > 0)
                            {
                                //新增后执行存储过程 
                                iExcuteActionReturn = this.doAfterAdd(strNewTreeCode);
                                strActionMsg = TreeActionBll.GetTreeActionTips(this.TID, TreeActionBll.ActionID_AfterAdd, iExcuteActionReturn, this.Language);
                            }
                        }
                        else
                        {
                            log.Error("页面Tree_TreeDetail.aspx中新增前执行动作失败，树：" + this.strTid + "(TREECODE = '" + strNewTreeCode + "')；返回值:" + iExcuteActionReturn.ToString() + ";返回提示：" + strActionMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("页面TreeDetail.aspx中保存数据时对数据库操作失败，方法aSave_Click，SQL：" + strSql.ToString());
                if (!String.IsNullOrEmpty(strActionMsg))
                {
                    this.strTipSaveFailed = strActionMsg;
                }
                base.AlertMessageBox(this, this.strTipSaveFailed);
            }


            if (iExcuteDBCount > 0)
            {
                if (!String.IsNullOrEmpty(strActionMsg))
                {
                    this.strTipSaveSuccess = strActionMsg;
                }
                base.AlertMessageBox(this, this.strTipSaveSuccess);
                this.SuccessfullySaved();
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
    private void SuccessfullySaved()
    {
        //写日志
        this.WriteDataLog(this.arrayObjectHRLOG2,this.OPTYPE);

        StringBuilder strB = new StringBuilder();
        strB.Append("<script language=javascript>\r\n");
        //strB.Append("function SuccessSaved(){\r\n");
        strB.Append("    RefreshOpener();\r\n");
        //strB.Append("}\r\n");
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
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = TreeActionBll.DoExcuteSP_BeforeAdd(hsTableParam);
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
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = TreeActionBll.DoExcuteSP_AfterAdd(hsTableParam);
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
        hsTableParam.Add("Key", this.KEY);
        hsTableParam.Add("KeyValue", this.KEYVALUE);
        hsTableParam.Add("UserId", this.GetUserCode());

        int iCount = TreeActionBll.DoExcuteSP_AfterEdit(hsTableParam);
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

    /// <summary>
    /// 数据行DataRow填充到TB_HRTMPSD实体变量中返回实体对象
    /// </summary>
    /// <param name=dr></param>
    /// <returns>Entity_TB_HRTMPSD</returns>
    private Entity_TB_HRTMPSD SetDataToEntity_TB_HRTMPSD(DataRow dr)
    {
        Entity_TB_HRTMPSD entity = new Entity_TB_HRTMPSD();
        if (dr != null)
        {
            entity.TID = dr["TID"].ToString();
            //entity.SID = dr["SID"].ToString();
            entity.GID = "1";
            entity.PID = dr["PID"].ToString();
            entity.PDESC = dr["PDESC"].ToString();
            entity.PDESCCHS = dr["PDESCCHS"].ToString();
            entity.PTYPE = dr["PTYPE"].ToString();
            entity.PLEN = dr["PLEN"] == DBNull.Value ? 0 : int.Parse(dr["PLEN"].ToString());
            entity.PPREC = dr["PPREC"] == DBNull.Value ? 0 : int.Parse(dr["PPREC"].ToString());
            entity.PNULL = dr["PNULL"] == DBNull.Value ? 0 : int.Parse(dr["PNULL"].ToString());
            entity.PDEFAULT = dr["PDEFAULT"].ToString();
            entity.PISKEY = dr["PISKEY"] == DBNull.Value ? 0 : int.Parse(dr["PISKEY"].ToString());
            entity.PCTRL = dr["PCTRL"] == DBNull.Value ? 0 : int.Parse(dr["PCTRL"].ToString());
            entity.PCTRLID = dr["PCTRLID"].ToString();
            entity.PCTRLD = dr["PCTRLD"].ToString();
            entity.PORDER = dr["PORDER"] == DBNull.Value ? 0 : int.Parse(dr["PORDER"].ToString());
            entity.PRIGHT = dr["PRIGHT"] == DBNull.Value ? 0 : int.Parse(dr["PRIGHT"].ToString());
            entity.PSYS = dr["PSYS"] == DBNull.Value ? 0 : int.Parse(dr["PSYS"].ToString());
            entity.PLIST = dr["PLIST"] == DBNull.Value ? 0 : int.Parse(dr["PLIST"].ToString());
            entity.PWIDTH = dr["PWIDTH"] == DBNull.Value ? 0 : int.Parse(dr["PWIDTH"].ToString());
            //entity.PFONTL = dr["PFONTL"].ToString();
            //entity.PFONTC = dr["PFONTC"].ToString();
            //entity.PAGGR = dr["PAGGR"].ToString();
            //entity.PAGDEST = dr["PAGDEST"].ToString();
            entity.PMAST = dr["PMAST"].ToString();
            //entity.PSAVE = dr["PSAVE"] == DBNull.Value ? 0 : int.Parse(dr["PSAVE"].ToString());
        }
        return entity;
    }

    /// <summary>
    /// 根据TID和父节点ID获取自动获取自增编码
    /// </summary>
    /// <param name="strTid"></param>
    /// <param name="strParentId"></param>
    /// <returns></returns>
    private String GetAutoCode(String strTid,String strParentId)
    {
        String strAutoCode = "";
        String strDTString = DateTime.Now.ToString();        
        try
        {
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("TID", strTid);
            hsTableParam.Add("PARENTCODE", strParentId);
            hsTableParam.Add("USERCODE", this.GetUserCode());
            hsTableParam.Add("dtString", strDTString);
            int iCount = SqlParamDao.ExcuteSP("USP_Sys_Tree_GetAutoCode", hsTableParam);
            try
            {
                String StrSql = "select * FROM TB_TREE_AUTOCODE WHERE TID = '" + this.TID + "' AND SUSERID = '" + this.GetUserCode() + "' AND SYSTIME = '" + strDTString + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(StrSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    strAutoCode = dt.Rows[0]["AUTOCODE"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("页面TreeDetail.aspx中获取自动编号时出错，请检查数据库存储过程USP_Sys_Tree_GetAutoCode是否在本次获取时写入了表TB_TREE_AUTOCODE中");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("页面TreeDetail.aspx中获取自动编号时出错，请检查是否存在获取树形自动编号的数据库存储过程：USP_Sys_Tree_GetAutoCode");
            strAutoCode = "Error" + strDTString;
        }

        return strAutoCode;
    }

}
