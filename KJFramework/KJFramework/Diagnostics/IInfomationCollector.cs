using System;
using KJFramework.Basic.Enum;
using KJFramework.EventArgs;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     信息收集器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IInfomationCollector : ICollector
    {
        /// <summary>
        ///     收集时间间隔
        ///            * 时间单位： 毫秒。
        /// </summary>
        double CollectInterval { get; set; }
        /// <summary>
        ///     获取或设置被收集信息的对象类型
        /// </summary>
        Type CollectType { get; }
        /// <summary>
        ///     获取创建时间
        /// </summary>
        DateTime CreateTime { get; }
        /// <summary>
        ///     获取或设置附属属性
        /// </summary>
        Object Tag { get; set; }
        /// <summary>
        ///     获取信息收集器类型枚举
        /// </summary>
        InfomationCollectorTypes InfomationCollectorType { get; }
        /// <summary>
        ///     获取评审器
        /// </summary>
        /// <returns>返回评审器</returns>
        IInfomationReviewer GetReviewer();
        /// <summary>
        ///     重置收集时间
        /// </summary>
        /// <param name="interval">收集时间</param>
        void ResetCollectInterval(double interval);
        /// <summary>
        ///     新信息事件
        /// </summary>
        event EventHandler<NewInfomationEventArgs> NewInfomation;
        /// <summary>
        ///     收集时间被重置事件
        /// </summary>
        event EventHandler IntervalTimeChanged;
    }
}