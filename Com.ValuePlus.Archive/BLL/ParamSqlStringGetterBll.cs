using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Com.ValuePlus.Archive.Entity;
using Com.ValuePlus.Archive.DAL;
using System.Data;

namespace Com.ValuePlus.Archive.BLL
{
    public class ParamSqlStringGetterBll
    {
        /// <summary>
        /// 获取一条新增到特定表的SQL语句（insert）
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="arrListToDBObject"></param>
        /// <returns></returns>
        public static String GetInsertSqlString(String strTableName, ArrayList arrListToDBObject)
        {
            String strSql = "insert into " + strTableName + " ";
            String strFieldPart = "(";
            String strValuePart = "(";
            for (int i = 0; i < arrListToDBObject.Count; i++)
            {
                Entity_ToDBObject entity = (Entity_ToDBObject)arrListToDBObject[i];
                if (!String.IsNullOrEmpty(entity.FIELDVALUE_NEW))
                {
                    String strValueSingle = entity.FIELDVALUE_NEW.Replace("'", "''");

                    strFieldPart = strFieldPart + "["+entity.FIELDNAME + "],";
                    switch (entity.FIELDTYPE.ToLower())
                    {
                        case "varchar":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strValuePart = strValuePart + " [dbo].[Fun_Encrypt_String]('" + strValueSingle + "'),";
                            }
                            else
                            {
                                strValuePart = strValuePart + "'" + strValueSingle + "',";
                            }
                            break;
                        case "int":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strValuePart = strValuePart + " [dbo].[Fun_Encrypt_Int](" + strValueSingle + "),";
                            }
                            else
                            {
                                strValuePart = strValuePart + " " + strValueSingle + " ,";
                            }
                            break;
                        case "numeric":
                            strValueSingle = strValueSingle.Replace(",", "");//金额类型可能会存在逗号分开 add by sammen 20140312
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strValuePart = strValuePart + " [dbo].[Fun_Encrypt_Decimal](" + strValueSingle + "),";
                            }
                            else
                            {
                                strValuePart = strValuePart + " " + strValueSingle + " ,";
                            }
                            break;
                        case "datetime":
                        case "date":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strValuePart = strValuePart + " [dbo].[Fun_Encrypt_Datetime]('" + strValueSingle + "'),";
                            }
                            else
                            {
                                strValuePart = strValuePart + " '" + strValueSingle + "' ,";
                            }
                            break;
                        default:
                            strValuePart = strValuePart + "'" + strValueSingle + "',";
                            break;
                    }


                }

            }
            if (strFieldPart.EndsWith(","))
            {
                strFieldPart = strFieldPart.Substring(0, strFieldPart.Length - 1) + ")";
            }
            if (strValuePart.EndsWith(","))
            {
                strValuePart = strValuePart.Substring(0, strValuePart.Length - 1) + ")";
            }
            if ((strFieldPart.Equals("(")) || (strValuePart.Equals("(")))//如果没有一个字段有值
            {
                strSql = "";
            }
            else
            {
                strSql = strSql + strFieldPart + " values " + strValuePart;
            }
            return strSql;
        }

        /// <summary>
        /// 获取一条更新到特定表的SQL语句（update）
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="arrListToDBObject"></param>
        /// <param name="hsTableKey"></param>
        /// <returns></returns>
        public static String GetUpdateSqlString(String strTableName, ArrayList arrListToDBObject,Hashtable hsTableKey)
        {
            String strSql = "update " + strTableName + " set ";
            String strFieldPart = "";
            String strConditionPart = "";
            for (int i = 0; i < arrListToDBObject.Count; i++)
            {
                Entity_ToDBObject entity = (Entity_ToDBObject)arrListToDBObject[i];
                if (String.IsNullOrEmpty(entity.FIELDVALUE_NEW))
                {
                    strFieldPart = strFieldPart + "["+entity.FIELDNAME + "]=null,";
                }
                else
                {
                    String strValueSingle =  entity.FIELDVALUE_NEW.Replace("'", "''");
                    switch (entity.FIELDTYPE)
                    {
                        case "varchar":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]= [dbo].[Fun_Encrypt_String]('" + strValueSingle + "'),";
                            }
                            else
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]='" + strValueSingle + "',";
                            }
                            break;
                        case "int":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]= [dbo].[Fun_Encrypt_Int](" + strValueSingle + "),";
                            }
                            else
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]=" + strValueSingle + ",";
                            }
                            break;
                        case "numeric":
                            strValueSingle = strValueSingle.Replace(",", "");//金额类型可能会存在逗号分开 add by sammen 20140312
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]= [dbo].[Fun_Encrypt_Decimal](" + strValueSingle + "),";
                            }
                            else
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]=" + strValueSingle + ",";
                            }
                            break;
                        case "datetime":
                        case "date":
                            //判断是否加密字段
                            if ((!String.IsNullOrEmpty(entity.PSAVE) && (entity.PSAVE.Equals("1"))))
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]= [dbo].[Fun_Encrypt_Datetime]('" + strValueSingle + "'),";
                            }
                            else
                            {
                                strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]='" + strValueSingle + "',";
                            }
                            break;
                        default:
                            strFieldPart = strFieldPart + "[" + entity.FIELDNAME + "]='" + strValueSingle + "',";
                            break;
                    }
                }

            }
            if (strFieldPart.EndsWith(","))
            {
                strFieldPart = strFieldPart.Substring(0, strFieldPart.Length - 1);
            }
            if ((hsTableKey != null) && (hsTableKey.Count > 0))
            {
                foreach (System.Collections.DictionaryEntry item in hsTableKey)
                {
                    String strKey = item.Key.ToString();//键
                    String strValue = item.Value.ToString();//值
                    if (!String.IsNullOrEmpty(strKey))
                    {
                        strConditionPart = strConditionPart + " "+strKey + "='" + strValue + "' AND";
                    }
                }

            }
            if (strConditionPart.EndsWith("AND"))
            {
                strConditionPart = strConditionPart.Substring(0, strConditionPart.Length - 3);
                strSql = strSql + strFieldPart + " where " + strConditionPart;
            }
            return strSql;
        }


        /// <summary>
        /// 根据模板ID及场景分组获取对应字段的查询字段字符串
        /// 其中如果存在加密字段则自动拼写界面函数
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strSID"></param>
        /// <param name="strGID"></param>
        /// <returns></returns>
        public static String GetSelectFieldString(String strTID,String strSID,String strGID)
        {
            String strSelectField = " * ";
            String strSql = "select * from TB_HRTMPSD WHERE TID = '"+strTID+"' AND SID = '"+strSID+"' AND GID = '"+strGID+"' and PTYPE not in ('CS','CH') and PRIGHT <>2  ORDER BY PORDER ";
            DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
            if ((dt != null) && (dt.Rows.Count > 0))
            {
                DataRow[] drs = dt.Select("PSAVE='1'");//如果存在需加密字段
                if (drs.Length > 0)
                {
                    strSelectField = "";
                    String strFiledName = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        //如果存在加密字段
                        if ((dr["PSAVE"]!=null)&&(dr["PSAVE"].ToString().Equals("1")))
                        {
                            switch (dr["PTYPE"].ToString().ToLower())
                            {
                                case "int":
                                    strFiledName = "[dbo].[Fun_Decode_Int](" + dr["PID"].ToString() + ") as " + dr["PID"].ToString() ;
                                    break;
                                case "numeric":
                                    strFiledName = "[dbo].[Fun_Decode_Decimal](" + dr["PID"].ToString() + ") as "+ dr["PID"].ToString();
                                    break;
                                case "datetime":
                                case "date":
                                    strFiledName = "[dbo].[Fun_Decode_Datetime](" + dr["PID"].ToString() + ") as "+ dr["PID"].ToString();
                                    break;
                                default:
                                    strFiledName = "[dbo].[Fun_Decode_String](" + dr["PID"].ToString() + ") as " + dr["PID"].ToString();
                                    break;
                            }
                        }else
                        {
                            strFiledName = dr["PID"].ToString();
                        }

                        if (i == 0)
                        {
                            strSelectField = strFiledName;
                        }
                        else
                        {
                            strSelectField = strSelectField+","+strFiledName;
                        }

                    }
                }
            }

            return strSelectField;
        }
    }
}
