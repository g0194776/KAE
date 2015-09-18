using System;
using System.Threading.Tasks;
using KJFramework.TimingJob.EventArgs;

namespace KJFramework.TimingJob.Pools
{
    /// <summary>
    ///    远程数据源接口
    /// </summary>
    public interface IRemoteDataSource
    {
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
        ///    将指定数据发送到远程数据源上
        /// </summary>
        /// <param name="data">需要发送的数据</param>
        /// <param name="args">数据参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="ArgumentException">参数格式错误</exception>
        Task Send(byte[] data, params object[] args);

        #endregion

        #region Events.

        /// <summary>
        ///    接收到数据后的事件通知
        /// </summary>
        event EventHandler<DataRecvEventArgs> DataReceived;

        #endregion
    }
}