using System;
using System.IO.Pipes;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     �ͻ���Pipe������ṩ����صĻ���������
    /// </summary>
    public class ClientPipeStreamTransaction : PipeStreamTransaction<NamedPipeClientStream>
    {
        #region ���캯��

        /// <summary>
        ///     ����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="stream">��</param>
        /// <param name="callback">�ص�����</param>
        public ClientPipeStreamTransaction(NamedPipeClientStream stream, Action<byte[]> callback)
            : base(stream, callback)
        {
        }

        /// <summary>
        ///     ����������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="stream">��</param>
        /// <param name="canAsync">�첽��ʾ</param>
        /// <param name="callback">�ص�����</param>
        public ClientPipeStreamTransaction(NamedPipeClientStream stream, bool canAsync, Action<byte[]> callback)
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
            if (_stream != null)
            {
                _stream.Close();
            }
            //notify fail event.
            DisconnectedHandler(null);
        }

        #endregion
    }
}