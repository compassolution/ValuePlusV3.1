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
using Com.ValuePlus.Common.Security;

public partial class Regist_LicenseWarning : PageBase
{
    public String strWarningMsg = "";
    public String strReturnText = "";
    public String strTitleText = "";
    public String strCloseText = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["msg"] != null)
        {
            this.strWarningMsg = Request.Params["msg"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            this.strWarningMsg = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.strWarningMsg);
        }
        this.strReturnText = (!this.Language.Equals("zh-cn") ? "Click to back" : "点击返回");
        this.strCloseText = (!this.Language.Equals("zh-cn") ? "Close" : "关闭");
        this.strTitleText = (!this.Language.Equals("zh-cn") ? "License Notice" : "系统授权提醒");
    }

}
