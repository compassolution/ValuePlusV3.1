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

public partial class WinForm_Ajax_AdjustResultAjax : Com.ValuePlus.Web.PageBase
{
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
            string strModifyResults = Request["resultStr"] == null ? string.Empty : Request["resultStr"].ToString();//批量编辑排班结果组成的字符串
            string strStuffNo = Request["stuffNo"] == null ? string.Empty : Request["stuffNo"].ToString();//员工编号
            string strDay = Request["day"] == null ? string.Empty : Request["day"].ToString();//考勤日期（仅日）
            string strIsToNormalFlag = Request["toNormalFlag"] == null ? string.Empty : Request["toNormalFlag"].ToString();//是否调整为正常
            this.strFlagIsToNormal = strIsToNormalFlag;

            string strResultModifyStr = Request["resultModifyStr"] == null ? string.Empty : Server.UrlDecode(Request["resultModifyStr"].ToString());//调整结果字符串

            string strStart = Request["start"] == null ? string.Empty : Server.UrlDecode(Request["start"].ToString());//
            string strLimit = Request["limit"] == null ? string.Empty : Server.UrlDecode(Request["limit"].ToString());//

            this.strCurUserId = Request["userId"] == null ? string.Empty : Request["userId"].ToString();//请求操作的用户ID
            if (String.IsNullOrEmpty(this.strCurUserId))
            {
                this.strCurUserId = this.GetUserCode();
            }
               
            if (param != String.Empty)
            {
                switch (param)
                {
                    case "UnNormalResultColumnInfo"://加载员工异常考勤结果显示列
                        this.GetKQStuffResultGridColumn();
                        break;
                    case "UnNormalResultDataInfo"://加载员工异常考勤结果数据信息
                        this.GetKQStuffResultGridData(strYearMonth, this.strFlagIsToNormal, strLimit, strStart);
                        break;
                    case "saveUnNormalResultAdjustInfo"://保存员工异常考勤结果调整信息
                        this.SaveBatchAdjustData(strResultModifyStr);
                        break;
                    case "saveAdjustResultRealTime"://即时保存单个员工考勤结果调整信息add  by sammen 20120618 即时保存
                        string strSeqNo = Request["seqNo"] == null ? string.Empty : Server.UrlDecode(Request["seqNo"].ToString());//表KQRSSZ_2字段SEQNO
                        string strDate = Request["date"] == null ? string.Empty : Server.UrlDecode(Request["date"].ToString());//表KQRSSZ_2字段r_Date
                        this.SaveAdjustDataRealTime(strSeqNo,strDate, strResultModifyStr);
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
    #endregion

    #region 获取员工考勤结果信息表的所有列名
    /// <summary>
    /// 获取员工考勤结果信息表的所有列名
    /// </summary>
    private void GetKQStuffResultGridColumn()
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

    #region 获取员工考勤结果信息表的所有（异常获取调整为正常）数据总数
    private int GetDsTotalCount(String strYearMonth, String strUserId, String strIsToNormalFlag, bool bRefresh)
    {
        int iCountDs = 0;
        if (bRefresh)
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            iCountDs = bllKQPaiBan.GetKQResultCountByEmnoADayAUserId(strYearMonth, strUserId, strIsToNormalFlag);
            Session["strTotalCount"] = iCountDs.ToString();
        }
        else
        {
            iCountDs = Convert.ToInt32(Session["strTotalCount"]);
        }
        return iCountDs;
    }
    #endregion

    #region 获取员工考勤结果信息表的所有（异常获取调整为正常）数据
    /// <summary>
    /// 获取员工考勤结果信息表的所有（异常获取调整为正常）数据
    /// </summary>
    private void GetKQStuffResultGridData(String strYearMonth, String strIsToNormalFlag, String strLimit, String strStart)
    {
        try
        {
            String strUserId = this.strCurUserId;
            int iLimit = Convert.ToInt32(strLimit.Trim());
            int iStart = Convert.ToInt32(strStart.Trim());
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            //int iCountDs = bllKQPaiBan.GetKQResultCountByEmnoADayAUserId(strYearMonth, strUserId,strIsToNormalFlag);
            int iCountDs = 0;
            if (!String.IsNullOrEmpty(strStart))
            {
                if (strStart.Equals("0"))//如果翻页当前起始位置是0则重新加载
                {
                    iCountDs = GetDsTotalCount(strYearMonth, strUserId, strIsToNormalFlag,true);
                }
                else//否则表示是在翻页操作，则读取缓存
                {
                    iCountDs = GetDsTotalCount(strYearMonth, strUserId, strIsToNormalFlag, false);
                }
            }
            DataSet ds = bllKQPaiBan.GetKQResultByEmnoADayAUserId(strYearMonth, strUserId, iLimit, iStart, strIsToNormalFlag);

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
                            String strTemp1 = row[col.ColumnName].ToString();
                            ht.Add(col.ColumnName, strTemp1);
                        }
                        else
                        {
                            ht.Add(col.ColumnName, row[col.ColumnName]);
                        }
                    }
                    hashList.Add(ht);
                }
                string json = "{KQStuffResultTotalPorperty:" + iCountDs.ToString() + ",KQStuffResult:" + JsonConvert.SerializeObject(hashList) + "}";
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
    private void SaveBatchAdjustData(string strModifyResults)
    {
        try
        {
            KQPaiBanBll bllKQPaiBan = new KQPaiBanBll();
            int iCount = 0;
            if (!String.IsNullOrEmpty(strModifyResults))
            {
                iCount = bllKQPaiBan.ModifyBatchKQRSSZ2DataByKey(strModifyResults);
            }
            if (iCount > 0)
            {
                //GetKQPaiBanGridData(strYearMonth);
                ResponseMsg("恭喜你，异常调整成功！");
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
    private void SaveAdjustDataRealTime(String strSeqNo,String strDate, String strResultModifyStr)
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
