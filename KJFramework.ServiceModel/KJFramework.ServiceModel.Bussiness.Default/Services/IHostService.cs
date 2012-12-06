using System;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Objects;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     宿主服务元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IHostService
    {
        /// <summary>
        ///     获取内部服务类型
        /// </summary>
        /// <returns>返回服务类型</returns>
        Type GetServiceType();
        /// <summary>
        ///     获取元数据契约名称
        /// </summary>
        /// <returns>返回契约开放的名称</returns>
        string GetContractName();
        /// <summary>
        ///     创建一个新的内部服务实例
        /// </summary>
        /// <returns>返回新的内部服务实例</returns>
        object NewServiceInstance();
        /// <summary>
        ///     归还一个内部服务实例
        /// </summary>
        /// <param name="obj">内部服务实例</param>
        void Giveback(object obj);
        /// <summary>
        ///     获取所有服务方法
        /// </summary>
        /// <returns></returns>
        ServiceMethodPickupObject[] GetMethods();
        /// <summary>
        ///     获取具有指定名称和参数个数的服务方法
        /// </summary>
        /// <param name="methodToken">方法序列标记</param>
        /// <returns>返回方法</returns>
        ServiceMethodPickupObject GetMethod(int methodToken);
        /// <summary>
        ///     获取服务对象
        /// </summary>
        /// <returns></returns>
        Object GetServiceObject();
        /// <summary>
        ///     获取唯一编号
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取服务契约属性
        /// </summary>
        ServiceContractAttribute Contract { get; }
    }
}