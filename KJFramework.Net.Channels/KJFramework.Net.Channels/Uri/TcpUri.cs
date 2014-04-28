using KJFramework.Net.Channels.Enums;
using KJFramework.Tracing;
using System;

namespace KJFramework.Net.Channels.Uri
{
    /// <summary>
    ///     Tcp资源地址
    /// </summary>
    public class TcpUri : Uri
    {
        #region Members.

        protected String _hostAddress;
        protected String _serviceName;
        private int _port;
        protected bool _isHost;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (TcpUri));
        /// <summary>
        ///     该URI表示一个本地的动态TCP端口资源
        /// </summary>
        public static readonly TcpUri Dynamic = new TcpUri("tcp://localhost:*");

        /// <summary>
        ///     获取或设置服务名
        /// </summary>
        public String ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        /// <summary>
        ///     获取或设置主机地址
        /// </summary>
        public String HostAddress
        {
            get { return _hostAddress; }
            set { _hostAddress = value; }
        }

        /// <summary>
        ///     获取或设置端口号
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        ///     获取一个标示，表示了当前的地址是否是一个宿主地址类型
        /// </summary>
        public bool IsHost
        {
            get { return _isHost; }
        }
        /// <summary>
        ///    获取一个值，改值表示了当前的TCP资源是否需要使用随机的TCP端口资源
        /// </summary>
        public bool IsUseDynamicResource { get; private set; }

        #endregion

        #region Constrcutors.

        /// <summary>
        ///     Tcp资源地址
        /// </summary>
        public TcpUri() : base("")
        { }

        /// <summary>
        ///     Tcp资源地址
        /// </summary>
        /// <param name="url" type="string">
        ///     <para>
        ///         资源地址
        ///     </para>
        /// </param>
        public TcpUri(String url)
            : base(url)
        {
            int offset = url.LastIndexOf(':');
            _serverUri = url.Substring(offset, url.Length - offset);
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    获取当前URL所代表的网络类型
        /// </summary>
        public override NetworkTypes NetworkType
        {
            get { return NetworkTypes.TCP; }
        }

        /// <summary>
        ///     获取服务地址
        /// </summary>
        /// <returns>返回服务地址</returns>
        public override string GetServiceUri()
        {
            return _serverUri;
        }

        protected override void Split()
        {
            base.Split();
            try
            {
                if (_prefix.ToLower() != "tcp") throw new System.Exception("#Illegal TCP resource prefix!");
                int firstFlagOffset = _address.IndexOf("/");
                if (firstFlagOffset == -1) firstFlagOffset = _address.Length;
                String[] hostAddress = _address.Substring(0, firstFlagOffset).Split(new[] {":"},
                                                                                    StringSplitOptions.
                                                                                        RemoveEmptyEntries);
                String serviceName = string.Empty;
                if(_address.Length > firstFlagOffset)
                    serviceName = _address.Substring(firstFlagOffset + 1, _address.Length - (firstFlagOffset + 1));
                if (hostAddress.Length <= 1) throw new System.Exception("#Illegal TCP resource format!");
                _hostAddress = hostAddress[0];
                if (hostAddress[1].Length == 1 && hostAddress[1][0] == '*') IsUseDynamicResource = true;
                else
                {
                    IsUseDynamicResource = false;
                    _port = int.Parse(hostAddress[1]);
                    if (_port <= 0 || _port > 65535) throw new System.Exception("#Illegal TCP port!");
                }
                _serviceName = serviceName;
                if (_hostAddress.ToLower() == "localhost" || _hostAddress == "127.0.0.1")
                {
                    _hostAddress = "127.0.0.1";
                    _isHost = true;
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        #endregion
    }
}