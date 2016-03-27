namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试事务接口
    /// </summary>
    public interface ITestTransaction<TContext>
        where TContext : ITestContext
    {
        /// <summary>
        ///     获取或设置测试上下文
        /// </summary>
        TContext Context { get; set; }
        /// <summary>
        ///     获取测试信息
        /// </summary>
        ITestInfo Info { get; }
        /// <summary>
        ///     设置测试单元，整个测试事务将从这个测试单元开始执行
        /// </summary>
        /// <param name="unit">测试单元</param>
        void SetTestUnit(ITestUnit<TContext> unit);
        /// <summary>
        ///     开始执行
        /// </summary>
        void Start();
    }
}