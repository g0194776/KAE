namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     参数描述
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        ///     获取参数编号
        /// </summary>
        int Id { get; }
        /// <summary>
        ///     获取参数类型
        /// </summary>
        string Type { get; }
        /// <summary>
        ///     获取参数名称
        /// </summary>
        string Name { get; }
        /// <summary>
        ///     获取参数相关类型信息
        /// </summary>
        string ReferenceId { get; }
    }
}