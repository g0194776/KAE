using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Char�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class CharIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     Char�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public CharIntellectTypeProcessor()
        {
            _supportedType = typeof(char);
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
            char value;
            if (!isNullable) value = analyseResult.GetValue<char>(target);
            else
            {
                char? nullableValue = analyseResult.GetValue<char?>(target);
                if (nullableValue == null)
                {
                    if (!attribute.IsRequire) return;
                    throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id,
                                                                       analyseResult.Property.Name,
                                                                       analyseResult.Property.PropertyType));
                }
                value = (char)nullableValue;
            }
            if (attribute.AllowDefaultNull && value.Equals(DefaultValue.Char) && !isArrayElement) return;
            proxy.WriteByte((byte)attribute.Id);
            proxy.WriteChar(value);
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
            char value;
            if (!isNullable) value = (char)target;
            else
            {
                if (target == null) return;
                value = (char)target;
            }
            proxy.WriteChar(value);
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
            return BitConverter.ToChar(data, 0);
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
            if (result.Nullable) result.SetValue<char?>(instance, BitConverter.ToChar(data, offset));
            else result.SetValue(instance, BitConverter.ToChar(data, offset));
        }
        #endregion
    }
}