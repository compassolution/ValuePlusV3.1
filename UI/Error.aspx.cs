using Com.ValuePlus.Common.Security;
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

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            String strErrCode = base.Request.Params["ErrCode"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strErrCode = SQLInjectionDefense.ReplaceSQLReservedKeyword(strErrCode);
            if (strErrCode.Equals("001"))
            {
                this.lbErrTip001.Visible = true;
            }
            else if (strErrCode.Equals("002"))
            {
                this.lbErrTip002.Visible = true;
            }
        }
    }
}
