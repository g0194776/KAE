using KJFramework.Net.Channels.HostChannels;
using KJFramework.ServiceModel.Bussiness.Default.Centers;
using KJFramework.ServiceModel.Bussiness.Default.Descriptions;
using KJFramework.ServiceModel.Bussiness.Default.Dispatchers.Cores;
using KJFramework.ServiceModel.Bussiness.Default.Transactions;
using KJFramework.ServiceModel.Core.Helpers;
using KJFramework.ServiceModel.Core.Objects;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Enums;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;
using Uri = KJFramework.Net.Channels.Uri.Uri;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     服务句柄父类，提供了相关的基本操作。
    /// </summary>
    internal class ServiceHandle : IServiceHandle
    {
        #region Constructor
        
        /// <summary>
        ///     服务句柄父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="binding">绑定类型</param>
        /// <param name="service">内部服务</param>
        internal ServiceHandle(Type service, params Binding[] binding)
        {
            _binding = binding;
            if (_binding == null) throw new ArgumentNullException("binding");
            if (service == null) throw new ArgumentNullException("service");
            _uri = binding.Length == 1 ? binding[0].LogicalAddress : null;
            _service = new HostService(service);
            #region Get dispatcher.

            switch (_service.Contract.ServiceConcurrentType)
            {
                case ServiceConcurrentTypes.Concurrent:
                    _dispatcher = new ConcurrentCoreDispatcher();
                    break;
                case ServiceConcurrentTypes.Singleton:
                    _dispatcher = new SingletonCoreDispatcher();
                    break;
            }

            #endregion
        }

        #endregion

        #region Members

        protected Uri _uri;
        protected bool _enable;
        protected bool _isClosed;
        protected readonly HostService _service;
        protected IContractDescription _description;
        protected readonly ICoreDispatcher _dispatcher;
        protected List<IHostTransportChannel> _hostChannels = new List<IHostTransportChannel>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ServiceHandle));

        #endregion

        #region Implementation of IServiceHandle

        protected readonly Binding[] _binding;
        protected bool _isSupportExchange;

        /// <summary>
        ///     获取绑定对象
        /// </summary>
        public Binding[] Bindings
        {
            get { return _binding; }
        }

        /// <summary>
        ///     获取或设置地址URL
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        /// <summary>
        ///     获取宿主服务
        /// </summary>
        public HostService GetService()
        {
            return _service;
        }

        /// <summary>
        ///     获取一个值，该值表示了当前服务句柄的可用性
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
        }

        /// <summary>
        ///     开启服务
        /// </summary>
        public void Open()
        {
            if (_isClosed) throw new System.Exception("this service handle has been closed.");
            try
            {
                Initialize();
                if (_service == null) throw new System.Exception("can not open host service, because the inner service is null.");
                _enable = true;
                ServiceCenter.Regist(this);
                OpenedHandler(null);
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     关闭服务
        /// </summary>
        public void Close()
        {
            _enable = false;
            _isClosed = true;
            ServiceCenter.UnRegist(_uri.ToString());
            ClosedHandler(null);
        }

        /// <summary>
        ///     关闭服务，并关闭当前服务所开放的所有系统资源
        /// </summary>
        public void Shutdown()
        {
            Close();
            if (_hostChannels == null) return;
            foreach (IHostTransportChannel hostTransportChannel in _hostChannels)
            {
                if (hostTransportChannel is TcpHostTransportChannel)
                {
                    NetworkLayer.Instance.RemoveTcpHostChannel(((TcpHostTransportChannel)hostTransportChannel).Port);
                }
                else if (hostTransportChannel is PipeHostTransportChannel)
                {
                    NetworkLayer.Instance.RemoveIpcHostChannel(((PipeHostTransportChannel)hostTransportChannel).LogicalAddress.ToString());
                }
            }
            _hostChannels.Clear();
        }

        /// <summary>
        ///     初始化
        /// </summary>
        public void Initialize()
        {
            try
            {
                foreach (Binding binding in _binding)
                {
                    //开始匹配内部通道
                    HostTransportChannel hostTransportChannel = ChannelHelper.Create(binding);
                    if (hostTransportChannel == null) throw new System.Exception("can not creat host channel by binding.");
                    if (!NetworkLayer.Instance.Alloc(hostTransportChannel)) throw new System.Exception("无法按照流程申请一个宿主通道!");
                    if (!hostTransportChannel.Regist()) throw new System.Exception("Cannot regist a host channel, type: " + hostTransportChannel);
                    _hostChannels.Add(hostTransportChannel);
                }
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
            }
        }

        /// <summary>
        ///     已经开启事件
        /// </summary>
        public event EventHandler Opened;
        protected void OpenedHandler(System.EventArgs e)
        {
            EventHandler handler = Opened;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     关闭事件
        /// </summary>
        public event EventHandler Closed;
        protected void ClosedHandler(System.EventArgs e)
        {
            EventHandler handler = Closed;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     指派消息请求后的新入口
        /// </summary>
        /// <param name="message">请求消息</param>
        /// <param name="channel">通信信道</param>
        internal void PostRequest(RPCTransaction rpcTrans)
        {
            _dispatcher.Dispatch(this, rpcTrans);
        }

        #endregion

        #region Implementation of IMetadataExchange

        /// <summary>
        ///     创建一个关于当前服务节点的契约描述
        /// </summary>
        /// <returns>返回契约描述</returns>
        public IContractDescription CreateDescription()
        {
            if (!_isSupportExchange) 
                throw new System.Exception("当前服务契约 #" + _service.GetServiceType().FullName + "不支持契约元数据交换，如果想生成契约描述，请设置ServiceHandle.IsSupportExchange = true.");
            if (_description != null) return _description;
            ContractDescription contractDescription = new ContractDescription();
            //创建操作方法的描述
            foreach (ServiceMethodPickupObject serviceMethodPickupObject in _service.GetMethods())
                contractDescription.Add(serviceMethodPickupObject.Method);
            //创建契约信息
            contractDescription.Infomation = new ContractInfomation
            {
                OpenTime = _service.CreateTime,
                Name = _service.Contract.Name,
                Description = _service.Contract.Description,
                Version = _service.Contract.Version,
                ContractName = _service.GetContractName(),
                FullName = _service.GetServiceType().Name
            };
            _description = contractDescription;
            return _description;
        }

        /// <summary>
        ///      获取或设置一个值，该值表示了当前服务节点是否支持契约元数据交换
        /// </summary>
        public bool IsSupportExchange
        {
            get { return _isSupportExchange; }
            set { _isSupportExchange = value; }
        }

        #endregion
    }
}