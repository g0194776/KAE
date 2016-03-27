using KJFramework.Cores;
using KJFramework.Net.Caches;

namespace KJFramework.Net.Events
{
    /// <summary>
    ///     Socket接收到内存片段的事件
    /// </summary>
    public class SocketSegmentReceiveEventArgs : SegmentReceiveEventArgs
    {
        #region Constructor

        /// <summary>
        ///     Socket接收到内存片段的事件
        /// </summary>
        /// <param name="stub">带缓冲区的固定缓存存根</param>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        public SocketSegmentReceiveEventArgs(IFixedCacheStub<SocketBuffStub> stub, int bytesTransferred)
            : base(bytesTransferred)
        {
            Stub = stub;
        }

        #endregion

        #region Members.

        /// <summary>
        ///     获取带缓冲区的固定缓存存根
        /// </summary>
        public IFixedCacheStub<SocketBuffStub> Stub { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    完成当前处理
        /// </summary>
        public override void Complete()
        {
            ChannelConst.BuffAsyncStubPool.Giveback(Stub);
            ChannelCounter.Instance.RateOfFixedBufferStubGiveback.Increment();
        }

        /// <summary>
        ///    获取内部的缓冲区存根
        /// </summary>
        /// <returns>返回缓冲区存根</returns>
        public override BuffStub GetStub()
        {
            return Stub.Cache;
        }

        #endregion
    }
}