using KJFramework.Data.Synchronization.Enums;
using KJFramework.Data.Synchronization.Messages;
using KJFramework.Data.Synchronization.Transactions;
using KJFramework.EventArgs;
using KJFramework.Messages.Helpers;
using KJFramework.Net.Channels;
using KJFramework.Net.Exception;
using KJFramework.Net.Transaction.Messages;
using KJFramework.Tracing;
using System;
using System.Net;
using System.Threading;

namespace KJFramework.Data.Synchronization
{
    /// <summary>
    ///     远程数据发布者
    /// </summary>
    /// <typeparam name="TK">要发布数据的KEY类型</typeparam>
    /// <typeparam name="TV">要发布数据的VALUE类型</typeparam>
    public class DataBroadcaster<TK, TV> : IDataBroadcaster<TK, TV>
    {
        #region Contructor

        /// <summary>
        ///     远程数据订阅者
        /// </summary>
        /// <param name="catelog">分类名称</param>
        /// <param name="res">网络资源</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public DataBroadcaster(string catelog, INetworkResource res)
            : this(catelog, res, false)
        {
        }

        /// <summary>
        ///     远程数据订阅者
        /// </summary>
        /// <param name="catelog">分类名称</param>
        /// <param name="res">网络资源</param>
        /// <param name="isAutoReconnect">是否在网络出现故障的时候自动重连</param>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        public DataBroadcaster(string catelog, INetworkResource res, bool isAutoReconnect)
        {
            if (string.IsNullOrEmpty(catelog)) throw new ArgumentNullException("catelog");
            if (res == null) throw new ArgumentNullException("res");
            State = BroadcasterState.Disconnected;
            _resource = res;
            _isAutoReconnect = isAutoReconnect;
            _catalog = catelog;
            _keyType = typeof(TK);
            _valueType = typeof(TV);
            Initialize();
        }

        #endregion

        #region Members

        private bool _runThread = true;
        private DateTime _nextConnectTime;
        private Thread _reconnectThread;

        #endregion

        #region Methods

        /// <summary>
        ///     初始化
        /// </summary>
        private void Initialize()
        {
            try { Bind(); }
            catch(ConnectFailException ex)
            {
                _tracing.Error(ex, null);
                if (!_isAutoReconnect) throw;
                ReconnectProc();
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                throw;
            }
        }

        /// <summary>
        ///     重新连接远程终结点函数
        /// </summary>
        private void ReconnectProc()
        {
            State = BroadcasterState.Reconnecting;
            if (_reconnectThread != null) return;
            _reconnectThread = new Thread(delegate()
            {
                while (_runThread)
                {
                    TimeSpan remaining;
                    DateTime now = DateTime.Now;
                    if((remaining = (now-_nextConnectTime)).TotalSeconds < 0)
                    {
                        Thread.Sleep((Math.Abs((int) remaining.TotalSeconds)*1000));
                        continue;
                    }
                    try
                    {
                        Bind();
                        //done.
                        _reconnectThread = null;
                        return;
                    }
                    catch(ConnectFailException ex)
                    {
                        _tracing.Error(ex, null);
                        if(!_isAutoReconnect)
                        {
                            Close();
                            return;
                        }
                        //10s later will begain next round.
                        _nextConnectTime = DateTime.Now.AddSeconds(10);
                        Thread.Sleep(10000);
                    }
                    catch (System.Exception ex)
                    {
                        _tracing.Error(ex, null);
                        Close();
                    }
                }                         
            }) {Name = "BroadcastThread::Reconnect", IsBackground = true};
            _reconnectThread.Start();
        }

        /// <summary>
        ///     绑定资源
        /// </summary>
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.ArgumentException">资源类型错误</exception>
        /// <exception cref="ConnectFailException">连接失败</exception>
        /// <exception cref="System.Exception">网络资源无法被使用</exception>
        private void Bind()
        {
            if (_resource == null) throw new System.Exception("There isn't any inner network resource can be used!");
            if (_resource.Mode != ResourceMode.Remote) throw new ArgumentException("network resource *MUST* be a remote mode.");
            IPEndPoint ipEndPoint = _resource.GetResource<IPEndPoint>();
            TransportChannel transport = new TcpTransportChannel(ipEndPoint);
            transport.Connect();
            if (!transport.IsConnected) throw new ConnectFailException("Remote endpoint cannot reachable! #addr: " + ipEndPoint);
            _msgChannel = new MessageTransportChannel<BaseMessage>(transport, Global.ProtocolStack);
            _msgChannel.ReceivedMessage += MsgChannelReceivedMessage;
            _msgChannel.Disconnected += MsgChannelDisconnected;
            State = BroadcasterState.Connected;
        }

        #endregion

        #region Implementation of IDataBroadcaster<TK,TV>

