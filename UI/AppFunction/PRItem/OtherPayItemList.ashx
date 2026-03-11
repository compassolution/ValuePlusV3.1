<%@ WebHandler Language="C#" Class="OtherPayItemList" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class OtherPayItemList : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strItemCode = hsTableUrlQuery["itemcode"] == null ? string.Empty : hsTableUrlQuery["itemcode"].ToString();
        String strIsMonthFix = hsTableUrlQuery["ismonthfix"] == null ? string.Empty : hsTableUrlQuery["ismonthfix"].ToString();
        String strIsStop = hsTableUrlQuery["isstop"] == null ? string.Empty : hsTableUrlQuery["isstop"].ToString();
        String strIsReduce = hsTableUrlQuery["isreduce"] == null ? string.Empty : hsTableUrlQuery["isreduce"].ToString();
        String strCtrlId = hsTableUrlQuery["ctrlid"] == null ? string.Empty : hsTableUrlQuery["ctrlid"].ToString();
        String strCtrlValue = hsTableUrlQuery["ctrlvalue"] == null ? string.Empty : hsTableUrlQuery["ctrlvalue"].ToString();
        String strIsAdd = hsTableUrlQuery["isadd"] == null ? string.Empty : hsTableUrlQuery["isadd"].ToString();

        if (strParam.Equals("querylist"))
        {
            string json = this.QueryDataList(context,strItemCode,strIsMonthFix,strIsReduce,strIsStop);
            context.Response.Write(json);
        }
        if (strParam.Equals("deleteonitem"))
        {
            this.DeleteOneItem(context, strItemCode);
        }
        if (strParam.Equals("savedataonectrl"))
        {
            this.SaveDataOneCtrl(context,strItemCode,strCtrlId,strCtrlValue);
        }
        if (strParam.Equals("saveoneiteminfo"))
        {
            this.SaveOneItemInfo(context,strIsAdd);
        }
        if (strParam.Equals("toedititempage"))
        {
            this.ToEditItemPage(context,strItemCode);
        }
        if (strParam.Equals("querymyrefitemlist"))
        {
            string json = this.QueryMyRefItemList(context,strItemCode);
            context.Response.Write(json);
        }
    }

    /// <summary>
    /// 查询列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private String QueryDataList(HttpContext context, String strItemCode,String strIsMonthFix,String strIsReduce,String strIsStop)
    {
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * ");
            sbSql.Append(" ,(select ITEMNAMECHS from PRITEM_1 where ITEMCODE = A.SUMTO) AS SUMTO_DESC");
            sbSql.Append(" from OTHERPAYITEM_1 A WHERE 1=1 ");
            if(!String.IsNullOrEmpty(strItemCode))
            {
                sbSql.Append(" AND ITEMCODE = '" + strItemCode+"'" );
            }
            if(!String.IsNullOrEmpty(strIsMonthFix))
            {
                sbSql.Append(" AND ISMONTHFIX = '" + strIsMonthFix+"'" );
            }
            if(!String.IsNullOrEmpty(strIsReduce))
            {
                sbSql.Append(" AND ISREDUCE = '" + strIsReduce+"'" );
            }
            if(!String.IsNullOrEmpty(strIsStop))
            {
                sbSql.Append(" AND BISSTOP = '" + strIsStop+"'" );
            }
            sbSql.Append(" order by SORDER,ITEMCODE ");
            String strSql = sbSql.ToString();
            log.Error("OtherPayItemList.ashx查询列表数据:"+strSql);

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            int iRowsCount = 0;
            if (dt != null)
            {
                iRowsCount = dt.Rows.Count;
            }

            sBuilder.Append("{");
            sBuilder.Append("\"totalCount\":\""+iRowsCount.ToString()+"\"");
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt,"\"ResultData\"",true));
            sBuilder.Append("}");

            return sBuilder.ToString();

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "{\"totalCount\":\"-1\"}";
        }
    }

    /// <summary>
    /// 即时保存单独每个字段控件的数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    /// <param name="strCtrlId">控件ID支持多个ID，中间用*</param>
    /// <param name="strCtrlValue">控件值支持多个，中间用*，数量需跟CtrlId匹配</param>
    private void SaveDataOneCtrl(HttpContext context, String strItemCode,String strCtrlId,String strCtrlValue)
    {
        int iReturn = -1;
        try
        {
            //从context.Request.Form或者PRItemTree.html中的公式内容，因为从url中获取会因为公式中特殊字符特别是加号+引起错误数据
            if(context.Request.Form["txt_CurItemCode"]!=null&&context.Request.Form["txt_ItemEPPL"]!=null){
                strItemCode = context.Request.Form["txt_CurItemCode"].ToString();
                strCtrlId = "txt#Item#" + strItemCode + "#ITEMEPPL";
                strCtrlValue = context.Request.Form["txt_ItemEPPL"].ToString();
            }


            StringBuilder sbSql = new StringBuilder();
            if (!String.IsNullOrEmpty(strCtrlId))
            {
                String[] CtralIdArrary = strCtrlId.Split('*');
                String[] CtralValueArrary = strCtrlValue.Split('*');
                if (CtralIdArrary.Length > 0 && CtralIdArrary.Length == CtralValueArrary.Length)
                {
                    for(int i = 0; i < CtralIdArrary.Length; i++)
                    {
                        string[] strSelectCtrlId = CtralIdArrary[i].Split('#');
                        string[] strSelectCtrlValue = CtralValueArrary[i].Split('#');
                        String strOrderNum = strSelectCtrlId[2].ToString();
                        String strColumnName = strSelectCtrlId[3].ToString();

                        String strColumnValue = CtralValueArrary[i].ToString();
                        strColumnValue = strColumnValue.Replace("'", "''");
                        sbSql.Append("UPDATE OTHERPAYITEM_1 SET "+strColumnName+" = '"+strColumnValue+"' WHERE ITEMCODE = '" + strItemCode + "';");
                        //额外操作--更新汇总到的薪资项目名称
                        sbSql.Append("update A SET A.SUMTONAME = B.SHOWNAMECHS FROM OTHERPAYITEM_1 A INNER JOIN PRITEM_1 B ON A.SUMTO = B.ITEMCODE");
                        //sbSql.Append(" WHERE A.ITEMCODE = '" + strItemCode + "';");
                    }
                }

            }
            String strSql = sbSql.ToString();
            log.Error("即时保存单独每个字段控件的数据:"+strSql);
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"savedCount\":\""+iReturn.ToString()+"\"");
            sBuilder.Append(",");
            sBuilder.Append("\"savedReturnData\":"+this.QueryDataList(context,strItemCode,"","",""));
            sBuilder.Append("}");

            context.Response.Write(sBuilder.ToString());
        }
        catch (Exception ex)
        {
            context.Response.Write(iReturn);
            log.Error(ex);
        }
    }

    /// <summary>
    /// 保存整一条补充薪资项目数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strIsAdd"></param>
    private void SaveOneItemInfo(HttpContext context, String strIsAdd)
    {
        int iReturn = -1;
        String strReturnMsg = "保存补充薪资项目设置数据失败！";
        StringBuilder sBuilder = new StringBuilder();
        try
        {

            String strOtherItemCode = context.Request.Form["txt_PayItemCode"]!=null?context.Request.Form["txt_PayItemCode"].ToString():"";
            String strOtherItemName = context.Request.Form["txt_PayItemName"]!=null?context.Request.Form["txt_PayItemName"].ToString():"";
            String strOtherItemNamChs = context.Request.Form["txt_PayItemNameChs"]!=null?context.Request.Form["txt_PayItemNameChs"].ToString():"";
            String selSumToItemCode = context.Request.Form["sel_PRItem"]!=null?context.Request.Form["sel_PRItem"].ToString():"";
            String strIsReduce = context.Request.Form["sel_IsReduce"]!=null?context.Request.Form["sel_IsReduce"].ToString():"";
            String strIsMonthFix = context.Request.Form["sel_IsMonthFix"]!=null?context.Request.Form["sel_IsMonthFix"].ToString():"";
            String strPayItemOrder = context.Request.Form["txt_PayItemOrder"]!=null?context.Request.Form["txt_PayItemOrder"].ToString():"";
            String strBIsStop = context.Request.Form["sel_BIsStop"]!=null?context.Request.Form["sel_BIsStop"].ToString():"";
            String strCBIsAddPRItem = context.Request.Form["ckBox_IsAddPRItem"]!=null?context.Request.Form["ckBox_IsAddPRItem"].ToString():"";//on
            String strAddPRItemCode = context.Request.Form["txt_AddPRItemCode"]!=null?context.Request.Form["txt_AddPRItemCode"].ToString():"";

            StringBuilder sbSql = new StringBuilder();
            if(strIsAdd.Equals("1")){
                //新增时
                //首先判断该ItemCode是否存在
                sbSql.Append("select count(1) from OTHERPAYITEM_1 WHERE ITEMCODE = '" + strOtherItemCode + "'");
                int iCount_IsExists = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
                if(iCount_IsExists>0){
                    iReturn = -19;
                    strReturnMsg = "失败，新增时，补充薪资项目编码'"+strOtherItemCode+"'已经存在!";
                }else{
                    sbSql = new StringBuilder();
                    sbSql.Append("INSERT INTO [OTHERPAYITEM_1]([ITEMCODE],[ITEMNAME],[ITEMNAMECHS],[SUMTO],[SUMTONAME],[ISREDUCE],[ISMONTHFIX],[BISSTOP],[SORDER])values");
                    sbSql.Append("(");
                    sbSql.Append("'"+strOtherItemCode+"' ");
                    sbSql.Append(",'"+strOtherItemName+"' ");
                    sbSql.Append(",'"+strOtherItemNamChs+"' ");
                    sbSql.Append(",'"+(strCBIsAddPRItem.ToLower().Equals("on")?strAddPRItemCode:selSumToItemCode)+"' ");
                    sbSql.Append(",NULL ");
                    sbSql.Append(",'"+strIsReduce+"' ");
                    sbSql.Append(",'"+strIsMonthFix+"' ");
                    sbSql.Append(",'"+strBIsStop+"' ");
                    sbSql.Append(",'"+strPayItemOrder+"' ");
                    sbSql.Append(");");
                    iReturn = 0;
                }

            }else{
                //更新时
                sbSql = new StringBuilder();
                sbSql.Append("UPDATE OTHERPAYITEM_1 SET ITEMNAME = '"+strOtherItemName+"' ");
                sbSql.Append(" ,ITEMNAMECHS = '"+strOtherItemNamChs+"' ");
                sbSql.Append(" ,SUMTO = '"+selSumToItemCode+"' ");
                sbSql.Append(" ,ISREDUCE = '"+strIsReduce+"' ");
                sbSql.Append(" ,ISMONTHFIX = '"+strIsMonthFix+"' ");
                sbSql.Append(" ,BISSTOP = '"+strBIsStop+"' ");
                sbSql.Append(" ,SORDER = '"+strPayItemOrder+"' ");
                sbSql.Append(" WHERE ITEMCODE = '" + strOtherItemCode + "';");
                iReturn = 0;

            }
            if(iReturn>=0){
                //额外操作--更新汇总到的薪资项目名称
                sbSql.Append("update A SET A.SUMTONAME = B.SHOWNAMECHS FROM OTHERPAYITEM_1 A INNER JOIN PRITEM_1 B ON A.SUMTO = B.ITEMCODE WHERE A.ITEMCODE = '" + strOtherItemCode + "';");
                String strSql = sbSql.ToString();
                log.Error("保存整一条补充薪资项目数据SaveOneItemInfo:" + strSql);
                if (!String.IsNullOrEmpty(strSql))
                {
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                    if(iCount>0){
                        iReturn = iCount;
                        strReturnMsg = "保存补充薪资项目设置数据成功!";

                        //如果新增时需要同时新增对应的薪资项目，则判断此编码是否存在
                        if (strIsAdd.Equals("1")&&strCBIsAddPRItem.ToString().ToLower().Equals("on")){
                            sbSql = new StringBuilder();
                            sbSql.Append("select count(1) from PRITEM_1 WHERE ITEMCODE = '" + strAddPRItemCode + "'");
                            int iCount_IsExists = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
                            if (iCount_IsExists > 0)
                            {
                                iReturn = 1;
                                strReturnMsg = "保存补充薪资项目设置数据成功，但是同时新增薪资项目时失败，因为薪资项目编码'" + strAddPRItemCode + "'已经存在!";
                            }else{
                                //同时新增保存薪资项目
                                sbSql = new StringBuilder();
                                sbSql.Append("INSERT INTO [PRITEM_1]([ITEMCODE],[ITEMNAME],[ITEMNAMECHS],[SHOWNAME],[SHOWNAMECHS],[ITEMDESC],[ITEMPRINTALL],[ITEMPRINT],[ITEMPORDER],[ITEMECAL],[ITEMEVALUE],[ITEMFROMTABLE],[ITEMFROMFIELD],[FIELDKEY],[FIELDKEYVALUE]");
                                sbSql.Append(" ,[ITEMEPPL],[EPPLDESC],[ITEMCORDER],[TAXMODE],[ITEMVERIFY],[ITEMEDIT],[ITEMCLEAR],[ITEMDECI],[ITEMROUND],[ITEMFOCUS],[ITEMDEBIT],[ITEMCREDIT],[ITEMDUSE],[ITEMCUSE],[ITEMTOGL],[BISSTOP])values");
                                sbSql.Append("(");
                                sbSql.Append("'"+strAddPRItemCode+"' ");
                                sbSql.Append(",'"+strOtherItemName+"' ");
                                sbSql.Append(",'"+strOtherItemNamChs+"' ");
                                sbSql.Append(",'"+strOtherItemName+"' ");
                                sbSql.Append(",'"+strOtherItemNamChs+"' ");
                                sbSql.Append(",'新增补充薪资项目时自动新增'");
                                sbSql.Append(",'2' ,'2' ,'1000' ,'020' ,'0.00',null,null,null,null");
                                sbSql.Append(",null,null,1000,'001','020','1','1','4','1','1','','','2','2','2','2'");
                                sbSql.Append(");");
                                strSql = sbSql.ToString();
                                log.Error("SaveOneItemInfo同时新增保存薪资项目:" + strSql);
                                iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                                if (iCount > 0)
                                {
                                    iReturn = iCount;
                                    strReturnMsg = "保存补充薪资项目设置数据及新增薪资项目成功";
                                }else{
                                    iReturn = iCount;
                                    strReturnMsg = "保存补充薪资项目设置数据成功，但是同时新增薪资项目时失败!";
                                }
                            }
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            strReturnMsg = "保存补充薪资项目设置数据失败："+ex.Message.ToString();
            log.Error(ex);
        }finally{
            sBuilder.Append("{");
            sBuilder.Append("\"returnCode\":\"" + iReturn.ToString() + "\"");
            sBuilder.Append(",");
            sBuilder.Append("\"returnMsg\":\"" + strReturnMsg + "\"");
            sBuilder.Append("}");
            context.Response.Write(sBuilder.ToString());
        }
    }

    /// <summary>
    /// 删除某一条补充薪资项目记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private void DeleteOneItem(HttpContext context, String strItemCode)
    {
        int iReturn = -1;
        String strReturnMsg = "删除补充薪资项目"+strItemCode+"记录失败！";
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("DELETE FROM OTHERPAYITEM_1 WHERE ITEMCODE = '"+strItemCode+"'; ");
            sbSql.Append("DELETE FROM OTHERPAYITEM_2 WHERE ITEMCODE = '"+strItemCode+"'; ");
            String strSql = sbSql.ToString();
            log.Error("OtherPayItemList删除补充薪资项目"+strItemCode+"记录:"+strSql);

            int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
            iReturn = iCount;
            if(iCount>0){
                strReturnMsg = "删除补充薪资项目记录"+strItemCode+"成功！";
            }else{
                strReturnMsg = "删除补充薪资项目记录"+strItemCode+"失败！";
            }

        }
        catch (Exception ex)
        {
            strReturnMsg = "删除补充薪资项目记录"+strItemCode+"失败："+ex.Message.ToString();
            log.Error(ex);
        }finally{
            sBuilder.Append("{");
            sBuilder.Append("\"returnCode\":\"" + iReturn.ToString() + "\"");
            sBuilder.Append(",");
            sBuilder.Append("\"returnMsg\":\"" + strReturnMsg + "\"");
            sBuilder.Append("}");
            context.Response.Write(sBuilder.ToString());
        }
    }

    /// <summary>
    /// 返回跳转到编辑页面的链接
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private void ToEditItemPage(HttpContext context, String strItemCode)
    {
        try
        {
            String strParamString = "TID=OTHERPAYITEM&RID=Manager&SID=Manage&KEY=ITEMCODE&KEYVALUE=" + strItemCode + "&OPTYPE=" + "edit";

            String strUrl = "../../Archive/Detail/EditArchiveDetail.aspx?" + Com.ValuePlus.Common.UrlParamEncryption.EncryptionUrlParam(strParamString);

            context.Response.Write(strUrl);
        }
        catch (Exception ex)
        {
            context.Response.Write("");
            log.Error(ex);
        }
    }

    /// <summary>
    /// 与我关联的薪资项目
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private String QueryMyRefItemList(HttpContext context, String strItemCode)
    {
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            //我的公式涉及到项目
            StringBuilder sbSql_MySub = new StringBuilder();
            sbSql_MySub.Append("select * from PRITEM_1 A WHERE ITEMCODE IN (SELECT RefItemCode from PRITEM_4 where ITEMCODE = '"+strItemCode+"') ");
            sbSql_MySub.Append(" order by ITEMPORDER,ITEMCORDER ");
            String strSql_MySub = sbSql_MySub.ToString();
            log.Error("OtherPayItemList.ashx.QueryMyRefItemList查询与我关联的薪资项目:"+strSql_MySub);
            DataTable dt_MySub = SqlParamDao.GetDataTableBySql(strSql_MySub);

            //公式涉及到我的项目
            StringBuilder sbSql_RefMe = new StringBuilder();
            sbSql_RefMe.Append("select * from PRITEM_1 A WHERE ITEMCODE IN (SELECT ITEMCODE from PRITEM_4 where RefItemCode = '"+strItemCode+"') ");
            sbSql_RefMe.Append(" order by ITEMPORDER,ITEMCORDER ");
            String strSql_RefMe = sbSql_RefMe.ToString();
            log.Error("OtherPayItemList.ashx.QueryMyRefItemList查询与我关联的薪资项目:"+strSql_RefMe);
            DataTable dt_RefMe = SqlParamDao.GetDataTableBySql(strSql_RefMe);

            sBuilder.Append("{");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_MySub,"\"ResultData_MySub\"",true));
            sBuilder.Append(",");
            sBuilder.Append(WebCommon.GetJsonStringByDataTable(dt_RefMe,"\"ResultData_RefMe\"",true));
            sBuilder.Append("}");

            return sBuilder.ToString();

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "{\"totalCount\":\"-1\"}";
        }
    }


    public bool IsReusable {
        get {
            return false;
        }
    }

}