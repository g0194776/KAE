using System;
using System.Collections.Generic;
using System.Linq;
using KJFramework.Tracing;

namespace KJFramework.ApplicationEngine.Loggers
{
    /// <summary>
    ///     KAE����״̬��¼��
    /// </summary>
    internal class KAEStateLogger : IKAEStateLogger
    {
        #region Constructor.

        /// <summary>
        ///     KAE����״̬��¼��
        /// </summary>
        /// <param name="tracing">������־��¼��</param>
        /// <param name="maximumLogCount">�ڲ����ܹ���������־�������ֵ�����������Ŀ����ʷ���ݽ����Զ���ʧ</param>
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
        ///     ��ȡ�������ڲ����ܹ���������־�������ֵ�����������Ŀ����ʷ���ݽ����Զ���ʧ
        /// </summary>
        public int MaximumLogCount
        {
            get { return _maximumLogCount; }
        }

        #endregion

        /// <summary>
        ///     ��¼һ��״̬��Ϣ
        /// </summary>
        /// <param name="content">��Ҫ����¼��״̬��Ϣ����</param>
        /// <exception cref="ArgumentNullException">��������Ϊ��</exception>
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
        ///    �����ڲ�������������״̬��Ϣ
        /// </summary>
        /// <returns>���ذ���������</returns>
        public List<string> GetAllLogs()
        {
            lock (_lockObj) return _content.ToList();
        }
    }
}