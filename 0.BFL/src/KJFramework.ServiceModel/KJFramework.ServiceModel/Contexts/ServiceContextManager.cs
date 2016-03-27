using System;
using System.Collections.Generic;

namespace KJFramework.ServiceModel.Contexts
{
    /// <summary>
    ///     服务契约调用上下文管理器，提供了相关的基本操作。
    /// </summary>
    public class ServiceContextManager
    {
        #region 构造函数

        private ServiceContextManager()
        {
            
        }

        #endregion

        #region 成员

        public static readonly ServiceContextManager Instance = new ServiceContextManager();
        protected Dictionary<int, IServiceCallContext> _contexts = new Dictionary<int, IServiceCallContext>();
        private Object _lockObj = new Object();

        #endregion

        #region 方法

        /// <summary>
        ///     注册一个服务调用上下文
        /// </summary>
        /// <param name="id">唯一编号</param>
        /// <param name="sessionId">会话密钥</param>
        /// <param name="instance">调用实例</param>
        internal void Regist(int id, int sessionId, Object instance)
        {
            lock (_lockObj)
            {
                if (instance == null)
                {
                    return;
                }
                Regist(new ServiceCallContext(id, sessionId, instance));
            }
        }

        /// <summary>
        ///     注册一个服务调用上下文
        /// </summary>
        /// <param name="context">上下文</param>
        internal void Regist(IServiceCallContext context)
        {
            lock (_lockObj)
            {
                if (context == null)
                {
                    return;
                }
                if (!_contexts.ContainsKey(context.Id))
                {
                    _contexts.Add(context.Id, context);
                }
            }
        }

        /// <summary>
        ///     注销一个服务调用上下文
        /// </summary>
        /// <param name="id">唯一编号</param>
        internal void UnRegist(int id)
        {
            lock (_lockObj)
            {
                if (_contexts.ContainsKey(id))
                {
                    _contexts.Remove(id);
                }
            }
        }

        /// <summary>
        ///     获取当前服务调用上下文
        /// </summary>
        /// <param name="instance">服务对象</param>
        /// <returns>返回上下文</returns>
        public IServiceCallContext GetCurrentContext(Object instance)
        {
            if (instance == null)
            {
                return null;
            }
            lock (_lockObj)
            {
                int id = instance.GetHashCode();
                if (_contexts.ContainsKey(id))
                {
                    return _contexts[id];
                }
            }
            return null;
        }

        #endregion
    }
}