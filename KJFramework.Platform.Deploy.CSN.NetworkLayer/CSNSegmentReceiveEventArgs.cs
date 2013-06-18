using KJFramework.Cache.Cores;

namespace KJFramework.Platform.Deploy.CSN.NetworkLayer
{
    /// <summary>
    ///     接收到内存片段的事件
    /// </summary>
    public class CSNSegmentReceiveEventArgs : System.EventArgs
    {
        #region Constructor

        /// <summary>
        ///     接收到内存片段的事件
        /// </summary>
        /// <param name="stub">带缓冲区的固定缓存存根</param>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        public CSNSegmentReceiveEventArgs(IFixedCacheStub<CSNBuffSocketStub> stub, int bytesTransferred)
        {
            Stub = stub;
            BytesTransferred = bytesTransferred;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取带缓冲区的固定缓存存根
        /// </summary>
        public IFixedCacheStub<CSNBuffSocketStub> Stub { get; private set; }
        /// <summary>
        ///     获取接收到的数据真实长度
        /// </summary>
        public int BytesTransferred { get; private set; }

        #endregion
    }
}