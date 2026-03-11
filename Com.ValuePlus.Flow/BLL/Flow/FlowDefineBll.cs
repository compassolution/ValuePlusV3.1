using System;
using System.Text;
using Com.ValuePlus.Flow.DAL.Flow;
using System.Data;
using Com.ValuePlus.Flow.Entity;
using Com.ValuePlus.Entity;

namespace Com.ValuePlus.Flow.BLL.Flow
{
    public class FlowDefineBll
    {
        /// <summary>
        /// 获取所有流程定义信息,返回DS数据集
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetAllFlowDefineInfo()
        {
            FlowDefineDao dao = new FlowDefineDao();
            DataSet ds = dao.findAll();

            return ds;
        }
    }
}
