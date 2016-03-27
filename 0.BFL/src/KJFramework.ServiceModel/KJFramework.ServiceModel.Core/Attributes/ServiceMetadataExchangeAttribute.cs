using System;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     ����Ԫ���ݽ�������
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceMetadataExchangeAttribute : System.Attribute
    {
        #region Members

        private string _contractName;

        /// <summary>
        ///     ��ȡ��������Լ�Ĺ�������
        /// </summary>
        public string ContractName
        {
            get { return _contractName; }
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     ����Ԫ���ݽ�������
        /// </summary>
        /// <param name="contractName">
        ///     ��Ҫ���ŵ���Լ����
        ///     <para>* �����ƽ��ᱻ����HTTP��Ԫ���ݽ���</para>
        /// </param>
        public ServiceMetadataExchangeAttribute(string contractName)
        {
            if (string.IsNullOrEmpty(contractName)) throw new ArgumentNullException(contractName);
            _contractName = contractName;
        }

        #endregion
    }
}