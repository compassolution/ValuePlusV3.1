using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class ICCard_StaffList : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //获取操作类型【1、发放员工卡；2、用餐充值】
        this.strOpType = "1";
        if (Request.Params["opType"] != null)
        {
            this.strOpType = Request.Params["opType"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpType);
        }
        if (!Page.IsPostBack)
        {
            try
            {
                this.SetDeptDDList();
                this.SetCardUseTypeDDList();
                this.SetUserListInfo(true,"");
            }
            catch (Exception ex)
            {
                this.AlertMessageBox(this.Page,"页面加载失败！");
                log.Error(ex);
            }
        }
    }

    #region viewstate初始化区域
    private string strOpType
    {
        get
        {
            return ViewState["strOpType"] as string;
        }
        set
        {
            ViewState["strOpType"] = value;
        }
    }
    private string strIsFM
    {
        get
        {
            return ViewState["strIsFM"] as string;
        }
        set
        {
            ViewState["strIsFM"] = value;
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
    private DataTable dtCardUseTypeList
    {
        get
        {
            return (DataTable)this.ViewState["dtCardUseTypeList"];
        }
        set
        {
            this.ViewState["dtCardUseTypeList"] = value;
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
    private string strCurDeptCode
    {
        get
        {
            return ViewState["strCurDeptCode"] as string;
        }
        set
        {
            ViewState["strCurDeptCode"] = value;
        }
    }
    private string strCurCardUseTypeCode
    {
        get
        {
            return ViewState["strCurCardUseTypeCode"] as string;
        }
        set
        {
            ViewState["strCurCardUseTypeCode"] = value;
        }
    }
    private string strCurDCNO
    {
        get
        {
            return ViewState["strCurDCNO"] as string;
        }
        set
        {
            ViewState["strCurDCNO"] = value;
        }
    }
    private string strFilterCondition
    {
        get
        {
            return ViewState["userList_strFilterCondition_ViewState"] as string;
        }
        set
        {
            ViewState["userList_strFilterCondition_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 加载部门列表
    /// </summary>
    private void SetDeptDDList()
    {
        try
        {
            if (this.dtDeptList == null)
            {
                String strSql = "select * from VW_Sys_Department where LID = 'DEPTLINK'";
                this.dtDeptList = SqlParamDao.GetDataTableBySql(strSql);
            }
            DataTable dt = this.dtDeptList;

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.ddListDept.Controls.Clear();
                this.ddListDept.Items.Clear();
                this.ddListDept.Items.Add(new ListItem(">>>>请选择员工所在部门", ""));

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
        catch (Exception ex)
        {
            this.AlertMessageBox(this.Page, "部门列表加载失败！");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 加载员工卡使用类型列表
    /// </summary>
    private void SetCardUseTypeDDList()
    {
        try
        {
            if (this.dtCardUseTypeList == null)
            {
                String strSql = "select * from TB_HRLSTD WHERE LID = 'CARDUSERTYPE'";
                this.dtCardUseTypeList = SqlParamDao.GetDataTableBySql(strSql);
            }
            DataTable dt = this.dtCardUseTypeList;

            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.ddListCardUserType.Controls.Clear();
                this.ddListCardUserType.Items.Clear();
                this.ddListCardUserType.Items.Add(new ListItem(">>>>请选择员工卡使用类型", ""));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (this.Language.Equals("zh-cn"))
                    {
                        this.ddListCardUserType.Items.Add(new ListItem(dt.Rows[i]["CDESCCHS"].ToString(), dt.Rows[i]["CID"].ToString()));
                    }
                    else
                    {
                        this.ddListCardUserType.Items.Add(new ListItem(dt.Rows[i]["CDESC"].ToString(), dt.Rows[i]["CID"].ToString()));
                    }
                }
                this.ddListCardUserType.ClearSelection();
                this.ddListCardUserType.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            this.AlertMessageBox(this.Page, "部门列表加载失败！");
            log.Error(ex);
        }
    }
    
    /// <summary>
    /// 加载员工列表
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    /// <param name="strFilter">过滤条件</param>
    private void SetUserListInfo(bool bFresh, String strFilter)
    {
        if (String.IsNullOrEmpty(strFilter))
        {
            strFilter = "1=1";
        }
        try
        {
            DataTable dt = new DataTable();
            if (bFresh)
            {
                ViewState["UserListViewState"] = GetUserListInfo();
            }
            else
            {
                if (ViewState["UserListViewState"] == null)
                {
                    ViewState["UserListViewState"] = GetUserListInfo();
                }
            }
            this.lstBoxStaff.Items.Clear();
            dt = (DataTable)ViewState["UserListViewState"];
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow[] drs = dt.Select(strFilter);
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        ListItem lstItem = new ListItem();
                        String strUserId = dr["DCNO"].ToString();
                        String strUserName = "";
                        String strCardNo = dr["CARDNO"].ToString();
                        if (base.Language == "zh-cn")
                        {
                            strUserName = dr["DCNAMECHS"].ToString();
                        }
                        else
                        {
                            strUserName = dr["DCNAME"].ToString();
                        }
                        lstItem.Value = strUserId;
                        lstItem.Text = strUserId + strUserName + "(" + strCardNo + ")";
                        this.lstBoxStaff.Items.Add(lstItem);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.AlertMessageBox(this.Page, "加载员工列表失败！");
            log.Error(ex);
        }
    }
    
    /// <summary>
    /// 获取系统用户信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetUserListInfo()
    {
        String strSql = "select * from HRDOCU_1 WHERE DCSTATUS <> '3' ORDER BY DCNO";
        DataTable dtUserInfo = SqlParamDao.GetDataTableBySql(strSql);
        return dtUserInfo;
    }

    /// <summary>
    /// 部门下拉框选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.strCurDeptCode = this.ddListDept.SelectedValue;
        this.strFilterCondition = " 1=1 ";
        if (!String.IsNullOrEmpty(this.strCurCardUseTypeCode))
        {
            this.strFilterCondition = this.strFilterCondition + " and CARDUSERTYPE = '" + this.strCurCardUseTypeCode + "'";
        }
        if (!String.IsNullOrEmpty(this.strCurDeptCode))
        {
            this.strFilterCondition = this.strFilterCondition + " and DCDDESCCHS = '" + this.strCurDeptCode + "'";
        }

        this.doRefresh();

    }

    /// <summary>
    /// 部门下拉框选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListCardUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.strCurCardUseTypeCode = this.ddListCardUserType.SelectedValue;
        this.strFilterCondition = " 1=1 ";
        if (!String.IsNullOrEmpty(this.strCurDeptCode))
        {
            this.strFilterCondition = this.strFilterCondition + " and DCDDESCCHS = '" + this.strCurDeptCode + "'";
        }
        if (!String.IsNullOrEmpty(this.strCurCardUseTypeCode))
        {
            this.strFilterCondition = this.strFilterCondition + " and CARDUSERTYPE = '" + this.strCurCardUseTypeCode + "'";
        }

        this.doRefresh();

    }

    /// <summary>
    /// 过滤查找操作
    /// </summary>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        String strFilter = this.txtStaff.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            strFilterSql = "(DCNO LIKE '%" + strFilter + "%' OR DCNAME LIKE '%" + strFilter + "%' OR DCNAMECHS LIKE '%" + strFilter + "%')";
        }

        this.doRefresh();

    }

    /// <summary>
    /// 重新刷新操作
    /// </summary>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.doRefresh();
    }

    /// <summary>
    /// 重新刷新操作
    /// </summary>
    private void doRefresh()
    {
        if (this.cBoxIsFM.Checked)//家属列表
        {
            this.SetFMUserListInfo();
        }
        else
        {
            this.SetUserListInfo(true, this.strFilterCondition);
        }
    }

    /// <summary>
    /// 是否员工家属的选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cBoxIsFM_OnCheckedChanged(object sender, System.EventArgs e)
    {
        if (this.cBoxIsFM.Checked)//家属列表
        {
            this.SetFMUserListInfo();
        }
        else
        {
            this.SetUserListInfo(true, this.strFilterCondition);
        }
    }
    
    /// <summary>
    /// 加载员工家属列表
    /// </summary>
    private void SetFMUserListInfo()
    {
        this.lstBoxStaff.Items.Clear();
        String strSql = "select A.*,B.DCNAMECHS,B.DCNAME from HRDOCU_2 A LEFT JOIN HRDOCU_1 B ON A.DCNO = B.DCNO WHERE 1=1 ";
        if (!String.IsNullOrEmpty(this.strCurDeptCode))
        {
            strSql = strSql + " AND B.DCDDESCCHS = '" + this.strCurDeptCode + "'";
        }
        strSql = strSql + " order by B.DCDDESCCHS,B.DCNAMECHS";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            DataRow[] drs = dt.Select("1=1");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strUserId = dr["FMNO"].ToString();
                    String strFMName = dr["FMNAME"].ToString();
                    String strFMRelate = dr["RELATION"].ToString();
                    String strUserName = "";
                    String strCardNo = dr["CARDNO"].ToString();
                    if (base.Language == "zh-cn")
                    {
                        strUserName = dr["DCNAMECHS"].ToString();
                    }
                    else
                    {
                        strUserName = dr["DCNAME"].ToString();
                    }
                    lstItem.Value = strUserId;
                    lstItem.Text = strUserId + strFMName + "(" + strCardNo + ")————“" + strUserName + "”" + strFMRelate;
                    this.lstBoxStaff.Items.Add(lstItem);
                }
            }
        }
    }

    /// <summary>
    /// 员工列表选择事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstBoxStaff_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String strDCNO = this.lstBoxStaff.SelectedValue;

        String strParam = "DCNO=" + strDCNO;
        if (this.cBoxIsFM.Checked)
        {
            strParam = strParam + "&USER=FM";
        }
        else
        {
            strParam = strParam + "&USER=STAFF";
        }
        strParam = UrlParamEncryption.EncryptionUrlParam(strParam);

        if (this.strOpType.Equals("1"))//进入发放员工卡页面
        {
            this.FrmDetail.Attributes["src"] = "CardBinding.aspx?" + strParam;
        }
        else if (this.strOpType.Equals("2"))//进入用餐充值页面
        {
            this.FrmDetail.Attributes["src"] = "CardTopUp.aspx?" + strParam;
        }
        //this.Label1.Visible = false;
        this.btnRefresh.Visible = true;

    }



}