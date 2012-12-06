using System;
using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;
using KJFramework.Statistics;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     ��������ͨ�������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class HostTransportChannel : IHostTransportChannel
    {
        #region Constructor

        /// <summary>
        ///     ��������ͨ�������࣬�ṩ����صĻ���������
        /// </summary>
        protected HostTransportChannel()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region Members

        protected readonly Guid _id;
        protected Dictionary<StatisticTypes, IStatistic> _statistics;

        #endregion

        #region Implementation of IHostTransportChannel

        /// <summary>
        ///     ��ȡΨһ��ʶ
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }
        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        public abstract bool Regist();
        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        public abstract bool UnRegist();

        #endregion

        #region �¼�

        /// <summary>
        ///     ����ͨ���¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        protected void ChannelCreatedHandler(LightSingleArgEventArgs<ITransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<ITransportChannel>> created = ChannelCreated;
            if (created != null) created(this, e);
        }
        /// <summary>
        ///     �Ͽ�ͨ���¼�
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelDisconnected;
        protected void ChannelDisconnectedHandler(LightSingleArgEventArgs<ITransportChannel> e)
        {
            EventHandler<LightSingleArgEventArgs<ITransportChannel>> handler = ChannelDisconnected;
            if (handler != null) handler(this, e);
        }

        #endregion
    }
}