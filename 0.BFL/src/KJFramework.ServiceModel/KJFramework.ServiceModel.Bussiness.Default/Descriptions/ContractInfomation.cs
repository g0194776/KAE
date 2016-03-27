using System;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     契约信息，提供了相关的属性结构
    /// </summary>
    public class ContractInfomation : IContractInfomation
    {
        #region Implementation of IContractInfomation

        protected DateTime _openTime;
        protected string _contractName;
        protected string _name;
        protected string _description;
        protected string _version;

        /// <summary>
        ///     获取或设置契约开放时间
        /// </summary>
        public DateTime OpenTime
        {
            get { return _openTime; }
            set { _openTime = value; }
        }

        /// <summary>
        ///     获取或设置契约名称
        /// </summary>
        public string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }

        /// <summary>
        ///     获取或设置契约别名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置契约描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     获取或设置契约版本
        /// </summary>
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        ///     获取或设置契约全名称
        /// </summary>
        public string FullName { get; set; }

        #endregion
    }
}