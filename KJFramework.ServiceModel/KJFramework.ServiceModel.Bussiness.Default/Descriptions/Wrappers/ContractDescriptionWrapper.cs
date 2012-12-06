using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions.Wrappers
{
    /// <summary>
    ///     契约描述包装器抽象父类，提供了相关的基本操作。
    /// </summary>
    public abstract class ContractDescriptionWrapper : IContractDescriptionWrapper
    {
        #region Constructor

        /// <summary>
        ///     契约描述包装器抽象父类，提供了相关的基本操作。
        /// </summary>
        public ContractDescriptionWrapper(IContractDescription contractDescription)
        {
            _contractDescription = contractDescription;
        }

        #endregion

        #region Implementation of IContractDescriptionWrapper

        protected IContractDescription _contractDescription;

        /// <summary>
        ///     获取契约描述
        /// </summary>
        public IContractDescription ContractDescription
        {
            get { return _contractDescription; }
        }

        /// <summary>
        ///     获取契约描述的文本形式
        /// </summary>
        /// <returns>返回契约描述的文本形式</returns>
        public abstract String GetContent();

        #endregion
    }
}