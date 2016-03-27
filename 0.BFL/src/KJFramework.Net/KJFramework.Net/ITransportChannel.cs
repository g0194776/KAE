using System;
using System.Net;
using System.Net.Sockets;
using KJFramework.Buffers;
using KJFramework.Net.Enums;

namespace KJFramework.Net
{
    /// <summary>
    ///     ����ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ITransportChannel : IServiceChannel, ICommunicationChannelAddress
    {
        /// <summary>
        ///   ��ȡͨ���ŵ�������
        /// </summary>
        TransportChannelTypes ChannelType { get; }
        /// <summary>
        ///     ��ȡ�����ս���ַ
        /// </summary>
        EndPoint LocalEndPoint { get; }
        /// <summary>
        ///     ��ȡԶ���ս���ַ
        /// </summary>
        EndPoint RemoteEndPoint { get; }
        /// <summary>
        ///   ��ȡ�����û�����
        /// </summary>
        IByteArrayBuffer Buffer { get; set; }
        /// <summary>
        ///     ��ȡ�������ӳ�����
        /// </summary>
        LingerOption LingerState { get; set; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰͨ���Ƿ�������״̬
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        ///     ����
        /// </summary>
        void Connect();
        /// <summary>
        ///     �Ͽ�
        /// </summary>
        void Disconnect();
        /// <summary>
        ///     ��������
        /// </summary>
        /// <param name="data">Ҫ���͵�����</param>
        /// <returns>���ط��͵��ֽ���</returns>
        /// <exception cref="ArgumentNullException">��������</exception>
        int Send(byte[] data);
        /// <summary>
        ///     ͨ���������¼�
        /// </summary>
        event EventHandler Connected;
        /// <summary>
        ///     ͨ���ѶϿ��¼�
        /// </summary>
        event EventHandler Disconnected;
    }
}