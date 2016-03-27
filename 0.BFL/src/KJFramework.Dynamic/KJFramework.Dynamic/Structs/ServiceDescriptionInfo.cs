using System;

namespace KJFramework.Dynamic.Structs
{
    /// <summary>
    ///     服务描述信息
    /// </summary>
    public class ServiceDescriptionInfo
    {
        /// <summary>
        ///     获取或设置显示名称
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        public String ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        ///     获取或设置服务版本
        /// </summary>
        public String Version { get; set; } 
    }
}