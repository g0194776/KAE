using System;
using System.Net;
using System.Text;
using KJFramework.Messages.Contracts;

namespace KJFramework.Messages
{
    /// <summary>
    ///     二进制消息框架内部专用的内存流
    /// </summary>
    internal sealed class MemoryStream
    {
        #region Constructor

        /// <summary>
        ///     二进制消息框架内部专用的内存流
        /// </summary>
        /// <param name="suggestSize">初始化建议大小</param>
        /// <exception cref="ArgumentException">参数无效</exception>
        public MemoryStream(int suggestSize)
        {
            _suggestSize = suggestSize;
            if (suggestSize <= 0) throw new ArgumentException("#Illegal suggest size, value: " + suggestSize);
            _data = new byte[_suggestSize];
        }

        #endregion

        #region Members

        private byte[] _data;
        private int _position;
        private int _suggestSize;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     确保当前的缓冲区有足够的容量容纳下一次的数据
        /// </summary>
        /// <param name="size">需要容纳的数据大小</param>
        private void EnsureSize(int size)
        {
            if (_data.Length - _position >= size) return;
            int sizeLevel = size / _suggestSize;
            byte[] newData;
            if (sizeLevel <= 1) _suggestSize = _suggestSize * 2;
            else _suggestSize = _suggestSize * sizeLevel;
            newData = new byte[_suggestSize];
            Buffer.BlockCopy(_data, 0, newData, 0, _position);
            _data = newData;
        }

        public byte[] GetBuffer()
        {
            byte[] data = new byte[_position];
            Buffer.BlockCopy(_data, 0, data, 0, _position);
            return data;
        }

        public void Write(byte id, bool value)
        {
            EnsureSize(2);
            _data[_position++] = id;
            _data[_position++] = (byte)(value ? 1 : 0);
        }

        public void Write(byte id, byte value)
        {
            EnsureSize(2);
            _data[_position++] = id;
            _data[_position++] = value;
        }

        public void Write(byte id, char value)
        {
            EnsureSize(2);
            _data[_position++] = id;
            _data[_position++] = BitConverter.GetBytes(value)[0];
        }

        public unsafe void Write(byte id, DateTime value)
        {
            EnsureSize(9);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(long*) pByte = value.Ticks;
            _position += 8;
        }

        public void Write(byte id, decimal value)
        {
            EnsureSize(2);
            _data[_position++] = id;
            _data[_position++] = Convert.ToByte(value);
        }

        public unsafe void Write(byte id, double value)
        {
            EnsureSize(9);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(double*)pByte = value;
            _position += 8;
        }

        public unsafe void Write(byte id, float value)
        {
            EnsureSize(5);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(float*)pByte = value;
            _position += 4;
        }

        public unsafe void Write(byte id, Guid value)
        {
            EnsureSize(17);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(Guid*)pByte = value;
            _position += 16;
        }

        public unsafe void Write(byte id, short value)
        {
            EnsureSize(3);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(short*)pByte = value;
            _position += 2;
        }

        public unsafe void Write(byte id, int value)
        {
            EnsureSize(5);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(int*) pByte = value;
            _position += 4;
        }

        public unsafe void Write(byte id, long value)
        {
            EnsureSize(9);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(long*)pByte = value;
            _position += 8;
        }

        public unsafe void Write(byte id, IntPtr value)
        {
            EnsureSize(5);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(int*)pByte = value.ToInt32();
            _position += 4;
        }

        public void Write(byte id, IPEndPoint value)
        {
            EnsureSize(13);
            _data[_position++] = id;
            unsafe
            {
                fixed (byte* pByte = &_data[_position])
                {
                    *(long*)(pByte) = value.Address.Address;
                    *(int*)(pByte + 8) = value.Port;
                }
            }
            _position += 12;
        }

        public void Write(byte id, sbyte value)
        {
            EnsureSize(2);
            _data[_position++] = id;
            _data[_position++] = Convert.ToByte(value);
        }

        public unsafe void Write(byte id, TimeSpan value)
        {
            EnsureSize(9);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(long*)pByte = value.Ticks;
            _position += 8;
        }

