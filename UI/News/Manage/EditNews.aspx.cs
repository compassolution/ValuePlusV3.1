using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using System.Drawing;

public partial class News_Manage_EditNews : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.trContentCn.Visible = true;
            this.trContentEn.Visible = false;
            if (Request.Params["plateId"] != null)
            {
                this.strPlateId = Request.Params["plateId"].ToString();


                try
                {
                    if (Request.Params["newsId"] != null)
                    {
                        this.strNewsId = Request.Params["newsId"].ToString();
                        this.GetNewsDetailInfo(this.strNewsId);
                    }
                    this.GetPlateDetailInfo(this.strPlateId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
    }

    #region viewstate初始化区域
    private string strPlateId
    {
        get
        {
            return ViewState["strPlateId_ViewState"] as string;
        }
        set
        {
            ViewState["strPlateId_ViewState"] = value;
        }
    }
    private string strNewsId
    {
        get
        {
            return ViewState["strNewsId_ViewState"] as string;
        }
        set
        {
            ViewState["strNewsId_ViewState"] = value;
        }
    }
    #endregion

    #region 获取相应版块的明细信息
    /// <summary>
    /// 获取相应版块的明细信息
    /// </summary>
    /// <returns></returns>
    public void GetPlateDetailInfo(String strPlateId)
    {
        String strSql = "SELECT * FROM TB_NEWS_PLATE WHERE SPLATEID = '" + strPlateId + "'";
        DataTable dtPlateInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtPlateInfo != null) && (dtPlateInfo.Rows.Count > 0))
        {
            this.lbPlateId.Text = dtPlateInfo.Rows[0]["SPLATEID"].ToString();
            this.lbName.Text = dtPlateInfo.Rows[0]["SPLATENAMECHS"].ToString();
            this.lbDesc.Text = dtPlateInfo.Rows[0]["SDESCCHS"].ToString();
        }
    }
    #endregion
    
    #region 获取相应新闻的明细信息
    /// <summary>
    /// 获取相应新闻的明细信息
    /// </summary>
    /// <returns></returns>
    public void GetNewsDetailInfo(String strNewsId)
    {
        String strSql = "SELECT * FROM TB_NEWS_CONTENT WHERE SNEWSID = '" + strNewsId + "'";
        DataTable dtNewsInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtNewsInfo != null) && (dtNewsInfo.Rows.Count > 0))
        {
            this.txtTitleCn.Text = dtNewsInfo.Rows[0]["STITLECHS"].ToString();
            this.txtTitleEn.Text = dtNewsInfo.Rows[0]["STITLE"].ToString();
            this.FreeTextBox_Cn.Text = dtNewsInfo.Rows[0]["SCONTENTCHS"].ToString();
            this.FreeTextBox_En.Text = dtNewsInfo.Rows[0]["SCONTENT"].ToString();
            this.ddListIsStop.SelectedIndex = this.ddListIsStop.Items.IndexOf(this.ddListIsStop.Items.FindByValue(dtNewsInfo.Rows[0]["BISSTOP"].ToString()));
        }
    }
    #endregion

    #region 触发操作
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.rdBtnListLanguage.SelectedIndex == 0)
        {
            this.trContentCn.Visible = true;
            this.trContentEn.Visible = false;
        }
        else
        {
            this.trContentCn.Visible = false;
            this.trContentEn.Visible = true;
        }
    }
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.strPlateId))
        {
            String strSql = "";
            try
            {
                String strTitle_Cn = this.txtTitleCn.Text;
                if (String.IsNullOrEmpty(strTitle_Cn))
                {
                    this.AlertMessageBox(this.Page, "请输入中文标题后重试");
                    this.txtTitleCn.Focus();
                    this.txtTitleCn.BackColor = Color.Red;
                    //返回页面
                    return;

                }
                String strTitle_En = this.txtTitleEn.Text;
                String strContent_Cn = this.FreeTextBox_Cn.Text;
                if (String.IsNullOrEmpty(strContent_Cn))
                {
                    this.AlertMessageBox(this.Page, "请输入正文内容后重试");
                    this.FreeTextBox_Cn.Focus();
                    this.FreeTextBox_Cn.BackColor = Color.Red;
                    //返回页面
                    return;

                }
                String strContent_En = this.FreeTextBox_En.Text;
                String strIsstop = this.ddListIsStop.SelectedValue ;

                if (String.IsNullOrEmpty(this.strNewsId))//新增操作
                {
                    String strNewsIdTemp = this.GetNewsId();
                    //this.divShow.InnerHtml = strContent;

                    strSql = "insert into TB_NEWS_CONTENT([SNEWSID],[SPLATEID],[SUSERID],[STITLE],[STITLECHS],[SCONTENT],[SCONTENTCHS],[DTTIME],[BISSTOP]) values(";
                    strSql = strSql + "'" + strNewsIdTemp + "','" + strPlateId + "','" + this.GetUserCode() + "','" + strTitle_En + "','" + strTitle_Cn + "','" + strContent_En + "','" + strContent_Cn + "','" + DateTime.Now + "','" + strIsstop + "')";
                    int iAddCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                    if (iAddCount > 0)
                    {
                        this.strNewsId = strNewsIdTemp;
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>RefreshList();</script>"); 

                    }
                    else
                    {
                        this.AlertMessageBox(this.Page, "Failed!");
                    }
                }
                else//修改操作
                {
                    strSql = "UPDATE TB_NEWS_CONTENT SET [STITLE] ='" + strTitle_En + "' , [STITLECHS]='" + strTitle_Cn + "' ,[SCONTENT] ='" + strContent_En + "' ,[SCONTENTCHS]='" + strContent_Cn + "' ,[BISSTOP]='" + strIsstop + "' where SNEWSID = '" + this.strNewsId + "'";
                    int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    if (iUpdateCount > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "", "<script language=javascript>RefreshList();</script>"); 
                    }
                    else
                    {
                        this.AlertMessageBox(this.Page, "Failed!");
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "Failed!");
            }
        }
    }
    #endregion

    private String GetNewsId()
    {
        String strId = "";
        DateTime dtNow = DateTime.Now;
        String strCurDate = dtNow.ToString("yyyyMMdd");
        String strSql = "SELECT MAX(SNEWSID) as SNEWSID  FROM TB_NEWS_CONTENT WHERE LEFT(SNEWSID,8) = '" + strCurDate + "'";
        DataTable dtPlateInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtPlateInfo != null) && (dtPlateInfo.Rows.Count > 0))
        {
            String strLast = dtPlateInfo.Rows[0]["SNEWSID"].ToString();
            if (!String.IsNullOrEmpty(strLast))
            {
                strLast = strLast.Substring(8,5);
                int i = int.Parse("1"+strLast) + 1;
                strId = strCurDate + i.ToString().Substring(1, i.ToString().Length-1);
            }
            else
            {
                strId = strCurDate + "00001";
            }
        }
        else
        {
            strId = strCurDate + "00001";
        }

        return strId;
    }

}
