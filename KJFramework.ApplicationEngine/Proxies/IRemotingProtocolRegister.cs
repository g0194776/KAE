using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.EventArgs;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.Channels.Uri;
using KJFramework.Net.Transaction.Objects;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程协议注册器
    /// </summary>
    internal interface IRemotingProtocolRegister
    {
        #region Methods.

        /// <summary>
        ///     初始化
        /// </summary>
        /// <param name="hostName">KAE宿主实例所部署的主机名</param>
        /// <param name="defaultKAENetwork">KAE宿主的本地化通信资源</param>
        void Initialize(string hostName, TcpUri defaultKAENetwork);
        /// <summary>
        ///     将一个业务的通信协议与远程可访问地址注册到服务器上
        /// </summary>
        /// <param name="identity">业务协议编号</param>
        /// <param name="protocolTypes">通信协议类型</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="resourceUri">远程可访问的资源地址</param>
        /// <param name="kppUniqueId">KPP全局唯一编号</param>
        void Register(MessageIdentity identity, ProtocolTypes protocolTypes, ApplicationLevel level, Uri resourceUri, Guid kppUniqueId);
        /// <summary>
        ///    根据一组参数获取相应的远程目标资源
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="protocol">通信协议</param>
        /// <param name="protocolTypes">协议类型</param>
        /// <returns>返回远程目标可访问资源</returns>
        IProtocolResource GetProtocolResource(Protocols protocol, ProtocolTypes protocolTypes, ApplicationLevel level);

        #endregion

        #region Events.

        /// <summary>
        ///    远程资源列表变更事件
        /// </summary>
        event EventHandler<LightSingleArgEventArgs<IProtocolResource>> ChildrenChanged;

        #endregion
    }
}