<%@ WebHandler Language="C#" Class="OEList" %>

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

public class OEList : IHttpHandler {

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

        string strDeptCode = hsTableUrlQuery["deptcode"] == null ? string.Empty : hsTableUrlQuery["deptcode"].ToString();
        string strLocationCode = hsTableUrlQuery["locationcode"] == null ? string.Empty : hsTableUrlQuery["locationcode"].ToString();
        string strCategoryCode = hsTableUrlQuery["categorycode"] == null ? string.Empty : hsTableUrlQuery["categorycode"].ToString();
        string strSACODE = hsTableUrlQuery["sacode"] == null ? string.Empty : hsTableUrlQuery["sacode"].ToString();
        string strSBARCODE = hsTableUrlQuery["sbarcode"] == null ? string.Empty : hsTableUrlQuery["sbarcode"].ToString();
        string strSANAME = hsTableUrlQuery["saname"] == null ? string.Empty : hsTableUrlQuery["saname"].ToString();
        string strSANAMECHS = hsTableUrlQuery["sanamechs"] == null ? string.Empty : hsTableUrlQuery["sanamechs"].ToString();
        string strSMODEL = hsTableUrlQuery["smodel"] == null ? string.Empty : hsTableUrlQuery["smodel"].ToString();
        string strLabelType = hsTableUrlQuery["labeltype"] == null ? string.Empty : hsTableUrlQuery["labeltype"].ToString();
        string strIsBinding = hsTableUrlQuery["isbinding"] == null ? string.Empty : hsTableUrlQuery["isbinding"].ToString();
        string strIsHadImage = hsTableUrlQuery["ishadimage"] == null ? string.Empty : hsTableUrlQuery["ishadimage"].ToString();

        int iPageSize = hsTableUrlQuery["pagesize"] == null ? 20 : int.Parse(hsTableUrlQuery["pagesize"].ToString());
        int iPageIndex = hsTableUrlQuery["pageindex"] == null ? 0 : int.Parse(hsTableUrlQuery["pageindex"].ToString());

