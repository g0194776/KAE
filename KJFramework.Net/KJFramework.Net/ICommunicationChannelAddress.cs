using System.Net;

namespace KJFramework.Net
{
    /// <summary>
    ///     ͨѶͨ����ַԪ�ӿڣ��ṩ����صĻ������Խṹ��
    /// </summary>
    public interface ICommunicationChannelAddress
    {
        /// <summary>
        ///     ��ȡ�����������ַ
        /// </summary>
        IPEndPoint Address { get; set; }
        /// <summary>
        ///     ��ȡ�������߼���ַ
        /// </summary>
        Uri.Uri LogicalAddress { get; set; }
    }
}