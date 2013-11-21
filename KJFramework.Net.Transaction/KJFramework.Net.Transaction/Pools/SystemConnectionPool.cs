using System.Net;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///     系统连接池，仅供系统内部使用
    ///     <para>*key = xxxxxxx(IP):xxxxx(Port)</para>
    ///     <para>*demo = 127.0.0.1:8588</para>
    /// </summary>
    public class SystemConnectionPool : ConnectionPool<string>
    {
        #region Methods

        /// <summary>
        ///     获取具有指定标示的连接代理器，如果具有该条件的代理器不存在，则会创建一个新的代理器
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <param name="roleId">服务角色编号</param>
        /// <param name="protocolStack">连接所承载的协议栈</param>
        /// <param name="transactionManager">事务管理器</param>
        /// <returns>如果返回null, 则表示当前无法连接到目标远程终结点地址</returns>
        public IServerConnectionAgent GetChannel(string key, string roleId, IProtocolStack<BaseMessage> protocolStack, MessageTransactionManager transactionManager)
        {
            try
            {
                string fullKey = string.Format("{0}#{1}", roleId, key);
                IServerConnectionAgent agent = GetChannel(fullKey);
                if (agent != null)
                {
                    if (agent.GetChannel().IsConnected) return agent;
                    //remove this disconnected channel and recreate it.
                    Remove(fullKey);
                }
                //create new channel by connection str.
                int splitOffset = key.LastIndexOf(':');
                string ip = key.Substring(0, splitOffset);
                int port = int.Parse(key.Substring(splitOffset + 1, key.Length - (splitOffset + 1)));
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
                agent = ConnectionAgent.Create(iep, protocolStack, transactionManager);
                if (agent == null) return null;
                return Add(fullKey, agent) ? agent : null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     添加一个新的连接
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回添加后的状态</returns>
        public override bool Add(string key, IServerConnectionAgent channel)
        {
            if (base.Add(key, channel))
            {
                if (CommonCounter.Instance != null) CommonCounter.Instance.TotalOfServiceChannel.Increment();
                return true;
            }
            return false;
        }

        /// <summary>
        ///     移除具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回移除后的状态</returns>
        public override bool Remove(string key)
        {
            if (base.Remove(key))
            {
                if (CommonCounter.Instance != null) CommonCounter.Instance.TotalOfServiceChannel.Decrement();
                return true;
            }
            return false;
        }

        #endregion
    }
}