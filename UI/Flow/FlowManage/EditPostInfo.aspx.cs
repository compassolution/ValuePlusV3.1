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

public partial class Flow_FlowManage_EditPostInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strCurFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString().Trim();
            String strPostCode = Request.Params["postCode"] == null ? "" : Request.Params["postCode"].ToString().Trim();

            if ((strPostCode != null) && (!strPostCode.Equals("")))//修改当前流程岗位信息
            {
                ViewState["opKey"] = "modify";//页面修改

                try
                {
                    this.TextBox1.ReadOnly = true;
                    //加载某一特定流程岗位信息
                    this.BindFlowPostInfo(true, strPostCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    this.AlertMessageBox(this.Page, "加载流程岗位信息错误!");
                }
            }
            else//新增流程岗位信息
            {
                ViewState["opKey"] = "add";//页面新增
                this.Button1.Attributes.Remove("onclick");
                this.txtFlowCode.Text = this.strCurFlowId;
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
    #endregion

    #region 绑定流程岗位明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindFlowPostInfo(bool bFresh, String strPostCode)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["FlowPostViewState"] = GetFlowPostInfo(strPostCode);
        }
        else
        {
            if (ViewState["FlowPostViewState"] == null)
            {
                ViewState["FlowPostViewState"] = GetFlowPostInfo(strPostCode);
            }
        }
        dt = (DataTable)ViewState["FlowPostViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.txtFlowCode.Text = dt.Rows[0]["SFLOWCODE"].ToString();
            this.TextBox1.Text = dt.Rows[0]["SPOSTCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SPOSTNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SPOSTNAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["NWORKDAY"].ToString();
            this.TextBox5.Text = dt.Rows[0]["SPLUGIN_PRE"].ToString();
            this.TextBox6.Text = dt.Rows[0]["SPLUGIN_AFTER"].ToString();
            this.cbIsStartPost.Checked = dt.Rows[0]["BISSTARTPOST"].ToString() == "1" ? true : false;
            this.strCurFlowId = dt.Rows[0]["SFLOWCODE"].ToString();
        }
    }
    #endregion

    #region 获取相应流程岗位的明细信息
    /// <summary>
    /// 获取相应流程岗位的明细信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetFlowPostInfo(String strPostCode)
    {
        String strSql = "select * from TB_FLOW_POST_DEFINE where SPOSTCODE = '" + strPostCode +"'";
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
        String strPostCode = this.TextBox1.Text.Trim();
        String strPostName = this.TextBox2.Text.Trim();
        String strPostNameCN = this.TextBox3.Text.Trim();
        String strPostWorkDay = this.TextBox4.Text.Trim();
        String strPostPre = this.TextBox5.Text.Trim();
        String strPostAfter = this.TextBox6.Text.Trim();
        String strIsStartPost = this.cbIsStartPost.Checked ? "1" : "0";

        if ((strPostCode == null) || (strPostCode.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程岗位编码!");
            this.TextBox1.Focus();
            //返回页面
        }
        else if ((strPostName == null) || (strPostName.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程岗位英文名称!");
            this.TextBox2.Focus();
            //返回页面
        }
        else if ((strPostNameCN == null) || (strPostNameCN.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入流程岗位中文名称!");
            this.TextBox3.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strPostWorkDay))
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
                    if ((this.IsExsitStartPost())&&(strIsStartPost.Equals("1")))
                    {
                        this.AlertMessageBox(this.Page, "一个流程定义只能存在一个起始岗位!");
                    }
                    else if (this.IsExsitPostCode(strPostCode))
                    {
                        this.UpdatePostInfo(strPostCode, this.strCurFlowId, strPostName, strPostNameCN, strPostWorkDay, strPostPre, strPostAfter, strIsStartPost);
                        this.AlertMessageBox(this.Page, "流程岗位信息更新成功!");
                        this.BindFlowPostInfo(true, strPostCode);
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (this.IsExsitPostCode(strPostCode))
                    {
                        this.AlertMessageBox(this.Page, "已经存在相同的流程岗位编码，请重新输入!");
                    }
                    else if ((this.IsExsitStartPost()) && (strIsStartPost.Equals("1")))
                    {
                        this.AlertMessageBox(this.Page, "一个流程定义只能存在一个起始岗位!");
                    }
                    else
                    {
                        this.AddPostInfo(strPostCode, this.strCurFlowId, strPostName, strPostNameCN, strPostWorkDay, strPostPre, strPostAfter, strIsStartPost);
                        this.AlertMessageBox(this.Page, "流程岗位信息新增成功!");
                        this.BindFlowPostInfo(true, strPostCode);
                        ViewState["opKey"] = "modify";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "保存流程岗位信息出错!");
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
        Response.Redirect("FlowPostList.aspx?flowId="+this.strCurFlowId);
    }
    #endregion

    #region 判断是否已经存在相同的流程岗位编码
    /// <summary>
    /// 判断是否已经存在相同的流程岗位编码
    /// </summary>
    /// <returns></returns>
    public bool IsExsitPostCode(String strPostCode)
    {
        bool bIs = false;
        DataTable dt = this.GetFlowPostInfo(strPostCode);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            bIs = true;
        }
        return bIs;
    }
    #endregion

    #region 判断是否已经存在起始岗位
    /// <summary>
    /// 判断是否已经存在起始岗位
    /// </summary>
    /// <returns></returns>
    private bool IsExsitStartPost()
    {
        bool bIs = false;
        String strSql = "select * from TB_FLOW_POST_DEFINE where SFLOWCODE = '" + this.strCurFlowId + "' and BISSTARTPOST = '1'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            bIs = true;
        }
        return bIs;
    }
    #endregion

    #region 新增流程岗位的明细信息
    /// <summary>
    /// 获取相应流程岗位的明细信息
    /// </summary>
    /// <returns></returns>
    private void AddPostInfo(String strPostCode, String strFlowCode, String strPostName, String strPostNameCN, String strPostWorkDay, String strPostPre, String strPostAfter, String strIsStartPost)
    {
        String strSql = "insert into TB_FLOW_POST_DEFINE values('" + strPostCode + "','" + strFlowCode + "','" + strPostName + "','" + strPostNameCN + "'," + strPostWorkDay + ",'" + strPostPre + "','" + strPostAfter + "','" + strIsStartPost + "')";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion

    #region 根据流程岗位编码更新流程岗位的明细信息
    /// <summary>
    /// 根据流程岗位编码更新流程岗位的明细信息
    /// </summary>
    /// <returns></returns>
    private void UpdatePostInfo(String strPostCode, String strFlowCode, String strPostName, String strPostNameCN, String strPostWorkDay, String strPostPre, String strPostAfter, String strIsStartPost)
    {
        String strSql = "update TB_FLOW_POST_DEFINE set SPOSTCODE='" + strPostCode + "',SFLOWCODE='" + strFlowCode + "',SPOSTNAME='" + strPostName + "',SPOSTNAMECN='" + strPostNameCN + "',NWORKDAY=" + strPostWorkDay + ",SPLUGIN_PRE='" + strPostPre + "',SPLUGIN_AFTER='" + strPostAfter + "',BISSTARTPOST='" + strIsStartPost + "' WHERE SPOSTCODE='" + strPostCode + "' and SFLOWCODE='" + strFlowCode + "'";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion
}
