using System.Collections.Generic;
using KJFramework.Enums;

namespace KJFramework.Statistics
{
    /// <summary>
    ///     可统计元接口，提供了相关的基本操作。
    /// </summary>
    public interface IStatisticable<T> where T : IStatistic
    {
        /// <summary>
        ///     获取或设置统计器
        /// </summary>
        Dictionary<StatisticTypes, T> Statistics { get; set; }
    }
}