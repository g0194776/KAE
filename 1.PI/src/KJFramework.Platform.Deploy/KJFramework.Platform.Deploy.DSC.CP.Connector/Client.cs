using System;
using System.Collections.Generic;
using System.Linq;

namespace KJFramework.Platform.Deploy.DSC.CP.Connector
{
    internal class Client
    {
        #region Members

        private static Dictionary<string, Guid> _clients = new Dictionary<string, Guid>();

        #endregion

        #region Methods

        /// <summary>
        ///     添加一个客户端标示
        /// </summary>
        /// <param name="clientTag">客户端标示</param>
        /// <param name="channelId">客户端通道编号</param>
        public static void Add(string clientTag, Guid channelId)
        {
            if (!_clients.ContainsKey(clientTag))
            {
                _clients.Add(clientTag, channelId);
                return;
            }
            _clients[clientTag] = channelId;
        }

        /// <summary>
        ///     获取具有指定客户端标示的通道编号
        /// </summary>
        /// <param name="clientTag">客户端标示</param>
        /// <returns>返回通道编号</returns>
        public static Guid[] GetClient(string clientTag)
        {
            if (clientTag == null)
            {
                if (_clients.Count > 0)
                {
                    return _clients.Values.ToArray();
                }
                return null;
            }
            Guid channelId;
            if (_clients.TryGetValue(clientTag, out channelId))
            {
                return new[] {channelId};
            }
            return null;
        }

        /// <summary>
        ///     移除具有指定客户端标示的通道编号
        /// </summary>
        /// <param name="clientTag">客户端标示</param>
        public static void Remove(string clientTag)
        {
            _clients.Remove(clientTag);
        }

        /// <summary>
        ///     移除具有指定通道编号的客户端标示
        /// </summary>
        /// <param name="channelId">通道编号</param>
        public static void Remove(Guid channelId)
        {
            string clientTag = null;
            foreach (var client in _clients)
            {
                if (client.Value == channelId)
                {
                    clientTag = client.Key;
                    break;
                }
            }
            if (clientTag != null)
            {
                Remove(clientTag);
            }
        }
        
        #endregion
    }
}