using System;
using System.Collections;
using System.Collections.Generic;
using KJFramework.Net.Cloud.Exceptions;
using KJFramework.Net.Cloud.Processors;

namespace KJFramework.Net.Cloud.Nodes
{
    /// <summary>
    ///     功能节点，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">协议栈中父类消息类型。</typeparam>
    public abstract class FunctionNode<T> : IFunctionNode<T>
    {
        #region 构造函数

        /// <summary>
        ///     功能节点，提供了相关的基本操作。
        /// </summary>
        public FunctionNode()
        {
            _id = Guid.NewGuid();
        }

        #endregion

        #region 析构函数

        ~FunctionNode()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IFunctionNode<T>

        protected bool _enable;
        protected Guid _id;
        protected Object _tag;

        /// <summary>
        ///     获取可用标示
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
        }

        /// <summary>
        ///     获取唯一键值
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///   获取或设置附属属性
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        ///     初始化
        /// </summary>
        /// <returns>返回初始化状态</returns>
        /// <exception cref="InitializeFailedException">初始化失败</exception>
        public abstract bool Initialize();

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}