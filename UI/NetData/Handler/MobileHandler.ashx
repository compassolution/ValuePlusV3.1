<%@ WebHandler Language="C#" Class="MobileHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Common.Security;

public class MobileHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        //string param = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        //string strMobileNo = hsTableUrlQuery["mobileno"] == null ? string.Empty : hsTableUrlQuery["mobileno"].ToString();//
        string param = context.Request["param"] == null ? string.Empty : context.Request["param"].ToString();//param
        string strMobileNo = context.Request["mobileno"] == null ? string.Empty : context.Request["mobileno"].ToString();//
        
        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        param = SQLInjectionDefense.ReplaceSQLReservedKeyword(param);
        strMobileNo = SQLInjectionDefense.ReplaceSQLReservedKeyword(strMobileNo);
        //log.Error(strMobileNo);
        switch (param.ToLower().ToString())
        {
            case "querymobileinfo":
                Hashtable hsTable = new Hashtable();
                hsTable.Add("mobileCode", strMobileNo);
                hsTable.Add("userID", "");
                String strUrl = "http://www.webxml.com.cn/WebServices/MobileCodeWS.asmx";
                String strMethod = "getMobileCodeInfo ";

                this.BuildResponseJson(context, strMethod, strUrl, strMethod, hsTable);
                
                break;
        }
    }

    /// <summary>
    /// 根据所需格式创建Json
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strMobileNo"></param>
    /// <param name="strUrl"></param>
    /// <param name="strMethod"></param>
    /// <param name="hsTable"></param>
    private void BuildResponseJson(HttpContext context, String strMobileNo, String strUrl, String strMethod, Hashtable hsTable)
    {
        try
        {
            String strReturn = GetInnerTextFromWS(strUrl, strMethod, hsTable);
            String[] strArray1 = strReturn.Split('：');
            String[] strArray2 = strArray1[1].Split(' ');
            String strProvince = strArray2[0];
            String strCity = strArray2[1];
            String strCardType = strArray2[2];
            strCardType = strCardType.Replace(strProvince, "");
            strCardType = strCardType.Substring(0, 2);
            
            //获取在系统中省份对应的编码
            String strSql = "select * from TB_HRLSTD WHERE CDESCCHS LIKE '%" + strProvince + "%'";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            String strProvinceCode = "";
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                strProvinceCode = dt.Rows[0]["CID"].ToString();
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{ResultData:[ ");
            sBuilder.Append("{MobileNo:'" + strMobileNo + "',CardType:'" + strCardType + "',Province:'" + strProvince + "',ProvinceCode:'" + strProvinceCode + "'}");
            sBuilder.Append("]}");


            context.Response.Write(sBuilder.ToString());
        }
        catch (Exception ex)
        {
            context.Response.Write("error");
            log.Error(ex.ToString());
        }
    }

    /// <summary>
    /// 获取WebService返回的内容
    /// </summary>
    /// <param name="strUrl"></param>
    /// <param name="strMethod"></param>
    /// <param name="hsTable"></param>
    /// <returns></returns>
    private static String GetInnerTextFromWS(String strUrl, String strMethod, Hashtable hsTable)
    {
        XmlDocument xmlReturn = QueryGetWebService(strUrl, strMethod, hsTable);
        //读取节点
        //XmlNode snXmlNode = xmlReturn.SelectSingleNode("String");
        String strInnerText = xmlReturn.InnerText;

        return strInnerText;
    }

    /// <summary>
    /// 需要WebService支持Get调用
    /// </summary>
    /// <param name="URL"></param>
    /// <param name="MethodName"></param>
    /// <param name="Pars"></param>
    /// <returns></returns>
    private static XmlDocument QueryGetWebService(String URL, String MethodName, Hashtable Pars)
    {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName + "?" + ParsToString(Pars));
        request.Method = "GET";
        request.ContentType = "application/x-www-form-urlencoded";
        SetWebRequest(request);
        return ReadXmlResponse(request.GetResponse());
    }

    /// <summary>
    /// 读取Xml Document
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    private static XmlDocument ReadXmlResponse(WebResponse response)
    {
        StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        String retXml = sr.ReadToEnd();
        sr.Close();
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(retXml);
        return doc;
    }
    
    /// <summary>
    /// 设置凭证与超时时间
    /// </summary>
    /// <param name="request"></param>
    private static void SetWebRequest(HttpWebRequest request)
    {
        request.Credentials = CredentialCache.DefaultCredentials;
        request.Timeout = 10000;
    }

    /// <summary>
    /// 格式化链接参数
    /// </summary>
    /// <param name="Pars"></param>
    /// <returns></returns>
    private static String ParsToString(Hashtable Pars)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string k in Pars.Keys)
        {
            if (sb.Length > 0)
            {
                sb.Append("&");
            }
            sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
        }
        return sb.ToString();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}