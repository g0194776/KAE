using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    IntPtr�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class IntPtrIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     IntPtr�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public IntPtrIntellectTypeProcessor()
        {
            _supportedType = typeof(IntPtr);
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
            BitConvertHelper.GetBytes(((IntPtr)value).ToInt32(), memory, offset);
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="attribute">�ֶ�����</param>
        /// <param name="analyseResult">�������</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            IntPtr value;
            if (!isNullable) value = analyseResult.GetValue<IntPtr>(target);
            else
            {
                IntPtr? nullableValue = analyseResult.GetValue<IntPtr?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(
                        string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                        analyseResult.Property.Name,
                                        analyseResult.Property.PropertyType));
                }
                value = (IntPtr)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.IntPtr) && !isArrayElement) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteIntPtr(value);
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
            //4 bytes.
            return BitConverter.GetBytes(((IntPtr)value).ToInt32());
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
            return new IntPtr(BitConverter.ToInt32(data, offset));
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="instance">Ŀ�����</param>
        /// <param name="result">�������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵ�ƫ����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        public override void Process(object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0)
        {
            if (result.Nullable) result.SetValue<IntPtr?>(instance, new IntPtr(BitConverter.ToInt32(data, offset)));
            else result.SetValue(instance, new IntPtr(BitConverter.ToInt32(data, offset)));
        }

        #endregion
    }
}