using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Boolean�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class BooleanIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     Boolean�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public BooleanIntellectTypeProcessor()
        {
            _supportedType = typeof(bool);
        }

        #endregion

        #region Overrides of IntellectTypeProcessor

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="memory">��Ҫ�����ֽ�����</param>
        /// <param name="offset">��Ҫ����������ʼƫ����</param>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="value">�������ͻ�����</param>
        public override void Process(byte[] memory, int offset, IntellectPropertyAttribute attribute, object value)
        {
            memory[offset] = (byte)(((bool)value) ? 1 : 0);
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="attribute">�ֶ�����</param>
        /// <param name="analyseResult">�������</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            bool value;
            if (!isNullable) value = analyseResult.GetValue<bool>(target);
            else
            {
                bool? nullableValue = analyseResult.GetValue<bool?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                                            analyseResult.Property.Name,
                                                            analyseResult.Property.PropertyType));
                }
                value = (bool)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.Boolean) && !isArrayElement) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteBoolean(value);
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="value">�������ͻ�����</param>
        /// <returns>����ת�����Ԫ����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            if (attribute == null) throw new System.Exception("�Ƿ����������Ա�ǩ��");
            if (attribute.IsRequire && value == null) throw new System.Exception("�޷�����Ƿ�������ֵ��");
            return BitConverter.GetBytes((bool)value);
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            return Process(attribute, data, 0);
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵ�ƫ����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override object Process(IntellectPropertyAttribute attribute, byte[] data, int offset, int length = 0)
        {
            if (attribute == null) throw new System.Exception("�Ƿ����������Ա�ǩ��");
            if (attribute.IsRequire && data == null) throw new System.Exception("�޷�����Ƿ�������ֵ��");
            return BitConverter.ToBoolean(data, offset);
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="instance">Ŀ�����</param>
        /// <param name="result">�������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵı�����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        public override void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0)
        {
            if (result.Nullable)
                result.SetValue<bool?>(instance, BitConverter.ToBoolean(data, offset));
            else
                result.SetValue(instance, BitConverter.ToBoolean(data, offset));
        }

        #endregion
    }
}