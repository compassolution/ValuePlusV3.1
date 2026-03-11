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

public partial class WinForm_Ajax_AdjustStaffOtRestAjax : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["rmLocResourceManager"] == null)
            {
                Session["rmLocResourceManager"] = base.GetResourceManager("FrmAdjustOTREST");
            }
        }
        try
        {
            string param = Request["param"] == null ? string.Empty : Request["param"].ToString();//请求类型参数

            this.strCurUserId = Request["userId"] == null ? string.Empty : Request["userId"].ToString();//用户编号
            this.strCurYearMonth = Request["yearMonth"] == null ? string.Empty : Request["yearMonth"].ToString();//考勤区间
            this.strCurStaffNo = Request["staffNo"] == null ? string.Empty : Request["staffNo"].ToString();//员工编号

            string strFilterSql = Request["filterSql"] == null ? string.Empty : Server.UrlDecode(Request["filterSql"].ToString());//过滤条件

            string strStart = Request["start"] == null ? string.Empty : Server.UrlDecode(Request["start"].ToString());//
            string strLimit = Request["limit"] == null ? string.Empty : Server.UrlDecode(Request["limit"].ToString());//

            if (param != String.Empty)
            {
                switch (param)
                {
                    case "AdjustStaffListColumnInfo"://加载员工列表显示列
                        this.GetAdjustStaffListColumn(strCurUserId, strCurYearMonth);
                        break;
                    case "MultiPageStaffListDataInfo"://加载员工列表数据信息
                        this.GetAdjustStaffGridData(strLimit, strStart, strCurUserId, strCurYearMonth);
                        break;
                    case "FilterStaffListDataInfo"://根据过滤条件分页获取员工列表数据信息
                        this.GetStaffGridDataByFilter(strLimit, strStart, strFilterSql);
                        break;
                    case "StaffDateItemListColumnInfo"://加载加班调休调整日期列表显示列
                        this.GetDateItemListColumn();
                        break;
                    case "StaffDateItemListDataInfo"://加载加班调休调整日期列表数据信息
                        this.GetDateItemListGridData();
                        break;
                    case "saveAdjustOTRESTResult"://保存员工加班调休的调整信息
                        string strDate = Request["date"] == null ? string.Empty : Server.UrlDecode(Request["date"].ToString());//调整时间
                        string strResultModifyStr = Request["staffListModifyStr"] == null ? string.Empty : Server.UrlDecode(Request["staffListModifyStr"].ToString());//调整结果字符串
                        this.SaveAdjustData(strDate,strResultModifyStr);
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
    private IList<Hashtable> hashColListStaffList
    {
        get
        {
            return ViewState["hashColListStaffList"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashColListStaffList"] = value;
        }
    }
    private String strCurUserId
    {
        get
        {
            return ViewState["strCurUserId"] as String;
        }
        set
        {
            ViewState["strCurUserId"] = value;
        }
    }
    private String strCurYearMonth
    {
        get
        {
            return ViewState["strCurYearMonth"] as String;
        }
        set
        {
            ViewState["strCurYearMonth"] = value;
        }
    }
    private String strCurStaffNo
    {
        get
        {
            return ViewState["strCurStaffNo"] as String;
        }
        set
        {
            ViewState["strCurStaffNo"] = value;
        }
    }
    private String strFlagIsToNormal
    {
        get
        {
            return ViewState["strFlagIsToNormal"] as String;
        }
        set
        {
            ViewState["strFlagIsToNormal"] = value;
        }
    }
    private IList<Hashtable> hashColListDateItemList
    {
        get
        {
            return ViewState["hashColListDateItemList"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashColListDateItemList"] = value;
        }
    }
    #endregion

    #region 获取员工加班调休调整时员工列表的所有列名
    /// <summary>
    /// 获取员工加班调休调整时员工列表的所有列名
    /// </summary>
    private void GetAdjustStaffListColumn(String strCurUserId,String strCurYearMonth)
    {
        try
        {
            if (this.hashColListStaffList == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmAdjustOTREST");

                String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetStaffList_Col");
                strSql = strSql.Replace("@YEARMONTH", strCurYearMonth);
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

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
                this.hashColListStaffList = hashList;
                ResponseObject(hashList);
            }
            else
            {
                ResponseObject(this.hashColListStaffList);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工加班调休调整时员工列表信息数据总数
    /// <summary>
    /// 获取员工加班调休调整时员工列表信息数据总数
    /// </summary>
    /// <param name="bRefresh"></param>
    /// <returns></returns>
    private int GetDsTotalCount(bool bRefresh,String strCurUserId, String strCurYearMonth)
    {
        int iCountDs = 0;
        if (bRefresh)
        {
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetStaffList_COUNT");
            strSql = strSql.Replace("@SUSERID", strCurUserId);
            strSql = strSql.Replace("@YEARMONTH", strCurYearMonth);

            iCountDs = SqlParamDao.ExecuteScalarBySql(strSql);
            Session["strAdjustOTRESTStaffCount"] = iCountDs.ToString();
        }
        else
        {
            iCountDs = Convert.ToInt32(Session["strAdjustOTRESTStaffCount"]);
        }
        return iCountDs;
    }
    #endregion

    #region 获取员工加班调休调整时员工列表所有信息数据,返回DataSet
    /// <summary>
    /// 获取员工加班调休调整时员工列表所有信息数据,返回DataSet
    /// </summary>
    /// <returns></returns>
    private DataSet GetAllStaffListDataSet()
    {
        DataSet ds = null;
        if (Session["dsAdjustOTRESTStaffList"] == null)
        {
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetStaffList_DATA");
            strSql = strSql.Replace("@SUSERID", this.strCurUserId);

            ds = SqlParamDao.GetDataSetBySql(strSql);
            Session["dsAdjustOTRESTStaffList"] = ds;
        }
        else
        {
            ds = (DataSet)Session["dsAdjustOTRESTStaffList"];
        }
        return ds;
    }
    #endregion

    #region 根据过滤条件分页获取所有员工加班调休调整时员工列表
    /// <summary>
    /// 根据过滤条件分页获取所有员工加班调休调整时员工列表
    /// </summary>
    /// <returns></returns>
    private void GetStaffGridDataByFilter(String strLimit, String strStart, String strFilterSql)
    {
        try
        {
            int iLimit = Convert.ToInt32(strLimit.Trim());
            int iStart = Convert.ToInt32(strStart.Trim());
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetStaffList_MultiPage");
            strSql = strSql.Replace("@SUSERID", this.strCurUserId);
            strSql = strSql.Replace("@PAGESIZE", iLimit.ToString()).Replace("@STARTINDEX", iStart.ToString());
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

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

            List<Hashtable> hashList = new List<Hashtable>();
            int iRowsCount = 0;
            if (dsView != null && dsView.Tables[0].Rows.Count > 0)
            {
                iRowsCount = dsView.Tables[0].Rows.Count;
                int iGetterSize = iStart + iLimit;
                if (iGetterSize > iRowsCount)
                {
                    iGetterSize = iRowsCount;
                }

                for (int i = iStart; i < iGetterSize; i++)
                {
                    DataRow row = dsView.Tables[0].Rows[i];
                    Hashtable ht = new Hashtable();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        ht.Add(col.ColumnName, row[col.ColumnName]);
                    }
                    hashList.Add(ht);
                }
            }
            string json = "{AllStaffListTotalPorperty:" + iRowsCount.ToString() + ",AllStaffList:" + JsonConvert.SerializeObject(hashList) + "}";
            Response.Write(json);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 分页获取员工加班调休调整时员工列表信息数据
    /// <summary>
    /// 分页获取员工加班调休调整时员工列表信息数据
    /// </summary>
    private void GetAdjustStaffGridData(String strLimit, String strStart,String strCurUserId,String strCurYearMonth)
    {
        try
        {
            //String strUserId = base.GetUserCode();
            int iLimit = Convert.ToInt32(strLimit.Trim());
            int iStart = Convert.ToInt32(strStart.Trim());

            int iCountDs = 0;
            if (!String.IsNullOrEmpty(strStart))
            {
                if (strStart.Equals("0"))//如果翻页当前起始位置是0则重新加载
                {
                    iCountDs = GetDsTotalCount(true, strCurUserId, strCurYearMonth);
                }
                else//否则表示是在翻页操作，则读取缓存
                {
                    iCountDs = GetDsTotalCount(false, strCurUserId, strCurYearMonth);
                }
            }
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetStaffList_MultiPage");
            strSql = strSql.Replace("@SUSERID", strCurUserId);
            strSql = strSql.Replace("@PAGESIZE", iLimit.ToString()).Replace("@STARTINDEX", iStart.ToString());
            strSql = strSql.Replace("@YEARMONTH", strCurYearMonth);
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int rows = ds.Tables[0].Rows.Count;
                List<Hashtable> hashList = new List<Hashtable>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Hashtable ht = new Hashtable();
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        ht.Add(col.ColumnName, row[col.ColumnName]);
                    }
                    hashList.Add(ht);
                }
                string json = "{StaffListTotalPorperty:" + iCountDs.ToString() + ",StaffList:" + JsonConvert.SerializeObject(hashList) + "}";
                Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工加班调休调整时日期列表的所有列名
    /// <summary>
    /// 获取员工加班调休调整时日期列表的所有列名
    /// </summary>
    private void GetDateItemListColumn()
    {
        try
        {
            if (this.hashColListDateItemList == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmAdjustOTREST");

                String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetDateList_Col");
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

                String strRmKey = "";
                String strCaption = "";
                IList<Hashtable> hashList = new List<Hashtable>();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0] as DataRow;

                    foreach (DataColumn col in row.Table.Columns)
                    {
                        Hashtable hash = new Hashtable();
                        strRmKey = "col" + col.ColumnName.ToUpper();
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
                this.hashColListStaffList = hashList;
                ResponseObject(hashList);
            }
            else
            {
                ResponseObject(this.hashColListStaffList);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 获取员工加班调休调整时日期列表信息数据
    /// <summary>
    /// 获取员工加班调休调整时日期列表信息数据
    /// </summary>
    private void GetDateItemListGridData()
    {
        try
        {
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_OTREST_GetDateList_DATA");
            strSql = strSql.Replace("@YEARMONTH", this.strCurYearMonth);
            strSql = strSql.Replace("@STAFFNO", this.strCurStaffNo);

            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

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
                            DateTime dtTemp1 = DateTime.Parse(row[col.ColumnName].ToString());
                            String strTemp1 = dtTemp1.Date.ToString("yyyy-MM-dd");
                            ht.Add(col.ColumnName, strTemp1);
                        }
                        else
                        {
                            ht.Add(col.ColumnName, row[col.ColumnName]);
                        }
                        //ht.Add(col.ColumnName, row[col.ColumnName]);
                    }
                    hashList.Add(ht);
                }
                string json = "{StaffDateItemListTotalPorperty:" + rows.ToString() + ",StaffDateItemList:" + JsonConvert.SerializeObject(hashList) + "}";
                Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 保存员工加班调休的调整信息
    /// <summary>
    /// 保存员工加班调休的调整信息
    /// </summary>
    private void SaveAdjustData(String strDate,String strResultModifyStr)
    {
        try
        {
            if (!String.IsNullOrEmpty(strResultModifyStr))
            {
                String[] strArr = strResultModifyStr.Split('＠');
                if ((strArr != null) && (strArr.Length == 2))
                {
                    String strColName = strArr[0].ToString();
                    String strValue = strArr[1].ToString();
                    String strSql = "UPDATE KQRSSZ_2 SET ";
                    strSql = strSql + " "+strColName +"='" + strValue + "'";
                    strSql = strSql + " WHERE R_DATE = '" + strDate + "' AND EM_NO = '" + this.strCurStaffNo + "'";
                    int iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
                }
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
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
