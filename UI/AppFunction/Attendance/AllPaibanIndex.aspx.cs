using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using System.Resources;
using Com.ValuePlus.Common.Config;
using System.Text;
using Com.ValuePlus.Common.Security;

public partial class AppFunction_Attendance_AllPaibanIndex : PageBase
{
    //获取配置中的周锁定是否针对全部部门的设置
    private String strIsLockWeekForCurSection = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockWeekForCurSection");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                this.strDeptType = Request.Params["type"]==null?"":Request.Params["type"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strDeptType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strDeptType);

                //this.BuildDivisionList();
                this.BuildUserList();

                this.aVerifyAll.Attributes.Add("onclick", "return confirm('Are you sure to do?') ;");
                this.aFinishAll.Attributes.Add("onclick", "return confirm('Are you sure to do?') ;");
                this.aReturnAll.Attributes.Add("onclick", "return confirm('Are you sure to do?') ;");


                ResourceManager rmLocResourceManager = base.GetResourceManager("AllPaibanIndex");
                this.Label_Dept.Text = rmLocResourceManager.GetString("lbDept");
                this.Label_YearMonth.Text = rmLocResourceManager.GetString("lbYearMonth");
                this.Label_Verify.Text = rmLocResourceManager.GetString("lbVerify");
                this.Label_Finish.Text = rmLocResourceManager.GetString("lbFinish");
                this.Label_Return.Text = rmLocResourceManager.GetString("lbReturn");
                this.Label_VerifyAll.Text = rmLocResourceManager.GetString("lbVerifyAll");
                this.Label_FinishAll.Text = rmLocResourceManager.GetString("lbFinishAll");
                this.Label_ReturnAll.Text = rmLocResourceManager.GetString("lbReturnAll");

                this.Label_User.Text = rmLocResourceManager.GetString("lbAttUser");
                this.Label_ChargeSections.Text = rmLocResourceManager.GetString("lbChargeSection");
                this.lbCloase.Text = rmLocResourceManager.GetString("btnClose");

                //this.aLockPerd.Text = rmLocResourceManager.GetString("btnLockYearMonth");
                this.Label_WeekLock.Text = rmLocResourceManager.GetString("lbWeekLock");

            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("WinForm_AllPaibanIndex.Page_Load error!");
            }

        }
    }

    #region viewstate初始化区域
    private string strDeptType
    {
        get
        {
            return ViewState["strDeptType"] as string;
        }
        set
        {
            ViewState["strDeptType"] = value;
        }
    }
    private string strCurDivision
    {
        get
        {
            return ViewState["strCurDivision"] as string;
        }
        set
        {
            ViewState["strCurDivision"] = value;
        }
    }
    private string strCurDept
    {
        get
        {
            return ViewState["strCurDept"] as string;
        }
        set
        {
            ViewState["strCurDept"] = value;
        }
    }
    private string strCurSection
    {
        get
        {
            return ViewState["strCurSection"] as string;
        }
        set
        {
            ViewState["strCurSection"] = value;
        }
    }
    private string strCurYearMonth
    {
        get
        {
            return ViewState["strCurYearMonth"] as string;
        }
        set
        {
            ViewState["strCurYearMonth"] = value;
        }
    }
    private string strCurYearMonthLockFlag
    {
        get
        {
            return ViewState["strCurYearMonthLockFlag"] as string;
        }
        set
        {
            ViewState["strCurYearMonthLockFlag"] = value;
        }
    }
    private string strCurYearMonthLockDesc
    {
        get
        {
            return ViewState["strCurYearMonthLockDesc"] as string;
        }
        set
        {
            ViewState["strCurYearMonthLockDesc"] = value;
        }
    }
    private string strCurUserId
    {
        get
        {
            return ViewState["strCurUserId"] as string;
        }
        set
        {
            ViewState["strCurUserId"] = value;
        }
    }
    private string strMultiSection
    {
        get
        {
            return ViewState["strMultiSection"] as string;
        }
        set
        {
            ViewState["strMultiSection"] = value;
        }
    }
    #endregion

