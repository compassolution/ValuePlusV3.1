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

public partial class WinForm_Ajax_AdjustStaffPayAjax : Com.ValuePlus.Web.PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
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

            string strStart = Request["start"] == null ? string.Empty : Server.UrlDecode(Request["start"].ToString());//
            string strLimit = Request["limit"] == null ? string.Empty : Server.UrlDecode(Request["limit"].ToString());//

            string strResultModifyStr = Request["staffListModifyStr"] == null ? string.Empty : Server.UrlDecode(Request["staffListModifyStr"].ToString());//调整结果字符串

            if (param != String.Empty)
            {
                switch (param)
                {
                    case "AdjustStaffListColumnInfo"://加载薪资调整员工列表显示列
                        this.GetAdjustStaffListColumn();
                        break;
                    case "MultiPageStaffListDataInfo"://加载薪资调整员工列表数据信息
                        this.GetAdjustStaffGridData(strLimit, strStart);
                        break;
                    case "FilterStaffListDataInfo"://根据过滤条件分页获取员工列表数据信息
                        this.GetStaffGridDataByFilter(strLimit, strStart, strFilterSql);
                        break;
                    case "StaffPayItemListColumnInfo"://加载薪资调整薪资项目列表显示列
                        this.GetStaffPayItemListColumn();
                        break;
                    case "StaffPayItemListDataInfo"://加载薪资调整薪资项目列表数据信息
                        this.GetStaffPayItemListGridData(strYearMonthDcno);
                        break;
                    case "saveAdjustPayValueInfo"://保存员工异常考勤结果调整信息
                        this.SaveBatchAdjustData(strYearMonthDcno,strResultModifyStr);
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
    private IList<Hashtable> hashColListPayItemList
    {
        get
        {
            return ViewState["hashColListPayItemList"] as IList<Hashtable>;
        }
        set
        {
            ViewState["hashColListPayItemList"] = value;
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
                AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
                DataSet ds = bllAdjustStaffPay.GetStaffListCol();

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
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
            iCountDs = bllAdjustStaffPay.GetStaffListCount();
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
        if (Session["dsAllStaffList"]==null)
        {
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
            ds = bllAdjustStaffPay.GetAllStaffList();
            Session["dsAllStaffList"] = ds;
        }
        else
        {
            ds = (DataSet)Session["dsAllStaffList"];
        }
        return ds;
    }
    #endregion

    #region 根据过滤条件分页获取所有薪资调整员工列表
    /// <summary>
    /// 根据过滤条件分页获取所有薪资调整员工列表
    /// </summary>
    /// <returns></returns>
    private void GetStaffGridDataByFilter(String strLimit, String strStart,String strFilterSql)
    {
        try
        {
            int iLimit = Convert.ToInt32(strLimit.Trim());
            int iStart = Convert.ToInt32(strStart.Trim());
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
            DataSet ds = this.GetAllStaffListDataSet();
            DataSet dsView = new DataSet();

            if (!String.IsNullOrEmpty(strFilterSql))
            {
                ds.Tables[0].DefaultView.RowFilter = strFilterSql;

                //设置全局dataset
                DataView dv = ds.Tables[0].DefaultView;
                System.Data.DataTable dt = dv.ToTable();
                dsView.Tables.Add(dt.Copy());
            }else{
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
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
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
            DataSet ds = bllAdjustStaffPay.GetAllStaffListMultiPage(iLimit, iStart);

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

    #region 获取薪资调整薪资项目列表的所有列名
    /// <summary>
    /// 获取薪资调整薪资项目列表的所有列名
    /// </summary>
    private void GetStaffPayItemListColumn()
    {
        try
        {
            if (this.hashColListPayItemList == null)
            {
                ResourceManager rmLocResourceManager = base.GetResourceManager("FrmStaffPay");
                AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
                DataSet ds = bllAdjustStaffPay.GetPayItemListCol();

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

    #region 获取薪资调整薪资项目列表信息数据
    /// <summary>
    /// 获取薪资调整薪资项目列表信息数据
    /// </summary>
    private void GetStaffPayItemListGridData(String strYearMonthDcno)
    {
        try
        {
            //int iLimit = Convert.ToInt32(strLimit.Trim());
            //int iStart = Convert.ToInt32(strStart.Trim());
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
            //int iCountDs = 0;
            //if (!String.IsNullOrEmpty(strStart))
            //{
            //    if (strStart.Equals("0"))//如果翻页当前起始位置是0则重新加载
            //    {
            //        iCountDs = GetDsTotalCount(true);
            //    }
            //    else//否则表示是在翻页操作，则读取缓存
            //    {
            //        iCountDs = GetDsTotalCount(false);
            //    }
            //}
            DataSet ds = bllAdjustStaffPay.GetPayItemListByStaffNo(strYearMonthDcno);

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
                string json = "{StaffPayItemListTotalPorperty:" + rows.ToString() + ",StaffPayItemList:" + JsonConvert.SerializeObject(hashList) + "}";
                Response.Write(json);
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }
    #endregion

    #region 批量保存异常考勤调整信息的修改
    /// <summary>
    /// 批量保存异常考勤调整信息的修改
    /// </summary>
    private void SaveBatchAdjustData(String strYearMonthDcno,string strModifyResults)
    {
        try
        {
            AdjustStaffPayBll bllAdjustStaffPay = new AdjustStaffPayBll();
            int iCount = 0;
            if (!String.IsNullOrEmpty(strModifyResults))
            {
                iCount = bllAdjustStaffPay.SaveStaffPayValueBatch(strModifyResults);

                //修改成功后同时执行计算公式
                if ((!String.IsNullOrEmpty(this.strSPNAME_CalItemByYearMonthAndDcno))&&(!String.IsNullOrEmpty(this.strSPNAME_InitPRSLIPByYearMonthAndDcno)))
                {
                    String strSql = "exec [" + this.strSPNAME_CalItemByYearMonthAndDcno + "] '" + strYearMonthDcno + "','" + this.GetUserCode() + "';";
                    strSql = strSql + "exec [" + this.strSPNAME_InitPRSLIPByYearMonthAndDcno + "] '" + strYearMonthDcno + "','" + this.GetUserCode() + "';";
                    iCount = SqlParamDao.ExecuteNonQueryBySql(strSql);
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
