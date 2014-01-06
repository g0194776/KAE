using System.Collections.Concurrent;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///     连接池
    /// </summary>
    /// <typeparam name="T">标示一个连接</typeparam>
    /// <typeparam name="TMessage">消息类型</typeparam>
    public class ConnectionPool<T, TMessage>
    {
        #region Members

        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ConnectionPool<T, TMessage>));
        protected readonly ConcurrentDictionary<T, IServerConnectionAgent<TMessage>> _connections = new ConcurrentDictionary<T, IServerConnectionAgent<TMessage>>();

        #endregion

        #region Methods

        /// <summary>
        ///     添加一个新的连接
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回添加后的状态</returns>
        public virtual bool Add(T key, IServerConnectionAgent<TMessage> channel)
        {
            if (channel == null) return false;
            try
            {
                if (_connections.TryAdd(key, channel))
                {
                    //add key to Tag property.
                    channel.Tag = key;
                    //hold disconnected event.
                    channel.Disconnected += AgentDisconnected;
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return false;
            }
        }

        /// <summary>
        ///     获取具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回一个消息通信信道</returns>
        public virtual IServerConnectionAgent<TMessage> GetChannel(T key)
        {
            try
            {
                IServerConnectionAgent<TMessage> channel;
                return _connections.TryGetValue(key, out channel) ? channel : null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     移除具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回移除后的状态</returns>
        public virtual bool Remove(T key)
        {
            IServerConnectionAgent<TMessage> channel;
            return _connections.TryRemove(key, out channel);
        }

        #endregion

        #region Events

        //channel disconnected.
        protected virtual void AgentDisconnected(object sender, System.EventArgs e)
        {
            IConnectionAgent agent = (IConnectionAgent)sender;
            agent.Disconnected -= AgentDisconnected;
            T key = (T) agent.Tag;
            if(key != null) Remove(key);
        }

        #endregion
    }
}