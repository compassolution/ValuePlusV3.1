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

public partial class UserManager_SetUserRole : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("UserManager");
            this.Button1.Text = rmLocResourceManager.GetString("btnAssign");
            this.Button2.Attributes.Add("onclick", "return false;");
            this.ListBox1.Attributes.Add("onDblClick", "dbClickLB1();");
            this.ListBox2.Attributes.Add("onDblClick", "dbClickLB2();");

            this.strSelUserId = Request.Params["userId"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strSelUserId = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strSelUserId);

            this.hfUserId.Value = strSelUserId;
            this.lbUserId.Text = strSelUserId;
            this.strCurUserId = base.GetUserCode().ToString();
            this.strErr1 = rmLocResourceManager.GetString("Err1");
            this.strErr3 = rmLocResourceManager.GetString("Err3");
            this.strTip1 = rmLocResourceManager.GetString("Tip6");
            this.strTip7 = rmLocResourceManager.GetString("Tip7");
            this.strTip8 = rmLocResourceManager.GetString("Tip8");
            this.strSuccessTip = rmLocResourceManager.GetString("Success4");

            this.Label1.Text = this.strTip7;
            this.Label2.Text = this.strTip8;
            this.TextBox1.Attributes.Add("onkeypress", "EnterSearchTextBox()");
            //加载系统用户表所有数据到页面中的列表中显示
            try
            {
                if (!String.IsNullOrEmpty(strSelUserId))
                {
                    //加载系统用户表所有数据到页面中的列表中显示
                    this.BindUserRoleListInfo(true, this.strCurUserId, this.strSelUserId, "");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + this.strErr1 + "');</script>");
            }
        }
    }

    #region viewstate初始化区域
    private string strCurUserId
    {
        get
        {
            return ViewState["userRole_strCurUserId_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strCurUserId_ViewState"] = value;
        }
    }
    private string strSelUserId
    {
        get
        {
            return ViewState["userRole_strSelUserId_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strSelUserId_ViewState"] = value;
        }
    }
    private string strErr1
    {
        get
        {
            return ViewState["userRole_strErr1_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strErr1_ViewState"] = value;
        }
    }
    private string strErr3
    {
        get
        {
            return ViewState["userRole_strErr3_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strErr3_ViewState"] = value;
        }
    }
    private string strTip1
    {
        get
        {
            return ViewState["userRole_strTip1_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strTip1_ViewState"] = value;
        }
    }
    private string strTip7
    {
        get
        {
            return ViewState["userRole_strTip7_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strTip7_ViewState"] = value;
        }
    }
    private string strTip8
    {
        get
        {
            return ViewState["userRole_strTip8_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strTip8_ViewState"] = value;
        }
    }
    private string strSuccessTip
    {
        get
        {
            return ViewState["userRole_strSuccessTip_ViewState"] as string;
        }
        set
        {
            ViewState["userRole_strSuccessTip_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定当前用户的角色列表数据
    /// <summary>
    /// 绑定当前用户的角色列表数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindUserRoleListInfo(bool bFresh, String strCurUserId, String strSelUserId, String strFilterSql)
    {
        UserManagerBll bllUserManager = new UserManagerBll();
        if (bFresh)
        {
            ViewState["CurUserRoleListViewState"] = bllUserManager.GetRoleInfoByCurUserIdAndSelUser(strCurUserId, strSelUserId,base.IsAdminstrator());
            ViewState["SelUserRoleListViewState"] = bllUserManager.GetRoleInfoByUserId(strSelUserId);
        }
        else
        {
            if (ViewState["CurUserRoleListViewState"] == null)
            {
                ViewState["CurUserRoleListViewState"] = bllUserManager.GetRoleInfoByCurUserIdAndSelUser(strCurUserId, strSelUserId,base.IsAdminstrator());
                ViewState["SelUserRoleListViewState"] = bllUserManager.GetRoleInfoByUserId(strSelUserId);
            }
        }
        this.ListBox1.Items.Clear();
        this.ListBox2.Items.Clear();
        DataTable dt1 = (DataTable)ViewState["CurUserRoleListViewState"];
        DataTable dt2 = (DataTable)ViewState["SelUserRoleListViewState"];

        //填充当前用户所具有的角色列表ListBox1
        if ((dt1 != null) && (dt1.Rows.Count > 0))
        {
            DataRow[] drs = dt1.Select("1=1");
            if (!String.IsNullOrEmpty(strFilterSql))
            {
                drs = dt1.Select(strFilterSql);
            }
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strTid = dr["TID"].ToString();
                    String strRid = dr["RID"].ToString();
                    String strRoleName = "";
                    String strTDesc = "";
                    if (base.Language == "zh-cn")
                    {
                        strRoleName = dr["RDESCCHS"].ToString();
                        strTDesc = dr["TDESCCHS"].ToString();
                    }
                    else
                    {
                        strRoleName = dr["RDESC"].ToString();
                        strTDesc = dr["TDESC"].ToString();
                    }
                    lstItem.Value = strTid + "*" + strRid;
                    lstItem.Text = "【" + strTDesc + "】" + strRoleName;
                    this.ListBox1.Items.Add(lstItem);
                }
            }
        }
        //填充需分配角色的用户所具有的角色列表
        if ((dt2 != null) && (dt2.Rows.Count > 0))
        {
            DataRow[] drs = dt2.Select("1=1");
            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strTid = dr["TID"].ToString();
                    String strRid = dr["RID"].ToString();
                    String strRoleName = "";
                    String strTDesc = "";
                    if (base.Language == "zh-cn")
                    {
                        strRoleName = dr["RDESCCHS"].ToString();
                        strTDesc = dr["TDESCCHS"].ToString();
                    }
                    else
                    {
                        strRoleName = dr["RDESC"].ToString();
                        strTDesc = dr["TDESC"].ToString();
                    }
                    lstItem.Value = strTid + "*" + strRid;
                    lstItem.Text = "【" + strTDesc + "】" + strRoleName;
                    this.ListBox2.Items.Add(lstItem);
                }
            }
        }

    }
    #endregion

    #region 角色分配操作
    /// <summary>
    /// 角色分配操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strTidAndRid = this.hidTidRid.Value.ToString();
        int insertCount = 0;
        String[] strRidArr = null;
        if (!String.IsNullOrEmpty(strTidAndRid))
        {
            strRidArr = strTidAndRid.Split('#');
        }
        UserManagerBll bllUserManager = new UserManagerBll();
        try
        {
            insertCount = bllUserManager.AssignUserRole(this.strSelUserId, strRidArr);
            BindUserRoleListInfo(true, this.strCurUserId, this.strSelUserId,"");
            this.AlertMessageBox(this.Page, this.strSuccessTip);
            
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, this.strErr3);
        }
        
        
    }
    #endregion

    #region 过滤操作
    /// <summary>
    /// 过滤操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        String strFilter = this.TextBox1.Text;
        String strFilterSql = "";
        if (!String.IsNullOrEmpty(strFilter))
        {
            if (base.Language == "zh-cn")
            {
                strFilterSql = "(RDESCCHS LIKE '%" + strFilter + "%' OR TDESCCHS  LIKE '%" + strFilter + "%')";
            }
            else
            {
                strFilterSql = "(RDESC LIKE '%" + strFilter + "%' OR TDESC  LIKE '%" + strFilter + "%')";
            }
        }
        //this.strFilterCondition = strFilterSql;
        this.BindUserRoleListInfo(true, this.strCurUserId, this.strSelUserId, strFilterSql);

    }
    #endregion

}
