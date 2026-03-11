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

public partial class Notice_NoticeView : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strNoticeKey = "";
            if (Request.Params["noticeKey"] != null)
            {
                this.strNoticeKey = Request.Params["noticeKey"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strNoticeKey = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strNoticeKey);
            }

            //初始化页面组件
            this.InitPageComponet();

            //加载公告信息
            this.FillNoticeInfo();
        }
    }

    #region viewstate初始化区域
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
        this.Button1.Text = rmLocResourceManager.GetString("btnClose");

        this.strTipFaildLoadData = rmLocResourceManager.GetString("tipFaildLoadData").ToString();
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
            if (!String.IsNullOrEmpty(this.strNoticeKey))
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

}
