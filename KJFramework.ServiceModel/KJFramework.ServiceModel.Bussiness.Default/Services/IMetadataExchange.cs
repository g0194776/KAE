using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///      契约数据发布元接口，提供了相关的基本操作。
    /// </summary>
    public interface IMetadataExchange
    {
        /// <summary>
        ///     创建一个关于当前服务节点的契约描述
        /// </summary>
        /// <returns>返回契约描述</returns>
        IContractDescription CreateDescription();
        /// <summary>
        ///      获取或设置一个值，该值表示了当前服务节点是否支持契约元数据交换
        /// </summary>
        bool IsSupportExchange { get; set; }
    }
}