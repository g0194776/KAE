using System.Net;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;

namespace KJFramework.ServiceModel.Bussiness.Default.Metadata.Actions
{
    /// <summary>
    ///     基于HTTP协议的参数元数据交换页面，提供了相关的相关操作。
    /// </summary>
    internal class HttpMetadataArgumentDescriptionAction : HttpMetadataPageAction
    {
        #region Members

        private readonly string _argumentId;
        private readonly string _content;

        #endregion

        #region Constructor

        /// <summary>
        ///     基于HTTP协议的参数元数据交换页面，提供了相关的相关操作。
        /// </summary>
        public HttpMetadataArgumentDescriptionAction(IContractDescription contractDescription, string argumentId, string content)
            : this(contractDescription)
        {
            _argumentId = argumentId;
            _content = content;
        }

        /// <summary>
        ///     基于HTTP协议的参数元数据交换页面，提供了相关的相关操作。
        /// </summary>
        public HttpMetadataArgumentDescriptionAction(IContractDescription contractDescription)
            : base(contractDescription)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     执行动作
        /// </summary>
        /// <param name="httpListenerRequest">HTTP输入请求</param>
        public override string Execute(HttpListenerRequest httpListenerRequest)
        {
            return _content;
        }

        #endregion
    }
}