using KJFramework.Messages.Attributes;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Platform.Deploy.SMC.Common.Performances;

namespace KJFramework.Platform.Deploy.DSC.ProtocolStack
{
    /// <summary>
    ///     DSC注册请求消息
    /// </summary>
    public class DSCRegistRequestMessage : DSCMessage
    {
        #region Constructor

        /// <summary>
        ///     DSC注册请求消息
        /// </summary>
        public DSCRegistRequestMessage()
        {
            Header.ProtocolId = 0;
        }

        #endregion

        #region Members
        
        /// <summary>
        ///     获取或设置机器名
        ///     <para>* 协议中规定，每个SMC用自身的机器名作为唯一标识</para>
        /// </summary>
        [IntellectProperty(11, IsRequire = true)]
        public string MachineName { get; set; }
        /// <summary>
        ///     获取或设置拥有服务的详细信息
        /// </summary>
        [IntellectProperty(12, IsRequire = false)]
        public OwnServiceItem[] OwnServics { get; set; }
        /// <summary>
        ///     获取或设置相关性能项
        /// </summary>
        [IntellectProperty(13, IsRequire = false)]
        public ServicePerformanceItem[] PerformanceItems { get; set; }
        /// <summary>
        ///     获取或设置注册分类
        ///     <para>* 这里可能包含多个分类，比如：部署单元或者SMC</para>
        /// </summary>
        [IntellectProperty(14, IsRequire = true)]
        public string Category { get; set; }
        /// <summary>
        ///     获取或设置部署地址
        ///     <para>* 此属性只关联部署单元</para>
        /// </summary>
        [IntellectProperty(15, IsRequire = false)]
        public string DeployAddress { get; set; }
        /// <summary>
        ///     获取或设置SMC的控制地址
        /// </summary>
        [IntellectProperty(16, IsRequire = false)]
        public string ControlAddress { get; set; }

        #endregion

    }
}