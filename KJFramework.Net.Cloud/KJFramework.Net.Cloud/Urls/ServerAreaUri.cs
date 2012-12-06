using System;
using KJFramework.Net.Cloud.Urls.Parsers;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.HostChannels;

namespace KJFramework.Net.Cloud.Urls
{
    /// <summary>
    ///   �������ַ���ṩ����صĻ������Խṹ��
    /// </summary>
    public abstract class ServerAreaUri : IServerAreaUri
    {
        #region ��������

        ~ServerAreaUri()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServerAreaUri

        /// <summary>
        ///   ����һ�������ŵ�
        /// </summary>
        /// <returns>��������ͨ��</returns>
        public abstract IHostTransportChannel CreateHostChannel();
        /// <summary>
        ///   ����һ��ͨѶ�ŵ�
        /// </summary>
        /// <returns>����ͨѶ�ŵ�</returns>
        public abstract ITransportChannel CreateTransportChannel();

        #endregion

        #region Static Functions

        /// <summary>
        ///   ͨ��һ����Դ��ַ������Ӧ�ķ��������ӵ�ַ����
        /// </summary>
        /// <param name="uri">��Դ��ַ</param>
        /// <returns>���ط��������ӵ�ַ����</returns>
        public static IServerAreaUri Create(String uri)
        {
            if (String.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            string[] totalChunk = uri.Split(new [] {"://"}, StringSplitOptions.RemoveEmptyEntries);
            if (totalChunk.Length != 2)
            {
                throw new System.Exception("�Ƿ�����Դ��ַ��");
            }
            string[] keys = totalChunk[0].Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (keys.Length != 2)
            {
                throw new System.Exception("�Ƿ�����Դ��ַ��");
            }
            IServerAreaUriParser parser = ServeAreaUriParsers.Instance.GetParser(keys[1]);
            if (parser == null)
            {
                throw new System.Exception("�޷��ҵ���Ӧ����Դ��ַ���������Ƿ�����Դ��ַ��");
            }
            return parser.Parse(totalChunk[1], keys);
        }

        #endregion
    }
}