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
        ///     ���һ���ͻ��˱�ʾ
        /// </summary>
        /// <param name="clientTag">�ͻ��˱�ʾ</param>
        /// <param name="channelId">�ͻ���ͨ�����</param>
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
        ///     ��ȡ����ָ���ͻ��˱�ʾ��ͨ�����
        /// </summary>
        /// <param name="clientTag">�ͻ��˱�ʾ</param>
        /// <returns>����ͨ�����</returns>
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
        ///     �Ƴ�����ָ���ͻ��˱�ʾ��ͨ�����
        /// </summary>
        /// <param name="clientTag">�ͻ��˱�ʾ</param>
        public static void Remove(string clientTag)
        {
            _clients.Remove(clientTag);
        }

        /// <summary>
        ///     �Ƴ�����ָ��ͨ����ŵĿͻ��˱�ʾ
        /// </summary>
        /// <param name="channelId">ͨ�����</param>
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