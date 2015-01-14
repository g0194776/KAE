using KJFramework.Datas;

namespace KJFramework.ApplicationEngine.Apps.Configuration
{
    /// <summary>
    ///    内部全局唯一配置值存储类
    /// </summary>
    internal static class Global
    {
        #region Members.
        
        /// <summary>
        ///     获取内部CSNDB连接串地址 
        /// </summary>
        public static readonly string ConfigDB = "server=config.kae.com;user=root;database=csndb;port=3306;password=123123123;";
        /// <summary>
        ///     获取数据库实例
        /// </summary>
        public static Database Database;

        #endregion
    }
}