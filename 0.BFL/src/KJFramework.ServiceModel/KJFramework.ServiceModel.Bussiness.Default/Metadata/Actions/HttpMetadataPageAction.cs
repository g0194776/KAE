using System;
using System.Net;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的元数据交换页面动作抽象父类，提供了相关的相关操作。
    /// </summary>
    internal abstract class HttpMetadataPageAction : IHttpMetadataPageAction
    {
        #region Constructor

        /// <summary>
        ///     基于HTTP协议的元数据交换页面动作抽象父类，提供了相关的相关操作。
        /// </summary>
        /// <param name="contractDescription">契约描述</param>
        public HttpMetadataPageAction(IContractDescription contractDescription)
        {
            _contractDescription = contractDescription;
        }

        #endregion

        #region Members

        protected String _result;

        #endregion

        #region Implementation of IHttpMetadataPageAction

        protected IContractDescription _contractDescription;

        /// <summary>
        ///     获取契约描述
        /// </summary>
        public IContractDescription ContractDescription
        {
            get { return _contractDescription; }
        }

        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        public abstract string Execute(HttpListenerRequest httpListenerRequest);

        #endregion
    }
}