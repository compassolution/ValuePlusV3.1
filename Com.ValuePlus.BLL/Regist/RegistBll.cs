using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Com.ValuePlus.DAL;
using Com.ValuePlus.DAL.Regist;
using Com.ValuePlus.Entity.Regist;
using Com.ValuePlus.Utils;
using Com.ValuePlus.Utils.Cryptography;
using Com.ValuePlus.Common.Config;
using Com.ValuePlus.Common;

namespace Com.ValuePlus.BLL.Regist
{

    public class RegistBll
    {
        /// <summary>
        /// 日志声明
        /// </summary>
        protected static Com.ValuePlus.Log.ILog log = Com.ValuePlus.Log.LogFactory.CreateInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region 数据表方式注册
        #region 获取表TB_VP_REGIST所有记录,返回dataset
        /// <summary>
        /// 获取表TB_VP_REGIST所有记录,返回dataset
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegistInfo()
        {
            RegistDao daoRegist = new RegistDao();
            return daoRegist.selectRegistInfo();
        }
        #endregion

        #region 判断表TB_VP_REGIST是否存在记录
        /// <summary>
        /// 判断表TB_VP_REGIST是否存在记录
        /// </summary>
        /// <param name="entityRegist"></param>
        /// <returns>int</returns>
        /// <returns>int</returns>
        public bool IsHaveRecord()
        {
            bool bIs = false;
            DataSet ds = this.GetRegistInfo();
            if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
            {
                bIs = true;
            }
            return bIs;
        }
        #endregion

        #region 插入表TB_VP_REGIST记录
        /// <summary>
        /// 插入表TB_VP_REGIST记录
        /// </summary>
        /// <param name="entityRegist"></param>
        /// <returns>int</returns>
        /// <returns>int</returns>
        public int addRegistInfo(RegistInfoEntity entityRegist)
        {
            int iCount = 0;
            if (!this.IsHaveRecord())
            {
                RegistDao daoRegist = new RegistDao();
                if (entityRegist.strSASSIGNSTR.Equals(Encrypt3des(entityRegist.strSREGISTSTR, System.Text.Encoding.UTF8)))
                {
                    iCount = daoRegist.insertOneRow(entityRegist);
                }
                else
                {
                    iCount = -1;//表示输入的授权码无效
                }
            }
            return iCount;
        }
        #endregion

        #region 更新表TB_VP_REGIST记录
        /// <summary>
        /// 更新表TB_VP_REGIST记录
        /// </summary>
        /// <param name="entityRegist"></param>
        /// <returns>int</returns>
        public int modifyRegistInfo(RegistInfoEntity entityRegist)
        {
            int iCount = 0;
            if (!this.IsHaveRecord())
            {
                RegistDao daoRegist = new RegistDao();
                if (entityRegist.strSASSIGNSTR.Equals(Encrypt3des(entityRegist.strSREGISTSTR, System.Text.Encoding.UTF8)))
                {
                    iCount = daoRegist.updateRegistInfo(entityRegist);
                }
                else
                {
                    iCount = -1;//表示输入的授权码无效
                }
            }
            return iCount;
        }
        #endregion

