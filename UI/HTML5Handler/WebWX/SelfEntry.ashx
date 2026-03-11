<%@ WebHandler Language="C#" Class="SelfEntry" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Weixin;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class SelfEntry : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strLanguage = WebCommon.GetJsonValue(strParamJson,"language").ToString();
        string strMobileNo = WebCommon.GetJsonValue(strParamJson,"mobileno").ToString();
        string strProjectId = WebCommon.GetJsonValue(strParamJson,"projectid").ToString();
        string strSelfInputData = WebCommon.GetJsonObjectValue(strParamJson,"selfinputdata").ToString();//需保存的自助录入数据

        log.Error("SelfEntry.ashx,MobileNo:"+strMobileNo+";Language:"+strLanguage);
        if (strParam.Equals("gettmpddatawhenselfentry"))
        {
            //获取某Project入职登记时需要登记的字段数据
            context.Response.Write(this.GetTMPDDataWhenSelfEntry(strProjectId));
        }else if (strParam.Equals("getselfentryinputdata"))
        {
            //获取某MobileNo在某Project入职登记时的录入数据
            context.Response.Write(this.GetSelfEntryInputData(strProjectId,strMobileNo));
        }else if (strParam.Equals("saveoneselfinputdata"))
        {
            //保存自助录入的入职信息
            context.Response.Write(this.SaveOneSelfInputData(strProjectId,strMobileNo,strSelfInputData));
        }

    }

    /// <summary>
    /// 获取某Project入职登记时需要登记的字段数据
    /// </summary>
    /// <param name="strProjectId"></param>
    private String GetTMPDDataWhenSelfEntry(String strProjectId)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某Project入职登记时需要登记的字段数据";
        try
        {
            StringBuilder sbSql_TMPD = new StringBuilder();
            sbSql_TMPD.Append("select * from TB_Remote_HRTMPD where ProjectId = '"+strProjectId+"' ");
            sbSql_TMPD.Append(" and BusinessType = 'SelfEntry' and TID = 'OANEW' AND GID = '1' ORDER BY PORDER");
            String strSql_TMPD = sbSql_TMPD.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql_TMPD);

            DataTable dt_TMPD = SqlParamDao.GetDataTableBySql(strSql_TMPD);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_TMPD,"\"TMPD\"",false));

            //再获取TMPD中对应的LSTD数据
            String strLIDS = "";
            StringBuilder sbSql_LSTD = new StringBuilder();
            sbSql_LSTD.Append("select * from TB_Remote_HRLSTD where ProjectId = '"+strProjectId+"' ");
            if (dt_TMPD!=null && dt_TMPD.Rows.Count>0){
                for(int i=0;i<dt_TMPD.Rows.Count;i++){
                    String strPCTL = dt_TMPD.Rows[i]["PCTRL"].ToString();
                    String strPCTLID = dt_TMPD.Rows[i]["PCTRLID"].ToString();
                    if(strPCTL.Equals("1")){
                        strLIDS = strLIDS + (strLIDS.Equals("") ? "" : ",") + "'" + strPCTLID + "'";
                    }
                }
                sbSql_LSTD.Append(" and LID IN ("+strLIDS+") ");
            }else{
                sbSql_LSTD.Append(" and LID IN ('"+strLIDS+"') ");
            }
            DataTable dt_LSTD = SqlParamDao.GetDataTableBySql(sbSql_LSTD.ToString());
            sbResultData.Append(",");
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_LSTD,"\"LSTD\"",false));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
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
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    /// 获取某MobileNo在某Project入职登记时的录入数据
    /// </summary>
    /// <param name="strProjectId"></param>
    /// <param name="strMobileNo"></param>
    private String GetSelfEntryInputData(String strProjectId,String strMobileNo)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取某MobileNo在某Project入职登记时的录入数据";
        try
        {
            StringBuilder sbSql_TMPD = new StringBuilder();
            sbSql_TMPD.Append("select * from TB_Remote_SelfInputData where ProjectId = '"+strProjectId+"' and MobileNo = '"+strMobileNo+"' ");
            sbSql_TMPD.Append(" and BusinessType = 'SelfEntry' and TID = 'OANEW' AND GID = '1' ORDER BY PID");
            String strSql_TMPD = sbSql_TMPD.ToString();
            log.Error(strMethodDesc+"SQL:"+strSql_TMPD);

            DataTable dt_TMPD = SqlParamDao.GetDataTableBySql(strSql_TMPD);
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt_TMPD,"\"ResultData\"",true));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错";
            log.Error(ex);
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
            log.Error(strMethodDesc+"Return Json:"+sbResult.ToString());
        }
        return sbResult.ToString();
    }
    
    /// <summary>
    /// 保存自助录入的入职信息
    /// </summary>
    /// <param name="strProjectId"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strSelfInputData"></param>
    public String SaveOneSelfInputData(String strProjectId,String strMobileNo,String strSelfInputData)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "保存自助录入的入职信息";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            if (String.IsNullOrEmpty(strProjectId)||String.IsNullOrEmpty(strMobileNo)||String.IsNullOrEmpty(strSelfInputData)){
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc+"失败，数据提交有误!";

            }else {
                String strTableName = "TB_Remote_SelfInputData";
                String strBusinessType = "SelfEntry";
                String strTID = "OANEW";
                String strGID = "1";
                String strNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                JObject itemJArray = (JObject)JsonConvert.DeserializeObject(strSelfInputData);
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from " + strTableName + " where ProjectId = '"+strProjectId+"' and BusinessType = '"+strBusinessType+"' and MobileNo = '"+strMobileNo+"' ");
                sbSql.Append("and TID = '"+strTID+"' and GID = '"+strGID+"' \r\n");

                //json字符串转json的JObject对象
                JObject jo = (JObject)JsonConvert.DeserializeObject(strSelfInputData);
                foreach (var item in jo)
                {
                    string strColumnName = item.Key;
                    string strColumnValue = item.Value.ToString();
                    //处理特殊字符
                    strColumnValue = strColumnValue.Replace("\r\n","").Replace("\r","").Replace("\n","");
                    strColumnValue = strColumnValue.Replace("'","''");
                    
                    sbSql.Append("INSERT INTO [TB_Remote_SelfInputData]([ProjectId],[BusinessType],[MobileNo],[TID],[GID],[PID],[PVALUE],[INPUTTIME]) \r\n");
                    sbSql.Append("values('"+strProjectId+"','"+strBusinessType+"','"+strMobileNo+"','"+strTID+"','"+strGID+"','"+strColumnName+"','"+strColumnValue+"','"+strNow+"') \r\n");
                }

                //log.Error(strMethodDesc + " Execute Sql :" + sbSql.ToString());
                if (!string.IsNullOrEmpty(sbSql.ToString()))
                {
                    int iUpdateCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                }

                strReturnCode = "1";
                strReturnMsg = strMethodDesc+"成功";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc+"出错,请稍候重试";
            log.Error(ex);
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
                sbResult.Append(",\"ResultData\":{"+sbResultData.ToString()+"}");
            }
            sbResult.Append("}");
        }
        return sbResult.ToString();
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}