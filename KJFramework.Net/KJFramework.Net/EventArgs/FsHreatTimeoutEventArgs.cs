using System;

namespace KJFramework.Net.EventArgs
{
    public delegate void DELEGATE_FS_TIMEOUT<I>(Object Sender, FsHreatTimeoutEventArgs<I> e);
    /// <summary>
    ///     FS心跳超时事件
    /// </summary>
    public class FsHreatTimeoutEventArgs<I> : System.EventArgs
    {
        private I _fsobject;
        /// <summary>
        ///     超时的FS连接对象
        /// </summary>
        public I FsObject
        {
            get { return _fsobject; }
        }

        /// <summary>
        ///     FS心跳超时事件
        /// </summary>
        /// <param name="FsObject" type="T">
        ///     <para>
        ///         超时的FS连接对象
        ///     </para>
        /// </param>
        public FsHreatTimeoutEventArgs(I FsObject)
        {
            _fsobject = FsObject;
        }
    }
}
