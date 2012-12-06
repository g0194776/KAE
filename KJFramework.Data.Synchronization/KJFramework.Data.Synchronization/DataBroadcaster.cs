using KJFramework.Data.Synchronization.Messages;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.Messages.Helpers;
using KJFramework.Net.Channels;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;
using System;
using System.Net;
using System.Threading;

namespace KJFramework.Data.Synchronization
{
    public class DataBroadcaster<TK, TV> : IDataBroadcaster<TK, TV>
    {
        #region Contructor
        /// <summary>
        ///     远程数据订阅者
        /// </summary>
        /// <param name="catelog">分类名称</param>
        /// <param name="res">网络资源</param>
        /// <param name="isAutoReconnect">是否自动启动重连的标识</param>
        public DataBroadcaster(string catelog, INetworkResource res)
        {
            if (string.IsNullOrEmpty(catelog)) throw new ArgumentNullException("catelog");
            if (res == null) throw new ArgumentNullException("res");
            _catalog = catelog;
            _keyType = typeof(TK);
            _valueType = typeof(TV);
            Bind(res);
        }
        #endregion

        #region Implementation of IDataBroadcaster<TK,TV>

        private Type _keyType;
        private Type _valueType;
        private string _catalog;
        private INetworkResource _resource;
        private static ITracing _tracing = TracingManager.GetTracing(typeof (DataBroadcaster<TK, TV>));
        private TimeSpan _broadcasterTimeout = new TimeSpan(0, 0, 60);
        private IMessageTransportChannel<BaseMessage> _msgChannel;

        /// <summary>
        ///     设置广播的超时时间
        /// </summary>
        public TimeSpan BroadcasterTimeout
        {
            get { return _broadcasterTimeout; }
            set { _broadcasterTimeout = value; }
        }

        /// <summary>
        ///     分类信息
        /// </summary>
        public string Catalog
        {
            get { return _catalog; }
            set { _catalog = value; }
        }

        /// <summary>
        ///     广播的资源
        /// </summary>
        public INetworkResource Resource
        {
            get { return _resource; }
        }

        /// <summary>
        ///     当前是否可以发送
        /// </summary>
        public bool IsConnected
        {
            get { return _msgChannel != null && _msgChannel.IsConnected; }
        }

        /// <summary>
        ///     绑定资源
        /// </summary>
        /// <param name="resource">资源信息</param>
        public void Bind(INetworkResource resource)
        {
            if (resource == null) throw new ArgumentNullException("resource");
            IPEndPoint ipEndPoint = resource.GetResource<IPEndPoint>();
            TransportChannel transport = new TcpTransportChannel(ipEndPoint);
            transport.Connect();
            if (!transport.IsConnected)
                throw new System.Exception("Remote endpoint cannot reachable! #addr: " + ipEndPoint);
            _msgChannel = new MessageTransportChannel<BaseMessage>(transport, Global.ProtocolStack);
            _msgChannel.ReceivedMessage += MsgChannel_ReceivedMessage;
            _msgChannel.Disconnected += MsgChannel_Disconnected;
        }

        void MsgChannel_Disconnected(object sender, System.EventArgs e)
        {
            Close();
        }

        void MsgChannel_ReceivedMessage(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<System.Collections.Generic.List<BaseMessage>> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>) sender;
            foreach (BaseMessage msg in e.Target)
            {
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, msg.ToString());
                if (!msg.TransactionIdentity.IsRequest) SyncDataTransactionManager.Instance.Active(msg.TransactionIdentity, msg);
            }
        }

        /// <summary>
        ///     同步的广播方法
        /// </summary>
        /// <param name="key">广播信息的Key</param>
        /// <param name="value">广播信息的Value</param>
        /// <returns>广播是否成功</returns>
        public bool Broadcast(TK key, TV value)
        {
            if (key == null) throw new ArgumentNullException("key");
            bool result = false;
            AutoResetEvent are = new AutoResetEvent(false);
            BroadcastRequestMessage request = new BroadcastRequestMessage();
            request.Catalog = _catalog;
            request.Key = DataHelper.ToBytes(_keyType, key);
            request.Value = (value == null ? null : DataHelper.ToBytes(_valueType, value));
            SyncDataTransaction trans = SyncDataTransactionManager.Instance.Create(_msgChannel);
            trans.ResponseArrived += delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<BaseMessage> e)
            {
                BroadcastResponseMessage rsp = (BroadcastResponseMessage)e.Target;
                if (rsp.ErrorId == 0) result = true;
                are.Set();
            };
            trans.Timeout += delegate { result = false; };
            trans.Failed += delegate { result = false; };
            trans.SendRequest(request);
            TimeSpan ts = new TimeSpan(_broadcasterTimeout.Ticks + 10000);
            if (!are.WaitOne(ts)) result = false;
            return result;
        }

        /// <summary>
        ///     广播的异步方法
        /// </summary>
        /// <param name="key">广播信息的Key</param>
        /// <param name="value">广播信息的Value</param>
        /// <param name="callback">回调函数，返回广播是否成功</param>
        public void BroadcastAsync(TK key, TV value, Action<bool> callback)
        {
            if (key == null) throw new ArgumentNullException("key");
            BroadcastRequestMessage request = new BroadcastRequestMessage();
            request.Catalog = _catalog;
            request.Key = DataHelper.ToBytes(_keyType, key);
            request.Value = (value == null ? null : DataHelper.ToBytes(_valueType, value));
            SyncDataTransaction trans = SyncDataTransactionManager.Instance.Create(_msgChannel);
            trans.ResponseArrived += delegate(object sender, KJFramework.EventArgs.LightSingleArgEventArgs<BaseMessage> e)
            {
                BroadcastResponseMessage rsp = (BroadcastResponseMessage)e.Target;
                if (rsp.ErrorId == 0)
                {
                    if (callback != null)
                        callback(true);
                }
            };
            trans.Failed += delegate { if (callback != null) callback(false); };
            trans.Timeout += delegate { if (callback != null) callback(false); };
            trans.SendRequest(request);
        }

        /// <summary>
        ///     关闭当前广播者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
        /// </summary>
        public void Close()
        {
            if (_msgChannel == null) return;
            _msgChannel.ReceivedMessage -= MsgChannel_ReceivedMessage;
            _msgChannel.Disconnected -= MsgChannel_Disconnected;
            if (_msgChannel.IsConnected)
                _msgChannel.Close();
            _msgChannel = null;
        }

        #endregion
    }
}