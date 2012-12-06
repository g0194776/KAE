using System;
using System.Diagnostics;

namespace KJFramework.Cache.Objects
{
    /// <summary>
    ///     �ֽ������ڴ�Ƭ�Σ��ṩ����صĻ�������
    /// </summary>
    [DebuggerDisplay("Usedbytes: {UsedBytes}, Offset: {Segment.Offset}")]
    public class MemorySegment : IMemorySegment
    {
        #region Constructor

        /// <summary>
        ///     �ֽ������ڴ�Ƭ�Σ��ṩ����صĻ�������
        /// </summary>
        /// <param name="segment">�ֽ������ڴ�Ƭ��</param>
        public MemorySegment(ArraySegment<byte> segment)
        {
            _segment = segment;
        }

        #endregion

        #region Implementation of IMemorySegment

        private readonly ArraySegment<byte> _segment;
        private int _usedBytes;

        /// <summary>
        ///     ��ȡ�ڲ����ֽ�����Ƭ��
        /// </summary>
        public ArraySegment<byte> Segment
        {
            get { return _segment; }
        }

        /// <summary>
        ///     ��ȡ��������ʹ�õ��ֽ�����
        /// </summary>
        public int UsedBytes
        {
            get { return _usedBytes; }
            set { _usedBytes = value; }
        }

        /// <summary>
        ///     ��ȡ��������ʹ��ƫ����
        ///     <para>* ���ǽ�����Ӧ������ʹ�ô��������жϵ�ǰ�������ݵ���ʵƫ����.</para>
        ///     <para>* �����ô����Ժ����ǽ����Զ�����UsedBytes.</para>
        /// </summary>
        public int UsedOffset
        {
            get { return _segment.Offset + _usedBytes; }
            set { _usedBytes += value; }
        }

        #endregion
    }
}