using KJFramework.ApplicationEngine.Attributes;
using KJFramework.Data.Synchronization.EventArgs;
using KJFramework.EventArgs;
using KJFramework.Helpers;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.CSN.NetworkLayer;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///    独立的获取远程配置信息代理器
    /// </summary>
    public class SolitaryRemoteConfigurationProxy : IRemoteConfigurationProxy
    {
        #region Constructor

        /// <summary>
        ///     独立的获取远程配置信息代理器
        /// </summary>
        /// <param name="csnAddress">远程CSN地址</param>
        /// <param name="csnDataPublisherAddress">远程CSN的数据发布者访问地址</param>
        /// <exception cref="ArgumentNullException">远程CSN地址不能为空</exception>
        public SolitaryRemoteConfigurationProxy(string csnAddress, string csnDataPublisherAddress = null)
        {
            if (string.IsNullOrEmpty(csnAddress)) throw new ArgumentNullException("csnAddress");
            _csnAddress = csnAddress;
            _csnDataPublisherAddress = csnDataPublisherAddress;
            _protocolStack = new CSNProtocolStack();
            _protocolStack.Initialize();
        }

        #endregion

        #region Members

        private readonly string _csnAddress;
        private readonly string _csnDataPublisherAddress;
        private IRemoteDataSyncProxy _syncProxy;
        private object _lockTablesObj = new object();
        private object _lockCallbackObj = new object();
        private IMessageTransportChannel<BaseMessage> _channel;
        private static string _subscriberKey = "SystemSubscriber";
        private static CSNProtocolStack _protocolStack;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (SolitaryRemoteConfigurationProxy));
        private static readonly CSNMessageTransactionManager _transactionManager = new CSNMessageTransactionManager(new TransactionIdentityComparer());
        private readonly Dictionary<string, string> _configCaches = new Dictionary<string, string>();
        private readonly Dictionary<string, Action<string>> _callbacks = new Dictionary<string, Action<string>>();
        private readonly Dictionary<string, DataTable> _cacheTables = new Dictionary<string, DataTable>();
        private readonly Dictionary<string, Dictionary<string, string>> _cacheKeys = new Dictionary<string, Dictionary<string, string>>();

        #endregion

        #region Methods

        /// <summary>
        ///    准备到远程CSN服务的TCP链接
        /// </summary>
        /// <exception cref="Exception">无法连接到远程的CSN服务</exception>
        private void PrepareConnection()
        {
            if (_channel != null && _channel.IsConnected) return;
            if (!string.IsNullOrEmpty(_csnDataPublisherAddress) && _syncProxy == null)
            {
                _syncProxy = new RemoteDataSyncProxy(_csnDataPublisherAddress);
                _syncProxy.Regist("*", SyncUpdatingConfig, true);
            }
            int offset = _csnAddress.LastIndexOf(':');
            string ip = _csnAddress.Substring(0, offset);
            int port = int.Parse(_csnAddress.Substring(offset + 1, _csnAddress.Length - (offset + 1)));

            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
            ITransportChannel channel = new CSNTcpTransportChannel(iep);
            channel.Connect();
            if (!channel.IsConnected) throw new System.Exception("Cannot connect to target CSN service. #address: " + iep);
            _channel = new CSNMessageTransportChannel<BaseMessage>((ICSNRawTransportChannel)channel, _protocolStack);
            _channel.Disconnected += ChannelDisconnected;
            _channel.ReceivedMessage += ChannelReceivedMessage;
        }

        /// <summary>
        ///    获取缓存表
        /// </summary>
        /// <param name="key">缓存KEY</param>
        /// <returns>返回缓存数据表</returns>
        private DataTable GetCacheTable(string key)
        {
            lock (_lockTablesObj)
            {
                DataTable dataTable;
                return _cacheTables.TryGetValue(key, out dataTable) ? dataTable : null;
            }
        }

        private object ConvertValue(Type type, string value)
        {
            return Convert.ChangeType(value, type);
        }

        #endregion

        #region Implementation of IRemoteConfigurationProxy
        
        /// <summary>
        ///     根据一个角色名和一个配置项的KEY名称来获取一个配置信息
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">配置信息的KEY</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field, Action<string> callback = null)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException("field");
            return GetField("CSNDB", "HA_ConfigInfo", role, field, callback);
        }

        /// <summary>
        ///     根据一个数据库名、表名称、角色名和一个配置项的KEY名称来获取一个配置信息
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="table">数据表名称</param>
        /// <param name="service">角色名</param>
        /// <param name="field">配置信息的KEY</param>
        /// <param name="callback">配置信息更新后的回调函数</param>
        /// <returns>返回相应的配置信息</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        private string GetField(string database, string table, string service, string field, Action<string> callback = null)
        {
            if (string.IsNullOrEmpty(database)) throw new ArgumentNullException("database");
            if (string.IsNullOrEmpty(table)) throw new ArgumentNullException("table");
            if (string.IsNullOrEmpty(service)) throw new ArgumentNullException("service");
            if (string.IsNullOrEmpty(field)) throw new ArgumentNullException("field");
            string result = null;
            #region Get cache first.

            Dictionary<string, string> items;
            //has cache.
            if (_cacheKeys.TryGetValue(service, out items))
                return items.TryGetValue(field, out result) ? result : null;

            #endregion
            PrepareConnection();
            #region Get remote config.

            System.Exception exception = null;
            CSNGetKeyDataRequestMessage requestMessage = new CSNGetKeyDataRequestMessage();
            requestMessage.DatabaseName = database;
            requestMessage.TableName = table;
            requestMessage.ServiceName = service;

            CSNBusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
            if (transaction == null) throw new System.Exception("Cannot create a CSN message transaction!");
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
            {
                CSNGetKeyDataResponseMessage rspMessage = (CSNGetKeyDataResponseMessage)e.Target;
                #region Add Cache.

                Dictionary<string, string> values = new Dictionary<string, string>();
                if (rspMessage.Items != null)
                {
                    foreach (KeyValueItem item in rspMessage.Items)
                        values.Add(item.Key, item.Value);
                    _cacheKeys.Add(service, values);
                }
                //get result.
                result = values.ContainsKey(field) ? values[field] : null;

                #endregion

                #region Register subscriber & updating callbacks.

                if (callback != null)
                {
                    //Registers configuration updating callbacks.
                    lock (_lockCallbackObj)
                    {
                        Action<string> tempCallback;
                        string tempKey = string.Format("{0}.{1}.{2}.{3}", database, table, service, field);
                        if (!_callbacks.TryGetValue(tempKey, out tempCallback)) _callbacks[tempKey] = callback;
                    }
                }

                #endregion
                autoResetEvent.Set();
            };
            transaction.Failed += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                exception = new System.Exception("Failed! Cannot get key data config from CSN service, #key: " + field);
                autoResetEvent.Set();
            };
            transaction.Timeout += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                exception = new System.Exception("Timeout! Cannot get key data config from CSN service, #key: " + field);
                autoResetEvent.Set();
            };
            transaction.SendRequest(requestMessage);
            //wait 35s.
            autoResetEvent.WaitOne(35000);
            autoResetEvent.Dispose();
            if (exception != null) throw exception;

            #endregion

            return result;
        }

        /// <summary>
        ///     获取指定配置文件中的配置信息段落
        /// </summary>
        /// <param name="configKey">配置信息段落名称</param>
        /// <returns>返回指定的配置信息段落</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetPartialConfig(string configKey)
        {
            if (string.IsNullOrEmpty(configKey)) throw new ArgumentNullException("configKey");
            #region Get cache first.

            string config = null;
            //has cache.
            if (_configCaches.TryGetValue(configKey, out config)) return config;

            #endregion
            PrepareConnection();
            #region Get remote config.

            CSNGetPartialConfigRequestMessage requestMessage = new CSNGetPartialConfigRequestMessage();
            requestMessage.Key = configKey;
            CSNBusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
            if (transaction == null) throw new System.Exception("Cannot create a CSN message transaction!");
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
            {
                CSNGetPartialConfigResponseMessage rspMsg = (CSNGetPartialConfigResponseMessage)e.Target;
                if(rspMsg.ErrorId == 0 && !string.IsNullOrEmpty(rspMsg.Config))
                {
                    //add cache.
                    _configCaches[configKey] = rspMsg.Config;
                    config = rspMsg.Config;
                }
                autoResetEvent.Set();
            };
            transaction.Failed += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                autoResetEvent.Set();
            };
            transaction.Timeout += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                autoResetEvent.Set();
            };
            transaction.SendRequest(requestMessage);
            //wait 35s.
            autoResetEvent.WaitOne(35000);
            autoResetEvent.Dispose();
            return config;

            #endregion
        }

        /// <summary>
        ///     根据指定的数据库名和数据表名获取一个数据集合
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="table">数据表名称</param>
        /// <returns>返回指定的数据集合</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public DataTable GetTable(string database, string table)
        {
            return GetTable(database, table, true);
        }

        /// <summary>
        ///     根据指定的数据库名和数据表名获取一个数据集合
        /// </summary>
        /// <param name="database">数据库名称</param>
        /// <param name="table">数据表名称</param>
        /// <param name="hasCache">是否缓存标示</param>
        /// <returns>返回指定的数据集合</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public DataTable GetTable(string database, string table, bool hasCache)
        {
            DataTable result = null;
            System.Exception exception = null;
            if (hasCache)
            {
                //get cache by default.
                if ((result = GetCacheTable(string.Format("{0}.{1}", database, table))) != null) return result;
            }
            PrepareConnection();
            CSNGetDataTableRequestMessage requestMessage = new CSNGetDataTableRequestMessage();
            requestMessage.DatabaseName = database;
            requestMessage.TableName = table;
            CSNBusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
            if (transaction == null) throw new System.Exception("Cannot create a CSN message transaction!");
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            transaction.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
            {
                CSNGetDataTableResponseMessage responseMessage = (CSNGetDataTableResponseMessage)e.Target;
                result = responseMessage.Tables == null ? null : responseMessage.Tables;
                autoResetEvent.Set();
            };
            transaction.Failed += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                exception = new System.Exception("Failed! Cannot get table data config from CSN service, #table: " + table);
                autoResetEvent.Set();
            };
            transaction.Timeout += delegate
            {
                _transactionManager.Remove(transaction.Identity);
                exception = new System.Exception("Timeout! Cannot get table data config from CSN service, #table: " + table);
                autoResetEvent.Set();
            };
            transaction.SendRequest(requestMessage);
            //wait 35s.
            autoResetEvent.WaitOne(35000);
            autoResetEvent.Dispose();
            if (exception != null) throw exception;
            #region Add table cahce.

            if (hasCache && result != null)
            {
                lock (_lockTablesObj)
                    _cacheTables.Add(string.Format("{0}.{1}", database, table), result);
            }

            #endregion
            return result;
        }

        /// <summary>
        ///     根据指定的数据表名获取一个数据集合，并将这个数据集合中的每一行转换为一个自定义类型
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <returns>返回自定义的数据类型集合</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public T[] GetTable<T>(string table) where T : class, new()
        {
            return GetTable<T>(table, true);
        }

        /// <summary>
        ///     根据指定的数据表名获取一个数据集合，并将这个数据集合中的每一行转换为一个自定义类型
        /// </summary>
        /// <param name="table">数据表名称</param>
        /// <param name="hasCache">是否缓存标示</param>
        /// <returns>返回自定义的数据类型集合</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public T[] GetTable<T>(string table, bool hasCache) where T : class, new()
        {
            #region Security check & initializtion.

            if (string.IsNullOrEmpty(table)) throw new ArgumentNullException("table");
            DataTable dataTable = GetTable("CSNDB", table, hasCache);
            if (dataTable == null) return null;
            if (dataTable.Rows == null || dataTable.Rows.Length == 0) return null;
            T[] targets = new T[dataTable.Rows.Length];
            var result = typeof(T).GetProperties()
                .Where(p => AttributeHelper.GetCustomerAttribute<CodeItemAttribute>(p) != null)
                .Select(p => new { Property = p, Attribute = AttributeHelper.GetCustomerAttribute<CodeItemAttribute>(p) });
            if (result.Count() == 0) return null;

            #endregion

            #region Convertions.

            int index = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                T element = new T();
                //set value for each property.
                foreach (var anonymous in result)
                    if (anonymous.Property.PropertyType == typeof(bool))
                        anonymous.Property.SetValue(element, (row.Columns[Array.IndexOf(dataTable.Columns, anonymous.Attribute.ItemName)].Value == "1"), null);
                    else anonymous.Property.SetValue(element, ConvertValue(anonymous.Property.PropertyType, row.Columns[Array.IndexOf(dataTable.Columns, anonymous.Attribute.ItemName)].Value), null);
                targets[index++] = element;
            }

            #endregion

            return targets;
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
                Action<string> callback;
                try { if (_callbacks.TryGetValue(key, out callback)) callback(value); }
                catch (System.Exception ex) { _tracing.Error(ex, null); }
            }
            ConfigurationUpdatedHandler(new LightSingleArgEventArgs<Tuple<string, string>>(new Tuple<string, string>(key, value)));
        }

        /// <summary>
        ///    如果收到了来自CSN的配置信息变更通知，则触发此事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> ConfigurationUpdated;
        protected virtual void ConfigurationUpdatedHandler(LightSingleArgEventArgs<Tuple<string, string>> e)
        {
            EventHandler<LightSingleArgEventArgs<Tuple<string, string>>> handler = ConfigurationUpdated;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///    Asynchronously actives callback method for updating specified configuiration value.
        /// </summary>
        /// <param name="sender">event owner</param>
        /// <param name="e">specified based-on KEY-VALUE style's value pair.</param>
        private void SyncUpdatingConfig(object sender, LightSingleArgEventArgs<DataRecvEventArgs<string, string>> e)
        {
            UpdateConfiguration(e.Target.Key, e.Target.Value);
        }

        #endregion

        #region Events

        void ChannelDisconnected(object sender, System.EventArgs e)
        {
            _channel.Disconnected -= ChannelDisconnected;
            _channel.ReceivedMessage -= ChannelReceivedMessage;
        }

        void ChannelReceivedMessage(object sender, LightSingleArgEventArgs<List<BaseMessage>> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>)sender;
            foreach (BaseMessage message in e.Target)
            {
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, message.ToString());
                if(message is CSNGetKeyDataResponseMessage || message is CSNGetDataTableResponseMessage || message is CSNGetPartialConfigResponseMessage)
                    _transactionManager.Active(message.TransactionIdentity, message);
            }
        }

        #endregion
    }
}