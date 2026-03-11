using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Threading;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.DAL;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common.Security;

public partial class UserManager_UserDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strCurUserId = Request.Params["userId"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strCurUserId = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strCurUserId);

            ResourceManager rmLocResourceManager = base.GetResourceManager("UserManager");
            String strErr1 = rmLocResourceManager.GetString("Err1");
            String strTip_modify_Confirm = rmLocResourceManager.GetString("Tip_confirmModify");
            String strTip_del_Confirm = rmLocResourceManager.GetString("Tip2");


            this.hfIsRequiredMatchStaffNo.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsRequired_UserMatchStaffNo").ToString();
            if (String.IsNullOrEmpty(this.hfIsRequiredMatchStaffNo.Value))
            {
                this.hfIsRequiredMatchStaffNo.Value = "0";
            }

            //自定义设置页面文字显示的中英文字符串
            this.Label1.Text = rmLocResourceManager.GetString("lbTitle1");
            this.Label2.Text = rmLocResourceManager.GetString("lbDept");
            this.Label3.Text = rmLocResourceManager.GetString("lbDesc1");
            this.Label4.Text = rmLocResourceManager.GetString("lbDesc2");
            this.Label5.Text = rmLocResourceManager.GetString("lbDesc3");
            this.Label6.Text = rmLocResourceManager.GetString("lbDesc4");
            this.Label7.Text = rmLocResourceManager.GetString("lbDesc5");
            this.Label8.Text = rmLocResourceManager.GetString("lbDesc6");
            this.Label9.Text = rmLocResourceManager.GetString("lbDesc7");
            this.Label10.Text = rmLocResourceManager.GetString("lbDesc8");
            this.Label11.Text = rmLocResourceManager.GetString("lbDesc9");
            this.Label12.Text = rmLocResourceManager.GetString("lbDesc10");
            this.Label13.Text = rmLocResourceManager.GetString("lbDesc11");
            this.Label14.Text = rmLocResourceManager.GetString("lbDesc12");
            this.Label15.Text = rmLocResourceManager.GetString("lbDesc13");
            //下拉框中英文设置
            this.DropDownList1.Items[0].Text = rmLocResourceManager.GetString("IsAlertItem1");
            this.DropDownList1.Items[1].Text = rmLocResourceManager.GetString("IsAlertItem2");
            this.DropDownList2.Items[0].Text = rmLocResourceManager.GetString("IsGroupItem1");
            this.DropDownList2.Items[1].Text = rmLocResourceManager.GetString("IsGroupItem2");
            this.DropDownList3.Items[0].Text = rmLocResourceManager.GetString("IsStopItem1");
            this.DropDownList3.Items[1].Text = rmLocResourceManager.GetString("IsStopItem2");
            //按钮中英文设置
            this.BtnSave.Text = rmLocResourceManager.GetString("btnSave");
            this.BtnAdd.Text = rmLocResourceManager.GetString("btnAdd");
            this.BtnDelete.Text = rmLocResourceManager.GetString("btnDelete");

            this.strErr2 = rmLocResourceManager.GetString("Err2");
            this.strTip3 = rmLocResourceManager.GetString("Tip3");
            this.strTip5 = rmLocResourceManager.GetString("Tip5");
            this.strSuccessTip = rmLocResourceManager.GetString("Success1");

            this.BtnSave.Attributes.Remove("onclick");
            this.BtnSave.Attributes.Add("onclick", "return window.confirm( '" + strTip_modify_Confirm + " '); ");
            this.BtnDelete.Attributes.Remove("onclick");
            this.BtnDelete.Attributes.Add("onclick", "return window.confirm( '" + strTip_del_Confirm + "'); ");
            //加载部门下拉框
            this.SetDeptDDList();
            //加载员工编号下拉框
            this.SetDcnoDDList("");
            //加载用户类别列表
            this.SetUserTypeDDList();
            //加载用户数据
            this.SetUserInfo();

        }

    }


    /// <summary>
    /// 加载用户数据
    /// </summary>
    /// 
    private void SetUserInfo()
    {
        if ((this.strCurUserId != null) && (!this.strCurUserId.Equals("")))//修改当前用户信息
        {
            ViewState["opKey"] = "modify";//页面修改

            try
            {
                //加载某一特定用户信息
                this.BindUserInfo(true, this.strCurUserId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('加载某一特定用户信息错误？');</script>");
            }
        }
        else//新增用户信息
        {
            ViewState["opKey"] = "add";//页面新增
            //String strGUID = Guid.NewGuid().ToString().ToUpper();
            //this.TextBox1.Text = strGUID;
            this.TextBox2.Attributes.Add("onkeyup", "setUserId();");
            this.TextBox2.ReadOnly = false;
            //this.TextBox3.TextMode = TextBoxMode.Password;
            this.BtnSave.Attributes.Remove("onclick");
            this.BtnDelete.Attributes.Remove("onclick");
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue("0"));
            this.DropDownList1.Enabled = false;
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue("0"));
            this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue("0"));
            this.DropDownList3.Enabled = false;
            this.BtnAdd.Enabled = false;
            this.BtnDelete.Enabled = false;
        }
        if (this.strCurUserId.ToLower().Equals("admin"))
        {
            this.BtnDelete.Visible = false;
        }
        else
        {
            this.BtnDelete.Visible = true;
        }

    }
    
    #region viewstate初始化区域
    private string strCurUserId
    {
        get
        {
            return ViewState["menuDetail_strCurUserId_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strCurUserId_ViewState"] = value;
        }
    }
    private DataTable dtDeptList
    {
        get
        {
            return (DataTable)this.ViewState["dtDeptList"];
        }
        set
        {
            this.ViewState["dtDeptList"] = value;
        }
    }
    private DataTable dtDcnoList
    {
        get
        {
            return (DataTable)this.ViewState["dtDcnoList"];
        }
        set
        {
            this.ViewState["dtDcnoList"] = value;
        }
    }
    private DataTable dtUserTypeList
    {
        get
        {
            return (DataTable)this.ViewState["dtUserTypeList"];
        }
        set
        {
            this.ViewState["dtUserTypeList"] = value;
        }
    }
    private string strErr2
    {
        get
        {
            return ViewState["menuDetail_strErr2_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strErr2_ViewState"] = value;
        }
    }
    private string strTip3
    {
        get
        {
            return ViewState["menuDetail_strTip3_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip3_ViewState"] = value;
        }
    }
    private string strTip5
    {
        get
        {
            return ViewState["menuDetail_strTip5_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip5_ViewState"] = value;
        }
    }
    private string strSuccessTip
    {
        get
        {
            return ViewState["menuDetail_strSuccessTip_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strSuccessTip_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定用户明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindUserInfo(bool bFresh, String strUserId)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["UserDetailViewState"] = GetUserDetailInfo(strUserId);
        }
        else
        {
            if (ViewState["UserDetailViewState"] == null)
            {
                ViewState["UserDetailViewState"] = GetUserDetailInfo(strUserId);
            }
        }
        dt = (DataTable)ViewState["UserDetailViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            //用户明细页面中是否显示密码
            String strIsShowPwd_UserDetailPage = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsShowPwd_UserDetailPage");

            this.TextBox1.Text = dt.Rows[0]["SUSERID"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SACCOUNTID"].ToString();
            if (strIsShowPwd_UserDetailPage.Equals("1"))
            {
                this.TextBox3.Text = dt.Rows[0]["SPWD"].ToString();
            }
            //this.TextBox4.Text = dt.Rows[0]["STAFFNO"].ToString();
            if (dt.Rows[0]["SDEPTCODE"] != null)
            {
                this.ddListDept.SelectedIndex = this.ddListDept.Items.IndexOf(this.ddListDept.Items.FindByValue(dt.Rows[0]["SDEPTCODE"].ToString()));
            }
            this.ddListDCNO.SelectedIndex = this.ddListDCNO.Items.IndexOf(this.ddListDCNO.Items.FindByValue(dt.Rows[0]["STAFFNO"].ToString()));
            this.TextBox5.Text = dt.Rows[0]["SUSERNAME"].ToString();
            this.TextBox6.Text = dt.Rows[0]["SUSERNAMECN"].ToString();
            this.TextBox7.Text = dt.Rows[0]["SDEPT"].ToString();
            this.TextBox8.Text = dt.Rows[0]["SDEPTCN"].ToString();
            this.TextBox9.Text = dt.Rows[0]["SPOSI"].ToString();
            this.TextBox10.Text = dt.Rows[0]["SPOSICN"].ToString();
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["BISALERT"].ToString()));
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue(dt.Rows[0]["BISGROUPUSER"].ToString()));
            this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue(dt.Rows[0]["BISSTOP"].ToString()));
            //this.ddListUserType.SelectedIndex = this.ddListUserType.Items.IndexOf(this.ddListUserType.Items.FindByValue(dt.Rows[0]["SUSERTYPE"].ToString()));
        }
    }
    #endregion

    /// <summary>
    /// 加载部门列表
    /// </summary>
    private void SetDeptDDList()
    {
        if (this.dtDeptList == null)
        {
            String strSql = "select * from VW_Sys_Department where LID = 'DEPTLINK'";
            this.dtDeptList = SqlParamDao.GetDataTableBySql(strSql);
        }
        DataTable dt = this.dtDeptList;
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            //
            this.ddListDept.Controls.Clear();
            this.ddListDept.Items.Clear();
            this.ddListDept.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (this.Language.Equals("zh-cn"))
                {
                    this.ddListDept.Items.Add(new ListItem(dt.Rows[i]["CDESCCHS"].ToString(), dt.Rows[i]["CID"].ToString()));
                }
                else
                {
                    this.ddListDept.Items.Add(new ListItem(dt.Rows[i]["CDESC"].ToString(), dt.Rows[i]["CID"].ToString()));
                }
            }
            this.ddListDept.ClearSelection();
            this.ddListDept.SelectedIndex = 0;
        }

    }

    /// <summary>
    /// 加载员工编号列表
    /// </summary>
    private void SetDcnoDDList(String strDeptCode)
    {
        String strSql = "select A.DCNO,A.DCNAME,A.DCNAMECHS from [VW_HR_DOCU] A where A.DCSTATUS <> '3' order by A.DCNO";
        if (!String.IsNullOrEmpty(strDeptCode))
        {
            strSql = "select A.DCNO,A.DCNAME,A.DCNAMECHS from [VW_HR_DOCU] A where A.DCSTATUS <> '3' and DCDDESCCHS = '" + strDeptCode + "' order by A.DCNO";
        }
        DataTable dt = new DataTable();
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        catch (Exception ex)
        {
            log.Error("错误提示:[VW_HR_DOCU]视图读取出错!");
            strSql = "select A.DCNO,A.DCNAME,A.DCNAMECHS from hrdocu_1 A where A.DCSTATUS <> '3' order by A.DCNO";
            if (!String.IsNullOrEmpty(strDeptCode))
            {
                strSql = "select A.DCNO,A.DCNAME,A.DCNAMECHS from hrdocu_1 A where A.DCSTATUS <> '3' and DCDDESCCHS = '" + strDeptCode + "' order by A.DCNO";
            }
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        //}
        //DataTable dt = this.dtDcnoList;
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            //
            this.ddListDCNO.Controls.Clear();
            this.ddListDCNO.Items.Clear();
            this.ddListDCNO.Items.Add(new ListItem("", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strDCNO = dt.Rows[i]["DCNO"].ToString();
                String strName = dt.Rows[i]["DCNAME"].ToString();
                String strNameChs = dt.Rows[i]["DCNAMECHS"].ToString();
                //String strDept = dt.Rows[i]["ODESC"].ToString();
                //String strDeptChs = dt.Rows[i]["ODESCCHS"].ToString();
                //String strPosi = dt.Rows[i]["PDESC"].ToString();
                //String strPosiChs = dt.Rows[i]["PDESCCHS"].ToString();
                this.ddListDCNO.Items.Add(new ListItem(strDCNO + "---" + strNameChs + "(" + strName + ")", strDCNO));
            }
            this.ddListDCNO.ClearSelection();
            this.ddListDCNO.SelectedIndex = 0;
        }

    }

    /// <summary>
    /// 部门下拉框选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        String strDeptCode = this.ddListDept.SelectedValue;

        String strSql = "select * from [VW_HR_DEPT] WHERE OID = '" + strDeptCode + "'";
        DataTable dt = new DataTable();
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        catch (Exception ex)
        {
            log.Error("错误提示:[VW_HR_DEPT]视图读取出错!");
            strSql = "select * from [CSORGA_1] WHERE OID = '" + strDeptCode + "'";
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }

        if ((dt != null) && (dt.Rows.Count == 1))
        {
            this.TextBox7.Text = dt.Rows[0]["ODESC"].ToString();
            this.TextBox8.Text = dt.Rows[0]["ODESCCHS"].ToString();
        } 

        this.SetDcnoDDList(strDeptCode);
  

    }

    /// <summary>
    /// 员工下拉框选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListDCNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        String strDCNO = this.ddListDCNO.SelectedValue;

        String strSql = "select A.* from [VW_HR_DOCU] A WHERE A.DCNO = '" + strDCNO + "'";
        DataTable dt = new DataTable();
        try
        {
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }
        catch (Exception ex)
        {
            log.Error("错误提示:[VW_HR_DOCU]视图读取出错!");
            strSql = "select A.*,B.PDESC,B.PDESCCHS from hrdocu_1 A INNER JOIN CSPOSI_1 B ON A.DCHRPOSI = B.PID WHERE A.DCNO = '" + strDCNO + "'";
            dt = SqlParamDao.GetDataTableBySql(strSql);
        }

        if ((dt != null) && (dt.Rows.Count ==1 ))
        {
            this.TextBox5.Text = dt.Rows[0]["DCNAME"].ToString();
            this.TextBox6.Text = dt.Rows[0]["DCNAMECHS"].ToString();
            this.TextBox9.Text = dt.Rows[0]["PDESC"].ToString();
            this.TextBox10.Text = dt.Rows[0]["PDESCCHS"].ToString();
        }

    }

    /// <summary>
    /// 加载用户类别列表
    /// </summary>
    private void SetUserTypeDDList()
    {
        //if (this.dtUserTypeList == null)
        //{
        //    String strSql = "select * from TB_HRLSTD WHERE LID = 'USERTYPE'";
        //    this.dtUserTypeList = SqlParamDao.GetDataTableBySql(strSql);
        //}
        //DataTable dt = this.dtUserTypeList;
        //if ((dt != null) && (dt.Rows.Count > 0))
        //{
        //    //
        //    this.ddListUserType.Controls.Clear();
        //    this.ddListUserType.Items.Clear();

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (this.Language.Equals("zh-cn"))
        //        {
        //            this.ddListUserType.Items.Add(new ListItem(dt.Rows[i]["CDESCCHS"].ToString(), dt.Rows[i]["CID"].ToString()));
        //        }
        //        else
        //        {
        //            this.ddListUserType.Items.Add(new ListItem(dt.Rows[i]["CDESC"].ToString(), dt.Rows[i]["CID"].ToString()));
        //        }
        //    }
        //    this.ddListUserType.ClearSelection();
        //    this.ddListUserType.SelectedIndex = 0;
        //}

    }

    /// <summary>
    /// 刷新数据操作
    /// </summary>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        //加载用户数据
        this.SetUserInfo();
    }

    #region 获取相应系统用户的明细信息
    /// <summary>
    /// 获取相应系统用户的明细信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetUserDetailInfo(String strUserId)
    {
        UserManagerBll bllUser = new UserManagerBll();
        DataTable dtUserInfo = bllUser.GetUserInfoByUserId(strUserId);
        return dtUserInfo;
    }
    #endregion

    #region 保存操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        String strUserId = this.TextBox2.Text.Trim();
        String strAccountId = this.TextBox2.Text.Trim();
        String strPwd = this.TextBox3.Text.Trim();
        //String strStaffNo = this.TextBox4.Text.Trim();
        String strStaffNo = this.ddListDCNO.SelectedValue;
        String strUserName = this.TextBox5.Text.Trim();
        String strUserNameChs = this.TextBox6.Text.Trim();
        String strDeptCode = this.ddListDept.SelectedValue;
        String strDeptName = this.TextBox7.Text.Trim();
        String strDeptNameChs = this.TextBox8.Text.Trim();
        String strPosiName = this.TextBox9.Text.Trim();
        String strPosiNameChs = this.TextBox10.Text.Trim();

        String strIsAlert = this.DropDownList1.SelectedValue.ToUpper();
        String strIsGroup = this.DropDownList2.SelectedValue.ToUpper();
        String strIsStop = this.DropDownList3.SelectedValue.ToUpper();
        //String strUserType = this.ddListUserType.SelectedValue.ToUpper();

        //用户明细页面中是否显示密码
        String strIsShowPwd_UserDetailPage = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsShowPwd_UserDetailPage");

        if (String.IsNullOrEmpty(strAccountId))
        {
            this.AlertMessageBox(this.Page,this.strTip3);
            this.TextBox2.Focus();
            this.TextBox2.BackColor = Color.Red;
            //返回页面
            return;
        }
        if (String.IsNullOrEmpty(strPwd)&& ((strIsShowPwd_UserDetailPage.Equals("1"))|| ViewState["opKey"].Equals("add")))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox3.Focus();
            this.TextBox3.BackColor = Color.Red;
            //返回页面
            return;
        }
        if (String.IsNullOrEmpty(strUserName))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox5.BackColor = Color.Red;
            this.TextBox5.Focus();
            //返回页面
            return;
        }
        if (String.IsNullOrEmpty(strUserNameChs))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox8.Focus();
            this.TextBox8.BackColor = Color.Red;
            //返回页面
            return;
        }
        //else
        //{
        if (this.hfIsRequiredMatchStaffNo.Value.Equals("1"))
        {
            if (String.IsNullOrEmpty(strDeptCode))
            {
                this.AlertMessageBox(this.Page, this.strTip3);
                this.ddListDept.Focus();
                this.ddListDept.BackColor = Color.Red;
                //返回页面
                return;
            }
            else if (String.IsNullOrEmpty(strStaffNo))
            {
                this.AlertMessageBox(this.Page, this.strTip3);
                this.ddListDCNO.Focus();
                this.ddListDCNO.BackColor = Color.Red;
                //返回页面
                return;
            }
        }
        try
        {
            UserManagerBll bllUser = new UserManagerBll();
            if (ViewState["opKey"].Equals("modify"))
            {
                if (bllUser.IsExsitAccountId(strAccountId))
                {
                    //bllUser.UpdateUserInfo(strUserId, strAccountId, strPwd, strStaffNo, strUserName, strUserNameChs, strDeptName, strDeptNameChs, strPosiName, strPosiNameChs, null, null, strIsAlert, strIsGroup, strIsStop);

                    String strSql = "UPDATE TB_HR_USER SET SUSERID='" + strUserId + "',SACCOUNTID='" + strAccountId+"' ";
                    if (!String.IsNullOrEmpty(strPwd))
                    {
                        strSql = strSql + ",SPWD=[dbo].[Fun_Encrypt_Password]('" + strPwd + "')";
                    }
                    strSql = strSql + ",STAFFNO ='" + strStaffNo + "',SUSERNAME='" + strUserName + "',SUSERNAMECN='" + strUserNameChs + "'";
                    strSql = strSql + ",SDEPTCODE = '" + strDeptCode + "',SDEPT='" + strDeptName + "',SDEPTCN='" + strDeptNameChs + "',SPOSI='" + strPosiName + "',SPOSICN='" + strPosiNameChs + "',BISALERT='" + strIsAlert + "'";
                    strSql = strSql + ",BISGROUPUSER='" + strIsGroup + "',BISSTOP='" + strIsStop + "' WHERE SUSERID ='" + strUserId + "'";
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                    //如果激活账户时，则清空用户登录错误信息 add by sammen 20150604
                    if (!strIsStop.Equals("1"))
                    {
                        UserLoginError.ClearLoginErrorLog(strUserId);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=\"javascript\">RefreshUserList();alert('" + this.strSuccessTip + "');</script>");

                }
            }
            else if (ViewState["opKey"].Equals("add"))
            {
                if (bllUser.IsExsitAccountId(strAccountId))
                {
                    this.AlertMessageBox(this.Page, this.strTip5);
                }
                else
                {
                    //bllUser.AddUserInfo(strUserId, strAccountId, strPwd, strStaffNo, strUserName, strUserNameChs, strDeptName, strDeptNameChs, strPosiName, strPosiNameChs, null, null, strIsAlert, strIsGroup, strIsStop);
                    String strSql = "INSERT INTO TB_HR_USER( SUSERID,SACCOUNTID,SPWD,STAFFNO,SUSERNAME,SUSERNAMECN,SDEPTCODE,SDEPT,SDEPTCN,SPOSI,SPOSICN,STREECLR,SWORKCLR,BISALERT,BISGROUPUSER,BISSTOP) VALUES ";
                    strSql = strSql + "('" + strUserId + "','" + strAccountId + "',[dbo].[Fun_Encrypt_Password]('" + strPwd + "'),'" + strStaffNo + "','" + strUserName + "','" + strUserNameChs + "','" + strDeptCode + "','" + strDeptName + "','" + strDeptNameChs + "'";
                    strSql = strSql + ",'" + strPosiName + "','" + strPosiNameChs + "',NULL,NULL,'" + strIsAlert + "','" + strIsGroup + "','" + strIsStop + "')";
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                    this.strCurUserId = strUserId;
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=\"javascript\">alert('" + this.strSuccessTip + "');UserListPageAddUser();</script>");


                }
            }

            //对用户信息操作后记录用户信息变更记录 add by sammen 20180114
            String strSpName = "USP_SYS_RecordUserInfoHis";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("UserId", strUserId); //被修改用户ID
            hsTableParam.Add("ModifyType", ViewState["opKey"].Equals("add") ? "010" : "020"); //010	新增用户; 020 修改用户信息
            hsTableParam.Add("OPUserId", this.GetUserCode());//操作用户ID
            hsTableParam.Add("OPUserIP", Com.ValuePlus.Utils.RequestUtils.GetIP());//操作用户IP
            try
            {
                SqlParamDao.ExcuteSP(strSpName, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("用户" + strUserId + "被增加或者修改后记录用户信息变更记录,执行存储过程失败: SPNAME:" + strSpName);
            }
            //对用户信息操作后记录用户信息变更记录 add by sammen 20180114

        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, this.strErr2);
        }
        //}
    }
    #endregion

    #region 触发新增操作
    /// <summary>
    /// 触发新增操作
    /// </summary>
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        ViewState["opKey"] = "add";
        Response.Redirect("UserDetail.aspx?userId=");
    }
    #endregion

    #region 删除操作
    /// <summary>
    /// 删除操作
    /// </summary>
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hsTable = new Hashtable();
            hsTable.Add("UserId", this.TextBox1.Text);
            hsTable.Add("OPUserId", this.GetUserCode());//操作用户ID
            hsTable.Add("OPUserIP", Com.ValuePlus.Utils.RequestUtils.GetIP());//操作用户IP
            int iReturn = SqlParamDao.ExcuteSP("USP_SYS_DELETE_USER", hsTable);

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=\"javascript\">alert('Deleted Successfully！');UserListPageAddUser();</script>"); 
            
        }
        catch (Exception ex)
        {
            log.Error(ex);
            //Response.Redirect("UserDetail.aspx?userId=" + this.TextBox1.Text);
            this.AlertMessageBox(this.Page, "Deleted Failed！请确认存储过程USP_SYS_DELETE_USER!");
        }
    }
    #endregion
}
