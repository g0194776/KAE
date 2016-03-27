using System;
using KJFramework.Messages.Analysers;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Proxies;
using KJFramework.Statistics;

namespace KJFramework.Messages.TypeProcessors
{
    /// <summary>
    ///     ���ܵ����ʹ�����Ԫ�ӿڣ��ṩ�˶����ض������ض����������Ļ���֧�֡�
    /// </summary>
    public interface IIntellectTypeProcessor : IStatisticable<IStatistic>
    {
        /// <summary>
        ///     ��ȡΨһ���
        /// </summary>
        Guid Id { get; }
        /// <summary>
        ///     ��ȡһ��ֵ����ֵ��ʾ�˵�ǰ�������Ƿ�֧���Է��йܵķ�ʽ����ִ��
        /// </summary>
        bool SupportUnmanagement { get; }
        /// <summary>
        ///     ��ȡ������һ��ֵ����ֵ��ʾ�˵�ǰ��Ҫ�����Id��š�
        ///     <para>* ��һ�����ܶ�������Լ����д���ָ���ı�����ԣ��򽫻ύ�������ʹ���������</para>
        ///     <para>* ��SupportedId == nullʱ�������˵�ǰ�������ʹ��������������Ե�ID��ֻ�������Ե����͡�</para>
        /// </summary>
        int? SupportedId { get; set; }
        /// <summary>
        ///     ��ȡ֧�ֵ�����
        /// </summary>
        Type SupportedType { get; }
        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="attribute">�ֶ�����</param>
        /// <param name="analyseResult">�������</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        void Process(IMemorySegmentProxy proxy, IntellectPropertyAttribute attribute, ToBytesAnalyseResult analyseResult, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     �ӵ������ͻ�����ת��ΪԪ����
        ///     <para>* �˷������ᱻ��������DataHelper��ʹ�ã�����д������ݽ�����ӵ�б��(Id)</para>
        /// </summary>
        /// <param name="proxy">�ڴ�Ƭ�δ�����</param>
        /// <param name="target">Ŀ�����ʵ��</param>
        /// <param name="isArrayElement">��ǰд���ֵ�Ƿ�Ϊ����Ԫ�ر�ʾ</param>
        /// <param name="isNullable">�Ƿ�Ϊ�ɿ��ֶα�ʾ</param>
        void Process(IMemorySegmentProxy proxy, object target, bool isArrayElement = false, bool isNullable = false);
        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="attribute">��ǰ�ֶα�ע������</param>
        /// <param name="data">Ԫ����</param>
        /// <returns>����ת����ĵ������ͻ�����</returns>
        /// <exception cref="Exception">ת��ʧ��</exception>
        Object Process(IntellectPropertyAttribute attribute, byte[] data);
        /// <summary>
        ///     ��Ԫ����ת��Ϊ�������ͻ�����
        /// </summary>
        /// <param name="instance">Ŀ�����</param>
        /// <param name="result">�������</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="offset">Ԫ�������ڵı�����</param>
        /// <param name="length">Ԫ���ݳ���</param>
        void Process(Object instance, GetObjectAnalyseResult result, byte[] data, int offset, int length = 0);
    }
}