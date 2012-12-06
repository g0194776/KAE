using System;
using System.Diagnostics;

namespace KJFramework.Attribute
{
    /// <summary>
    ///     性能计数器标签，用于标记当前类所需要的性能计数器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PerformanceCounterAttribute : System.Attribute
    {
        #region Constructor

        /// <summary>
        ///     性能计数器标签，用于标记当前类所需要的性能计数器
        ///     <para>* 使用此构造将会创建一个系统内置类型的性能计数器</para>
        /// </summary>
        /// <param name="id">性能计数器编号，可通过此编号来获取相应的性能计数器</param>
        /// <param name="categoryName">此性能计数器关联的性能计数器类别（性能对象）的名称。</param>
        /// <param name="counterName">性能计数器的名称。</param>
        public PerformanceCounterAttribute(int id, string categoryName, string counterName)
            : this(id, categoryName, counterName, false)
        {

        }

        /// <summary>
        ///     性能计数器标签，用于标记当前类所需要的性能计数器
        ///     <para>* 使用此构造将会创建一个系统内置类型的性能计数器</para>
        /// </summary>
        /// <param name="id">性能计数器编号，可通过此编号来获取相应的性能计数器</param>
        /// <param name="categoryName">此性能计数器关联的性能计数器类别（性能对象）的名称。</param>
        /// <param name="counterName">性能计数器的名称。</param>
        /// <param name="readOnly">若要以只读模式访问计数器，则为 true；若要以读/写模式访问计数器，则为 false。</param>
        public PerformanceCounterAttribute(int id, string categoryName, string counterName, bool readOnly)
        {
            _id = id;
            _categoryName = categoryName;
            _counterName = counterName;
            _readOnly = readOnly;
            _isSystemCounter = true;
        }

        /// <summary>
        ///     性能计数器标签，用于标记当前类所需要的性能计数器
        ///     <para>* 使用此构造将会创建一个自定义的性能计数器</para>
        /// </summary>
        /// <param name="id">性能计数器编号，可通过此编号来获取相应的性能计数器</param>
        /// <param name="counterName">性能计数器名称</param>
        /// <param name="counterType">性能计数器类型</param>
        public PerformanceCounterAttribute(int id, string counterName, PerformanceCounterType counterType)
            : this(id, counterName, string.Empty, counterType)
        {

        }

        /// <summary>
        ///     性能计数器标签，用于标记当前类所需要的性能计数器
        ///     <para>* 使用此构造将会创建一个自定义的性能计数器</para>
        /// </summary>
        /// <param name="id">性能计数器编号，可通过此编号来获取相应的性能计数器</param>
        /// <param name="counterName">性能计数器名称</param>
        /// <param name="counterHelp">性能计数器帮助信息</param>
        /// <param name="counterType">性能计数器类型</param>
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
        ///     获取或设置分组名称
        /// </summary>
        public string CategoryName
        {
            get { return _categoryName; }
            set { _categoryName = value; }
        }

        /// <summary>
        ///     获取或设置性能计数器名称
        /// </summary>
        public string CounterName
        {
            get { return _counterName; }
            set { _counterName = value; }
        }

        /// <summary>
        ///     获取或设置一个值， 该值标示了当前性能计数器是否为只读
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
        ///     获取一个值, 该值表示了当前的计数器标签是否代表了一个系统类型的性能计数器 
        /// </summary>
        internal bool IsSystemCounter
        {
            get { return _isSystemCounter; }
        }

        /// <summary>
        ///     获取自定义的性能计数器类别
        /// </summary>
        public PerformanceCounterType CounterType
        {
            get { return _counterType; }
        }

        /// <summary>
        ///     获取自定义的性能计数器帮助信息
        /// </summary>
        public string CounterHelp
        {
            get { return _counterHelp; }
        }

        #endregion
    }
}