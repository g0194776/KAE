using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Byte�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class ByteIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     Byte�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public ByteIntellectTypeProcessor()
        {
            _supportedType = typeof(byte);
        }

        #endregion

        #region Overrides of IntellectTypeProcessor

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
            byte value;
            if (!isNullable) value = analyseResult.GetValue<byte>(target);
            else
            {
                byte? nullableValue = analyseResult.GetValue<byte?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                                                       analyseResult.Property.Name,
                                                                       analyseResult.Property.PropertyType));
                }
                value = (byte)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.Byte) && !isArrayElement) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteByte(value);
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        ///     <para>* �˷������ᱻ��������DataHelper��ʹ�ã�����д������ݽ�����ӵ�б��(Id)</para>
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        public override void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false)
        {
            byte value;
            if (!isNullable) value = (byte)target;
            else
            {
                if (target == null) return;
                value = (byte)target;
            }
            proxy.WriteByte(value);
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
            if (attribute == null) throw new System.Exception("�Ƿ����������Ա�ǩ��");
            if (attribute.IsRequire && data == null) throw new System.Exception("�޷������Ƿ�������ֵ��");
            return data[0];
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
            if (result.Nullable) result.SetValue<byte?>(instance, data[offset]); 
            else result.SetValue(instance, data[offset]); 
        }

        #endregion
    }
}