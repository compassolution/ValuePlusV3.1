using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Form;
using System.Data;

namespace Com.ValuePlus.Flow.BLL.Form
{
    public class FormDefineBll
    {
        #region 查询表TB_FORM_DEFINE所有记录，返回dateset记录集
        /// <summary>
        /// 查询表TB_FORM_DEFINE所有记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetAllFromDefineList()
        {
            FormDefineDao dao = new FormDefineDao();
            return dao.findAll();
        }
        #endregion

        #region 根据主键查询表TB_FORM_DEFINE记录，返回dateset记录集
        /// <summary>
        /// 根据主键查询表TB_FORM_DEFINE记录，返回dateset记录集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetFromDefineInfoByKey(String strFormId)
        {
            FormDefineDao dao = new FormDefineDao();
            return dao.findByKey(strFormId);
        }
        #endregion

        #region 判断表单表编码是否已经存在
        /// <summary>
        /// 判断表单表编码是否已经存在
        /// </summary>
        /// <param name="strFormCode"></param>
        /// <returns></returns>
        public Boolean IsExsitFormCode(String strFormCode)
        {
            Boolean bIsExsit = true; 
            FormDefineDao daoForm = new FormDefineDao();
            DataSet ds = daoForm.findByCode(strFormCode);
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

        #region 插入菜单字典定义表一条记录
        /// <summary>
        /// 插入菜单字典定义表一条记录
        /// </summary>
        /// <param name="strSFORMID"></param>
        /// <param name="strSFORMCODE"></param>
        /// <param name="strSFORMNAME"></param>
        /// <param name="strSFORMAMECN"></param>
        /// <param name="strSFORMDESC"></param>
        /// <param name="strSFORMDESCCN"></param>
        /// <param name="strSPLUGINAFTERSVAE"></param>
        /// <param name="strBISVERSION"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int AddFormDefineInfo(String strSFORMID, String strSFORMCODE, String strSFORMNAME, String strSFORMAMECN, String strSFORMDESC, String strSFORMDESCCN, String strSPLUGINAFTERSVAE, String strBISVERSION, String strBISSTOP)
        {
            FormDefineDao daoForm = new FormDefineDao();
            return daoForm.insertOneRow(strSFORMID, strSFORMCODE, strSFORMNAME, strSFORMAMECN, strSFORMDESC, strSFORMDESCCN, strSPLUGINAFTERSVAE, strBISVERSION, "0");
        }
        #endregion

        #region 根据表单定义表主键删除一条记录
        /// <summary>
        /// 根据表单定义表主键删除一条记录
        /// </summary>
        /// <returns></returns>
        public int deleteFormDefineInfo(String strFormKey)
        {
            int iCount = 0;
            FormDefineDao daoForm = new FormDefineDao();
            iCount = daoForm.deleteByKey(strFormKey);
            return iCount;
        }
        #endregion

        #region 根据表单定义表主键更新一条记录
        /// <summary>
        /// 根据表单定义表主键更新一条记录
        /// </summary>
        /// <param name="strSFORMID"></param>
        /// <param name="strSFORMCODE"></param>
        /// <param name="strSFORMNAME"></param>
        /// <param name="strSFORMAMECN"></param>
        /// <param name="strSFORMDESC"></param>
        /// <param name="strSFORMDESCCN"></param>
        /// <param name="strSPLUGINAFTERSVAE"></param>
        /// <param name="strBISVERSION"></param>
        /// <param name="strBISSTOP"></param>
        /// <returns></returns>
        public int updateDicListDefine(String strSFORMID, String strSFORMCODE, String strSFORMNAME, String strSFORMAMECN, String strSFORMDESC, String strSFORMDESCCN, String strSPLUGINAFTERSVAE, String strBISVERSION, String strBISSTOP)
        {
            int iCount = 0;
            FormDefineDao daoForm = new FormDefineDao();
            iCount = daoForm.updateByKey(strSFORMID, strSFORMCODE, strSFORMNAME, strSFORMAMECN, strSFORMDESC, strSFORMDESCCN, strSPLUGINAFTERSVAE, strBISVERSION, strBISSTOP);
            return iCount;
        }
        #endregion


    }


}
