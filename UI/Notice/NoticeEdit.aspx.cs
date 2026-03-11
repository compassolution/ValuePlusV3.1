using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Threading;
using System.Resources;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.Notice;
using Com.ValuePlus.Entity.Notice;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Common.Security;

public partial class Notice_NoticeEdit :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strOpType = "edit";
            this.strNoticeKey = "";
            if (Request.Params["opType"] != null)
            {//add表示新增，edit表示修改
                this.strOpType = Request.Params["opType"].ToString();
            }
            if (Request.Params["noticeKey"] != null)
            {
                this.strNoticeKey = Request.Params["noticeKey"].ToString();
            }

            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpType);
            this.strNoticeKey = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strNoticeKey);

            //初始化页面组件
            this.InitPageComponet();

        }
    }

    #region viewstate初始化区域
    private string strOpType
    {
        get
        {
            return ViewState["NoticeEdit_strOpType"] as string;
        }
        set
        {
            ViewState["NoticeEdit_strOpType"] = value;
        }
    }
    private string strNoticeKey
    {
        get
        {
            return ViewState["NoticeEdit_strNoticeKey"] as string;
        }
        set
        {
            ViewState["NoticeEdit_strNoticeKey"] = value;
        }
    }
    private string strTipIsToDelete
    {
        get
        {
            return ViewState["NoticeList_tipIsToDelete"] as string;
        }
        set
        {
            ViewState["NoticeList_tipIsToDelete"] = value;
        }
    }
    private string strTipFaildLoadData
    {
        get
        {
            return ViewState["NoticeList_tipFaildLoadData"] as string;
        }
        set
        {
            ViewState["NoticeList_tipFaildLoadData"] = value;
        }
    }
    private string strTipSuccessAdd
    {
        get
        {
            return ViewState["NoticeList_tipSuccessAdd"] as string;
        }
        set
        {
            ViewState["NoticeList_tipSuccessAdd"] = value;
        }
    }
    private string strTipSuccessModify
    {
        get
        {
            return ViewState["NoticeList_tipSuccessModify"] as string;
        }
        set
        {
            ViewState["NoticeList_tipSuccessModify"] = value;
        }
    }
    private string strTipSuccessDelete
    {
        get
        {
            return ViewState["NoticeList_tipSuccessDelete"] as string;
        }
        set
        {
            ViewState["NoticeList_tipSuccessDelete"] = value;
        }
    }  
    private string strTipFaildDataOp
    {
        get
        {
            return ViewState["NoticeList_tipFaildDataOp"] as string;
        }
        set
        {
            ViewState["NoticeList_tipFaildDataOp"] = value;
        }
    }
    private string strTipInfoNotFull
    {
        get
        {
            return ViewState["NoticeList_tipInfoNotFull"] as string;
        }
        set
        {
            ViewState["NoticeList_tipInfoNotFull"] = value;
        }
    }
    #endregion

    #region 初始化页面组件
    /// <summary>
    /// 初始化页面组件
    /// </summary>
    private void InitPageComponet()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("PublicNotice");

        this.Label_title.Text = rmLocResourceManager.GetString("lbPageTitle");
        this.Label1.Text = rmLocResourceManager.GetString("lbNoticeTitle");
        this.Label2.Text = rmLocResourceManager.GetString("lbNoticeContent");
        this.Label3.Text = rmLocResourceManager.GetString("lbPublishor");
        this.Label4.Text = rmLocResourceManager.GetString("lbPublishDate");
        this.Label5.Text = rmLocResourceManager.GetString("lbSource");
        this.Label6.Text = rmLocResourceManager.GetString("lbIsStop");

        this.Button1.Text = rmLocResourceManager.GetString("btnSave");
        this.Button2.Text = rmLocResourceManager.GetString("btnDelete");
        this.Button3.Text = rmLocResourceManager.GetString("btnBackList");
        this.Button2.Attributes.Add("onclick", "return window.confirm('" + rmLocResourceManager.GetString("tipIsToDelete").ToString() + "');");

        this.strTipInfoNotFull = rmLocResourceManager.GetString("tipInfoNotFull").ToString();
        this.strTipIsToDelete = rmLocResourceManager.GetString("tipIsToDelete").ToString();
        this.strTipFaildLoadData = rmLocResourceManager.GetString("tipFaildLoadData").ToString();
        this.strTipSuccessAdd = rmLocResourceManager.GetString("tipSuccessAdd").ToString();
        this.strTipSuccessModify = rmLocResourceManager.GetString("tipSuccessModify").ToString();
        this.strTipSuccessDelete = rmLocResourceManager.GetString("tipSuccessDelete").ToString();
        this.strTipFaildDataOp = rmLocResourceManager.GetString("tipFaildDataOp").ToString();

        if (!String.IsNullOrEmpty(this.strOpType))
        {
            if (strOpType.Equals("add"))
            {
                DateTime dt = DateTime.Now;
                UserInfo infoUser = base.GetUserInfo();
                this.TextBox3.Text = infoUser.SACCOUNTID;
                this.TextBox4.Text = dt.ToString() ;
                this.Button2.Visible = false;
            }else if(strOpType.Equals("edit"))
            {
                this.FillNoticeInfo();
            }
        }
    }
    #endregion

    #region 加载公告信息
    /// <summary>
    /// 加载公告信息
    /// </summary>
    private void FillNoticeInfo()
    {
        try
        {
            if (!String.IsNullOrEmpty(this.strOpType))
            {
                if (strOpType.Equals("edit"))
                {
                    NoticeBll bllNotice = new NoticeBll();
                    NoticeEntity entity = bllNotice.GetNoticeEntityByKey(this.strNoticeKey);
                    if (entity != null)
                    {
                        this.TextBox1.Text = entity.strSTITLE.ToString();
                        this.TextBox2.Text = entity.strSCONTENT.ToString();
                        this.TextBox3.Text = entity.strSPUBLISHOR.ToString();
                        if (entity.dtDTPUBLISHTIME != null)
                        {
                            this.TextBox4.Text = entity.dtDTPUBLISHTIME.ToString();
                        }
                        this.TextBox5.Text = entity.strSSOURCE.ToString();
                        this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(entity.strBISSTOP.ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildLoadData + "');</script>");
        }
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
            Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
        }
        else if (String.IsNullOrEmpty(this.TextBox2.Text.Trim()))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
        }
        else if (String.IsNullOrEmpty(this.TextBox3.Text.Trim()))
        {
            Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
        }
        else
        {
            try
            {
                NoticeBll bllNotice = new NoticeBll();

                NoticeEntity entity = bllNotice.GetNoticeEntityByKey(this.strNoticeKey);
                entity.strSTITLE = this.TextBox1.Text.Trim();
                entity.strSCONTENT = this.TextBox2.Text.Trim();
                entity.strSPUBLISHOR = this.TextBox3.Text.Trim();
                entity.strSSOURCE = this.TextBox5.Text.Trim();
                entity.strBISSTOP = this.DropDownList1.SelectedValue.ToString();

                if (this.strOpType.Equals("edit"))
                {
                    if (!String.IsNullOrEmpty(this.strNoticeKey))
                    {
                        DateTime dt = DateTime.Now;
                        entity.strSEDITOR = this.TextBox3.Text.Trim();
                        entity.dtDTEDITTIME = dt;
                        int iUpdateCount = bllNotice.updateNoticeInfo(entity);
                        if (iUpdateCount > 0)
                        {
                            Response.Write("<script language=\"javascript\">alert('" + this.strTipSuccessModify + "');</script>");
                            //Response.Write("<script language=\"javascript\">window.parent.location.reload();;</script>");
                        }
                        else
                        {
                            Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
                        }
                    }
                }
                else if (this.strOpType.Equals("add"))
                {
                    DateTime dt = DateTime.Now;
                    entity.strSKEY = System.Guid.NewGuid().ToString();
                    entity.dtDTPUBLISHTIME = dt;
                    entity.strSEDITOR = "";
                    entity.dtDTEDITTIME = dt;

                    int iAddCount = bllNotice.AddNoticeInfo(entity);
                    if (iAddCount > 0)
                    {
                        Response.Write("<script language=\"javascript\">alert('" + this.strTipSuccessAdd + "');</script>");
                    }
                    else
                    {
                        Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
            }
        }
    }
    #endregion

    #region 删除操作
    /// <summary>
    /// 删除操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.strNoticeKey))
        {
            try
            {
                NoticeBll bllNotice = new NoticeBll();
                int iDeleteCount = bllNotice.deleteNoticeInfo(this.strNoticeKey);
                if (iDeleteCount > 0)
                {
                    Response.Write("<script language=\"javascript\">alert('" + this.strTipSuccessDelete + "');</script>");
                    Response.Redirect("NoticeList.aspx?opType=manage");
                }
                else
                {
                    Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + this.strTipFaildDataOp + "');</script>");
            }
        }
    }
    #endregion

    #region 返回列表操作
    /// <summary>
    /// 返回列表操作
    /// </summary>
    protected void Button3_Click(object sender, EventArgs e)
    {
        Response.Redirect("NoticeList.aspx?opType=manage");
    }
    #endregion


}
