using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Form;
using System.Data;

namespace Com.ValuePlus.Flow.BLL.Form
{
    public class FormFieldBll
    {
        #region 查询表TB_FORM_FIELD所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FORM_FIELD所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetAllFromFieldList()
        {
            FormFieldDao dao = new FormFieldDao(); 
            return dao.findAll();
        }
        #endregion

        #region 根据主键查询表TB_FORM_FIELD记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询表TB_FORM_FIELD记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetFromFieldInfoByKey(String strFieldId)
        {
            FormFieldDao dao = new FormFieldDao();
            return dao.findByKey(strFieldId);
        }
        #endregion

        #region 根据表单定义ID查询表TB_FORM_FIELD记录，返回dateset记录集
        /// <summary>
        /// 根据表单定义ID查询表TB_FORM_FIELD记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetFromFieldInfoByFormId(String strFormId)
        {
            FormFieldDao dao = new FormFieldDao();
            return dao.findByFormId(strFormId);
        }
        #endregion

        #region 判断表单表编码是否已经存在
        /// <summary>
        /// 判断表单表编码是否已经存在
        /// </summary>
        /// <param name="strFormCode"></param>
        /// <returns></returns>
        public Boolean IsExsitFieldCode(String strFormId, String strFieldCode)
        {
            Boolean bIsExsit = true;
            FormFieldDao daoForm = new FormFieldDao();
            DataSet ds = daoForm.findByCodeAndFormId(strFormId, strFieldCode);
            if (ds == null)//ds为空
            {
                bIsExsit = false;
            }
            else
            {
                if (ds.Tables.Count == 0)//ds中没有表
                {
                    bIsExsit = false;
                }
                else
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)//ds中的表没有数据
                    {
                        bIsExsit = false;
                    }
                }
            }
            return bIsExsit;
        }
        #endregion

        #region 插入表单字段表一条记录
        /// <summary>
        /// 插入表单字段表一条记录
        /// </summary>
        /// <param name="strSFIELDID"></param>
        /// <param name="strSFORMID"></param>
        /// <param name="strSFIELDCODE"></param>
        /// <param name="strSFIELDNAME"></param>
        /// <param name="strSFIELDNAMECN"></param>
        /// <param name="strBISKEY"></param>
        /// <param name="strSFIELDTYPECODE"></param>
        /// <param name="strNFIELDLENGTH"></param>
        /// <param name="strSFIELDPRECISION"></param>
        /// <param name="strBISNULL"></param>
        /// <param name="strSDEFAULTVALUE"></param>
        /// <param name="strSCTRLTYPECODE"></param>
        /// <param name="strSCTRLDSSQL"></param>
        /// <param name="strNORDER"></param>
        /// <param name="strBISMAINVIEW"></param>
        /// <param name="strNCTRLLENGTH"></param>
        /// <param name="strBISMUST"></param>
        /// <param name="strSTIPDESC"></param>
        /// <param name="strSTIPDESCCN"></param>
        /// <param name="strSCTRLRIGHTTYPECODE"></param>
        /// <param name="strBISFKEY"></param>
        /// <param name="strSFKEYTABLE"></param>
        /// <param name="strSFKEYFIELD"></param>
        /// <returns></returns>
        public int AddFormFieldInfo(String strSFIELDID, String strSFORMID, String strSFIELDCODE, String strSFIELDNAME, String strSFIELDNAMECN, String strBISKEY, String strSFIELDTYPECODE, String strNFIELDLENGTH, String strSFIELDPRECISION, String strBISNULL, String strSDEFAULTVALUE, String strSCTRLTYPECODE, String strSCTRLDSSQL, String strNORDER, String strBISMAINVIEW, String strNCTRLLENGTH, String strBISMUST, String strSTIPDESC, String strSTIPDESCCN, String strSCTRLRIGHTTYPECODE, String strBISFKEY, String strSFKEYTABLE, String strSFKEYFIELD)
        {
            FormFieldDao daoForm = new FormFieldDao();
            return daoForm.insertOneRow(strSFIELDID, strSFORMID, strSFIELDCODE, strSFIELDNAME, strSFIELDNAMECN, strBISKEY, strSFIELDTYPECODE, strNFIELDLENGTH, strSFIELDPRECISION, strBISNULL, strSDEFAULTVALUE, strSCTRLTYPECODE, strSCTRLDSSQL, strNORDER, strBISMAINVIEW, strNCTRLLENGTH, strBISMUST, strSTIPDESC, strSTIPDESCCN, strSCTRLRIGHTTYPECODE, strBISFKEY, strSFKEYTABLE, strSFKEYFIELD);
        }
        #endregion

        #region 根据表单字段表主键删除一条记录
        /// <summary>
        /// 根据表单字段表主键删除一条记录
        /// </summary>
        /// <param name="strFieldId"></param>
        /// <returns></returns>
        public int deleteFormFieldInfo(String strFieldId)
        {
            int iCount = 0;
            FormFieldDao daoForm = new FormFieldDao();
            iCount = daoForm.deleteByKey(strFieldId);
            return iCount;
        }
        #endregion

        #region 根据表单定义表主键更新一条记录
        /// <summary>
        /// 根据表单定义表主键更新一条记录
        /// </summary>
        /// <param name="strSFIELDID"></param>
        /// <param name="strSFORMID"></param>
        /// <param name="strSFIELDCODE"></param>
        /// <param name="strSFIELDNAME"></param>
        /// <param name="strSFIELDNAMECN"></param>
        /// <param name="strBISKEY"></param>
        /// <param name="strSFIELDTYPECODE"></param>
        /// <param name="strNFIELDLENGTH"></param>
        /// <param name="strSFIELDPRECISION"></param>
        /// <param name="strBISNULL"></param>
        /// <param name="strSDEFAULTVALUE"></param>
        /// <param name="strSCTRLTYPECODE"></param>
        /// <param name="strSCTRLDSSQL"></param>
        /// <param name="strNORDER"></param>
        /// <param name="strBISMAINVIEW"></param>
        /// <param name="strNCTRLLENGTH"></param>
        /// <param name="strBISMUST"></param>
        /// <param name="strSTIPDESC"></param>
        /// <param name="strSTIPDESCCN"></param>
        /// <param name="strSCTRLRIGHTTYPECODE"></param>
        /// <param name="strBISFKEY"></param>
        /// <param name="strSFKEYTABLE"></param>
        /// <param name="strSFKEYFIELD"></param>
        /// <returns></returns>
        public int updateFormFieldInfo(String strSFIELDID, String strSFORMID, String strSFIELDCODE, String strSFIELDNAME, String strSFIELDNAMECN, String strBISKEY, String strSFIELDTYPECODE, String strNFIELDLENGTH, String strSFIELDPRECISION, String strBISNULL, String strSDEFAULTVALUE, String strSCTRLTYPECODE, String strSCTRLDSSQL, String strNORDER, String strBISMAINVIEW, String strNCTRLLENGTH, String strBISMUST, String strSTIPDESC, String strSTIPDESCCN, String strSCTRLRIGHTTYPECODE, String strBISFKEY, String strSFKEYTABLE, String strSFKEYFIELD)
        {
            int iCount = 0;
            FormFieldDao daoForm = new FormFieldDao();
            iCount = daoForm.updateByKey(strSFIELDID, strSFORMID, strSFIELDCODE, strSFIELDNAME, strSFIELDNAMECN, strBISKEY, strSFIELDTYPECODE, strNFIELDLENGTH, strSFIELDPRECISION, strBISNULL, strSDEFAULTVALUE, strSCTRLTYPECODE, strSCTRLDSSQL, strNORDER, strBISMAINVIEW, strNCTRLLENGTH, strBISMUST, strSTIPDESC, strSTIPDESCCN, strSCTRLRIGHTTYPECODE, strBISFKEY, strSFKEYTABLE, strSFKEYFIELD);
            return iCount;
        }
        #endregion

        #region 通过数据表名称，获取该表的所有字段
        /// <summary>
        /// 通过数据表名称，获取该表的所有字段
        /// </summary>
        /// <param name="strTableName"></param>
        /// <returns>DataTable</returns>
        public DataTable GetCoulmnInfoFromTable(String strTableName)
        {
            FormFieldDao dao = new FormFieldDao(); 
            return dao.selectCoulmnInfoFromTable(strTableName);
        }
        #endregion
    }
}
