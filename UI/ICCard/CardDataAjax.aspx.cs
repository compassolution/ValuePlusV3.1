using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Com.ValuePlus.Web;

public partial class ICCard_CardDataAjax : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    #region 根据员工卡号获取员工信息
    /// <summary>
    /// 根据员工卡号获取员工信息
    /// </summary>
    /// <param name="strCardNo"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public int GetStaffInfoByCardNo(String strCardNo)
    {
        //if (String.IsNullOrEmpty(strAllValue))
        //{
        //    return 0;
        //}
        //String[] strArr1 = strAllValue.Split('*');
        //if (strArr1 == null)
        //{
        //    return 0;
        //}
        int iCount = 0;
        //StringBuilder strBuilderSql = new StringBuilder();
        //for (int i = 0; i < strArr1.Length; i++)
        //{
        //    String strValue1 = strArr1[i].ToString();
        //    int index = strValue1.IndexOf("cbSelect");
        //    strValue1 = strValue1.Substring(index);
        //    String[] strArr2 = strValue1.Split(':');
        //    String strIDString = strArr2[0];
        //    String strPValue = strArr2[1];//是否被选中的值（true/false）
        //    String[] strArr3 = strIDString.Split('_');
        //    String strTid = strArr3[1];//TID
        //    String strGid = strArr3[2];//GID
        //    String strPid = strArr3[3];//字段名
        //    String strCurKey = strArr3[4];//主键名
        //    String strCurKeyValue = strArr3[5];//主键值

        //    String strMainTableName = strTid + "_" + strGid;
        //    String strSql = "update " + strMainTableName + " set " + strPid + "='" + strPValue + "' WHERE " + strCurKey + "='" + strCurKeyValue + "'";
        //    strBuilderSql.Append(strSql + ";\r\n");
        //}
        //if (strBuilderSql != null)
        //{
        //    iCount = SqlParamDao.ExecuteNonQueryBySql(strBuilderSql.ToString());
        //}
        return iCount;
    }
    #endregion

}