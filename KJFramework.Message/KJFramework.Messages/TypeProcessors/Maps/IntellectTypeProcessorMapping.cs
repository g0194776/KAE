using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Logger;

namespace KJFramework.Messages.TypeProcessors.Maps
{
    /// <summary>
    ///     智能的类型处理器映射表，提供了相关的基本操作。
    /// </summary>
    public sealed class IntellectTypeProcessorMapping
    {
        #region 构造函数

        /// <summary>
        ///     智能的类型处理器映射表，提供了相关的基本操作。
        /// </summary>
        private IntellectTypeProcessorMapping()
        {
            Initialize();
        }

        #endregion

        #region 成员

        private readonly Dictionary<Type, IIntellectTypeProcessor> _processor = new Dictionary<Type, IIntellectTypeProcessor>();
        private readonly Dictionary<int?, IIntellectTypeProcessor> _customerIdProcessor = new Dictionary<int?, IIntellectTypeProcessor>();
        public static readonly IntellectTypeProcessorMapping Instance = new IntellectTypeProcessorMapping();
        public readonly static IntellectPropertyAttribute DefaultAttribute = new IntellectPropertyAttribute(0, false);

        #endregion

        #region 方法

        /// <summary>
        ///     初始化所有系统内部提供的智能类型处理器
        /// </summary>
        private void Initialize()
        {
            Regist(new CharIntellectTypeProcessor());
            Regist(new Int32IntellectTypeProcessor());
            Regist(new Int64IntellectTypeProcessor());
            Regist(new Int16IntellectTypeProcessor());
            Regist(new UInt32IntellectTypeProcessor());
            Regist(new UInt16IntellectTypeProcessor());
            Regist(new UInt64IntellectTypeProcessor());
            Regist(new BooleanIntellectTypeProcessor());
            Regist(new FloatIntellectTypeProcessor());
            Regist(new DoubleIntellectTypeProcessor());
            Regist(new StringIntellectTypeProcessor());
            Regist(new ByteIntellectTypeProcessor());
            Regist(new SByteIntellectTypeProcessor());
            Regist(new DecimalIntellectTypeProcessor());
            Regist(new DateTimeIntellectTypeProcessor());
            Regist(new IntPtrIntellectTypeProcessor());
            Regist(new GuidIntellectTypeProcessor());
            Regist(new BitFlagIntellectTypeProcessor());
            Regist(new IPEndPointIntellectTypeProcessor());
            Regist(new TimeSpanIntellectTypeProcessor());
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
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
            if (processor.SupportedId != null) _customerIdProcessor.Add(processor.SupportedId, processor);
        }

        /// <summary>
        ///     注销一个具有指定支持类型的智能类型处理器
        /// </summary>
        /// <param name="supportedType">支持的处理类型</param>
        public void UnRegist(Type supportedType)
        {
            if (supportedType == null) return;
            try {  _processor.Remove(supportedType); }
            catch (System.Exception ex) { Logs.Logger.Log(ex); }
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
                Logs.Logger.Log(ex);
                return null;
            }
        }

        /// <summary>
        ///     获取一个具有指定支持顺序编号的智能类型处理器
        /// </summary>
        /// <param name="id">顺序编号</param>
        /// <returns>返回智能类型处理器</returns>
        public IIntellectTypeProcessor GetProcessor(int id)
        {
            try
            {
                IIntellectTypeProcessor result;
                return _customerIdProcessor.TryGetValue(id, out result) ? result : null;
            }
            catch (System.Exception ex)
            {
                Logs.Logger.Log(ex);
                return null;
            }
        }

        #endregion
    }
}