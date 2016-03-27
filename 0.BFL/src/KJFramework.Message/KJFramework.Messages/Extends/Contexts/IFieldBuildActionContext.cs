using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;

namespace KJFramework.Messages.Extends.Contexts
{
    /// <summary>
    ///     ��Ϣ�ֶι��춯�������Ķ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IFieldBuildActionContext<T> : IDisposable
    {
        /// <summary>
        ///     ��ȡ����ָ����ŵ���Ϣ�ֶ�������
        /// </summary>
        /// <param name="id">�ֶα��</param>
        /// <returns>������Ϣ�ֶ�������</returns>
        KeyValuePair<IIntellectProperty, T>? GetValue(int id);
        /// <summary>
        ///     ��ȡ����ָ����ŵ���Ϣ�ֶ�������
        /// </summary>
        /// <param name="tag">�ֶθ�������</param>
        /// <returns>������Ϣ�ֶ�������</returns>
        KeyValuePair<IIntellectProperty, T>? GetValue(String tag);
    }
}