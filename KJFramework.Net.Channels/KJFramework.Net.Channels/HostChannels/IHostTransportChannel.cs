using System;
using KJFramework.EventArgs;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     ��������ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IHostTransportChannel
    {
        /// <summary>
        ///     ��ȡΨһ��ʶ
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        bool Regist();
        /// <summary>
        ///     ע������
        /// </summary>
        /// <returns>����ע���״̬</returns>
        bool UnRegist();
        /// <summary>
        ///     ����ͨ���¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelCreated;
        /// <summary>
        ///     ͨ���Ͽ��¼�
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<ITransportChannel>> ChannelDisconnected;
    }
}