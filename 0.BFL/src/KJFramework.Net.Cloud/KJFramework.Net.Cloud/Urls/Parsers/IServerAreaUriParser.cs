using System;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   服务域资源地址解析器，提供了相关的基本操作。
    /// </summary>
    public interface IServerAreaUriParser : IDisposable
    {
        /// <summary>
        ///   获取支持的协议键值
        /// </summary>
        String ProtocolKey { get; }
        /// <summary>
        ///   从给定的资源中解析出一个服务域连接地址对象
        /// </summary>
        /// <param name="uri">除了资源地址头以外的部分</param>
        /// <param name="keys">关键字结合</param>
        /// <returns>服务域连接地址对象</returns>
        IServerAreaUri Parse(String uri, String[] keys);
    }
}