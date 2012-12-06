using System;
using System.Collections.Generic;

namespace KJFramework.IO.Buffers
{
    /// <summary>
    ///   �ֽ����黺�������������ڽ�����Ϣ��ʱ������յ����ֽ�����
    /// </summary>
    public abstract class ByteArrayBuffer : IByteArrayBuffer
    {
        private object oo = new object();

        #region ���캯��

        /// <summary>
        ///   �ֽ����黺�������������ڽ�����Ϣ��ʱ������յ����ֽ�����
        ///   <para>* �������Ĵ�СӦ������Ϊ������������ * ������</para>
        /// </summary>
        /// <param name="bufferSize">����������</param>
        /// <param name="capacity">����</param>
        public ByteArrayBuffer(int bufferSize)
        {
            if (bufferSize <= 0)
            {
                throw new System.Exception("�޷���ʼ�����������Ƿ��Ļ���������");
            }
            _buffer = new byte[bufferSize];
        }

        #endregion

        #region Members

        protected int _offset;
        protected byte[] _buffer;

        #endregion

        #region Implementation of IByteArrayBuffer

        protected int _bufferSize;
        protected bool _autoClear;

        /// <summary>
        ///   ��ȡ��������С
        ///   <para>* �������Ĵ�СӦ������Ϊ������������ * ������</para>
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
        }

        /// <summary>
        ///   ��ӻ���
        /// </summary>
        /// <param name="data">���յ�������</param>
        public List<byte[]> Add(byte[] data)
        {
            lock (oo)
            {
                //�����ǰ������ʣ��Ŀռ��Ѿ��޷������½������ݵĳ��ȡ�
                if (_buffer.Length - _offset < data.Length)
                {
                    //�Զ����û�����
                    if (_autoClear) Clear();
                    else throw new System.Exception("�������޷��ٽ����µ����ݣ���Ϊ�������Ѿ����ˡ��볢������AutoClear = true����������⡣");
                }
                //��������
                Buffer.BlockCopy(data, 0, _buffer, _offset, data.Length);
                _offset += data.Length;
                //��ȡ����
                int currentOffset = 0;
                List<byte[]> result = PickupData(ref currentOffset, _offset);
                //û����ȡ����������
                if (currentOffset <= 0) return null;
                //��ȡ������������
                if (currentOffset > 0)
                {
                    int leaveSize = _buffer.Length - currentOffset;
                    //�����ǰ���õ����ݳ��ȣ����õ��ڻ������ܳ��ȣ������û�����ƫ����
                    if (leaveSize <= 0)
                    {
                        //��������ƫ����Ϊ0���������³�ʼ����������Ŀ����Ϊ�˼����ڴ����
                        //��һ�εĲ�����ֱ�Ӹ�д0λ������
                        _offset = 0;
                        return result;
                    }
                    //����ʣ�������
                    byte[] lastData = new byte[leaveSize];
                    Buffer.BlockCopy(_buffer, currentOffset, lastData, 0, lastData.Length);
                    //���Ƶ�ԭ���Ļ�������
                    Buffer.BlockCopy(lastData, 0, _buffer, 0, lastData.Length);
                    //����ƫ����
                    _offset -= currentOffset;
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        ///   ��ջ�����
        /// </summary>
        public void Clear()
        {
            _buffer.Initialize();
            _offset = 0;
        }

        /// <summary>
        ///   ��ȡ������һ��ֵ����ֵ��ʾ�����������������ʱ���Ƿ��Զ����û�����
        /// </summary>
        public bool AutoClear
        {
            get { return _autoClear; }
            set { _autoClear = value; }
        }

        #endregion

        #region Functions

        /// <summary>
        ///   �������û�ʹ�õķ���������ʹ���Լ��ķ�ʽ��ȡ���õ�����
        /// </summary>
        /// <returns></returns>
        protected abstract List<byte[]> PickupData(ref int nextOffset, int offset);

        #endregion
    }
}