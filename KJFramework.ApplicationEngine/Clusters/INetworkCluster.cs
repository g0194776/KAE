using System;
using System.Collections.Generic;
using KJFramework.ApplicationEngine.Eums;
using KJFramework.Net.Channels.Identities;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Objects;

namespace KJFramework.ApplicationEngine.Clusters
{
    /// <summary>
    ///     网络群集负载器接口
    /// </summary>
    public interface INetworkCluster<TMessage>
    {
        #region Members.

        /// <summary>
        ///     获取所支持的网络协议类型
        /// </summary>
        ProtocolTypes ProtocolType { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    更新网络缓存信息
        /// </summary>
        /// <param name="level">应用等级</param>
        /// <param name="cache">远程目标终结点信息列表</param>
        /// <param name="identity">通信协议</param>
        void UpdateCache(MessageIdentity identity, ApplicationLevel level,  IList<string> cache);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannel(Protocols target, ApplicationLevel level, IProtocolStack protocolStack, out string errMsg);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="target">标示网络终点接的协议簇组合</param>
        /// <param name="level">KAE应用等级</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannel(Protocols target, ApplicationLevel level, IProtocolStack protocolStack, long balanceFlag, out string errMsg);

        #endregion
    }
}