using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Archive.DAL;
using System.Data;

namespace Com.ValuePlus.Archive.Property
{
    public class TB_HRLSTDProperty
    {
        public string LID;
        public string TABLENAME = "TB_HRLSTD";
        public string VERSION = "1";
        public string TYPE = "TABLE";
        public string ORDER = "CID";

        /// <summary>
        /// 通过表TB_HRTMPD的字段PCTRILID获取其对应的LID
        /// 其中可以通过视图配置，而视图中的字段定义需与TB_HRLSTD表中的LID/CID/CDESC/CDESCCHS/CUID一直
        /// 视图配置的格式为：@viewname:LID(P9字段为排序规则字段)
        /// </summary>
        /// <param name="strPCTRLID"></param>
        public TB_HRLSTDProperty(string strPCTRLID)
        {
            if (strPCTRLID.Length > 0)
            {
                if (strPCTRLID.Substring(0, 1).Equals("@"))
                {
                    this.VERSION = "2";
                    strPCTRLID = strPCTRLID.Replace("@", "").ToUpper();
                    string[] strArray = strPCTRLID.Split(new char[] { ';' });
                    this.TABLENAME = strArray[0];
                    this.LID = strArray[1];
                    this.TYPE = "VIEW";
                    String strSql = "select * from syscolumns A,sys.views B where A.ID = B.OBJECT_ID AND A.name = 'P9' AND B.NAME = '" + this.TABLENAME + "'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSql);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        this.ORDER = "P9";
                    }
                    else
                    {
                        this.ORDER = "CID";
                    }
                }
                else
                {
                    this.VERSION = "1";
                    this.TABLENAME = "TB_HRLSTD";
                    this.LID = strPCTRLID;
                    this.TYPE = "TABLE";
                    this.ORDER = "CID";
                }
            }
            else
            {
                this.VERSION = "1";
                this.TABLENAME = "TB_HRLSTD";
                this.LID = strPCTRLID;
                this.TYPE = "TABLE";
                this.ORDER = "CID";
            }
        }
    }
}
