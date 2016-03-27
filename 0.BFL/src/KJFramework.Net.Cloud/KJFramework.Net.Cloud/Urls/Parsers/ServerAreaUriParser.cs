using System;

namespace KJFramework.Net.Cloud.Urls.Parsers
{
    /// <summary>
    ///   ��������Դ��ַ���������࣬�ṩ����صĻ���������
    /// </summary>
    public abstract class ServerAreaUriParser : IServerAreaUriParser
    {
        #region ��������

        ~ServerAreaUriParser()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected string _protocolKey;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServerAreaUriParser

        /// <summary>
        ///   ��ȡ֧�ֵ�Э���ֵ
        /// </summary>
        public string ProtocolKey
        {
            get { return _protocolKey; }
        }

        /// <summary>
        ///   �Ӹ�������Դ�н�����һ�����������ӵ�ַ����
        /// </summary>
        /// <param name="uri">������Դ��ַͷ����Ĳ���</param>
        /// <param name="keys">�ؼ��ֽ��</param>
        /// <returns>���������ӵ�ַ����</returns>
        public abstract IServerAreaUri Parse(string uri, string[] keys);

        #endregion
    }
}