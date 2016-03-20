using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using KJFramework.EventArgs;
using KJFramework.Platform.Deploy.Metadata.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     基于ETCD集群的远程配置代理器
    /// </summary>
    internal sealed  class EtcdRemoteConfigurationProxy : IRemoteConfigurationProxy
    {
        #region Constructor.

        /// <summary>
        ///     基于ETCD集群的远程配置代理器
        /// </summary>
        /// <param name="etcdUri">远程ETCD集群负载地址</param>
        public EtcdRemoteConfigurationProxy(Uri etcdUri)
        {
            _etcdUri = etcdUri;
        }

        #endregion

        #region Members.

        private readonly Uri _etcdUri;
        private readonly object _lockObj = new object();
        private readonly Dictionary<string, string> _dic = new Dictionary<string, string>(); 

        #endregion

        #region Implementation of IRemoteConfigurationProxy

        /// <summary>
        ///     获取某个字段的值
        ///     <para>* 使用此方法将会自动从核心配置表中读取数据</para>
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">字段名</param>
        /// <param name="useCache">是否使用本地缓存的标示</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field, bool useCache = true)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException("field");
            string value;
            string relativePath = string.Format("configs/pairs/{0}/{1}", role, field);
            lock(_lockObj) 
                if (useCache && _dic.TryGetValue(relativePath, out value)) return value;
            Uri destUri = new Uri(_etcdUri, relativePath);
            JObject obj = InnerGetEtcdValue(destUri);
            //unhandled exception occured during communicating with remote ETCD.
            if (obj["errorCode"] != null) throw new KeyNotFoundException(obj.ToString());
            //correctly retrieved the value of targeted KEY.
            value = obj["node"]["value"].ToString();
            lock (_lockObj) _dic[relativePath] = value;
            return value;
        }

        /// <summary>
        ///     获取某个字段的值
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="service">服务名称</param>
        /// <param name="field">字段名</param>
        /// <param name="useCache">是否使用本地缓存的标示</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        [Obsolete("This method will be discard after we release the new version of v2.1.0.")]
        public string GetField(string database, string table, string service, string field, bool useCache = true)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取某个字段的值
        ///     <para>* 使用此方法将会自动从核心配置表中读取数据</para>
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">字段名</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field, Action<string> callback = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取部分配置信息
        /// </summary>
        /// <param name="configKey">配置文件KEY名称</param>
        /// <returns>如果不存在指定条件的配置文件，则返回null</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetPartialConfig(string configKey)
        {
            if (string.IsNullOrEmpty(configKey)) throw new ArgumentNullException("configKey");
            string value;
            string relativePath = string.Format("configs/*PARTIAL-CONFIGS*/{0}", configKey);
            lock (_lockObj)
                if (_dic.TryGetValue(relativePath, out value)) return value;
            Uri destUri = new Uri(_etcdUri, relativePath);
            JObject obj = InnerGetEtcdValue(destUri);
            //unhandled exception occured during communicating with remote ETCD.
            if (obj["errorCode"] != null) throw new KeyNotFoundException(obj["message"].ToString());
            //correctly retrieved the value of targeted KEY.
            value = obj["node"]["value"].ToString();
            lock (_lockObj) _dic[relativePath] = value;
            return value;
        }

        /// <summary>
        ///    接收到了来自远程CSN的配置信息更新推送
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="value">VALUE</param>
        public void UpdateConfiguration(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        [Obsolete("This method will be discard after we release the new version of v2.1.0.")]
        public DataTable GetTable(string database, string table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        [Obsolete("This method will be discard after we release the new version of v2.1.0.")]
        public DataTable GetTable(string database, string table, bool hasCache)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        [Obsolete("This method will be discard after we release the new version of v2.1.0.")]
        public T[] GetTable<T>(string table) where T : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        [Obsolete("This method will be discard after we release the new version of v2.1.0.")]
        public T[] GetTable<T>(string table, bool hasCache) where T : class, new()
        {
            throw new NotImplementedException();
        }

        private JObject InnerGetEtcdValue(Uri destUri)
        {
            HttpWebResponse response = null;
            HttpWebRequest request = HttpWebRequest.CreateHttp(destUri);
            request.Method = "GET";
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                return InnerGetHttpResponseContent(response);
            }
            catch (WebException ex) { return InnerGetHttpResponseContent(ex.Response); }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response.Dispose();
                }
            }
        }

        private JObject InnerGetHttpResponseContent(WebResponse response)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string content = reader.ReadToEnd();
                return (JObject) JsonConvert.DeserializeObject(content);
            }
        }

        #endregion

        #region Events.

        /// <summary>
        ///    如果收到了来自CSN的配置信息变更通知，则触发此事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> ConfigurationUpdated;
        private void OnConfigurationUpdated(LightSingleArgEventArgs<Tuple<string, string>> e)
        {
            EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> handler = ConfigurationUpdated;
            if (handler != null) handler(this, e);
        }

        #endregion

    }
}
