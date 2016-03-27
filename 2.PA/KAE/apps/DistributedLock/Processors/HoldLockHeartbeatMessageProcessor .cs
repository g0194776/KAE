using System;
using KJFramework.ApplicationEngine;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;

namespace DistributedLock.Processors
{
    /// <summary>
    ///     分布式锁持有后的心跳消息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0, DetailsId = 4)]
    public class HoldLockHeartbeatMessageProcessor  : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///     分布式锁持有后的心跳消息处理器
        /// </summary>
        /// <param name="application">自身KAE APP实例</param>
        public HoldLockHeartbeatMessageProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Members.

        /// <summary>
        ///    处理一个网络请求
        /// </summary>
        /// <param name="request">消息事务</param>
        protected override MetadataContainer InnerProcess(MetadataContainer request)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}