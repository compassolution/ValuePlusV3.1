using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL.Flow;
using Com.ValuePlus.Common;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Flow.Config;

public partial class Flow_WorkFlow_FlowDealPage : PageBase
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
                    //首先获取并绑定流程实例信息
                    this.BindCurInstanceInfo(this.strCurFlowId);
                    //在判断该流程是否已经接收
                    this.IsAcceptted = this.JudgeIsAcceptted(this.strCurFlowId);
                    //根据是否已经接收的标志，设置页面数据
                    this.SettingPageData(this.IsAcceptted);
                    //设置查看历史明细信息事件
                    this.SetHisClick();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载当前流程处理信息失败" + "');</script>");
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
    private string strCurPostCode
    {
        get
        {
            return ViewState["strCurPostCode_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPostCode_ViewState"] = value;
        }
    }
    private string strCurFlowBizState
    {
        get
        {
            return ViewState["strCurFlowBizState_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFlowBizState_ViewState"] = value;
        }
    }
    private bool IsAcceptted
    {
        get
        {
            if (ViewState["IsAcceptted_ViewState"] != null)
            {
                return (bool)ViewState["IsAcceptted_ViewState"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsAcceptted_ViewState"] = value;
        }
    }
    #endregion

    #region 根据是否已经接收的标志，设置页面数据
    /// <summary>
    /// 根据是否已经接收的标志，设置页面数据
    /// </summary>
    /// <param name="IsAcceptted"></param>
    private void SettingPageData(bool IsAcceptted)
    {
        if (IsAcceptted)
        {
            //获取并绑定岗位活动信息
            this.BuildActionList();
            //获取并绑定下一岗位路径信息
            this.BuildPathList();

            this.aAccept.Visible = false;
            this.aBack.Visible = false;
        }
        else
        {
            this.aAccept.Visible = true;
            this.aBack.Visible = true;
        }
    }
    #endregion

    #region 判断该流程是否已经接收，同时如果该流程定义不需要接收，则直接将接收标志进行变更成“已接收未转出”
    /// <summary>
    /// 判断该流程是否已经接收，同时如果该流程定义不需要接收，则直接将接收标志进行变更成“已接收未转出”
    /// </summary>
    /// <returns></returns>
    private bool JudgeIsAcceptted(String strFlowId)
    {
        bool IsAcceptted = false;
        FlowInstanceBll bllInstance = new FlowInstanceBll();
        Entity_TB_FLOW_DEFINE entityDefine = bllInstance.GetFlowDefineInfoEntityByFlowId(strFlowId);
        String strNeedAccept = entityDefine.BISNEEDACCEPT;
        //首先判断该流程定义是否需要接收
        if (!String.IsNullOrEmpty(strNeedAccept))
        {
            Entity_TB_FLOW_WORK_INSTANCE entityInstance = bllInstance.GetFlowInstanceInfoEntityByFlowId(strFlowId);
            if (strNeedAccept.Equals("0"))//不需要接收
            {
                IsAcceptted = true;
                if (!entityInstance.SFLOWMOVECODE.Equals(Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotTransferred")))
                {
                    //更新成“已接收未转出”状态
                    bllInstance.SetFlowMoveStateByFlowId(strFlowId, Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotTransferred").ToString());
                }

            }
            else//需要接收，则需要判断流程实例是否已经接收
            {
                String strMoveState = entityInstance.SFLOWMOVECODE;
                if (!String.IsNullOrEmpty(strMoveState))
                {
                    if (strMoveState.Equals(Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotAccepted")))//如果状态为已转出未接收
                    {
                        IsAcceptted = false;
                    }
                    else//其他状态表示已经接收
                    {
                        IsAcceptted = true;
                    }
                }
                else
                {
                    IsAcceptted = true;
                }

            }
        }
        else
        {
            IsAcceptted = true;
        }
        return IsAcceptted;
        
    }
    #endregion

    #region 获取当前流程实例的相关数据数据并绑定到页面显示
    /// <summary>
    /// 获取当前流程实例的相关数据数据并绑定到页面显示
    /// </summary>
    /// <returns></returns>
    private void BindCurInstanceInfo(String strFlowId)
    {
        FlowInstanceBll bll = new FlowInstanceBll();
        DataSet ds = (DataSet)bll.GetPreviousTrasferInfoDsByFlowId(strFlowId);
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
            this.lbFlowName.Text = base.Language.Equals("en-us") ? ds.Tables[0].Rows[0]["SWORKFLOWNAME"].ToString() : ds.Tables[0].Rows[0]["SWORKFLOWNAMECN"].ToString();
            this.lbPrePostName.Text = base.Language.Equals("en-us") ? ds.Tables[0].Rows[0]["SSOURPOSTNAME"].ToString() : ds.Tables[0].Rows[0]["SSOURPOSTNAMECN"].ToString();
            this.lbPrePostUserName.Text = base.Language.Equals("en-us") ? ds.Tables[0].Rows[0]["SSOURUSERNAME"].ToString() : ds.Tables[0].Rows[0]["SSOURUSERNAMECN"].ToString();
            if ((ds.Tables[0].Rows[0]["NNUMBER"].ToString().Equals("1")) && (ds.Tables[0].Rows[0]["BISCURSTEP"].ToString().Equals("1")))//起始岗位，即流程实例第一步
            {
                this.lbCurPostName.Text = base.Language.Equals("en-us") ? ds.Tables[0].Rows[0]["SSOURUSERNAME"].ToString() : ds.Tables[0].Rows[0]["SSOURUSERNAMECN"].ToString();
                this.strCurPostCode = ds.Tables[0].Rows[0]["SSOURPOSTCODE"].ToString();
            }
            else
            {
                this.lbCurPostName.Text = base.Language.Equals("en-us") ? ds.Tables[0].Rows[0]["SDESTPOSTNAME"].ToString() : ds.Tables[0].Rows[0]["SDESTPOSTNAMECN"].ToString();
                this.strCurPostCode = ds.Tables[0].Rows[0]["SDESTPOSTCODE"].ToString();
            }
            this.lbPrePostComment.Text = ds.Tables[0].Rows[0]["SMEMO"].ToString();
        }
        this.strCurFlowBizState = bll.GetFlowBizStateByFlowId(strFlowId);
    }
    #endregion

    #region 获取并绑定岗位活动信息
    /// <summary>
    /// 绑定岗位活动信息
    /// </summary>
    private void BuildActionList()
    {
        DataTable dtActionList = this.GetCurPostActionInfo(this.strCurPostCode);
        if ((dtActionList != null) && (dtActionList.Rows.Count > 0))
        {
            StringBuilder strBuilderAction = new StringBuilder();
            String strActionCode = "";//岗位活动编码
            String strActionName = "";//岗位活动名称
            String strActionUrl = "";//岗位活动对应链接
            strBuilderAction.Append("\r\n");

            for (int j = 0; j <= dtActionList.Rows.Count - 1; j++)
            {
                DataRow dr = dtActionList.Rows[j];
                strActionCode = dr["SFLOWACTIONCODE"].ToString();
                if (this.Language == "zh-cn")
                {
                    strActionName = dr["SFLOWACTIONNAMECN"].ToString();
                }
                else
                {
                    strActionName = dr["SFLOWACTIONNAME"].ToString();
                }
                strActionUrl = dr["SACTIONDETAIL"].ToString();

                String strParam = "flowId=" + this.strCurFlowId + "&postId=" + this.strCurPostCode+"&pageUrl="+strActionUrl;//链接参数
                strActionUrl = "?" + UrlParamEncryption.EncryptionUrlParam(strParam);

                if (dr["SBIZSTATUS"].ToString().Equals(this.strCurFlowBizState))
                {
                    strBuilderAction.Append("			<tr>\r\n");
                    strBuilderAction.Append("		        <td>\r\n");
                    strBuilderAction.Append("		            " + strActionName + "\r\n");
                    strBuilderAction.Append("		        </td>\r\n");
                    strBuilderAction.Append("		        <td>\r\n");
                    strBuilderAction.Append("		            <a id=\"Action" + j.ToString() + "\" class=\"a_Left\" href=\"FlowFormDealPage.aspx" + strActionUrl + "\">点击处理</a>\r\n");
                    strBuilderAction.Append("		        </td>\r\n");
                    strBuilderAction.Append("			</tr>\r\n");
                }
            }
            //在页面显示
            this.divActionArea.InnerHtml = strBuilderAction.ToString();
        }
    }

    /// <summary>
    /// 根据当前岗位编码获取ACTION列表
    /// </summary>
    /// <param name="strCurPostCode"></param>
    /// <returns></returns>
    private DataTable GetCurPostActionInfo(String strCurPostCode)
    {
        DataTable tb = new DataTable();
        FlowPostBll bllPost = new FlowPostBll();
        DataSet ds = bllPost.GetActionInfoByPostCode(strCurPostCode);
        if ((ds != null) && (ds.Tables.Count > 0))
        {
            tb = ds.Tables[0];
        }
        return tb;
    }
    #endregion

    #region 获取并绑定岗位路径信息
    /// <summary>
    /// 绑定岗位路径信息
    /// </summary>
    private void BuildPathList()
    {
        DataTable dtPathList = this.GetCurPostNextPostInfo(this.strCurPostCode);
        if ((dtPathList != null) && (dtPathList.Rows.Count > 0))
        {
            StringBuilder strBuilderPath = new StringBuilder();
            String strPathCode = "";//岗位路径编码
            String strPathDesc = "";//岗位路径描述
            String strNextPostCode = "";//下一岗位编码
            String strNextPostName = "";//下一岗位名称
            strBuilderPath.Append("\r\n");

            for (int j = 0; j <= dtPathList.Rows.Count - 1; j++)
            {
                DataRow dr = dtPathList.Rows[j];
                strPathCode = dr["SFLOWPATHCODE"].ToString();
                strNextPostCode = dr["SPOSTCODE_NEXT"].ToString();
                if (this.Language == "zh-cn")
                {
                    strNextPostName = dr["SPOSTNAMECN"].ToString();
                    strPathDesc = dr["SWAYDESCCN"].ToString();
                }
                else
                {
                    strNextPostName = dr["SPOSTNAME"].ToString();
                    strPathDesc = dr["SWAYDESC"].ToString();
                }
                if(String.IsNullOrEmpty(strPathDesc)){
                    strPathDesc = strNextPostName;
                }

                String strPathUrl = "PostTransferPage.aspx";
                String strParam = "flowId=" + this.strCurFlowId + "&postId=" + this.strCurPostCode + "&nextPostId=" + strNextPostCode + "&pathId=" + strPathCode;//链接参数
                strPathUrl = strPathUrl + "?" + UrlParamEncryption.EncryptionUrlParam(strParam);

                if (dr["SBIZSTATUS"].ToString().Equals(this.strCurFlowBizState))
                {
                    strBuilderPath.Append("			<tr>\r\n");
                    strBuilderPath.Append("		        <td>\r\n");
                    strBuilderPath.Append("		            " + strPathDesc + "\r\n");
                    strBuilderPath.Append("		        </td>\r\n");
                    strBuilderPath.Append("		        <td>\r\n");
                    strBuilderPath.Append("		            <a id=\"Path" + j.ToString() + "\" class=\"a_Left\" href=\"" + strPathUrl + "\">点击转交</a>\r\n");
                    strBuilderPath.Append("		        </td>\r\n");
                    strBuilderPath.Append("			</tr>\r\n");
                }
            }
            //在页面显示
            this.divPathArea.InnerHtml = strBuilderPath.ToString();
        }
    }
    /// <summary>
    /// 根据当前岗位编码获取下一岗位列表
    /// </summary>
    /// <param name="strCurPostCode"></param>
    /// <returns></returns>
    private DataTable GetCurPostNextPostInfo(String strCurPostCode)
    {
        DataTable tb = new DataTable();
        FlowPostBll bllPost = new FlowPostBll();
        DataSet ds = bllPost.GetNextPostInfoByPrePostCode(strCurPostCode);
        if ((ds != null) && (ds.Tables.Count > 0))
        {
            tb = ds.Tables[0];
        }
        return tb;
    }
    #endregion

    #region 页面按钮点击事件
    /// <summary>
    /// 返回按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Back_Click(object sender, EventArgs e)
    {
        Response.Redirect("PendingList.aspx",false);
    }

    /// <summary>
    /// 接收流程按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Accept_Click(object sender, EventArgs e)
    {
        FlowInstanceBll bllInstance = new FlowInstanceBll();
        //更新成“已接收未转出”状态
        bllInstance.SetFlowMoveStateByFlowId(this.strCurFlowId, Com.ValuePlus.SysParams.FlowConfigParamGetter.GetFlowConfigParamValue("FlowMoveState_NotTransferred").ToString());
        this.IsAcceptted = true;
        //重新加载页面
        this.SettingPageData(true);
    }

    /// <summary>
    /// 拒绝流程并退回按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Refuse_Click(object sender, EventArgs e)
    {
        FlowTransferBll bllTransfer = new FlowTransferBll();
        bllTransfer.ReturnFlowToPrePost(this.strCurFlowId);
        Response.Redirect("PendingList.aspx", false);

    }

    /// <summary>
    /// 查看流转信息按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SetHisClick()
    {
        String strParam = "flowId=" + this.strCurFlowId;
        String strUrl = "FlowHisTransferInfo.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);
        //Response.Redirect("FlowHisTransferInfo.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam));
        this.aHisInfo.Attributes.Add("onclick", "showOpenWindow('" + strUrl + "')");
    }

    #endregion

}
