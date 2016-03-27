using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.ProtocolStack;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    网络协议栈容器
    /// </summary>
    internal class ProtocolStackContainer : IProtocolStackContainer
    {
        #region Members.

        private IProtocolStack _intellectProtocolStack = new BusinessProtocolStack();
        private IProtocolStack _metadataProtocolStack = new MetadataProtocolStack();
        private readonly Dictionary<string, IProtocolStack>  _protocolStacks = new Dictionary<string, IProtocolStack>();

        #endregion

        #region Methods.

        /// <summary>
        ///     注册指定服务角色的协议栈
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="protocolStack">协议栈</param>
        public void Regist(string role, IProtocolStack protocolStack)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            _protocolStacks[role] = protocolStack;
        }

        /// <summary>
        ///    获取具有指定角色的网络协议栈
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <returns>返回对应的网络协议栈</returns>
        public IProtocolStack GetProtocolStack(string role)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            IProtocolStack protocolStack;
            return (_protocolStacks.TryGetValue(role, out protocolStack) ? protocolStack : null);
        }

        /// <summary>
        ///    获取默认支持的协议栈
        /// </summary>
        /// <param name="protocolType">
        ///     协议类型
        ///     <para>* 默认只会支持Metadata和Intellect这两种协议栈</para>
        /// </param>
        /// <returns>返回默认的协议栈</returns>
        /// <exception cref="NotSupportedException">当前传入的协议不被支持</exception>
        public IProtocolStack GetDefaultProtocolStack(ProtocolTypes protocolType)
        {
            if (protocolType == ProtocolTypes.Metadata) return _metadataProtocolStack;
            if (protocolType == ProtocolTypes.Intellegence) return _intellectProtocolStack;
            throw new NotSupportedException("#Current type of protocol you passed had not supported yet.");
        }

        #endregion
    }
}