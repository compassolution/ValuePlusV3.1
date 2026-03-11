using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.ValuePlus.Common.Security
{
    public class SQLInjectionDefense
    {
        /// <summary>
        /// 替换SQL保留关键字以防止SQL注入风险
        /// 替换后，SQL语句会无效，执行会失败，但是可以阻止恶意目的
        /// </summary>
        /// <param name="strInputString"></param>
        /// <returns></returns>
        public static String ReplaceSQLReservedKeyword(String strInputString)
        {
            ////使用此方法进行Replace可以忽略大小写进行全替代
            //Regex.Replace(strReplaceString, "insert", "", RegexOptions.IgnoreCase)
            if(String.IsNullOrEmpty(strInputString)){
                return "";
            }

            String strReplaceString = strInputString;
            //##插入新增数据表型脚本
            if (strReplaceString.ToLower().IndexOf("insert ") > -1 && strReplaceString.ToLower().IndexOf("into ") > -1)
            {
                strReplaceString = Regex.Replace(strReplaceString, "insert", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "into", "", RegexOptions.IgnoreCase);
            }
            //##查询删除数据表型脚本
            else if (strReplaceString.ToLower().IndexOf("from ") > -1 &&
                (
                    strReplaceString.ToLower().IndexOf("delete ") > -1 
                    || strReplaceString.ToLower().IndexOf("select ") > -1
                ))
            {
                strReplaceString = Regex.Replace(strReplaceString, "from", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "delete", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "select", "", RegexOptions.IgnoreCase);
            }
            //##修改更新数据表型脚本
            else if (strReplaceString.ToLower().IndexOf("update ") > -1 && strReplaceString.ToLower().IndexOf("set ") > -1)
            {
                strReplaceString = Regex.Replace(strReplaceString, "update", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "set", "", RegexOptions.IgnoreCase);
            }
            //##创建修改删除对象型脚本
            else if (
                (strReplaceString.ToLower().IndexOf("create ") > -1 
                    || strReplaceString.ToLower().IndexOf("alter ") > -1
                    || strReplaceString.ToLower().IndexOf("drop ") > -1
                ) && 
                (strReplaceString.ToLower().IndexOf("add ") > -1
                    ||strReplaceString.ToLower().IndexOf("drop ") > -1
                    || strReplaceString.ToLower().IndexOf("alter ") > -1
                    || strReplaceString.ToLower().IndexOf("table ") > -1
                    || strReplaceString.ToLower().IndexOf("view ") > -1
                    || strReplaceString.ToLower().IndexOf("function ") > -1
                    || strReplaceString.ToLower().IndexOf("func ") > -1
                    || strReplaceString.ToLower().IndexOf("procedure ") > -1
                    || strReplaceString.ToLower().IndexOf("pro ") > -1
                ))
            {
                strReplaceString = Regex.Replace(strReplaceString, "create", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "alter", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "drop", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "add", "", RegexOptions.IgnoreCase);
            }
            //##延时执行脚本
            else if (strReplaceString.ToLower().IndexOf("waitfor") > -1 &&
                (
                    strReplaceString.ToLower().IndexOf(" delay") > -1 
                    || strReplaceString.ToLower().IndexOf(" time") > -1)
                )
            {
                strReplaceString = Regex.Replace(strReplaceString, "waitfor", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "delay", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "time", "", RegexOptions.IgnoreCase);
            }
            //##锁定数据表型脚本
            else if (strReplaceString.ToLower().IndexOf("(holdlock)") > -1 || strReplaceString.ToLower().IndexOf("(nolock)") > -1
                || strReplaceString.ToLower().IndexOf("(updlock)") > -1 || strReplaceString.ToLower().IndexOf("(tablock)") > -1
                || strReplaceString.ToLower().IndexOf("(tablockx)") > -1 || strReplaceString.ToLower().IndexOf("(paglock)") > -1 )
            {
                strReplaceString = Regex.Replace(strReplaceString, "lock", "", RegexOptions.IgnoreCase);
            }
            //##备份恢复数据库脚本
            else if (strReplaceString.ToLower().IndexOf("database") > -1 &&
                (
                    strReplaceString.ToLower().IndexOf("backup") > -1
                    || strReplaceString.ToLower().IndexOf("restore") > -1)
                )
            {
                strReplaceString = Regex.Replace(strReplaceString, "database", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "backup", "", RegexOptions.IgnoreCase);
                strReplaceString = Regex.Replace(strReplaceString, "restore", "", RegexOptions.IgnoreCase);
            }
            //##执行脚本或者存储过程脚本
            else if (strReplaceString.ToLower().IndexOf("exec ") > -1 || strReplaceString.ToLower().IndexOf("exec(") > -1)
            {
                strReplaceString = Regex.Replace(strReplaceString, "exec", "", RegexOptions.IgnoreCase);
            }
            //strReplaceString = Regex.Replace(strReplaceString, "sleep(", "", RegexOptions.IgnoreCase);
            return strReplaceString;
        }
    }
}
