using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程协议注册器
    /// </summary>
    internal interface IRemotingProtocolRegister
    {
        #region Methods.

        /// <summary>
        ///     将一个业务的通信协议与远程可访问地址注册到服务器上
        /// </summary>
        /// <param name="identity">业务协议编号</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="resourceUri">远程可访问的资源地址</param>
        /// <param name="protocolTypes">通信协议类型</param>
        void Register(MessageIdentity identity, ProtocolTypes protocolTypes, ApplicationLevel level, Uri resourceUri);
        /// <summary>
        ///    根据一组参数获取相应的远程目标资源
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        /// <returns>返回远程目标可访问资源</returns>
        IProtocolResource GetProtocolResource(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level);

        #endregion
    }
}