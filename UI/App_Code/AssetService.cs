using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Com.ValuePlus.Utils;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.Utils.Serializable;
using Com.ValuePlus.Utils.Cache;
using System.Text;
using System.Collections;

/// <summary>
///WSAsset 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class AssetService : System.Web.Services.WebService
{

    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public AssetService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }


    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetUserInfo()
    {
        //String strSql = "select * from TB_HR_USER where BISSTOP = '0'";
        String strSql = "select SUSERID,SACCOUNTID,dbo.Fun_Decode_Password(SPWD) AS SPWD,SUSERNAME,SUSERNAMECN,SDEPTCODE,SDEPT,SDEPTCN,SPOSI,SPOSICN from TB_HR_USER where BISSTOP = '0'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
        return db;
    }

    /// <summary>
    /// 获取资产存放地点信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetLoactionInfo()
    {
        String strSql = "select * from [VW_Location_Moblie] order by SLCODE";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
        return db;
    }


    /// <summary>
    /// 获取资产基本信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetAssetsInfo()
    {
        String strSql = "select * from VW_AssetDetail_Moblie ORDER BY ACODE";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
        return db;
    }


    /// <summary>
    /// 客户端上传盘点信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public int SetStockInfo(byte[] byteStock)
    {
        DataSet ds = WinCeDataSetHelper.GetDataSetByZipBytes(byteStock);
        StringBuilder sbSql = new StringBuilder();
        int iCount = 0;
        try
        {
            //判断是否存在后来加上的BATCHID字段
            String strTemp = "SELECT count(*) FROM syscolumns a inner join sysobjects b on a.id = b.id where b.name = 'AMORIG_1' and a.name = 'BATCHID'";
            int iTempCount = SqlParamDao.ExecuteScalarBySql(strTemp);

            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                String strBatchId = DateTime.Now.ToString("yyyyMMddHHmmssffff");//批次号
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    String strUpdateTime = DateTime.Parse(dr["STOKETIME"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    sbSql.Append("if exists (select SSEQ FROM AMORIG_1 where SSEQ = '" + dr["SEQ"].ToString() + "')\r\n");
                    sbSql.Append("begin\r\n");
                    sbSql.Append("    UPDATE AMORIG_1 SET ");
                    sbSql.Append("    SEPCID = '" + dr["EPCID"].ToString() + "' ");
                    sbSql.Append("    ,SLCODE = '" + dr["LCODE"].ToString() + "' ");
                    sbSql.Append("    ,DTTIME = '" + strUpdateTime + "' ");
                    sbSql.Append("    ,SUSERID = '" + dr["SUSERID"].ToString() + "'\r\n");
                    sbSql.Append("    where SSEQ = '" + dr["SEQ"].ToString() + "';\r\n");
                    sbSql.Append("end\r\n");
                    sbSql.Append("else begin\r\n");
                    sbSql.Append("    insert into AMORIG_1(SSEQ,SEPCID,SLCODE,DTTIME,SUSERID,ISGET");
                    if (iTempCount == 1)
                    {
                        sbSql.Append(",BATCHID");
                    }
                    sbSql.Append(") VALUES(");
                    sbSql.Append("'" + dr["SEQ"].ToString() + "'");
                    sbSql.Append(",'" + dr["EPCID"].ToString() + "'");
                    sbSql.Append(",'" + dr["LCODE"].ToString() + "'");
                    sbSql.Append(",'" + strUpdateTime + "'");
                    sbSql.Append(",'" + dr["SUSERID"].ToString() + "'");
                    sbSql.Append(",'2'");
                    if (iTempCount == 1)
                    {
                        sbSql.Append(",'" + strBatchId + "'");
                    }
                    sbSql.Append(");\r\n");
                    sbSql.Append("end\r\n");

                }
                if (!String.IsNullOrEmpty(sbSql.ToString()))
                {
                    log.Error("盘点上传的数据脚本\r\n" + sbSql.ToString());
                    iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                    log.Error("扫描枪上传盘点信息，盘点记录数：" + iCount.ToString()+",批次号："+ strBatchId);

                    //上传保存完成后执行后续操作，在存储过程中进行
                    int iReturn = -1;
                    String strSpName = "USP_AM_AMPLAN_AfterUploadData";
                    try
                    {
                        Hashtable hsTableParam = new Hashtable();
                        hsTableParam.Add("BatchId", strBatchId);
                        hsTableParam.Add("SUSERID", ds.Tables[0].Rows[0]["SUSERID"].ToString());
                        if (!String.IsNullOrEmpty(strSpName))
                        {
                            iReturn = SqlParamDao.ExcuteSP(strSpName, hsTableParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.Error("上传保存完成后执行后续操作,执行存储过程失败: SPNAME:" + strSpName);
                        return -1;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            log.Error("客户端上传盘点信息\r\n" + ex);
            log.Error(sbSql.ToString());
            return -1;
        }
        return iCount;
    }


    /// <summary>
    /// 根据EPCID获取资产详细信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetAssetDetailByEpcID(String strEpcID)
    {
        String strSql = "select TOP 1 * from VW_AssetDetail_Moblie where SBARCODE = '" + strEpcID + "'";
        DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
        byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);
        return db;
    }


    /// <summary>
    /// 传递EPCID到服务器端的绑定页面
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public void SendEpcIDToBoundPage(String strEpcID)
    {
        CacheHelper.SetCache("EpcIdFromMoblie",strEpcID);
    }

}

