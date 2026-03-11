<%@ WebHandler Language="C#" Class="NewsPlateTreeHandler" %>

using System;
using System.Web;
using System.Data;
using Com.ValuePlus.BLL.User;
using Com.ValuePlus.DAL;

public class NewsPlateTreeHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState {

    #region 日志声明
    /// <summary>
    /// 日志声明
    /// </summary>
    private Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #endregion

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/xml";
        try
        {
            context.Response.Write(this.GetMenuTreeAll(UserLoginBll.Language));
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        context.Response.Flush();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }


    #region 获得所有栏目树
    /// <summary>
    /// 获得所有栏目树
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    private string GetMenuTreeAll(string language)
    {
        string sResult = string.Empty;
        String strSql = "SELECT * FROM TB_NEWS_PLATE order by SLEVEL, sorder";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if (dt != null && dt.Rows.Count > 0)
        {
            CreateTreeXmlStructure(dt, language, ref sResult);
        }
        return sResult;
    }
    #endregion

    #region 创建树结构
    /// <summary>
    /// 创建树结构
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="language"></param>
    /// <param name="sResult"></param>
    private void CreateTreeXmlStructure(DataTable dt, string language, ref string sResult)
    {
        sResult = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><tree id=\"0\">";
        //首先创建根节点
        DataRow[] drs = dt.Select("SPARENTID is null or SPARENTID = ''");
        if (drs != null && drs.Length > 0)
        {
            string sTitle = drs[0]["SORDER"].ToString()+"、"+drs[0]["SPLATENAME"].ToString() + "【" + drs[0]["SPLATEID"].ToString() + "】";
            if (language == "zh-cn")
            {
                sTitle = drs[0]["SORDER"].ToString() + "、" + drs[0]["SPLATENAMECHS"].ToString() + "【" + drs[0]["SPLATEID"].ToString() + "】";
            }
            sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
            sResult = sResult + "<item id=\"" + drs[0]["SPLATEID"].ToString() + "\" open=\"true\" text=\"" + sTitle + "\" tooltip=\"" + sTitle + "\" >";
            
            sResult = sResult + "<userdata name=\"url\">";
            sResult = sResult + "NewsList.aspx?plateId=" + drs[0]["SPLATEID"].ToString();
            sResult = sResult + "</userdata>";

            //创建子节点
            CreateSubTreeXmlStructure(dt, drs[0]["SPLATEID"].ToString(), language, ref sResult);
            sResult += "</item>";
        }
        sResult += "</tree>";
    }
    #endregion

    #region 创建树子节点结构
    /// <summary>
    /// 创建树子节点结构
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="id"></param>
    /// <param name="language"></param>
    /// <param name="sResult"></param>
    private void CreateSubTreeXmlStructure(DataTable dt, string id, string language, ref string sResult)
    {
        DataRow[] drs = dt.Select("SPARENTID = '" + id + "'", "SORDER ASC");
        if (drs != null && drs.Length > 0)
        {
            foreach (DataRow dr in drs)
            {
                string sTitle = dr["SORDER"].ToString() + "、" + dr["SPLATENAME"].ToString() + "【" + dr["SPLATEID"].ToString() + "】";
                if (language == "zh-cn")
                {
                    sTitle = dr["SORDER"].ToString() + "、" + dr["SPLATENAMECHS"].ToString() + "【" + dr["SPLATEID"].ToString() + "】";
                }
                sTitle = Com.ValuePlus.Utils.HtmlUtils.HtmlEncode(sTitle);
                
                sResult = sResult + "<item id=\"sub" + dr["SPLATEID"].ToString() + "\" text=\"" + sTitle + "\"  tooltip=\"" + sTitle + "\" >";
                
                sResult = sResult + "<userdata name=\"url\">";
                sResult = sResult + "NewsList.aspx?plateId=" + dr["SPLATEID"].ToString();
                sResult = sResult + "</userdata>";

                //创建子节点
                CreateSubTreeXmlStructure(dt, dr["SPLATEID"].ToString(), language, ref sResult);
                sResult += "</item>";
            }
        }

    }
    #endregion

}