        public unsafe void Write(byte id, ushort value)
        {
            EnsureSize(3);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(short*)pByte = (short) value;
            _position += 2;
        }

        public unsafe void Write(byte id, uint value)
        {
            EnsureSize(5);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(int*)pByte = (int)value;
            _position += 4;
        }

        public unsafe void Write(byte id, ulong value)
        {
            EnsureSize(9);
            _data[_position++] = id;
            fixed (byte* pByte = &_data[_position]) *(long*)pByte = (long)value;
            _position += 8;
        }

        public unsafe void Write(byte id, string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                EnsureSize(5);
                fixed (byte* pByte = &_data[_position])
                {
                    *pByte = id;
                    *(int*) (pByte + 1) = 0;
                }
                _position += 5;
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                EnsureSize(5 + data.Length);
                fixed (byte* pByte = &_data[_position])
                {
                    *pByte = id;
                    *(int*)(pByte + 1) = data.Length;
                }
                _position += 5;
                Buffer.BlockCopy(data, 0, _data, _position, data.Length);
                _position += data.Length;
            }
        }

        public void Write(byte id, IntellectObject value)
        {

        }

        public void Write(bool value)
        {
            EnsureSize(1);
            _data[_position++] = (byte) (value ? 1 : 0);
        }

        public void Write(byte value)
        {
            EnsureSize(1);
            _data[_position++] = value;
        }

        public void Write(char value)
        {
            EnsureSize(1);
            _data[_position++] = Convert.ToByte(value);
        }

        public void Write(DateTime value)
        {
            EnsureSize(8);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(long*) pByte = value.Ticks;
            }
            _position += 8;
        }

        public void Write(decimal value)
        {
            EnsureSize(1);
            _data[_position++] = Convert.ToByte(value);
        }

        public void Write(double value)
        {
            EnsureSize(8);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(double*)pByte = value;
            }
            _position += 8;
        }

        public void Write(float value)
        {
            EnsureSize(4);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(float*)pByte = value;
            }
            _position += 4;
        }

        public void Write(Guid value)
        {
            EnsureSize(16);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(Guid*)pByte = value;
            }
            _position += 16;
        }

        public void Write(short value)
        {
            EnsureSize(2);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(short*)pByte = value;
            }
            _position += 2;
        }

        public void Write(int value)
        {
            EnsureSize(4);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(int*)pByte = value;
            }
            _position += 4;
        }

        public void Write(long value)
        {
            EnsureSize(8);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(long*)pByte = value;
            }
            _position += 8;
        }

        public void Write(IntPtr value)
        {
            EnsureSize(4);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(int*)pByte = value.ToInt32();
            }
            _position += 4;
        }

        public void Write(IPEndPoint value)
        {
            EnsureSize(12);
            unsafe
            {
                fixed (byte* pByte = &_data[_position])
                {
                    *(long*)(pByte) = value.Address.Address;
                    *(int*)(pByte + 8) = value.Port;
                }
            }
            _position += 12;
        }

        public void Write(sbyte value)
        {
            EnsureSize(1);
            _data[_position++] = Convert.ToByte(value);
        }

        public void Write(TimeSpan value)
        {
            EnsureSize(8);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(long*)pByte = value.Ticks;
            }
            _position += 8;
        }

        public void Write(ushort value)
        {
            EnsureSize(2);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(short*)pByte = (short) value;
            }
            _position += 2;
        }

        public void Write(uint value)
        {
            EnsureSize(4);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(int*)pByte = (int)value;
            }
            _position += 4;
        }

        public void Write(ulong value)
        {
            EnsureSize(8);
            unsafe
            {
                fixed (byte* pByte = &_data[_position]) *(long*)pByte = (long)value;
            }
            _position += 8;
        }

        public unsafe void Write(string value)
        {
            if (string.IsNullOrEmpty(value)) Write((short)0);
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                EnsureSize(data.Length + 2);
                fixed (byte* pByte = &_data[_position]) *(short*)pByte = (short)data.Length;
                _position += 2;
                Buffer.BlockCopy(data, 0, _data, _position, data.Length);
                _position += data.Length;
            }
        }

        public void Write(IntellectObject value)
        {

        }

        #endregion
    }
}