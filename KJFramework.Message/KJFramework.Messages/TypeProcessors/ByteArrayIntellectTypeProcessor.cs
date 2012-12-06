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
        /// <param name="memory">��Ҫ�����ֽ�����</param>
        /// <param name="offset">��Ҫ����������ʼƫ����</param>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="value">�������ͻ�����</param>
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override void Process(byte[] memory, int offset, IntellectPropertyAttribute attribute, object value)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
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
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="value">�������ͻ�����</param>
        /// <returns>����ת�����Ԫ����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        public override byte[] Process(IntellectPropertyAttribute attribute, object value)
        {
            byte[] memory;
            if (value == null && attribute.IsRequire) throw new ArgumentNullException("value");
            byte[] arr = (byte[])value;
            //id(1) + total length(4) + rank(4)
            memory = new byte[9 + Size.Byte * arr.Length];
            memory[0] = (byte)attribute.Id;
            BitConvertHelper.GetBytes(memory.Length - 5, memory, 1);
            BitConvertHelper.GetBytes(arr.Length, memory, 5);
            if (arr.Length == 0) return memory;
            Buffer.BlockCopy(arr, 0, memory, 9, arr.Length);
            return memory;
        }

        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override object Process(IntellectPropertyAttribute attribute, byte[] data)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
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
            if (length == 4) return new byte[0];
            int arrLength = BitConverter.ToInt32(data, offset);
            byte[] ret = new byte[arrLength];
            Buffer.BlockCopy(data, offset + 4, ret, 0, arrLength);
            return ret;
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