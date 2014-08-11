using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Packages
{
    /// <summary>
    ///    业务包裹接口
    /// </summary>
    public interface IBusinessPackage
    {
        #region Members.

        /// <summary>
        ///    获取本次通信所使用的网络消息事务
        /// </summary>
        MetadataMessageTransaction Transaction { get; }
        /// <summary>
        ///    获取当前业务包裹的状态
        /// </summary>
        BusinessPackageStates State { get; }
        /// <summary>
        ///    获取当前业务包裹所使用的协议类别
        /// </summary>
        ProtocolTypes ProtocolType { get; }

        #endregion
    }
}