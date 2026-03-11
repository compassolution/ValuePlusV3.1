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
using Com.ValuePlus.Flow.BLL.Form;
using Com.ValuePlus.Flow.BLL;
using Com.ValuePlus.Flow.DAL;

public partial class Flow_FormManage_FormList : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //加载表单定义表所有数据到页面中的列表中显示
            try
            {
                //获取表单字典表所有数据dataset
                this.BindDataGrid(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Response.Write("<script language=\"javascript\">alert('" + "err" + "');</script>");
            }
        }
    }

    #region 绑定数据
    /// <summary>
    /// 绑定数据
    /// </summary>
    /// <param name="bFresh">是否重新获取数据</param>
    private void BindDataGrid(bool bFresh)
    {
        if (bFresh)
        {
            ViewState["FormListViewState"] = GetDsFromDb();
        }
        else
        {
            if (ViewState["FormListViewState"] == null)
            {
                ViewState["FormListViewState"] = GetDsFromDb();
            }
        }
        this.DataGrid1.DataSource = ViewState["FormListViewState"];
        this.DataGrid1.DataBind();
    }
    #endregion

    #region 获取表单定义表数据
    /// <summary>
    /// 获取表单定义表数据
    /// </summary>
    /// <returns></returns>
    public DataSet GetDsFromDb()
    {
        FormDefineBll bll = new FormDefineBll();
        DataSet dsAll = bll.GetAllFromDefineList();
        return dsAll;
    }
    #endregion

    #region datagrid Item Created
    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton ImBtn = (ImageButton)e.Item.FindControl("Imagebutton1");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "编辑";
        }
        ImBtn = (ImageButton)e.Item.FindControl("Imagebutton2");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "删除";
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '是否确定要删除？ '); ");
        }
        ImBtn = (ImageButton)e.Item.FindControl("ImageButton3");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "结构";
        }
        ImBtn = (ImageButton)e.Item.FindControl("Imagebutton4");
        if (ImBtn != null)
        {
            ImBtn.ToolTip = "创建";
            ImBtn.Attributes.Add("onclick ", "return   window.confirm( '表单数据将被清除，是否确定要重新创建？ '); ");
        }

    }
    #endregion

    #region 删除表单定义表当前记录
    protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        ////获取DATAGRID的当前行
        string strFormId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();

        ////执行删除操作
        FormDefineBll bllFormDefine = new FormDefineBll();
        try
        {
            int iCount = bllFormDefine.deleteFormDefineInfo(strFormId);
            if (iCount > 0)
            {
                Response.Write("<script language=javascript> alert('" + "删除表单定义成功！" + "') </script>");
            }
            else
            {
                Response.Write("<script language=javascript> alert('" + "删除表单定义失败！" + "') </script>");
            }
            //返回页面
            this.BindDataGrid(true);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + "删除表单定义失败！" + "');</script>");
        }
    }
    #endregion

    #region DataGrid排序
    /// <summary>
    /// DataGrid排序
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void DataGrid1_SortCommand(object sender, DataGridSortCommandEventArgs e)
    {
        if (ViewState["FormListViewState"] != null)
        {
            DataSet ds = (DataSet)ViewState["FormListViewState"];
            DataView defaultView = ds.Tables[0].DefaultView;
            if (this.SortAscending)
            {
                defaultView.Sort = e.SortExpression;
            }
            else
            {
                defaultView.Sort = e.SortExpression + " DESC";
            }
            this.SortAscending = !this.SortAscending;

            //填充DataGrid的数据

            //设置全局dataset
            DataSet dsTemp = new DataSet();
            System.Data.DataTable dt = defaultView.ToTable();
            dsTemp.Tables.Add(dt.Copy());
            ViewState["FormListViewState"] = dsTemp;

            this.DataGrid1.DataSource = defaultView;
            this.DataGrid1.DataBind();
        }
    }
    #endregion

    #region 排序规则
    /// <summary>
    /// 排序规则
    /// </summary>
    private bool SortAscending
    {
        get
        {
            object obj2 = this.ViewState["SortAscending"];
            return ((obj2 == null) || ((bool)obj2));
        }
        set
        {
            this.ViewState["SortAscending"] = value;
        }
    }
    #endregion

    #region 对表单的系列操作定义
    protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Detail")
        {
            string strFormId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("EditFormDefine.aspx?formId=" + strFormId);
        }
        else if (e.CommandName == "Field")
        {
            string strFormId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            Response.Redirect("FormFieldList.aspx?formId=" + strFormId);
        }
        else if (e.CommandName == "Create")
        {
            string strFormId = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            this.CreateFormDBTable(strFormId);
            Response.Write("<script language=\"javascript\">alert('" + "创建表单结构成功！" + "');</script>");

        }
    }
    #endregion

    #region 新增按钮操作
    /// <summary>
    /// 新增按钮操作
    /// </summary>
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditFormDefine.aspx");
    }
    #endregion

    /// <summary>
    /// 根据表单ID创建对应的表对象
    /// </summary>
    /// <param name="strFormId"></param>
    private void CreateFormDBTable(String strFormId)
    {
        try
        {
            String strTableName = "TB_AF_" + strFormId.Trim().ToUpper();//表对象名
            StringBuilder strBuilderSql = new StringBuilder();
            StringBuilder strBuilderSql_Drop = new StringBuilder();
            StringBuilder strBuilderSql_Create = new StringBuilder();
            String strPKField = "";

            FormFieldBll bllFormField = new FormFieldBll();
            DataSet dsField = bllFormField.GetFromFieldInfoByFormId(strFormId);

            if ((dsField != null) && (dsField.Tables[0] != null))
            {
                strBuilderSql_Drop.Append("if exists (select 1\r\n");
                strBuilderSql_Drop.Append("            from  sysobjects\r\n");
                strBuilderSql_Drop.Append("           where  id = object_id('" + strTableName + "')\r\n");
                strBuilderSql_Drop.Append("            and   type = 'U')\r\n");
                strBuilderSql_Drop.Append("   drop table " + strTableName + "\r\n");
                strBuilderSql_Drop.Append(";\r\n");

                strBuilderSql_Create.Append("/*==============================================================*/\r\n");
                strBuilderSql_Create.Append("/* Table: " + strTableName + "                                  */\r\n");
                strBuilderSql_Create.Append("/*==============================================================*/\r\n");
                strBuilderSql_Create.Append("create table " + strTableName + " (\r\n");

                //循环读取字段
                DataRow[] drs = dsField.Tables[0].Select("1=1");
                if (drs != null && drs.Length > 0)
                {
                    String strFieldName = "";
                    String strFieldType = "";
                    float fFieldLength = new float();
                    String strIsNull = "null";

                    foreach (DataRow dr in drs)
                    {
                        strFieldName = dr["SFIELDCODE"].ToString();
                        strFieldType = DicDealBll.GetFieldTypeNameByCode(dr["SFIELDTYPECODE"].ToString(), "en-us");
                        fFieldLength = System.Convert.ToInt32(dr["NFIELDLENGTH"].ToString());
                        strIsNull = dr["BISNULL"].ToString() == "1" ? "null" : "not null";

                        strBuilderSql_Create.Append("   " + strFieldName + "      " + strFieldType + "(" + dr["NFIELDLENGTH"].ToString() + ")         " + strIsNull + ",\r\n");

                        if ((dr["BISKEY"] != null) && (dr["BISKEY"].ToString().Equals("1")))
                        {
                            strPKField = dr["SFIELDCODE"].ToString();
                        }
                    }
                    strBuilderSql_Create.Append("   constraint PK_" + strTableName + " primary key (" + strPKField + ")\r\n");
                }

                strBuilderSql_Create.Append(")\r\n");
                strBuilderSql_Create.Append(";\r\n");
            }

            strBuilderSql.Append(strBuilderSql_Drop);
            strBuilderSql.Append(strBuilderSql_Create);
            SqlParamDao.ExecuteScalarBySql(strBuilderSql.ToString());
        }
        catch (Exception ex)
        {
            log.Error(ex);
            Response.Write("<script language=\"javascript\">alert('" + ex.ToString() + "');</script>");
        }
    }

}
