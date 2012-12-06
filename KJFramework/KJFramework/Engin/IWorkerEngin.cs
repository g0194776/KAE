using KJFramework.Engin.Scheduler;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin
{
    /// <summary>
    ///     �����߹������棬�ṩ����صĻ���������
    /// </summary>
    public interface IWorkerEngin<TWorker, THost> : IEngin
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     ���֧�ֵĵ���������
        ///         * Ĭ��Ϊ 5
        /// </summary>
        int SchedulerCount { get; set; }
        /// <summary>
        ///     ��ӵ�����
        /// </summary>
        /// <param name="scheduler">�µĵ�����</param>
        /// <returns>���ؼ����״̬</returns>
        bool AddScheduler(IWorkerScheduler<TWorker, THost> scheduler);
        /// <summary>
        ///     ��ӹ�����
        /// </summary>
        /// <param name="worker">�µĹ�����</param>
        /// <returns>������ӵ�״̬</returns>
        bool AddWorker(TWorker worker);
    }
}