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
using Com.ValuePlus.Common.Security;

public partial class UserManager_UserList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //获取操作类型【0、用户信息管理；1、用户栏目授权；2、用户角色分配】
        String strOpType = Request.Params["opType"].ToString();
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpType);

        ViewState["OpType"] = strOpType;
        if (!Page.IsPostBack)
        {
            ViewState["IsGroupUser"] = false;//默认是非集团用户
            ResourceManager rmLocResourceManager = base.GetResourceManager("UserManager");
            //下拉框中英文设置
            this.DropDownList1.Items[0].Text = ">>>>>>>>>>>>>>" + rmLocResourceManager.GetString("IsGroupItem1");
            this.DropDownList1.Items[1].Text = ">>>>>>>>>>>>>>" + rmLocResourceManager.GetString("IsGroupItem2");
            this.BtnRefresh.Text = rmLocResourceManager.GetString("btnRefresh");
            if (ViewState["OpType"].Equals("0"))//0、用户信息管理
            {
                this.Label1.Text = rmLocResourceManager.GetString("lbWarmTip1");
            }
            else if (ViewState["OpType"].Equals("1"))//1、用户栏目授权
            {
                this.Label1.Text = rmLocResourceManager.GetString("lbWarmTip2");
            }
            else if (ViewState["OpType"].Equals("2"))//2、用户角色分配
            {
                this.Label1.Text = rmLocResourceManager.GetString("lbWarmTip3");
            }

            this.Label1.Visible = true;
            this.BtnRefresh.Visible = false;
            this.TextBox1.Attributes.Add("onkeypress", "EnterSearchTextBox()");

            this.strLbDesc2 = rmLocResourceManager.GetString("lbDesc2");
            this.strLbDesc6 = rmLocResourceManager.GetString("lbDesc6");
            this.strLbDesc5 = rmLocResourceManager.GetString("lbDesc5");

            //加载系统用户表所有数据到页面中的列表中显示
            try
            {
                //加载系统用户表所有数据到页面中的列表中显示
                this.BindUserListInfo(true, (bool)ViewState["IsGroupUser"],null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    #region viewstate初始化区域
    private string strSelectedUser
    {
        get
        {
            return ViewState["userList_strSelectedUser_ViewState"] as string;
        }
        set
        {
            ViewState["userList_strSelectedUser_ViewState"] = value;
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
    private void BindUserListInfo(bool bFresh, bool IsGroupUser,String strFilter)
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
                    String strUserDept = "";
                    String strStaffNo = dr["STAFFNO"] == null ? "" : dr["STAFFNO"].ToString();
                    String strStatus = dr["BISSTOP"] == null ? "0" : dr["BISSTOP"].ToString();
                    if (base.Language == "zh-cn")
                    {
                        strUserName = dr["SUSERNAMECN"].ToString();
                        strUserDept = dr["SDEPTCN"]==null?"":dr["SDEPTCN"].ToString();
                    }
                    else
                    {
                        strUserName = dr["SUSERNAME"].ToString();
                        strUserDept = dr["SDEPT"] == null ? "" : dr["SDEPT"].ToString();
                    }
                    lstItem.Value = strUserId;
                    String strLab1 = "[" + strStaffNo + "]";
                    if (strStatus.Equals("1"))
                    {
                        strLab1 = "[停用用户]"+"[" + strStaffNo + "]"; 
                    }
                    String strLab2 =  "[" + strUserDept + "]";
                    lstItem.Text = strLab1 + fillStrBySpace(strAccountId, strLab1.Length + strAccountId.Length) + strUserName + strLab2;
                    if (strStatus.Equals("1"))
                    {
                        lstItem.Attributes.CssStyle.Add("text-decoration", "line-through");
                        lstItem.Attributes.CssStyle.Add("color", "gray");
                    }
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

    #region 用户类型选择变更事件
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DropDownList1.SelectedValue == "0")
        {
            ViewState["IsGroupUser"] = false;//选择非集团用户
        }else if (this.DropDownList1.SelectedValue == "1")
        {
            ViewState["IsGroupUser"] = true;//选择集团用户
        }
        this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], this.strFilterCondition);
        this.FrmDetail.Attributes["src"] = "";
    }
    #endregion

    #region 重新加载操作
    /// <summary>
    /// 重新加载操作
    /// </summary>
    protected void BtnRefresh_Click(object sender, EventArgs e)
    {
        //重新加载系统用户表所有数据到页面中的列表中显示
        this.TextBox1.Text = "";
        this.BindUserListInfo(true, (bool)ViewState["IsGroupUser"], "");
        //this.FrmDetail.Attributes["src"] = "";
        this.Label1.Visible = true;
    }
    #endregion

    #region 新增用户操作
    /// <summary>
    /// 新增用户操作
    /// </summary>
    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        this.FrmDetail.Attributes["src"] = "UserDetail.aspx?userId=";
        this.BtnRefresh.Visible = true;
        this.BindUserListInfo(true, (bool)ViewState["IsGroupUser"], "");//同时刷新列表
        this.TextBox1.Text = "";
        this.strFilterCondition = "";
    }
    #endregion

    #region 过滤操作
    /// <summary>
    /// 过滤操作
    /// </summary>
    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        String strFilter = this.TextBox1.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            strFilterSql = "(SACCOUNTID LIKE '%" + strFilter + "%' OR SUSERNAMECN LIKE '%" + strFilter + "%' OR SUSERNAME LIKE '%" + strFilter + "%')";
        }
        this.strFilterCondition = strFilterSql;
        this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], strFilterSql);

        this.strSelectedUser = this.ListBox1.SelectedValue;
        this.RedirectNextPage(this.strSelectedUser);
        
    }
    #endregion

    #region ListBox框变更事件
    protected void ListBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String strUserId = this.ListBox1.SelectedValue;
        this.strSelectedUser = strUserId;
        int iSelected = this.ListBox1.SelectedIndex;
        this.RedirectNextPage(this.strSelectedUser);
        this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], this.strFilterCondition);
        if (this.ListBox1.Items.Count > iSelected)
        {
            this.ListBox1.Items[iSelected].Selected = true;
        }

    }

    /// <summary>
    /// 转向下一步处理页面
    /// </summary>
    /// <param name="strUserId"></param>
    private void RedirectNextPage(String strUserId)
    {
        if (ViewState["OpType"].Equals("0"))//进入用户信息管理页面
        {
            this.FrmDetail.Attributes["src"] = "UserDetail.aspx?userId=" + strUserId;
            //this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], this.strFilterCondition);
        }
        else if (ViewState["OpType"].Equals("1"))//进入栏目授权页面
        {
            this.FrmDetail.Attributes["src"] = "SetUserMenu.aspx?userId=" + strUserId;
            //this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], this.strFilterCondition);
        }
        else if (ViewState["OpType"].Equals("2"))//进入用户角色分配页面
        {
            this.FrmDetail.Attributes["src"] = "SetUserRole.aspx?userId=" + strUserId;
            //this.BindUserListInfo(false, (bool)ViewState["IsGroupUser"], this.strFilterCondition);
        }
        //this.Label1.Visible = false;
        this.BtnRefresh.Visible = true;
    }
    #endregion

    #region 如果一字符串长度不满一个固定值，则在后面加上字符填充，以满足固定长度值
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    private String fillStrBySpace(String str,int iLength)
    {
        int iAllLength = 40;
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


}
