using System;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     服务元数据交换属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceMetadataExchangeAttribute : System.Attribute
    {
        #region Members

        private string _contractName;

        /// <summary>
        ///     获取或设置契约的公开名称
        /// </summary>
        public string ContractName
        {
            get { return _contractName; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     服务元数据交换属性
        /// </summary>
        /// <param name="contractName">
        ///     需要开放的契约名称
        ///     <para>* 该名称将会被用于HTTP的元数据交换</para>
        /// </param>
        public ServiceMetadataExchangeAttribute(string contractName)
        {
            if (string.IsNullOrEmpty(contractName)) throw new ArgumentNullException(contractName);
            _contractName = contractName;
        }

        #endregion
    }
}