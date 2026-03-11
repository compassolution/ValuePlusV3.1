using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL.Form;

public partial class Flow_FormManage_EditFormDefine : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFormId = Request.Params["formId"] == null ? "" : Request.Params["formId"].ToString();
            if ((strFormId != null) && (!strFormId.Equals("")))//修改当前表单定义信息
            {
                ViewState["opKey"] = "modify";//页面修改

                try
                {
                    this.TextBox1.ReadOnly = true;
                    //加载某一特定表单定义信息
                    this.BindFormDefineInfo(true, strFormId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Response.Write("<script language=\"javascript\">alert('" + "加载表单定义信息错误" + "');</script>");
                }
            }
            else//新增表单定义信息
            {
                ViewState["opKey"] = "add";//页面新增
                this.Button1.Attributes.Remove("onclick");
                this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue("0"));
                this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue("0"));
            }
        }
    }

    #region 绑定表单定义明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindFormDefineInfo(bool bFresh, String strFormId)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["FormDetailViewState"] = GetFormDetailInfo(strFormId);
        }
        else
        {
            if (ViewState["FormDetailViewState"] == null)
            {
                ViewState["FormDetailViewState"] = GetFormDetailInfo(strFormId);
            }
        }
        dt = (DataTable)ViewState["FormDetailViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.TextBox1.Text = dt.Rows[0]["SFORMCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SFORMNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SFORMAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["SFORMDESC"].ToString();
            this.TextBox5.Text = dt.Rows[0]["SFORMDESCCN"].ToString();
            this.TextBox6.Text = dt.Rows[0]["SPLUGINAFTERSVAE"].ToString();
            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["BISVERSION"].ToString()));
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue(dt.Rows[0]["BISSTOP"].ToString()));
        }
    }
    #endregion
    
    #region 获取相应表单定义的明细信息
    /// <summary>
    /// 获取相应表单定义的明细信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetFormDetailInfo(String strFormId)
    {
        FormDefineBll bll = new FormDefineBll();
        DataSet dsInfo = bll.GetFromDefineInfoByKey(strFormId);
        DataTable dt = new DataTable();
        if ((dsInfo != null) && (dsInfo.Tables.Count > 0))
        {
            dt = dsInfo.Tables[0];
        }
        return dt;
    }
    #endregion

    #region 保存操作
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        String strFormId = this.TextBox1.Text.Trim();
        String strFormCode = this.TextBox1.Text.Trim();
        String strFormName = this.TextBox2.Text.Trim();
        String strFormNameCN = this.TextBox3.Text.Trim();
        String strFormDesc = this.TextBox4.Text.Trim();
        String strFormDescCN = this.TextBox5.Text.Trim();
        String strFormPlugin = this.TextBox6.Text.Trim();

        String strIsVersion = this.DropDownList1.SelectedValue.ToUpper();
        String strIsStop = this.DropDownList2.SelectedValue.ToUpper();

        if ((strFormCode == null) || (strFormCode.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入表单定义编码" + "');</script>");
            //返回页面
        }
        else if ((strFormName == null) || (strFormName.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入表单定义英文名称" + "');</script>");
            //返回页面
        }
        else if ((strFormNameCN == null) || (strFormNameCN.Equals("")))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入表单定义中文名称" + "');</script>");
            //返回页面
        }
        else
        {
            try
            {
                FormDefineBll bllFormDefine = new FormDefineBll();
                if (ViewState["opKey"].Equals("modify"))
                {
                    if (bllFormDefine.IsExsitFormCode(strFormCode))
                    {
                        bllFormDefine.updateDicListDefine(strFormId, strFormCode, strFormName, strFormNameCN, strFormDesc, strFormDescCN, strFormPlugin, strIsVersion, strIsStop);
                        Response.Write("<script language=\"javascript\">alert('" + "表单定义信息更新成功" + "');</script>");
                        this.BindFormDefineInfo(true, strFormId);
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    if (bllFormDefine.IsExsitFormCode(strFormCode))
                    {
                        Response.Write("<script language=\"javascript\">alert('" + "已经存在相同的表单定义编码，请重新输入！" + "');</script>");
                    }
                    else
                    {
                        bllFormDefine.AddFormDefineInfo(strFormId, strFormCode, strFormName, strFormNameCN, strFormDesc, strFormDescCN, strFormPlugin, strIsVersion, strIsStop);
                        Response.Write("<script language=\"javascript\">alert('" + "表单定义信息新增成功" + "');</script>");
                        this.BindFormDefineInfo(true, strFormId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "保存表单定义信息出错！" + "');</script>");
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
        Response.Redirect("FormList.aspx");
    }
    #endregion
}
