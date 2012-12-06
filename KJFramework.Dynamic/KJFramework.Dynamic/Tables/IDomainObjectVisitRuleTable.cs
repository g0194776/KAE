using System;

namespace KJFramework.Dynamic.Tables
{
    /// <summary>
    ///     �����������ʹ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDomainObjectVisitRuleTable : IDisposable
    {
        /// <summary>
        ///     ���һ�����ʹ���
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        /// <param name="func">���صĶ���</param>
        void Add(String key, Func<Object[], Object> func);
        /// <summary>
        ///     �Ƴ�һ������ָ�����ʼ�ֵ�Ķ���
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        void Remove(String key);
        /// <summary>
        ///     �ж�ָ�����ʼ�ֵ�Ƿ����
        /// </summary>
        /// <param name="key">���ʼ�ֵ</param>
        /// <returns>��ȡ�Ƿ���ڵı�ʾ</returns>
        bool Exists(String key);

        /// <summary>
        ///     ��ȡ����ָ�����Ƶ����Զ���
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="name">������</param>
        /// <param name="args">���ò���</param>
        /// <returns>�������Զ���</returns>
        T Get<T>(String name, params Object[] args);
    }
}