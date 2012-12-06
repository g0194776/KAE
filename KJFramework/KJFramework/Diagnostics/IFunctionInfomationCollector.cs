using System;
namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     功能信息收集器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IFunctionInfomationCollector : IInfomationCollector
    {
        /// <summary>
        ///     通知
        /// </summary>
        /// <typeparam name="T">结果类型</typeparam>
        /// <param name="args">参数</param>
        /// <returns>返回通知的结果</returns>
        T Notify<T>(params Object[] args);
    }
}