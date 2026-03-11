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
using Com.ValuePlus.Flow.BLL.Flow;
using Com.ValuePlus.Common;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Flow.Config;

public partial class Flow_WorkFlow_FlowHisTransferInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                ////解密传递字符串并获取对应参数值
                Hashtable htUrlQuery = UrlParamEncryption.DecryptionUrlParam(base.Request.Url.Query.ToString());
                this.strCurFlowId = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "flowId");
                if (!String.IsNullOrEmpty(this.strCurFlowId))
                {
                    FlowInstanceBll bllInstance = new FlowInstanceBll();
                    this.lbFlowName.Text = bllInstance.GetFlowNameByFlowId(this.strCurFlowId, base.Language);

                    //首先获取并绑定流程历史流转明细信息
                    this.BindTransferListDataGrid(true);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载流程历史流转明细信息失败" + "');</script>");
            }
        }
    }

    #region viewstate初始化区域
    private string strCurFlowId
    {
        get
        {
            return ViewState["strCurFlowId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFlowId_ViewState"] = value;
        }
    }
    private DataSet dsGridList
    {
        get
        {
            if (this.ViewState["DsHisTransferListViewState"] == null)
            {
                return new DataSet();
            }
            return (DataSet)this.ViewState["DsHisTransferListViewState"];
        }
        set
        {
            this.ViewState["DsHisTransferListViewState"] = value;
        }
    }
    #endregion

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindTransferListDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            this.dsGridList = GetHisTransferListDsFromDb();
        }
        else
        {
            if (this.dsGridList == null)
            {
                this.dsGridList = GetHisTransferListDsFromDb();
            }
        }
        this.DataGrid1.DataSource = this.dsGridList;
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 当前流程的流程历史流转明细信息数据
    /// <summary>
    /// 当前流程的流程历史流转明细信息数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetHisTransferListDsFromDb()
    {
        FlowTransferBll bll = new FlowTransferBll();
        DataSet dsAll = bll.GetAllTransferInfoByFlowId(this.strCurFlowId);
        return dsAll;
    }
    #endregion

}
