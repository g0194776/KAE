using System;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;

namespace KJFramework.Net.Transaction.Clusters
{
    /// <summary>
    ///     网络群集负载器接口
    /// </summary>
    public interface INetworkCluster<TMessage>
    {
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannel(string roleId, IProtocolStack<TMessage> protocolStack, out string errMsg);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<TMessage> protocolStack, int balanceFlag, out string errMsg);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<TMessage> protocolStack, long balanceFlag, out string errMsg);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<TMessage> protocolStack, string balanceFlag, out string errMsg);
        /// <summary>
        ///     根据当前负载器规则获取一个通信信道
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="balanceFlag">负载位</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns>如果指定条件的通信信道不存在，则会创建它并返回</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        IServerConnectionAgent<TMessage> GetChannelBySpecificCondition(string roleId, IProtocolStack<TMessage> protocolStack, Guid balanceFlag, out string errMsg);
    }
}