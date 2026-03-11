using System;
using System.Collections.Generic;
using System.Text;

namespace Com.ValuePlus.Config
{
    /// <summary>
    /// 配置管理接口,配置文件管理类,限制只能读properties,txt,xml,config文件.针对properties和txt都采用同一种方式处理.针对xml和config文件采用同一种方式处理
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        string GetProperty(string key);


        /// <summary>
        /// 获得对象
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        Com.ValuePlus.Utils.SqlBasicMetaData GetSqlBasicMetaData(string key);

         #region 加载配置文件并初始化参数
        /// <summary>
        /// 加载配置文件并初始化参数
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="filetype"></param>
        void Load(string FilePath, FileTypeEnum filetype);
        #endregion



        /// <summary>
        /// 获得对象,主要用于xml可以序列化的情形
        /// </summary>
        /// <returns>value</returns>
        object GetProperty();

        #region 加载配置文件并初始化参数
        /// <summary>
        /// 加载配置文件并初始化参数,主要用于xml可以序列化的情形
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="objecttype">文件类型</param>
        void Load(string FilePath, Type objecttype);
        #endregion
    }
}
