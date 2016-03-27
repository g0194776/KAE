using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions.Wrappers
{
    /// <summary>
    ///     契约描述包装器元接口，提供了相关的基本操作。
    /// </summary>
    public interface IContractDescriptionWrapper
    {
        /// <summary>
        ///     获取契约描述
        /// </summary>
        IContractDescription ContractDescription { get; }
        /// <summary>
        ///     获取契约描述的文本形式
        /// </summary>
        /// <returns>返回契约描述的文本形式</returns>
        String GetContent();
    }
}