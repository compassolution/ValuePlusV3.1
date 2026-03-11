using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Archive.Entity;
using System.Data;
using Com.ValuePlus.Archive.DAL;

namespace Com.ValuePlus.Archive.BLL
{
    public class AutoIncreaseFiledBll
    {
        #region 获取客户端自增字段的新值
        /// <summary>
        /// 获取客户端自增字段的新值
        /// </summary>
        /// <param name="entityHRTMPD"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static int GetClientAutoFiledNo(Entity_TB_HRTMPD entityHRTMPD,String strKey,String strKeyValue)
        {
            String strTID = entityHRTMPD.TID;
            String strGID = entityHRTMPD.GID;
            String strPID = entityHRTMPD.PID;
            String strPTYPE = entityHRTMPD.PTYPE;
            return GetClientAutoFiledNo(strTID, strGID, strPID, strPTYPE, strKey, strKeyValue);
        }
        /// <summary>
        /// 获取客户端自增字段的新值
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static int GetClientAutoFiledNo(Entity_TB_HRTMPSD entityHRTMPSD,String strKey,String strKeyValue)
        {
            String strTID = entityHRTMPSD.TID;
            String strGID = entityHRTMPSD.GID;
            String strPID = entityHRTMPSD.PID;
            String strPTYPE = entityHRTMPSD.PTYPE;
            return GetClientAutoFiledNo(strTID, strGID, strPID, strPTYPE, strKey, strKeyValue);
        }
        /// <summary>
        /// 获取客户端自增字段的新值
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strPTYPE"></param>
        /// <param name="strKey"></param>
        /// <param name="strKeyValue"></param>
        /// <returns></returns>
        public static int GetClientAutoFiledNo(String strTID, String strGID, String strPID,String strPTYPE,String strKey,String strKeyValue)
        {
            int iNo= 0;
            if ((!String.IsNullOrEmpty(strTID)) && (!String.IsNullOrEmpty(strGID)) && (!String.IsNullOrEmpty(strPID)) && (!String.IsNullOrEmpty(strKey)) && (!String.IsNullOrEmpty(strKeyValue)))
            {
                if (String.IsNullOrEmpty(strPTYPE))
                {
                    String strSqlTemp = "select PTYPE FROM TB_HRTMPD WHERE TID = '" + strTID + "' AND GID = '" + strGID + "' AND PID = '" + strPID + "'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSqlTemp);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        strPTYPE = dt.Rows[0]["PTYPE"].ToString();
                    }
                }
                //客户端自增字段
                if (strPTYPE.ToLower().Equals("intc"))
                {
                    String strTableName = strTID + "_" + strGID;
                    String strSqlRecord = "select " + strPID + " from " + strTableName + " where " + strKey + "='" + strKeyValue + "' order by " + strPID;
                    String strSql = "select Max(" + strPID + ") from " + strTableName + " where " + strKey + "='" + strKeyValue + "'";
                    DataTable dtRecord = SqlParamDao.GetDataTableBySql(strSqlRecord);
                    if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
                    {
                        Object obj = SqlParamDao.ExecuteScalarBySql(strSql);
                        if (obj != null)
                        {
                            iNo = Convert.ToInt32(obj) + 1;
                        }
                    }
                    else
                    {
                        iNo = 1;
                    }
                }
            }
            return iNo;
        }
        #endregion


        #region 获取服务器端自增字段的新值
        /// <summary>
        /// 获取服务器端自增字段的新值
        /// </summary>
        /// <param name="entityHRTMPD"></param>
        /// <returns></returns>
        public static String GetServerAutoFiledNo(Entity_TB_HRTMPD entityHRTMPD)
        {
            String strTID = entityHRTMPD.TID;
            String strGID = entityHRTMPD.GID;
            String strPID = entityHRTMPD.PID;
            String strPCTRLD = entityHRTMPD.PCTRLD;
            return GetServerAutoFiledNo(strTID, strGID, strPID, strPCTRLD);
        }
        /// <summary>
        /// 获取服务器端自增字段的新值
        /// </summary>
        /// <param name="entityHRTMPSD"></param>
        /// <returns></returns>
        public static String GetServerAutoFiledNo(Entity_TB_HRTMPSD entityHRTMPSD)
        {
            String strTID = entityHRTMPSD.TID;
            String strGID = entityHRTMPSD.GID;
            String strPID = entityHRTMPSD.PID;
            String strPCTRLD = entityHRTMPSD.PCTRLD;
            return GetServerAutoFiledNo(strTID, strGID, strPID, strPCTRLD);
        }
        /// <summary>
        /// 获取服务器端自增字段的新值
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strPCTRLD"></param>
        /// <returns></returns>
        public static String GetServerAutoFiledNo(String strTID, String strGID, String strPID,String strPCTRLD)
        {
            String strNo = "";
            if ((!String.IsNullOrEmpty(strTID)) && (!String.IsNullOrEmpty(strGID)) && (!String.IsNullOrEmpty(strPID)))
            {
                if (String.IsNullOrEmpty(strPCTRLD))
                {
                    String strSqlTemp = "select PCTRLD FROM TB_HRTMPD WHERE TID = '" + strTID + "' AND GID = '" + strGID + "' AND PID = '" + strPID + "'";
                    DataTable dt = SqlParamDao.GetDataTableBySql(strSqlTemp);
                    if ((dt != null) && (dt.Rows.Count > 0))
                    {
                        strPCTRLD = dt.Rows[0]["PCTRLD"].ToString();
                    }
                }
                strNo = GetServerAutoFiledNo(strPCTRLD);
            }
            return strNo;
        }

