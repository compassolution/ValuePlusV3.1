using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL;
using Com.ValuePlus.Flow.DAL;
using Com.ValuePlus.Utils;

public partial class Flow_FlowManage_EditFlowDefine : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString();
            if ((strFlowId != null) && (!strFlowId.Equals("")))//修改当前流程定义信息
            {
                ViewState["opKey"] = "modify";//页面修改

                try
                {
                    this.TextBox1.ReadOnly = true;
                    //加载某一特定流程定义信息
                    this.BindFlowDefineInfo(true, strFlowId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    this.AlertMessageBox(this.Page, "加载流程定义信息错误!");
                }
            }
            else//新增流程定义信息
            {
                ViewState["opKey"] = "add";//页面新增
                this.Button1.Attributes.Remove("onclick");
                this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue("0"));
                this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue("0"));
                this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue("0"));
            }
        }
    }

    #region 绑定流程定义明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindFlowDefineInfo(bool bFresh, String strFlowId)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["FlowDefineViewState"] = GetFlowDefineInfo(strFlowId);
        }
        else
        {
            if (ViewState["FlowDefineViewState"] == null)
            {
                ViewState["FlowDefineViewState"] = GetFlowDefineInfo(strFlowId);
            }
        }
        dt = (DataTable)ViewState["FlowDefineViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.TextBox1.Text = dt.Rows[0]["SFLOWCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SFLOWNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SFLOWNAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["SFLOWDESC"].ToString();
            this.TextBox5.Text = dt.Rows[0]["SFLOWDESCCN"].ToString();
            this.TextBox6.Text = dt.Rows[0]["NWORKCOUNTDAY"].ToString();
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["BISNEEDACCEPT"].ToString()));
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue(dt.Rows[0]["BISACTIVEWITHSUB"].ToString()));
            this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue(dt.Rows[0]["BSTOP"].ToString()));
        }
    }
    #endregion

    #region 获取相应流程定义的明细信息
    /// <summary>
    /// 获取相应流程定义的明细信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetFlowDefineInfo(String strFlowId)
    {
        String strSql = "select * from TB_FLOW_DEFINE where SFLOWCODE = '" + strFlowId + "'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        return dt;
    }
    #endregion

    #region 保存操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strFlowId = this.TextBox1.Text.Trim();
        String strFlowCode = this.TextBox1.Text.Trim();
        String strFlowName = this.TextBox2.Text.Trim();
        String strFlowNameCN = this.TextBox3.Text.Trim();
        String strFlowDesc = this.TextBox4.Text.Trim();
        String strFlowDescCN = this.TextBox5.Text.Trim();
        String strFlowWorkDay = this.TextBox6.Text.Trim();

        String strIsAccept = this.DropDownList1.SelectedValue.ToUpper();
        String strIsWithSub = this.DropDownList2.SelectedValue.ToUpper();
        String strIsStop = this.DropDownList3.SelectedValue.ToUpper();

        if ((strFlowCode == null) || (strFlowCode.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程定义编码!");
            this.TextBox1.Focus();
            //返回页面
        }
        else if ((strFlowName == null) || (strFlowName.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程定义英文名称!");
            this.TextBox2.Focus();
            //返回页面
        }
        else if ((strFlowNameCN == null) || (strFlowNameCN.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程定义中文名称!");
            this.TextBox3.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strFlowWorkDay))
        {
            this.AlertMessageBox(this.Page, "流程工作日应该为正整数!");
            this.TextBox6.Focus();
            //返回页面
        }
        else
        {
            try
            {
                if (ViewState["opKey"].Equals("modify"))
                {
                    if (this.IsExsitFlowCode(strFlowCode))
                    {
                        this.UpdateFlowListDefine(strFlowCode, strFlowName, strFlowNameCN, strFlowDesc, strFlowDescCN, strFlowWorkDay, strIsAccept, strIsWithSub, strIsStop);
                        this.AlertMessageBox(this.Page, "流程定义信息更新成功!");
                        this.BindFlowDefineInfo(true, strFlowId);
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (this.IsExsitFlowCode(strFlowCode))
                    {
                        this.AlertMessageBox(this.Page, "已经存在相同的流程定义编码，请重新输入!");
                    }
                    else
                    {
                        this.AddFlowDefineInfo(strFlowCode, strFlowName, strFlowNameCN, strFlowDesc, strFlowDescCN, strFlowWorkDay, strIsAccept, strIsWithSub, strIsStop);

                        this.AlertMessageBox(this.Page, "流程定义信息新增成功!");
                        this.BindFlowDefineInfo(true, strFlowId);
                        ViewState["opKey"] = "modify";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "保存流程定义信息出错!");
            }
        }
    }
    #endregion

    #region 返回操作
    /// <summary>
    /// 返回操作
    /// </summary>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("FlowDefineList.aspx");
    }
    #endregion

    #region 判断是否已经存在相同的流程定义编码
    /// <summary>
    /// 判断是否已经存在相同的流程定义编码
    /// </summary>
    /// <returns></returns>
    public bool IsExsitFlowCode(String strFlowId)
    {
        bool bIs = false;
        DataTable dt = this.GetFlowDefineInfo(strFlowId);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            bIs = true;
        }
        else//还需要看在模板定义中是否存在
        {
            String strSql = "select * from TB_HRTMPH WHERE TID = '" + strFlowId + "'";
            dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                bIs = true;
            }
        }
        return bIs;
    }
    #endregion

    #region 新增流程定义的明细信息
    /// <summary>
    /// 获取相应流程定义的明细信息
    /// </summary>
    /// <returns></returns>
    private void AddFlowDefineInfo(String strFlowCode, String strFlowName, String strFlowNameCN, String strFlowDesc, String strFlowDescCN, String strFlowWorkDay, String strIsAccept, String strIsWithSub, String strIsStop)
    {
        String strSql = "insert into TB_FLOW_DEFINE ( SFLOWCODE,SFLOWNAME,SFLOWNAMECN,SFLOWDESC,SFLOWDESCCN,NWORKCOUNTDAY,BISNEEDACCEPT,BISACTIVEWITHSUB,BSTOP)  values('" + strFlowCode + "','" + strFlowName + "','" + strFlowNameCN + "','" + strFlowDesc + "','" + strFlowDescCN + "'," + strFlowWorkDay + ",'" + strIsAccept + "','" + strIsWithSub + "','" + strIsStop + "')";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion
    
    #region 根据流程定义编码更新流程定义的明细信息
    /// <summary>
    /// 根据流程定义编码更新流程定义的明细信息
    /// </summary>
    /// <returns></returns>
    private void UpdateFlowListDefine(String strFlowCode, String strFlowName, String strFlowNameCN, String strFlowDesc, String strFlowDescCN, String strFlowWorkDay, String strIsAccept, String strIsWithSub, String strIsStop)
    {
        String strSql = "update TB_FLOW_DEFINE set SFLOWCODE='" + strFlowCode + "',SFLOWNAME='" + strFlowName + "',SFLOWNAMECN='" + strFlowNameCN + "',SFLOWDESC='" + strFlowDesc + "',SFLOWDESCCN='" + strFlowDescCN + "',NWORKCOUNTDAY=" + strFlowWorkDay + ",BISNEEDACCEPT='" + strIsAccept + "',BISACTIVEWITHSUB='" + strIsWithSub + "',BSTOP='" + strIsStop + "' where SFLOWCODE='" + strFlowCode + "'";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion

}
