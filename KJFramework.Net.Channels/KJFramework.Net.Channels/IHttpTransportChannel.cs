using System.Collections.Specialized;
using System.Net;
using System.Text;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     基于HTTP协议的传输信道元接口，提供了相关的基本操作
    /// </summary>
    public interface IHttpTransportChannel : IRawTransportChannel
    {
        /// <summary>
        ///     获取信道类型
        /// </summary>
        HttpChannelTypes ChannelType { get; }
        /// <summary>
        ///     获取包含在请求中的正文数据的长度
        /// </summary>
        long ContentLength64 { get; }
        /// <summary>
        ///     获取或设置返回给客户端的 HTTP 状态代码
        /// </summary>
        int StatusCode { get; set; }
        /// <summary>
        ///     获取客户端请求的 URL 信息（不包括主机和端口）
        /// </summary>
        string RawUrl { get; }
        /// <summary>
        ///     获取发出请求的客户端 IP 地址和端口号
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
        /// <summary>
        ///     获取客户端请求的 Uri 对象
        /// </summary>
        System.Uri Url { get; }
        /// <summary>
        ///     获取请求被定向到的服务器 IP 地址和端口号
        /// </summary>
        string UserHostAddress { get; }
        /// <summary>
        ///     获取由客户端指定的 HTTP 方法
        /// </summary>
        string HttpMethod { get; }
        /// <summary>
        ///     获取客户端接受的 MIME 类型
        /// </summary>
        string[] AcceptTypes { get; }
        /// <summary>
        ///     获取包含在请求中的正文数据的 MIME 类型
        /// </summary>
        string ContentType { get; }
        /// <summary>
        ///     获取一个 Boolean 值，该值指示客户端是否请求持续型连接
        /// </summary>
        bool KeepAlive { get; }
        /// <summary>
        ///     获取 Boolean 值，该值指示该请求是否来自本地计算机
        /// </summary>
        bool IsLocal { get; }
        /// <summary>
        ///     获取一个 Boolean 值，该值指示请求是否有关联的正文数据
        /// </summary>
        bool HasEntityBody { get; }
        /// <summary>
        ///     获取在请求中发送的标头名称/值对的集合
        /// </summary>
        NameValueCollection Headers { get; }
        /// <summary>
        ///     获取包含在请求中的查询字符串
        /// </summary>
        NameValueCollection QueryString { get; }
        /// <summary>
        ///     获取随请求发送的 Cookie
        /// </summary>
        CookieCollection Cookies { get; }
        /// <summary>
        ///     获取可用于随请求发送的数据的内容编码
        /// </summary>
        Encoding ContentEncoding { get; }
        /// <summary>
        ///     发送HTTP请求
        /// </summary>
        void Send();
        /// <summary>
        ///     获取内部核心请求对象
        /// </summary>
        /// <returns>返回请求对象</returns>
        HttpListenerRequest GetRequest();
        /// <summary>
        ///     获取内部核心回馈对象
        /// </summary>
        /// <returns>返回回馈对象</returns>
        HttpListenerResponse GetResponse();
    }
}