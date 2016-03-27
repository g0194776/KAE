using System.Net.Sockets;

namespace KJFramework.Net
{
    /// <summary>
    ///     ����TCPЭ��Ĵ���ͨ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    internal interface ITcpTransportChannel : ITransportChannel
    {
        /// <summary>
        ///     ��ȡ�ڲ������׽���
        /// </summary>
        /// <returns>�����ڲ������׽���</returns>
        Socket GetStream();
        /// <summary>
        ///     ��ȡ��ǰTCPЭ�鴫��ͨ����Ψһ��ֵ
        /// </summary>
        int ChannelKey { get; }
    }
}