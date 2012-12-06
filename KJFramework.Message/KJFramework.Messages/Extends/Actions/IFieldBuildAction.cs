using System.Reflection;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Extends.Contexts;

namespace KJFramework.Messages.Extends.Actions
{
    /// <summary>
    ///     ��Ϣ�ֶι��춯��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IFieldBuildAction : IBuildAction
    {
        /// <summary>
        ///     ����һ����Ϣ�ֶ�
        /// </summary>
        /// <typeparam name="T">�ֶ�����</typeparam>
        /// <typeparam name="TContextKey">������ֵ����</typeparam>
        /// <param name="value">�ֶ�ֵ</param>
        /// <param name="attribute">�ֶ����Ա�ǩ</param>
        /// <param name="propertyInfo">�ֶ�������Ϣ</param>
        /// <param name="context">������</param>
        /// <returns>���ع�����Ԫ����</returns>
        byte[] Bind<T, TContextKey>(T value, IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, IFieldBuildActionContext<TContextKey> context);
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
        T Pickup<T, TContextKey>(IntellectPropertyAttribute attribute, PropertyInfo propertyInfo, byte[] data, IFieldBuildActionContext<TContextKey> context);
    }
}