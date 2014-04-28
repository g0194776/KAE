using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Resources;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Channels.HostChannels;
using KJFramework.Net.Channels.Uri;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.ApplicationEngine.Helpers
{
    /// <summary>
    ///     网络帮助器
    /// </summary>
    internal static class NetworkHelper
    {
        #region Members.

        private static readonly List<int> _usedPorts = new List<int>();
        private static readonly MetadataProtocolStack _metadataPk = new MetadataProtocolStack();
        private static readonly BusinessProtocolStack _intellegencePk = new BusinessProtocolStack();
        private static readonly MessageTransactionManager _intellegenceTm = new MessageTransactionManager(new TransactionIdentityComparer());

        #endregion

        #region Methods.

        /// <summary>
        ///     根据KAE网络资源来构建一个网络宿主信道
        /// </summary>
        /// <param name="resource">KAE网络资源</param>
        /// <returns>返回构建后的宿主信道</returns>
        /// <exception cref="NotSupportedException">不支持的网络类型</exception>
        public static IHostTransportChannel BuildHostChannel(KAENetworkResource resource)
        {
            IHostTransportChannel channel;
            switch (resource.NetworkUri.NetworkType)
            {
                case NetworkTypes.Pipe:
                    channel = new PipeHostTransportChannel((PipeUri) resource.NetworkUri, 254);
                    break;
                case NetworkTypes.TCP:
                    channel = new TcpHostTransportChannel((((TcpUri)resource.NetworkUri).IsUseDynamicResource ? GetDynamicTCPPort() : ((TcpUri)resource.NetworkUri).Port));
                    break;
                default: throw new NotSupportedException("#Sadly, current network type wasn't supported yet! #Network Type: " + resource.NetworkUri.NetworkType);
            }
            return channel;
        }

        /// <summary>
        ///     构建KAE宿主的默认宿主通信信道
        /// </summary>
        /// <returns>返回构建后的宿主信道</returns>
        public static IHostTransportChannel BuildDefaultHostChannel()
        {
            return new TcpHostTransportChannel(GetDynamicTCPPort());
        }

        /// <summary>
        ///    获取一个动态而且可用的TCP端口资源
        /// </summary>
        /// <returns>返回一个可用的TCP端口资源</returns>
        public static int GetDynamicTCPPort()
        {
            List<int> ports = new List<int>(Enumerable.Range(5000, 65535));
            foreach (int usedPort in _usedPorts) ports.Remove(usedPort);
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            IEnumerator myEnum = tcpConnInfoArray.GetEnumerator();
            while (myEnum.MoveNext()) ports.Remove(((IPEndPoint) myEnum.Current).Port);
            _usedPorts.Add(ports[0]);
            return ports[0];
        }

        /// <summary>
        ///    获取一个动态而且可用的UDP端口资源
        /// </summary>
        /// <returns>返回一个可用的UDP端口资源</returns>
        public static int GetDynamicUDPPort()
        {
            List<int> ports = new List<int>(Enumerable.Range(5000, 65535));
            foreach (int usedPort in _usedPorts) ports.Remove(usedPort);
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveUdpListeners();
            IEnumerator myEnum = tcpConnInfoArray.GetEnumerator();
            while (myEnum.MoveNext()) ports.Remove(((IPEndPoint)myEnum.Current).Port);
            _usedPorts.Add(ports[0]);
            return ports[0];
        }

        /// <summary>
        ///     获取不同协议类型的网络协议栈
        /// </summary>
        /// <param name="protocol">协议类型</param>
        /// <returns>返回相应的协议栈</returns>
        /// <exception cref="NotSupportedException">不支持的协议类型</exception>
        public static object GetProtocolStack(ProtocolTypes protocol)
        {
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    return _metadataPk;
                case ProtocolTypes.Intellegence:
                    return _intellegencePk;
                default:
                    throw new NotSupportedException(string.Format("#We've not supported current protocol: {0}!", protocol));
            }
        }

        /// <summary>
        ///     获取不同协议类型的网络协议栈
        /// </summary>
        /// <param name="protocol">协议类型</param>
        /// <returns>返回相应的协议栈</returns>
        /// <exception cref="NotSupportedException">不支持的协议类型</exception>
        public static object GetTransactionManager(ProtocolTypes protocol)
        {
            switch (protocol)
            {
                case ProtocolTypes.Metadata:
                    return _metadataPk;
                case ProtocolTypes.Intellegence:
                    return _intellegencePk;
                default:
                    throw new NotSupportedException(string.Format("#We've not supported current protocol: {0}!", protocol));
            }
        }

        #endregion
    }
}