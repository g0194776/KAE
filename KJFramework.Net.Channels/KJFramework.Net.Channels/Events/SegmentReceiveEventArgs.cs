using KJFramework.Cache.Cores;
using KJFramework.Net.Channels.Caches;

namespace KJFramework.Net.Channels.Events
{
    /// <summary>
    ///     接收到内存片段的事件
    /// </summary>
    public class SegmentReceiveEventArgs : System.EventArgs
    {
        #region Constructor

        /// <summary>
        ///     接收到内存片段的事件
        /// </summary>
        /// <param name="stub">带缓冲区的固定缓存存根</param>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        public SegmentReceiveEventArgs(IFixedCacheStub<BuffSocketStub> stub, int bytesTransferred)
        {
            Stub = stub;
            BytesTransferred = bytesTransferred;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取带缓冲区的固定缓存存根
        /// </summary>
        public IFixedCacheStub<BuffSocketStub> Stub { get; private set; }
        /// <summary>
        ///     获取接收到的数据真实长度
        /// </summary>
        public int BytesTransferred { get; private set; }

        #endregion
    }
}