using KJFramework.Net.Channels.Identities;

namespace KJFramework.ServiceModel.Core.EventArgs
{
    /// <summary>
    ///     动态客户端代理最底层请求事件对象
    /// </summary>
    public class ClientLowProxyRequestEventArgs : System.EventArgs
    {
        #region Members

        /// <summary>
        ///     获取或设置会话编号
        /// </summary>
        public TransactionIdentity Identity;
        /// <summary>
        ///     获取或设置方法令牌
        /// </summary>
        public int MethodToken;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前请求是否为异步方法
        /// </summary>
        public bool IsAsync;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前请求是否需要回馈
        /// </summary>
        public bool NeedAck;
        /// <summary>
        ///     获取或设置参数集合
        /// </summary>
        public object[] Arguments;
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的调用是否需要回调函数
        /// </summary>
        public bool HasCallback;

        #endregion
    }
}