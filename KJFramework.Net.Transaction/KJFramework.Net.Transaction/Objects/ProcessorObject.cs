using System.Collections.Generic;
using KJFramework.Net.Transaction.Processors;
using KJFramework.PerformanceProvider;

namespace KJFramework.Net.Transaction.Objects
{
    /// <summary>
    ///     处理器对象信息集合
    /// </summary>
    internal class ProcessorObject
    {
        #region Members

        /// <summary>
        ///     获取或设置处理器
        /// </summary>
        public IMessageProcessor Processor { get; set; }
        /// <summary>
        ///     获取或设置内部性能计数器集合
        /// </summary>
        public IList<PerfCounter> Counters { get; set; }

        #endregion
    }
}