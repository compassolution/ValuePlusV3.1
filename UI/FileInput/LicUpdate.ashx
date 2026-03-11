<%@ WebHandler Language="C#" Class="LicUpdate" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class LicUpdate : IHttpHandler {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param

        //获取服务器端文件件地址
        String strPath = "~/SysFile/";
        String strServerFilePath = context.Server.MapPath(strPath);

        if (strParam.Equals("filelist"))
        {
            this.GetDFileList(context,strServerFilePath);
        }
        else if (strParam.Equals("savefile"))
        {
            this.SaveInputFile(context,strServerFilePath);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    private void SaveInputFile(HttpContext context,String strServerFilePath)
    {
        try
        {
            HttpFileCollection Files = HttpContext.Current.Request.Files;//该集合是所有fileupload文件的集合。
            if (Files.Count ==1)
            {
                log.Error("LicUpdate.html上传文件，ServerFilePath：" + strServerFilePath);
                if (!Directory.Exists(strServerFilePath))
                {
                    Directory.CreateDirectory(strServerFilePath);
                }

                bool bIsTypeValid = true;
                HttpPostedFile PostedFile = Files[0];
                if (PostedFile.ContentLength > 0)
                {
                    string FileName = PostedFile.FileName;//文件名自行处理
                    if(FileName.ToLower().Equals("license.lic")){
                        PostedFile.SaveAs(strServerFilePath + FileName);
                        log.Error("LicUpdate.html更新Lic：" + strServerFilePath + FileName);
                    }else{
                        bIsTypeValid = false;
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
            }
            else
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

    #region 获取源目录的文件列表
    /// <summary>
    /// 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
    /// </summary>
    /// <param name="strFilePath"></param>
    private void GetDFileList(HttpContext context,String strServerFilePath)
    {
        if (Directory.Exists(strServerFilePath))
        {
            String strEncodePath =  HttpUtility.UrlEncode(strServerFilePath) ;
            DirectoryInfo thisOne = new DirectoryInfo(strServerFilePath);
            FileInfo[] fileInfo = thisOne.GetFiles();
            StringBuilder sb = new StringBuilder("");
            sb.Append("\r\n");
            // 遍历所有的文件和目录
            foreach (FileInfo file in fileInfo)
            {
                String strFileName = file.Name;
                String strFileLength = file.Length.ToString();
                String strEncodeFileName = HttpUtility.UrlEncode(strFileName);
            }


            StringBuilder sBuilder = new StringBuilder();

            //sBuilder.Append("{");
            //sBuilder.Append("\"totalCount\":"+iRowsCount.ToString());
            //sBuilder.Append(",");
            //sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
            //sBuilder.Append("}");
        }
    }
    #endregion


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