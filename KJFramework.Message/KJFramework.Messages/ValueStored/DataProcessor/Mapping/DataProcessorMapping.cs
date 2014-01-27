using System;
using System.Collections.Generic;
using KJFramework.Messages.Enums;
using KJFramework.Tracing;

namespace KJFramework.Messages.ValueStored.DataProcessor.Mapping
{
    /// <summary>
    ///     DataProcessor对象生成器
    /// </summary>
    public sealed class DataProcessorMapping
    {
        #region Constructor

        /// <summary>
        ///     DataProcessor对象生成器
        /// </summary>
        private DataProcessorMapping()
        {
            Initialize();
        }

        #endregion

        #region Members

        private readonly Dictionary<PropertyTypes, IDataProcessor> _processors = new Dictionary<PropertyTypes, IDataProcessor>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DataProcessorMapping));

        /// <summary>
        ///      返回一个DataProcessor的实例
        /// </summary>
        public static readonly DataProcessorMapping Instance = new DataProcessorMapping();

        #endregion  

        #region Methods

        /// <summary>
        ///    初始化
        /// </summary>
        private void Initialize()
        {
            Regist(new BooleanArrayDataProcessor());
            Regist(new CharArrayDataProcessor());
            Regist(new ByteArrayDataProcessor());
            Regist(new SByteArrayDataProcessor());
            Regist(new Int16ArrayDataProcessor());
            Regist(new UInt16ArrayDataProcessor());
            Regist(new Int32ArrayDataProcessor());
            Regist(new UInt32ArrayDataProcessor());
            Regist(new Int64ArrayDataProcessor());
            Regist(new UInt64ArrayDataProcessor());
            Regist(new FloatArrayDataProcessor());
            Regist(new DoubleArrayDataProcessor());
            Regist(new DecimalArrayDataProcessor());
            Regist(new DateTimeArrayDataProcessor());
            Regist(new GuidArrayDataProcessor());
            Regist(new IntPtrArrayDataProcessor());
            Regist(new TimeSpanArrayDataProcessor());
            Regist(new IPEndPointArrayDataProcessor());
            Regist(new StringArrayDataProcessor());
            Regist(new IntellectObjectDataProcessor());
            Regist(new IntellectObjectArrayDataProcessor());
            Regist(new ResourceBlockArrayDataProcessor());
        }

        /// <summary>
        ///     注册数组数据处理器
        /// </summary>
        /// <param name="processor">数据处理器实例</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public void Regist(IDataProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");
            _processors[processor.TypeId] = processor;
        }

        /// <summary>
        ///     返回一个指定数组的数据处理器
        /// </summary>
        /// <param name="typeId">数据处理器类型</param>
        /// <returns>返回一个数组处理器</returns>
        public IDataProcessor GetProcessor(PropertyTypes typeId)
        {
            IDataProcessor result;
            return _processors.TryGetValue(typeId, out result) ? result : null;
        }

        #endregion
    }
}
