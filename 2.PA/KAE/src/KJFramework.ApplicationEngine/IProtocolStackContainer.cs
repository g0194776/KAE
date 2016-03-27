using System;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    网络协议栈容器接口
    /// </summary>
    internal interface IProtocolStackContainer
    {
        #region Methods.

        /// <summary>
        ///     注册指定服务角色的协议栈
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="protocolStack">协议栈</param>
        void Regist(string role, IProtocolStack protocolStack);
        /// <summary>
        ///    获取具有指定角色的网络协议栈
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <returns>返回对应的网络协议栈</returns>
        IProtocolStack GetProtocolStack(string role);
        /// <summary>
        ///    获取默认支持的协议栈
        /// </summary>
        /// <param name="protocolType">
        ///     协议类型
        ///     <para>* 默认只会支持Metadata和Intellect这两种协议栈</para>
        /// </param>
        /// <returns>返回默认的协议栈</returns>
        /// <exception cref="NotSupportedException">当前传入的协议不被支持</exception>
        IProtocolStack GetDefaultProtocolStack(ProtocolTypes protocolType);

        #endregion
    }
}
