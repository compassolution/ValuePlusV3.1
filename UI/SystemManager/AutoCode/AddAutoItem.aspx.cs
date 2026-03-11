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
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.Web;
using System.Resources;
using System.Threading;

public partial class SystemManager_AutoCode_AddAutoItem : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("AutoCodeManager");

            //自定义设置页面文字显示的中英文字符串
            this.Label1.Text = rmLocResourceManager.GetString("lbAddAutoId");
            this.Label2.Text = rmLocResourceManager.GetString("lbAid");
            this.Label3.Text = rmLocResourceManager.GetString("lbAdesc");
            this.Label4.Text = rmLocResourceManager.GetString("lbAdescChs");
            this.Label5.Text = rmLocResourceManager.GetString("lbAprefix");
            this.Label6.Text = rmLocResourceManager.GetString("lbAdate");
            this.Label7.Text = rmLocResourceManager.GetString("lbAlength");
            this.Label8.Text = rmLocResourceManager.GetString("lbAnextNo");
            this.Label9.Text = rmLocResourceManager.GetString("lbAlastDate");
            this.Button1.Text = rmLocResourceManager.GetString("btnSave");
            this.Button2.Text = rmLocResourceManager.GetString("btnClose");

            //viewstate设置
            this.strTip_isEmpty = rmLocResourceManager.GetString("Tip_isEmpty");
            this.strTip_repeat = rmLocResourceManager.GetString("Tip_repeat");
            this.strErr_saveFailed = rmLocResourceManager.GetString("Err_saveFailed");
            this.strSuccess_save = rmLocResourceManager.GetString("Success_save");

        }
    }

    #region viewstate初始化区域
    private string strTip_isEmpty
    {
        get
        {
            return ViewState["autoList_strTip_isEmpty_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strTip_isEmpty_ViewState"] = value;
        }
    }
    private string strTip_repeat
    {
        get
        {
            return ViewState["autoList_strTip_repeat_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strTip_repeat_ViewState"] = value;
        }
    }
    private string strErr_saveFailed
    {
        get
        {
            return ViewState["autoList_strErr_saveFailed_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strErr_saveFailed_ViewState"] = value;
        }
    }
    private string strSuccess_save
    {
        get
        {
            return ViewState["autoList_strSuccess_save_ViewState"] as string;
        }
        set
        {
            ViewState["autoList_strSuccess_save_ViewState"] = value;
        }
    }
    #endregion

    #region 新增保存操作
    /// <summary>
    /// 新增保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strAid = this.TextBox1.Text.Trim();//自动编号项目编码
        String strAdesc = this.TextBox2.Text.Trim();//自动编号项目英文名
        String strAdescChs = this.TextBox3.Text.Trim();//自动编号项目中文名
        String strPrefix = this.TextBox4.Text.Trim();//前缀
        String strDateType = this.DropDownList1.SelectedValue.Trim(); //日期类型
        int iLength = 0;//长度
        if (!String.IsNullOrEmpty(this.TextBox5.Text.Trim()))
        {
            iLength = int.Parse(this.TextBox5.Text.Trim());//长度
        }
        int iNextNo = 0;//下一个编号
        if (!String.IsNullOrEmpty(this.TextBox6.Text.Trim()))
        {
            iNextNo = int.Parse(this.TextBox6.Text.Trim());//下一个编号
        }
        String strLastDate = this.TextBox7.Text.Trim();//最后日期


        if ((strAid == null) || (strAid.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip_isEmpty + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else if ((strAdesc == null) || (strAdesc.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip_isEmpty + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else if ((strAdescChs == null) || (strAdescChs.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip_isEmpty + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else
        {
            AutoCodeManagerBll bllAuto = new AutoCodeManagerBll();
            //首先判断自动编号项目编码的唯一性
            if (bllAuto.IsExsitAId(strAid))
            {
                Response.Write("<script language=\"javascript\">alert('" + this.strTip_repeat + "');</script>");
                Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
            }
            else
            {
                try
                {
                    bllAuto.AddAutoInfo(strAid, strAdesc, strAdescChs, strPrefix, strDateType, iLength, iNextNo, strLastDate);
                    Response.Write("<script language=\"javascript\">alert('" + this.strSuccess_save + "');</script>");
                    Response.Write("<script language=\"javascript\">window.opener.location.href='AutoCodeList.aspx';</script>");
                    Response.Write("<script language=\"javascript\">window.close();</script>");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Response.Write("<script language=\"javascript\">alert('" + this.strErr_saveFailed + "');</script>");
                }
            }
        }
    }
    #endregion


}
