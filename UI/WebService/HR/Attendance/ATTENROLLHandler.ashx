<%@ WebHandler Language="C#" Class="ATTENROLLHandler" %>

using System;
using System.Web;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using Com.ValuePlus.BLL.SysManager;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Entity;
using Com.ValuePlus.Web;
using Com.ValuePlus.Utils.Serializable;

public class ATTENROLLHandler : IHttpHandler {

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public void ProcessRequest (HttpContext context) {
        //解析客户端传递过来的json data
        //StreamReader reader = new StreamReader(context.Request.InputStream);
        //String strParamJson = HttpUtility.UrlDecode(reader.ReadToEnd());
        //log.Error("传入参数的Json data字符串:" + strParamJson.ToString());

        //string strParam = WebCommon.GetJsonValue(strParamJson,"param").ToString();//请求类型参数

        String strUrlQueryString = context.Server.UrlDecode(context.Request.Url.Query.ToString());
        Hashtable hsTableUrlQuery = WebCommon.GetUrlAnalyse(strUrlQueryString);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        string strAccountId = hsTableUrlQuery["accountid"] == null ? string.Empty : hsTableUrlQuery["accountid"].ToString();
        string strPassword = hsTableUrlQuery["password"] == null ? string.Empty : hsTableUrlQuery["password"].ToString();
        string strLid = hsTableUrlQuery["lid"] == null ? string.Empty : hsTableUrlQuery["lid"].ToString();

        string strspname = hsTableUrlQuery["spname"] == null ? string.Empty : hsTableUrlQuery["spname"].ToString();//param
        string strbiztype = hsTableUrlQuery["biztype"] == null ? string.Empty : hsTableUrlQuery["biztype"].ToString();//param
        string strclock = hsTableUrlQuery["clock"] == null ? string.Empty : hsTableUrlQuery["clock"].ToString();//param
        string stretype = hsTableUrlQuery["etype"] == null ? string.Empty : hsTableUrlQuery["etype"].ToString();//param

        string strdcno = hsTableUrlQuery["dcno"] == null ? string.Empty : hsTableUrlQuery["dcno"].ToString();//param
        string strbiodata = hsTableUrlQuery["biodata"] == null ? string.Empty : hsTableUrlQuery["biodata"].ToString();//param
        string strlength = hsTableUrlQuery["length"] == null ? string.Empty : hsTableUrlQuery["length"].ToString();//param
        string strfaceindex = hsTableUrlQuery["faceindex"] == null ? string.Empty : hsTableUrlQuery["faceindex"].ToString();//param
        string strbiotype = hsTableUrlQuery["biotype"] == null ? string.Empty : hsTableUrlQuery["biotype"].ToString();//param

        string strdt = hsTableUrlQuery["dt"] == null ? string.Empty : hsTableUrlQuery["dt"].ToString();//param
        string strverifymode = hsTableUrlQuery["verifymode"] == null ? string.Empty : hsTableUrlQuery["verifymode"].ToString();//param
        string strworkcode = hsTableUrlQuery["workcode"] == null ? string.Empty : hsTableUrlQuery["workcode"].ToString();//param

        switch (strParam.ToLower().ToString())
        {
            case "testconnetct":
                context.Response.Write(this.TestConnetct().ToString());
                break;
            case "getuploadqueue":
                context.Response.Write(this.GETUPLOADQUEUE(strclock).ToString());
                break;
            case "getuserinfo":
                context.Response.Write(this.GETUSERINFO(strspname,strbiztype).ToString());
                break;
            case "executeupdateclock":
                context.Response.Write(this.ExecuteUpdateClock(strclock,stretype).ToString());
                break;
            case "getdatatable":
                context.Response.Write(this.GetDataTable().ToString());
                break;
            case "getbiodatatable":
                context.Response.Write(this.GetBioDataTable(strdcno,strbiotype).ToString());
                break;
            case "savebiodata":
                context.Response.Write(this.SaveBioData(strdcno,strbiodata,strlength,strfaceindex,strbiotype).ToString());
                break;
            case "addattlog":
                context.Response.Write(this.ADDATTLOG(strdcno,strclock,strverifymode,strdt,strworkcode).ToString());
                break;
            case "attupdate":
                context.Response.Write(this.ATTUPDATE(strdcno,strclock,stretype).ToString());
                break;
            case "getallcategorylist":
                context.Response.Write(this.GetAllCategoryList().ToString());
                break;
        }
    }


