using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels.Buffers;
using KJFramework.Net.Channels.Spy;
using KJFramework.Net.ProtocolStacks;

namespace KJFramework.Net.Channels.OneWay
{
    /// <summary>
    ///     单方向信道抽象父类，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息父类类型</typeparam>
    public abstract class OnewayChannel<T> : IOnewayChannel<T>
        where T : class
    {
        #region Constructor

        /// <summary>
        ///     单方向信道抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="protocolStack">协议栈</param>
        protected OnewayChannel(IProtocolStack<T> protocolStack)
        {
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!protocolStack.Initialize()) throw new ArgumentException("Cannot initialize current protocol stack!");
            _protocolStack = protocolStack;
        }

        /// <summary>
        ///     单方向信道抽象父类，提供了相关的基本操作
        /// </summary>
        /// <param name="channel">基于流的通讯信道</param>
        /// <param name="protocolStack">协议栈</param>
        protected OnewayChannel(IRawTransportChannel channel, IProtocolStack<T> protocolStack)
        {
            if (channel == null) throw new ArgumentNullException("channel");
            if (!channel.IsConnected) throw new ArgumentException("Current raw transport channel hsa been disconnected!");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (!protocolStack.Initialize()) throw new ArgumentException("Cannot initialize current protocol stack!");
            _channel = channel;
            if (_channel.Buffer == null) _channel.Buffer = new ReceiveBuffer(102400);
            _logicalAddress = _channel.LogicalAddress;
            _address = _channel.Address;
            _channel.Disconnected += RawChannelDisconnected;
            _channel.ReceivedData += RawChannelReceivedData;
            _protocolStack = protocolStack;
        }

        #endregion

        #region Members

        protected IRawTransportChannel _channel;
        private readonly ConcurrentDictionary<Type, IMessageSpy<T>> _spys = new ConcurrentDictionary<Type, IMessageSpy<T>>();

        #endregion

        #region Methods

        /// <summary>
        ///     使用此方法来发送响应消息
        /// </summary>
        /// <param name="message">响应消息</param>
        protected abstract void SendCallbackAsync(T message);

        #endregion

        #region Implementation of ICommunicationChannelAddress

        protected IPEndPoint _address;
        protected Uri.Uri _logicalAddress;
        protected bool _connected;
        protected readonly DateTime _createTime;
        protected readonly Guid _key;
        protected readonly IProtocolStack<T> _protocolStack;

        /// <summary>
        ///     获取或设置物理地址
        /// </summary>
        public IPEndPoint Address
        {
            get { return _address; }
            set
            {
                _address = value;
                if (_channel != null) _channel.Address = _address;
            }
        }

        /// <summary>
        ///     获取或设置逻辑地址
        /// </summary>
        public Uri.Uri LogicalAddress
        {
            get { return _logicalAddress; }
            set
            {
                _logicalAddress = value;
                if (_channel != null) _channel.LogicalAddress = _logicalAddress;
            }
        }

        #endregion

        #region Implementation of IOnewayChannel<T>

        /// <summary>
        ///     获取当前信道的连接状态
        /// </summary>
        public bool Connected
        {
            get { return _connected; }
        }

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取通道唯一标示
        /// </summary>
        public Guid Key
        {
            get { return _key; }
        }

        /// <summary>
        ///     获取协议栈
        /// </summary>
        public IProtocolStack<T> ProtocolStack
        {
            get { return _protocolStack; }
        }

        /// <summary>
        ///     连接到远程终结点
        /// </summary>
        /// <param name="channel">基于流的通讯信道</param>
        /// <exception cref="NullReferenceException">远程终结点地址不能为空</exception>
        public void Connect(IRawTransportChannel channel)
        {
            try
            {
                #region Clear resource before.

                //get clear for org channel.
                if (_channel != null)
                {
                    _channel.Buffer = null;
                    _channel.Disconnected -= RawChannelDisconnected;
                    _channel.ReceivedData -= RawChannelReceivedData;
                    if (_channel.IsConnected) _channel.Disconnect();
                    _channel = null;
                }

                #endregion

                if (channel == null) throw new ArgumentNullException("channel");
                channel.Connect();
                if (_connected = channel.IsConnected)
                {
                    _channel = channel;
                    if (_channel.Buffer == null) _channel.Buffer = new ReceiveBuffer(102400);
                    _logicalAddress = _channel.LogicalAddress;
                    _address = _channel.Address;
                    _channel.Disconnected += RawChannelDisconnected;
                    _channel.ReceivedData += RawChannelReceivedData;
                    ChannelConnectedHandler(null);
                }
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        ///     断开当前信道的连接
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (!_connected) return;
                _connected = false;
                _channel.Disconnect();
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        /// <summary>
        ///     注册一个消息拦截器
        /// </summary>
        /// <param name="spy">消息拦截器</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">非法参数</exception>
        public void RegistSpy(IMessageSpy<T> spy)
        {
            if (spy == null) throw new ArgumentNullException("spy");
            if (spy.SupportType == null) throw new ArgumentException("IMessageSpy.SupportType cannot be null!");
            _spys.TryAdd(spy.SupportType, spy);
        }

        /// <summary>
        ///     信道已连接事件
        /// </summary>
        public event EventHandler ChannelConnected;
        protected void ChannelConnectedHandler(System.EventArgs e)
        {
            EventHandler handler = ChannelConnected;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     信道已断开事件
        /// </summary>
        public event EventHandler ChannelDisconnected;
        protected void ChannelDisconnectedHandler(System.EventArgs e)
        {
            EventHandler handler = ChannelDisconnected;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     已拦截未知消息事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<T>> UnknownSpyMessage;
        protected void UnknownSpyMessageHandler(LightSingleArgEventArgs<T> e)
        {
            EventHandler<LightSingleArgEventArgs<T>> handler = UnknownSpyMessage;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        //has data can be receive.
        void RawChannelReceivedData(object sender, LightSingleArgEventArgs<byte[]> e)
        {
            List<T> objs = null;
            #region parse raw data.

            if (_channel.Buffer == null) objs = _protocolStack.Parse(e.Target);
            else
            {
                List<byte[]> data = _channel.Buffer.Add(e.Target);
                if (data != null && data.Count > 0)
                {
                    objs = new List<T>();
                    foreach (var d in data)
                    {
                        List<T> list = _protocolStack.Parse(d);
                        if (list != null && list.Count > 0)
                        {
                            objs.AddRange(list);
                        }
                    }
                }
            }

            #endregion

            if (objs == null) return;
            IMessageSpy<T> spy;
            //dispatch message.
            foreach (T msg in objs)
            {
                //this message cannot be spy.
                if (!_spys.TryGetValue(msg.GetType(), out spy))
                {
                    UnknownSpyMessageHandler(new LightSingleArgEventArgs<T>(msg));
                    continue;
                }
                T rspMessag;
                if ((rspMessag = spy.Spy(msg)) == null) continue;
                //use threadpool to send response msg.
                Task.Factory.StartNew(delegate { SendCallbackAsync(rspMessag); });
            }
        }

        //raw transport channel disconnected.
        void RawChannelDisconnected(object sender, System.EventArgs e)
        {
            _connected = false;
            _channel.Disconnected -= RawChannelDisconnected;
            _channel.ReceivedData -= RawChannelReceivedData;
            _channel.Buffer = null;
            ChannelDisconnectedHandler(null);
        }

        #endregion
    }
}