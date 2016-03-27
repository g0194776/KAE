using System;
using KJFramework.Enums;

namespace KJFramework.EventArgs
{
    public delegate void DelegateWorkerProcessing(Object sender, WorkerProcessingEventArgs e);
    /// <summary>
    ///     �����߹���״̬�㱨�¼�
    /// </summary>
    public class WorkerProcessingEventArgs : System.EventArgs
    {
        #region ��Ա

        private String _state;
        /// <summary>
        ///     ��ȡ��ǰ�Ĺ���״̬��Ϣ
        /// </summary>
        public string State
        {
            get { return _state; }
        }

        private ProcessingTypes? _processingType;
        /// <summary>
        ///     ��ȡ��Ϣ״̬
        /// </summary>
        public ProcessingTypes? ProcessingType
        {
            get { return _processingType; }
        }


        #endregion

        #region ���캯��

        /// <summary>
        ///     �����߹���״̬�㱨�¼�
        /// </summary>
        /// <param name="state">����״̬��Ϣ</param>
        public WorkerProcessingEventArgs(String state)
        {
            _state = state;
        }

        /// <summary>
        ///     �����߹���״̬�㱨�¼�
        /// </summary>
        /// <param name="processingType">��Ϣ״̬</param>
        public WorkerProcessingEventArgs(ProcessingTypes? processingType)
        {
            _processingType = processingType;
        }

        /// <summary>
        ///     �����߹���״̬�㱨�¼�
        /// </summary>
        /// <param name="state">����״̬��Ϣ</param>
        /// <param name="processingType">��Ϣ״̬</param>
        public WorkerProcessingEventArgs(String state, ProcessingTypes? processingType)
        {
            _state = state;
            _processingType = processingType;
        }

        #endregion
    }
}