using System;
using System.Collections.Generic;
using KJFramework.Messages.Attributes;
using KJFramework.Tracing;

namespace KJFramework.Messages.TypeProcessors.Maps
{
    /// <summary>
    ///     �������ʹ�����ӳ����ṩ����صĻ���������
    /// </summary>
    public sealed class ArrayTypeProcessorMapping
    {
        #region ���캯��

        /// <summary>
        ///     ���ܵ����ʹ�����ӳ����ṩ����صĻ���������
        /// </summary>
        private ArrayTypeProcessorMapping()
        {
            Initialize();
        }

        #endregion

        #region ��Ա

        private readonly Dictionary<Type, IIntellectTypeProcessor> _processor = new Dictionary<Type, IIntellectTypeProcessor>();
        public static readonly ArrayTypeProcessorMapping Instance = new ArrayTypeProcessorMapping();
        public readonly static IntellectPropertyAttribute DefaultAttribute = new IntellectPropertyAttribute(0, false);
        private static readonly ITracing _tracing = TracingManager.GetTracing(typeof(ArrayTypeProcessorMapping));

        #endregion

        #region ����

        /// <summary>
        ///     ��ʼ������ϵͳ�ڲ��ṩ���������ʹ�����
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

        #endregion
    }
}