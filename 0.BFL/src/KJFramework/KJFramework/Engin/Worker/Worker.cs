using System;
using System.Diagnostics;
using KJFramework.EventArgs;

namespace KJFramework.Engin.Worker
{
    /// <summary>
    ///     ����Ĺ����ߣ���һЩ��Ҫ�ķ����ṩ��ʵ�֡�
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    public abstract class Worker<T> : IWorker<T>
    {
        #region ���캯��

        /// <summary>
        ///     �����ߵĳ���ʵ��
        /// </summary>
        public Worker()
        {
            _state = true;
        }

        #endregion

        #region ��Ա

        private delegate bool DelegateDoWork(T item);

        #endregion

        #region IWorker<T> Members

        private bool _state;
        /// <summary>
        ///     ��ȡ��ǰ�����ߵĹ���״̬
        /// </summary>
        public bool State
        {
            get { return _state; }
        }

        /// <summary>
        ///     ִ�з�����ʹ�õ�ǰ�߳̽��з�����ִ�в�����
        /// </summary>
        /// <param name="item">�������� [ref]</param>
        /// <returns>���ع�����״̬</returns>
        public abstract bool DoWork(T item);

        /// <summary>
        ///     ִ�з������첽���з�����ִ�в�����
        /// </summary>
        /// <param name="item">�������� [ref]</param>
        /// <returns>���ع�����״̬</returns>
        public bool DoWorkAsyn(T item)
        {
            DelegateDoWork delegateDoWork = DoWork;
            try
            {
                IAsyncResult result =  delegateDoWork.BeginInvoke(item, null, null);
                return delegateDoWork.EndInvoke(result);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine("�첽ִ�в���ʧ�� �� " + e.Message);
            }
            return false;
        }

        /// <summary>
        ///     �����߹���״̬�㱨�¼�
        /// </summary>
        public event DelegateWorkerProcessing WorkerProcessing;
        protected void WorkerProcessingHandler(WorkerProcessingEventArgs e)
        {
            DelegateWorkerProcessing processing = WorkerProcessing;
            if (processing != null) processing(this, e);
        }

        /// <summary>
        ///     �����߿�ʼ�����¼�
        /// </summary>
        public event DelegateBeginWork BeginWork;
        protected void BeginWorkHandler(BeginWorkEventArgs e)
        {
            DelegateBeginWork work = BeginWork;
            if (work != null) work(this, e);
        }

        /// <summary>
        ///     ������ֹͣ�����¼�
        /// </summary>
        public event DelegateEndWork EndWork;
        protected void EndWorkHandler(EndWorkEventArgs e)
        {
            DelegateEndWork work = EndWork;
            if (work != null) work(this, e);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}