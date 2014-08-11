using System;
using System.Collections.Generic;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    转为KPP准备的内部配置获取代理器
    /// </summary>
    internal class KPPConfigurationProxy : IRemoteConfigurationProxy
    {
        #region Constructor.

        /// <summary>
        ///    转为KPP准备的内部配置获取代理器
        /// </summary>
        /// <param name="proxy">KAE宿主代理器</param>
        public KPPConfigurationProxy(IKAEHostProxy proxy)
        {
            _proxy = proxy;
        }

        #endregion

        #region Members.

        private readonly IKAEHostProxy _proxy;
        private object _lockCallbackObj = new object();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (KPPConfigurationProxy));
        private readonly Dictionary<string, Action<string>> _callbacks = new Dictionary<string, Action<string>>();

        #endregion

        #region Methods.

        /// <summary>
        ///     根据一个角色名和一个配置项的KEY名称从远程CSN服务获取一个配置信息
        /// </summary>
        /// <param name="role">角色名</param>
        /// <param name="field">配置信息的KEY</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field, Action<string> callback = null)
        {
            string value = _proxy.GetField(role, field);
            if (callback == null) return value;
            string tempKey = string.Format("CSNDB.HA_ConfigInfo.{0}.{1}", role, field);
            lock (_lockCallbackObj)
            {
                Action<string> tempCallback;
                if (!_callbacks.TryGetValue(tempKey, out tempCallback)) _callbacks[tempKey] = callback;
            }
            return value;
        }

        [Obsolete("#We have not supported this method yet!", true)]
        public string GetPartialConfig(string configKey)
        {
            throw new NotImplementedException();
        }

        [Obsolete("#We have not supported this method yet!", true)]
        public DataTable GetTable(string database, string table)
        {
            throw new NotImplementedException();
        }

        [Obsolete("#We have not supported this method yet!", true)]
        public DataTable GetTable(string database, string table, bool hasCache)
        {
            throw new NotImplementedException();
        }

        [Obsolete("#We have not supported this method yet!", true)]
        public T[] GetTable<T>(string table) where T : class, new()
        {
            throw new NotImplementedException();
        }

        [Obsolete("#We have not supported this method yet!", true)]
        public T[] GetTable<T>(string table, bool hasCache) where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///    接收到了来自远程CSN的配置信息更新推送
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            lock (_lockCallbackObj)
            {
                Action<string> tempCallback;
                if (!_callbacks.TryGetValue(key, out tempCallback)) return;
                //callback.
                try { tempCallback(value); }
                catch (System.Exception ex) { _tracing.Error(ex, null); }
            }
        }

        /// <summary>
        ///    如果收到了来自CSN的配置信息变更通知，则触发此事件
        /// </summary>
        [Obsolete("#We have not supported this method yet!", true)]
        public event EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> ConfigurationUpdated;

        #endregion
    }
}