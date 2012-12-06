using System;
using System.Text;
using System.Threading;
using KJFramework.Basic.Enum;

namespace KJFramework.Logger.LogObject
{
    /// <summary>
    ///     �����������쳣��־��ṩ����صĻ���������
    ///            * ����־�������ʽ�Ѿ���ӵ�֧���ˡ���־�鿴������
    /// </summary>
    public sealed class BasicTextLog : ITextLog
    {
        #region ���캯��

        /// <summary>
        ///     �����������쳣��־��ṩ����صĻ���������
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="debugGrade">�쳣�ȼ�</param>
        public BasicTextLog(System.Exception exception, DebugGrade debugGrade)
            : this(exception, debugGrade, null)
        {}

        /// <summary>
        ///     �����������쳣��־��ṩ����صĻ���������
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="debugGrade">�쳣�ȼ�</param>
        /// <param name="format">��¼���ʽ</param>
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
        ///     ��ȡ��ǰҪд����־�����ݡ�
        ///         * [ע] �������ֱ�ӷ��ش��и�ʽ����־���ݡ�
        /// </summary>
        /// <returns>������־������</returns>
        public String GetLogContent()
        {
            StringBuilder headBuilder = new StringBuilder();
            headBuilder.Append(String.Format("<<exception level=\"{0}\" time=\"{1}\" >>\r\n", GetGradeText(), _time));
            headBuilder.Append("��־�ȼ�    �� " + GetGradeText() + "\r\n");
            headBuilder.Append("�쳣λ��    �� " + _location + "\r\n");
            headBuilder.Append("����ʱ��    �� " + _time + "\r\n");
            headBuilder.Append("�̱߳��    �� " + Thread.CurrentThread.ManagedThreadId.ToString() + "\r\n");
            headBuilder.Append("�쳣��Ϣ    �� " + _debugMessage + "\r\n");
            headBuilder.Append("��ջ��Ϣ    �� " + _strackMessage + "\r\n");
            headBuilder.Append("<</exception>>");
            return headBuilder.ToString();
        }

        /// <summary>
        ///     ��ȡ��־ͷ����Ϣ
        /// </summary>
        /// <returns>����ͷ����Ϣ</returns>
        public string GetHead()
        {
            StringBuilder headBuilder = new StringBuilder();
            headBuilder.Append(String.Format("<<report time=\"{0}\">>\r\n", DateTime.Now));
            headBuilder.Append("<<head>>\r\n");
            headBuilder.Append("*******************************************************************");
            headBuilder.Append("\r\n");
            headBuilder.Append("* Ӧ�ó����쳣��־");
            headBuilder.Append("\r\n");
            headBuilder.Append("* ��־��¼�� �� " + _name + "\r\n");
            headBuilder.Append("* ��¼ʱ��    �� " + _time + "\r\n");
            headBuilder.Append("*******************************************************************\r\n");
            headBuilder.Append("<</head>>");
            headBuilder.Append("\r\n");
            return headBuilder.ToString();
        }

        private bool _isHead;
        /// <summary>
        ///     ��ȡ�����õ�ǰ��¼���Ƿ���ͷ����¼�
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
        ///     ��ȡ��������־��¼���ʽ
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
        ///     ��ջ��Ϣ
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
        ///     ������Ϣ
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
        ///     �쳣λ��
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
        ///     �쳣�ȼ�
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
        ///     ��ȡ�����ü�¼���������¼�
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
        ///     ��ȡ�����ü�¼����
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
        ///     ��ȡ�����ü�¼��
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
        ///     ��ȡ�ü�¼���������
        /// </summary>
        public LogTypes LogType
        {
            get { return LogTypes.ApplicationDebug; }
        }

        private String _tag;

        /// <summary>
        ///     ��ȡ�����ü�¼��������
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

        #region ����

        /// <summary>
        ///     ���ݵȼ�ö��ֵ������Ӧ�ĵȼ���������
        /// </summary>
        /// <returns>���صȼ���������</returns>
        private String GetGradeText()
        {
            String grade = String.Empty;
            switch (_grade)
            {
                case DebugGrade.Fatal:
                    grade = "����";
                    break;
                case DebugGrade.High:
                    grade = "��";
                    break;
                case DebugGrade.Low:
                    grade = "��";
                    break;
                case DebugGrade.Standard:
                    grade = "��ͨ";
                    break;
            }
            return grade;
        }

        #endregion
    }
}