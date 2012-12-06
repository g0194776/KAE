namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     策略接口，提供了相关的基本操作。
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        ///     获取一个值，该值表示了当前策略是否已经被部署
        /// </summary>
        bool Deployed { get; }
        /// <summary>
        ///     获取策略信息
        /// </summary>
        IPolicyInfomation Infomation { get; }
    }
}