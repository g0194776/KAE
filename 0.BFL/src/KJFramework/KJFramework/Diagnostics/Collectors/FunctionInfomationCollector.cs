using System;
using KJFramework.Enums;

namespace KJFramework.Diagnostics.Collectors
{
    /// <summary>
    ///     功能信息收集器抽象类，提供了相关的基本操作。
    /// </summary>
    public abstract class FunctionInfomationCollector : InfomationCollector, IFunctionInfomationCollector
    {
        #region 构造函数

        /// <summary>
        ///     功能信息收集器抽象类，提供了相关的基本操作。
        /// </summary>
        /// <param name="collectType">收集对象类型</param>
        /// <param name="reviewere">信息评审器</param>
        protected FunctionInfomationCollector(Type collectType, IInfomationReviewer reviewere)
            : base(collectType, reviewere)
        {
            _infomationCollectorType = InfomationCollectorTypes.Special;
        }

        #endregion

        #region Implementation of IFunctionInfomationCollector

        /// <summary>
        ///     通知
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="args">参数</param>
        /// <returns>返回通知的结果</returns>
        public abstract T Notify<T>(params object[] args);

        #endregion
    }
}