#region 根据部门选择的，暂时不用

    /// <summary>
    /// 大部门下拉框加载
    /// </summary>
    private void BuildDivisionList()
    {
        String strSql = "SELECT * FROM CSORGA_1 WHERE OTEST = '1'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListDivision.Items.Clear();
        ListItem lItem = new ListItem(">>>>Division", "");
        this.ddListDivision.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCode = dt.Rows[i]["OID"].ToString(); ;
                String strName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strName = dt.Rows[i]["ODESCCHS"].ToString();
                }
                else
                {
                    strName = dt.Rows[i]["ODESC"].ToString();
                }
                lItem = new ListItem(strName, strCode);
                this.ddListDivision.Items.Add(lItem);
            }
        }
    }

    /// <summary>
    /// 部门下拉框加载
    /// </summary>
    private void BuildDeptList(string strDivisionCode)
    {
        String strSql = "SELECT * FROM CSORGA_1 WHERE OPID = '" + strDivisionCode + "'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListDept.Items.Clear();
        ListItem lItem = new ListItem(">>>>Department", "");
        this.ddListDept.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCode = dt.Rows[i]["OID"].ToString(); ;
                String strName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strName = dt.Rows[i]["ODESCCHS"].ToString();
                }
                else
                {
                    strName = dt.Rows[i]["ODESC"].ToString();
                }
                lItem = new ListItem(strName, strCode);
                this.ddListDept.Items.Add(lItem);
            }
        }
    }

    /// <summary>
    /// 小部门下拉框加载
    /// </summary>
    private void BuildSectionList(string strDeptCode)
    {
        String strSql = "SELECT * FROM CSORGA_1 WHERE OPID = '" + strDeptCode + "'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListSection.Items.Clear();
        ListItem lItem = new ListItem(">>>>Section", "");
        this.ddListSection.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCode = dt.Rows[i]["OID"].ToString(); ;
                String strName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strName = dt.Rows[i]["ODESCCHS"].ToString();
                }
                else
                {
                    strName = dt.Rows[i]["ODESC"].ToString();
                }
                lItem = new ListItem(strName, strCode);
                this.ddListSection.Items.Add(lItem);
            }
        }
    }

    /// <summary>
    /// 大部门选择变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListDivision_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.strCurDivision = this.ddListDivision.SelectedValue;
            this.BuildDeptList(this.strCurDivision);

            this.ddListSection.Items.Clear();
            this.ddListYearMonth.Items.Clear();
            this.aVerify.Visible = false;
            this.aFinish.Visible = false;
            this.aReturn.Visible = false;
            this.aVerifyAll.Visible = false;
            this.aFinishAll.Visible = false;
            this.aReturnAll.Visible = false;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("WinForm_AllPaibanIndex.ddListDivision_SelectedIndexChanged error!");
        }
    }

    /// <summary>
    /// 部门选择变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.strCurDept = this.ddListDept.SelectedValue;
            this.BuildSectionList(this.strCurDept);

            this.ddListYearMonth.Items.Clear();
            this.aVerify.Visible = false;
            this.aFinish.Visible = false;
            this.aReturn.Visible = false;
            this.aVerifyAll.Visible = false;
            this.aFinishAll.Visible = false;
            this.aReturnAll.Visible = false;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("WinForm_AllPaibanIndex.ddListDept_SelectedIndexChanged error!");
        }
    }

    /// <summary>
    /// 小部门选择变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.strCurSection = this.ddListSection.SelectedValue;
            //初始化 月份下拉框
            this.BuildYearMonthList("");
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("WinForm_AllPaibanIndex.ddListSection_SelectedIndexChanged error!");
        }
    }
