using System;
using System.Data;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Config;
using Com.ValuePlus.Archive.BLL;
using System.Collections;
using Com.ValuePlus.Archive.Property;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.WebCtrls;
using Microsoft.Web.UI.WebControls;
using System.Web.UI.WebControls;
using Com.ValuePlus.DataLog.Enum;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Entity;
using System.Resources;

public partial class Archive_Detail_ArchiveDetailAjax : ArchivePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region 获取受控制的其他控件列表
    /// <summary>
    /// 获取受控制的其他控件列表
    /// </summary>
    /// <param name="strCtrlId"></param>
    /// <param name="strValue"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public ArrayList GetBeMastControlList(String strCtrlId, String strValue,String strKeyValue)
    {
        ArrayList arrListDataTable = new ArrayList();
        try
        {
            TmpdPMastBll bll = new TmpdPMastBll();
            arrListDataTable = bll.GetBeMastControlList(strCtrlId, strValue,base.Language);

            ////保存服务器端控件界面选择的值，以供回传后服务器端可以获取到，而避免动态生成的服务器端控件在回传后无法获取客户端赋值的问题（主要用于下拉框等控件）
            //this.SaveServerCtrlClientSettingValue(strCtrlId, strValue, strKeyValue);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveDetailAjax.GetBeMastControlList() Error！");
        }
        return arrListDataTable;
    }
    #endregion


    #region 获取受经加密的url参数字符串
    /// <summary>
    /// 获取受经加密的url参数字符串
    /// </summary>
    /// <param name="strUrlParams"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public String GetEncryptionUrlParamString(String strUrlParams)
    {
        return UrlParamEncryption.EncryptionUrlParam(strUrlParams); 
    }
    #endregion


    #region 删除模板列表类型分组中某条记录
    /// <summary>
    /// 删除模板列表类型分组中某条记录
    /// </summary>
    /// <param name="strCtrlId"></param>
    /// <param name="strValue"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public String DeleteGridOneRecord(String strTid,String strGid,String strKey,String strKeyValue,String strGridKey,String strGridKeyValue)
    {

        ResourceManager rmLocResourceManager = base.GetResourceManager("Archive");

        Hashtable hsTableParam = new Hashtable();
        hsTableParam.Add("TID", strTid);
        hsTableParam.Add("GID", strGid);
        hsTableParam.Add("Key", strKey);
        hsTableParam.Add("KeyValue", strKeyValue);
        hsTableParam.Add("GridKey", strGridKey);
        hsTableParam.Add("GridKeyValue", strGridKeyValue);
        hsTableParam.Add("UserId", this.GetUserCode());

        //先执行删除前的动作执行 add by sammen 20131120
        int iCount = ArchiveGridActionBll.DoExcuteSP_BeforeDelete(hsTableParam);
        String strMsg = ArchiveGridActionBll.GetArchiveGridActionTips(strTid, strGid, ArchiveGridActionBll.ActionID_BeforeDelete, iCount, this.Language);
        if (iCount == 1)
        {
            ///返回值为1时才可以继续执行
            String strTableName = strTid + "_" + strGid;
            string strSql = "delete from " + strTableName + " where " + strKey + "='" + strKeyValue + "' and " + strGridKey + "='" + strGridKeyValue + "'";
            try
            {
                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                if (iCount > 0)
                {//同时写入删除日志
                    this.WriteDeleteDataLog(strTid, strGid, strKey, strKeyValue, strGridKey, strGridKeyValue);


                    //先执行删除后的动作执行 add by sammen 20131120
                    iCount = ArchiveGridActionBll.DoExcuteSP_AfterDelete(hsTableParam);
                    strMsg = ArchiveGridActionBll.GetArchiveGridActionTips(strTid, strGid, ArchiveGridActionBll.ActionID_AfterDelete, iCount, this.Language);
                    if (String.IsNullOrEmpty(strMsg))
                    {
                        strMsg = rmLocResourceManager.GetString("tipDeleteSuccess");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("\r\n");
                log.Error("页面EditArchiveDetail.aspx中删除列表失败，方法DeleteGridOneRecord，SQL：" + strSql.ToString());
                if (String.IsNullOrEmpty(strMsg))
                {
                    strMsg = rmLocResourceManager.GetString("tipDeleteFailed");
                }
            }

        }
        return strMsg;
    }

    /// <summary>
    /// 写入删除档案日志
    /// </summary>
    /// <param name="strKeyValue"></param>
    private void WriteDeleteDataLog(String strTid,String strGid,String strKey,String strKeyValue,String strGridKey,String strGridKeyValue)
    {
        if ((Session["ArchiveIsLog"] != null) && (Session["ArchiveIsLog"].ToString().Equals("1")))
        {
            Entity_HRLOG_2 entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(strTid, strGid, strGridKey, strGridKeyValue, "");
            DataLogWriter.Log_Archive(this.GetUserCode(), strTid, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Delete, strKeyValue, entityLog2);
        }
    }

    #endregion


    #region 设置服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
    /// <summary>
    /// 设置服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
    /// </summary>
    /// <param name="strCtrlId"></param>
    /// <param name="strMastValue"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public void SetSeverCtrlMastValue(String strCtrlId, String strMastValue)
    {
        try
        {
            //保存服务器端控件客户端界面选择的值时设置其他控件的MASTVALUE
            this.SaveClientSettingMastValue(strCtrlId, strMastValue);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("\r\n");
            log.Error("Archive_Detail_ArchiveDetailAjax.SetSeverCtrlMastValue() Error！");
        }
    }
    #endregion

}
