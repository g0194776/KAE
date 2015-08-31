using KJFramework.Net.Caches;

namespace KJFramework.Net.Events
{
    /// <summary>
    ///     接收到内存片段的事件
    /// </summary>
    public abstract class SegmentReceiveEventArgs : System.EventArgs
    {
        #region Constructor

        /// <summary>
        ///     接收到内存片段的事件
        /// </summary>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        protected SegmentReceiveEventArgs(int bytesTransferred)
        {
            BytesTransferred = bytesTransferred;
        }

        #endregion

        #region Members
        /// <summary>
        ///     获取接收到的数据真实长度
        /// </summary>
        public int BytesTransferred { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    完成当前处理
        /// </summary>
        public abstract void Complete();
        /// <summary>
        ///    获取内部的缓冲区存根
        /// </summary>
        /// <returns>返回缓冲区存根</returns>
        public abstract BuffStub GetStub();

        #endregion
    }
}