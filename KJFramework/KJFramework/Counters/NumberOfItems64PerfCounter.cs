using System;
using System.Threading;

namespace KJFramework.Counters
{
    /// <summary>
    ///    即时计数器，它显示最近观测到的值。 例如，用于维护大量的项或操作的简单计数。 它与 NumberOfItems32 相同，但它使用更大的字段来容纳较大的值
    /// </summary>
    public class NumberOfItems64PerfCounter : LightPerfCounter
    {
        #region Constructor.

        /// <summary>
        ///    即时计数器，它显示最近观测到的值。 例如，用于维护大量的项或操作的简单计数。 它与 NumberOfItems32 相同，但它使用更大的字段来容纳较大的值
        /// </summary>
        /// <param name="name">性能计数器名称</param>
        /// <param name="help">性能计数器描述信息</param>
        public NumberOfItems64PerfCounter(string name, string help)
            : base(name, help)
        {
        }

        #endregion

        #region Members.

        private long _value;

        #endregion

        #region Methods.

        /// <summary>
        ///    递增一个数字
        /// </summary>
        public override void Increment()
        {
            Interlocked.Increment(ref _value);
        }

        /// <summary>
        ///    递减一个值
        /// </summary>
        public override void Decrement()
        {
            Interlocked.Decrement(ref _value);
        }

        /// <summary>
        ///    获取当前性能计数器的值
        /// </summary>
        /// <returns>返回当前性能计数器的值</returns>
        public override float GetValue()
        {
            return _value;
        }

        #endregion
    }
}