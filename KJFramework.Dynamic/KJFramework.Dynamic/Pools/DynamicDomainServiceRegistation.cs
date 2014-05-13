using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;

namespace KJFramework.Dynamic.Pools
{
    /// <summary>
    ///     动态程序域服务注册站，提供了相关的基本操作。
    /// </summary>
    public class DynamicDomainServiceRegistation
    {
        #region Constructor

        /// <summary>
        ///     动态程序域服务注册站，提供了相关的基本操作。
        /// </summary>
        private DynamicDomainServiceRegistation()
        {

        }

        #endregion

        #region Members

        /// <summary>
        ///     动态程序域服务注册站，提供了相关的基本操作。
        /// </summary>
        public static readonly DynamicDomainServiceRegistation Instance = new DynamicDomainServiceRegistation();
        private Dictionary<String, IDynamicDomainService> _dynamicServices = new Dictionary<String, IDynamicDomainService>();
        private Object _lockObj = new Object();

        #endregion

        #region Methods

        /// <summary>
        ///     注册一个动态程序域服务
        ///     <para>* 如果已经存在当前要注册的动态程序域服务，则进行替换操作。</para>
        /// </summary>
        /// <param name="service"></param>
        public void Regist(IDynamicDomainService service)
        {
            if (service == null) return;
            lock (_lockObj)
            {
                if (_dynamicServices.ContainsKey(service.Infomation.ServiceName))
                {
                    _dynamicServices[service.Infomation.ServiceName] = service;
                    return;
                }
                _dynamicServices.Add(service.Infomation.ServiceName, service);
            }
        }

        /// <summary>
        ///     注销一个具有指定服务名称的动态程序域服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void UnRegist(String serviceName)
        {
            if (String.IsNullOrEmpty(serviceName)) return;
            lock (_lockObj)
            {
                if (_dynamicServices.ContainsKey(serviceName)) _dynamicServices.Remove(serviceName);
            }
        }

        /// <summary>
        ///     获取具有指定服务名称动态程序域服务健康状态
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回健康状态</returns>
        public HealthStatus GetServiceHealth(String serviceName)
        {
            if (String.IsNullOrEmpty(serviceName)) return HealthStatus.Death;
            lock (_lockObj)
            {
                IDynamicDomainService service;
                if (_dynamicServices.TryGetValue(serviceName, out service)) return service.CheckHealth();
                return HealthStatus.Death;
            }
        }

        /// <summary>
        ///     获取所有已注册动态程序与服务的名称列表
        /// </summary>
        /// <returns>返回名称列表</returns>
        public String[] GetServiceNames()
        {
            lock (_lockObj)
            {
                if (_dynamicServices.Count == 0) return null;
                return _dynamicServices.Values.Select(service => service.Infomation.ServiceName).ToArray();
            }
        }

        /// <summary>
        ///     更新具有指定服务名称的动态程序域服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回更新的状态</returns>
        public bool Update(String serviceName)
        {
            if (String.IsNullOrEmpty(serviceName)) return false;
            lock (_lockObj)
            {
                IDynamicDomainService service;
                if (_dynamicServices.TryGetValue(serviceName, out service)) return service.Update();
                return false;
            }
        }

        /// <summary>
        ///     根据指定的服务名称和组件名称来获取一个程序域组件
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="moduleName">组件名称</param>
        /// <returns>返回获取到的程序域组件</returns>
        internal IDynamicDomainComponent GetComponentByService(String serviceName, String moduleName)
        {
            lock (_lockObj)
            {
                IDynamicDomainService service;
                if (_dynamicServices.TryGetValue(serviceName, out service)) return service.GetObject(moduleName);
                return null;
            }
        }

        /// <summary>
        ///     获取指定的服务对象
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>返回服务对象</returns>
        internal  IDynamicDomainService GetService(String serviceName)
        {
            lock (_lockObj)
            {
                IDynamicDomainService service;
                return _dynamicServices.TryGetValue(serviceName, out service) ? service : null;
            }
        }

        #endregion
    }
}