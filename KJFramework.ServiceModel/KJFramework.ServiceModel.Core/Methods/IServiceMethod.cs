using System;
using KJFramework.ServiceModel.Core.Attributes;

namespace KJFramework.ServiceModel.Core.Methods
{
    /// <summary>
    ///     服务方法元接口，提供了相关的基本操作
    /// </summary>
    public interface IServiceMethod : IDisposable
    {
        /// <summary>
        ///     获取或设置操作属性
        /// </summary>
        OperationAttribute Attribute { get; set; }
        /// <summary>
        ///     获取或设置唯一编号
        /// </summary>
        Guid Id { get;  }
        /// <summary>
        ///     获取或设置参数个数
        /// </summary>
        int ArgsCount { get; set; }
        /// <summary>
        ///     获取或设置方法名
        /// </summary>
        String Name { get; set; }
        /// <summary>
        ///     获取或设置一个值，该值表示了当前方法是否具有返回值
        ///     <para>* void 不算拥有返回值。</para>
        /// </summary>
        bool HasReturnValue { get; set; }
        /// <summary>
        ///     获取或设置返回值类型
        /// </summary>
        Type ReturnType { get; set; }
        /// <summary>
        ///     获取参数类型
        /// </summary>
        /// <param name="paraIndex">参数索引</param>
        /// <returns>返回参数类型</returns>
        Type GetParameterType(int paraIndex);
    }
}