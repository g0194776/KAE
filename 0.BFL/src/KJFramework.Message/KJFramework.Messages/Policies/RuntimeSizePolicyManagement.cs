using System;
using System.Collections.Concurrent;

namespace KJFramework.Messages.Policies
{
    /// <summary>
    ///     运行时长度策略管理
    /// </summary>
    public static class RuntimeSizePolicyManagement
    {
        #region Constructor

        /// <summary>
        ///     运行时长度策略管理
        /// </summary>
        static RuntimeSizePolicyManagement()
        {
            _policies.TryAdd(typeof (string).FullName, new StringRuntimeSizeCalcPolicy());
        }

        #endregion

        #region Members

        private static readonly ConcurrentDictionary<string, IRuntimeSizeCalcPolicy> _policies = new ConcurrentDictionary<string, IRuntimeSizeCalcPolicy>();

        #endregion

        #region Methods

        /// <summary>
        ///     为指定类型添加运行时长度计算策略
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <param name="policy">计算策略</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public static void Add(Type type, IRuntimeSizeCalcPolicy policy)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (policy == null) throw new ArgumentNullException("policy");
            _policies[type.FullName] = policy;
        }

        /// <summary>
        ///     获取指定类型的运行时长度计算策略
        /// </summary>
        /// <param name="type">指定类型</param>
        /// <returns>返回指定策略</returns>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public static IRuntimeSizeCalcPolicy GetPolicy(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            IRuntimeSizeCalcPolicy policy;
            return _policies.TryGetValue(type.FullName, out policy) ? policy : null;
        }

        #endregion
    }
}