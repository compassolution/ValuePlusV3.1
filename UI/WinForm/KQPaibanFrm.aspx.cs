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
using Com.ValuePlus.BLL.WinForm;
using System.Collections.Generic;
using System.Resources;
using Newtonsoft.Json;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common.Security;

public partial class WinForm_KQPaibanFrm : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.hfUserId.Value = this.GetUserCode();
            if (Request.Params["userId"] != null)
            {
                //排班操作用户ID
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
            this.hfYearMonth.Value = "";
            if (Request.Params["yearMonth"] != null)
            {
                //月份
                if (!String.IsNullOrEmpty(Request.Params["yearMonth"]))
                {
                    this.hfYearMonth.Value = Request.Params["yearMonth"].ToString();
                    //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
                    this.hfYearMonth.Value = SQLInjectionDefense.ReplaceSQLReservedKeyword(this.hfYearMonth.Value);
                }
            }

            ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
            this.hfBtnSave.Value = rmLocResourceManager.GetString("btnSave");
            this.hfBtnClose.Value = rmLocResourceManager.GetString("tipBtnClose");
            this.hfBtnLoadInfo.Value = rmLocResourceManager.GetString("btnLoadInfo");
            this.hfTipTitle.Value = rmLocResourceManager.GetString("tipTitle");
            this.hfTipTip.Value = rmLocResourceManager.GetString("tipTip");
            this.hfTipSelectMonth.Value = rmLocResourceManager.GetString("tipSelectMonth");
            this.hfTipMsg.Value = rmLocResourceManager.GetString("tipMsg");
            this.hfTipNoSave.Value = rmLocResourceManager.GetString("tipNoSave");
            this.hfTipCanCopy.Value = rmLocResourceManager.GetString("tipCanCopy");
            this.hfTipGridColumnsText.Value = rmLocResourceManager.GetString("tipGridColumnsText");
            this.hfTipGridSortAscText.Value = rmLocResourceManager.GetString("tipGridSortAscText");
            this.hfTipGridSortDescText.Value = rmLocResourceManager.GetString("tipGridSortDescText");
            this.hfTipSaveMsg.Value = rmLocResourceManager.GetString("tipSaveSuccess");
            this.hfBtnExcuteSp_Analyse.Value = rmLocResourceManager.GetString("btnExcuteSp_Analyse");
            this.hfTipNoAnalyseData.Value = rmLocResourceManager.GetString("tipNoAnalyseData");
            this.hfTipAnalyseSuccess.Value = rmLocResourceManager.GetString("tipNoAnalyseSuccess");
            this.hfTipTobeAnalyse.Value = rmLocResourceManager.GetString("tipTobeAnalyse");

            this.hfTipNoAnalyseFailed.Value = rmLocResourceManager.GetString("tipNoAnalyseFailed");
            this.hfTipPeriodLocked.Value = rmLocResourceManager.GetString("tipPeriodLocked");

            this.hfTipAnalysStatus1.Value = rmLocResourceManager.GetString("tipAnalysStatus1");
            this.hfTipAnalysStatus2.Value = rmLocResourceManager.GetString("tipAnalysStatus2");
            this.hfTipAnalysStatus3.Value = rmLocResourceManager.GetString("tipAnalysStatus3");

            this.hfListNormal.Value = rmLocResourceManager.GetString("tipListNormal");
            this.hfListUnNormal.Value = rmLocResourceManager.GetString("tipListUnNormal");
            this.hfMenuCopyShifCode.Value = rmLocResourceManager.GetString("tipMenuCopyShifCode");
            this.hfMenuShowUnNormalGird.Value = rmLocResourceManager.GetString("tipMenuShowUnNormalGird");
            this.hfBatchAdjustResult.Value = rmLocResourceManager.GetString("tipBatchAdjustResult");
            this.hfMenuCopyThisRow.Value = rmLocResourceManager.GetString("tipMenuCopyThisRow");
            this.hfMenuPasteToRow.Value = rmLocResourceManager.GetString("tipMenuPasteToRow");
            this.hfMenuViewResultGrid.Value = rmLocResourceManager.GetString("tipMenuViewResultGrid");
            this.hfBtnAdjustResult.Value = rmLocResourceManager.GetString("btnAdjustResult");
            this.hfBtnAdjustOTREST.Value = rmLocResourceManager.GetString("btnAdjustOTREST");

            this.hfMenuFillCheckInOut.Value = rmLocResourceManager.GetString("tipMenuFillCheckInOut");
            this.hfMenuAnalyzeTheStaff.Value = rmLocResourceManager.GetString("tipMenuAnalyzeTheStaff");
            this.hfMenuLVOTRecord.Value = rmLocResourceManager.GetString("tipMenuLVOTRecord");
            this.hfMenuCheckInOutRecord.Value = rmLocResourceManager.GetString("tipMenuCheckInOutRecord");
            this.hfMenuAttendanceSummary.Value = rmLocResourceManager.GetString("tipMenuAttendanceSummary");
            this.hfMenuAttendanceAnalysisResult.Value = rmLocResourceManager.GetString("tipMenuAttendanceAnalysisResult");
            this.hfMenuAttendanceReport.Value = rmLocResourceManager.GetString("tipMenuAttendanceReport");
            this.hfMenuAttendanceExceptionReport.Value = rmLocResourceManager.GetString("tipMenuAttendanceExceptionReport");
            this.hfMenuOvertimeBalanceReport.Value = rmLocResourceManager.GetString("tipMenuOvertimeBalanceReport");
            if (this.GetProjectId().ToUpper().Equals("VP_HR_XMPP"))
            {
                this.hfMenuAttendanceReport.Value = "考勤汇总表";
                this.hfMenuOvertimeBalanceReport.Value = "考勤明细表";
            }
            this.hfMenuLeaveDocument.Value = rmLocResourceManager.GetString("tipMenuLeaveDocument");
            this.hfMenuOvertimeDocument.Value = rmLocResourceManager.GetString("tipMenuOvertimeDocument");

            this.hfLVOVMustInt.Value = rmLocResourceManager.GetString("tipLVOVMustInt");
            //过滤班次
            try
            {
                this.hfTxtShiftCodeFilter.Value = rmLocResourceManager.GetString("tipShiftCodeFilter");
            }
            catch (Exception ex)
            {
                log.Error("WinForm/KQPaibanFrm.aspx页面中不存在对象tipShiftCodeFilter");
            }
            

            this.hfLanguage.Value = this.Language; 

            //获取是否可以编辑异常结果调休信息的配置
            String strIsCanEditTX = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditTX");
            String strIsCanEditJB = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditJB");
            String strIsCanEditIsToNormal = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsCanEditIsToNormal");///部门是否可以编辑调整异常标识的开关 add by sammen 20180105
            this.hfIsCanEditTx.Value = strIsCanEditTX;
            this.hfIsCanEditJb.Value = strIsCanEditJB;
            this.hfIsCanEditIsToNormal.Value = strIsCanEditIsToNormal;
            //获取是否进行实时保存排班以及是否实时进行考勤分析的开关设置
            String strIsRealTimeSave = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsRealTimeSavePaiBan");
            if (String.IsNullOrEmpty(strIsRealTimeSave)) strIsRealTimeSave = "0";
            String strIsRealTimeAnalyse = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsRealTimeAnalysePaiBanReuslt");
            if (String.IsNullOrEmpty(strIsRealTimeAnalyse)) strIsRealTimeAnalyse = "0";
            this.hfIsRealTimeSave.Value = strIsRealTimeSave;
            this.hfIsRealTimeAnalyse.Value = strIsRealTimeAnalyse;

            ///是否按员工依次执行考勤分析 add by sammen 20180122
            this.hfIsKQAnalyseByStaff.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsKQAnalyseByStaff");
            this.hfIsKQAnalyseByStaff.Value = String.IsNullOrEmpty(this.hfIsKQAnalyseByStaff.Value) ? "0" : this.hfIsKQAnalyseByStaff.Value;
            ///是否考勤分析后直接弹出查询页面 add by sammen 20180122
            this.hfIsShowPageAfterKQAnalyse.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsShowPageAfterKQAnalyse");
            this.hfIsShowPageAfterKQAnalyse.Value = String.IsNullOrEmpty(this.hfIsShowPageAfterKQAnalyse.Value) ? "0" : this.hfIsShowPageAfterKQAnalyse.Value;

            this.hfCurServerDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            this.hfCurServerTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            //获取加班休假是否用OA流程的配置
            this.hfIsUseOA_LV.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsUseOA_LV");
            this.hfIsUseOA_OT.Value = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsUseOA_OT");


        }
    }


}
