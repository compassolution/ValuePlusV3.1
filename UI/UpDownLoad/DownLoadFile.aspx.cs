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
using System.IO;
using Com.ValuePlus.Web;
using Com.ValuePlus.Common;
using Com.ValuePlus.Common.Security;

public partial class UpDownLoad_DownLoadFile : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        string strPath = Server.UrlDecode(Request.QueryString["path"]);
        string strFileName = Server.UrlDecode(Request.QueryString["fileName"]);
        string strOpType = Server.UrlDecode(Request.QueryString["opType"]);

        //替换SQL保留关键字以防止SQL注入风险 add by sammen 20230329
        strPath = SQLInjectionDefense.ReplaceSQLReservedKeyword(strPath);
        strFileName = SQLInjectionDefense.ReplaceSQLReservedKeyword(strFileName);
        strOpType = SQLInjectionDefense.ReplaceSQLReservedKeyword(strOpType);

        //add by sammane by sammen 20230328
        //增加一个token机制，实现通过页面看到文件了以后才能点击进行下载，直接通过url参数输入时不能下载
        //UpDownLoad/FileInput.aspx、UpDownLoad/DownFileList.aspx等页面传递
        string strFileFolder = Server.UrlDecode(Request.QueryString["folder"]);
        string strToken = Server.UrlDecode(Request.QueryString["token"]);
        bool isTokenValid = false;
        if (!string.IsNullOrEmpty(strFileFolder) && !string.IsNullOrEmpty(strToken))
        {
            try
            {
                strToken = Microsoft.JScript.GlobalObject.decodeURIComponent(strToken);//先解密
                //strToken = UrlParamEncryption.Decrypt3des(strToken, System.Text.Encoding.UTF8);//先解密
                String[] strArray = strToken.Split('＃');
                if (strArray.Length == 2 && strArray[0].Equals(strFileFolder) && strArray[1].Length == 14)
                {
                    isTokenValid = true;
                }
            }catch(Exception ex)
            {
                isTokenValid = false;
            }
        }

        if (!isTokenValid)
        {
            Response.Write("<script language=\"javascript\">alert('Invalid file operation token!');</script>");
        }
        else
        {
            if (!string.IsNullOrEmpty(strPath) && !string.IsNullOrEmpty(strFileName) )
            {
                strPath = System.IO.Path.Combine(strPath, strFileName);
                if (File.Exists(strPath))
                {
                    if (!String.IsNullOrEmpty(strOpType))
                    {
                        if (strOpType.Equals("down"))
                        {
                            System.IO.FileInfo fileInfo = new System.IO.FileInfo(strPath);
                            if (fileInfo.Length <= 0)
                            {
                                Response.Write("<script language=javascript>alert('The File Size is Zero,Cannot DownLoad!');</script>");
                                return;
                            }
                            else
                            {
                                try
                                {
                                    Com.ValuePlus.Utils.RequestUtils.DownLoadFile(strFileName, strPath);
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("<script language=\"javascript\">alert('Failed,The is Using,Please try again later!');</script>");
                                    log.Error(ex);
                                }
                            }
                        }
                        else if (strOpType.Equals("del"))
                        {
                            System.IO.File.Delete(strPath);
                        }
                    }
                    else
                    {
                        Com.ValuePlus.Utils.RequestUtils.DownLoadFile(strFileName, strPath);
                    }
                }
                else
                {
                    Response.Write("<script language=\"javascript\">alert('The File is not Exsits!');</script>");
                }
            }
        }
    }

}
