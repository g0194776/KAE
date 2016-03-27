using System;
using KJFramework.ServiceModel.Proxy;

namespace KJFramework.ServiceModel.Bussiness.Default.Async
{
    /// <summary>
    ///     异步方法代理器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IAsyncMethodDelegator
    {
        /// <summary>
        ///     获取此代理器绑定到的异步方法名称
        /// </summary>
        String AsyncMethodName { get; }
        /// <summary>
        ///     添加一个临时代理
        /// </summary>
        /// <param name="callback">回调函数</param>
        void AddDelegate(AsyncMethodCallback callback);
        /// <summary>
        ///     根据会话Id，获取指定的回调函数
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns>返回回调函数</returns>
        AsyncMethodCallback GetDelegate(int sessionId);
        /// <summary>
        ///     将一个回调函数绑定到一个指定的会话Id上
        ///     <para>* 注意：使用此函数应该与调用AddDelegate函数在同一个线程上。</para>
        /// </summary>
        /// <param name="sessionId">会话Id</param>
        /// <returns>返回绑定的结果</returns>
        bool Bind(int sessionId);
        /// <summary>
        ///     抛弃所有内部等待的回调函数
        ///     <para>* 掉用此方法后，内部将会指定所有存储的回调函数，并使用超时来当做失败的原因。</para>
        /// </summary>
        void DiscardAllOperationForTimeout();
        /// <summary>
        ///     获取最后一次更新的时间
        /// </summary>
        DateTime LastUpdateTime { get; }
    }
}