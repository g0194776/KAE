using System;

namespace KJFramework.Platform.Deploy.CSN.Common.Caches
{
    /// <summary>
    ///     ���ݻ���Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IDataCache<T>
    {
        /// <summary>
        ///     ��ȡ�����û����Ψһ��ֵ
        /// </summary>
        string Key { get; set; }
        /// <summary>
        ///     ��ȡ������Ҫ�������
        /// </summary>
        T Item { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        DateTime LastVisitTime { get; set; }
        /// <summary>
        ///     ��ȡ������������ʱ��
        /// </summary>
        DateTime LastUpdateTime { get; set; }
    }
}