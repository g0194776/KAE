using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Statistics;
using KJFramework.Net.EventArgs;
using KJFramework.Net.Listener;
using KJFramework.Net.Listener.Asynchronous;
using KJFramework.Statistics;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Net;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     基于TCP协议的宿主传输通道，提供了相关的基本操作。
    /// </summary>
    public class TcpHostTransportChannel : HostTransportChannel
    {
        #region 构造函数

        /// <summary>
        ///     基于TCP协议的宿主传输通道，提供了相关的基本操作。
        /// </summary>
        /// <param name="port" type="int">
        ///     <para>
        ///         监听的端口
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentException">参数错误</exception>
        public TcpHostTransportChannel(int port)
        {
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentException("Illegal host port. #port: " + port);
            }
            _port = port;
            #if (DEBUG)
            {
                _statistics = new Dictionary<StatisticTypes, IStatistic>();
                TcpHostTransportChannelStatistic statistic = new TcpHostTransportChannelStatistic();
                statistic.Initialize(this);
                _statistics.Add(StatisticTypes.Network, statistic);
            }
            #endif      
        }

        #endregion

        #region Members

        protected readonly int _port;
        protected BasicTcpAsynListenerV2<BasicPortListenerInfomation> _listener;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (TcpHostTransportChannel));

        /// <summary>
        ///     获取监听的端口
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        #endregion

        #region 事件

        void ListenerConnected(object sender, IocpPortListenerConnectedEventArgs<BasicPortListenerInfomation> e)
        {
            TcpTransportChannel tcpTransportChannel = new TcpTransportChannel(e.ConnectStream);
            //care disconnect event.
            tcpTransportChannel.Disconnected += TransportChannelDisconnected;
            ChannelCreatedHandler(new LightSingleArgEventArgs<ITransportChannel>(tcpTransportChannel));
        }

        void TransportChannelDisconnected(object sender, System.EventArgs e)
        {
            ITransportChannel transportChannel = (ITransportChannel)sender;
            transportChannel.Disconnected -= TransportChannelDisconnected;
            ChannelDisconnectedHandler(new LightSingleArgEventArgs<ITransportChannel>(transportChannel));
        }

        #endregion

        #region Overrides of HostTransportChannel

        /// <summary>
        ///     注册网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool Regist()
        {
            try
            {
                if (_listener == null)
                {
                    _listener = new BasicTcpAsynListenerV2<BasicPortListenerInfomation>(_port);
                    _listener.Start();
                    if (_listener.State) _listener.Connected += ListenerConnected;
                }
                return _listener.State;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return false;
            }
        }

        /// <summary>
        ///     注销网络
        /// </summary>
        /// <returns>返回注册的状态</returns>
        public override bool UnRegist()
        {
            try
            {
                if (_listener != null)
                {
                    _listener.Connected -= ListenerConnected;
                    _listener.Stop();
                    _listener = null;
                    return true;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return false;
            }
        }

        #endregion
    }
}