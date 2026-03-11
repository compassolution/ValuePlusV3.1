using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.Property
{
    public class PMASTProperty
    {
        public string MASTFIELDNAME ="";
        public string TABLENAME = "";
        public string MASTFIELDVALUE = "";

        /// <summary>
        /// 通过主控字段获取其对应的值
        /// </summary>
        /// <param name="strPMAST"></param>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strKeyName"></param>
        /// <param name="strKeyValue"></param>
        public PMASTProperty(string strPMAST,string strTID,string strGID,string strKeyName,string strKeyValue)
        {
            if (!String.IsNullOrEmpty(strPMAST))
            {
                if (strPMAST.IndexOf(';') >= 0)
                {
                    string[] strArray2 = strPMAST.Split(new char[] { ';' });
                    this.TABLENAME = strTID + "_"+strArray2[0];
                    this.MASTFIELDNAME = strArray2[1];

                }
                else
                {
                    this.MASTFIELDNAME = strPMAST;
                    this.TABLENAME = strTID + "_" + strGID;

                }

                String strSql = "select " + this.MASTFIELDNAME + " from " + this.TABLENAME + " where " + strKeyName + " ='" + strKeyValue + "'";
                DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                if ((dt != null) && (dt.Rows.Count > 0))
                {
                    this.MASTFIELDVALUE = dt.Rows[0][0].ToString(); 
                }
            }
        }
    }
}
