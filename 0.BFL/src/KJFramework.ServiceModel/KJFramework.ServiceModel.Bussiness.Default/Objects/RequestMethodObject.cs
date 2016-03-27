using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;
using KJFramework.ServiceModel.Bussiness.Default.Metadata;
using KJFramework.ServiceModel.Metadata;

namespace KJFramework.ServiceModel.Bussiness.Default.Objects
{
    /// <summary>
    ///     请求方法对象，提供了相关的基本操作。
    /// </summary>
    public class RequestMethodObject : IntellectObject
    {
        #region Constructor

        /// <summary>
        ///     请求方法对象，提供了相关的基本操作。
        /// </summary>
        public RequestMethodObject()
        {
            
        }

        /// <summary>
        ///     请求方法对象，提供了相关的基本操作。
        /// </summary>
        /// <param name="argsCount">参数个数</param>
        public RequestMethodObject(int argsCount)
        {
            if (argsCount > 0) Context = new BinaryArgContext[argsCount];
        }

        /// <summary>
        ///     请求方法对象，提供了相关的基本操作。
        /// </summary>
        /// <param name="data">元数据</param>
        public RequestMethodObject(byte[] data)
        {
            _body = data;
        }

        #endregion

        #region Members

        /// <summary>
        ///     获取参数集合
        /// </summary>
        [IntellectProperty(0)]
        public BinaryArgContext[] Context { get; set; }
        /// <summary>
        ///     获取或设置方法序列标记
        /// </summary>
        [IntellectProperty(1, IsRequire = true)]
        public int MethodToken { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     添加一个二进制参数上下文到集合中
        /// </summary>
        /// <param name="context">二进制参数上下文</param>
        ///<exception cref="System.Exception">添加失败</exception>
        public virtual void AddArg(BinaryArgContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            Context[context.Id] = context;
        }

        /// <summary>
        ///     根据指定编号获取二进制参数上下文
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回二进制参数上下文</returns>
        public virtual IBinaryArgContext GetArg(int id)
        {
            return Context[id];
        }

        /// <summary>
        ///     获取所有参数
        /// </summary>
        /// <returns>返回参数集合</returns>
        public virtual IBinaryArgContext[] GetArgs()
        {
            return Context;
        }

        #endregion
    }
}