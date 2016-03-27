using System.Diagnostics;

namespace KJFramework.Counters
{
    /// <summary>
    ///    轻量级性能计数器接口，只记录在内存中不注册WIN32对象
    /// </summary>
    public interface ILightPerfCounter
    {
        #region Members.

        /// <summary>
        ///    获取性能计数器的类型
        /// </summary>
        PerformanceCounterType CounterType { get; }
        /// <summary>
        ///    获取性能计数器名称
        /// </summary>
        string Name { get; }
        /// <summary>
        ///    获取性能计数器帮助信息
        /// </summary>
        string Help { get; }
        /// <summary>
        ///    递增一个数字
        /// </summary>
        void Increment();
        /// <summary>
        ///    递减一个值
        /// </summary>
        void Decrement();
        /// <summary>
        ///    获取当前性能计数器的值
        /// </summary>
        /// <returns>返回当前性能计数器的值</returns>
        float GetValue();

        #endregion
    }
}