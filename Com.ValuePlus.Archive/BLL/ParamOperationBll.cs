using System;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Config;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    /// <summary>
    /// 与参数替换相关的方法集合
    /// </summary>
    public class ParamOperationBll
    {
        /// <summary>
        /// 将用户对应参数填充到HASHTABLE中
        /// </summary>
        /// <param name="row">每个用户对应的一条记录</param>
        /// <returns>Hashtable</returns>
        public static Hashtable SetUserParamValue(DataRow row)
        {
            Hashtable hsUserParamValue = new Hashtable();
            if (row != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    hsUserParamValue.Add("P" + i.ToString(), row["P" + i.ToString()].ToString());
                }
            }
            return hsUserParamValue;
        }

        /// <summary>
        /// 将某一模板的某一角色定义中的相关参数值填充到HASHTABLE中
        /// </summary>
        /// <param name="row"></param>
        /// <param name="strUserId"></param>
        /// <param name="hsUserParamValue"></param>
        /// <returns>Hashtable</returns>
        public static Hashtable SetRoleParamValue(DataRow row, String strUserId, Hashtable hsUserParamValue)
        {
            Hashtable hsRoleParamValue = new Hashtable();
            if (row != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    String strParamNam = row["RPARA" + i.ToString()].ToString();
                    if (strParamNam == "%USERCODE%")//情况一：用户编码
                    {
                        hsRoleParamValue.Add("P" + i.ToString(), strUserId);
                    }
                    else
                    {
                        if ((strParamNam.Length == 4) && (strParamNam.Substring(0, 2).Equals("%P")))//情况二：%PI%模式
                        {
                            String strITemp = strParamNam.Substring(2,1);

                            if ((hsUserParamValue != null) && (hsUserParamValue.ContainsKey("P" + strITemp)))
                            {
                                hsRoleParamValue.Add("P" + i.ToString(), hsUserParamValue["P" + strITemp].ToString());
                            }
                            else
                            {
                                hsRoleParamValue.Add("P" + i.ToString(), "");
                            }
                        }
                        else//情况三：自定义字符串
                        {
                            hsRoleParamValue.Add("P" + i.ToString(), strParamNam);
                        }
                    }
                }
            }

            return hsRoleParamValue;
        }

        /// <summary>
        /// 将场景表中的SSLCT字段中的相应参数替换成角色定义中的相应参数值(主要针对SQL语句)
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <param name="hsRoleParamValue"></param>
        /// <returns></returns>
        public static string ReplaceSceneSqlParam(string sqlstring, Hashtable hsRoleParamValue)
        {
            if ((hsRoleParamValue != null) && (hsRoleParamValue.Count > 0))
            {
                if (sqlstring.IndexOf("?") > -1)
                {
                    //？所代表的是指角色参数定义中的第一个参数
                    sqlstring = sqlstring.Replace("?", "'" + hsRoleParamValue["P0"].ToString() + "'");
                }
                if (sqlstring.IndexOf("@") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("@P" + i.ToString() + "@", "'" + hsRoleParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
                //******不存在带%的配置参数
                if (sqlstring.IndexOf("%") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("%P" + i.ToString() + "%", "'" + hsRoleParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
            }
            return sqlstring;
        }


        /// <summary>
        /// 将字段明细表中的PCTRLD字段中的相应参数替换成角色定义中的相应参数值(主要针对SQL语句)
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <param name="strMastValue"></param>
        /// <param name="hsRoleParamValue"></param>
        /// <returns></returns>
        public static string ReplacePctrlDSqlParam(string sqlstring,string strMastValue, Hashtable hsRoleParamValue)
        {
            if (sqlstring.IndexOf("?") > -1)
            {
                //？所代表的是指MASTVALUE的值
                sqlstring = sqlstring.Replace("?", "'" + strMastValue + "'");
            }
            if ((hsRoleParamValue != null) && (hsRoleParamValue.Count > 0))
            {
                if (sqlstring.IndexOf("@") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("@P" + i.ToString() + "@", "'" + hsRoleParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
                //******不存在带%的配置参数
                if (sqlstring.IndexOf("%") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("%P" + i.ToString() + "%", "'" + hsRoleParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
            }
            else
            {
                sqlstring = sqlstring.Replace("@", "'").Replace("%", "'");
            }
            return sqlstring;
        }


        /// <summary>
        /// 将字符串参数中相应参数替换成用户参数视图定义中的相应参数值(针对非sql语句字符串)
        /// </summary>
        /// <param name="strParams"></param>
        /// <param name="hsUserParamValue"></param>
        /// <returns></returns>
        public static string ReplaceParamToUserValue(string strParams, Hashtable hsUserParamValue)
        {
            if ((hsUserParamValue != null) && (hsUserParamValue.Count > 0))
            {
                if (strParams.IndexOf("?") > -1)
                {
                    //？所代表的是指用户参数视图定义中的第一个参数
                    strParams = strParams.Replace("?", "'" + hsUserParamValue["P0"].ToString() + "'");
                }
                if (strParams.IndexOf("%") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        strParams = strParams.Replace("%P" + i.ToString() + "%", hsUserParamValue["P" + i.ToString()].ToString());
                    }
                }
            }
            return strParams;
        }


        /// <summary>
        /// 将字符串参数中相应参数替换成角色定义中的相应参数值(针对非sql语句字符串)
        /// </summary>
        /// <param name="strParams"></param>
        /// <param name="hsRoleParamValue"></param>
        /// <returns></returns>
        public static string ReplaceParamToValue(string strParams, Hashtable hsRoleParamValue)
        {
            if ((hsRoleParamValue != null) && (hsRoleParamValue.Count > 0))
            {
                if (strParams.IndexOf("?") > -1)
                {
                    //？所代表的是指角色参数定义中的第一个参数
                    strParams = strParams.Replace("?", "'" + hsRoleParamValue["P0"].ToString() + "'");
                }
                if (strParams.IndexOf("@") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        strParams = strParams.Replace("@P" + i.ToString() + "@", hsRoleParamValue["P" + i.ToString()].ToString());
                    }
                }
                //******不存在带%的配置参数
                if (strParams.IndexOf("%") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        strParams = strParams.Replace("%P" + i.ToString() + "%", hsRoleParamValue["P" + i.ToString()].ToString());
                    }
                }
            }
            return strParams;
        }

        /// <summary>
        /// 将SQL语句中的特殊相应参数替换成相应值（适合sql语句）
        /// </summary>
        /// <param name="sqlstring"></param>
        /// <param name="hsParamValue"></param>
        /// <returns></returns>
        public static string ReplaceSqlSpecialParam(string sqlstring, Hashtable hsParamValue)
        {
            if ((hsParamValue != null) && (hsParamValue.Count > 0))
            {
                if (sqlstring.IndexOf("@") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("@P" + i.ToString() + "@", "'" + hsParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
                //******不存在带%的配置参数
                if (sqlstring.IndexOf("%") > -1)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        sqlstring = sqlstring.Replace("%P" + i.ToString() + "%", "'" + hsParamValue["P" + i.ToString()].ToString() + "'");
                    }
                }
            }
            else
            {
                sqlstring = sqlstring.Replace("@", "'").Replace("%","'");
            }
            return sqlstring;
        }

        /// <summary>
        /// 将ACTION定义中的参数替换成相应值（TB_HRTMPSA中的APARA0.APARA1.APARA2......APARA9）
        /// </summary>
        /// <param name="strActionParam"></param>
        /// <param name="strTID"></param>
        /// <param name="strRID"></param>
        /// <param name="strSID"></param>
        /// <param name="strAID"></param>
        /// <param name="strKeyValue"></param>
        /// <param name="strUserId"></param>
        /// <param name="isHis"></param>
        /// <returns></returns>
        public static string ReplaceActionParam(string strActionParam,string strTID, string strRID, string strSID, string strAID, string strKeyValue, string strUserId, string isHis)
        {
            String strReplace = "";
            switch (strActionParam)
            {
                case "%TID%":
                    strReplace = strTID;
                    break;

                case "%SID%":
                    strReplace = strSID;
                    break;

                case "%AID%":
                    strReplace = strAID;
                    break;

                case "%RID%":
                    strReplace = strRID;
                    break;

                case "%USERCODE%":
                    strReplace = strUserId;
                    break;

                case "%ISHIS%":
                    strReplace = isHis;
                    break;

                case "%KEY%":
                    strReplace = strKeyValue;
                    break;

                case "%KEYS%":
                    strReplace = "%KEYS%";
                    break;

                default:
                    strReplace = strActionParam;
                    break;
            }
            return strReplace;
        }

    }
}
