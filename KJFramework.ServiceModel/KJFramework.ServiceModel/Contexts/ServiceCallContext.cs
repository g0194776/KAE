using System;

namespace KJFramework.ServiceModel.Contexts
{
    /// <summary>
    ///     服务契约操作调用上下文，提供了相关的基本属性结构。
    /// </summary>
    public class ServiceCallContext : IServiceCallContext
    {
        #region 构造函数

        /// <summary>
        ///     服务契约操作调用上下文，提供了相关的基本属性结构。
        /// </summary>
        public ServiceCallContext()
        {
            
        }

        /// <summary>
        ///     服务契约操作调用上下文，提供了相关的基本属性结构。
        /// </summary>
        /// <param name="id">唯一编号</param>
        /// <param name="sessionId">会话密钥</param>
        /// <param name="instance">调用实例</param>
        public ServiceCallContext(int id, int sessionId, Object instance)
        {
            _id = id;
            _sessionId = sessionId;
            _instance = instance;
        }

        #endregion

        #region Implementation of IDisposable

        protected int _id;
        protected int _sessionId;
        protected object _instance;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServiceCallContext

        /// <summary>
        ///     获取或设置唯一编号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     获取会话密钥
        /// </summary>
        public int SessionId
        {
            get { return _sessionId; }
            internal set { _sessionId = value; }
        }

        /// <summary>
        ///     获取调用实例
        /// </summary>
        public object Instance
        {
            get { return _instance; }
            internal set { _instance = value; }
        }

        #endregion
    }
}