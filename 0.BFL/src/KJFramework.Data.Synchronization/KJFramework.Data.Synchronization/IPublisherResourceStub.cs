using System;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.EventArgs;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     发布者资源存根接口
    /// </summary>
    internal interface IPublisherResourceStub
    {
        /// <summary>
        ///     获取当前资源存根的使用关联次数
        /// </summary>
        int UseCount { get; }
        /// <summary>
        ///     获取资源唯一标识
        /// </summary>
        string ResourceKey { get; }
        /// <summary>
        ///     绑定网络资源
        /// </summary>
        /// <param name="res">网络资源</param>
        void Bind(INetworkResource res);
        /// <summary>
        ///     丢弃当期资源存根
        /// </summary>
        void Discard();
        /// <summary>
        ///     已注销事件
        /// </summary>
        event EventHandler Disposed;
        /// <summary>
        ///     新事物事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<NewTransactionEventArgs>> NewTransaction;
    }
}