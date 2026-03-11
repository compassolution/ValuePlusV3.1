using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Com.ValuePlus.Web;
using Com.ValuePlus.BLL.WinForm;
using System.Collections.Generic;
using System.Resources;
using Newtonsoft.Json;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.DAL;
using System.Text;

public partial class WinForm_Ajax_KQPaiBanAjax : Com.ValuePlus.Web.PageBase
{
    private static String[] strArrDay = new String[]{ "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11", "D12", "D13", "D14", "D15", "D16", "D17", "D18", "D19", "D20", "D21", "D22", "D23", "D24", "D25", "D26", "D27", "D28", "D29", "D30", "D31" };

    //获取配置中的周锁定是否针对全部部门的设置
    private String strIsLockWeekForCurSection = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsLockWeekForCurSection");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["rmLocResourceManager"] == null)
            {
                Session["rmLocResourceManager"] = base.GetResourceManager("FrmKQPaiBan");
            }
        }
        try
        {
            string param = Request["param"] == null ? string.Empty : Request["param"].ToString();//请求类型参数
            string strYearMonth = Request["yearMonth"] == null ? string.Empty : Request["yearMonth"].ToString();//考勤月份
            //this.strCurYearMonth = strYearMonth;
            string strYearMonthStartDay = Request["yearMonthStartDay"] == null ? string.Empty : Request["yearMonthStartDay"].ToString();//考勤月份起始时间
            string strModifyResults = Request["resultStr"] == null ? string.Empty : Request["resultStr"].ToString();//批量编辑排班结果组成的字符串
            string strStuffNo = Request["stuffNo"] == null ? string.Empty : Request["stuffNo"].ToString();//员工编号
            string strDay = Request["day"] == null ? string.Empty : Request["day"].ToString();//考勤日期（仅日）
            string strResultModifyStr = Request["resultModifyStr"] == null ? string.Empty : Server.UrlDecode(Request["resultModifyStr"].ToString());//是否调整为正常
            string strStartDay = Request["startday"] == null ? string.Empty : Request["startday"].ToString();//分析开始日期
            string strEndDay = Request["endday"] == null ? string.Empty : Request["endday"].ToString();//分析开始日期
            string strCondition = Request["condition"] == null ? string.Empty : Request["condition"].ToString();//查询条件
            string strFilterSql = Request["filterSql"] == null ? string.Empty : Server.UrlDecode(Request["filterSql"].ToString()).Replace("¥", "%");//过滤条件

            this.strCurUserId = Request["userId"] == null ? string.Empty : Request["userId"].ToString();//请求操作的用户ID
            if (String.IsNullOrEmpty(this.strCurUserId))
            {
                this.strCurUserId = this.GetUserCode();
            }
            this.strCurSectionId = this.GetSectionIdByUserId(this.strCurUserId);
                        
            if (param != String.Empty)
            {
                switch (param)
                {
                    case "KQPerd"://加载班次信息
                        GetKQPerdInfo();
                        break;
                    case "GetKQPredLock"://通过排班月份ID获取排班月份是否锁定信息
                        this.GetKQPerdLockById(strYearMonth);
                        break;
                    case "KQPaiBanColumnInfo"://加载特定月份的考勤排班显示列
                        GetKQPaiBanGridColumn(strYearMonth);
                        break;
                    case "KQPaiBanDataInfo"://加载特定月份的考勤排班显示数据信息
                        GetKQPaiBanGridData(strYearMonth, strCondition);
                        break;
                    case "KQShifColumnInfo"://加载排班班次显示列
                        this.GetKQShifGridColumn();
                        break;
                    case "KQShifDataInfo"://加载排班班次数据信息
                        this.GetKQShifGridData(strFilterSql);
                        break;
                    case "KQStuffResultColumnInfo"://加载员工考勤结果显示列
                        this.GetKQStuffResultGridColumn();
                        break;
                    case "KQStuffResultDataInfo"://加载员工考勤结果数据信息
                        this.GetKQStuffResultGridData(strStuffNo, strYearMonth, strDay, strYearMonthStartDay);
                        break;
                    case "saveKQStuffResultData"://保存员工考勤结果调整信息
                        this.saveKQStuffResultData(strStuffNo, strYearMonth, strDay, strResultModifyStr, strYearMonthStartDay);
                        break;
                    case "KQUnNormalResultInfo"://加载员工异常考勤结果数据信息
                        this.GetKQUnNormalResultData(strYearMonth);
                        break;
                    case "saveKQPaiBanData"://保存考勤排班信息
                        SaveKQPaiBanGridData(strYearMonth, strModifyResults);
                        break;
                    case "ExcuteSP_Analyse"://执行考勤分析存储过程(批量)
                        ExcuteSP_Analyse(strYearMonth);
                        break;
                    //###########add by sammen 20180122
                    case "GetStaffList"://获取当前账户所管辖的员工列表 
                        GetStaffList(this.strCurUserId, strYearMonth);
                        break;
                    case "ExcuteAnalyseByStaffInit"://按员工执行分析前的数据初始化准备 
                        ExcuteAnalyseByStaff_DoInit(strYearMonth, strStartDay, strEndDay, this.strCurUserId);
                        break;
                    case "ExcuteAnalyseByStaff"://执行考勤分析存储过程(按员工执行)
                        ExcuteAnalyseByStaff(strStuffNo, strYearMonth, strStartDay, strEndDay,this.strCurUserId);
                        break;
                    case "ExcuteAnalyseByStaffSummary"://按员工执行分析后的汇总分析 
                        ExcuteAnalyseByStaff_DoSummary(strYearMonth, strStartDay, strEndDay,this.strCurUserId);
                        break;
                    //###########add by sammen 20180122
                    case "savePaibanRealTime"://即时保存单个员工某一天的排版信息add  by sammen 20120618 即时保存
                        this.SavePaibanDataRealTime(strStuffNo, strYearMonth, strResultModifyStr);
                        break;
                    case "saveAdjustResultRealTime"://即时保存单个员工考勤结果调整信息add  by sammen 20120618 即时保存
                        string strSeqNo = Request["seqNo"] == null ? string.Empty : Server.UrlDecode(Request["seqNo"].ToString());//表KQRSSZ_2字段SEQNO
                        string strDate = Request["date"] == null ? string.Empty : Server.UrlDecode(Request["date"].ToString());//表KQRSSZ_2字段r_Date
                        this.SaveAdjustDataRealTime(strSeqNo, strDate, strResultModifyStr);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    #region viewstate初始化区域
    private string strCurUserId
    {
        get
        {
            return ViewState["strCurUserId"] as string;
        }
        set
        {
            ViewState["strCurUserId"] = value;
        }
    }
    private string strCurSectionId
    {
        get
        {
            return ViewState["strCurSectionId"] as string;
        }
        set
        {
            ViewState["strCurSectionId"] = value;
        }
    }
    private string strCurYearMonth
    {
        get
        {
            return ViewState["strCurYearMonth"] as string;
        }
        set
        {
            ViewState["strCurYearMonth"] = value;
        }
    }
    private DataSet dsKQPerdInfo
    {
        get
        {
            return ViewState["dsKQPerdInfo"] as DataSet;
        }
        set
        {
            ViewState["dsKQPerdInfo"] = value;
        }
    }
    private IList<Hashtable> hashColListKQPaiban
    {
        get
        {
            return ViewState["hashColListKQPaiban"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashColListKQPaiban"] = value;
        }
    }
    private IList<Hashtable> hashColListKQShif
    {
        get
        {
            return ViewState["hashListKQShif"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashListKQShif"] = value;
        }
    }
    private IList<Hashtable> hashColListKQStuffResult
    {
        get
        {
            return ViewState["hashListKQStuffResult"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashListKQStuffResult"] = value;
        }
    }
    #endregion

    #region 根据用户ID获取其所在的小部门
    /// <summary>
    /// 根据用户ID获取其所在的小部门
    /// </summary>
    /// <param name="strUserId"></param>
    /// <returns></returns>
    private String GetSectionIdByUserId(String strUserId)
    {
        String strSectionId = "";
        try
        {
            if (!String.IsNullOrEmpty(strUserId))
            {
                String strSql = "SELECT * FROM KQDL_1 WHERE DCNO = '"+strUserId+"'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    strSectionId = row["DCDDESCCHS"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("WinForm_Ajax_KQPaiBanAjax.GetSectionIdByUserId(" + strUserId + ")/R/N");
            log.Error("WinForm_Ajax_KQPaiBanAjax.GetSectionIdByUserId" + ex);
        }
        return strSectionId;
    }
    #endregion

    #region 获取排班月份信息
    /// <summary>
    /// 获取排班月份信息
    /// </summary>
    private void GetKQPerdInfo()
    {
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            DataSet ds = bllKQPaiBan.GetAllKQPred();
            this.dsKQPerdInfo = ds;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                IList<Hashtable> hashList = new List<Hashtable>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Hashtable hash = new Hashtable();
                    hash["PID"] = row["PID"].ToString();
                    hashList.Add(hash);
                }
                ResponseObject(hashList);
            }
            else
            {
                ResponseMsg("暂无排班月份表！");
                return;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 通过排班月份ID获取排班月份是否锁定信息
    /// <summary>
    /// 通过排班月份ID获取排班月份是否锁定信息
    /// </summary>
    private void GetKQPerdLockById(String strPid)
    {
        //获取之前如果有自动设置，则执行自动设置的逻辑 add by sammen 20171204
        this.AutoSetPerdLock(strPid);
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            if (!String.IsNullOrEmpty(strPid))
            {
                DataSet ds = bllKQPaiBan.GetKQPredInfoById(strPid);
                //IList<Hashtable> hashList = new List<Hashtable>();
                Hashtable hash = new Hashtable();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    String strRowPid = row["PID"].ToString();
                    DateTime dtStartDay = (DateTime)row["PSTART"];
                    String strStartDay = dtStartDay.Date.ToString("yyyy-MM-dd");
                    DateTime dtEndDay = (DateTime)row["PEND"];
                    String strEndDay = dtEndDay.Date.ToString("yyyy-MM-dd");
                    String strDayCount = row["PDAYS"].ToString();
                    String strPlock = row["PLOCK"].ToString();

                    //获取特定月份特定小部门的考勤分析状态位
                    String strAnalysStatus = "1";
                    String strSql = "SELECT * FROM KQPERD_4 WHERE PID = '" + strRowPid + "' AND SECTIONCODE = '" + this.strCurSectionId + "'";

                    //String strSql = "SELECT * FROM KQPERD_4 WHERE PID = '" + strRowPid + "' AND SECTIONCODE IN (select SEPNO from KQDL_2 where DCNO = '" + this.strCurUserId + "') ";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        strAnalysStatus = dr["KQSTATUS"].ToString();
                    }
                    
                    hash.Add(strPid, strStartDay + "*" + strEndDay + "*" + strPlock + "*" + strAnalysStatus);
                }
                ResponseObject(hash);
            }
            else
            {
                ResponseMsg("GetKQPerdLockById Failed");
                return;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region //判断是否是日期字段
    /// <summary>
    /// //判断是否是日期字段
    /// </summary>
    /// <param name="strColName"></param>
    /// <returns></returns>
    private bool IsDayCol(String strColName)
    {
        bool bIs = false;
        int iCount = strArrDay.Length;
        for (int i = 0; i < iCount; i++)
        {
            if (strColName == strArrDay[i])
            {
                bIs = true;
                break;
            }
        }
        return bIs;
    }
    #endregion

    #region 获取特定月份的考勤排班信息(从KQPERD_3表中读取D1/D2/D3......)
    /// <summary>
    /// 获取特定月份的考勤排班信息的所有列名
    /// <param name="strYearMonth"></param>
    /// </summary>
    void GetKQPaiBanGridColumn(String strYearMonth)
    {
        try
        {
            if (this.hashColListKQPaiban == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select * ");
                sbSql.Append(" ,ISNULL((SELECT PLOCK FROM KQPERD_1 WHERE PID = A.PID),'2') AS MonthIsLocked");
                sbSql.Append(" from KQPERD_3 A where PID='" + strYearMonth + "' order by DATE");
                String strSql = sbSql.ToString();
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);

                IList<Hashtable> hashList = new List<Hashtable>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    //首先植入非日期的字段
                    ////modify by sammen 20191010新增部门名称列的显示
                    String[] strCol = new String[] { "EMPLOYEE", "YEARMONTH", "ENAME", "CNAME", "DCPOSI", "DCPOSICHS" ,"DeptName", "DeptNameChs" };
                    int iCount = strCol.Length;
                    for (int i = 0; i < iCount; i++)
                    {
                        String strColName = strCol[i];
                        Hashtable hash = new Hashtable();
                        String strRmKey = "col" + strColName;
                        String strCaption = strColName;
                        if (rmLocResourceManager.GetString(strRmKey) != null)
                        {
                            strCaption = rmLocResourceManager.GetString(strRmKey);
                        }
                        hash.Add(strColName, strCaption);
                        hashList.Add(hash);
                    }
                    String strDaysAll = "";
                    //加载日期列（D1/D2/...）
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Hashtable hash = new Hashtable();
                        DataRow row = dt.Rows[i] as DataRow;
                        String strMonthIsLocked = row["MonthIsLocked"].ToString();
                        String strDNUM = row["DNUM"].ToString();
                        DateTime dtDate = DateTime.Parse(row["Date"].ToString());
                        String strDate = dtDate.Day.ToString();

                        String strPlock = row["PLOCK"].ToString();
                        ////新增针对各个部门分别进行周锁定的功能 add by sammen 20181029
                        if (strMonthIsLocked.Equals("1"))
                        {   //如果月度锁定，则每天都锁定
                            strPlock = "1";
                        }
                        else
                        {
                            //获取配置中的周锁定是否针对全部部门的设置
                            if (!strIsLockWeekForCurSection.Equals("1"))
                            {
                                //针对所有部门一起
                            }else
                            {
                                String strLockedSections = row["LockedSections"].ToString();
                                if ((strPlock.Equals("1")) && (strLockedSections.ToUpper().IndexOf((this.strCurUserId + ",").ToUpper()) > -1))
                                {
                                    //如果周锁定部门中存在当前用户部门，则认为被锁定，否则非锁定状态
                                    strPlock = "1";
                                }
                                else
                                {
                                    strPlock = "2";
                                }
                            }
                        }

                        hash.Add(strDNUM, strDate + "*" + strPlock);

                        hashList.Add(hash);
                    }
                }
                this.hashColListKQPaiban = hashList;
                ResponseObject(hashList);
            }
            else
            {
                ResponseObject(this.hashColListKQPaiban);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    /// <summary>
    /// 获取特定月份的考勤排班信息的所有数据
    /// <param name="strYearMonth"></param>
    /// <param name="strCondition"></param>
    /// </summary>
    void GetKQPaiBanGridData(String strYearMonth,String strCondition)
    {
        try
        {
            //首先获取需要读取的列
            String strSql_Col = "select * from KQPERD_3 where PID='" + strYearMonth + "' order by DATE";
            DataTable dt_Col = SqlParamDao.GetDataTableBySql(strSql_Col);
            String strDaysColAll = "";
            if (dt_Col != null && dt_Col.Rows.Count > 0)
            {
                //加载日期列
                for (int i = 0; i < dt_Col.Rows.Count; i++)
                {
                    DataRow row = dt_Col.Rows[i] as DataRow;
                    String strDNUM = row["DNUM"].ToString();

                    strDaysColAll = strDaysColAll + ",B." + strDNUM;
                }
            }

            //在获取相应列的数据
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("select A.* from (");
            //判断是否在排班界面的最前端显示客房入住率 add by sammen 20180307
            String strIsShowRoomRateInPaibanPage = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsShowRoomRateInPaibanPage");
            if (strIsShowRoomRateInPaibanPage.Equals("1"))
            {
                sbSql.Append(" select * from dbo.[Fun_GetRoomRate_To_PaibanPage]('" + strYearMonth + "')");
                sbSql.Append(" union all");
            }
            ////modify by sammen 20191010新增部门名称列的显示
            sbSql.Append(" select A.ENAME,A.CNAME,B.EMPLOYEE,B.YEARMONTH,vwName.DCPOSI,vwName.DCPOSICHS,vwName.DeptName,vwName.DeptNameChs,B.D1,B.D2,B.D3,B.D4,B.D5,B.D6,B.D7,B.D8,B.D9,B.D10,B.D11,B.D12,B.D13,B.D14,B.D15,B.D16,B.D17,B.D18,B.D19,B.D20,B.D21,B.D22,B.D23,B.D24,B.D25,B.D26,B.D27,B.D28,B.D29,B.D30,B.D31");
            sbSql.Append(" from FUN_KQTOE_1('" + strYearMonth + "') A inner join KQTOE_2 B on A.EMPLOYEE = B.EMPLOYEE");
            sbSql.Append(" inner join FUN_VW_PAIBAN_STAFF_FILTER('" + strYearMonth + "') vwName on A.EMPLOYEE = vwName.STAFFID");
            //add by sammen 20190629，为了配合页面中过滤员工结果列表行时增加一些数据点
            sbSql.Append(" inner join csorga_1 c on a.empdept=c.oid left join kqrssz_1 d on b.employee=d.em_no and b.yearmonth=d.ymonth and d.ymonth = B.YEARMONTH ");
            sbSql.Append(" WHERE B.YEARMONTH = '" + strYearMonth + "' AND vwName.SUSERID = '" + this.strCurUserId + "' ");
            if (!String.IsNullOrEmpty(strCondition))
            {
                sbSql.Append(" AND " + strCondition);
            }
            sbSql.Append(" ) as A left join HRDOCU_1 B ON A.EMPLOYEE = B.DCNO");
            ////modify by sammen 20191010新增默认排序规则（部门、级别、工号）
            sbSql.Append(" ORDER BY B.DCDDESCCHS,B.DCPLEVEL,A.EMPLOYEE");

            //strIsShowRoomRateInPaibanPage = String.IsNullOrEmpty("") ? "0" : strIsShowRoomRateInPaibanPage;

            String strSql = sbSql.ToString();
            DataTable dt = new DataTable();
            try
            {
                dt = SqlParamDao.GetDataTableBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("排班界面根据方法FUN_KQTOE_1或者FUN_VW_PAIBAN_STAFF_FILTER获取当前用户能管辖的员工清单错误:" + strSql + "\r\n" + ex.ToString());
                strSql = strSql.Replace("FUN_KQTOE_1('" + strYearMonth + "')", "KQTOE_1");
                strSql = strSql.Replace("FUN_VW_PAIBAN_STAFF_FILTER('" + strYearMonth + "')", "VW_PAIBAN_STAFF_FILTER");
                dt = SqlParamDao.GetDataTableBySql(strSql);
            }

            //String strSql = "select A.ENAME,A.CNAME,B.EMPLOYEE,B.YEARMONTH,vwName.DCPOSI,vwName.DCPOSICHS" + strDaysColAll + " from KQTOE_1 A,KQTOE_2 B,[VIEWNAME] vwName"; 
            //strSql = strSql + " WHERE A.EMPLOYEE = B.EMPLOYEE and B.YEARMONTH = @YEARMONTH AND A.EMPLOYEE = vwName.[STAFFID] AND vwName.[SUSERID] = @SUSERID ";
            //if (this.Language.Equals("zh-cn"))
            //{
            //    strSql = strSql + " ORDER BY vwName.DCPOSICHS";//根据职位排序
            //}
            //else
            //{
            //    strSql = strSql + " ORDER BY vwName.DCPOSI";//根据职位排序
            //}
            /////////根据视图VIEWNAME中预设的排序查询出

            //String strVwName = BaseConfig.Instance.GetConfigValueByKey("VwName_PaibanStaff_Filter");
            //String strVwParam1 = BaseConfig.Instance.GetConfigValueByKey("VwCol1_PaibanStaff_Filter");
            //String strVwParam2 = BaseConfig.Instance.GetConfigValueByKey("VwCol2_PaibanStaff_Filter");
            //strSql = strSql.Replace("[VIEWNAME]", strVwName);
            //strSql = strSql.Replace("[SUSERID]", strVwParam1);
            //strSql = strSql.Replace("[STAFFID]", strVwParam2);

            //strSql = strSql.Replace("@YEARMONTH", "'"+strYearMonth+"'");
            //strSql = strSql.Replace("@SUSERID", "'"+this.strCurUserId+"'");


            //为了跟踪，打印出语句
            log.Error("排班界面排班数据:"+strSql);

            string json = "";
            int rows = 0;
            List<Hashtable> hashList = new List<Hashtable>();
            if (dt != null && dt.Rows.Count > 0)
            {
                rows = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        ht.Add(col.ColumnName, row[col.ColumnName]);
                    }
                    hashList.Add(ht);
                }
            }
            json = "{KQPaiBanTotalPorperty:" + rows + ",KQPaiBanResult:" + JsonConvert.SerializeObject(hashList) + "}";
            Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取特定月份的考勤排班信息(从KQTOE_2表中读取D1/D2/D3......)【暂时屏蔽】
    ///// <summary>
    ///// 获取特定月份的考勤排班信息的所有列名
    ///// <param name="strYearMonth"></param>
    ///// </summary>
    //void GetKQPaiBanGridColumn(String strYearMonth)
    //{
    //    try
    //    {
    //        if (this.hashColListKQPaiban == null)
    //        {
    //            ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
    //            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
    //            DataSet ds = bllKQPaiBan.GetKQTOE1A2InfoGridCol();

    //            //获取当前排班月份的起始时间（为了显示实际的期间日期即不一定从D1/D2 开始）
    //            //DataSet dsPred = bllKQPaiBan.GetKQPredInfoById(strYearMonth);
    //            //DateTime dtStartDay = new DateTime();
    //            //String strStartDay = "";
    //            //int iDaysCount = 0;
    //            //if (dsPred != null && dsPred.Tables[0].Rows.Count > 0)
    //            //{
    //            //    DataRow row = dsPred.Tables[0].Rows[0];
    //            //    dtStartDay = (DateTime)row["PSTART"];
    //            //    strStartDay = dtStartDay.Date.ToString("yyyy-MM-dd");
    //            //    iDaysCount = (int)row["PDAYS"];
    //            //}

    //            String strRmKey = "";
    //            String strCaption = "";
    //            IList<Hashtable> hashList = new List<Hashtable>();
    //            if (ds != null && ds.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow row = ds.Tables[0].Rows[0] as DataRow;

    //                foreach (DataColumn col in row.Table.Columns)
    //                {
    //                    Hashtable hash = new Hashtable();
    //                    strRmKey = "col" + col.ColumnName;
    //                    if (rmLocResourceManager.GetString(strRmKey) != null)
    //                    {
    //                        strCaption = rmLocResourceManager.GetString(strRmKey);
    //                        hash.Add(col.ColumnName, strCaption);
    //                    }
    //                    else
    //                    {
    //                        strCaption = col.Caption;
    //                        //**********如下部分处理，是显示实际的期间日期,即不一定从D1/D2 开始
    //                        //if (IsDayCol(strCaption))//判断是否是日期字段
    //                        //{
    //                        //    //如果是，则根据当前排班月起始时间获取实际的日期
    //                        //    String strTempDay = strCaption.TrimStart('D');
    //                        //    int iDay = int.Parse(strTempDay);
    //                        //    if (iDay <= iDaysCount)
    //                        //    {
    //                        //        DateTime dtTemp = Convert.ToDateTime(strStartDay);
    //                        //        dtTemp = dtTemp.AddDays(iDay - 1);
    //                        //        strCaption = "D" + dtTemp.Day.ToString();

    //                        //        hash.Add(col.ColumnName, strCaption);
    //                        //    }
    //                        //}
    //                        //else
    //                        //{
    //                        //    hash.Add(col.ColumnName, strCaption);
    //                        //}
    //                        hash.Add(col.ColumnName, strCaption);
    //                    }
    //                    hashList.Add(hash);
    //                }
    //            }
    //            this.hashColListKQPaiban = hashList;
    //            ResponseObject(hashList);
    //        }
    //        else
    //        {
    //            ResponseObject(this.hashColListKQPaiban);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }
    //}

    ///// <summary>
    ///// 获取特定月份的考勤排班信息的所有数据
    ///// <param name="strYearMonth"></param>
    ///// </summary>
    //void GetKQPaiBanGridData(String strYearMonth)
    //{
    //    try
    //    {
    //        KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
    //        //DataSet ds = bllKQPaiBan.GetAllKQTOE1A2InfoByYearMonth(strYearMonth);
    //        String strCurUserId = this.strCurUserId;
    //        DataSet ds = bllKQPaiBan.GetAllKQTOE1A2InfoByYearMonthAUserId(strYearMonth, strCurUserId);

    //        string json = "";
    //        int rows = 0;
    //        List<Hashtable> hashList = new List<Hashtable>();
    //        if (ds != null && ds.Tables[0].Rows.Count > 0)
    //        {
    //            rows = ds.Tables[0].Rows.Count;
    //            foreach (DataRow row in ds.Tables[0].Rows)
    //            {
    //                Hashtable ht = new Hashtable();
    //                foreach (DataColumn col in row.Table.Columns)
    //                {
    //                    ht.Add(col.ColumnName, row[col.ColumnName]);
    //                }
    //                hashList.Add(ht);
    //            }
    //        }
    //        json = "{KQPaiBanTotalPorperty:" + rows + ",KQPaiBanResult:" + JsonConvert.SerializeObject(hashList) + "}";
    //        Response.Write(json);
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }
    //}
    #endregion

    #region 获取班次信息表的所有列名
    /// <summary>
    /// 获取班次信息表的所有列名
    /// </summary>
    void GetKQShifGridColumn()
    {
        try
        {
            if (this.hashColListKQShif == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
                KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
                DataSet ds = bllKQPaiBan.GetKQShifGridCol();

                String strRmKey = "";
                String strCaption = "";
                IList<Hashtable> hashList = new List<Hashtable>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0] as DataRow;

                    foreach (DataColumn col in row.Table.Columns)
                    {
                        Hashtable hash = new Hashtable();
                        strRmKey = "col" + col.ColumnName;
                        if (rmLocResourceManager.GetString(strRmKey) != null)
                        {
                            strCaption = rmLocResourceManager.GetString(strRmKey);
                        }
                        else
                        {
                            strCaption = col.Caption;
                        }
                        hash.Add(col.ColumnName, strCaption);
                        hashList.Add(hash);
                    }
                }
                this.hashColListKQShif = hashList;
                ResponseObject(hashList);
            }
            else
            {
                ResponseObject(this.hashColListKQShif);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取班次信息表的所有数据
    /// <summary>
    /// 获取班次信息表的所有数据
    /// </summary>
    void GetKQShifGridData(String strFilterSql)
    {
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            //DataSet ds = bllKQPaiBan.GetAllKQShif();
            String strCurUserId = this.strCurUserId;
            DataSet ds = bllKQPaiBan.GetKQShifInfoByUserId(strCurUserId);

            DataSet dsView = new DataSet();

            if (!String.IsNullOrEmpty(strFilterSql))
            {
                ds.Tables[0].DefaultView.RowFilter = strFilterSql;

                //设置全局dataset
                DataView dv = ds.Tables[0].DefaultView;
                System.Data.DataTable dt = dv.ToTable();
                dsView.Tables.Add(dt.Copy());
            }
            else
            {
                dsView = ds;
            }

            if (dsView != null && dsView.Tables[0].Rows.Count > 0)
            {
                int rows = dsView.Tables[0].Rows.Count;
                List<Hashtable> hashList = new List<Hashtable>();
                foreach (DataRow row in dsView.Tables[0].Rows)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (col.DataType.Equals(System.Type.GetType("System.DateTime")))
                        {
                            //日期类型转换
                            String strTempDate = row[col.ColumnName].ToString();
                            String[] arrTemp = strTempDate.Split(' ');
                            String strTemp = "";
                            if ((arrTemp != null) && (arrTemp.Length == 3))
                            {
                                strTemp = arrTemp[1] + " " + arrTemp[2];
                            }
                            if (!strTemp.Equals(""))
                            {
                                strTempDate = strTemp;
                            }
                            ht.Add(col.ColumnName, strTempDate);
                        }
                        else
                        {
                            ht.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                    hashList.Add(ht);
                }
                string json = "{KQShifTotalPorperty:" + rows + ",KQShifResult:" + JsonConvert.SerializeObject(hashList) + "}";
                Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工考勤结果信息表的所有列名
    /// <summary>
    /// 获取员工考勤结果信息表的所有列名
    /// </summary>
    void GetKQStuffResultGridColumn()
    {
        try
        {
            if (this.hashColListKQStuffResult == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmKQPaiBan");
                KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
                DataSet ds = bllKQPaiBan.GetKQResultGridCol();

                String strRmKey = "";
                String strCaption = "";
                IList<Hashtable> hashList = new List<Hashtable>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0] as DataRow;

                    foreach (DataColumn col in row.Table.Columns)
                    {
                        Hashtable hash = new Hashtable();
                        strRmKey = "col" + col.ColumnName;
                        if (rmLocResourceManager.GetString(strRmKey) != null)
                        {
                            strCaption = rmLocResourceManager.GetString(strRmKey);
                        }
                        else
                        {
                            strCaption = col.Caption;
                        }
                        hash.Add(col.ColumnName, strCaption);
                        hashList.Add(hash);
                    }
                }
                this.hashColListKQStuffResult = hashList;
                ResponseObject(hashList);
            }
            else
            {
                ResponseObject(this.hashColListKQStuffResult);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工考勤结果信息表的所有数据
    /// <summary>
    /// 获取员工考勤结果信息表的所有数据
    /// </summary>
    void GetKQStuffResultGridData(String strStuffNo, String strYearMonth, String strDay,String strYearMonthStartDay)
    {
        try
        {
            DateTime dtDay = parseStringToDateTime(strYearMonth, strDay, strYearMonthStartDay);
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            DataSet ds = bllKQPaiBan.GetKQResultByEmnoADay(strStuffNo, strYearMonth, dtDay);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int rows = ds.Tables[0].Rows.Count;
                List<Hashtable> hashList = new List<Hashtable>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (col.DataType.Equals(System.Type.GetType("System.DateTime")))
                        {
                            //日期类型转换
                            String strTemp1 = "";
                            if (row[col.ColumnName] != DBNull.Value)
                            {
                                DateTime dtTemp1 = (DateTime)row[col.ColumnName];
                                if (col.ColumnName.Equals("r_Date"))
                                {
                                    strTemp1 = dtTemp1.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    strTemp1 = dtTemp1.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                            ht.Add(col.ColumnName, strTemp1);
                        }
                        else
                        {
                            ht.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                    hashList.Add(ht);
                }
                string json = "{KQStuffResultTotalPorperty:" + rows + ",KQStuffResult:" + JsonConvert.SerializeObject(hashList) + "}";
                Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工某一月份考勤非正常结果信息数据
    /// <summary>
    /// 获取员工某一月份考勤非正常结果信息数据
    /// </summary>
    void GetKQUnNormalResultData(String strYearMonth)
    {
        try
        {
            //KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            //DataSet ds = bllKQPaiBan.GetUnNormalInfoByYearMonth(strYearMonth);
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT A.* FROM VW_HR_KQ_UNNORMAL A LEFT JOIN FUN_VW_PAIBAN_STAFF_FILTER('" + strYearMonth + "') B ON A.SSTAFFNO = B.STAFFID ");
            sbSql.Append(" WHERE [SYEARMONTH]='" + strYearMonth + "' and B.SUSERID = '" + this.strCurUserId + "' order by convert(int,[sday])");

            String strSql = sbSql.ToString();
            DataSet ds = new DataSet();
            try
            {
                ds = SqlParamDao.GetDataSetBySql(strSql);
            }
            catch (Exception ex)
            {
                log.Error("排班界面根据方法FUN_VW_PAIBAN_STAFF_FILTER获取员工某一月份考勤非正常结果信息数据错误:" + strSql + "\r\n" + ex.ToString());
                strSql = strSql.Replace("FUN_VW_PAIBAN_STAFF_FILTER('" + strYearMonth + "')", "VW_PAIBAN_STAFF_FILTER");
                ds = SqlParamDao.GetDataSetBySql(strSql);
            }


            List<Hashtable> hashList = new List<Hashtable>();

            String strStuffNo = "";
            String strDay = "";
            String strState = "";
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int rows = ds.Tables[0].Rows.Count;
                Hashtable ht = new Hashtable();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strStuffNo = row["SSTAFFNO"].ToString();
                    strDay = row["SDAY"].ToString();
                    strState = row["SSTATE"].ToString();

                    ht.Add(strStuffNo + "*" + strDay, strState);
                }
                hashList.Add(ht);
            }
            ResponseObject(hashList);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 保存调整后的员工考勤结果信息
    /// <summary>
    /// 保存调整后的员工考勤结果信息
    /// </summary>
    private void saveKQStuffResultData(String strStuffNo, String strYearMonth, String strDay, String strResultModifyStr, String strYearMonthStartDay)
    {
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            int iCount = 0;
            String strJbHour = "null";
            String strTxHour = "null";
            String strRemark = "null";
            String strIsNormal = "null";
            if ((!String.IsNullOrEmpty(strStuffNo)) && (!String.IsNullOrEmpty(strYearMonth)) && (!String.IsNullOrEmpty(strDay)) && (!String.IsNullOrEmpty(strResultModifyStr)))
            {
                String[] strArrModifyItemAValue = strResultModifyStr.Split('^');//每条记录中的修改对象组，各个对象组间用^隔开
                if ((strArrModifyItemAValue != null) && (strArrModifyItemAValue.Length > 0))
                {
                    for (int j = 0; j < strArrModifyItemAValue.Length; j++)
                    {
                        String strModifyItem = "";
                        String strModifyValue = "";
                        String[] strArrTemp = strArrModifyItemAValue[j].Split('*');//每个对象组中的item和value，中间用*隔开
                        if ((strArrTemp != null) && (strArrTemp.Length > 1))
                        {
                            strModifyItem = strArrTemp[0].ToString();
                            if (strArrTemp[1].ToString().Equals("space"))
                            {
                                strModifyValue = "";
                            }
                            else
                            {
                                strModifyValue = strArrTemp[1].ToString();
                            }
                            if ((strModifyValue == null) || (strModifyValue.ToLower().Equals("null")))
                            {
                                strModifyValue = "";
                            }
                            switch (strModifyItem.ToUpper())
                            {
                                case "SJBHOUR":
                                    strJbHour = strModifyValue == "" ? "0" : strModifyValue;
                                    break;
                                case "STXHOUR":
                                    strTxHour = strModifyValue == "" ? "0" : strModifyValue;
                                    break;
                                case "SREMARK":
                                    strRemark = strModifyValue == "" ? " " : strModifyValue;
                                    break;
                                case "BISTONORMAL":
                                    strIsNormal = strModifyValue == "" ? "0" : strModifyValue;
                                    break;
                            }
                        }
                    }
                }

                DateTime dtDay = parseStringToDateTime(strYearMonth, strDay,strYearMonthStartDay);
                iCount = bllKQPaiBan.ModifyKQResultByEmnoADay(strJbHour, strTxHour, strRemark, strIsNormal, "0", strStuffNo, dtDay);

                if (strIsNormal.Equals("1"))//如果是调整为正常
                {
                    //同时更新非正常结果信息表数据，将state设置为“2”表示调整为正常
                    bllKQPaiBan.ModifyUnNormalByEmnoADay(strStuffNo, strYearMonth, strDay, "2");
                }
            }
            if (iCount > 0)
            {
                //GetKQPaiBanGridData(strYearMonth);
                ResponseMsg("恭喜你，排班成功！");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 批量保存所做的修改
    /// <summary>
    /// 批量保存所做的修改
    /// </summary>
    private void SaveKQPaiBanGridData(String strYearMonth,string strModifyResults)
    {
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            int iCount = 0;
            if (!String.IsNullOrEmpty(strModifyResults))
            {
                iCount = bllKQPaiBan.ModifyBatchKQTOE2DataByKey(strModifyResults);

                //add by sammen 20190929 新增保存班次后的后续逻辑处理
                this.DoAfterSaveShift(strYearMonth, strModifyResults,true);
            }
            if (iCount > 0)
            {
                //GetKQPaiBanGridData(strYearMonth);
                ResponseMsg("恭喜你，排班成功！");
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }

    /// <summary>
    /// 保存班次后的后续逻辑处理
    /// add by sammen 20190929 新增
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strModifyResults"></param>
    private void DoAfterSaveShift(String strYearMonth, string strModifyResults,bool IsBatch)
    {
        //add by sammen 20190929 新增保存班次后的后续逻辑处理
        String strSPName = "USP_HR_KQ_AfterSaveShift";
        try
        {
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("ModifyResult", strModifyResults);
            hsTableParam.Add("IsBatch", IsBatch.ToString());
            hsTableParam.Add("SUSERID", this.GetUserCode());
            SqlParamDao.ExcuteSP(strSPName, hsTableParam);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("保存班次后的后续逻辑处理出错，请检查存储过程" + strSPName + "是否存在");
        }
    }

    #endregion

    #region 执行作考勤分析的存储过程
    /// <summary>
    /// 执行作考勤分析的存储过程
    /// </summary>
    private void ExcuteSP_Analyse(String strYearMonth)
    {
        try
        {
            String strUserId = this.strCurUserId;
            String strSpName = BaseConfig.Instance.GetConfigValueByKey("SpName_AnalysePaiBanReuslt");
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam1_AnalysePaiBanReuslt"), strUserId);
            hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam2_AnalysePaiBanReuslt"), strYearMonth);
            log.Error("考勤分析执行：exec " + strSpName + " '" + strUserId + "','" + strYearMonth + "'");

            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            int iCount = 0;
            if (!String.IsNullOrEmpty(strSpName))
            {
                iCount = bllKQPaiBan.ExcuteSP(strSpName, hsTableParam);

                log.Error("考勤分析执行成功!'");
            }
            ResponseObject(iCount.ToString());
        }
        catch (Exception ex)
        {
            log.Error("考勤分析执行失败!'");
            log.Error(ex);
            ResponseObject("-1");
        }
    }
    #endregion

    #region 将特定格式的日期字符串转化为datetime格式
    /// <summary>
    /// 将特定格式的日期字符串转化为datetime格式2010/5/5 0:00:00
    /// <param name="strYearMonth"></param>格式：yymm，四位字符串
    /// <param name="strDay"></param>格式：dd ，一位或者2位字符串
    /// <param name="strYearMonthStartDay"></param>
    /// <returns></returns>格式：YYYY/MM/DD 0:00:00
    /// </summary>
    private DateTime parseStringToDateTime(String strYearMonth,String strDay,String strYearMonthStartDay){
        String strYear = "";
        String strMonth = "";
        //DateTime dt = DateTime.Now;
        //if ((!String.IsNullOrEmpty(strYearMonth))&&(strYearMonth.Length==4) && (!String.IsNullOrEmpty(strDay)))
        //{
        //    strYear = "20"+strYearMonth.Substring(0,2);
        //    strMonth = strYearMonth.Substring(2,2);
        //    dt = Convert.ToDateTime(strYear + "/" + strMonth + "/" + strDay + "  0:00:00");
        //}

        DateTime dt = Convert.ToDateTime(strYearMonthStartDay);
        int iDay = int.Parse(strDay)-1;
        dt = dt.AddDays(iDay);
        return dt;
    }
    #endregion

    #region 即时保存员工排班班次信息add  by sammen 20120618 即时保存
    /// <summary>
    /// 即时保存员工排班班次信息add  by sammen 20120618 即时保存
    /// </summary>
    private void SavePaibanDataRealTime(String strStuffNo, String strYearMonth, String strResultModifyStr)
    {
        try
        {
            if ((!String.IsNullOrEmpty(strStuffNo)) && (!String.IsNullOrEmpty(strYearMonth)) && (!String.IsNullOrEmpty(strResultModifyStr)))
            {
                String strDay = "";
                String[] strArrTemp = strResultModifyStr.Split('＄');
                if (strArrTemp.Length > 0)
                {
                    String strSql = "UPDATE KQTOE_2 SET EMPLOYEE = '" + strStuffNo + "', YEARMONTH = '" + strYearMonth + "' ";
                    for (int i = 0; i < strArrTemp.Length ; i++)
                    {
                        String[] strArr = strArrTemp[i].Split('＠');
                        if ((strArr != null) && (strArr.Length == 2))
                        {
                            String strColName = strArr[0].ToString();
                            String strValue = strArr[1].ToString();
                            strSql = strSql + ", " + strColName + "='" + strValue +"'";
                            //获取相应的日期
                            strDay += strColName + "*";
                        }
                    }
                    strDay = strDay.TrimEnd('*');
                    strSql = strSql + "  WHERE EMPLOYEE = '" + strStuffNo + "' AND YEARMONTH = '" + strYearMonth + "'";
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                    //add by sammen 20190929 新增保存班次后的后续逻辑处理
                    this.DoAfterSaveShift(strYearMonth, strResultModifyStr, false);
                }

                //同时考虑即时进行考勤分析
                String strIsRealTimeAnalyse = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("Switch_IsRealTimeAnalysePaiBanReuslt");
                if (String.IsNullOrEmpty(strIsRealTimeAnalyse)) strIsRealTimeAnalyse = "0";
                if (!String.IsNullOrEmpty(strDay) && strIsRealTimeAnalyse.Equals("1"))
                {
                    String strSpNameRealTime = BaseConfig.Instance.GetConfigValueByKey("SpName_RealTimeAnalysePaiBanReuslt");
                    if (!String.IsNullOrEmpty(strSpNameRealTime))
                    {
                        Hashtable hsTableParam = new Hashtable();
                        hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam1_RealTimeAnalysePaiBanReuslt"), strStuffNo);
                        hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam2_RealTimeAnalysePaiBanReuslt"), strYearMonth);
                        hsTableParam.Add(BaseConfig.Instance.GetConfigValueByKey("SpParam3_RealTimeAnalysePaiBanReuslt"), strDay);

                        int iCount = 0;
                        try
                        {
                            if (!String.IsNullOrEmpty(strSpNameRealTime))
                            {
                                iCount = SqlParamDao.ExcuteSP(strSpNameRealTime, hsTableParam);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 即时保存员工异常考勤信息add  by sammen 20120618 即时保存
    /// <summary>
    /// 即时保存员工异常考勤信息add  by sammen 20120618 即时保存
    /// </summary>
    private void SaveAdjustDataRealTime(String strSeqNo, String strDate, String strResultModifyStr)
    {
        try
        {
            if ((!String.IsNullOrEmpty(strSeqNo)) && (!String.IsNullOrEmpty(strDate)) && (!String.IsNullOrEmpty(strResultModifyStr)))
            {
                String[] strArr = strResultModifyStr.Split('＠');
                if ((strArr != null) && (strArr.Length == 2))
                {
                    String strColName = strArr[0].ToString();
                    String strValue = strArr[1].ToString();
                    String strSql = "UPDATE KQRSSZ_2 SET ";
                    strSql = strSql + " " + strColName + "='" + strValue + "'";
                    strSql = strSql + " WHERE R_DATE = '" + strDate + "' AND SEQNO = '" + strSeqNo + "'";
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);

                    //add by sammen 20210827 新增保存考勤信息后的后续逻辑处理
                    String strSPName = "USP_HR_KQ_WriteModifyLog";
                    try
                    {
                        Hashtable hsTableParam = new Hashtable();
                        hsTableParam.Add("YearMonthDCNO", strSeqNo);
                        hsTableParam.Add("Date", strDate);
                        hsTableParam.Add("PID", strColName);
                        hsTableParam.Add("OldValue", "");
                        hsTableParam.Add("NewValue", strValue);
                        hsTableParam.Add("SUSERID", this.GetUserCode());
                        SqlParamDao.ExcuteSP(strSPName, hsTableParam);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.Error("保存考勤信息后的后续逻辑处理出错，请检查存储过程" + strSPName + "是否存在");
                    }


                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    /// <summary>
    /// 期间设置管理(KQPERD)的期间锁定的自动设置
    /// add by sammen 20171204
    /// </summary>
    /// <param name="strYearMonth"></param>
    public void AutoSetPerdLock(String strYearMonth)
    {
        String strSPName = "USP_HR_KQPERD_AutoSetLock";
        Hashtable hsParam = new Hashtable();
        hsParam.Add("PID", strYearMonth);
        try
        {
            int iCount = SqlParamDao.ExcuteSP(strSPName, hsParam);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            log.Error("执行存储过程"+ strSPName + "失败！(期间设置管理(KQPERD)的期间锁定的自动设置)");
        }
    }


    #region 新增根据员工依次分析考勤以提高效率 add  by sammen 20180122
    /// <summary>
    /// 根据用户ID获取其可分析的员工列表
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private DataTable GetStaffNoByUserId(String strUserId,String strYearMonth)
    {
        DataTable dtStaffList = new DataTable();
        try
        {
            if (!String.IsNullOrEmpty(strUserId))
            {
                String strStartDay = "";
                String strEndDay = "";
                DataTable dtYearMonth = SqlParamDao.GetDataTableBySql("select * from KQPERD_1 WHERE PID = '" + strYearMonth + "'");
                if (dtYearMonth != null && dtYearMonth.Rows.Count > 0)
                {
                    DataRow row = dtYearMonth.Rows[0];
                    DateTime dtStartDay = (DateTime)row["PSTART"];
                    strStartDay = dtStartDay.Date.ToString("yyyy-MM-dd");
                    DateTime dtEndDay = (DateTime)row["PEND"];
                    strEndDay = dtEndDay.Date.ToString("yyyy-MM-dd");
                }
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("select '"+ strStartDay + "' as StartDay, '"+strEndDay + "' as EndDay,* from FUN_KQTOE_1('" + strYearMonth + "') A ");
                sbSql.Append(" WHERE EMPDEPT IN (SELECT SEPNO FROM KQDL_2 WHERE DCNO = '" + strUserId + "') ORDER BY A.EMPLOYEE");

                String strSql = sbSql.ToString();
                log.Error("根据用户ID获取其可分析的员工列表.GetStaffNoByUserId,SQL:" + strSql);
                //String strSql = "SELECT A.* FROM FUN_KQTOE_1('" + strYearMonth + "') A INNER JOIN KQDL_1 B ON A.EMPDEPT = B.DCDDESCCHS WHERE B.DCNO = '" + strUserId + "' ORDER BY A.EMPLOYEE";
                dtStaffList = SqlParamDao.GetDataTableBySql(strSql);
            }
        }
        catch (Exception ex)
        {
            log.Error("WinForm_Ajax_KQPaiBanAjax.GetStaffNoByUserId(" + strUserId + "，" + strYearMonth + ")/R/N");
            log.Error("WinForm_Ajax_KQPaiBanAjax.GetStaffNoByUserId" + ex);
        }
        return dtStaffList;
    }

    /// <summary>
    /// 根据用户ID获取其可分析的员工列表
    /// </summary>
    /// <param name="strUserId"></param>
    /// <param name="strYearMonth"></param>
    /// <returns></returns>
    private void GetStaffList(String strUserId, String strYearMonth)
    {
        StringBuilder sbJson = new StringBuilder();
        try
        {
            //获取员工列表之前先更新月度员工清单表
            String strSpName = "USP_HR_Update_MonthStaffList";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            log.Error("更新月度员工清单表：exec " + strSpName + " '" + strYearMonth + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            DataTable dtStaff = GetStaffNoByUserId(strUserId, strYearMonth);
            sbJson.Append("{");
            sbJson.Append(WebCommon.GetJsonStringByDataTable(dtStaff, "ResultData", true));//当前SID配置的一行记录信息
            sbJson.Append("}");

            Response.Write(sbJson.ToString());
            //Response.Write(sbJson.ToString());//返回单个值时都会带双引号
        }
        catch (Exception ex)
        {
            log.Error("根据用户ID获取其可分析的员工列表失败!'");
            log.Error(ex);
            Response.Write("error");
            //ResponseObject("error");
        }
    }

    /// <summary>
    /// 按员工依次执行作考勤分析前的数据初始化操作
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    private void ExcuteAnalyseByStaff_DoInit(String strYearMonth, String strStartDay, String strEndDay, String strUserId)
    {
        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_Init";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            log.Error("按员工依次执行作考勤分析前的数据初始化操作：exec " + strSpName + " '" + strYearMonth + "', '" + strStartDay + "'," + " '" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            Response.Write(iSPReturn.ToString());
            //ResponseObject(iSPReturn.ToString());//返回单个值时都会带双引号
        }
        catch (Exception ex)
        {
            log.Error("按员工依次执行作考勤分析前的数据初始化操作失败(USP_KQ_ByStaff_ANALY_Init)!'");
            log.Error(ex);
            Response.Write("-1");
            //ResponseObject("-1");
        }
    }

    /// <summary>
    /// 按员工依次执行作考勤分析的存储过程
    /// </summary>
    /// <param name="strStuffNo"></param>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    private void ExcuteAnalyseByStaff(String strStaffNo,String strYearMonth, String strStartDay, String strEndDay,String strUserId)
    {
        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_DoStaff";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("em_No", strStaffNo);
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            log.Error("按员工依次执行考勤分析：exec " + strSpName + " '" + strStaffNo + "'," + " '" + strYearMonth +  "', '" + strStartDay + "'," + " '" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            Response.Write(iSPReturn.ToString());
            //ResponseObject(iSPReturn.ToString());//返回单个值时都会带双引号
        }
        catch (Exception ex)
        {
            log.Error("按员工依次分析考勤执行失败（USP_KQ_ByStaff_ANALY_DoStaff）!'");
            log.Error(ex);
            Response.Write("-1");
            //ResponseObject("-1");
        }
    }

    /// <summary>
    /// 按员工依次执行作考勤分析的存储过程后的汇总分析
    /// </summary>
    /// <param name="strYearMonth"></param>
    /// <param name="strStartDay"></param>
    /// <param name="strEndDay"></param>
    /// <param name="strUserId"></param>
    private void ExcuteAnalyseByStaff_DoSummary(String strYearMonth, String strStartDay, String strEndDay, String strUserId)
    {
        try
        {
            String strSpName = "USP_KQ_ByStaff_ANALY_Summary";
            Hashtable hsTableParam = new Hashtable();
            hsTableParam.Add("YearMonth", strYearMonth);
            hsTableParam.Add("DATEF", strStartDay);
            hsTableParam.Add("DATET", strEndDay);
            hsTableParam.Add("SUSERID", strUserId);
            log.Error("按员工依次执行考勤分析后的汇总分析：exec " + strSpName  + " '" + strYearMonth + "', '" + strStartDay + "'," + " '" + strEndDay + "','" + strUserId + "'");
            int iSPReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);

            Response.Write(iSPReturn.ToString());
            //ResponseObject(iSPReturn.ToString());//返回单个值时都会带双引号
        }
        catch (Exception ex)
        {
            log.Error("按员工依次执行作考勤分析的存储过程后的汇总分析失败(USP_KQ_ByStaff_ANALY_Summary)!'");
            log.Error(ex);
            Response.Write("-1");
            //ResponseObject("-1");
        }
    }

    #endregion

    //输出对象
    void ResponseObject(object obj)
    {
        Response.Write(JsonConvert.SerializeObject(obj));
    }

    //输出消息
    void ResponseMsg(string msg)
    {
        Response.Write(JsonConvert.SerializeObject(msg));
    }

}
