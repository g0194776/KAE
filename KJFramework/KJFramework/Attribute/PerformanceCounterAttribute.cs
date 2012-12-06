using System;
using System.Diagnostics;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     ���ܼ�������ǩ�����ڱ�ǵ�ǰ������Ҫ�����ܼ�����
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PerformanceCounterAttribute : System.Attribute
    {
        #region Constructor

        /// <summary>
        ///     ���ܼ�������ǩ�����ڱ�ǵ�ǰ������Ҫ�����ܼ�����
        ///     <para>* ʹ�ô˹��콫�ᴴ��һ��ϵͳ�������͵����ܼ�����</para>
        /// </summary>
        /// <param name="id">���ܼ�������ţ���ͨ���˱������ȡ��Ӧ�����ܼ�����</param>
        /// <param name="categoryName">�����ܼ��������������ܼ�����������ܶ��󣩵����ơ�</param>
        /// <param name="counterName">���ܼ����������ơ�</param>
        public PerformanceCounterAttribute(int id, string categoryName, string counterName)
            : this(id, categoryName, counterName, false)
        {

        }

        /// <summary>
        ///     ���ܼ�������ǩ�����ڱ�ǵ�ǰ������Ҫ�����ܼ�����
        ///     <para>* ʹ�ô˹��콫�ᴴ��һ��ϵͳ�������͵����ܼ�����</para>
        /// </summary>
        /// <param name="id">���ܼ�������ţ���ͨ���˱������ȡ��Ӧ�����ܼ�����</param>
        /// <param name="categoryName">�����ܼ��������������ܼ�����������ܶ��󣩵����ơ�</param>
        /// <param name="counterName">���ܼ����������ơ�</param>
        /// <param name="readOnly">��Ҫ��ֻ��ģʽ���ʼ���������Ϊ true����Ҫ�Զ�/дģʽ���ʼ���������Ϊ false��</param>
        public PerformanceCounterAttribute(int id, string categoryName, string counterName, bool readOnly)
        {
            _id = id;
            _categoryName = categoryName;
            _counterName = counterName;
            _readOnly = readOnly;
            _isSystemCounter = true;
        }

        /// <summary>
        ///     ���ܼ�������ǩ�����ڱ�ǵ�ǰ������Ҫ�����ܼ�����
        ///     <para>* ʹ�ô˹��콫�ᴴ��һ���Զ�������ܼ�����</para>
        /// </summary>
        /// <param name="id">���ܼ�������ţ���ͨ���˱������ȡ��Ӧ�����ܼ�����</param>
        /// <param name="counterName">���ܼ���������</param>
        /// <param name="counterType">���ܼ���������</param>
        public PerformanceCounterAttribute(int id, string counterName, PerformanceCounterType counterType)
            : this(id, counterName, string.Empty, counterType)
        {

        }

        /// <summary>
        ///     ���ܼ�������ǩ�����ڱ�ǵ�ǰ������Ҫ�����ܼ�����
        ///     <para>* ʹ�ô˹��콫�ᴴ��һ���Զ�������ܼ�����</para>
        /// </summary>
        /// <param name="id">���ܼ�������ţ���ͨ���˱������ȡ��Ӧ�����ܼ�����</param>
        /// <param name="counterName">���ܼ���������</param>
        /// <param name="counterHelp">���ܼ�����������Ϣ</param>
        /// <param name="counterType">���ܼ���������</param>
        public PerformanceCounterAttribute(int id, string counterName, string counterHelp, PerformanceCounterType counterType)
        {
            _id = id;
            _counterName = counterName;
            _counterHelp = counterHelp;
            _counterType = counterType;
            _isSystemCounter = false;
        }

        #endregion

        #region Members

        private string _categoryName;
        private string _counterName;
        private bool _readOnly;
        private int _id;
        private bool _isSystemCounter;
        private string _counterHelp;
        private PerformanceCounterType _counterType;

        /// <summary>
        ///     ��ȡ�����÷�������
        /// </summary>
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        ///     ��ȡ���������ܼ���������
        /// </summary>
        public string CounterName
        {
            get { return _counterName; }
            set { _counterName = value; }
        }

        /// <summary>
        ///     ��ȡ������һ��ֵ�� ��ֵ��ʾ�˵�ǰ���ܼ������Ƿ�Ϊֻ��
        /// </summary>
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        ///     ��ȡһ��ֵ, ��ֵ��ʾ�˵�ǰ�ļ�������ǩ�Ƿ������һ��ϵͳ���͵����ܼ����� 
        /// </summary>
        internal bool IsSystemCounter
        {
            get { return _isSystemCounter; }
        }

        /// <summary>
        ///     ��ȡ�Զ�������ܼ��������
        /// </summary>
        public PerformanceCounterType CounterType
        {
            get { return _counterType; }
        }

        /// <summary>
        ///     ��ȡ�Զ�������ܼ�����������Ϣ
        /// </summary>
        public string CounterHelp
        {
            get { return _counterHelp; }
        }

        #endregion
    }
}