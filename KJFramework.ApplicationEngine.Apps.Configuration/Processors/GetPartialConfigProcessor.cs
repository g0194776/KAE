using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Apps.Configuration.Processors
{
    /// <summary>
    ///    获取配置节信息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0x01, DetailsId = 0x04)]
    public class GetPartialConfigProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///    获取配置节信息处理器
        /// </summary>
        public GetPartialConfigProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(GetPartialConfigProcessor));

        #endregion

        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        ///        
        ///     [REQ MESSAGE]
        /// ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x0A - Key
        /// 
        /// 
        ///     [RSP MESSAGE]
        /// ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x0A - Error Id
        ///      0x0B - Error Reason (Optional)
        ///      0x0C - Config Data
        /// </summary>
        /// <param name="package">消息事务</param>
        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            MetadataContainer reqMessage = package.Request;
            MetadataContainer rspMessage = new MetadataContainer();
            string key = reqMessage.GetAttributeAsType<string>(0x0A);
            if (string.IsNullOrEmpty(key))
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.IllegalArgument));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Key cannot be null."));
                return;
            }
            try
            {
                rspMessage.SetAttribute(0x0C, new StringValueStored(ConfigurationDataBuilder.GetPartialConfig(key)));
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.OK));
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex);
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.Unknown));
                rspMessage.SetAttribute(0x0B, new StringValueStored(ex.Message));
            }
            package.SendResponse(rspMessage);
        }

        #endregion
    }
}