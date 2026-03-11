using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using Com.ValuePlus.DAL;
using System.Collections;
using Com.ValuePlus.Common;

public partial class ICCard_CardBinding : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtCardNo.Focus();
        if (!Page.IsPostBack)
        {
            this.btnBinding.Attributes.Add("onclick", "return confirm('是否确定发放此卡?') ;");
            this.btnRecover.Attributes.Add("onclick", "return confirm('是否确定回收该家属卡?') ;");

            ////解密传递字符串并获取对应参数值
            Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            this.curStrDCNO = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "DCNO");
            this.curStrUSER = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "USER");

            try
            {
                if (this.curStrUSER.ToUpper().Equals("STAFF"))//员工
                {
                    this.spanTitle.InnerText = "员工卡发放";
                    this.lbCardNo.Text = "员工卡号";
                    this.lbDCNO.Text = "员工编号";
                    this.lbNAME.Text = "员工姓名";
                    this.btnRecover.Visible = false;
                    this.SetStaffUserInfo(this.curStrDCNO);
                }
                else if (this.curStrUSER.ToUpper().Equals("FM"))//员工家属
                {
                    this.spanTitle.InnerText = "员工家属卡发放";
                    this.lbCardNo.Text = "家属卡号";
                    this.lbDCNO.Text = "家属编号";
                    this.lbNAME.Text = "家属姓名";
                    this.btnRecover.Visible = true;
                    this.SetFmUserInfo(this.curStrDCNO);
                }
            }
            catch (Exception ex)
            {
                this.AlertMessageBox(this.Page, "页面加载失败！");
                log.Error(ex);
            }
            this.txtCardNo.Focus();
        }
    }

    #region viewstate初始化区域
    private string curStrDCNO
    {
        get
        {
            return ViewState["curStrDCNO"] as string;
        }
        set
        {
            ViewState["curStrDCNO"] = value;
        }
    }
    private string curStrUSER
    {
        get
        {
            return ViewState["curStrUSER"] as string;
        }
        set
        {
            ViewState["curStrUSER"] = value;
        }
    }
    private string curStrDCNOsCARDNO
    {
        get
        {
            return ViewState["curStrDCNOsCARDNO"] as string;
        }
        set
        {
            ViewState["curStrDCNOsCARDNO"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 获取员工用户信息
    /// </summary>
    /// <returns></returns>
    public void SetStaffUserInfo(String strDcno)
    {
        String strSql = "select A.*,B.CARDSEQ from HRDOCU_1 A LEFT JOIN CARD_1 B ON A.CARDNO = B.CARDNO where A.DCNO = '" + strDcno + "'";
        DataTable dtUserInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtUserInfo != null) && (dtUserInfo.Rows.Count > 0))
        {
            DataRow dr = dtUserInfo.Rows[0];
            this.curStrDCNOsCARDNO = dr["CARDNO"] == null ? "" : dr["CARDNO"].ToString();
            this.txtCardNo.Text = this.curStrDCNOsCARDNO;
            this.txtCardSeq.Text = dr["CARDSEQ"] == null ? "" : dr["CARDSEQ"].ToString();
            this.txtDCNO.Text = strDcno;
            this.txtName.Text = dr["DCNAMECHS"].ToString();
        }
    }

    /// <summary>
    /// 获取员工家属信息
    /// </summary>
    /// <returns></returns>
    public void SetFmUserInfo(String strDcno)
    {
        String strSql = "select A.*,B.CARDSEQ from HRDOCU_2 A LEFT JOIN CARD_1 B ON A.CARDNO = B.CARDNO where A.FMNO = '" + strDcno + "'";
        DataTable dtUserInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtUserInfo != null) && (dtUserInfo.Rows.Count > 0))
        {
            DataRow dr = dtUserInfo.Rows[0];
            this.curStrDCNOsCARDNO = dr["CARDNO"] == null ? "" : dr["CARDNO"].ToString();
            this.txtCardNo.Text = this.curStrDCNOsCARDNO;
            this.txtCardSeq.Text = dr["CARDSEQ"] == null ? "" : dr["CARDSEQ"].ToString();
            this.txtDCNO.Text = strDcno;
            this.txtName.Text = dr["FMNAME"].ToString();
        }
    }
    
    /// <summary>
    /// 确定发放按钮事件
    /// </summary>
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        String strMsg = "";
        try
        {
            if (this.curStrUSER.ToUpper().Equals("STAFF"))//员工
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add("DCNO", this.curStrDCNO);
                hsTable.Add("UserId", this.GetUserCode());
                hsTable.Add("CardNo", this.txtCardNo.Text);
                hsTable.Add("CardSeq", this.txtCardSeq.Text);
                int iReturn = SqlParamDao.ExcuteSP("USP_CARD_BindingStaff", hsTable);
                strMsg = this.GetPageTipAfterExcuteSp("HRDOCU", "CardBinding", iReturn);
            }
            else if (this.curStrUSER.ToUpper().Equals("FM"))//家属
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add("FMNO", this.curStrDCNO);
                hsTable.Add("UserId", this.GetUserCode());
                hsTable.Add("CardNo", this.txtCardNo.Text);
                hsTable.Add("CardSeq", this.txtCardSeq.Text);
                int iReturn = SqlParamDao.ExcuteSP("USP_CARD_BindingFM", hsTable);
                strMsg = this.GetPageTipAfterExcuteSp("HRDOCU", "BindingFM", iReturn);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "发放卡片失败！");
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "doBindingSuccess", "<script language=\"javascript\">doRefresh('" + strMsg +"');</script>");

            //Response.Redirect("CardBinding.aspx?DCNO=" + this.curStrDCNO);
        //}
    }
    
    /// <summary>
    /// 回收家属卡
    /// </summary>
    protected void btnRecover_Click(object sender, EventArgs e)
    {
        String strMsg = "";
        try
        {
            Hashtable hsTable = new Hashtable();
            hsTable.Add("FMNO", this.curStrDCNO);
            hsTable.Add("USERID", this.GetUserCode());
            int iReturn = SqlParamDao.ExcuteSP("USP_CARD_RecoverFMCard", hsTable);
            strMsg = this.GetPageTipAfterExcuteSp("HRDOCU", "RecoverFMCard", iReturn);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            this.AlertMessageBox(this.Page, "回收家属卡失败！");
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "doRecoverSuccess", "<script language=\"javascript\">doRefresh('" + strMsg + "');</script>");

    }


    /// <summary>
    /// 根据执行存储过程的结果返回页面提示
    /// </summary>
    private String GetPageTipAfterExcuteSp(String strTid,String strAid,int iReturn)
    {
        String strMsgName = "";
        String strMsg = "";
        if (this.Language == "zh-cn")
        {
            strMsgName = "MESSCHS";
        }
        else
        {
            strMsgName = "MESSENG";
        }
        String strSql = "SELECT " + strMsgName + " FROM TB_HRTMPAR WHERE TID='" + strTid + "' AND AID='" + strAid + "' AND ECFROM<=" + iReturn + " AND ECTO>=" + iReturn;
        DataSet set = SqlParamDao.GetDataSetBySql(strSql);
        if ((set.Tables.Count > 0) && (set.Tables[0].Rows.Count > 0))
        {
            strMsg = set.Tables[0].Rows[0][0].ToString().Replace("@S@", iReturn.ToString());
        }
        else
        {
            if (iReturn < 0)
            {
                strMsg = "Excute Failed";
            }
            else
            {
                strMsg = "Successfully Excuted！";
            }
        }
        return strMsg;
    }

}