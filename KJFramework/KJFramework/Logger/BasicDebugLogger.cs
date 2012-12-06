using System;
using System.Collections.Generic;
using System.IO;
using KJFramework.Basic.Enum;
using KJFramework.Exception;
using KJFramework.Helpers;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     基础的应用程序错误记录器
    /// </summary>
    public class BasicDebugLogger: IDebugLogger<IDebugLog>
    {
        #region 构造函数

        /// <summary>
        ///     基础的应用程序错误记录器
        /// </summary>
        public BasicDebugLogger()
        {
            _debuglist = new SerializableDebugLogList();
        }

        /// <summary>
        ///     基础的应用程序错误记录器
        /// </summary>
        /// <param name="logFilePath" type="string">
        ///     <para>
        ///         日志文件路径
        ///            * 如果给定的日志文件不存在，则会在该位置上新建日志文件。
        ///            * 如果给定的日志文件存在，则会尝试读取该日志文件。
        ///     </para>
        /// </param>
        public BasicDebugLogger(String logFilePath)
        {
            if (logFilePath == null)
            {
                throw new ArgumentNullException();
            }
            _logfilepath = logFilePath;
            if (File.Exists(logFilePath))
            {
                _debuglist = SerializableHelper.DeSerializable<SerializableDebugLogList>(logFilePath, SerializableTypes.DeEncryptd);
                return;
            }
            _debuglist = new SerializableDebugLogList();
            Save();
        }

        #endregion

        #region 成员

        protected SerializableDebugLogList _debuglist;

        #endregion

        #region IDebugLogger<IDebugLog> 成员

        /// <summary>
        ///     将指定记录类型添加到记录集合中
        /// </summary>
        /// <param name="log" type="T">
        ///     <para>
        ///         记录
        ///     </para>
        /// </param>
        public void Log(IDebugLog log)
        {
            if (log == null)
            {
                throw new LogObjectHasNullException();
            }
            if (_debuglist == null)
            {
                _debuglist = new SerializableDebugLogList();
            }
            _debuglist.Logs.Add(log);
            Save();
        }

        /// <summary>
        ///     使用默认的警告等级来记录错误。
        /// </summary>
        /// <param name="e" type="System.Exception">
        ///     <para>
        ///         异常对象
        ///     </para>
        /// </param>
        public void Log(System.Exception e)
        {
            Log(e, DebugGrade.Standard);
        }

        /// <summary>
        ///     使用一个指定的等级来记录错误。
        /// </summary>
        /// <param name="e" type="System.Exception">
        ///     <para>
        ///         异常对象
        ///     </para>
        /// </param>
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         错误等级
        ///     </para>
        /// </param>
        public void Log(System.Exception e, DebugGrade grade)
        {
            IDebugLog debuglog = new BasicDebugLog();
            debuglog.Time = DateTime.Now;
            debuglog.Grade = grade;
            debuglog.DebugMessage = e.Message;
            debuglog.StrackMessage = e.StackTrace;
            debuglog.Location = e.Source;
            debuglog.Name = "应用程序异常";
            debuglog.User = "日志记录用户";
            if (_debuglist == null)
            {
                _debuglist = new SerializableDebugLogList();
            }
            _debuglist.Logs.Add(debuglog);
            Save();
        }

        /// <summary>
        ///     获取具有指定异常等级异常记录集合
        /// </summary>
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         异常等级
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在该等级的异常。
        /// </returns>
        public List<IDebugLog> GetLog(DebugGrade grade)
        {
            List<IDebugLog> logs = null;
            lock (_debuglist.Logs)
            {
                foreach (IDebugLog log in _debuglist.Logs)
                {
                    if (log.Grade == grade)
                    {
                        if (logs == null)
                        {
                            logs = new List<IDebugLog>();
                        }
                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

        /// <summary>
        ///     获取在指定时间范围内并且具有指定异常等级的异常记录
        /// </summary>
        /// <param name="startTime" type="System.DateTime">
        ///     <para>
        ///         起始时间
        ///     </para>
        /// </param>
        /// <param name="endTime" type="System.DateTime">
        ///     <para>
        ///         结束时间
        ///     </para>
        /// </param>
        /// <param name="grade" type="KJFramework.Basic.Enum.DebugGrade">
        ///     <para>
        ///         异常等级
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null, 表示不存在指定时间范围内的异常。
        /// </returns>
        public List<IDebugLog> GetLog(DateTime startTime, DateTime endTime, DebugGrade grade)
        {
            List<IDebugLog> logs = null;
            lock (_debuglist.Logs)
            {
                foreach (IDebugLog log in _debuglist.Logs)
                {
                    if (log.Grade == grade && (log.Time >= startTime && log.Time <= endTime))
                    {
                        if (logs == null)
                        {
                            logs = new List<IDebugLog>();
                        }
                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

        /// <summary>
        ///     使用指定的异常等级，将指定文字添加到记录集合中
        ///         * 记录时会将当前时间自动追加到记录信息头部。
        /// </summary>
        /// <param name="text">要记录的文字</param>
        /// <param name="grade">异常等级</param>
        public void Log(string text, DebugGrade grade)
        {
            IDebugLog debuglog = new BasicDebugLog();
            debuglog.Time = DateTime.Now;
            debuglog.Grade = grade;
            debuglog.DebugMessage = "[" + DateTime.Now + "]  " + text;
            debuglog.StrackMessage = "None";
            debuglog.Location = "None";
            debuglog.Name = "应用程序报告信息";
            debuglog.User = "日志记录用户";
            if (_debuglist == null)
            {
                _debuglist = new SerializableDebugLogList();
            }
            _debuglist.Logs.Add(debuglog);
            Save();
        }

        #endregion

        #region ILogger<IDebugLog> 成员

        private String _logfilepath;

        /// <summary>
        ///     获取或设置记录文件路径
        /// </summary>
        public string LogFilePath
        {
            get
            {
                return _logfilepath;
            }
            set
            {
                _logfilepath = value;
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
            _logfilepath = logFilePath;
            if (_debuglist == null)
            {
                _debuglist = new SerializableDebugLogList();
            }
            if (File.Exists(logFilePath))
            {
                _debuglist = SerializableHelper.DeSerializable<SerializableDebugLogList>(logFilePath, SerializableTypes.DeEncryptd);
            }
            else
            {
                if (!SerializableHelper.Serializable(logFilePath, _debuglist, SerializableTypes.Encrypt))
                {
                    throw new SaveLogFileException();
                }
            }
        }

        /// <summary>
        ///     保存
        /// </summary>
        public void Save()
        {
            if (_logfilepath == null)
            {
                throw new LogFilePathNotFoundException();
            }
            if (_debuglist == null)
            {
                _debuglist = new SerializableDebugLogList();
            }
            if (!SerializableHelper.Serializable(_logfilepath, _debuglist, SerializableTypes.Encrypt))
            {
                throw new SaveLogFileException();
            }
        }

        /// <summary>
        ///     获取所有异常记录
        /// </summary>
        /// <returns>返回异常记录列表</returns>
        public List<IDebugLog> GetLog()
        {
            return _debuglist.Logs;
        }

        /// <summary>
        ///     返回在开始日期到截至日期中存在的异常记录
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns>返回异常记录列表</returns>
        public List<IDebugLog> GetLog(DateTime startTime, DateTime endTime)
        {
            List<IDebugLog> logs = null;
            lock (_debuglist.Logs)
            {
                foreach (IDebugLog log in _debuglist.Logs)
                {
                    if (log.Time >= startTime && log.Time <= endTime)
                    {
                        if (logs == null)
                        {
                            logs = new List<IDebugLog>();
                        }
                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

        #endregion
    }
}
