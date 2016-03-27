namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     服务信息接口
    /// </summary>
    public interface IServiceInfomation
    {
        /// <summary>
        ///     获取服务名称
        /// </summary>
        string Name { get; }
        /// <summary>
        ///     获取服务全名称
        /// </summary>
        string FullName { get; }
        /// <summary>
        ///     获取服务描述
        /// </summary>
        string Description { get; }
        /// <summary>
        ///     获取服务当前版本号
        /// </summary>
        string Version { get; }
        /// <summary>
        ///     获取远程服务地址
        /// </summary>
        string EndPoint { get; }
    }
}