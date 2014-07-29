using System;
using System.Net;
using KJFramework.ApplicationEngine.Eums;

namespace KJFramework.ApplicationEngine.Rings
{
    /// <summary>
    ///    KAE宿主的网络节点对象
    /// </summary>
    internal sealed class KAEHostNode
    {
        #region Constructor

        /// <summary>
        ///    KAE宿主的网络节点对象
        /// </summary>
        /// <param name="address">远程KAE宿主终结点地址</param>
        public KAEHostNode(string address)
        {
            RawAddress = address;
            string newAddress = address.Substring(address.LastIndexOf("://") + 3);
            string[] contents = newAddress.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            EndPoint = new IPEndPoint(IPAddress.Parse(contents[0]), int.Parse(contents[1]));
        }

        #endregion

        #region Members.

        private readonly string _address;
        /// <summary>
        ///    获取网络终端地址
        /// </summary>
        public IPEndPoint EndPoint { get; private set; }
        /// <summary>
        ///    获取当前KAE宿主所支持的网络协议类型
        /// </summary>
        public ProtocolTypes Protocol { get; private set; }
        /// <summary>
        ///    获取远程通信地址的原始形态链接
        /// </summary>
        public string RawAddress { get; private set; }


        #endregion

        #region Methods.

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return _address;
        }

        #endregion
    }
}