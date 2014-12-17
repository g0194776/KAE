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
            return null;
        }

        #endregion
    }
}