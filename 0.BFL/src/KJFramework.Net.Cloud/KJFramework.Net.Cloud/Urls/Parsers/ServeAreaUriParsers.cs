using System;
using System.Collections.Generic;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   ��������Դ��ַ���������ϣ��ṩ����صĻ���������
    /// </summary>
    public class ServeAreaUriParsers
    {
        #region ���캯��

        /// <summary>
        ///   ��������Դ��ַ���������ϣ��ṩ����صĻ���������
        /// </summary>
        private ServeAreaUriParsers()
        {
            Regist(new ServerAreaTcpUriParser());
        }

        #endregion

        #region Members

        private Dictionary<String, IServerAreaUriParser> _parsers = new Dictionary<string, IServerAreaUriParser>();
        /// <summary>
        ///   ��������Դ��ַ���������ϣ��ṩ����صĻ���������
        /// </summary>
        public static readonly ServeAreaUriParsers Instance = new ServeAreaUriParsers();

        #endregion

        #region Functions

        /// <summary>
        ///   ע�������
        ///   <para>* �������ָ����ProtocolKey��������������滻��</para>
        /// </summary>
        /// <param name="parser">������</param>
        public void Regist(IServerAreaUriParser parser)
        {
            if (parser == null || String.IsNullOrEmpty(parser.ProtocolKey))
            {
                throw new System.Exception("�Ƿ��Ľ�����");
            }
            if (_parsers.ContainsKey(parser.ProtocolKey))
            {
                _parsers[parser.ProtocolKey] = parser;
                return;
            }
            _parsers.Add(parser.ProtocolKey, parser);
        }

        /// <summary>
        ///   ͨ��һ��Э��ؼ�������ȡ��Ӧ����Դ��ַ������
        /// </summary>
        /// <param name="protocolKey">Э��ؼ���</param>
        /// <returns>������Դ��ַ������</returns>
        public IServerAreaUriParser GetParser(String protocolKey)
        {
            if (String.IsNullOrEmpty(protocolKey))
            {
                return null;
            }
            return _parsers.ContainsKey(protocolKey) ? _parsers[protocolKey] : null;
        }

        #endregion
    }
}