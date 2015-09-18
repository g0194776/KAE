using System;
using System.Threading.Tasks;
using KJFramework.TimingJob.Enums;
using KJFramework.TimingJob.EventArgs;

namespace KJFramework.TimingJob.Pools
{
    /// <summary>
    ///    任务池接口
    /// </summary>
    /// <typeparam name="T">业务任务类型</typeparam>
    public interface IRemoteTaskPool<T>
    {
        #region Members.

        /// <summary>
        ///    获取任务池的状态
        /// </summary>
        TaskPoolStates State { get; }

        #endregion

        #region Methods.

        /// <summary>
        ///    开始数据接收
        /// </summary>
        void Open();
        /// <summary>
        ///    暂停数据接收
        /// </summary>
        void Pause();
        /// <summary>
        ///    停止数据接收，并回收内部所有资源
        /// </summary>
        void Close();
        /// <summary>
        ///    发送一个本地的任务数据到远程任务池中
        /// </summary>
        /// <param name="content">任务数据</param>
        /// <param name="args">数据参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="InvalidOperationException">操作类型不匹配</exception>
        Task Send(string content, params object[] args);
        /// <summary>
        ///    发送一个本地的任务数据到远程任务池中
        /// </summary>
        /// <param name="task">需要被发送的任务对象</param>
        /// <param name="args">数据参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="InvalidOperationException">操作类型不匹配</exception>
        Task Send(T task, params object[] args);

        #endregion

        #region Events.

        /// <summary>
        ///    接收到数据后的事件通知
        /// </summary>
        event EventHandler<TaskRecvEventArgs<T>> NewTask;

        #endregion
    }
}