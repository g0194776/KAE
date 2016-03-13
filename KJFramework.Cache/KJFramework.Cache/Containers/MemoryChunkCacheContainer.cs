using System;
using System.Collections.Concurrent;
using System.Runtime;
using System.Web;
using KJFramework.Cache.Objects;

namespace KJFramework.Cache.Containers
{
    /// <summary>
    ///     �����ڴ�λ����������ṩ����صĻ�������
    /// </summary>
    public class MemoryChunkCacheContainer : IMemoryChunkCacheContainer
    {
        #region Constructor

        /// <summary>
        ///     �����ڴ�λ����������ṩ����صĻ�������
        /// </summary>
        /// <param name="segmentSize">�ڴ�Ƭ�δ�С</param>
        /// <param name="memoryChunkSize">
        ///     �����ڴ�δ�С
        ///     <para>* ��λ��byte</para>
        ///     <para>* �ò���Ӧ����segmentSize�ı���</para>
        /// </param>
        public MemoryChunkCacheContainer(int segmentSize, int memoryChunkSize)
        {
            if (segmentSize <= 0) throw new ArgumentException("Illegal segment size. #" + segmentSize);
            if (memoryChunkSize <= 0) throw new ArgumentException("Illegal memory chunk size. #" + memoryChunkSize);
            if (memoryChunkSize < segmentSize) throw new ArgumentException("Illegal memory chunk size, because current memory chunk size must > segment size. #" + memoryChunkSize);
            _segmentSize = segmentSize;
            _memoryChunkSize = memoryChunkSize;
            #region Memory Fail Point Check.
            
            #if(!MONO_ENV)
            {
                //There is a BUG when your .NETFRAMEWORK version is .NET 4.5 and host enviroment is IIS
                //MemoryFailPoint wil' not be work correctly...
                //only execution host.
                if (HttpContext.Current == null)
                {
                    int totalMemory = (memoryChunkSize / 1024) / 1024;
                    new MemoryFailPoint(totalMemory > 0 ? totalMemory : 1);
                }

            }
            #endif

            #endregion
            _data = new byte[_memoryChunkSize];
            //try to initialize memory segment.
            Initialize();
        }

        #endregion

        #region Implementation of IMemoryChunkCacheContainer

        private readonly int _segmentSize;
        private readonly int _memoryChunkSize;
        private readonly byte[] _data;
        private readonly ConcurrentStack<IMemorySegment> _segments = new ConcurrentStack<IMemorySegment>();

        /// <summary>
        ///     ��ȡ�ڴ�Ƭ�δ�С
        /// </summary>
        public int SegmentSize
        {
            get { return _segmentSize; }
        }

        /// <summary>
        ///     ��ȡ����ռ�õ��ڴ�δ�С
        /// </summary>
        public int MemoryChunkSize
        {
            get { return _memoryChunkSize; }
        }

        /// <summary>
        ///     ���һ���ڴ�Ƭ��
        /// </summary>
        /// <returns>�������null, ��֤���Ѿ�û��ʣ����ڴ�Ƭ�ο��Ա����</returns>
        public IMemorySegment Rent()
        {
            IMemorySegment segment;
            return _segments.TryPop(out segment) ? segment : null;
        }

        /// <summary>
        ///     �黹һ���ڴ�Ƭ��
        /// </summary>
        /// <param name="segment">�ڴ�Ƭ��</param>
        public void Giveback(IMemorySegment segment)
        {
            if (segment == null) throw new ArgumentNullException("segment");
            segment.UsedBytes = 0;
            _segments.Push(segment);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     ��ʼ���ڴ�Ƭ�μ���
        /// </summary>
        private void Initialize()
        {
            int remainingSize = _memoryChunkSize;
            int offset = 0;
            do
            {
                IMemorySegment segment = new MemorySegment(new ArraySegment<byte>(_data, offset, remainingSize > _segmentSize ? _segmentSize : remainingSize));
                offset += _segmentSize;
                remainingSize -= _segmentSize;
                _segments.Push(segment);
            } while (remainingSize > 0);
        }

        #endregion
    }
}