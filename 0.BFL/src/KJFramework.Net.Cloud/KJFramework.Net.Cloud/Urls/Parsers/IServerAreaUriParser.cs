using System;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   ��������Դ��ַ���������ṩ����صĻ���������
    /// </summary>
    public interface IServerAreaUriParser : IDisposable
    {
        /// <summary>
        ///   ��ȡ֧�ֵ�Э���ֵ
        /// </summary>
        String ProtocolKey { get; }
        /// <summary>
        ///   �Ӹ�������Դ�н�����һ�����������ӵ�ַ����
        /// </summary>
        /// <param name="uri">������Դ��ַͷ����Ĳ���</param>
        /// <param name="keys">�ؼ��ֽ��</param>
        /// <returns>���������ӵ�ַ����</returns>
        IServerAreaUri Parse(String uri, String[] keys);
    }
}