using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Tracing;

namespace KJFramework.Messages.TypeProcessors.Maps
{
    /// <summary>
    ///     ���ܵ����ʹ�����ӳ����ṩ����صĻ���������
    /// </summary>
    public sealed class IntellectTypeProcessorMapping
    {
        #region ���캯��

        /// <summary>
        ///     ���ܵ����ʹ�����ӳ����ṩ����صĻ���������
        /// </summary>
        private IntellectTypeProcessorMapping()
        {
            Initialize();
        }

        #endregion

        #region ��Ա

        private readonly Dictionary<Type, IIntellectTypeProcessor> _processor = new Dictionary<Type, IIntellectTypeProcessor>();
        private readonly Dictionary<int?, IIntellectTypeProcessor> _customerIdProcessor = new Dictionary<int?, IIntellectTypeProcessor>();
        public static readonly IntellectTypeProcessorMapping Instance = new IntellectTypeProcessorMapping();
        public readonly static IntellectPropertyAttribute DefaultAttribute = new IntellectPropertyAttribute(0, false);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(IntellectTypeProcessorMapping));

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ������ϵͳ�ڲ��ṩ���������ʹ�����
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
            Regist(new BlobIntellectTypeProcessor());
        }

        /// <summary>
        ///     ע��һ���������ʹ�����
        ///     <para>* ��������͵Ĵ������Ѿ����ڣ�������滻������</para>
        /// </summary>
        /// <param name="processor">�������ʹ�����</param>
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
            if (processor.SupportedId != null) _customerIdProcessor.Add(processor.SupportedId, processor);
        }

        /// <summary>
        ///     ע��һ������ָ��֧�����͵��������ʹ�����
        /// </summary>
        /// <param name="supportedType">֧�ֵĴ�������</param>
        public void UnRegist(Type supportedType)
        {
            if (supportedType == null) return;
            try {  _processor.Remove(supportedType); }
            catch (System.Exception ex) { _tracing.Error(ex, null); }
        }

        /// <summary>
        ///     ��ȡһ������ָ��֧�����͵��������ʹ�����
        /// </summary>
        /// <param name="supportedType">֧�ֵĴ�������</param>
        /// <returns>�����������ʹ�����</returns>
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

        /// <summary>
        ///     ��ȡһ������ָ��֧��˳���ŵ��������ʹ�����
        /// </summary>
        /// <param name="id">˳����</param>
        /// <returns>�����������ʹ�����</returns>
        public IIntellectTypeProcessor GetProcessor(int id)
        {
            try
            {
                IIntellectTypeProcessor result;
                return _customerIdProcessor.TryGetValue(id, out result) ? result : null;
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