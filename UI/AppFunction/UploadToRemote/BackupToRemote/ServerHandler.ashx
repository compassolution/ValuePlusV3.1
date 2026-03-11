<%@ WebHandler Language="C#" Class="ServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.Common.Security;

public class ServerHandler : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    private string strFileSaveRootPath = "~/UserFile/BackupFileFromClient/";
    public void ProcessRequest (HttpContext context) {
        //跨域提交表单，前端ajax不用做任何修改
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//支持全域名访问，不安全，部署后需要固定限制为客户端网址

        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param
        string strFileName = hsTableUrlQuery["filename"] == null ? string.Empty : hsTableUrlQuery["filename"].ToString();//param

        //log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("sendbackupfile"))
        {
            //上传备份文件到远程服务器
            this.DoSendBackupFileToRemote(context,strProjectId,strFileName);
        }
    }

    /// <summary>
    /// 上传备份文件到远程服务器
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    /// <param name="strFileName"></param>
    private void DoSendBackupFileToRemote(HttpContext context,String strProjectId,String strFileSaveName)
    {
        int iReturnResult = -1;
        try
        {
            String strFileBase64String = context.Request["txt_BuildScriptsContent"].ToString();
            //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
            strFileBase64String = SQLInjectionDefense.ReplaceSQLReservedKeyword(strFileBase64String);

            //string strFileSavePath = strFileSaveRootPath + strProjectId;
            string strFileSavePath = strFileSaveRootPath;
            string strCurrentSaveFolderPath = HttpContext.Current.Server.MapPath(strFileSavePath);
            string strCurrentSaveFilePath = strCurrentSaveFolderPath + "\\" + strFileSaveName;

            if (!Directory.Exists(strCurrentSaveFolderPath))
            {
                Directory.CreateDirectory(strCurrentSaveFolderPath);
            }


            byte[] fileByte = Convert.FromBase64String(strFileBase64String);
            String strFileType = "";
            if (fileByte != null && fileByte.Length > 0)//数据是否为空
            {
                //可以获取文件类型
                strFileType = fileByte[0].ToString() + fileByte[1].ToString();

                System.IO.File.WriteAllBytes(strCurrentSaveFilePath, fileByte);//your_bytes就是byte数组

                iReturnResult = 1;
            }
        }
        catch (Exception ex)
        {
            log.Error("上传备份文件到远程服务器出错，解析薪资数据json失败\r\n");
            log.Error("错误信息："+ex.ToString());
        }
        finally
        {
            context.Response.Write(iReturnResult.ToString());
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}