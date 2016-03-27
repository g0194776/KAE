using System;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   服务域资源地址解析器父类，提供了相关的基本操作。
    /// </summary>
    public abstract class ServerAreaUriParser : IServerAreaUriParser
    {
        #region 析构函数

        ~ServerAreaUriParser()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected string _protocolKey;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServerAreaUriParser

        /// <summary>
        ///   获取支持的协议键值
        /// </summary>
        public string ProtocolKey
        {
            get { return _protocolKey; }
        }

        /// <summary>
        ///   从给定的资源中解析出一个服务域连接地址对象
        /// </summary>
        /// <param name="uri">除了资源地址头以外的部分</param>
        /// <param name="keys">关键字结合</param>
        /// <returns>服务域连接地址对象</returns>
        public abstract IServerAreaUri Parse(string uri, string[] keys);

        #endregion
    }
}