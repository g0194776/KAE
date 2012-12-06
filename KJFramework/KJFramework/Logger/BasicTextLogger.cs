using System;
using System.IO;
using KJFramework.Basic.Enum;
using KJFramework.Exception;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     基础的文字记录器，提供了相关的基本操作。
    /// </summary>
    /// <typeparam name="T">文字日志记录项类型</typeparam>
    public sealed class BasicTextLogger<T> : ITextLogger<T>
        where T : ITextLog
    {
        #region 成员

        private StreamWriter _streamWriter;
        private bool _isHead = true;
        private Object _obj = new Object();
        private DateTime _currentLogTime;

        #endregion

        #region 构造函数

        /// <summary>
        ///     基础的文字记录器，提供了相关的基本操作。
        /// </summary>
        /// <param name="logFilePath">日志文件地址</param>
        public BasicTextLogger(String logFilePath)
        {
            _currentLogTime = DateTime.Now;
            _enable = true;
            if (String.IsNullOrEmpty(logFilePath))
            {
                throw new LogFilePathUnCorrectException();
            }
            _logFilePath = logFilePath;
            Initialize(_logFilePath);
        }

        #endregion

        #region ILogger<T> Members

        private String _logFilePath;
        /// <summary>
        ///     获取或设置记录文件路径
        /// </summary>
        public String LogFilePath
        {
            get
            {
                return _logFilePath;
            }
            set
            {
                _logFilePath = value;
            }
        }

        private bool _enable;
        /// <summary>
        ///     获取或设置记录器的可用状态
        /// </summary>
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
            }
        }

        /// <summary>
        ///     使用一个日志文件来初始化日志记录器, 如果该文件不存在，则自动创建。
        /// </summary>
        /// <param name="logFilePath" type="string">
        ///     <para>
        ///         日志文件全路径
        ///     </para>
        /// </param>
        public void Initialize(string logFilePath)
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter = null;
            }
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }
            if (_streamWriter == null)
            {
                _streamWriter = new StreamWriter(String.Format("{0}{1}.{2}", logFilePath, DateTime.Now.ToString("yyyy-MMdd-hh"), "Log"), true);
            }
        }

        /// <summary>
        ///     保存
        /// </summary>
        public void Save()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Flush();
            }
        }

        /// <summary>
        ///     将指定记录类型添加到记录集合中
        /// </summary>
        /// <param name="log" type="T">
        ///     <para>
        ///         记录
        ///     </para>
        /// </param>
        public void Log(T log)
        {
            lock (_obj)
            {
                if (DateTime.Now.Hour != _currentLogTime.Hour)
                {
                    Initialize(_logFilePath);
                }
                if (!Equals(log, default(T)))
                {
                    if (log.IsHead)
                    {
                        _streamWriter.WriteLine(log.GetHead());
                    }
                    if (log.Formatter != null)
                    {
                        _streamWriter.WriteLine(log.Formatter.Up);
                        _streamWriter.Write(log.Formatter.LeftSplitChar);
                    }
                    _streamWriter.WriteLine(log.GetLogContent());
                    if (log.Formatter != null)
                    {
                        _streamWriter.WriteLine(log.Formatter.Down);
                    }
                    //默认写入一次后就自动保存
                    Save();
                }
            }
        }


        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        ///           *  使用此方法，记录的异常优先等级默认为：普通
        /// </summary>
        /// <param name="exception">异常对象</param>
        public void Log(System.Exception exception)
        {
            Log(exception, DebugGrade.Standard, Logs.Name);
        }

        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        public void Log(System.Exception exception, DebugGrade grade)
        {
            Log(exception, grade, Logs.Name);
        }

        /// <summary>
        ///     将指定记录类型添加到记录集合中
        ///           *  记录器会自动判断是否为头部标示。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        /// <param name="name">
        ///     记录人
        ///         * [注] ： 如果不当作头来用，可以设置为null
        /// </param>
        public void Log(System.Exception exception, DebugGrade grade, string name)
        {
            Log(exception, grade, _isHead, name);
            _isHead = false;
        }

        /// <summary>
        ///     将指定记录类型添加到记录集合中
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="grade">异常等级</param>
        /// <param name="isHead">头标示</param>
        /// <param name="name">
        ///     记录人
        ///         * [注] ： 如果不当作头来用，可以设置为null
        /// </param>
        public void Log(System.Exception exception, DebugGrade grade, bool isHead, String name)
        {
            ITextLog textLog = new BasicTextLog(exception, grade);
            textLog.IsHead = isHead;
            if (isHead)
            {
                textLog.Name = name;
            }
            Log((T)textLog);
        }

        /// <summary>
        ///     将指定文字内容记录到记录集合中
        /// </summary>
        /// <param name="text">文字内容</param>
        public void Log(string text)
        {
            if (!String.IsNullOrEmpty(text) && _streamWriter != null)
            {
                if (DateTime.Now.Hour != _currentLogTime.Hour)
                {
                    Initialize(_logFilePath);
                }
                _streamWriter.WriteLine("<<common>>" + DateTime.Now.ToString() + "           " + text);
                _streamWriter.Flush();
                Save();
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        ///     销毁当前记录器
        /// </summary>
        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Flush();
                _streamWriter.Close();
            }
            _streamWriter = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}