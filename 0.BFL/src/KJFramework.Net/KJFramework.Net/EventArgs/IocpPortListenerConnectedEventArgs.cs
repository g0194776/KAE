using System;
using System.Net.Sockets;
using KJFramework.Net.Listener;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_IOCP_PORTLISTENER_CONNECTED<TListenerInfo>(Object sender, IocpPortListenerConnectedEventArgs<TListenerInfo> e) where TListenerInfo : IPortListenerInfomation;
    /// <summary>
    ///     遵循完成端口模型的端口监听器连接事件
    /// </summary>
    /// <typeparam name="TListenerInfo">端口信息类型</typeparam>
    public class IocpPortListenerConnectedEventArgs<TListenerInfo> : System.EventArgs
        where TListenerInfo : IPortListenerInfomation
    {
         private Socket _connectstream;
        /// <summary>
        ///     连接者基础套接字
        /// </summary>
         public Socket ConnectStream
        {
            get { return _connectstream; }
            set { _connectstream = value; }
        }


        private TListenerInfo _listenerinfo;
        /// <summary>
        ///     监听器信息
        /// </summary>
        public TListenerInfo ListenerInfo
        {
            get { return _listenerinfo; }
        }

        /// <summary>
        ///     遵循完成端口模型的端口监听器连接事件
        /// </summary>
        /// <param name="connectStream">连接者基础套接字</param>
        /// <param name="listenerInfo">端口信息类型</param>
        public IocpPortListenerConnectedEventArgs(Socket connectStream, TListenerInfo listenerInfo)
        {
            _connectstream = connectStream;
            _listenerinfo = listenerInfo;
        }
    }
}
