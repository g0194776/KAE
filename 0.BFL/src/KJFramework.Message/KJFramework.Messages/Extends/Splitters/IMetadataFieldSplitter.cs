using System;
using System.Collections.Generic;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages.Extends.Splitters
{
    /// <summary>
    ///     ��ϢԪ�����ֶηָ������ṩ����صĻ���������
    /// </summary>
    public interface IMetadataFieldSplitter
    {
        /// <summary>
        ///     �ָ��ֶ�
        /// </summary>
        /// <param name="target">���ܶ���</param>
        /// <param name="data">��ϢԪ����</param>
        /// <param name="head">��Ϣͷ��ʾ����</param>
        /// <param name="end">��Ϣβ��ʾ����</param>
        /// <returns>���طָ����ֶμ���</returns>
        /// <exception cref="System.Exception">�ָ�ʧ��</exception>
        Dictionary<int, byte[]> Split(IntellectObject target, byte[] data, out Dictionary<int, byte[]> head, out Dictionary<int, byte[]> end);
    }
}