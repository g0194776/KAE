using System;
using System.Collections.Generic;

namespace KJFramework.Counters
{
    /// <summary>
    ///    轻量级性能计数器分组
    /// </summary>
    public class LightPerfCounterCategory
    {
        #region Members.

        /// <summary>
        ///    获取性能计数器分组名称
        /// </summary>
        public string Name { get; private set; }
        private readonly IDictionary<string, ILightPerfCounter> _counters = new Dictionary<string, ILightPerfCounter>(); 

        #endregion

        #region Methods.

        /// <summary>
        ///    注册一个轻量级性能计数器
        /// </summary>
        /// <param name="counter">轻量级性能计数器</param>
        /// <returns>返回分组实例</returns>
        /// <exception cref="ArgumentNullException">参数或者性能计数器的名称不能为空</exception>
        public LightPerfCounterCategory Register(ILightPerfCounter counter)
        {
            if (counter == null) throw new ArgumentNullException("counter");
            if (string.IsNullOrEmpty(counter.Name)) throw new ArgumentNullException("counter.Name");
            _counters[counter.Name] = counter;
            return this;
        }


        #endregion
    }
}