using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CrystalImageHandler : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CrystalDecisions.Web.CrystalImageHandler hndlr = new CrystalDecisions.Web.CrystalImageHandler();
        hndlr.ProcessRequest(Context);
    }
}