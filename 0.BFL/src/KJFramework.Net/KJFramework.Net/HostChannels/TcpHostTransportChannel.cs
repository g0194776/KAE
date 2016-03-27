using System;
using System.Collections.Generic;
using System.Net;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Net.EventArgs;
using KJFramework.Net.Listener;
using KJFramework.Net.Listener.Asynchronous;
using KJFramework.Net.Statistics;
using KJFramework.Statistics;
using KJFramework.Tracing;

namespace KJFramework.Net.HostChannels
{
    /// <summary>
    ///     ����TCPЭ�����������ͨ�����ṩ����صĻ���������
    /// </summary>
    public class TcpHostTransportChannel : HostTransportChannel
    {
        #region ���캯��

        /// <summary>
        ///     ����TCPЭ�����������ͨ�����ṩ����صĻ���������
        /// </summary>
        /// <param name="port" type="int">
        ///     <para>
        ///         �����Ķ˿�
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentException">��������</exception>
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
        ///     ��ȡ�����Ķ˿�
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        #endregion

        #region �¼�

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
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
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
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
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