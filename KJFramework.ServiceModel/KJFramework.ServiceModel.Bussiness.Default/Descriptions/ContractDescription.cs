using System;
using System.Collections.Generic;
using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     契约描述，提供了相关的基本操作。
    /// </summary>
    public class ContractDescription : IContractDescription
    {
        #region Implementation of IContractDescription

        protected IContractInfomation _infomation;
        protected List<IDescriptionMethod> _descMethods = new List<IDescriptionMethod>();

        /// <summary>
        ///      获取所有服务契约中被定义的操作描述
        /// </summary>
        /// <returns>返回操作描述集合</returns>
        public IDescriptionMethod[] GetMethods()
        {
            return _descMethods.ToArray();
        }

        /// <summary>
        ///     获取服务契约信息
        /// </summary>
        public IContractInfomation Infomation
        {
            get { return _infomation; }
            internal set { _infomation = value; }
        }

        #endregion

        #region Methods
        
        /// <summary>
        ///     添加一个将要被描述的服务契约操作
        /// </summary>
        /// <param name="method">契约操作</param>
        internal void Add(ServiceMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            _descMethods.Add(new DescriptionMethod(method.GetCoreMethod()));
        }

        #endregion
    }
}