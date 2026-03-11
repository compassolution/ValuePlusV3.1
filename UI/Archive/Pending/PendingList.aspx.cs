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
using System.Resources;
using System.Reflection;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.BLL;

public partial class Archive_Pending_PendingList : PageBase
{
    protected void DataGrid1_EditCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            DataSet set = SqlParamDao.GetDataSetBySql("SELECT islarge ,GVIEW,GSQL FROM TB_HRTMPG WHERE TID='" + e.Item.Cells[5].Text + "' AND GTYPE=0");
            if (set.Tables[0].Rows.Count > 0)
            {
                String strTID = e.Item.Cells[5].Text;
                String strRID = e.Item.Cells[7].Text;
                String strSID = e.Item.Cells[6].Text;
                String strParam = "TID=" + strTID + "&RID=" + strRID + "&SID=" + strSID + "&pageIndex=0&status=";
                String strUrl = "../ArchiveMain.aspx?" + UrlParamEncryption.EncryptionUrlParam(strParam);
                //base.Response.Redirect(strUrl, false);

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "", "<script language=javascript>OpenArchiveMain('" + strUrl + "');</script>");
            }
        }
    }

    protected void DataGrid1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        ImageButton button = (ImageButton)e.Item.FindControl("ImageButton1");
        if (button != null)
        {
            ResourceManager rm = base.GetResourceManager("VPCommon");
            button.AlternateText = rm.GetString("pendingedit");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ResourceManager rm = base.GetResourceManager("VPCommon");
        this.Label1.Text = rm.GetString("alert");
        this.DataGrid1.Columns[0].HeaderText = rm.GetString("TDESC");
        this.DataGrid1.Columns[1].HeaderText = rm.GetString("RDESC");
        this.DataGrid1.Columns[2].HeaderText = rm.GetString("SDESC");
        this.DataGrid1.Columns[3].HeaderText = rm.GetString("count");
        this.DataGrid1.Columns[4].HeaderText = rm.GetString("pendingedit");
        this.aRefresh_PendingList.Text = this.Language.Equals("zh-cn") ? "刷新" : "Refresh";
        if (!base.IsPostBack)
        {
            this.RefreshPage();
        }
    }


    /// <summary>
    /// 重新加载数据
    /// </summary>
    protected void aRefresh_Click(object sender, EventArgs e)
    {
        this.RefreshPage();
    }

    public void RefreshPage()
    {
        try
        {
            //在加载Pending执行，需要执行一个存储过程，针对当前用户所特设的状态场景进行更新
            //主要是为了PR系统新增
            String strSpName = "USP_Archive_BeforeLoadPending";
            try
            {
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("UserId",this.GetUserCode());
                if (!String.IsNullOrEmpty(strSpName))
                {
                    int iCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("加载存储过程"+ strSpName + "失败!");
            }
            //ArchiveAlertBll bllAlert = new ArchiveAlertBll();
            DataSet set = ArchiveAlertBll.getPendingItemLists(this.GetUserCode(), this.Language, this.IsAdminstrator(), false);
            this.DataGrid1.DataSource = set.Tables[0].DefaultView;
            this.DataGrid1.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("加载待办列表失败!");
        }
    }

}
