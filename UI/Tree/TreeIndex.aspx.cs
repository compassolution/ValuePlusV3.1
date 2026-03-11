using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.DAL;

using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class Tree_TreeIndex : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack)
        {
            //树形模板编码
            if (base.Request.Params["TREE"] != null)
            {
                this.strTid = base.Request.Params["TREE"].ToString();
                this.strOpType = "readonly";
                if (base.Request.Params["OPTYPE"] != null)
                {
                    this.strOpType = base.Request.Params["OPTYPE"].ToString();
                }

                //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                this.strTid = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strTid);
                this.strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strOpType);

                this.LoadPageData();
            }
            else
            {
                this.AlertMessageBox(this.Page,"链接配置有误，请核对后重试！");
            }
        }
    }

    private void LoadPageData()
    {
        try
        {
            this.hfFieldTreeCode.Value = strTid;
            this.hfOpType.Value = this.strOpType;
            
            //如果是对树形结构的操作
            if ((strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")) || (strOpType.ToLower().Equals("add")))
            {
                String strParamString = "TREE=" + strTid + "&CODE=ROOT&OPTYPE=" + this.strOpType;
                strParamString = UrlParamEncryption.EncryptionUrlParam(strParamString);
                this.contentFrame.Attributes.Add("src", "TreeList.aspx?" + strParamString);
            }
            else
            {
                //其他页面操作链接
                String strSql1 = "select * from TREECONFIG_3 WHERE TID = '" + strTid + "' AND OPTYPE = '" + strOpType + "'";
                DataTable dt1 = SqlParamDao.GetDataTableBySql(strSql1);
                if ((dt1 != null) && (dt1.Rows.Count > 0))
                {
                    String strPageUrl = dt1.Rows[0]["OPPAGE"].ToString().Replace("%TREECODE%", "ROOT");
                    this.contentFrame.Attributes.Add("src", strPageUrl);
                }
            }

            String strSql = "SELECT * FROM TB_HRTREEH WHERE TID = '" + this.strTid + "' ";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                this.Label_Code.Text = this.strTid;
                this.Label_NameCn.Text = dt.Rows[0]["TDESCCHS"].ToString();
                this.Label_NameEn.Text = dt.Rows[0]["TDESC"].ToString();
                if (this.Language.Equals("zh-cn"))
                {
                    this.pageTitle.Text = dt.Rows[0]["TDESCCHS"].ToString();
                }
                else
                {
                    this.pageTitle.Text = dt.Rows[0]["TDESC"].ToString();
                }
            }
            else
            {
                this.AlertMessageBox(this.Page, "配置有误，该树形编码并不存在，请核对后重试！");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message.ToString());
            this.AlertMessageBox(this.Page, "配置有误，请核对后重试！");
        }
    }

    #region viewstate初始化区域
    private string strTid
    {
        get
        {
            return ViewState["TID_ViewState"] as string;
        }
        set
        {
            ViewState["TID_ViewState"] = value;
        }
    }
    private string strOpType
    {
        get
        {
            return ViewState["strOpType_ViewState"] as string;
        }
        set
        {
            ViewState["strOpType_ViewState"] = value;
        }
    }
    #endregion


    /// <summary>
    /// 刷新操作
    /// </summary>
    protected void aRefreshTreeData_Click(object sender, EventArgs e)
    {
        this.LoadPageData();
    }


}
