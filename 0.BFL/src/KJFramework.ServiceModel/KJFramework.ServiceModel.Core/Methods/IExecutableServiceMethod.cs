using System;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     可执行的服务方法元接口，提供了相关的基本操作。
    /// </summary>
    internal interface IExecutableServiceMethod : IServiceMethod
    {
        /// <summary>
        ///     获取或设置运行实例
        /// </summary>
        Object Instance { get; set; }
        /// <summary>
        ///     执行当前服务方法
        /// </summary>
        /// <param name="args">参数数组</param>
        /// <returns>
        ///     返回当前服务方法的返回值
        ///     <para>* 如果当前方法不具有返回值，则也会返回null。</para>
        /// </returns>
        Object Invoke(params Object[] args);
    }
}