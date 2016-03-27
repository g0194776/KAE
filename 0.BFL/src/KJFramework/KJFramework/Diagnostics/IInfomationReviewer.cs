using System;

namespace KJFramework.Diagnostics
{
    /// <summary>
    ///     信息评审器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IInfomationReviewer
    {
        /// <summary>
        ///     注册信息收集器
        /// </summary>
        /// <param name="collector">信息收集器</param>
        void Regist(IInfomationCollector collector);
        /// <summary>
        ///     注销信息收集器
        /// </summary>
        /// <param name="collector">信息收集器</param>
        void UnRegist(IInfomationCollector collector);
        /// <summary>
        ///     注册信息收集器
        /// </summary>
        /// <param name="id">信息收集器唯一标示</param>
        void UnRegist(Guid id);
        /// <summary>
        ///     获取具有指定唯一标示的信息收集器
        /// </summary>
        /// <param name="id">信息收集器唯一标示</param>
        /// <returns>返回对应的信息收集器</returns>
        IInfomationCollector GetCollector(Guid id);
    }
}