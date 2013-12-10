using System;
using System.Collections.Generic;
using KJFramework.Tracing;

namespace KJFramework.Messages.ValueStored.DataProcessor.Mapping
{
    /// <summary>
    ///     可扩展类型处理器映射
    /// </summary>
    public sealed class SystemTypeMapping
    {
        #region Constructor

        /// <summary>
        ///     SystemTypeMapping对象生成器
        /// </summary>
        private SystemTypeMapping()
        {
            Initialize();
        }

        #endregion

        #region Members

        private static readonly Dictionary<byte, BaseValueStored> _valueStoreds = new Dictionary<byte, BaseValueStored>();
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(DataProcessorMapping));

        /// <summary>
        ///      返回一个SystemTypeMapping的实例
        /// </summary>
        public static readonly SystemTypeMapping Instance = new SystemTypeMapping();

        #endregion        
        
        /// <summary>
        ///    初始化
        /// </summary>
        private void Initialize()
        {
            Regist(new BooleanValueStored());
            Regist(new ByteValueStored());
            Regist(new SByteValueStored());
            Regist(new CharValueStored());
            Regist(new Int16ValueStored());
            Regist(new UInt16ValueStored());
            Regist(new Int32ValueStored());
            Regist(new UInt32ValueStored());
            Regist(new Int64ValueStored());
            Regist(new UInt64ValueStored());
            Regist(new FloatValueStored());
            Regist(new DoubleValueStored());
            Regist(new DecimalValueStored());
            Regist(new StringValueStored());
            Regist(new DateTimeValueStored());
            Regist(new GuidValueStored());
            Regist(new IPEndPointValueStored());
            Regist(new IntPtrValueStored());
            Regist(new TimeSpanValueStored());
            Regist(new BitFlagValueStored());
            Regist(new BlobValueStored());
            Regist(new ResourceBlockStored());
            Regist(new NullValueStored());
            Regist(new BooleanArrayValueStored());
            Regist(new ByteArrayValueStored());
            Regist(new SByteArrayValueStored());
            Regist(new CharArrayValueStored());
            Regist(new Int16ArrayValueStored());
            Regist(new UInt16ArrayValueStored());
            Regist(new Int32ArrayValueStored());
            Regist(new UInt32ArrayValueStored());
            Regist(new Int64ArrayValueStored());
            Regist(new UInt64ArrayValueStored());
            Regist(new FloatArrayValueStored());
            Regist(new DoubleArrayValueStored());
            Regist(new DecimalArrayValueStored());
            Regist(new StringArrayValueStored());
            Regist(new DateTimeArrayValueStored());
            Regist(new GuidArrayValueStored());
            Regist(new IPEndPointArrayValueStored());
            Regist(new IntPtrArrayValueStored());
            Regist(new TimeSpanArrayValueStored());
            Regist(new IntellectObjectValueStored());
            Regist(new IntellectObjectArrayValueStored());
            Regist(new ResourceBlockArrayStored());
        }

        /// <summary>
        ///     注册扩展类型数据valueStored
        /// </summary>
        /// <param name="valueStored">数据处理器实例</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        public static void Regist(BaseValueStored valueStored)
        {
            if (valueStored == null) throw new ArgumentNullException("valueStored");
            _valueStoreds[valueStored.TypeId] = valueStored;
        }

        /// <summary>
        ///     返回一个指定扩展类型数据valueStored
        /// </summary>
        /// <param name="typeId">数据类型</param>
        /// <returns>返回一个数组处理器</returns>
        public static BaseValueStored GetValueStored(byte typeId)
        {
            BaseValueStored result;
            return _valueStoreds.TryGetValue(typeId, out result) ? result : null;
        }

        /// <summary>
        ///     注销一个指定扩展类型数据valueStored
        /// </summary>
        /// <param name="typeId">数据类型</param>
        /// <returns>返回一个数组处理器</returns>
        public static void RemoveValueStored(byte typeId)
        {
            try { _valueStoreds.Remove(typeId); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }
    }
}
