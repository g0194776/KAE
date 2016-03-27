using System;

namespace KJFramework.Dynamic
{
    /// <summary>
    ///     插件信息基础结构体
    /// </summary>
    [Serializable]
    public struct PluginInfomation
    {
        /// <summary>
        ///     服务名称
        /// </summary>
        public String ServiceName;
        /// <summary>
        ///     插件分类名称
        /// </summary>
        public String CatalogName;
        /// <summary>
        ///     插件描述
        /// </summary>
        public String Description;
        /// <summary>
        ///     插件版本
        /// </summary>
        public String Version;
    }
}
