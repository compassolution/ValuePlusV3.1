<%@ WebHandler Language="C#" Class="PRItemIndex" %>

using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections;
using Com.ValuePlus.Web;
using Com.ValuePlus.DAL;

public class PRItemIndex : HandlerBase, IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    public void ProcessRequest (HttpContext context) {
        Hashtable hsTableUrlQuery = this.GetUrlAnalyse(context);
        string strParam = hsTableUrlQuery["param"] == null ? string.Empty : hsTableUrlQuery["param"].ToString();//param
        String strItemCode = hsTableUrlQuery["itemcode"] == null ? string.Empty : hsTableUrlQuery["itemcode"].ToString();
        String strItemType = hsTableUrlQuery["itemtype"] == null ? string.Empty : hsTableUrlQuery["itemtype"].ToString();
        String strIsStop = hsTableUrlQuery["isstop"] == null ? string.Empty : hsTableUrlQuery["isstop"].ToString();
        String strIsShowPrint = hsTableUrlQuery["isshowprint"] == null ? string.Empty : hsTableUrlQuery["isshowprint"].ToString();
        String strCtrlId = hsTableUrlQuery["ctrlid"] == null ? string.Empty : hsTableUrlQuery["ctrlid"].ToString();
        String strCtrlValue = hsTableUrlQuery["ctrlvalue"] == null ? string.Empty : hsTableUrlQuery["ctrlvalue"].ToString();
        String strCondition = hsTableUrlQuery["condition"] == null ? string.Empty : hsTableUrlQuery["condition"].ToString();

