using System;
using System.IO;
using KJFramework.Basic.Enum;
using KJFramework.Exception;
using KJFramework.Logger.LogObject;

namespace KJFramework.Logger
{
    /// <summary>
    ///     ���������ּ�¼�����ṩ����صĻ���������
    /// </summary>
    /// <typeparam name="T">������־��¼������</typeparam>
    public sealed class BasicTextLogger<T> : ITextLogger<T>
        where T : ITextLog
    {
        #region ��Ա

        private StreamWriter _streamWriter;
        private bool _isHead = true;
        private Object _obj = new Object();
        private DateTime _currentLogTime;

        #endregion

        #region ���캯��

        /// <summary>
        ///     ���������ּ�¼�����ṩ����صĻ���������
        /// </summary>
        /// <param name="logFilePath">��־�ļ���ַ</param>
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
        ///     ��ȡ�����ü�¼�ļ�·��
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
        ///     ��ȡ�����ü�¼���Ŀ���״̬
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
        ///     ʹ��һ����־�ļ�����ʼ����־��¼��, ������ļ������ڣ����Զ�������
        /// </summary>
        /// <param name="logFilePath" type="string">
        ///     <para>
        ///         ��־�ļ�ȫ·��
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
        ///     ����
        /// </summary>
        public void Save()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Flush();
            }
        }

        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        /// </summary>
        /// <param name="log" type="T">
        ///     <para>
        ///         ��¼
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
                    //Ĭ��д��һ�κ���Զ�����
                    Save();
                }
            }
        }


        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        ///           *  ʹ�ô˷�������¼���쳣���ȵȼ�Ĭ��Ϊ����ͨ
        /// </summary>
        /// <param name="exception">�쳣����</param>
        public void Log(System.Exception exception)
        {
            Log(exception, DebugGrade.Standard, Logs.Name);
        }

        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        public void Log(System.Exception exception, DebugGrade grade)
        {
            Log(exception, grade, Logs.Name);
        }

        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        ///           *  ��¼�����Զ��ж��Ƿ�Ϊͷ����ʾ��
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        /// <param name="name">
        ///     ��¼��
        ///         * [ע] �� ���������ͷ���ã���������Ϊnull
        /// </param>
        public void Log(System.Exception exception, DebugGrade grade, string name)
        {
            Log(exception, grade, _isHead, name);
            _isHead = false;
        }

        /// <summary>
        ///     ��ָ����¼������ӵ���¼������
        /// </summary>
        /// <param name="exception">�쳣����</param>
        /// <param name="grade">�쳣�ȼ�</param>
        /// <param name="isHead">ͷ��ʾ</param>
        /// <param name="name">
        ///     ��¼��
        ///         * [ע] �� ���������ͷ���ã���������Ϊnull
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
        ///     ��ָ���������ݼ�¼����¼������
        /// </summary>
        /// <param name="text">��������</param>
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
        ///     ���ٵ�ǰ��¼��
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