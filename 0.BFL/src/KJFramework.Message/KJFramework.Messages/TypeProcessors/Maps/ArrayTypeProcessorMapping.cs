using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Tracing;

namespace KJFramework.Messages.TypeProcessors.Maps
{
    /// <summary>
    ///     数组类型处理器映射表，提供了相关的基本操作。
    /// </summary>
    public sealed class ArrayTypeProcessorMapping
    {
        #region 构造函数

        /// <summary>
        ///     智能的类型处理器映射表，提供了相关的基本操作。
        /// </summary>
        private ArrayTypeProcessorMapping()
        {
            Initialize();
        }

        #endregion

        #region 成员

        private readonly Dictionary<Type, IIntellectTypeProcessor> _processor = new Dictionary<Type, IIntellectTypeProcessor>();
        public static readonly ArrayTypeProcessorMapping Instance = new ArrayTypeProcessorMapping();
        public readonly static IntellectPropertyAttribute DefaultAttribute = new IntellectPropertyAttribute(0, false);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ArrayTypeProcessorMapping));

        #endregion

        #region 方法

        /// <summary>
        ///     初始化所有系统内部提供的智能类型处理器
        /// </summary>
        private void Initialize()
        {
            Regist(new Int16ArrayIntellectTypeProcessor());
            Regist(new Int32ArrayIntellectTypeProcessor());
            Regist(new Int64ArrayIntellectTypeProcessor());
            Regist(new FloatArrayIntellectTypeProcessor());
            Regist(new BooleanArrayIntellectTypeProcessor());
            Regist(new GuidArrayIntellectTypeProcessor());
            Regist(new DoubleArrayIntellectTypeProcessor());
            Regist(new ByteArrayIntellectTypeProcessor());
            Regist(new CharArrayIntellectTypeProcessor());
            Regist(new DateTimeArrayIntellectTypeProcessor());
            Regist(new DecimalArrayIntellectTypeProcessor());
            Regist(new IntPtrArrayIntellectTypeProcessor());
            Regist(new SByteArrayIntellectTypeProcessor());
            Regist(new TimeSpanArrayIntellectTypeProcessor());
            Regist(new UInt16ArrayIntellectTypeProcessor());
            Regist(new UInt32ArrayIntellectTypeProcessor());
            Regist(new UInt64ArrayIntellectTypeProcessor());
            Regist(new StringArrayIntellectTypeProcessor());
            Regist(new IPEndPointArrayIntellectTypeProcessor());
        }

        /// <summary>
        ///     注册一个智能类型处理器
        ///     <para>* 如果该类型的处理器已经存在，则进行替换操作。</para>
        /// </summary>
        /// <param name="processor">智能类型处理器</param>
        public void Regist(IIntellectTypeProcessor processor)
        {
            if (processor == null) return;
            try
            {
                if (_processor.ContainsKey(processor.SupportedType))
                {
                    _processor[processor.SupportedType] = processor;
                    return;
                }
                _processor.Add(processor.SupportedType, processor);
            }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     注销一个具有指定支持类型的智能类型处理器
        /// </summary>
        /// <param name="supportedType">支持的处理类型</param>
        public void UnRegist(Type supportedType)
        {
            if (supportedType == null) return;
            try {  _processor.Remove(supportedType); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     获取一个具有指定支持类型的智能类型处理器
        /// </summary>
        /// <param name="supportedType">支持的处理类型</param>
        /// <returns>返回智能类型处理器</returns>
        public IIntellectTypeProcessor GetProcessor(Type supportedType)
        {
            if (supportedType == null) return null;
            try
            {
                IIntellectTypeProcessor result;
                return _processor.TryGetValue(supportedType, out result) ? result : null;
            }
            catch (System.Exception ex)
            {
                _tracing.Error(ex, null); 
                return null;
            }
        }

        #endregion
    }
}