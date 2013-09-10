using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using KJFramework.Net.Channels.Disconvery.Protocols;
using Newtonsoft.Json;

namespace KJFramework.Net.Channels.Disconvery
{
    /// <summary>
    ///   探索模式的输出节点
    /// </summary>
    public class DiscoveryOnputPin
    {
        #region Constructors

        /// <summary>
        ///   探索模式的输出节点
        /// </summary>
        /// <param name="port">目标UDP端口</param>
        public DiscoveryOnputPin(int port)
        {
            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort) throw new ArgumentException("#Incorrect input UDP port.");
            _port = port;
            _broadcastIep = new IPEndPoint(IPAddress.Broadcast, _port);
            Initialize();
        }

        #endregion

        #region Members

        private Socket _socket;
        private readonly int _port;
        private readonly IPEndPoint _broadcastIep;

        #endregion

        #region Methods

        /// <summary>
        ///   初始化内部数据
        /// </summary>
        private void Initialize()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
        }

        /// <summary>
        ///   发送一个消息
        /// </summary>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">字段不能没有值</exception>
        /// <exception cref="Exception">内部无法发送出任何数据到目标网络</exception>
        public void Send(CommonBoradcastProtocol obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (string.IsNullOrEmpty(obj.Key)) throw new ArgumentException("#Key cannot be null or empty.");
            if (string.IsNullOrEmpty(obj.Environment)) throw new ArgumentException("#Environment cannot be null or empty.");
            string content = JsonConvert.SerializeObject(obj);
            if (_socket.SendTo(Encoding.UTF8.GetBytes(content), _broadcastIep) <= 0) throw new System.Exception("#Sadly, We had sent nothing to destination network.");
        }

        #endregion
    }
}