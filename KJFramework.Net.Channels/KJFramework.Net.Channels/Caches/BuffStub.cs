using KJFramework.Cache;
using KJFramework.Cache.Objects;

namespace KJFramework.Net.Channels.Caches
{
    /// <summary>
    ///    缓冲区存根基础类
    /// </summary>
    public class BuffStub : IClearable
    {
        #region Constructor

        /// <summary>
        ///     缓冲区存根基础类
        /// </summary>
        public BuffStub()
        {
            _segment = GlobalMemory.SegmentContainer.Rent();
            if (_segment == null) throw new System.Exception("[BuffSocketStub-Prealloc] #There has no enough MemorySegment can be used.");
            ChannelCounter.Instance.RateOfRentMemSegment.Increment();
        }

        #endregion

        #region Members

        protected readonly IMemorySegment _segment;

        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     获取内部关联的内存数据片段
        /// </summary>
        public IMemorySegment Segment
        {
            get { return _segment; }
        }

        #endregion

        #region Implementation of IClearable

        /// <summary>
        ///     清理资源
        /// </summary>
        public virtual void Clear()
        {
            Tag = null;
            _segment.UsedBytes = 0;
        }

        #endregion
    }
}