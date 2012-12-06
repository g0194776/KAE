using System;
using System.Collections.Generic;
using KJFramework.Messages.Extends.Actions;
using KJFramework.Messages.Extends.Splitters;

namespace KJFramework.Messages.Extends
{
    /// <summary>
    ///     第三方消息结构定义器元接口，提供了相关的基本操作。
    /// </summary>
    public interface ICustomerMessageDefiner : IDisposable
    {
        /// <summary>
        ///     获取定义器唯一编号
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     获取定义器键值
        /// </summary>
        String Key { get; }
        /// <summary>
        ///     获取扩展动作集合
        /// </summary>
        SortedSet<IExtendBuildAction> ExtendActions { get; }
        /// <summary>
        ///     获取消息字段构造动作
        /// </summary>
        IFieldBuildAction FieldAction { get; }
        /// <summary>
        ///     获取消息头部构造动作
        /// </summary>
        IHeadBuildAction HeadAction { get; }
        /// <summary>
        ///     获取消息字尾部构造动作
        /// </summary>
        IEndBuildAction EndAction { get; }
        /// <summary>
        ///     获取消息元数据字段分离器
        /// </summary>
        IMetadataFieldSplitter Splitter { get; }
        /// <summary>
        ///     检查自身合法性
        /// </summary>
        void Check();
    }
}