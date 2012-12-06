using System.Collections.Specialized;
using System.Net;
using System.Text;
using KJFramework.Net.Channels.Enums;

namespace KJFramework.Net.Channels
{
    /// <summary>
    ///     ����HTTPЭ��Ĵ����ŵ�Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface IHttpTransportChannel : IRawTransportChannel
    {
        /// <summary>
        ///     ��ȡ�ŵ�����
        /// </summary>
        HttpChannelTypes ChannelType { get; }
        /// <summary>
        ///     ��ȡ�����������е��������ݵĳ���
        /// </summary>
        long ContentLength64 { get; }
        /// <summary>
        ///     ��ȡ�����÷��ظ��ͻ��˵� HTTP ״̬����
        /// </summary>
        int StatusCode { get; set; }
        /// <summary>
        ///     ��ȡ�ͻ�������� URL ��Ϣ�������������Ͷ˿ڣ�
        /// </summary>
        string RawUrl { get; }
        /// <summary>
        ///     ��ȡ��������Ŀͻ��� IP ��ַ�Ͷ˿ں�
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }
        /// <summary>
        ///     ��ȡ�ͻ�������� Uri ����
        /// </summary>
        System.Uri Url { get; }
        /// <summary>
        ///     ��ȡ���󱻶��򵽵ķ����� IP ��ַ�Ͷ˿ں�
        /// </summary>
        string UserHostAddress { get; }
        /// <summary>
        ///     ��ȡ�ɿͻ���ָ���� HTTP ����
        /// </summary>
        string HttpMethod { get; }
        /// <summary>
        ///     ��ȡ�ͻ��˽��ܵ� MIME ����
        /// </summary>
        string[] AcceptTypes { get; }
        /// <summary>
        ///     ��ȡ�����������е��������ݵ� MIME ����
        /// </summary>
        string ContentType { get; }
        /// <summary>
        ///     ��ȡһ�� Boolean ֵ����ֵָʾ�ͻ����Ƿ��������������
        /// </summary>
        bool KeepAlive { get; }
        /// <summary>
        ///     ��ȡ Boolean ֵ����ֵָʾ�������Ƿ����Ա��ؼ����
        /// </summary>
        bool IsLocal { get; }
        /// <summary>
        ///     ��ȡһ�� Boolean ֵ����ֵָʾ�����Ƿ��й�������������
        /// </summary>
        bool HasEntityBody { get; }
        /// <summary>
        ///     ��ȡ�������з��͵ı�ͷ����/ֵ�Եļ���
        /// </summary>
        NameValueCollection Headers { get; }
        /// <summary>
        ///     ��ȡ�����������еĲ�ѯ�ַ���
        /// </summary>
        NameValueCollection QueryString { get; }
        /// <summary>
        ///     ��ȡ�������͵� Cookie
        /// </summary>
        CookieCollection Cookies { get; }
        /// <summary>
        ///     ��ȡ�������������͵����ݵ����ݱ���
        /// </summary>
        Encoding ContentEncoding { get; }
        /// <summary>
        ///     ����HTTP����
        /// </summary>
        void Send();
        /// <summary>
        ///     ��ȡ�ڲ������������
        /// </summary>
        /// <returns>�����������</returns>
        HttpListenerRequest GetRequest();
        /// <summary>
        ///     ��ȡ�ڲ����Ļ�������
        /// </summary>
        /// <returns>���ػ�������</returns>
        HttpListenerResponse GetResponse();
    }
}