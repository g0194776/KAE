using System;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;

namespace SecurityQuota.Processors
{
    /// <summary>
    ///    计算配额的业务处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 200, ServiceId = 1, DetailsId = 0)]
    public class CalculateQuotaProcessor : MetadataKAEProcessor
    {
        #region Members.

        private static readonly SQSManager _manager = new SQSManager();

        #endregion

        #region Constructor.

        /// <summary>
        ///    KAE - Metadata协议的消息处理器
        /// </summary>
        public CalculateQuotaProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="package">消息事务</param>
        protected override MetadataContainer InnerProcess(MetadataContainer package)
        {
            MetadataContainer rspMsg = new MetadataContainer();
            try
            {
                string name = package.GetAttributeAsType<string>(0x0A);
                string quotaRule = package.GetAttributeAsType<string>(0x0B);
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(quotaRule))
                {
                    rspMsg.SetAttribute(0x0A, new ByteValueStored(239));
                    rspMsg.SetAttribute(0x0B, new StringValueStored("Invalid parameters"));
                    return rspMsg;
                }
                if (_manager.Check(name, quotaRule))
                {
                    rspMsg.SetAttribute(0x0A, new ByteValueStored(238));
                    rspMsg.SetAttribute(0x0B, new StringValueStored("Quota is ultralimit"));
                    return rspMsg;
                }
                rspMsg.SetAttribute(0x0A, new ByteValueStored(0));
                return rspMsg;
            }
            catch (Exception)
            {
                rspMsg.SetAttribute(0x0A, new ByteValueStored(237));
                rspMsg.SetAttribute(0x0B, new StringValueStored("Unknown error"));
                return rspMsg;
            }
        }

        #endregion
    }
}