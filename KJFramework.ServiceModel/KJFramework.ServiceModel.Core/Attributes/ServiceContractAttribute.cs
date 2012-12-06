using System;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Core.Attributes
{
    /// <summary>
    ///     指定指定类型为开放服务的契约
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ServiceContractAttribute : System.Attribute
    {
        #region 成员

        private ServiceConcurrentTypes _serviceConcurrentType = ServiceConcurrentTypes.Multi;
        private String _description;
        private String _version;
        private String _name;

        /// <summary>
        ///     获取或设置服务实例类型枚举
        ///     <para>* 此配置的默认值为：Multi。</para>
        /// </summary>
        public ServiceConcurrentTypes ServiceConcurrentType
        {
            get { return _serviceConcurrentType; }
            set { _serviceConcurrentType = value; }
        }

        /// <summary>
        ///     获取或设置服务契约的描述信息
        /// </summary>
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     获取或设置服务契约的版本
        /// </summary>
        public String Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        ///     获取或设置服务契约的别名
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion
    }
}