using System;

namespace KJFramework.Messages.Extends.Rules
{
    /// <summary>
    ///     �ָ����Ԫ�ӿڣ��ṩ����صĻ���������
    /// </summary>
    public interface ISplitRule : IDisposable
    {
        /// <summary>
        ///     ��ȡ������֧�ֵ�����
        /// </summary>
        Type SupportType { get; set; }
        /// <summary>
        ///     ��ȡ�����÷ָ��
        /// </summary>
        int SplitLength { get; set; }

        /// <summary>
        ///     ȡֵ���
        ///     <para>* �˷���ͨ�����ڼ���ѡ�ֶ��Ƿ�ӵ��ֵʱ���жϡ�</para>
        /// </summary>
        /// <param name="offset">��ǰԪ���ݵ�ƫ����</param>
        /// <param name="data">Ԫ����</param>
        /// <param name="tagOffset">
        ///     ���صĸ���ƫ����
        ///     <para>* ������صĽ����false�Ļ�������Ҫ��д����ƫ������</para>
        /// </param>
        /// <param name="targetContentLength">��ǰҪ��ȡ�����ݳ���</param>
        /// <returns>�����Ƿ����ȡֵ�Ľ��</returns>
        bool Check(int offset, byte[] data, ref int tagOffset, ref int targetContentLength);
    }
}