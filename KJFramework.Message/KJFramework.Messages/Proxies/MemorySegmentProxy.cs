using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using KJFramework.Core.Native;
using KJFramework.Messages.Helpers;
using KJFramework.Messages.Types;

namespace KJFramework.Messages.Proxies
{
    /// <summary>
    ///     �ڴ�Ƭ�δ�����
    /// </summary>
    internal sealed unsafe class MemorySegmentProxy : IMemorySegmentProxy
    {
        #region Members

        private bool _dispose;
        private int _currentIndex;
        private IList<IMemorySegment> _segments = new List<IMemorySegment>();
        /// <summary>
        ///     ��ȡ��ǰ�������ڲ����������ڴ�Ƭ�θ���
        /// </summary>
        public int SegmentCount { get { return _segments.Count; } }

        #endregion

        #region Implementation of IMemorySegment

        /// <summary>
        ///     ����ָ���ֽڳ���
        /// </summary>
        /// <param name="length">��Ҫ�������ֽڳ���</param>
        public void Skip(uint length)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(length, out remainingSize)) segment.Skip(length);
            else
            {
                uint trueRemainingSize = length;
                while (trueRemainingSize > 0U)
                {
                    if (remainingSize > 0U)
                    {
                        segment.Skip(remainingSize);
                        trueRemainingSize -= remainingSize;
                    }
                    segment = GetSegment(++_currentIndex);
                    if (!segment.EnsureSize(trueRemainingSize, out remainingSize)) continue;
                    segment.Skip(trueRemainingSize);
                    break;
                }
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt32(int value)
        {
            int* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Int32, out remainingSize)) segment.WriteInt32(pValue);
            else
            {
                uint trueRemainingSize = Size.Int32;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt64(long value)
        {
            long* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Int64, out remainingSize)) segment.WriteInt64(pValue);
            else
            {
                uint trueRemainingSize = Size.Int64;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt64(long* value)
        {
            long* pValue = value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Int64, out remainingSize)) segment.WriteInt64(pValue);
            else
            {
                uint trueRemainingSize = Size.Int64;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteInt16(short value)
        {
            short* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Int16, out remainingSize)) segment.WriteInt16(pValue);
            else
            {
                uint trueRemainingSize = Size.Int16;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteChar(char value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Char, out remainingSize)) segment.WriteChar(value);
            else
            {
                segment = GetSegment(++_currentIndex);
                segment.WriteChar(value);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt32(uint value)
        {
            uint* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.UInt32, out remainingSize)) segment.WriteUInt32(pValue);
            else
            {
                uint trueRemainingSize = Size.UInt32;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt64(ulong value)
        {
            ulong* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.UInt64, out remainingSize)) segment.WriteUInt64(pValue);
            else
            {
                uint trueRemainingSize = Size.UInt64;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt64(ulong* value)
        {
            ulong* pValue = value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.UInt64, out remainingSize)) segment.WriteUInt64(pValue);
            else
            {
                uint trueRemainingSize = Size.UInt64;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteUInt16(ushort value)
        {
            ushort* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.UInt16, out remainingSize)) segment.WriteUInt16(pValue);
            else
            {
                uint trueRemainingSize = Size.UInt16;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteBoolean(bool value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Bool, out remainingSize)) segment.WriteBoolean(value);
            else
            {
                segment = GetSegment(++_currentIndex);
                segment.WriteBoolean(value);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteFloat(float value)
        {
            float* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Float, out remainingSize)) segment.WriteFloat(pValue);
            else
            {
                uint trueRemainingSize = Size.Float;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDouble(double value)
        {
            double* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Double, out remainingSize)) segment.WriteDouble(pValue);
            else
            {
                uint trueRemainingSize = Size.Double;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDouble(double* value)
        {
            double* pValue = value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Double, out remainingSize)) segment.WriteDouble(pValue);
            else
            {
                uint trueRemainingSize = Size.Double;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteString(string value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            byte[] data = Encoding.UTF8.GetBytes(value);
            uint remainingSize, trueRemainingSize;
            if (segment.EnsureSize((trueRemainingSize = (uint)data.Length), out remainingSize))
                fixed (byte* pByte = data) segment.WriteMemory(new IntPtr(pByte), (uint)data.Length);
            else
            {
                fixed(byte* pByte = data)
                {
                    uint usedByte = 0U;
                    while (trueRemainingSize > 0U)
                    {
                        if (remainingSize > 0U)
                        {
                            segment.WriteMemory(new IntPtr(pByte + usedByte), remainingSize);
                            trueRemainingSize -= remainingSize;
                            usedByte += remainingSize;
                        }
                        segment = GetSegment(++_currentIndex);
                        if (!segment.EnsureSize(trueRemainingSize, out remainingSize)) continue;
                        segment.WriteMemory(new IntPtr(pByte + usedByte), trueRemainingSize);
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteByte(byte value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Byte, out remainingSize)) segment.WriteByte(value);
            else
            {
                segment = GetSegment(++_currentIndex);
                segment.WriteByte(value);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteSByte(sbyte value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.SByte, out remainingSize)) segment.WriteSByte(value);
            else
            {
                segment = GetSegment(++_currentIndex);
                segment.WriteSByte(value);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDecimal(decimal value)
        {
            decimal* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Decimal, out remainingSize)) segment.WriteDecimal(pValue);
            else
            {
                uint trueRemainingSize = Size.Decimal;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDecimal(decimal* value)
        {
            decimal* pValue = value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Decimal, out remainingSize)) segment.WriteDecimal(pValue);
            else
            {
                uint trueRemainingSize = Size.Decimal;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)pValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteDateTime(DateTime value)
        {
            long temp = value.Ticks;
            long* tempValue = &temp;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.DateTime, out remainingSize)) segment.WriteInt64(tempValue);
            else
            {
                uint trueRemainingSize = Size.DateTime;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)tempValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)tempValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteIntPtr(IntPtr value)
        {
            int temp = value.ToInt32();
            int* tempValue = &temp;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.IntPtr, out remainingSize)) segment.WriteInt32(tempValue);
            else
            {
                uint trueRemainingSize = Size.IntPtr;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)tempValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)tempValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteGuid(Guid value)
        {
            Guid* pValue = &value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Guid, out remainingSize)) segment.WriteGuid(pValue);
            else
            {
                byte* pByte = (byte*)pValue;
                uint trueRemainingSize = Size.Guid;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pByte, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)(pByte + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteGuid(Guid* value)
        {
            Guid* pValue = value;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.Guid, out remainingSize)) segment.WriteGuid(pValue);
            else
            {
                byte* pByte = (byte*)pValue;
                uint trueRemainingSize = Size.Guid;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)pByte, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)(pByte + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��int32��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        public void WriteBackInt32(MemoryPosition position, int value)
        {
            //well-done, needn't cross memory segments.
            uint remainingSize = (MemoryAllotter.SegmentSize - position.SegmentOffset);
            if (remainingSize >= Size.Int32)
                *(int*)(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset) = value;
            else
            {
                uint trueRemainingSize = Size.Int32;
                byte* data = (byte*) &value;
                if(remainingSize != 0U)
                {
                    Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset), new IntPtr(data), remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                //write at head.
                Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex + 1].GetPointer()), new IntPtr(data + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��int16��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        public void WriteBackInt16(MemoryPosition position, short value)
        {
            //well-done, needn't cross memory segments.
            uint remainingSize = (MemoryAllotter.SegmentSize - position.SegmentOffset);
            if (remainingSize >= Size.Int16)
                *(short*)(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset) = value;
            else
            {
                uint trueRemainingSize = Size.Int16;
                byte* data = (byte*)&value;
                if (remainingSize != 0U)
                {
                    Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset), new IntPtr(data), remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                //write at head.
                Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex + 1].GetPointer()), new IntPtr(data + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     ��ָ���ڴ�ε�ָ��ƫ�ƴ���дһ��uint16��ֵ
        /// </summary>
        /// <param name="position">��дλ��</param>
        /// <param name="value">��дֵ</param>
        public void WriteBackUInt16(MemoryPosition position, ushort value)
        {
            //well-done, needn't cross memory segments.
            uint remainingSize = (MemoryAllotter.SegmentSize - position.SegmentOffset);
            if (remainingSize >= Size.UInt16)
                *(ushort*)(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset) = value;
            else
            {
                uint trueRemainingSize = Size.UInt16;
                byte* data = (byte*)&value;
                if (remainingSize != 0U)
                {
                    Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex].GetPointer() + position.SegmentOffset), new IntPtr(data), remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                //write at head.
                Native.Win32API.memcpy(new IntPtr(_segments[position.SegmentIndex + 1].GetPointer()), new IntPtr(data + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteBitFlag(BitFlag value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.BitFlag, out remainingSize)) segment.WriteBitFlag(value);
            else
            {
                segment = GetSegment(++_currentIndex);
                segment.WriteBitFlag(value);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteIPEndPoint(IPEndPoint value)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.IPEndPoint, out remainingSize)) segment.WriteIPEndPoint(value);
            else
            {
                byte[] data = new byte[Size.IPEndPoint];
                fixed (byte* pByte = data)
                {
                    *(long*) (pByte) = value.Address.Address;
                    *(int*) (pByte + 8) = value.Port;
                    uint trueRemainingSize = Size.IPEndPoint;
                    if (remainingSize > 0U)
                    {
                        segment.WriteMemory((IntPtr)pByte, remainingSize);
                        trueRemainingSize -= remainingSize;
                    }
                    segment = GetSegment(++_currentIndex);
                    segment.WriteMemory((IntPtr)(pByte + remainingSize), trueRemainingSize);
                }
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ�����͵�ֵ</param>
        public void WriteTimeSpan(TimeSpan value)
        {
            long temp = value.Ticks;
            long* tempValue = &temp;
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            if (segment.EnsureSize(Size.TimeSpan, out remainingSize)) segment.WriteInt64(tempValue);
            else
            {
                uint trueRemainingSize = Size.TimeSpan;
                if (remainingSize > 0U)
                {
                    segment.WriteMemory((IntPtr)tempValue, remainingSize);
                    trueRemainingSize -= remainingSize;
                }
                segment = GetSegment(++_currentIndex);
                segment.WriteMemory((IntPtr)((byte*)tempValue + remainingSize), trueRemainingSize);
            }
        }

        /// <summary>
        ///     д��һ��ָ�����͵�ֵ
        /// </summary>
        /// <param name="value">ָ������ֵ���ڴ��ַ</param>
        /// <param name="length">д�볤��</param>
        public void WriteMemory(void* value, uint length)
        {
            IMemorySegment segment = GetSegment(_currentIndex);
            uint remainingSize;
            uint continueSize = 0U;
            if (segment.EnsureSize(length, out remainingSize)) segment.WriteMemory(new IntPtr(value), length);
            else
            {
                uint trueRemainingSize = length;
                do
                {
                    if (remainingSize > 0U)
                    {
                        segment.WriteMemory(new IntPtr((byte*)value + continueSize), remainingSize);
                        trueRemainingSize -= remainingSize;
                        continueSize += remainingSize;
                    }
                    segment = GetSegment(++_currentIndex);
                    if (!segment.EnsureSize(trueRemainingSize, out remainingSize)) continue;
                    segment.WriteMemory(new IntPtr((byte*)value + continueSize), trueRemainingSize);
                    break;
                } while (trueRemainingSize > 0U);
            }
        }

        /// <summary>
        ///     ��ȡһ����ǰ�ڲ�������λ�õļ�¼��
        /// </summary>
        /// <returns>�����ڲ�������λ�õļ�¼��</returns>
        public MemoryPosition GetPosition()
        {
            return new MemoryPosition(_currentIndex, _segments.Count == 0 ? 0 : _segments[_currentIndex].Offset);
        }

        /// <summary>
        ///     ��ȡ�ڲ��Ļ������ڴ�
        /// </summary>
        /// <param name="recoverResource">�����ڲ���Դ��ʶ�����Ϊture��ִ���굱ǰ�����󣬻�����ڲ���ӵ�е��ڴ�Ƭ��</param>
        /// <returns>���ػ������ڴ�</returns>
        public byte[] GetBytes(bool recoverResource = false)
        {
            if (_segments.Count == 0) return null;
            int totalSize = _segments.Count == 1
                                ? (int) _segments[0].Offset
                                : (int) _segments.Sum(segment => MemoryAllotter.SegmentSize - segment.RemainingSize);
            uint offset = 0;
            byte[] data = new byte[totalSize];
            fixed (byte* pByte = data)
            {
                for (int i = 0; i < _segments.Count; i++)
                {
                    IMemorySegment segment = _segments[i];
                    if (i != _segments.Count - 1)
                    {
                        Native.Win32API.memcpy(new IntPtr(pByte + offset), new IntPtr(segment.GetPointer()), MemoryAllotter.SegmentSize);
                        offset += MemoryAllotter.SegmentSize;
                    }
                    else Native.Win32API.memcpy(new IntPtr(pByte + offset), new IntPtr(segment.GetPointer()), segment.Offset);
                }
            }
            //recover resources.
            if(recoverResource) Dispose();
            return data;
        }

        #endregion

        #region Methods

        private IMemorySegment GetSegment(int index)
        {
            if(_segments.Count == 0 &&  index == 0)
            {
                IMemorySegment segment = MemoryAllotter.Instance.Rent();
                if (segment == null) throw new System.Exception("#Cannot rent new memory segment!");
                _segments.Add(segment);
                return segment;
            }
            if (index > _segments.Count - 1)
            {
                while ((_segments.Count - 1) < index)
                {
                    IMemorySegment segment = MemoryAllotter.Instance.Rent();
                    if (segment == null) throw new System.Exception("#Cannot rent new memory segment!");
                    _segments.Add(segment);
                }
            }
            return _segments[index];
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_dispose) return;
            for (int i = 0; i < _segments.Count; i++)
                MemoryAllotter.Instance.Giveback(_segments[i]);
            _currentIndex = 0;
            _segments.Clear();
            _dispose = true;
        }

        #endregion
    }
}