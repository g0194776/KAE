using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Packages
{
    /// <summary>
    ///     业务包裹
    /// </summary>
    internal class BusinessPackage : IBusinessPackage
    {
        #region Constructors.

        /// <summary>
        ///     业务包裹
        /// </summary>
        /// <param name="transaction">本次通信所使用的网络消息事务</param>
        public BusinessPackage(MetadataMessageTransaction transaction)
        {
            Transaction = transaction;
        }

        #endregion

        #region Members.

        /// <summary>
        ///    获取或设置本次通信所使用的网络消息事务
        /// </summary>
        public MetadataMessageTransaction Transaction { get; internal set; }
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