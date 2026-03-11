using System;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.Property;

namespace Com.ValuePlus.Archive.BLL
{
    public class TmpdPMastBll 
    {

        #region 获取受控制的其他控件列表
        /// <summary>
        /// 获取受控制的其他控件列表
        /// PMAST字段配置可以包括
        /// 1、纯字段名称；(pid)
        /// 2、分组名+;+字段名称（gid;pid）
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strValue"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public ArrayList GetBeMastControlList(String strCtrlId, String strValue,String strLanguage)
        {
            ArrayList arrListDataTable = new ArrayList();
            ArrayList arrListCtrlId = new ArrayList();

            String strTID = ServerCtrlIDGetterBll.GetTIDByCtrlId(strCtrlId);
            String strGID = ServerCtrlIDGetterBll.GetGIDByCtrlId(strCtrlId);
            String strPID = ServerCtrlIDGetterBll.GetPIDByCtrlId(strCtrlId);
            string strSql = "select * from TB_HRTMPD WHERE TID = '" + strTID + "' AND (PMAST = '" + strPID + "' or PMAST = '" + strGID + ";" + strPID + "')";
            //ADD BY SAMMEN 20130125 只获取“主信息”和“常规信息”类型的组 
            //delete by sammen 20241106
            //strSql = strSql + " AND GID IN (SELECT GID FROM TB_HRTMPG WHERE TID  = '" + strTID + "' AND GTYPE IN ('0','1'))";

            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    String strTempGID = dr["GID"].ToString();
                    String strTempPID = dr["PID"].ToString();
                    int iCtrlType = int.Parse(dr["PCTRL"].ToString());
                    String strPMAST = dr["PMAST"].ToString();

                    if (strPMAST.IndexOf(';') >= 0)
                    {
                        string[] strArray = strPMAST.Split(new char[] { ';' });
                        if (!strArray[0].Equals(strGID))
                        {
                            break;
                        }
                    }
                    switch (iCtrlType)
                    {
                        case 0://文本框
                        case 3://文本框(多行)
                        case 6://文本框(宽行）
                        case 9://文本框(密码)
                            String strCtrlIDString0 = ServerCtrlIDGetterBll.GetCtrlID_TextBox(strTID, strTempGID, strTempPID);
                            arrListCtrlId.Add(strCtrlIDString0);
                            break;
                        case 1://List列表
                            String strCtrlIDString1 = ServerCtrlIDGetterBll.GetCtrlID_DropDownList(strTID, strTempGID, strTempPID);
                            arrListCtrlId.Add(strCtrlIDString1);
                            break;
                        case 2://DB List数据列表
                            String strCtrlIDString2 = ServerCtrlIDGetterBll.GetCtrlID_DBTextBox(strTID, strTempGID, strTempPID);
                            arrListCtrlId.Add(strCtrlIDString2);
                            break;
                    }
                }
            }
            if (arrListCtrlId != null && arrListCtrlId.Count > 0)
            {
                arrListDataTable = this.GetControlDataSet(arrListCtrlId, strValue, strLanguage);
            }
            return arrListDataTable;
        }
        #endregion

        #region 根据控件ID填充其记录值
        /// <summary>
        /// 根据控件ID填充其记录值
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strMastValue"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        private ArrayList GetControlDataSet(ArrayList arrListCtrlId, String strMastValue,String strLanguage)
        {
            ArrayList arrListDataTable = new ArrayList();
            if ((arrListCtrlId != null) && (arrListCtrlId.Count > 0))
            {
                for (int i = 0; i < arrListCtrlId.Count; i++)
                {
                    String strCtrlId = arrListCtrlId[i].ToString();
                    String strTID = ServerCtrlIDGetterBll.GetTIDByCtrlId(strCtrlId);
                    String strGID = ServerCtrlIDGetterBll.GetGIDByCtrlId(strCtrlId);
                    String strPID = ServerCtrlIDGetterBll.GetPIDByCtrlId(strCtrlId);
                    String strGetPCTRLID = "select PCTRL,PCTRLID from TB_HRTMPD where TID='" + strTID + "' AND GID = '" + strGID + "' AND PID ='" + strPID + "'";
                    DataTable dtTemp = SqlParamDao.GetDataTableBySql(strGetPCTRLID);
                    String strCtrlType = "";
                    String strPCTRLID = "";
                    if ((dtTemp != null) && (dtTemp.Rows.Count > 0))
                    {
                        strCtrlType = dtTemp.Rows[0][0].ToString();
                        strPCTRLID = dtTemp.Rows[0][1].ToString();
                    }
                    String strCDESCNAME = "CDESC";
                    if (strLanguage.Equals("zh-cn"))
                    {
                        strCDESCNAME = "CDESCCHS";
                    }

                    TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(strPCTRLID);
                    String strSql = "SELECT LID,CID," + strCDESCNAME + " as CDESC,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "' and CUID ='" + strMastValue + "' ORDER BY CID";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    Entity_AjaxPMastObject entityAjaxPMastObject = new Entity_AjaxPMastObject();
                    entityAjaxPMastObject.CtrlId = strCtrlId;
                    entityAjaxPMastObject.CtrlType = strCtrlType;
                    entityAjaxPMastObject.MastValue = strMastValue;
                    entityAjaxPMastObject.DtResult = dt;
                    entityAjaxPMastObject.TID = strTID;
                    entityAjaxPMastObject.GID = strGID;

                    arrListDataTable.Add(entityAjaxPMastObject);
                }
            }
            return arrListDataTable;
        }
        #endregion

        #region 通过字段获取其主控字段的值
        /// <summary>
        /// 通过字段获取其主控字段的值
        /// </summary>
        /// <param name="strCtrlId"></param>
        /// <param name="strMast"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public String GetMastValueByCtrlId(String strCtrlId, String strPMAST, String strKey, String strKeyValue,String strGridKey,String strGridKeyValue)
        {
            String strReturnValue = "";
            if ((!String.IsNullOrEmpty(strCtrlId)) && (!String.IsNullOrEmpty(strPMAST)) && (!String.IsNullOrEmpty(strKey)))
            {
                String strTID = ServerCtrlIDGetterBll.GetTIDByCtrlId(strCtrlId);
                String strGID = ServerCtrlIDGetterBll.GetGIDByCtrlId(strCtrlId);
                String strPID = ServerCtrlIDGetterBll.GetPIDByCtrlId(strCtrlId);

                String strGIDTemp = "";
                String strPIDTemp = "";
                if (strPMAST.IndexOf(';') >= 0)
                {
                    string[] strArray = strPMAST.Split(new char[] { ';' });
                    if (strArray.Length==2)
                    {
                        strGIDTemp = strArray[0].ToString();
                        strPIDTemp = strArray[1].ToString();
                    }
                }
                else
                {
                    strGIDTemp = strGID;
                    strPIDTemp = strPMAST;
                }
                String strGroupTable = ServerCtrlIDGetterBll.GetArchiveTableNameByGroup(strTID,strGIDTemp);

                string strSql = "select " + strPIDTemp + " from " + strGroupTable + " WHERE  " + strKey + "= '" + strKeyValue + "'";

                //如果Mast字段来源于列表分组，则需要定位到列表分组的某条记录值
                DataTable dt_IsMainGroup = SqlParamDao.GetDataTableBySql("SELECT GID FROM TB_HRTMPG WHERE TID='" + strTID + "' AND GTYPE=0");
                String strMainGID = dt_IsMainGroup.Rows[0]["GID"].ToString();//主分组编码
                //////////////////不是主分组且GridKey和GridKeyValue都有值
                //if ((!strMainGID.Equals(strGIDTemp)) &&(!String.IsNullOrEmpty(strGridKey)) && (!String.IsNullOrEmpty(strGridKeyValue)))
                if ((!strMainGID.Equals(strGIDTemp)) && (!String.IsNullOrEmpty(strGridKey)))
                {
                    strSql = "select " + strPIDTemp + " from " + strGroupTable + " WHERE  " + strKey + "= '" + strKeyValue + "' and " + strGridKey + "= '" + strGridKeyValue + "'";
                }    

                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    DataRow dr = dt.Rows[0];
                    strReturnValue = dr[strPIDTemp].ToString();
                }
            }
            return strReturnValue;
        }
        #endregion

    }
}
