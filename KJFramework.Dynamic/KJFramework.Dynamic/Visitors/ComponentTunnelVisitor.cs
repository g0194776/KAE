using KJFramework.ServiceModel.Bussiness.Default.Proxy;
using KJFramework.ServiceModel.Elements;
using KJFramework.ServiceModel.Proxy;
using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     ������������
    /// </summary>
    public class ComponentTunnelVisitor : IComponentTunnelVisitor
    {
        #region Constructor

        /// <summary>
        ///     ������������
        /// </summary>
        /// <param name="addresses">�����ַ����</param>
        public ComponentTunnelVisitor(Dictionary<string, string> addresses)
        {
            if (addresses == null) throw new ArgumentNullException("addresses");
            _addresses = addresses;
        }

        #endregion

        #region Members

        private readonly Dictionary<string, string> _addresses;
        private readonly Dictionary<string, Object> _clients = new Dictionary<string, Object>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ComponentTunnelVisitor));

        #endregion

        #region Implementation of IComponentTunnelVisitor

        /// <summary>
        ///     ��ȡָ����������
        /// </summary>
        /// <param name="componentName">�������</param>
        /// <returns>������������</returns>
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            if (componentName == null) throw new ArgumentNullException("componentName");
            Object client;
            if (_clients.TryGetValue(componentName, out client)) return ((IClientProxy<T>)client).Channel;
            string address;
            if (!_addresses.TryGetValue(componentName, out address)) return null;
            IClientProxy<T> ct = Create<T>(address);
            if (ct != null) _clients.Add(componentName, ct);
            return ct == null ? null : ct.Channel;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ����һ�����
        /// </summary>
        /// <param name="address">�����ַ</param>
        /// <returns>���ش����õ����</returns>
        private IClientProxy<T> Create<T>(string address)
            where T : class 
        {
            try
            {
                IClientProxy<T> client = new DefaultClientProxy<T>(new PipeBinding(address));
                return client;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        #endregion
    }
}