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
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    internal class ServiceHandle : IServiceHandle
    {
        #region Constructor
        
        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="binding">������</param>
        /// <param name="service">�ڲ�����</param>
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
        ///     ��ȡ�󶨶���
        /// </summary>
        public Binding[] Bindings
        {
            get { return _binding; }
        }

        /// <summary>
        ///     ��ȡ�����õ�ַURL
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        /// <summary>
        ///     ��ȡ��������
        /// </summary>
        public HostService GetService()
        {
            return _service;
        }

        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ŀ�����
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
        }

        /// <summary>
        ///     ��������
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
        ///     �رշ���
        /// </summary>
        public void Close()
        {
            _enable = false;
            _isClosed = true;
            ServiceCenter.UnRegist(_uri.ToString());
            ClosedHandler(null);
        }

        /// <summary>
        ///     �رշ��񣬲��رյ�ǰ���������ŵ�����ϵͳ��Դ
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
        ///     ��ʼ��
        /// </summary>
        public void Initialize()
        {
            try
            {
                foreach (Binding binding in _binding)
                {
                    //��ʼƥ���ڲ�ͨ��
                    HostTransportChannel hostTransportChannel = ChannelHelper.Create(binding);
                    if (hostTransportChannel == null) throw new System.Exception("can not creat host channel by binding.");
                    if (!NetworkLayer.Instance.Alloc(hostTransportChannel)) throw new System.Exception("�޷�������������һ������ͨ��!");
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
        ///     �Ѿ������¼�
        /// </summary>
        public event EventHandler Opened;
        protected void OpenedHandler(System.EventArgs e)
        {
            EventHandler handler = Opened;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///     �ر��¼�
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
        ///     ָ����Ϣ�����������
        /// </summary>
        /// <param name="message">������Ϣ</param>
        /// <param name="channel">ͨ���ŵ�</param>
        internal void PostRequest(RPCTransaction rpcTrans)
        {
            _dispatcher.Dispatch(this, rpcTrans);
        }

        #endregion

        #region Implementation of IMetadataExchange

        /// <summary>
        ///     ����һ�����ڵ�ǰ����ڵ����Լ����
        /// </summary>
        /// <returns>������Լ����</returns>
        public IContractDescription CreateDescription()
        {
            if (!_isSupportExchange) 
                throw new System.Exception("��ǰ������Լ #" + _service.GetServiceType().FullName + "��֧����ԼԪ���ݽ����������������Լ������������ServiceHandle.IsSupportExchange = true.");
            if (_description != null) return _description;
            ContractDescription contractDescription = new ContractDescription();
            //������������������
            foreach (ServiceMethodPickupObject serviceMethodPickupObject in _service.GetMethods())
                contractDescription.Add(serviceMethodPickupObject.Method);
            //������Լ��Ϣ
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
        ///      ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ����ڵ��Ƿ�֧����ԼԪ���ݽ���
        /// </summary>
        public bool IsSupportExchange
        {
            get { return _isSupportExchange; }
            set { _isSupportExchange = value; }
        }

        #endregion
    }
}