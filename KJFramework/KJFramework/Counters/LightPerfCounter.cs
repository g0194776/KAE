using System;
using System.Diagnostics;

namespace KJFramework.Counters
{
    /// <summary>
    ///    轻量级性能计数器
    /// </summary>
    public abstract class LightPerfCounter : ILightPerfCounter
    {
        #region Constructor.

        /// <summary>
        ///    轻量级性能计数器
        /// </summary>
        /// <param name="name">性能计数器名称</param>
        /// <param name="help">性能计数器描述信息</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        protected LightPerfCounter(string name, string help)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(help)) throw new ArgumentNullException("help");
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取性能计数器的类型
        /// </summary>
        public PerformanceCounterType CounterType { get; protected set; }
        /// <summary>
        ///    获取性能计数器名称
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        ///    获取性能计数器帮助信息
        /// </summary>
        public string Help { get; protected set; }

        #endregion

        #region Methods.

        /// <summary>
        ///    递增一个数字
        /// </summary>
        public abstract void Increment();
        /// <summary>
        ///    递减一个值
        /// </summary>
        public abstract void Decrement();
        /// <summary>
        ///    获取当前性能计数器的值
        /// </summary>
        /// <returns>返回当前性能计数器的值</returns>
        public abstract float GetValue();

        #endregion
    }
}