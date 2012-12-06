using System;
using System.Collections.Generic;
using KJFramework.Messages.Extends.Actions;
using KJFramework.Messages.Extends.Splitters;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     第三方消息结构定义器，提供了相关的基本操作。
    /// </summary>
    public sealed class CustomerMessageDefiner : ICustomerMessageDefiner
    {
        #region 构造函数

        /// <summary>
        ///     第三方消息结构定义器，提供了相关的基本操作。
        /// </summary>
        public CustomerMessageDefiner() : this(null)
        {
            
        }

        /// <summary>
        ///     第三方消息结构定义器，提供了相关的基本操作。
        /// </summary>
        /// <param name="key">键值</param>
        public CustomerMessageDefiner(String key)
        {
            _key = key;
            _id = Guid.NewGuid();
            _extendActions = new SortedSet<IExtendBuildAction>();
        }

        #endregion

        #region 析构函数

        ~CustomerMessageDefiner()
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        protected Guid _id;
        protected string _key;
        protected SortedSet<IExtendBuildAction> _extendActions;
        protected IFieldBuildAction _fieldAction;
        protected IHeadBuildAction _headAction;
        protected IEndBuildAction _endAction;
        protected IMetadataFieldSplitter _splitter;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of ICustomerMessageDefiner

        /// <summary>
        ///     获取定义器唯一编号
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        ///     获取或设置定义器键值
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        ///     获取或设置扩展动作集合
        /// </summary>
        public SortedSet<IExtendBuildAction> ExtendActions
        {
            get { return _extendActions; }
            set { _extendActions = value; }
        }

        /// <summary>
        ///     获取或设置消息字段构造动作
        /// </summary>
        public IFieldBuildAction FieldAction
        {
            get { return _fieldAction; }
            set { _fieldAction = value; }
        }

        /// <summary>
        ///     获取或设置消息头部构造动作
        /// </summary>
        public IHeadBuildAction HeadAction
        {
            get { return _headAction; }
            set { _headAction = value; }
        }

        /// <summary>
        ///     获取或设置消息字尾部构造动作
        /// </summary>
        public IEndBuildAction EndAction
        {
            get { return _endAction; }
            set { _endAction = value; }
        }

        /// <summary>
        ///     获取或设置消息元数据字段分离器
        /// </summary>
        public IMetadataFieldSplitter Splitter
        {
            get { return _splitter; }
            set { _splitter = value; }
        }

        /// <summary>
        ///     检查自身合法性
        /// </summary>
        public void Check()
        {
            if (_fieldAction == null)
            {
                throw new System.Exception("非法的第三方消息结构定义器，缺失的字段构造动作。");
            }
            if (_splitter == null)
            {
                throw new System.Exception("非法的第三方消息结构定义器，缺失的消息元数据字段分离器。");
            }
            if (String.IsNullOrEmpty(_key))
            {
                throw new System.Exception("非法的第三方消息结构定义器，缺失的用户唯一标示。");
            }
        }

        #endregion
    }
}