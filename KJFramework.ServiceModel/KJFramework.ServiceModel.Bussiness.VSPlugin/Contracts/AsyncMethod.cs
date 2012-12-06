using System.Collections.Generic;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     异步方法，用于自动生成使用
    /// </summary>
    public class AsyncMethod : Method
    {
        #region Constructor

        /// <summary>
        ///     异步方法，用于自动生成使用
        /// </summary>
        /// <param name="method">基础方法</param>
        /// <param name="basePath">契约基础地址</param>
        public AsyncMethod(Method method, string basePath)
            : base(basePath)
        {
            _method = method;
            method.Name = method.Name + "Async";
            method.IsAsync = true;
            method.IsOneway = false;
            if (method.Arguments != null && method.Arguments.Count > 0)
                method.Arguments.Add(new Argument { Id = method.Arguments.Count, Name = "callback", Type = "KJFramework.ServiceModel.Proxy.AsyncMethodCallback" });
        }

        #endregion

        #region Members

        private readonly Method _method;

        /// <summary>
        ///     获取方法名
        /// </summary>
        public override string Name
        {
            get
            {
                return _method.Name;
            }
            internal set
            {
                _method.Name = value;
            }
        }

        /// <summary>
        ///     获取一个标示，该标示表示了当前方法是否为异步方法
        /// </summary>
        public override bool IsAsync
        {
            get
            {
                return _method.IsAsync;
            }
            internal set
            {
                _method.IsAsync = value;
            }
        }

        /// <summary>
        ///     获取方法全名
        /// </summary>
        public override string FullName
        {
            get
            {
                return _method.FullName;
            }
            internal set
            {
                _method.FullName = value;
            }
        }

        /// <summary>
        ///     获取方法令牌
        /// </summary>
        public override string MethodToken
        {
            get
            {
                return _method.MethodToken;
            }
            internal set
            {
                _method.MethodToken = value;
            }
        }

        /// <summary>
        ///     获取参数集合
        /// </summary>
        public override IList<IArgument> Arguments
        {
            get
            {
                return _method.Arguments;
            }
        }

        /// <summary>
        ///      获取一个标示，该标示表示了当前方法时候具有返回值
        /// </summary>
        public override bool HasReturnValue
        {
            get
            {
                return _method.HasReturnValue;
            }
            internal set
            {
                _method.HasReturnValue = value;
            }
        }

        /// <summary>
        ///     获取返回值类型
        /// </summary>
        public override string ReturnType
        {
            get
            {
                return _method.ReturnType;
            }
            internal set
            {
                _method.ReturnType = value;
            }
        }

        /// <summary>
        ///     返回值类型定义Id
        ///     <para>* 此值仅当IsRetTypeCustome = true时有效.</para>
        /// </summary>
        public override string ReferenceId
        {
            get
            {
                return _method.ReferenceId;
            }
            internal set
            {
                _method.ReferenceId = value;
            }
        }

        /// <summary>
        ///     获取一个标示，该标示表示了当前方法是否为单向的
        /// </summary>
        public override bool IsOneway
        {
            get
            {
                return _method.IsOneway;
            }
            internal set
            {
                _method.IsOneway = value;
            }
        }

        #endregion
    }
}