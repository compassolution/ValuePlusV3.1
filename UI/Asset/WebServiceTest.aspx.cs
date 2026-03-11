using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.BLL.Regist;
using System.Text;
using System.Data;
using Com.ValuePlus.Utils.Serializable;
using Com.ValuePlus.DAL;

public partial class Asset_WebServiceTest : System.Web.UI.Page
{
    /// <summary>
    /// 日志声明
    /// </summary>
    protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        TestDownLoadAssetInfo();
    }

    public void TestDownLoadAssetInfo()
    {
        try
        {
            StringBuilder sbSql = new StringBuilder();
            String strSql = "select * from VW_AssetDetail_Moblie ORDER BY ACODE";
            DataSet ds = SqlParamDao.GetDataSetBySql(strSql);
            byte[] db = WinCeDataSetHelper.GetZipBytesByDataSet(ds);

            DataSet dsAssets = WinCeDataSetHelper.GetDataSetByZipBytes(db);
            if ((dsAssets != null) && (dsAssets.Tables.Count > 0) && (dsAssets.Tables[0].Rows.Count > 0))
            {
                for (int i = 0; i < dsAssets.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsAssets.Tables[0].Rows[i];
                    String strACode = dr["ACODE"] == null ? "" : dr["ACODE"].ToString().Replace("'", "''");
                    String strAName = dr["ANAME"] == null ? "" : dr["ANAME"].ToString().Replace("'", "''");
                    String strANameChs = dr["ANAMECHS"] == null ? "" : dr["ANAMECHS"].ToString().Replace("'", "''");

                    String strEPCID = dr["EPCID"] == null ? "" : dr["EPCID"].ToString().ToUpper().Replace("'", "''");

                    String strLCode = dr["LCODE"] == null ? "" : dr["LCODE"].ToString().Replace("'", "''");
                    String strLName = dr["LNAME"] == null ? "" : dr["LNAME"].ToString().Replace("'", "''");
                    String strLNameChs = dr["LNAMECHS"] == null ? "" : dr["LNAMECHS"].ToString().Replace("'", "''");

                    String strCCode = dr["CCODE"] == null ? "" : dr["CCODE"].ToString().Replace("'", "''");
                    String strCName = dr["CNAME"] == null ? "" : dr["CNAME"].ToString().Replace("'", "''");
                    String strCNameChs = dr["CNAMECHS"] == null ? "" : dr["CNAMECHS"].ToString().Replace("'", "''");

                    String strSTACode = dr["STACODE"] == null ? "" : dr["STACODE"].ToString().Replace("'", "''");
                    String strSTAName = dr["STANAME"] == null ? "" : dr["STANAME"].ToString().Replace("'", "''");
                    String strSTANameChs = dr["STANAMECHS"] == null ? "" : dr["STANAMECHS"].ToString().Replace("'", "''");

                    sbSql.Append("insert into TB_ASSETS(ACODE,EPCID,ANAME,ANAMECHS,LCODE,LNAME,LNAMECHS,CCODE,CNAME,CNAMECHS,STACODE,STANAME,STANAMECHS) VALUES ");
                    //sbSql.Append("('" + dr["ACODE"] + "','" + dr["EPCID"] + "','" + dr["ANAME"] + "','" + dr["ANAMECHS"] + "',");
                    //sbSql.Append("'" + dr["LCODE"] + "','" + dr["LNAME"] + "','" + dr["LNAMECHS"] + "',");
                    //sbSql.Append("'" + dr["CCODE"] + "','" +  dr["CNAME"] + "','" + dr["CNAMECHS"] + "',");
                    //sbSql.Append("'" + dr["STACODE"] + "','" + dr["STANAME"] + "','" + dr["STANAMECHS"] + "');");

                    sbSql.Append("('" + strACode + "','" + strEPCID + "','" + strAName + "','" + strANameChs + "',");
                    sbSql.Append("'" + strLCode + "','" + strLName + "','" + strLNameChs + "',");
                    sbSql.Append("'" + strCCode + "','" + strCName + "','" + strCNameChs + "',");
                    sbSql.Append("'" + strSTACode + "','" + strSTAName + "','" + strSTANameChs + "');");
                }

            }
            else
            {
            }
            log.Error("插入语句：" + sbSql.ToString());
            int iReturn = ExecuteNonQueryBatch(sbSql.ToString());
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }

    }

    /// <summary>
    /// 批量执行sql语句，返回影响行数
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public int ExecuteNonQueryBatch(string sql)
    {
        int iCount = 0;
        if (sql.IndexOf(';') > 0)
        {
            String[] strSqlArr = sql.Split(';');
            if (strSqlArr.Length > 0)
            {
                for (int i = 0; i < strSqlArr.Length; i++)
                {
                    if (!String.IsNullOrEmpty(strSqlArr[i]))
                    {
                        int result = -1;
                        try
                        {
                            result = SqlParamDao.ExecuteNonQueryBySql(strSqlArr[i]);
                        }
                        catch (DataException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        iCount = iCount + result;
                    }
                }
            }
        }

        return iCount;
    }

}