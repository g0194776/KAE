using System;
using System.IO.Pipes;

namespace KJFramework.Net.Channels.Transactions
{
    /// <summary>
    ///     客户端Pipe流事物，提供了相关的基本操作。
    /// </summary>
    public class ClientPipeStreamTransaction : PipeStreamTransaction<NamedPipeClientStream>
    {
        #region 构造函数

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="callback">回调函数</param>
        public ClientPipeStreamTransaction(NamedPipeClientStream stream, Action<byte[]> callback)
            : base(stream, callback)
        {
        }

        /// <summary>
        ///     流事物抽象父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="canAsync">异步标示</param>
        /// <param name="callback">回调函数</param>
        public ClientPipeStreamTransaction(NamedPipeClientStream stream, bool canAsync, Action<byte[]> callback)
            : base(stream, canAsync, callback)
        {
        }

        #endregion

        #region Overrides of PipeStreamTransaction

        /// <summary>
        ///     停止工作
        ///     <para>* 此方法在事物异常或者结束工作的时候将会被调用。</para>
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