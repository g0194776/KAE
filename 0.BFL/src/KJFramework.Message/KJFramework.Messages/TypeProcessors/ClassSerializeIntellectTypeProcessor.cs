using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Objects;
using KJFramework.Messages.Proxies;
using KJFramework.Serializers;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///    Class Serialize�������ܴ��������ṩ����صĻ���������
    /// </summary>
    public class ClassSerializeIntellectTypeProcessor : IntellectTypeProcessor
    {
        #region ���캯��

        /// <summary>
        ///     Class Serialize�������ܴ��������ṩ����صĻ���������
        /// </summary>
        public ClassSerializeIntellectTypeProcessor()
        {
            _supportedType = typeof(ClassSerializeObject);
            _supportUnmanagement = false;
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
        [Obsolete("Cannot use this method, because current type doesn't supported.", true)]
        public override void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false)
        {
            throw new NotSupportedException("Cannot use this method, because current type doesn't supported.");
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
            if (value == null) return null;
            using (ClassMetadataSerializer classMetadataSerializer = new ClassMetadataSerializer())
            {
                byte[] data = classMetadataSerializer.Serialize(value);
                if (attribute.IsRequire && data == null)
                {
                    throw new System.Exception("�޷�����Ƿ���ֵ��");
                }
                return data;
            }
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
            if (attribute.IsRequire && data == null) throw new System.Exception("�޷�����Ƿ�������ֵ��");
            if (data != null)
            {
                using (ClassMetadataSerializer classMetadataSerializer = new ClassMetadataSerializer())
                {
                    object value = classMetadataSerializer.Deserialize<Object>(data);
                    if (attribute.IsRequire && value == null)
                    {
                        throw new System.Exception("�޷�����Ƿ���ֵ��");
                    }
                    return value;
                }
            }
            return null;
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
            throw new NotImplementedException("Pls use management method to un-serialize current data!");
        }

        #endregion
    }
}