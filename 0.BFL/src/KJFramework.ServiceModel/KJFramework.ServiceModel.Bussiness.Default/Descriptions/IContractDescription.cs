namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     契约描述元接口，提供了相关的基本操作。
    /// </summary>
    public interface IContractDescription
    {
        /// <summary>
        ///      获取所有服务契约中被定义的操作描述
        /// </summary>
        /// <returns>返回操作描述集合</returns>
        IDescriptionMethod[] GetMethods();
        /// <summary>
        ///     获取服务契约信息
        /// </summary>
        IContractInfomation Infomation { get; }
    }
}