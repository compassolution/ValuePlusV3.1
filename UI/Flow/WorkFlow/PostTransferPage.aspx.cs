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
using Com.ValuePlus.Flow.Entity;

public partial class Flow_WorkFlow_PostTransferPage : PageBase
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
                this.strCurPostCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "postId");
                this.strCurPathCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "pathId");
                this.strNextPostCode = UrlParamEncryption.GetUrlParamValue(htUrlQuery, "nextPostId");

                //加载流程信息区域信息
                if (!String.IsNullOrEmpty(this.strCurFlowId))
                {
                    this.BuildFlowInstanceInfo();
                }
                //加载下一岗位接收人员下拉列表
                if (!String.IsNullOrEmpty(this.strNextPostCode))
                {
                    this.BuildActorDDList(this.strNextPostCode);
                }
                //加载预留意见下拉列表
                if (!String.IsNullOrEmpty(this.strCurPathCode))
                {
                    this.BuildReservedMemoDDList(this.strCurPathCode);
                }
                //设置查看历史明细信息事件
                this.SetHisClick();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "加载当前流程转岗信息页面失败" + "');</script>");
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
    private string strNextPostCode
    {
        get
        {
            return ViewState["strNextPostCode_ViewState"] as string;
        }
        set
        {
            ViewState["strNextPostCode_ViewState"] = value;
        }
    }
    private string strCurPathCode
    {
        get
        {
            return ViewState["strCurPathCode_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPathCode_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 加载流程信息区域信息
    /// </summary>
    /// <param name="strNextPostCode"></param>
    private void BuildFlowInstanceInfo()
    {
        FlowInstanceBll bllInstance = new FlowInstanceBll();
        FlowPostBll bllPost = new FlowPostBll();
        this.lbFlowName.Text = bllInstance.GetFlowNameByFlowId(this.strCurFlowId, base.Language);
        this.lbCurPostName.Text = bllPost.GetPostNameByPostCode(this.strCurPostCode,base.Language);
        this.lbNextPostName.Text = bllPost.GetPostNameByPostCode(this.strNextPostCode, base.Language);
    }

    /// <summary>
    /// 加载下一岗位接收人员下拉列表
    /// </summary>
    /// <param name="strNextPostCode"></param>
    private void BuildActorDDList(String strNextPostCode)
    {
        FlowPostBll bll = new FlowPostBll();
        DataSet ds = bll.GetActorInfoByPostCode(strNextPostCode);
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {

            this.ddListNextPostUserName.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                String strUserId = ds.Tables[0].Rows[i]["SUSERID"].ToString();
                String strUserName = base.Language.Equals("en-us") ? ds.Tables[0].Rows[i]["SUSERNAME"].ToString() : ds.Tables[0].Rows[i]["SUSERNAMECN"].ToString();

                ListItem lItem = new ListItem(strUserName, strUserId);
                this.ddListNextPostUserName.Items.Insert(i, lItem);
            }
        }
    }

    /// <summary>
    /// 加载预留意见下拉列表
    /// </summary>
    /// <param name="strNextPostCode"></param>
    private void BuildReservedMemoDDList(String strPathCode)
    {
        FlowPostBll bll = new FlowPostBll();
        DataSet ds = bll.GetReservedMemoByPathCode(strPathCode);
        if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
        {
            this.ddListMemo.Items.Clear();
            ListItem lItemFirst = new ListItem("您可选择已经预留的转岗意见", "0");
            this.ddListMemo.Items.Insert(0, lItemFirst);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                String strMemoCode = ds.Tables[0].Rows[i]["SRESERVEDCODE"].ToString();
                String strMemoDesc = base.Language.Equals("en-us") ? ds.Tables[0].Rows[i]["SRESERVEDMEMO"].ToString() : ds.Tables[0].Rows[i]["SRESERVEDMEMOCN"].ToString();

                ListItem lItem = new ListItem(strMemoDesc, strMemoCode);
                this.ddListMemo.Items.Insert(i+1, lItem);
            }
        }
    }

    #region 页面按钮点击事件
    /// <summary>
    /// 返回按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Back_Click(object sender, EventArgs e)
    {
        String strParam = "flowId=" + this.strCurFlowId;
        Response.Redirect("FlowDealPage.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam));
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

    /// <summary>
    /// 流程转交按钮事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Transfer_Click(object sender, EventArgs e)
    {
        this.DoTransferToNextPost();
    }

    /// <summary>
    /// 流程岗位转交
    /// </summary>
    private void DoTransferToNextPost()
    {
        try
        {
            String strTransferIdea = this.txtTrasferIdea.Text;
            String strNextUserId = this.ddListNextPostUserName.SelectedValue.ToString();

            FlowPostBll bllPost = new FlowPostBll();
            //下岗位信息实体
            Entity_TB_FLOW_POST_DEFINE entityNextPost = bllPost.GetPostDefineEntityByPostCode(this.strNextPostCode);
            //当前用户信息实体
            Com.ValuePlus.Entity.UserInfo entityCurUser = base.GetUserInfo();
            //下岗位接收人信息实体
            Com.ValuePlus.BLL.User.UserBll bllUser = new Com.ValuePlus.BLL.User.UserBll();
            Com.ValuePlus.Entity.UserInfo entityNextUser = bllUser.GetUserInfoByUserId(strNextUserId);

            FlowTransferBll bllTransfer = new FlowTransferBll();
            int iCount = bllTransfer.TransferFlowToNextPost(this.strCurFlowId, strCurPathCode, strTransferIdea, entityNextPost, entityCurUser, entityNextUser);
            Response.Write("<script language=\"javascript\">alert('" + "流程转岗成功" + "');</script>");
            Response.Redirect("PendingList.aspx",false);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + "流程转岗失败" + "');</script>");
        }
    }
    
    #endregion

}
