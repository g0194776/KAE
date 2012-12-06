using System;
using System.IO.Pipes;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     �����Pipe������ṩ����صĻ���������
    /// </summary>
    public class ServerPipeStreamTransaction : PipeStreamTransaction<NamedPipeServerStream>
    {
        #region ���캯��

        /// <summary>
        ///     ����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="stream">��</param>
        /// <param name="callback">�ص�����</param>
        public ServerPipeStreamTransaction(NamedPipeServerStream stream, Action<byte[]> callback)
            : base(stream, callback)
        {
        }

        /// <summary>
        ///     ����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="stream">��</param>
        /// <param name="canAsync">�첽��ʾ</param>
        /// <param name="callback">�ص�����</param>
        public ServerPipeStreamTransaction(NamedPipeServerStream stream, bool canAsync, Action<byte[]> callback)
            : base(stream, canAsync, callback)
        {
        }

        #endregion

        #region Overrides of PipeStreamTransaction

        /// <summary>
        ///     ֹͣ����
        ///     <para>* �˷����������쳣���߽���������ʱ�򽫻ᱻ���á�</para>
        /// </summary>
        protected override void InnerEndWork()
        {
            if (_stream != null && _stream.IsConnected) _stream.Disconnect();
            DisconnectedHandler(null);
        }

        #endregion
    }
}