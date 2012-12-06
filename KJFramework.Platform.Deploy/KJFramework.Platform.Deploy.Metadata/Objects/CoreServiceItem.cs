using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     核心服务信息对象
    /// </summary>
    public class CoreServiceItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置分组信息
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string Category { get; set; }
        /// <summary>
        ///     获取或设置机器名
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置控制地址
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string ControlAddress { get; set; }
        /// <summary>
        ///     获取或设置最后心跳时间
        /// </summary>
        [IntellectProperty(3, IsRequire = false)]
        public DateTime LastHeartBeatTime { get; set; }
        /// <summary>
        ///     获取或设置部署地址
        /// </summary>
        [IntellectProperty(4, IsRequire = false)]
        public string DeployAddress { get; set; }
        /// <summary>
        ///     获取或设置内部的服务信息
        /// </summary>
        [IntellectProperty(5, IsRequire = false)]
        public OwnServiceItem[] Services { get; set; }

        #endregion
    }
}