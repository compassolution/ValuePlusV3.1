using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Flow.BLL.Form;
using Com.ValuePlus.Flow.BLL;
using Com.ValuePlus.Utils;

public partial class Flow_FormManage_FormFieldInfor : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strFormId = Request.Params["formId"] == null ? "" : Request.Params["formId"].ToString();
            String strFieldId = Request.Params["fieldId"] == null ? "" : Request.Params["fieldId"].ToString();
            this.strCurFormId = strFormId;
            this.strCurFieldId = strFieldId;

            //加载下拉框原始数据
            this.BuildFieldTypeDDList();
            this.BuildCtrlTypeDDList();
            this.BuildCtrlRightTypeDDList();

            if ((strFieldId != null) && (!strFieldId.Equals("")))//修改当前表单定义信息
            {
                ViewState["opKey"] = "modify";//页面修改

                try
                {
                    this.TextBox1.ReadOnly = true;
                    this.cbIsFkey.Enabled = false;
                    //加载某一特定表单定义信息
                    this.BindFormFieldInfo(true, strFieldId);
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
                this.cbIsFkey.Attributes.Add("onclick", "return showFkeyArea();");
                this.txtFkTable.Attributes.Add("onkeypress", "EnterTableNameTextBox()");
            }
        }
    }

    #region viewstate初始化区域
    private string strCurFormId
    {
        get
        {
            return ViewState["strCurFormId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFormId_ViewState"] = value;
        }
    }
    private string strCurFieldId
    {
        get
        {
            return ViewState["strCurFieldId_ViewState"] as string;
        }
        set
        {
            ViewState["strCurFieldId_ViewState"] = value;
        }
    }
    #endregion

    #region 绑定表单定义明细数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindFormFieldInfo(bool bFresh, String strFieldId)
    {
        DataTable dt = new DataTable();
        if (bFresh)
        {
            ViewState["FormFieldViewState"] = GetFormFieldInfo(strFieldId);
        }
        else
        {
            if (ViewState["FormFieldViewState"] == null)
            {
                ViewState["FormFieldViewState"] = GetFormFieldInfo(strFieldId);
            }
        }
        dt = (DataTable)ViewState["FormFieldViewState"];
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            this.TextBox1.Text = dt.Rows[0]["SFIELDCODE"].ToString();
            this.TextBox2.Text = dt.Rows[0]["SFIELDNAME"].ToString();
            this.TextBox3.Text = dt.Rows[0]["SFIELDNAMECN"].ToString();
            this.TextBox4.Text = dt.Rows[0]["NFIELDLENGTH"].ToString();
            this.TextBox5.Text = dt.Rows[0]["SFIELDPRECISION"].ToString();
            this.TextBox6.Text = dt.Rows[0]["SDEFAULTVALUE"].ToString();
            this.TextBox7.Text = dt.Rows[0]["NCTRLLENGTH"].ToString();
            this.TextBox8.Text = dt.Rows[0]["NORDER"].ToString();
            this.TextBox9.Text = dt.Rows[0]["SCTRLDSSQL"].ToString();
            this.TextBox10.Text = dt.Rows[0]["STIPDESC"].ToString();
            this.TextBox11.Text = dt.Rows[0]["STIPDESCCN"].ToString();

            this.DropDownList1.SelectedIndex = this.DropDownList1.Items.IndexOf(this.DropDownList1.Items.FindByValue(dt.Rows[0]["SFIELDTYPECODE"].ToString()));
            this.DropDownList2.SelectedIndex = this.DropDownList2.Items.IndexOf(this.DropDownList2.Items.FindByValue(dt.Rows[0]["SCTRLTYPECODE"].ToString()));
            this.DropDownList3.SelectedIndex = this.DropDownList3.Items.IndexOf(this.DropDownList3.Items.FindByValue(dt.Rows[0]["SCTRLRIGHTTYPECODE"].ToString()));

            this.CheckBox1.Checked = dt.Rows[0]["BISKEY"].ToString() == "0" ? false : true;
            this.CheckBox2.Checked = dt.Rows[0]["BISNULL"].ToString() == "0" ? false : true;
            this.CheckBox3.Checked = dt.Rows[0]["BISMAINVIEW"].ToString() == "0" ? false : true;
            this.CheckBox4.Checked = dt.Rows[0]["BISMUST"].ToString() == "0" ? false : true;

            this.cbIsFkey.Checked = dt.Rows[0]["BISFKEY"].ToString() == "0" ? false : true;
            if (!String.IsNullOrEmpty(dt.Rows[0]["SFKEYTABLE"].ToString()))
            {
                this.txtFkTable.Text = dt.Rows[0]["SFKEYTABLE"].ToString();
                this.BuildCoulmnDDList(dt.Rows[0]["SFKEYTABLE"].ToString());
                this.ddListFkField.SelectedIndex = this.ddListFkField.Items.IndexOf(this.ddListFkField.Items.FindByValue(dt.Rows[0]["SFKEYFIELD"].ToString()));
            }
        }
    }
    #endregion

    #region 获取相应表单定义的明细信息
    /// <summary>
    /// 获取相应表单定义的明细信息
    /// </summary>
    /// <returns></returns>
    public DataTable GetFormFieldInfo(String strFieldId)
    {
        FormFieldBll bll = new FormFieldBll();
        DataSet dsInfo = bll.GetFromFieldInfoByKey(strFieldId);
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
        String strFieldCode = this.TextBox1.Text.Trim();
        String strFieldName = this.TextBox2.Text.Trim();
        String strFieldNameCN = this.TextBox3.Text.Trim();
        String strFieldLength = this.TextBox4.Text.Trim();
        String strFieldDprecision = this.TextBox5.Text.Trim();
        String strFieldDefaultValue = this.TextBox6.Text.Trim();
        String strCtrlLength = this.TextBox7.Text.Trim();
        String strCtrlOrder = this.TextBox8.Text.Trim();
        String strCtrlDsSql = this.TextBox9.Text.Trim();
        String strTipDesc = this.TextBox10.Text.Trim();
        String strTipDescCN = this.TextBox11.Text.Trim();

        String strFieldTypeCode = this.DropDownList1.SelectedValue.ToUpper();
        String strCtrlTypeCode = this.DropDownList2.SelectedValue.ToUpper();
        String strCtrlRightTypeCode = this.DropDownList3.SelectedValue.ToString();

        String strIsKey = this.cbIsFkey.Checked ? "1" : "0";
        String strIsNull = this.CheckBox2.Checked ? "1" : "0";
        String strIsMainView = this.CheckBox3.Checked ? "1" : "0";
        String strIsMust = this.CheckBox4.Checked ? "1" : "0";

        String strIsFKey = this.CheckBox1.Checked ? "1" : "0";
        String strFkTableName = this.txtFkTable.Text.Trim();
        String strFkField = this.ddListFkField.SelectedValue.ToUpper();

        if (String.IsNullOrEmpty(strFieldCode))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入字段编码" + "');</script>");
            this.TextBox1.Focus();
            //返回页面
        }
        else if ((strIsFKey.Equals("1")) && (String.IsNullOrEmpty(strFkTableName)) && (String.IsNullOrEmpty(strFkField)))
        {
            Response.Write("<script language=\"javascript\">alert('" + "如果是外键，必须输入关联表名，且选择关联字段" + "');</script>");
            this.txtFkTable.Focus();
            //返回页面
        }
        else if (String.IsNullOrEmpty(strFieldName))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入字段英文名称" + "');</script>");
            this.TextBox2.Focus();
            //this.TextBox2.BackColor = System.Drawing.Color.Red;
            //返回页面
        }
        else if (String.IsNullOrEmpty(strFieldNameCN))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入字段中文名称" + "');</script>");
            this.TextBox3.Focus();
            //返回页面
        }
        else if (String.IsNullOrEmpty(strFieldLength))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入字段长度" + "');</script>");
            this.TextBox4.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strFieldLength))
        {
            Response.Write("<script language=\"javascript\">alert('" + "字段长度应该为正整数" + "');</script>");
            this.TextBox4.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strFieldDprecision))
        {
            Response.Write("<script language=\"javascript\">alert('" + "字段精度应该为正整数" + "');</script>");
            this.TextBox5.Focus();
            //返回页面
        }
        else if (String.IsNullOrEmpty(strCtrlLength))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入控件长度" + "');</script>");
            this.TextBox7.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strCtrlLength))
        {
            Response.Write("<script language=\"javascript\">alert('" + "控件长度应该为正整数" + "');</script>");
            this.TextBox7.Focus();
            //返回页面
        }
        else if (String.IsNullOrEmpty(strCtrlOrder))
        {
            Response.Write("<script language=\"javascript\">alert('" + "请输入控件显示顺序" + "');</script>");
            this.TextBox8.Focus();
            //返回页面
        }
        else if (!StringUtils.isPositiveInt(strCtrlOrder))
        {
            Response.Write("<script language=\"javascript\">alert('" + "控件显示顺序应该为正整数" + "');</script>");
            this.TextBox8.Focus();
            //返回页面
        }
        else if ((strIsKey.Equals("1")) && (strIsMust.Equals("0")))
        {
            Response.Write("<script language=\"javascript\">alert('" + "主键字段必须为必填项！" + "');</script>");
            this.CheckBox4.Checked = true;
            this.CheckBox4.Focus();
            //返回页面
        }
        else
        {
            try
            {
                FormFieldBll bllFormField = new FormFieldBll();
                if (ViewState["opKey"].Equals("modify"))
                {
                    if (bllFormField.IsExsitFieldCode(this.strCurFormId,strFieldCode))
                    {
                        bllFormField.updateFormFieldInfo(this.strCurFieldId, this.strCurFormId, strFieldCode, strFieldName, strFieldNameCN, strIsKey, strFieldTypeCode, strFieldLength, strFieldDprecision, strIsNull, strFieldDefaultValue, strCtrlTypeCode, strCtrlDsSql, strCtrlOrder, strIsMainView, strCtrlLength, strIsMust, strTipDesc, strTipDescCN, strCtrlRightTypeCode, strIsFKey, strFkTableName, strFkField);
                        Response.Write("<script language=\"javascript\">alert('" + "表单字段信息更新成功" + "');</script>");
                        this.BindFormFieldInfo(true, this.strCurFieldId);
                    }
                }
                else if (ViewState["opKey"].Equals("add"))
                {
                    this.strCurFieldId = Guid.NewGuid().ToString();
                    if (bllFormField.IsExsitFieldCode(this.strCurFormId, strFieldCode))
                    {
                        Response.Write("<script language=\"javascript\">alert('" + "在该表单中已经存在相同的字段编码定义，请重新输入！" + "');</script>");
                    }
                    else
                    {
                        bllFormField.AddFormFieldInfo(this.strCurFieldId, this.strCurFormId, strFieldCode, strFieldName, strFieldNameCN, strIsKey, strFieldTypeCode, strFieldLength, strFieldDprecision, strIsNull, strFieldDefaultValue, strCtrlTypeCode, strCtrlDsSql, strCtrlOrder, strIsMainView, strCtrlLength, strIsMust, strTipDesc, strTipDescCN, strCtrlRightTypeCode, strIsFKey, strFkTableName, strFkField);
                        Response.Write("<script language=\"javascript\">alert('" + "表单字段信息新增成功" + "');</script>");
                        this.BindFormFieldInfo(true, this.strCurFieldId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "保存表单字段信息出错！" + "');</script>");
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
        Response.Redirect("FormFieldList.aspx?formId="+this.strCurFormId);
    }
    #endregion

    #region 根据表名填充其字段
    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnLoadCoulmn_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(this.txtFkTable.Text))
        {
            this.BuildCoulmnDDList(this.txtFkTable.Text.ToString().Trim());
            this.divFKeyArea.Visible = true;
        }
    }

    /// <summary>
    /// 根据表名填充其字段
    /// </summary>
    /// <param name="strTableName"></param>
    private void BuildCoulmnDDList(String strTableName)
    {
        FormFieldBll bllFormField = new FormFieldBll();
        DataTable dt = bllFormField.GetCoulmnInfoFromTable(strTableName);
        if ((dt != null)&(dt.Rows.Count>0))
        {
            this.ddListFkField.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCoulmnName = dt.Rows[i]["columns"].ToString();
                ListItem lItem = new ListItem(strCoulmnName, strCoulmnName);
                this.ddListFkField.Items.Insert(i, lItem);
            }
        }

    }
    #endregion

    #region 填充字段类型下拉框
    /// <summary>
    /// 填充字段类型下拉框
    /// </summary>
    private void BuildFieldTypeDDList()
    {
        DataTable dt = DicDealBll.GetAllDicList("TB_DIC_FORM_FIELD_TYPE","0");
        if ((dt != null)&(dt.Rows.Count>0))
        {
            this.DropDownList1.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strFieldTypeCode = dt.Rows[i]["SFIELDTYPECODE"].ToString(); ;
                String strFieldTypeName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strFieldTypeName = dt.Rows[i]["SFIELDTYPENAMECN"].ToString();
                }
                else
                {
                    strFieldTypeName = dt.Rows[i]["SFIELDTYPENAME"].ToString();
                }
                ListItem lItem = new ListItem(strFieldTypeName, strFieldTypeCode);
                this.DropDownList1.Items.Insert(i, lItem);
            }
        }

    }
    #endregion

    #region 填充控件类型下拉框
    /// <summary>
    /// 填充控件类型下拉框
    /// </summary>
    private void BuildCtrlTypeDDList()
    {
        DataTable dt = DicDealBll.GetAllDicList("TB_DIC_FORM_CTRL_TYPE", "0");
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            this.DropDownList2.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCtrlTypeCode = dt.Rows[i]["SCTRLTYPECODE"].ToString(); ;
                String strCtrlTypeName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strCtrlTypeName = dt.Rows[i]["SCTRLTYPENAMECN"].ToString();
                }
                else
                {
                    strCtrlTypeName = dt.Rows[i]["SCTRLTYPENAME"].ToString();
                }
                ListItem lItem = new ListItem(strCtrlTypeName, strCtrlTypeCode);
                this.DropDownList2.Items.Insert(i, lItem);
            }
        }

    }
    #endregion

    #region 填充控件权限类型下拉框
    /// <summary>
    /// 填充控件权限类型下拉框
    /// </summary>
    private void BuildCtrlRightTypeDDList()
    {
        DataTable dt = DicDealBll.GetAllDicList("TB_DIC_FORM_CTRL_RIGHT_TYPE", "0");
        if ((dt != null) & (dt.Rows.Count > 0))
        {
            this.DropDownList3.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                String strCtrlTypeCode = dt.Rows[i]["SCTRLRIGHTTYPECODE"].ToString(); ;
                String strCtrlTypeName = "";
                if (base.Language.Equals("zh-cn"))
                {
                    strCtrlTypeName = dt.Rows[i]["SCTRLRIGHTTYPENAMECN"].ToString();
                }
                else
                {
                    strCtrlTypeName = dt.Rows[i]["SCTRLRIGHTTYPENAME"].ToString();
                }
                ListItem lItem = new ListItem(strCtrlTypeName, strCtrlTypeCode);
                this.DropDownList3.Items.Insert(i, lItem);
            }
        }

    }
    #endregion

    #region 外键关联表字段选择变更事件
    /// <summary>
    /// 外键关联表字段选择变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddListFkField_Click(object sender, EventArgs e)
    {
        this.TextBox1.Text = this.ddListFkField.SelectedValue.ToString();
    }
    #endregion


}
