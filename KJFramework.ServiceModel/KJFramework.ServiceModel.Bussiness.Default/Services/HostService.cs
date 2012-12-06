using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KJFramework.Helpers;
using KJFramework.ServiceModel.Configurations;
using KJFramework.ServiceModel.Core.Attributes;
using KJFramework.ServiceModel.Core.Helpers;
using KJFramework.ServiceModel.Core.Objects;
using KJFramework.ServiceModel.Enums;

namespace KJFramework.ServiceModel.Bussiness.Default.Services
{
    /// <summary>
    ///     ���������࣬�ṩ����صĻ���������
    /// </summary>
    internal class HostService : IHostService
    {
        #region Constructor

        /// <summary>
        ///     ���������࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="service">����</param>
        internal HostService(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            _id = Guid.NewGuid();
            _createTime = DateTime.Now;
            _service = service;
            Initialize();
        }

        #endregion

        #region Members

        protected readonly Guid _id;
        protected readonly DateTime _createTime;
        protected readonly Type _service;
        protected List<ServiceMethodPickupObject> _methods;
        protected Object _singletonInstance;
        protected Object _singletonInstanceObj = new Object();
        protected readonly Dictionary<int, ServiceMethodPickupObject> _cacheMethods = new Dictionary<int, ServiceMethodPickupObject>();
        private ConcurrentStack<object> _objectPool;

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        protected void Initialize()
        {
            _contract = AttributeHelper.GetCustomerAttribute<ServiceContractAttribute>(_service);
            if (_contract != null) _methods = ServiceHelper.GetServiceMethods(_service).ToList();
            else
            {
                Type[] interfaceTypes = _service.GetInterfaces();
                if (interfaceTypes == null || interfaceTypes.Length == 0) throw new System.Exception("Invalid service contract !");
                foreach (Type interfaceType in interfaceTypes)
                {
                    _contract = AttributeHelper.GetCustomerAttribute<ServiceContractAttribute>(interfaceType);
                    //�ҵ�һ���ӿ�
                    if (_contract != null)
                    {
                        _methods = ServiceHelper.GetServiceMethods(interfaceType).ToList();
                        break;
                    }
                }
                if (_contract == null) throw new System.Exception("Invalid service contract !");
            }
            //�����ŷ����ģʽ ��= ��ʵ��ʱ����ʼ�������
            if (_contract.ServiceConcurrentType != ServiceConcurrentTypes.Singleton)
            {
                _objectPool = new ConcurrentStack<object>();
                Parallel.For(0, ServiceModelSettingConfigSection.Current.NetworkLayer.ServiceProviderObjectPoolCount,
                delegate(int index)
                {
                    _objectPool.Push(Activator.CreateInstance(_service));
                });
            }
            //���ӷ������б�ǻ���
            foreach (ServiceMethodPickupObject serviceMethodPickupObject in _methods)
            {
                _cacheMethods.Add(serviceMethodPickupObject.Method.CoreMethodInfo.MetadataToken, serviceMethodPickupObject);
            }
        }

        #endregion

        #region Implementation of IHostService

        /// <summary>
        ///     ��ȡ�ڲ���������
        /// </summary>
        /// <returns>���ط�������</returns>
        public Type GetServiceType()
        {
            return _service;
        }

        /// <summary>
        ///     ��ȡԪ������Լ����
        /// </summary>
        /// <returns>������Լ���ŵ�����</returns>
        public string GetContractName()
        {
            ServiceMetadataExchangeAttribute attribute = AttributeHelper.GetCustomerAttribute<ServiceMetadataExchangeAttribute>(_service);
            return attribute == null ? _service.FullName : attribute.ContractName;
        }

        /// <summary>
        ///     ����һ���µ��ڲ�����ʵ��
        /// </summary>
        /// <returns>�����µ��ڲ�����ʵ��</returns>
        public Object NewServiceInstance()
        {
            Object obj;
            return _objectPool.TryPop(out obj) ? obj : null;
        }

        /// <summary>
        ///     ����һ���ڲ�����ʵ��
        /// </summary>
        /// <param name="obj">�ڲ�����ʵ��</param>
        public void Giveback(Object obj)
        {
            if (obj != null) _objectPool.Push(obj);
        }

        /// <summary>
        ///     ��ȡ���з��񷽷�
        /// </summary>
        /// <returns></returns>
        public virtual ServiceMethodPickupObject[] GetMethods()
        {
            if (_methods != null) return _methods.ToArray();
            return null;
        }

        /// <summary>
        ///     ��ȡ����ָ�����ƺͲ��������ķ��񷽷�
        /// </summary>
        /// <param name="methodToken">�������б��</param>
        /// <returns>���ط���</returns>
        public virtual ServiceMethodPickupObject GetMethod(int methodToken)
        {
            ServiceMethodPickupObject serviceMethodPickupObject;
            return _cacheMethods.TryGetValue(methodToken, out serviceMethodPickupObject) ? serviceMethodPickupObject : null;
        }

        /// <summary>
        ///     ��ȡ�������
        ///     <para>* �˷������ڻ�ȡ��һʵ���ķ������</para>
        /// </summary>
        /// <returns>���ط������</returns>
        public virtual Object GetServiceObject()
        {
            if (_singletonInstance == null)
            {
                lock (_singletonInstanceObj)
                {
                    _singletonInstance = _service.Assembly.CreateInstance(_service.FullName);
                }
            }
            return _singletonInstance;
        }

        /// <summary>
        ///     ��ȡΨһ���
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     ��ȡ����ʱ��
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        protected ServiceContractAttribute _contract;
        /// <summary>
        ///     ��ȡ������Լ����
        /// </summary>
        public ServiceContractAttribute Contract
        {
            get { return _contract; }
        }

        #endregion
    }
}