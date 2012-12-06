using System.Reflection;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Extends.Contexts;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     消息字段构造动作元接口，提供了相关的基本操作。
    /// </summary>
    public interface IFieldBuildAction : IBuildAction
    {
        /// <summary>
        ///     构造一个消息字段
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <typeparam name="TContextKey">上下文值类型</typeparam>
        /// <param name="value">字段值</param>
        /// <param name="attribute">字段属性标签</param>
        /// <param name="propertyInfo">字段类型信息</param>
        /// <param name="context">上下文</param>
        /// <returns>返回构造后的元数据</returns>
        byte[] Bind<T, TContextKey>(T value, IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, IFieldBuildActionContext<TContextKey> context);
        /// <summary>
        ///     提取一个消息字段
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <typeparam name="TContextKey">上下文值类型</typeparam>
        /// <param name="attribute">字段属性标签</param>
        /// <param name="propertyInfo">字段类型信息</param>
        /// <param name="data">元数据</param>
        /// <param name="context">上下文</param>
        /// <returns>返回消息字段</returns>
        T Pickup<T, TContextKey>(IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, byte[] data, IFieldBuildActionContext<TContextKey> context);
    }
}