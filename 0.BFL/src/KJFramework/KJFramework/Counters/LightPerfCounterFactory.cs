using System;
using System.Diagnostics;

namespace KJFramework.Counters
{
    /// <summary>
    ///    轻量级性能计数器工厂
    /// </summary>
    public static class LightPerfCounterFactory
    {
        #region Members.

        private static readonly EmptyLightPerfCounter _emptyPerfCounter = new EmptyLightPerfCounter();

        #endregion

        #region Methods.

        /// <summary>
        ///    创建一个新的轻量级性能计数器
        /// </summary>
        /// <param name="counterType">性能计数器类型</param>
        /// <param name="name">性能计数器名称</param>
        /// <param name="help">性能计数器帮助信息</param>
        /// <returns>返回创建好的轻量级性能计数器</returns>
        /// <exception cref="ArgumentNullException">性能计数器名称不能为空</exception>
        public static ILightPerfCounter CreateNew(PerformanceCounterType counterType, string name, string help)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            switch (counterType)
            {
                default:
                    return _emptyPerfCounter;
            }
        }

        #endregion
    }
}