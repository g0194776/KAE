using KJFramework.EventArgs;
using KJFramework.Net.Cloud.Tasks;
using KJFramework.Net.Exception;
using KJFramework.Tracing;
using System;
using System.Collections.Concurrent;

namespace KJFramework.Net.Cloud.Pools
{
    /// <summary>
    ///     ��������أ��ṩ����صĻ�������
    /// </summary>
    /// <typeparam name="T">Э��ջ�и�����Ϣ���͡�</typeparam>
    public class RequestTaskPool<T> : IRequestTaskPool<T>
    {
        #region ���캯��

        /// <summary>
        ///     ��������أ��ṩ����صĻ�������
        /// </summary>
        /// <param name="timeoutEvent">��ʱ�¼�</param>
        /// <param name="successEvent">�ɹ��¼�</param>
        /// <param name="failEvent">ʧ���¼�</param>
        /// <param name="maxCount">
        ///     ���������
        ///     <para>* Ĭ��ֵΪ: 30000</para>
        /// </param>
        internal RequestTaskPool(EventHandler successEvent, EventHandler failEvent, EventHandler<LightSingleArgEventArgs<T>> timeoutEvent, int maxCount = 30000)
        {
            if (maxCount <= 0) throw new System.Exception("�Ƿ��������������");
            if (successEvent == null) throw new ArgumentNullException("successEvent");
            if (failEvent == null) throw new ArgumentNullException("failEvent");
            if (timeoutEvent == null) throw new ArgumentNullException("timeoutEvent");
            _successEvent = successEvent;
            _failEvent = failEvent;
            _timeoutEvent = timeoutEvent;
            _maxCount = maxCount;
        }

        #endregion

        #region Members

        private int _maxCount;
        private readonly EventHandler _successEvent;
        private readonly EventHandler _failEvent;
        private readonly EventHandler<LightSingleArgEventArgs<T>> _timeoutEvent;
        private ConcurrentQueue<IRequestTask<T>> _tasks = new ConcurrentQueue<IRequestTask<T>>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RequestTaskPool<T>));

        #endregion

        #region Implementation of IRequestTaskPool<T>

        /// <summary>
        ///     ��ȡ�����õ�ǰ��֧�ֵ������������
        /// </summary>
        public int MaxCount
        {
            get { return _maxCount; }
            set { _maxCount = value; }
        }

        /// <summary>
        ///     ��ʼ��
        /// </summary>
        /// <exception cref="InitializeFailedException">��ʼ��ʧ��</exception>
        public void Initialzie()
        {
            if (_tasks.Count > 0) _tasks = new ConcurrentQueue<IRequestTask<T>>();
            for (int i = 0; i < _maxCount; i++)
            {
                IRequestTask<T> task = new RequestTask<T>();
                task.ExecuteSuccessful += _successEvent;
                task.ExecuteFail += _failEvent;
                task.ExecuteTimeout += _timeoutEvent;
                _tasks.Enqueue(task);
            }
        }

        /// <summary>
        ///     ��һ����������
        /// </summary>
        /// <returns>
        ///     ������������
        /// </returns>
        public IRequestTask<T> Rent()
        {
            try
            {
                IRequestTask<T> task;
                if (_tasks.TryDequeue(out task))
                {
                    task.HasRented = true;
                    return task;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null);
                return null;
            }
        }

        /// <summary>
        ///     �黹һ����������
        /// </summary>
        /// <param name="task">����</param>
        public void Giveback(IRequestTask<T> task)
        {
            task.Reset();
            _tasks.Enqueue(task);
        }

        #endregion
    }
}