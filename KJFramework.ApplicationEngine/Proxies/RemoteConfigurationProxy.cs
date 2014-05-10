using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using KJFramework.ApplicationEngine.Attributes;
using KJFramework.EventArgs;
using KJFramework.Helpers;
using KJFramework.Net.Channels;
using KJFramework.Net.Channels.Enums;
using KJFramework.Net.Transaction;
using KJFramework.Net.Transaction.Comparers;
using KJFramework.Net.Transaction.Helpers;
using KJFramework.Net.Transaction.Managers;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Platform.Deploy.CSN.ProtocolStack;
using KJFramework.Platform.Deploy.Metadata.Objects;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Proxies
{
    /// <summary>
    ///     远程配置信息代理器元接口，提供了相关的基本操作
    /// </summary>
    public class RemoteConfigurationProxy : IRemoteConfigurationProxy
    {
        #region Constructor

        /// <summary>
        ///     远程配置信息代理器元接口，提供了相关的基本操作
        /// </summary>
        /// <exception cref="Exception">参数不能为空</exception>
        public RemoteConfigurationProxy(string csnAddress)
        {
            if (string.IsNullOrEmpty(csnAddress)) throw new ArgumentNullException("csnAddress");
            _csnAddress = csnAddress;
            _protocolStack = new CSNProtocolStack();
            _protocolStack.Initialize();
        }

        #endregion

        #region Members

        private readonly string _csnAddress;
        private object _lockTablesObj = new object();
        private IMessageTransportChannel<BaseMessage> _channel;
        private static string _subscriberKey = "SystemSubscriber";
        private static CSNProtocolStack _protocolStack;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (RemoteConfigurationProxy));
        private static readonly MessageTransactionManager _transactionManager = new MessageTransactionManager(new TransactionIdentityComparer());
        private readonly Dictionary<string, string> _configCaches = new Dictionary<string, string>();
        private readonly Dictionary<string, DataTable> _cacheTables = new Dictionary<string, DataTable>();
        private readonly Dictionary<string, Dictionary<string, string>> _cacheKeys = new Dictionary<string, Dictionary<string, string>>();

        #endregion

        #region Methods

        /// <summary>
        ///     准备与CSN之间的通信连接
        /// </summary>
        /// <exception cref="Exception">准备失败</exception>
        private void PrepareConnection()
        {
            if (_channel != null && _channel.IsConnected) return;
            int offset = _csnAddress.LastIndexOf(':');
            string ip = _csnAddress.Substring(0, offset);
            int port = int.Parse(_csnAddress.Substring(offset + 1, _csnAddress.Length - (offset + 1)));
            
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), port);
            ITransportChannel channel = new TcpTransportChannel(iep);
            channel.Connect();
            if (!channel.IsConnected) throw new System.Exception("Cannot connect to target CSN service. #address: " + iep);
            _channel = new MessageTransportChannel<BaseMessage>((IRawTransportChannel)channel, _protocolStack);
            _channel.Disconnected += ChannelDisconnected;
            _channel.ReceivedMessage += ChannelReceivedMessage;
        }

        /// <summary>
        ///     获取缓存的数据表
        /// </summary>
        /// <param name="key">数据表唯一标示</param>
        /// <returns>返回数据表</returns>
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
        ///     获取某个字段的值
        ///     <para>* 使用此方法将会自动从核心配置表中读取数据</para>
        /// </summary>
        /// <param name="role">角色名称</param>
        /// <param name="field">字段名</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string role, string field)
        {
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException("role");
            return GetField("CSNDB", "HA_ConfigInfo", role, field);
        }

        /// <summary>
        ///     获取某个字段的值
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="service">服务名称</param>
        /// <param name="field">字段名</param>
        /// <returns>返回相应字段的值</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public string GetField(string database, string table, string service, string field)
        {
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

            BusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
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
        ///     获取部分配置信息
        /// </summary>
        /// <param name="configKey">配置文件KEY名称</param>
        /// <returns>如果不存在指定条件的配置文件，则返回null</returns>
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
            BusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
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
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table"></param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public DataTable GetTable(string database, string table)
        {
            return GetTable(database, table, true);
        }

        /// <summary>
        ///     获取指定表的数据
        /// </summary>
        /// <param name="database">数据库名</param>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
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
            BusinessMessageTransaction transaction = _transactionManager.Create(IdentityHelper.Create(_channel.LocalEndPoint, TransportChannelTypes.TCP), _channel);
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
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>返回表数据</returns>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public T[] GetTable<T>(string table) where T : class, new()
        {
            return GetTable<T>(table, true);
        }

        /// <summary>
        ///     获取指定表的所有记录，并将每一个记录包装为指定类型的形式
        ///     <para>* 使用此方法只能从CSNDB中过去指定表的内容</para>
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="hasCache">缓存标识</param>
        /// <returns>返回表数据</returns>
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
                    anonymous.Property.SetValue(element, ConvertValue(anonymous.Property.PropertyType, row.Columns[Array.IndexOf(dataTable.Columns, anonymous.Attribute.ItemName)].Value), null);
                targets[index++] = element;
            }

            #endregion

            return targets;
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
                if(message is CSNGetKeyDataResponseMessage || message is CSNGetDataTableResponseMessage)
                    _transactionManager.Active(message.TransactionIdentity, message);
            }
        }

        #endregion
    }
}