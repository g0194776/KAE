using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     Tcp资源地址标示类，提供了相关的基本操作。
    /// </summary>
    public class TcpUri : Uri
    {
        #region 成员

        protected String _hostAddress;
        protected String _serviceName;
        private int _port;
        protected bool _isHost;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (TcpUri));

        /// <summary>
        ///     获取或设置服务名称
        /// </summary>
        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     获取或设置宿主地址
        /// </summary>
        public String HostAddress
        {
            get { return _hostAddress; }
            set { _hostAddress = value; }
        }

        /// <summary>
        ///     获取或设置宿主端口
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        ///     获取一个值，该值标示了当前给定的宿主地址是否为本地地址。
        /// </summary>
        public bool IsHost
        {
            get { return _isHost; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        ///     Tcp资源地址标示类，提供了相关的基本操作。
        /// </summary>
        public TcpUri() : base("")
        { }

        /// <summary>
        ///     Tcp资源地址标示类，提供了相关的基本操作。
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         完整的URL地址
        ///     </para>
        /// </param>
        public TcpUri(String url)
            : base(url)
        {
            int offset = url.LastIndexOf(':');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region 父类方法

        /// <summary>
        ///     获取服务器内部使用的Uri形态
        /// </summary>
        /// <returns>返回Uri</returns>
        public override string GetServiceUri()
        {
            return _serverUri;
        }

        protected override void Split()
        {
            base.Split();
            try
            {
                if (_prefix.ToLower() != "tcp")
                {
                    throw new System.Exception("非法的TCP资源地址标示。");
                }
                int firstFlagOffset = _address.IndexOf("/");
                if (firstFlagOffset == -1)
                {
                    throw new System.Exception("非法的TCP资源地址标示。");
                }
                String[] hostAddress = _address.Substring(0, firstFlagOffset).Split(new[] {":"},
                                                                                    StringSplitOptions.
                                                                                        RemoveEmptyEntries);
                String serviceName = _address.Substring(firstFlagOffset + 1, _address.Length - (firstFlagOffset + 1));
                if (hostAddress.Length <= 1 || String.IsNullOrEmpty(serviceName))
                {
                    throw new System.Exception("非法的TCP资源地址标示。");
                }
                _hostAddress = hostAddress[0];
                _port = int.Parse(hostAddress[1]);
                _serviceName = serviceName;
                if (_port <= 0 || _port > 65535)
                {
                    throw new System.Exception("非法的TCP资源地址标示。");
                }
                if (_hostAddress.ToLower() == "localhost" || _hostAddress == "127.0.0.1")
                {
                    _hostAddress = "127.0.0.1";
                    _isHost = true;
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw new System.Exception("非法的TCP资源地址标示。");
            }
        }

        #endregion
    }
}