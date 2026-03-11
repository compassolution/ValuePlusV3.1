using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;
using System.Resources;

public partial class SysUpdate_Client_UpdateList : PageBase
{
    public String strLbUpdateList;
    public String strLbNeedUpdate;
    public String strLbHadUpdated;
    public String strLbPackageCode;
    public String strLbReleaseTime;
    public String strLbReleaseDesc;
    public String strLbReleaseKey;
    public String strLbOperation;
    public String strLbUpdateUser;
    public String strLbUpdateTime;
    public String strLbUpdateClientIP;
    public String strLbAimProjectId;
    public String strLbAimWebSite;
    public String strLbRemoteServer;
    public String strLbIsSuccess;
    public String strLbUpdateFlag;
    public String strLbUpdateSuccess;
    public String strBtnUpdate;
    public String strErrorGetList;
    public String strErrorGetFile;
    public String strErrorNoFile;
    public String strErrorUpdateFailed;
    public String strErrorAfterUpdate;

    protected void Page_Load(object sender, EventArgs e)
    {
        ResourceManager rmLocResourceManager = base.GetResourceManager("SysUpdate");
        strLbUpdateList = rmLocResourceManager.GetString("lbUpdateList");
        strLbNeedUpdate = rmLocResourceManager.GetString("lbNeedUpdate");
        strLbHadUpdated = rmLocResourceManager.GetString("lbHadUpdated");
        strLbPackageCode = rmLocResourceManager.GetString("lbPackageCode");
        strLbReleaseTime = rmLocResourceManager.GetString("lbReleaseTime");
        strLbReleaseDesc = rmLocResourceManager.GetString("lbReleaseDesc");
        strLbReleaseKey = rmLocResourceManager.GetString("lbReleaseKey");
        strLbOperation = rmLocResourceManager.GetString("lbOperation");
        strLbUpdateUser = rmLocResourceManager.GetString("lbUpdateUser");
        strLbUpdateTime = rmLocResourceManager.GetString("lbUpdateTime");
        strLbUpdateClientIP = rmLocResourceManager.GetString("lbUpdateClientIP");
        strLbAimProjectId = rmLocResourceManager.GetString("lbAimProjectId");
        strLbAimWebSite = rmLocResourceManager.GetString("lbAimWebSite");
        strLbRemoteServer = rmLocResourceManager.GetString("lbRemoteServer");
        strLbIsSuccess = rmLocResourceManager.GetString("lbIsSuccess");
        strLbUpdateFlag = rmLocResourceManager.GetString("lbUpdateFlag");
        strLbUpdateSuccess = rmLocResourceManager.GetString("lbUpdateSuccess");
        strBtnUpdate = rmLocResourceManager.GetString("btnUpdate");

        strErrorGetList = rmLocResourceManager.GetString("lbErrorGetList");
        strErrorGetFile = rmLocResourceManager.GetString("lbErrorGetFile");
        strErrorNoFile = rmLocResourceManager.GetString("lbErrorNoFile");
        strErrorUpdateFailed = rmLocResourceManager.GetString("lbErrorUpdateFailed");
        strErrorAfterUpdate = rmLocResourceManager.GetString("lbErrorAfterUpdate");

    }
}