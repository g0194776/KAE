using System;
using System.Reflection;
using KJFramework.Logger;
using KJFramework.ServiceModel.Core.Helpers;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     可执行的服务方法父类，提供了相关的基本操作
    /// </summary>
    public class ExecutableServiceMethod : ServiceMethod, IExecutableServiceMethod
    {
        #region 构造函数

        /// <summary>
        ///     服务方法父类，提供了相关的基本操作。
        /// </summary>
        /// <param name="method">方法信息</param>
        internal ExecutableServiceMethod(MethodInfo method)
            : base(method)
        {
        }

        /// <summary>
        ///     服务方法父类，提供了相关的基本操作。
        ///     <para>使用一个MethodInfo来初始化当前服务方法。</para>
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="method">基础服务</param>
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
        ///     获取或设置运行实例
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
        ///     执行当前服务方法
        /// </summary>
        /// <param name="args">参数数组</param>
        /// <returns>
        ///     返回当前服务方法的返回值
        ///     <para>* 如果当前方法不具有返回值，则也会返回null。</para>
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