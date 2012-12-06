using System;
using KJFramework.ServiceModel.Proxy;

namespace KJFramework.ServiceModel.Bussiness.Default.Async
{
    /// <summary>
    ///     异步方法代理控制器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IAsyncMethodDelegatorController
    {
        /// <summary>
        ///     获取一个方法的异步代理器，如果当前方法不存在与现有的列表中，将会自动添加。
        /// </summary>
        /// <param name="asyncMethodName">异步方法</param>
        /// <returns>返回一个异步方法代理器</returns>
        IAsyncMethodDelegator this[String asyncMethodName] { get; }
        /// <summary>
        ///     获取一个方法的异步代理器，如果当前方法不存在与现有的列表中，将会自动添加。
        /// </summary>
        /// <param name="asyncMethodName">异步方法</param>
        /// <returns>返回一个异步方法代理器</returns>
        IAsyncMethodDelegator GetDelegator(String asyncMethodName);
        /// <summary>
        ///     添加一个代理
        /// </summary>
        /// <param name="asyncMethodName">异步操作名称</param>
        /// <param name="callback">回调函数</param>
        void AddDelegate(String asyncMethodName, AsyncMethodCallback callback);
    }
}