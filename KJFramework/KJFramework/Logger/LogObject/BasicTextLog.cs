using System;
using System.Text;
using System.Threading;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     基础的文字异常日志项，提供了相关的基本操作。
    ///            * 此日志项输出格式已经间接的支持了“日志查看器”。
    /// </summary>
    public sealed class BasicTextLog : ITextLog
    {
        #region 构造函数

        /// <summary>
        ///     基础的文字异常日志项，提供了相关的基本操作。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="debugGrade">异常等级</param>
        public BasicTextLog(System.Exception exception, DebugGrade debugGrade)
            : this(exception, debugGrade, null)
        {}

        /// <summary>
        ///     基础的文字异常日志项，提供了相关的基本操作。
        /// </summary>
        /// <param name="exception">异常对象</param>
        /// <param name="debugGrade">异常等级</param>
        /// <param name="format">记录项格式</param>
        public BasicTextLog(System.Exception exception, DebugGrade debugGrade, ITextLogFormatter format)
        {
            _time = DateTime.Now;
            _debugMessage = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
            _strackMessage = exception.InnerException != null ? exception.InnerException.StackTrace : exception.StackTrace;
            _location = exception.Source;
            _grade = debugGrade;
            _formatter = format;
        }

        #endregion

        #region ITextLog Members

        /// <summary>
        ///     获取当前要写入日志的内容。
        ///         * [注] 这里可以直接返回带有格式的日志内容。
        /// </summary>
        /// <returns>返回日志的内容</returns>
        public String GetLogContent()
        {
            StringBuilder headBuilder = new StringBuilder();
            headBuilder.Append(String.Format("<<exception level=\"{0}\" time=\"{1}\" >>\r\n", GetGradeText(), _time));
            headBuilder.Append("日志等级    ： " + GetGradeText() + "\r\n");
            headBuilder.Append("异常位置    ： " + _location + "\r\n");
            headBuilder.Append("出现时间    ： " + _time + "\r\n");
            headBuilder.Append("线程编号    ： " + Thread.CurrentThread.ManagedThreadId.ToString() + "\r\n");
            headBuilder.Append("异常信息    ： " + _debugMessage + "\r\n");
            headBuilder.Append("堆栈信息    ： " + _strackMessage + "\r\n");
            headBuilder.Append("<</exception>>");
            return headBuilder.ToString();
        }

        /// <summary>
        ///     获取日志头部信息
        /// </summary>
        /// <returns>返回头部信息</returns>
        public string GetHead()
        {
            StringBuilder headBuilder = new StringBuilder();
            headBuilder.Append(String.Format("<<report time=\"{0}\">>\r\n", DateTime.Now));
            headBuilder.Append("<<head>>\r\n");
            headBuilder.Append("*******************************************************************");
            headBuilder.Append("\r\n");
            headBuilder.Append("* 应用程序异常日志");
            headBuilder.Append("\r\n");
            headBuilder.Append("* 日志记录人 ： " + _name + "\r\n");
            headBuilder.Append("* 记录时间    ： " + _time + "\r\n");
            headBuilder.Append("*******************************************************************\r\n");
            headBuilder.Append("<</head>>");
            headBuilder.Append("\r\n");
            return headBuilder.ToString();
        }

        private bool _isHead;
        /// <summary>
        ///     获取或设置当前记录项是否当作头部记录项。
        /// </summary>
        public bool IsHead
        {
            get
            {
                return _isHead;
            }
            set
            {
                _isHead = value;
            }
        }

        private ITextLogFormatter _formatter;
        /// <summary>
        ///     获取或设置日志记录项格式
        /// </summary>
        public ITextLogFormatter Formatter
        {
            get
            {
                return _formatter;
            }
            set
            {
                _formatter = value;
            }
        }

        #endregion

        #region IDebugLog Members

        private String _strackMessage;
        /// <summary>
        ///     堆栈消息
        /// </summary>
        public String StrackMessage
        {
            get
            {
                return _strackMessage;
            }
            set
            {
                _strackMessage = value;
            }
        }

        private String _debugMessage;
        /// <summary>
        ///     调试消息
        /// </summary>
        public String DebugMessage
        {
            get
            {
                return _debugMessage;
            }
            set
            {
                _debugMessage = value;
            }
        }

        private String _location;
        /// <summary>
        ///     异常位置
        /// </summary>
        public String Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        private DebugGrade _grade;
        /// <summary>
        ///     异常等级
        /// </summary>
        public DebugGrade Grade
        {
            get
            {
                return _grade;
            }
            set
            {
                _grade = value;
            }
        }

        #endregion

        #region ILog Members

        private DateTime _time;

        /// <summary>
        ///     获取或设置记录具体日期事件
        /// </summary>
        public DateTime Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        private String _name;

        /// <summary>
        ///     获取或设置记录名称
        /// </summary>
        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private String _user;

        /// <summary>
        ///     获取或设置记录人
        /// </summary>
        public String User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        /// <summary>
        ///     获取该记录对象的类型
        /// </summary>
        public LogTypes LogType
        {
            get { return LogTypes.ApplicationDebug; }
        }

        private String _tag;

        /// <summary>
        ///     获取或设置记录辅助数据
        /// </summary>
        public String Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///     根据等级枚举值返回相应的等级描述文字
        /// </summary>
        /// <returns>返回等级描述文字</returns>
        private String GetGradeText()
        {
            String grade = String.Empty;
            switch (_grade)
            {
                case DebugGrade.Fatal:
                    grade = "致命";
                    break;
                case DebugGrade.High:
                    grade = "高";
                    break;
                case DebugGrade.Low:
                    grade = "低";
                    break;
                case DebugGrade.Standard:
                    grade = "普通";
                    break;
            }
            return grade;
        }

        #endregion
    }
}