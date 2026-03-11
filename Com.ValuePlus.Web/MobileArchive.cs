using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Archive.BLL;
using System.Web;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.SysParams;
using Com.ValuePlus.Archive.Property;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Com.ValuePlus.DataLog.Entity;
using Com.ValuePlus.DataLog;
using Com.ValuePlus.DataLog.Enum;

namespace Com.ValuePlus.Web
{
    public class MobileArchive: PageBase
    {
        /// <summary>
        ///MobileArchive 的摘要说明
        ///主要用于从移动客户端的模板类业务的请求处理
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 【手机端应用】获取特定模板的角色列表数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strIsLoadAllRole">是否加载所有角色[1是/0否]</param>
        public String GetJsonData_RoleList(String strUserID, String strTID, String strRID, String strIsLoadAllRole)
        {
            try
            {
                //首先之前在加载角色之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadSence）
                DoExcuteBeforeLoadRole(strTID, strRID, strUserID);

                //获取角色列表
                DataTable dt_RoleList = this.GetDataTable_RoleList(strTID, "", strUserID);
                if (String.IsNullOrEmpty(strRID))
                {
                    //如果RID为空，则获取第一个角色
                    if (dt_RoleList != null && dt_RoleList.Rows.Count > 0)
                    {
                        strRID = dt_RoleList.Rows[0]["RID"].ToString();
                    }
                }
                DataView dv = dt_RoleList.DefaultView;
                dv.RowFilter = "TID = '" + strTID + "' AND RID = '" + strRID + "'";
                DataTable dt_CurRoleData = dv.ToTable();

                //如果不加载所有角色，所有角色表=当前角色表
                if (!strIsLoadAllRole.Equals("1")){
                    dt_RoleList = dt_CurRoleData;
                }

                //首先之前在加载角色之后需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadSence）
                DoExcuteAfterLoadRole(strTID, strRID, strUserID);

                String strSqlTMPH = "select * from TB_HRTMPH WHERE TID = '" + strTID + "'";
                DataTable dt_TMPH = SqlParamDao.GetDataTableBySql(strSqlTMPH);

                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_TMPH, "\"CurTMPHData\"", true));//当前TID的TMPH信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_RoleList, "\"RoleList\"", true));//当前TID的所有RID场景信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CurRoleData, "\"CurRoleData\"", true));//当前TID的当前RID的记录信息

                string json = sbJson.ToString();

                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// 【手机端应用】获取特定模板特定角色下的场景列表数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        public String GetJsonData_SceneList(String strUserID, String strTID, String strRID, String strSID)
        {
            try
            {
                //首先之前在加载场景之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadSence）
                DoExcuteBeforeLoadSence(strTID, strRID, strSID, strUserID);
                 
                //获取模板场景
                DataTable dt_SceneList = this.GetDataTable_SceneList(strTID, strRID, "", strUserID);

                if (String.IsNullOrEmpty(strSID))
                {
                    //如果SID为空，则获取第一个角色
                    if (dt_SceneList != null && dt_SceneList.Rows.Count > 0)
                    {
                        strSID = dt_SceneList.Rows[0]["SID"].ToString();
                    }
                }
                DataView dv = dt_SceneList.DefaultView;
                dv.RowFilter = "TID = '" + strTID + "' AND SID = '"+strSID+"'";
                DataTable dt_CurSceneData = dv.ToTable();

                //在加载场景之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadSence）
                DoExcuteAfterLoadSence(strTID, strRID, strSID, strUserID);

                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_SceneList, "\"SceneList\"", true));//当前TID的所有SID场景信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CurSceneData, "\"CurSceneData\"", true));//当前SID配置的一行记录信息