        #region 获取表TB_VP_REGIST当前记录,返回实体RegistInfoEntity
        /// <summary>
        /// 获取表TB_VP_REGIST当前记录,返回实体RegistInfoEntity
        /// </summary>
        /// <returns></returns>
        public RegistInfoEntity GetRegistEntityInfo()
        {
            RegistInfoEntity entityRegist = new RegistInfoEntity();
            DataSet ds = GetRegistInfo();
            if ((ds != null) && (ds.Tables.Count > 0))
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    entityRegist.strSKEY = dr["SKEY"].ToString();
                    entityRegist.strSCLIENTNAME = dr["SCLIENTNAME"].ToString();
                    entityRegist.strSCONTACTOR = dr["SCONTACTOR"].ToString();
                    entityRegist.strSCONTACTWAY = dr["SCONTACTWAY"].ToString();
                    entityRegist.strSGROUPNAME = dr["SGROUPNAME"].ToString();
                    entityRegist.strSREGISTSTR = dr["SREGISTSTR"].ToString();
                    entityRegist.strSASSIGNSTR = dr["SASSIGNSTR"].ToString();
                    entityRegist.dtDTREGISTDATA = (DateTime)dr["DTREGISTDATA"];
                    entityRegist.strSREQUESTIP = dr["SREQUESTIP"].ToString();
                }
            }
            return entityRegist;
        }
        #endregion

        #region 判断是否完成合法授权
        /// <summary>
        /// 判断是否完成合法授权
        /// </summary>
        /// <returns></returns>
        public bool IsHavaAssigned()
        {
            bool bIsHave = true;
            DataSet ds = GetRegistInfo();
            //首先判断数据库是否有注册信息
            if (this.IsHaveRecord())
            {
                DataRow dr = ds.Tables[0].Rows[0];
                String strRegistStr = dr["SREGISTSTR"].ToString();
                String strAssignStr = dr["SASSIGNSTR"].ToString();
                //如果有注册信息，则判断注册码是否匹配
                if (strRegistStr.Equals(this.GetCurSeverMachineCode()))
                {
                    if (!(strAssignStr.Equals(Encrypt3des(this.GetCurSeverMachineCode(), System.Text.Encoding.UTF8))))
                    {
                        bIsHave = false;
                    }
                }
                else
                {
                    bIsHave = false;
                }
            }else{
                bIsHave = false;
            }
            return bIsHave;
        }
        #endregion
        
        #region //3des加密方法
        /// <summary>
        /// 3des加密
        /// </summary>
        /// <param name="strTobeEnCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Encrypt3des(string strTobeEnCrypted, Encoding encoding)
        {
            return CryptographyHelper.Encrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strTobeEnCrypted, encoding);

        }
        #endregion

        #region //3des解密方法
        /// <summary>
        /// 3des解密
        /// </summary>
        /// <param name="strTobeDeCrypted"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string Decrypt3des(string strTobeDeCrypted, Encoding encoding)
        {
            return CryptographyHelper.Decrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strTobeDeCrypted, encoding);
        }
        #endregion

 #endregion
        
        #region 获取当前服务器机器码，由CPU/网卡/硬盘组成
        /// <summary>
        /// 获取当前服务器机器码，由CPU/网卡/硬盘组成
        /// </summary>
        /// <returns></returns>
        public String GetCurSeverMachineCode()
        {
            String strReturn = "";
            try
            {
                HardwareInfo info = new HardwareInfo();
                String strCpuCode = info.GetCpuInfo();
                int iCpuCodeLength = strCpuCode.Length;
                strCpuCode = strCpuCode.Substring(0, 2) + "Value" + strCpuCode.Substring(2, iCpuCodeLength - 4) + "Plus" + strCpuCode.Substring(iCpuCodeLength - 2, 2);
                String strNetCardCode = info.GetNetWorkInfo();
                int iNetCardCodeLength = strNetCardCode.Length;
                strNetCardCode = strNetCardCode.Substring(0, 2) + "Value" + strNetCardCode.Substring(2, iNetCardCodeLength - 4) + "Plus" + strNetCardCode.Substring(iNetCardCodeLength - 2, 2);
                String strHardDiskCode = info.GetHDid();
                int iHardDiskCOdeLength = strHardDiskCode.Length;
                strHardDiskCode = strHardDiskCode.Substring(0, 2) + "Value" + strHardDiskCode.Substring(2, iHardDiskCOdeLength - 4) + "Plus" + strHardDiskCode.Substring(iHardDiskCOdeLength - 2, 2);

                //机器码由三段组成（CPU+网卡+硬盘）
                //String strReturn = (strCpuCode + strNetCardCode + strHardDiskCode).Replace(" ", "");

                //只获取CPU信息进行验证 Modify by Sammen 20210702
                strReturn = (strCpuCode).Replace(" ", "");
            }catch(Exception ex){
                log.Error("获取当前服务器机器码时出错："+ex);
            }
            return strReturn;

        }
        #endregion

        #region license文件方式注册
        /// <summary> 
        /// 获取license文件
        /// </summary>
        public LicenseEntity GetLicenseInfo() 
        {
            LicenseEntity entity = new LicenseEntity();
            String strLicense = "";
            strLicense = License.Instance.GetLicenseString();
            if (!String.IsNullOrEmpty(strLicense))
            {
                //对许可文件进行解密
                strLicense = UrlParamEncryption.Decrypt3des(strLicense, System.Text.Encoding.UTF8);
                strLicense = UrlParamEncryption.Decrypt3des(strLicense, System.Text.Encoding.UTF8);
                strLicense = UrlParamEncryption.Decrypt3des(strLicense, System.Text.Encoding.UTF8);
                strLicense = UrlParamEncryption.Decrypt3des(strLicense, System.Text.Encoding.UTF8);
                strLicense = UrlParamEncryption.Decrypt3des(strLicense, System.Text.Encoding.UTF8);

                String[] strArr = strLicense.Split('＊');
                if (strArr != null)
                {
                    if (strArr.Length == 5)
                    {
                        entity.strClientNameChs = strArr[0].ToString();
                        entity.strClientName = strArr[1].ToString();
                        entity.strProductName = strArr[2].ToString();
                        entity.strVersion = strArr[3].ToString();
                        entity.dtValid = DateTime.Parse(strArr[4].ToString());
                    }
                    else if (strArr.Length == 6)//后来增加了一项机器码
                    {
                        entity.strClientNameChs = strArr[0].ToString();
                        entity.strClientName = strArr[1].ToString();
                        entity.strProductName = strArr[2].ToString();
                        entity.strVersion = strArr[3].ToString();
                        entity.strMachine = strArr[4].ToString();
                        //只获取CPU信息进行验证 Modify by Sammen 20210702
                        if(!String.IsNullOrEmpty(entity.strMachine)){
                            //第一次出现Plus的位置
                            int iFirstPlus = entity.strMachine.IndexOf("Plus");
                            //获取从最开始位置到第一次出现Plus字符串位置后面+6个字符
                            entity.strMachine = entity.strMachine.Substring(0, iFirstPlus + 6);
                        }

                        entity.dtValid = DateTime.Parse(strArr[5].ToString());
                    }
                }
            }
            return entity;

        }
        #endregion


        #region 从Session和Cookie获得或设置软件授权License信息
        /// <summary>
        /// 从Session和Cookie获得或设置软件授权License信息
        /// </summary>
        /// <returns></returns>
        public static LicenseEntity LicenseInfo
        {
            get
            {
                return GetLicenseEntityFromSessionOrCookie;
            }
            set
            {
                GetLicenseEntityFromSessionOrCookie = value;
            }
        }
        #endregion

        #region 从Session和Cookie获得或设置软件授权License是否有效的标记
        /// <summary>
        /// 从Session和Cookie获得或设置软件授权License是否有效的标记
        /// </summary>
        /// <returns></returns>
        public static bool LicenseIsValid
        {
            get 
            {
                return GetLicenseIsValidFromSessionOrCookie;
            }
            set
            {
                GetLicenseIsValidFromSessionOrCookie = value;
            }
        }
        #endregion

        /// <summary>
        /// 将License写入数据库add by sammen 20220610
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="strUserId"></param>
        /// <param name="strProjectId"></param>
        /// <param name="strServerMachineCode"></param>
        /// <returns></returns>
        public bool WriteLicenseInfoToDB(LicenseEntity entity,String strUserId,String strProjectId,String strServerMachineCode)
        {
            bool bIsSuccess = true;
            try
            {                
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append("delete from [TB_LICENSE] where [LICENSECODE] = '" + strProjectId + "'; \r\n");
                sbSql.Append("insert into [TB_LICENSE]([LICENSECODE],[CLIENTNAME],[CLIENTNAMECHS],[PRODUCTORNAME],[VERSION],[BOUNDCODE],[VALIDDATE],[MACHINECODE],[LASTUSERID],[LASTTIME])");
                sbSql.Append("values( ");
                sbSql.Append("'"+ strProjectId + "'");
                sbSql.Append(",'"+ entity .strClientName+ "'");
                sbSql.Append(",'" + entity.strClientNameChs + "'");
                sbSql.Append(",'" + entity.strProductName + "'");
                sbSql.Append(",'" + entity.strVersion + "'");
                sbSql.Append(",'" + entity.strMachine + "'");
                sbSql.Append(",'" + entity.dtValid.ToString("yyyy-MM-dd") + "'");
                sbSql.Append(",'" + strServerMachineCode + "'");
                sbSql.Append(",'" + strUserId + "'");
                sbSql.Append(",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                sbSql.Append(")");

                int iCount = SqlParamDao.ExecuteNonQueryBySql(sbSql.ToString());
                if(iCount<1){
                    bIsSuccess = false;
                }
            }
            catch(Exception ex)
            {
                bIsSuccess = false;
            }
            return bIsSuccess;
        }

        #region 获取或设置License的session和cookie信息
        /// <summary>
        /// 获取或设置License的session和cookie信息
        /// </summary>
        private static LicenseEntity GetLicenseEntityFromSessionOrCookie
        {

            get
            {
                //获取session值
                LicenseEntity entityLicense = Com.ValuePlus.Utils.Session.SessionHelper.GetSession(CacheName.LicenseSessionName) as LicenseEntity;
                if (entityLicense != null && !string.IsNullOrEmpty(entityLicense.strClientName))
                {
                    return entityLicense;
                }
                //获取cookie值
                string sLicenseEntity = Com.ValuePlus.Utils.Cookie.CookieHelper.GetCookie(CacheName.LicenseCookieName);
                if (!string.IsNullOrEmpty(sLicenseEntity))
                {
                    try
                    {
                        //反序列化
                        sLicenseEntity = System.Web.HttpContext.Current.Server.UrlDecode(sLicenseEntity);
                        if (!string.IsNullOrEmpty(sLicenseEntity))
                        {
                            sLicenseEntity = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, sLicenseEntity, System.Text.Encoding.UTF8);
                            entityLicense = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLDeserialize(sLicenseEntity, typeof(LicenseEntity), System.Text.Encoding.UTF8) as LicenseEntity;
                            if (entityLicense != null && !string.IsNullOrEmpty(entityLicense.strClientName))
                            {
                                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LicenseSessionName, entityLicense);
                                return entityLicense;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogFactory.CreateInstance().Error(ex);
                    }
                }
                return null;

            }
            set
            {
                //设置session
                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LicenseSessionName, value);
                if (value != null)
                {
                    //设置cookie
                    string sLicenseEntity0 = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLSerialize(value, System.Text.Encoding.UTF8);
                    sLicenseEntity0 = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Encrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, sLicenseEntity0, System.Text.Encoding.UTF8);
                    sLicenseEntity0 = System.Web.HttpContext.Current.Server.UrlEncode(sLicenseEntity0);

                    int iTimeOut = 0;
                    String strTimeOut = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SessionTimeOut");
                    if (!String.IsNullOrEmpty(strTimeOut))
                    {
                        iTimeOut = int.Parse(strTimeOut);
                    }
                    Com.ValuePlus.Utils.Cookie.CookieHelper.WriteCookie(CacheName.LicenseCookieName, sLicenseEntity0, iTimeOut);
                }

            }
        }
        #endregion

        #region 获取或设置License是否Valid的session和cookie信息
        /// <summary>
        /// 获取或设置License是否Valid的session和cookie信息
        /// </summary>
        private static bool GetLicenseIsValidFromSessionOrCookie
        {

            get
            {
                //获取session值
                String strIsLicenseValid ="";
                if (Com.ValuePlus.Utils.Session.SessionHelper.GetSession(CacheName.LicenseIsValidSessionName)!=null)
                {
                    return bool.Parse(Com.ValuePlus.Utils.Session.SessionHelper.GetSession(CacheName.LicenseIsValidSessionName).ToString().ToLower());
                }
                //获取cookie值
                if (Com.ValuePlus.Utils.Cookie.CookieHelper.GetCookie(CacheName.LicenseIsValidCookieName)!=null)
                {
                    strIsLicenseValid = Com.ValuePlus.Utils.Cookie.CookieHelper.GetCookie(CacheName.LicenseIsValidCookieName).ToString().ToLower();
                    try
                    {
                        //反序列化
                        strIsLicenseValid = System.Web.HttpContext.Current.Server.UrlDecode(strIsLicenseValid);
                        if (!string.IsNullOrEmpty(strIsLicenseValid))
                        {
                            strIsLicenseValid = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strIsLicenseValid, System.Text.Encoding.UTF8);
                            strIsLicenseValid = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLDeserialize(strIsLicenseValid, typeof(LicenseEntity), System.Text.Encoding.UTF8) as string;
                            if (!string.IsNullOrEmpty(strIsLicenseValid))
                            {
                                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LicenseIsValidSessionName, bool.Parse(strIsLicenseValid));
                                return bool.Parse(strIsLicenseValid);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.LogFactory.CreateInstance().Error(ex);
                    }
                }
                return false;

            }
            set
            {
                //设置session
                Com.ValuePlus.Utils.Session.SessionHelper.SetSession(CacheName.LicenseIsValidSessionName, value);
                if (value != null)
                {
                    //设置cookie
                    string strIsLicenseValid0 = Com.ValuePlus.Utils.Serializable.SerializableHelper.XMLSerialize(value, System.Text.Encoding.UTF8);
                    strIsLicenseValid0 = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Encrypt3des(new byte[] { 0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38, 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66, 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2 }, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, strIsLicenseValid0, System.Text.Encoding.UTF8);
                    strIsLicenseValid0 = System.Web.HttpContext.Current.Server.UrlEncode(strIsLicenseValid0);

                    int iTimeOut = 0;
                    String strTimeOut = Com.ValuePlus.SysParams.BaseParamsGetter.GetBasicParamValue("SessionTimeOut");
                    if (!String.IsNullOrEmpty(strTimeOut))
                    {
                        iTimeOut = int.Parse(strTimeOut);
                    }
                    Com.ValuePlus.Utils.Cookie.CookieHelper.WriteCookie(CacheName.LicenseIsValidCookieName, strIsLicenseValid0, iTimeOut);
                }

            }
        }
        #endregion

    }
}
