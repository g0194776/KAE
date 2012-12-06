using System;
using System.Net;
using System.Text;
using KJFramework.Core.Native;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.Proxies
{
    /// <summary>
    ///     �ڴ�Ƭ��
    /// </summary>
    internal unsafe sealed class MemorySegment : IMemorySegment
    {
        #region Constructor

        /// <summary>
        ///     �ڴ�Ƭ��
        /// </summary>
        /// <param name="data">���ڴ����ݶ�</param>
        /// <param name="startOffset">��ǰ�ڴ����ʼλ��</param>
        /// <param name="length">��ǰ�ڴ�ο��ó���</param>
        public MemorySegment(byte* data, uint startOffset, uint length)
        {
            _data = data;
            _startOffset = startOffset;
            _length = length;
            _endOffset = _startOffset + length;
            _startData = (_data + startOffset);
        }

        #endregion

        #region Members

        private readonly byte* _data;
        private readonly byte* _startData;
        private readonly uint _startOffset;
        private readonly uint _endOffset;
        private readonly uint _length;
        private uint _currentOffset;

        #endregion

        #region Implementation of IMemorySegment

        /// <summary>
        ///     ��ȡ��ǰ�ڴ�Ƭ�ε�ʣ����ó���
        /// </summary>
        public uint RemainingSize { get { return MemoryAllotter.SegmentSize - _currentOffset; } }
        /// <summary>
        ///     ��ȡ��ǰ�ڴ�Ƭ�εĿ���ƫ��
        /// </summary>
        public uint Offset { get { return _currentOffset; } }

        /// <summary>
        ///     ��ȡ�ڴ���ڲ���������ʼλ��ָ��
        /// </summary>
        /// <returns>����������ʼλ��ָ��</returns>
        public byte* GetPointer()
        {
            return _startData;
        }

        /// <summary>
        ///     ��ʼ����ǰ�ڴ�Ƭ��
        /// </summary>
        public IMemorySegment Initialize()
        {
            _currentOffset = 0;
            return this;
        }

        /// <summary>
        ///     ����ָ���ֽڳ���
        /// </summary>
        /// <param name="length">��Ҫ�������ֽڳ���</param>
        public void Skip(uint length)
        {
            _currentOffset += length;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt32(int* value)
        {
            *(int*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Int32;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt64(long* value)
        {
            *(long*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Int64;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt16(short* value)
        {
            *(short*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Int16;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteChar(char value)
        {
            *(char*)(&_startData[_currentOffset]) = value;
            _currentOffset += Size.Char;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt32(uint* value)
        {
            *(uint*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.UInt32;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt64(ulong* value)
        {
            *(ulong*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.UInt64;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt16(ushort* value)
        {
            *(ushort*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.UInt16;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteBoolean(bool value)
        {
            *(bool*)(&_startData[_currentOffset]) = value;
            _currentOffset += Size.Bool;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteFloat(float* value)
        {
            *(float*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Float;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDouble(double* value)
        {
            *(double*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Double;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteString(string value)
        {
            byte[] strBytes;
            fixed (byte* pByte = (strBytes = Encoding.UTF8.GetBytes(value)))
                Native.Win32API.memcpy((IntPtr)(&_startData[_currentOffset]), (IntPtr)pByte, (uint)strBytes.Length);
            _currentOffset += (uint)strBytes.Length;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteByte(byte value)
        {
            *&_startData[_currentOffset] = value;
            _currentOffset += Size.Byte;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteSByte(sbyte value)
        {
            *(sbyte*)(&_startData[_currentOffset]) = value;
            _currentOffset += Size.SByte;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDecimal(decimal* value)
        {
            *(decimal*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Decimal;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDateTime(DateTime* value)
        {
            *(long*)(&_startData[_currentOffset]) = (*value).Ticks;
            _currentOffset += Size.DateTime;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteIntPtr(IntPtr* value)
        {
            *(int*)(&_startData[_currentOffset]) = (*value).ToInt32();
            _currentOffset += Size.IntPtr;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteGuid(Guid* value)
        {
            *(Guid*)(&_startData[_currentOffset]) = *value;
            _currentOffset += Size.Guid;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteBitFlag(BitFlag value)
        {
            *&_startData[_currentOffset] = BitConvertHelper.ConvertToByte(value);
            _currentOffset += Size.BitFlag;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteIPEndPoint(IPEndPoint value)
        {
            byte* pByte = &_startData[_currentOffset];
            *(long*)(pByte) = value.Address.Address;
            *(int*)(pByte + 8) = value.Port;
            _currentOffset += Size.IPEndPoint;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteTimeSpan(TimeSpan* value)
        {
            *(long*)(&_startData[_currentOffset]) = (*value).Ticks;
            _currentOffset += Size.TimeSpan;
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ������ֵ���ڴ��ַ</param>
        /// <param name="length">д�볤��</param>
        public void WriteMemory(IntPtr value, uint length)
        {
            Native.Win32API.memcpy(new IntPtr(_startData + _currentOffset), value, length);
            _currentOffset += length;
        }

        /// <summary>
        ///     ȷ����ǰ�ڴ�Ƭ���Ƿ����㹻�Ĵ�С�ռ�
        /// </summary>
        /// <param name="size">����Ŀռ��С</param>
        /// <param name="remainingSize">ʣ�೤�� </param>
        /// <returns>�����жϺ�Ľ��</returns>
        public bool EnsureSize(uint size, out uint remainingSize)
        {
            remainingSize = RemainingSize;
            return remainingSize >= size;
        }

        #endregion
    }
}