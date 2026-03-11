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
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Common.Security;

public partial class Archive_ViewArchiveDetail : ArchivePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if ((Request.Params["TID"] != null) && (Request.Params["KEYVALUE"] != null))
                {
                    if (this.Language.Equals("zh-cn"))
                    {
                        this.Page.Title = Session["ArchiveDesc"] + "列表";
                    }
                    else
                    {
                        this.Page.Title = Session["ArchiveDesc"] + " List";
                    }
                    this.TID = Request.Params["TID"].ToString();
                    this.KEYVALUE = Request.Params["KEYVALUE"].ToString();
                    this.OPTYPE = "readonly";//操作类型（add新增，edit编辑，readonly只读）

                    if (Request.Params["RID"] != null)
                    {
                        this.RID = Request.Params["RID"].ToString();
                    }
                    if (Request.Params["SID"] != null)
                    {
                        this.SID = Request.Params["SID"].ToString();
                    }
                    if (Request.Params["KEY"] != null)
                    {
                        this.KEY = Request.Params["KEY"].ToString();
                    }

                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.TID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.TID);
                    this.RID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.RID);
                    this.SID = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.SID);
                    this.KEY = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.KEY);
                    this.KEYVALUE = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.KEYVALUE);

                    //根据模板ID获取相应信息
                    this.GetArchiveInfoByTID(this.TID);
                    if (!(String.IsNullOrEmpty(this.RID)) &&(!String.IsNullOrEmpty(this.SID)))
                    {
                        this.RedirectToViewPage();
                    }
                    else
                    {
                        this.lbTip.Text = "非常抱歉，您暂不具备此页面的查看权限！";
                    }
                }
                else
                {
                    this.lbTip.Text = "非常抱歉，缺乏必要的参数传递！";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                base.AlertMessageBox(this, "页面错误！");
            }
        }
    }

    /// <summary>
    /// 根据模板ID获取相应信息
    /// </summary>
    /// <param name="strTid"></param>
    private void GetArchiveInfoByTID(String strTid)
    {
        if (!String.IsNullOrEmpty(strTid))
        {
            String strSql = "";
            DataTable dt = new DataTable();
            if (String.IsNullOrEmpty(this.KEY))
            {
                //获取主键字段
                strSql = "SELECT A.TID,A.PID FROM TB_HRTMPD A,TB_HRTMPG B WHERE A.TID = B.TID AND A.GID = B.GID AND B.GTYPE = 0 AND A.TID='" + strTid + "' AND A.PISKEY = 1";
                dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    this.KEY = dt.Rows[0]["PID"].ToString();
                }
            }
            if ((String.IsNullOrEmpty(this.RID)) || (String.IsNullOrEmpty(this.SID)))
            {
                //根据当前用户获取其具有相应查看权限的角色和状态
                if (this.IsAdminstrator())
                {
                    strSql = "SELECT TOP 1 A.TID,A.SID,B.RID FROM TB_HRTMPSG A, TB_HRTMPRD B WHERE A.TID = '" + strTid + "' AND A.GTYPE = 0 AND A.GRIGHT = 0  AND A.TID = B.TID AND A.SID = B.SID";
                    dt = SqlParamDao.GetDataTableBySql(strSql);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        this.RID = dt.Rows[0]["RID"].ToString();
                        this.SID = dt.Rows[0]["SID"].ToString();
                    }
                }
                else
                {
                    strSql = "SELECT TOP 1 A.TID,A.SID,B.RID FROM TB_HRTMPSG A, TB_HRTMPRD B,TB_HR_USERROLE C WHERE  A.GTYPE = 0 AND A.GRIGHT = 0 AND A.TID = B.TID AND A.SID = B.SID AND A.GRIGHT = 0 AND B.TID = C.TID AND B.RID = C.RID AND A.TID = '" + strTid + "' AND C.SUSERID = '" + this.GetUserCode() + "'";
                    dt = SqlParamDao.GetDataTableBySql(strSql);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        this.RID = dt.Rows[0]["RID"].ToString();
                        this.SID = dt.Rows[0]["SID"].ToString();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 直接跳转到明细查看页面
    /// </summary>
    private void RedirectToViewPage()
    {
        String strParamString = "TID=" + this.TID + "&RID=" + this.RID + "&SID=" + this.SID + "&KEY=" + this.KEY + "&KEYVALUE=" + this.KEYVALUE + "&OPTYPE=" + this.OPTYPE;

        String strUrl = "Detail/EditArchiveDetail.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParamString);
        Response.Redirect(strUrl, false);
    }

}
