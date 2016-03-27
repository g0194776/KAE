using System;
using KJFramework.Basic.Enum;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     组件详细信息检查项
    /// </summary>
    public class ComponentDetailItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置组件名称
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置健康状态
        /// </summary>
        [IntellectProperty(1, IsRequire = false)]
        public HealthStatus Status { get; set; }
        /// <summary>
        ///     获取或设置组件版本
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置组件描述信息
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置组件的全名
        /// </summary>
        [IntellectProperty(4, IsRequire = false)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置组建的分组
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public string CatalogName { get; set; }
        /// <summary>
        ///     获取或设置组件最后更新时间
        /// </summary>
        [IntellectProperty(6, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }

        #endregion

    }
}