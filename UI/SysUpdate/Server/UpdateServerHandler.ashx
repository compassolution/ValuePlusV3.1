<%@ WebHandler Language="C#" Class="UpdateServerHandler" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;

public class UpdateServerHandler :  HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strCallback = hsTableUrlQuery["callback"] == null ? string.Empty : hsTableUrlQuery["callback"].ToString();//param
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strDeployNo = hsTableUrlQuery["deployno"] == null ? string.Empty : hsTableUrlQuery["deployno"].ToString();//param
        string strProjectId = hsTableUrlQuery["projectid"] == null ? string.Empty : hsTableUrlQuery["projectid"].ToString();//param

        string strUpdateUser = hsTableUrlQuery["updateuser"] == null ? string.Empty : hsTableUrlQuery["updateuser"].ToString();//param
        string strUpdateIP = hsTableUrlQuery["updateip"] == null ? string.Empty : hsTableUrlQuery["updateip"].ToString();//param
        string strUpdateAppWebSite = hsTableUrlQuery["updatewebsite"] == null ? string.Empty : hsTableUrlQuery["updatewebsite"].ToString();//param

        if (String.IsNullOrEmpty(strParam)) {
            //如果通过get方式链接获取参数失败，则采用post过来的字符串中获取，主要是配合新版CS平台中的部分接口
            String strParamJson = WebCommon.GetJsonParamsFromContext(context);
            strParam = WebCommon.GetJsonValue(strParamJson, "param").ToString();
            strDeployNo = WebCommon.GetJsonValue(strParamJson, "deployno").ToString();
            strProjectId = WebCommon.GetJsonValue(strParamJson, "projectid").ToString();
            strUpdateUser = WebCommon.GetJsonValue(strParamJson, "updateuser").ToString();
            strUpdateIP = WebCommon.GetJsonValue(strParamJson, "updateip").ToString();
            strUpdateAppWebSite = WebCommon.GetJsonValue(strParamJson, "updatewebsite").ToString();
        }

        log.Error("(UpdateServerHandler)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Param:"+strParam);
        if (strParam.Equals("getcount"))
        {
            this.GetUpdateCount(context,strProjectId,strUpdateAppWebSite);
        }
        else if (strParam.Equals("getlist"))
        {
            this.GetUpdateList(context,strProjectId,strUpdateAppWebSite);
        }
        else if (strParam.Equals("getfile"))
        {
            this.GetSysUpdateFile(context, strDeployNo);
        }
        else if (strParam.Equals("success"))
        {
            this.UpdateFileSuccess(context, strDeployNo,strProjectId,strUpdateAppWebSite,strUpdateUser,strUpdateIP);
        }
    }

    /// <summary>
    /// 获取可更新的数量
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    private void GetUpdateCount(HttpContext context,String strProjectId,String strUpdateAppWebSite)
    {
        StringBuilder sbSql = new StringBuilder();
        sbSql.Append("select count(*) from Deploy_1 A inner join Deploy_2 B ON A.DEPLOYNO = B.DEPLOYNO ");
        sbSql.Append(" where B.ProjectId = '" + strProjectId + "' ");
        sbSql.Append(" AND ISNULL(B.ISDEPLOY,'false') = 'false'");

        //在线更新功能，同一个ProjectId多个网站可同时进行在线更新。
        //sbSql.Append(" AND charindex('"+strUpdateAppWebSite+"',isnull(SREMARK,'')) <=0");

        try
        {
            int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
            //log.Error("(UpdateServerHandler--GetUpdateCount)"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"---Sql:"+sbSql.ToString());
            context.Response.Write("jsonpCallback({\"count\":\""+iCount.ToString()+"\"})");
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
            context.Response.Write("jsonpCallback({\"count\":\"-1\"})");
        }
    }

    /// <summary>
    /// 获取可更新的更新列表
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strProjectId"></param>
    private void GetUpdateList(HttpContext context,String strProjectId,String strUpdateAppWebSite)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            sbSql.Append("select A.*,CONVERT(VARCHAR(20),A.DeployDate,120) as DeployTime from Deploy_1 A inner join Deploy_2 B ON A.DEPLOYNO = B.DEPLOYNO ");
            sbSql.Append(" where B.ProjectId = '" + strProjectId + "' ");
            sbSql.Append(" AND ISNULL(B.ISDEPLOY,'false') = 'false'");

            //在线更新功能，同一个ProjectId多个网站可同时进行在线更新。
            //sbSql.Append(" AND charindex('"+strUpdateAppWebSite+"',isnull(SREMARK,'') )<=0");

            sbSql.Append(" order by A.DEPLOYNO DESC");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            int iColCount = dt.Columns.Count;
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("jsonpCallback(");
            sBuilder.Append("{ \"ResultData\":[ ");
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                //sBuilder.Append("   " + this.ulHeaderMenu.ClientID + ".innerHTML =\"\r\n");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0)
                    {
                        sBuilder.Append(",{");
                    }
                    else
                    {
                        sBuilder.Append("{");
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        String strColName = dt.Columns[j].ColumnName;
                        //对值进行编码处理特殊字符，如引号等
                        String strColValue = Microsoft.JScript.GlobalObject.escape(dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                        //String strColValue = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (j == 0)
                        {
                            sBuilder.Append("\"" + strColName + "\":\"" + strColValue + "\"");
                        }
                        else
                        {
                            sBuilder.Append(",\"" + strColName + "\":\"" + strColValue + "\"");
                        }
                    }

                    sBuilder.Append("}");
                }

            }
            sBuilder.Append("]");
            sBuilder.Append("}");
            sBuilder.Append(")");

            string json = sBuilder.ToString();

            context.Response.Write(json);

        }
        catch (Exception ex)
        {
            log.Error(strProjectId+"获取可更新的更新列表出错："+ex+" \r\n");
            log.Error("sql语句为："+sbSql.ToString());
            context.Response.Write("jsonpCallback({\"ResultData\":\"error\"})");
        }
    }

    /// <summary>
    /// 获取更新文件包
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDeployNo"></param>
    /// <returns></returns>
    public void GetSysUpdateFile(HttpContext context,String strDeployNo)
    {
        try
        {
            FileStream fs = null;
            byte[] btReturn = new byte[0];
            String strSql = "select * from TB_HRTMPD WHERE TID = 'Deploy' and GID = '1' AND PID = 'DEPLOYATT'";
            String strTempFolder = "DeployFile";
            try
            {
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    strTempFolder = dt.Rows[0]["PCTRLD"].ToString();
                }
            }
            catch (Exception ex)
            {
                btReturn = new byte[0];
                log.Error(ex.ToString());
            }
            string strFilePath = "~/UserFile/ArchiveAtt/" + strTempFolder + "/DEPLOY-1/" + strDeployNo;
            string CurrentUploadFolderPath = HttpContext.Current.Server.MapPath(strFilePath);

            if (Directory.Exists(CurrentUploadFolderPath))
            {
                String strEncodePath = HttpUtility.UrlEncode(CurrentUploadFolderPath);
                DirectoryInfo thisOne = new DirectoryInfo(CurrentUploadFolderPath);
                FileInfo[] fileInfo = thisOne.GetFiles();
                StringBuilder sb = new StringBuilder("");
                int i = 0;

                // 遍历所有的文件和目录
                foreach (FileInfo file in fileInfo)
                {
                    //只取第一个文件，只支持一个文件
                    if (i > 0)
                    {
                        break;
                    }
                    String strFileName = file.Name;
                    String strFileLength = file.Length.ToString();
                    String strEncodeFileName = HttpUtility.UrlEncode(strFileName);
                    String CurrentUploadFilePath = CurrentUploadFolderPath + "\\" + strFileName;
                    if (File.Exists(CurrentUploadFilePath))
                    {
                        try
                        {
                            ///打开现有文件以进行读取。
                            fs = File.OpenRead(CurrentUploadFilePath);
                            int b1;
                            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
                            while ((b1 = fs.ReadByte()) != -1)
                            {
                                tempStream.WriteByte(((byte)b1));
                            }
                            btReturn = tempStream.ToArray();
                        }
                        catch (Exception ex)
                        {
                            btReturn = new byte[0];
                            log.Error(ex.ToString());
                        }
                        finally
                        {
                            fs.Close();
                        }
                    }
                    else
                    {
                        btReturn = new byte[0];
                    }

                    i++;
                }
                context.Response.Write("jsonpCallback({\"fileString\":\"" + Convert.ToBase64String(btReturn) + "\"})");
            }else
            {
                context.Response.Write("jsonpCallback({\"fileString\":\"\"})");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            context.Response.Write("jsonpCallback({\"fileString\":\"error\"})");
        }

    }

    /// <summary>
    /// 成功更新后设置服务器更新标志
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strDeployNo"></param>
    /// <param name="strProjectId"></param>
    private void UpdateFileSuccess(HttpContext context,String strDeployNo,String strProjectId,String strUpdateAppWebSite,String strUpdateUser,String strUpdateIP)
    {
        StringBuilder sbSql = new StringBuilder();
        try
        {
            String strUpdateDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            sbSql.Append("update Deploy_2 set ISDEPLOY = 'true',DEPLOYDATE = '" + strUpdateDateTime +"'");
            sbSql.Append(" ,SREMARK = isnull(SREMARK,'') + '\r\n\r\n系统自动更新至服务器："+strUpdateAppWebSite+"\r\n更新用户："+strUpdateUser+"\r\n用户客户端IP："+strUpdateIP+"'");
            sbSql.Append(" WHERE DEPLOYNO = '"+ strDeployNo + "' AND ProjectId = '"+ strProjectId + "'");

            //log.Error("更新成功后的处理:"+sbSql.ToString());
            int btReturn = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());

            context.Response.Write("jsonpCallback({\"flag\":\"" + btReturn.ToString() + "\"})");
        }
        catch (Exception ex)
        {
            log.Error("成功更新后设置服务器更新标志出错，SQL:"+sbSql.ToString());
            log.Error(ex.ToString());
            context.Response.Write("jsonpCallback({\"flag\":\"-1\"})");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}