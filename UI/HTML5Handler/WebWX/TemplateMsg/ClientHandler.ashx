<%@ WebHandler Language="C#" Class="ClientHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Com.ValuePlus.Archive.BLL;

public class ClientHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strTID = hsTableUrlQuery["tid"] == null ? string.Empty : hsTableUrlQuery["tid"].ToString();//
        string strRID = hsTableUrlQuery["rid"] == null ? string.Empty : hsTableUrlQuery["rid"].ToString();//
        string strSID = hsTableUrlQuery["sid"] == null ? string.Empty : hsTableUrlQuery["sid"].ToString();//
        string strAID = hsTableUrlQuery["aid"] == null ? string.Empty : hsTableUrlQuery["aid"].ToString();//
        string strKeyValue = hsTableUrlQuery["keyvalue"] == null ? string.Empty : hsTableUrlQuery["keyvalue"].ToString();//

        log.Error("(HTML5Handler/WebWX/TemplateMsg/ClientHandler.ashx)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if(String.IsNullOrEmpty(strParam)){
            //如果传参为空，则考虑是Uniapp手机端的入口，则采用另外一种获取参数的模式            
            //解析客户端传递过来的json data
            StreamReader reader = new StreamReader(context.Request.InputStream);
            String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
            //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

            strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
            strTID = WebCommon.GetJsonValue(strParamJson,"tid").ToString();//
            strRID = WebCommon.GetJsonValue(strParamJson,"rid").ToString();//
            strSID = WebCommon.GetJsonValue(strParamJson,"sid").ToString();//
            strAID = WebCommon.GetJsonValue(strParamJson,"aid").ToString();//
            strKeyValue = WebCommon.GetJsonValue(strParamJson,"keyvalue").ToString();//
        }
        if (strParam.Equals("getneedpushuserdata"))
        {
            this.GetNeedPushUserData(context,strTID,strRID,strAID,strKeyValue);
        }
    }

    /// <summary>
    /// 根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景、用户及其待办数量
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strTID"></param>
    /// <param name="strRID"></param>
    /// <param name="strAID"></param>
    private void GetNeedPushUserData(HttpContext context,String strTID,String strRID,String strAID,String strKeyValue)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "根据TID/RID/AID获取流程FLFlowConfig_3中对应的下一个岗位的FLUser中配置所需推送提醒的场景、用户及其待办数量";
        StringBuilder sbResult = new StringBuilder();
        try
        {
            //动作执行成功，则根据FLUser_6的配置进行公众号的推送【先调用客户端脚本以便去调用远程公众号服务器】
            DataTable dt = Com.ValuePlus.Archive.Flow.FLUserSMS.GetNeedNoticeScene(strTID, strRID, strAID,strKeyValue,"2");

            StringBuilder sbResult_NextInfo = new StringBuilder();
            sbResult_NextInfo.Append("[{");
            //获取下一岗位场景相关信息及对应场景下的待办数量
            if(dt != null && dt.Rows.Count>0){
                sbResult_NextInfo.Append("\"NextPostArray\":[");
                for(int i=0;i<dt.Rows.Count;i++){
                    sbResult_NextInfo.Append((i==0?"":",")+"");
                    DataRow dr = dt.Rows[i];
                    String strNextTID = dr["FCODE"].ToString();
                    String strNextRID = dr["PostCode"].ToString();
                    String strNextSID = dr["SceneCode"].ToString();
                    String strNextUSERID = dr["SUSERID"].ToString();
                    String strNextDCMOBILE = dr["DCMOBILE"].ToString();
                    String strNextSTAFFNO = dr["STAFFNO"].ToString();

                    //下一岗位场景的待办数量
                    String strSqlSSLCT = ArchiveMainDealBll.GetArchiveSceneSSLCT(strNextTID, strNextRID, strNextSID, this.GetUserCode(), this.Language, false).ToUpper();
                    //由于可能有ORDER BY 语句，所有需要加上top 100 percent
                    strSqlSSLCT = strSqlSSLCT.TrimStart();//去掉前面的空格
                    strSqlSSLCT = strSqlSSLCT.Substring(6);//去掉最前面的SELECT
                    strSqlSSLCT = "SELECT TOP 100 PERCENT "+strSqlSSLCT;

                    //下一岗位场景相关信息
                    StringBuilder sbSql_Desc = new StringBuilder();
                    sbSql_Desc.Append("select top 1 *");
                    sbSql_Desc.Append(",'"+strNextUSERID+"' AS SUSERID");
                    sbSql_Desc.Append(",'"+strNextDCMOBILE+"' AS MOBILENO");
                    sbSql_Desc.Append(",'"+strNextSTAFFNO+"' AS STAFFNO");
                    sbSql_Desc.Append(",(select count(1) from ("+strSqlSSLCT+") A) AS PendingQty");
                    sbSql_Desc.Append(",(select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'ProjectId') AS ProjectId");
                    sbSql_Desc.Append(",(select top 1 paramValue from BASICPARAM_1 WHERE paramName = 'SalaryRemoteServer') AS RemoteServer");
                    sbSql_Desc.Append(" from [VW_FL_FlowPostScene] where FCODE = '"+strNextTID+"' AND PostCode = '"+strNextRID+"' AND SceneCode = '"+strNextSID+"'");

                    String strSql_Desc = sbSql_Desc.ToString();
                    DataTable dt_Desc = SqlParamDao.GetDataTableBySql(strSql_Desc);
                    String strJsonString_Desc = WebCommon.GetJsonStringByFirstDataRow(dt_Desc,  false);
                    sbResult_NextInfo.Append(strJsonString_Desc);

                    ////下一岗位场景的待办数量
                    //String strSql_PendingQty = ArchiveMainDealBll.GetArchiveSceneSSLCT(strNextTID, strNextRID, strNextSID, this.GetUserCode(), this.Language, false).ToUpper();
                    //DataTable dt_PendingQty = SqlParamDao.GetDataTableBySql(strSql_PendingQty);
                    //int iPendingQty = dt_PendingQty.Rows.Count;

                    //将信息拼成JSON字符串
                    //sbResult_NextInfo.Append(WebCommon.GetJsonStringByDataTable(dt_Desc,"\"\"",false));
                    //sbResult_NextInfo.Append(",\"ClientName\":" + "'" + iPendingQty.ToString() + "'");

                }
                sbResult_NextInfo.Append("]");
                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }else{
                strReturnCode = "0";
                strReturnMsg = strMethodDesc+"成功，但无配置推送";
            }
            sbResult_NextInfo.Append("}]");

            sbResultData.Append("\"ResultData\":"+sbResult_NextInfo.ToString());
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(strReturnMsg + ex.ToString());
        }
        finally{
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("\"ReturnStatus\":{");
            sbResultStatus.Append("\"ReturnCode\":\""+strReturnCode.ToString()+"\"");
            sbResultStatus.Append(",\"ReturnMsg\":\""+strReturnMsg.ToString()+"\"");
            sbResultStatus.Append("}");

            sbResult.Append("{");
            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbResultData.ToString()))
            {
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc + "时返回数据：" + sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}