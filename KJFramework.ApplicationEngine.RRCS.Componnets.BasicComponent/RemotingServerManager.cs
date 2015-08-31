using KJFramework.ApplicationEngine.Helpers;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using KJFramework.Net.Uri;
using Uri = KJFramework.Net.Uri.Uri;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent
{
    /// <summary>
    ///     远程服务地址管理器
    /// </summary>
    internal static class RemotingServerManager
    {
        #region Members.

        private const string _defaultPublisherKey = "*";
        private static readonly object _lockObj = new object();
        private static readonly object _lockObjForPublisher = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemotingServerManager));
        //key = MessageIdentity + Supported Protocol + Application's Level + Application's Version
        private static readonly Dictionary<string, SortedDictionary<Guid, List<string>>> _addresses = new Dictionary<string, SortedDictionary<Guid, List<string>>>();
        private static readonly Dictionary<string, IDataPublisher<string, string[]>> _publishers = new Dictionary<string, IDataPublisher<string, string[]>>();

        public static Uri RemotingPublisherUri { get; private set; }

        #endregion

        #region Methods.

        public static void Initialize()
        {
            //initializes publisher.
            INetworkResource resource = new NetworkResource(NetworkHelper.GetDynamicTCPPort());
            IDataPublisher<string, string[]> defaultPublisher = DataPublisherFactory.Instance.Create<string, string[]>("*", resource);
            if (defaultPublisher.Open() != PublisherState.Open)
            {
                _tracing.Critical("#RRCS couldn't open a defaut remoting server publisher.");
                throw new System.Exception("#RRCS couldn't open a defaut remoting server publisher.");
            }
            lock (_lockObjForPublisher) _publishers.Add("*", defaultPublisher);
            RemotingPublisherUri = new TcpUri(string.Format("tcp://{0}:{1}", NetworkHelper.GetCurrentMachineIP(), resource.GetResource<int>()));
        }

        public static void Register(Guid guid, IDictionary<string, List<string>> dic)
        {
            IDictionary<string, List<string>> effectedKeys;
            lock (_lockObj)
            {
                effectedKeys = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, List<string>> pair in dic)
                {
                    SortedDictionary<Guid, List<string>> networkResources;
                    if (!_addresses.TryGetValue(pair.Key, out networkResources))
                        _addresses.Add(pair.Key, (networkResources = new SortedDictionary<Guid, List<string>>()));
                    List<string> innerDic;
                    if(!networkResources.TryGetValue(guid, out innerDic))
                        networkResources.Add(guid, (innerDic = new List<string>()));
                    innerDic.AddRange(pair.Value);
                    List<string> values = new List<string>();
                    effectedKeys[pair.Key] = (values =new List<string>());
                    foreach (KeyValuePair<Guid, List<string>> valuePair in networkResources)
                        values.AddRange(valuePair.Value);
                }
            }
            /*Uses effected keys to make the notifications Here.*/
            Notify(effectedKeys);
        }

        public static void UnRegister(Guid guid)
        {
            IDictionary<string, List<string>> effectedKeys;
            lock (_lockObj)
            {
                effectedKeys = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, SortedDictionary<Guid, List<string>>> pair in _addresses)
                {
                    if (!pair.Value.ContainsKey(guid)) continue;
                    pair.Value.Remove(guid);
                    effectedKeys.Add(pair.Key, new List<string>());
                }
                foreach (KeyValuePair<string, List<string>> pair in effectedKeys)
                {
                    SortedDictionary<Guid, List<string>> innerDic;
                    if (!_addresses.TryGetValue(pair.Key, out innerDic)) continue;
                    if (innerDic.Count == 0) _addresses.Remove(pair.Key);
                    foreach (KeyValuePair<Guid, List<string>> innerPair in innerDic) pair.Value.AddRange(innerPair.Value);
                }
            }
            /*Uses effected keys to make the notifications Here.*/
            Notify(effectedKeys);
        }

        private static void Notify(IDictionary<string, List<string>> dic)
        {
            lock (_lockObjForPublisher)
            {
                IDataPublisher<string, string[]> publisher;
                foreach (KeyValuePair<string, List<string>> pair in dic)
                {
                    if (!_publishers.TryGetValue(pair.Key, out publisher))
                        //if there has nothing with specified network key then uses the default publisher.
                        publisher = _publishers[_defaultPublisherKey];
                    publisher.Publish(pair.Key, pair.Value.ToArray());
                }
            }
        }

        #endregion

        /// <summary>
        ///    获取RRCS内部目前所支持的所有通信节点信息
        /// </summary>
        /// <returns>返回RRCS内部的所有信息</returns>
        public static IDictionary<string, List<string>> GetAllInformation()
        {
            lock (_lockObj)
            {
                IDictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                foreach (KeyValuePair<string, SortedDictionary<Guid, List<string>>> pair in _addresses)
                {
                    List<string> values = new List<string>();
                    foreach (KeyValuePair<Guid, List<string>> valuePair in pair.Value) values.AddRange(valuePair.Value);
                    dic.Add(pair.Key, values);
                }
                return dic;
            }
        }
    }
}