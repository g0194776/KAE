using KJFramework.Cores;
using KJFramework.Net.Caches;

namespace KJFramework.Net.Events
{
    /// <summary>
    ///     命名管道接收到内存片段的事件
    /// </summary>
    public class NamedPipeSegmentReceiveEventArgs : SegmentReceiveEventArgs
    {
        #region Constructor

        /// <summary>
        ///     命名管道接收到内存片段的事件
        /// </summary>
        /// <param name="bytesTransferred">接收到的数据长度</param>
        public NamedPipeSegmentReceiveEventArgs(IFixedCacheStub<BuffStub> stub, int bytesTransferred)
            : base(bytesTransferred)
        {
            Stub = stub;
        }

        #endregion

        #region Members.

        /// <summary>
        ///     获取带缓冲区的固定缓存存根
        /// </summary>
        public IFixedCacheStub<BuffStub> Stub { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    完成当前处理
        /// </summary>
        public override void Complete()
        {
            ChannelConst.NamedPipeBuffPool.Giveback(Stub);
            ChannelCounter.Instance.RateOfNamedPipeBufferStubGiveback.Increment();
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