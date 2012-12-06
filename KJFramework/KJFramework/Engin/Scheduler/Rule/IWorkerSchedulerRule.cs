using System.Collections.Generic;
using KJFramework.Basic.Enum;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin.Scheduler.Rule
{
    /// <summary>
    ///     �����ߵ��ȹ����ṩ����صĵ��ȹ淶��
    /// </summary>
    public interface IWorkerSchedulerRule<TWorker, THost>
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     ��ȡ�������ȼ�
        /// </summary>
        SchedulerPriority Priority { get; }
        /// <summary>
        ///     ���ȼ�⣬�����ص�����������Ҫ�Ĺ����߼���
        /// </summary>
        /// <param name="worker">�����߼���</param>
        /// <returns>������Ҫ�Ĺ����߼���</returns>
        List<TWorker> Check(List<TWorker> worker);
    }
}