        /// <summary>
        /// 获取服务器端自增字段的新值
        /// </summary>
        /// <param name="strTID"></param>
        /// <param name="strGID"></param>
        /// <param name="strPID"></param>
        /// <param name="strPCTRLD"></param>
        /// <returns></returns>
        public static String GetServerAutoFiledNo(String strAID)
        {
            String strNo = "";
            String strSql = "select * from TB_HRAUTO where AID ='" + strAID + "'";
            DataTable dtRecord = SqlParamDao.GetDataTableBySql(strSql);
            if ((dtRecord != null) && (dtRecord.Rows.Count > 0))
            {
                DataRow dr = dtRecord.Rows[0];
                String strPreFix = dr["APREFIX"] == null ? "" : dr["APREFIX"].ToString();
                String strDateFormat = dr["ADATE"] == null ? "" : dr["ADATE"].ToString();
                int iLength = dr["ALENGTH"] == null ? 0 : int.Parse(dr["ALENGTH"].ToString());
                int iNextNo = dr["ANEXTNO"] == null ? 0 : int.Parse(dr["ANEXTNO"].ToString());
                String strLastDate = dr["ALASTDATE"] == null ? "" : dr["ALASTDATE"].ToString();

                if (String.IsNullOrEmpty(strDateFormat))//不用根据时间生成
                {
                    strNo = strPreFix + getNextNoByLength(iLength, iNextNo);
                    SqlParamDao.ExecuteNonQueryBySql("UPDATE TB_HRAUTO SET ANEXTNO=ANEXTNO+1 WHERE AID='" + strAID + "'");

                }
                else//需要根据时间生成
                {
                    String strDate = getDate(strDateFormat);
                    if (strDate.Equals(strLastDate))//同一月份
                    {
                        strNo = strPreFix + strDate + getNextNoByLength(iLength, iNextNo);
                        SqlParamDao.ExecuteNonQueryBySql("UPDATE TB_HRAUTO SET ANEXTNO=ANEXTNO+1 WHERE AID='" + strAID + "'");
                    }
                    else//不同月份
                    {
                        strNo = strPreFix + strDate + getNextNoByLength(iLength, 1);
                        SqlParamDao.ExecuteNonQueryBySql("UPDATE TB_HRAUTO SET ANEXTNO=2 ,ALASTDATE='" + strDate + "' WHERE AID='" + strAID + "'");
                    }
                
                }
            }
            else
            {
                //特殊处理(默认三位随机数)
                DateTime dtNowTime = DateTime.Now;
                strNo = string.Format("{0}{1}{2}{3}", dtNowTime.Year.ToString(),RandomNum(3),dtNowTime.Millisecond.ToString().PadLeft(3, '0'),dtNowTime.Second.ToString().PadLeft(2, '0'));
            }
            return strNo;
        }

        #endregion

        #region 生成随机数
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="n">随机数位数</param>
        /// <returns>随机数</returns>
        public static string RandomNum(int n) //
        {
            string strchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = strchar.Split(',');
            string VNum = "";
            Random rand = new Random();
            for (int i = 1; i < n + 1; i++)
            {
                int t = rand.Next(10);
                VNum += VcArray[t];
            }
            return VNum;//返回生成的随机数
        }
        /// <summary>
        /// 根据数字及需要的长度，拼成字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private static string getNextNoByLength(int len, int num)
        {
            string str = "";
            if (len == 0)
            {
                str = num.ToString();
            }
            else
            {
                for (int i = 0; i < (len - num.ToString().Length); i++)
                {
                    str = str + "0";
                }
                str = (str + num.ToString());
            }
            return str;
        }

        /// <summary>
        /// 获取指定格式时间字符串
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string getDate(string format)
        {
            string str = format.Replace("Y", "y").Replace("D", "d");
            return DateTime.Now.ToString(str);
        }

        #endregion
    }
}
