using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Apps.Configuration.Processors
{
    /// <summary>
    ///    获取整体数据表配置信息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0x01, DetailsId = 0x00)]
    public class GetDataTableProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        public GetDataTableProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">消息事务</param>
        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}