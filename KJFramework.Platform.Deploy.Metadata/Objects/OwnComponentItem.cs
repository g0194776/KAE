using System;
using KJFramework.Basic.Enum;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     拥有组件项
    /// </summary>
    public class OwnComponentItem : IntellectObject
    {
        /// <summary>
        ///     获取或设置组件别名
        /// </summary>
        [IntellectProperty(0, IsRequire = false)]
        public string Name { get; set;}
        /// <summary>
        ///     获取或设置组件名称
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string ComponentName { get; set; }
        /// <summary>
        ///     获取或设置组件版本号
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Version { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置组件健康状态
        /// </summary>
        [IntellectProperty(4, IsRequire = true)]
        public HealthStatus Status { get; set; }
        /// <summary>
        ///     获取或设置最后更新时间
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public DateTime LastUpdateTime { get; set; }
    }
}