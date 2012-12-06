using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using KJFramework.Net.Exception;
using System.Collections;
using KJFramework.Core.Native;

namespace KJFramework.Net
{
    /// <summary>
    ///     网络探测器, 提供了指定范围内的可用端口等常用网络探测功能。
    /// </summary>
    public class NetworkDirecter
    {
        #region 成员

        /// <summary>
        ///     可用的端口列表
        /// </summary>
        protected static ArrayList _aviablePortList;

        #endregion
        /// <summary>
        ///     从基础端口开始探测指定协议的可用端口
        ///     截止到指定端口号
        /// </summary>
        /// <param name="baseProt">基础端口号</param>
        /// <param name="endPort">结束探测端口号</param>
        /// <param name="protocol">协议</param> 
        /// <returns>返回-1 则代表无可用端口</returns>
        public static int DirectPort(int baseProt, int endPort, Protocol protocol)
        {
            if ((baseProt > endPort) || baseProt <= 0 || endPort <= 0)
            {
                throw new DirectPortUnCorrectException();
            }
            switch (protocol)
            {
                case Protocol.TCP:
                    _aviablePortList = Native.Network.GetTcpOpenPort();
                    break;
                case Protocol.UDP:
                    _aviablePortList = Native.Network.GetUdpOpenPort();
                    break;
            }

            for (int i = baseProt; i < endPort; i++)
            {
                if (_aviablePortList.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }


        /// <summary>
        ///     端口协议枚举
        /// </summary>
        public enum Protocol
        {
            /// <summary>
            ///     基于TCP协议
            /// </summary>
            TCP,
            /// <summary>
            ///     基于UDP协议
            /// </summary>
            UDP
        }

        /// <summary>
        ///     从基础端口开始探测指定协议的可用端口
        ///     截止到指定端口号
        /// </summary>
        /// <param name="baseProt">基础端口号</param>
        /// <param name="endPort">结束探测端口号</param>
        /// <param name="protocol">协议</param> 
        /// <returns>返回-1 则代表无可用端口</returns>
        public static int DirectPortEx(int baseProt, int endPort, Protocol protocol)
        {
            if ((baseProt > endPort) || baseProt <= 0 || endPort <= 0)
            {
                throw new DirectPortUnCorrectException();
            }
            //fill port array
            int[] ports = new int[endPort - baseProt];
            int offset = 0;
            for (int i = baseProt; i < endPort; i++)
            {
                ports[offset++] = i;
            }
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] avaPorts = protocol == Protocol.TCP
                                        ? ipGlobalProperties.GetActiveTcpListeners()
                                        : ipGlobalProperties.GetActiveUdpListeners();
            if (avaPorts.Length == 0)
            {
                return -1;
            }
            Dictionary<int, string> tempPorts = new Dictionary<int, string>();
            foreach (IPEndPoint t in avaPorts)
            {
                if (!tempPorts.ContainsKey(t.Port))
                {
                    tempPorts.Add(t.Port, null);
                }
            }
            foreach (int t in ports.Where(t => !tempPorts.ContainsKey(t)))
            {
                return t;
            }
            return -1;
        }

        /// <summary>
        ///     查询指定端口是否可用
        /// </summary>
        /// <param name="protocol">网络协议</param>
        /// <param name="port">端口号</param>
        /// <returns>返回一个可用的标示</returns>
        public static bool IsAvailablePort(Protocol protocol, int port)
        {
            if (port <= 0 || port > 65535)
            {
                throw new ArgumentException("Illegal port number. #" + port);
            }
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] avaPorts = protocol == Protocol.TCP
                                        ? ipGlobalProperties.GetActiveTcpListeners()
                                        : ipGlobalProperties.GetActiveUdpListeners();
            if (avaPorts.Length == 0)
            {
                return false;
            }
            bool result = false;
            for (int i = 0; i < avaPorts.Length; i++)
            {
                //has been used.
                if (avaPorts[i].Port == port)
                {
                    result = true;
                    break;
                }
            }
            return result ? false : true;
        }
    }
}
