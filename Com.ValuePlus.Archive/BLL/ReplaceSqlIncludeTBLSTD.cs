using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Entity;
using System.Text.RegularExpressions;
using System;
using System.Data;
using Com.ValuePlus.Archive.DAL;
using Com.ValuePlus.Archive.Property;

namespace Com.ValuePlus.Archive.BLL
{
    public class ReplaceSqlIncludeTBLSTD
    {
        private String strCurTID;
        private String strCurSID;
        private String strCurGID;
        private String strCurLanguage;

        /// <summary>
        /// 将sql语句中涉及字典表的替换成字典表相应字段(Archive)
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public String GetSqlIncludeLSTHDetail(String strSql, String strTID, String strSID, String strGID,String strLanguage)
        {
            this.strCurTID = strTID;
            this.strCurSID = strSID;
            this.strCurGID = strGID;
            this.strCurLanguage = strLanguage;
            String strSqlOutPut = strSql;

            Regex regex = new Regex(@"[0-9a-zA-Z]+\.\*", RegexOptions.Compiled);
            MatchEvaluator evaluator = new MatchEvaluator(RegexReplace);
            if (regex.IsMatch(strSql))
            {
                strSqlOutPut = Regex.Replace(strSql, @"(?<alias>[0-9a-zA-Z])+\.\*", evaluator);
            }
            else
            {
                String strTableName = strTID + "_" + strGID;
                String strReplaced = ReplaceLSTHDetail(strTableName);
                if (!String.IsNullOrEmpty(strReplaced))
                {
                    strSqlOutPut = strSql.Replace("*", strReplaced);
                }
            }

            //strSqlOutPut = strSql.Replace("*", ReplaceLSTHDetail(strTID + "_" + strGID));

            return strSqlOutPut;
        }

        /// <summary>
        /// 将sql语句中涉及字典表的替换成字典表相应字段(Tree)
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        public String GetSqlIncludeLSTHDetail_Tree(String strSql, String strTID, String strLanguage)
        {
            this.strCurTID = strTID;
            this.strCurLanguage = strLanguage;
            String strSqlOutPut = strSql;

            Regex regex = new Regex(@"[0-9a-zA-Z]+\.\*", RegexOptions.Compiled);
            MatchEvaluator evaluator = new MatchEvaluator(RegexReplace);
            if (regex.IsMatch(strSql))
            {
                strSqlOutPut = Regex.Replace(strSql, @"(?<alias>[0-9a-zA-Z])+\.\*", evaluator);
            }
            else
            {
                String strTableName = "TREE_" + strTID;
                String strReplaced = ReplaceLSTHDetail_Tree(strTableName);
                if (!String.IsNullOrEmpty(strReplaced)) 
                {
                    strSqlOutPut = strSql.Replace("*", strReplaced);
                }
            }

            //strSqlOutPut = strSql.Replace("*", ReplaceLSTHDetail(strTID + "_" + strGID));

            return strSqlOutPut;
        }


