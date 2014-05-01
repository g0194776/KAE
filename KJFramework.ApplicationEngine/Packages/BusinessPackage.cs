using KJFramework.ApplicationEngine.Eums;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction;
using System;

namespace KJFramework.ApplicationEngine.Packages
{
    /// <summary>
    ///     业务包裹
    /// </summary>
    internal class BusinessPackage : MetadataMessageTransaction, IBusinessPackage
    {
        #region Constructors.

        /// <summary>
        ///     业务包裹
        /// </summary>
        /// <param name="channel">承载元数据通信协议的消息通信信道</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public BusinessPackage(IMessageTransportChannel<MetadataContainer> channel)
            : base(channel)
        {
            if (channel == null) throw new ArgumentNullException("channel");
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取当前业务包裹的状态
        /// </summary>
        public BusinessPackageStates State { get; internal set; }
        /// <summary>
        ///    获取当前业务包裹所使用的协议类别
        /// </summary>
        public ProtocolTypes ProtocolType { get; internal set; }

        #endregion
    }
}