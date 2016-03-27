using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_IOCP_PORTLISTENER_DISCONNECTED<TListener>(Object sender, IocpPortListenerDisconnectedEventArgs<TListener> e);
    /// <summary>
    ///     端口监听器断开事件（遵循完成端口模型的端口监听器）
    /// </summary>
    public class IocpPortListenerDisconnectedEventArgs<TListener> : System.EventArgs
    {
        private TListener _listener;
        /// <summary>
        ///     断开连接的端口监听器
        /// </summary>
        public TListener Listener
        {
            get { return _listener; }
        }

        /// <summary>
        ///     端口监听器断开事件（遵循完成端口模型的端口监听器）
        /// </summary>
        public IocpPortListenerDisconnectedEventArgs(TListener Listener)
        {
            _listener = Listener;   
        }

    }
}
