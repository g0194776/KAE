using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using KJFramework.Buffers;
using KJFramework.Enums;
using KJFramework.EventArgs;
using KJFramework.Net.Buffers;
using KJFramework.Net.Channel;
using KJFramework.Net.Enums;
using KJFramework.Net.Events;
using KJFramework.Net.Identities;
using KJFramework.Net.Managers;
using KJFramework.Net.Parsers;
using KJFramework.Net.ProtocolStacks;
using KJFramework.Statistics;

namespace KJFramework.Net
{
    /// <summary>
    ///     消息传输信道，提供了相关的基本操作
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    public class MessageTransportChannel<T> : IMessageTransportChannel<T>
    {
        #region Constructor
        
        /// <summary>
        ///     消息传输信道
        /// </summary>
        /// <param name="rawChannel">数据流信道</param>
        /// <param name="protocolStack">协议栈</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransportChannel(IRawTransportChannel rawChannel, IProtocolStack protocolStack)
            : this(rawChannel, protocolStack, new SegmentDataParser<T>(protocolStack))
        {

        }

        /// <summary>
        ///     消息传输信道 
        /// </summary>
        /// <param name="rawChannel">数据流信道</param>
        /// <param name="protocolStack">协议栈</param>
        /// <param name="parser">解析器</param>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public MessageTransportChannel(IRawTransportChannel rawChannel, IProtocolStack protocolStack, ISegmentDataParser<T> parser)
        {
            if (rawChannel == null) throw new ArgumentNullException("rawChannel");
            if (protocolStack == null) throw new ArgumentNullException("protocolStack");
            if (rawChannel.SupportSegment && parser == null) throw new ArgumentNullException("parser");
            _rawChannel = rawChannel;
            _channelType = _rawChannel.ChannelType;
            //initialize segment data parser, if current channel support segment data recv.
            if (_rawChannel.SupportSegment)
            {
                _parser = parser;
                _parser.ParseSucceed += SegmentParseSucceed;
                _rawChannel.ReceivedDataSegment += ReceivedDataSegment;
            }
            else
            {
                _rawChannel.ReceivedData += RawReceivedData;
                if (_rawChannel.Buffer == null) _rawChannel.Buffer = new ReceiveBuffer(ChannelConst.RecvBufferSize);
            }
            _protocolStack = protocolStack;
            _rawChannel.Opened += Opened;
            _rawChannel.Opening += Opening;
            _rawChannel.Closed += Closed;
            _rawChannel.Closing += Closing;
            _rawChannel.Faulted += Faulted;
            _rawChannel.Disconnected += RawChannelDisconnected;
            _rawChannel.Connected += Connected;
            _key = _rawChannel.Key;
            _localIep = _rawChannel.LocalEndPoint;
            _remoteIep = _rawChannel.RemoteEndPoint;
            //open this channel at last.
            _rawChannel.Open();
        }

        #endregion

        #region Members

        private readonly Guid _key;
        private IRawTransportChannel _rawChannel;
        private readonly IProtocolStack _protocolStack;
        private IMultiPacketManager<T> _multiPacketManager;
        private EndPoint _localIep;
        private EndPoint _remoteIep;
        private ISegmentDataParser<T> _parser;
        private readonly TransportChannelTypes _channelType;

        /// <summary>
        ///     获取或设置附属标记
        /// </summary>
        public object Tag { get; set; }

        #endregion

        #region Implementation of IChannel<BasicChannelInfomation>

        /// <summary>
        /// 获取或设置当前通道信息
        /// </summary>
        public BasicChannelInfomation ChannelInfo
        {
            get { return _rawChannel.ChannelInfo; }
            set { _rawChannel.ChannelInfo = value; }
        }

        #endregion

        #region Implementation of IStatisticable<IStatistic>

        /// <summary>
        /// 获取或设置统计器
        /// </summary>
        public Dictionary<StatisticTypes, IStatistic> Statistics
        {
            get { return _rawChannel.Statistics; }
            set { _rawChannel.Statistics = value; }
        }

        #endregion

        #region Implementation of ICommunicationObject

        /// <summary>
        ///     停止
        /// </summary>
        public void Abort()
        {
            _rawChannel.Abort();
        }

        /// <summary>
        ///     打开
        /// </summary>
        public void Open()
        {
            _rawChannel.Open();
        }

        /// <summary>
        ///     关闭
        /// </summary>
        public void Close()
        {
            if (_rawChannel != null)
            {
                _rawChannel.Close();
                _rawChannel = null;
            }
            if (_multiPacketManager != null)
            {
                _multiPacketManager.Dispose();
                _multiPacketManager = null;
            }
        }

        /// <summary>
        ///     异步打开
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            return _rawChannel.BeginOpen(callback, state);
        }

