using System;
using KJFramework.Enums;

namespace KJFramework.EventArgs
{
    public delegate void DelegateWorkerProcessing(Object sender, WorkerProcessingEventArgs e);
    /// <summary>
    ///     工作者工作状态汇报事件
    /// </summary>
    public class WorkerProcessingEventArgs : System.EventArgs
    {
        #region 成员

        private String _state;
        /// <summary>
        ///     获取当前的工作状态信息
        /// </summary>
        public string State
        {
            get { return _state; }
        }

        private ProcessingTypes? _processingType;
        /// <summary>
        ///     获取信息状态
        /// </summary>
        public ProcessingTypes? ProcessingType
        {
            get { return _processingType; }
        }


        #endregion

        #region 构造函数

        /// <summary>
        ///     工作者工作状态汇报事件
        /// </summary>
        /// <param name="state">工作状态信息</param>
        public WorkerProcessingEventArgs(String state)
        {
            _state = state;
        }

        /// <summary>
        ///     工作者工作状态汇报事件
        /// </summary>
        /// <param name="processingType">信息状态</param>
        public WorkerProcessingEventArgs(ProcessingTypes? processingType)
        {
            _processingType = processingType;
        }

        /// <summary>
        ///     工作者工作状态汇报事件
        /// </summary>
        /// <param name="state">工作状态信息</param>
        /// <param name="processingType">信息状态</param>
        public WorkerProcessingEventArgs(String state, ProcessingTypes? processingType)
        {
            _state = state;
            _processingType = processingType;
        }

        #endregion
    }
}