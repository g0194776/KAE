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
    ///    获取基于KEY/VALUE类型的配置信息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0x01, DetailsId = 0x02)]
    public class GetKeyDataProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///    获取基于KEY/VALUE类型的配置信息处理器
        /// </summary>
        public GetKeyDataProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(GetKeyDataProcessor));

        #endregion

        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        ///        
        ///     [REQ MESSAGE]
        /// ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x0A - Database Name
        ///      0x0B - Table Name
        ///      0x0C - Service Name
        /// 
        /// 
        ///     [RSP MESSAGE]
        /// ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x0A - Error Id
        ///      0x0B - Error Reason (Optional)
        ///      0x0C - Key/Value Pairs Data Collection (ResourceBlock[])
        ///             0x00 - Key
        ///             0x01 - Value
        ///             0x02 - Create Time
        ///             0x03 - LastOprTime
        /// </summary>
        /// <param name="package">消息事务</param>
        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            MetadataContainer reqMessage = package.Request;
            MetadataContainer rspMessage=  new MetadataContainer();
            string databaseName = reqMessage.GetAttributeAsType<string>(0x0A);
            string tableName = reqMessage.GetAttributeAsType<string>(0x0B);
            string serviceName = reqMessage.GetAttributeAsType<string>(0x0C);
            if (string.IsNullOrEmpty(databaseName))
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.IllegalArgument));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Database-Name cannot be null."));
                return;
            }
            if (string.IsNullOrEmpty(tableName))
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.IllegalArgument));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Table-Name cannot be null."));
                return;
            }
            if (string.IsNullOrEmpty(serviceName))
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored((byte)KAEErrorCodes.IllegalArgument));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Service-Name cannot be null."));
                return;
            }
            try
            {
                rspMessage.SetAttribute(0x0C, new ResourceBlockArrayStored(ConfigurationDataBuilder.GetKeyValueConfigurations(databaseName, tableName, serviceName)));
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