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
using Com.ValuePlus.Entity.Regist;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.Regist;

public partial class Regist_RegistPage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ResourceManager rmLocResourceManager = base.GetResourceManager("RegistCopyright");
            this.Label_Title.Text = rmLocResourceManager.GetString("lbRegistPage");
            this.Label1.Text = rmLocResourceManager.GetString("lbClientName");
            this.Label2.Text = rmLocResourceManager.GetString("lbContactor");
            this.Label3.Text = rmLocResourceManager.GetString("lbContactWay"); 
            this.Label4.Text = rmLocResourceManager.GetString("lbGroupName");
            this.Label5.Text = rmLocResourceManager.GetString("lbRegistCode");
            this.Label6.Text = rmLocResourceManager.GetString("lbAssignCode"); 
            this.Button1.Text = rmLocResourceManager.GetString("btnRegist"); 
            this.Button2.Text = rmLocResourceManager.GetString("btReset");
            this.Button2.Attributes.Add("onclick", "document.form1.reset();return false;");

            this.strTipAssignCodeErr = rmLocResourceManager.GetString("tipAssignCodeErr");
            this.strTipSuccessAssign = rmLocResourceManager.GetString("tipSuccessAssign");
            this.strTipFailedAssign = rmLocResourceManager.GetString("tipFailedAssign");
            this.strTipNeedCompleteInfo = rmLocResourceManager.GetString("tipNeedCompleteInfo");
            //默认获取当前机器码填充到页面中
            this.FillRegistCode();
        }
    }

    #region viewstate初始化区域
    private string strTipAssignCodeErr
    {
        get
        {
            return ViewState["Regist_strTipAssignCodeErr_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipAssignCodeErr_ViewState"] = value;
        }
    }
    private string strTipSuccessAssign
    {
        get
        {
            return ViewState["Regist_strTipSuccessAssign_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipSuccessAssign_ViewState"] = value;
        }
    }
    private string strTipFailedAssign
    {
        get
        {
            return ViewState["Regist_strTipFailedAssign_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipFailedAssign_ViewState"] = value;
        }
    }
    private string strTipNeedCompleteInfo
    {
        get
        {
            return ViewState["Regist_strTipNeedCompleteInfo_ViewState"] as string;
        }
        set
        {
            ViewState["Regist_strTipNeedCompleteInfo_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 默认获取当前机器码填充到页面中
    /// </summary>
    private void FillRegistCode()
    {
        RegistBll bllRegist = new RegistBll();
        String strMachineCode = bllRegist.GetCurSeverMachineCode();
        this.TextBox5.Text = strMachineCode;
    }

   /// <summary>
   /// 点击注册
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        RegistInfoEntity entityRegist = new RegistInfoEntity();
        entityRegist.strSKEY = System.Guid.NewGuid().ToString();
        entityRegist.strSCLIENTNAME = this.TextBox1.Text;
        entityRegist.strSCONTACTOR = this.TextBox2.Text;
        entityRegist.strSCONTACTWAY = this.TextBox3.Text;
        entityRegist.strSGROUPNAME = this.TextBox4.Text;
        entityRegist.strSREGISTSTR = this.TextBox5.Text;
        entityRegist.strSASSIGNSTR = this.TextBox6.Text;

        try
        {
            if ((!String.IsNullOrEmpty(entityRegist.strSCLIENTNAME)) && (!String.IsNullOrEmpty(entityRegist.strSASSIGNSTR)))
            {
                int iCount = 0;
                DateTime dtCurDate = DateTime.Now;
                entityRegist.dtDTREGISTDATA = dtCurDate;
                RegistBll bllRegist = new RegistBll();
                iCount = bllRegist.addRegistInfo(entityRegist);
                if (iCount > 0)
                {
                    Response.Write("<script language=\"javascript\">alert('" + this.strTipSuccessAssign + "');</script>");
                    Response.Redirect("CopyrightInfo.aspx");
                }
                else if (iCount ==-1)//输入的授权码无效
                {
                    Response.Write("<script language=\"javascript\">alert('" + this.strTipAssignCodeErr + "');</script>");
                }
                else
                {
                    Response.Write("<script language=\"javascript\">alert('" + this.strTipFailedAssign + "');</script>");
                }
            }
            else
            {
                Response.Write("<script language=\"javascript\">alert('" + this.strTipNeedCompleteInfo + "');</script>");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + this.strTipFailedAssign + "');</script>");
        }
    }


}
