using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.BLL.Regist;

public partial class Server : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RegistBll bllLicense = new RegistBll();
            //服务器机器码
            this.txtServerCode.Text = bllLicense.GetCurSeverMachineCode();
        }
    }
}