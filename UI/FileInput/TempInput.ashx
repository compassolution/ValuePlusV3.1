<%@ WebHandler Language="C#" Class="TempInput" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class TempInput : IHttpHandler {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private String strPath = "~/UserFile/TempInput/";
    private String strServerFilePath = "";
    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param

        //获取服务器端文件件地址
        strServerFilePath = context.Server.MapPath(strPath);

        if (strParam.Equals("savefile"))
        {
            this.SaveInputFile(context);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void SaveInputFile(HttpContext context)
    {
        try
        {
            HttpFileCollection Files = HttpContext.Current.Request.Files;//该集合是所有fileupload文件的集合。
            if (Files.Count > 0)
            {
                log.Error("TempInput.html上传文件，ServerFilePath：" + strServerFilePath);
                if (!Directory.Exists(strServerFilePath))
                {
                    Directory.CreateDirectory(strServerFilePath);
                }

                String strAllowType = "txt,doc,docx,pdf,xls,xlsx,jpg,png,xml,rpt,bak,";
                bool bIsTypeValid = true;
                for (int i = 0; i < Files.Count; i++)
                {
                    HttpPostedFile PostedFile = Files[i];
                    if (PostedFile.ContentLength > 0)
                    {
                        string FileName = PostedFile.FileName;//文件名自行处理
                        String[] arrayFileName = FileName.Split('.');
                        int iLength = arrayFileName.Length;
                        String strExtType = iLength >= 2 ? arrayFileName[iLength - 1] : "";
                        if(strAllowType.ToLower().IndexOf(strExtType.ToLower() + ",")>-1){
                            PostedFile.SaveAs(strServerFilePath + FileName);
                            log.Error("TempInput.html上传文件：" + strServerFilePath + FileName);
                        }else{
                            bIsTypeValid = false;
                            break;
                        }
                    }
                }
                if (bIsTypeValid)
                {
                    context.Response.Write("1");
                }
                else 
                { 
                    context.Response.Write("-19");
                }
            }else
            {
                context.Response.Write("-9");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("-1");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 根据特殊算法分析url连接，返回参数及其值的hashtable
    /// </summary>
    /// <param name="strUrlQueryString"></param>
    /// <returns>Hashtable</returns>
    public Hashtable GetUrlAnalyse(HttpContext context)
    {
        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        return WebCommon.GetUrlAnalyse(strUrlQueryString);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}