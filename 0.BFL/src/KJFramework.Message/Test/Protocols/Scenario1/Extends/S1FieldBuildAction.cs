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
        ///     ����һ����Ϣ�ֶ�
        /// </summary>
        /// <typeparam name="T">�ֶ�����</typeparam>
        /// <typeparam name="TContextKey">������ֵ����</typeparam>
        /// <param name="value">�ֶ�ֵ</param>
        /// <param name="attribute">�ֶ����Ա�ǩ</param>
        /// <param name="context">������</param>
        /// <returns>���ع�����Ԫ����</returns>
        public override byte[] Bind<T, TContextKey>(T value, IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, IFieldBuildActionContext<TContextKey> context)
        {
            Type type = propertyInfo.PropertyType;
            IIntellectTypeProcessor processor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            return processor.Process(attribute, value);
        }

        /// <summary>
        ///     ��ȡһ����Ϣ�ֶ�
        /// </summary>
        /// <typeparam name="T">�ֶ�����</typeparam>
        /// <typeparam name="TContextKey">������ֵ����</typeparam>
        /// <param name="attribute">�ֶ����Ա�ǩ</param>
        /// <param name="propertyInfo">�ֶ�������Ϣ</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="context">������</param>
        /// <returns>������Ϣ�ֶ�</returns>
        public override T Pickup<T, TContextKey>(IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, byte[] data, IFieldBuildActionContext<TContextKey> context)
        {
            Type type = propertyInfo.PropertyType;
            IIntellectTypeProcessor processor = IntellectTypeProcessorMapping.Instance.GetProcessor(type);
            return (T) processor.Process(attribute, data);
        }

        #endregion
    }
}