        /// <summary>
        ///     异步关闭
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="state">状态</param>
        /// <returns>返回异步结果</returns>
        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            if (_multiPacketManager != null)
            {
                _multiPacketManager.Dispose();
                _multiPacketManager = null;
            }
            return _rawChannel.BeginClose(callback, state);
        }

        /// <summary>
        ///     异步打开
        /// </summary>
        public void EndOpen(IAsyncResult result)
        {
            _rawChannel.EndOpen(result);
        }

        /// <summary>
        ///     异步关闭
        /// </summary>
        public void EndClose(IAsyncResult result)
        {
            _rawChannel.EndClose(result);
        }

        /// <summary>
        ///     获取或设置当前可用状态
        /// </summary>
        public bool Enable
        {
            get { return _rawChannel.Enable; }
            set { _rawChannel.Enable = value; }
        }

        /// <summary>
        ///     获取当前通讯状态
        /// </summary>
        public CommunicationStates CommunicationState
        {
            get { return _rawChannel.CommunicationState; }
        }

        public event EventHandler Closed;
        public event EventHandler Closing;
        public event EventHandler Faulted;
        public event EventHandler Opened;
        public event EventHandler Opening;

        #endregion

        #region Implementation of IServiceChannel

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _rawChannel.CreateTime; }
        }

        /// <summary>
        ///     获取通道唯一标示
        /// </summary>
        public Guid Key
        {
            get { return _key; }
        }

        #endregion

        #region Implementation of ICommunicationChannelAddress

        /// <summary>
        ///     获取或设置物理地址
        /// </summary>
        public IPEndPoint Address
        {
            get { return _rawChannel.Address; }
            set { _rawChannel.Address = value; }
        }

        /// <summary>
        ///     获取或设置逻辑地址
        /// </summary>
        public Uri.Uri LogicalAddress
        {
            get { return _rawChannel.LogicalAddress; }
            set { _rawChannel.LogicalAddress = value; }
        }

        #endregion

        #region Implementation of ITransportChannel

        /// <summary>
        ///   获取通信信道的类型
        /// </summary>
        public TransportChannelTypes ChannelType
        {
            get { return _channelType; }
        }

        /// <summary>
        ///     获取本地终结点地址
        /// </summary>
        public EndPoint LocalEndPoint
        {
            get { return _localIep; }
        }

        /// <summary>
        ///     获取远程终结点地址
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return _remoteIep; }
        }

        /// <summary>
        ///   获取或设置缓冲区
        /// </summary>
        public IByteArrayBuffer Buffer
        {
            get { return _rawChannel.Buffer; }
            set { _rawChannel.Buffer = value; }
        }

        /// <summary>
        ///     获取或设置延迟设置
        /// </summary>
        public LingerOption LingerState
        {
            get { return _rawChannel.LingerState; }
            set { _rawChannel.LingerState = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前通道是否处于连接状态
        /// </summary>
        public bool IsConnected
        {
            get { return _rawChannel == null ? false : _rawChannel.IsConnected; }
        }

        /// <summary>
        ///     连接
        /// </summary>
        public void Connect()
        {
            _rawChannel.Connect();
        }

        /// <summary>
        ///     断开
        /// </summary>
        public void Disconnect()
        {
            if (_rawChannel != null) _rawChannel.Disconnect();
            if (_multiPacketManager != null)
            {
                _multiPacketManager.Dispose();
                _multiPacketManager = null;
            }
        }

        /// <summary>
        ///     发送数据
        /// </summary>
        /// <param name="data">要发送的数据</param>
        /// <returns>返回发送的字节数</returns>
        /// <exception cref="ArgumentNullException">参数错误</exception>
        public int Send(byte[] data)
        {
            return _rawChannel.Send(data);
        }

        /// <summary>
        ///     通道已连接事件
        /// </summary>
        public event EventHandler Connected;
        /// <summary>
        ///     通道已断开事件
        /// </summary>
        public event EventHandler Disconnected;
        protected void DisconnectedHandler(System.EventArgs e)
        {
            EventHandler disconnected = Disconnected;
            if (disconnected != null) disconnected(this, e);
        }

        #endregion

        #region Implementation of IMessageTransportChannel<T>

        /// <summary>
        ///     获取协议栈
        /// </summary>
        public IProtocolStack ProtocolStack
        {
            get { return _protocolStack; }
        }

        /// <summary>
        ///     获取或设置封包片消息管理器
        /// </summary>
        public IMultiPacketManager<T> MultiPacketManager
        {
            get { return _multiPacketManager; }
            set { _multiPacketManager = value; }
        }

        /// <summary>
        ///     发送一个消息
        /// </summary>
        /// <param name="obj">要发送的消息</param>
        /// <returns>返回发送的字节数</returns>
        public int Send(T obj)
        {
            byte[] data = _protocolStack.ConvertToBytes(obj);
            if (data == null) return 0;
            if (data.Length <= ChannelConst.MaxMessageDataLength) return Send(data);
            //need multi messages transport.
            List<byte[]> multiDatas = _protocolStack.ConvertMultiMessage(obj, ChannelConst.MaxMessageDataLength);
            if (multiDatas == null) return 0;
            int sendCount = 0;
            foreach (byte[] multiData in multiDatas)
            {
                sendCount += Send(multiData);
            }
            return sendCount;
        }

        /// <summary>
        ///     接收到消息事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<List<T>>> ReceivedMessage;

        /// <summary>
        ///   生成一个请求的事务唯一标示
        /// </summary>
        /// <param name="messageId">消息编号</param>
        /// <returns>返回创建后的事务唯一标示</returns>
        public TransactionIdentity GenerateRequestIdentity(uint messageId)
        {
            if (_rawChannel == null) throw new NotImplementedException("#You cannot generate TransactionIdentity from a un-initialized MessageChannel.");
            if (_rawChannel.ChannelType == TransportChannelTypes.TCP) return new TCPTransactionIdentity { EndPoint = _rawChannel.RemoteEndPoint, IsRequest = true, MessageId = messageId };
            throw new NotSupportedException("#Not supported current type of Channel. " + _rawChannel.ChannelType);
        }

        protected void ReceivedMessageHandler(LightSingleArgEventArgs<List<T>> e)
        {
            EventHandler<LightSingleArgEventArgs<List<T>>> handler = ReceivedMessage;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        void SegmentParseSucceed(object sender, LightSingleArgEventArgs<List<T>> e)
        {
            ReceivedMessageHandler(new LightSingleArgEventArgs<List<T>>(e.Target));
        }

        //new data received event.
        void RawReceivedData(object sender, LightSingleArgEventArgs<byte[]> e)
        {
            List<T> objs = null;
            #region parse raw data.

            if (_rawChannel.Buffer == null)
            {
                objs = _protocolStack.Parse<T>(e.Target);
            }
            else
            {
                List<byte[]> data = _rawChannel.Buffer.Add(e.Target);
                if (data != null && data.Count > 0)
                {
                    objs = new List<T>();
                    foreach (var d in data)
                    {
                        List<T> list = _protocolStack.Parse<T>(d);
                        if (list != null && list.Count > 0)
                        {
                            objs.AddRange(list);
                        }
                    }
                }
            }

            #endregion
            if (objs != null)
            {
                ReceivedMessageHandler(new LightSingleArgEventArgs<List<T>>(objs));
            }
        }

        //raw channel disconnected.
        void RawChannelDisconnected(object sender, System.EventArgs e)
        {
            if (_rawChannel != null)
            {
                _rawChannel.Buffer = null;
                _rawChannel.ReceivedData -= RawReceivedData;
                _rawChannel.Opened -= Opened;
                _rawChannel.Opening -= Opening;
                _rawChannel.Closed -= Closed;
                _rawChannel.Closing -= Closing;
                _rawChannel.Faulted -= Faulted;
                _rawChannel.Disconnected -= Disconnected;
                _rawChannel.Connected -= Connected;
                _rawChannel.ReceivedDataSegment -= ReceivedDataSegment;
                _rawChannel.Dispose();
                _rawChannel = null;
            }
            if (_multiPacketManager != null)
            {
                _multiPacketManager.Dispose();
                _multiPacketManager = null;
            }
            if (_parser != null)
            {
                _parser.ParseSucceed -= SegmentParseSucceed;
                _parser.Dispose();
                _parser = null;
            }
            DisconnectedHandler(null);
        }

        void ReceivedDataSegment(object sender, SegmentReceiveEventArgs e)
        {
            _parser.Append(e);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        #endregion
    }
}