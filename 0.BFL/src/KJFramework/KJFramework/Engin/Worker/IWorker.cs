using System;
using KJFramework.EventArgs;

namespace KJFramework.Engin.Worker
{
    /// <summary>
    ///     ������Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public interface IWorker<T> : IDisposable
    {
        /// <summary>
        ///     ��ȡ��ǰ�����ߵĹ���״̬
        /// </summary>
        bool State { get; }
        /// <summary>
        ///     ִ�з�����ʹ�õ�ǰ�߳̽��з�����ִ�в�����
        /// </summary>
        /// <param name="item">�������� [ref]</param>
        /// <returns>���ع�����״̬</returns>
        bool DoWork(T item);
        /// <summary>
        ///     ִ�з������첽���з�����ִ�в�����
        /// </summary>
        /// <param name="item">�������� [ref]</param>
        /// <returns>���ع�����״̬</returns>
        bool DoWorkAsyn(T item);
        /// <summary>
        ///     �����߹���״̬�㱨�¼�
        /// </summary>
        event DelegateWorkerProcessing WorkerProcessing;
        /// <summary>
        ///     �����߿�ʼ�����¼�
        /// </summary>
        event DelegateBeginWork BeginWork;
        /// <summary>
        ///     ������ֹͣ�����¼�
        /// </summary>
        event DelegateEndWork EndWork;
    }
}