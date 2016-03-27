using System;
using KJFramework.Matcher;

namespace KJFramework.Loader
{
    /// <summary>
    ///     װ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ILoader
    {
        /// <summary>
        ///     ��ȡ������ƥ����
        /// </summary>
        IMatcher Matcher { get; set; }
        /// <summary>
        ///     ������������ΪҪװ�ص����ͣ������Զ�ע�����
        /// </summary>
        /// <param name="type">��������� [ref]</param>
        /// <param name="path">�ļ�ȫ·��</param>
        /// <param name="obj">����ʵ��</param>
        /// <returns>����װ�ص�״̬</returns>
        bool Load(Type type, String path, Object obj);
        /// <summary>
        ///     ������������ΪҪװ�ص����ͣ������Զ�ע�����
        /// </summary>
        /// <param name="path">�ļ�ȫ·��</param>
        /// <returns>����װ�ص�״̬</returns>
        bool Load(String path);
        /// <summary>
        ///     ��һ���ļ��Զ�ע�뵽�������͵ı�Ҫ�ֶ���
        /// </summary>
        /// <typeparam name="T">Ҫע�������</typeparam>
        /// <param name="path">�ļ�ȫ·��</param>
        /// <returns>����װ�صĺ�����������null, ���ʾװ��ʧ��</returns>
        T Load<T> (String path);
    }
}