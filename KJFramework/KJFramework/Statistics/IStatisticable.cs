using System.Collections.Generic;
using KJFramework.Enums;

namespace KJFramework.Statistics
{
    /// <summary>
    ///     ��ͳ��Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IStatisticable<T> where T : IStatistic
    {
        /// <summary>
        ///     ��ȡ������ͳ����
        /// </summary>
        Dictionary<StatisticTypes, T> Statistics { get; set; }
    }
}