using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Byte Array�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class ByteArrayIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     Byte Array�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public ByteArrayIntellectTypeProcessor()
        {
            _supportedType = typeof(byte[]);
            _supportUnmanagement = true;
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
            byte[] value = analyseResult.GetValue<byte[]>(target);
            if (value == null)
            {
                if (!attribute.IsRequire) return;
                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
            }
            //id(1) + total length(4) + rank(4)
            proxy.WriteByte((byte)attribute.Id);
            MemoryPosition position = proxy.GetPosition();
            proxy.Skip(4U);
            proxy.WriteInt32(value.Length);
            if (value.Length == 0)
            {
                proxy.WriteBackInt32(position, 4);
                return;
            }
            unsafe
            {
                fixed (byte* pByte = value) proxy.WriteMemory(pByte, (uint)value.Length * Size.Byte);
            }
            proxy.WriteBackInt32(position, (int)(value.Length * Size.Byte + 4));
        }

        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        ///     <para>* �˷������ᱻ��������DataHelper��ʹ�ã�����д������ݽ�����ӵ�б��(Id)</para>
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        public unsafe override void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false)
        {
            byte[] array = (byte[])target;
            if (array == null || array.Length == 0) return;
            if (array.Length > 10) fixed (byte* pByte = array) proxy.WriteMemory(pByte, (uint)array.Length * Size.Byte);
            else for (int i = 0; i < array.Length; i++) proxy.WriteByte(array[i]);
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
            return data;
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
            if (length == 4)
            {
                result.SetValue(instance, new byte[0]);
                return;
            }
            int arrLength = BitConverter.ToInt32(data, offset);
            byte[] ret = new byte[arrLength];
            Buffer.BlockCopy(data, offset + 4, ret, 0, arrLength);
            result.SetValue(instance, ret);
        }

        #endregion
    }
}