using System;
using System.Net;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   TCPЭ��ķ�������Դ��ַ���������ṩ����صĻ���������
    /// </summary>
    public class ServerAreaTcpUriParser : ServerAreaUriParser
    {
        #region ���캯��

        /// <summary>
        ///   TCPЭ��ķ�������Դ��ַ���������ṩ����صĻ���������
        /// </summary>
        public ServerAreaTcpUriParser()
        {
            _protocolKey = "tcp";
        }


        #endregion

        #region Overrides of ServerAreaUriParser

        /// <summary>
        ///   �Ӹ�������Դ�н�����һ�����������ӵ�ַ����
        /// </summary>
        /// <param name="uri">������Դ��ַͷ����Ĳ���</param>
        /// <param name="keys">�ؼ��ֽ��</param>
        /// <returns>���������ӵ�ַ����</returns>
        public override IServerAreaUri Parse(string uri, string[] keys)
        {
            if (String.IsNullOrEmpty(uri) || keys == null || keys.Length != 2)
            {
                throw new System.Exception("�Ƿ��Ľ���������");
            }
            String[] resources = uri.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
            if (resources.Length != 2)
            {
                throw new System.Exception("�Ƿ�����Դ��ַ��");
            }
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(resources[0]), int.Parse(resources[1]));
            return new TcpServerAreaUri(iep);
        }

        #endregion
    }
}