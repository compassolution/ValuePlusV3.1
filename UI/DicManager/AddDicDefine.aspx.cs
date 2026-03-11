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

public partial class DicManager_AddDicDefine : PageBase
{
    #region 页面加载程序,双语言设置
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("DicList");
            
            //自定义设置页面文字显示的中英文字符串
            this.Label1.Text = rmLocResourceManager.GetString("lb_addDicDefine");
            this.Label3.Text = rmLocResourceManager.GetString("lb_LID");
            this.Label4.Text = rmLocResourceManager.GetString("lb_LDesc");
            this.Label5.Text = rmLocResourceManager.GetString("lb_LDescChs");
            this.Button1.Text = rmLocResourceManager.GetString("btnSave");
            this.Button2.Text = rmLocResourceManager.GetString("btnClose");

            this.strTip1 = rmLocResourceManager.GetString("Tip1");
            this.strTip3 = rmLocResourceManager.GetString("Tip3");
            this.strErr1 = rmLocResourceManager.GetString("Err2");
            this.strSuccessTip = rmLocResourceManager.GetString("Success1");
            
        }
    }
    #endregion

    #region viewstate初始化区域
    private string strTip1
    {
        get
        {
            return ViewState["addDic_strTip1_ViewState"] as string;
        }
        set
        {
            ViewState["addDic_strTip1_ViewState"] = value;
        }
    }
    private string strTip3
    {
        get
        {
            return ViewState["addDic_strTip3_ViewState"] as string;
        }
        set
        {
            ViewState["addDic_strTip3_ViewState"] = value;
        }
    }
    private string strErr1
    {
        get
        {
            return ViewState["addDic_strErr1_ViewState"] as string;
        }
        set
        {
            ViewState["addDic_strErr1_ViewState"] = value;
        }
    }
    private string strSuccessTip
    {
        get
        {
            return ViewState["addDic_strSuccessTip_ViewState"] as string;
        }
        set
        {
            ViewState["addDic_strSuccessTip_ViewState"] = value;
        }
    }
    #endregion

    #region 新增保存操作
    /// <summary>
    /// 新增保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strLid = this.TextBox1.Text.Trim();//清单编码
        String strLDesc = this.TextBox2.Text.Trim();//清单英文名
        String strLdescChs = this.TextBox3.Text.Trim();//清单中文名

        if ((strLid == null) || (strLid.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip3 + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else if ((strLDesc == null) || (strLDesc.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip3 + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else if ((strLdescChs == null) || (strLdescChs.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTip3 + "');</script>");
            Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
        }
        else
        {
            DicManagerBll bllDic = new DicManagerBll();
            //首先判断清单编码的唯一性
            if (bllDic.IsExsitLID(strLid))
            {
                Response.Write("<script language=\"javascript\">alert('" + this.strTip1 + "');</script>");
                Response.Write("<script language=\"javascript\">window.location.href=window.location.href;</script>");
            }
            else
            {
                try
                {
                    bllDic.AddDicListInfo(strLid, strLDesc, strLdescChs, "0");
                    Response.Write("<script language=\"javascript\">alert('" + this.strSuccessTip + "');</script>");
                    Response.Write("<script language=\"javascript\">window.close();</script>");
                    Response.Write("<script language=\"javascript\">window.opener.location.href='DicDefineList.aspx';</script>");
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Response.Write("<script language=\"javascript\">alert('" + this.strErr1 + "');</script>");
                }
            }
        }
    }
    #endregion
}
