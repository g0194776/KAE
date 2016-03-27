namespace KJFramework.Test.LoadRunner
{
    /// <summary>
    ///     测试信息接口
    /// </summary>
    public interface ITestInfo
    {
        /// <summary>
        ///     获取或设置测试事务名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        ///     获取或设置测试事务描述
        /// </summary>
        string Description { get; set; } 
    }
}