using System;
using System.Reflection;
using KJFramework.Logger;
using KJFramework.ServiceModel.Core.Helpers;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     ��ִ�еķ��񷽷����࣬�ṩ����صĻ�������
    /// </summary>
    public class ExecutableServiceMethod : ServiceMethod, IExecutableServiceMethod
    {
        #region ���캯��

        /// <summary>
        ///     ���񷽷����࣬�ṩ����صĻ���������
        /// </summary>
        /// <param name="method">������Ϣ</param>
        internal ExecutableServiceMethod(MethodInfo method)
            : base(method)
        {
        }

        /// <summary>
        ///     ���񷽷����࣬�ṩ����صĻ���������
        ///     <para>ʹ��һ��MethodInfo����ʼ����ǰ���񷽷���</para>
        /// </summary>
        /// <param name="service">����</param>
        /// <param name="method">��������</param>
        internal ExecutableServiceMethod(object service, MethodInfo method)
            : base(method)
        {
            _instance = service;
        }

        #endregion

        #region Members

        internal DynamicHelper.FastInvokeHandler Handler;

        #endregion

        #region IExecutableServiceMethod Members

        protected Object _instance;
        /// <summary>
        ///     ��ȡ����������ʵ��
        /// </summary>
        public Object Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        /// <summary>
        ///     ִ�е�ǰ���񷽷�
        /// </summary>
        /// <param name="args">��������</param>
        /// <returns>
        ///     ���ص�ǰ���񷽷��ķ���ֵ
        ///     <para>* �����ǰ���������з���ֵ����Ҳ�᷵��null��</para>
        /// </returns>
        public object Invoke(params object[] args)
        {
            try
            {
                if (_instance == null) throw new System.Exception("can not invoke current method, because not set inner service");
                return Handler(_instance, args);
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                throw;
            }
        }

        #endregion
    }
}