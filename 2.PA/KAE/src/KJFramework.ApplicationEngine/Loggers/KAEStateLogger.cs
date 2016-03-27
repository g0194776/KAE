using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Loggers
{
    /// <summary>
    ///     KAE宿主状态记录器
    /// </summary>
    internal class KAEStateLogger : IKAEStateLogger
    {
        #region Constructor.

        /// <summary>
        ///     KAE宿主状态记录器
        /// </summary>
        /// <param name="tracing">本地日志记录器</param>
        /// <param name="maximumLogCount">内部所能够包含的日志条数最大值，超过这个数目的历史数据将会自动消失</param>
        public KAEStateLogger(ITracing tracing, int maximumLogCount = 1024)
        {
            if (tracing == null) throw new ArgumentNullException("tracing");
            _tracing = tracing;
            _maximumLogCount = maximumLogCount;
            _content = new Queue<string>(maximumLogCount); 
        }

        #endregion

        #region Members.

        private readonly ITracing _tracing;
        private readonly int _maximumLogCount;
        private readonly Queue<string> _content;
        private readonly object _lockObj = new object();

        /// <summary>
        ///     获取或设置内部所能够包含的日志条数最大值，超过这个数目的历史数据将会自动消失
        /// </summary>
        public int MaximumLogCount
        {
            get { return _maximumLogCount; }
        }

        #endregion

        /// <summary>
        ///     记录一条状态信息
        /// </summary>
        /// <param name="content">需要被记录的状态信息内容</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Log(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            string realContent = string.Format("[{0}] {1}", DateTime.Now, content);
            lock (_lockObj)
            {
                if (_content.Count >= _maximumLogCount) 
                    while (_content.Count >= _maximumLogCount) { _content.Dequeue(); }
                _content.Enqueue(realContent);
            }
        }

        /// <summary>
        ///    返回内部所包含的所有状态信息
        /// </summary>
        /// <returns>返回包含的数据</returns>
        public List<string> GetAllLogs()
        {
            lock (_lockObj) return _content.ToList();
        }
    }
}