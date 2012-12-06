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
    ///     宿主服务父类，提供了相关的基本操作。
    /// </summary>
    internal class HostService : IHostService
    {
        #region Constructor

        /// <summary>
        ///     宿主服务父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="service">服务</param>
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
        ///     初始化
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
                    //找到一个接口
                    if (_contract != null)
                    {
                        _methods = ServiceHelper.GetServiceMethods(interfaceType).ToList();
                        break;
                    }
                }
                if (_contract == null) throw new System.Exception("Invalid service contract !");
            }
            //当开放服务的模式 ！= 单实例时，初始化对象池
            if (_contract.ServiceConcurrentType != ServiceConcurrentTypes.Singleton)
            {
                _objectPool = new ConcurrentStack<object>();
                Parallel.For(0, ServiceModelSettingConfigSection.Current.NetworkLayer.ServiceProviderObjectPoolCount,
                delegate(int index)
                {
                    _objectPool.Push(Activator.CreateInstance(_service));
                });
            }
            //增加方法序列标记缓存
            foreach (ServiceMethodPickupObject serviceMethodPickupObject in _methods)
            {
                _cacheMethods.Add(serviceMethodPickupObject.Method.CoreMethodInfo.MetadataToken, serviceMethodPickupObject);
            }
        }

        #endregion

        #region Implementation of IHostService

        /// <summary>
        ///     获取内部服务类型
        /// </summary>
        /// <returns>返回服务类型</returns>
        public Type GetServiceType()
        {
            return _service;
        }

        /// <summary>
        ///     获取元数据契约名称
        /// </summary>
        /// <returns>返回契约开放的名称</returns>
        public string GetContractName()
        {
            ServiceMetadataExchangeAttribute attribute = AttributeHelper.GetCustomerAttribute<ServiceMetadataExchangeAttribute>(_service);
            return attribute == null ? _service.FullName : attribute.ContractName;
        }

        /// <summary>
        ///     创建一个新的内部服务实例
        /// </summary>
        /// <returns>返回新的内部服务实例</returns>
        public Object NewServiceInstance()
        {
            Object obj;
            return _objectPool.TryPop(out obj) ? obj : null;
        }

        /// <summary>
        ///     还回一个内部服务实例
        /// </summary>
        /// <param name="obj">内部服务实例</param>
        public void Giveback(Object obj)
        {
            if (obj != null) _objectPool.Push(obj);
        }

        /// <summary>
        ///     获取所有服务方法
        /// </summary>
        /// <returns></returns>
        public virtual ServiceMethodPickupObject[] GetMethods()
        {
            if (_methods != null) return _methods.ToArray();
            return null;
        }

        /// <summary>
        ///     获取具有指定名称和参数个数的服务方法
        /// </summary>
        /// <param name="methodToken">方法序列标记</param>
        /// <returns>返回方法</returns>
        public virtual ServiceMethodPickupObject GetMethod(int methodToken)
        {
            ServiceMethodPickupObject serviceMethodPickupObject;
            return _cacheMethods.TryGetValue(methodToken, out serviceMethodPickupObject) ? serviceMethodPickupObject : null;
        }

        /// <summary>
        ///     获取服务对象
        ///     <para>* 此方法用于获取单一实例的服务对象。</para>
        /// </summary>
        /// <returns>返回服务对象</returns>
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
        ///     获取唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        protected ServiceContractAttribute _contract;
        /// <summary>
        ///     获取服务契约属性
        /// </summary>
        public ServiceContractAttribute Contract
        {
            get { return _contract; }
        }

        #endregion
    }
}