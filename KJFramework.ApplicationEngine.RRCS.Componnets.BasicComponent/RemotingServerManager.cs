using System.Collections.Concurrent;
using System.Collections.Generic;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.RRCS.Componnets.BasicComponent
{
    /// <summary>
    ///     远程服务地址管理器
    /// </summary>
    internal  static class RemotingServerManager
    {
        #region Members.

        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemotingServerManager));
        private static readonly object _lockObj = new object();
        //key = MessageIdentity + Supported Protocol + Application's Level + Application's Version
        private static readonly Dictionary<string, List<string>> _addresses = new Dictionary<string, List<string>>();

        #endregion

        #region Methods.

        public static void Register(IDictionary<string, List<string>>  dic)
        {
            lock (_lockObj)
            {
                foreach (KeyValuePair<string, List<string>> pair in dic)
                {
                    List<string> networkResources;
                    if (!_addresses.TryGetValue(pair.Key, out networkResources))
                        _addresses.Add(pair.Key, (networkResources = new List<string>()));
                    networkResources.AddRange(pair.Value);
                }
            }
        }

        #endregion
    }
}