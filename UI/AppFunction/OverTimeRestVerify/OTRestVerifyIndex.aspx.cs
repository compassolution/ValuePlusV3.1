using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.AppFunction.OverTimeRestVerify;
using Com.ValuePlus.Common;

public partial class AppFunction_OverTimeRestVerify_OTRestUserList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //加载系统用户表所有数据到页面中的列表中显示
            try
            {
                //加载系统用户表所有数据到页面中的列表中显示
                this.BindUserListInfo(null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }

    #region 绑定用户列表数据
    /// <summary>
    /// 绑定用户列表数据
    /// </summary>
    private void BindUserListInfo(String strFilterCon)
    {
        if (ViewState["UserListViewState"] == null)
        {
            ViewState["UserListViewState"] = GetUserListInfo();
        }

        DataTable dt = (DataTable)ViewState["UserListViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.ListBox1.Items.Clear();

            DataRow[] drs = null;
            if (String.IsNullOrEmpty(strFilterCon))
            {
                drs = dt.Select("0=0");
            }
            else
            {
                drs = dt.Select(strFilterCon);
            }

            if (drs != null && drs.Length > 0)
            {
                foreach (DataRow dr in drs)
                {
                    ListItem lstItem = new ListItem();
                    String strStuffId = dr["STAFFID"].ToString();
                    String strUserName = "";
                    if (base.Language == "zh-cn")
                    {
                        strUserName = dr["DCNAMECHS"].ToString();
                    }
                    else
                    {
                        strUserName = dr["DCNAME"].ToString();
                    }
                    lstItem.Value = strStuffId;
                    lstItem.Text = strStuffId+"--->" + strUserName;
                    this.ListBox1.Items.Add(lstItem);
                }
            }
        }
    }
    #endregion

    #region 获取当前用户所能管理的用户信息
    /// <summary>
    /// 获取当前用户所能管理的用户信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetUserListInfo()
    {
        String strCurUserId = base.GetUserCode();
        OTRestVerifyBll bllVerify = new OTRestVerifyBll();
        DataSet dsUserInfo = bllVerify.GetStuffInfoByUserId(strCurUserId);
        DataTable dtUserInfo = new DataTable();
        if ((dsUserInfo != null) && (dsUserInfo.Tables.Count>0))
        {
            dtUserInfo = dsUserInfo.Tables[0];
        }
        return dtUserInfo;
    }
    #endregion

    #region ListBox框变更事件
    protected void ListBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        String strStuffId = this.ListBox1.SelectedValue;
        String strStuffName = this.ListBox1.SelectedItem.Text;
        this.frmVerify.Attributes["src"] = "OTRestVerify.aspx?" + UrlParamEncryption.EncryptionUrlParam("stuffId=" + strStuffId + "&stuffName=" + strStuffName);
    }
    #endregion

    #region 文本框过滤事件
    protected void TextBox1_TextChanged(object sender, System.EventArgs e)
    {
        String strStuffId = this.TextBox1.Text;
        if (!String.IsNullOrEmpty(strStuffId))
        {
            this.BindUserListInfo("STAFFID like '%" + strStuffId + "%' OR DCNAMECHS LIKE '%" + strStuffId + "%' OR DCNAME LIKE '%" + strStuffId + "%'");
        }
        else
        {
            this.BindUserListInfo(null);
        }
    }
    #endregion


}
