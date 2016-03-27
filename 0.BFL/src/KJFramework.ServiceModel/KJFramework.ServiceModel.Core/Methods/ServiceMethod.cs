using System;
using System.Reflection;
using KJFramework.Helpers;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     服务方法父类，提供了相关的基本操作。
    /// </summary>
    public class ServiceMethod : IServiceMethod
    {
        #region 构造函数

        /// <summary>
        ///     服务方法父类，提供了相关的基本操作。
        ///     <para>使用一个MethodInfo来初始化当前服务方法。</para>
        /// </summary>
        /// <param name="method">基础服务</param>
        public ServiceMethod(MethodInfo method)
        {
            _id = Guid.NewGuid();
            _method = method;
            if (_method != null)
            {
                Initialize();
            }
        }

        #endregion

        #region 成员

        protected ParameterInfo[] _parameterInfos;
        protected MethodInfo _method;
        /// <summary>
        ///     获取内部核心方法信息
        /// </summary>
        public MethodInfo CoreMethodInfo
        {
            get { return _method; }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化
        /// </summary>
        protected virtual void Initialize()
        {
            _parameterInfos = _method.GetParameters();
            _argsCount = _parameterInfos == null ? 0 : _parameterInfos.Length;
            _name = _method.DeclaringType.FullName + "." +  _method.Name;
            if (_method.ReturnParameter != null && _method.ReturnType.FullName.ToLower() != "system.void")
            {
                _hasReturnValue = true;
                _returnType = _method.ReturnType;
            }
            _attribute = AttributeHelper.GetCustomerAttribute<OperationAttribute>(_method);
        }

        /// <summary>
        ///     获取核心方法对象
        /// </summary>
        /// <returns>返回核心方法对象</returns>
        public MethodInfo GetCoreMethod()
        {
            return _method;
        }

        #endregion

        #region Implementation of IDisposable

        protected Guid _id;
        protected int _argsCount;
        protected string _name;
        protected bool _hasReturnValue;
        protected Type _returnType;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IServiceMethod

        protected OperationAttribute _attribute;

        /// <summary>
        ///     获取或设置操作属性
        /// </summary>
        public OperationAttribute Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        /// <summary>
        ///     获取或设置唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取或设置参数个数
        /// </summary>
        public int ArgsCount
        {
            get { return _argsCount; }
            set { _argsCount = value; }
        }

        /// <summary>
        ///     获取或设置方法名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     获取或设置一个值，该值表示了当前方法是否具有返回值
        ///     <para>* void 不算拥有返回值。</para>
        /// </summary>
        public bool HasReturnValue
        {
            get { return _hasReturnValue; }
            set { _hasReturnValue = value; }
        }

        /// <summary>
        ///     获取或设置返回值类型
        /// </summary>
        public Type ReturnType
        {
            get { return _returnType; }
            set { _returnType = value; }
        }

        /// <summary>
        ///     获取参数类型
        /// </summary>
        /// <param name="paraIndex">参数索引</param>
        /// <returns>返回参数类型</returns>
        public Type GetParameterType(int paraIndex)
        {
            if (_parameterInfos == null || _parameterInfos.Length == 0) return null;
            return _parameterInfos[paraIndex].ParameterType;
        }

        #endregion
    }
}