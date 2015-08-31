using System.Collections.Generic;
using KJFramework.Exceptions;

namespace KJFramework.Cores.Segments
{
    /// <summary>
    ///     Ƭ��ʽ����ֲ�����Ԫ�ӿڣ��ṩ����صĻ�������
    /// </summary>
    public interface ISegmentCachePolicy
    {
        /// <summary>
        ///     ��ȡƬ�ηֲ��ȼ�
        /// </summary>
        int SegmentLevel { get; }
        /// <summary>
        ///     ����һ��Ƭ�ηֲ�����
        /// </summary>
        /// <param name="size">Ƭ�δ�С</param>
        /// <param name="percent">ռ�������ڴ�İٷֱ�</param>
        /// <exception cref="OutOfRangeException">����Ԥ���ķ�Χ</exception>
        void Set(int size, decimal percent);
        /// <summary>
        ///     ��ȡ���е�Ƭ�ηֲ�
        /// </summary>
        /// <returns>����Ƭ�ηֲ�����</returns>
        List<SegmentSizePair> Get();
    }
}