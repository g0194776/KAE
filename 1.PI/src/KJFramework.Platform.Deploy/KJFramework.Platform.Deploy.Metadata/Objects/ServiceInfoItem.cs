using System.ServiceProcess;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Platform.Deploy.Metadata.Objects
{
    /// <summary>
    ///     服务详细信息对象
    /// </summary>
    public class ServiceInfoItem : IntellectObject
    {
        #region Members

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        [IntellectProperty(0, IsRequire = true)]
        public string ServiceName { get; set; }
        /// <summary>
        ///     获取或设置服务别名
        /// </summary>
        [IntellectProperty(1, IsRequire = false)]
        public string Name { get; set; }
        /// <summary>
        ///     获取或设置服务描述
        /// </summary>
        [IntellectProperty(2, IsRequire = false)]
        public string Description { get; set; }
        /// <summary>
        ///     获取或设置服务状态
        /// </summary>
        [IntellectProperty(3, IsRequire = true)]
        public ServiceControllerStatus Status { get; set; }

        #endregion
    }
}