using System;
using KJFramework.Net.Channels.Uri;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     �����ܵ�ͨѶͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface IPiepHostTransportChannel : IHostTransportChannel
    {
        /// <summary>
        ///     ��ȡʵ������
        /// </summary>
        int InstanceCount { get; }
        /// <summary>
        ///     ��ȡ�����ܵ���ʵ������
        /// </summary>
        String Name { get; }
        /// <summary>
        ///     ��ȡ�����Ĺܵ���ַ
        /// </summary>
        PipeUri LogicalAddress { get; }
    }
}