        if (strParam.Equals("querylist"))
        {
            string json = this.QueryDataList(context,strItemCode,strItemType,strIsShowPrint,strIsStop,strCondition);
            context.Response.Write(json);
        }else if (strParam.Equals("querynot030list"))
        {
            strCondition = " ITEMECAL NOT IN ('030')";
            string json = this.QueryDataList(context,strItemCode,strItemType,strIsShowPrint,strIsStop,strCondition);
            context.Response.Write(json);
        }else if (strParam.Equals("deleteonitem"))
        {
            this.DeleteOneItem(context, strItemCode);
        }else if (strParam.Equals("savedataonectrl"))
        {
            this.SaveDataOneCtrl(context,strItemCode,strCtrlId,strCtrlValue);
        }else if (strParam.Equals("toedititempage"))
        {
            this.ToEditItemPage(context,strItemCode);
        }else if (strParam.Equals("querymyrefitemlist"))
        {
            string json = this.QueryMyRefItemList(context,strItemCode);
            context.Response.Write(json);
        }else if (strParam.Equals("exportdatasql_pritem"))
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(this.ExportDataSql(context,"PRITEM_1",strItemCode));
            sBuilder.Append(this.ExportDataSql(context,"PRITEM_2",strItemCode));
            sBuilder.Append(this.ExportDataSql(context,"PRITEM_3",strItemCode));
            sBuilder.Append(this.ExportDataSql(context,"PRITEM_4",strItemCode));
            sBuilder.Append("\r\nGO\r\n");
            context.Response.Write("{ \"ResultData\":\""+Microsoft.JScript.GlobalObject.escape(sBuilder.ToString())+"\" }");
        }else if (strParam.Equals("exportdatasql_otherpayitem"))
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append(this.ExportDataSql(context,"OTHERPAYITEM_1",strItemCode));
            sBuilder.Append(this.ExportDataSql(context,"OTHERPAYITEM_2",strItemCode));
            sBuilder.Append("\r\nGO\r\n");
            context.Response.Write("{ \"ResultData\":\""+Microsoft.JScript.GlobalObject.escape(sBuilder.ToString())+"\" }");
        }
    }

    /// <summary>
    /// 查询列表数据
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private String QueryDataList(HttpContext context, String strItemCode,String strItemType,String strIsShowPrint,String strIsStop,String strCondition)
    {
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select * ");
            sbSql.Append(" ,(select CDESCCHS from tb_hrlstd where lid = 'VERIFYSTATUS' and CID = A.ITEMVERIFY) AS ITEMVERIFY_DESC");
            sbSql.Append(" ,(select CDESCCHS from tb_hrlstd where lid = 'PRICAL' and CID = A.ITEMECAL) AS ITEMECAL_DESC");
            sbSql.Append(" from PRITEM_1 A WHERE 1=1 ");
            if(!String.IsNullOrEmpty(strItemCode))
            {
                sbSql.Append(" AND ITEMCODE = '" + strItemCode+"'" );
            }
            if(!String.IsNullOrEmpty(strItemType))
            {
                sbSql.Append(" AND ITEMECAL = '" + strItemType+"'" );
            }
            if(!String.IsNullOrEmpty(strIsShowPrint))
            {
                sbSql.Append(" AND ITEMPRINT = '" + strIsShowPrint+"'" );
            }
            if(!String.IsNullOrEmpty(strIsStop))
            {
                sbSql.Append(" AND BISSTOP = '" + strIsStop+"'" );
            }
            if(!String.IsNullOrEmpty(strCondition))
            {
                sbSql.Append(" AND "+strCondition );
            }
            sbSql.Append(" order by ITEMPORDER,ITEMCORDER ");
            String strSql = sbSql.ToString();
            log.Error("PRItemIndex.ashx查询列表数据:"+strSql);

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
                        sbSql.Append("UPDATE PRITEM_1 SET "+strColumnName+" = '"+strColumnValue+"' WHERE ITEMCODE = '" + strItemCode + "';");

                        //switch (strColumnName){
                        //    case "ITEMEPPL":
                        //        //保存后的操作
                        //        this.DealAfterSave(strItemCode, "AfterEdit");
                        //        break;

                        //}
                    }
                }

            }
            String strSql = sbSql.ToString();
            log.Error("即时保存单独每个字段控件的数据:"+strSql);
            if (!String.IsNullOrEmpty(strSql))
            {
                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;

                //保存后的操作
                this.DealAfterSave(strItemCode, "AfterEdit");
            }

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"savedCount\":\""+iReturn.ToString()+"\"");
            sBuilder.Append(",");
            sBuilder.Append("\"savedReturnData\":"+this.QueryDataList(context,strItemCode,"","","",""));
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
    /// 返回跳转到编辑页面的链接
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private void ToEditItemPage(HttpContext context, String strItemCode)
    {
        try
        {
            String strParamString = "TID=PRITEM&RID=MAIN&SID=MAIN&KEY=ITEMCODE&KEYVALUE=" + strItemCode + "&OPTYPE=" + "edit";

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
            log.Error("PRItemIndex.ashx.QueryMyRefItemList查询与我关联的薪资项目:"+strSql_MySub);
            DataTable dt_MySub = SqlParamDao.GetDataTableBySql(strSql_MySub);

            //公式涉及到我的项目
            StringBuilder sbSql_RefMe = new StringBuilder();
            sbSql_RefMe.Append("select * from PRITEM_1 A WHERE ITEMCODE IN (SELECT ITEMCODE from PRITEM_4 where RefItemCode = '"+strItemCode+"') ");
            sbSql_RefMe.Append(" order by ITEMPORDER,ITEMCORDER ");
            String strSql_RefMe = sbSql_RefMe.ToString();
            log.Error("PRItemIndex.ashx.QueryMyRefItemList查询与我关联的薪资项目:"+strSql_RefMe);
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

    /// <summary>
    /// 操作保存后的后续操作
    /// </summary>
    /// <param name="strItemCode"></param>
    /// <param name="strAID"></param>
    private void DealAfterSave(String strItemCode,String strAID){
        try
        {
            String strSQL_ExecuteSP = "USP_HR_PRITEM_AfterOperation";
            String NowDate = DateTime.Now.ToString("yyyy-MM-dd");
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("ITEMCODE", strItemCode);
            hsTableParam.Add("AID", strAID);
            hsTableParam.Add("USERCODE",this.GetUserCode());
            int iiReturnValue = SqlParamDao.ExcuteSP(strSQL_ExecuteSP, hsTableParam);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    /// <summary>
    /// 删除薪资项目记录
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private void DeleteOneItem(HttpContext context, String strItemCode)
    {
        int iReturn = -1;
        String strReturnMsg = "删除薪资项目"+strItemCode+"失败！";
        StringBuilder sBuilder = new StringBuilder();
        try
        {
            StringBuilder sbSql = new StringBuilder();
            //首先判断该项目是否有被其他项目使用到，如果有则不予删除
            sbSql.Append("select count(1) from PRITEM_4 where RefItemCode = '"+strItemCode+"'");
            int iCount_IsExists = SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
            sbSql = new StringBuilder();
            sbSql.Append("select count(1) from OTHERPAYITEM_1 where SUMTO = '"+strItemCode+"'");
            iCount_IsExists = iCount_IsExists + SqlParamDao.ExecuteScalarBySql(sbSql.ToString());
            if (iCount_IsExists > 0)
            {
                iReturn = -19;
                strReturnMsg = "删除薪资项目"+strItemCode+"失败，该项目被其他薪资项目或者补充薪资项目所引用!";
            }else{
                sbSql.Append("DELETE FROM PRITEM_1 WHERE ITEMCODE = '"+strItemCode+"'; ");
                sbSql.Append("DELETE FROM PRITEM_2 WHERE ITEMCODE = '"+strItemCode+"'; ");
                sbSql.Append("DELETE FROM PRITEM_3 WHERE ITEMCODE = '"+strItemCode+"'; ");
                sbSql.Append("DELETE FROM PRITEM_4 WHERE ITEMCODE = '"+strItemCode+"'; ");
                sbSql.Append("exec [USP_HR_PRITEM_AfterOperation] '"+strItemCode+"','AfterDelete','"+this.GetUserCode()+"'; ");
                String strSql = sbSql.ToString();
                log.Error("OtherPayItemList删除某一条补充薪资项目"+strItemCode+":"+strSql);

                int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                iReturn = iCount;
                if(iCount>0){
                    strReturnMsg = "删除薪资项目"+strItemCode+"成功！";
                }else{
                    strReturnMsg = "删除薪资项目"+strItemCode+"失败！";
                }
            }
        }
        catch (Exception ex)
        {
            strReturnMsg = "删除薪资项目"+strItemCode+"失败："+ex.Message.ToString();
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
    /// 薪资项目的SQL数据脚本
    /// </summary>
    /// <param name="context"></param>
    /// <param name="strItemCode"></param>
    private String ExportDataSql(HttpContext context,String strTableName, String strItemCode)
    {
        try
        {
            StringBuilder sbSqlResult = new StringBuilder();

            Hashtable hsTableHadDeleteItem = new Hashtable();
            StringBuilder sbSql_Query = new StringBuilder();
            sbSql_Query.Append("SELECT * FROM "+strTableName+" WHERE 1=1");
            if(!String.IsNullOrEmpty(strItemCode)){
                sbSql_Query.Append(" and ITEMCODE = '"+strItemCode+"'");
            }
            DataTable dt_Query = SqlParamDao.GetDataTableBySql(sbSql_Query.ToString());
            if(dt_Query!=null&&dt_Query.Rows.Count>0){
                for(int i=0;i<dt_Query.Rows.Count;i++){
                    DataRow dr = dt_Query.Rows[i];
                    String strRowItemCode = dr["ITEMCODE"].ToString();

                    StringBuilder sbColumnName = new StringBuilder();
                    StringBuilder sbColumnValue = new StringBuilder();

                    for(int j=0;j<dt_Query.Columns.Count;j++){
                        DataColumn dc = dt_Query.Columns[j];
                        String strDataType = dc.DataType.ToString();
                        String strColumnName = "[" + dc.ToString() + "]";
                        String strColumnValue = dr[dc].ToString().Replace("'", "''");

                        //如果为空时根据不同数据类型的处理
                        if(String.IsNullOrEmpty(strColumnValue)){
                            switch (strDataType.ToString())
                            {
                                case "System.Int16"://整型      
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    strColumnValue = "0";
                                    break;
                                case "System.Double"://浮点型
                                case "System.Decimal"://高精度数值型 
                                    strColumnValue = "0.00";
                                    break;
                                default:
                                    strColumnValue = "";
                                    break;
                            }
                        }

                        if(j==0){
                            sbColumnName.Append(strColumnName);
                            sbColumnValue.Append("'"+strColumnValue+"'");
                        }else{
                            sbColumnName.Append(","+strColumnName+"");
                            sbColumnValue.Append(",'"+strColumnValue+"'");
                        }
                    }

                    //删除语句的条数
                    if(!hsTableHadDeleteItem.ContainsKey(strRowItemCode)){
                        sbSqlResult.Append("delete from " + strTableName + " where ITEMCODE = '" + strRowItemCode + "';\r\n");
                        hsTableHadDeleteItem.Add(strRowItemCode, "");
                    }


                    sbSqlResult.Append("INSERT INTO "+strTableName+"("+sbColumnName.ToString()+")VALUES("+sbColumnValue.ToString()+");\r\n");
                    if(strTableName.ToUpper().Equals("PRITEM_1")){
                        sbSqlResult.Append("exec [USP_HR_PRITEM_AfterOperation] '"+strItemCode+"','AfterAdd','"+this.GetUserCode()+"';\r\n");
                    }
                }
                sbSqlResult.Append("GO\r\n");

            }

            return sbSqlResult.ToString();

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return "";
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}