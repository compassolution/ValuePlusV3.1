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
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.Common.Security;

public partial class MenuManager_MenuDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strMenuCode = Request.Params["menuCode"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strMenuCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMenuCode);

            ResourceManager rmLocResourceManager = base.GetResourceManager("MenuManager");

            //自定义设置页面文字显示的中英文字符串
            this.Label1.Text = rmLocResourceManager.GetString("lbMenuDetail");
            this.Label2.Text = rmLocResourceManager.GetString("lbLocation");
            this.Label3.Text = rmLocResourceManager.GetString("lbMenuCode");
            this.Label4.Text = rmLocResourceManager.GetString("lbMenuName");
            this.Label5.Text = rmLocResourceManager.GetString("lbMenuNameChs");
            this.Label6.Text = rmLocResourceManager.GetString("lbUrlDetail");
            this.Label7.Text = rmLocResourceManager.GetString("lbMenuType");
            this.Label8.Text = rmLocResourceManager.GetString("lbImage");
            this.Label9.Text = rmLocResourceManager.GetString("lbParentCode");
            this.Label10.Text = rmLocResourceManager.GetString("lbLevel");
            this.Label11.Text = rmLocResourceManager.GetString("lbOrder");
            this.Label12.Text = rmLocResourceManager.GetString("lbIsStop");
            this.Button1.Text = rmLocResourceManager.GetString("btnSave");
            this.Button2.Text = rmLocResourceManager.GetString("btnAdd");
            this.Button3.Text = rmLocResourceManager.GetString("btnBack");
            this.Button4.Text = rmLocResourceManager.GetString("btnDelete");
            //栏目类型下拉框中英文设置
            this.DropDownList1.Items[0].Text = rmLocResourceManager.GetString("menuType1");
            this.DropDownList1.Items[1].Text = rmLocResourceManager.GetString("menuType2");
            this.DropDownList1.Items[2].Text = rmLocResourceManager.GetString("menuType3");
            this.DropDownList1.Items[3].Text = rmLocResourceManager.GetString("menuType4");
            this.DropDownList1.Items[4].Text = rmLocResourceManager.GetString("menuType5");
            this.DropDownList1.Items[5].Text = rmLocResourceManager.GetString("menuType6");
            this.DropDownList1.Items[6].Text = rmLocResourceManager.GetString("menuType7");
            this.DropDownList1.Items[7].Text = rmLocResourceManager.GetString("menuType8");
            this.DropDownList1.Items[8].Text = rmLocResourceManager.GetString("menuType9");
            this.DropDownList1.Items[9].Text = rmLocResourceManager.GetString("menuType10");
            this.DropDownList1.Items[10].Text = rmLocResourceManager.GetString("menuType11");
            this.DropDownList1.Items[11].Text = rmLocResourceManager.GetString("menuType12");
            //窗口打开位置中英文设置
            this.ddList_ShowLocation.Items[0].Text = rmLocResourceManager.GetString("LocationItem1");
            this.ddList_ShowLocation.Items[1].Text = rmLocResourceManager.GetString("LocationItem2");
            //是否停用下拉框中英文设置
            this.DropDownList2.Items[0].Text = rmLocResourceManager.GetString("IsStopItem1");
            this.DropDownList2.Items[1].Text = rmLocResourceManager.GetString("IsStopItem2");
            //设置某些控件只读
            this.TextBox6.Attributes["readOnly"] = "true";
            this.TextBox7.Attributes["readOnly"] = "true";

            this.strErr1 = rmLocResourceManager.GetString("Err1");
            this.strErr2 = rmLocResourceManager.GetString("Err2");
            this.strErr3 = rmLocResourceManager.GetString("Err3");
            this.strErr4 = rmLocResourceManager.GetString("Err4");
            this.strTip_modify_Confirm = rmLocResourceManager.GetString("Tip_confirmModify");
            this.strTip_ExsitMenuCode = rmLocResourceManager.GetString("Tip1");
            this.strTip_ExsitOrder = rmLocResourceManager.GetString("Tip4");
            this.strTip_del_Confirm = rmLocResourceManager.GetString("Tip2");
            this.strTip3 = rmLocResourceManager.GetString("Tip3");
            this.strTip5 = rmLocResourceManager.GetString("Tip5");
            this.strSuccessTip1 = rmLocResourceManager.GetString("Success1");
            this.strSuccessTip2 = rmLocResourceManager.GetString("Success2");

            //加载父栏目的下拉选择框
            this.CreateParentMenuList();

            if ((strMenuCode != null) && (!strMenuCode.Equals("")))//修改当前栏目信息
            {
                ViewState["opKey"] = "modify";//页面修改
                this.TextBox1.ReadOnly = true;
                this.Button1.Attributes.Remove("onclick");
                this.Button1.Attributes.Add("onclick", "return window.confirm( '" + strTip_modify_Confirm + " '); ");
                this.Button2.Enabled = true;
                this.Button4.Attributes.Remove("onclick");
                this.Button4.Attributes.Add("onclick", "return window.confirm( '" + strTip_del_Confirm + " '); ");
                this.Button4.Enabled = true;
                
                //加载栏目表所有数据到页面中的列表中显示
                try
                {
                    //获取栏目表所有数据dataset
                    this.BindMenuInfo(true, strMenuCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Response.Write("<script language=\"javascript\">alert('" + strErr1 + "');</script>");
                }
            }
            else//新增栏目信息
            {
                ViewState["opKey"] = "add";//页面新增
                this.TextBox1.ReadOnly = false;
                this.Button1.Attributes.Remove("onclick");
                this.Button2.Enabled = false;
                this.Button4.Enabled = false;
                this.Button4.Attributes.Remove("onclick");
            }
        }
    }

    #region viewstate初始化区域
    private string strErr1
    {
        get
        {
            return ViewState["menuDetail_strErr1_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strErr1_ViewState"] = value;
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
    private string strErr3
    {
        get
        {
            return ViewState["menuDetail_strErr3_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strErr3_ViewState"] = value;
        }
    }
    private string strErr4
    {
        get
        {
            return ViewState["menuDetail_strErr4_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strErr4_ViewState"] = value;
        }
    }
    private string strTip_modify_Confirm
    {
        get
        {
            return ViewState["menuDetail_strTip_modify_Confirm_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip_modify_Confirm_ViewState"] = value;
        }
    }
    private string strTip_ExsitMenuCode
    {
        get
        {
            return ViewState["menuDetail_strTip_ExsitMenuCode_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip_ExsitMenuCode_ViewState"] = value;
        }
    }
    private string strTip_ExsitOrder
    {
        get
        {
            return ViewState["menuDetail_strTip_ExsitOrder_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip_ExsitOrder_ViewState"] = value;
        }
    }
    private string strTip_del_Confirm
    {
        get
        {
            return ViewState["menuDetail_strTip_del_Confirm_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strTip_del_Confirm_ViewState"] = value;
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
    private string strSuccessTip1
    {
        get
        {
            return ViewState["menuDetail_strSuccessTip1_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strSuccessTip1_ViewState"] = value;
        }
    }
    private string strSuccessTip2
    {
        get
        {
            return ViewState["menuDetail_strSuccessTip2_ViewState"] as string;
        }
        set
        {
            ViewState["menuDetail_strSuccessTip2_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindMenuInfo(bool bFresh, String strMenuCode)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["MenuDetailViewState"] = GetMenuDetailInfo(strMenuCode);
            
        }
        else
        {
            if (ViewState["MenuDetailViewState"] == null)
            {
                ViewState["MenuDetailViewState"] = GetMenuDetailInfo(strMenuCode);
            }
        } 
        dt = (DataTable)ViewState["MenuDetailViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.TextBox1.Text = dt.Rows[0]["SMENUCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SMENUNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SMENUNAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["SURLDETAIL"].ToString();
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["SMENUTYPE"].ToString()));
            this.TextBox5.Text = dt.Rows[0]["SIMAGE"].ToString();
            String strParentCode = dt.Rows[0]["SPARENTCODE"].ToString();
            String strLevel = dt.Rows[0]["SLEVEL"].ToString();
            String strParentLevel = (int.Parse(strLevel)-1).ToString();
            String strOrder = dt.Rows[0]["SORDER"].ToString();
            String strParentOrder = strOrder.Substring(0, strOrder.Length-2);
            String strSelectedValue = strParentCode + "*" + strParentLevel + "*" + strParentOrder;
            this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue(strSelectedValue));
            this.TextBox6.Text = dt.Rows[0]["SLEVEL"].ToString();
            this.TextBox7.Text = strParentOrder;
            this.TextBox8.Text = strOrder.Substring(strOrder.Length - 2, 2);
            this.ddList_ShowLocation.SelectedIndex = this.ddList_ShowLocation.Items.IndexOf(this.ddList_ShowLocation.Items.FindByValue(dt.Rows[0]["SSHOWLOCATION"].ToString()));
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue(dt.Rows[0]["BISSTOP"].ToString()));
            if (strLevel.Equals("0"))//如果是根目录，则禁止选择上级目录
            {
                this.DropDownList3.Enabled = false;
                this.TextBox8.Enabled = false;
            }
        }
    }
    #endregion

    #region 获取相应栏目的明细信息
    /// <summary>
    /// 获取菜单定义表数据
    /// </summary>
    /// <returns></returns>
    public DataTable GetMenuDetailInfo(String strMenuCode)
    {
        MenuManagerBll bllMenu = new MenuManagerBll();
        DataTable dtMenuInfo = bllMenu.GetMenuInfoByMenuCode(strMenuCode);
        return dtMenuInfo;
    }
    #endregion

    #region 获取所有栏目信息
    /// <summary>
    /// 获取所有栏目信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetAllMenuInfo()
    {
        MenuManagerBll bllMenu = new MenuManagerBll();

        if (ViewState["AllMenuInfoViewState"] == null)
        {
            DataSet ds = bllMenu.GetAllMenuInfo();
            if (ds != null)
            {
                ViewState["AllMenuInfoViewState"] = ds.Tables[0];
            }
        }
        return (DataTable)ViewState["AllMenuInfoViewState"];
    }
    #endregion

    #region 保存操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(this.TextBox1.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox1.Focus();
            this.TextBox1.BackColor = Color.Red;
            //返回页面
            return;
        }
        else if (String.IsNullOrEmpty(this.TextBox2.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox2.Focus();
            this.TextBox2.BackColor = Color.Red;
            //返回页面
            return;
        }
        else if (String.IsNullOrEmpty(this.TextBox3.Text.Trim()))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox3.Focus();
            this.TextBox3.BackColor = Color.Red;
            //返回页面
            return;
        }else if (String.IsNullOrEmpty(this.TextBox8.Text))
        {
            this.AlertMessageBox(this.Page, this.strTip3);
            this.TextBox8.Focus();
            this.TextBox8.BackColor = Color.Red;
            //返回页面
            return;
        }
        else
        {
            try
            {
                String strMenuCode = this.TextBox1.Text.Trim();
                String strMenuName = this.TextBox2.Text.Trim();
                String strMenuNameChs = this.TextBox3.Text.Trim();
                String strUrlDetail = this.TextBox4.Text.Trim();
                String strMenuType = this.DropDownList1.SelectedValue.ToUpper();
                String strImage = this.TextBox5.Text.Trim();

                String[] strArr = this.DropDownList3.SelectedValue.Split('*');
                String strParentCode = strArr[0].ToString();
                String strLevel = this.TextBox6.Text.Trim();

                String strLastOrder = this.TextBox8.Text;
                if (strLastOrder.Length == 1)
                {
                    strLastOrder = "0" + strLastOrder;
                }
                String strOrder = this.TextBox7.Text + strLastOrder;

                String strIsstop = this.DropDownList2.SelectedValue.ToUpper();
                String strShowLocation = this.ddList_ShowLocation.SelectedValue.ToUpper();

                //设置root根的上级为空
                if (strLevel.Equals("0"))
                {
                    strParentCode = "";
                }
                MenuManagerBll bllMenu = new MenuManagerBll();
                if (ViewState["opKey"].Equals("modify"))
                {
                    if (!bllMenu.IsCanModifyByOrder(strMenuCode,strOrder))
                    {
                        this.AlertMessageBox(this.Page, this.strTip_ExsitOrder);
                    }
                    else
                    {
                        int iUpdateCount = bllMenu.updateMenuInfo(strMenuCode, strMenuName, strMenuNameChs, strUrlDetail, strMenuType, strImage, strParentCode, strLevel, strOrder, strIsstop, strShowLocation);
                        if (iUpdateCount > 0)
                        {
                            this.AlertMessageBox(this.Page, this.strSuccessTip1);
                            this.BindMenuInfo(true, strMenuCode);
                        }
                        else
                        {
                            this.AlertMessageBox(this.Page, this.strErr3);
                        }
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (bllMenu.IsExsitMenuCode(strMenuCode))
                    {
                        this.AlertMessageBox(this.Page, this.strTip_ExsitMenuCode);
                    }
                    else if (bllMenu.IsExsitOrderCode(strOrder))
                    {
                        this.AlertMessageBox(this.Page, this.strTip_ExsitOrder);
                    }
                    else
                    {
                        int iAddCount = bllMenu.AddMenuInfo(strMenuCode, strMenuName, strMenuNameChs, strUrlDetail, strMenuType, strImage, strParentCode, strLevel, strOrder, strShowLocation);
                        if (iAddCount > 0)
                        {
                            this.AlertMessageBox(this.Page, this.strSuccessTip1);
                            Response.Write("<script language=\"javascript\">window.parent.location.reload();;</script>");
                        }
                        else
                        {
                            this.AlertMessageBox(this.Page, this.strErr3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, this.strErr2);
            }
        }
    }
    #endregion

    #region 触发新增操作
    /// <summary>
    /// 触发新增操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        ViewState["opKey"] = "add";
        Response.Redirect("MenuDetail.aspx?menuCode=");
    }
    #endregion

    #region 返回列表操作
    /// <summary>
    /// 返回列表操作
    /// </summary>
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuList.aspx");
    }
    #endregion

    #region 删除操作
    /// <summary>
    /// 删除操作
    /// </summary>
    protected void Button4_Click(object sender, EventArgs e)
    {
        String strMenuCode = Request.Params["menuCode"].ToString();
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strMenuCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMenuCode);

        if ((strMenuCode != null) && (!strMenuCode.Equals("")))
        {
            try
            {
                MenuManagerBll bllMenu = new MenuManagerBll();
                if (!bllMenu.IsExsitSubLevel(strMenuCode))
                {
                    bllMenu.deleteMenuInfo(strMenuCode);
                    this.AlertMessageBox(this.Page, this.strSuccessTip2);
                    Response.Write("<script language=\"javascript\">window.parent.location.reload();;</script>");
                }
                else
                {
                    this.AlertMessageBox(this.Page, this.strTip5);
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, this.strErr4);
            }
        }
    }
    #endregion

    #region 加载父栏目的下拉选择框
    /// <summary>
    ///加载父栏目的下拉选择框
    /// </summary>
    private void CreateParentMenuList()
    {
        String strLanguage = UserLoginBll.Language;

        MenuManagerBll bllMenu = new MenuManagerBll();
        DataTable dt = this.GetAllMenuInfo();

        if ((dt != null) && (dt.Rows.Count > 0))
        {
            foreach (DataRow row in dt.Rows)
            {
                String strMenuCode = row["SMENUCODE"].ToString();
                String strMenuName = "";
                if (strLanguage.Equals("zh-cn"))
                {
                    strMenuName = row["SMENUNAMECN"].ToString();
                }
                else
                {
                    strMenuName = row["SMENUNAME"].ToString();
                }
                String strMenuLevel = row["SLEVEL"].ToString();
                String strOrder = row["SORDER"].ToString();
                int iMenuLevel = int.Parse(strMenuLevel);

                String strLine = "";
                for (int i = 0; i < iMenuLevel; i++)
                {
                    strLine = strLine + ">>>>>";
                }

                String strListCaption = strLine + strMenuName + "【" + strOrder + "】";
                String strListValue = strMenuCode + "*" + strMenuLevel + "*" + strOrder;

                ListItem lItem = new ListItem(strListCaption,strListValue);// 构造一项
                this.DropDownList3.Items.Add(lItem);
            }
        }

    }
    #endregion

}
