using System.Diagnostics;

namespace KJFramework.Counters
{
    /// <summary>
    ///     空的轻量级性能计数器
    /// </summary>
    public sealed class EmptyLightPerfCounter : ILightPerfCounter
    {
        #region Constructor

        /// <summary>
        ///     空的轻量级性能计数器
        /// </summary>
        public EmptyLightPerfCounter()
        {
            Name = "KJFramework.LightPerfCounter.Empty";
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取性能计数器的类型
        /// </summary>
        public PerformanceCounterType CounterType { get; private set; }
        /// <summary>
        ///    获取性能计数器名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        ///    获取性能计数器帮助信息
        /// </summary>
        public string Help { get; private set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    递增一个数字
        /// </summary>
        public void Increment()
        {
        }

        /// <summary>
        ///    递减一个值
        /// </summary>
        public void Decrement()
        {
        }

        /// <summary>
        ///    获取当前性能计数器的值
        /// </summary>
        /// <returns>返回当前性能计数器的值</returns>
        public float GetValue()
        {
            return 0F;
        }

        #endregion
    }
}