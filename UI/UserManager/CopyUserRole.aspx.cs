using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
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
using Com.ValuePlus.Common.Security;

public partial class UserManager_CopyUserRole : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.TextBox1.Attributes.Add("onkeypress", "EnterSearchTextBox()");

            //加载系统用户表所有数据到页面中的列表中显示
            try
            {
                //获取操作类型【0、用户信息管理；1、用户栏目授权；2、用户角色分配】
                if (Request.Params["opType"] != null)
                {
                    this.strOpType = Request.Params["opType"].ToString();
                }
                //需要复制的用户
                if (Request.Params["userId"] != null)
                {
                    this.strUserIdTo = Request.Params["userId"].ToString();
                }
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpType);
                this.strUserIdTo = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strUserIdTo);

                //加载系统用户表所有数据到页面中的列表中显示
                this.BindUserListInfo(true, false, null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    #region viewstate初始化区域
    private string strOpType
    {
        get
        {
            return ViewState["strOpType_ViewState"] as string;
        }
        set
        {
            ViewState["strOpType_ViewState"] = value;
        }
    }
    private string strUserIdTo
    {
        get
        {
            return ViewState["strUserIdTo_ViewState"] as string;
        }
        set
        {
            ViewState["strUserIdTo_ViewState"] = value;
        }
    }
    private string strLbDesc2
    {
        get
        {
            return ViewState["userList_strLbDesc2_ViewState"] as string;
        }
        set
        {
            ViewState["userList_strLbDesc2_ViewState"] = value;
        }
    }
    private string strLbDesc6
    {
        get
        {
            return ViewState["userList_strLbDesc6_ViewState"] as string;
        }
        set
        {
            ViewState["userList_strLbDesc6_ViewState"] = value;
        }
    }
    private string strLbDesc5
    {
        get
        {
            return ViewState["userList_strLbDesc5_ViewState"] as string;
        }
        set
        {
            ViewState["userList_strLbDesc5_ViewState"] = value;
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

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    /// <param name="IsGroupUser">是否是集团用户</param>
    private void BindUserListInfo(bool bFresh, bool IsGroupUser, String strFilter)
    {
        DataTable dt = new DataTable();
        String strLab1 = "[" + strLbDesc2 + "]"; ;
        String strLab2 = "";
        if (base.Language == "zh-cn")
        {
            strLab2 = strLbDesc6;
        }
        else
        {
            strLab2 = strLbDesc5;
        }
        strLab2 = "[" + strLab2 + "]";

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
        this.ListBox1.Items.Clear();
        dt = (DataTable)ViewState["UserListViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            String strFilterSql = "BISGROUPUSER='0'";
            if (IsGroupUser)
            {
                strFilterSql = "BISGROUPUSER='1'";
            }
            if (!String.IsNullOrEmpty(strFilter))
            {
                strFilterSql = strFilterSql + " AND " + strFilter;
            }
            DataRow[] drs = dt.Select(strFilterSql);
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strUserId = dr["SUSERID"].ToString();
                    String strAccountId = dr["SACCOUNTID"].ToString();
                    String strUserName = "";
                    if (base.Language == "zh-cn")
                    {
                        strUserName = dr["SUSERNAMECN"].ToString();
                    }
                    else
                    {
                        strUserName = dr["SUSERNAME"].ToString();
                    }
                    lstItem.Value = strUserId;
                    lstItem.Text = strLab1 + fillStrBySpace(strAccountId) + strLab2 + strUserName;
                    this.ListBox1.Items.Add(lstItem);
                }
            }
        }
    }
    #endregion

    #region 获取系统用户信息
    /// <summary>
    /// 获取系统用户信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetUserListInfo()
    {
        UserManagerBll bllUser = new UserManagerBll();
        DataTable dtUserInfo = bllUser.GetAllUserInfoTable();
        return dtUserInfo;
    }
    #endregion

    #region 如果一字符串长度不满一个固定值，则在后面加上字符填充，以满足固定长度值
    private String fillStrBySpace(String str)
    {
        int iAllLength = 30;
        int iLength = str.Length;
        int iMid = iAllLength - iLength;
        if (iMid > 0)
        {
            for (int i = 0; i < iMid; i++)
            {
                str = str + "-";
            }
        }
        return str;
    }
    #endregion

    #region 过滤操作
    /// <summary>
    /// 过滤操作
    /// </summary>
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        String strFilter = this.TextBox1.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            strFilterSql = "(SACCOUNTID LIKE '%" + strFilter + "%' OR SUSERNAMECN LIKE '%" + strFilter + "%' OR SUSERNAME LIKE '%" + strFilter + "%')";
        }
        this.strFilterCondition = strFilterSql;
        this.BindUserListInfo(false, false, strFilterSql);

    }
    #endregion

    #region 确定复制操作
    /// <summary>
    /// 确定复制操作
    /// </summary>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(this.ListBox1.SelectedValue))
            {
                this.AlertMessageBox(this.Page, "Please select first!");
                return;
            }
            String strCopyUserId = this.ListBox1.SelectedValue.ToString();
            String strSql = "";
            if (this.strOpType.Equals("1"))
            {
                strSql = "DELETE FROM TB_HR_USERMENU WHERE SUSERID = '" + strUserIdTo + "';INSERT INTO TB_HR_USERMENU (SKEY,SUSERID,SMENUCODE,SRIGHTS) SELECT NewID(),'" + this.strUserIdTo + "',SMENUCODE,'1' FROM TB_HR_USERMENU WHERE SUSERID = '" + strCopyUserId + "'";
            }
            else if (this.strOpType.Equals("2"))
            {
                strSql = "DELETE FROM TB_HR_USERROLE WHERE SUSERID = '" + strUserIdTo + "';INSERT INTO TB_HR_USERROLE (SUSERID,TID,RID) SELECT '" + this.strUserIdTo + "',TID,RID FROM TB_HR_USERROLE WHERE SUSERID = '" + strCopyUserId + "'";
            }
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>RefreshOpener();</script>");
            }
        }
        catch (Exception ex)
        {
            this.AlertMessageBox(this.Page, "Copy Failed !");
            log.Error(ex);
        }

    }
    #endregion


}