                string json = sbJson.ToString();

                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// 【手机端应用】获取特定模板特定角色特定场景下的业务数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strCondition"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="strRequestLanguage"></param>
        /// <returns></returns>
        public String GetJsonData_BusinessDataList(String strUserID, String strTID, String strRID, String strSID,String strCondition
            ,int iPageSize,int iPageIndex, String strRequestLanguage)
        {
            String strSqlString = "";
            try
            {
                Hashtable hsMainGroup = GetGroupInfo(strTID);
                String strSortExp = "";
                DataSet dsGridList = new DataSet();
                int iRecordCount = 0;

                Hashtable hsTableMainGroup = this.GetGroupInfo(strTID);
                String strMainGID = hsTableMainGroup["MainGroupID"].ToString();
                String strKey = DoGetGroupKeyInfo(strTID, strMainGID)[0].ToString();

                //获取移动端模板列表配置数据集
                DataTable dt_ListColumnConfig = this.GetDataTable_MBArchive_2(strTID, strMainGID);

                //获取模板场景数据
                DataTable dt_SceneList = this.GetDataTable_SceneList(strTID, strRID, strSID, strUserID);
                if ((dt_SceneList != null) && (dt_SceneList.Rows.Count > 0))
                {
                    //通过TID获取当前角色下所设置的参数值
                    GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                    Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserID, this.IsAdminstrator());

                    DataRow dr_Scene = dt_SceneList.Rows[0];
                    string strSID_Temp = dr_Scene["SID"].ToString();
                    string strSSLCT = dr_Scene["SSLCT"].ToString();
                    if (String.IsNullOrEmpty(strSSLCT))
                    {
                        strSSLCT = "select * from " + strTID + "_" + hsMainGroup["MainGroupID"].ToString();
                    }

                    strSSLCT = ParamOperationBll.ReplaceSceneSqlParam(strSSLCT, hsCurRoleParamValue);
                    //去掉sql语句中得order by ,并设置排序字符串
                    strSSLCT = RemoveOrderby(strSSLCT, ref strSortExp);

                    strSqlString = strSSLCT;

                    StringBuilder sbSqlAddition = new StringBuilder();
                    //如果有查询条件，则先设置过滤条件SQL语句
                    if (!string.IsNullOrEmpty(strCondition))
                    {
                        sbSqlAddition.Append("(");
                        for (int i = 0; i < dt_ListColumnConfig.Rows.Count; i++)
                        {
                            DataRow dr = dt_ListColumnConfig.Rows[i];
                            String strFilterPID = dr["PID"].ToString();
                            String strFilterPType = dr["PTYPE"].ToString();

                            //日期格式处理
                            if (strFilterPType.ToLower().Equals("date"))
                            {
                                strFilterPID = " CONVERT(varchar(100), " + strFilterPID + ", 23)";
                            }
                            else if (strFilterPType.ToLower().Equals("datetime"))
                            {
                                strFilterPID = " CONVERT(varchar(100), " + strFilterPID + ", 120)";
                            }
                            //处理拼音检索汉字
                            if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
                            {
                                strFilterPID = "dbo.fun_getPY(" + strFilterPID + ") ";
                            }

                            if (i >= 1)
                            {
                                sbSqlAddition.Append(" OR ");
                            }
                            sbSqlAddition.Append("(" + strFilterPID + " like '%" + strCondition + "%')");

                        }
                        sbSqlAddition.Append(")");
                    }
                    String strSqlAddtion = sbSqlAddition.ToString();

                    try
                    {
                        //将sql语句中涉及字典表的替换成字典表相应字段
                        ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
                        String strSql_Replaced = lstdReplace.GetSqlIncludeLSTHDetail(strSqlString, strTID, strSID_Temp, strMainGID, strRequestLanguage);

                        //查询过滤条件
                        if (!string.IsNullOrEmpty(strSqlAddtion))
                        {
                            strSql_Replaced = PagingSql(strSql_Replaced, strSqlAddtion, " asc");
                        }

                        //加载读取数据集，并返回记录数
                        dsGridList = ArchiveMainDealBll.GetArchiveListByServerPaging(strSql_Replaced, iPageSize, iPageIndex, strKey, strSortExp, ref iRecordCount);
                        
                        strSqlString = strSql_Replaced;
                    }
                    catch (Exception e)
                    {
                        if (strSqlString.IndexOf('*') > -1)
                        {
                            //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
                            String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(strTID, strSID, strMainGID);
                            strSqlString = strSqlString.Replace("*", strFieldNameString);
                        }
                        //查询过滤条件
                        if (!string.IsNullOrEmpty(strSqlAddtion))
                        {
                            strSqlString = PagingSql(strSqlString, strSqlAddtion, " asc");
                        }

                        //加载读取数据集，并返回记录数
                        dsGridList = ArchiveMainDealBll.GetArchiveListByServerPaging(strSqlString, iPageSize, iPageIndex, strKey, strSortExp, ref iRecordCount);
                    }
                }
                //列表数据
                DataTable dtResultData = new DataTable();
                if (dsGridList != null && dsGridList.Tables.Count > 0)
                {
                    dtResultData = dsGridList.Tables[0];
                }

                //在加载场景之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadSence）
                DoExcuteAfterLoadSence(strTID, strRID, strSID, strUserID);

                //返回分页信息
                int iTotalPage = iRecordCount / iPageSize;
                if(iRecordCount % iPageSize != 0){ iTotalPage = iTotalPage + 1; }

                StringBuilder sbPageInfo = new StringBuilder();
                sbPageInfo.Append("{");
                sbPageInfo.Append("\"TotalCount\":\"" + iRecordCount.ToString() + "\"");
                sbPageInfo.Append(",\"TotalPage\":\"" + iTotalPage.ToString() + "\"");
                sbPageInfo.Append(",\"PageSize\":\"" + iPageSize.ToString() + "\"");
                sbPageInfo.Append(",\"PageIndex\":\"" + iPageIndex.ToString() + "\"");
                sbPageInfo.Append("}");


                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dtResultData, "\"BusinessDataList\"", true));//列表数据
                sbJson.Append(",");
                sbJson.Append("\"PageInfo\":" + sbPageInfo.ToString() + "");//返回分页信息
                sbJson.Append(",");
                sbJson.Append("\"MainGID\":\"" + strMainGID + "\"");//主分组ID
                sbJson.Append(",");
                sbJson.Append("\"MainKey\":\"" + strKey + "\"");//主分组的主键字段名称
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_ListColumnConfig, "\"ListColumns\"", true));//列表中列的配置

                string json = sbJson.ToString();

                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("【手机端应用】获取特定模板特定角色特定场景下的业务数据" + strTID + "数据列表：" + strSqlString.ToString());
                return "";
            }
        }

        /// <summary>
        /// 根据单据编码获取模板主键及其类型
        /// </summary>
        /// <param name="strTID"></param>
        private String[] DoGetGroupKeyInfo(string strTID, string strGID)
        {
            String strPID = "";
            //通过档案编码和主信息分组编码获取对应的字段明细表数据,判断该单据主信息表中是否有自定义字段
            DataTable dt = SqlParamDao.GetDataTableBySql("SELECT PID,PTYPE FROM TB_HRTMPD WHERE TID='" + strTID + "' AND PISKEY=1 AND GID='" + strGID + "' order by PORDER");
            if (dt.Rows.Count > 0)
            {
                for(int i=0;i< dt.Rows.Count;i++){
                    strPID = strPID+(i==0?"":";")+ dt.Rows[i]["PID"].ToString();
                }
            }
            String[] strArray = strPID.Split(';');
            return strArray;
        }

        /// <summary>
        /// 获取模板页面相关业务数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strCondition"></param>
        /// <param name="strRequestLanguage"></param>
        public String GetArchiveListPageData( String strUserID, String strTID, String strRID, String strSID, String strCondition, String strRequestLanguage)
        {
            try
            {
                Hashtable hsMainGroup = GetGroupInfo(strTID);
                //获取角色列表
                DataTable dt_RoleList = this.GetDataTable_RoleList(strTID, strRID,strUserID);
                if (String.IsNullOrEmpty(strRID))
                {
                    //如果RID为空，则获取第一个角色
                    if (dt_RoleList!=null&& dt_RoleList.Rows.Count>0)
                    {
                        strRID = dt_RoleList.Rows[0]["RID"].ToString();
                    }
                }
                DataView dv = dt_RoleList.DefaultView;
                dv.RowFilter = "TID = '" + strTID + "' AND RID = '" + strRID + "'";
                DataTable dt_CurRoleData = dv.ToTable();

                String strSql_CurSID = "";
                String strCurSDESC = "";
                String strCurSDESCCHS = "";
                DataTable dt_CurSceneData = new DataTable();

                //首先之前在加载场景之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadSence）
                DoExcuteBeforeLoadSence(strTID, strRID, strSID, strUserID);

                Hashtable hsTableMainGroup = this.GetGroupInfo(strTID);
                String strMainGID = hsTableMainGroup["MainGroupID"].ToString();

                //获取模板场景
                DataTable dt_SceneList = this.GetDataTable_SceneList(strTID,strRID,"",strUserID);
                if ((dt_SceneList != null) && (dt_SceneList.Rows.Count > 0))
                {
                    String strSqlAddtion = strCondition;

                    //通过TID获取当前角色下所设置的参数值
                    GetArchiveSettingBll bllGetArchiveSetting = new GetArchiveSettingBll();
                    Hashtable hsCurRoleParamValue = bllGetArchiveSetting.GetRoleParamValueByTidARid(strTID, strRID, strUserID, this.IsAdminstrator());

                    for (int i = 0; i < dt_SceneList.Rows.Count; i++)
                    {
                        string strSID_Temp = dt_SceneList.Rows[i]["SID"].ToString();
                        string strSSLCT = dt_SceneList.Rows[i]["SSLCT"].ToString();
                        if (String.IsNullOrEmpty(strSSLCT))
                        {
                            strSSLCT = "select * from " + strTID + "_" + hsMainGroup["MainGroupID"].ToString();
                        }

                        strSSLCT = ParamOperationBll.ReplaceSceneSqlParam(strSSLCT, hsCurRoleParamValue);

                        //字典类型字段的替换
                        ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
                        String strSql_Replaced = lstdReplace.GetSqlIncludeLSTHDetail(strSSLCT, strTID, strSID_Temp, "1", strRequestLanguage);

                        if (strSql_Replaced.IndexOf('*') > -1)
                        {
                            //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
                            String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(strTID, strSID, hsMainGroup["MainGroupID"].ToString());
                            strSql_Replaced = strSql_Replaced.Replace("*", strFieldNameString);
                        }

                        //查询过滤条件
                        if (!string.IsNullOrEmpty(strSqlAddtion))
                        {
                            strSql_Replaced = PagingSql(strSql_Replaced, strSqlAddtion, " asc");
                        }
                        //如果传入的SID为空，则默认为第一个顺序号的SID
                        if ((String.IsNullOrEmpty(strSID)) && (i == 0))
                        {
                            strSID = strSID_Temp;
                        }

                        if (strSID.Equals(strSID_Temp))
                        {
                            strSql_CurSID = strSql_Replaced;
                            strCurSDESC = dt_SceneList.Rows[i]["SDESC"].ToString();
                            strCurSDESCCHS = dt_SceneList.Rows[i]["SDESCCHS"].ToString();
                            //复制一行当前场景的数据到新表中
                            dt_CurSceneData = dt_SceneList.Clone();
                            dt_CurSceneData.ImportRow(dt_SceneList.Rows[i]);
                        }
                    }
                }
                log.Error(strTID + "数据列表：" + strSql_CurSID.ToString());

                //在加载场景之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadSence）
                DoExcuteAfterLoadSence(strTID, strRID, strSID, strUserID);

                //列表数据
                DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql_CurSID);

                //获取移动端模板列表配置数据集
                DataTable dt_ListColumnConfig = this.GetDataTable_MBArchive_2(strTID, strMainGID);

                //当前SID下可视的Action信息
                MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
                DataTable dt_SceneActionList = mobileArchiveAction.GetDataTable_ActionList(strTID,strRID,strSID,"","'0'",strUserID);
                
                StringBuilder sbJson = new StringBuilder();
                //sbJson.Append("{");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dtResultData, "\"ResultData\"", true));//列表数据
                sbJson.Append(",");
                sbJson.Append("\"ResultCount\":[{\"TotalCount\":\"" + dtResultData.Rows.Count.ToString()+ "\"}]");//列表数据总数
                sbJson.Append(",");
                sbJson.Append("\"MainGID\":[{\"MainGID\":\"" + strMainGID + "\"}]");//主分组ID
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_ListColumnConfig, "\"ListColumns\"", true));//列表中列的配置
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_RoleList, "\"RoleList\"", true));//当前TID的所有RID场景信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CurRoleData, "\"CurRoleInfo\"", true));//当前TID的当前RID的记录信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_SceneList, "\"SceneList\"", true));//当前TID的所有SID场景信息
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CurSceneData, "\"CurSceneData\"", true));//当前SID配置的一行记录信息
                
                //sbJson.Append("\"CurSceneData\""+":[{SID:'" + strSID + "',SDESC:'" + Microsoft.JScript.GlobalObject.escape(strCurSDESC) + "',SDESCCHS:'" + Microsoft.JScript.GlobalObject.escape(strCurSDESCCHS) + "'}]");//当前对应场景信息

                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_SceneActionList, "\"ActionList\"", true));//当前SID下可视的Action信息
                //sbJson.Append("}");

                string json = sbJson.ToString();

                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "error";
            }
        }

        /// <summary>
        /// 获取模板页面相关业务明细数据
        /// 同时获取主表信息
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strRequestLanguage"></param>
        public String GetArchiveDetailMain(String strUserId, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strRequestLanguage)
        {
            string json = "";
            try
            {
                //是否是Grid分组保存
                bool IsSaveGridDetail = false;
                if (!String.IsNullOrEmpty(strGridKey))
                {
                    IsSaveGridDetail = true;
                }
                //如果主键列不存在，则获取
                if (String.IsNullOrEmpty(strKey))
                {
                    string strGetKey = "SELECT TOP 1 * FROM TB_HRTMPD A INNER JOIN TB_HRTMPG B ON A.TID = B.TID AND A.GID = B.GID WHERE A.TID = '" + strTID + "' AND B.GTYPE = '0' AND A.PISKEY = '1' ORDER BY PORDER";

                    DataTable dtGetKey = SqlParamDao.GetDataTableBySql(strGetKey);
                    strKey = dtGetKey.Rows[0]["PID"].ToString();
                }

                if (!String.IsNullOrEmpty(strTID) && !String.IsNullOrEmpty(strRID) && !String.IsNullOrEmpty(strSID) && !String.IsNullOrEmpty(strKey))
                {
                    //获取当前模板当前场景信息
                    DataTable dt_CurSceneData = this.GetDataTable_SceneList(strTID, strRID, strSID, strUserId);

                    //获取当前模板当前场景下的可视分组列表
                    DataTable dt_GID = new DataTable();

                    //获取当前模板当前场景下的可视动作的列表    
                    DataTable dt_AID = new DataTable();

                    if (!IsSaveGridDetail)
                    {//加载模板主页面
                     //*************首先之前在加载明细之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadDetail）
                        this.DoExcuteBeforeLoadDetail(strTID, strRID, strSID, strKey, strKeyValue, strUserId);

                        //获取当前模板当前场景下的可视分组列表
                        //GID=""则获取全部分组
                        dt_GID = this.GetDataTable_GroupList(strTID, strRID, strSID, "", strUserId);

                        //获取当前模板当前场景下的可视动作的列表   
                        MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
                        dt_AID = mobileArchiveAction.GetDataTable_ActionList(strTID, strRID, strSID, "", "'1'", strUserId);
                    }
                    else
                    {
                        //加载模板表格类分组的明细
                        //*************首先之前在加载明细之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadGridDetail）
                        this.DoExcuteBeforeLoadGridDetail(strTID, strRID, strSID, strGID, strKey, strKeyValue, strGridKey, strGridKeyValue, strUserId);

                        //获取当前模板当前场景下的可视分组列表
                        dt_GID = this.GetDataTable_GroupList(strTID, strRID, strSID, strGID, strUserId);
                    }

                    //获取模板业务数据明细信息
                    if (String.IsNullOrEmpty(strGID))
                    {
                        Hashtable hsTableMainGroup = this.GetGroupInfo(strTID);
                        strGID = hsTableMainGroup["MainGroupID"].ToString();
                    }

                    //根据模板分组及主键值获取业务数据集
                    DataTable dt_Record = this.GetDataTable_RecordData(strTID, strSID, strGID, strKey, strKeyValue, strGridKey, strGridKeyValue, strUserId);
                    int iRecordRows = dt_Record.Rows.Count;
                    Hashtable hsTable_Record = new Hashtable();
                    //if(dt_Record!=null && iRecordRows > 0)
                    //{
                    for (int i = 0; i < dt_Record.Columns.Count; i++)
                    {
                        //DataRow dr_Columns = dt_Record.Columns[i];
                        String strColumnName = dt_Record.Columns[i].ColumnName;
                        String strColumnType = dt_Record.Columns[i].DataType.ToString();
                        String strColumnValue = iRecordRows > 0 ? dt_Record.Rows[0][strColumnName].ToString() : "";
                        switch (strColumnType)
                        {
                            case "System.DateTime":
                                try
                                {
                                    strColumnValue = DateTime.Parse(strColumnValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    //如果是短日期
                                    if (strColumnValue.EndsWith("00:00:00"))
                                    {
                                        strColumnValue = strColumnValue.Substring(0, strColumnValue.Length - 9);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                break;
                            case "System.Byte[]":
                                //如果是二进制的类型，则转化成Base64编码
                                if (iRecordRows > 0)
                                {
                                    byte[] btValue = (byte[])dt_Record.Rows[0][strColumnName];
                                    strColumnValue = Convert.ToBase64String(btValue);
                                }
                                break;
                            default:
                                break;
                        }
                        hsTable_Record.Remove(strTID + strGID + strColumnName);
                        hsTable_Record.Add(strTID + strGID + strColumnName, strColumnValue);
                    }

                    StringBuilder sbJson = new StringBuilder();
                    //sbJson.Append("{");
                    sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CurSceneData, "\"CurSceneData\"", true));//当前SID配置的一行记录信息\
                    sbJson.Append(",");
                    sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_GID, "\"GroupList\"", true));//分组列表
                    sbJson.Append(",");
                    sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_AID, "\"ActionList\"", true));//动作列表


                    if (!IsSaveGridDetail)
                    {
                        //*************在加载明细之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadDetail）
                        this.DoExcuteAfterLoadDetail(strTID, strRID, strSID, strKey, strKeyValue, strUserId);
                    }
                    else
                    {
                        //*************在加载表格明细之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadGridDetail）
                        this.DoExcuteAfterLoadGridDetail(strTID, strRID, strSID, strGID, strKey, strKeyValue, strGridKey, strGridKeyValue, strUserId);
                    }

                    json = sbJson.ToString();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                json = "";
            }
            return json;
        }


        /// <summary>
        /// 获取模板页面单个分组的字段及其相关业务明细数据
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strRequestLanguage"></param>
        public String GetOneGroupDetailData( String strUserId, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strRequestLanguage)
        {
            try
            {
                DataTable dt_CommonGroupProperty = this.GetDataTable_PropertyAndData(strUserId, strTID, strRID, strSID, strGID, strKey, strKeyValue, strGridKey, strGridKeyValue, strRequestLanguage);
                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CommonGroupProperty, "\"PropertyData\"", true));//列表中列的配置

                string json = sbJson.ToString();
                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// 获取模板列表型分组的业务数据
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strCondition"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="strRequestLanguage"></param>
        /// <returns></returns>
        public String GetOneListGroupListData( String strUserId, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue
            ,String strCondition, int iPageSize, int iPageIndex, String strRequestLanguage)
        {
            String strSortExp = "";
            int iRecordCount = 0;
            try
            {
                //获取移动端模板列表配置数据集
                DataTable dt_ListColumnConfig = this.GetDataTable_MBArchive_2(strTID, strGID);

                //GTYPE=2时的多主键列
                String strMultiKey = strKey;
                String[] strArrayKey = this.DoGetGroupKeyInfo(strTID, strGID);
                if(strArrayKey.Length==2)
                {
                    strMultiKey = strArrayKey[0] + "," + strArrayKey[1];
                }

                DataTable dtResultData = new DataTable();
                String strTableName = strTID + "_" + strGID;
                //根据模板配置判断是否存在加密字段，如果存在则需要根据数据类型进行解密语句
                String strFieldNameString = ParamSqlStringGetterBll.GetSelectFieldString(strTID, strSID, strGID);

                string strSql = "select *  from " + strTableName + " where " + strKey + "='" + strKeyValue + "' order by " + strMultiKey;
                //将sql语句中涉及字典表的替换成字典表相应字段
                ReplaceSqlIncludeTBLSTD lstdReplace = new ReplaceSqlIncludeTBLSTD();
                strSql = lstdReplace.GetSqlIncludeLSTHDetail(strSql, strTID, strSID, strGID, strRequestLanguage);

                StringBuilder sbSqlAddition = new StringBuilder();
                //如果有查询条件，则先设置过滤条件SQL语句
                if (!string.IsNullOrEmpty(strCondition))
                {
                    sbSqlAddition.Append("(");
                    for (int i = 0; i < dt_ListColumnConfig.Rows.Count; i++)
                    {
                        DataRow dr = dt_ListColumnConfig.Rows[i];
                        String strFilterPID = dr["PID"].ToString();
                        String strFilterPType = dr["PTYPE"].ToString();

                        //日期格式处理
                        if (strFilterPType.ToLower().Equals("date"))
                        {
                            strFilterPID = " CONVERT(varchar(100), " + strFilterPID + ", 23)";
                        }
                        else if (strFilterPType.ToLower().Equals("datetime"))
                        {
                            strFilterPID = " CONVERT(varchar(100), " + strFilterPID + ", 120)";
                        }
                        //处理拼音检索汉字
                        if (SqlParamDao.IsExsitDbObject("fun_getPY", "FN"))
                        {
                            strFilterPID = "dbo.fun_getPY(" + strFilterPID + ") ";
                        }

                        if (i >= 1)
                        {
                            sbSqlAddition.Append(" OR ");
                        }
                        sbSqlAddition.Append("(" + strFilterPID + " like '%" + strCondition + "%')");

                    }
                    sbSqlAddition.Append(")");
                }
                String strSqlAddtion = sbSqlAddition.ToString();

                try
                {
                    log.Error("获取模板列表型分组的业务数据strSql：" + strSql);
                    //去掉sql语句中得order by ,并设置排序字符串
                    strSql = RemoveOrderby(strSql,ref strSortExp);

                    //查询过滤条件
                    if (!string.IsNullOrEmpty(strSqlAddtion))
                    {
                        strSql = PagingSql(strSql, strSqlAddtion, " asc");
                    }

                    //加载读取数据集，并返回记录数
                    DataSet dsGridList = ArchiveMainDealBll.GetArchiveListByServerPaging(strSql, iPageSize, iPageIndex, strMultiKey, strSortExp, ref iRecordCount);

                    dtResultData = dsGridList.Tables[0];
                }
                catch (Exception ex)
                {
                    log.Error("获取模板列表型分组的业务数据strFieldNameString：" + strFieldNameString);
                    strSql = strSql.Replace("*", strFieldNameString);
                    dtResultData = SqlParamDao.GetDataTableBySql(strSql);

                    log.Error(ex);
                    log.Error("\r\n");
                    log.Error("获取模板列表型分组的业务数据的操作失败，SQL：" + strSql);
                }

                //返回分页信息
                int iTotalPage = iRecordCount / iPageSize;
                if (iRecordCount % iPageSize != 0) { iTotalPage = iTotalPage + 1; }

                StringBuilder sbPageInfo = new StringBuilder();
                sbPageInfo.Append("{");
                sbPageInfo.Append("\"TotalCount\":\"" + iRecordCount.ToString() + "\"");
                sbPageInfo.Append(",\"TotalPage\":\"" + iTotalPage.ToString() + "\"");
                sbPageInfo.Append(",\"PageSize\":\"" + iPageSize.ToString() + "\"");
                sbPageInfo.Append(",\"PageIndex\":\"" + iPageIndex.ToString() + "\"");
                sbPageInfo.Append("}");

                StringBuilder sbJson = new StringBuilder();
                //sbJson.Append("{");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dtResultData, "\"BusinessDataList\"", true));
                sbJson.Append(",");
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_ListColumnConfig, "\"ListColumns\"", true));//列表中列的配置
                sbJson.Append(",");
                sbJson.Append("\"PageInfo\":" + sbPageInfo.ToString() + "");//返回分页信息
                sbJson.Append(",");
                sbJson.Append("\"KeyInfo\":{\"Key1\":\""+ strArrayKey[0] + "\",\"Key2\":\"" + strArrayKey[1] + "\"}");//返回分页信息
                //sbJson.Append("}");

                string json = sbJson.ToString();
                return json ;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }


        /// <summary>
        /// 获取模板场景分组下的有效事件数据
        /// add by sammen 20260305
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strEID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public String GetArchiveEventData(String strTID, String strSID, String strGID, String strPID, String strEID, String strUserId)
        {
            try
            {
                DataTable dt_CommonGroupProperty = this.GetDataTable_EventList(strTID,strSID, strGID, strPID, strEID, strUserId);
                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dt_CommonGroupProperty, "\"EventData\"", true));

                string json = sbJson.ToString();
                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// 根据sql语句获取数据集返回json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strCondition"></param>
        /// <param name="strRequestLanguage"></param>
        public String GetDataBySQL( String strSql, String strCondition,String strRequestLanguage)
        {
            try
            {
                strSql = Microsoft.JScript.GlobalObject.unescape(strSql);

                if (!(strSql.Contains("top")) && !(strSql.ToLower().Contains("percent")))
                {
                    strSql = "select top 100 percent " + strSql.Remove(0, 6);
                }
                String strTableName = "select * from (" + strSql + ") as A";
                String strSql_Record = "select * from (" + strSql + ") as A WHERE 1=1 ";
                String strSql_Top1Row = "select top 1 * from (" + strSql + ") as A ";

                //String strSql_Record = strSql;
                //String strSql_Top1Row = strSql;

                DataTable dtTop1Row = SqlParamDao.GetDataTableBySql(strSql_Top1Row);

                StringBuilder sBuilder_ColFilterSql = new StringBuilder();
                StringBuilder sBuilder_ColName = new StringBuilder();
                sBuilder_ColFilterSql.Append(" and (");
                sBuilder_ColName.Append("\"ColumnListData\":[ ");
                //数据列
                for (int i = 0; i < dtTop1Row.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        sBuilder_ColName.Append(",");
                        sBuilder_ColFilterSql.Append(" OR ");
                    }
                    String strColumnName = dtTop1Row.Columns[i].ColumnName;
                    //strColumnName = Microsoft.JScript.GlobalObject.escape(strColumnName);
                    sBuilder_ColName.Append("{\"id\":\""+(i+1).ToString()+"\",\"name\":\"" + strColumnName + "\"}");
                    sBuilder_ColFilterSql.Append("["+ strColumnName+"] like '%"+ strCondition + "%'");
                }
                sBuilder_ColFilterSql.Append(" )");
                sBuilder_ColName.Append("]");

                if (!String.IsNullOrEmpty(strCondition)){
                    strSql_Record = strSql_Record + sBuilder_ColFilterSql.ToString();
                }
                log.Error("查询语句：" + strSql_Record);
                DataTable dtResultData = SqlParamDao.GetDataTableBySql(strSql_Record);

                StringBuilder sbJson = new StringBuilder();
                sbJson.Append(WebCommon.GetJsonStringByDataTable(dtResultData, "\"RowListData\"", true));//当前SID配置的一行记录信息
                sbJson.Append(",");
                sbJson.Append(sBuilder_ColName.ToString());
                string json = sbJson.ToString();

                //json = "2";
                return json;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
        }

        /// <summary>
        /// 删除主模板业务数据，可批量删除
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        public String DeleteArchiveListData( String strUserId, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strLanguage)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            StringBuilder sbReturnJson = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(strTID) && !String.IsNullOrEmpty(strRID) && !String.IsNullOrEmpty(strSID) && !String.IsNullOrEmpty(strKey) && !String.IsNullOrEmpty(strKeyValue))
                {
                    StringBuilder sbSql_Delete = new StringBuilder();
                    if (String.IsNullOrEmpty(strGID))
                    {
                        //如果输入的GID为空，则默认为主信息表的GID
                        Hashtable hsMainGroup = this.GetGroupInfo(strTID);
                        strGID = hsMainGroup["MainGroupID"].ToString();
                    }
                    string[] strArrayKeyValue = strKeyValue.Split(';');
                    int iNeedDeleteCount = strArrayKeyValue.Length;
                    int iHadDeleteCount = 0;
                    String strLastFailMsg = "";

                    for (int i = 0; i < iNeedDeleteCount; i++)
                    {
                        String strOneKeyValue = strArrayKeyValue[i].ToString();
                        try
                        {
                            //先作删除前的判断
                            int iPreDeleteSpCount = ArchiveMainDealBll.JudgeBeforeDelete(strTID, strRID, strSID, strKeyValue, strUserId, this.IsAdminstrator(), "0");
                            if (iPreDeleteSpCount == 0)
                            {
                                //添加删除主表的语句包括所有分组
                                string strSqlstring = "SELECT GID FROM TB_HRTMPG WHERE TID='" + strTID + "' AND GVIEW=0";
                                DataTable dt_Group = SqlParamDao.GetDataTableBySql(strSqlstring);
                                if ((dt_Group != null) && (dt_Group.Rows.Count > 0))
                                {
                                    foreach (DataRow dr in dt_Group.Rows)
                                    {
                                        String strDeleteTempGID = dr["GID"].ToString();
                                        String strTableName = strTID + "_" + strDeleteTempGID;
                                        String strSql = "delete from " + strTableName + " where " + strKey + " = '" + strOneKeyValue + "';\r\n";
                                        sbSql_Delete.Append(strSql);
                                    }
                                }

                                int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Delete.ToString());
                                if (iCount > 0)
                                {
                                    //同时写入删除日志
                                    this.WriteDeleteDataLog(strUserId, strTID, strGID, strKey, strKeyValue, "", "");
                                    //如果存在删除后需要执行的存储过程，则先执行
                                    MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();
                                    ArrayList arrAction_AfterDelete = mobileArchiveAction.GetActionList(strTID, strRID, strSID, "4", strUserId);
                                    if (arrAction_AfterDelete != null && arrAction_AfterDelete.Count > 0)
                                    {
                                        Entity_CreateAction entity_AfterDelete = (Entity_CreateAction)arrAction_AfterDelete[0];
                                        bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 4, strKeyValue, strUserId, this.IsAdminstrator(), "0");
                                        log.Error("删除后存储过程语句" + entity_AfterDelete.ADETAIL);
                                    }

                                    iHadDeleteCount++;
                                }
                            }
                            else
                            {
                                ArchiveActionBll bllArchiveAction = new ArchiveActionBll();
                                strLastFailMsg = bllArchiveAction.GetPageTipAfterExcuteSp(strTID, strSID, 6, iPreDeleteSpCount, strLanguage);
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.Error("\r\n");
                            log.Error("手机端删除KeyValue执行失败，SQL：" + sbSql_Delete.ToString());

                            //strLastFailMsg
                        }
                    }
                    if(iHadDeleteCount== iNeedDeleteCount)
                    {
                        sbReturnJson.Append("{");
                        sbReturnJson.Append("\"returnCode\":\"1\"");
                        sbReturnJson.Append(",\"returnDesc\":\"" + iHadDeleteCount.ToString() + " Records Deleted Successfully!\"");
                        sbReturnJson.Append(",\"returnDescChs\":\"" + iHadDeleteCount.ToString() + "条记录删除成功!\"");
                        sbReturnJson.Append("}");
                    }else
                    {
                        sbReturnJson.Append("{");
                        sbReturnJson.Append("\"returnCode\":\"-1\"");
                        sbReturnJson.Append(",\"returnDesc\":\"" + iHadDeleteCount.ToString() + " Records Deleted Successfully，But other failed,because "+ strLastFailMsg + "\"");
                        sbReturnJson.Append(",\"returnDescChs\":\"" + iHadDeleteCount.ToString() + "条记录删除成功,但第"+(iHadDeleteCount+1).ToString() +"删除失败，原因是："+ strLastFailMsg + "\"");
                        sbReturnJson.Append("}");
                    }
                }
                else
                {
                    sbReturnJson.Append("{");
                    sbReturnJson.Append("\"returnCode\":\"-98\"");
                    sbReturnJson.Append(",\"returnDesc\":\"Failed,None Records Selected to delete!\"");
                    sbReturnJson.Append(",\"returnDescChs\":\"失败，未选择任何记录!\"");
                    sbReturnJson.Append("}");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                sbReturnJson.Append("{");
                sbReturnJson.Append("\"returnCode\":\"-99\"");
                sbReturnJson.Append(",\"returnDesc\":\"Failed,these Is some errors!\"");
                sbReturnJson.Append(",\"returnDescChs\":\"失败，请联系管理员处理！!\" ");
                sbReturnJson.Append("}");
            }
            return sbReturnJson.ToString();
        }

        /// <summary>
        /// 删除模板Grid表格业务数据，可批量删除，主表KeyValue唯一，GridKeyValue可多条
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strstrGridKeyValue"></param>
        public String DeleteArchiveGridData(String strUserId, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue
            , String strGridKey, String strGridKeyValue,String strLanguage)
        {
            ArchiveActionBll bllAction = new ArchiveActionBll();
            StringBuilder sbReturnJson = new StringBuilder();
            try
            {
                if (!String.IsNullOrEmpty(strTID) && !String.IsNullOrEmpty(strRID) && !String.IsNullOrEmpty(strSID) && !String.IsNullOrEmpty(strGID)
                    && !String.IsNullOrEmpty(strKey) && !String.IsNullOrEmpty(strKeyValue) && !String.IsNullOrEmpty(strGridKey) && !String.IsNullOrEmpty(strGridKeyValue))
                {
                    StringBuilder sbSql_Delete = new StringBuilder();

                    string[] strArrayKeyValue = strGridKeyValue.Split(';');
                    int iNeedDeleteCount = strArrayKeyValue.Length;
                    int iHadDeleteCount = 0;
                    String strLastFailMsg = "";

                    for (int i = 0; i < iNeedDeleteCount; i++)
                    {
                        String strOneGridKeyValue = strArrayKeyValue[i].ToString();
                        Hashtable hsTableParam = new Hashtable();
                        hsTableParam.Add("TID", strTID);
                        hsTableParam.Add("GID", strGID);
                        hsTableParam.Add("Key", strKey);
                        hsTableParam.Add("KeyValue", strKeyValue);
                        hsTableParam.Add("GridKey", strGridKey);
                        hsTableParam.Add("GridKeyValue", strOneGridKeyValue);
                        hsTableParam.Add("UserId", strUserId);

                        //先执行删除前的动作执行 add by sammen 20131120
                        int iCount = ArchiveGridActionBll.DoExcuteSP_BeforeDelete(hsTableParam);
                        String strMsg = ArchiveGridActionBll.GetArchiveGridActionTips(strTID, strGID, ArchiveGridActionBll.ActionID_BeforeDelete, iCount, strLanguage);
                        if (iCount == 1)
                        {
                            ///返回值为1时才可以继续执行
                            String strTableName = strTID + "_" + strGID;

                            //添加删除的语句
                            sbSql_Delete.Append("delete from " + strTableName + " where " + strKey + " = '" + strKeyValue + "' and " + strGridKey + " = '" + strOneGridKeyValue + "'");
                            try
                            {
                                iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql_Delete.ToString());
                                if (iCount > 0)
                                {
                                    //同时写入删除日志
                                    this.WriteDeleteDataLog(strUserId,strTID, strGID, strKey, strKeyValue, strGridKey, strGridKeyValue);

                                    //先执行删除后的动作执行 add by sammen 20131120
                                    iCount = ArchiveGridActionBll.DoExcuteSP_AfterDelete(hsTableParam);
                                    strLastFailMsg = ArchiveGridActionBll.GetArchiveGridActionTips(strTID, strGID, ArchiveGridActionBll.ActionID_AfterDelete, iCount, strLanguage);

                                    iHadDeleteCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                log.Error("\r\n");
                                log.Error("手机端删除GridValue执行失败，SQL：" + sbSql_Delete.ToString());

                                //strLastFailMsg
                            }
                        }
                    }
                    if (iHadDeleteCount == iNeedDeleteCount)
                    {
                        sbReturnJson.Append("{");
                        sbReturnJson.Append("\"returnCode\":\"1\"");
                        sbReturnJson.Append(",\"returnDesc\":\"" + iHadDeleteCount.ToString() + " Records Deleted Successfully!\"");
                        sbReturnJson.Append(",\"returnDescChs\":\"" + iHadDeleteCount.ToString() + "条记录删除成功!\"");
                        sbReturnJson.Append("}");
                    }
                    else
                    {
                        sbReturnJson.Append("{");
                        sbReturnJson.Append("\"returnCode\":\"-1\"");
                        sbReturnJson.Append(",\"returnDesc\":\"" + iHadDeleteCount.ToString() + " Records Deleted Successfully，But other failed,because " + strLastFailMsg + "\"");
                        sbReturnJson.Append(",\"returnDescChs\":\"" + iHadDeleteCount.ToString() + "条记录删除成功,但第" + (iHadDeleteCount + 1).ToString() + "删除失败，原因是：" + strLastFailMsg + "\"");
                        sbReturnJson.Append("}");
                    }
                }
                else
                {
                    sbReturnJson.Append("{");
                    sbReturnJson.Append("\"returnCode\":\"-98\"");
                    sbReturnJson.Append(",\"returnDesc\":\"Failed,None Records Selected to delete!\"");
                    sbReturnJson.Append(",\"returnDescChs\":\"失败，未选择任何记录!\"");
                    sbReturnJson.Append("}");
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                sbReturnJson.Append("{");
                sbReturnJson.Append("\"returnCode\":\"-99\"");
                sbReturnJson.Append(",\"returnDesc\":\"Failed,these Is some errors!\"");
                sbReturnJson.Append(",\"returnDescChs\":\"失败，请联系管理员处理！!\" ");
                sbReturnJson.Append("}");
            }
            return sbReturnJson.ToString();
        }

        /// <summary>
        /// 根据单据编码获取模板分组表TB_HRTMPG中的数据
        /// </summary>
        /// <param name="strDocuName"></param>
        public Hashtable GetGroupInfo(string strDocuName)
        {
            //根据单据编码获取模板分组表TB_HRTMPG中主信息的数据
            Hashtable hsMainGroup = new Hashtable();
            DataTable dt = SqlParamDao.GetDataTableBySql("SELECT GID,GVIEW,GSQL ,islarge FROM TB_HRTMPG WHERE TID='" + strDocuName + "' AND GTYPE=0");
            hsMainGroup.Add("MainGroupID", dt.Rows[0][0].ToString());//分组编码
            hsMainGroup.Add("MainGview", dt.Rows[0][1].ToString());//是否是根据视图读取（0表示不是，1表示是）
            hsMainGroup.Add("MainGsql", dt.Rows[0][2].ToString());//视图读取的语句（如果前面加符号@，表示从外部数据源读取）

            return hsMainGroup;
        }

        #region 获取数据集
        /// <summary>
        /// 获取模板页面单个分组的字段及其相关业务明细数据
        /// </summary>
        /// <param name="strUserID"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strRequestLanguage"></param>
        public DataTable GetDataTable_PropertyAndData( String strUserID, String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strRequestLanguage)
        {
            DataTable dt_CommonGroupProperty = new DataTable();
            try
            {
                if (!String.IsNullOrEmpty(strTID) && !String.IsNullOrEmpty(strRID) && !String.IsNullOrEmpty(strSID) && !String.IsNullOrEmpty(strGID) && !String.IsNullOrEmpty(strKey))
                {
                    //根据模板常规分组及主键值获取业务数据集
                    DataTable dt_Record = this.GetDataTable_RecordData(strTID, strSID,strGID, strKey, strKeyValue, strGridKey, strGridKeyValue, strUserID);
                    int iRecordRows = dt_Record.Rows.Count;
                    Hashtable hsTable_Record = new Hashtable();
                    //if(dt_Record!=null && iRecordRows > 0)
                    //{
                    for (int i = 0; i < dt_Record.Columns.Count; i++)
                    {
                        //DataRow dr_Columns = dt_Record.Columns[i];
                        String strColumnName = dt_Record.Columns[i].ColumnName;
                        String strColumnType = dt_Record.Columns[i].DataType.ToString();
                        String strColumnValue = iRecordRows > 0 ? dt_Record.Rows[0][strColumnName].ToString() : "";
                        switch (strColumnType)
                        {
                            case "System.DateTime":
                                try
                                {
                                    strColumnValue = DateTime.Parse(strColumnValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    //如果是短日期
                                    if (strColumnValue.EndsWith("00:00:00"))
                                    {
                                        strColumnValue = strColumnValue.Substring(0, strColumnValue.Length - 9);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                break;
                            case "System.Byte[]":
                                //如果是二进制的类型，则转化成Base64编码
                                if (iRecordRows > 0)
                                {
                                    byte[] btValue = (byte[])dt_Record.Rows[0][strColumnName];
                                    strColumnValue = Convert.ToBase64String(btValue);
                                }
                                break;
                            default:
                                break;
                        }
                        String strHsKey = (strTID + strGID + strColumnName).ToUpper();
                        hsTable_Record.Remove(strHsKey);
                        hsTable_Record.Add(strHsKey, strColumnValue);
                    }
                    //}

                    //获取字段属性记录集
                    dt_CommonGroupProperty = this.GetDataTable_PropertyList(strTID, strSID, strGID, "", strUserID);
                    if (dt_CommonGroupProperty != null && dt_CommonGroupProperty.Rows.Count > 0)
                    {
                        //#########新增当前数据值的列
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("POldValue", typeof(string))); //当前值，数据类型为文本
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PNewValue", typeof(string))); //当前值，数据类型为文本
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PValueDesc", typeof(string))); //控件为下拉框的情况下使用,即CID对应的CDESC或者CDESCCHS
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PIsDataValid", typeof(string))); //判断该控件的数据是否有效是否可保存
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PListSQL", typeof(string))); //控件为下拉框或者数据列表的情况下使用
                        //dt_CommonGroupProperty.Columns.Add(new DataColumn("PListJsonData", typeof(string))); //控件为下拉框或者数据列表的情况下的数据集Json
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PListFiled", typeof(string))); //控件数据列表的情况下使用（返回自动填写的字段）
                        dt_CommonGroupProperty.Columns.Add(new DataColumn("PIsMaster", typeof(string))); //是否是主控其他列的主人列

                        int iKeyQty = 0;
                        for (int i = 0; i < dt_CommonGroupProperty.Rows.Count; i++)
                        {
                            String strPID = dt_CommonGroupProperty.Rows[i]["PID"].ToString();
                            String strPTYPE = dt_CommonGroupProperty.Rows[i]["PTYPE"].ToString();
                            String strPCTRL = dt_CommonGroupProperty.Rows[i]["PCTRL"].ToString();
                            String strPCTRLID = dt_CommonGroupProperty.Rows[i]["PCTRLID"].ToString();
                            String strPCTRLD = dt_CommonGroupProperty.Rows[i]["PCTRLD"].ToString();
                            String strPDEFAULT = dt_CommonGroupProperty.Rows[i]["PDEFAULT"].ToString();
                            String strPSYS = dt_CommonGroupProperty.Rows[i]["PSYS"].ToString();
                            String strPMAST = dt_CommonGroupProperty.Rows[i]["PMAST"].ToString();
                            String strPSAVE = dt_CommonGroupProperty.Rows[i]["PSAVE"].ToString();
                            String strPISKEY = dt_CommonGroupProperty.Rows[i]["PISKEY"].ToString();

                            if (strPTYPE.ToLower().Equals("date"))
                            {
                                //如果是日期类型，则使用日期控件
                                strPCTRL = "12";
                                dt_CommonGroupProperty.Rows[i]["PCTRL"] = strPCTRL;
                            }
                            else if (strPTYPE.ToLower().Equals("datetime"))
                            {
                                //如果是日期时间类型，则使用日期时间控件
                                strPCTRL = "112";
                                dt_CommonGroupProperty.Rows[i]["PCTRL"] = strPCTRL;
                            }
                            if (strPCTRL.ToLower().Equals("1")&& strPCTRL.ToLower().Equals("bool"))
                            {
                                //如果是bool类型下拉框，则使用开关控件
                                strPCTRL = "11";
                                dt_CommonGroupProperty.Rows[i]["PCTRL"] = strPCTRL;
                            }


                            if (!String.IsNullOrEmpty(strPMAST) && strPMAST.IndexOf(";") < 0)
                            {
                                //如果PMAST有值，且我配置分组GID，则写成GID;PID的形式，方便后续处理
                                strPMAST = strGID + ";" + strPMAST;
                                dt_CommonGroupProperty.Rows[i]["PMAST"] = strPMAST;
                            }


                            ///#########新增当前数据值的列
                            //String strAddColumnValue = hsTable_Record[(strTID + strGID + strPID).ToUpper()].ToString();
                            String strHsKey = (strTID + strGID + strPID).ToUpper();
                            String strAddColumnValue = "";
                            if (hsTable_Record.ContainsKey(strHsKey)){
                                strAddColumnValue = hsTable_Record[strHsKey].ToString();
                            }
                            if (strPISKEY.Equals("1"))
                            {
                                if (String.IsNullOrEmpty(strAddColumnValue) && iKeyQty == 0)
                                {
                                    strAddColumnValue = strKeyValue;
                                }
                                if (String.IsNullOrEmpty(strAddColumnValue) && iKeyQty == 1)
                                {
                                    strAddColumnValue = strGridKeyValue;
                                }
                                iKeyQty++;
                            }

                            if (String.IsNullOrEmpty(strAddColumnValue))
                            {
                                //如果数据值无数据，则从配置中获取默认值
                                if (strPTYPE.ToLower().Equals("ints"))
                                {
                                    strAddColumnValue = BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo").Trim();
                                }else if (strPTYPE.ToLower().Equals("intc"))
                                {
                                    strAddColumnValue = BaseParamsGetter.GetBasicParamValue("DefaultShowStr_CleintIncreaceNo").Trim();
                                }
                                else
                                {
                                    strAddColumnValue = strPDEFAULT;
                                    switch (strPSYS)
                                    {
                                        case "1"://当前用户     
                                            strAddColumnValue = strUserID;
                                            break;
                                        case "2"://当前时间     
                                            strAddColumnValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                            break;

                                    }
                                }
                            }
                            
                            dt_CommonGroupProperty.Rows[i]["POldValue"] = strAddColumnValue;
                            dt_CommonGroupProperty.Rows[i]["PNewValue"] = strAddColumnValue;
                            dt_CommonGroupProperty.Rows[i]["PValueDesc"] = strAddColumnValue;
                            dt_CommonGroupProperty.Rows[i]["PIsDataValid"] = "1";//默认数据有效

                            ///#########写入是否是主控其他列的主人列
                            String strIsMaster = "0";//默认不是
                            StringBuilder sbJudgeIsMaster = new StringBuilder();
                            sbJudgeIsMaster.Append("select count(1) from TB_HRTMPSD WHERE TID = '" + strTID + "' AND SID = '" + strSID + "' ");
                            sbJudgeIsMaster.Append(" and (ISNULL(PMAST,'') = '" + strPID + "' or ISNULL(PMAST,'') = '" + strGID + ";" + strPID + "') ");
                            int iJudgeIsMaster = SqlParamDao.ExecuteScalarBySql(sbJudgeIsMaster.ToString());
                            strIsMaster = (iJudgeIsMaster > 0) ? "1" : "0";
                            dt_CommonGroupProperty.Rows[i]["PIsMaster"] = strIsMaster;

                            ///#########根据不同控件类型加载所需展示在页面上的相关数据
                            switch (strPCTRL)
                            {
                                case "1"://下拉框                                    
                                    TB_HRLSTDProperty property_LSTD = new TB_HRLSTDProperty(strPCTRLID);
                                    String strWhereBIsstop = "";
                                    if (property_LSTD.TABLENAME.Equals("TB_HRLSTD"))
                                    {
                                        strWhereBIsstop = " AND isnull(BISSTOP,'0') <> '1' ";//只显示正常使用的项目
                                    }
                                    String strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "'" + strWhereBIsstop + " ORDER BY " + property_LSTD.ORDER;
                                    if (!String.IsNullOrEmpty(strPMAST))
                                    {
                                        strSql = "SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "' and CUID = '@MastValue@' ORDER BY " + property_LSTD.ORDER;
                                    }

                                    //dt_CommonGroupProperty.Rows[i]["PListSQL"] = Microsoft.JScript.GlobalObject.escape(strSql);
                                    dt_CommonGroupProperty.Rows[i]["PListSQL"] = strSql;

                                    //控件为下拉框的情况下使用,即CID对应的CDESC或者CDESCCHS
                                    StringBuilder sbSql_GetDesc = new StringBuilder();
                                    sbSql_GetDesc.Append("SELECT LID,CID,CDESC,CDESCCHS,CUID FROM " + property_LSTD.TABLENAME + " WHERE LID= '" + property_LSTD.LID + "'");
                                    sbSql_GetDesc.Append(" AND CID= '" + strAddColumnValue + "'");
                                    DataTable dt_GetDesc = SqlParamDao.GetDataTableBySql(sbSql_GetDesc.ToString());
                                    if(dt_GetDesc!=null && dt_GetDesc.Rows.Count>0)
                                    {
                                        String strGetDesc = strRequestLanguage.ToLower().Equals("zh-cn")? dt_GetDesc.Rows[0]["CDESCCHS"].ToString():dt_GetDesc.Rows[0]["CDESC"].ToString();
                                        dt_CommonGroupProperty.Rows[i]["PValueDesc"] = strGetDesc;
                                    }

                                    ////控件为下拉框或者数据列表的情况下的数据集Json
                                    //DataTable dtListJson = SqlParamDao.GetDataTableBySql(strSql);
                                    //String strListJsonData = "{"+WebCommon.GetJsonStringByDataTable(dtListJson, "\"ResultData\"", true)+"}";
                                    //dt_CommonGroupProperty.Rows[i]["PListJsonData"] = strListJsonData;
                                    //strRequestLanguage//
                                    break;
                                case "2"://数据列表

                                    GetArchiveSettingBll bll = new GetArchiveSettingBll();
                                    Hashtable hsTableRoleParams = bll.GetRoleParamValueByTidARid(strTID, strRID, strUserID, this.IsAdminstrator());
                                    strSql = ParamOperationBll.ReplacePctrlDSqlParam(strPCTRLD, "@MastValue@", hsTableRoleParams);

                                    //dt_CommonGroupProperty.Rows[i]["PListSQL"] = Microsoft.JScript.GlobalObject.escape(strSql);
                                    dt_CommonGroupProperty.Rows[i]["PListSQL"] = strSql;
                                    //dt_CommonGroupProperty.Rows[i]["PListFiled"] = Microsoft.JScript.GlobalObject.escape(strPCTRLID);
                                    dt_CommonGroupProperty.Rows[i]["PListFiled"] = strPCTRLID;

                                    //控件为下拉框或者数据列表的情况下的数据集Json
                                    //dtListJson = SqlParamDao.GetDataTableBySql(strSql);
                                    //strListJsonData = WebCommon.GetJsonStringByDataTable(dtListJson, "\"\"", true);
                                    //dt_CommonGroupProperty.Rows[i]["PListJsonData"] = strListJsonData;
                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt_CommonGroupProperty;
        }

        /// <summary>
        /// 获取角色列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_RoleList(string strTID, String strRID,String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                if(this.IsAdminstrator())
                {
                    sbSql.Append("SELECT A.* FROM TB_HRTMPR A WHERE A.TID = '" + strTID + "' ");
                }else
                {
                    sbSql.Append("SELECT A.* FROM TB_HRTMPR A INNER JOIN TB_HR_USERROLE B ON A.TID = B.TID AND A.RID = B.RID ");
                    sbSql.Append(" WHERE A.TID = '" + strTID + "' AND B.SUSERID = '" + strUserId + "' ");
                }
                if (!String.IsNullOrEmpty(strRID))
                {
                    sbSql.Append(" AND A.RID = '" + strRID + "'");
                }
                sbSql.Append(" ORDER BY A.RORDER");
                log.Error(strTID + "角色列表：" + sbSql.ToString());
                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 获取场景列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_SceneList(string strTID, String strRID, String strSID, String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("SELECT A.* FROM TB_HRTMPS A INNER JOIN TB_HRTMPRD B ON A.TID = B.TID AND A.[SID] = B.[SID]");
                sbSql.Append(" WHERE A.TID = '" + strTID + "' AND B.RID = '" + strRID + "' ");
                if (!String.IsNullOrEmpty(strSID))
                {
                    sbSql.Append(" AND A.SID = '" + strSID + "'");
                }
                sbSql.Append(" ORDER BY A.SORDER");
                log.Error(strTID + "场景列表：" + sbSql.ToString());
                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 获取分组列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_GroupList(string strTID, String strRID, String strSID, String strGID, String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                //获取当前模板当前场景下的可视分组列表
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select A.*,B.GVIEW,B.GSQL from TB_HRTMPSG A INNER JOIN TB_HRTMPG B ON A.TID = B.TID AND A.GID = B.GID ");
                sbSql.Append(" WHERE A.TID = '" + strTID + "' AND A.[SID] = '" + strSID + "' AND A.GRIGHT <> '8' ");
                if (!String.IsNullOrEmpty(strGID))
                {
                    sbSql.Append(" AND A.GID = '" + strGID + "'");
                }
                sbSql.Append(" ORDER BY A.GORDER");

                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 获取分组字段属性列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_PropertyList(string strTID, String strSID, String strGID,String strPID,String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                //获取当前模板当前场景下的可视分组列表
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select * from TB_HRTMPSD WHERE TID = '" + strTID + "' AND [SID] = '" + strSID + "' AND GID = '"+ strGID + "' ");
                sbSql.Append(" AND PTYPE NOT IN ('CS','CH')");//换行和页眉类型不获取
                sbSql.Append(" AND PRIGHT IN ('0','1')");//权限为只读和编辑的，隐藏和排除的不获取
                if (!String.IsNullOrEmpty(strPID))
                {
                    sbSql.Append(" AND PID = '" + strPID + "'");
                }
                sbSql.Append(" ORDER BY GID,PORDER");

                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 获取场景分组下有效事件列表的数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strEID"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_EventList(string strTID, String strSID, String strGID, String strPID, String strEID, String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                //获取当前模板当前场景下的可视分组列表
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select * from TB_HRTMPSE where TID = '" + strTID + "' AND SID ='" + strSID + "' AND GID = '" + strGID + "' ");
                sbSql.Append(" AND ERIGHT IN ('1')");//权限为有效
                if (!String.IsNullOrEmpty(strPID))
                {
                    sbSql.Append(" AND PID = '" + strPID + "'");
                }
                if (!String.IsNullOrEmpty(strEID))
                {
                    sbSql.Append(" AND EID = '" + strEID + "'");
                }
                sbSql.Append(" ORDER BY EID");

                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }


        /// <summary>
        /// 根据模板分组及主键值获取业务数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <returns></returns>
        public DataTable GetDataTable_RecordData(string strTID,String strSID, String strGID, String strKey, String strKeyValue,String strGridKey,String strGridKeyValue, String strUserId)
        {
            DataTable dt = new DataTable();
            try
            {
                String strSelectFiledSql = ParamSqlStringGetterBll.GetSelectFieldString(strTID, strSID, strGID);
                //获取当前模板当前场景下的可视分组列表
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select "+ strSelectFiledSql + " from "+strTID+ "_" + strGID + " WHERE " + strKey + " = '" + strKeyValue + "'");
                //if ((!String.IsNullOrEmpty(strGridKey))&&(!String.IsNullOrEmpty(strGridKeyValue)))
                if (!String.IsNullOrEmpty(strGridKey))
                {
                    sbSql.Append(" AND "+ strGridKey + " = '" + strGridKeyValue + "'");
                    sbSql.Append(" ORDER BY "+ strGridKey);
                }else
                {
                    sbSql.Append(" ORDER BY " + strKey);
                }
                log.Error(strTID + ":根据模板分组及主键值获取业务数据集：" + sbSql.ToString());
                dt = SqlParamDao.GetDataTableBySql(sbSql.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }


        /// <summary>
        /// 获取移动端模板列表配置数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public DataTable GetDataTable_MBArchive_2(string strTID, String strGID)
        {
            DataTable dt = new DataTable();
            try
            {
                //获取移动端模板列表配置数据集
                StringBuilder sbSql_ListColumn = new StringBuilder();
                sbSql_ListColumn.Append("select * from MBArchive_2 where TID = '" + strTID + "'  AND GID = '" + strGID + "'  order by ShowRowFlag,PORDER");
                dt = SqlParamDao.GetDataTableBySql(sbSql_ListColumn.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        /// <summary>
        /// 获取移动端模板列表中特殊场景的配置数据集
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strCaseCode"></param>
        /// <returns></returns>
        public static DataTable GetDataTable_MBArchive_3(string strTID, String strRID,string strSID,String strCaseCode)
        {
            DataTable dt = new DataTable();
            try
            {
                //获取移动端模板列表配置数据集
                StringBuilder sbSql_ListColumn = new StringBuilder();
                sbSql_ListColumn.Append("select * from MBArchive_3 where TID = '" + strTID + "'");
                if(!String.IsNullOrEmpty(strRID)){
                    sbSql_ListColumn.Append(" AND RID = '" + strRID + "'");
                }
                if (!String.IsNullOrEmpty(strSID))
                {
                    sbSql_ListColumn.Append(" AND SID = '" + strSID + "'");
                }
                if (!String.IsNullOrEmpty(strCaseCode))
                {
                    sbSql_ListColumn.Append(" AND CaseCode = '" + strCaseCode + "'");
                }
                dt = SqlParamDao.GetDataTableBySql(sbSql_ListColumn.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return dt;
        }

        #endregion

        #region 单独执行特殊的存储过程
        /// <summary>
        /// 加载角色之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadRole）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteBeforeLoadRole(String strTID, String strRID, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_BeforeLoadRole = "USP_Archive_BeforeLoadRole";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_BeforeLoadRole, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载场景之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadSence）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteBeforeLoadSence(String strTID,String strRID,String strSID, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_BeforeLoadSence = "USP_Archive_BeforeLoadSence";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_BeforeLoadSence, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载明细之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadDetail）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteBeforeLoadDetail(String strTID, String strRID, String strSID,String strKey,String strKeyValue, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_BeforeLoadDetail = "USP_Archive_BeforeLoadDetail";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("Key", strKey);
                hsTableParam.Add("KeyValue", strKeyValue);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_BeforeLoadDetail, hsTableParam);

                log.Error("执行完存储过程USP_Archive_BeforeLoadDetail，返回结果："+ iReturnValue.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载列表型明细页面之前需要处理的业务逻辑（存储过程USP_Archive_BeforeLoadGridDetail）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public int DoExcuteBeforeLoadGridDetail(String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_BeforeLoadGridDetail = "USP_Archive_BeforeLoadGridDetail";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("GID", strGID);
                hsTableParam.Add("Key", strKey);
                hsTableParam.Add("KeyValue", strKeyValue);
                hsTableParam.Add("GridKey", strGridKey);
                hsTableParam.Add("GridKeyValue", strGridKeyValue);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_BeforeLoadGridDetail, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载角色之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadRole）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteAfterLoadRole(String strTID, String strRID, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_AfterLoadRole = "USP_Archive_AfterLoadRole";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_AfterLoadRole, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载场景之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadSence）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteAfterLoadSence(String strTID, String strRID, String strSID, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_AfterLoadSence = "USP_Archive_AfterLoadSence";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_AfterLoadSence, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载明细之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadDetail）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strUserID"></param>
        public int DoExcuteAfterLoadDetail(String strTID, String strRID, String strSID, String strKey, String strKeyValue, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_AfterLoadDetail = "USP_Archive_AfterLoadDetail";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("Key", strKey);
                hsTableParam.Add("KeyValue", strKeyValue);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_AfterLoadDetail, hsTableParam);

                log.Error("执行完存储过程USP_Archive_AfterLoadDetail，返回结果：" + iReturnValue.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }

        /// <summary>
        /// 加载列表型明细页面之后需要处理的业务逻辑（存储过程USP_Archive_AfterLoadGridDetail）
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strUserID"></param>
        /// <returns></returns>
        public int DoExcuteAfterLoadGridDetail(String strTID, String strRID, String strSID, String strGID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String strUserID)
        {
            int iReturnValue = 0;
            try
            {
                String strSPName_AfterLoadGridDetail = "USP_Archive_AfterLoadGridDetail";
                Hashtable hsTableParam = new Hashtable();
                hsTableParam.Add("TID", strTID);
                hsTableParam.Add("RID", strRID);
                hsTableParam.Add("SID", strSID);
                hsTableParam.Add("GID", strGID);
                hsTableParam.Add("Key", strKey);
                hsTableParam.Add("KeyValue", strKeyValue);
                hsTableParam.Add("GridKey", strGridKey);
                hsTableParam.Add("GridKeyValue", strGridKeyValue);
                hsTableParam.Add("UserId", strUserID);
                iReturnValue = SqlParamDao.ExcuteSP(strSPName_AfterLoadGridDetail, hsTableParam);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                iReturnValue = -1;
            }
            return iReturnValue;
        }
        #endregion

        # region 保存档案数据
        /// <summary>
        /// 保存档案数据(单挑记录保存，包括主表或者其他类型分组表)同时只是保存一张数据表
        /// </summary>
        /// <param name="strUserId"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKey"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="strSaveData"></param>
        /// <param name="strRequestLanguage"></param>
        public String SaveArchiveDetailData( String strUserId, String strTID, String strRID, String strSID, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue, String IsInsert, String strSaveData, String strRequestLanguage)
        {
            log.Error("本次请求SaveArchiveDetailData：TID=" + strTID + ";RID=" + strRID + ";SID=" + strSID + "；strKEY=" + strKey + ";strKEYVALUE=" + strKeyValue + "strGridKey=" + strGridKey + ";strGridKeyValue=" + strGridKeyValue + ";IsInsert=" + IsInsert);

            ArchiveActionBll bllAction = new ArchiveActionBll();
            StringBuilder sbReturnJson = new StringBuilder();
            MobileArchiveAction mobileArchiveAction = new MobileArchiveAction();

            //是否是Grid分组保存
            bool IsSaveGridDetail = false;
            if (!String.IsNullOrEmpty(strGridKey))
            {
                IsSaveGridDetail = true;
            }
            try
            {
                log.Error("本次请求：strSaveData=" + strSaveData);

                String strOneTableName = "";
                StringBuilder sbSubmitSql_Update = new StringBuilder();
                StringBuilder sbSubmitSql_Insert = new StringBuilder();
                //string jsonText = "["+strSaveData+"]";


                //服务器自增字段备用常量
                String strServerIncreace_GID = "";
                String strServerIncreace_PID = "";
                String strServerIncreace_PTYPE = "";
                String strServerIncreace_GTYPE = "";
                String strServerIncreace_CtrlValue = "";

                //客户端自增字段备用常量
                String strClientIncreace_GID = "";
                String strClientIncreace_PID = "";
                String strClientIncreace_PTYPE = "";
                String strClientIncreace_GTYPE = "";
                String strClientIncreace_CtrlValue = "";

                Hashtable hsTableTemp = new Hashtable();
                string jsonText = strSaveData;
                JArray jsonArray = (JArray)JsonConvert.DeserializeObject(jsonText);
                int iCount = 0;
                int iKeyQty = 0;
                foreach (JObject itemJArray in jsonArray)
                {
                    String strSubmitTID = itemJArray["TID"].ToString();
                    String strSubmitGID = itemJArray["GID"].ToString();
                    String strSubmitPID = itemJArray["PID"].ToString();
                    String strSubmitValue = itemJArray["PNewValue"].ToString();
                    String strSubmitTableName = strSubmitTID + "_" + strSubmitGID;
                    strOneTableName = strSubmitTableName;
                    String strSubmitFiledName = strSubmitPID;
                    String strSubmitPTYPE = itemJArray["PTYPE"].ToString();
                    String strSubmitPSAVE = itemJArray["PSAVE"].ToString();

                    String strPISKEY = itemJArray["PISKEY"].ToString();
                    if(strPISKEY.Equals("1")){
                        if(String.IsNullOrEmpty(strKeyValue)&&iKeyQty==0){
                            strKeyValue = strSubmitValue;
                        }
                        if (String.IsNullOrEmpty(strGridKeyValue) && iKeyQty == 1)
                        {
                            strGridKeyValue = strSubmitValue;
                        }

                        iKeyQty++;
                    }

                    //log.Error("SaveArchiveData-" + iCount.ToString() + ":{" + strSubmitTID+"_"+ strSubmitGID + "_"+ strSubmitPID + ":" + strSubmitValue + "}");

                    //服务器自增字段备用常量
                    if (String.IsNullOrEmpty(strServerIncreace_GID) && (strSubmitGID.Equals("1")) && (strSubmitPTYPE.ToLower().Equals("ints")))
                    {
                        strServerIncreace_GID = strSubmitGID;
                        strServerIncreace_PID = strSubmitPID;
                        strServerIncreace_PTYPE = strSubmitPTYPE;
                        strServerIncreace_CtrlValue = strSubmitValue;
                    }

                    //客户端自增字段备用常量
                    if (String.IsNullOrEmpty(strClientIncreace_GID) && strSubmitPTYPE.ToLower().Equals("intc"))
                    {
                        strClientIncreace_GID = strSubmitGID;
                        strClientIncreace_PID = strSubmitPID;
                        strClientIncreace_PTYPE = strSubmitPTYPE;
                        strClientIncreace_CtrlValue = strSubmitValue;
                    }

                    Entity_ToDBObject entityToDBObject = new Entity_ToDBObject();
                    //entityToDBObject.CTRLID = strCtrlId;
                    entityToDBObject.TID = strSubmitTID;
                    entityToDBObject.GID = strSubmitGID;
                    entityToDBObject.FIELDNAME = strSubmitPID;
                    entityToDBObject.FIELDVALUE_NEW = strSubmitValue;
                    entityToDBObject.FIELDTYPE = strSubmitPTYPE;
                    entityToDBObject.PSAVE = strSubmitPSAVE;

                    //遍历组织每个数据表对应的字段EntityObject
                    if (!hsTableTemp.ContainsKey(strSubmitTableName))
                    {
                        ArrayList arrListObject = new ArrayList();
                        arrListObject.Add(entityToDBObject);
                        hsTableTemp.Add(strSubmitTableName, arrListObject);
                    }
                    else
                    {
                        ArrayList arrListObject = (ArrayList)hsTableTemp[strSubmitTableName];
                        arrListObject.Add(entityToDBObject);
                        hsTableTemp.Remove(strSubmitTableName);
                        hsTableTemp.Add(strSubmitTableName, arrListObject);
                    }
                }

                //数据表主键及主键值的对键
                Hashtable hsTableKeyAndKeyValue = new Hashtable();
                hsTableKeyAndKeyValue.Remove(strKey);
                hsTableKeyAndKeyValue.Add(strKey, strKeyValue);
                if (IsSaveGridDetail)
                {
                    hsTableKeyAndKeyValue.Remove(strGridKey);
                    hsTableKeyAndKeyValue.Add(strGridKey, strGridKeyValue);
                }

                //遍历方法一：遍历哈希表中的键
                foreach (string key in hsTableTemp.Keys)
                {
                    String strTableName = key;
                    ArrayList arrListObject = (ArrayList)hsTableTemp[strTableName];
                    if (IsInsert.Equals("1"))
                    {
                        //插入语句
                        String strGetInsertSql = ParamSqlStringGetterBll.GetInsertSqlString(strTableName, arrListObject);
                        sbSubmitSql_Insert.Append(strGetInsertSql + ";");
                    }
                    else
                    {
                        //更新语句
                        String strGetUpdateSql = ParamSqlStringGetterBll.GetUpdateSqlString(strTableName, arrListObject, hsTableKeyAndKeyValue);
                        sbSubmitSql_Update.Append(strGetUpdateSql + ";");
                    }
                }

                log.Error("插入语句为：" + sbSubmitSql_Insert.ToString());
                log.Error("更新语句为：" + sbSubmitSql_Update.ToString());

                int iExcuteCount = 0;
                if (IsInsert.Equals("1"))//插入
                {
                    if (!String.IsNullOrEmpty(sbSubmitSql_Insert.ToString()))
                    {
                        //如果存在新增前需要执行的存储过程，则先执行
                        ArrayList arrAction_BeforeAdd = mobileArchiveAction.GetActionList(strTID, strRID, strSID, "5", strUserId);
                        if (arrAction_BeforeAdd != null && arrAction_BeforeAdd.Count > 0)
                        {
                            Entity_CreateAction entity_BeforeAdd = (Entity_CreateAction)arrAction_BeforeAdd[0];
                            log.Error("新增保存前存储过程语句：" + entity_BeforeAdd.ADETAIL);
                            bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 5, strKeyValue, strUserId, this.IsAdminstrator(), "0");
                        }

                        String strInsertSql = sbSubmitSql_Insert.ToString();

                        //主表新增时，才生成服务器自增控件值
                        if (!IsSaveGridDetail)
                        {
                            //如果有服务器自增字段，则获取自增主键值
                            if (strInsertSql.IndexOf(BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo").Trim()) > 0)
                            {
                                String strNewNo = AutoIncreaseFiledBll.GetServerAutoFiledNo(strTID, strServerIncreace_GID, strServerIncreace_PID, "");
                                log.Error("服务器端自增字段：{TID:'" + strTID + "',GID:'" + strServerIncreace_GID + "',PID:'" + strServerIncreace_PID + "',PTYPE:'" + strServerIncreace_PTYPE + "',CtrlValue:'" + strServerIncreace_CtrlValue + "',NewNo:'" + strNewNo + "'}");
                                strInsertSql = strInsertSql.Replace(BaseParamsGetter.GetBasicParamValue("DefaultShowStr_ServerIncreaceNo").Trim(), strNewNo);
                                strKeyValue = strNewNo;
                            }
                        }
                        else
                        {
                            //Grid分组新增明细时，获取客户端自增主键值
                            if (strInsertSql.IndexOf(BaseParamsGetter.GetBasicParamValue("DefaultShowStr_CleintIncreaceNo").Trim()) > 0)
                            {
                                String strNewNo = AutoIncreaseFiledBll.GetClientAutoFiledNo(strTID, strClientIncreace_GID, strClientIncreace_PID, strClientIncreace_PTYPE, strKey, strKeyValue).ToString();
                                log.Error("客户端自增字段：{TID:'" + strTID + "',GID:'" + strClientIncreace_GID + "',PID:'" + strClientIncreace_PID + "',PTYPE:'" + strClientIncreace_PTYPE + "',KEY:'" + strKey + "',KEYVALUE:'" + strKeyValue + "',NewNo:'" + strNewNo + "'}");
                                strInsertSql = strInsertSql.Replace(BaseParamsGetter.GetBasicParamValue("DefaultShowStr_CleintIncreaceNo").Trim(), strNewNo);
                                strGridKeyValue = strNewNo;
                            }
                        }
                        //新增之前判断该主键值是否存在？
                        String strSqlIsExistsKeyValue = "select count(1) from "+ strOneTableName + " where "+strKey+" = '"+strKeyValue+"'";
                        if(IsSaveGridDetail){
                            strSqlIsExistsKeyValue = "select count(1) from " + strOneTableName + " where " + strKey + " = '" + strKeyValue + "' and "+strGridKey+" = '"+strGridKeyValue+"'";
                        }
                        int iExistsCount = SqlParamDao.ExecuteScalarBySql(strSqlIsExistsKeyValue);
                        if(iExistsCount>0)
                        {
                            //主键值重复
                            sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "-1"));
                        }
                        else
                        {
                            log.Error("新增模板一条记录,SQL语句：" + strInsertSql);
                            iExcuteCount = SqlParamDao.ExecuteNonQueryBySql(strInsertSql);
                            if (iExcuteCount > 0)
                            {
                                //如果存在新增后需要执行的存储过程，则先执行
                                ArrayList arrAction_AfterAdd = mobileArchiveAction.GetActionList(strTID, strRID, strSID, "2", strUserId);
                                if (arrAction_AfterAdd != null && arrAction_AfterAdd.Count > 0)
                                {
                                    Entity_CreateAction entity_AfterAdd = (Entity_CreateAction)arrAction_AfterAdd[0];
                                    log.Error("新增保存后存储过程语句：" + entity_AfterAdd.ADETAIL);
                                    bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 2, strKeyValue, strUserId, this.IsAdminstrator(), "0");
                                }

                                sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "1"));
                            }
                            else
                            {
                                sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "2"));
                            }
                        }
                    }
                    else
                    {
                        sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "3"));
                    }
                }
                else//更新
                {
                    if (!String.IsNullOrEmpty(sbSubmitSql_Update.ToString()))
                    {
                        log.Error("更新模板一条记录,SQL语句：" + sbSubmitSql_Update.ToString());
                        iExcuteCount = SqlParamDao.ExecuteNonQueryBySql(sbSubmitSql_Update.ToString());
                        if (iExcuteCount > 0)
                        {
                            //如果存在编辑保存后需要执行的存储过程，则先执行
                            ArrayList arrAction_AfterEdit = mobileArchiveAction.GetActionList(strTID, strRID, strSID, "3", strUserId);
                            if (arrAction_AfterEdit != null && arrAction_AfterEdit.Count > 0)
                            {
                                Entity_CreateAction entity_AfterEdit = (Entity_CreateAction)arrAction_AfterEdit[0];
                                log.Error("编辑保存后存储过程语句：" + entity_AfterEdit.ADETAIL);
                                bllAction.ExcuteActionDetailSPByLocation(strTID, strRID, strSID, 3, strKeyValue, strUserId, this.IsAdminstrator(), "0");
                            }

                            sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "1"));
                        }
                        else
                        {
                            sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "2"));
                        }
                    }
                    else
                    {
                        sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "3"));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                sbReturnJson.Append(this.GetReturnMsgAfterSaveDetailData(strKeyValue, strGridKeyValue, IsSaveGridDetail, IsInsert, "4"));
            }
            return sbReturnJson.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strKeyValue"></param>
        /// <param name="strGridKeyValue"></param>
        /// <param name="bIsGridSave"></param>
        /// <param name="strIsInsert"></param>
        /// <param name="strResultFlag"></param>
        /// 0：exists
        /// 1：success
        /// 2：failed
        /// 3：norecord
        /// 4：exception
        /// <returns></returns>
        private string GetReturnMsgAfterSaveDetailData(String strKeyValue,String strGridKeyValue,bool bIsGridSave,String strIsInsert,String strResultFlag){
            StringBuilder sbReturnJson = new StringBuilder();
            String strReturnCode = "";
            String strReturnDesc = "";
            String strReturnDescChs = "";
            String strShowGridKeyValueString = bIsGridSave ? "-["+ strGridKeyValue + "]" : "";
            String strAddDesc = strIsInsert.Equals("1") ? "added" : "modified";
            String strAddDescChs = strIsInsert.Equals("1") ? "新增" : "修改";
            if (strResultFlag.Equals("-1"))
            {
                strReturnCode = "-1";
                strReturnDesc = "Failed,Records " + strKeyValue + strShowGridKeyValueString + " had be exists ,can not to be new!";
                strReturnDescChs = "失败，记录" + strKeyValue + strShowGridKeyValueString + "已经存在，不能再新增相同的主键值!";
            }
            else if (strResultFlag.Equals("1"))
            {
                strReturnCode = "1";
                strReturnDesc = "Records " + strKeyValue + strShowGridKeyValueString + " had be "+ strAddDesc + " successfully!";
                strReturnDescChs = "记录" + strKeyValue + strShowGridKeyValueString + " "+ strAddDescChs + "保存成功!";
            }
            else if (strResultFlag.Equals("2"))
            {
                strReturnCode = "-98";
                strReturnDesc = "Records " + strKeyValue + strShowGridKeyValueString + " had be " + strAddDesc + " failed!";
                strReturnDescChs = "记录" + strKeyValue + strShowGridKeyValueString + " " + strAddDescChs + "保存失败!";
            } else if (strResultFlag.Equals("3"))
            {
                strReturnCode = "-98";
                strReturnDesc = "Failed,No records had be " + strAddDesc + " Failed,these Is some errors!";
                strReturnDescChs = "失败,没有任何记录被" + strAddDescChs + "";
            }
            else if (strResultFlag.Equals("4"))
            {
                strReturnCode = "-99";
                strReturnDesc = "Records " + strKeyValue + strShowGridKeyValueString + " had be " + strAddDesc + " Failed,these Is some errors!";
                strReturnDescChs = "记录" + strKeyValue + strShowGridKeyValueString + " " + strAddDescChs + "保存失败，请联系管理员处理!";
            }

            sbReturnJson.Append("\"returnResult\":");
            sbReturnJson.Append("{");
            sbReturnJson.Append("\"returnCode\":\""+ strReturnCode + "\"");
            sbReturnJson.Append(",\"returnKeyValue\":\"" + strKeyValue + "\"");
            sbReturnJson.Append(",\"returnGridKeyValue\":\"" + strGridKeyValue + "\"");
            sbReturnJson.Append(",\"returnDesc\":\""+ strReturnDesc + "\"");
            sbReturnJson.Append(",\"returnDescChs\":\""+ strReturnDescChs + "\"");
            sbReturnJson.Append("}");

            return sbReturnJson.ToString();
        }

        #endregion

        /// <summary>
        /// 写入删除档案日志
        /// </summary>
        /// <param name="strKeyValue"></param>
        private void WriteDeleteDataLog(String strUserId,String strTid, String strGid, String strKey, String strKeyValue, String strGridKey, String strGridKeyValue)
        {
            if ((Session["ArchiveIsLog"] != null) && (Session["ArchiveIsLog"].ToString().Equals("1")))
            {
                Entity_HRLOG_2 entityLog2 = null;
                if (!String.IsNullOrEmpty(strGridKey) && !String.IsNullOrEmpty(strGridKeyValue))
                {
                    entityLog2 = DataLogWriter.SetDataToEntity_HRLOG_2(strTid, strGid, strGridKey, strGridKeyValue, "");
                }
                DataLogWriter.Log_Archive(strUserId, strTid, Com.ValuePlus.Utils.RequestUtils.GetIP(), LogActionType.Archive_Delete, strKeyValue, entityLog2);
            }
        }

        /// <summary>
        /// 特殊拼语句
        /// </summary>
        /// <param name="strOldSql"></param>
        /// <param name="PID"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string PagingSql(String strOldSql, String PID, String order)
        {
            string str = "SELECT * FROM (" + strOldSql + ") tempTable ";
            if (PID != "")
            {
                //return (str + " WHERE tempTable." + PID);
                return (str + " WHERE " + PID);
            }
            if (order != "")
            {
                str = str + " " + order;
            }
            return str;
        }

        /// <summary>
        /// 去掉sql语句中得order by ,并设置排序字符串
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        private string RemoveOrderby(string strOldSql, ref String strSortExp)
        {
            int startIndex = strOldSql.ToUpper().IndexOf("ORDER BY", 0, StringComparison.InvariantCultureIgnoreCase);
            if (startIndex > -1)
            {
                //add by sammen 20210516,如果已经排序了，则不从sql中获取
                if (String.IsNullOrEmpty(strSortExp))
                {
                    strSortExp = strOldSql.Substring(startIndex).ToUpper().Replace("ORDER BY", "");
                }
                strOldSql = strOldSql.Remove(startIndex);
            }
            return strOldSql;
        }
    }
}
