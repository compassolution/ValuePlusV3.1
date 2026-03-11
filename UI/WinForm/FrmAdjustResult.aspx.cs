using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Resources;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common.Security;

public partial class WinForm_FrmAdjustResult : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.hfUserId.Value = this.GetUserCode();
            if (Request.Params["userId"] != null)
            {
                if (!String.IsNullOrEmpty(Request.Params["userId"]))
                {
                    this.hfUserId.Value = Request.Params["userId"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.hfUserId.Value = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.hfUserId.Value);
                }
            }
            this.hfUserType.Value = "0";
            if (Request.Params["userType"] != null)
            {
                //排班操作用户类型（0或者空为小部门用户,1为人事部）
                if (!String.IsNullOrEmpty(Request.Params["userType"]))
                {
                    this.hfUserType.Value = Request.Params["userType"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.hfUserType.Value = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.hfUserType.Value);
                }
            }
            this.hfIsLock.Value = "0";
            if (Request.Params["isLock"] != null)
            {
                //月份锁定标志（0未锁定，1已锁定）
                if (!String.IsNullOrEmpty(Request.Params["isLock"]))
                {
                    this.hfIsLock.Value = Request.Params["isLock"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.hfIsLock.Value = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.hfIsLock.Value);
                }
            }
            this.hfAnalysStatus.Value = "1";
            if (Request.Params["analysStatus"] != null)
            {
                //考勤分析状态（1小部门分析，2人事部审核，3分析完成）
                if (!String.IsNullOrEmpty(Request.Params["analysStatus"]))
                {
                    this.hfAnalysStatus.Value = Request.Params["analysStatus"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.hfAnalysStatus.Value = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.hfAnalysStatus.Value);
                }
            }

            string strCurYearMonth = Request["yearMonth"] == null ? string.Empty : Request["yearMonth"].ToString();//请求类型参数
            ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
            this.hfCurYearMonth.Value = strCurYearMonth;
            this.hfLanguage.Value = this.Language;
            this.hfTipGridColumnsText.Value = rmLocResourceManager.GetString("tipGridColumnsText");
            this.hfTipGridSortAscText.Value = rmLocResourceManager.GetString("tipGridSortAscText");
            this.hfTipGridSortDescText.Value = rmLocResourceManager.GetString("tipGridSortDescText");
            this.hfTipMsg.Value = rmLocResourceManager.GetString("tipMsg");
            this.hfTipNoSave.Value = rmLocResourceManager.GetString("tipNoSave");
            this.hfTipSaveMsg.Value = rmLocResourceManager.GetString("tipSaveSuccess");

            //获取是否可以编辑异常结果调休信息的配置
            String strIsCanEditTX = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditTX");
            String strIsCanEditJB = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditJB");
            String strIsCanEditIsToNormal = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditIsToNormal");
            this.hfIsCanEditTx.Value = strIsCanEditTX;
            this.hfIsCanEditJb.Value = strIsCanEditJB;
            this.hfIsCanEditIsToNormal.Value = strIsCanEditIsToNormal;

            this.hfListNormal.Value = rmLocResourceManager.GetString("tipListNormal");
            this.hfListUnNormal.Value = rmLocResourceManager.GetString("tipListUnNormal");

            //label中英文
            this.Title = rmLocResourceManager.GetString("tipLabelUnNormalList");
            this.hfLabelYouCanAdjust.Value = rmLocResourceManager.GetString("tipLableYoucanAdjust");
            this.hfLabelUnNormalInfo.Value = rmLocResourceManager.GetString("tipLabelUnNormalInfo");
            this.hfLabelToNormalInfo.Value = rmLocResourceManager.GetString("tipLabelToNormalInfo");
            this.hfBtnSaveAdjust.Value = rmLocResourceManager.GetString("tipBtnSaveAdjust");
            this.hfLVOVMustInt.Value = rmLocResourceManager.GetString("tipLVOVMustInt");

            this.hfTipPagingShowMsg.Value = rmLocResourceManager.GetString("tipPagingShowMsg");
            this.hfTipPagingNoRecord.Value = rmLocResourceManager.GetString("tipPagingNoRecord");
        }
    }
}
