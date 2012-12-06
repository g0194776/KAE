using System.Collections.Concurrent;
using System.Threading;
using KJFramework.Net.Transaction.Agent;
using KJFramework.Tracing;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///     连接池
    /// </summary>
    /// <typeparam name="T">标示一个连接</typeparam>
    public class ConnectionPool<T>
    {
        #region Members

        protected readonly ReaderWriterLockSlim _rwLocker = new ReaderWriterLockSlim();
        protected static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ConnectionPool<T>));
        protected readonly ConcurrentDictionary<T, IServerConnectionAgent> _connections = new ConcurrentDictionary<T, IServerConnectionAgent>();

        #endregion

        #region Methods

        /// <summary>
        ///     添加一个新的连接
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <param name="channel">消息通信信道</param>
        /// <returns>返回添加后的状态</returns>
        public virtual bool Add(T key, IServerConnectionAgent channel)
        {
            if (channel == null) return false;
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

        /// <summary>
        ///     获取具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回一个消息通信信道</returns>
        public virtual IServerConnectionAgent GetChannel(T key)
        {
            _rwLocker.EnterReadLock();
            try
            {
                IServerConnectionAgent channel;
                return _connections.TryGetValue(key, out channel) ? channel : null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
            finally { _rwLocker.ExitReadLock(); }
        }

        /// <summary>
        ///     移除具有指定唯一标示的消息通信信道
        /// </summary>
        /// <param name="key">连接标示</param>
        /// <returns>返回移除后的状态</returns>
        public virtual bool Remove(T key)
        {
            IServerConnectionAgent channel;
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