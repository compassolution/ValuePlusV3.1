using System;
using System.Collections.Generic;
using System.Text;
using Com.ValuePlus.Database;
using Com.ValuePlus.Common.Config;

namespace Com.ValuePlus.Flow.DAL
{
    public class DALFactory
    {
        /// <summary>
        /// 创建DATABASEBAO
        /// </summary>
        /// <returns></returns>
        public static IDatabaseDAO CreateSqlServerDAO()
        {
            return DAOFactory.CreateSqlServerDAO(BaseConfig.Instance.GetConnectionString());
        }
    }
}
