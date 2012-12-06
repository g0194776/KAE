using System.Net;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   TCP协议的服务域URI，提供了相关的基本操作
    /// </summary>
    public class TcpServerAreaUri : ServerAreaUri
    {
        #region 构造函数

        /// <summary>
        ///   TCP协议的服务域URI，提供了相关的基本操作
        /// </summary>
        public TcpServerAreaUri()
        {
            
        }

        /// <summary>
        ///   TCP协议的服务域URI，提供了相关的基本操作
        /// </summary>
        /// <param name="address">远程终结点地址</param>
        public TcpServerAreaUri(IPEndPoint address)
        {
            _address = address;
        }

        #endregion

        #region Members

        private IPEndPoint _address;
        /// <summary>
        ///   获取或设置远程终结点地址
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set { _address = value; }
        }

        #endregion

        #region Overrides of ServerAreaUri

        /// <summary>
        ///   创建一个宿主信道
        /// </summary>
        /// <returns>返回宿主通道</returns>
        public override IHostTransportChannel CreateHostChannel()
        {
            return new TcpHostTransportChannel(_address.Port);
        }

        /// <summary>
        ///   创建一个通讯信道
        /// </summary>
        /// <returns>返回通讯信道</returns>
        public override ITransportChannel CreateTransportChannel()
        {
            return new TcpTransportChannel(_address);
        }

        #endregion
    }
}