#endregion

    #region 月份下拉框加载
    /// <summary>
    /// 月份下拉框加载
    /// </summary>
    private void BuildYearMonthList(String strDefaultYearMonth)
    {
        DateTime dtNow = DateTime.Today;
        String strSql = "SELECT * FROM KQPERD_1 order by PID";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListYearMonth.Items.Clear();
        ListItem lItem = new ListItem(">>>>YearMonth", "");
        this.ddListYearMonth.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strPID = dt.Rows[i]["PID"].ToString();
                String strPLOCK = dt.Rows[i]["PLOCK"].ToString();
                String strPLOCKShowDesc = strPLOCK == "1" ? "【期间已锁定】" : "";
                if (!base.Language.Equals("zh-cn"))
                {
                    strPLOCKShowDesc = strPLOCK == "1" ? "【Locked】" : "";
                }
                DateTime dtPSTART = DateTime.Parse(dt.Rows[i]["PSTART"].ToString());
                DateTime dtPEND = DateTime.Parse(dt.Rows[i]["PEND"].ToString());
                lItem = new ListItem(strPID+ strPLOCKShowDesc, strPID);
                this.ddListYearMonth.Items.Add(lItem);
                if (strDefaultYearMonth.Equals(""))
                {
                    if ((dtNow >= dtPSTART) && (dtNow <= dtPEND))
                    {
                        this.strCurYearMonth = strPID;
                    }
                }else
                {
                    this.strCurYearMonth = strDefaultYearMonth;
                }
            }
            //获取当前日期并自动选择所在月份
            this.ddListYearMonth.SelectedIndex = this.ddListYearMonth.Items.IndexOf(this.ddListYearMonth.Items.FindByValue(this.strCurYearMonth));
            this.SelectYearMonth(this.strCurYearMonth);

        }

    }
    #endregion
    
    #region 初始化操作按钮
    /// <summary>
    /// 初始化操作按钮
    /// </summary>
    private void BuildOperationBtn(String strMonth,String strSection)
    {
        if (!String.IsNullOrEmpty(strMonth))
        {
            //针对单个部门的按钮控制
            String strSql = "SELECT * FROM KQPERD_4 where PID = '" + strMonth + "' AND SECTIONCODE = '" + strSection + "'";

            //String strSql = "SELECT * FROM KQPERD_4 WHERE PID = '" + strMonth + "' AND SECTIONCODE IN (select SEPNO from KQDL_2 where DCNO = '" + this.strCurUserId + "') ";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) & (dt.Rows.Count > 0))
            {
                DataRow dr = dt.Rows[0];
                String strStatus = dr["KQSTATUS"].ToString();
                switch (strStatus)
                {
                    case "1":
                        this.aVerify.Visible = true;//审核该部门
                        this.aFinish.Visible = false;//该部门审核完毕
                        this.aReturn.Visible = false;//退回该部门
                        break;
                    case "2":
                        this.aVerify.Visible = false;//审核该部门
                        this.aFinish.Visible = true;//该部门审核完毕
                        this.aReturn.Visible = true;//退回该部门
                        break;
                    case "3":
                        this.aVerify.Visible = false;//审核该部门
                        this.aFinish.Visible = false;//该部门审核完毕
                        this.aReturn.Visible = true;//退回该部门
                        break;
                    default:
                        this.aVerify.Visible = false;//审核该部门
                        this.aFinish.Visible = false;//该部门审核完毕
                        this.aReturn.Visible = false;//退回该部门
                        break;
                }
            }
            //针对所有部门的按钮控制
            this.aVerifyAll.Visible = true;//审核所有部门
            this.aFinishAll.Visible = true;//所有审核完毕
            this.aReturnAll.Visible = true;//退回所有部门
            //strSql = "SELECT * FROM KQPERD_4 where PID = '" + strMonth + "'";
            //dt = SqlParamDao.GetDataTableBySql(strSql);
            //if ((dt != null) & (dt.Rows.Count > 0))
            //{
            //}

        }

    }
    #endregion

    #region 系统考勤员用户列表下拉框加载
    /// <summary>
    /// 系统考勤员用户列表下拉框加载
    /// </summary>
    private void BuildUserList()
    {
        //String strSql = "SELECT * FROM GRROTO_3 WHERE GRNO = '00001'";
        String strSql = SqlConfig_wsm.Instance.GetSql_GRROTO_3_selectAtt_byId().CommandSql.ToString() ;
        if (this.strDeptType.ToUpper().Equals("FB"))
        {
            strSql = "SELECT * FROM GRROTO_3 WHERE GRNO = '"+ this.strDeptType + "'";
        }

        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListUser.Items.Clear();
        ListItem lItem = new ListItem(">>>>Attendance Users", "");
        this.ddListUser.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCode = dt.Rows[i]["DCNO"].ToString(); ;
                String strName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strName = strCode + ">>>>" + dt.Rows[i]["DCNAMECHS"].ToString();
                }
                else
                {
                    strName = strCode + ">>>>" + dt.Rows[i]["DCNAME"].ToString();
                }
                lItem = new ListItem(strName, strCode);
                this.ddListUser.Items.Add(lItem);
            }
        }
    }
    #endregion

    #region 用户列表选择变更事件
    protected void ddListUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            this.Label_ChargeSections.Visible = true;
            //获取小部门对应的考勤员用户ID
            this.strCurUserId = this.ddListUser.SelectedValue;
            String strSql = "select * from KQDL_2 A,CSORGA_1 B,KQDL_1 C where A.SEPNO = B.OID AND A.DCNO = C.DCNO AND a.dcno = '" + this.strCurUserId + "' order by a.SEPNO";

            String strDivisionsName = "";
            this.strMultiSection = "";
            this.strCurSection = "";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) & (dt.Rows.Count > 0))
            {
                this.strCurSection = dt.Rows[0]["DCDDESCCHS"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String strSectionId = dt.Rows[i]["SEPNO"].ToString();
                    String strName = "";
                    if (base.Language.Equals("zh-cn"))
                    {
                        strName = dt.Rows[i]["ODESCCHS"].ToString();
                    }
                    else
                    {
                        strName = dt.Rows[i]["ODESC"].ToString();
                    }
                    if (i == 0)
                    {
                        this.strMultiSection = "('"+strSectionId;
                        strDivisionsName = strName;
                    }
                    else
                    {
                        this.strMultiSection = this.strMultiSection + "','" + strSectionId;
                        strDivisionsName = strDivisionsName+"、"+strName;
                    }
                }
                this.strMultiSection = this.strMultiSection + "')";
                this.Label_Sections.Text = strDivisionsName;
            }

            if (!String.IsNullOrEmpty(this.strCurYearMonth))
            {
                this.SelectYearMonth(this.strCurYearMonth);
                //月份周数下拉框加载
                this.BuildMonthlyWeekList();
            }
            else
            {
                //初始化 月份下拉框
                this.BuildYearMonthList("");
                //月份周数下拉框加载
                this.BuildMonthlyWeekList();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("WinForm_AllPaibanIndex.ddListSection_SelectedIndexChanged error!");
        }
    }
    #endregion

    #region 月份下拉框选择变更事件
    protected void ddListYearMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.strCurYearMonth = this.ddListYearMonth.SelectedValue;

        this.SelectYearMonth(this.strCurYearMonth);
        //月份周数下拉框加载
        this.BuildMonthlyWeekList();
    }

    /// <summary>
    /// 选择月份的操作
    /// </summary>
    private void SelectYearMonth(string strMonth)
    {
        this.strCurYearMonthLockFlag = this.ddListYearMonth.SelectedItem.Text.IndexOf((base.Language.Equals("zh-cn")) ? "【期间已锁定】": "【Locked】") > 0 ? "1" : "2";
        this.strCurYearMonthLockDesc = this.ddListYearMonth.SelectedItem.Text.IndexOf((base.Language.Equals("zh-cn")) ? "【期间已锁定】": "【Locked】") > 0 ? "--------->期间已锁定" : "2";
        this.aLockPerd.Visible = true;
        if (this.strCurYearMonthLockFlag.Equals("1"))
        {
            this.Label_WeekLock.Visible = false;
            this.ddListWeekLock.Visible = false;
            //this.tdSetWeekLock.Style.Remove("display");
            //this.tdSetWeekLock.Style.Add("display","none");

            this.Label_Lock.Text = (base.Language.Equals("zh-cn"))?"解锁该期间":"UnLock Period";
        }
        else
        {
            this.Label_WeekLock.Visible = true;
            this.ddListWeekLock.Visible = true;
            //this.tdSetWeekLock.Style.Remove("display");
            this.Label_Lock.Text = (base.Language.Equals("zh-cn"))?"锁定该期间": "Lock Period";
        }
        try
        {
            //初始化操作按钮区域
            this.BuildOperationBtn(this.strCurYearMonth, this.strCurSection);

            if (!String.IsNullOrEmpty(this.strCurSection))
            {
                String strRedirectPage = "../../Winform/KQPaibanFrm.aspx?userId=" + this.strCurUserId + "&userType=1" + "&yearMonth=" + strMonth;
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>document.getElementById('mainFrame').src='" + strRedirectPage + "';document.getElementById('mainFrame').style.height = document.body.offsetHeight;</script>");

            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("WinForm_AllPaibanIndex.SelectYearMonth error!");
        }
    }
    #endregion
    
    #region 月份周数下拉框加载
    /// <summary>
    /// 月份周数下拉框加载
    /// </summary>
    private void BuildMonthlyWeekList()
    {
        DateTime dtNow = DateTime.Today;
        StringBuilder sbSql = new StringBuilder();
        sbSql.Append("SELECT * ");
        if (!strIsLockWeekForCurSection.Equals("1"))
        {
            sbSql.Append(" ,(case when exists (select * from KQPERD_3 where PID = A.PID AND MonthlyWeekNo = A.MonthlyWeekNo and PLOCK <> '1') THEN '2' ELSE '1' END) as PLOCK");
        }
        else
        {
            ////新增针对各个部门分别进行周锁定的功能 add by sammen 20181029
            sbSql.Append(" ,(case when exists (select * from KQPERD_3 where PID = A.PID AND MonthlyWeekNo = A.MonthlyWeekNo and PLOCK = '1' ");
            sbSql.Append(" and CHARINDEX(UPPER('" + this.strCurUserId + ",'),UPPER(isnull(LockedSections,'')))>0) ");
            sbSql.Append(" THEN '1' ELSE '2' END) as PLOCK");
        }
        sbSql.Append(" FROM (select DISTINCT PID,MonthlyWeekNo from KQPERD_3 where PID = '" + this.strCurYearMonth + "' group by PID,MonthlyWeekNo ) A");

        String strSql = sbSql.ToString();
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        this.ddListWeekLock.Items.Clear();

        String strTtileList = ">>>>本期间周数";
        if (!base.Language.Equals("zh-cn")) {
            strTtileList = ">>>>The Weeks";
        }
        ListItem lItem = new ListItem(strTtileList, "");
        this.ddListWeekLock.Items.Add(lItem);
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strPID = dt.Rows[i]["PID"].ToString();
                String strMonthlyWeekNo = dt.Rows[i]["MonthlyWeekNo"].ToString();
                String strPLOCK = dt.Rows[i]["PLOCK"].ToString();
                
                if (base.Language.Equals("zh-cn"))
                {
                    String strPLOCKShowDesc = strPLOCK == "1" ? "【点击可解锁】" : "【点击可锁定本周】";

                    lItem = new ListItem("本期间第" + strMonthlyWeekNo + "周" + strPLOCKShowDesc, strPID + "-" + strMonthlyWeekNo + "-" + strPLOCK);
                    this.ddListWeekLock.Items.Add(lItem);
                }
                else
                {
                    String strPLOCKShowDesc = strPLOCK == "1" ? "【Clike To Unlock】" : "【Clike To Lock Week】";

                    lItem = new ListItem("The Week " + strMonthlyWeekNo + " Of this Period " + strPLOCKShowDesc, strPID + "-" + strMonthlyWeekNo + "-" + strPLOCK);
                    this.ddListWeekLock.Items.Add(lItem);
                }
            }
        }

    }
    #endregion

    #region 月份周数下拉框选择变更事件
    /// <summary>
    /// 月份周数下拉框选择变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListWeekLock_SelectedIndexChanged(object sender, EventArgs e)
    {
        String strSectionCodeString = this.strCurUserId + ",";
        String strSelectedValue = this.ddListWeekLock.SelectedValue;
        String strYearMonth = strSelectedValue.Split('-')[0];
        String strMonthlyWeekNo = strSelectedValue.Split('-')[1];
        String strCurLockFlag = strSelectedValue.Split('-')[2];
        String strSetToLockFlag = strCurLockFlag == "1" ? "2" : "1";
        StringBuilder sbSql = new StringBuilder();
        String strSql = "";

        if (!strIsLockWeekForCurSection.Equals("1"))
        {
            //针对所有部门一起
            sbSql.Append("update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + strYearMonth + "' AND MonthlyWeekNo = '" + strMonthlyWeekNo + "'");
            strSql = sbSql.ToString();
        }
        else
        {
            ////新增针对各个部门分别进行周锁定的功能 add by sammen 20181029
            if (strSetToLockFlag.Equals("1"))
            {
                sbSql.Append("update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "'");
                //字段新增锁定的部门代码集合
                sbSql.Append(" ,LockedSections = replace(isnull(LockedSections,''),'" + strSectionCodeString + "','') + '" + strSectionCodeString + "'");
            }
            else
            {
                sbSql.Append("update KQPERD_3 set PLOCK = PLOCK");
                //字段删除锁定的部门代码集合
                sbSql.Append(" ,LockedSections = replace(isnull(LockedSections,''),'" + strSectionCodeString + "','')");
            }
            sbSql.Append("  WHERE PID = '" + strYearMonth + "' AND MonthlyWeekNo = '" + strMonthlyWeekNo + "'");
            strSql = sbSql.ToString();

        }
        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            this.SelectYearMonth(this.strCurYearMonth);
            //月份周数下拉框加载
            this.BuildMonthlyWeekList();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("排班界面针对月度周的锁定解锁操作失败！SQL"+ strSql);
        }
    }
    #endregion

    /// <summary>
    /// 绑定期间的锁定和解锁操作
    /// add by sammen 20171204
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void aLockPerd_Click(object sender, EventArgs e)
    {
        String strSetToLockFlag = this.strCurYearMonthLockFlag == "1" ? "2" : "1";
        
        String strSql = "update KQPERD_1 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + this.strCurYearMonth + "' ";
        strSql = strSql + ";update KQPERD_3 set PLOCK = '" + strSetToLockFlag + "' WHERE PID = '" + this.strCurYearMonth + "'";

        try
        {
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            //初始化 月份下拉框
            this.BuildYearMonthList(this.strCurYearMonth);
            //月份周数下拉框加载
            this.BuildMonthlyWeekList();

        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("绑定期间的锁定和解锁操作操作失败！SQL" + strSql);
        }
    }

    #region 按钮操作事件
    /// <summary>
    /// 按钮操作事件
    /// </summary>
    private void BtnOperation(String strMonth, String strSection,String strOpType)
    {
        if (!String.IsNullOrEmpty(strMonth))
        {
            //针对单个部门的按钮控制
            String strSql = "";
            switch (strOpType)
            {
                case "verify":
                    strSql = "update KQPERD_4 set KQSTATUS = '2' WHERE PID = '" + strMonth + "' AND SECTIONCODE in " + strSection;
                    break;
                case "finish":
                    strSql = "update KQPERD_4 set KQSTATUS = '3' WHERE PID = '" + strMonth + "' AND SECTIONCODE in " + strSection;
                    break;
                case "return":
                    strSql = "update KQPERD_4 set KQSTATUS = '1' WHERE PID = '" + strMonth + "' AND SECTIONCODE in " + strSection;
                    break;
                case "verifyAll":
                    strSql = "update KQPERD_4 set KQSTATUS = '2' WHERE PID = '" + strMonth + "'";
                    break;
                case "finishAll":
                    strSql = "update KQPERD_4 set KQSTATUS = '3' WHERE PID = '" + strMonth + "'";
                    break;
                case "returnAll":
                    strSql = "update KQPERD_4 set KQSTATUS = '1' WHERE PID = '" + strMonth + "'";
                    break;
            }
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                this.SelectYearMonth(this.strCurYearMonth);
            }

        }

    }

    protected void aVerify_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth,this.strMultiSection,"verify");
    }
    protected void aFinish_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth, this.strMultiSection, "finish");
    }
    protected void aReturn_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth, this.strMultiSection, "return");
    }
    protected void aVerifyAll_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth, this.strMultiSection, "verifyAll");
    }
    protected void aFinishAll_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth, this.strMultiSection, "finishAll");
    }
    protected void aReturnAll_Click(object sender, EventArgs e)
    {
        this.BtnOperation(this.strCurYearMonth, this.strMultiSection, "returnAll");
    }

    #endregion

}
