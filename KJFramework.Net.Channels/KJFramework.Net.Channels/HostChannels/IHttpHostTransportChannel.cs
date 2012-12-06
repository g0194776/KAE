using System.Net;

namespace KJFramework.Net.Channels.HostChannels
{
    /// <summary>
    ///     HTTPЭ�������ŵ�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IHttpHostTransportChannel : IHostTransportChannel
    {
        /// <summary>
        ///     ��ȡ�ɴ� HttpListener �������ͳһ��Դ��ʶ�� (URI) ǰ׺
        /// </summary>
        HttpListenerPrefixCollection Prefixes { get; }
        /// <summary>
        ///     ��ȡ��������� HttpListener ����������������Դ����
        /// </summary>
        string Realm { get; set; }
        /// <summary>
        ///     ��ȡ������ Boolean ֵ����ֵ���Ƶ�ʹ�� NTLM ʱ�Ƿ���Ҫ��ʹ��ͬһ�������Э�� (TCP) ���ӵ�����������������֤
        /// </summary>
        bool UnsafeConnectionNtlmAuthentication { get; set; }
        /// <summary>
        ///     ��ȡ������ Boolean ֵ����ֵָ��Ӧ�ó����Ƿ���� HttpListener ��ͻ��˷�����Ӧʱ�������쳣
        /// </summary>
        bool IgnoreWriteExceptions { get; set; }
    }
}