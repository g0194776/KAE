namespace KJFramework.Security.Policy
{
    /// <summary>
    ///     补救策略元接口，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="TRemedy">补救对象</typeparam>
    public interface IRemedyPolicy<TRemedy> : IPolicy
    {
        /// <summary>
        ///     获取或设置做大补救次数
        /// </summary>
        int MaxRemedyCount { get; set; }
        /// <summary>
        ///     补救
        /// </summary>
        /// <returns>返回补救的结果</returns>
        bool Remedy(TRemedy remedyObject);
    }
}