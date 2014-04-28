using KJFramework.ApplicationEngine.Eums;
using KJFramework.Messages.Contracts;
using KJFramework.Net.Transaction;

namespace KJFramework.ApplicationEngine.Packages
{
    /// <summary>
    ///    业务包裹接口
    /// </summary>
    public interface IBusinessPackage : IMessageTransaction<MetadataContainer>
    {
        #region Members.

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