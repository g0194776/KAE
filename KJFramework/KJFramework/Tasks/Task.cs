using System;
using System.Threading;
using KJFramework.Enums;

namespace KJFramework.Tasks
{
    /// <summary>
    ///      任务抽象类，提供了相关的基本操作。
    /// </summary>
    public abstract class Task : ITask
    {
        #region 析构函数

        ~Task()
        {
            Dispose();
        }

        #endregion

        #region ITask成员

        protected int _id;
        protected bool _isFinished;
        protected bool _isCanceled;
        protected DateTime _createTime = DateTime.Now;
        protected DateTime? _expiredTime;
        protected TaskPriority _priority;
        protected String _description;


        /// <summary>
        ///     获取或设置任务描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        ///     获取或设置任务唯一标示
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前任务是否已经完成
        /// </summary>
        public bool IsFinished
        {
            get { return _isFinished; }
        }

        /// <summary>
        ///     获取一个值，该值表示了当前任务是否已经取消
        /// </summary>
        public bool IsCanceled
        {
            get { return _isCanceled; }
        }

        /// <summary>
        ///     获取任务创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        ///     获取或设置任务过期时间
        ///             * 如果设置为null, 则表示永远不过期
        /// </summary>
        public DateTime? ExpiredTime
        {
            get { return _expiredTime; }
            set { _expiredTime = value; }
        }

        /// <summary>
        ///     获取或设置任务优先级
        /// </summary>
        public TaskPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        /// <summary>
        ///     取消任务
        /// </summary>
        public abstract void Cancel();
        /// <summary>
        ///     执行任务
        /// </summary>
        public abstract void Execute();

        /// <summary>
        ///     异步执行任务
        /// </summary>
        public virtual void ExecuteAsyn()
        {
            ThreadPool.QueueUserWorkItem(Execute);
        }

        public event EventHandler ExecuteSuccessful;
        protected void ExecuteSuccessfulHandler(System.EventArgs e)
        {
            EventHandler handler = ExecuteSuccessful;
            if (handler != null) handler(this, e);
        }

        public event EventHandler ExecuteFail;
        protected void ExecuteFailHandler(System.EventArgs e)
        {
            EventHandler handler = ExecuteFail;
            if (handler != null) handler(this, e);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     内部异步执行方法
        /// </summary>
        /// <param name="arg">线程参数</param>
        protected virtual void Execute(Object arg)
        {
            Execute();
        }

        #endregion

        #region IDispose成员

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