        private string RegexReplace(Match match)
        {
            return this.ReplaceLSTHDetail(match.Groups[1].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        private string ReplaceLSTHDetail(String strTableAsName)
        {
            //String strTableName = strTID + "_" + strGID;
            string sqlstring = "SELECT PID,PCTRL,PCTRLID,PISKEY,PTYPE,PSAVE,PLEN,PPREC FROM TB_HRTMPSD WHERE PRIGHT<2 AND TID='" + this.strCurTID + "' AND SID='" + this.strCurSID + "' AND GID='" + this.strCurGID + "'  AND PTYPE<>'CH' AND PTYPE<>'CS' ORDER BY PORDER";
            DataTable dt = SqlParamDao.GetDataTableBySql(sqlstring);
            sqlstring = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (i > 0)
                {
                    sqlstring = sqlstring + ",";
                }
                //如果存在加密字段
                String strFiledName = "";
                if ((row["PSAVE"] != null) && (row["PSAVE"].ToString().Equals("1")))
                {
                    switch (row["PTYPE"].ToString().ToLower())
                    {
                        case "int":
                            strFiledName = "[dbo].[Fun_Decode_Int](tableName." + row["PID"].ToString() + ")";
                            break;
                        case "numeric":
                            strFiledName = "[dbo].[Fun_Decode_Decimal](tableName." + row["PID"].ToString() + ")";
                            break;
                        case "datetime":
                        case "date":
                            strFiledName = "[dbo].[Fun_Decode_Datetime](tableName." + row["PID"].ToString() + ")";
                            break;
                        default:
                            strFiledName = "[dbo].[Fun_Decode_String](tableName." + row["PID"].ToString() + ")";
                            break;
                    }
                }
                else
                {
                    strFiledName = "tableName." + row["PID"].ToString();
                }
                if ((row["PCTRL"] != null) && (row["PCTRL"].ToString().Equals("4")))////金额类型可能会存在逗号分开 add by sammen 20140312
                {
                    int iPrec = row["PPREC"] == null ? 2 : int.Parse(row["PPREC"].ToString());///精度(保留小数位)
                    //strFiledName = "(cast(tableName." + row["PID"].ToString() + " as money)) as " + row["PID"].ToString();
                    //////此种方式显示，只能保留2为小数
                    strFiledName = "convert(varchar, cast(" + strFiledName + " as money),1) as " + row["PID"].ToString();
                }
                else
                {
                    strFiledName = strFiledName + " as " + row["PID"].ToString();
                }

                if ((row["PCTRL"].ToString() == "1") && (row["PISKEY"].ToString() != "1"))
                {
                    strFiledName = strFiledName.Replace(" as " + row["PID"].ToString(), "");
                    sqlstring = sqlstring + "(SELECT CDESC";
                    if (this.strCurLanguage.ToLower() == "zh-cn")
                    {
                        sqlstring = sqlstring + "CHS";
                    }
                    TB_HRLSTDProperty propertyHRLSTD = new TB_HRLSTDProperty(row["PCTRLID"].ToString());
                    row["PCTRLID"] = propertyHRLSTD.LID;
                    sqlstring = sqlstring + " FROM " + propertyHRLSTD.TABLENAME + " WHERE LID collate database_default ='" + row["PCTRLID"].ToString() + "' AND CID collate database_default =" + strFiledName.Replace("tableName", strTableAsName) + ") AS '" + row["PID"].ToString() + "'";
                }
                else
                {
                    sqlstring = sqlstring + strFiledName.Replace("tableName", strTableAsName); //strTableAsName + "." + row["PID"].ToString();
                }
            }
            return sqlstring;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <param name="strLanguage"></param>
        /// <returns></returns>
        private string ReplaceLSTHDetail_Tree(String strTableAsName)
        {
            //String strTableName = strTID + "_" + strGID;
            string sqlstring = "SELECT PID,PCTRL,PCTRLID,PISKEY,PTYPE,PLEN,PPREC FROM TB_HRTREED WHERE PRIGHT<2 AND TID='" + this.strCurTID + "'  AND PTYPE<>'CH' AND PTYPE<>'CS' ORDER BY PORDER";
            DataTable dt = SqlParamDao.GetDataTableBySql(sqlstring);
            sqlstring = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (i > 0)
                {
                    sqlstring = sqlstring + ",";
                }
                String strFiledName = "tableName." + row["PID"].ToString();
                if ((row["PCTRL"] != null) && (row["PCTRL"].ToString().Equals("4")))////金额类型可能会存在逗号分开 add by sammen 20140312
                {
                    int iPrec = row["PPREC"] == null ? 2 : int.Parse(row["PPREC"].ToString());///精度(保留小数位)
                    //strFiledName = "(cast(tableName." + row["PID"].ToString() + " as money)) as " + row["PID"].ToString();
                    //////此种方式显示，只能保留2为小数
                    strFiledName = "convert(varchar, cast(" + strFiledName + " as money),1) as " + row["PID"].ToString();
                }
                else
                {
                    strFiledName = strFiledName + " as " + row["PID"].ToString();
                }

                if ((row["PCTRL"].ToString() == "1") && (row["PISKEY"].ToString() != "1"))
                {
                    strFiledName = strFiledName.Replace(" as " + row["PID"].ToString(), "");
                    sqlstring = sqlstring + "(SELECT CDESC";
                    if (this.strCurLanguage.ToLower() == "zh-cn")
                    {
                        sqlstring = sqlstring + "CHS";
                    }
                    TB_HRLSTDProperty propertyHRLSTD = new TB_HRLSTDProperty(row["PCTRLID"].ToString());
                    row["PCTRLID"] = propertyHRLSTD.LID;
                    sqlstring = sqlstring + " FROM " + propertyHRLSTD.TABLENAME + " WHERE LID collate database_default ='" + row["PCTRLID"].ToString() + "' AND CID collate database_default =" + strFiledName.Replace("tableName", strTableAsName) + ") AS '" + row["PID"].ToString() + "'";
                }
                else
                {
                    sqlstring = sqlstring + strFiledName.Replace("tableName", strTableAsName); //strTableAsName + "." + row["PID"].ToString();
                }
            }
            return sqlstring;
        }
    }
}
