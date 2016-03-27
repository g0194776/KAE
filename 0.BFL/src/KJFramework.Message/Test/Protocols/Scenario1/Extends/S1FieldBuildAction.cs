using System;
using System.Reflection;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Extends.Actions;
using KJFramework.Messages.Extends.Contexts;
using KJFramework.Messages.TypeProcessors;
using KJFramework.Messages.TypeProcessors.Maps;

namespace Test.Protocols.Scenario1.Extends
{
    public class S1FieldBuildAction : FieldBuildAction
    {
        #region Overrides of FieldBuildAction

        /// <summary>
        ///     构造一个消息字段
        /// </summary>
        /// <typeparam name="T">字段类型</typeparam>
        /// <typeparam name="TContextKey">上下文值类型</typeparam>
        /// <param name="value">字段值</param>
        /// <param name="attribute">字段属性标签</param>
        /// <param name="context">上下文</param>
        /// <returns>返回构造后的元数据</returns>
        public override byte[] Bind<T, TContextKey>(T value, IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, IFieldBuildActionContext<TContextKey> context)
        {
            Type type = propertyInfo.PropertyType;
            IIntellectTypeProcessor processor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            return processor.Process(attribute, value);
        }

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
        public override T Pickup<T, TContextKey>(IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, byte[] data, IFieldBuildActionContext<TContextKey> context)
        {
            Type type = propertyInfo.PropertyType;
            IIntellectTypeProcessor processor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            return (T) processor.Process(attribute, data);
        }

        #endregion
    }
}