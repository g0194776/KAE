using System;
using System.Collections.Generic;

namespace KJFramework.ServiceModel.Contexts
{
    /// <summary>
    ///     ������Լ���������Ĺ��������ṩ����صĻ���������
    /// </summary>
    public class ServiceContextManager
    {
        #region ���캯��

        private ServiceContextManager()
        {
            
        }

        #endregion

        #region ��Ա

        public static readonly ServiceContextManager Instance = new ServiceContextManager();
        protected Dictionary<int, IServiceCallContext> _contexts = new Dictionary<int, IServiceCallContext>();
        private Object _lockObj = new Object();

        #endregion

        #region ����

        /// <summary>
        ///     ע��һ���������������
        /// </summary>
        /// <param name="id">Ψһ���</param>
        /// <param name="sessionId">�Ự��Կ</param>
        /// <param name="instance">����ʵ��</param>
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
        ///     ע��һ���������������
        /// </summary>
        /// <param name="context">������</param>
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
        ///     ע��һ���������������
        /// </summary>
        /// <param name="id">Ψһ���</param>
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
        ///     ��ȡ��ǰ�������������
        /// </summary>
        /// <param name="instance">�������</param>
        /// <returns>����������</returns>
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