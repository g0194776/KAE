using System;
using System.Text;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Exceptions;
using KJFramework.Messages.Proxies;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    String�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class StringIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     String�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public StringIntellectTypeProcessor()
        {
            _supportedType = typeof(string);
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
            string value = isArrayElement ? (string)target : analyseResult.GetValue<string>(target);
            if (value == null)
            {
                if (!attribute.IsRequire || isArrayElement) return;
                throw new PropertyNullValueException(string.Format(ExceptionMessage.EX_PROPERTY_VALUE, attribute.Id, analyseResult.Property.Name, analyseResult.Property.PropertyType));
            }
            if (!isArrayElement)
            {
                proxy.WriteByte((byte)attribute.Id);
                proxy.WriteInt32(Encoding.UTF8.GetByteCount(value));
            }
            proxy.WriteString(value);
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
            if (string.IsNullOrEmpty((string) target)) return;
            string value = (string) target;
            proxy.WriteString(value);
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
            if (attribute == null) throw new ArgumentNullException("attribute");
            if (attribute.IsRequire && data == null)
                throw new System.Exception("Cannot process a required value, because current binary data is null! #attr id: " + attribute.Id);
            if (data == null) return null;
            if (data.Length == 0) return "";
            return Encoding.UTF8.GetString(data);
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
            if (length == 0 || data == null || data.Length == 0) return;
            unsafe
            {
                fixed (byte* old = &data[offset])
                {
                    int charCount = Encoding.UTF8.GetCharCount(old, length);
                    //allcate memory at thread stack.
                    if (charCount <= MemoryAllotter.CharSizeCanAllcateAtStack)
                    {
                        char* newObj = stackalloc char[charCount];
                        int len = Encoding.UTF8.GetChars(old, length, newObj, charCount);
                        result.SetValue(instance, new string(newObj, 0, len));
                    }
                    //allocate memory at heap.
                    else
                    {
                        fixed (char* newObj = new char[charCount])
                        {
                            int len = Encoding.UTF8.GetChars(old, length, newObj, charCount);
                            result.SetValue(instance, new string(newObj, 0, len));
                        }
                    }
                }
            }
        }

        #endregion
    }
}