using System;
using System.Net;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.Proxies
{
    /// <summary>
    ///     �ڴ�Ƭ�δ�����
    /// </summary>
    public unsafe interface IMemorySegmentProxy : IDisposable
    {
        /// <summary>
        ///     ��ȡ��ǰ�������ڲ����������ڴ�Ƭ�θ���
        /// </summary>
        int SegmentCount { get; }
        /// <summary>
        ///     ����ָ���ֽڳ���
        /// </summary>
        /// <param name="length">��Ҫ�������ֽڳ���</param>
        void Skip(uint length);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteInt32(int value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteInt64(long value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteInt64(long* value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteInt16(short value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteChar(char value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteUInt32(uint value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteUInt64(ulong value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteUInt64(ulong* value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteUInt16(ushort value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteBoolean(bool value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteFloat(float value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteDouble(double value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteDouble(double* value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteString(string value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteByte(byte value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteSByte(sbyte value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteDecimal(decimal value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteDecimal(decimal* value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteDateTime(DateTime value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteIntPtr(IntPtr value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteGuid(Guid value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteGuid(Guid* value);
        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��int32��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        void WriteBackInt32(MemoryPosition position, int value);
        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��int16��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        void WriteBackInt16(MemoryPosition position, short value);
        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��uint16��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        void WriteBackUInt16(MemoryPosition position, ushort value);
        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��uint32��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        void WriteBackUInt32(MemoryPosition position, uint value);
        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��void*
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        /// <param name="length">��д����</param>
        void WriteBackMemory(MemoryPosition position, void* value, uint length);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteBitFlag(BitFlag value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteIPEndPoint(IPEndPoint value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        void WriteTimeSpan(TimeSpan value);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ������ֵ���ڴ��ַ</param>
        /// <param name="length">д�볤��</param>
        void WriteMemory(void* value, uint length);
        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="data">��Ҫд����ڴ�</param>
        /// <param name="offset">��ʼ�ڴ�ƫ��</param>
        /// <param name="length">д�볤��</param>
        void WriteMemory(byte[] data, uint offset, uint length);
        /// <summary>
        ///     ��ȡһ����ǰ�ڲ�������λ�õļ�¼��
        /// </summary>
        /// <returns>�����ڲ�������λ�õļ�¼��</returns>
        MemoryPosition GetPosition();
        /// <summary>
        ///     ��ȡ�ڲ��Ļ������ڴ�
        /// </summary>
        ///     <para>* ʹ�ô˷��������ǻ�ǿ�ƻ����ڲ���Դ</para>
        /// <returns>���ػ������ڴ�</returns>
        byte[] GetBytes();
    }
}