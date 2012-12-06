using System;
using KJFramework.Statistics;

namespace KJFramework.Dynamic.Visitors
{
    /// <summary>
    ///     ��̬���������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDynamicObjectVisitor : IStatisticable<IStatistic>, IDisposable
    {
        /// <summary>
        ///     ��ȡһ������ָ�����ƵĶ�̬���������
        ///     <para>* �����ǰҪ��ȡ������������������ȡ�������Ϊ����ǰ�汾��</para>
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="flag">�������</param>
        /// <param name="args">��ѡ����</param>
        /// <returns>����ָ������</returns>
        T GetObject<T>(String flag, params Object[] args);
    }
}