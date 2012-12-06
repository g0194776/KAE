using KJFramework.Attribute;

namespace KJFramework.ServiceModel.Configurations
{
    /// <summary>
    ///   KJFramework.ServiceModel配置项
    /// </summary>
    public sealed class SettingConfiguration
    {
        /// <summary>
        ///   请求中心等待对象池化数量
        /// </summary>
        [CustomerField("RequestCenterWaitObjectPoolCount")]
        public int RequestCenterWaitObjectPoolCount;
        /// <summary>
        ///    请求消息池化数量
        /// </summary>
        [CustomerField("RequestServiceMessagePoolCount")]
        public int RequestServiceMessagePoolCount;
        /// <summary>
        ///   响应消息池化数量
        /// </summary>
        [CustomerField("ResponseServiceMessagePoolCount")]
        public int ResponseServiceMessagePoolCount;
        /// <summary>
        ///   请求中心等待对象池化数量
        /// </summary>
        [CustomerField("ServiceCallContextPoolCount")]
        public int ServiceCallContextPoolCount;
        /// <summary>
        ///     服务契约对象的池化数量
        /// </summary>
        [CustomerField("ServiceProviderObjectPoolCount")]
        public int ServiceProviderObjectPoolCount;
        /// <summary>
        ///     允许客户端信道缓存标示
        /// </summary>
        [CustomerField("AllowClientCache")]
        public bool AllowClientCache;
    }
}