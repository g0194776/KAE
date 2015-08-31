using System.Collections.Generic;
using KJFramework.Exceptions;

namespace KJFramework.Cores.Segments
{
    /// <summary>
    ///     片段式缓存分布策略，提供了相关的基本操作
    /// </summary>
    public class SegmentCachePolicy : ISegmentCachePolicy
    {
        #region Constructor

        /// <summary>
        ///     片段式缓存分布策略，提供了相关的基本操作
        /// </summary>
        public SegmentCachePolicy()
        { }

        /// <summary>
        ///     片段式缓存分布策略，提供了相关的基本操作
        /// </summary>
        /// <param name="pairs">片段分布集合</param>
        public SegmentCachePolicy(params SegmentSizePair[] pairs)
        {
            foreach (SegmentSizePair segmentSizePair in pairs)
            {
                Set(segmentSizePair.Size, (decimal) segmentSizePair.Percent);
            }
        }

        #endregion

        #region Implementation of ISegmentCachePolicy

        protected int _segmentLevel;
        private List<SegmentSizePair> _pairs = new List<SegmentSizePair>();
        private decimal _currentPercent;

        /// <summary>
        ///     获取片段分布等级
        /// </summary>
        public int SegmentLevel
        {
            get { return _segmentLevel; }
        }

        /// <summary>
        ///     设置一个片段分布策略
        ///     <para>* 此方法将会把剩余的片段百分比全都分给当前的大小</para>
        /// </summary>
        /// <param name="size">片段大小</param>
        /// <exception cref="OutOfRangeException">超出预定的范围</exception>
        public void Set(int size)
        {
            Set(size, 1 - _currentPercent);
        }

        /// <summary>
        ///     设置一个片段分布策略
        /// </summary>
        /// <param name="size">片段大小</param>
        /// <param name="percent">占用总体内存的百分比</param>
        /// <exception cref="OutOfRangeException">超出预定的范围</exception>
        public void Set(int size, decimal percent)
        {
            if (size <= 0)
            {
                throw new OutOfRangeException("Illegal cache size.");
            }
            if (percent <= 0 || percent > 1)
            {
                throw new OutOfRangeException("Illegal cache percent.");
            }
            if (_currentPercent == 1 || (_currentPercent + percent) > 1)
            {
                throw new OutOfRangeException("Cannot set a segment cache policy ,because current percent is full(100%).");
            }
            _currentPercent += percent;
            _segmentLevel++;
            _pairs.Add(new SegmentSizePair{Percent = (float) percent, Size = size});
        }

        /// <summary>
        ///     获取所有的片段分布
        /// </summary>
        /// <returns>返回片段分布集合</returns>
        public List<SegmentSizePair> Get()
        {
            return _pairs;
        }

        #endregion
    }
}