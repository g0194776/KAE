using System;
using KJFramework.Net.Cloud.Urls.Parsers;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   服务域地址，提供了相关的基本属性结构。
    /// </summary>
    public abstract class ServerAreaUri : IServerAreaUri
    {
        #region 析构函数

        ~ServerAreaUri()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServerAreaUri

        /// <summary>
        ///   创建一个宿主信道
        /// </summary>
        /// <returns>返回宿主通道</returns>
        public abstract IHostTransportChannel CreateHostChannel();
        /// <summary>
        ///   创建一个通讯信道
        /// </summary>
        /// <returns>返回通讯信道</returns>
        public abstract ITransportChannel CreateTransportChannel();

        #endregion

        #region Static Functions

        /// <summary>
        ///   通过一个资源地址创建对应的服务域连接地址对象
        /// </summary>
        /// <param name="uri">资源地址</param>
        /// <returns>返回服务域连接地址对象</returns>
        public static IServerAreaUri Create(String uri)
        {
            if (String.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            string[] totalChunk = uri.Split(new [] {"://"}, StringSplitOptions.RemoveEmptyEntries);
            if (totalChunk.Length != 2)
            {
                throw new System.Exception("非法的资源地址。");
            }
            string[] keys = totalChunk[0].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length != 2)
            {
                throw new System.Exception("非法的资源地址。");
            }
            IServerAreaUriParser parser = ServeAreaUriParsers.Instance.GetParser(keys[1]);
            if (parser == null)
            {
                throw new System.Exception("无法找到对应的资源地址解析器，非法的资源地址。");
            }
            return parser.Parse(totalChunk[1], keys);
        }

        #endregion
    }
}