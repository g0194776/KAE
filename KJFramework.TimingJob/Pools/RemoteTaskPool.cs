using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KJFramework.TimingJob.Encoders;
using KJFramework.TimingJob.Enums;
using KJFramework.TimingJob.EventArgs;
using KJFramework.TimingJob.Formatters;
using KJFramework.Tracing;
using Newtonsoft.Json.Linq;

namespace KJFramework.TimingJob.Pools
{
    /// <summary>
    ///    任务池
    /// </summary>
    /// <typeparam name="T">MediaD的业务任务类型</typeparam>
    public class RemoteTaskPool<T> : IRemoteTaskPool<T>
    {
        #region Constructor.

        /// <summary>
        ///    任务池
        /// </summary>
        /// <param name="dataSource">远程数据源</param>
        /// <param name="formatter">任务数据格式化器</param>
        /// <param name="encoder">数据编码器</param>
        /// <param name="poolType">任务池的类型</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public RemoteTaskPool(IRemoteDataSource dataSource, ITaskDataFormatter<T> formatter, IDataEncoder encoder, TaskPoolTypes poolType = TaskPoolTypes.ReadWrite)
        {
            if(dataSource == null) throw new ArgumentNullException("dataSource");
            if (formatter == null) throw new ArgumentNullException("formatter");
            if (encoder == null) throw new ArgumentNullException("encoder");
            _formatter = formatter;
            _encoder = encoder;
            _poolType = poolType;
            _dataSource = dataSource;
        }

        #endregion

        #region Members.

        private int _isOpen;
        private bool _isOk = true;
        private readonly IDataEncoder _encoder;
        private readonly TaskPoolTypes _poolType;
        private readonly object _lockObj = new object();
        private readonly ITaskDataFormatter<T> _formatter;
        private readonly IRemoteDataSource _dataSource;
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(RemoteTaskPool<T>));

        /// <summary>
        ///    获取任务池的状态
        /// </summary>
        public TaskPoolStates State { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///    开始数据接收
        /// </summary>
        public void Open()
        {
            lock (_lockObj)
            {
                if (Interlocked.CompareExchange(ref _isOpen, 1, 0) == 1) return;
                if (_poolType == TaskPoolTypes.ReadOnly || _poolType == TaskPoolTypes.ReadWrite)
                {
                    _dataSource.DataReceived += DataReceived;
                    _dataSource.Open();
                }
            }
        }

        /// <summary>
        ///    暂停数据接收
        /// </summary>
        public void Pause()
        {
            lock (_lockObj)
            {
                if (Interlocked.CompareExchange(ref _isOpen, 0, 1) == 0) return;
                if (_poolType == TaskPoolTypes.ReadOnly || _poolType == TaskPoolTypes.ReadWrite)
                {
                    _dataSource.DataReceived -= DataReceived;
                    _dataSource.Pause();
                }
            }
        }

        /// <summary>
        ///    停止数据接收，并回收内部所有资源
        /// </summary>
        public void Close()
        {
            lock (_lockObj)
            {
                if (Interlocked.CompareExchange(ref _isOpen, 0, 1) == 0) return;
                if (_poolType == TaskPoolTypes.ReadOnly || _poolType == TaskPoolTypes.ReadWrite)
                {
                    _dataSource.DataReceived -= DataReceived;
                    _dataSource.Close();
                }
            }
        }

        /// <summary>
        ///    发送一个本地的任务数据到远程任务池中
        /// </summary>
        /// <param name="content">任务数据</param>
        /// <param name="args">数据参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="InvalidOperationException">操作类型不匹配</exception>
        public Task Send(string content, params object[] args)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException("content");
            if (_poolType == TaskPoolTypes.ReadOnly) throw new InvalidOperationException("#You CANNOT send your message through an *READ-ONLY* remote task pool.");
            byte[] data = Encoding.UTF8.GetBytes(content);
            data = _encoder.Encode(data);
            return _dataSource.Send(data, args);
        }

        /// <summary>
        ///    发送一个本地的任务数据到远程任务池中
        /// </summary>
        /// <param name="task">需要被发送的任务对象</param>
        /// <param name="args">数据参数</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="InvalidOperationException">操作类型不匹配</exception>
        public Task Send(T task, params object[] args)
        {
            if (task == null) throw new ArgumentNullException("task");
            JObject obj = new JObject();
            if (!_formatter.TrySerialize(obj, task)) return null;
            return Send(obj.ToString(), args);
        }

        #endregion

        #region Events.

        /// <summary>
        ///    接收到数据后的事件通知
        /// </summary>
        public event EventHandler<TaskRecvEventArgs<T>> NewTask;
        protected virtual void OnNewTask(TaskRecvEventArgs<T> e)
        {
            EventHandler<TaskRecvEventArgs<T>> handler = NewTask;
            if (handler != null) handler(this, e);
        }

        //Event callback proc for receiving specified binary message data.
        void DataReceived(object sender, DataRecvEventArgs e)
        {
            try
            {
                T task;
                if (_formatter.TryParse((JObject)e.Data, out task)) OnNewTask(new TaskRecvEventArgs<T>(task));
            }
            catch (Exception ex)
            {
                _tracing.Error("Error", ex);
            }
        }

        #endregion
    }
}