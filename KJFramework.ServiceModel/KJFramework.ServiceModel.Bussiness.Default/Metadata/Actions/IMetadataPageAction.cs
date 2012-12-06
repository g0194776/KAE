using System.Net;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     元数据页面动作元接口，提供了等相关的基本操作。
    /// </summary>
    public interface IHttpMetadataPageAction
    {
        /// <summary>
        ///     获取契约描述
        /// </summary>
        IContractDescription ContractDescription { get; }
        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        string Execute(HttpListenerRequest httpListenerRequest);
    }
}