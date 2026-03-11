<%@ WebHandler Language="C#" Class="InnerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Com.ValuePlus.Common.Security;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class InnerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strDSCode = hsTableUrlQuery["dscode"] == null ? string.Empty : hsTableUrlQuery["dscode"].ToString();//param
        string strParamName = hsTableUrlQuery["paramname"] == null ? string.Empty : hsTableUrlQuery["paramname"].ToString();//paramname
        string strParamValue = hsTableUrlQuery["paramvalue"] == null ? string.Empty : hsTableUrlQuery["paramvalue"].ToString();//paramvalue

        log.Error("(AppFunction/OADataSync/InnerHandler.ashx)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("getinnerdata"))
        {
            //从内网数据库中获取需同步到外网的数据集
            this.GetInnerData(context,strDSCode);
        }else if (strParam.Equals("downloaddatatoinner"))
        {
            //下载数据信息到内容服务器
            this.UploadDataToInner(context, strDSCode);
        }
        else if (strParam.Equals("updatebasicparamvaluebyname"))
        {
            //修改BasicParam_1中的某个paramName的值
            this.UpdateBasicParamValueByName(context,strParamName,strParamValue);
        }
    }

    /// <summary>
    /// 从内网数据库中获取需同步到外网的数据集
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDSCode"></param>
    private void GetInnerData(HttpContext context,String strDSCode)
    {
        try
        {
            StringBuilder sBuilder = new StringBuilder();
            String strOADataSync2ConfigData = context.Request["txt_OADataSync2ConfigData"].ToString();
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(strOADataSync2ConfigData);
            string json = OADataSync.GetDataJsonByOADataSync2Config(strDSCode,jsonObject,OADataSync.prefix_InnerToOuter);
            context.Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("error");
        }
    }
    
    /// <summary>
    /// 下载数据信息到内网服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDSCode"></param>
    private void UploadDataToInner(HttpContext context,String strDSCode)
    {
        int iReturnResult = 0;
        StringBuilder sBuilder = new StringBuilder();
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String strResultData = context.Request["txt_DataSyncResult"].ToString();

            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strResultData = SQLInjectionDefense.ReplaceSQLReservedKeyword(strResultData);
            String strOADataSync2ConfigData = context.Request["txt_OADataSync2ConfigData"].ToString();

            log.Error("AppFunction/OADataSync/OuterHandler.ashx 下载数据信息到内容服务器");
            JObject jsonOADataSync2ConfigObject = (JObject)JsonConvert.DeserializeObject(strOADataSync2ConfigData);
            JObject jsonSyncDataObject = (JObject)JsonConvert.DeserializeObject(strResultData);

            //第一步：首先将表数据插入到临时表
            iReturnResult  = OADataSync.InsertDataToTempTableByOADataSync2Config(strDSCode,jsonOADataSync2ConfigObject,jsonSyncDataObject,OADataSync.prefix_OuterToInner,this.GetUserCode(),ref sbSql);

            //第二步：执行存储过程，将临时表中数据更新到正式表中
            iReturnResult = DoSyncDataFromTempTable(strDSCode,iReturnResult);
        }
        catch (Exception ex)
        {
            log.Error("下载数据信息到内容服务器出错,SQL:"+sbSql.ToString()+"\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult);
        }
    }
    /// <summary>
    /// 从临时表中同步数据到内网的正式数据表中
    /// </summary>
    /// <param name="strDSCode"></param>
    /// <param name="iReturnResult"></param>
    /// <returns></returns>
    private int DoSyncDataFromTempTable(String strDSCode,int iReturnResult)
    {
        String strSpName = "USP_DS_Update_OuterToInner";
        try{
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("DSCODE", strDSCode);
            hsTableParam.Add("USERCODE", this.GetUserCode());
            iReturnResult = SqlParamDao.ExcuteSP(strSpName,hsTableParam);
        }catch(Exception ex){
            iReturnResult = -1;
            log.Error("从临时表中同步数据到正式数据表中出错,"+strSpName+"执行失败{DSCODE="+strDSCode+"}");
            log.Error("错误信息："+ex.ToString());
        }
        return iReturnResult;
    }
    
    /// <summary>
    /// 修改BasicParam_1中的某个paramName的值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strParamName"></param>
    /// <param name="strParamValue"></param>
    private void UpdateBasicParamValueByName(HttpContext context,String strParamName,String strParamValue)
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if(!String.IsNullOrEmpty(strParamName) && !String.IsNullOrEmpty(strParamValue))
            {
                strParamValue = Microsoft.JScript.GlobalObject.decodeURIComponent(strParamValue);
                sbSql.Append("UPDATE BASICPARAM_1 SET paramValue = '"+strParamValue+"' WHERE paramName = '"+strParamName+"'");
            }
            int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            context.Response.Write(iCount);

        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("-1");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}