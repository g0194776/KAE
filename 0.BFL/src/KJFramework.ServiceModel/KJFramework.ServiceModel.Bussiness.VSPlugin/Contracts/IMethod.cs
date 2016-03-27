using System;
using System.Collections.Generic;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts
{
    /// <summary>
    ///     预览方法元接口，提供了相关的基本操作
    /// </summary>
    public interface IMethod : ICloneable
    {
        /// <summary>
        ///     获取方法名
        /// </summary>
        string Name { get; }
        /// <summary>
        ///     获取方法全名
        /// </summary>
        string FullName { get; }
        /// <summary>
        ///     获取方法令牌
        /// </summary>
        string MethodToken { get; }
        /// <summary>
        ///     获取返回值类型
        /// </summary>
        string ReturnType { get; }
        /// <summary>
        ///     返回值类型定义Id
        ///     <para>* 此值仅当返回值类型为客户端自定义的时候才有效.</para>
        /// </summary>
        string ReferenceId { get; }
        /// <summary>
        ///      获取一个标示，该标示表示了当前方法时候具有返回值
        /// </summary>
        bool HasReturnValue { get; }
        /// <summary>
        ///     获取一个标示，该标示表示了当前方法是否为异步方法
        /// </summary>
        bool IsAsync { get; }
        /// <summary>
        ///     获取一个标示，该标示表示了当前方法是否为单向的
        /// </summary>
        bool IsOneway { get; }
        /// <summary>
        ///     获取参数集合
        /// </summary>
        IList<IArgument> Arguments { get; }
    }
}