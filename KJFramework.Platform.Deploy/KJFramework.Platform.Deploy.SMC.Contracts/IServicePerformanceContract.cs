using KJFramework.Platform.Deploy.SMC.Common.Performances;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.Platform.Deploy.SMC.Contracts
{
    /// <summary>
    ///     服务性能汇报器契约元接口，提供了相关的基本操作。
    /// </summary>
    [ServiceContract(Description = "服务性能汇报器契约", Name = "Service Performance Contract", Version = "0.0.0.1")]
    public interface IServicePerformanceContract
    {
        /// <summary>
        ///     获取性能信息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="performanceFlag">
        ///     性能标识
        /// <para>* 此性能标识可以组合</para>
        /// </param>
        /// <returns>返回服务性能信息</returns>
        ServicePerformanceItem[] GetPerformance(string serviceName, string performanceFlag);
        /// <summary>
        ///     获取受控范围内所有服务的性能信息
        /// </summary>
        /// <param name="performanceFlag">
        ///     性能标识
        /// <para>* 此性能标识可以组合</para>
        /// </param>
        /// <returns>返回服务性能信息</returns>
        ServicePerformanceItem[] GetPerformance(string performanceFlag);
    }
}