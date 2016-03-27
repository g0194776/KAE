using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;

namespace DistributedLock.Processors
{
    /// <summary>
    ///     获取分布式锁请求消息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xEF, ServiceId = 0, DetailsId = 0)]
    public class TestMessageProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///     获取分布式锁请求消息处理器
        /// </summary>
        /// <param name="application">自身KAE APP实例</param>
        public TestMessageProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Members.

        protected override MetadataContainer InnerProcess(MetadataContainer request)
        {
            MetadataContainer rspMessage = new MetadataContainer();
            rspMessage.SetAttribute(0x0C, new StringValueStored("test RSP result."));
            return rspMessage;
        }


        #endregion
    }
}