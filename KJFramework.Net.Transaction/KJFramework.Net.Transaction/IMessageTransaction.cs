using System;
using KJFramework.EventArgs;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Identities;

namespace KJFramework.Net.Transaction
{
    /// <summary>
    ///     消息事务，用于承载网络消息处理的专用事务
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public interface IMessageTransaction<T> : ITransaction
    {
        #region Members.

        /// <summary>
        ///     获取事务的创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取成功操作后的请求时间
        /// </summary>
        DateTime RequestTime { get; }
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的事务是否需要响应消息
        /// </summary>
        bool NeedResponse { get; set; }
        /// <summary>
        ///     获取或设置请求消息
        /// </summary>
        T Request { get; set; }
        /// <summary>
        ///     获取或设置响应消息
        /// </summary>
        T Response { get; set; }       
        /// <summary>
        ///    获取当前事务的唯一标识 
        /// </summary>
        TransactionIdentity Identity { get; }
        /// <summary>
        ///     获取事务管理器
        /// </summary>
        ITransactionManager<T> TransactionManager { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///     设置响应消息，并激活处理流程
        /// </summary>
        /// <param name="response">响应消息</param>
        void SetResponse(T response);
        /// <summary>
        ///     发送一个请求消息
        /// </summary>
        /// <param name="message">请求消息</param>
        void SendRequest(T message);
        /// <summary>
        ///     发送一个响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        void SendResponse(T message);
        /// <summary>
        ///     获取内部的传输信道
        /// </summary>
        IMessageTransportChannel<T> GetChannel();

        #endregion

        #region Events.

        /// <summary>
        ///     事物超时事件
        /// </summary>
        event EventHandler Timeout;
        /// <summary>
        ///     事物失败事件
        /// </summary>
        event EventHandler Failed;
        /// <summary>
        ///     响应消息抵达事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<T>> ResponseArrived;

        #endregion
    }
}