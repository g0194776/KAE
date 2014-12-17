using KJFramework.Tracing;
using System;
using System.Collections.Generic;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     组件隧道访问器
    /// </summary>
    public class ComponentTunnelVisitor : IComponentTunnelVisitor
    {
        #region Constructor

        /// <summary>
        ///     组件隧道访问器
        /// </summary>
        /// <param name="addresses">隧道地址集合</param>
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
        ///     获取指定组件的隧道
        /// </summary>
        /// <param name="componentName">组件名称</param>
        /// <returns>返回组件的隧道</returns>
        public T GetTunnel<T>(string componentName)
            where T : class
        {
            return null;
        }

        #endregion
    }
}