using KJFramework.Cache.Objects;

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
        /// <param name="segment">内存片段</param>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        public SegmentReceiveEventArgs(IMemorySegment segment, int bytesTransferred)
        {
            Segment = segment;
            BytesTransferred = bytesTransferred;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取接收到的内存片段
        /// </summary>
        public IMemorySegment Segment { get; private set; }
        /// <summary>
        ///     获取接收到的数据真实长度
        /// </summary>
        public int BytesTransferred { get; private set; }

        #endregion
    }
}