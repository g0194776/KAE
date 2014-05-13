using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Dynamic.Components;
using KJFramework.Enums;

namespace KJFramework.Dynamic.Pools
{
    /// <summary>
    ///     ��̬���������ע��վ���ṩ����صĻ���������
    /// </summary>
    public class DynamicDomainServiceRegistation
    {
        #region Constructor

        /// <summary>
        ///     ��̬���������ע��վ���ṩ����صĻ���������
        /// </summary>
        private DynamicDomainServiceRegistation()
        {

        }

        #endregion

        #region Members

        /// <summary>
        ///     ��̬���������ע��վ���ṩ����صĻ���������
        /// </summary>
        public static readonly DynamicDomainServiceRegistation Instance = new DynamicDomainServiceRegistation();
        private Dictionary<String, IDynamicDomainService> _dynamicServices = new Dictionary<String, IDynamicDomainService>();
        private Object _lockObj = new Object();

        #endregion

        #region Methods

        /// <summary>
        ///     ע��һ����̬���������
        ///     <para>* ����Ѿ����ڵ�ǰҪע��Ķ�̬���������������滻������</para>
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
        ///     ע��һ������ָ���������ƵĶ�̬���������
        /// </summary>
        /// <param name="serviceName">��������</param>
        public void UnRegist(String serviceName)
        {
            if (String.IsNullOrEmpty(serviceName)) return;
            lock (_lockObj)
            {
                if (_dynamicServices.ContainsKey(serviceName)) _dynamicServices.Remove(serviceName);
            }
        }

        /// <summary>
        ///     ��ȡ����ָ���������ƶ�̬��������񽡿�״̬
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ؽ���״̬</returns>
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
        ///     ��ȡ������ע�ᶯ̬���������������б�
        /// </summary>
        /// <returns>���������б�</returns>
        public String[] GetServiceNames()
        {
            lock (_lockObj)
            {
                if (_dynamicServices.Count == 0) return null;
                return _dynamicServices.Values.Select(service => service.Infomation.ServiceName).ToArray();
            }
        }

        /// <summary>
        ///     ���¾���ָ���������ƵĶ�̬���������
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ظ��µ�״̬</returns>
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
        ///     ����ָ���ķ������ƺ������������ȡһ�����������
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <param name="moduleName">�������</param>
        /// <returns>���ػ�ȡ���ĳ��������</returns>
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
        ///     ��ȡָ���ķ������
        /// </summary>
        /// <param name="serviceName">��������</param>
        /// <returns>���ط������</returns>
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