        switch (strParam.ToLower().ToString())
        {
            case "getoelistbycondition":
                context.Response.Write(this.GetOEListCondition(strDeptCode,strLocationCode,strCategoryCode,strSACODE,strSBARCODE,strSANAME,strSANAMECHS
                    ,strSMODEL,strLabelType,strIsBinding,strIsHadImage,iPageSize,iPageIndex).ToString());
                break;
            case "getoelist"://获取需要导出图片的oe列表
                context.Response.Write(this.GetOEList(strDeptCode,strLocationCode,strIsHadImage).ToString());
                break;
        }
    }

    /// <summary>
    ///根据条件获取资产数据记录
    /// </summary>
    private String GetOEListCondition(String strDeptCode,String strLocationCode,String strCategoryCode,String strSACODE,String strSBARCODE,String strSANAME,String strSANAMECHS
        ,String strSMODEL,String strLabelType,String strIsBinding,String strIsHadImage,int iPageSize,int iPageIndex)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据条件获取资产数据记录";

        int iTotalRecord = 0;
        int iTotalPage = 0;
        try
        {
            StringBuilder sbCondition = new StringBuilder();
            StringBuilder sbOrderBy = new StringBuilder();
            sbCondition.Append(" and 1=1 \r\n");
            sbOrderBy.Append("");
            
            if (!String.IsNullOrEmpty(strSACODE))
            {
                sbCondition.Append(" and (A.SACODE like '%" + strSACODE.Replace("'","''") + "%') \r\n");
            }else{
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"SACODE");
            }
            if (!String.IsNullOrEmpty(strDeptCode))
            {
                sbCondition.Append(" AND (A.SUSEDEPT in (SELECT SubCode from dbo.[Fun_AM_GetSubDept]('" + strDeptCode.Replace("'", "''") + "','1')) or A.SUSEDEPT = '" + strDeptCode.Replace("'", "''") + "') \r\n");
            }
            else
            {
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString()) ? "" : ",") + "SUSEDEPT");
            }
            if (!String.IsNullOrEmpty(strLocationCode))
            {
                sbCondition.Append(" AND (A.SLCODE in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('" + strLocationCode.Replace("'", "''") + "','1')) or A.SLCODE = '" + strLocationCode.Replace("'", "''") + "') \r\n");
            }
            else
            {
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString()) ? "" : ",") + "SLCODE");
            }
            if (!String.IsNullOrEmpty(strCategoryCode))
            {
                sbCondition.Append(" AND (A.STYPE in (SELECT SubCode from dbo.[Fun_AM_GetSubAMClass]('" + strCategoryCode.Replace("'","''") + "','1')) or A.STYPE = '" + strCategoryCode.Replace("'","''") + "') \r\n");
            }
            //由于界面没有加载stype的数据会报错
            //else{
            //    sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"STYPE");
            //}
            if (!String.IsNullOrEmpty(strSBARCODE))
            {
                sbCondition.Append(" and (A.SBARCODE = '" + strSBARCODE.Replace("'","''") + "') \r\n");
            }
            if (!String.IsNullOrEmpty(strSANAME))
            {
                sbCondition.Append(" and (A.SANAME LIKE '%" + strSANAME.Replace("'","''") + "%') \r\n");
            }else{
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"SANAME");
            }
            if (!String.IsNullOrEmpty(strSANAMECHS))
            {
                sbCondition.Append(" and (A.SANAMECHS LIKE '%" + strSANAMECHS.Replace("'","''") + "%') \r\n");
            }else{
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"SANAMECHS");
            }
            if (!String.IsNullOrEmpty(strSMODEL))
            {
                sbCondition.Append(" and (A.SMODEL LIKE '%" + strSMODEL.Replace("'","''") + "%') \r\n");
            }else{
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"SMODEL");
            }
            if (!String.IsNullOrEmpty(strLabelType))
            {
                sbCondition.Append(" and (A.LABELTYPE = '" + strLabelType + "') \r\n");
            }else{
                sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"LABELTYPE");
            }
            if (!String.IsNullOrEmpty(strIsBinding))
            {
                ///已绑定RFID标签
                if (strIsBinding.Equals("1"))
                {
                    sbCondition.Append(" and (ISNULL(A.SBARCODE,'') <> '' and ISNULL(A.SBARCODE,'')<>A.SACODE) \r\n");
                    sbOrderBy.Append((String.IsNullOrEmpty(sbOrderBy.ToString())?"":",")+"SBARCODE");
                }
                ///未绑定RFID标签
                else if (strIsBinding.Equals("2"))
                {
                    sbCondition.Append(" and (ISNULL(A.SBARCODE,'') = '' or ISNULL(A.SBARCODE,'')=A.SACODE) \r\n");
                }
            }
            if (!String.IsNullOrEmpty(strIsHadImage))
            {
                sbCondition.Append(" and (ISNULL(A.BISHADIMAGE,'2') = '" + strIsHadImage + "') \r\n");
            }

            String strCondition = sbCondition.ToString();

            String strTableName_OEInfo = "VW_OE_AssetDetail_ForLabelPrint";
            StringBuilder sbSql_RecordCount = new StringBuilder();
            StringBuilder sbSql = new StringBuilder();
            sbSql_RecordCount.Append("SELECT count(1)");
            //sbSql.Append("SELECT ROW_NUMBER() OVER(ORDER BY A.SUSEDEPT,A.SLCODE,A.SACODE DESC) as SEQNO \r\n");
            sbSql.Append(" SELECT A.SACODE,A.SBARCODE,A.LABELTYPE,A.LABELTYPENAMECHS,A.SANAME,A.SANAMECHS,A.CLASSNAMECN,A.SMODEL,ltrim(rtrim(A.UNITNAMECN)) as UNITNAMECN \r\n");
            sbSql.Append(" ,A.SLCODE,A.LOCATIONNAMECN,A.SUSEDEPT,A.SUSEDEPTNAMECN \r\n");
            sbSql.Append(" ,A.UnitPrice,A.NQUANTITY,A.TotalQty \r\n");
            //sbSql.Append(" ,(CASE WHEN (ISNULL(A.SBARCODE,'') <> '' and ISNULL(A.SBARCODE,'')<>A.SACODE) THEN '是' ELSE '否' END) AS IsBinding \r\n");
            sbSql.Append(" ,isnull((select CDESCCHS from TB_HRLSTD WHERE LID = 'BOOL' AND CID = isnull(A.BISHADIMAGE,'')),'否') as IsHadImage,A.BISHADIMAGE  \r\n");
            sbSql.Append(" ,A.ImageFileFullName \r\n");

            sbSql_RecordCount.Append(" FROM " + strTableName_OEInfo + " A where 1=1 \r\n");
            sbSql.Append(" FROM " + strTableName_OEInfo + " A where 1=1 \r\n");
            if (!String.IsNullOrEmpty(strCondition))
            {
                sbSql_RecordCount.Append(strCondition);
                sbSql.Append(strCondition);
            }
            //sbSql.Append(" order by A.SUSEDEPT,A.SLCODE,A.SACODE DESC");
            String strSql_RecordCount = sbSql_RecordCount.ToString();
            //分页前需要设置排序规则
            String strSql = "SELECT ROW_NUMBER() OVER(ORDER BY "+sbOrderBy.ToString()+") as SEQNO,* from ("+sbSql.ToString()+") tbAll \r\n";

            //全部记录数
            iTotalRecord = SqlParamDao.ExecuteScalarBySql(strSql_RecordCount);
            iTotalPage = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(iTotalRecord) / Convert.ToDecimal(iPageSize))));
            int iCurPage = iPageIndex + 1 > iTotalPage ? iTotalPage : iPageIndex + 1;
            iCurPage = iCurPage == 0 ? 1 : iCurPage;
            //this.iPageIndex = int.Parse(((ListItem)this.ddList_PageIndex.SelectedItem).Value.ToString());

            StringBuilder sbSql_Page = new StringBuilder();
            sbSql_Page.Append("select TOP "+(iPageSize).ToString()+" * ");
            sbSql_Page.Append(" from ("+ strSql + ") as tbA");
            sbSql_Page.Append(" where 1=1 ");
            sbSql_Page.Append(" and tbA.SACODE NOT IN (SELECT TOP " + ((iCurPage-1) * iPageSize).ToString() + " SACODE FROM ("+ strSql + ") AS tbB order by tbB.SEQNO) ");
            sbSql_Page.Append(" order by tbA.SEQNO ");
            String strSql_Page = sbSql_Page.ToString();

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql_Page);
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
            sbResultStatus.Append(",\"TotalRecord\":\"" + iTotalRecord.ToString() + "\"");
            sbResultStatus.Append(",\"TotalPage\":\"" + iTotalPage.ToString() + "\"");
            sbResultStatus.Append(",\"PageSize\":\"" + iPageSize.ToString() + "\"");
            sbResultStatus.Append(",\"PageIndex\":\"" + iPageIndex.ToString() + "\"");

            sbResult.Append(sbResultStatus.ToString());
            if (!String.IsNullOrEmpty(sbReturnData.ToString()))
            {
                sbResult.Append(",\"ReturnData\":\"" + sbReturnData.ToString()+"\"");
            }
            sbResult.Append("}");
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    /// <summary>
    ///根据条件获取OE资产数据记录
    /// </summary>
    private String GetOEList(String strDeptCode,String strLocationCode,String strIsHadImage)
    {
        String strReturnCode = "0";
        String strReturnMsg = "";
        StringBuilder sbReturnData = new StringBuilder();
        StringBuilder sbResult = new StringBuilder();
        String strMethodDesc = "根据条件获取OE资产数据记录";

        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * from [VW_OE_AssetDetail_ForLabelPrint] A where 1=1 ");
            //String strSql = "select * from [VW_OE_AssetDetail_ForLabelPrint] where SIMAGE IS NOT NULL order by SACODE,SLCODE";

            if (!String.IsNullOrEmpty(strDeptCode))
            {
                sbSql.Append(" AND (ISNULL(A.SUSEDEPT,'') in (SELECT SubCode from dbo.[Fun_AM_GetSubDept]('" + strDeptCode + "','1')) or ISNULL(A.SUSEDEPT,'') = '" + strDeptCode + "')");
            }
            if (!String.IsNullOrEmpty(strLocationCode))
            {
                sbSql.Append(" AND (ISNULL(A.SLCODE,'') in (SELECT SubCode from dbo.[Fun_AM_GetSubLocation]('" + strLocationCode + "','1')) or ISNULL(A.SLCODE,'') = '" + strLocationCode + "')");
            }
            if (!String.IsNullOrEmpty(strIsHadImage))
            {
                sbSql.Append(" and ((case when A.BISHADIMAGE = '1' then '1' else '2' end) = '" + strIsHadImage + "')");
            }
            sbSql.Append(" order by SACODE,SLCODE");

            DataTable dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
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
            log.Error(strMethodDesc + "Return Json:" + sbResult.ToString());
        }
        return sbResult.ToString();
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}