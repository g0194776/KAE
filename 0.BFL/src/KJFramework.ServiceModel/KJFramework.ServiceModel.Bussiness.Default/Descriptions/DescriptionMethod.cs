using System;
using System.Collections.Generic;
using System.Reflection;
using KJFramework.ServiceModel.Core.Methods;

namespace KJFramework.ServiceModel.Bussiness.Default.Descriptions
{
    /// <summary>
    ///     描述方法父类，提供了相关的基本操作。
    /// </summary>
    internal class DescriptionMethod : ServiceMethod, IDescriptionMethod
    {
        #region 构造函数

        /// <summary>
        ///     服务方法父类，提供了相关的基本操作。
        ///     <para>使用一个MethodInfo来初始化当前服务方法。</para>
        /// </summary>
        /// <param name="method">基础服务</param>
        internal DescriptionMethod(MethodInfo method)
            : base(method)
        {
            ParameterInfo[] parameters;
            if ((parameters = method.GetParameters()) != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    //这里默认都是可以为空的
                    _arguments.Add(new DescriptionArgument(i, true, parameters[i]));
                }
            }
            if (method.ReturnType.FullName != "System.Void")
            {
                _returnType = method.ReturnType;
                _hasReturnValue = true;
            }
            else
            {
                _hasReturnValue = false;
            }
        }

        #endregion

        #region Implementation of IDescriptionMethod

        protected String _returnTypeFullName;
        protected List<IDescriptionArgument> _arguments = new List<IDescriptionArgument>();

        /// <summary>
        ///     获取或设置返回类型全名称
        /// </summary>
        public string ReturnTypeFullName
        {
            get { return _returnTypeFullName; }
            set { _returnTypeFullName = value; }
        }

        #endregion

        #region Implementation of IDescriptionMethod

        /// <summary>
        ///     获取所有该操作的参数
        /// </summary>
        /// <returns>返回参数集合</returns>
        public IDescriptionArgument[] GetArguments()
        {
            return _arguments == null ? null : _arguments.ToArray();
        }

        /// <summary>
        ///     获取操作的标示
        /// </summary>
        public int MethodToken
        {
            get
            {
                if (_attribute.MethodToken != 0)
                {
                    return _attribute.MethodToken;
                }
                return _method.MetadataToken;
            }
        }

        #endregion
    }
}