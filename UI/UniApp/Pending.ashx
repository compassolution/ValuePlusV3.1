<%@ WebHandler Language="C#" Class="Pending" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Web;
using Com.ValuePlus.Archive.BLL;

public class Pending : HandlerBase,IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion


    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        StreamReader reader = new StreamReader(context.Request.InputStream);
        String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        string param = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数
        string strUserCode = WebCommon.GetJsonValue(strParamJson,"usercode").ToString();//用户编码
        string strCulture = WebCommon.GetJsonValue(strParamJson,"culture").ToString();//文化编码[0中文1英文]

        String strRequestLanguage = strCulture.Equals("0") ? "zh-cn" : "en-us";
        switch (param.ToLower().ToString())
        {
            case "getpendinglist":
                this.GetPendingList(context,strRequestLanguage, strUserCode);
                break;
            case "getallarchivelist":
                this.GetAllArchiveList(context,strRequestLanguage, strUserCode);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取待办列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetPendingList(HttpContext context,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取待办列表";
        StringBuilder sbResult = new StringBuilder();

        //在加载Pending执行，需要执行一个存储过程，针对当前用户所特设的状态场景进行更新
        String strSpName = "USP_Archive_BeforeLoadPending";
        try
        {
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("UserId",strUserCode);
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

        try
        {
            //查询表中MBArchive_1需要在待办中出现的模板
            String strSql = "SELECT TID FROM MBArchive_1 A WHERE A.IsValid = '1' AND A.IsOAShow = '1'";
            DataTable dt_Need = SqlParamDao.GetDataTableBySql(strSql);
            String strFilterString = "";
            if(dt_Need!=null&&dt_Need.Rows.Count>0){
                for(int i=0;i<dt_Need.Rows.Count;i++){
                    String strTID = dt_Need.Rows[i]["TID"].ToString();
                    strFilterString = strFilterString +  (i == 0?"":",") + "'" + strTID + "'";
                }
            }

            DataSet ds = ArchiveAlertBll.getPendingItemLists(strUserCode, strRequestLanguage, this.IsAdminstrator(), false);
            DataView dv = ds.Tables[0].DefaultView; //创建DataView对象
            if (!String.IsNullOrEmpty(strFilterString))
            {
                String strFilterSql = "[TID] in (" + strFilterString + ")";
                dv.RowFilter = strFilterSql;
            }
            DataTable dt = dv.ToTable();

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }


    /// <summary>
    /// 获取所有OA模板列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strUserCode"></param>
    public void GetAllArchiveList(HttpContext context,String strRequestLanguage,String strUserCode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbResultData = new StringBuilder();
        String strMethodDesc = "获取所有OA模板列表";
        StringBuilder sbResult = new StringBuilder();

        try
        {
            //查询表中MBArchive_1需要在待办中出现的模板
            String strSql = "SELECT * FROM MBArchive_1 A WHERE A.IsValid = '1' AND A.IsOAShow = '1' ORDER BY TORDER";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

            strReturnCode = "1";
            strReturnMsg = strMethodDesc+"成功";
            sbResultData.Append(WebCommon.GetJsonStringByDataTable(dt, "\"ResultData\"", false));
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
                sbResult.Append(","+sbResultData.ToString());
            }
            sbResult.Append("}");
        }
        //log.Error(strMethodDesc+"时返回数据："+sbResult.ToString());
        context.Response.Write(sbResult.ToString());
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}