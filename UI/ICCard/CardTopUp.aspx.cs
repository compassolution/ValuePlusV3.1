using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Data;
using System.Drawing;
using Com.ValuePlus.DAL;
using System.Collections;
using Com.ValuePlus.Common;

public partial class ICCard_CardTopUp : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.txtCardNo.Focus();
        if (!Page.IsPostBack)
        {
            this.btnTopUp.Attributes.Add("onclick", "return confirm('是否确定充值?') ;");

            
            ////解密传递字符串并获取对应参数值
            Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            this.curStrDCNO = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "DCNO");

            try
            {

            }
            catch (Exception ex)
            {
                this.AlertMessageBox(this.Page, "页面加载失败！");
                log.Error(ex);
            }

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
    #endregion

    #region 获取信息按钮事件
    /// <summary>
    /// 获取信息按钮事件
    /// </summary>
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtCardNo.Text))
        {
            try
            {
                this.SetCardInfoByCardNo(this.txtCardNo.Text);
                this.BindCardTopUpRecord(this.txtCardNo.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "获取员工卡信息失败！");
            }
        }
    }

    /// <summary>
    /// 获取员工及卡片信息
    /// </summary>
    /// <returns></returns>
    private void SetCardInfoByCardNo(String strCardNo)
    {
        //获取员工信息
        String strSql = "SELECT * FROM HRDOCU_1 where CARDNO = '" + strCardNo + "'";
        DataTable dtUserInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtUserInfo != null) && (dtUserInfo.Rows.Count > 0))
        {
            DataRow dr = dtUserInfo.Rows[0];
            this.lbStaff.Text = "(" + dr["DCNO"].ToString() + ")" + dr["DCNAMECHS"].ToString();
        }
        else
        {
            this.lbStaff.Text = "此卡尚未发放给任何员工，请先将此卡发放给某个员工！";
            this.lbStaff.ForeColor = Color.Red;
        } 
        
        //获取卡片信息
        strSql = "SELECT * from CARD_1 where CARDNO = '" + strCardNo + "'";
        DataTable dtCardInfo = SqlParamDao.GetDataTableBySql(strSql);
        if ((dtCardInfo != null) && (dtCardInfo.Rows.Count > 0))
        {
            DataRow drCard = dtCardInfo.Rows[0];
            this.lbBalance.Text = drCard["BALANCE"] == null ? "0.00" : drCard["BALANCE"].ToString();
            this.btnTopUp.Visible = true;
        }
        else
        {
            this.lbStaff.Text = "此卡尚未在系统中登记，请先将此卡发放给某个员工！";
            this.lbStaff.ForeColor = Color.Red;
            this.lbBalance.Text = "";
            this.btnTopUp.Visible = false;
        } 
    }
    
    /// <summary>
    /// 绑定列表数据
    /// </summary>
    private void BindCardTopUpRecord(String strCardNo)
    {
        String strSql = "SELECT * FROM CARD_3 where CARDNO = '" + strCardNo + "' ORDER BY SEQNO DESC";
        DataTable dtCardInfo = SqlParamDao.GetDataTableBySql(strSql);
        this.DataGrid1.DataSource = dtCardInfo;
        this.DataGrid1.DataBind();

    }
    #endregion

    #region 充值事件

    /// <summary>
    /// 确定充值按钮事件
    /// </summary>
    protected void btnTopUp_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtCardNo.Text))
        {
            String strMsg = "";
            try
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add("UserId", this.GetUserCode());
                hsTable.Add("CardNo", this.txtCardNo.Text);
                hsTable.Add("Amount", this.ddListAmount.SelectedValue);
                int iReturn = SqlParamDao.ExcuteSP("USP_CARD_TopUp", hsTable);
                strMsg = this.GetPageTipAfterExcuteSp("HRDOCU", "CardBinding", iReturn);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "充值失败！");
            }

            Page.ClientScript.RegisterStartupScript(typeof(Page), "doTopUpSuccess", "<script language=\"javascript\">document.getElementById('btnGetInfo').click();</script>");

        }
    }

    /// <summary>
    /// 根据执行存储过程的结果返回页面提示
    /// </summary>
    private String GetPageTipAfterExcuteSp(String strTid, String strAid, int iReturn)
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


    #endregion
}