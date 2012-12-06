using System;
using System.Collections.Generic;

namespace KJFramework.Net.Channels.Caches
{
    /// <summary>
    ///     封包片存根，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class MultiPacketStub<T> : IMultiPacketStub<T>
    {
        #region Constructor

        /// <summary>
        ///     封包片存根，提供了相关的基本操作
        /// </summary>
        /// <exception cref="System.ArgumentException">参数错误</exception>
        public MultiPacketStub(int sessionId, int maxPacketCount)
        {
            if (maxPacketCount <= 0) throw new ArgumentException("Illegal maxPacketCount: " + maxPacketCount);
            _sessionId = sessionId;
            _maxPacketCount = maxPacketCount;
            _messages = new List<T>(maxPacketCount);
        }

        #endregion

        #region Members

        private readonly IList<T> _messages;

        #endregion

        #region Implementation of IMultiPacketStub<T>

        protected readonly int _sessionId;
        protected readonly int _maxPacketCount;

        /// <summary>
        ///     获取当前完整消息的编号
        /// </summary>
        public int SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        ///     获取最大封包片数目
        /// </summary>
        public int MaxPacketCount
        {
            get { return _maxPacketCount; }
        }

        /// <summary>
        ///     添加一个封包片
        /// </summary>
        /// <param name="message">封包片消息</param>
        /// <returns>如果返回值不为false, 则证明已经接收一个完整的消息</returns>
        public bool AddPacket(T message)
        {
            _messages.Add(message);
            return _messages.Count == _maxPacketCount ? true : false;
        }

        /// <summary>
        ///     获取内部所有的封包片
        /// </summary>
        /// <returns>返回封包片集合</returns>
        public IList<T> GetPackets()
        {
            return _messages;
        }

        #endregion
    }
}