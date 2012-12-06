using System.Net;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     HTTP协议宿主信道元接口，提供了相关的基本操作
    /// </summary>
    public interface IHttpHostTransportChannel : IHostTransportChannel
    {
        /// <summary>
        ///     获取由此 HttpListener 对象处理的统一资源标识符 (URI) 前缀
        /// </summary>
        HttpListenerPrefixCollection Prefixes { get; }
        /// <summary>
        ///     获取或设置与此 HttpListener 对象关联的领域或资源分区
        /// </summary>
        string Realm { get; set; }
        /// <summary>
        ///     获取或设置 Boolean 值，该值控制当使用 NTLM 时是否需要对使用同一传输控制协议 (TCP) 连接的其他请求进行身份验证
        /// </summary>
        bool UnsafeConnectionNtlmAuthentication { get; set; }
        /// <summary>
        ///     获取或设置 Boolean 值，该值指定应用程序是否接收 HttpListener 向客户端发送响应时发生的异常
        /// </summary>
        bool IgnoreWriteExceptions { get; set; }
    }
}