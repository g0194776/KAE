using System;
using System.Net;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Net.Transaction.Agent;

namespace KJFramework.Net.Transaction.Pools
{
    /// <summary>
    ///    支持随机算法的连接容器
    /// </summary>
    /// <typeparam name="T">存放的连接类型</typeparam>
    internal class RamdomConnectionSet<T> : ConnectionSet<T>
    {
        #region Constructor.

        /// <summary>
        ///    支持随机算法的连接容器
        /// </summary>
        /// <typeparam name="T">存放的连接类型</typeparam>
        public RamdomConnectionSet(int min, int max, Tuple<IPEndPoint, IProtocolStack<T>, object> tuple, Func<IPEndPoint, IProtocolStack<T>, object, IServerConnectionAgent<T>> createFunc)
            : base(min, max, tuple, createFunc)
        {
        }

        #endregion

        #region Methods.

        /// <summary>
        ///    返回一个当前连接容器中的存活连接
        /// </summary>
        /// <returns></returns>
        public override IServerConnectionAgent<T> InnerGetConnection()
        {
            lock (_lockObj)
            {
                ushort value = (ushort) ((DateTime.Now.Ticks & 0x3FFF)%_connections.Count);
                return _connections[value];
            }
        }

        #endregion
    }
}