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

public partial class Flow_FlowManage_EditPostActionInfo : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.strCurFlowId = Request.Params["flowId"] == null ? "" : Request.Params["flowId"].ToString().Trim();
            String strPostCode = Request.Params["postCode"] == null ? "" : Request.Params["postCode"].ToString().Trim();
            String strPostActionCode = Request.Params["actionCode"] == null ? "" : Request.Params["actionCode"].ToString().Trim();

            this.strCurPostId = strPostCode;
            this.txtFlowId.Text = this.strCurFlowId;
            this.txtFlowId.Enabled = false;
            this.txtPostCode.Text = strPostCode;
            this.txtPostCode.Enabled = false;
            if ((strPostActionCode != null) && (!strPostActionCode.Equals("")))//修改当前岗位活动信息
            {
                ViewState["opKey"] = "modify";//页面修改

                try
                {
                    this.TextBox1.ReadOnly = true;
                    //加载某一特定岗位活动信息
                    this.BindPostActionInfo(true, strPostActionCode);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    this.AlertMessageBox(this.Page, "加载岗位活动信息错误!");
                }
            }
            else//新增岗位活动信息
            {
                ViewState["opKey"] = "add";//页面新增
                this.Button1.Attributes.Remove("onclick");
                this.txtPostCode.Text = this.strCurPostId;
                this.TextBox7.Text = "10";
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
    private string strCurPostId
    {
        get
        {
            return ViewState["strCurPostId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurPostId_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定岗位活动明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindPostActionInfo(bool bFresh, String strPostCode)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["PostActionViewState"] = GetPostActionInfo(strPostCode);
        }
        else
        {
            if (ViewState["PostActionViewState"] == null)
            {
                ViewState["PostActionViewState"] = GetPostActionInfo(strPostCode);
            }
        }
        dt = (DataTable)ViewState["PostActionViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.txtPostCode.Text = dt.Rows[0]["SPOSTCODE"].ToString();
            this.TextBox1.Text = dt.Rows[0]["SFLOWACTIONCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SFLOWACTIONNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SFLOWACTIONNAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["SACTIONDETAIL"].ToString();
            this.TextBox5.Text = dt.Rows[0]["SPLUGIN_PRE"].ToString();
            this.TextBox6.Text = dt.Rows[0]["SPLUGIN_AFTER"].ToString();
            this.TextBox7.Text = dt.Rows[0]["NINDEX"].ToString();
            this.TextBox8.Text = dt.Rows[0]["SBIZSTATUS"].ToString();
        }
    }
    #endregion

    #region 获取相应岗位活动的明细信息
    /// <summary>
    /// 获取相应岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private DataTable GetPostActionInfo(String strPostActionCode)
    {
        String strSql = "select * from TB_FLOW_POST_ACTION where SFLOWACTIONCODE = '" + strPostActionCode + "' order by NINDEX";
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
        String strPostActionCode = this.TextBox1.Text.Trim();
        String strPostActionName = this.TextBox2.Text.Trim();
        String strPostActionNameCN = this.TextBox3.Text.Trim();
        String strPostActionDetail = this.TextBox4.Text.Trim();
        String strPostActionPre = this.TextBox5.Text.Trim();
        String strPostActionAfter = this.TextBox6.Text.Trim();
        String strPostActionIndex = this.TextBox7.Text.Trim();
        String strPostActionBiz = this.TextBox8.Text.Trim();

        if ((strPostActionCode == null) || (strPostActionCode.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入岗位活动编码!");
            this.TextBox1.Focus();
            //返回页面
        }
        else if ((strPostActionName == null) || (strPostActionName.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入岗位活动英文名称!");
            this.TextBox2.Focus();
            //返回页面
        }
        else if ((strPostActionNameCN == null) || (strPostActionNameCN.Equals("")))
        {
            this.AlertMessageBox(this.Page, "请输入岗位活动中文名称!");
            this.TextBox3.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strPostActionIndex))
        {
            this.AlertMessageBox(this.Page, "顺序号应该为正整数!");
            this.TextBox7.Focus();
            //返回页面
        }
        else
        {
            try
            {
                if (ViewState["opKey"].Equals("modify"))
                {
                    if (this.IsExsitPostActionCode(strPostActionCode))
                    {
                        this.UpdatePostActionInfo(strPostActionCode, this.strCurPostId, strPostActionName, strPostActionNameCN, strPostActionDetail, strPostActionIndex, strPostActionBiz,strPostActionPre, strPostActionAfter);
                        
                        this.AlertMessageBox(this.Page, "岗位活动信息更新成功!");
                        this.BindPostActionInfo(true, this.strCurPostId);
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (this.IsExsitPostActionCode(strPostActionCode))
                    {
                        this.AlertMessageBox(this.Page, "已经存在相同的岗位活动编码，请重新输入!");;
                    }
                    else
                    {
                        this.AddPostActionInfo(strPostActionCode, this.strCurPostId, strPostActionName, strPostActionNameCN, strPostActionDetail, strPostActionIndex, strPostActionBiz, strPostActionPre, strPostActionAfter);
                        
                        this.AlertMessageBox(this.Page, "岗位活动信息新增成功!");
                        this.BindPostActionInfo(true, this.strCurPostId);
                        ViewState["opKey"] = "modify";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.AlertMessageBox(this.Page, "保存岗位活动信息出错!");
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
        Response.Redirect("PostActionList.aspx?flowId="+this.strCurFlowId+"&postCode=" + this.strCurPostId);
    }
    #endregion

    #region 判断是否已经存在相同的岗位活动编码
    /// <summary>
    /// 判断是否已经存在相同的岗位活动编码
    /// </summary>
    /// <returns></returns>
    public bool IsExsitPostActionCode(String strPostActionCode)
    {
        bool bIs = false;
        DataTable dt = this.GetPostActionInfo(strPostActionCode);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            bIs = true;
        }
        return bIs;
    }
    #endregion

    #region 新增岗位活动的明细信息
    /// <summary>
    /// 获取相应岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private void AddPostActionInfo(String strPostActionCode, String strPostId, String strPostActionName, String strPostActionNameCN, String strPostActionDetail, String strPostActionIndex, String strPostActionBiz, String strPostActionPre, String strPostActionAfter)
    {
        String strSql = "insert into TB_FLOW_POST_ACTION ( SFLOWACTIONCODE,SPOSTCODE,SFLOWACTIONNAME,SFLOWACTIONNAMECN,SACTIONDETAIL,NINDEX,SBIZSTATUS,SPLUGIN_PRE,SPLUGIN_AFTER) values('" + strPostActionCode + "','" + strPostId + "','" + strPostActionName + "','" + strPostActionNameCN + "','" + strPostActionDetail + "'," + strPostActionIndex + ",'" + strPostActionBiz + "','" + strPostActionPre + "','" + strPostActionAfter + "')";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion

    #region 根据岗位活动编码更新岗位活动的明细信息
    /// <summary>
    /// 根据岗位活动编码更新岗位活动的明细信息
    /// </summary>
    /// <returns></returns>
    private void UpdatePostActionInfo(String strPostActionCode, String strPostId, String strPostActionName, String strPostActionNameCN, String strPostActionDetail, String strPostActionIndex, String strPostActionBiz, String strPostActionPre, String strPostActionAfter)
    {
        String strSql = "update TB_FLOW_POST_ACTION set SFLOWACTIONCODE='" + strPostActionCode + "',SPOSTCODE='" + strPostId + "',SFLOWACTIONNAME='" + strPostActionName + "',SFLOWACTIONNAMECN='" + strPostActionNameCN + "',SACTIONDETAIL='" + strPostActionDetail + "',NINDEX=" + strPostActionIndex + ",SBIZSTATUS='" + strPostActionBiz + "',SPLUGIN_PRE='" + strPostActionPre + "',SPLUGIN_AFTER='" + strPostActionAfter + "' where SFLOWACTIONCODE='" + strPostActionCode+"'";
        SqlParamDao.ExecuteScalarBySql(strSql);
    }
    #endregion
}
