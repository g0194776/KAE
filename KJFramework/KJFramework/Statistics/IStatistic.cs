using System;
using KJFramework.Enums;

namespace KJFramework.Statistics
{
    /// <summary>
    ///     统计元接口，提供了相关的基本操作。
    /// </summary>
    public interface IStatistic : IDisposable
    {
        /// <summary>
        ///     获取统计类型
        /// </summary>
        StatisticTypes StatisticType { get; }
        /// <summary>
        ///     获取或设置可用标示
        /// </summary>
        bool IsEnable { get; set; }
        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="element">统计类型</param>
        /// <typeparam name="T">统计类型</typeparam>
        void Initialize<T>(T element);
        /// <summary>
        ///     关闭统计
        /// </summary>
        void Close();
    }
}