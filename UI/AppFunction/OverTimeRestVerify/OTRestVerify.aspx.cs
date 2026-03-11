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
using System.Resources;
using Com.ValuePlus.Common.Config;

public partial class AppFunction_OverTimeRestVerify_OTRestVerify : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //解密传递字符串并获取对应参数值
            Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
            this.strStuffId = UrlParamEncryption.GetUrlParamValue(htUrlQuery,"stuffId");
            this.strStuffName = UrlParamEncryption.GetUrlParamValue(htUrlQuery,"stuffName");

            try
            {
                //this.DoLanguageSetting();
                if (String.IsNullOrEmpty(this.strStuffId))
                {
                    this.lbTip1.Visible = true;
                    this.lbTip2.Visible = false;
                    this.tbAction.Visible = false;
                }
                else
                {
                    this.lbTip1.Visible = false;
                    this.lbTip2.Visible = true;
                    this.lbTip2.Text = this.lbTip2.Text.Replace("stuffName", this.strStuffName);
                    this.tbAction.Visible = true;
                    this.txtDate.Attributes.Add("onkeypress", "EnterDateTextBox()");
                    this.aConfirm.Attributes.Add("onclick ", "return window.confirm( '" + "提示：此操作将无法还原，确定是否进行核销？" + " '); ");

                    //获取9999-12-12的DateTime格式
                    DateTime dtFrom = new DateTime(9999,12,12);
                    //绑定员工加班信息列表数据
                    this.BindOTDataGrid(true, this.strStuffId, dtFrom);
                    //绑定员工调休信息列表数据
                    this.BindLVDataGrid(true, this.strStuffId, dtFrom);
                    //设置加班及调休的合计数显示
                    this.SetOTAndLVTotal();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + strStuffId + "');</script>");
            }
        }

    }

    #region viewstate初始化区域
    private string strStuffId
    {
        get
        {
            return ViewState["strStuffId"] as string;
        }
        set
        {
            ViewState["strStuffId"] = value;
        }
    }
    private string strStuffName
    {
        get
        {
            return ViewState["strStuffName"] as string;
        }
        set
        {
            ViewState["strStuffName"] = value;
        }
    }
    private float iOTTotal
    {
        get
        {
            if (this.ViewState["iOTTotal"] != null)
            {
                return (float)this.ViewState["iOTTotal"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iOTTotal"] = value;
        }
    }
    private float iOTHave
    {
        get
        {
            if (this.ViewState["iOTHave"] != null)
            {
                return (float)this.ViewState["iOTHave"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iOTHave"] = value;
        }
    }
    private float iLVTotal
    {
        get
        {
            if (this.ViewState["iLVTotal"] != null)
            {
                return (float)this.ViewState["iLVTotal"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iLVTotal"] = value;
        }
    }
    private float iLVHave
    {
        get
        {
            if (this.ViewState["iLVHave"] != null)
            {
                return (float)this.ViewState["iLVHave"];
            }
            return 0;
        }
        set
        {
            this.ViewState["iLVHave"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 页面语言设置以及基础设置
    /// </summary>
    private void DoLanguageSetting()
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("otRestVerify");
        this.lbTip1.Text = rmLocResourceManager.GetString("lbTip1");
        this.lbTip2.Text = rmLocResourceManager.GetString("lbTip2");
        this.lbTipOT.Text = rmLocResourceManager.GetString("lbTipOT");
        this.lbTipLV.Text = rmLocResourceManager.GetString("lbTipLV");
        this.aPreview.Text = rmLocResourceManager.GetString("btnPreview");
        this.aConfirm.Text = rmLocResourceManager.GetString("btnVerify");
        this.lbInputDate.Text = rmLocResourceManager.GetString("lbInputDate");
        this.LabelOTTotal.Text = rmLocResourceManager.GetString("lbOTTotal");
        this.LabelOTHave.Text = rmLocResourceManager.GetString("lbOTHave");
        this.LabelLVTotal.Text = rmLocResourceManager.GetString("lbLVTotal");
        this.LabelLVHave.Text = rmLocResourceManager.GetString("lbLVHave");

        this.DataGrid1.Columns[0].HeaderText = rmLocResourceManager.GetString("colOTDateF");
        this.DataGrid1.Columns[1].HeaderText = rmLocResourceManager.GetString("colOTDateT");
        this.DataGrid1.Columns[2].HeaderText = rmLocResourceManager.GetString("colOTHour");
        this.DataGrid1.Columns[3].HeaderText = rmLocResourceManager.GetString("colOTHave");
        this.DataGrid1.Columns[4].HeaderText = rmLocResourceManager.GetString("colOTType");
        this.DataGrid1.Columns[5].HeaderText = rmLocResourceManager.GetString("colOTNO");

        this.DataGrid2.Columns[0].HeaderText = rmLocResourceManager.GetString("colLVDateF");
        this.DataGrid2.Columns[1].HeaderText = rmLocResourceManager.GetString("colLVDateT");
        this.DataGrid2.Columns[2].HeaderText = rmLocResourceManager.GetString("colLVHour");
        this.DataGrid2.Columns[3].HeaderText = rmLocResourceManager.GetString("colLVHave");
        this.DataGrid2.Columns[4].HeaderText = rmLocResourceManager.GetString("colLVNO");
    }

    #region 绑定员工加班信息列表数据
    /// <summary>
    /// 绑定员工加班信息列表数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    /// <param name="strStuffId"></param>
    private void BindOTDataGrid(bool bFresh,String strStuffId,DateTime dtFrom)
    {
        if (bFresh)
        {
            ViewState["OTListViewState"] = GetOTInfoData(strStuffId, dtFrom);
        }
        else
        {
            if (ViewState["OTListViewState"] == null)
            {
                ViewState["OTListViewState"] = GetOTInfoData(strStuffId, dtFrom);
            }
        }
        DataSet ds = ViewState["OTListViewState"] as DataSet;
        if (this.DsIsHaveRecord(ds))
        {
            this.DataGrid1.DataSource = ds;
            this.DataGrid1.DataBind();
        }
    }
    #endregion

    #region 绑定员工调休信息列表数据
    /// <summary>
    /// 绑定员工调休信息列表数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    /// <param name="strStuffId"></param>
    private void BindLVDataGrid(bool bFresh, String strStuffId, DateTime dtFrom)
    {
        if (bFresh)
        {
            ViewState["LVListViewState"] = GetLVInfoData(strStuffId, dtFrom);
        }
        else
        {
            if (ViewState["LVListViewState"] == null)
            {
                ViewState["LVListViewState"] = GetLVInfoData(strStuffId, dtFrom);
            }
        }
        DataSet ds = ViewState["LVListViewState"] as DataSet;
        if (this.DsIsHaveRecord(ds))
        {
            this.DataGrid2.DataSource = ds;
            this.DataGrid2.DataBind();
        }
    }
    #endregion

    #region 根据员工编号获取其对应的加班信息数据
    /// <summary>
    /// 根据员工编号获取其对应的加班信息数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetOTInfoData(String strStuffId, DateTime dtFrom)
    {
        OTRestVerifyBll bll = new OTRestVerifyBll();
        return bll.GetOverTimeInfoByStuffId(strStuffId, dtFrom);
    }
    #endregion

    #region 根据员工编号获取其对应的调休信息数据
    /// <summary>
    /// 根据员工编号获取其对应的调休信息数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetLVInfoData(String strStuffId, DateTime dtFrom)
    {
        OTRestVerifyBll bll = new OTRestVerifyBll();
        return bll.GetLvInfoByStuffId(strStuffId, dtFrom);
    }
    #endregion

    #region 设置加班及调休的合计数显示
    /// <summary>
    /// 设置加班及调休的合计数显示
    /// </summary>
    private void SetOTAndLVTotal()
    {
        this.GetOTTotalHour();
        this.GetLVTotalHour();
        this.lbOTTotal.Text = this.iOTTotal.ToString();
        this.lbOTHave.Text = this.iOTHave.ToString();
        this.lbLVTotal.Text = this.iLVTotal.ToString();
        this.lbLVHave.Text = this.iLVHave.ToString();
    }
    #endregion

    #region 获取并计算加班小时数合计及其已核销
    /// <summary>
    /// 获取并计算加班小时数合计及其已核销
    /// </summary>
    private void GetOTTotalHour()
    {
        if (ViewState["OTListViewState"] != null)
        {
            DataSet ds = ViewState["OTListViewState"] as DataSet;
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow[] drs = dt.Select("0=0");
                float iTotal = 0;
                float iHave = 0;
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        float iTempTotal = 0;
                        float iTempHave = 0;
                        if (dr["OTTIME"] != null)
                        {
                            iTempTotal = float.Parse(dr["OTTIME"].ToString());
                        }
                        if (dr["OTCTIME"] != null)
                        {
                            iTempHave = float.Parse(dr["OTCTIME"].ToString());
                        }

                        iTotal = iTotal + iTempTotal;
                        iHave = iHave + iTempHave;
                    }
                }
                this.iOTTotal = iTotal;
                this.iOTHave = iHave;
            }
        }
    }
    #endregion

    #region 获取并计算调休小时数合计及其已核销
    /// <summary>
    /// 获取并计算调休小时数合计及其已核销
    /// </summary>
    private void GetLVTotalHour()
    {
        if (ViewState["LVListViewState"] != null)
        {
            DataSet ds = ViewState["LVListViewState"] as DataSet;
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                DataRow[] drs = dt.Select("0=0");
                float iTotal = 0;
                float iHave = 0;
                if (drs != null && drs.Length > 0)
                {
                    foreach (DataRow dr in drs)
                    {
                        float iTempTotal = 0;
                        float iTempHave = 0;
                        if (dr["LVTIME"] != null)
                        {
                            iTempTotal = float.Parse(dr["LVTIME"].ToString());
                        }
                        if (dr["LVSST"] != null)
                        {
                            iTempHave = float.Parse(dr["LVSST"].ToString());
                        }

                        iTotal = iTotal + iTempTotal;
                        iHave = iHave + iTempHave;
                    }
                }
                this.iLVTotal = iTotal;
                this.iLVHave = iHave;
            }
        }
    }
    #endregion

    #region 预览核销结果事件
    /// <summary>
    /// 预览核销结果事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void aPreview_Click(object sender, EventArgs e)
    {
        this.PreviewVerifyList();
    }

    /// <summary>
    /// 预览核销结果
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PreviewVerifyList()
    {
        String strDate = this.txtDate.Text;
        DateTime dtDate = new DateTime();
        if (!String.IsNullOrEmpty(strDate))
        {
            dtDate = DateTime.Parse(strDate);
            //绑定员工加班信息列表数据
            this.BindOTDataGrid(true, this.strStuffId, dtDate);
            //绑定员工调休信息列表数据
            this.BindLVDataGrid(true, this.strStuffId, dtDate);
            this.txtDate.Text = strDate;
            //设置加班及调休的合计数显示
            this.SetOTAndLVTotal();
        }
    }

    #endregion

    #region 操作核销事件
    /// <summary>
    /// 操作核销事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void aVerify_Click(object sender, EventArgs e)
    {
        this.DoVerifyOTLV();
    }

    /// <summary>
    /// 核销操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoVerifyOTLV()
    {
        String strDate = this.txtDate.Text;
        DateTime dtDate = System.DateTime.Parse("9999-12-12");
        if (!String.IsNullOrEmpty(strDate))
        {
            dtDate = DateTime.Parse(strDate);
        }

        try
        {
            String strSpName = BaseConfig.Instance.GetConfigValueByKey("SpName_OTRestVerify");
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam1_OTRestVerify"), this.strStuffId);
            hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam2_OTRestVerify"), dtDate);

            OTRestVerifyBll bllOTLV = new OTRestVerifyBll();
            int iCount = 0;
            if (!String.IsNullOrEmpty(strSpName))
            {
                iCount = bllOTLV.ExcuteSP(strSpName, hsTableParam);
            }
            this.PreviewVerifyList();
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion
    
    /// <summary>
    /// 判断DataSet是否存在记录
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    private bool DsIsHaveRecord(DataSet ds)
    {
        bool bIs = false;
        if (ds != null)
        {
            bIs = true;
        }
        return bIs;
    }

}
