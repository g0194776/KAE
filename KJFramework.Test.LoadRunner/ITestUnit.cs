namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     最小测试单元接口
    /// </summary>
    public interface ITestUnit<TContext>
        where TContext : ITestContext
    {
        /// <summary>
        ///     获取测试信息
        /// </summary>
        ITestInfo Info { get; }
        /// <summary>
        ///     获取或设置下一个要执行的测试单元
        /// </summary>
        ITestUnit<TContext> Next { get; set; }
        /// <summary>
        ///     执行当前测试单元
        /// </summary>
        /// <param name="context">测试上下文</param>
        /// <returns>返回执行结果</returns>
        bool Execute(TContext context);
    }
}