        private Type _keyType;
        private Type _valueType;
        private string _catalog;
        private INetworkResource _resource;
        private readonly bool _isAutoReconnect;
        private IMessageTransportChannel<BaseMessage> _msgChannel;
        private static ITracing _tracing = TracingManager.GetTracing(typeof(DataBroadcaster<TK, TV>));

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
        ///     获取当前连接状态
        /// </summary>
        public BroadcasterState State { get; private set; }

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
        /// <exception cref="System.ArgumentNullException">参数不能为空</exception>
        /// <exception cref="System.ArgumentException">资源类型错误</exception>
        /// <exception cref="ConnectFailException">连接失败</exception>
        /// <exception cref="System.Exception">网络资源无法被使用</exception>
        public void Bind(INetworkResource resource)
        {
            _resource = resource;
            Bind();
        }

        /// <summary>
        ///     同步的广播方法
        /// </summary>
        /// <param name="key">广播信息的Key</param>
        /// <param name="value">广播信息的Value</param>
        /// <returns>广播是否成功</returns>
        /// <exception cref="System.ArgumentNullException">KEY不能为空</exception>
        public bool Broadcast(TK key, TV value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (!IsConnected) return false;
            bool result = false;
            AutoResetEvent are = new AutoResetEvent(false);
            BroadcastRequestMessage request = new BroadcastRequestMessage
            {
                Catalog = _catalog,
                Key = DataHelper.ToBytes(_keyType, key),
                Value = (value == null ? null : DataHelper.ToBytes(_valueType, value))
            };
            SyncDataTransaction trans = SyncDataTransactionManager.Instance.Create(_msgChannel);
            trans.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
            {
                BroadcastResponseMessage rsp = (BroadcastResponseMessage)e.Target;
                if (rsp.ErrorId == 0) result = true;
                are.Set();
            };
            trans.Timeout += delegate
            {
                result = false;
                are.Set();
            };
            trans.Failed += delegate
            {
                result = false;
                are.Set();
            };
            trans.SendRequest(request);
            are.WaitOne();
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
            if (callback == null) throw new ArgumentNullException("callback");
            if (!IsConnected)
            {
                callback(false);
                return;
            }
            BroadcastRequestMessage request = new BroadcastRequestMessage
            {
                Catalog = _catalog,
                Key = DataHelper.ToBytes(_keyType, key),
                Value = (value == null ? null : DataHelper.ToBytes(_valueType, value))
            };
            SyncDataTransaction trans = SyncDataTransactionManager.Instance.Create(_msgChannel);
            trans.ResponseArrived += delegate(object sender, LightSingleArgEventArgs<BaseMessage> e)
            {
                BroadcastResponseMessage rsp = (BroadcastResponseMessage)e.Target;
                callback(rsp.ErrorId == 0);
            };
            trans.Failed += delegate { callback(false); };
            trans.Timeout += delegate { callback(false); };
            trans.SendRequest(request);
        }

        /// <summary>
        ///     关闭当前广播者
        ///     <para>* 此操作将会导致断开与远程发布者的通信信道</para>
        /// </summary>
        public void Close()
        {
            if (_msgChannel == null) return;
            _msgChannel.ReceivedMessage -= MsgChannelReceivedMessage;
            _msgChannel.Disconnected -= MsgChannelDisconnected;
            if (_msgChannel.IsConnected) _msgChannel.Close();
            _runThread = false;
            State = BroadcasterState.Disconnected;
            _resource = null;
            _msgChannel = null;
            _keyType = null;
            _valueType = null;
            if (_reconnectThread != null)
            {
                try { _reconnectThread.Abort(); } catch { }
                _reconnectThread = null;
            }
            ClosedHandler(null);
        }

        /// <summary>
        ///     已关闭事件
        /// </summary>
        public event EventHandler Closed;
        protected void ClosedHandler(System.EventArgs e)
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //inner msg channel disconnected.
        void MsgChannelDisconnected(object sender, System.EventArgs e)
        {
            if (!_isAutoReconnect) Close();
            else ReconnectProc();
        }

        //rsp messages received.
        void MsgChannelReceivedMessage(object sender, LightSingleArgEventArgs<System.Collections.Generic.List<BaseMessage>> e)
        {
            IMessageTransportChannel<BaseMessage> msgChannel = (IMessageTransportChannel<BaseMessage>)sender;
            foreach (BaseMessage msg in e.Target)
            {
                _tracing.Info("L: {0}\r\nR: {1}\r\n{2}", msgChannel.LocalEndPoint, msgChannel.RemoteEndPoint, msg.ToString());
                if (!msg.TransactionIdentity.IsRequest) SyncDataTransactionManager.Instance.Active(msg.TransactionIdentity, msg);
            }
        }

        #endregion
    }
}