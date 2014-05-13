﻿using KJFramework.ApplicationEngine.Helpers;
using KJFramework.Data.Synchronization;
using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Factories;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

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
    }
}