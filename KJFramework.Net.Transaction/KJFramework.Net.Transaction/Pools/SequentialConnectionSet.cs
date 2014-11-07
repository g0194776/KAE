using System;
using System.Net;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///    支持顺序算法的连接容器
    /// </summary>
    /// <typeparam name="T">存放的连接类型</typeparam>
    internal class SequentialConnectionSet<T> : ConnectionSet<T>
    {
        #region Constructor.

        /// <summary>
        ///    支持顺序算法的连接容器
        /// </summary>
        /// <typeparam name="T">存放的连接类型</typeparam>
        public SequentialConnectionSet(int min, int max, Tuple<IPEndPoint, IProtocolStack<T>, object> tuple, Func<IPEndPoint, IProtocolStack<T>, object, IServerConnectionAgent<T>> createFunc)
            : base(min, max, tuple, createFunc)
        {
        }

        #endregion

        #region Members.

        private int _sequenceIndex;

        #endregion

        /// <summary>
        ///    根据不同算法，获取一个当前连接容器中的存活连接
        /// </summary>
        /// <returns>返回一个当前连接容器中的存活连接</returns>
        public override IServerConnectionAgent<T> InnerGetConnection()
        {
            lock (_lockObj)
            {
                if (_sequenceIndex < _connections.Count) return _connections[_sequenceIndex++];
                if (_connections.Count == 0) return null;
                return _connections[(_sequenceIndex = 0)];
            }
        }
    }
}