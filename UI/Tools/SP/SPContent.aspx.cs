using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.BLL.Query;
using Com.ValuePlus.Common.Security;

public partial class Tools_SP_SPContent : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.btnSave.Attributes.Add("onclick", "return confirm('Save,Are you sure?');");
            this.btnExcute.Attributes.Add("onclick", "return false;");
            this.btnExcuteSp.Attributes.Add("onclick", "return confirm('Execute Sp,Are you sure?');");

            if (Request.Params["type"] != null)
            {
                this.strObjectType = Request.Params["type"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strObjectType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strObjectType);
                if (this.strObjectType.ToLower().Equals("sp"))
                {
                    this.btnExcute.Visible = true;
                }
                else
                {
                    this.btnExcute.Visible = false;
                }
            }
            if (Request.Params["name"] != null)
            {
                this.strObjectName = Request.Params["name"].ToString();
                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strObjectName = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strObjectName);
                this.LoadCurPageData();
            }
        }
    }


    #region viewstate初始化区域
    private String strObjectName
    {
        get
        {
            return ViewState["SPContent_strObjectName_ViewState"] as String;
        }
        set
        {
            ViewState["SPContent_strObjectName_ViewState"] = value;
        }
    }
    private String strObjectType
    {
        get
        {
            return ViewState["SPContent_strObjectType_ViewState"] as String;
        }
        set
        {
            ViewState["SPContent_strObjectType_ViewState"] = value;
        }
    }
    private String strOldContent
    {
        get
        {
            return ViewState["SPContent_strOldContent_ViewState"] as String;
        }
        set
        {
            ViewState["SPContent_strOldContent_ViewState"] = value;
        }
    }
    #endregion

    /// <summary>
    /// 加载当前页面数据
    /// </summary>
    private void LoadCurPageData()
    {
        this.Label2.Text = this.strObjectName;
        this.txtContent.Text = this.GetSpContent(this.strObjectName);

        if(this.strObjectType.ToLower().Equals("sp"))
        {
            ArrayList arrParams = this.GetSpParams(this.strObjectName);
            String strParams = "";
            if ((arrParams != null) && (arrParams.Count > 0))
            {
                for (int i = 0; i < arrParams.Count; i++)
                {
                    if (i == 0)
                    {
                        strParams = strParams + " ''";
                    }
                    else
                    {
                        strParams = strParams + ",''";
                    }
                }
            }
            this.txtExcuteSp.Text = "EXEC " + this.strObjectName + " " + strParams;
        }
    }

    /// <summary>
    /// 获取存储过程内容体
    /// </summary>
    /// <param name="strObjectName"></param>
    /// <returns></returns>
    private String GetSpContent(String strObjectName)
    {
        StringBuilder sbContent = new StringBuilder();
        try
        {
            String strCommentSql = "select A.TEXT from syscomments A,sysobjects B where A.ID = B.ID AND B.[XTYPE] in ('P','FN','TF','V') AND B.[NAME] = '" + strObjectName + "'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strCommentSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    String strContent = dt.Rows[i]["text"].ToString();
                    strContent = strContent.Replace("CREATE", "ALTER").Replace("create", "alter");
                    sbContent.Append(strContent);
                }
                this.strOldContent = sbContent.ToString();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("获取存储过程内容体失败！Tools_SP_SPContent.GetSpContent(" + strObjectName + "");
        }
        return sbContent.ToString();
    }

    /// <summary>
    /// 获取存储过程参数集
    /// </summary>
    /// <param name="strObjectName"></param>
    /// <returns></returns>
    private ArrayList GetSpParams(String strObjectName)
    {
        //获取存储过程对应需传入的参数
        SPQueryBll bllSpQuery = new SPQueryBll();
        ArrayList arrListParam = bllSpQuery.GetSpParamInfo(strObjectName);
        return arrListParam;
    }

    #region 按钮操作
    /// <summary>
    /// 刷新操作
    /// </summary>
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.LoadCurPageData();
    }

    /// <summary>
    /// 保存操作
    /// </summary>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            String strContent = this.txtContent.Text.ToString();
            if (!String.IsNullOrEmpty(strContent))
            {
                SqlParamDao.ExecuteNonQueryBySql(strContent);
                this.LoadCurPageData();
                this.AlertMessageBox(this.Page, "Successfully Saved：" + this.strObjectName);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("Tools_SP_SPContent.btnSave_Click() Error!");
            this.AlertMessageBox(this.Page, "Save Failed!-->"+this.strObjectName);
        }
    }

    /// <summary>
    /// 执行存储过程操作
    /// </summary>
    protected void btnExcuteSp_Click(object sender, EventArgs e)
    {
        try
        {
            String strExecuteContent = this.txtExcuteSp.Text.ToString();
            if (!String.IsNullOrEmpty(strExecuteContent))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strExecuteContent);
                this.AlertMessageBox(this.Page, "Successfully Execute：" + this.strObjectName + "。影响记录数：" + iCount.ToString());
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            log.Error("Tools_SP_SPContent.btnExcuteSp_Click() Error!");
            this.AlertMessageBox(this.Page, "Execute Failed!-->" + this.strObjectName);
        }
    }
    #endregion
}
