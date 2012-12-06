using System;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   �������ַԪ�ӿڣ��ṩ����صĻ������Խṹ��
    /// </summary>
    public interface IServerAreaUri : IDisposable
    {
        /// <summary>
        ///   ����һ�������ŵ�
        /// </summary>
        /// <returns>��������ͨ��</returns>
        IHostTransportChannel CreateHostChannel();
        /// <summary>
        ///   ����һ��ͨѶ�ŵ�
        /// </summary>
        /// <returns>����ͨѶ�ŵ�</returns>
        ITransportChannel CreateTransportChannel();
    }
}