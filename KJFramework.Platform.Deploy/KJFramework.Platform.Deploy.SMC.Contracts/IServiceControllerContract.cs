using KJFramework.Platform.Deploy.Metadata.Enums;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.Platform.Deploy.SMC.Contracts
{
    /// <summary>
    ///     服务控制器契约元接口，提供了相关的基本操作。
    /// </summary>
    [ServiceContract(Description = "服务控制器契约", Name = "Service Controller Contract", Version = "0.0.0.1")]
    public interface IServiceControllerContract
    {
        /// <summary>
        ///     开启服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        [Operation]
        ServiceStatus Open(string serviceName);
        /// <summary>
        ///     关闭服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        [Operation]
        ServiceStatus Close(string serviceName);
        /// <summary>
        ///     暂停服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        [Operation]
        ServiceStatus Pause(string serviceName);
        /// <summary>
        ///     查询服务状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务状态</returns>
        [Operation]
        ServiceStatus GetStatus(string serviceName);
    }
}