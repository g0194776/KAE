using System;
using System.Collections.Generic;
using KJFramework.Engin.Scheduler.Rule;
using KJFramework.Engin.Worker;

namespace KJFramework.Engin.Scheduler
{
    /// <summary>
    ///     �����ߵ�����Ԫ�ӿڣ��ṩ����صĵ��ȹ�����
    /// </summary>
    /// <typeparam name="TWorker">����������</typeparam>
    /// <typeparam name="THost">����������Ҫ����������</typeparam>
    public interface IWorkerScheduler<TWorker, THost> : IDisposable
        where TWorker : IWorker<THost>
    {
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�ĵ������Ƿ��Ѿ������ȫ���ĵ��ȹ���
        /// </summary>
        bool IsFinish { get; }
        /// <summary>
        ///     ��ȡ���ȹ���
        /// </summary>
        IWorkerSchedulerRule<TWorker, THost> Rule { get; }
        /// <summary>
        ///     ִ�е��ȹ���
        /// </summary>
        /// <param name="worker">Ҫ���ȵĹ����߼���</param>
        void Dispatcher(List<TWorker> worker);
    }
}