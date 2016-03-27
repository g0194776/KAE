namespace KJFramework.Data.Synchronization.Policies
{
    /// <summary>
    ///     发布者策略接口
    /// </summary>
    public interface IPublisherPolicy
    {
        /// <summary>
        ///     获取或设置一个值，该值标示了当前的发布者是否可以对于失败的发布消息进行重试操作
        /// </summary>
        bool CanRetry { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前发布的所有消息是不是单向的
        /// </summary>
        bool IsOneway { get; set; }
        /// <summary>
        ///     获取或设置重试次数
        ///     <para>* 此字段仅当 CanRetry = true的时候有效</para>
        /// </summary>
        byte RetryCount { get; set; }
        /// <summary>
        ///     获取或设置同步数据时的超时时间
        ///     <para>* 单位: 秒</para>
        /// </summary>
        int TimeoutSec { get; set; }
    }
}