using System.Net.Sockets;
using KJFramework.Net.EventArgs;

namespace KJFramework.Net.Listener
{
    /// <summary>
    ///     遵循完成端口模型，基于TCP协议的端口监听器元接口
    /// </summary>
    public interface ITcpIocpListener<TListenerInfo> : IPortListener
        where TListenerInfo : IPortListenerInfomation
    {
        /// <summary>
        ///     获取TCP端口监听对象
        /// </summary>
        Socket Listener { get; }
        /// <summary>
        ///     获取或设置端口监听器详细信息元接口
        /// </summary>
        TListenerInfo ListenerInfomation { get;set;}
        /// <summary>
        ///     接受当前新的连接请求
        /// </summary>
        void GetPedding();
        /// <summary>
        ///     新用户连接事件
        /// </summary>
        event DELEGATE_IOCP_PORTLISTENER_CONNECTED<TListenerInfo> Connected;
    }
}
