using System;
using KJFramework.Enums;
using KJFramework.Statistics;

namespace KJFramework.Configurations.Loaders
{
    /// <summary>
    ///     ���ü�����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface IConfigurationLoader : IStatisticable<Statistic>, IDisposable
    {
        /// <summary>
        ///     ��ȡ���ü���������
        /// </summary>
        ConfigurationLoaderTypes ConfigurationLoaderType { get; }
        /// <summary>
        ///     ��������
        /// </summary>
        /// <typeparam name="T">�Զ������ý�����</typeparam>
        /// <param name="action">��ֵ�Զ������ýڵĶ���</param>
        void Load<T>(Action<T> action) where T : class, new();
    }
}