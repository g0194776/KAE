using System;
using System.Collections.Generic;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.ApplicationEngine
{
    /// <summary>
    ///    网络协议栈容器
    /// </summary>
    internal class ProtocolStackContainer : IProtocolStackContainer
    {
        #region Members.

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

        #endregion
    }
}