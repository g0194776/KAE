using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using KJFramework.Cache.Containers;
using KJFramework.Cache.Cores;
using KJFramework.EventArgs;
using KJFramework.Logger;
using KJFramework.Net.Channels;
using KJFramework.ServiceModel.Bussiness.Default.Messages;
using KJFramework.ServiceModel.Bussiness.Default.Metadata;
using KJFramework.ServiceModel.Bussiness.Default.Objects;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.ServiceModel.Comparers;
using KJFramework.ServiceModel.Configurations;
using KJFramework.ServiceModel.Core.Contracts;
using KJFramework.ServiceModel.Core.EventArgs;
using KJFramework.ServiceModel.Core.Helpers;
using KJFramework.ServiceModel.Core.Managers;
using KJFramework.ServiceModel.Core.Objects;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Enums;
using KJFramework.ServiceModel.Exceptions;
using KJFramework.ServiceModel.Identity;
using KJFramework.ServiceModel.Objects;
using KJFramework.ServiceModel.Proxy;
using KJFramework.Tracing;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ServiceModel.Bussiness.Default.Proxy
{
    /// <summary>
    ///     基于CONNECT.技术的客户端代理器，提供了相关的基本操作  
    /// </summary>
    /// <typeparam name="T">服务端契约接口</typeparam>
    public class DefaultClientProxy<T> : IClientProxy<T>
        where T : class
    {
        #region Constructor

        private ProxyStatus _status;

        /// <summary>
        ///     基于CONNECT.技术的客户端代理器，提供了相关的基本操作
        /// </summary>
        /// <param name="binding">绑定方式</param>
        public DefaultClientProxy(Binding binding)
            : this(binding, binding.LogicalAddress)
        { }

        /// <summary>
        ///     基于CONNECT.技术的客户端代理器，提供了相关的基本操作
        /// </summary>
        /// <param name="binding">绑定方式</param>
        /// <param name="logicalAddress">逻辑地址</param>
        public DefaultClientProxy(Binding binding, Uri logicalAddress)
        {
            if (binding == null) throw new ArgumentNullException("binding");
            if (logicalAddress == null) throw new ArgumentNullException("logicalAddress");
            _logicalAddress = logicalAddress;
            _status = ProxyStatus.Unknown;
            _channel = DynamicHelper.Create<T>();
            if (_channel == null) throw new System.Exception("Cannot create dynamic client proxy channel!");
            //hold event.
            IContractDefaultAction action = (IContractDefaultAction) _channel;
            action.Calling += Calling;
            action.AfterCall += AfterCall;
            _contractAction = action;
            _contractAction.Manager = new RequestManager();
            IReconnectionTransportChannel transportChannel = ChannelHelper.Create(logicalAddress, binding);
            if (transportChannel == null) throw new System.Exception("Unsupport binding type!");
            //try to connect remote endpoint.
            transportChannel.Connect();
            if (!transportChannel.IsConnected) throw new System.Exception("Cannot connect to remote endpoint ! #endpoint: " + transportChannel.LogicalAddress);
            GlobalMemory.Initialize();
            ServiceModel.Initialize();
            _msgChannel = new MessageTransportChannel<Message>((IRawTransportChannel)transportChannel, ServiceModel.ProtocolStack);
            _msgChannel.ReceivedMessage += ReceivedMessage;
            _msgChannel.Disconnected += Disconnected;
            _contractAction.LocalEndPoint = _msgChannel.LocalEndPoint;
            if (ServiceModel.FixedRequestMessage == null)
                ServiceModel.FixedRequestMessage = ServiceModel.Tenant.Rent<RequestServiceMessage>("Fixed:RequestServiceMessage", ServiceModelSettingConfigSection.Current.NetworkLayer.RequestServiceMessagePoolCount);
            if (ServiceModel.FixedRequestWaitObject == null)
                ServiceModel.FixedRequestWaitObject = ServiceModel.Tenant.Rent<RequestCenterWaitObject>("Fixed:RequestCenterWaitObject", ServiceModelSettingConfigSection.Current.NetworkLayer.RequestCenterWaitObjectPoolCount);
            _delegators = ServiceModel.Tenant.Rent<TransactionIdentity, AsyncMethodCallback>("Cache:Callbacks, Id: " + Guid.NewGuid(), new TransactionIdentityComparer());
            _delegators.CacheExpired += CacheExpired;
            _status = ProxyStatus.Opend;
        }

        #endregion

        #region Members

        private T _channel;
        private readonly IContractDefaultAction _contractAction;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof (DefaultClientProxy<T>));
        private readonly Uri _logicalAddress;
        private IMessageTransportChannel<Message> _msgChannel;
        private readonly ICacheContainer<TransactionIdentity, AsyncMethodCallback> _delegators;
        private readonly ConcurrentDictionary<TransactionIdentity, RequestCenterWaitObject> _waitObjects = new ConcurrentDictionary<TransactionIdentity, RequestCenterWaitObject>(new TransactionIdentityComparer());

        #endregion

        #region Methods

        /// <summary>
        ///     回调用户函数
        /// </summary>
        /// <param name="rspMessage">响应消息</param>
        /// <param name="isSuccess">成功标示</param>
        /// <param name="lastError">最后错误信息</param>
        private void Callback(ResponseServiceMessage rspMessage, bool isSuccess, System.Exception lastError)
        {
            IReadonlyCacheStub<AsyncMethodCallback> stub = _delegators.Get(rspMessage.TransactionIdentity);
            if (stub == null) return;
            AsyncMethodCallback callback = stub.Cache;
            //能取到回调函数
            if (callback == null) return;
            //failed.
            if (!isSuccess)
            {
                callback(new AsyncCallResult(false, lastError));
                return;
            }
            //successed.
            callback(rspMessage.ServiceReturnValue.HasReturnValue
                         ? new AsyncCallResult(true, true, rspMessage.TransactionIdentity, _contractAction.Manager)
                         : new AsyncCallResult(true));
        }

        #endregion

        #region Implementation of IClientProxy<T>

        /// <summary>
        ///     获取代理器状态
        /// </summary>
        public ProxyStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        ///     获取契约信道
        /// </summary>
        public T Channel
        {
            get { return _channel; }
        }

        /// <summary>
        ///     关闭当前的代理器
        /// </summary>
        public void Close()
        {
            if(_channel != null)
            {
                IContractDefaultAction action = (IContractDefaultAction) _channel;
                action.Calling -= Calling;
                action.AfterCall -= AfterCall;
                _channel = null;
            }
            if(_msgChannel != null)
            {
                _msgChannel.ReceivedMessage -= ReceivedMessage;
                _msgChannel.Disconnected -= Disconnected;
                if (_msgChannel.IsConnected) _msgChannel.Disconnect();
                _msgChannel = null;
            }
            _status = ProxyStatus.Closed;
        }

        /// <summary>
        ///     客户端代理器发生错误事件
        /// </summary>
        public event EventHandler<LightSingleArgEventArgs<System.Exception>> OnError;
        protected void ErrorHandler(LightSingleArgEventArgs<System.Exception> e)
        {
            EventHandler<LightSingleArgEventArgs<System.Exception>> handler = OnError;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Events

        void AfterCall(object sender, AfterCallEventArgs e)
        {
        }

        //requesting.
        void Calling(object sender, ClientLowProxyRequestEventArgs e)
        {
            IFixedCacheStub<RequestServiceMessage> stub = null;
            try
            {
                #region Rent cache & assemble parameters.

                AsyncMethodCallback callback = null;
                stub = ServiceModel.FixedRequestMessage.Rent();
                if (stub == null) throw new System.Exception("Client request message pool has been empty!");
                int paraLength;
                if (e.Arguments == null) paraLength = 0;
                else if (e.IsAsync) paraLength = e.Arguments.Length - 1;
                else paraLength = e.Arguments.Length;
                RequestServiceMessage message = stub.Cache;
                message.TransactionIdentity = e.Identity;
                message.TransactionIdentity.IsOneway = message.IsOneway = !e.NeedAck;
                message.LogicalRequestAddress = _logicalAddress.GetServiceUri();
                message.RequestObject = new RequestMethodObject(e.Arguments == null ? 0 : paraLength);
                message.RequestObject.MethodToken = e.MethodToken;
                message.IsAsync = e.IsAsync;
                //一个例外情况就是 void CallAsync(); 这个时候，这个方法也许没有回调函数的传入，但也算是APM的
                if (e.IsAsync && e.NeedAck && e.HasCallback)
                {
                    //regist callback func.
                    callback = (AsyncMethodCallback)e.Arguments[e.Arguments.Length - 1];
                    //default timeout: 1min.
                    if (_delegators.Add(e.Identity, callback, new TimeSpan(0, 1, 0)) == null)
                        Logs.Logger.Log("#Cannot add callback func for current request. #session id: " + e.Identity);
                }
                //打入请求参数, index 0 == Session Id， index 1 == Method Full name
                if (e.Arguments != null)
                {
                    for (int i = 0; i < paraLength; i++)
                    {
                        object argument = e.Arguments[i];
                        BinaryArgContext context;
                        context = argument == null ? BinaryArgContext.CreateNullContext(i) : new BinaryArgContext(i, argument.GetType(), argument);
                        message.RequestObject.AddArg(context);
                    }
                }
                if (!_msgChannel.IsConnected) throw new System.Exception("Current remote endpoint channel has been disconnected.");

                #endregion

                #region Create transaction.

                RPCTransaction transaction = e.NeedAck
                                                 ? Global.TransactionManager.CreateTransaction(e.Identity, _msgChannel)
                                                 : new RPCTransaction(e.Identity, _msgChannel);
                transaction.Fail += delegate
                {
                    CannotReachableException exception = new CannotReachableException("Cannot reach remote service point.");
                    if (e.IsAsync && e.HasCallback) callback(new AsyncCallResult(false, exception));
                    else throw exception;
                };
                transaction.Timeout += delegate
                {
                    NonresponseException exception = new NonresponseException("Oops, your request cannot get expect response at specify time!");
                    if (e.IsAsync && e.HasCallback) callback(new AsyncCallResult(false, exception));
                    else throw exception;
                };
                transaction.ResponseRecv += delegate(object s, LightSingleArgEventArgs<Message> ee)
                {
                    ResponseServiceMessage rspMessage = (ResponseServiceMessage)ee.Target;

                    #region Callee exception.

                    //callee exception.
                    if (rspMessage.ServiceReturnValue.ProcessResult == ServiceProcessResult.Exception)
                    {
                        System.Exception exception = rspMessage.ServiceReturnValue.CreateException();
                        //error notification.
                        ErrorHandler(new LightSingleArgEventArgs<System.Exception>(exception));
                        if (e.IsAsync) Callback(rspMessage, false, exception);
                        else
                        {
                            _contractAction.Manager.AddResult(rspMessage.TransactionIdentity, BinaryArgContext.CreateExceptionContext(exception));
                            //sync
                            RequestCenterWaitObject waitObject;
                            if (_waitObjects.TryRemove(rspMessage.TransactionIdentity, out waitObject))
                                waitObject.ResetEvent.Set();
                        }
                        return;
                    }

                    #endregion

                    #region Active sync block or callback.

                    //add result.
                    _contractAction.Manager.AddResult(rspMessage.TransactionIdentity, rspMessage.ServiceReturnValue.ReturnValue);
                    //sync
                    if (!e.IsAsync)
                    {
                        RequestCenterWaitObject waitObject;
                        if (_waitObjects.TryRemove(message.TransactionIdentity, out waitObject)) waitObject.ResetEvent.Set();
                    }
                    //async
                    else Callback(rspMessage, true, null);

                    #endregion
                };

                #endregion

                #region Sync && Signal

                if (e.IsAsync || !e.NeedAck)
                {
                    transaction.SendRequest(message);
                    return;
                }
                //sync wait.
                #region Initialize wait object.

                //rent a cache stub.
                IFixedCacheStub<RequestCenterWaitObject> wstub = ServiceModel.FixedRequestWaitObject.Rent();
                if (wstub == null) throw new System.Exception("Request center wait object pool has been empty!");
                RequestCenterWaitObject requestCenterWaitObject = wstub.Cache;
                requestCenterWaitObject.Key = e.Identity;
                requestCenterWaitObject.ResetEvent = new AutoResetEvent(false);
                requestCenterWaitObject.Time = DateTime.Now;
                _waitObjects.TryAdd(e.Identity, requestCenterWaitObject);

                #endregion

                #region Reset signal.

                try
                {
                    transaction.SendRequest(message);
                    //没有收到实例信号，则为超时.
                    if (!requestCenterWaitObject.ResetEvent.WaitOne(60000))
                    {
                        RequestCenterWaitObject waitObj;
                        _waitObjects.TryRemove(e.Identity, out waitObj);
                        throw new RequestTimeoutException();
                    }
                }
                catch (System.Exception ex)
                {
                    Logs.Logger.Log(ex);
                    throw;
                }
                finally
                {
                    ServiceModel.FixedRequestWaitObject.Giveback(wstub);
                }

                #endregion

                #endregion
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
            finally { if (stub != null) ServiceModel.FixedRequestMessage.Giveback(stub); }
        }

        //recv messages from remote endpoint.
        void ReceivedMessage(object sender, LightSingleArgEventArgs<List<Message>> e)
        {
            //current proxy has been closed.
            if (_contractAction == null) return;
            foreach (Message message in e.Target)
            {
                RPCTransaction transaction = Global.TransactionManager.GetRemove(message.TransactionIdentity);
                if(transaction == null)
                {
                    _tracing.Error("#Cannot get transaction with key. #tid: " + message.TransactionIdentity);
                    return;
                }
                transaction.SetResponse(message);
            }
        }

        //cache expired.
        void CacheExpired(object sender, Cache.EventArgs.ExpiredCacheEventArgs<TransactionIdentity, AsyncMethodCallback> e)
        {
        }

        //low transport channel disconnected.
        void Disconnected(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion
    }
}