    /// <summary>
    ///测试远程服务链接
    /// </summary>
    private String TestConnetct()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "测试远程服务链接";
        try
        {

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///获取待上传人员名单
    /// </summary>
    private String GETUPLOADQUEUE(string clock)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "将待同步人员加载入待办队列";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSpName = "USP_HR_ATT_UPLOADQUEUE";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("clock", clock);

            int iSaveCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            if(iSaveCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///获取待上传人员名单
    /// </summary>
    private String ExecuteUpdateClock(string clock,string etype)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "更新考勤机设置";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSpName = "USP_HR_ATT_UPATEATTCLOCK";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("clock", clock);
            hsTableParam.Add("etype", etype);

            int iSaveCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            if(iSaveCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }
    private String ATTUPDATE(string dcno,string clock,string etype)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取待上传人员名单";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSpName = "USP_HR_ATT_UPDATE";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("clock", clock);
            hsTableParam.Add("dcno", dcno);
            hsTableParam.Add("etype", etype);

            int iSaveCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            if(iSaveCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "失败: 获取到0个信息";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    private String ADDATTLOG(string dcno,string clock,string verifymode,string dt,string workcode)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取待上传人员名单";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSpName = "USP_HR_ATT_INSERT" ;
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("clock", clock);
            hsTableParam.Add("dcno", dcno);
            hsTableParam.Add("verifymode", verifymode);
            hsTableParam.Add("dt", dt);
            hsTableParam.Add("workcode", workcode);
            int iSaveCount = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            if(iSaveCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "失败: 获取到0个信息";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    private String SaveBioData(string dcno,string biodata,string length,string faceindex,string biotype)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取待上传人员名单";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("delete from tb_biotable where dcno='" + dcno + "' and BIOType='" + biotype + "' and FaceIndex='" + faceindex + "'  insert into TB_Biotable (dcno,biodata,dt,BDLength,FaceIndex,BIOType) values ('" + dcno + "','" + biodata + "',getdate(),'" + length + "','" + faceindex + "','" + biotype + "')  ");
            Hashtable hsTableParam = new Hashtable();
            String strSql = sbSql.ToString();
            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

            if(iCount>0){
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }else{
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "失败: 获取到0个信息";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///账号密码登录
    /// </summary>
    private String DoLogin(String strAccountId,String strPassword)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "账号密码登录";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" SELECT COUNT(*) FROM TB_HR_USER WHERE SUSERID = '" + strAccountId + "' AND dbo.fun_Decode_Password(SPWD) = '" + strPassword + "'");
            int iCount = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());

            if (iCount > 0)
            {
                strReturnCode = "1";
                strReturnMsg = strMethodDesc + "成功";
            }
            else
            {
                strReturnCode = "-1";
                strReturnMsg = strMethodDesc + "失败,请确认账号密码的正确性!";
            }
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    ///根据ClockID获取VW_HRCARD数据信息
    /// </summary>
    private String GETUSERINFO(String spanme,string biztype)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据ClockID获取VW_HRCARD数据信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            if (spanme=="IP")
            {
                sbSql.Append("select 1 from attclock_1 where acip='" + biztype + "' and isnull(acclearenroll,'')='true'");
            }else if  (spanme=="AttClock")
            {
                if (biztype=="UserInfo1")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N1UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='IN' and isnull(CLOCKID,'')='1' and uploaddt is null)");
                else if  (biztype=="UserInfo2")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N2UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='OUT' and isnull(CLOCKID,'')='2' and uploaddt is null)");
                else if  (biztype=="UserInfo3")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N3UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='CANTEEN' and isnull(CLOCKID,'')='3' and uploaddt is null)");
                else if (biztype=="UserInfo11")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N11UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='IN' and isnull(CLOCKID,'')='11' and uploaddt is null)");
                else if  (biztype=="UserInfo22")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N22UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='OUT' and isnull(CLOCKID,'')='22' and uploaddt is null)");
                else if  (biztype=="UserInfo33")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N33UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='CANTEEN' and isnull(CLOCKID,'')='33' and uploaddt is null)");
                else if  (biztype=="UserInfo4")
                    sbSql.Append("select dcno,DCNAMECHS,cardnum,bEnable,pwd from VW_HRATTCARD where (isnull(cardnum,'')<>'' and isnull(N4UL,'')<>'true') or (isnull(attstat,'')='000' and ISSTOP='true' or isnull(attstat,'')='090' and ISSTOP='false' ) and dcno in (select dcno from TB_HRATTCARD_UPLOADQUEUE where isnull(CLOCKTYPE,'')='HR' and isnull(CLOCKID,'')='4' and uploaddt is null)");

            }
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///根据ClockID获取VW_HRCARD数据信息
    /// </summary>
    private String GetDataTable()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据ClockID获取VW_HRCARD数据信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select dcno  from VW_HRATTCARD where isnull(ISDWBIO,'')='true' ");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///根据ClockID获取VW_HRCARD数据信息
    /// </summary>
    private String GetBioDataTable(string dcno,string biotype)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据DCNO获取BIO数据信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select dcno,biodata,bdlength,faceindex from TB_Biotable where dcno='" + dcno + "' and biotype='" + biotype + "' ");

            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///获取全部固定格式的使用部门信息
    /// </summary>
    private String GetAllDeptList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的使用部门信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_CSORGA where LID = 'CSORGA1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    ///获取全部固定格式的存放地址信息
    /// </summary>
    private String GetAllLocationList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的存放地址信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_AMLOCATION where LID = 'AMLOCATION1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }

    /// <summary>
    ///获取全部固定格式的资产分类信息
    /// </summary>
    private String GetAllCategoryList()
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "获取全部固定格式的资产分类信息";
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from VW_Sys_TREE_ASSETSCLASS where LID = 'ASSETSCLASS1'");
            sbSql.Append(" ORDER BY P9");
            String strSql = sbSql.ToString();
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            //将DataTable转为Byte[]
            DataSet ds = dt.DataSet;
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
            sbReturnData.Append(Convert.ToBase64String(db));

            strReturnCode = "1";
            strReturnMsg = strMethodDesc + "成功";
        }
        catch (Exception ex)
        {
            strReturnCode = "-99";
            strReturnMsg = strMethodDesc + "出错";
            log.Error(ex);
        }
        finally
        {
            StringBuilder sbResultStatus = new StringBuilder();
            sbResultStatus.Append("{");
            sbResultStatus.Append("\"ReturnCode\":\"" + strReturnCode.ToString() + "\"");
            sbResultStatus.Append(",\"ReturnMsg\":\"" + strReturnMsg.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            //log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}