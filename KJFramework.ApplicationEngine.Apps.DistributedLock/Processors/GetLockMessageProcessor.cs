using KJFramework.ApplicationEngine.Attributes;
using KJFramework.ApplicationEngine.Processors;
using KJFramework.Messages.Contracts;
using KJFramework.Messages.ValueStored;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Apps.DistributedLock.Processors
{
    /// <summary>
    ///     获取分布式锁请求消息处理器
    /// </summary>
    [KAEProcessorProperties(ProtocolId = 0xFB, ServiceId = 0, DetailsId = 0)]
    public class GetLockMessageProcessor : MetadataKAEProcessor
    {
        #region Constructor.

        /// <summary>
        ///     获取分布式锁请求消息处理器
        /// </summary>
        /// <param name="application">自身KAE APP实例</param>
        public GetLockMessageProcessor(IApplication application)
            : base(application)
        {
        }

        #endregion

        #region Members.

        /// <summary>
        ///    处理一个网络请求
        ///  
        ///  [REQ MESSAGE]
        ///  ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x02 - Named Locking Resource (REQUIRED)
        ///      0x03 - Accessing Types
        ///                     0x00 - Readonly (Multiple people can read current locked resource at the same time which locking type is R/W)
        ///                     0x01 - Write  (Only one person can write current locked resource at the same time which locking type is R/W)
        ///                     0x02 - Selfish (Only one person can read/write current locked resource at the same time which locking type is Selfish)
        /// 
        /// 
        ///  [RSP MESSAGE]
        ///  ===========================================
        ///      0x00 - Message Identity
        ///      0x01 - Transaction Identity
        ///      0x0A - Error ID (REQUIRED)
        ///      0x0B - Error Reason
        /// </summary>
        /// <param name="package">消息事务</param>
        protected override void InnerProcess(IMessageTransaction<MetadataContainer> package)
        {
            MetadataContainer rspMessage = new MetadataContainer();
            string resource = package.Request.GetAttributeAsType<string>(0x02);
            byte lockType = package.Request.GetAttributeAsType<byte>(0x03);
            if (string.IsNullOrEmpty(resource))
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored(0xFD));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Illegal named locking resource."));
                package.SendResponse(rspMessage);
                return;
            }
            if (lockType > 0x02)
            {
                rspMessage.SetAttribute(0x0A, new ByteValueStored(0xFD));
                rspMessage.SetAttribute(0x0B, new StringValueStored("#Illegal locking type."));
                package.SendResponse(rspMessage);
                return;
            }
            LockingManager.GetOrNewLockAsync(resource, (byte)(lockType % 2), package, delegate(IDistributedLock distributedLock) {  });
        }


        #endregion
    }
}