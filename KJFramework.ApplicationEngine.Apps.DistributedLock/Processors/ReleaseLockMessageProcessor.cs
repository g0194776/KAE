using System;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;

namespace KJFramework.ApplicationEngine.Apps.DistributedLock.Processors
{
    /// <summary>
    ///     释放分布式锁请求消息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0, DetailsId = 2)]
    public class ReleaseLockMessageProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///     释放分布式锁请求消息处理器
        /// </summary>
        /// <param name="application">自身KAE APP实例</param>
        public ReleaseLockMessageProcessor(IApplication application)
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