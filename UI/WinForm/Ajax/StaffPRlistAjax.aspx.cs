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

public partial class WinForm_Ajax_StaffPRlistAjax : Com.ValuePlus.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["dsAllStaffList_PRSLIP"] = null;
            if (Session["rmLocResourceManager"] == null)
            {
                Session["rmLocResourceManager"] = base.GetResourceManager("FrmStaffPay");
            }
            this.strSPNAME_CalItemByYearMonthAndDcno = BaseConfig.Instance.GetCommonConfigSqlByKey("SpName_CalPRITEM_ByYearMonthAndDCNO");
            this.strSPNAME_InitPRSLIPByYearMonthAndDcno = BaseConfig.Instance.GetCommonConfigSqlByKey("SpName_InitPRSLIP_ByYearMonthAndDCNO");
        }
        try
        {
            string param = Request["param"] == null ? string.Empty : Request["param"].ToString();//请求类型参数
            string strYearMonthDcno = Request["YEARMONTHDCNO"] == null ? string.Empty : Request["YEARMONTHDCNO"].ToString();//员工编号

            string strFilterSql = Request["filterSql"] == null ? string.Empty : Server.UrlDecode(Request["filterSql"].ToString());//过滤条件
            //string strFilterSql = Request["filterSql"] == null ? string.Empty : Request["filterSql"].ToString();//过滤条件

            string strStart = Request["start"] == null ? string.Empty : Server.UrlDecode(Request["start"].ToString());//
            string strLimit = Request["limit"] == null ? string.Empty : Server.UrlDecode(Request["limit"].ToString());//


            if (param != String.Empty)
            {
                switch (param)
                {
                    case "PRListStaffListColumnInfo"://加载薪资调整员工列表显示列
                        this.GetAdjustStaffListColumn();
                        break;
                    case "PRListMultiPageStaffListDataInfo"://加载薪资调整员工列表数据信息
                        this.GetAdjustStaffGridData(strLimit, strStart);
                        break;
                    case "PRListFilterStaffListDataInfo"://根据过滤条件分页获取员工列表数据信息
                        this.GetStaffGridDataByFilter(strLimit, strStart, strFilterSql);
                        break;
                    case "saveAdjustResultRealTime"://即时保存单个员工某个薪资项目的值
                        string strResultModifyStr = Request["resultModifyStr"] == null ? string.Empty : Server.UrlDecode(Request["resultModifyStr"].ToString());//调整结果字符串
                        this.SaveAdjustDataRealTime(strYearMonthDcno, strResultModifyStr);
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
    private String strSPNAME_CalItemByYearMonthAndDcno
    {
        get
        {
            return ViewState["strSPNAME_CalItemByYearMonthAndDcno"] as String;
        }
        set
        {
            ViewState["strSPNAME_CalItemByYearMonthAndDcno"] = value;
        }
    }
    private String strSPNAME_InitPRSLIPByYearMonthAndDcno
    {
        get
        {
            return ViewState["strSPNAME_InitPRSLIPByYearMonthAndDcno"] as String;
        }
        set
        {
            ViewState["strSPNAME_InitPRSLIPByYearMonthAndDcno"] = value;
        }
    }
    #endregion

    #region 获取薪资调整员工列表的所有列名
    /// <summary>
    /// 获取薪资调整员工列表的所有列名
    /// </summary>
    private void GetAdjustStaffListColumn()
    {
        try
        {
            if (this.hashColListStaffList == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmStaffPay");
                String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_PRSLIP_GetStaffList_Col");
                DataSet ds = SqlParamDao.GetDataSetBySql(strSql);

                Hashtable hashTablePRItemName = this.GetPRItemName();

                String strRmKey = "";
                String strCaption = "";
                IList<Hashtable> hashList = new List<Hashtable>();
                //获取列名
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0] as DataRow;

                    foreach (DataColumn col in row.Table.Columns)
                    {
                        Hashtable hash = new Hashtable();
                        strRmKey = "col" + col.ColumnName;
                        if (rmLocResourceManager.GetString(strRmKey) != null)
                        {
                            //从资源文件获取
                            strCaption = rmLocResourceManager.GetString(strRmKey);
                        }
                        else
                        {
                            strCaption = col.ColumnName;

                            //从薪资项目表中获取
                            if (hashTablePRItemName.ContainsKey(col.ColumnName))
                            {
                                strCaption = hashTablePRItemName[col.ColumnName].ToString();
                            }
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

    #region 分页获取薪资调整员工列表信息数据
    /// <summary>
    /// 分页获取薪资调整员工列表信息数据
    /// </summary>
    private void GetAdjustStaffGridData(String strLimit, String strStart)
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
                    iCountDs = GetDsTotalCount(true);
                }
                else//否则表示是在翻页操作，则读取缓存
                {
                    iCountDs = GetDsTotalCount(false);
                }
            }
            DataSet ds = this.GetAllStaffListMultiPage(iLimit, iStart);

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
    
    #region 根据过滤条件分页获取所有薪资调整员工列表
    /// <summary>
    /// 根据过滤条件分页获取所有薪资调整员工列表
    /// </summary>
    /// <returns></returns>
    private void GetStaffGridDataByFilter(String strLimit, String strStart, String strFilterSql)
    {
        try
        {
            int iLimit = Convert.ToInt32(strLimit.Trim());
            int iStart = Convert.ToInt32(strStart.Trim());
            DataSet ds = this.GetAllStaffListDataSet();
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
    
    #region 获取薪资调整员工列表信息数据总数
    /// <summary>
    /// 获取薪资调整员工列表信息数据总数
    /// </summary>
    /// <param name="bRefresh"></param>
    /// <returns></returns>
    private int GetDsTotalCount(bool bRefresh)
    {
        int iCountDs = 0;
        if (bRefresh)
        {
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_PRSLIP_GetStaffList_Count");

            iCountDs = SqlParamDao.ExecuteScalarBySql(strSql); 
            Session["strTotalCount"] = iCountDs.ToString();
        }
        else
        {
            iCountDs = Convert.ToInt32(Session["strTotalCount"]);
        }
        return iCountDs;
    }
    #endregion

    #region 获取薪资调整员工列表所有信息数据,返回DataSet
    /// <summary>
    /// 获取薪资调整员工列表所有信息数据,返回DataSet
    /// </summary>
    /// <returns></returns>
    private DataSet GetAllStaffListDataSet()
    {
        DataSet ds = null;
        if (Session["dsAllStaffList_PRSLIP"] == null)
        {
            String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_PRSLIP_GetStaffList");
            ds = SqlParamDao.GetDataSetBySql(strSql);

            Session["dsAllStaffList_PRSLIP"] = ds;
        }
        else
        {
            ds = (DataSet)Session["dsAllStaffList_PRSLIP"];
        }
        return ds;
    }
    #endregion

    #region 分页查询薪资库员工列表所有记录，返回DataSet记录集
    /// <summary>
    /// 分页查询薪资库员工列表所有记录，返回DataSet记录集
    /// </summary>
    /// <returns>DataSet</returns>
    public DataSet GetAllStaffListMultiPage(int iPageSize, int iStartIndex)
    {
        DataSet ds = new DataSet();
        String strSql = BaseConfig.Instance.GetCommonConfigSqlByKey("strSql_PRSLIP_GetStaffList_MultiPage").Replace("@PAGESIZE", iPageSize.ToString()).Replace("@STARTINDEX", iStartIndex.ToString()) + "";
        ds = SqlParamDao.GetDataSetBySql(strSql);
        return ds;
    }
    #endregion

    #region 即时保存员工薪资项目信息
    /// <summary>
    /// 即时保存员工薪资项目信息
    /// </summary>
    private void SaveAdjustDataRealTime(String strYearMonthDcno, String strResultModifyStr)
    {
        try
        {
            if (!String.IsNullOrEmpty(strYearMonthDcno) && (!String.IsNullOrEmpty(strResultModifyStr)))
            {
                String[] strArr = strResultModifyStr.Split('＠');
                if ((strArr != null) && (strArr.Length == 2))
                {
                    String strColName = strArr[0].ToString();
                    String strValue = strArr[1].ToString();
                    String strSql = "UPDATE PREMPL_2 set ITEMEVALUE = CONVERT(decimal(18,2)," + strValue + ") ";
                    strSql = strSql + " WHERE YEARMONTHDCNO = '" + strYearMonthDcno + "' AND ITEMCODE = '" + strColName + "';";
                    strSql = strSql + "exec ["+this.strSPNAME_CalItemByYearMonthAndDcno+"] '" + strYearMonthDcno + "','" + this.GetUserCode() + "';";
                    strSql = strSql + "exec ["+this.strSPNAME_InitPRSLIPByYearMonthAndDcno+"] '" + strYearMonthDcno + "','" + this.GetUserCode() + "';";
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

    #region 获取薪资项目编码及显示名称的哈希表
    /// <summary>
    /// 获取薪资项目编码及显示名称的哈希表
    /// </summary>
    /// <returns></returns>
    private Hashtable GetPRItemName()
    {
        Hashtable hsTable = new Hashtable();
        String strSql = "select * from pritem_1 where bisstop = '2'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (this.Language.Equals("zh-cn"))
                {
                    hsTable.Add(dr["ITEMCODE"].ToString(), dr["SHOWNAMECHS"].ToString() + dr["ITEMEDIT"].ToString());
                }
                else
                {
                    hsTable.Add(dr["ITEMCODE"].ToString(), dr["SHOWNAME"].ToString() + dr["ITEMEDIT"].ToString());
                }
            }
        }
        return hsTable;
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