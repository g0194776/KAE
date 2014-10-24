using System.Collections.Generic;
using KJFramework.ApplicationEngine.Proxies;

namespace KJFramework.ApplicationEngine.Factories
{
    /// <summary>
    ///    KAE内部所使用的默认资源工厂
    /// </summary>
    internal class DefaultInternalResourceFactory : IInternalResourceFactory
    {
        #region Members.

        private IDictionary<string, object> _resources = new Dictionary<string, object>();
        
        #endregion

        #region Methods.

        /// <summary>
        ///    初始化当前资源工厂
        /// </summary>
        public void Initialize()
        {
            _resources.Add(KAESystemInternalResource.APPDownloader, new RemotingApplicationDownloader());
            _resources.Add(KAESystemInternalResource.KISProxy, new RemotingKISProxy(SystemWorker.Instance.ConfigurationProxy.GetField("KAEWorker", "KIS-Address")));
        }

        /// <summary>
        ///    通过一个字符串的全名称来获取指定的资源
        /// </summary>
        /// <param name="fullname">资源的全名称</param>
        /// <returns>返回资源实例，如果不存在则返回null</returns>
        public object GetResource(string fullname)
        {
            object obj;
            return (this._resources.TryGetValue(fullname, out obj) ? obj : null);
        }

        #endregion
    }
}