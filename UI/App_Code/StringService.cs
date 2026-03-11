using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using Com.ValuePlus.DAL;

/// <summary>
///StringService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class StringService : System.Web.Services.WebService
{

    public StringService()
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
    /// 根据MAC地址获取房间号
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public String getRoomNoByMac(String strMacAdd)
    {
        String strRoomNo = "NULL";//字符串null在调用时将进行为空匹配，不可随便修改
        String strSql = "SELECT * FROM PAD_1 WHERE SMAC = '" + strMacAdd + "'";
        DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
        if ((dt != null) && (dt.Rows.Count > 0))
        {
            strRoomNo = dt.Rows[0]["SROOMNO"].ToString();
        }
        return strRoomNo;
    }
}

